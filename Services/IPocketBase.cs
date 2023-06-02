namespace MagicArchiver.Services; 

using FluentResults;
using MagicArchiver.Models;

public interface IPocketBase {
  Task<Result<LoginResponse>> CreateAccount (string user, string pass, string passConf);
  Task<Result<LoginResponse>> Login (string user, string pass);

  Task<Result<Page<Card>>> GetCardDetails (string filterStr);
  //Task<HttpResponseMessage> AddCardBySetNum   (string cardId, bool isFoil);
  Task<Result> AddCardBySetAbr (string cardId, bool isFoil);
  //Task<HttpResponseMessage> AddCardByCardName (string cardId, bool isFoil);
}