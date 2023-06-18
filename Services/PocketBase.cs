namespace MagicArchiver.Services; 

using System.Text;
using System.Text.Json;
using FluentResults;
using FluentResults.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using MagicArchiver.Services;
using MagicArchiver.Models;

public class PocketBase: IPocketBase {
  private readonly HttpClient   _httpClient;
  private readonly IGlobalState _globalState;

  public PocketBase(HttpClient httpClient, IGlobalState globalState) {
    _httpClient  = httpClient;
    _globalState = globalState;
  }
  
  
  private async Task<Result<T>> ProcessHttpResponse<T>(HttpResponseMessage response) {
    if (response.IsSuccessStatusCode) {
      string json = await response.Content.ReadAsStringAsync();
      T? data = JsonSerializer.Deserialize<T>(json);
      Result<T> res = data != null ? Result.Ok(data) : Result.Fail(new Error("Invalid conversion"));
      return res;
    }
    else {
      // TODO: Return a proper error record instead of a string
      return Result.Fail(response.ToString());
    }
  }
  
  private async Task<Result<Page<T>>> GetCollectionRecords<T>(string filterStr, string collectionName) {
    try {
      var queryPerams = new Dictionary<string, string>() {
        ["perPage"] = "500",
        ["filter"] = filterStr
      };

      string uri = QueryHelpers.AddQueryString(
        _httpClient.BaseAddress + $"collections/{collectionName}/records",
        queryPerams
      );

      var message = new HttpRequestMessage(HttpMethod.Get, uri);
      message.Headers.Add("Authorization", _globalState.GetLoginDetails()?.token);

      HttpResponseMessage response = await _httpClient.SendAsync(message);
      return await ProcessHttpResponse<Page<T>>(response);
    }
    catch (Exception e) {
      // TODO: Return a proper error record instead of a string
      return Result.Fail(new Error(e.ToString()));
    }
  }


  private async Task<Result<T>> HttpSendAsync<T>(
    HttpMethod                  method, 
    string                      url, 
    Dictionary<string, string>? queryPerams, 
    Object?                     body, 
    string?                     token
  ) {
    try {
      string uri = url;
      if (queryPerams != null) {
        uri = QueryHelpers.AddQueryString(_httpClient.BaseAddress + url, queryPerams);
      }

      var message = new HttpRequestMessage(method, uri);
      if (token != null) {
        message.Headers.Add("Authorization", token);
      }

      if (body != null) {
        message.Content = new StringContent(
          JsonSerializer.Serialize(body),
          Encoding.UTF8,
          "application/json"
        );
      }

      HttpResponseMessage response = await _httpClient.SendAsync(message);
      return await ProcessHttpResponse<T>(response);

    }
    catch (Exception e) {
      // TODO: Return a proper error record instead of a string
      return Result.Fail(new Error(e.ToString()));
    }
  }

  
  public async Task<Result<LoginDetails>> CreateAccount(string user, string pass, string passConf) {
    try {
      var body = new {
        username = user,
        password = pass,
        passwordConfirm = passConf
      };

      HttpResponseMessage response = await _httpClient.PostAsync(
        "collections/users/records",
        new StringContent(
          JsonSerializer.Serialize(body),
          Encoding.UTF8,
          "application/json"
        )
      );

      return await ProcessHttpResponse<LoginDetails>(response);
    }
    catch (Exception e) {
      // TODO: Return a proper error record instead of a string
      return Result.Fail(new Error(e.ToString()));
    }
  }

  
  public async Task<Result<LoginDetails>> Login(string user, string pass) {
    try {
      var body = new {
        identity = user,
        password = pass
      };

      HttpResponseMessage response = await _httpClient.PostAsync(
        "collections/users/auth-with-password",
        new StringContent(
          JsonSerializer.Serialize(body),
          Encoding.UTF8,
          "application/json"
        )
      );

      return await ProcessHttpResponse<LoginDetails>(response);
    }
    catch (Exception e) {
      // TODO: Return a proper error record instead of a string
      return Result.Fail(new Error(e.ToString()));
    }
  }
  

  private async Task<Result<Page<Collection>>> GetCardInCollection(string uuid, bool isFoil) {
    Result<Page<Collection>> collection = await GetCollectionRecords<Collection>(
      $"uuid = '{uuid}' && isFoil = {isFoil.ToString().ToLower()}",
      "collection"
    );
    
    if (collection.IsSuccess) {
      return Result.Ok(collection.Value);
    }
    else {
      return Result.Fail(collection.Errors);
    }
  }


