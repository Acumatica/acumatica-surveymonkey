using System.Web;
using RestSharp;

namespace PX.SurveyMonkeyReader.Extensions
{
    public class RestRequestExt : RestRequest
    {
        public RestRequestExt()
        {
        }

        public RestRequestExt(string resource, Method method)
            :base(resource, method)
        {
        }

        public IRestRequest AddParameterIfExists(string name, object value)
        {
            return value != null ? AddParameter(name, value) : null;
        }

        public IRestRequest AddParameterUrlEncoded(string name, string value)
        {
            return AddParameter(name, HttpUtility.UrlEncode(value));
        }
    }
}
