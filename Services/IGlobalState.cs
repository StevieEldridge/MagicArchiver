namespace MagicArchiver.Services; 

public interface IGlobalState {
  public string? GetToken();
  public void    SetToken(string token);
  public void    ClearToken();
  public event   Action OnStateChange;
}