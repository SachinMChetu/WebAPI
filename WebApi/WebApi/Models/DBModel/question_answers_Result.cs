using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class question_answers_Result
    {
       
        public string answer_text { get; set; }
        public bool? acp_required { get; set; }
        //public float? answer_points { get; set; }
        public bool? custom_comment_required { get; set; }
        public bool? right_answer { get; set; }
        public bool? autoselect { get; set; }
        public int id { get; set; }
    }
}