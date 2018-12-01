using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AnswerCommentModel
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int? answerId { get; set; }
        public int? questionId { get; set; }
        public int? answerPoints { get; set; }
        public int? fixedPos { get; set; }
        public int? answerOrder { get; set; }
        public bool? active { get; set; }
        public string csText { get; set; }
        public int? csId { get; set; }
        public string answerStatement { get; set; }
    }
}
