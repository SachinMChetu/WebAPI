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
    
    public partial class link_list
    {
        public int id { get; set; }
        public string link { get; set; }
        public string url { get; set; }
        public Nullable<bool> admin { get; set; }
        public Nullable<bool> qa { get; set; }
        public Nullable<bool> agent { get; set; }
        public Nullable<bool> supervisor { get; set; }
        public Nullable<bool> client { get; set; }
        public Nullable<bool> qa_lead { get; set; }
        public Nullable<bool> Trainee { get; set; }
        public Nullable<bool> Manager { get; set; }
        public Nullable<bool> Center_manager { get; set; }
        public Nullable<bool> calibrator { get; set; }
        public Nullable<bool> call_center { get; set; }
        public Nullable<int> link_order { get; set; }
        public Nullable<bool> partner { get; set; }
        public Nullable<int> recalibrator { get; set; }
        public Nullable<bool> client_calibrator { get; set; }
        public Nullable<bool> tango_tl { get; set; }
        public string link_class { get; set; }
    }
}
