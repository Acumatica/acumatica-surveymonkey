using System;
using PX.Data;
using PX.Objects.CR;

namespace PXSurveyMonkeyCRExt.DAC
{
    public class CRCaseExt: PXCacheExtension<CRCase>
    {
        #region UsrResponseID
        public abstract class usrResponseID : IBqlField { }
        [PXDBLong]
        [PXUIField(DisplayName = "Response ID")]
        public virtual long? UsrResponseID { get; set; }
        #endregion
        #region UsrResponseDate
        public abstract class usrResponseDate : IBqlField { }
        [PXDBDate]
        [PXUIField(DisplayName = "Response Date")]
        public virtual DateTime? UsrResponseDate { get; set; }
        #endregion
        #region UsrInternalComment
        public abstract class usrInternalComment : IBqlField { }
        [PXDBString(255)]
        [PXUIField(DisplayName = "Internal Comment")]
        public virtual string UsrInternalComment { get; set; }
        #endregion
        #region UsrInternalCommentByName
        public abstract class usrInternalCommentByName : IBqlField { }
        [PXDBString(255)]
        [PXUIField(DisplayName = "By")]
        public virtual string UsrInternalCommentByName { get; set; }
        #endregion
        #region UsrSurveyReportingEligible
        public abstract class usrSurveyReportingEligible : IBqlField { }
        [PXDBBool]
        [PXUIField(DisplayName = "Survey Reporting Eligible")]
        public virtual bool? UsrSurveyReportingEligible { get; set; }
        #endregion
    }
}
