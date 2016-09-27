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

        // NOTE:  When the AnswerLineNbr for a CRCaseSurveyResponse row is equal to 0, that means is no entry for it in the CRSurveyAnswers table.
        // This means it either hasn't been answered, or has a freetext answer.  Either way, we don't want to display "0" in the Answer column.
        // So the following event handler replaces any "0" found in the Answer column with the empty string.
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

        // To follow up from the above event handler - we cannot put null entries into the Answer column, which is part of the primary key.
        // So when we update this field, any zeroes we replaced with the empty string need to be switched back to "0".
        // It should be noted that although we set these values to the empty string to display them, they will appear null to this event handler.
        protected virtual void CRCaseSurveyResponse_AnswerLineNbr_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            if (e.Row == null)
                return;

            if (e.NewValue == null)
            {
                e.NewValue = 0;
            }
        }

        // If a case has no survey responses, set the Internal Comment field and SurveyReportingEligible checkbox to read-only.
        public virtual void CRCase_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            if (e.Row == null)
                return;

            var crCase = (CRCase) e.Row;
            var crCaseExt = PXCache<CRCase>.GetExtension<CRCaseExt>(crCase);

            PXUIFieldAttribute.SetEnabled<CRCaseExt.usrInternalComment>(cache, e.Row, crCaseExt.UsrResponseDate != null);
            PXUIFieldAttribute.SetEnabled<CRCaseExt.usrSurveyReportingEligible>(cache, e.Row, crCaseExt.UsrResponseDate != null);
        }
    }
}
