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
    
    public partial class call_centers
    {
        public int id { get; set; }
        public string center_name { get; set; }
        public string contact { get; set; }
        public string phone { get; set; }
        public Nullable<double> bill_rate { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public string center_manager { get; set; }
    }
}