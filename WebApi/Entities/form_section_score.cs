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
    
    public partial class form_section_score
    {
        public int id { get; set; }
        public Nullable<int> form_id { get; set; }
        public Nullable<int> section_id { get; set; }
        public Nullable<int> total_score { get; set; }
        public Nullable<int> total_points { get; set; }
        public Nullable<int> total_NA { get; set; }
    }
}