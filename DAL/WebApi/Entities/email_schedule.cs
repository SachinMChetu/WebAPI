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
    
    public partial class email_schedule
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string chron_schedule { get; set; }
        public Nullable<System.DateTime> LastRun { get; set; }
        public Nullable<System.DateTime> NextRun { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<int> end_iteration { get; set; }
        public int send_count { get; set; }
        public Nullable<bool> active { get; set; }
    }
}