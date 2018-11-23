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
    
    public partial class form_score_other
    {
        public int id { get; set; }
        public string session_id { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
        public Nullable<int> review_ID { get; set; }
        public string Comments { get; set; }
        public string autofail { get; set; }
        public string reviewer { get; set; }
        public string appname { get; set; }
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
    }
}