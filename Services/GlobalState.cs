using Blazored.LocalStorage;

namespace MagicArchiver.Services;

public class GlobalState: IGlobalState {
  private readonly ISyncLocalStorageService _localStorage;

  public GlobalState(ISyncLocalStorageService localStorage) {
    _localStorage = localStorage;
  }

  /// <summary>
  /// The event that wll be raised for state change
  /// </summary>
  public event Action? OnStateChange;

  public string? GetToken() {
    string jwtToken = _localStorage.GetItem<string>("jwtToken");
    return jwtToken == "" ? null : jwtToken;
  }

  public void SetToken(string token) {
    _localStorage.SetItem("jwtToken", token);
    NotifyStateChanged();
  }

  public void ClearToken() {
    _localStorage.RemoveItem("jwtToken");
    NotifyStateChanged();
  }

  /// <summary>
  /// The state change event notification
  /// </summary>
  private void NotifyStateChanged() => OnStateChange?.Invoke();
}