using System;
using PX.Data;
using PX.Objects.CR;
using PX.SM;

namespace PXSurveyMonkeyCRExt.DAC
{
    public class CRSetupExt : PXCacheExtension<CRSetup>
    {
        #region UsrNotificationMapID
        public abstract class usrNotificationMapID : IBqlField { }
        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template")]
        [PXSelector(typeof(Search<Notification.notificationID>),
            SubstituteKey = typeof(Notification.name),
            DescriptionField = typeof(Notification.name))]
        public virtual int? UsrNotificationMapID { get; set; }
        #endregion
        #region UsrSurveyID
        public abstract class usrSurveyID : IBqlField { }
        [PXDBString(20)]
        [PXUIField(DisplayName = "Survey ID")]
        public virtual string UsrSurveyID { get; set; }
        #endregion
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
        #region UsrAPIKey
        public abstract class usrAPIKey : IBqlField { }
        [PXDBString(100)]
        [PXUIField(DisplayName = "API Key")]
        public virtual string UsrAPIKey { get; set; }
        #endregion
        #region UsrAccessToken
        public abstract class usrAccessToken : IBqlField { }
        [PXDBString(300)]
        [PXUIField(DisplayName = "Access Token")]
        public virtual string UsrAccessToken { get; set; }
        #endregion
        #region UsrLastSurveySyncDate
        public abstract class usrLastSurveySyncDate : IBqlField { }
        [PXDBDateAndTime(UseTimeZone=false, PreserveTime=true)]
        [PXUIField(DisplayName = "Last Survey Sync Date")]
        public virtual DateTime? UsrLastSurveySyncDate { get; set; }
        #endregion
    }
}
