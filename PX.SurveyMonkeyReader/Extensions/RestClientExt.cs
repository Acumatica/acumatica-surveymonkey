using System;
using PX.SurveyMonkeyReader.Models;
using RestSharp;

namespace PX.SurveyMonkeyReader.Extensions
{
    public class RestClientExt : RestClient
    {
        // Note:  MaxCallsPerSecond is based on the SurveyMonkey plan you have.
        // Basic select plan allows 4 calls per second.  You can increase this if you have a better plan.
        // See https://developer.surveymonkey.com/docs/overview/limits-and-quotas/
        public const int MaxCallsPerSecond = 4;

        private const int MsPerSecond = 1000;
        private const int OneSecond = 1;

        private static LastNQueue _callTimestampTracker;

        public RestClientExt(string restClientUrl)
            :base(restClientUrl)
        {
            _callTimestampTracker = new LastNQueue(MaxCallsPerSecond);
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            ThrottleIfNeeded();
            RecordCallTimeStamp();
            return base.Execute(request);
        }

        private static void RecordCallTimeStamp()
        {
            _callTimestampTracker.Add(DateTime.UtcNow);
        }

        private static void ThrottleIfNeeded()
        {
            if (_callTimestampTracker.Count() < MaxCallsPerSecond)
                return;

            var currTimestamp = DateTime.UtcNow;
            var oldestTimestamp = _callTimestampTracker.GetOldest();
            var timePassed = (int)Math.Floor(currTimestamp.Subtract(oldestTimestamp).TotalSeconds);
            if (timePassed > OneSecond)
                return;

            System.Threading.Thread.Sleep(MsPerSecond);
        }
    }
}
