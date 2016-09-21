using System;

namespace PX.SurveyMonkeyReader.Models
{
    public class SurveyResponseQuestion
    {
        public DateTime SurveyLastModified { get; set; }
        public long? ParentQuestionID { get; set; }
        public QuestionAnswerIdentifier ResponseIdentifier { get; set; }
        public string Answer { get; set; }
    }
}
