using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.NotificationModels
{
    public class NotificationResponseModel
    {
        public List<NotificationCallsModel> notificationCallsModel { get; set; }
        public int totalCalls { get; set; }
    }

    public class NotificationCallsModel
    {
        public NotificationCalls calls { get; set; }
        public List<NotificationsV2> notifications { get; set; }
        public int notificationCount { get; set; }
       
    }

    public class NotificationCalls
    {
        public string agent { get; set; }
        public string phone { get; set; }
        public ScorecardInfo scorecard { get; set; }
        public DateTime? reviewDate { get; set; }
        public string reviewer { get; set; }
        public int callId { get; set; }
        public string callType { get; set; }
        public string teamLead { get; set; }

    }
    public class NotificationsV2
    {
        public int notifId { get; set; }
        public int callId { get; set; }
        public DateTime? openDate { get; set; }
        public DateTime? closedDate { get; set; }
        public NotificationAssigned assignTo { get; set; }
        public string comment { get; set; }
        public string notificationStatus { get; set; }
        public bool? isNotificationOwner { get; set; }
        public bool? notificationCommentsPresent { get; set; }
        public bool? reviewCommentsPresent { get; set; }
    }
    public class NotificationAssigned
    {
        public User user { get; set; }
        public string role { get; set; }
    }
}
