using System.Collections.Generic;
using System;
using PX.SurveyMonkeyReader.Models;

namespace PX.SurveyMonkeyReader.Repository
{
    public interface IApiRepository
    {
        DateTime? GetSurveyLastModifiedDateTime(string surveyId);
        SurveyQuestion GetSingleSurveyQuestion(string surveyId, long questionId);
        List<SurveyQuestion> GetSurveyQuestions(string surveyId);
        List<SurveyAnswer> GetSurveyAnswers(string surveyId);
        List<SurveyResponse> GetSurveyResponsesByIdAndDateRange(string surveyId, DateTime? startDate, DateTime? endDate, int? page);
    }
}
