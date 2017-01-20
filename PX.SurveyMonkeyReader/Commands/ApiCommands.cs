﻿using RestSharp;
using System;
using PX.SurveyMonkeyReader.Extensions;

namespace PX.SurveyMonkeyReader.Commands
{
    public class ApiCommands
    {
        public readonly int ResultsPerPage;

        private const string SurveyMonkeyDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private const string SurveyMonkeyApiUrl = "https://api.surveymonkey.net/v3";

        private static RestClientExt _restClient;

        private readonly string _apiKey;
        private readonly string _authorizationHeaderValue;
        
        
        public ApiCommands(string accessToken, int resultsPerPage)
        {
            _restClient = new RestClientExt(SurveyMonkeyApiUrl);
            _authorizationHeaderValue = string.Concat("bearer ", accessToken);
            ResultsPerPage = resultsPerPage;
        }

        public string GetSurveyDetailsById(string surveyId)
        {
            var restRequest = new RestRequest("surveys/{id}/details", Method.GET);
            restRequest.AddUrlSegment("id", surveyId);
            restRequest.AddParameter("api_key", _apiKey);
            restRequest.AddHeader("Authorization", _authorizationHeaderValue);

            var restResponse = _restClient.Execute(restRequest);
            return restResponse.Content;
        }

        public string GetSurveyResponsesByIdAndDateRange(string surveyId, DateTime? startDate, DateTime? endDate, int page)
        {
            var startDateFormatted = startDate?.ToString(SurveyMonkeyDateTimeFormat);
            var endDateFormatted = endDate?.ToString(SurveyMonkeyDateTimeFormat);

            var restRequest = new RestRequestExt("surveys/{id}/responses/bulk", Method.GET);
            restRequest.AddUrlSegment("id", surveyId);

            restRequest.AddParameter("api_key", _apiKey);
            restRequest.AddParameter("status", "completed");
            restRequest.AddParameter("per_page", ResultsPerPage);
            restRequest.AddParameter("page", page);
            restRequest.AddParameterIfExists("start_modified_at", startDateFormatted);
            restRequest.AddParameterIfExists("end_modified_at", endDateFormatted);
            
            restRequest.AddHeader("Authorization", _authorizationHeaderValue);

            var restResponse = _restClient.Execute(restRequest);
            return restResponse.Content;
        }
    }
}
