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
    
    public partial class question_calcs
    {
        public int id { get; set; }
        public Nullable<int> qid { get; set; }
        public string rule_description { get; set; }
        public Nullable<bool> rule_active { get; set; }
        public Nullable<int> q_answer { get; set; }
        public Nullable<int> old_QID { get; set; }
        public Nullable<int> old_id { get; set; }
    }
}
