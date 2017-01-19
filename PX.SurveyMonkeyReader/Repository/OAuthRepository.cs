using System;
using Newtonsoft.Json.Linq;
using PX.SurveyMonkeyReader.Commands;
using PX.SurveyMonkeyReader.Extensions;

namespace PX.SurveyMonkeyReader.Repository
{
    public class OAuthRepository : IOAuthRepository
    {
        private static OAuthCommands _commands;

        public OAuthRepository(string clientId, string clientSecret, string redirectUri)
        {
            _commands = new OAuthCommands(clientId, clientSecret, redirectUri);
        }

        public string GetAuthorizationPageUri()
        {
            return _commands.GetAuthorizationPageUri();
        }

        public string GetAccessToken(string code)
        {
            var accessTokenJsonString = _commands.GetAccessToken(code);

            JObject accessTokenJson;
            if (!JObjectExt.TryParse(accessTokenJsonString, out accessTokenJson))
                return null;

            if (accessTokenJson.SelectToken("access_token") != null)
            {
                return accessTokenJson.SelectToken("access_token").ToString();
            }

            if (accessTokenJson.SelectToken("error") != null)
            {
                throw new Exception(string.Concat(accessTokenJson.SelectToken("error").ToString(), 
                    accessTokenJson.SelectToken("error_description")?.ToString()));
            }

            throw new Exception("Could not retrieve an access token");
        }
    }
}

