using System;

namespace DAL.Models
{
    public class NotificationComment
    {
        public DateTime? openDate { get; set; }
        public DateTime? closedDate { get; set; }
        public UserInformation openedBy { get; set; }
        public UserInformation closedBy { get; set; }
        public string text { get; set; }
        public int id { get; set; }
        public Role notificationRole { get; set; }

    }
}