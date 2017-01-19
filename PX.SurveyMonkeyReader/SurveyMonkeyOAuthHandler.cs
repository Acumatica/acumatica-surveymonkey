using PX.SurveyMonkeyReader.Repository;

namespace PX.SurveyMonkeyReader
{
    public class SurveyMonkeyOAuthHandler
    {
        private readonly OAuthRepository _repository;

        public SurveyMonkeyOAuthHandler(string clientId, string clientSecret, string redirectUri)
        {
            _repository = new OAuthRepository(clientId, clientSecret, redirectUri);
        }

        public string GetAuthorizationPageUri()
        {
            return _repository.GetAuthorizationPageUri();
        }

        public string GetAccessToken(string code)
        {
            return _repository.GetAccessToken(code);
        }
    }
}
