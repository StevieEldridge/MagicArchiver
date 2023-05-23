namespace MagicArchiver.Pages.Services; 

public class GlobalState: IGlobalState {
  private string? token = null;

  public GlobalState() {}

  public string? GetToken() {
    return this.token;
  }

  public void SetToken(string? token) {
    this.token = token;
  }
}