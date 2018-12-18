using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getPopulatedScorecardComplex_Result
    {
        public int id { get; set; }
        public string description { get; set; }
        public string short_name { get; set; }
        public string appname { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public string who_added { get; set; }
        public Nullable<bool> active { get; set; }
        public string review_type { get; set; }
        public string golden_user { get; set; }
        public Nullable<int> calib_percent { get; set; }
        public Nullable<bool> isCalibrated { get; set; }
        public Nullable<int> fail_score { get; set; }
        public string team_lead { get; set; }
        public string sc_sort { get; set; }
        public Nullable<int> training_count { get; set; }
        public Nullable<bool> ni_scorecard { get; set; }
        public Nullable<bool> transcribe { get; set; }
        public string score_type { get; set; }
        public Nullable<int> recal_percent { get; set; }
        public Nullable<bool> sectionless { get; set; }
        public Nullable<double> website_cost { get; set; }
        public string website_display { get; set; }
        public Nullable<int> min_transcript_count { get; set; }
        public string user_cant_dispute { get; set; }
        public Nullable<int> max_speed { get; set; }
        public Nullable<double> min_cal { get; set; }
        public Nullable<int> num_cal_check { get; set; }
        public string import_type { get; set; }
        public Nullable<int> import_percent { get; set; }
        public string required_dispositions { get; set; }
        public Nullable<int> min_call_length { get; set; }
        public string post_import_sp { get; set; }
        public Nullable<int> pass_percent { get; set; }
        public Nullable<int> cutoff_percent { get; set; }
        public Nullable<int> cutoff_count { get; set; }
        public Nullable<int> import_agents { get; set; }
        public Nullable<bool> keep_daily_calls { get; set; }
        public string hide_data { get; set; }
        public string hide_school_data { get; set; }
        public Nullable<int> sc_notification_score { get; set; }
        public Nullable<int> cutoff_percent_avg { get; set; }
        public string scorecard_status { get; set; }
        public Nullable<int> sc_notification_profile { get; set; }
        public Nullable<int> sc_profile { get; set; }
        public Nullable<bool> dedupe { get; set; }
        public Nullable<int> max_per_day { get; set; }
        public Nullable<bool> no_agent_login { get; set; }
        public Nullable<bool> redact { get; set; }
        public string account_manager { get; set; }
        public Nullable<bool> email_failed { get; set; }
        public Nullable<bool> show_custom_questions { get; set; }
        public Nullable<bool> onhold { get; set; }
        public Nullable<int> retain_non_used_calls { get; set; }
        public Nullable<int> max_call_length { get; set; }
        public string meta_sort { get; set; }
        public Nullable<bool> overwrite_group { get; set; }
        public Nullable<bool> tango_calibrated { get; set; }
        public string calib_role { get; set; }
        public string qa_selected_role { get; set; }
        public string admin_selected_role { get; set; }
        public string client_selected_role { get; set; }
        public string recalib_role { get; set; }
        public Nullable<bool> manager_sees_supervisor { get; set; }
        public Nullable<int> rejection_profile { get; set; }
        public string tango_team_lead { get; set; }
        public Nullable<double> truncate_time { get; set; }
        public Nullable<double> end_truncate_time { get; set; }
        public Nullable<bool> high_priority { get; set; }
        public Nullable<double> load_rate_15 { get; set; }
        public Nullable<double> load_rate_60 { get; set; }
        public Nullable<double> burn_rate_15 { get; set; }
        public Nullable<double> burn_rate_60 { get; set; }
        public Nullable<int> working_team { get; set; }
        public Nullable<int> pending_queue { get; set; }
        public Nullable<double> avg_review_time { get; set; }
        public Nullable<double> avg_call_length { get; set; }
        public Nullable<int> qa_qa_scorecard { get; set; }
        public string shift_end { get; set; }
        public string shift_start { get; set; }
        public Nullable<int> allow_others { get; set; }
        public Nullable<bool> isQAQACard { get; set; }
        public Nullable<int> calibration_floor { get; set; }
        public Nullable<double> call_turn_time { get; set; }
        public Nullable<bool> auto_accept_bad_call { get; set; }
        public Nullable<System.DateTime> allow_other_set { get; set; }
        public string pay_type { get; set; }
        public string qa_pay { get; set; }
        public string cal_spot_user_role { get; set; }
        public Nullable<int> dispute_base_percent { get; set; }
        public Nullable<int> delete_after_days { get; set; }
        public Nullable<int> monthly_minute_cap { get; set; }
        public Nullable<int> qless_parent { get; set; }
        public Nullable<bool> third_party_scorecard { get; set; }
        public string listen_type { get; set; }
    }
}