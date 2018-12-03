using System;

namespace DAL.Models
{
    public class SystemComment
    {
        public UserInformation user { get; set; }
        public DateTime? commentDate { get; set; }
        public string text { get; set; }
        public int id { get; set; }
    }
}