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
    
    public partial class agent_hours2
    {
        public int id { get; set; }
        public Nullable<System.DateTime> we_date { get; set; }
        public string agent_name { get; set; }
        public Nullable<double> agent_hours { get; set; }
        public Nullable<double> calibration_score { get; set; }
        public Nullable<double> rate { get; set; }
        public string appname { get; set; }
        public Nullable<System.DateTime> worked_date { get; set; }
    }
}
