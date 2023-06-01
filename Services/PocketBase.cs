using System.Net.Cache;
using System.Text;
using System.Text.Json;
using MagicArchiver.Services;

public class PocketBase: IPocketBase {
  private readonly HttpClient _httpClient;

  public PocketBase(HttpClient httpClient) {
    _httpClient = httpClient;
  }

  public async Task<HttpResponseMessage> CreateAccount(string user, string pass, string passConf) {
    var body = new {
      username        = user,
      password        = pass,
      passwordConfirm = passConf
    };

    return await _httpClient.PostAsync(
      "collections/users/records",
      new StringContent (
        JsonSerializer.Serialize(body),
        Encoding.UTF8,
        "application/json"
      )
    );
  }

  public async Task<HttpResponseMessage> Login(string user, string pass) {
    var body = new {
      identity = user,
      password = pass
    };
    
    return await _httpClient.PostAsync(
      "collections/users/auth-with-password",
      new StringContent(
        JsonSerializer.Serialize(body),
        Encoding.UTF8,
        "application/json"
      )
    );
  }
}