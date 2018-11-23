using System.Text;

using DAL.Models;
using DAL.Models.UserGroupModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DAL.Extensions;
using System.Web.Security;
using DAL.GenericRepository;
using Dapper;

namespace DAL.Layers
{
    public class ClientUserLayer
    {


        /// <summary>
        /// GetUserList
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserList(ClientUserFilters userListFilters)
        {

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "papadmin";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = "[GetClientUserList]",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@pagenum", userListFilters.pagination.pagenum);
                sqlComm.Parameters.AddWithValue("@pagerows", userListFilters.pagination.pagerows);
                sqlComm.Parameters.AddWithValue("@searchtext", userListFilters.searchText);
                sqlComm.Parameters.AddWithValue("@hideInactive", userListFilters.hideInactive);

                if (userListFilters.sorting?.sortBy != null && userListFilters.sorting.sortOrder != null && userListFilters.sorting.sortBy != "" && userListFilters.sorting.sortOrder != "")
                {

                    sqlComm.Parameters.AddWithValue("@OrderByColumn", userListFilters.sorting.sortBy);
                    sqlComm.Parameters.AddWithValue("@sortOrder", (userListFilters.sorting.sortOrder != "desc"));
                }

                if (userListFilters.filters != null)
                { 
                    if (userListFilters.filters.filterGroups != null && (userListFilters.filters.filterGroups.Count > 0))
                    {

                        var preparedLst = new StringBuilder();
                        foreach (var value in userListFilters.filters.filterGroups)
                        {
                            preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                        }

                        sqlComm.Parameters.AddWithValue("@groupNames", preparedLst.ToString().Trim(','));
                    }

                    if (userListFilters.filters.filterScorecards != null && (userListFilters.filters.filterScorecards.Count > 0))
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in userListFilters.filters.filterScorecards)
                        {
                            preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                        }

                        sqlComm.Parameters.AddWithValue("@scorecardIDs", preparedLst.ToString().Trim(','));
                    }
                }

                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                List<ClientUserListModel> clientUserListModels = new List<ClientUserListModel>();
                List<ScorecardInfo> scInf = new List<ScorecardInfo>();
                try
                {
                    while (reader.Read())
                    {
                        clientUserListModels.Add(new ClientUserListModel()
                        {
                            user = new Models.User()
                            {
                                userName = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetValue(reader.GetOrdinal("username")).ToString(),
                                userId = reader.IsDBNull(reader.GetOrdinal("userId")) ? null : reader.GetFieldValue<int?>(reader.GetOrdinal("userId")),
                            },
                            lastActivity = reader.IsDBNull(reader.GetOrdinal("lastActiveDate")) ? null : reader.GetFieldValue<DateTime?>(reader.GetOrdinal("lastActiveDate")),
                            userRole = reader.IsDBNull(reader.GetOrdinal("user_role")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("user_role")),
                            active = reader.IsDBNull(reader.GetOrdinal("active")) ? false : (int.Parse(reader.GetValue(reader.GetOrdinal("active")).ToString()) == 1),
                            appname = reader.IsDBNull(reader.GetOrdinal("appname")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("appname"))

                        });
                    }
                    int count = 0;
                    if (reader.NextResult())
                        while (reader.Read())
                        {
                            count = reader.GetFieldValue<int>(reader.GetOrdinal("count"));
                        }

                    if (reader.NextResult())
                        while (reader.Read())
                        {
                            int userId = reader.GetFieldValue<int>(reader.GetOrdinal("userId"));

                            foreach (var model in clientUserListModels)
                            {
                                if (model.user.userId == userId)
                                {
                                    if (model.app == null)
                                    {
                                        model.app = new Models.UserGroupModels.App();
                                        model.app.app = reader.GetFieldValue<string>(reader.GetOrdinal("appname"));
                                        model.app.scorecards = new List<ScorecardInfo>();
                                        model.app.scorecards.Add(new ScorecardInfo()
                                        {
                                            scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecardId")),
                                            scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                                        });
                                    }
                                    else
                                    {
                                        var sc = new ScorecardInfo()
                                        {
                                            scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecardId")),
                                            scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                                        };
                                        if (model.app.scorecards.Any(a => a.scorecardId != sc.scorecardId))
                                        {
                                            model.app.scorecards.Add(sc);
                                        }
                                    }
                                    if (model.userGroups == null)
                                    {
                                        model.userGroups = new List<GroupInfo>();
                                        model.userGroups.Add(new GroupInfo()
                                        {
                                            id = reader.GetFieldValue<int>(reader.GetOrdinal("groupId")),
                                            name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName"))
                                        });

                                    }
                                    else
                                    {
                                        var gr = new GroupInfo()
                                        {
                                            id = reader.GetFieldValue<int>(reader.GetOrdinal("groupId")),
                                            name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName"))
                                        };
                                        if (model.userGroups.Any(a => a.name != gr.name))
                                        {
                                            model.userGroups.Add(new GroupInfo()
                                            {
                                                id = reader.GetFieldValue<int>(reader.GetOrdinal("groupId")),
                                                name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName"))
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    foreach (var item in clientUserListModels)
                    {
                        if (item.app != null && item.app.scorecards != null)
                        {
                            var rez = item.app.scorecards == null ? new List<ScorecardInfo>() : item.app.scorecards.GroupBy(x => x.scorecardId).Select(q => q.FirstOrDefault()).ToList();
                            item.app.scorecards = rez;
                        }
                      
                    }
                    foreach (var item in clientUserListModels)
                    {
                        if (item.userGroups != null)
                        {
                            var rez = item.userGroups == null ? new List<GroupInfo>() : item.userGroups.GroupBy(x => x.name).Select(q => q.FirstOrDefault()).Distinct().ToList();
                            item.userGroups = rez;
                        }
                    }
                   
                    return new { total = count, clientUserListModels };
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public object GetScorecardGroups(int? scorecardId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserOwnedGroup(int userId)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "winnie";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                string sql = @"SELECT distinct " +
                                "groupName  " +
                                ",scorecard_groups.scorecard  " +
                                ",short_name " +
                                ",user_groups.id ugId" +
                                ",user_groups.userID" +
                                " FROM scorecard_groups " +
                                " left JOIN user_groups ON user_groups.userGroup = scorecard_groups.id " +
                                " LEFT JOIN scorecards ON scorecards.id = scorecard_groups.scorecard " +
                                " WHERE " +
                                " scorecard_groups.scorecard " +
                                " IN ( select sc.id from userapps ua " +
                                " left join userextrainfo ux on ua.userName = ux.userName " +
                                " left join scorecards sc on sc.id = ua.user_scorecard " +
                                " where sc.appname in  (select appname from userapps where username = @userName ))" +
                                " and user_groups.userID =@userId and scorecards.active = 1;";
                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);

                sqlComm.Parameters.AddWithValue("@userId", userId);

                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                List<ClientUserGroupList> clientUserGroupList = new List<ClientUserGroupList>();
                while (reader.Read())
                {
                    var scorecardUserModels = new ScorecardUserModel();

                    var scorecardsInfo = new ScorecardInfo()
                    {
                        scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecard")),
                        scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                    };
                    var sclist = new List<ScorecardInfo>();
                    sclist.Add(scorecardsInfo);
                    GroupInfo groupInfo = new GroupInfo()
                    {
                        id = reader.GetFieldValue<int>(reader.GetOrdinal("ugId")),
                        name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName"))
                    };

                    //scorecardUserModels = new ScorecardUserModel() { scorecard = scorecardsInfo, user = userList };

                    clientUserGroupList.Add(new ClientUserGroupList
                    {
                        groupInfo = groupInfo,
                        //scorecardsUserInfo = scorecardUserModels,
                        scorecardsInfo = sclist
                    });

                }
                var data = (from inf in clientUserGroupList
                            group inf by new { inf.groupInfo } into gInf
                            select new
                            {
                                groupInfo = gInf.Key.groupInfo,
                                scorecardsInfo = (from i in gInf select i.scorecardsInfo).First().Distinct()
                            });
                return data;
            }
        }


        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.UserGroupModels.UserInfo GetUserInfo(int userId)
        {

            string sql = @"select Userextrainfo.id as userid, UserName,first_name,Last_name,email_address,default_page,user_role,manager" +
                            " ,cast(non_Edit as bit) as non_Edit  ,cast(non_calib as bit) as non_calib  ,cast(no_dash as bit) as no_dash ,cast(force_review as bit) as force_review," +
                            "excludeCalls,lastLoginDate,null as who_added,null as dateadded,active from  Userextrainfo where id=@userId  ;"
                            + "select distinct  userextrainfo.id,  userextrainfo.username from userextrainfo join userapps on userapps.username = " +
                            " userextrainfo.username where user_scorecard in (select user_scorecard from userapps where " +
                            "username = (select userName from userextrainfo where username=@userName  )) and user_role = 'Manager'" +
                            " union all" +
                            " select  -1,manager from userextrainfo where username = @userName and manager is not null and manager != '' " +
                            " ";
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                    
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@userId", userId);
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                Models.UserGroupModels.UserInfo userInfo = new UserInfo();
                userInfo.user = new EditUserModel();
                bool? nullBoolean = false;
                while (reader.Read())
                {
                    try
                    {
                        userInfo.user = new EditUserModel()
                        {
                            userId = reader.GetFieldValue<int>(reader.GetOrdinal("userid")),
                            userName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("UserName")),
                            firstName = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("first_name")),
                            lastName = reader.IsDBNull(reader.GetOrdinal("Last_name")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("Last_name")),
                            email = reader.IsDBNull(reader.GetOrdinal("email_address")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("email_address")),
                            defaultPage = reader.IsDBNull(reader.GetOrdinal("default_page")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("default_page")),
                            userRole = reader.GetFieldValue<string>(reader.GetOrdinal("user_role")),
                            userManager = reader.IsDBNull(reader.GetOrdinal("manager")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("manager")),
                            nonEdit = reader.IsDBNull(reader.GetOrdinal("non_Edit")) ? nullBoolean : (bool)reader["non_Edit"],
                            //                            --reader.GetFieldValue<bool>(reader.GetOrdinal("nonEdit")),
                            nonCalibrating = reader.IsDBNull(reader.GetOrdinal("non_calib")) ? nullBoolean : (bool)reader["non_calib"],
                            nondashboardAccess = reader.IsDBNull(reader.GetOrdinal("no_dash")) ? nullBoolean : (bool)reader["no_dash"],
                            updateOlderData = false,
                            forceRewiew = reader.IsDBNull(reader.GetOrdinal("force_review")) ? nullBoolean : (bool)reader["force_review"],
                            excludeCalls = reader.IsDBNull(reader.GetOrdinal("excludeCalls")) ? nullBoolean : (bool)reader["excludeCalls"],
                            lastLogin = reader.IsDBNull(reader.GetOrdinal("lastLoginDate")) ? null : reader.GetFieldValue<DateTime?>(reader.GetOrdinal("lastLoginDate")),
                            whoAdded = reader.IsDBNull(reader.GetOrdinal("who_added")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("who_added")),
                            dateAdded = reader.IsDBNull(reader.GetOrdinal("dateadded")) ? null : reader.GetFieldValue<DateTime?>(reader.GetOrdinal("dateadded")),
                            password = "",
                            active = reader.IsDBNull(reader.GetOrdinal("active")) ? false : (reader.GetFieldValue<int>(reader.GetOrdinal("active")) == 1),
                            //scorecardId = reader.IsDBNull(reader.GetOrdinal("active")) ? -1 : reader.GetFieldValue<int>(reader.GetOrdinal("active"))
                        };
                    }
                    catch (Exception ex) { }
                }
                if (reader.NextResult())
                {
                    userInfo.managers = new List<User>();
                    while (reader.Read())
                    {
                        userInfo.managers.Add(new User()
                        {
                            userId = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                            userName = reader.GetFieldValue<string>(reader.GetOrdinal("UserName"))
                        });

                    }
                }
                sqlCon.Close();

                var comm = new SqlCommand
                {
                    CommandText = @"select top 1 user_role from UserExtraInfo where username = @username",
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                comm.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                var userRole = "";
                var reader2 = comm.ExecuteReader();
                while (reader2.Read())
                {
                    userRole = reader2.GetValue(reader2.GetOrdinal("user_role")).ToString();
                }
                switch (userRole)
                {
                    case "Client":
                        userInfo.roles = new List<string>
                        {
                            "Agent","Supervisor","Manager","Client"
                        };
                        break;
                    case "Manager":
                        userInfo.roles = new List<string>
                        {
                            "Agent","Supervisor"
                        };
                        break;
                    case "Supervisor":
                        userInfo.roles = new List<string>
                        {
                            "Agent"
                        };
                        break;
                    case "Admin":
                        userInfo.roles = new List<string>
                        {
                             "Agent","Supervisor","Manager","Client"
                        };
                        break;
                    default:
                        userInfo.roles = new List<string>
                        {
                            "Agent"
                        };
                        break;
                }
                //try
                //{
                //    var mapper = DapperHelper.GetList<List<int>>("select scorecard from user_groups where userID =" + userId);
                //    //userInfo.user.scorecardId = mapper;

                //}
                //catch(Exception ex)
                //{
                //    throw ex;
                //}
                return userInfo;
            }

        }

        /// <summary>
        /// AddNewUser
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public dynamic AddNewUser(UserModel userInfo)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                int userID;
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                try
                {
                    //var rolesArray = Roles.GetAllRoles();
                    var user = Membership.CreateUser(userInfo.userName, userInfo.password);
                    
                    // Roles.AddUserToRole(userInfo.userName, userInfo.userRole);
                    //var role = Roles.GetRolesForUser(userInfo.userName);

                    var sql = @"insert into UserExtraInfo (username,first_name,last_name,email_address,default_page,user_role,manager,non_edit,non_calib,no_dash,force_review,excludeCalls)" +
  "values(@userName,@firstName,@lastName,@email,@defaultPage,@userRole,@manager,@nonEdit,@nonCalib,@noDash,@forceReview,@excludeCalls)";

                    var sqlComm = new SqlCommand
                    {
                        CommandText = sql,
                        CommandType = CommandType.Text,
                        CommandTimeout = int.MaxValue,
                        Connection = sqlCon
                    };
                    sqlComm.Parameters.AddWithValue("@userName", userInfo.userName);
                    sqlComm.Parameters.AddWithValue("@firstName", userInfo.firstName);
                    sqlComm.Parameters.AddWithValue("@lastName", userInfo.lastName);
                    sqlComm.Parameters.AddWithValue("@email", userInfo.email);
                    sqlComm.Parameters.AddWithValue("@defaultPage", userInfo.defaultPage);
                    sqlComm.Parameters.AddWithValue("@userRole", userInfo.userRole);
                    sqlComm.Parameters.AddWithValue("@manager", userInfo.userManager);
                    sqlComm.Parameters.AddWithValue("@nonEdit", userInfo.nonEdit);
                    sqlComm.Parameters.AddWithValue("@nonCalib", userInfo.nonCalibrating);
                    sqlComm.Parameters.AddWithValue("@noDash", userInfo.nondashboardAccess);
                    sqlComm.Parameters.AddWithValue("@forceReview", userInfo.forceRewiew);
                    sqlComm.Parameters.AddWithValue("@excludeCalls", userInfo.excludeCalls);
                    //sqlComm.Parameters.AddWithValue("@lastloginDate", userInfo.lastLogin);
                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                    
                }
                catch (Exception ex) { throw ex; }

                try
                {
                   
                        string sql2 = @"insert into UserApps(username,appname,dateadded,who_added,user_scorecard,scorecard_role,speed_increment,userid)values" +
                                 "(@userName2,@appName2,GetDate(),@whoAdded,@scorecard,@userRole,20,(select top 1 id from userextrainfo where username = @userName2))";
                        var sqlComm = new SqlCommand
                        {
                            CommandText = sql2,
                            CommandType = CommandType.Text,
                            CommandTimeout = int.MaxValue,
                            Connection = sqlCon
                        };
                        sqlComm.Parameters.AddWithValue("@userName2", userInfo.userName);
                        sqlComm.Parameters.AddWithValue("@appName2", userInfo.appName);
                        sqlComm.Parameters.AddWithValue("@whoAdded", userName);
                        sqlComm.Parameters.AddWithValue("@scorecard", userInfo.scorecardId);
                        sqlComm.Parameters.AddWithValue("@userRole", userInfo.userRole);

                        sqlCon.Open();
                        sqlComm.ExecuteNonQuery();
                        sqlCon.Close();
                    

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    string sqlUser = @"select top 1 id from UserExtraInfo where username ='"+userInfo.userName+"'";
                    userID = DapperHelper.GetSingle<int>(sqlUser);
                    sqlCon.Close();
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                try
                {
                    string sql3 = @"insert into user_groups (userID,userGroup,scorecard,dateadded)values(@userId,@userGroup,@sc,dbo.GetMTDate());";
                    var sqlComm = new SqlCommand
                    {
                        CommandText = sql3,
                        CommandType = CommandType.Text,
                        CommandTimeout = int.MaxValue,
                        Connection = sqlCon
                    };
                    sqlComm.Parameters.AddWithValue("@userId", userID);
                    sqlComm.Parameters.AddWithValue("@userGroup", userInfo.groupId);
                    sqlComm.Parameters.AddWithValue("@sc", userInfo.scorecardId);


                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlCon.Close();


                }
                catch (Exception ex)
                {
                    throw ex;
                }


                return "Added successfully!";
            }
        }

        /// <summary>
        /// GetUserGroupList
        /// </summary>
        /// <returns></returns>
        public dynamic GetGroupListWithScorecard(string username = null)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "NataliaAdmin";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                string sql = @"SELECT distinct scorecard_groups.id " +
                            ",groupName " +
                            ",scorecard_groups.scorecard " +
                            ",short_name " +
                            " FROM scorecard_groups " +
                            " left JOIN user_groups ON user_groups.userGroup = scorecard_groups.id " +
                            " LEFT JOIN scorecards ON scorecards.id = scorecard_groups.scorecard " +
                            " WHERE " +
                            " scorecard_groups.scorecard " +
                            " IN ( select sc.id from userapps ua " +
                            " left join userextrainfo ux on ua.userName = ux.userName " +
                            " left join scorecards sc on sc.id = ua.user_scorecard " +
                            " where sc.appname in  (select appname from userapps where username = @userName ))" +
                            " and (@groupName is null or @groupName= groupName ) and scorecards.active = 1  order by short_name;";
                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                if (username != null)
                {
                    sqlComm.Parameters.AddWithValue("@groupName", username);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@groupName", DBNull.Value);
                }
                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                List<ClientGroupScorecardUserList> clientUserGroupList = new List<ClientGroupScorecardUserList>();
                while (reader.Read())
                {
                    var scorecardUserModels = new ScorecardUserModel();

                    var scorecardsInfo = new ScorecardInfo()
                    {
                        scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecard")),
                        scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                    };
                    var sclist = new List<ScorecardInfo>
                    {
                        scorecardsInfo
                    };


                    //scorecardUserModels = new ScorecardUserModel() { scorecard = scorecardsInfo, user = userList };

                    clientUserGroupList.Add(new ClientGroupScorecardUserList
                    {
                        groupInfo = new GroupInfo()
                        {
                            id = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                            name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName")),
                        },
                        //scorecardsUserInfo = scorecardUserModels,
                        scorecardsInfo = scorecardsInfo
                    });

                }
                var data = (from inf in clientUserGroupList
                            group inf by new { inf.groupInfo.name } into gInf
                            select new
                            {
                                groupName = gInf.Key.name,
                                scorecardsInfo = (from i in gInf select i.scorecardsInfo).Distinct()
                            });
                return data;
            }
        }



        /// <summary>
        /// DeleteUserFromGroup
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic DeleteUserFromGroup(UserGroup user)
        {
            StringBuilder sql = new StringBuilder();


            for (var i =0; i<user.scorecards.Count; i++)
            {
                sql = new StringBuilder(@" delete from user_groups where userGroup in (");
                for (int j = 0; j < user.scorecards[i].groups.Count; j++)
                {
                    sql.Append("@groupId" + i + j * 2+",");
                }
                sql.Remove(sql.Length - 1, 1);
                sql.Append(") and scorecard = @scorecard" + i*2+ " and userID = @userId" + i * 2 + ";");

            }
           
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                try
                {
                    try
                    {
                        var sqlComm = new SqlCommand
                        {
                            CommandText = sql.ToString(),
                            CommandType = CommandType.Text,
                            CommandTimeout = int.MaxValue,
                            Connection = sqlCon
                        };

                        for (var i = 0; i < user.scorecards.Count; i++)
                        {
                            for (int j = 0; j < user.scorecards[i].groups.Count; j++)
                            {
                                sqlComm.Parameters.AddWithValue("@groupId" + i + j * 2, user.scorecards[i].groups[j].id);
                            }
                            sqlComm.Parameters.AddWithValue("@scorecard" + i * 2, user.scorecards[i].scorecardInfo.id);
                            sqlComm.Parameters.AddWithValue("@userId" + i * 2, user.userId);
                        }

                        sqlCon.Open();
                        sqlComm.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }

                  


                    try
                    {
                        
                        var userSQL = "select top 1 username as userName,id as userId from UserExtraInfo where id =" + user.userId;
                        var SqlUser = DapperHelper.GetSingle<User>(userSQL);
                        foreach (var item in user.scorecards)
                        {
                                string command = @"if(not exists(select * from user_groups where userID = @userid)) delete from UserApps where (userid = @userid or username = @username) and user_scorecard = @scorecard";
                                var sqlComm2 = new SqlCommand
                                {
                                    CommandText = command.ToString(),
                                    CommandType = CommandType.Text,
                                    CommandTimeout = int.MaxValue,
                                    Connection = sqlCon
                                };
                                sqlComm2.Parameters.AddWithValue("@userid", user.userId);
                                sqlComm2.Parameters.AddWithValue("@username", SqlUser.userName);
                                sqlComm2.Parameters.AddWithValue("@scorecard", item.scorecardInfo.id);
                                sqlCon.Open();
                                sqlComm2.ExecuteNonQuery();
                                sqlCon.Close();
                            
                        }
                       
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    
                    return GetGroupListWithScorecard();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public dynamic AddNewClientUserGroup(ClientUserGroupInfo groupInfo)
        {
            string sql = @"insert into scorecard_groups(scorecard,groupName,dateadded)values(@sc,@group,GetDate());";

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                try
                {
                    sqlComm.Parameters.AddWithValue("@sc", groupInfo.scorecardId);
                    sqlComm.Parameters.AddWithValue("@group", groupInfo.groupName);
                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    //var reader = sqlComm.ExecuteReader();
                    return GetGroupListWithScorecard();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }


        /// <summary>
        /// UpdateClientUserGroup
        /// </summary>
        /// <param name="clientUserGroupInfo"></param>
        /// <returns></returns>
        public dynamic UpdateClientUserGroup(ClientUserGroupUpdate clientUserGroupInfo)
        {
            string sql = @"update scorecard_groups set groupName = @NewGroup where groupName = @OldGroup;"; //and scorecard = @scorecard;";

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                try
                {
                    //sqlComm.Parameters.AddWithValue("@scorecard", clientUserGroupInfo.scorecardId);
                    sqlComm.Parameters.AddWithValue("@NewGroup", clientUserGroupInfo.newGroupName);
                    sqlComm.Parameters.AddWithValue("@OldGroup", clientUserGroupInfo.oldGroupName);
                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    //var reader = sqlComm.ExecuteReader();
                    return GetGroupListWithScorecard();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }


        public dynamic DeleteClientUserGroup(ClientUserGroupInfo clientUserGroupInfo)
        {
            string sql = @"delete from scorecard_groups  where groupName = @group; delete from user_groups where userGroup = @groupId";

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                try
                {
                    sqlComm.Parameters.AddWithValue("@group", clientUserGroupInfo.groupName);
                    sqlComm.Parameters.AddWithValue("@groupId", clientUserGroupInfo.id);

                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    //var reader = sqlComm.ExecuteReader();
                    return GetGroupListWithScorecard();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }


        /// <summary>
        /// GetUsersByScorecard
        /// </summary>
        /// <param name="scorecardInfo"></param>
        /// <returns></returns>
        public dynamic GetUsersByScorecard(int scorecardId)
        {
            string sql = @"select username,id from UserExtraInfo where id in (select userId from user_groups where scorecard = @sc) and UserExtraInfo.active = 1 ";

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                try
                {
                    sqlComm.Parameters.AddWithValue("@sc", scorecardId);


                    sqlCon.Open();
                    //sqlComm.ExecuteNonQuery();
                    List<User> users = new List<User>();
                    var reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            userId = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                            userName = reader.GetFieldValue<string>(reader.GetOrdinal("username"))
                        });
                    }
                    return users;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        /// <summary>
        /// AddUserToMultipleGroup
        /// </summary>
        /// <param name="multipleUser"></param>
        /// <returns></returns>
        public dynamic AddUserToMultipleGroup(MultipleUserAddingToGroupModelV2 multipleUser)
        {
            if (multipleUser.scorecards == null || multipleUser.scorecards.Count == 0)
            {
                return "No group or scorecard selected!";
            }
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                try
                {
                    StringBuilder sql = new StringBuilder("insert into user_groups (userID,userGroup,scorecard,dateadded)values");// +
                                                                                                                                  // "(@userId,(select top 1 id from scorecard_groups where groupName = @groupName and scorecard = @sc),@sc,GetDate())");
                    for (var i = 0; i < multipleUser.scorecards.Count; i++)
                    {

                        for (var j = 0; j < multipleUser.scorecards[i].groups.Count; j++)
                        {
                            sql.Append("(@userId" + i + j * 2 + ",(select top 1 id from scorecard_groups" +
                                " where groupName = @groupName" + i + j * 2 + " and scorecard = @sc" + i + j * 2 + "),@sc" + i + j * 2 + ",GetDate()),");


                        }
                        //if (i < multipleUser.scorecardsInGroups.Count - 1)
                        //{
                        //    sql.Append(",");
                        //}

                    }
                    sql.Remove(sql.Length - 1, 1);


                    var sqlComm = new SqlCommand
                    {
                        CommandText = sql.ToString(),
                        CommandType = CommandType.Text,
                        CommandTimeout = int.MaxValue,
                        Connection = sqlCon
                    };

                    for (var i = 0; i < multipleUser.scorecards.Count; i++)
                    {
                        for (var j = 0; j < multipleUser.scorecards[i].groups.Count; j++)
                        {
                            sqlComm.Parameters.AddWithValue("@sc" + i + j * 2, multipleUser.scorecards[i].scorecardInfo.id);
                            sqlComm.Parameters.AddWithValue("@userId" + i + j * 2, multipleUser.userId);
                            sqlComm.Parameters.AddWithValue("@groupName" + i + j * 2, multipleUser.scorecards[i].groups[j].name);
                        }
                    }

                  
                    sqlComm.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

               

                UserInformationSimple user =new UserInformationSimple();
                List<string> apps = new List<string>();
                try
                {
                    string sqlUser = @"select top 1 user_role as userRole,username as userName,id as userId from UserExtraInfo where id =" + multipleUser.userId;
                    user = DapperHelper.GetSingle<UserInformationSimple>(sqlUser);
                   
                    
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    int x = 0;
                    foreach (var item in multipleUser.scorecards)
                    {

                        string sqlapp = @"select top 1 appname from scorecards where id= " + item.scorecardInfo.id;
                        apps.Add(DapperHelper.GetSingle<string>(sqlapp));

                        string command = "insert into UserApps " +
                       "(username,appname,dateadded,who_added,user_scorecard,scorecard_role,speed_increment,userid)" +
                       "values(" +
                       "@username," +
                       "@appname," +
                       "dbo.GetMTDate()," +
                       "@who_added," +
                       "@user_scorecard," +
                       "@scorecard_role," +
                       "20," +
                       "@userid" +
                       ")";
                        var sqlComm = new SqlCommand
                        {
                            CommandText = command.ToString(),
                            CommandType = CommandType.Text,
                            CommandTimeout = int.MaxValue,
                            Connection = sqlCon
                        };
                        sqlComm.Parameters.AddWithValue("@username", user.userName);
                        sqlComm.Parameters.AddWithValue("@appname",apps[x] );
                        sqlComm.Parameters.AddWithValue("@who_added", userName);
                        sqlComm.Parameters.AddWithValue("@user_scorecard", item.scorecardInfo.id);
                        sqlComm.Parameters.AddWithValue("@scorecard_role", user.userRole);
                        sqlComm.Parameters.AddWithValue("@userid", user.userId);
                        sqlComm.ExecuteNonQuery();
                        x++;
                    }
                   

                }
                catch(Exception ex)
                {
                    throw ex;
                }


                try
                {
                    if (user.userRole == "Agent")
                    {
                        for (var i = 0; i < multipleUser.scorecards.Count; i++)
                        {
                            for (var j = 0; j < multipleUser.scorecards[i].groups.Count; j++)
                            {
                                if (multipleUser.scorecards[i].groups[j].updateOlderData == true)
                                {
                                    var sqlComm2 = new SqlCommand
                                    {
                                        CommandText = "update XCC_REPORT_NEW set AGENT_GROUP = @group where AGENT = @userName and scorecard = @scorecard",
                                        CommandType = CommandType.Text,
                                        CommandTimeout = int.MaxValue,
                                        Connection = sqlCon
                                    };
                                    try
                                    {
                                        sqlComm2.Parameters.AddWithValue("@scorecard", multipleUser.scorecards[i].scorecardInfo.id);
                                        sqlComm2.Parameters.AddWithValue("@userName", user.userName);
                                        sqlComm2.Parameters.AddWithValue("@group", multipleUser.scorecards[i].groups[j].name);
                                        
                                        var num = sqlComm2.ExecuteNonQuery();
                                        
                                    }catch(Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                sqlCon.Close();
                return "success";

            }
        }

        /// <summary>
        /// MoveUserToOtherGroup
        /// </summary>
        /// <param name="moveUser"></param>
        /// <returns></returns>
        public dynamic MoveUserToOtherGroup(MoveUserToOtherGroupModel moveUser)
        {

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                try
                {
                    var sql = @"delete from user_groups where userID = @userId and scorecard = @OldSc " +
                      "and userGroup in (select top 1 id from scorecard_groups where groupName = @OldGroupName and scorecard = @OldSc); " +
                     " insert into user_groups (userID,userGroup,scorecard,dateadded)values" +
                     "(@userId,(select top 1 id from scorecard_groups where groupName = @NewGroupName and scorecard = @NewSc),@NewSc,GetDate())";
                    var sqlComm = new SqlCommand
                    {
                        CommandText = sql.ToString(),
                        CommandType = CommandType.Text,
                        CommandTimeout = int.MaxValue,
                        Connection = sqlCon
                    };
                    sqlComm.Parameters.AddWithValue("@OldSc", moveUser.oldScorecardId);
                    sqlComm.Parameters.AddWithValue("@NewSc", moveUser.newScorecardId);
                    sqlComm.Parameters.AddWithValue("@OldGroupName", moveUser.oldGroupName);
                    sqlComm.Parameters.AddWithValue("@NewGroupName", moveUser.newGroupName);
                    sqlComm.Parameters.AddWithValue("@userId", moveUser.userId);

                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }



                return "success";
            }
        }


        /// <summary>
        /// SaveUserInfo
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Models.UserGroupModels.UserInfo SaveUserInfo(EditUserModel user)
        {

            string sql = @"update Userextrainfo set " +
                "UserName=  @username," +
                "first_name=@firstname," +
                "Last_name=@lastname," +
                "email_address=@EmailAddress," +
                "default_page= @defaultPage," +
                "user_role = @userRole," +
                "manager = @manager" +
                ",non_Edit =@non_Edit  " +
                ",non_calib =@non_calib  " +
                ",no_dash =@no_dash " +
                ",force_review =@force_review" +
                ",excludeCalls = @excludeCalls" +
                ",active = @active " +
                " where id=@userId;" +
                " update UserApps set username = @username where userid = @userId";

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nicole feagins";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@userId", user.userId);
                sqlComm.Parameters.AddWithValue("@username", user.userName);
                sqlComm.Parameters.AddWithValue("@firstname", user.firstName);
                sqlComm.Parameters.AddWithValue("@lastname", user.lastName);
                sqlComm.Parameters.AddWithValue("@EmailAddress", user.email);
                sqlComm.Parameters.AddWithValue("@defaultPage", user.defaultPage);
                sqlComm.Parameters.AddWithValue("@userRole", user.userRole);
                sqlComm.Parameters.AddWithValue("@manager", user.userManager);
                sqlComm.Parameters.AddWithValue("@non_Edit", user.nonEdit);
                sqlComm.Parameters.AddWithValue("@non_calib", user.nonCalibrating);
                sqlComm.Parameters.AddWithValue("@no_dash", user.nondashboardAccess);
                sqlComm.Parameters.AddWithValue("@force_review", user.forceRewiew);
                sqlComm.Parameters.AddWithValue("@excludeCalls", user.excludeCalls);
                sqlComm.Parameters.AddWithValue("@active", user.active == true ? 1 : 0);
                sqlCon.Open();
                try
                {
                    var reader = sqlComm.ExecuteNonQuery();
                }
                catch (Exception ex) { throw ex; }
                sqlCon.Close();


               



                //if (user.updateOlderData == true)
                //{
                //    string comm = "update xcc_report_new set agent_group = '" & user. & "' where agent = '" & e.OldValues("username").ToString & "' and isnull(agent_group,'') <> '" & agent_group & "' and scorecard in (select user_scorecard from userapps where username = '" & User.Identity.Name & "') ";
                //    try
                //    {
                //        var sqlCommand = new SqlCommand
                //        {
                //            CommandText = comm,
                //            CommandType = CommandType.Text,
                //            CommandTimeout = int.MaxValue,
                //            Connection = sqlCon
                //        };
                //        //sqlCommand.Parameters.AddWithValue("@username", user.userName);
                //        sqlCommand.Parameters.AddWithValue("@userId", user.userId);
                //        sqlCommand.Parameters.AddWithValue("@newName", user.userName);
                //        sqlCon.Open();
                //        sqlCommand.ExecuteNonQuery();
                //        sqlCon.Close();
                //    }

                    
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}

              

            }
            return GetUserInfo(user.userId);
        }


        /// <summary>
        /// GetScorecardsWithGroups
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public dynamic GetScorecardsWithGroups(int? scIds)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "NataliaAdmin";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                string sql = @"select distinct sc.id as ScorecardId,sc.short_name as ScorecardName,sg.groupName,sg.id as groupId from scorecards sc " +
                            "join scorecard_groups sg on sg.scorecard = sc.id " +
                            "left JOIN user_groups ON user_groups.userGroup = sg.id " +
                            "WHERE " +
                            "sg.scorecard in (select sc.id from userapps ua" +
                            " left JOIN user_groups ON user_groups.userGroup = sg.id " +
                            " LEFT JOIN scorecards ON scorecards.id = sg.scorecard " +
                            " WHERE " +
                            " sg.scorecard " +
                            " IN ( select sc.id from userapps ua " +
                            " left join userextrainfo ux on ua.userName = ux.userName  " +
                            " left join scorecards sc on sc.id = ua.user_scorecard " +
                            " where sc.appname in  (select appname from userapps where username = @userName ))" +
                            " and (@scIds is null or @scIds= sc.id )) order by sc.short_name;";
                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@userName", userName);
                if (scIds != null)
                {
                    sqlComm.Parameters.AddWithValue("@scIds", scIds);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@scIds", DBNull.Value);
                }
                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                List<ClientGroupScorecardUserList> clientUserGroupList = new List<ClientGroupScorecardUserList>();
                List<ClientUserScorecardGroupList> clientUserScorecardGroupLists = new List<ClientUserScorecardGroupList>();
                while (reader.Read())
                {
                    var scorecardUserModels = new ScorecardUserModel();

                    var groupInfo = new GroupInfo()
                    {
                        id = reader.GetFieldValue<int>(reader.GetOrdinal("groupId")),
                        name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName")),
                    };
                    var grlist = new List<GroupInfo>
                    {
                        groupInfo
                    };


                    //scorecardUserModels = new ScorecardUserModel() { scorecard = scorecardsInfo, user = userList };

                    clientUserScorecardGroupLists.Add(new ClientUserScorecardGroupList
                    {
                        scporecard = new ScorecardInfo()
                        {
                            scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("ScorecardId")),
                            scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("ScorecardName")),
                        },
                        //scorecardsUserInfo = scorecardUserModels,
                        groupInfos = groupInfo
                    });

                }
                var data = (from inf in clientUserScorecardGroupLists
                            group inf by new { inf.scporecard.scorecardName, inf.scporecard.scorecardId } into gInf
                            select new
                            {
                                gInf.Key.scorecardName,
                                gInf.Key.scorecardId,
                                groupInfo = (from i in gInf select i.groupInfos).Distinct()
                            });
                return data;
            }
        }



        public dynamic GetAppScorecardList(Search search = null)
        {
            var userName = HttpContext.Current.GetUserName();
            List<AppsListModel> response = new List<AppsListModel>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = "[GetAppsWithScorecardsClient]",
                    Connection = sqlCon
                };
                if (search?.columns != null && search.text != null && search.text != "" && search.columns.Count > 0)
                {
                    var preparedLst = new StringBuilder();

                    SqlComm.Parameters.AddWithValue("@searchstr", search.text);
                    foreach (var value in search.columns)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    SqlComm.Parameters.AddWithValue("@searchColumn", preparedLst.ToString());
                }
                SqlComm.Parameters.AddWithValue("@username", userName);

                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                try
                {
                    response = AppsListModel.Create(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                return response;
            }
        }


        public dynamic GetGroupByscorecardIds(List<int> scorecardIds)
        {
            var userName = HttpContext.Current.GetUserName();
            List<string> groups = new List<string>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                foreach (var item in scorecardIds)
                {
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = @"select groupName from scorecard_groups where scorecard = @sc",
                        Connection = sqlCon
                    };
                    SqlComm.Parameters.AddWithValue("@sc", item);
                    sqlCon.Open();
                    var reader = SqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            groups.Add(reader.Get<string>("groupName"));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                    sqlCon.Close();
                }

                return groups;

            }

        }





        public List<GroupInfo> GetGroupListFull(int scorecardId)
        {
            var userName = HttpContext.Current.GetUserName();
            List<string> groups = new List<string>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var command = @"select groupName as name,id from scorecard_groups where scorecard ="+ scorecardId;
                var mapper = DapperHelper.GetList<GroupInfo>(command).ToList();
                return mapper;
            }
        }

        public string ChangeUserActiveStatus(UserInfoActiveModel user)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = @"update UserExtraInfo set active = @state where  id = @id",
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@state", user.active);
                SqlComm.Parameters.AddWithValue("@id", user.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();

            }
            return "success";
        }

