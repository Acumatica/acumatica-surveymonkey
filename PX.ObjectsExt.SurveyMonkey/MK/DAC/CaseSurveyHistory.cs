using System;
using PX.Data;

namespace PXSurveyMonkeyMKExt.DAC
{
    [Serializable]
    public class CaseSurveyHistory : IBqlTable
    {
        #region PKID
        public abstract class pKID : IBqlField { }
        [PXDBIdentity(IsKey = true)]
        [PXUIField(Enabled = false)]
        public virtual int? PKID { get; set; }
        #endregion
        #region ContactID
        public abstract class contactID : IBqlField { }
        [PXDBInt()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "ContactID")]
        public virtual int? ContactID { get; set; }
        #endregion
        #region OwnerID
        public abstract class ownerID : IBqlField { }
        [PXDBGuid()]
        [PXDefault()]
        [PXUIField(DisplayName = "OwnerID")]
        public virtual Guid? OwnerID { get; set; }
        #endregion
        #region CreatedByID
        public abstract class createdByID : IBqlField { }
        [PXDBCreatedByID]
        [PXUIField(DisplayName = "Created By")]
        public virtual Guid? CreatedByID { get; set; }
        #endregion
        #region CreatedDateTime
        public abstract class createdDateTime : IBqlField { }
        [PXDBCreatedDateTime]
        [PXUIField(DisplayName = "Created Date")]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion
    }
}
