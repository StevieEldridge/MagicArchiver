namespace MagicArchiver.Services; 

using FluentResults;
using MagicArchiver.Models;

public interface IPocketBase {
  Task<Result<LoginDetails>> CreateAccount (string user, string pass, string passConf);
  Task<Result<LoginDetails>> Login (string user, string pass);
  Task<Result<Collection>> AddCardBySetSize   (string cardId, bool isFoil);
  Task<Result<Collection>> AddCardBySetAbr (string cardId, bool isFoil);
  Task<Result<Collection>> AddCardByCardName (string cardId, bool isFoil);
}