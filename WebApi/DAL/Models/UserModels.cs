using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class UserObject
    {

        public string UserRole { get; set; }
        public List<LinkList> UserLinks { get; set; }
        public string UserTitle { get; set; }
        public string UserName { get; set; }
        public string SpeedInc { get; set; }
        public bool StartImmediately { get; set; }
        public List<UserModule> modules { get; set; }
        public List<MyScorecards> scorecards { get; set; }
        public List<Apps> apps { get; set; }
        public string PreviousUser { get; set; }
        public string userClientLogo { get; set; }
        public List<UserModule> modulesRequired { get; set; }
        public bool useNewDashboard { get; set; }
    }

    public class LinkList
    {
        public string LinkText { get; set; }
        public string LinkURL { get; set; }
        public string LinkSpan { get; set; }
    }
    public class UserModule
    {
        public string module_title { get; set; }
        public string module_width { get; set; }
        public string module_function { get; set; }
        public bool module_active { get; set; }

        public int module_order { get; set; }
    }

    public class MyScorecards
    {
        public int scorecard { get; set; }
        public string scorcard_name { get; set; }
        public string scorecard_role { get; set; }
        public string scorecard_appname { get; set; }
        public string accountManager { get; set; }
        public bool isNew { get; set; }
        public int appId { get; set; }
    }

    public class Apps
    {
        public string  appname { get; set; }
        public List<MyScorecards> scorecards { get; set; }
    }
}
