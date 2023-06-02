namespace MagicArchiver.Services; 

using System.Text;
using System.Text.Json;
using FluentResults;
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
  

  public async Task<Result<LoginResponse>> CreateAccount(string user, string pass, string passConf) {
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

      return await ProcessHttpResponse<LoginResponse>(response);
    }
    catch (Exception e) {
      return Result.Fail(new Error(e.ToString()));
    }
  }

  
  public async Task<Result<LoginResponse>> Login(string user, string pass) {
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

      return await ProcessHttpResponse<LoginResponse>(response);
    }
    catch (Exception e) {
      return Result.Fail(new Error(e.ToString()));
    }
  }
  

  public async Task<Result<Page<Card>>> GetCardDetails(string filterStr) {
    try {
      var queryPerams = new Dictionary<string, string>() {
        ["perPage"] = "500",
        ["filter"] = filterStr
      };

      string uri = QueryHelpers.AddQueryString(
        _httpClient.BaseAddress + "collections/cards/records",
        queryPerams
      );

      var message = new HttpRequestMessage(HttpMethod.Get, uri);
      message.Headers.Add("Authorization", _globalState.GetToken());

      HttpResponseMessage response = await _httpClient.SendAsync(message);
      return await ProcessHttpResponse<Page<Card>>(response);
    }
    catch (Exception e) {
      return Result.Fail(new Error(e.ToString()));
    }
  }
  

  public async Task<Result> AddCardBySetAbr(string cardId, bool isFoil) {
    try {
      string setNum = cardId.Substring(0, 3);
      setNum = int.Parse(setNum).ToString();
      string setAbr = cardId.Substring(3, 3).ToUpper();

      Result<Page<Card>> cards = await GetCardDetails($"number = '{setNum}' && setCode = '{setAbr}'");

      if (cards.IsSuccess) {
        // TODO: Implement this
        throw new NotImplementedException();
      }
      else {
        return Result.Fail(cards.Errors);
      }
    }
    catch (Exception e) {
      return Result.Fail(new Error(e.ToString()));
    }
  }
}