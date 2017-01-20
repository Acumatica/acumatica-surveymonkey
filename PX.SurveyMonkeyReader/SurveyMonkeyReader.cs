using System;
using System.Collections.Generic;
using PX.SurveyMonkeyReader.Models;
using PX.SurveyMonkeyReader.Repository;

namespace PX.SurveyMonkeyReader
{
    public class SurveyMonkeyReader
    {
        private const int ResultsPerPage = 100;
        
        private readonly string _surveyId;
        private readonly ApiRepository _repository;

        public SurveyMonkeyReader(string surveyId, string accessToken)
        {
            _surveyId = surveyId;
            _repository = new ApiRepository(accessToken, ResultsPerPage);
        }

        public SurveyQuestion GetSurveyQuestion(long questionId)
        {
            return _repository.GetSingleSurveyQuestion(_surveyId, questionId);
        }

        public List<SurveyResponse> GetSurveyResponsesByDateRange(DateTime? startDate, DateTime? endDate = null)
        {
            var surveyLastModified = _repository.GetSurveyLastModifiedDateTime(_surveyId);

            var surveyResponses = new List<SurveyResponse>();
            int page = 1;
            bool isLastPage = false;
            do
            {
                var newSurveyResponses = _repository.GetSurveyResponsesByIdAndDateRange(_surveyId, startDate, endDate, page, out isLastPage);

                foreach (var response in newSurveyResponses)
                {
                    foreach (var question in response.Questions)
                    {
                        question.SurveyLastModified = surveyLastModified.GetValueOrDefault();
                    }
                }

                surveyResponses.AddRange(newSurveyResponses);

                page += 1;

            } while (!isLastPage);

            return surveyResponses;
        }
    }
}
