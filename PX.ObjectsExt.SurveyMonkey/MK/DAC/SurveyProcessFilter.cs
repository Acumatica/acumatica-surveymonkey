using System;
using PX.Data;

namespace PXSurveyMonkeyMKExt.DAC
{
    [Serializable]
    public class SurveyProcessFilter : IBqlTable
    {
        #region NumberOfDays
        public abstract class numberOfDays : IBqlField { }
        [PXDBInt]
        [PXDefault(90)]
        [PXUIField(DisplayName = "Not Surveyed in last (# Of days)")]
        public virtual int? NumberOfDays { get; set; }
        #endregion
        #region NumberOfDaysMostRecentCaseCreatedIn
        public abstract class numberOfDaysMostRecentCaseCreatedIn : IBqlField { }
        [PXDBInt]
        [PXDefault(5)]
        [PXUIField(DisplayName = "Most Recent Case Closed in last (# Of days)")]
        public virtual int? NumberOfDaysMostRecentCaseCreatedIn { get; set; }
        #endregion
    }
}
