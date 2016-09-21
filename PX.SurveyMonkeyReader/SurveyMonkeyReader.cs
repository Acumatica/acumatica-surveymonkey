using System;
using System.Collections.Generic;
using PX.SurveyMonkeyReader.Models;
using PX.SurveyMonkeyReader.Repository;

namespace PX.SurveyMonkeyReader
{
    public class SurveyMonkeyReader
    {
        private const int ResultsPerPage = 1000;
        
        private readonly string _surveyId;
        private readonly ApiRepository _repository;

        public SurveyMonkeyReader(string surveyId, string apiKey, string accessToken)
        {
            _surveyId = surveyId;
            _repository = new ApiRepository(apiKey, accessToken, ResultsPerPage);
        }

        public SurveyQuestion GetSurveyQuestion(long questionId)
        {
            return _repository.GetSingleSurveyQuestion(_surveyId, questionId);
        }

        public List<SurveyResponse> GetSurveyResponsesByDateRange(DateTime? startDate, DateTime? endDate = null)
        {
            var surveyLastModified = _repository.GetSurveyLastModifiedDateTime(_surveyId);

            var surveyResponses = new List<SurveyResponse>();
            int? page = null;
            int responseCount;
            do
            {
                var newSurveyResponses = _repository.GetSurveyResponsesByIdAndDateRange(_surveyId, startDate, endDate, page);

                responseCount = 0;
                foreach (var response in newSurveyResponses)
                {
                    responseCount++;
                    foreach (var question in response.Questions)
                    {
                        question.SurveyLastModified = surveyLastModified.GetValueOrDefault();
                    }
                }

                surveyResponses.AddRange(newSurveyResponses);

                page = (page == null ? 1 : page + 1);

            } while (responseCount == ResultsPerPage);

            return surveyResponses;
        }
    }
}
