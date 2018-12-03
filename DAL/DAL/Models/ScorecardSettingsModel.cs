using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ScorecardSettingsModel
    {
        public int id { get; set; }//
        public string description { get; set; }//
        public string short_name { get; set; }//
        public string appname { get; set; }//
        public DateTime date_added { get; set; }//
        public string who_added { get; set; }
        public bool active { get; set; }//
        public string review_type { get; set; }//
        public string golden_user { get; set; }
        public int calib_percent { get; set; }
        public bool isCalibrated { get; set; }
        public int fail_score { get; set; }
        public string team_lead { get; set; }
        public string sc_sort { get; set; }//
        public int training_count { get; set; }
        public bool ni_scorecard { get; set; }
        public bool transcribe { get; set; }
        public string score_type { get; set; }
        public int recal_percent { get; set; }
        public bool sectionless { get; set; }
        public float website_cost { get; set; }
        public string website_display { get; set; }
        public int min_transcript_count { get; set; }
        public string user_cant_dispute { get; set; }
        public int max_speed { get; set; }
        public float min_cal { get; set; }
        public int num_cal_check { get; set; }
        public string import_type { get; set; }
        public int import_percent { get; set; }
        public string required_dispositions { get; set; }
        public int min_call_length { get; set; }
        public string post_import_sp { get; set; }
        public int pass_percent { get; set; }
        public int cutoff_percent { get; set; }
        public int cutoff_count { get; set; }
        public int import_agents { get; set; }
        public bool keep_daily_calls { get; set; }
        public string hide_data { get; set; }
        public string hide_school_data { get; set; }
        public int sc_notification_score { get; set; }
        public int cutoff_percent_avg { get; set; }
        public string scorecard_status { get; set; }
        public int sc_notification_profile { get; set; }
        public int sc_profile { get; set; }
        public bool dedupe { get; set; }
        public int max_per_day { get; set; }
        public bool no_agent_login { get; set; }
        public bool redact { get; set; }
        public string account_manager { get; set; }
        public bool email_failed { get; set; }
        public bool show_custom_questions { get; set; }
        public bool onhold { get; set; }
        public int retain_non_used_calls { get; set; }
        public int max_call_length { get; set; }
        public string meta_sort { get; set; }
        public bool overwrite_group { get; set; }
        public bool tango_calibrated { get; set; }
        public string calib_role { get; set; }
        public string qa_selected_role { get; set; }
        public string admin_selected_role { get; set; }
        public string client_selected_role { get; set; }
        public string recalib_role { get; set; }
        public bool manager_sees_supervisor { get; set; }
        public int rejection_profile { get; set; }
        public string tango_team_lead { get; set; }
        public float truncate_time { get; set; }
        public float end_truncate_time { get; set; }
        public bool high_priority { get; set; }
        public float load_rate_15 { get; set; }
        public float load_rate_60 { get; set; }
        public float burn_rate_15 { get; set; }
        public float burn_rate_60 { get; set; }
        public int working_team { get; set; }
        public int pending_queue { get; set; }
        public float avg_review_time { get; set; }
        public float avg_call_length { get; set; }
        public int qa_qa_scorecard { get; set; }
        public string shift_end { get; set; }
        public string shift_start { get; set; }
        public int allow_others { get; set; }
        public bool isQAQACard { get; set; }
        public int calibration_floor { get; set; }
        public float call_turn_time { get; set; }
        public bool auto_accept_bad_call { get; set; }
        public DateTime allow_other_set { get; set; }
        public string pay_type { get; set; }
        public string qa_pay { get; set; }
        public string cal_spot_user_role { get; set; }
        public int dispute_base_percent { get; set; }

    }
}
