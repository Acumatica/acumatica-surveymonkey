using System;
using System.Collections.Generic;

namespace PX.SurveyMonkeyReader.Models
{
    public class SurveyQuestion
    {
        public DateTime SurveyLastModified { get; set; }
        public long? ParentQuestionID { get; set; }
        public long QuestionID { get; set; }
        public string Question { get; set; }
        public List<SurveyAnswer> Answers;
    }
}
