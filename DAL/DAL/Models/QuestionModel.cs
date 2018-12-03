using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class QuestionModel
    {
        public int id { get; set; }
        public int section { get; set; }
        public int category { get; set; }
        public int q_order { get; set; }
        public string question { get; set; }
        public string q_type { get; set; }
        public string q_short_name { get; set; }
        public bool active { get; set; }
        public int heading { get; set; }
        public int order { get; set; }
        public string questionText { get; set; }
        public float q_percent { get; set; }
        public string appname { get; set; }
        public bool auto_yes { get; set; }
        public bool auto_no { get; set; }
        public string agent_display { get; set; }
        public int default_answer { get; set; }
        public int orig_id { get; set; }
        public float total_points { get; set; }
        public string template { get; set; }
        public string temlate_items { get; set; }
        public string linked_question { get; set; }
        public bool email_wrong { get; set; }
        public string campaign_specific { get; set; }
        public int scorecard_id { get; set; }
        public float qaPoints { get; set; }
        public bool compliance { get; set; }
        public DateTime? date_q_added { get; set; }
        public bool non_billable { get; set; }
        public bool comments_allowed { get; set; }
        public int linked_answer { get; set; }
        public int linked_comment { get; set; }
        public bool client_visible { get; set; }
        public bool client_guideline_visible { get; set; }
        public int sectionlessOrder { get; set; }
        public bool linkedVisible { get; set; }
        public bool client_dashboard_visible { get; set; }
        public bool pinned { get; set; }
        public bool pre_production { get; set; }
        public bool single_comment { get; set; }
        public bool points_paused { get; set; }
        public DateTime? points_paused_date { get; set; }
        public bool full_width { get; set; }
        public bool wide_q { get; set; }
        public bool require_custom_comment { get; set; }
        public string sentence { get; set; }
        public string ddl_type { get; set; }
        public string ddlQuery { get; set; }
        public string options_verb { get; set; }
        public bool left_column_question { get; set; }
        public string sectionName { get; set; }
        public string linkedQuestionName { get; set; }
        public string linkedAnswerText { get; set; }
        public string linkedCommentText { get; set; }

    }
}
