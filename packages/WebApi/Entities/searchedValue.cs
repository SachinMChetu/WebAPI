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
    
    public partial class searchedValue
    {
        public int id { get; set; }
        public string who_searched { get; set; }
        public Nullable<System.DateTime> date_searched { get; set; }
        public string value_searched { get; set; }
    }
}