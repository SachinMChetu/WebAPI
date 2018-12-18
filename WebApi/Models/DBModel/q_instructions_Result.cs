using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class q_instructions_Result
    {
        public int id { get; set; }
        public Nullable<int> question_id { get; set; }
        public string question_text { get; set; }
        public string answer_type { get; set; }
        public Nullable<int> q_order { get; set; }
        public Nullable<System.DateTime> dateadded { get; set; }
    }
}