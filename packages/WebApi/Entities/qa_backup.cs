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
    
    public partial class qa_backup
    {
        public int id { get; set; }
        public Nullable<int> question_id { get; set; }
        public string answer_text { get; set; }
        public Nullable<double> answer_points { get; set; }
        public Nullable<bool> isAutoFail { get; set; }
        public Nullable<bool> autoselect { get; set; }
        public Nullable<bool> right_answer { get; set; }
        public string linked_answer { get; set; }
        public Nullable<int> old_answer_id { get; set; }
        public Nullable<int> old_question_id { get; set; }
    }
}
