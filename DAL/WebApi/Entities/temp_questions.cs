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
    
    public partial class temp_questions
    {
        public int id { get; set; }
        public Nullable<int> section { get; set; }
        public Nullable<int> category { get; set; }
        public Nullable<int> q_order { get; set; }
        public string question { get; set; }
        public string q_type { get; set; }
        public string q_short_name { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<int> heading { get; set; }
        public Nullable<int> Order { get; set; }
        public string QuestionText { get; set; }
        public Nullable<double> q_percent { get; set; }
        public string appname { get; set; }
        public Nullable<bool> auto_yes { get; set; }
        public Nullable<bool> auto_no { get; set; }
        public string agent_display { get; set; }
        public Nullable<int> default_answer { get; set; }
    }
}
