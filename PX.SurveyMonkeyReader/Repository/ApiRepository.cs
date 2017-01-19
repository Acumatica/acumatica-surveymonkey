using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using PX.SurveyMonkeyReader.Commands;
using PX.SurveyMonkeyReader.Extensions;
using PX.SurveyMonkeyReader.Models;

namespace PX.SurveyMonkeyReader.Repository
{
    public class ApiRepository : IApiRepository
    {
        private DateTime? _surveyLastModifiedDateTimeCache;
        private List<SurveyQuestion> _surveyQuestionListCache;
        private List<SurveyAnswer> _surveyAnswerListCache;

        private static ApiCommands _commands;

        public ApiRepository(string accessToken, int resultsPerPage)
        {
            _commands = new ApiCommands(accessToken, resultsPerPage);
        }

        public DateTime? GetSurveyLastModifiedDateTime(string surveyId)
        {
            if (_surveyLastModifiedDateTimeCache != null)
                return _surveyLastModifiedDateTimeCache;

            var surveyJsonString = _commands.GetSurveyDetailsById(surveyId);
            JObject surveyJson;
            if (!JObjectExt.TryParse(surveyJsonString, out surveyJson))
                return null;
            
            _surveyLastModifiedDateTimeCache = DateTime.Parse(surveyJson.SelectToken("date_modified").ToString());
            return _surveyLastModifiedDateTimeCache;
        }

        public List<SurveyQuestion> GetAllSurveyQuestions(string surveyId)
        {
            var allSurveyQuestions = GetSurveyQuestions(surveyId);
            var allSurveyAnswers = GetSurveyAnswers(surveyId);
            var returnList = new List<SurveyQuestion>();

            foreach (var question in allSurveyQuestions)
            {
                var answers = allSurveyAnswers.Where(answer => answer.QuestionID == question.QuestionID).ToList();
                returnList.Add(new SurveyQuestion
                {
                    SurveyLastModified = GetSurveyLastModifiedDateTime(surveyId).GetValueOrDefault(),
                    ParentQuestionID = question.ParentQuestionID,
                    QuestionID = question.QuestionID,
                    Question = question.Question,
                    Answers = answers.Count == 0 ? null : answers
                });
            }

            return returnList;
        }

        public SurveyQuestion GetSingleSurveyQuestion(string surveyId, long questionId)
        {
            var allSurveyQuestions = GetSurveyQuestions(surveyId);
            SurveyQuestion questionDetails = null;

            foreach (var surveyQuestion in allSurveyQuestions)
            {
                if (surveyQuestion.QuestionID == questionId)
                {
                    questionDetails = surveyQuestion;
                    break;
                }
            }
            return questionDetails;
        }

        public List<SurveyQuestion> GetSurveyQuestions(string surveyId)
        {
            if (_surveyQuestionListCache != null)
                return _surveyQuestionListCache;

            var surveyQuestions = new List<SurveyQuestion>();

            var surveyJsonString = _commands.GetSurveyDetailsById(surveyId);

            JObject surveyJson;
            if (!JObjectExt.TryParse(surveyJsonString, out surveyJson))
                return surveyQuestions;

            var surveyLastModified = GetSurveyLastModifiedDateTime(surveyId).GetValueOrDefault();

            foreach (var contentPage in surveyJson.SelectToken("pages").Children())
            {
                foreach (var pageQuestion in contentPage.SelectToken("questions").Children())
                {
                    var questionFamily = pageQuestion.SelectToken("family").ToString();
                    long tempQuestionId;
                    if (!string.Equals(questionFamily, "matrix"))
                    {
                        tempQuestionId = long.Parse(pageQuestion.SelectToken("id").ToString());
                        surveyQuestions.Add(new SurveyQuestion
                        {
                            SurveyLastModified = surveyLastModified,
                            ParentQuestionID = null,
                            QuestionID = tempQuestionId,
                            Question = pageQuestion.SelectToken("headings")[0].SelectToken("heading").ToString(),
                            Answers = GetSurveyAnswersForSingleQuestion(surveyId, tempQuestionId)
                        });

                        if (pageQuestion.SelectToken("answers") != null 
                            && pageQuestion.SelectToken("answers").SelectToken("other") != null)
                        {
                            tempQuestionId = long.Parse(pageQuestion.SelectToken("answers").SelectToken("other").SelectToken("id").ToString());
                            surveyQuestions.Add(new SurveyQuestion
                            {
                                SurveyLastModified = surveyLastModified,
                                ParentQuestionID = null,
                                QuestionID = long.Parse(pageQuestion.SelectToken("answers").SelectToken("other").SelectToken("id").ToString()),
                                Question = pageQuestion.SelectToken("answers").SelectToken("other").SelectToken("text").ToString(),
                                Answers = GetSurveyAnswersForSingleQuestion(surveyId, tempQuestionId)
                            });
                        }

                        continue;
                    }

                    var parentQuestionId = long.Parse(pageQuestion.SelectToken("id").ToString());

                    foreach (var questionRow in pageQuestion.SelectToken("answers").SelectToken("rows").Children())
                    {
                        tempQuestionId = long.Parse(questionRow.SelectToken("id").ToString());
                        surveyQuestions.Add(new SurveyQuestion
                        {
                            SurveyLastModified = surveyLastModified,
                            ParentQuestionID = parentQuestionId,
                            QuestionID = tempQuestionId,
                            Question = questionRow.SelectToken("text").ToString(),
                            Answers = GetSurveyAnswersForSingleQuestion(surveyId, tempQuestionId)
                        });
                    }

                    if (pageQuestion.SelectToken("answers").SelectToken("other") != null)
                    {
                        tempQuestionId = long.Parse(pageQuestion.SelectToken("answers").SelectToken("other").SelectToken("id").ToString());
                        surveyQuestions.Add(new SurveyQuestion
                        {
                            SurveyLastModified = surveyLastModified,
                            ParentQuestionID = parentQuestionId,
                            QuestionID = tempQuestionId,
                            Question = pageQuestion.SelectToken("answers").SelectToken("other").SelectToken("text").ToString(),
                            Answers = GetSurveyAnswersForSingleQuestion(surveyId, tempQuestionId)
                        });
                    }

                    tempQuestionId = long.Parse(pageQuestion.SelectToken("id").ToString());
                    surveyQuestions.Add(new SurveyQuestion
                    {
                        SurveyLastModified = surveyLastModified,
                        ParentQuestionID = null,
                        QuestionID = tempQuestionId,
                        Question = pageQuestion.SelectToken("headings")[0].SelectToken("heading").ToString(),
                        Answers = GetSurveyAnswersForSingleQuestion(surveyId, tempQuestionId)
                    });
                }
            }

            _surveyQuestionListCache = surveyQuestions;
            return _surveyQuestionListCache;
        }

