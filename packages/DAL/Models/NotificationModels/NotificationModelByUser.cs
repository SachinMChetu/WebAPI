using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.NotificationModels
{
    public class NotificationResponseByUsers
    {
        List<NotificationModelByUser> scorecards { get; set; }
        List<NotificationsByUserRole> roles { get; set; }
        List<NotificationsByUserName> users { get; set; }
    }


  

    public class NotificationModelByUser
    {
        public NotificationScorecardInfo scorecard { get; set; }
        public List<NotificationsByUserRole> roles { get; set; }
    }


    public class NotificationsByUserRole
    {
        public NotificationsByUserRoleInfo role { get; set; }
        public List<NotificationsByUserName> users { get; set; }
    }


    public class NotificationsByUserName
    {
        public ScorecardInfo scorecard { get; set; }
        public User user { get; set; }
        public string role { get; set; }
        public string callRole { get; set; }
        //public int? notificationsCount { get; set; }
        public int? totalClosed { get; set; }
        public int? ccClosed { get; set; }
        public int? clientClosed { get; set; }
        public int? totalOpenPending { get; set; }
        public int? ccPending { get; set; }
        public int? clientPending { get; set; }
        public double? avgDaysOpen { get; set; }
    }

    public class NotificationsByUserRoleInfo
    {
        public ScorecardInfo scorecard { get; set; }
        public string role { get; set; }
        public int? callsCount { get; set; }
        public int? totalClosed { get; set; }
        public int? ccClosed { get; set; }
        public int? clientClosed { get; set; }
        public int? totalOpenPending { get; set; }
        public int? ccPending { get; set; }
        public int? clientPending { get; set; }
        public double? avgDaysOpen { get; set; }
        
    }
    public class NotificationScorecardInfo
    {
        public ScorecardInfo scorecard { get; set; }
        public int? callsCount { get; set; }
        public int? totalClosed { get; set; }
        public int? ccClosed { get; set; }
        public int? clientClosed { get; set; }
        public int? totalOpenPending { get; set; }
        public int? ccPending { get; set; }
        public int? clientPending { get; set; }
        public double? avgDaysOpen { get; set; }
    }


}
