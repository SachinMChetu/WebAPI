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
    
    public partial class st_clients
    {
        public int id { get; set; }
        public string client_name { get; set; }
        public string agent_list { get; set; }
        public Nullable<int> monthly_cap { get; set; }
        public string agent_group { get; set; }
        public string Notes { get; set; }
        public Nullable<int> st_scorecard { get; set; }
        public string billing_id { get; set; }
        public string call_source { get; set; }
    }
}