        public List<SurveyAnswer> GetSurveyAnswersForSingleQuestion(string surveyId, long questionId)
        {
            var allSurveyAnswers = GetSurveyAnswers(surveyId);
            var questionAnswers = new List<SurveyAnswer>();

            foreach (var surveyAnswer in allSurveyAnswers)
            {
                if (surveyAnswer.QuestionID == questionId)
                {
                    questionAnswers.Add(surveyAnswer);
                }
            }
            return questionAnswers;
        }

        public List<SurveyAnswer> GetSurveyAnswers(string surveyId)
        {
            if (_surveyAnswerListCache != null)
                return _surveyAnswerListCache;

            var surveyAnswers = new List<SurveyAnswer>();

            var surveyJsonString = _commands.GetSurveyDetailsById(surveyId);

            JObject surveyJson;
            if (!JObjectExt.TryParse(surveyJsonString, out surveyJson))
                return surveyAnswers;
            
            foreach (var contentPage in surveyJson.SelectToken("pages").Children())
            {
                foreach (var pageQuestion in contentPage.SelectToken("questions").Children())
                {
                    if (pageQuestion.SelectToken("answers") == null)
                        continue;
                    
                    var questionFamily = pageQuestion.SelectToken("family").ToString();
                    if (!string.Equals(questionFamily, "matrix"))
                    {
                        if (pageQuestion.SelectToken("answers").SelectToken("choices") == null)
                            continue;
                        
                        var questionId = long.Parse(pageQuestion.SelectToken("id").ToString());
                        foreach (var questionChoice in pageQuestion.SelectToken("answers").SelectToken("choices").Children())
                        {
                            surveyAnswers.Add(new SurveyAnswer
                            {
                                QuestionID = questionId,
                                AnswerID = long.Parse(questionChoice.SelectToken("id").ToString()),
                                Answer = questionChoice.SelectToken("text").ToString()
                            });
                        }

                        continue;
                    }

                    foreach (var questionRow in pageQuestion.SelectToken("answers").SelectToken("rows").Children())
                    {
                        var questionID = long.Parse(questionRow.SelectToken("id").ToString());

                        foreach (var questionChoice in pageQuestion.SelectToken("answers").SelectToken("choices").Children())
                        {
                            surveyAnswers.Add(new SurveyAnswer
                            {
                                QuestionID = questionID,
                                AnswerID = long.Parse(questionChoice.SelectToken("id").ToString()),
                                Answer = questionChoice.SelectToken("text").ToString()
                            });
                        }
                    }
                }
            }

            _surveyAnswerListCache = surveyAnswers;
            return _surveyAnswerListCache;
        }

