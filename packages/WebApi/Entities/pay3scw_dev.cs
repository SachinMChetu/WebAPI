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
    
    public partial class pay3scw_dev
    {
        public string reviewer { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public Nullable<decimal> @base { get; set; }
        public string calltime { get; set; }
        public string reviewtime { get; set; }
        public Nullable<double> efficiency { get; set; }
        public decimal calibration_score { get; set; }
        public int num_calibrations { get; set; }
        public string appname { get; set; }
        public Nullable<int> num_calls { get; set; }
        public Nullable<double> cal_percent { get; set; }
        public Nullable<decimal> eff_percent { get; set; }
        public Nullable<int> num_disputes { get; set; }
        public Nullable<int> scorecard { get; set; }
        public decimal deduct { get; set; }
        public string short_name { get; set; }
        public decimal app_difficulty { get; set; }
        public string sc_date { get; set; }
        public Nullable<int> websites { get; set; }
        public string email { get; set; }
    }
}
