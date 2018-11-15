using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class ClientUserGroupInfo
    {
        public int id { get; set; }
        public int scorecardId { get; set; }
        public string groupName { get; set; }
        public DateTime dateAdded { get; set; }
    }
    public class ClientUserGroupUpdate
    {
        public int id { get; set; }
        public int scorecardId { get; set; }
        public string newGroupName { get; set; }
        public DateTime dateAdded { get; set; }
        public string oldGroupName { get; set; }
    }
}
