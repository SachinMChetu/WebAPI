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
    
    public partial class cpc_deleted
    {
        public int id { get; set; }
        public string bad_value { get; set; }
        public Nullable<int> form_id { get; set; }
        public string reviewer { get; set; }
        public string review_type { get; set; }
        public string appname { get; set; }
        public Nullable<System.DateTime> date_started { get; set; }
        public Nullable<System.DateTime> date_completed { get; set; }
        public string who_processed { get; set; }
        public Nullable<System.DateTime> week_ending { get; set; }
        public Nullable<bool> skipped { get; set; }
        public string skip_reason { get; set; }
        public string skip_review_who { get; set; }
        public Nullable<System.DateTime> skip_review_when { get; set; }
        public string skip_review_response { get; set; }
        public string skip_review_comments { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public Nullable<System.DateTime> client_start { get; set; }
        public Nullable<System.DateTime> client_end { get; set; }
        public string client_processed { get; set; }
        public string assigned_to { get; set; }
        public Nullable<System.DateTime> dateadded { get; set; }
    }
}
