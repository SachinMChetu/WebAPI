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
    
    public partial class dailyAgentMissed
    {
        public string agent { get; set; }
        public Nullable<int> QID { get; set; }
        public Nullable<System.DateTime> q_date { get; set; }
        public int id { get; set; }
        public Nullable<int> number_missed { get; set; }
        public Nullable<int> total_Qs { get; set; }
        public string agent_group { get; set; }
    }
}
