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
    
    public partial class available_columns
    {
        public int id { get; set; }
        public string column_name { get; set; }
        public string column_sql { get; set; }
        public string column_description { get; set; }
        public Nullable<bool> column_required { get; set; }
        public Nullable<bool> column_default { get; set; }
        public Nullable<bool> column_internal { get; set; }
        public Nullable<bool> sortable { get; set; }
        public string sort_key { get; set; }
    }
}