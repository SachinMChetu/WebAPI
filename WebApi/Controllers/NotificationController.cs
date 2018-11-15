using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using static WebApi.DataLayer.NotificationLayer;
using WebApi.DataLayer;
using System.Text;
using DAL.Models;
using DAL.Models.NotificationModels;
using System.Linq;
using DAL.Extensions;

namespace WebApi.Controllers
{

    //  [Authorize]
    public class NotificationController : ApiController
    {
        [Route("v1.0/notification/GetAvailableNotificationFilters")]
        [HttpPost]
        [ResponseType(typeof(NotificationFilters))]
        public NotificationFilters GetAvailableNotificationFilters(GetAvailableNotificationFiltersRequestData filter_req)
        {
            NotificationFilters notiFilters = new NotificationFilters();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sq = new SqlCommand("[getNotificationFilters]");
                sq.Connection = sqlCon;
                sq.CommandType = CommandType.StoredProcedure;

                if (filter_req.filters != null)
                {

                    if (filter_req.filters.apps != null && filter_req.filters.apps.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.apps)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@apps", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("apps", filter_req.filters.apps);
                    if (filter_req.filters.supervisors != null && filter_req.filters.supervisors.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.supervisors)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@supervisors", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("supervisors", (filter_req.filters.supervisors));
                    if (filter_req.filters.scorecards != null && filter_req.filters.scorecards.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.scorecards)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@scorecards", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("scorecards", (filter_req.filters.scorecards));
                    if (filter_req.filters.QAs != null && filter_req.filters.QAs.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.QAs)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@QAs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("QAs", (filter_req.filters.QAs));
                    if (filter_req.filters.teamLeads != null && filter_req.filters.teamLeads.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.teamLeads)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@teamLeads", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("teamLeads",(filter_req.filters.teamLeads));
                    if (filter_req.filters.calibrators != null && filter_req.filters.calibrators.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter_req.filters.calibrators)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sq.Parameters.AddWithValue("@calibrators", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    //sq.Parameters.AddWithValue("calibrators", (filter_req.filters.calibrators));
                    sq.Parameters.AddWithValue("filterByDateClosed", filter_req.filters.filterByDateClosed);
                }
                sq.Parameters.AddWithValue("start", filter_req.range.start);
                sq.Parameters.AddWithValue("end", filter_req.range.end);
                sq.Parameters.AddWithValue("username", userName);
                sq.CommandTimeout = int.MaxValue;
                sqlCon.Open();

                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sq);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable notify_dt = ds.Tables[0];

                //apps

                List<FilterItem> fitems = new List<FilterItem>();
                notiFilters.filters = new NotiFilters();
                foreach (DataRow dr in notify_dt.Rows)
                {
                    FilterItem fi = new FilterItem
                    {
                        name = dr.Field<string>("appname"),
                        id = dr.Field<string>("appname"),
                        count = dr.Field<int>("count")
                    };

                    fitems.Add(fi);
                }

                notiFilters.filters.apps = fitems;

                fitems = new List<FilterItem>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    FilterItem fi = new FilterItem
                    {
                        name = dr.Field<string>("agent_group"),
                        id = dr.Field<string>("agent_group"),
                        count = dr.Field<int>("count")
                    };

                    fitems.Add(fi);
                }

                notiFilters.filters.supervisors = fitems;

                fitems = new List<FilterItem>();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    FilterItem fi = new FilterItem
                    {
                        name = dr.Field<string>("scorecard_name"),
                        id = dr.Field<int>("scorecard"),
                        count = dr.Field<int>("count")
                    };

                    fitems.Add(fi);
                }

                notiFilters.filters.scorecards = fitems;



                fitems = new List<FilterItem>();
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    FilterItem filterItem = new FilterItem
                    {
                        name = dr.Field<string>("QA"),
                        id = dr.Field<string>("QA"),
                        count = dr.Field<int>("count")
                    };


