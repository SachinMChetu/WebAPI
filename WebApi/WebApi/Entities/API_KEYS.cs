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
    
    public partial class API_KEYS
    {
        public int id { get; set; }
        public Nullable<System.Guid> api_key { get; set; }
        public string appname { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public string client_location { get; set; }
    }
}