namespace MagicArchiver.Services;

using Blazored.LocalStorage;
using MagicArchiver.Models;


public class GlobalState: IGlobalState {
  private readonly ISyncLocalStorageService _localStorage;

  public GlobalState(ISyncLocalStorageService localStorage) {
    _localStorage = localStorage;
  }

  /// <summary>
  /// The event that wll be raised for state change
  /// </summary>
  public event Action? OnStateChange;

  public LoginDetails? GetLoginDetails() {
    return _localStorage.GetItem<LoginDetails?>("LoginDetails");
  }

  public void SetLoginDetails(LoginDetails loginDetails) {
    _localStorage.SetItem<LoginDetails>("LoginDetails", loginDetails);
    NotifyStateChanged();
  }

  public void ClearLoginDetails() {
    _localStorage.RemoveItem("LoginDetails");
    NotifyStateChanged();
  }

  /// <summary>
  /// The state change event notification
  /// </summary>
  private void NotifyStateChanged() => OnStateChange?.Invoke();
}