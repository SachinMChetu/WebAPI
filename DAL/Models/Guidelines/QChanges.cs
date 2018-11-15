using System;

namespace DAL.Models.Guidelines
{
    public class QChanges
    { 
        public int id { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public string fromText { get; set; }
        public string toText { get; set; }
        public string fromAnswer { get; set; }
        public string toAnswer { get; set; }
    }
}