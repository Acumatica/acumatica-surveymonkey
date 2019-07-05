using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.SM;

namespace PXSurveyMonkeyMKExt.DAC
{
    [Serializable]
	[PXPrimaryGraph(typeof(CaseSurveyProcessing))]
    [PXProjection(typeof(Select5<Contact,
            InnerJoin<CRCase, On<CRCase.contactID, Equal<Contact.contactID>>,
            InnerJoin<CRCaseClass, On<CRCaseClass.caseClassID, Equal<CRCase.caseClassID>,
                                    And<CRCaseClassExt.usrNotificationMapID, IsNotNull, And<CRCaseClassExt.usrSurveyURL, IsNotNull>>>,
            InnerJoin<Notification, On<Notification.notificationID, Equal<CRCaseClassExt.usrNotificationMapID>>,
            LeftJoin<BAccount, On<BAccount.bAccountID, Equal<CRCase.customerID>>,
            LeftJoin<CSAnswers, On<CSAnswers.refNoteID, Equal<BAccount.noteID>,
                                     And<CSAnswers.attributeID, Equal<AttribNamesCustom.PartnerGroupAttrbName>>>,
            LeftJoin<PX.SM.Users, On<PX.SM.Users.pKID, Equal<CRCase.ownerID>>,
            LeftJoin<CaseSurveyHistory, On<CaseSurveyHistory.contactID, Equal<Contact.contactID>,
                                        And<CaseSurveyHistory.ownerID, Equal<CRCase.ownerID>>>>>>>>>>,
            Where<CRCase.ownerID, IsNotNull,
                And<CRCase.majorStatus, Equal<CRCaseMajorStatusesAttribute.closed>>>,
                Aggregate<GroupBy<Contact.contactID, GroupBy<CRCase.caseClassID, GroupBy<CRCase.ownerID, Count<CRCase.caseID>>>>>>), Persistent = false)]
    public class SurveyContactInfo : IBqlTable
	{
		#region Selected
		public abstract class selected : IBqlField { }
		[PXBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Selected")]
		public virtual bool? Selected { get; set; }
		#endregion
		#region ContactID
		public abstract class contactID : IBqlField { }
		[PXDBIdentity(IsKey = true, BqlField = typeof(Contact.contactID))]
		[PXUIField(DisplayName = "Contact ID", Visible = false)]
		public virtual Int32? ContactID { get; set; }
		#endregion
		#region FullName
		public abstract class fullName : IBqlField { }
		[PXDBString(255, IsUnicode = true, BqlField = typeof(Contact.fullName))]
		[PXUIField(DisplayName = "Partner Name")]
		public virtual String FullName { get; set; }
		#endregion
		#region DisplayName
		public abstract class displayName : IBqlField { }
		[PXDBString(IsUnicode = true, BqlField = typeof(Contact.displayName))]
		[PXUIField(DisplayName = "Partner Contact Name")]
		public virtual String DisplayName { get; set; }
		#endregion
		#region Title
		public abstract class title : IBqlField { }
		[PXDBString(50, IsUnicode = true, BqlField = typeof(Contact.title))]
		[Titles]
		[PXUIField(DisplayName = "Partner Contact Title", Visible = false)]
		public virtual String Title { get; set; }
		#endregion
		#region Email
		public abstract class eMail : IBqlField { }
		[PXDBEmail(BqlField = typeof(Contact.eMail))]
		[PXUIField(DisplayName = "Partner Contact Email")]
		public virtual String EMail { get; set; }
		#endregion
		#region Phone1
		public abstract class phone1 : IBqlField { }
		[PXDBString(50, BqlField = typeof(Contact.phone1))]
		[PXUIField(DisplayName = "Partner Contact Tel 1")]
		public virtual String Phone1 { get; set; }
		#endregion
		#region Phone1Type
		public abstract class phone1Type : IBqlField { }
		[PXDBString(3, BqlField = typeof(Contact.phone1Type))]
		[PXUIField(DisplayName = "Tel 1 Type", Visible = false)]
		[PhoneTypes]
		public virtual String Phone1Type { get; set; }
		#endregion
		#region Phone2
		public abstract class phone2 : IBqlField { }
		[PXDBString(50, BqlField = typeof(Contact.phone2))]
		[PXUIField(DisplayName = "Partner Contact Tel 2")]
		public virtual String Phone2 { get; set; }
		#endregion
		#region Phone2Type
		public abstract class phone2Type : IBqlField { }
		[PXDBString(3, BqlField = typeof(Contact.phone2Type))]
		[PXUIField(DisplayName = "Tel 2 Type", Visible = false)]
		[PhoneTypes]
		public virtual String Phone2Type { get; set; }
		#endregion
		#region Phone3
		public abstract class phone3 : IBqlField { }
		[PXDBString(50, BqlField = typeof(Contact.phone3))]
		[PXUIField(DisplayName = "Partner Contact Tel 3")]
		public virtual String Phone3 { get; set; }
		#endregion
		#region Phone3Type
		public abstract class phone3Type : IBqlField { }
		[PXDBString(3, BqlField = typeof(Contact.phone3Type))]
		[PXUIField(DisplayName = "Tel 3 Type", Visible = false)]
		[PhoneTypes]
		public virtual String Phone3Type { get; set; }
		#endregion
		#region UsrLastSurveyed
		public abstract class usrLastSurveyed : IBqlField { }
		[PXDBDateAndTime(BqlField = typeof(CaseSurveyHistory.createdDateTime))]
		[PXUIField(DisplayName = "Last Surveyed")]
		public virtual DateTime? UsrLastSurveyed { get; set; }
		#endregion
		#region RecentCaseCreationDate
		public abstract class recentCaseCreationDate : IBqlField { }
		[PXDBDateAndTime(BqlField = typeof(CRCase.createdDateTime))]
		[PXUIField(DisplayName = "Recent Case Creation Date")]
		public virtual DateTime? RecentCaseCreationDate { get; set; }
		#endregion
		#region SurveyCaseCount
		public abstract class surveyCaseCount : IBqlField { }
		[PXDBInt(BqlField = typeof(CRCase.caseID))]
		[PXUIField(DisplayName = "Cases", Visible = false)]
		public virtual Int32? SurveyCaseCount { get; set; }
		#endregion
		#region UserFullName
		public abstract class userFullName : IBqlField { }
		[PXDBString(BqlField = typeof(PX.SM.Users.fullName))]
		[PXUIField(DisplayName = "Support Engineer")]
		public virtual String UserFullName { get; set; }
		#endregion
		#region CaseCD
		public abstract class caseCD : IBqlField { }
		[PXDBString(50, IsUnicode = true, BqlField = typeof(CRCase.caseCD))]
		[PXUIField(DisplayName = "Most Recent Case")]
		public virtual String CaseCD { get; set; }
		#endregion
		#region CaseOwnerID
		public abstract class caseOwnerID : IBqlField { }
		[PXDBGuid(IsKey = true, BqlField = typeof(CRCase.ownerID))]
		public virtual Guid? CaseOwnerID { get; set; }
		#endregion
		#region TechUserName
		public abstract class techUserName : IBqlField { }
		[PXDBString(BqlField = typeof(PX.SM.Users.username))]
		[PXUIField(DisplayName = "User ID", Visible = false)]
		public virtual string TechUserName { get; set; }
		#endregion
		#region TechFirstName
		public abstract class techFirstName : IBqlField { }
		[PXDBString(BqlField = typeof(PX.SM.Users.firstName))]
		[PXUIField(DisplayName = "Tech First Name", Visible = false)]
		public virtual string TechFirstName { get; set; }
		#endregion
		#region TechLastName
		public abstract class techLastName : IBqlField { }
		[PXDBString(BqlField = typeof(PX.SM.Users.lastName))]
		[PXUIField(DisplayName = "Tech Last Name", Visible = false)]
		public virtual string TechLastName { get; set; }
		#endregion
		#region RecentCaseResolutionDate
		public abstract class recentCaseResolutionDate : IBqlField { }
		[PXDBDateAndTime(BqlField = typeof(CRCase.resolutionDate))]
		[PXUIField(DisplayName = "Recent Case Resolution Date")]
		public virtual DateTime? RecentCaseResolutionDate { get; set; }
		#endregion
		#region ContactFirstName
		public abstract class contactFirstName : IBqlField { }
		[PXDBString(IsUnicode = true, BqlField = typeof(Contact.firstName))]
		[PXUIField(DisplayName = "Contact First Name")]
		public virtual String ContactFirstName { get; set; }
		#endregion
		#region ContactLastName
		public abstract class contactLastName : IBqlField { }
		[PXDBString(IsUnicode = true, BqlField = typeof(Contact.lastName))]
		[PXUIField(DisplayName = "Contact Last Name")]
		public virtual String ContactLastName { get; set; }
		#endregion
		#region CaseSubject
		public abstract class caseSubject : IBqlField { }
		[PXDBString(255, IsUnicode = true, BqlField = typeof(CRCase.subject))]
		[PXUIField(DisplayName = "Most Recent Case Subject")]
		public virtual String CaseSubject { get; set; }
		#endregion
		#region NoteId
		public abstract class noteID : IBqlField { }
		[PXNote(BqlField = typeof(CRCase.noteID))]
		public virtual Guid? NoteID { get; set; }
		#endregion
		#region CaseCustomerID
		public abstract class caseCustomerID : IBqlField { }
		[PXDBInt(BqlField = typeof(CRCase.customerID))]
		[PXUIField(DisplayName = "Customer ID", Visible = false)]
		public virtual Int32? CaseCustomerID { get; set; }
		#endregion
		#region CaseClassID
		public abstract class caseClassID : IBqlField { }
		[PXDBString(10, IsKey = true, IsUnicode = true, BqlField = typeof(CRCase.caseClassID))]
        [PXUIField(DisplayName = "Case Class")]
        public virtual String CaseClassID { get; set; }
		#endregion
		#region PartnerGroup
		public abstract class partnerGroup : IBqlField { }
		[PXDBString(IsUnicode = true, BqlField = typeof(CSAnswers.value))]
		[PXUIField(DisplayName = "Partner Group")]
		public virtual string PartnerGroup { get; set; }
        #endregion
        #region SurveyUrl
        public abstract class surveyUrl : IBqlField { }
        [PXDBString(IsUnicode = true, BqlField = typeof(CRCaseClassExt.usrSurveyURL))]
        [PXUIField(DisplayName = "Survey Url")]
        public virtual string SurveyUrl { get; set; }
        #endregion
        #region NotificationMapID
        public abstract class notificationMapID : IBqlField { }
        [PXDBInt(BqlField = typeof(CRCaseClassExt.usrNotificationMapID))]
        [PXUIField(DisplayName = "Notification ID")]
        public virtual Int32? NotificationMapID { get; set; }
        #endregion
        #region NotificationNFrom
        public abstract class notificationNFrom : IBqlField { }
        [PXDBInt(BqlField = typeof(Notification.nfrom))]
        [PXUIField(DisplayName = "Notification From")]
        public virtual Int32? NotificationNFrom { get; set; }
        #endregion
    }

    public static class AttribNamesCustom
	{
		public class PartnerGroupAttrbName : Constant<string>
		{
			public PartnerGroupAttrbName() : base("PARTNERGRP") { }
		}
	}
}