        public dynamic GetScorecardsWithGroups(string group = null)
        {
            var userName = HttpContext.Current.GetUserName();
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"SELECT distinct scorecard_groups.id " +
                           ",groupName " +
                           ",scorecard_groups.scorecard " +
                           ",short_name " +
                           " FROM scorecard_groups " +
                           " left JOIN user_groups ON user_groups.userGroup = scorecard_groups.id " +
                           " LEFT JOIN scorecards ON scorecards.id = scorecard_groups.scorecard " +
                           " WHERE " +
                           " scorecard_groups.scorecard " +
                           " IN ( select sc.id from userapps ua " +
                           " left join userextrainfo ux on ua.userName = ux.userName " +
                           " left join scorecards sc on sc.id = ua.user_scorecard " +
                           " where sc.appname in  (select appname from userapps where username = @userName ))" +
                           " and (@groupName is null or @groupName= groupName ) and scorecards.active = 1  order by short_name,groupName ;";
                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                if (group != null && group != string.Empty)
                {
                    sqlComm.Parameters.AddWithValue("@groupName", group);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@groupName", DBNull.Value);
                }
                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                List<ClientUserGroupListV2> response = new List<ClientUserGroupListV2>();
                List<ClientGroupScorecardUserList> clientUserGroupList = new List<ClientGroupScorecardUserList>();
                while (reader.Read())
                {
                    var scorecardUserModels = new ScorecardUserModel();

                    var scorecardsInfo = new ScorecardInfo()
                    {
                        scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecard")),
                        scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                    };
                    var sclist = new List<ScorecardInfo>
                    {
                        scorecardsInfo
                    };
                    //scorecardUserModels = new ScorecardUserModel() { scorecard = scorecardsInfo, user = userList };
                    clientUserGroupList.Add(new ClientGroupScorecardUserList
                    {
                        groupInfo = new GroupInfo()
                        {
                            id = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                            name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName")),
                        },
                        //scorecardsUserInfo = scorecardUserModels,
                        scorecardsInfo = scorecardsInfo
                    });
                }

                var data = (from inf in clientUserGroupList
                            group inf by new { inf.scorecardsInfo.scorecardId, inf.scorecardsInfo.scorecardName } into ginf
                            select new
                            {
                                groups = (from i in ginf select i.groupInfo).Distinct(),
                                scorecardInfo = new ScorecardInfoV2()
                                {
                                    id = ginf.Key.scorecardId,
                                    name = ginf.Key.scorecardName
                                }
                            });
                return data;
            }
        }

        /// <summary>
        /// GetUserOwnedGroupNew
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserOwnedGroupNew(int userId)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "winnie";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                string sql = @"SELECT distinct " +
                                "groupName  " +
                                ",scorecard_groups.scorecard  " +
                                ",short_name " +
                                ",user_groups.id ugId" +
                                ",user_groups.userID" +
                                ",scorecard_groups.id as groupId" +
                                " FROM scorecard_groups " +
                                " left JOIN user_groups ON user_groups.userGroup = scorecard_groups.id " +
                                " LEFT JOIN scorecards ON scorecards.id = scorecard_groups.scorecard " +
                                " WHERE " +
                                " scorecard_groups.scorecard " +
                                " IN ( select sc.id from userapps ua " +
                                " left join userextrainfo ux on ua.userName = ux.userName " +
                                " left join scorecards sc on sc.id = ua.user_scorecard " +
                                " where sc.appname in  (select appname from userapps where username = @userName ))" +
                                " and user_groups.userID =@userId and scorecards.active = 1;";
                var sqlComm = new SqlCommand
                {
                    CommandText = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);

                sqlComm.Parameters.AddWithValue("@userId", userId);

                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();

                List<ClientGroupScorecardUserList> clientUserGroupList = new List<ClientGroupScorecardUserList>();
                while (reader.Read())
                {
                    var scorecardUserModels = new ScorecardUserModel();

                    var scorecardsInfo = new ScorecardInfo()
                    {
                        scorecardId = reader.GetFieldValue<int>(reader.GetOrdinal("scorecard")),
                        scorecardName = reader.GetFieldValue<string>(reader.GetOrdinal("short_name"))
                    };
                    var sclist = new List<ScorecardInfo>()
                    {
                        scorecardsInfo
                    };


                    //scorecardUserModels = new ScorecardUserModel() { scorecard = scorecardsInfo, user = userList };

                    clientUserGroupList.Add(new ClientGroupScorecardUserList
                    {
                        groupInfo = new GroupInfo()
                        {
                            id = reader.GetFieldValue<int>(reader.GetOrdinal("groupId")),
                            name = reader.GetFieldValue<string>(reader.GetOrdinal("groupName")),
                        },
                        //scorecardsUserInfo = scorecardUserModels,
                        scorecardsInfo = scorecardsInfo
                    });


                }
                var data = (from inf in clientUserGroupList
                            group inf by new { inf.scorecardsInfo.scorecardId, inf.scorecardsInfo.scorecardName } into gInf
                            select new
                            {
                                groups = (from i in gInf select i.groupInfo).Distinct(),
                                scorecardInfo = new ScorecardInfoV2()
                                {
                                    id = gInf.Key.scorecardId,
                                    name = gInf.Key.scorecardName
                                }
                            });
                return data;
            }

        }
    }
}

