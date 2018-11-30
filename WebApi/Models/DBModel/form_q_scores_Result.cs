using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class form_q_scores_Result
    {
        public int? section_id { get; set; }
        public string q_type { get; set; }
        public string q_short_name { get; set; }
        public string options_verb { get; set; }
        public Nullable<bool> left_column_question { get; set; }
        public string sentence { get; set; }
        public string answer_text { get; set; }
        public string q_position { get; set; }
        public Nullable<int> q_id { get; set; }
        public Nullable<double> QA_points { get; set; }
        public string view_link { get; set; }
        public Nullable<bool> comments_allowed { get; set; }
        public Nullable<bool> right_answer { get; set; }
    }
}