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
    
    public partial class outgoing_email
    {
        public string profile_name { get; set; }
        public string recipients { get; set; }
        public string copy_recipients { get; set; }
        public string blind_copy_recipients { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string body_format { get; set; }
        public string importance { get; set; }
        public string sensitivity { get; set; }
        public string file_attachments { get; set; }
        public string query { get; set; }
        public string execute_query_database { get; set; }
        public Nullable<bool> attach_query_result_as_file { get; set; }
        public string query_attachment_filename { get; set; }
        public Nullable<bool> query_result_header { get; set; }
        public Nullable<int> query_result_width { get; set; }
        public string query_result_separator { get; set; }
        public Nullable<bool> exclude_query_output { get; set; }
        public Nullable<bool> append_query_error { get; set; }
        public Nullable<bool> query_no_truncate { get; set; }
        public Nullable<bool> query_result_no_padding { get; set; }
        public Nullable<int> mailitem_id { get; set; }
        public string from_address { get; set; }
        public string reply_to { get; set; }
        public Nullable<System.DateTime> date_sent { get; set; }
        public int id { get; set; }
    }
}
