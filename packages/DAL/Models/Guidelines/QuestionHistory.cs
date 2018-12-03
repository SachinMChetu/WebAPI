using System;

namespace DAL.Models.Guidelines
{
    public class QuestionHistory
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string questionName { get; set; }
        public bool isNew { get; set; }
        public DateTime updatedDate { get; set; }
    }
}