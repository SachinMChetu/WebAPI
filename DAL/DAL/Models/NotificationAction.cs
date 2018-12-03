using DAL.Models;

namespace DAL.Models
{
    public class NotificationAction
    {
        public int callId { get; set; }
        public string text { get; set; }
        public Role assignToRole { get; set; }
        public string action { get; set; }
        // public int notificationId { get; set; }
    }
}