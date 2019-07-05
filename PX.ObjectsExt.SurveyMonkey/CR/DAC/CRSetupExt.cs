using System;
using PX.Data;
using PX.Objects.CR;
using PX.SM;

namespace PXSurveyMonkeyCRExt.DAC
{
    public class CRSetupExt : PXCacheExtension<CRSetup>
    {
        #region UsrSurveyClientID
        public abstract class usrSurveyClientID : IBqlField { }
        [PXDBString(100)]
        [PXUIField(DisplayName = "Client ID")]
        public virtual string UsrSurveyClientID { get; set; }
        #endregion
        #region UsrSurveyClientSecret
        public abstract class usrSurveyClientSecret : IBqlField { }
        [PXDBString(100)]
        [PXUIField(DisplayName = "Client Secret")]
        public virtual string UsrSurveyClientSecret { get; set; }
        #endregion
        #region UsrAccessToken
        public abstract class usrAccessToken : IBqlField { }
        [PXDBString(300)]
        [PXUIField(DisplayName = "Access Token")]
        public virtual string UsrAccessToken { get; set; }
        #endregion
    }
}
