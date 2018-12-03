using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace WebApi.DataLayer
{
    public class NotificationLayer
    {


        public static string dest_list = "Notes Only";

        public static string[] @internal = {
                                    "QA",
                                    "Calibrator",
                                    "Center Manager",
                                    "Tango TL",
                                    "QA Lead",
                                    "Team Lead",
                                    "Account Manager"
                                };
        public static string[] external = {
                                    "Agent",
                                    "Supervisor",
                                    "Manager",
                                    "Client",
                                    "Account Manager"
                                };
        public List<Role> GetNotificationSteps(int form_id)
        {


            string myRole = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Single();
            // Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Single();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlCon;
                sqlComm.CommandText = @"select * from [dbo].[notification_steps] where profile_id = 
                                        (select isnull(isnull(sc_profile, setting_profile),1) from 
                                        vwForm join app_settings on app_settings.appname = vwForm.appname join scorecards on scorecards.id = vwForm.scorecard where f_id = "
                                        + form_id + ") and role = '" + myRole + "'";

                sqlComm.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                var roleLst = new List<Role>();
                if (dt.Rows.Count > 0)
                {
                    if ((bool)dt.Rows[0]["admin"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Admin", userRoleName = "Admin" });
                    }
                    if ((bool)dt.Rows[0]["account_manager"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Account Manager", userRoleName = "Account Manager" });
                    }
                    if ((bool)dt.Rows[0]["tango_tl"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Tango TL", userRoleName = "Tango TL" });
                    }
                    if ((bool)dt.Rows[0]["client"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Client", userRoleName = "Client" });
                    }
                    if ((bool)dt.Rows[0]["Manager"] == true | HttpContext.Current.User.Identity.Name == "QALVia")
                    {
                        roleLst.Add(new Role() { userRoleId = "Manager", userRoleName = "Manager" });
                    }
                    if ((bool)dt.Rows[0]["supervisor"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Supervisor", userRoleName = "Supervisor" });
                    }
                    if ((bool)dt.Rows[0]["Agent"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Agent", userRoleName = "Agent" });
                    }
                    if ((bool)dt.Rows[0]["QA"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "QA", userRoleName = "QA" });
                    }
                    if ((bool)dt.Rows[0]["Calibrator"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Calibrator", userRoleName = "Calibrator" });
                    }
                    if ((bool)dt.Rows[0]["TL"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Team Lead", userRoleName = "Team Lead" });
                    }

                    if ((bool)dt.Rows[0]["CM"] == true)
                    {
                        roleLst.Add(new Role() { userRoleId = "Center Manager", userRoleName = "Center Manager" });
                    }
                    return roleLst;
                }
            }


            return new List<Role>();
        }
        public bool CanClose(int callId)
        {

            string myRole = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Single();

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlCon;
                sqlComm.CommandText = "select top 1 role from VWFN where date_closed  is null and f_id = " + callId;
                sqlComm.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable notify_dt = ds.Tables[0];

                if (notify_dt.Rows.Count > 0)
                {
                    if (Array.IndexOf(@internal, notify_dt.Rows[0]["role"].ToString()) <= Array.IndexOf(@internal, myRole) & Array.IndexOf(@internal, notify_dt.Rows[0]["role"].ToString()) > -1)
                    {
                        return true;
                    }

                    if (Array.IndexOf(external, notify_dt.Rows[0]["role"].ToString()) <= Array.IndexOf(external, myRole) & Array.IndexOf(external, notify_dt.Rows[0]["role"].ToString()) > -1)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public SqlCommand PrepareParams(string v, string userName, GetNCallsRequestData noti_post)
        {
            SqlCommand sqlComm = new SqlCommand(v);


            if (noti_post != null)
            {
                if (noti_post.filters.apps != null && noti_post.filters.apps.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.apps)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@apps", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (noti_post.filters.supervisors != null && noti_post.filters.supervisors.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.supervisors)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@supervisors", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (noti_post.filters.scorecards != null && noti_post.filters.scorecards.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.scorecards)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@scorecards", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }

                if (noti_post.filters.QAs != null && noti_post.filters.QAs.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.QAs)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@QAs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (noti_post.filters.teamLeads != null && noti_post.filters.teamLeads.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.teamLeads)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@teamLeads", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (noti_post.filters.calibrators != null && noti_post.filters.calibrators.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in noti_post.filters.calibrators)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@calibrators", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
            }
            if (noti_post.searchText != null && noti_post.searchText != "")
            {
                sqlComm.Parameters.AddWithValue("@searchText", noti_post.searchText);
            }

            sqlComm.Parameters.AddWithValue("@username", userName);
            sqlComm.Parameters.AddWithValue("@start", noti_post.range.start);
            sqlComm.Parameters.AddWithValue("@end", noti_post.range.end);
            if (noti_post.pagination != null)
            {
                sqlComm.Parameters.AddWithValue("@pagenum", noti_post.pagination.pagenum);
                sqlComm.Parameters.AddWithValue("@pagerows", noti_post.pagination.pagerows);
            }
            if (noti_post.userTabFilter != null)
            {
                sqlComm.Parameters.AddWithValue("@fUser", noti_post.userTabFilter.userId);
                sqlComm.Parameters.AddWithValue("@userAction", noti_post.userTabFilter.action);
            }
            if (noti_post.scorecardTabFilter != null)
            {
                sqlComm.Parameters.AddWithValue("@isDispute", noti_post.scorecardTabFilter.isDispute);
                sqlComm.Parameters.AddWithValue("@isInternal", noti_post.scorecardTabFilter.isInternal);
            }
            
                sqlComm.Parameters.AddWithValue("@filterByDateClosed", noti_post.filters.filterByDateClosed);
               
            
            return sqlComm;
        }

        public class GetAvailableNotificationFiltersRequestData
        {
            public NFilters filters { get; set; }
            public Period range { get; set; }
        }

        public class GetNotificationCallsRequestData
        {
            public NotiFilters filters { get; set; }
            public Period range { get; set; }
            public Pagination pagination { get; set; }
            public string searchText { get; set; }
            public SortType sorting { get; set; }
        }
        public class GetNCallsRequestData
        {
            public NFilters filters { get; set; }
            public Period range { get; set; }
            public Pagination pagination { get; set; }
            public string searchText { get; set; }
            public SortType sorting { get; set; }
            /// <summary>
            /// default object state is null this object can be not null only for USER TAB FILTER
            /// </summary>
            public UserFilter userTabFilter { get; set; }
            /// <summary>
            /// default object state is null this object can be not null only for USER TAB FILTER
            /// </summary>
            public ScorecardFilter scorecardTabFilter { get; set; }

        }
        /// <summary>
        /// GetNCallsRequestDataV2 model
        /// </summary>
        public class GetNCallsRequestDataV2
        {
            
            public Period range { get; set; }
            public Pagination pagination { get; set; }
            public string searchText { get; set; }
            public SortType sorting { get; set; }
            public SpecificFilter filters { get; set; }
            public bool filterByDateClosed { get; set; }

        }

        public class SummaryNotifFilters
        {
            public Period range { get; set; }
            public bool filterByDateClosed { get; set; }
        }
        /// <summary>
        /// The filters wich used to filter nubers on summary tab to redirect to calls tab
        /// </summary>
        public class SpecificFilter
        {
            /// <summary>
            /// filter wich shows by wath we are filtering
            /// </summary>
            public string filterBy { get; set; }
            /// <summary>
            /// when we filter by role
            /// </summary>
            public string filterRole { get; set; }
            /// <summary>
            /// when we filter by specific user
            /// </summary>
            public int? userId { get; set; }
            /// <summary>
            /// when we filter by scorecard
            /// </summary>
            public int? scorecardId { get; set; }
        }

        public class GetNotificationCallsResponseData
        {
            public List<Notification> notifications { get; set; }
            public int total { get; set; }
            public int filterd { get; set; }

            public List<NotificationByUser> userNotifications { get; set; }
            public List<NotificationByScorecard> scNotifications { get; set; }

        }


        public class NotificationByUser
        {
            public int canWork { get; set; }
            public int closed { get; set; }
            public int created { get; set; }

            public UserApp app { get; set; }
            public Scorecard scorecard { get; set; }
            public double? avgDaysOpen { get; set; }
            public UserInformation user { get; set; }


        }

        public class NotificationByScorecard
        {
            public int totalCount { get; set; }
            public int openedByCC { get; set; }
            public int openedByClient { get; set; }
            public int avgDaysOpen { get; set; }
            public Scorecard scorecard { get; set; }
            public UserApp app { get; set; }
            public int closedWithDisp { get; set; }
            public int closedWithoutDisp { get; set; }
            public int billableTime { get; set; }
            public int nonbillableTime { get; set; }
        }

        public class Notification
        {
            public int notificationId { get; set; }
            public Scorecard scorecard { get; set; }
            public string callType { get; set; }
            public User supervisor { get; set; }
            public UserInformation assignTo { get; set; }
            public QuestionInfo questionInfo { get; set; }
            public UserApp app { get; set; }
            public DateTime? openDate { get; set; }
            public DateTime? closedDate { get; set; }
            public DateTime? reviewedDate { get; set; }
            public string reviewerName { get; set; }
            public string calibratorName { get; set; }
            public string teamLeadName { get; set; }
            public int callId { get; set; }
            public string phone { get; set; }
            public string agentName { get; set; }
            public string notificationStatus { get; set; }
        }
        public class UserApp
        {
            public string appId { get; set; }
            public string appName { get; set; }
        }

        public class Scorecard
        {
            public int scorecardId { get; set; }
            public string scorecardName { get; set; }
        }
        public class ScorecardFilter
        {
            public bool? isInternal { get; set; }
            public bool? isDispute { get; set; }
        }



        public class NotificationFilters
        {
            public NotiFilters filters { get; set; }
            public int countsTotal { get; set; }
            public int countsFiltered { get; set; }


            internal static dynamic ToParam(List<FilterItem> apps)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (apps == null || apps.Count == 0) { return DBNull.Value; }
                else
                {
                    foreach (var app in apps)
                    {
                        stringBuilder.Append("'" + app.id + "',");
                    }
                    return stringBuilder.ToString().Trim(Convert.ToChar(","));
                }
            }
        }

        public class NotiFilters
        {
            public bool filterByDateClosed { get; set; }
            public List<FilterItem> apps { get; set; }
            public List<FilterItem> supervisors { get; set; }
            public List<FilterItem> scorecards { get; set; }
            public List<FilterItem> QAs { get; set; }
            public List<FilterItem> teamLeads { get; set; }
            public List<FilterItem> calibrators { get; set; }
        }
        public class NFilters
        {
            public bool filterByDateClosed { get; set; }
            public List<string> apps { get; set; }
            public List<string> supervisors { get; set; }
            public List<string> scorecards { get; set; }
            public List<string> QAs { get; set; }
            public List<string> teamLeads { get; set; }
            public List<string> calibrators { get; set; }
        }

       
        public class FilterItem
        {
            public int? count { get; set; }
            public dynamic id { get; set; }
            public string name { get; set; }
        }



        public class Count
        {
            public string day { get; set; }
            public int filtered { get; set; }
            public int total { get; set; }

        }
    }
}