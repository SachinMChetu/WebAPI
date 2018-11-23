using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class ClientUserListModel
    {
        public Models.User user { get; set; }
        public string userRole { get; set; }
        public List<GroupInfo> userGroups { get; set; }
        public string userGroup { get; set; }
        public DateTime? lastActivity { get; set; }
        public App app { get; set; }
        public bool? active { get; set; }
        public string appname { get; set; }

    }
    public class ClientUserListModelRaw
    {
        public Models.User user { get; set; }
        public string userRole { get; set; }
        public string userGroup { get; set; }
        public DateTime? lastActivity { get; set; }
        public ScorecardInfo scorecard { get; set; }
        public string appName { get; set; }
    }

}
