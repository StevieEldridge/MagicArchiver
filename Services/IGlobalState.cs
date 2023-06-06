namespace MagicArchiver.Services; 

using MagicArchiver.Models;

public interface IGlobalState {
  public LoginDetails? GetLoginDetails();
  public void      SetLoginDetails(LoginDetails loginDetails);
  public void      ClearLoginDetails();
  public event     Action OnStateChange;
}