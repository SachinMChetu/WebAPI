using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getListenQuestionsAll_Result
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string q_short { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? section { get; set; }
        public int? category { get; set; }
        public int? q_order { get; set; }
        public string question { get; set; }
        public string q_type { get; set; }
        public string q_short_name { get; set; }
        public bool? active { get; set; }
        public int? heading { get; set; }
        public int? Order { get; set; }
        public string QuestionText { get; set; }
        //public double? q_percent { get; set; }
        public string agent_display { get; set; }
        public string template { get; set; }
        public string template_items { get; set; }
        //public double? scorecard_id { get; set; }
        //public float? QA_points { get; set; }
        public string options_verb { get; set; }
        public int? linked_answer { get; set; }
        public int? linked_comment { get; set; }
        public bool? linked_visible { get; set; }
        public bool? single_comment { get; set; }
        public bool? wide_q { get; set; }
        public bool? require_custom_comment { get; set; }
        public string ddl_type { get; set; }
        public bool? left_column_question { get; set; }
        public string sentence { get; set; }
        public string ddlQuery { get; set; }
        public bool? comments_allowed { get; set; }
        

    }
}