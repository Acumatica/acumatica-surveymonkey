using System;
using Newtonsoft.Json.Linq;

namespace PX.SurveyMonkeyReader.Extensions
{
    public class JObjectExt : JObject
    {
        public static bool TryParse(string stringToParse, out JObject result)
        {
            try
            {
                result = Parse(stringToParse);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"SurveyMonkey returned an error: {stringToParse}", ex);
            }
        }
    }
}
