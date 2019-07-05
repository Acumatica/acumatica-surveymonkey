using System;
using PX.Data;
using PX.Objects.CR;
using PX.SM;

namespace PXSurveyMonkeyMKExt.DAC
{
    public class CRCaseClassExt : PXCacheExtension<CRCaseClass>
    {
        #region UsrSurveyURL
        public abstract class usrSurveyURL : IBqlField { }
        [PXDBWeblink]
        [PXUIField(DisplayName = "Survey URL")]
        public virtual string UsrSurveyURL { get; set; }
        #endregion

        #region UsrNotificationMapID
        public abstract class usrNotificationMapID : IBqlField { }
        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template")]
        [PXSelector(typeof(Search<Notification.notificationID>), DescriptionField = typeof(Notification.name))]
        public virtual int? UsrNotificationMapID { get; set; }
        #endregion

        #region UsrSurveyID
        public abstract class usrSurveyID : IBqlField { }
        [PXDBString(20, IsUnicode = true)]
        [PXUIField(DisplayName = "Survey ID")]
        public virtual string UsrSurveyID { get; set; }
        #endregion

        #region UsrLastSurveySyncDate
        public abstract class usrLastSurveySyncDate : IBqlField { }
        [PXDBDateAndTime(UseTimeZone = false, PreserveTime = true)]
        [PXUIField(DisplayName = "Last Survey Sync Date")]
        public virtual DateTime? UsrLastSurveySyncDate { get; set; }
        #endregion
    }
}