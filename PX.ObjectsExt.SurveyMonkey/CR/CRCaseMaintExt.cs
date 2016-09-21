using PX.Data;
using PX.Objects.CR;
using PXSurveyMonkeyCRExt.DAC;
using PXSurveyMonkeyMKExt.DAC;

namespace PXSurveyMonkeyCRExt
{
    public class CRCaseMaintExt : PXGraphExtension<CRCaseMaint>
    { 
        public PXSelectJoin<CRCaseSurveyResponse,
            RightJoin<CRSurveyQuestion, On<CRCaseSurveyResponse.questionID, Equal<CRSurveyQuestion.questionID>>,
                FullJoin<ParentCRSurveyQuestion, On<CRSurveyQuestion.parentQuestionID, Equal<ParentCRSurveyQuestion.questionID>>,
                    FullJoin<CRSurveyAnswer,
                        On<CRSurveyAnswer.questionID, Equal<CRCaseSurveyResponse.questionID>,
                            And<CRSurveyAnswer.answerLineNbr, Equal<CRCaseSurveyResponse.answerLineNbr>>>>>>,
            Where<CRCaseSurveyResponse.caseID, Equal<Current<CRCaseSurveyResponse.caseID>>>> Response;

        public override void Initialize()
        {
            base.Initialize();
            
            Response.AllowInsert = false;
            Response.AllowDelete = false;
        }

        public virtual void CRCase_UsrInternalComment_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            if (e.Row == null)
                return;

            sender.SetValue<CRCaseExt.usrInternalCommentByName>(e.Row, Base.Accessinfo.UserName);
        }

        protected virtual void CRCaseSurveyResponse_AnswerLineNbr_FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e)
        {
            if (e.Row == null)
                return;

            int answerLineNbr;
            if (int.TryParse(e.ReturnValue?.ToString(), out answerLineNbr) && answerLineNbr == 0)
            {
                e.ReturnValue = string.Empty;
            }
        }

        protected virtual void CRCaseSurveyResponse_AnswerLineNbr_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            if (e.Row == null)
                return;

            if (e.NewValue == null)
            {
                e.NewValue = 0;
            }
        }

        public virtual void CRCase_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            if (e.Row == null)
                return;

            var @case = (CRCase) e.Row;
            var crCaseExt = PXCache<CRCase>.GetExtension<CRCaseExt>(@case);

            PXUIFieldAttribute.SetEnabled<CRCaseExt.usrInternalComment>(cache, e.Row, crCaseExt.UsrResponseDate != null);
            PXUIFieldAttribute.SetEnabled<CRCaseExt.usrSurveyReportingEligible>(cache, e.Row, crCaseExt.UsrResponseDate != null);
        }
    }
}
