using System;
using PX.Data;

namespace PXSurveyMonkeyMKExt.DAC
{
    [Serializable]
    public class CRSurveyQuestion : IBqlTable
    {
        #region QuestionID
        public abstract class questionID : IBqlField { }
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Question ID")]
        public virtual int? QuestionID { get; set; }
        #endregion
        #region ParentQuestionID
        public abstract class parentQuestionID : IBqlField { }
        [PXDBInt()]
        [PXUIField(DisplayName = "Parent Question ID")]
        [PXParent(typeof(Select<CRSurveyQuestion,
                            Where<CRSurveyQuestion.questionID,
                                Equal<CRSurveyQuestion.parentQuestionID>>>))]
        [PXDBDefault(typeof(CRSurveyQuestion.questionID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? ParentQuestionID { get; set; }
        #endregion
        #region SurveyQuestionID
        public abstract class surveyQuestionID : IBqlField { }
        [PXDBLong()]
        [PXUIField(DisplayName = "Survey Question ID")]
        public virtual long? SurveyQuestionID { get; set; }
        #endregion
        #region SurveyLastModified
        public abstract class surveyLastModified : IBqlField { }
        [PXDBDate()]
        [PXUIField(DisplayName = "Survey Last Modified")]
        public virtual DateTime? SurveyLastModified { get; set; }
        #endregion
        #region Question
        public abstract class question : IBqlField { }
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Question")]
        public virtual string Question { get; set; }
        #endregion
        #region AnswerLineCntr
        public abstract class answerLineCntr : IBqlField { }
        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? AnswerLineCntr { get; set; }
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

    [Serializable]
    public class ParentCRSurveyQuestion : CRSurveyQuestion
    {
        public new abstract class questionID : IBqlField { }
    }
}
