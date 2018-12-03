using System.Collections.Generic;

namespace DAL.Models
{
    public class NotificationInfo
    {
        public Role assignedTo { get; set; }
        public string notificationStatus { get; set; }
        public List<NotificationComment> notificationComments { get; set; }
        public List<SystemComment> systemComments { get; set; }
        public List<Role> reassignOptions { get; set; }
        public bool canClose { get; set; }
    }

    public class NotificationInfo1
    {
        public string agentName { get; set; }
        public string qaName { get; set; }
        public List<NotificationComment> notificationComments { get; set; }
        public List<SystemComment> systemComments { get; set; }
    }
}