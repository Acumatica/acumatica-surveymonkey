using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PX.SurveyMonkeyReader.Extensions
{
    public class NameValueCollectionExt : NameValueCollection
    {
        public string ToQueryString()
        {
            var queryStringParameters = AllKeys.Select(key => $"{key}={HttpUtility.UrlEncode(this[key])}").ToList();
            return string.Join("&", queryStringParameters.ToArray());
        }
    }
}
