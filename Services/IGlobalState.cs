namespace MagicArchiver.Pages.Services; 

public interface IGlobalState {
  public string? GetToken();
  public void    SetToken(string token);
}