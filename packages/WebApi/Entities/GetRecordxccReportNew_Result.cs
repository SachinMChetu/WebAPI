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
    
    public partial class GetRecordxccReportNew_Result
    {
        public Nullable<int> F_ID { get; set; }
        public int X_ID { get; set; }
        public string SESSION_ID { get; set; }
        public string AGENT { get; set; }
        public string DISPOSITION { get; set; }
        public string CAMPAIGN { get; set; }
        public string ANI { get; set; }
        public string DNIS { get; set; }
        public string TIMESTAMP { get; set; }
        public string TALK_TIME { get; set; }
        public Nullable<System.DateTime> CALL_TIME { get; set; }
        public string HANDLE_TIME { get; set; }
        public string CALL_TYPE { get; set; }
        public string LIST_NAME { get; set; }
        public string leadid { get; set; }
        public string AGENT_GROUP { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Nullable<double> Datacapturekey { get; set; }
        public Nullable<double> Datacapture { get; set; }
        public string Status { get; set; }
        public string Program { get; set; }
        public int ID { get; set; }
        public string Datacapture_Status { get; set; }
        public string num_of_schools { get; set; }
        public Nullable<int> MAX_REVIEWS { get; set; }
        public Nullable<System.DateTime> review_started { get; set; }
        public string Number_of_Schools { get; set; }
        public string EducationLevel { get; set; }
        public string HighSchoolGradYear { get; set; }
        public string DegreeStartTimeframe { get; set; }
        public string appname { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public Nullable<System.DateTime> call_date { get; set; }
        public string audio_link { get; set; }
        public string profile_id { get; set; }
        public string audio_user { get; set; }
        public string audio_password { get; set; }
        public Nullable<int> bad_call { get; set; }
        public string bad_call_who { get; set; }
        public Nullable<System.DateTime> bad_call_date { get; set; }
        public Nullable<System.DateTime> bad_call_delete_date { get; set; }
        public string bad_call_reason { get; set; }
        public Nullable<bool> to_upload { get; set; }
        public Nullable<int> pending_id { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public string AreaOfInterest { get; set; }
        public string ProgramsOfInterestType { get; set; }
        public string Citizenship { get; set; }
        public string DegreeOfInterest { get; set; }
        public string Gender { get; set; }
        public string Military { get; set; }
        public string secondphone { get; set; }
        public string agent_name { get; set; }
        public Nullable<System.DateTime> bad_call_accepted { get; set; }
        public string bad_call_accepted_who { get; set; }
        public Nullable<int> scorecard { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> uploaded { get; set; }
        public string text_only { get; set; }
        public Nullable<bool> file_missing { get; set; }
        public string fileUrl { get; set; }
        public string statusMessage { get; set; }
        public string mediaId { get; set; }
        public string requestStatus { get; set; }
        public string fileStatus { get; set; }
        public string response { get; set; }
        public string website { get; set; }
        public Nullable<int> schools_loaded { get; set; }
        public string compliance_sheet { get; set; }
        public Nullable<int> sort_order { get; set; }
        public Nullable<int> failed_downloads { get; set; }
        public Nullable<bool> onAWS { get; set; }
        public Nullable<int> RedactionStep { get; set; }
        public string transcript { get; set; }
        public Nullable<System.DateTime> transcript_analyzed { get; set; }
        public Nullable<bool> transcript_flagged { get; set; }
        public string formatted_transcript { get; set; }
        public string banned_list { get; set; }
        public string transcript_flagged_reason { get; set; }
        public Nullable<int> call_duration { get; set; }
        public Nullable<int> transcript_count { get; set; }
        public Nullable<int> api_version { get; set; }
        public Nullable<bool> call_length_truncated { get; set; }
        public Nullable<int> failed_upload { get; set; }
        public Nullable<bool> must_review { get; set; }
        public Nullable<int> sm_id { get; set; }
        public string sm_json { get; set; }
        public Nullable<int> sm_trans_count { get; set; }
        public Nullable<bool> sm_must_review { get; set; }
        public string data_file { get; set; }
        public Nullable<int> total_reviews { get; set; }
        public Nullable<bool> recreate_call { get; set; }
        public Nullable<System.DateTime> last_heartbeat { get; set; }
        public string heartbeat_who { get; set; }
        public Nullable<int> secondary_orig_id { get; set; }
        public Nullable<int> hold_time { get; set; }
        public Nullable<System.DateTime> loaded_date { get; set; }
        public string flagged_by { get; set; }
        public Nullable<System.DateTime> flagged_by_date { get; set; }
        public string secondary_orig_qa { get; set; }
        public Nullable<bool> needs_trim { get; set; }
        public Nullable<System.DateTime> xcc_review_date { get; set; }
        public Nullable<System.DateTime> last_queried { get; set; }
        public Nullable<int> id1 { get; set; }
        public string session_id1 { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
        public Nullable<int> review_ID { get; set; }
        public string Comments { get; set; }
        public string autofail { get; set; }
        public string reviewer { get; set; }
        public string appname1 { get; set; }
        public string dispute { get; set; }
        public Nullable<System.DateTime> dispute_date { get; set; }
        public string dispute_by { get; set; }
        public string agent_comment { get; set; }
        public Nullable<System.DateTime> agent_reviewed { get; set; }
        public string Supervisor_comment { get; set; }
        public Nullable<System.DateTime> Supervisor_reviewed { get; set; }
        public Nullable<decimal> total_score { get; set; }
        public Nullable<decimal> total_score_with_fails { get; set; }
        public Nullable<int> old_review_id { get; set; }
        public Nullable<double> call_length { get; set; }
        public Nullable<bool> has_cardinal { get; set; }
        public string fs_audio { get; set; }
        public Nullable<System.DateTime> week_ending_date { get; set; }
        public Nullable<int> num_missed { get; set; }
        public string missed_list { get; set; }
        public Nullable<System.DateTime> call_made_date { get; set; }
        public Nullable<double> agent_deviation { get; set; }
        public Nullable<double> calib_deviation { get; set; }
        public Nullable<double> total_score_test { get; set; }
        public string pass_fail { get; set; }
        public string formatted_comments { get; set; }
        public string formatted_missed { get; set; }
        public Nullable<System.DateTime> deviation_reviewed { get; set; }
        public string deviation_reviewed_by { get; set; }
        public Nullable<int> review_time { get; set; }
        public Nullable<bool> wasEdited { get; set; }
        public Nullable<decimal> calib_score { get; set; }
        public Nullable<decimal> edited_score { get; set; }
        public Nullable<int> isDispute { get; set; }
        public string processed_dispute { get; set; }
        public Nullable<bool> first_10 { get; set; }
        public Nullable<decimal> section_score { get; set; }
        public Nullable<bool> non_billable { get; set; }
        public Nullable<System.DateTime> qa_start { get; set; }
        public Nullable<System.DateTime> qa_last_action { get; set; }
        public Nullable<int> whisperID { get; set; }
        public Nullable<int> QAwhisper { get; set; }
        public Nullable<System.DateTime> whisper_forgiven { get; set; }
        public string whisper_forgiven_by { get; set; }
        public Nullable<double> display_score { get; set; }
        public Nullable<double> original_qa_score { get; set; }
        public Nullable<bool> client_visible { get; set; }
        public Nullable<bool> retrieved { get; set; }
        public string notification_status { get; set; }
        public Nullable<System.DateTime> date_fid_created { get; set; }
        public Nullable<System.DateTime> simple_rev_date { get; set; }
        public Nullable<int> num_missed_a { get; set; }
        public Nullable<int> num_missed_q { get; set; }
        public Nullable<int> num_missed_cal { get; set; }
    }
}
