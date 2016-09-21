using System;
using System.Collections.Generic;

namespace PX.SurveyMonkeyReader.Models
{
    public class SurveyResponse
    {
        public string CaseCD { get; set; }
        public long ResponseID { get; set; }
        public DateTime ResponseDate { get; set; }
        public List<SurveyResponseQuestion> Questions { get; set; }
    }
}
