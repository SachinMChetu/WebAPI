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
    
    public partial class getListenQuestions_Result
    {
        public string q_short_name { get; set; }
        public string question { get; set; }
        public int linked_answer { get; set; }
        public int linked_comment { get; set; }
        public bool single_comment { get; set; }
        public bool comments_allowed { get; set; }
        public int id { get; set; }
    }
}
