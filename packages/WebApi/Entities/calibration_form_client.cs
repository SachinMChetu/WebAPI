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
    
    public partial class calibration_form_client
    {
        public int id { get; set; }
        public Nullable<int> original_form { get; set; }
        public string reviewed_by { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
        public Nullable<double> total_score { get; set; }
        public Nullable<double> total_score_with_fails { get; set; }
        public string calibration_comment { get; set; }
        public Nullable<System.DateTime> review_started { get; set; }
        public Nullable<double> cali_form_score { get; set; }
        public Nullable<double> cali_dev { get; set; }
        public Nullable<double> original_score { get; set; }
        public Nullable<bool> selected { get; set; }
        public Nullable<int> cpc_ID { get; set; }
    }
}
