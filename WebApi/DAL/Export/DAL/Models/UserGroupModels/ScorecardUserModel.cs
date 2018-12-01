using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class ScorecardUserModel
    {
        public ScorecardInfo scorecard { get; set; }
        public List<User> user { get; set; }
    }
}