                    fitems.Add(filterItem);
                }

                notiFilters.filters.QAs = fitems;



                fitems = new List<FilterItem>();
                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    FilterItem fi = new FilterItem
                    {
                        name = dr.Field<string>("team_Lead"),
                        id = dr.Field<string>("team_Lead"),
                        count = dr.Field<int>("count")
                    };

                    fitems.Add(fi);
                }

                notiFilters.filters.teamLeads = fitems;

                fitems = new List<FilterItem>();
                foreach (DataRow dr in ds.Tables[5].Rows)
                {
                    FilterItem fi = new FilterItem
                    {
                        name = dr.Field<string>("calibrator"),
                        id = dr.Field<string>("calibrator"),
                        count = dr.Field<int>("count")
                    };

                    fitems.Add(fi);
                }

                notiFilters.filters.calibrators = fitems;

                List<Count> counts = new List<Count>();
                foreach (DataRow dr in ds.Tables[6].Rows)
                {
                    notiFilters.countsFiltered = dr.Field<int>("filtered_total");
                    notiFilters.countsTotal = dr.Field<int>("total");

                }

                return notiFilters;
            }
        }




        /// <summary>
        /// GetNotificationCalls
        /// </summary>
        /// <param name="noti_post"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationCalls")]
        [HttpPost]
        [ResponseType(responseType: typeof(GetNCallsRequestData))]
        public dynamic GetNotificationCalls([FromBody]GetNCallsRequestData noti_post)
        {
            GetNotificationCallsResponseData noti_response = new GetNotificationCallsResponseData();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sq = new NotificationLayer().PrepareParams("[getNotificationsReportAllCals]", userName, noti_post);


                sq.CommandType = CommandType.StoredProcedure;
                sq.CommandTimeout = int.MaxValue;
                sq.Connection = sqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(sq);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable notify_dt = ds.Tables[1];

                int total = notify_dt.Rows[0].Field<int>("total");

                List<Notification> notifications = new List<Notification>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Notification not = new Notification
                    {
                        app = new UserApp() { appId = dr.Field<string>("appname").ToString(), appName = dr.Field<string>("appname").ToString() }
                    };
                    UserInformation ui = new UserInformation
                    {
                        userData = new User()
                    };
                    ui.userData.userName = dr.Field<string>("assigned_to") == null ? "" : dr.Field<string>("assigned_to");
                    ui.userData.userId = dr.Field<string>("assigned_to") == null ? "" : dr.Field<string>("assigned_to");
                    ui.userRole = new Role
                    {
                        userRoleName = dr.Field<string>("role") == null ? "" : dr.Field<string>("role"),
                        userRoleId = dr.Field<string>("role") == null ? "" : dr.Field<string>("role")
                    };
                    not.assignTo = ui;
                    not.calibratorName = "";
                    not.callType = dr.Field<string>("review_type") == null ? "" : dr.Field<string>("review_type");
                    not.closedDate = dr.Field<DateTime?>("date_closed");
                    not.notificationId = dr.Field<int>("FN_ID");
                    not.callId = dr.Field<int>("F_ID");
                    not.phone = dr.Field<string>("phone");
                    not.openDate = dr.Field<DateTime?>("date_created");
                    //not.questionInfo = 
                    not.reviewedDate = dr.Field<DateTime?>("review_date");
                    not.reviewerName = dr.Field<string>("reviewer") == null ? "" : dr.Field<string>("reviewer");
                    not.agentName = dr.Field<string>("agent") == null ? "" : dr.Field<string>("agent");
                    Scorecard sc = new Scorecard();
                    sc.scorecardId = dr.Field<int>("scorecard");
                    sc.scorecardName = dr.Field<string>("scorecard_name") == null ? "" : dr.Field<string>("scorecard_name");
                    not.scorecard = sc;
                    //not.supervisor = "";
                    not.teamLeadName = dr.Field<string>("team_lead") == null ? "" : dr.Field<string>("team_lead");
                    not.notificationStatus = dr.Field<string>("notificationStatus") == null ? "" : dr.Field<string>("notificationStatus");
                    notifications.Add(not);
                }
                return new { notifications, total };
            }
        }


        /// <summary>
        /// GetNotifications
        /// </summary>
        /// <param name="noti_post"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationsByUser")]
        [HttpPost]
        [ResponseType(responseType: typeof(GetNotificationCallsResponseData))]
        public dynamic GetNotifications([FromBody]GetNCallsRequestData noti_post)
        {
            GetNotificationCallsResponseData noti_response = new GetNotificationCallsResponseData();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "winnie";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sq = new NotificationLayer().PrepareParams("[getNotificationsReportByUser]", userName, noti_post);
                sq.CommandType = CommandType.StoredProcedure;
                sq.Connection = sqlCon;
                sq.CommandTimeout = int.MaxValue;
                SqlDataAdapter adapter = new SqlDataAdapter(sq);
                DataSet ds = new DataSet();

                adapter.Fill(ds);

                List<NotificationByUser> usernot = new List<NotificationByUser>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NotificationByUser not = new NotificationByUser();
                    UserInformation ui = new UserInformation();

                    ui.userData = new User();
                    ui.userRole = new Role();
                    not.scorecard = new Scorecard()
                    {
                        scorecardName = dr.Field<string>("short_name"),
                        scorecardId = dr.Field<int>("scorecard")
                    };
                    ui.userData.userName = dr.Field<string>("userName");
                    ui.userData.userId = dr.Field<string>("userName");
                    ui.userRole.userRoleName = dr.Field<string>("user_role");
                    ui.userRole.userRoleId = dr.Field<string>("user_role");
                    not.app = new UserApp()
                    {
                        appName = dr.Field<string>("appname"),
                        appId = dr.Field<string>("appname")
                    };
                    not.user = ui;
                    not.canWork = dr.Field<int>("canWork");
                    not.created = dr.Field<int>("created");
                    not.closed = dr.Field<int>("closed"); ;
                    not.avgDaysOpen = dr.Field<double?>("avgDaysOpen");
                    usernot.Add(not);
                }

                return usernot;

            }
        }

        /// <summary>
        /// GetNotificationCallss
        /// </summary>
        /// <param name="noti_post"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationsByScorecard")]
        [HttpPost]
        [ResponseType(responseType: typeof(GetNCallsRequestData))]
        public dynamic GetNotificationCallss([FromBody]GetNCallsRequestData noti_post)
        {
            GetNotificationCallsResponseData noti_response = new GetNotificationCallsResponseData();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sq = new NotificationLayer().PrepareParams("[getNotificationsReportByScorecard]", userName, noti_post);
                sq.CommandType = CommandType.StoredProcedure;
                sq.CommandTimeout = int.MaxValue;
                sq.Connection = sqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(sq);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                List<NotificationByScorecard> scnot = new List<NotificationByScorecard>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NotificationByScorecard not = new NotificationByScorecard();
                    Scorecard sc = new Scorecard();
                    UserApp app = new UserApp();
                    sc.scorecardId = dr.Field<int>("scorecard");
                    sc.scorecardName = dr.Field<string>("scorecardName").ToString();
                    not.scorecard = sc;
                    not.app = app;
                    not.totalCount = dr.Field<int>("totalNotifications");
                    not.openedByCC = dr.Field<int>("openNotificationsByCC");
                    not.openedByClient = dr.Field<int>("openNotificationsByClient");
                    not.closedWithDisp = dr.Field<int>("closedWithDisp");
                    not.closedWithoutDisp = dr.Field<int>("closedWithoutDisp");
                    not.nonbillableTime = dr.Field<int>("nonbillableTime");
                    not.billableTime = dr.Field<int>("billableTime");
                    not.avgDaysOpen = dr.Field<int>("avgDaysOpen");

                    scnot.Add(not);
                }
                return scnot;

            }
        }

        /// <summary>
        /// GetNotificationCallss
        /// </summary>
        /// <param name="callId"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationFlow")]
        [HttpPost]
        [ResponseType(responseType: typeof(List<NotificationComment>))]
        public dynamic GetNotificationFlow([FromBody]int callId)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sq = new SqlCommand(" select fn.[id] , fn.[role] , fn.[date_created] , fn.[date_closed], " +
                                                " fn.[opened_by], CASE WHEN  role = 'Agent' THEN uic.username ELSE fn.[closed_by] end, fn.[comment], " +
                                                " uio.[user_role] AS UserOpenedRole, uic.[user_role] as UserClosedRole " +
                                                " from form_notifications fn left join userextrainfo uio on uio.username = [opened_by] left join userextrainfo uic on uic.username = [closed_by] " +
                                                " where form_id = @callID  order by[date_created] desc ; " +
                                                " select[comment_who],dateadd(s, -1,[comment_date]) as [comment_date],[comment],sc.id,uio.[user_role] as userRole" +
                                                " from system_comments sc" +
                                                " left join userextrainfo uio on uio.username= [comment_who]" +
                                                " where comment_id = @callID  order by [comment_date] desc;" +
                                                " select reviewer,agent from vwForm where F_ID = @callID");



                sq.Parameters.AddWithValue("@callID", callId);
                sq.CommandType = CommandType.Text;
                sq.CommandTimeout = int.MaxValue;
                sq.Connection = sqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(sq);
                var dt = new DataSet();
                adapter.Fill(dt);
                NotificationInfo1 notificationInfo = new NotificationInfo1();
                // List<NotificationByScorecard> scnot = new List<NotificationByScorecard>();
                var notificationFlow = new List<NotificationComment>();

                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    var openedBy = new UserInformation();
                    try
                    {
                        openedBy.userData = new User()
                        {
                            userId = dr.Field<string>("opened_by"),
                            userName = dr.Field<string>("opened_by"),
                        };
                        openedBy.userRole = new Role()
                        {
                            userRoleId = dr.Field<string>("UserOpenedRole"),
                            userRoleName = dr.Field<string>("UserOpenedRole")
                        };
                    }
                    catch
                    {
                        openedBy.userData = null;
                        openedBy.userRole = null;
                    }


                    var closedBy = new UserInformation();
                    try
                    {
                        closedBy.userData = new User()
                        {
                            userId = dr.Field<string>("closed_by"),
                            userName = dr.Field<string>("closed_by")
                        };
                        closedBy.userRole = new Role()
                        {
                            userRoleId = dr.Field<string>("UserClosedRole"),
                            userRoleName = dr.Field<string>("UserClosedRole")
                        };
                    }
                    catch
                    {
                        closedBy.userData = null;
                        closedBy.userRole = null;
                    }
                    Role role = null;
                    try
                    {
                        role = new Role()
                        {
                            userRoleId = dr.Field<string>("role"),
                            userRoleName = dr.Field<string>("role")
                        };
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        notificationFlow.Add(new NotificationComment()
                        {
                            id = dr.Field<int>("id"),
                            closedBy = closedBy,
                            closedDate = dr.Field<DateTime?>("date_closed"),
                            notificationRole = role,
                            openDate = dr.Field<DateTime?>("date_created"),
                            openedBy = openedBy,
                            text = dr.Field<string>("comment")


                        });
                    }
                    catch
                    {
                    }


                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                notificationInfo.notificationComments = notificationFlow;

                List<SystemComment> systemComment = new List<SystemComment>();
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    var userInfo = new UserInformation();
                    try
                    {
                        userInfo.userData = new User()
                        {
                            userId = dr.Field<string>("comment_who"),
                            userName = dr.Field<string>("comment_who")
                        };
                        userInfo.userRole = new Role()
                        {
                            userRoleId = dr.Field<string>("userRole"),
                            userRoleName = dr.Field<string>("userRole")
                        };
                    }
                    catch
                    {
                        userInfo.userData = null;
                        userInfo.userRole = null;
                    }
                    try
                    {
                        systemComment.Add(new SystemComment()
                        {
                            id = dr.Field<int>("id"),
                            user = userInfo,
                            commentDate = dr.Field<DateTime?>("comment_date"),
                            text = dr.Field<string>("comment"),
                        });
                    }
                    catch
                    {
                        // ignored
                    }


                }
                notificationInfo.systemComments = systemComment;
                foreach (DataRow dr in dt.Tables[2].Rows)
                {
                    notificationInfo.qaName = dr.Field<string>("reviewer");
                    notificationInfo.agentName = dr.Field<string>("agent");
                }

                return notificationInfo;



            }
        }


        /// <summary>
        /// Data for calls tab 
        /// </summary>
        /// <param name="noti_post"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetCallsWithNotifications")]
        [HttpPost]
        [ResponseType(responseType: typeof(NotificationResponseModel))]
        public dynamic GetCallsWithNotifications([FromBody]GetNCallsRequestDataV2 noti_post)
        {
            List<NotificationCallsModel> notificationCallsModel = new List<NotificationCallsModel>();
            List<NotificationsV2> notifications = new List<NotificationsV2>();
            List<NotificationCalls> notificationCalls = new List<NotificationCalls>();
            int total = 0;
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand
                {
                    CommandText = "[getNotificationsReportAllCalsV2]",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = int.MaxValue,
                    Connection = sqlCon
                };
                #region FILTERS
                if (noti_post != null)
                {
                    if (noti_post.filters != null)
                    {
                        if (noti_post.filters.filterBy != "")
                        {
                            sqlComm.Parameters.AddWithValue("@filterBy", noti_post.filters.filterBy);
                        }
                        if (noti_post.filters.filterRole != "")
                        {
                            sqlComm.Parameters.AddWithValue("@filterRole", noti_post.filters.filterRole);
                        }
                        if (noti_post.filters.userId != 0 && noti_post.filters.userId != null)
                        {
                            sqlComm.Parameters.AddWithValue("@userId", noti_post.filters.userId);
                        }
                        if (noti_post.filters.scorecardId != 0 && noti_post.filters.scorecardId != null)
                        {
                            sqlComm.Parameters.AddWithValue("@scorecards", noti_post.filters.scorecardId);
                        }
                    }
                    
                    if (noti_post.searchText != null && noti_post.searchText != "")
                    {
                        sqlComm.Parameters.AddWithValue("@searchText", noti_post.searchText);
                    }
                    if (noti_post.pagination != null)
                    {
                        sqlComm.Parameters.AddWithValue("@pagenum", noti_post.pagination.pagenum);
                        sqlComm.Parameters.AddWithValue("@pagerows", noti_post.pagination.pagerows);
                    }
                    sqlComm.Parameters.AddWithValue("@username", userName);
                    sqlComm.Parameters.AddWithValue("@start", noti_post.range.start);
                    sqlComm.Parameters.AddWithValue("@end", noti_post.range.end);
                    sqlComm.Parameters.AddWithValue("@filterByDateClosed", noti_post.filterByDateClosed);
                }
                #endregion

                //sqlCon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds);
                }
                catch (Exception ex) { throw ex; }

                try
                {
                    DataTable notify_dt = ds.Tables[2];

                    total = notify_dt.Rows[0].Field<int>("total_calls");


                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        NotificationCalls call = new NotificationCalls
                        {
                            callId = dr.Field<int>("F_ID"),
                            agent = dr.Field<string>("AGENT") ?? "",
                            phone = dr.Field<string>("phone") ?? "",
                            scorecard = new ScorecardInfo
                            {
                                scorecardId = dr.Field<int>("scorecardId"),
                                scorecardName = dr.Field<string>("Scorecard_name")
                            },
                            reviewDate = dr.Field<DateTime?>("review_date"),
                            reviewer = dr.Field<string>("reviewer") ?? "",
                            callType = dr.Field<string>("calltype") ?? "",
                            teamLead = dr.Field<string>("team_lead") ?? ""
                        };

                        notificationCalls.Add(call);
                    }
                }
                catch (Exception ex) { throw ex; }
                try
                {

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        NotificationsV2 notif = new NotificationsV2
                        {
                            callId = dr.Field<int>("F_ID"),
                            notifId = dr.Field<int>("fn_id"),
                            openDate = dr.Field<DateTime?>("date_created"),
                            closedDate = dr.Field<DateTime?>("date_closed"),
                            assignTo = new NotificationAssigned
                            {
                                role = dr.Field<string>("role") ?? "",
                                user = new User
                                {
                                    userId = dr.Field<string>("assigned_to") ?? "",
                                    userName = dr.Field<string>("assigned_to") ?? ""
                                }
                            },
                            comment = dr.Field<string>("comment") ?? "",
                            notificationStatus = dr.Field<string>("notificationStatus") ?? "",
                            isNotificationOwner = dr.Field<bool?>("OwnedNotification") ?? null,
                            notificationCommentsPresent = dr.Field<bool?>("notificationCommentsPresent") ?? null,
                            reviewCommentsPresent = dr.Field<bool?>("reviewCommentsPresent") ?? null
                        };
                        notifications.Add(notif);
                    }
                }
                catch (Exception ex) { throw ex; }


                foreach (var i in notificationCalls)
                {
                    // notificationCallsModel.Add(new NotificationCallsModel());
                    int count = 0;
                    NotificationCallsModel notific = new NotificationCallsModel
                    {
                        calls = i,
                        notifications = new List<NotificationsV2>()
                    };
                    foreach (var j in notifications)
                    {
                        if (i.callId == j.callId)
                        {
                            notific.notifications.Add(j);
                            count++;
                        }
                    }
                    notific.notificationCount = count;
                    notific.notifications = notific.notifications.OrderBy(x => x.openDate.Value).ToList();
                    notificationCallsModel.Add(notific);
                }

                //if(noti_post.filetrs != null)
                //{
                //    if(noti_post.filetrs.filterBy != null && noti_post.filetrs.filterBy != "")
                //    {
                //        if((noti_post.filetrs.filterRole == null || noti_post.filetrs.filterRole == "") && (noti_post.filetrs.userId == null || noti_post.filetrs.userId == 0))
                //        {
                //            foreach (var item in notificationCallsModel)
                //            {
                //                item.notifications = item.notifications.Where(x => x.closedDate != null && x.notificationStatus == "Notification Closed").Select(x => x).ToList();
                //            }
                            
                //        }
                //    }
                //}
                return new NotificationResponseModel() { notificationCallsModel = notificationCallsModel, totalCalls = total };
            }
        }


        /// <summary>
        /// GetNotificationDisput
        /// </summary>
        /// <param name="callId"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationDisputes")]
        [HttpPost]
        [ResponseType(responseType: typeof(List<DisputeModel>))]
        public dynamic GetNotificationDispute([FromBody]int callId)
        {
            List<DisputeModel> disputeData = new List<DisputeModel>();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "winnie";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "getDisputes"
                };
                sqlCommand.Parameters.AddWithValue("@f_id", callId);
                sqlCommand.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    disputeData.Add(new DisputeModel
                    {
                        dateClosed = reader.IsDBNull(reader.GetOrdinal("date_closed")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("date_closed")).ToString()),
                        dateCreated = reader.IsDBNull(reader.GetOrdinal("date_created")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("date_created")).ToString()),
                        role = reader.GetValue(reader.GetOrdinal("role")).ToString(),
                        closedBy = reader.GetValue(reader.GetOrdinal("closed_by")).ToString(),
                        notificationId = reader.IsDBNull(reader.GetOrdinal("fn_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("fn_id")).ToString()),
                        comment = reader.GetValue(reader.GetOrdinal("comment")).ToString(),
                        assignedTo = reader.GetValue(reader.GetOrdinal("assignedTo")).ToString()
                    });


                }
                return disputeData;
            }
        }
        /// <summary>
        /// GetNotificationSummary
        /// </summary>
        /// <param name="noti_post"></param>
        /// <returns></returns>
        [Route("v1.0/notification/GetNotificationSummary")]
        [HttpPost]
        [ResponseType(typeof(List<NotificationModelByUser>))]
        public dynamic GetNotificationSummary([FromBody]SummaryNotifFilters noti_post)
        {
            List<NotificationModelByUser> response = new List<NotificationModelByUser>();

            var userName = HttpContext.Current.GetUserName();

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                //SqlCommand sq = new NotificationLayer().PrepareParams("[getNotificationsReportByUserNEW]", userName, noti_post);
                SqlCommand sq = new SqlCommand
                {
                    CommandText = "getNotificationsReportByUserNEW",
                    CommandType = CommandType.StoredProcedure
                };
                if (noti_post != null)
                {
                    if (noti_post.range != null)
                    {
                        sq.Parameters.AddWithValue("@start", noti_post.range.start);
                        sq.Parameters.AddWithValue("@end", noti_post.range.end);
                    }
                    sq.Parameters.AddWithValue("@filterByDateClosed", noti_post.filterByDateClosed);
                }
                sq.Parameters.AddWithValue("@username", userName);
                sq.Connection = sqlCon;
                sq.CommandTimeout = int.MaxValue;
                sqlCon.Open();
                var reader = sq.ExecuteReader();
                List<NotificationScorecardInfo> notificationScorecards = new List<NotificationScorecardInfo>();

                while (reader.Read())
                {
                    NotificationScorecardInfo scorecard = new NotificationScorecardInfo
                    {
                        scorecard = new ScorecardInfo
                        {
                            scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                            scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                        },
                        callsCount = reader.IsDBNull(reader.GetOrdinal("callsCount")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("callsCount")).ToString()),
                        totalClosed = reader.IsDBNull(reader.GetOrdinal("totalClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalClosed")).ToString()),
                        ccClosed = reader.IsDBNull(reader.GetOrdinal("ccClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccClosed")).ToString()),
                        clientClosed = reader.IsDBNull(reader.GetOrdinal("clientClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientClosed")).ToString()),
                        totalOpenPending = reader.IsDBNull(reader.GetOrdinal("totalOpenPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalOpenPending")).ToString()),
                        ccPending = reader.IsDBNull(reader.GetOrdinal("ccPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccPending")).ToString()),
                        clientPending = reader.IsDBNull(reader.GetOrdinal("clientPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientPending")).ToString()),
                        avgDaysOpen = Math.Round(reader.IsDBNull(reader.GetOrdinal("avgDaysOpen")) ? 0 : double.Parse(reader.GetValue(reader.GetOrdinal("avgDaysOpen")).ToString()), 2)
                    };

                    notificationScorecards.Add(scorecard);
                }

                List<NotificationsByUserRoleInfo> roles = new List<NotificationsByUserRoleInfo>();
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        roles.Add(new NotificationsByUserRoleInfo
                        {
                            scorecard = new ScorecardInfo
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                            },
                            role = reader.GetValue(reader.GetOrdinal("role")).ToString(),
                            callsCount = reader.IsDBNull(reader.GetOrdinal("callsCount")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("callsCount")).ToString()),
                            totalClosed = reader.IsDBNull(reader.GetOrdinal("totalClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalClosed")).ToString()),
                            ccClosed = reader.IsDBNull(reader.GetOrdinal("ccClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccClosed")).ToString()),
                            clientClosed = reader.IsDBNull(reader.GetOrdinal("clientClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientClosed")).ToString()),
                            totalOpenPending = reader.IsDBNull(reader.GetOrdinal("totalOpenPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalOpenPending")).ToString()),
                            ccPending = reader.IsDBNull(reader.GetOrdinal("ccPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccPending")).ToString()),
                            clientPending = reader.IsDBNull(reader.GetOrdinal("clientPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientPending")).ToString()),
                            avgDaysOpen = Math.Round(reader.IsDBNull(reader.GetOrdinal("avgDaysOpen")) ? 0 : float.Parse(reader.GetValue(reader.GetOrdinal("avgDaysOpen")).ToString()), 2)
                        });
                    }
                }
                List<NotificationsByUserName> users = new List<NotificationsByUserName>();
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        users.Add(new NotificationsByUserName
                        {
                            scorecard = new ScorecardInfo
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                            },
                            user = new User
                            {
                                userName = reader.GetValue(reader.GetOrdinal("user")).ToString(),
                                userId = reader.IsDBNull(reader.GetOrdinal("userId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("userId")).ToString()),
                            },
                            role = reader.GetValue(reader.GetOrdinal("user_role")).ToString(),
                            callRole = reader.GetValue(reader.GetOrdinal("call_role")).ToString(),
                            //notificationsCount = reader.IsDBNull(reader.GetOrdinal("notificationCount")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("notificationCount")).ToString()),
                            totalClosed = reader.IsDBNull(reader.GetOrdinal("totalClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalClosed")).ToString()),
                            ccClosed = reader.IsDBNull(reader.GetOrdinal("ccClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccClosed")).ToString()),
                            clientClosed = reader.IsDBNull(reader.GetOrdinal("clientClosed")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientClosed")).ToString()),
                            totalOpenPending = reader.IsDBNull(reader.GetOrdinal("totalOpenPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalOpenPending")).ToString()),
                            ccPending = reader.IsDBNull(reader.GetOrdinal("ccPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("ccPending")).ToString()),
                            clientPending = reader.IsDBNull(reader.GetOrdinal("clientPending")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("clientPending")).ToString()),
                            avgDaysOpen = Math.Round(reader.IsDBNull(reader.GetOrdinal("avgDaysOpen")) ? 0 : float.Parse(reader.GetValue(reader.GetOrdinal("avgDaysOpen")).ToString()), 2)
                        });
                    }
                }
                List<NotificationsByUserRole> rolesWithUsers = new List<NotificationsByUserRole>();
                foreach (var item in roles)
                {
                    rolesWithUsers.Add(new NotificationsByUserRole
                    {
                        role = item,
                        users = users.Where(x => x.scorecard.scorecardId == item.scorecard.scorecardId && x.role == item.role).Select(x => x).ToList()
                    });
                }
                foreach (var item in notificationScorecards)
                {
                    response.Add(new NotificationModelByUser
                    {
                        scorecard = item,
                        roles = rolesWithUsers.Where(x => x.role.scorecard.scorecardId == item.scorecard.scorecardId).Select(x => x).ToList()
                    });
                }
                foreach (var item in response)
                {
                    foreach (var i in item.roles)
                    {
                        i.users = i.users.GroupBy(x => x.user.userId).Select(q => q.FirstOrDefault()).Distinct().ToList();

                    }
                }
            }
            return response;
        }

    }
}