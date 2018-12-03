using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{

    public class MultipleUserAddingToGroupModel
    {
        public int userId { get; set; }
        public List<ScorecardsInGroup> scorecardsInGroups { get; set; }
    }
    public class ScorecardsInGroup
    {
        public string groupName { get; set; }
        public bool updateOlderData { get; set; }
        public List<ScorecardInfo> scorecards { get; set; }
    }





    public class ScorecardsInGroupV2
    {
        public List<GroupInfoV2> groups { get; set; }
        public ScorecardInfoV2 scorecardInfo { get; set; }
    }
    public class MultipleUserAddingToGroupModelV2
    {
        public int userId { get; set; }
        public List<ScorecardsInGroupV2> scorecards { get; set; }
    }


}
