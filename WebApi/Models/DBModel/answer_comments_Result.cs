﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class answer_comments_Result
    {
        public int id { get; set; }
        public string comment { get; set; }
        public Nullable<int> answer_id { get; set; }
        public Nullable<int> question_id { get; set; }
        public Nullable<int> last_id { get; set; }
        public Nullable<bool> non_billable { get; set; }
        public Nullable<bool> special2 { get; set; }
        public Nullable<int> ac_order { get; set; }
        public Nullable<int> comment_points { get; set; }
        public Nullable<int> old_ac_id { get; set; }
        public Nullable<int> old_ac_qid { get; set; }
        public Nullable<bool> ac_active { get; set; }
        public string cs_text_returned { get; set; }
        public Nullable<int> cs_id_returned { get; set; }
        public string answer_statement { get; set; }
        public Nullable<int> univeral_postition { get; set; }
    }
}