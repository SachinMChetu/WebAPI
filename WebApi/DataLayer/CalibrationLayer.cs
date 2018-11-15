using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using DAL.Models.CalibrationModels;
using static WebApi.Controllers.CalibrationController;
using DAL.Models;
using WebApi.Entities;

namespace WebApi.DataLayer
{
    public class CalibrationLayer
    {
        /// <summary>
        /// SearchCalls calibration layer
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public CallDetailsResponseData SearchCalls(string searchText)
        {
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            var dbconn = new WebApi.Entities.CC_ProdEntities();
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "winnie";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlCon;
            sqlComm.CommandText = "[ApiCallSearch]";
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@number_days", 7);

            sqlComm.Parameters.AddWithValue("@userName", userName);
            sqlComm.Parameters.AddWithValue("@hidePending", true);
            sqlComm.Parameters.AddWithValue("@includeMissed", true);
            sqlComm.Parameters.AddWithValue("@hideEdited", true);

            if (searchText != null)
            {
                sqlComm.Parameters.AddWithValue("@SearchQuery", searchText);
            }

            List<CallDetailV2> callDetailsList = new List<CallDetailV2>();
            sqlCon.Open();
            SqlDataReader reader = sqlComm.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    var callSystemData = (new CallSystemData()
                    {
                        callId = reader.IsDBNull(reader.GetOrdinal("callId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                        callType = reader.GetValue(reader.GetOrdinal("calltype")).ToString(),
                        callReviewStatus = (reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString()),
                        callAudioUrl = reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                        callAudioLength = reader.IsDBNull(reader.GetOrdinal("callAudioLength")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                        websiteUrl = reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                        scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                        scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                        scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                        receivedDate = reader.IsDBNull(reader.GetOrdinal("receivedDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("receivedDate")),
                        reviewDate = reader.IsDBNull(reader.GetOrdinal("reviewDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("reviewDate")),
                        reviewerUserRole = reader.GetValue(reader.GetOrdinal("reviewerUserRole")).ToString(),
                        reviewerName = reader.GetValue(reader.GetOrdinal("reviewerName")).ToString(),
                        calibratorId = reader.GetValue(reader.GetOrdinal("calibratorId")).ToString(),
                        calibratorName = reader.GetValue(reader.GetOrdinal("calibratorName")).ToString(),
                        missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                        agentScore = reader.IsDBNull(reader.GetOrdinal("agentScore")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                        callFailed = ((reader.GetValue(reader.GetOrdinal("callFailed")).ToString() == "Pass") ? (false) : (true)),
                        reviewCommentsPresent = ((reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString() == "0") ? (false) : (true)),
                        notificationCommentsPresent = ((reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString() == "0") ? (false) : (true)),
                        notificationStatus = (reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString().ToLower()),
                        xId = reader.IsDBNull(reader.GetOrdinal("x_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("x_id")).ToString()),
                        inernalAdded = (reader.GetValue(reader.GetOrdinal("inernalAdded")).ToString() == "1"),
                        externalAdded = (reader.GetValue(reader.GetOrdinal("externalAdded")).ToString() == "1"),

                        missedItems = new List<CallMissedItem>()
                    });
                    var callMetaData = (new CallMetaData()
                    {
                        callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                        agentGroup = reader.GetValue(reader.GetOrdinal("agentGroup")).ToString(),
                        campaign = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                        agentName = reader.GetValue(reader.GetOrdinal("agentName")).ToString(),
                        sessionId = reader.GetValue(reader.GetOrdinal("sessionId")).ToString(),
                        profileId = reader.GetValue(reader.GetOrdinal("profileId")).ToString(),
                        prospectFirstName = reader.GetValue(reader.GetOrdinal("prospectFirstName")).ToString(),
                        prospectLastName = reader.GetValue(reader.GetOrdinal("prospectLastName")).ToString(),
                        prospectPhone = reader.GetValue(reader.GetOrdinal("prospectPhone")).ToString(),
                        prospectEmail = reader.GetValue(reader.GetOrdinal("prospectEmail")).ToString(),
                    });

                    callDetailsList.Add(new CallDetailV2()
                    {
                        metaData = callMetaData,
                        systemData = callSystemData
                    });

                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            List<CallMissedItem> MissedItemList = new List<CallMissedItem>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    MissedItemList.Add(new CallMissedItem()
                    {
                        itemDescription = reader.GetValue(reader.GetOrdinal("itemDescription")).ToString(),
                        position = float.Parse(reader.GetValue(reader.GetOrdinal("q_pos")).ToString()),
                        callId = int.Parse(reader.GetValue(reader.GetOrdinal("CallID")).ToString())
                    });
                }

                foreach (var item in callDetailsList)
                {
                    item.systemData.missedItems = new List<CallMissedItem>();
                    item.systemData.missedItems.AddRange(from v in MissedItemList where v.callId == item.systemData.callId select v);
                    item.callMissedItems = item.systemData.missedItems;
                }
            }

            CallDetailsResponseData CallDetailsLst = new CallDetailsResponseData();
            CallDetailsLst.itemsTotal = callDetailsList.Count;
            CallDetailsLst.calls = callDetailsList;

            return CallDetailsLst;
        }

        /// <summary>
        /// SearchCallsByAppName calibration layer
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public CallDetailsResponseData SearchCallsByAppName(CalibrationPageSearch search)
        {
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            var dbconn = new WebApi.Entities.CC_ProdEntities();
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "PDMike";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlCon;
            sqlComm.CommandText = "[ApiCallSearch]";
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@number_days", 7);

            sqlComm.Parameters.AddWithValue("@userName", userName);
            sqlComm.Parameters.AddWithValue("@hidePending", true);

            sqlComm.Parameters.AddWithValue("@hideEdited", true);
            sqlComm.Parameters.AddWithValue("@includeMissed", true);

            if (search != null)
            {
                sqlComm.Parameters.AddWithValue("@SearchQuery", search.searchText);
                sqlComm.Parameters.AddWithValue("@appName", search.filters.appName);
                sqlComm.Parameters.AddWithValue("@scorecardId", search.filters.scorecardId);
                sqlComm.Parameters.AddWithValue("@pagenum", search.pagination.pagenum);
                sqlComm.Parameters.AddWithValue("@pagerows", search.pagination.pagerows);
                sqlComm.Parameters.AddWithValue("@start", DateTime.Parse(search.range.start));
                sqlComm.Parameters.AddWithValue("@end", DateTime.Parse(search.range.end));
                if (search.sorting != null)
                {
                    sqlComm.Parameters.AddWithValue("@OrderByColumn", search.sorting.sortBy);
                    sqlComm.Parameters.AddWithValue("@sortOrder", (search.sorting.sortOrder == "desc") ? (false) : (true));
                }
            }

            List<CallDetailV2> callDetailsList = new List<CallDetailV2>();
            sqlCon.Open();
            SqlDataReader reader = sqlComm.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    var callSystemData = (new CallSystemData()
                    {
                        callId = reader.IsDBNull(reader.GetOrdinal("callId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                        callType = reader.GetValue(reader.GetOrdinal("calltype")).ToString(),
                        callReviewStatus = (reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString()),
                        callAudioUrl = reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                        callAudioLength = reader.IsDBNull(reader.GetOrdinal("callAudioLength")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                        websiteUrl = reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                        scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                        scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                        scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                        receivedDate = reader.IsDBNull(reader.GetOrdinal("receivedDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("receivedDate")),
                        reviewDate = reader.IsDBNull(reader.GetOrdinal("reviewDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("reviewDate")),
                        reviewerUserRole = reader.GetValue(reader.GetOrdinal("reviewerUserRole")).ToString(),
                        reviewerName = reader.GetValue(reader.GetOrdinal("reviewerName")).ToString(),
                        calibratorId = reader.GetValue(reader.GetOrdinal("calibratorId")).ToString(),
                        calibratorName = reader.GetValue(reader.GetOrdinal("calibratorName")).ToString(),
                        missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                        agentScore = reader.IsDBNull(reader.GetOrdinal("agentScore")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                        callFailed = ((reader.GetValue(reader.GetOrdinal("callFailed")).ToString() == "Pass") ? (false) : (true)),
                        reviewCommentsPresent = ((reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString() == "0") ? (false) : (true)),
                        notificationCommentsPresent = ((reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString() == "0") ? (false) : (true)),
                        notificationStatus = (reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString().ToLower()),
                        xId = reader.IsDBNull(reader.GetOrdinal("x_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("x_id")).ToString()),
                        inernalAdded = bool.Parse(reader.GetValue(reader.GetOrdinal("inernalAdded")).ToString()),
                        externalAdded = bool.Parse(reader.GetValue(reader.GetOrdinal("externalAdded")).ToString()),

                        missedItems = new List<CallMissedItem>()
                    });
                    var callMetaData = (new CallMetaData()
                    {
                        callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                        agentGroup = reader.GetValue(reader.GetOrdinal("agentGroup")).ToString(),
                        campaign = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                        agentName = reader.GetValue(reader.GetOrdinal("agentName")).ToString(),
                        sessionId = reader.GetValue(reader.GetOrdinal("sessionId")).ToString(),
                        profileId = reader.GetValue(reader.GetOrdinal("profileId")).ToString(),
                        prospectFirstName = reader.GetValue(reader.GetOrdinal("prospectFirstName")).ToString(),
                        prospectLastName = reader.GetValue(reader.GetOrdinal("prospectLastName")).ToString(),
                        prospectPhone = reader.GetValue(reader.GetOrdinal("prospectPhone")).ToString(),
                        prospectEmail = reader.GetValue(reader.GetOrdinal("prospectEmail")).ToString(),
                    });

                    callDetailsList.Add(new CallDetailV2()
                    {
                        metaData = callMetaData,
                        systemData = callSystemData
                    });

                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            List<CallMissedItem> MissedItemList = new List<CallMissedItem>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    try
                    {
                        MissedItemList.Add(new CallMissedItem()
                        {
                            itemDescription = reader.GetValue(reader.GetOrdinal("itemDescription")).ToString(),
                            position = float.Parse(reader.GetValue(reader.GetOrdinal("q_pos")).ToString()),
                            callId = int.Parse(reader.GetValue(reader.GetOrdinal("CallID")).ToString())
                        });
                    }
                    catch  { }
                }

                foreach (var item in callDetailsList)
                {
                    item.systemData.missedItems = new List<CallMissedItem>();
                    item.systemData.missedItems.AddRange(from v in MissedItemList where v.callId == item.systemData.callId select v);
                    item.callMissedItems = item.systemData.missedItems;
                }
            }

            CallDetailsResponseData CallDetailsLst = new CallDetailsResponseData();
            CallDetailsLst.itemsTotal = callDetailsList.Count;
            CallDetailsLst.calls = callDetailsList;

            return CallDetailsLst;
        }
        /// <summary>
        /// AddCallsToCalibration calibration layer
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string AddCallsToCalibration(EndPointData action)
        {


            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }

            var preparedLst = new StringBuilder();
            if (action.ids != null && (action.ids.Count > 0))
            {
                foreach (var value in action.ids)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }
            }

            try
            {
                using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
                {
                    sqlCon.Open();
                    SqlCommand reply = new SqlCommand(@"select appname, scorecard,scorecard_name, f_id from vwForm where f_id in (" + preparedLst.ToString().Trim(',') + ")", sqlCon);
                    reply.CommandTimeout = 60;
                    reply.CommandType = CommandType.Text;
                    SqlDataReader reader = reply.ExecuteReader();
                    var callsInfo = new List<CallsInfo>();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            Scorecard scorecard = new Scorecard()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString()
                            };
                            callsInfo.Add(new CallsInfo()
                            {
                                appname = reader.GetValue(reader.GetOrdinal("appname")).ToString(),
                                scorecard = scorecard,
                                id = int.Parse(reader.GetValue(reader.GetOrdinal("f_id")).ToString())
                            });
                        }
                    }
                    SqlCommand sqlCommandInsert = new SqlCommand();
                    sqlCommandInsert.Connection = sqlCon;
                    sqlCommandInsert.CommandTimeout = 60;
                    sqlCommandInsert.CommandType = CommandType.Text;
                    SqlDataReader insertReader;
                    string nextStep = "";
                    try
                    {
                        insertReader = sqlCommandInsert.ExecuteReader();
                    }
                    catch { }
                    string selected_by = "";
                    foreach (var info in callsInfo)
                    {
                        if (action.side == "external")//"external"//"internal"
                        {
                            selected_by = "Client";

                            sqlCommandInsert = new SqlCommand
                            (@"insert into cali_pending_client (bad_value, form_id, reviewer, review_type, week_ending, appname, assigned_to, cpc_who_added) select '"
                            + selected_by + " Selected','" + info.id +
                            @"',(select reviewer from form_score3 where id = " + info.id +
                            @"),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + info.appname +
                            @"', userapps.username,'" + userName +
                            @"' from userextrainfo join userapps on userextrainfo.username = userapps.username where user_role 
                                in ('Supervisor','Client','Manager', 'Client Calibrator') and user_scorecard = (select scorecard from vwForm where f_id = '" + info.id + "'); " +

                            @" insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, client_start, isrecal, sc_id) " +
                            @" select 'Client Selected','" + info.id + "',(select reviewer from form_score3 where id = " + info.id + "),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + info.appname + "','" + userName + "', dbo.getMTDate(),1,(select scorecard from vwForm where f_id = " + info.id + ") where 0 = (select count(*) from calibration_pending where form_id = " + info.id + " and isrecal = 1);  " +
                            @" update top (1) calibration_pending set client_start = dbo.getMTdate() where form_id = '" + info.id + "' and Client_Review_Completed is null; " +
                            @" select count(*) from calibration_pending where form_id = " + info.id + " and isrecal = 1;", sqlCon);

                            insertReader = sqlCommandInsert.ExecuteReader();

                        }
                        else if (action.side == "internal")
                        {
                            //var A = ("select count(*) from calibration_pending where form_id = " + info.id + " and isrecal = 0");

                            nextStep = ("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, client_start, isrecal, sc_id) select '" +
                                        selected_by + " Selected','" +
                                        info.id + "',(select reviewer from form_score3 where id = " + info.id + "),'" +
                                        selected_by +
                                        " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + info.appname + "','" + userName +
                                        "', dbo.getMTDate(),0,'" + info.scorecard.scorecardId + "'"
                                        + "       update calibration_pending set client_start = dbo.getMTDate() where form_id = " + info.id);


                            sqlCommandInsert = new SqlCommand(nextStep, sqlCon);
                            insertReader = sqlCommandInsert.ExecuteReader();
                            //{
                            //    var B = ("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, client_start, isrecal, sc_id) select '" + selected_by + " Selected','" + info.id + "',(select reviewer from form_score3 where id = " + info.id + "),'" + selected_by + " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + info.appname + "','" + userName + "', dbo.getMTDate(),0,'" + info.scorecard + "'");
                            //}                    
                            //var BQ = ("update calibration_pending set client_start = dbo.getMTDate() where form_id = " + info.id);
                        }

                    }
                    sqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "OK";
        }

        /// <summary>
        /// GetCalibrationQueue calibration layer
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public List<CalibrationsPendingInfo> GetCalibrationQueue(string appName)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = "getCaliQueue";
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@appname", appName);

                sqlComm.Connection = sqlCon;

                var calibrationsPendingInfoList = new List<CalibrationsPendingInfo>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            Scorecard scorecard = new Scorecard()
                            {
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString())
                            };
                            calibrationsPendingInfoList.Add(new CalibrationsPendingInfo
                            {
                                pendingCalibrations = int.Parse(reader.GetValue(reader.GetOrdinal("Pending_Calibs")).ToString()),
                                oldestCall = DateTime.Parse(reader.GetValue(reader.GetOrdinal("oldest_call")).ToString()),
                                pendingReviewTime = decimal.Parse(reader.GetValue(reader.GetOrdinal("pending_review_time")).ToString()),
                                scorecard = scorecard
                            });
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    };
                    return calibrationsPendingInfoList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        /// <summary>
        /// GetCalibrationCalls calibration layer
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        public CalibrationCallsInfo GetCalibrationCalls(int scorecardId)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "winnie";
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = "getClientCalibrationsSM";
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@show_completed", 1);

                sqlComm.Parameters.AddWithValue("@scorecardId", scorecardId);


                sqlComm.Connection = sqlCon;

                var calibrationsCalibratedInfoList = new List<CalibrationCalls>();
                var completedUserList = new List<CompletedUserList>();
                var calibrationsPendingInfoList = new List<CalibrationCalls>();

                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    var clientSidelst = new List<AssignedUserList>();
                    var ccSidelst = new List<AssignedUserList>();
                    while (reader.Read())
                    {
                        var a = new AssignedUserList
                        {
                            formId = int.Parse(reader.GetValue(reader.GetOrdinal("form_id")).ToString()),
                            processed = (reader.GetValue(reader.GetOrdinal("processed")).ToString()),
                            assigned = (reader.GetValue(reader.GetOrdinal("assigned")).ToString()),
                        };
                        clientSidelst.Add(a);
                    }

                    if (reader.NextResult())
                        while (reader.Read())
                        {
                            var a = new AssignedUserList
                            {
                                formId = int.Parse(reader.GetValue(reader.GetOrdinal("form_id")).ToString()),
                                processed = (reader.GetValue(reader.GetOrdinal("processed")).ToString()),
                                assigned = (reader.GetValue(reader.GetOrdinal("assigned")).ToString()),
                            };
                            ccSidelst.Add(a);
                        }

                    if (reader.NextResult())
                        while (reader.Read())
                        {
                            try
                            {

                                CalibrationStatus ccSide = new CalibrationStatus()
                                {
                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("cc_completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("cc_available")).ToString()),
                                };

                                CalibrationStatus clientSide = new CalibrationStatus()
                                {

                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("Client_Completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("client_available")).ToString()),
                                };
                                CalibrationStatus status = new CalibrationStatus()
                                {
                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("Real_Num_Completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("Real_Num_Reviews")).ToString()),

                                };
                                Scorecard scorecard = new Scorecard()
                                {
                                    scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                    scorecardName = reader.GetValue(reader.GetOrdinal("Short_Name")).ToString(),

                                };
                                calibrationsPendingInfoList.Add(new CalibrationCalls
                                {

                                    ccSide = ccSide,
                                    clientSide = clientSide,
                                    status = status,
                                    scorecard = scorecard,
                                    calibrationId = int.Parse(reader.GetValue(reader.GetOrdinal("Form_ID")).ToString()),
                                    dateAdded = DateTime.Parse(reader.GetValue(reader.GetOrdinal("DateAdded")).ToString()),
                                    weekEndDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("week_ending_date")).ToString()),
                                    phone = reader.GetValue(reader.GetOrdinal("Phone")).ToString(),
                                    callDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("call_date")).ToString()),
                                    callLength = float.Parse(reader.GetValue(reader.GetOrdinal("call_length")).ToString()),
                                    callType = reader.GetValue(reader.GetOrdinal("callType")).ToString(),
                                    ownedCall = bool.Parse(reader.GetValue(reader.GetOrdinal("owned_call")).ToString()),
                                    callId = int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString())
                                });
                            }

                            catch (Exception ex) { }
                        };
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                CalibrationStatus ccSide = new CalibrationStatus()
                                {
                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("cc_completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("cc_available")).ToString()),
                                };

                                CalibrationStatus clientSide = new CalibrationStatus()
                                {
                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("Client_Completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("client_available")).ToString()),
                                };
                                CalibrationStatus status = new CalibrationStatus()
                                {
                                    completed = int.Parse(reader.GetValue(reader.GetOrdinal("Real_Num_Completed")).ToString()),
                                    reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("Real_Num_Reviews")).ToString()),

                                };
                                Scorecard scorecard = new Scorecard()
                                {
                                    scorecardId = 1,
                                    scorecardName = reader.GetValue(reader.GetOrdinal("Short_Name")).ToString(),

                                };
                                completedUserList.Add(new CompletedUserList()
                                {
                                    formId = int.Parse(reader.GetValue(reader.GetOrdinal("Form_ID")).ToString()),
                                    completedBy = reader.GetValue(reader.GetOrdinal("who_processed")).ToString(),
                                    reviewTime = int.Parse(reader.GetValue(reader.GetOrdinal("review_time")).ToString()),
                                });
                                calibrationsCalibratedInfoList.Add(new CalibrationCalls
                                {
                                    dateAdded = DateTime.Parse(reader.GetValue(reader.GetOrdinal("DateAdded")).ToString()),
                                    ccSide = ccSide,
                                    clientSide = clientSide,
                                    status = status,
                                    scorecard = scorecard,
                                    calibrationId = int.Parse(reader.GetValue(reader.GetOrdinal("Form_ID")).ToString()),
                                    phone = reader.GetValue(reader.GetOrdinal("Phone")).ToString(),
                                    callDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("call_date")).ToString()),
                                    weekEndDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("week_ending_date")).ToString()),
                                    callLength = float.Parse(reader.GetValue(reader.GetOrdinal("call_length")).ToString()),
                                    callType = reader.GetValue(reader.GetOrdinal("callType")).ToString(),
                                    callId = int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString())

                                });
                            }
                            catch (Exception ex) { }
                        }
                    }

                    var calibrationsPendingInfoListGrouped = new List<CalibrationCalls>();
                    foreach (var cal in calibrationsPendingInfoList)
                    {
                        var caltmp = cal;
                        caltmp.ccSide.whoProcessed = (from c in ccSidelst where c.formId == caltmp.callId select c.processed).ToList();
                        caltmp.ccSide.assignedTo = (from c in ccSidelst where c.formId == caltmp.callId select c.assigned).ToList();
                        caltmp.clientSide.whoProcessed = (from c in clientSidelst where c.formId == caltmp.callId select c.processed).ToList();
                        caltmp.clientSide.assignedTo = (from c in clientSidelst where c.formId == caltmp.callId select c.assigned).ToList();

                        calibrationsPendingInfoListGrouped.Add(caltmp);
                        //continue;

                    }
                    var calibrationsCalibratedInfoListGrouped = new List<CalibrationCalls>();
                    foreach (var c in calibrationsCalibratedInfoList.Distinct())
                    {
                        var tmp = c;
                        tmp.completedUserList = (from a in completedUserList where a.formId == tmp.callId select a).ToList();
                        calibrationsCalibratedInfoListGrouped.Add(tmp);
                    }

                    return new CalibrationCallsInfo() { pending = calibrationsPendingInfoListGrouped, completed = calibrationsCalibratedInfoListGrouped };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }


        /// <summary>
        /// CompleteReview calibration layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CompleteReview(int id)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                SqlCommand reply = new SqlCommand(@"update calibration_pending set client_review_completed = dbo.getMTDate(), client_who_closed = '" +
                    HttpContext.Current.User.Identity.Name + "' where form_id = " + id +
                            "    delete from [cali_pending_client] where form_id = " + id + " and date_completed is null", sqlCon);
                reply.CommandTimeout = 60;
                reply.CommandType = CommandType.Text;
                SqlDataReader reader = reply.ExecuteReader();
                return "OK";
            }
        }

        /// <summary>
        /// DeleteReview calibration layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteReview(int id)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                SqlCommand reply = new SqlCommand(@"update calibration_pending set client_start = null where form_id = " + id +
                    "       delete from cali_pending_client where form_id =" + id, sqlCon);
                reply.CommandTimeout = 60;
                reply.CommandType = CommandType.Text;
                SqlDataReader reader = reply.ExecuteReader();
                return "OK";
            }
        }

    }
}