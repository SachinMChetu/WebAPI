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
    
    public partial class MyReportField
    {
        public int id { get; set; }
        public Nullable<int> report_id { get; set; }
        public Nullable<int> field_id { get; set; }
        public Nullable<int> field_order { get; set; }
        public string field_operation { get; set; }
    }
}
