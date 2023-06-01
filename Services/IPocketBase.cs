namespace MagicArchiver.Services; 

public interface IPocketBase {
  Task<HttpResponseMessage> CreateAccount (string user, string pass, string passConf);
  Task<HttpResponseMessage> Login         (string user, string pass);
}