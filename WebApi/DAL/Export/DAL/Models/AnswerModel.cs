using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AnswerModel
    {
        public int id { get; set; }
        public int questionId { get; set; }
        public string answerText { get; set; }
        public float answerPoints { get; set; }
        public bool isAutoFail { get; set; }
        public bool autoselect { get; set; }
        public bool rightAnswer { get; set; }
        public string linkedAnswer { get; set; }
        public int oldAnsewerId { get; set; }
        public int oldQuestionId { get; set; }
        public bool acpRequired { get; set; }
        public int? answerOrder { get; set; }
        public bool answeractive { get; set; }
        public string csTextReturned { get; set; }
        public int csIdReturned { get; set; }

    }
}
