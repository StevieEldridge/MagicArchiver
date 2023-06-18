namespace MagicArchiver.Models;

public enum AddCardMethod {
  SetSize  = 1,
  SetAbr   = 2,
  CardName = 3
}

public record struct AuthData {
  public string? token    { get; init; }
  public string? userId   { get; init; }
  public string? username { get; init; }
}
