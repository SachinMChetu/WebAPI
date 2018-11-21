using DAL.Models.UserGroupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class UserGroup
    {
        public int userId { get; set; }
        public List<ClientUserGroupListV2> scorecards { get; set; }
    }
   
}
