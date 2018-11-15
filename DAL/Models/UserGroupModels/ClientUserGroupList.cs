using System.Collections.Generic;

namespace DAL.Models.UserGroupModels
{
    public class ClientUserGroupList
    {
        //ClientUserGroupInfo
        public GroupInfo groupInfo { get;set; }       
        public List<ScorecardInfo> scorecardsInfo { get; set; }        
    }

    public class ClientUserGroupListV2
    {
        //ClientUserGroupInfo
        public List<GroupInfoV2> groups { get; set; }
        public ScorecardInfoV2 scorecardInfo{ get; set; }
    }
    public class ClientGroupScorecardUserList
    {
        //ClientUserGroupInfo
        public GroupInfo groupInfo { get; set; }
        public ScorecardInfo scorecardsInfo { get; set; }
        public ScorecardUserModel scorecardsUserInfo { get; set; }
    }
    public class ClientUserScorecardGroupList
    {
        public ScorecardInfo scporecard { get; set; }
        public GroupInfo groupInfos { get; set; }
    }
}