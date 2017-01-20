using System;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.CR;
using PX.SurveyMonkeyReader.Models;
using PXSurveyMonkeyCRExt.DAC;

namespace PXSurveyMonkeyMKExt
{
    public class CaseSurveyResponseProcessing : PXGraph<CaseSurveyResponseProcessing>
    {
        [Serializable]
        [PXProjection(typeof(Select<CRSetup>))]
        public class CRCaseSurveySetup : IBqlTable, IPXSelectable
        {
            #region Selected
            public abstract class selected : IBqlField { }
            [PXBool]
            [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "Selected")]
            public bool? Selected { get; set; }
            #endregion
            #region UsrSurveyID
            public abstract class usrSurveyID : IBqlField { }
            [PXDBString(20, BqlField = typeof(CRSetupExt.usrSurveyID))]
            [PXUIField(DisplayName = "Survey ID")]
            public virtual string UsrSurveyID { get; set; }
            #endregion
            #region UsrLastSurveySyncDate
            public abstract class usrLastSurveySyncDate : IBqlField { }
            [PXDBDate(BqlField = typeof(CRSetupExt.usrLastSurveySyncDate), UseTimeZone = false, PreserveTime = true)]
            [PXUIField(DisplayName = "Last Survey Sync Date (UTC)")]
            public virtual DateTime? UsrLastSurveySyncDate { get; set; }
            #endregion
        }

        public PXSetup<CRSetup> CRSetup;
        public PXCancel<CRCaseSurveySetup> Cancel;
        public PXProcessing<CRCaseSurveySetup> SurveyList;

        public CaseSurveyResponseProcessing()
        {
            SurveyList.SetProcessDelegate(ProcessSurveyResponse);
        }

        public static void ProcessSurveyResponse(List<CRCaseSurveySetup> setup)
        {
            var graph = PXGraph.CreateInstance<CaseSurveyResponseEngine>();
            var setupRecord = graph.SetupRecord.SelectSingle();
            var setupRecordExt = graph.SetupRecord.Cache.GetExtension<CRSetupExt>(setupRecord);

            var fromDate = setupRecordExt.UsrLastSurveySyncDate;
            var toDate = DateTime.UtcNow;

            var dataReader = new PX.SurveyMonkeyReader.SurveyMonkeyReader(setupRecordExt.UsrSurveyID,
                setupRecordExt.UsrAccessToken);

            List<SurveyResponse> surveyResponses;
            try
            {
                surveyResponses = dataReader.GetSurveyResponsesByDateRange(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new PXException(ex.Message);
            }

            foreach (var surveyResponse in surveyResponses)
            {
                graph.Cases.Current = (CRCase) graph.Cases.Search<CRCase.caseCD>(surveyResponse.CaseCD);
                if (graph.Cases.Current == null)
                    continue;
                
                // Surveys can technically be answered by more than one person; if responses exist already just skip over the response since
                // the integration doesn't handle multiple responses per case.
                if (graph.CaseSurveyResponses.SelectSingle() != null)
                    continue;

                var caseExt = PXCache<CRCase>.GetExtension<CRCaseExt>(graph.Cases.Current);
                caseExt.UsrResponseID = surveyResponse.ResponseID;
                caseExt.UsrResponseDate = surveyResponse.ResponseDate;
                graph.Cases.Current = graph.Cases.Update(graph.Cases.Current);

                foreach (var question in surveyResponse.Questions)
                {
                    graph.SetQuestionAnswer(dataReader, question);
                }
                
                graph.Actions.PressSave();
            }

            setupRecordExt.UsrLastSurveySyncDate = toDate;
            graph.SetupRecord.Update(setupRecord);

            graph.Actions.PressSave();
           
            PXProcessing<CRCaseSurveySetup>.SetInfo("Successfully synched");
        }
    }
}