  private async Task<Result<Collection>> AddCardToCollection(string uuid, bool isFoil) {
    var body = new {
      uuid     = uuid,
      userId   = _globalState.GetLoginDetails()?.record.id,
      quantity = 1,
      isFoil   = isFoil
    };

    return await HttpSendAsync<Collection> (
      HttpMethod.Post, 
      "collections/collection/records",
      null,
      body,
      _globalState.GetLoginDetails()?.token
    );
  }

  
  private async Task<Result<Collection>> UpdateCollectionQuantity(Collection collection) {
    var body = new {
      uuid     = collection.uuid,
      userId   = _globalState.GetLoginDetails()?.record.id,
      quantity = collection.quantity + 1,
      isFoil   = collection.isFoil
    };

    return await HttpSendAsync<Collection> (
      HttpMethod.Patch,
      "collections/collection/records/" + collection.id,
      null,
      body,
      _globalState.GetLoginDetails()?.token
    );
  }


  public async Task<Result<Collection>> AddCardBySetAbr(string cardId, bool isFoil) {
    string setNum = "";
    string setAbr = "";
    
    try {
      setNum = cardId.Substring(0, 3);
      setNum = int.Parse(setNum).ToString();
      setAbr = cardId.Substring(3, 3).ToUpper();
    }
    catch (Exception e) {
      return Result.Fail(new Error("The id string is invalid"));
    }

    Result<Page<Card>> cards = await GetCollectionRecords<Card>(
      $"number = '{setNum}' && setCode = '{setAbr}'", 
      "cards"
    );

    return await cards
      .Bind(c =>
        c.items.Length == 1
          ? Result.Ok(c)
          : Result.Fail(new Error("Id string matched more than 1 card"))
      )
      .Bind(c =>
        GetCardInCollection(c.items[0].uuid, isFoil)
      )
      .Bind(collection =>
        collection.items.Length == 0
          ? AddCardToCollection(cards.Value.items[0].uuid, isFoil)
          : UpdateCollectionQuantity(collection.items[0])
      );
  }
  
  
  // TODO: Add handling for multiple matched cards with the cardId
  public async Task<Result<Collection>> AddCardBySetSize(string cardId, bool isFoil) {
    string setNum  = "";
    string setSize = "";
    
    try {
      setNum = cardId.Substring(0, 3);
      setNum = int.Parse(setNum).ToString();
      setSize = cardId.Substring(3, 3);
      setSize = int.Parse(setSize).ToString();
    }
    catch (Exception e) {
      return Result.Fail(new Error("The id string is invalid"));
    }
    

    Result<Page<CardSet>> cards = await GetCollectionRecords<CardSet>(
      $"number = '{setNum}' && baseSetSize = '{setSize}'", 
      "card_sets"
    );

    return await cards
      .Bind(c =>
        c.items.Length == 1
          ? Result.Ok(c)
          : Result.Fail(new Error("Id string matched more than 1 card"))
      )
      .Bind(c =>
        GetCardInCollection(c.items[0].uuid, isFoil)
      )
      .Bind(collection =>
        collection.items.Length == 0
          ? AddCardToCollection(cards.Value.items[0].uuid, isFoil)
          : UpdateCollectionQuantity(collection.items[0])
      );
  }
  
  
  // TODO: Add handling for multiple matched cards with the cardId
  public async Task<Result<Collection>> AddCardByCardName(string cardId, bool isFoil) {
    string setNum   = "";
    string cardName = "";
    
    try {
      setNum = cardId.Substring(0, 3);
      setNum = int.Parse(setNum).ToString();
      cardName = cardId.Substring(3, 3);
    }
    catch (Exception e) {
      return Result.Fail(new Error("The id string is invalid"));
    }

    Result<Page<Card>> cards = await GetCollectionRecords<Card>(
      $"number = '{setNum}' && name ~ '{cardName}%'", 
      "cards"
    );

   return await cards
     .Bind(c =>
       c.items.Length == 1
         ? Result.Ok(c)
         : Result.Fail(new Error("Id string matched more than 1 card"))
     )
     .Bind(c =>
       GetCardInCollection(c.items[0].uuid, isFoil)
     )
     .Bind(collection =>
       collection.items.Length == 0
         ? AddCardToCollection(cards.Value.items[0].uuid, isFoil)
         : UpdateCollectionQuantity(collection.items[0])
     );
 }
}