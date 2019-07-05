using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.CR;
using PX.SurveyMonkeyReader.Models;
using PXSurveyMonkeyMKExt.DAC;

namespace PXSurveyMonkeyMKExt
{
    public class CaseSurveyResponseEngine : PXGraph<CaseSurveyResponseEngine>
    {
        public PXSelect<CRSetup> SetupRecord;

        public PXSelect<CRCase> Cases;

        public PXSelect<CRCaseClass> CaseClassRecord;

        public PXSelect<CRSurveyQuestion> SurveyQuestions;
        public PXSelect<CRSurveyQuestion,
                    Where<CRSurveyQuestion.questionID,
                        Equal<Current<CRSurveyQuestion.parentQuestionID>>>> ParentSurveyQuestions;
        public PXSelect<CRSurveyAnswer,
                    Where<CRSurveyAnswer.questionID,
                        Equal<Current<CRSurveyQuestion.questionID>>>> SurveyAnswers;

        public PXSelect<CRCaseSurveyResponse,
                    Where<CRCaseSurveyResponse.caseID,
                        Equal<Current<CRCase.caseID>>>> CaseSurveyResponses;

        public void SetQuestionAnswer(PX.SurveyMonkeyReader.SurveyMonkeyReader dataReader, SurveyResponseQuestion surveyResponseQuestion)
        {
            var question = (CRSurveyQuestion)PXSelect<CRSurveyQuestion,
                                Where<CRSurveyQuestion.surveyQuestionID,
                                    Equal<Required<CRSurveyQuestion.surveyQuestionID>>, 
                                And<CRSurveyQuestion.surveyLastModified,
                                    Equal<Required<CRSurveyQuestion.surveyLastModified>>>>>
                                    .Select(this, surveyResponseQuestion.ResponseIdentifier.QuestionID, surveyResponseQuestion.SurveyLastModified);
            
            // Question already in DB?  Set the current question to it
            if (question != null)
            {
                SurveyQuestions.Current = question;
            }
            else
            {
                CreateQuestion(dataReader, surveyResponseQuestion);
            }

            // If the current question's answers can be found in the answers table, use the answer ID for the current answer.
            // Otherwise, the answer is stored as freetext in the survey responses table, so don't store it in the current answer at all
            SurveyAnswers.Current = surveyResponseQuestion.ResponseIdentifier.AnswerID > 0
                                             ? SurveyAnswers.Search<CRSurveyAnswer.surveyAnswerID>(surveyResponseQuestion.ResponseIdentifier.AnswerID) 
                                             : null;

            //Other CRCaseSurveyResponse fields will be field based on current values for other views through PXDBDefault
            var caseSurveyResponse = new CRCaseSurveyResponse { Answer = surveyResponseQuestion.Answer };
            if (surveyResponseQuestion.ResponseIdentifier.AnswerID == 0)
            {
                caseSurveyResponse.AnswerLineNbr = 0;
            }
            CaseSurveyResponses.Insert(caseSurveyResponse);
        }

        private void CreateQuestion(PX.SurveyMonkeyReader.SurveyMonkeyReader dataReader, SurveyResponseQuestion surveyResponseQuestion)
        {
            // Question does not specify a parent question?  Set current parent question to null.
            if (surveyResponseQuestion.ParentQuestionID == null)
            {
                ParentSurveyQuestions.Current = null;
            }
            else
            {
                CRSurveyQuestion parentQuestion =
                    PXSelect<CRSurveyQuestion,
                        Where<CRSurveyQuestion.surveyQuestionID,
                            Equal<Required<CRSurveyQuestion.surveyQuestionID>>,
                            And<CRSurveyQuestion.surveyLastModified,
                                Equal<Required<CRSurveyQuestion.surveyLastModified>>>>>
                        .Select(this,
                            surveyResponseQuestion.ParentQuestionID,
                            surveyResponseQuestion.SurveyLastModified);

                // Parent question not yet in DB?  Request parent question info from api and insert it into the DB
                if (parentQuestion == null)
                {
                    parentQuestion = GetQuestionDetails(dataReader, surveyResponseQuestion.ParentQuestionID.Value);
                    SurveyQuestions.Current = null; // Don't hang onto old question info
                    parentQuestion = ParentSurveyQuestions.Insert(parentQuestion);
                }

                ParentSurveyQuestions.Current = parentQuestion;
            }

            // Request question info from api and insert it into the DB
            var question = GetQuestionDetails(dataReader, surveyResponseQuestion.ResponseIdentifier.QuestionID);
            SurveyQuestions.Current = SurveyQuestions.Insert(question);

            // Put the answers for the newly created question into the database too
            var newSurveyAnswers = GetQuestionAnswers(dataReader, surveyResponseQuestion.ResponseIdentifier.QuestionID);
            if (newSurveyAnswers != null)
            {
                foreach (var newSurveyAnswer in newSurveyAnswers)
                {
                    SurveyAnswers.Insert(newSurveyAnswer);
                }
            }
        }

        private static CRSurveyQuestion GetQuestionDetails(PX.SurveyMonkeyReader.SurveyMonkeyReader dataReader, 
            long surveyQuestionId)
        {
            SurveyQuestion questionDetails;
            try
            {
                questionDetails = dataReader.GetSurveyQuestion(surveyQuestionId);
            }
            catch (Exception ex)
            {
                throw new PXException(ex.Message);
            }

            var newQuestion = new CRSurveyQuestion
            {
                SurveyQuestionID = questionDetails.QuestionID,
                SurveyLastModified = questionDetails.SurveyLastModified,
                Question = questionDetails.Question
            };

            return newQuestion;
        }

        private static IEnumerable<CRSurveyAnswer> GetQuestionAnswers(PX.SurveyMonkeyReader.SurveyMonkeyReader dataReader, long surveyQuestionId)
        {
            SurveyQuestion surveyQuestion;
            try
            {
                surveyQuestion = dataReader.GetSurveyQuestion(surveyQuestionId);
            }
            catch (Exception ex)
            {
                throw new PXException(ex.Message);
            }

            return surveyQuestion.Answers?.Select(answer => new CRSurveyAnswer
            {
                SurveyAnswerID = answer.AnswerID, SurveyLastModified = surveyQuestion.SurveyLastModified, Answer = answer.Answer
            }).ToList();
        }
    }
}