using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.SM;
using PXSurveyMonkeyCRExt.DAC;
using PXSurveyMonkeyMKExt.DAC;

namespace PXSurveyMonkeyMKExt
{
    public class CaseSurveyProcessing : PXGraph<CaseSurveyProcessing>
    {

        public PXCancel<SurveyProcessFilter> Cancel;

        public PXFilter<SurveyProcessFilter> Filter;

        public PXFilteredProcessing<SurveyContactInfo, SurveyProcessFilter,
                Where<Where2<Where<SurveyContactInfo.usrLastSurveyed, IsNull,
                Or<DateDiff<SurveyContactInfo.usrLastSurveyed, Now, DateDiff.day>, Greater<Current<SurveyProcessFilter.numberOfDays>>>>,
                    And<DateDiff<SurveyContactInfo.recentCaseResolutionDate, Now, DateDiff.day>,
                                  LessEqual<Current<SurveyProcessFilter.numberOfDaysMostRecentCaseCreatedIn>>>>>> Records;

        public CaseSurveyProcessing()
        {
            this.Records.SetProcessDelegate(ProcessSurvey);
        }

        public static void ProcessSurvey(List<SurveyContactInfo> contacts)
        {
            bool erroroccurred = false;
            
            ContactMaint graphNotification = PXGraph.CreateInstance<ContactMaint>();
            Notification rowNotification = PXSelectJoin<Notification,
                InnerJoin<CRSetup, On<Notification.notificationID, Equal<CRSetupExt.usrNotificationMapID>>>>.Select(graphNotification);

            if (rowNotification == null)
            {
                throw new PXException("Notification Template for Case Survey is not specified.");
            }            

            List<SurveyContactInfo> contactsToProceed = new List<SurveyContactInfo>(contacts);
            
            CaseSurveyHistoryMaint graph = PXGraph.CreateInstance<CaseSurveyHistoryMaint>();

            //Get From Address from Notification Template if not specified then use default
            string sFromEmail = PX.Data.EP.MailAccountManager.GetDefaultEmailAccount().Address;
            if (rowNotification.NFrom.HasValue)
            {
                PX.SM.EMailAccount EMA = PXSelect<PX.SM.EMailAccount,
                    Where<PX.SM.EMailAccount.emailAccountID, Equal<Required<PX.SM.EMailAccount.emailAccountID>>>>.Select(graphNotification, rowNotification.NFrom);
                if (EMA != null)
                {
                    sFromEmail = EMA.Address;
                }
            }

            if (String.IsNullOrEmpty(sFromEmail))
            {
                throw new PXException("E-mail account is not setup to send Surveys.");
            }

            foreach (var rec in contactsToProceed)
            {
                try
                {
                    if (!String.IsNullOrEmpty(rec.EMail))
                    {
                        AddEmailActivity(rec, rowNotification);

                        //Update Case Survey History
                        CaseSurveyHistory newCaseHistory = new CaseSurveyHistory();
                        newCaseHistory.ContactID = rec.ContactID;
                        newCaseHistory.OwnerID = rec.CaseOwnerID;

                        graph.CaseSurveyHistoryRecord.Insert(newCaseHistory);
                        graph.Actions.PressSave();

                        PXProcessing<SurveyContactInfo>.SetInfo(contacts.IndexOf(rec),
                                                        String.Format("Survey has been sent to {0}", rec.EMail));
                    }
                    else
                    {
                        erroroccurred = true;
                        PXProcessing<SurveyContactInfo>.SetError(contacts.IndexOf(rec),
                                                        String.Format("E-mail address is not specified."));
                    }
                }
                catch (Exception e)
                {
                    erroroccurred = true;
                    PXProcessing<SurveyContactInfo>.SetError(contacts.IndexOf(rec), e);
                }
            }
            if (erroroccurred)
                throw new PXException("At least one Survey hasn't been processed.");
        }

        private static void AddEmailActivity(SurveyContactInfo CurrentCase, Notification rowNotiication)
        {
            bool sent = false;
            string sError = "Failed to send E-mail.";
            try
            {
                var sender = TemplateNotificationGenerator.Create(CurrentCase, rowNotiication.NotificationID.Value);
                sender.MailAccountId = (rowNotiication.NFrom.HasValue) ? rowNotiication.NFrom.Value :
                                                         PX.Data.EP.MailAccountManager.DefaultMailAccountID;
                sender.To = CurrentCase.EMail;
                sent |= sender.Send().Any();
            }
            catch (Exception Err)
            {
                sent = false;
                sError = Err.Message;
            }
            if (!sent)
                throw new PXException(sError);
        }
    }
}