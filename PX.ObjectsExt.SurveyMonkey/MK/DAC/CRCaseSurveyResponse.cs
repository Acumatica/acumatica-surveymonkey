using System;
using PX.Data;
using PX.Objects.CR;

namespace PXSurveyMonkeyMKExt.DAC
{
    [Serializable]
    public class CRCaseSurveyResponse : IBqlTable
    {
        #region CaseID
        public abstract class caseID : IBqlField { }
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Case ID")]
        [PXParent(typeof(Select<CRCase,  Where<CRCase.caseID, Equal<CRCaseSurveyResponse.caseID>>>))]
        [PXDBDefault(typeof(CRCase.caseID))]
        public virtual int? CaseID { get; set; }
        #endregion
        #region QuestionID
        public abstract class questionID : IBqlField { }
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Question ID")]
        [PXParent(typeof(Select<CRSurveyQuestion, Where<CRSurveyQuestion.questionID, Equal<CRCaseSurveyResponse.questionID>>>))]
        [PXDBDefault(typeof(CRSurveyQuestion.questionID))]
        public virtual int? QuestionID { get; set; }
        #endregion
        #region AnswerLineNbr
        public abstract class answerLineNbr : IBqlField { }
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Answer")]
        [PXParent(typeof(Select<CRSurveyAnswer,
            Where<CRSurveyAnswer.questionID, Equal<CRCaseSurveyResponse.questionID>,
                And<CRSurveyAnswer.answerLineNbr, Equal<CRCaseSurveyResponse.answerLineNbr>>>>))]
        [PXDBDefault(typeof(CRSurveyAnswer.answerLineNbr), PersistingCheck = PXPersistingCheck.Null)]
        [PXSelector(typeof(Search<CRSurveyAnswer.answerLineNbr, 
            Where<CRSurveyAnswer.questionID, Equal<Current<CRCaseSurveyResponse.questionID>>>>),
            DescriptionField = typeof(CRSurveyAnswer.answer))]
        public virtual int? AnswerLineNbr { get; set; }
        #endregion
        #region Answer
        public abstract class answer : IBqlField { }
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "Comment")]
        public virtual string Answer { get; set; }
        #endregion

        #region System Fields

        #region tstamp
        public abstract class Tstamp : PX.Data.IBqlField
        {
        }

        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
        #region CreatedByID
        public abstract class createdByID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion
        #region CreatedByScreenID
        public abstract class createdByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID { get; set; }
        #endregion
        #region CreatedDateTime
        public abstract class createdDateTime : PX.Data.IBqlField
        {
        }

        [PXDBCreatedDateTime]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion
        #region LastModifiedByID
        public abstract class lastModifiedByID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion
        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID { get; set; }
        #endregion
        #region LastModifiedDateTime
        public abstract class lastModifiedDateTime : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedDateTime]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #endregion
    }
}