using System.IO;
using System.Net;
using System.Text;
using PX.SurveyMonkeyReader.Extensions;

namespace PX.SurveyMonkeyReader.Commands
{
    public class OAuthCommands
    {
        private const string ResponseTypeCode = "code";
        private const string GrantTypeAuthorizationCode = "authorization_code";
        private const string SurveyMonkeyOAuthUrl = "https://api.surveymonkey.net/oauth";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public OAuthCommands(string clientId, string clientSecret, string redirectUri)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _redirectUri = redirectUri;
        }

        public string GetAuthorizationPageUri()
        {
            var parameters = new NameValueCollectionExt
            {
                {"client_id", _clientId},
                {"response_type", ResponseTypeCode},
                {"redirect_uri", _redirectUri}
            };

            return string.Concat(SurveyMonkeyOAuthUrl, "/authorize?", parameters.ToQueryString());
        }

        // Note:  RestSharp has trouble with POST + x-www-form-urlencoded content type + query string params + body params
        // so this function uses the WebRequest/WebResponse/StreamReader method.
        public string GetAccessToken(string code)
        {
            var fullRequestUrl = string.Concat(SurveyMonkeyOAuthUrl, "/token");

            var bodyData = new NameValueCollectionExt
            {
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"code", code},
                {"redirect_uri", _redirectUri},
                {"grant_type", GrantTypeAuthorizationCode}
            };
            var bodyQueryString = bodyData.ToQueryString();
            var bodyBytes = Encoding.ASCII.GetBytes(bodyQueryString);

            var request = WebRequest.Create(fullRequestUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bodyBytes.Length;

            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(bodyBytes, 0, bodyBytes.Length);
                dataStream.Close();
            }

            var accessTokenJson = "";
            using (var response = request.GetResponse())
            using (var dataStream = response.GetResponseStream())
            {
                if (dataStream != null)
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        accessTokenJson = reader.ReadToEnd();
                    }
                }
            }

            return accessTokenJson;
        }
    }
}