        public List<SurveyResponse> GetSurveyResponsesByIdAndDateRange(string surveyId, DateTime? startDate, DateTime? endDate, int page, out bool isLastPage)
        {
            var surveyResponseList = new List<SurveyResponse>();
            var allSurveyQuestions = GetSurveyQuestions(surveyId);

            var surveyJsonString = _commands.GetSurveyResponsesByIdAndDateRange(surveyId, startDate, endDate, page);

            JObject surveyJson;
            if (!JObjectExt.TryParse(surveyJsonString, out surveyJson))
            {
                isLastPage = true;
                return surveyResponseList;
            }

            if (surveyJson.SelectToken("data") == null)
            {
                isLastPage = true;
                return surveyResponseList;
            }

            var totalResponses = long.Parse(surveyJson.SelectToken("total").ToString());
            isLastPage = (totalResponses <= _commands.ResultsPerPage * page);

            foreach (var dataItem in surveyJson.SelectToken("data").Children())
            {
                var surveyResponseCustomValues = GetSurveyResponseCustomData(dataItem.SelectToken("custom_value"));
                if (surveyResponseCustomValues == null)
                    continue;

                var allAnsweredQuestions = new Dictionary<QuestionAnswerIdentifier, SurveyResponseQuestion>();
                var surveyResponseQuestionList = new List<SurveyResponseQuestion>();
                foreach (var pageItem in dataItem.SelectToken("pages").Children())
                {
                    foreach (var question in pageItem.SelectToken("questions").Children())
                    {
                        var questionHeaderId = question.SelectToken("id").ToString();

                        foreach (var answer in question.SelectToken("answers").Children())
                        {
                            var choiceId = answer.SelectToken("choice_id");
                            var rowId = answer.SelectToken("row_id");
                            var text = answer.SelectToken("text");
                            var otherId = answer.SelectToken("other_id");

                            SurveyResponseQuestion newResponseQuestion = null;
                            if (choiceId != null && rowId != null)
                            {
                                newResponseQuestion = new SurveyResponseQuestion
                                {
                                    ParentQuestionID = long.Parse(questionHeaderId),
                                    ResponseIdentifier = new QuestionAnswerIdentifier
                                    {
                                        QuestionID = long.Parse(rowId.ToString()),
                                        AnswerID = long.Parse(choiceId.ToString())
                                    },
                                    Answer = null
                                };
                            }
                            else if (choiceId != null)
                            {
                                newResponseQuestion = new SurveyResponseQuestion
                                {
                                    ParentQuestionID = null,
                                    ResponseIdentifier = new QuestionAnswerIdentifier
                                    {
                                        QuestionID = long.Parse(questionHeaderId),
                                        AnswerID = long.Parse(choiceId.ToString())
                                    },
                                    Answer = null
                                };
                            }
                            else if (otherId != null && text != null)
                            {
                                newResponseQuestion = new SurveyResponseQuestion
                                {
                                    ParentQuestionID = null,
                                    ResponseIdentifier = new QuestionAnswerIdentifier
                                    {
                                        QuestionID = long.Parse(otherId.ToString()),
                                        AnswerID = 0
                                    },
                                    Answer = text.ToString()
                                };
                            }
                            else if (text != null)
                            {
                                newResponseQuestion = new SurveyResponseQuestion
                                {
                                    ParentQuestionID = null,
                                    ResponseIdentifier = new QuestionAnswerIdentifier
                                    {
                                        QuestionID = long.Parse(questionHeaderId),
                                        AnswerID = 0
                                    },
                                    Answer = text.ToString()
                                };
                            }

                            if (newResponseQuestion != null)
                            {
                                allAnsweredQuestions.Add(newResponseQuestion.ResponseIdentifier, newResponseQuestion);
                                surveyResponseQuestionList.Add(newResponseQuestion);
                            }
                        }
                    }
                }

                // Add rows for questions that weren't answered by the respondent, in case we want to add answers for them later
                var allAvailableQuestions = GetSurveyQuestions(surveyId);
                foreach (var availableQuestion in allAvailableQuestions)
                {
                    var answeredQuestion = from answers in allAnsweredQuestions
                                  where answers.Key.QuestionID == availableQuestion.QuestionID
                                  select answers;

                    if (!answeredQuestion.Any())
                    {
                        surveyResponseQuestionList.Add(new SurveyResponseQuestion
                        {
                            ParentQuestionID = availableQuestion.ParentQuestionID,
                            ResponseIdentifier = new QuestionAnswerIdentifier
                            {
                                QuestionID = availableQuestion.QuestionID,
                                AnswerID = 0
                            },
                            Answer = null
                        });
                    }
                    else
                    {
                        foreach (var answer in answeredQuestion)
                        {
                            surveyResponseQuestionList.Add(new SurveyResponseQuestion
                            {
                                ParentQuestionID = answer.Value.ParentQuestionID,
                                ResponseIdentifier = new QuestionAnswerIdentifier
                                {
                                    QuestionID = answer.Key.QuestionID,
                                    AnswerID = answer.Key.AnswerID
                                },
                                Answer = answer.Value.Answer
                            });
                        }
                    }
                }

                surveyResponseList.Add(new SurveyResponse
                {
                    CaseCD = surveyResponseCustomValues[3],
                    ResponseID = long.Parse(dataItem.SelectToken("id").ToString()),
                    ResponseDate = DateTime.Parse(dataItem.SelectToken("date_modified").ToString()),
                    Questions = surveyResponseQuestionList
                });
            }

            return surveyResponseList;
        }
        
        private static string[] GetSurveyResponseCustomData(JToken customData)
        {
            try
            {
                var returnArray = customData.ToString().Replace("||", "|").Split('|');

                if (returnArray.Length != 7)
                    return null;
                
                // Make sure that CaseCD type is an integer
                int output;
                return !int.TryParse(returnArray[3], out output) ? null : returnArray;

            } catch
            {
                return null;
            }
        }
    }
}

