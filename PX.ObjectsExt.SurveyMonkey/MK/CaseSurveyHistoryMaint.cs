using PX.Data;
using PXSurveyMonkeyMKExt.DAC;

namespace PXSurveyMonkeyMKExt
{
    public class CaseSurveyHistoryMaint : PXGraph<CaseSurveyHistoryMaint>
    {
        public PXSelect<CaseSurveyHistory> CaseSurveyHistoryRecord;
        public PXSave<CaseSurveyHistory> Save;
        public PXCancel<CaseSurveyHistory> Cancel;
    }
}
