using DAL.Layers;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DAL.Models.UserGroupModels;
using System.Web.Security;

namespace WebApi.Controllers
{
    /// <summary>
    /// ClientUserController
    /// </summary>
    public class ClientUserController : ApiController
    {
        // GET: ClientUser
        /// <summary>
        /// Get all list of users for this client
        /// </summary>
        /// <returns></returns>
        [Route("clientuser/GetUserList")]
        [HttpPost]
        [ResponseType(typeof(List<ClientUserListModel>))]
        //[Authorize(Roles = "getMyModules")]
        public dynamic GetUserList([FromBody]ClientUserFilters userListFilters)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetUserList(userListFilters);
            return rez;
        }


        /// <summary>
        /// GetUserGroupList
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("clientuser/GetUserGroupList")]
        [HttpPost]
        [ResponseType(typeof(List<ClientUserGroupList>))]
        public dynamic GetUserGroupList(string username = null)

        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetGroupListWithScorecard(username);
            return rez;
        }

        /// <summary>
        /// GetUserGroupList
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("clientuser/GetUserOwnedGroup")]
        [HttpPost]
        [ResponseType(typeof(List<ClientUserGroupList>))]
        public dynamic GetUserOwnedGroup([FromBody]int userId)

        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetUserOwnedGroup(userId);
            return rez;
        }


        /// <summary>
        /// GetUserGroupList
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Route("clientuser/GetScorecardGroupList")]
        [HttpPost]
        [ResponseType(typeof(List<ClientUserGroupList>))]
        public dynamic GetScorecardGroupList(string group = null)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetGroupListWithScorecard(group);
            return rez;
        }


        /// <summary>
        /// DeleteUserFromGroup
        /// </summary>
        /// <returns></returns>
        [Route("clientuser/DeleteUserFromGroup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteUserFromGroup([FromBody] UserGroup user)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.DeleteUserFromGroup(user);
            return rez;
        }
        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <returns></returns>
        [Route("clientuser/GetUserInfo")]
        [HttpPost]
        [ResponseType(typeof(LoginUserInfo))]
        //[Authorize(Roles = "getMyModules")]
        public dynamic GetUserInfo([FromBody] int userId)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetUserInfo(userId);
            return rez;
        }
        /// <summary>
        /// SaveUserInfo(edit user)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("clientuser/SaveUserInfo")]
        [HttpPost]
        [ResponseType(typeof(LoginUserInfo))]
        //[Authorize(Roles = "getMyModules")]
        public dynamic SaveUserInfo([FromBody] EditUserModel user)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.SaveUserInfo(user);
            return rez;
        }
        /// <summary>
        /// edit user
        /// </summary>
        /// <returns></returns>
        [Route("clientuser/GetUsersByScorecard")]
        [HttpPost]
        [ResponseType(typeof(string))]
        //[Authorize(Roles = "getMyModules")]
        public dynamic GetUsersByScorecard([FromBody] int scorecardId)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetUsersByScorecard(scorecardId);
            return rez;
        }

        /// <summary>
        /// Adding user to selected groups with selected scorecards
        /// </summary>
        /// <param name="multipleUser"></param>
        /// <returns></returns>
        [Route("clientuser/AddUserToMultipleGroup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddUserToMultipleGroup([FromBody]MultipleUserAddingToGroupModelV2 multipleUser)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.AddUserToMultipleGroup(multipleUser);
            return rez;
        }

        /// <summary>
        /// Move User To Other Group
        /// </summary>
        /// <param name="moveUser"></param>
        /// <returns></returns>
        [Route("clientuser/MoveUserToOtherGroup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic MoveUserToOtherGroup(MoveUserToOtherGroupModel moveUser)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.MoveUserToOtherGroup(moveUser);
            return rez;
        }
        /// <summary>
        /// Change User password
        /// </summary>
        /// <param name="editPassword"></param>
        /// <returns></returns>
        [Route("clientuser/changeUserPassword")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic ChangeUserPassword([FromBody]EditPasswordModel editPassword)
        {
            try
            {
                if ( editPassword.password!="" || editPassword.user.userName == "")
                {
                    MembershipUser user = Membership.GetUser(editPassword.user.userName);
                    var op = user.ResetPassword();
                    user.ChangePassword(op, editPassword.password);
                    return Ok();
                }
                throw new Exception("Password didn't math or user not found");
            }
            catch(Exception ex){
                return ex;
            }           
        }
        /// <summary>
        /// api to add new user to system
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [Route("clientuser/AddNewUser")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddNewUser(UserModel userInfo)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.AddNewUser(userInfo);
            return rez;
        }
        /// <summary>
        /// AddNewClientUserGroup
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        [Route("clientuser/AddNewClientUserGroup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddNewClientUserGroup([FromBody]ClientUserGroupInfo groupInfo)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.AddNewClientUserGroup(groupInfo);
            return rez;
        }

        /// <summary>
        /// GetScorecardsWithGroups
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        [Route("clientuser/GetScorecardsWithGroups")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetScorecardsWithGroups([FromBody]int? scorecardId)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetScorecardsWithGroups(scorecardId);
            return rez;
        }
        /// <summary>
        /// Get groups by scorecard id
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        [Route("clientuser/GetScorecardGroups")]
        [HttpPost]
        [ResponseType(typeof(List<GroupInfo>))]
        public dynamic GetScorecardGroups([FromBody]int scorecardId)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.GetGroupListFull(scorecardId);
            return rez;
        }

        /// <summary>
        /// UpdateClientUserGroup
        /// </summary>
        /// <param name="clientUserGroupInfo"></param>
        /// <returns></returns>
        [Route("clientuser/UpdateClientUserGroup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateClientUserGroup([FromBody]ClientUserGroupUpdate clientUserGroupInfo)
        {
            ClientUserLayer clientUserLayer = new ClientUserLayer();
            var rez = clientUserLayer.UpdateClientUserGroup(clientUserGroupInfo);
            return rez;
        }



        /// <summary>
        /// GetAppListWithScorecards
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("clientuser/GetAppListWithScorecards")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic GetAppListWithScorecards([FromBody]Search search)
        {
            ClientUserLayer clientUser = new ClientUserLayer();
            var rez = clientUser.GetAppScorecardList(search);
            return rez;
        }


        /// <summary>
        /// GetGroupByscorecardIds
        /// </summary>
        /// <param name="scorecardIds"></param>
        /// <returns></returns>
        [Route("clientuser/GetGroupByscorecardIds")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic GetGroupByscorecardIds([FromBody]List<int> scorecardIds)
        {
            ClientUserLayer clientUser = new ClientUserLayer();
            var rez = clientUser.GetGroupByscorecardIds(scorecardIds);
            return rez;
        }

        /// <summary>
        /// ChangeUserActiveStatus
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("clientuser/ChangeUserActiveStatus")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic ChangeUserActiveStatus([FromBody]UserInfoActiveModel user)
        {
            ClientUserLayer clientUser = new ClientUserLayer();
            var rez = clientUser.ChangeUserActiveStatus(user);
            return rez;
        }

        /// <summary>
        /// GetScorecardsWithGroups
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Route("clientuser/GetScorecardWithGroupsList")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic GetScorecardsWithGroups([FromBody]string group)
        {
            ClientUserLayer clientUser = new ClientUserLayer();
            var rez = clientUser.GetScorecardsWithGroups(group);
            return rez;
        }


        /// <summary>
        /// GetUserOwnedGroupNew
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("clientuser/GetUserOwnedGroupNew")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic GetUserOwnedGroupNew([FromBody]int userId)
        {
            ClientUserLayer clientUser = new ClientUserLayer();
            var rez = clientUser.GetUserOwnedGroupNew(userId);
            return rez;
        }

    }
}