using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AnswersModel
    {
        public int id { get; set; }
        public int? questionId { get; set; }
        public string answer { get; set; }
        public float? points { get; set; }
        public int? answerOrder { get; set; }
        public bool? isAutoFail { get; set; }
        public bool? autoSelect { get; set; }
        public bool? rightAnswer { get; set; }
        public bool? commentRequired { get; set; }
        public bool? answerActive { get; set; }
        public string csText { get; set; }
        public int? csId { get; set; }
    }
}
