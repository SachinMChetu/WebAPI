using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    class MoveUsersToGroupModel
    {
        public List<int> userIds { get; set; }
        public string groupName { get; set; }
    }
}
