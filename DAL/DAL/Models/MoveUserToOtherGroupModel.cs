using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MoveUserToOtherGroupModel
    {
        public int oldScorecardId { get; set; }
        public int newScorecardId { get; set; }
        public string oldGroupName { get; set; }
        public string newGroupName { get; set; }
        public int userId { get; set; }
    }
}
