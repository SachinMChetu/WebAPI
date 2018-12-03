//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class app_settings
    {
        public string recording_url { get; set; }
        public string recording_user { get; set; }
        public string record_password { get; set; }
        public string appname { get; set; }
        public int id { get; set; }
        public string recording_dirs { get; set; }
        public string wav_key { get; set; }
        public string url_prefix { get; set; }
        public string manager { get; set; }
        public Nullable<bool> uses_notifications { get; set; }
        public string ftp_site { get; set; }
        public string ftp_port { get; set; }
        public string ftp_login { get; set; }
        public string ftp_password { get; set; }
        public Nullable<int> default_score { get; set; }
        public Nullable<int> notification_score { get; set; }
        public string vertical { get; set; }
        public Nullable<double> fixed_bonus { get; set; }
        public string FullName { get; set; }
        public string logo { get; set; }
        public string import_file { get; set; }
        public Nullable<double> bill_rate { get; set; }
        public string previous_file { get; set; }
        public Nullable<int> priority { get; set; }
        public Nullable<bool> auto_submit { get; set; }
        public Nullable<double> margin { get; set; }
        public Nullable<int> fail_score { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<System.DateTime> down_time { get; set; }
        public Nullable<int> default_calib { get; set; }
        public Nullable<bool> isCalibrated { get; set; }
        public Nullable<int> NA_affect { get; set; }
        public Nullable<bool> truncate_scores { get; set; }
        public Nullable<double> top_score { get; set; }
        public string listen_template { get; set; }
        public Nullable<bool> show_section_score { get; set; }
        public Nullable<int> calibration_count { get; set; }
        public Nullable<int> current_scorecard { get; set; }
        public Nullable<int> use_scorecard { get; set; }
        public string record_format { get; set; }
        public string audio_port { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public string base_color { get; set; }
        public Nullable<int> calibration_percent { get; set; }
        public Nullable<bool> email_on_fail { get; set; }
        public string host_key { get; set; }
        public Nullable<bool> stream_only { get; set; }
        public string app_difficulty { get; set; }
        public string app_status { get; set; }
        public string review_type { get; set; }
        public string first_noti { get; set; }
        public string client_logo { get; set; }
        public Nullable<bool> redaction { get; set; }
        public string lead_response { get; set; }
        public Nullable<bool> agent_summary_only { get; set; }
        public Nullable<bool> Weekly_Email { get; set; }
        public Nullable<bool> Daily_Email { get; set; }
        public Nullable<int> notification_profile { get; set; }
        public Nullable<int> setting_profile { get; set; }
        public string account_manager { get; set; }
        public Nullable<int> rejection_profile { get; set; }
        public string dashboard { get; set; }
        public string client_logo_small { get; set; }
        public Nullable<double> transcript_rate { get; set; }
        public Nullable<int> minimum_minutes { get; set; }
        public Nullable<double> budget { get; set; }
        public bool allowDocumentUpload { get; set; }
        public string smtp_host { get; set; }
        public Nullable<int> smtp_port { get; set; }
        public string smtp_username { get; set; }
        public string smtp_password { get; set; }
        public string smtp_email { get; set; }
        public string smtp_friendly_name { get; set; }
    }
}