using ClosedXML.Excel;
using DAL.DataLayer;
using DAL.Export;
using DAL.GenericRepository;
using DAL.Models;
using Itenso.TimePeriod;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Code;
using WebApi.DataLayer;
using DAL.Models.CalibrationModels;
using DAL.Code;
using DAL.Extensions;
using WebApi.Models.CallCriteriaAPI;

namespace WebApi.Controllers
{
    //#if DEBUG

    //#else
    //    [Authorize]
    //#endif
    /// <summary>
    /// DashboardControllerv2_4Controller
    /// </summary>
    [RoutePrefix("v2.4")]

    [ApiExplorerSettings(IgnoreApi = false)]
    public class DashboardControllerv2_4Controller : ApiController
    {

        /// <summary>
        /// GetCallShortInfo
        /// </summary>
        /// <param name="callID"></param>
        /// <returns></returns>
        [Route("dashboard/GetCallShortInfo")]
        [HttpPost]
        [ResponseType(typeof(CallShortInfo))]
        public CallShortInfov2 GetCallShortInfo([FromBody]int callID)
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                //   SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[getCoachingQueueJsonV2]", userName);
                //Filter f = new Filter() { filters = filters.filters, range = filters.range };
                var sqlComm = new SqlCommand
                {
                    CommandText = "CallShortInfo1",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("CallID", callID);
                sqlComm.Parameters.AddWithValue("userName", userName);
                sqlComm.Connection = sqlCon;


                var coachingQueueLst = new List<CoachingQueue>();
                var cqcd = new List<CoachingQueueCallDetails>();
                var cq = new CoachingQueueResponceData();
                var callShortInfo = new CallShortInfov2();
                try
                {
                    sqlCon.Open();
                    var reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        var callSystemData = (new CallSystemData()
                        {
                            callId = int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                            callType = reader.GetValue(reader.GetOrdinal("calltype")).ToString(),
                            callReviewStatus = reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString(),
                            callAudioUrl = reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                            callAudioLength = reader.IsDBNull(reader.GetOrdinal("callAudioLength")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                            //float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                            websiteUrl = reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                            scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                            //int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),//
                            scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                            scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                            receivedDate = reader.IsDBNull(reader.GetOrdinal("receivedDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("receivedDate")),
                            //DateTime.Parse(reader.GetValue(reader.GetOrdinal("receivedDate")).ToString()),
                            reviewDate = reader.IsDBNull(reader.GetOrdinal("reviewDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("reviewDate")),
                            // DateTime.Parse(reader.GetValue(reader.GetOrdinal("reviewDate")).ToString()),
                            reviewerUserRole = reader.GetValue(reader.GetOrdinal("reviewerUserRole")).ToString(),
                            reviewerName = reader.GetValue(reader.GetOrdinal("reviewerName")).ToString(),
                            calibratorId = reader.GetValue(reader.GetOrdinal("calibratorId")).ToString(),
                            calibratorName = reader.GetValue(reader.GetOrdinal("calibratorName")).ToString(),
                            missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                            //int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                            agentScore = reader.IsDBNull(reader.GetOrdinal("agentScore")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                            // float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                            callFailed = ((reader.GetValue(reader.GetOrdinal("callFailed")).ToString() != "Pass")),
                            reviewCommentsPresent = ((reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString() != "0")),
                            notificationCommentsPresent = ((reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString() != "0")),
                            notificationStatus = (reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString().ToLower()),
                            isNotificationOwner = (reader.GetValue(reader.GetOrdinal("OwnedNotification")).ToString() == "1"),
                            badCallReason = reader.GetValue(reader.GetOrdinal("badCallReason")).ToString(),
                            scoreChanged = reader.IsDBNull(reader.GetOrdinal("scoreChanged")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scoreChanged")).ToString()),
                            audioStatus = reader.IsDBNull(reader.GetOrdinal("audioReady")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("audioReady")).ToString()),
                            missedItems = new List<CallMissedItem>()
                        });
                        var callMetaData = (new CallMetaData()
                        {
                            callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                            agentId = reader.GetValue(reader.GetOrdinal("agentId")).ToString(),
                            //DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
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
                        callShortInfo.systemData = callSystemData;
                        callShortInfo.metaData = callMetaData;

                    }

                    NotificationInfo notificationInfo = new NotificationInfo();

                    if (reader.NextResult())
                    {
                        notificationInfo.notificationComments = new List<NotificationComment>();
                        while (reader.Read())
                        {

                            var openedBy = new UserInformation();
                            try
                            {
                                openedBy.userData = new User()
                                {
                                    userId = reader.GetValue(reader.GetOrdinal("opened_by")).ToString(),
                                    userName = reader.GetValue(reader.GetOrdinal("opened_by")).ToString()
                                };
                                openedBy.userRole = new Role()
                                {
                                    userRoleId = reader.GetValue(reader.GetOrdinal("UserOpenedRole")).ToString(),
                                    userRoleName = reader.GetValue(reader.GetOrdinal("UserOpenedRole")).ToString()
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
                                    userId = reader.GetValue(reader.GetOrdinal("closed_by")).ToString(),
                                    userName = reader.GetValue(reader.GetOrdinal("closed_by")).ToString()
                                };
                                closedBy.userRole = new Role()
                                {
                                    userRoleId = reader.GetValue(reader.GetOrdinal("UserClosedRole")).ToString(),
                                    userRoleName = reader.GetValue(reader.GetOrdinal("UserClosedRole")).ToString()
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
                                    userRoleId = reader.GetValue(reader.GetOrdinal("role")).ToString(),
                                    userRoleName = reader.GetValue(reader.GetOrdinal("role")).ToString()
                                };
                            }
                            catch
                            {
                                // ignored
                            }

                            try
                            {
                                notificationInfo.notificationComments.Add(new NotificationComment()
                                {
                                    id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                    closedBy = closedBy,
                                    closedDate = !reader.IsDBNull(reader.GetOrdinal("date_closed")) ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("date_closed")) : null,
                                    notificationRole = role,
                                    openDate = !reader.IsDBNull(reader.GetOrdinal("date_created")) ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("date_created")) : null,
                                    openedBy = openedBy,
                                    text = reader.GetValue(reader.GetOrdinal("comment")).ToString()
                                });
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }

                    }
                    if (reader.NextResult())
                    {
                        notificationInfo.systemComments = new List<SystemComment>();
                        while (reader.Read())
                        {
                            var userInfo = new UserInformation();
                            try
                            {
                                userInfo.userData = new User()
                                {
                                    userId = reader.GetValue(reader.GetOrdinal("comment_who")).ToString(),
                                    userName = reader.GetValue(reader.GetOrdinal("comment_who")).ToString()
                                };
                                userInfo.userRole = new Role()
                                {
                                    userRoleId = reader.GetValue(reader.GetOrdinal("userRole")).ToString(),
                                    userRoleName = reader.GetValue(reader.GetOrdinal("userRole")).ToString()
                                };
                            }
                            catch
                            {
                                userInfo.userData = null;
                                userInfo.userRole = null;
                            }
                            try
                            {
                                notificationInfo.systemComments.Add(new SystemComment()
                                {
                                    id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                    user = userInfo,
                                    commentDate = reader.IsDBNull(reader.GetOrdinal("comment_date")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("comment_date")),
                                    text = reader.GetValue(reader.GetOrdinal("comment")).ToString(),
                                });
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        callShortInfo.notificationInfo = notificationInfo;

                    }
                    var questionDetails = new List<QuestionDetails_v2>();
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            try
                            {
                                var qd = new QuestionDetails_v2()
                                {
                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionID")).ToString()),
                                    questionShortName = reader.GetValue(reader.GetOrdinal("questionShortName")).ToString(),
                                    questionSectionName = (reader.GetValue(reader.GetOrdinal("questionSectionName")).ToString()),
                                    questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                    isRightAnswer = bool.Parse(reader.GetValue(reader.GetOrdinal("isRightAnswer")).ToString()),
                                    isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()),
                                    isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()),
                                };
                                qd.questionType = qd.questionType.ToLower();
                                if (!qd.isComposite)
                                {
                                    var sqa = new SimpleQuestionAnswer()
                                    {
                                        answerId = int.Parse(reader.GetValue(reader.GetOrdinal("answerId")).ToString()),
                                        answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                                        position = reader.IsDBNull(reader.GetOrdinal("position")) ? (double?)null : float.Parse(reader.GetValue(reader.GetOrdinal("position")).ToString()),
                                    };
                                    qd.simpleQuestionAnswer = sqa;
                                }
                                else
                                {
                                    var compositeAnswerComment = new List<CompositeAnswerComment>();
                                    qd.compositeQuestionAnswer = new CompositeQuestionAnswer();
                                    qd.compositeQuestionAnswer.comments = compositeAnswerComment;
                                    qd.compositeQuestionAnswer.customComment = reader.GetValue(reader.GetOrdinal("answerText")).ToString();
                                    qd.compositeQuestionAnswer.position = reader.IsDBNull(reader.GetOrdinal("position")) ? (double?)null : float.Parse(reader.GetValue(reader.GetOrdinal("position")).ToString());
                                }
                                questionDetails.Add(qd);

                            }
                            catch { }
                        }
                    }
                    var commentList = new List<SimpleAnswerComment>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var coment = new SimpleAnswerComment()
                                {
                                    questionID = int.Parse(reader.GetValue(reader.GetOrdinal("questionID")).ToString()),
                                    commentText = reader.GetValue(reader.GetOrdinal("commentText")).ToString()
                                };
                                commentList.Add(coment);
                            }
                            catch
                            {

                            }
                        }

                    }
                    List<CompositeAnswerComment> compositeAnswerComments = new List<CompositeAnswerComment>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var compositecomment = new CompositeAnswerComment()
                                {
                                    commentId = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                    commentText = reader.GetValue(reader.GetOrdinal("option_text")).ToString(),
                                    isChecked = (reader.GetValue(reader.GetOrdinal("IsChecked")).ToString() != "0"),
                                    position = reader.IsDBNull(reader.GetOrdinal("option_pos")) ? null : (int?)reader.GetValue(reader.GetOrdinal("option_pos")),
                                    questionID = int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                                };
                                compositeAnswerComments.Add(compositecomment);
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                    }

                    foreach (var qd in questionDetails)
                    {
                        if (qd.isComposite)
                            qd.compositeQuestionAnswer.comments = (from a in compositeAnswerComments where a.questionID == qd.questionId select a).ToList();
                    }

                    foreach (var qd in questionDetails)
                    {
                        if (qd.isComposite)
                        {
                            try
                            {
                                qd.compositeQuestionAnswer.customComment = (from a in commentList where a.questionID == qd.questionId select a.commentText).First();
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        else
                        {
                            try
                            {
                                qd.simpleQuestionAnswer.comments = (from a in commentList where a.questionID == qd.questionId select a).ToList();
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                    }
                    callShortInfo.callMissedItems = questionDetails;



                    try
                    {
                        callShortInfo.notificationInfo.assignedTo = (from a in callShortInfo.notificationInfo.notificationComments where a.closedDate == null select a.notificationRole).First();
                    }
                    catch
                    {
                        // ignored
                    }


                    if (callShortInfo.notificationInfo != null)
                    {
                        callShortInfo.notificationInfo.notificationComments.RemoveAll(a => a.closedDate == null);

                        if (callShortInfo.systemData.callReviewStatus != "bad call")
                        {
                            callShortInfo.notificationInfo.reassignOptions = new NotificationLayer().GetNotificationSteps(callID);
                        }
                        else
                        {
                            callShortInfo.notificationInfo.reassignOptions = new List<Role>();
                        }
                        callShortInfo.notificationInfo.canClose = new NotificationLayer().CanClose(callID);


                        callShortInfo.systemData.callAudioUrl = new AudioHelper().GetAudioFileNameByFId(callID);

                        if (callShortInfo.notificationInfo.assignedTo != null && callShortInfo.systemData.notificationStatus == "")
                        {
                            callShortInfo.systemData.notificationStatus = "notification";
                        }

                        callShortInfo.notificationInfo.notificationStatus = callShortInfo.systemData.notificationStatus;
                    };
                    return callShortInfo;

                }

                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// GetCallDetails
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetCallDetails")]
        [HttpPost]
        [ResponseType(typeof(CallDetails))]
#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
        public CallDetails GetCallDetails([FromBody]CallDetailsFilter filters)
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
        {
            var userName = HttpContext.Current.GetUserName();
            #region filters
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            var dbconn = new Entities.CC_ProdEntities();



            SqlCommand sqlComm = new SqlCommand();
            UserLayer ul = new UserLayer();
            var uf = ul.GetUserForsedFilters(userName, sqlCon);

            filters.filters.agents.AddRange(uf.agent);
            filters.filters.QAs.AddRange(uf.qa);
            filters.filters.teamLeads.AddRange(uf.teamLead);
            filters.filters.groups.AddRange(uf.group);
            filters.filters.campaigns.AddRange(uf.campaign);



            sqlComm.Connection = sqlCon;
            if (filters.filters.pendingOnly == false)
                sqlComm.CommandText = "[getDetailDataArrayJson_mitemsV4]";
            else
                sqlComm.CommandText = "[GetPendingCalls]";
            sqlComm.CommandType = CommandType.StoredProcedure;

            #region Params
            sqlComm.Parameters.AddWithValue("@userName", userName);
            if (filters.filters.scorecards != null && (filters.filters.scorecards.Count > 0))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.scorecards)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }

                sqlComm.Parameters.AddWithValue("@scorecardIDs", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.campaigns != null && ((filters.filters.campaigns.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.campaigns)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@campaignIDs", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.groups != null && (filters.filters.groups.Count > 0))
            {

                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.groups)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@groupIDs", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.agents != null && ((filters.filters.agents.Count > 0)))
            {
                var preparedLst = new StringBuilder();

                foreach (var value in filters.filters.agents)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }


                sqlComm.Parameters.AddWithValue("@agentIDs", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.QAs != null && ((filters.filters.QAs.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.QAs)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@qaIDs", preparedLst.ToString().Trim(','));
            }

            if (filters.filters.missedItems != null && ((filters.filters.missedItems.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.missedItems)
                {
                    preparedLst.Append(((value + ",")));
                }

                sqlComm.Parameters.AddWithValue("@missedItemsIDs", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.teamLeads != null && ((filters.filters.teamLeads.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.teamLeads)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@teamLeadIDs", preparedLst.ToString().Trim(','));
            }

            if (filters.sorting?.sortBy != null && filters.sorting.sortOrder != null && filters.sorting.sortBy != "" && filters.sorting.sortOrder != "")
            {

                sqlComm.Parameters.AddWithValue("@OrderByColumn", filters.sorting.sortBy);
                sqlComm.Parameters.AddWithValue("@sortOrder", (filters.sorting.sortOrder != "desc"));
            }

            if (filters.filters.commentIds != null && ((filters.filters.commentIds.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.commentIds)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }

                sqlComm.Parameters.AddWithValue("@answerComment", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.answerIds != null && ((filters.filters.answerIds.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.answerIds)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }

                sqlComm.Parameters.AddWithValue("@answerIds", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.compositeCommentIds != null && ((filters.filters.compositeCommentIds.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.compositeCommentIds)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }

                sqlComm.Parameters.AddWithValue("@compositeCommentIds", preparedLst.ToString().Trim(','));
            }
            if (filters.filters.compositeAnswerIds != null && ((filters.filters.compositeAnswerIds.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.compositeAnswerIds)
                {
                    preparedLst.Append(("'" + (value + "',")));
                }

                sqlComm.Parameters.AddWithValue("@compositeAnswerIds", preparedLst.ToString().Trim(','));
            }

            if (filters.search?.columns != null && filters.search.text != null && filters.search.text != "" && filters.search.columns.Count > 0)
            {
                var preparedLst = new StringBuilder();

                sqlComm.Parameters.AddWithValue("@searchstr", filters.search.text);
                foreach (var value in filters.search.columns)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@searchColumn", preparedLst.ToString());
            }

            if (filters.filters.conversionFilters != null && ((filters.filters.conversionFilters.Count > 0)))
            {
                var preparedLst = new StringBuilder();
                foreach (var value in filters.filters.conversionFilters)
                {
                    preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                }

                sqlComm.Parameters.AddWithValue("@conversionFilters", preparedLst.ToString().Trim(','));
            }

            try
            {
                sqlComm.Parameters.AddWithValue("@badCallOnly", filters.filters.isConversion ? false : filters.filters.badCallsOnly);
            }
            catch { }
            sqlComm.Parameters.AddWithValue("@failed", filters.filters.isConversion ? false : filters.filters.failedOnly);
            sqlComm.Parameters.AddWithValue("@filterByReviewDate", filters.filters.isConversion ? false : filters.filters.filterByReviewDate);
            sqlComm.Parameters.AddWithValue("@passedOnly", filters.filters.isConversion ? false : filters.filters.passedOnly);
            sqlComm.Parameters.AddWithValue("@pagerows", filters.paging.pagerows);
            sqlComm.Parameters.AddWithValue("@pagenum", filters.paging.pagenum);
            sqlComm.Parameters.AddWithValue("@reviewType", filters.filters.reviewType);
            sqlComm.Parameters.AddWithValue("@missedBy", filters.filters.missedBy);
            if (filters.filters.isConversion) sqlComm.Parameters.AddWithValue("@isConversion", 1);
            //sqlComm.Parameters.AddWithValue("@missedBy", filters.filters.missedBy);

            if (((filters.range == null) || (filters.range.start.Length < 4)))
            {
                sqlComm.Parameters.AddWithValue("@Start", DateTime.Now.AddDays(-14));
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@Start", DateTime.Parse(filters.range.start));
            }

            if (((filters.range.end == null) || (filters.range.end.Length < 4)))
            {
                sqlComm.Parameters.AddWithValue("@end", DateTime.Now);
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@end", DateTime.Parse(filters.range.end));
            }


            #endregion

            sqlComm.CommandTimeout = int.MaxValue;

            #endregion

            sqlCon.Open();
            SqlDataReader reader = sqlComm.ExecuteReader();

            var callDetailsList = CallDetails.Create(reader);

            //if (filters.sorting?.sortBy != null && filters.sorting.sortOrder != null && filters.sorting.sortBy != "" && filters.sorting.sortOrder != "")
            //{
            //    if(filters.sorting.sortOrder == "1")
            //    {
            //        var calls = CallDetailsLst.calls.OrderBy(x => x.systemData.missedItemsCount).ToList();
            //        CallDetailsLst.calls = calls;
            //    }
            //    if (filters.sorting.sortOrder == "0")
            //    {
            //        var calls = CallDetailsLst.calls.OrderByDescending(x => x.systemData.missedItemsCount).ToList();
            //        CallDetailsLst.calls = calls;
            //    }
            //}

            return callDetailsList;
        }
        /// <summary>
        /// GetCallMissedItems
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/getCallMissedItems")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionDetails_v2>))]
        public async Task<List<QuestionDetails_v2>> GetCallMissedItems([FromBody]MissedItemRequest filters)
        {
            #region filters
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            var dbconn = new Entities.CC_ProdEntities();
            var userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlCon;
            sqlComm.CommandText = "[getCallMissedItems]";

            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@userName", userName);
            sqlComm.Parameters.AddWithValue("@callID", filters.callID);
            sqlComm.Parameters.AddWithValue("@missedBy", filters.missedBy);
            sqlComm.CommandTimeout = int.MaxValue;
            #endregion
            List<CallDetail> callDetailsList = new List<CallDetail>();
            await sqlCon.OpenAsync();
            SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            var questionDetails = new List<QuestionDetails_v2>();
            while (await reader.ReadAsync())
            {
                try
                {
                    var qd = new QuestionDetails_v2()
                    {
                        callId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("callId")),
                        questionId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("questionID")),
                        questionShortName = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("questionShortName")),
                        questionSectionName = await (reader.GetFieldValueAsync<string>(reader.GetOrdinal("questionSectionName"))),
                        questionType = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("questionType")),
                        isRightAnswer = await (reader.GetFieldValueAsync<bool>(reader.GetOrdinal("isRightAnswer"))),
                        isComposite = await (reader.GetFieldValueAsync<bool>(reader.GetOrdinal("hasTemplate"))),
                        isLinked = await (reader.GetFieldValueAsync<bool>(reader.GetOrdinal("isLinked"))),
                    };
                    qd.questionType = qd.questionType.ToLower();
                    if (!qd.isComposite)
                    {
                        var sqa = new SimpleQuestionAnswer()
                        {

                            answerId = await reader.GetFieldValueAsync<int>(reader.GetOrdinal("answerId")),
                            answerText = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("answerText")),
                            position = (float?)(await reader.IsDBNullAsync(reader.GetOrdinal("position")) ?
                            (double?)null :
                            await reader.GetFieldValueAsync<double>(reader.GetOrdinal("position"))),
                        };
                        qd.simpleQuestionAnswer = sqa;
                    }
                    else
                    {
                        var compositeAnswerComment = new List<CompositeAnswerComment>();
                        qd.compositeQuestionAnswer =
                            new CompositeQuestionAnswer
                            {
                                comments = compositeAnswerComment,
                                customComment = await reader.GetFieldValueAsync<string>(reader.GetOrdinal("answerText")),
                                position = (float?)(await reader.IsDBNullAsync(reader.GetOrdinal("position")) ?
                                (double?)null :
                                await reader.GetFieldValueAsync<double>(reader.GetOrdinal("position"))),

                            };
                    }
                    questionDetails.Add(qd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return questionDetails;
        }


        //[Route("dashboard/getCallMissedItemsSync")]
        //[HttpPost]
        //[ResponseType(typeof(List<QuestionDetails_v2>))]
        //public    List<QuestionDetails_v2> GetCallMissedItemsSync([FromBody]MissedItemRequest filters)
        //{
        //    #region filters
        //    var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
        //    var dbconn = new Entities.Entities();
        //    var userName = "";
        //    if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
        //    {
        //        userName = "test321";// HttpContext.Current.User.Identity.Name;
        //    }
        //    else
        //    {
        //        userName = HttpContext.Current.User.Identity.Name;
        //    }
        //    SqlCommand sqlComm = new SqlCommand();
        //    sqlComm.Connection = sqlCon;
        //    sqlComm.CommandText = "[getCallMissedItems]";

        //    sqlComm.CommandType = CommandType.StoredProcedure;
        //    sqlComm.Parameters.AddWithValue("@userName", userName);
        //    sqlComm.Parameters.AddWithValue("@callID", filters.callID);
        //    sqlComm.Parameters.AddWithValue("@missedBy", filters.missedBy);
        //    sqlComm.CommandTimeout = int.MaxValue;
        //    #endregion
        //    List<CallDetail> callDetailsList = new List<CallDetail>();
        //      sqlCon.Open();
        //    SqlDataReader reader = sqlComm.ExecuteReader();
        //    var questionDetails = new List<QuestionDetails_v2>();
        //    while (  reader.Read ())
        //    {
        //        try
        //        {
        //            var qd = new QuestionDetails_v2()
        //            {
        //                callId =   reader.GetFieldValue <int>(reader.GetOrdinal("callId")),
        //                questionId =   reader.GetFieldValue <int>(reader.GetOrdinal("questionID")),
        //                questionShortName =   reader.GetFieldValue <string>(reader.GetOrdinal("questionShortName")),
        //                questionSectionName =   (reader.GetFieldValue<string>(reader.GetOrdinal("questionSectionName"))),
        //                questionType =   reader.GetFieldValue<string>(reader.GetOrdinal("questionType")),
        //                isRightAnswer =   (reader.GetFieldValue<bool>(reader.GetOrdinal("isRightAnswer"))),
        //                isComposite =   (reader.GetFieldValue<bool>(reader.GetOrdinal("hasTemplate"))),
        //                isLinked =   (reader.GetFieldValue<bool>(reader.GetOrdinal("isLinked"))),
        //            };
        //            qd.questionType = qd.questionType.ToLower();
        //            if (!qd.isComposite)
        //            {
        //                var sqa = new SimpleQuestionAnswer()
        //                {

        //                    answerId =   reader.GetFieldValue<int>(reader.GetOrdinal("answerId")),
        //                    answerText =   reader.GetFieldValue<string>(reader.GetOrdinal("answerText")),
        //                    position = (float?)(  reader.IsDBNull(reader.GetOrdinal("position")) ?
        //                    (double?)null :
        //                      reader.GetFieldValue<double>(reader.GetOrdinal("position"))),
        //                };
        //                qd.simpleQuestionAnswer = sqa;
        //            }
        //            else
        //            {
        //                var compositeAnswerComment = new List<CompositeAnswerComment>();
        //                qd.compositeQuestionAnswer =
        //                    new CompositeQuestionAnswer
        //                    {
        //                        comments = compositeAnswerComment,
        //                        customComment =   reader.GetFieldValue<string>(reader.GetOrdinal("answerText")),
        //                        position = (float?)(  reader.IsDBNull(reader.GetOrdinal("position")) ?
        //                        (double?)null :
        //                          reader.GetFieldValue<double>(reader.GetOrdinal("position"))),

        //                    };
        //            }
        //            questionDetails.Add(qd);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    return questionDetails;
        //}





        /// <summary>
        /// SearchCalls
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [Route("dashboard/SearchCalls")]
        [HttpPost]
        [ResponseType(typeof(CallDetailsResponseData))]
        public CallDetailsResponseData SearchCalls([FromBody]string searchText)
        {
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            var dbconn = new WebApi.Entities.CC_ProdEntities();
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            SqlCommand sqlComm = new SqlCommand
            {
                Connection = sqlCon,
                CommandText = "[ApiCallSearch]",
                CommandType = CommandType.StoredProcedure
            };
            sqlComm.Parameters.AddWithValue("@userName", userName);
            if (!string.IsNullOrEmpty(searchText))
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
                        //int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                        callType = reader.GetValue(reader.GetOrdinal("calltype")).ToString(),
                        callReviewStatus = reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString(),
                        callAudioUrl = reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                        callAudioLength = reader.IsDBNull(reader.GetOrdinal("callAudioLength")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                        //float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                        websiteUrl = reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                        scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                        //int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),//
                        scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                        scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                        receivedDate = reader.IsDBNull(reader.GetOrdinal("receivedDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("receivedDate")),
                        //DateTime.Parse(reader.GetValue(reader.GetOrdinal("receivedDate")).ToString()),
                        reviewDate = reader.IsDBNull(reader.GetOrdinal("reviewDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("reviewDate")),
                        // DateTime.Parse(reader.GetValue(reader.GetOrdinal("reviewDate")).ToString()),
                        reviewerUserRole = reader.GetValue(reader.GetOrdinal("reviewerUserRole")).ToString(),
                        reviewerName = reader.GetValue(reader.GetOrdinal("reviewerName")).ToString(),
                        calibratorId = reader.GetValue(reader.GetOrdinal("calibratorId")).ToString(),
                        calibratorName = reader.GetValue(reader.GetOrdinal("calibratorName")).ToString(),
                        missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                        //int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                        agentScore = reader.IsDBNull(reader.GetOrdinal("agentScore")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                        // float.Parse(reader.GetValue(reader.GetOrdinal("agentScore")).ToString()),
                        callFailed = ((reader.GetValue(reader.GetOrdinal("callFailed")).ToString() != "Pass")),
                        reviewCommentsPresent = ((reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString() != "0")),
                        notificationCommentsPresent = ((reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString() != "0")),
                        notificationStatus = (reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString().ToLower()),
                        xId = reader.IsDBNull(reader.GetOrdinal("x_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("x_id")).ToString()),

                        // badCallReason = reader.GetValue(reader.GetOrdinal("badCallReason")).ToString(),
                        missedItems = new List<CallMissedItem>(),
                        //wasEdited =reader.GetOrdinal("wasEdited").ToString()=="0"
                        scoreChanged = int.Parse(reader.GetValue(reader.GetOrdinal("scoreChanged")).ToString())
                        //Boolean.Parse(reader.GetValue(reader.GetOrdinal("wasEdited")).ToString())
                    });
                    var callMetaData = (new CallMetaData()
                    {
                        callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                        //DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
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
                catch (Exception ex) { throw ex; }

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

            CallDetailsResponseData CallDetailsLst = new CallDetailsResponseData
            {
                itemsTotal = callDetailsList.Count,
                calls = callDetailsList
            };

            return CallDetailsLst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetTopMissedItems")]
        [HttpPost]
        [ResponseType(typeof(TopMissedItemsResponseData))]
        public TopMissedItemsResponseData GetTopMissedItems([FromBody]AverageFilter filters)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "Kashif Khan";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            Filter f = new Filter() { filters = filters.filters, range = filters.range };
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getTopMissedItemsJson_v2]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
                sqlComm.CommandTimeout = int.MaxValue;

                PageFiltersData pageFiltersData = new PageFiltersData();
                sqlCon.Open();

                TopMissedItemsResponseData topMissed = new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            topMissed.missedItems.Add(new MissedItem()
                            {
                                questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                questionShortName = (reader.GetValue(reader.GetOrdinal("questionShortName")).ToString()),
                                scorecardName = (reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()),
                                totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missedCalls")).ToString()),
                                questionSectionName = reader.GetValue(reader.GetOrdinal("sectionName")).ToString(),
                                isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()),
                                isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()),
                                questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                comparedMissedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedMissedCalls")).ToString()),
                                comparedTotalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedTotalCalls")).ToString()),
                            });
                        }
                        catch { }
                    }
                    reader.NextResult();
                    List<MissedItemAgentInfo> lst = new List<MissedItemAgentInfo>();
                    while (reader.Read())
                    {
                        try
                        {

                            lst.Add(new MissedItemAgentInfo()
                            {

                                questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                name = reader.GetValue(reader.GetOrdinal("agent")).ToString(),
                                totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total_calls")).ToString()),
                                missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_missed")).ToString()),
                            });
                        }
                        catch
                        {

                        }
                    }
                    foreach (var item in topMissed.missedItems)
                    {
                        item.top3Agents = new List<MissedItemAgentInfo>();
                        item.top3Agents.AddRange((from v in lst where v.questionId == item.questionId select v).ToList());
                    }

                    return topMissed;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetTopQAMissedItems")]
        [HttpPost]
        [ResponseType(typeof(TopMissedItemsResponseData))]
        public TopMissedItemsResponseData GetTopQAMissedItems([FromBody]AverageFilter filters)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            Filter f = new Filter() { filters = filters.filters, range = filters.range };

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                if (filters.filters.badCallsOnly == false)
                {
#pragma warning disable CS0436 // Type conflicts with imported type
                    SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getTopqaMissedItemsJson_v2]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type

                    sqlComm.Connection = sqlCon;

                    PageFiltersData pageFiltersData = new PageFiltersData();
                    sqlCon.Open();

                    TopMissedItemsResponseData topMissed = new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
                    try
                    {
                        SqlDataReader reader = sqlComm.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                topMissed.missedItems.Add(new MissedItem()
                                {
                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    questionShortName = (reader.GetValue(reader.GetOrdinal("questionShortName")).ToString()),
                                    scorecardName = (reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missedCalls")).ToString()),
                                    questionSectionName = reader.GetValue(reader.GetOrdinal("sectionName")).ToString(),
                                    isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()),
                                    isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()),
                                    questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                    comparedMissedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedMissedCalls")).ToString()),
                                    comparedTotalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedTotalCalls")).ToString()),
                                });
                            }
                            catch { }
                        }
                        reader.NextResult();
                        List<MissedItemAgentInfo> lst = new List<MissedItemAgentInfo>();
                        while (reader.Read())
                        {
                            try
                            {
                                lst.Add(new MissedItemAgentInfo()
                                {

                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    name = reader.GetValue(reader.GetOrdinal("reviewer")).ToString(),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total_calls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_missed")).ToString()),
                                });
                            }
                            catch { }
                        }
                        foreach (var item in topMissed.missedItems)
                        {
                            item.top3Agents = new List<MissedItemAgentInfo>();
                            item.top3Agents.AddRange((from v in lst where v.questionId == item.questionId select v).ToList());
                        }

                        return topMissed;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                var result = new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
                return result;
            }
        }

        /// <summary>
        /// GetTopCalibratorMissedItems
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetTopCalibratorMissedItems")]
        [HttpPost]
        [ResponseType(typeof(TopMissedItemsResponseData))]
        public TopMissedItemsResponseData GetTopCalibratorMissedItems([FromBody]AverageFilter filters)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "leolapaz";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            Filter f = new Filter() { filters = filters.filters, range = filters.range };

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                if (filters.filters.badCallsOnly == false)
                {
#pragma warning disable CS0436 // Type conflicts with imported type
                    SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[GetTopCalibratorMissedItems]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type
                    sqlComm.CommandTimeout = int.MaxValue;
                    sqlComm.Connection = sqlCon;

                    PageFiltersData pageFiltersData = new PageFiltersData();
                    sqlCon.Open();

                    TopMissedItemsResponseData topMissed = new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
                    try
                    {
                        SqlDataReader reader = sqlComm.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                topMissed.missedItems.Add(new MissedItem()
                                {
                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    questionShortName = (reader.GetValue(reader.GetOrdinal("questionShortName")).ToString()),
                                    scorecardName = (reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missedCalls")).ToString()),
                                    questionSectionName = reader.GetValue(reader.GetOrdinal("sectionName")).ToString(),
                                    isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()),
                                    isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()),
                                    questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                    comparedMissedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedMissedCalls")).ToString()),
                                    comparedTotalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedTotalCalls")).ToString()),
                                });
                            }
                            catch { }
                        }
                        reader.NextResult();
                        List<MissedItemAgentInfo> lst = new List<MissedItemAgentInfo>();
                        while (reader.Read())
                        {
                            try
                            {
                                lst.Add(new MissedItemAgentInfo()
                                {

                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    name = reader.GetValue(reader.GetOrdinal("reviewer")).ToString(),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total_calls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_missed")).ToString()),
                                });
                            }
                            catch { }
                        }
                        foreach (var item in topMissed.missedItems)
                        {
                            item.top3Agents = new List<MissedItemAgentInfo>();
                            item.top3Agents.AddRange((from v in lst where v.questionId == item.questionId select v).ToList());
                        }

                        return topMissed;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            return new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
        }

        /// <summary>
        /// GetAvgScore
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetAvgScore")]
        [HttpPost]
        [ResponseType(typeof(List<AvarageDayScore>))]
        public AvgScoreResponseData GetAvgScore([FromBody]AverageFilter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                if (filters.filters.badCallsOnly == true)
                {
                    return new AvgScoreResponseData();
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[GetAgentAvgsJsonV2]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
                PageFiltersData pageFiltersData = new PageFiltersData();
                sqlCon.Open();
                var avg = new AvgScoreResponseData();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    avg.averageScores = new List<AvarageDayScore>();
                    // Dim adapter = New SqlDataAdapter(reader)
                    // adapter.Fill(ds)
                    while (reader.Read())
                    {
                        try
                        {
                            avg.averageScores.Add(new AvarageDayScore()
                            {
                                averageScore = int.Parse(reader.GetValue(reader.GetOrdinal("avg_score")).ToString()),
                                date = reader.IsDBNull(reader.GetOrdinal("call_date")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("call_date")),
                                //DateTime.Parse(reader.GetValue(reader.GetOrdinal("call_date")).ToString())
                            });
                        }
                        catch { }
                    }
                    return avg;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
        }
        /// <summary>
        /// GetAvgCallTime
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetAvgCallTime")]
        [HttpPost]
        [ResponseType(typeof(List<AvarageCallTime>))]
        public dynamic GetAvgCallTime([FromBody]AverageFilter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "nataliaadmin";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[GetAvgsCallTime]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
                sqlCon.Open();
                var avg = new List<AvarageCallTime>();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            ScorecardInfo sc = new ScorecardInfo()
                            {
                                scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecard")) ? (int)0 : int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                scorecardName = reader.IsDBNull(reader.GetOrdinal("scorecardName")) ? "" : reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                            };
                            avg.Add(new AvarageCallTime()
                            {
                                scorecardInfo = sc,
                                dayCallTime = reader.IsDBNull(reader.GetOrdinal("dailyAvgRev")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("dailyAvgRev")).ToString()),
                                mtdCallTime = reader.IsDBNull(reader.GetOrdinal("monthlyAvgRev")) ? (int)0 : int.Parse(reader.GetValue(reader.GetOrdinal("monthlyAvgRev")).ToString())
                            });
                        }
                        catch { }
                    }
                    return avg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        /// <summary>
        /// GetAvailableDashboardFilter
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(FiltersFormData))]
        [Route("dashboard/GetAvailableDashboardFilter")]
        [HttpPost]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public FiltersFormData GetAvailableDashboardFilter([FromBody]AverageFilter filters)
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
#if IS_CLB
                    userName = "chrisbrunn";
#else
                    userName = "Val Shijaku";
#endif

                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAllDashboardFilters_v5]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type


                sqlComm.Connection = sqlCon;

                FiltersFormData pageFiltersData = new FiltersFormData();
                sqlCon.Open();
                try
                {
                    SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable tblAgentgroup = ds.Tables[0];
                    DataTable tblAgentCampain = ds.Tables[1];
                    DataTable tblAgentAgents = ds.Tables[2];
                    DataTable tblAgentScorecard = ds.Tables[3];
                    DataTable tblAgentQA = filters.filters.isConversion ? new DataTable() : ds.Tables[4];
                    DataTable tblTeamLeadsItems = filters.filters.isConversion ? new DataTable() : ds.Tables[5];

                    DataTable tblRangeCount = ds.Tables[6];
                    List<FilterGroupValue> AgentGrouplLst = new List<FilterGroupValue>();
                    List<FilterCampainValue> AgentCampainLst = new List<FilterCampainValue>();
                    List<FilterAgentValue> AgentAgentsLst = new List<FilterAgentValue>();
                    List<FilterScorecardValue> AgentScorecardLst = new List<FilterScorecardValue>();
                    List<FilterQAValue> AgentQALst = new List<FilterQAValue>();
                    List<FilterTeamLeadValue> teamLeads = new List<FilterTeamLeadValue>();
                    RangeCalls rangeCalls = new RangeCalls();
                    if (!HttpContext.Current.User.IsInRole("Agent"))
                        foreach (DataRow row in tblAgentgroup.Rows)
                        {
                            AgentGrouplLst.Add(new FilterGroupValue()
                            {
                                name = row[0].ToString(),
                                id = row[0].ToString(),
                                count = int.Parse(row[1].ToString()),
                                top3Agents = null
                            });
                        }
                    pageFiltersData.groups = AgentGrouplLst;

                    foreach (DataRow row in tblAgentCampain.Rows)
                    {
                        AgentCampainLst.Add(new FilterCampainValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString())
                        });
                    }
                    pageFiltersData.campaigns = AgentCampainLst;
                    foreach (DataRow row in tblAgentAgents.Rows)
                    {
                        AgentAgentsLst.Add(new FilterAgentValue()
                        {

                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString()),
                            agentGroup = null
                        });
                    }
                    pageFiltersData.agents = AgentAgentsLst;
                    foreach (DataRow row in tblAgentScorecard.Rows)
                    {
                        try
                        {
                            int _failScore = 0;
                            int.TryParse(row[3].ToString(), out _failScore);
                            AgentScorecardLst.Add(new FilterScorecardValue()
                            {
                                id = int.Parse(row[0].ToString()),
                                name = row[2].ToString(),
                                count = int.Parse(row[1].ToString()),
                                failScore = _failScore,
                            });
                        }
                        catch (Exception ex1)
                        {
                            throw ex1;
                        }

                    }
                    pageFiltersData.scorecards = AgentScorecardLst;

                    foreach (DataRow row in tblAgentQA.Rows)
                    {
                        AgentQALst.Add(new FilterQAValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString()),
                            qaTeam = null
                        });
                    }

                    pageFiltersData.QAs = AgentQALst;

                    foreach (DataRow row in tblTeamLeadsItems.Rows)
                    {
                        teamLeads.Add(new FilterTeamLeadValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString()),
                            top3QAs = null
                        });
                    }

                    pageFiltersData.teamLeads = teamLeads;

                    if (tblRangeCount.Rows.Count > 0)
                    {
                        rangeCalls.total = int.Parse(tblRangeCount.Rows[0][0].ToString());
                        rangeCalls.filtered = int.Parse(tblRangeCount.Rows[0][1].ToString());
                    }
                    pageFiltersData.rangeCalls = rangeCalls;
                    pageFiltersData.badCallsOnly = filters.filters.badCallsOnly;

                    return pageFiltersData;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        /// <summary>
        /// GetCalendarCounts
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(FiltersFormData))]
        [Route("dashboard/GetCalendarCounts")]
        [HttpPost]
        public List<DayCalls> GetCalendarCounts([FromBody]AverageFilter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[GetCalendarCounts]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                FiltersFormData pageFiltersData = new FiltersFormData();
                sqlCon.Open();
                try
                {
                    SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);


                    List<DayCalls> dayCalls = new List<DayCalls>();
                    RangeCalls rangeCalls = new RangeCalls();
                    //Dim AgentMissedItemsLst As List(Of PageFiltersData.FilterValue) = New List(Of PageFiltersData.FilterValue)
                    DataTable tblDayCalls = ds.Tables[0];

                    foreach (DataRow row in tblDayCalls.Rows)
                    {
                        try
                        {
                            dayCalls.Add(new DayCalls()
                            {
                                total = int.Parse(row[0].ToString()),
                                filtered = int.Parse(row[1].ToString()),
                                day = DateTime.Parse(row[2].ToString()),
                            });
                        }
                        catch { }
                    }
                    pageFiltersData.dayCalls = dayCalls;


                    return dayCalls;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        /// <summary>
        /// GetAgregatedStatistics
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(GetAgregatedStatisticsResponseData))]
        [Route("dashboard/GetAgregatedStatistics")]
        [HttpPost]
        public GetAgregatedStatisticsResponseData GetAgregatedStatistics([FromBody]AverageFilter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAgregatedStatisticsV3]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                PageFiltersData pageFiltersData = new PageFiltersData();

                var avg = new GetAgregatedStatisticsResponseData();

                sqlCon.Open();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    avg = new GetAgregatedStatisticsResponseData();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        avg.rangeStats = new AgregatedStatistics()
                        {
                            totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCals")).ToString()),
                            totalFailedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("failedCals")).ToString()),
                            totalCallsSeconds = decimal.Parse(reader.GetValue(reader.GetOrdinal("totalCallsSeconds")).ToString()),
                            totalAgents = int.Parse(reader.GetValue(reader.GetOrdinal("totalAgent")).ToString()),
                            totalBadCalls = int.Parse(reader.GetValue(reader.GetOrdinal("badCalls")).ToString()),
                            avgAgentScore = reader.IsDBNull(reader.GetOrdinal("avarageScore")) ? 0 : decimal.Parse(reader.GetValue(reader.GetOrdinal("avarageScore")).ToString())

                        };
                    }
                    if (reader.NextResult())
                    {
                        reader.Read();
                        avg.rangeStats.totalPending = int.Parse(reader.GetValue(reader.GetOrdinal("PendingCallsCount")).ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    f.range = filters.comparison;
#pragma warning disable CS0436 // Type conflicts with imported type
                    sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAgregatedStatisticsV3]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                    sqlComm.Connection = sqlCon;

                    SqlDataReader reader = sqlComm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        avg.comparisonStats = new AgregatedStatistics();

                        avg.comparisonStats.totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCals")).ToString());
                        avg.comparisonStats.totalFailedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("failedCals")).ToString());
                        avg.comparisonStats.totalCallsSeconds = decimal.Parse(reader.GetValue(reader.GetOrdinal("totalCallsSeconds")).ToString());
                        avg.comparisonStats.totalAgents = int.Parse(reader.GetValue(reader.GetOrdinal("totalAgent")).ToString());
                        avg.comparisonStats.totalBadCalls = int.Parse(reader.GetValue(reader.GetOrdinal("badCalls")).ToString());
                        avg.comparisonStats.avgAgentScore = decimal.Parse(reader.GetValue(reader.GetOrdinal("avarageScore")).ToString());
                        avg.comparisonStats.totalPending = 0;
                        reader.Close();
                    }
                    return avg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Get's data for Agent Ranking Module
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(AgentRankingResponseData))]
        [Route("dashboard/GetAgentRanking")]
        [HttpPost]
        public AgentRankingResponseData GetAgentRanking([FromBody]AverageFilter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "ruthnickerson";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAgentRankingJson_v2]", userName, filters.comparison);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
                var aRankingResponseData = new AgentRankingResponseData();



                List<AgentMissedPoint> agentRankingInfolst = new List<AgentMissedPoint>();
                List<Agent> agentRankinglst = new List<Agent>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            agentRankingInfolst.Add(new AgentMissedPoint
                            {
                                agentId = reader.GetValue(reader.GetOrdinal("agent")).ToString(),
                                questionShortName = reader.GetValue(reader.GetOrdinal("q_short_name")).ToString(),
                                missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missed")).ToString()),
                                totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total")).ToString()),
                                isComposite = int.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()) == 1,
                                isLinked = int.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()) == 1,
                                questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                            });
                        }
                        catch { }
                    };

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var temp_ranking = new Agent()
                                {
                                    id = reader.GetValue(reader.GetOrdinal("agentID")).ToString(),
                                    name = reader.GetValue(reader.GetOrdinal("AgentName")).ToString(),
                                    groupNames = new List<string>(),
                                    averageScore = reader.IsDBNull(reader.GetOrdinal("averageScore")) ? (decimal?)null : decimal.Parse(reader.GetValue(reader.GetOrdinal("averageScore")).ToString()),
                                    previousAverageScore = reader.IsDBNull(reader.GetOrdinal("previousAverageScore")) ? (decimal?)null : decimal.Parse(reader.GetValue(reader.GetOrdinal("previousAverageScore")).ToString()),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                    totalBadCalls = reader.IsDBNull(reader.GetOrdinal("totalBadCalls")) ? (int?)null : (int?)reader.GetValue(reader.GetOrdinal("totalBadCalls")),
                                    earliestCallDate = reader.IsDBNull(reader.GetOrdinal("earlier")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("earlier")),
                                    // DateTime.Parse(reader.GetValue(reader.GetOrdinal("earlier")).ToString()),
                                };
                                temp_ranking.top3MissedPoints = (from val in agentRankingInfolst where val.agentId.Trim().Equals(temp_ranking.id.Trim()) select val).ToList();
                                agentRankinglst.Add(temp_ranking);
                            }
                            catch (Exception ex) { throw ex; }
                        }
                    }
                    List<UserGroupInfo> ugi = new List<UserGroupInfo>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                ugi.Add(new UserGroupInfo()
                                {
                                    groupname = reader.GetValue(reader.GetOrdinal("user_group")).ToString(),
                                    username = reader.GetValue(reader.GetOrdinal("Agent")).ToString()
                                });
                            }
                            catch
                            {
                            }
                        }
                    }

                    foreach (var a in agentRankinglst)
                    {
                        a.groupNames = (from val in ugi where a.name.Equals(val.username) select val.groupname).ToList();
                    }
                    //


                    aRankingResponseData.agents = agentRankinglst;
                    return aRankingResponseData;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Geting export agent ranking
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetExportAgentRanking")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetExportAgentRanking([FromBody]AverageFilter filters)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "nataliaadmin";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      AgentRankingCode agentRanking = new AgentRankingCode();
                      agentRanking.AgentRankingExport(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok();
        }







        /// <summary>
        /// GetCoachingQueue
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetCoachingQueue")]
        [HttpPost]
        [ResponseType(typeof(CoachingQueueResponceData))]
        public CoachingQueueResponceData GetCoachingQueue([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";
                    // userName = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                //   SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[getCoachingQueueJsonV2]", userName);
                //Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[getCoachingQueueJsonv2]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type
                sqlComm.Connection = sqlCon;


                List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
                List<CoachingQueueCallDetails> cqcd = new List<CoachingQueueCallDetails>();
                CoachingQueueResponceData cq = new CoachingQueueResponceData();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            try
                            {



                                var CallsystemData = new CallSystemData()
                                {
                                    callId = int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                                    callType = reader.IsDBNull(reader.GetOrdinal("callType")) ? "" : reader.GetValue(reader.GetOrdinal("callType")).ToString(),
                                    callReviewStatus = reader.IsDBNull(reader.GetOrdinal("callReviewStatus")) ? "" : reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString(),
                                    callAudioUrl = reader.IsDBNull(reader.GetOrdinal("callAudioUrl")) ? "" : reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                                    callAudioLength = 0,// float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                                    websiteUrl = reader.IsDBNull(reader.GetOrdinal("websiteUrl")) ? "" : reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                                    agentScore = reader.IsDBNull(reader.GetOrdinal("total_score")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("total_score")).ToString()),
                                    missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                                    notificationId = reader.IsDBNull(reader.GetOrdinal("NotificationID")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("NotificationID")).ToString()),
                                    ////not  reader.GetValue(reader.GetOrdinal("notificationStep")).ToString(),
                                    //reader.GetValue(reader.GetOrdinal("form_id")).ToString(),
                                    //reader.GetValue(reader.GetOrdinal("first_error")).ToString(),
                                    reviewCommentsPresent = (reader.IsDBNull(reader.GetOrdinal("reviewCommentsPresent")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString())),
                                    notificationCommentsPresent = (reader.IsDBNull(reader.GetOrdinal("notificationCommentsPresent")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString())),
                                    isNotificationOwner = (reader.IsDBNull(reader.GetOrdinal("OwnedNotification")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("OwnedNotification")).ToString())),
                                    notificationStatus = reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString().ToLower(),
                                    assignedToRole = reader.GetValue(reader.GetOrdinal("AssignedToRole")).ToString(),
                                    scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (double?)null : float.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                                    wasEdited = (reader.IsDBNull(reader.GetOrdinal("wasEdit")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("wasEdit")).ToString())),
                                    scorecardInfo = new ScorecardInfo()
                                    {
                                        scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                        scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                                    }

                                };
                                // details.systemData.callAudioUrl = null;// new AudioHelper().GetAudioFileName((int)details.systemData.callId);
                                //cqcd.Add(details);
                                var callMetaData = (new CallMetaData()
                                {
                                    //agentId = reader.GetValue(reader.GetOrdinal("agentId")).ToString(),
                                    //callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                                    //agentGroup = reader.GetValue(reader.GetOrdinal("agentGroup")).ToString(),
                                    campaign = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                                    //agentName = reader.GetValue(reader.GetOrdinal("agentName")).ToString(),
                                    //sessionId = reader.GetValue(reader.GetOrdinal("sessionId")).ToString(),
                                    //profileId = reader.GetValue(reader.GetOrdinal("profileId")).ToString(),
                                    //prospectFirstName = reader.GetValue(reader.GetOrdinal("prospectFirstName")).ToString(),
                                    //prospectLastName = reader.GetValue(reader.GetOrdinal("prospectLastName")).ToString(),
                                    prospectPhone = reader.GetValue(reader.GetOrdinal("prospectPhone")).ToString()
                                    //prospectEmail = reader.GetValue(reader.GetOrdinal("prospectEmail")).ToString(),
                                });
                                CoachingQueueCallDetails details = new CoachingQueueCallDetails()
                                {
                                    metaDataAgentName = reader.IsDBNull(reader.GetOrdinal("agentName")) ? "" : reader.GetValue(reader.GetOrdinal("agentName")).ToString(),
                                    metaDataCallDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                                    //DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
                                    metaData = callMetaData,
                                    systemData = CallsystemData
                                };
                                cqcd.Add(details);

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        try
                        {
                            if (reader.NextResult())
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                var dynamicTable = table.ToDynamic();
                                foreach (var call in cqcd)
                                {
                                    call.customData = dynamicTable.Where(m => m.callId == call.systemData.callId).First();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        cq.calls = cqcd;
                        cq.totalNotification = cqcd.Count();
                        return cq;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //return new List<CoachingQueueCallDetails>();
                }
                catch (Exception ex) { throw ex; }
            }
        }
        /// <summary>
        /// SendNotification
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Route("dashboard/SendNotification")]
        [HttpPost]
        [ResponseType(typeof(IHttpActionResult))]
        public IHttpActionResult SendNotification([FromBody]NotificationAction action)
        {
            int notificationId = 0;

            string noti_notes = action.text;
            int form_id = action.callId;
            bool isDisputeComplete = true;


            string sql = "";
            string role = "";

            string userName = HttpContext.Current.User.Identity.Name;
            if (userName == "")
            {
                userName = "test321";
            }

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                SqlCommand reply = new SqlCommand(@"select top 1 username, user_role from [dbo].[UserExtraInfo] where username='" + userName + "'", sqlCon);
                reply.CommandTimeout = 60;
                reply.CommandType = CommandType.Text;
                SqlDataReader reader = reply.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        role = reader.GetValue(reader.GetOrdinal("user_role")).ToString();
                    }
                }


                reply = new SqlCommand(@"select  * from form_notifications  where form_Id= '" + action.callId + "'and date_closed is null and close_reason is null", sqlCon);
                reply.CommandTimeout = 60;
                reply.CommandType = CommandType.Text;
                reader = reply.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        try
                        {
                            notificationId = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString());
                        }
                        catch { }
                    }
                }

                string @from = "";
                reply = new SqlCommand(@"select role from vwFN where fn_ID = '" + notificationId + "'", sqlCon)
                {
                    CommandTimeout = 60,
                    CommandType = CommandType.Text
                };
                reader = reply.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        @from = reader.GetValue(reader.GetOrdinal("role")).ToString();
                    }

                }
                //add dispute 
                if (notificationId == 0 && action.action.ToLower() != "assign" && action.action.ToLower() != "close")
                {
                    sql = "insert into form_notifications (date_created,date_closed,closed_by,close_reason, comment, form_id, role) select dbo.getMTDate(), dbo.getMTDate(), '" + userName + "',  'Update',   @new_comments, " + form_id + ",(Select user_role from userextrainfo  with(nolock) where username = '" + userName + "'); ";
                }
                else if (action.action.ToLower() != "comment")
                {
                    sql = "update form_notifications set date_closed = dbo.getMTDate() ,closed_by = '" + userName + "',close_reason = 'Updated', comment = @new_comments where id = " + notificationId + ";";
                }

                if (action.assignToRole != null && action.action.ToLower() == "assign")
                {
                    switch (action.assignToRole.userRoleName)
                    {
                        case "Supervisor":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Supervisor',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Agent":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Agent',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Calibrator":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Calibrator',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "QA":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Team Lead":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA Lead',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Account Manager":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Admin',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Manager":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Manager',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Center Manager":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Manager',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Admin":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Admin',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;
                        case "Client":
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Client',  dbo.getMTDate(), " + form_id + ", '" + userName + "';";
                            isDisputeComplete = false;
                            break;

                    }
                }
                else if (action.action == "comment")
                {
                    sql = "insert into form_notifications (date_created,date_closed,closed_by,close_reason, comment, form_id, role) select dbo.getMTDate(), dbo.getMTDate(), '" + userName + "',  'Update',   @new_comments, " + form_id + ",(Select user_role from userextrainfo  with(nolock) where username = '" + userName + "'); ";
                }

                if (isDisputeComplete == false)
                {
                    sql += "insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" + userName + "', dbo.getMTDate(), 'Notification moved from <strong>" + @from + "</strong> to <strong>" + action.assignToRole.userRoleName + "</strong>'," + form_id + ", 'Call';";
                }

                if (sql == "") return Ok();
                reply = new SqlCommand(sql, sqlCon) { CommandTimeout = 60 };
                reply.Parameters.AddWithValue("new_comments", noti_notes.ToString());
                reply.ExecuteNonQuery();
                sqlCon.Close();
                sqlCon.Dispose();
                return Ok();
            }
        }


        /// <summary>
        /// GetGroupPerformance
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<GroupPerformance>))]
        [Route("dashboard/GetGroupPerformance")]
        [HttpPost]
        public List<GroupPerformance> GetGroupPerformance([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "getGroupPerfAPI", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                var gpl = new List<GroupPerformance>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            PeriodPerformance period = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("avg_score")).ToString())
                            };
                            PeriodPerformance prviousPeriod = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("prev_num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("prev_avg_score")).ToString())
                            };
                            GroupInfo groupInfo = new GroupInfo() { id = reader.GetValue(reader.GetOrdinal("agent_group")).ToString(), name = reader.GetValue(reader.GetOrdinal("agent_group")).ToString() };
                            gpl.Add(new GroupPerformance
                            {
                                groupInfo = groupInfo,
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                currentPeriod = period,
                                previousPeriod = prviousPeriod
                            });
                        }
                        catch
                        {

                        }
                    };
                    return gpl;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// GetCampaignPerformance
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<CampaignPerformance>))]
        [Route("dashboard/GetCampaignPerformance")]
        [HttpPost]
        public List<CampaignPerformance> GetCampaignPerformance([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268 && HttpContext.Current.Request.UrlReferrer.Port == 51268 && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "getCampaignPerfApi", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                var gpl = new List<CampaignPerformance>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            PeriodPerformance period = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("avg_score")).ToString())
                            };
                            PeriodPerformance prviousPeriod = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("prev_num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("prev_avg_score")).ToString())
                            };
                            Campaign campaign = new Campaign()
                            {
                                id = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                                name = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                            };
                            gpl.Add(new CampaignPerformance
                            {
                                campaignInfo = campaign,
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                currentPeriod = period,
                                previousPeriod = prviousPeriod
                            });
                        }
                        catch { }
                    };
                    return gpl;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// GetCallsLeft
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<CallsLeft>))]
        [Route("dashboard/GetCallsLeft")]
        [HttpPost]
        public List<CallsLeft> GetCallsLeft([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268 && HttpContext.Current.Request.UrlReferrer.Port == 51268 && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "GetCallsLeftList", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
                var callsLeftLst = new List<CallsLeft>();
                var pendingCalls = new List<PendingCall>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            ScorecardInfo sc = new ScorecardInfo()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                            };
                            CallsLeft callsLeft = new CallsLeft()
                            {
                                badCalls = int.Parse(reader.GetValue(reader.GetOrdinal("badCalls")).ToString()),
                                pending = int.Parse(reader.GetValue(reader.GetOrdinal("pending")).ToString()),
                                pendingNotReady = int.Parse(reader.GetValue(reader.GetOrdinal("pending_not_ready")).ToString()),
                                pendingReady = int.Parse(reader.GetValue(reader.GetOrdinal("pending_ready")).ToString()),
                                reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("reviewed")).ToString()),
                                callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                                //DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),


                                scorecard = sc,
                                pendingCalls = new List<PendingCall>()
                            };
                            callsLeftLst.Add(callsLeft);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                PendingCall sc = new PendingCall()
                                {
                                    scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                    callDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
                                    receiveDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("receiveDate")).ToString()),
                                };
                                pendingCalls.Add(sc);
                            }
                            catch { }
                        }

                    }
                    foreach (var call in callsLeftLst)
                    {
                        call.pendingCalls = (from a in pendingCalls where a.scorecardId == call.scorecard.scorecardId && a.callDate == call.callDate select a).ToList();
                    }

                    return callsLeftLst;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// GetMyPayInfo
        /// </summary>
        /// <param name="weekEnd"></param>
        /// <returns></returns>
        [ResponseType(typeof(MyPay))]
        [Route("dashboard/GetMyPayInfo")]
        [HttpPost]
        public MyPay GetMyPayInfo([FromBody]string weekEnd)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "adrianlapaz";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                //Filter f = new Filter() { filters = filters.filters, range = filters.range };
                string sql = "getPaymentInfo";
                SqlCommand sqlComm;

                sqlComm = new SqlCommand(sql, sqlCon) { CommandTimeout = 60 };
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlComm.Parameters.AddWithValue("@weekEnd", weekEnd);
                sqlComm.CommandType = CommandType.StoredProcedure;
                MyPay myPay = new MyPay();

                var callsLeftLst = new List<CallsLeft>();
                var pendingCalls = new List<PendingCall>();

                var qaPaymentInfo = new List<PaymentInfo>();
                var calibratorPaymentInfo = new List<PaymentInfo>();
                var notificationPaymentInfo = new List<PaymentInfo>();

                List<ScorecardPaymentInfo> niWeekInfo = new List<ScorecardPaymentInfo>();
                List<ScorecardPaymentInfo> qaweekInfo = new List<ScorecardPaymentInfo>();
                List<ScorecardPaymentInfo> finalInfo = new List<ScorecardPaymentInfo>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        myPay.startDate = DateTime.Parse(reader.GetValue(0).ToString());
                        myPay.weeks = new List<List<ScorecardPaymentInfo>>();
                    }
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            try
                            {
                                var pinfo = new PaymentInfo()
                                {
                                    totalCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("callTime")).ToString()),
                                    totalReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("reviewtime")).ToString()),
                                    periodEnding = DateTime.Parse(reader.GetValue(reader.GetOrdinal("Week_ending_date")).ToString()),
                                    baseRate = float.Parse(reader.GetValue(reader.GetOrdinal("base")).ToString()),
                                    score = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("calibrationScore")).ToString()), 2),
                                    disputeCost = float.Parse(reader.GetValue(reader.GetOrdinal("dispute_cost")).ToString()),
                                    disputeCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_disputes")).ToString()),
                                    totalBadCallReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_reviewtime")).ToString()),
                                    totalBadCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_call_lenght")).ToString()),
                                    percentChange = float.Parse(reader.GetValue(reader.GetOrdinal("percent_change")).ToString()),
                                    calibrationCount = float.Parse(reader.GetValue(reader.GetOrdinal("calibrationsCount")).ToString()),
                                    startDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("startDate")).ToString()),
                                    payType = reader.GetValue(reader.GetOrdinal("pay_type")).ToString(),
                                    qaPay = reader.GetValue(reader.GetOrdinal("qa_pay")).ToString(),
                                    numberCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_calls")).ToString()),
                                    totalPay = 0,
                                    adjustedRate = 0,
                                    callSpeed = 0,
                                };
                                ScorecardInfo scorecardInfo = new ScorecardInfo()
                                {
                                    scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                    scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()

                                };
                                if (scorecardInfo.scorecardId == 491)
                                {

                                }
                                ScorecardPaymentInfo spinf = new ScorecardPaymentInfo() { scorecard = scorecardInfo, qaPaymentInfo = pinfo, weekEnd = pinfo.periodEnding };
                                qaweekInfo.Add(spinf);

                                pinfo.baseRate = (float)Math.Round((decimal)(1.6f + 0.05f * new DateDiff(pinfo.startDate, pinfo.periodEnding).Months), 8);

                                pinfo.baseRate += (new DateDiff(pinfo.startDate, pinfo.periodEnding).Days < 0) ? .05f : 0.0f;

                                if (pinfo.baseRate > 3)
                                {
                                    pinfo.baseRate = 3;
                                }


                                pinfo.disputeCost = pinfo.baseRate * pinfo.disputeCost;

                                //pinfo.disputeCost = pinfo.baseRate * .2f;

                                pinfo.callSpeed = (new DateDiff(pinfo.startDate, pinfo.periodEnding).Days > 14) ? (float)Math.Round(((pinfo.totalCallTime - pinfo.totalBadCallTime) / 3600) / ((pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) / 3600) * 100, 8) : 100.0f;


                                if (float.IsNaN(pinfo.callSpeed))
                                {
                                    pinfo.callSpeed = 100;
                                }

                                pinfo.paymentRate = 100;

                                float capped_speed = pinfo.callSpeed;

                                if (capped_speed > 180 && (new DateDiff(new DateTime(2018, 9, 14, 0, 0, 0), pinfo.periodEnding).Days > 0))
                                    capped_speed = 180;

                                if (pinfo.callSpeed > 100)
                                {
                                    pinfo.paymentRate = (float)Math.Round(((capped_speed - 100) / 2 + 100) / 100, 8);
                                }
                                else
                                {
                                    pinfo.paymentRate = (float)Math.Round(capped_speed / 100, 8);
                                }
                                if (pinfo.calibrationCount != 0)
                                {
                                    try
                                    {
                                        pinfo.adjustedRate = (float)Math.Round((decimal)(pinfo.baseRate * (pinfo.paymentRate) * (1 + pinfo.percentChange / 100)), 2);
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        pinfo.adjustedRate = (float)Math.Round((decimal)(pinfo.baseRate * (pinfo.paymentRate)), 2);
                                    }
                                    catch
                                    {
                                    }
                                }




                                switch (pinfo.payType)
                                {
                                    case "Per Item":
                                        if ((float)Convert.ToSingle(pinfo.qaPay) > 0)
                                        {
                                            pinfo.totalPay = pinfo.baseRate / Convert.ToSingle(pinfo.qaPay) * (int)pinfo.numberCalls - (pinfo.disputeCost * pinfo.disputeCount);
                                            pinfo.adjustedRate = pinfo.baseRate;
                                            pinfo.callSpeed = 100;

                                        }
                                        else
                                        {
                                            pinfo.totalPay = Convert.ToSingle(pinfo.qaPay) * (int)pinfo.numberCalls - (pinfo.disputeCost * pinfo.disputeCount);
                                            pinfo.adjustedRate = pinfo.baseRate;
                                            pinfo.callSpeed = 100;

                                        }

                                        break;
                                    case "Per Call Time":
                                        pinfo.totalPay = (pinfo.adjustedRate * (pinfo.totalCallTime / 3600) - (pinfo.disputeCost * pinfo.disputeCount));
                                        break;
                                    default:
                                        pinfo.totalPay = (pinfo.adjustedRate * (pinfo.totalReviewTime / 3600) - (pinfo.disputeCost * pinfo.disputeCount));
                                        break;
                                }



                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var pinfo = new PaymentInfo()
                            {
                                totalCallTime = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("callTime")).ToString()), 2),
                                totalReviewTime = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("reviewtime")).ToString()), 2),
                                periodEnding = DateTime.Parse(reader.GetValue(reader.GetOrdinal("Week_ending_date")).ToString()),
                                baseRate = float.Parse(reader.GetValue(reader.GetOrdinal("base")).ToString()),
                                score = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("calibrationScore")).ToString()), 2),
                                disputeCost = float.Parse(reader.GetValue(reader.GetOrdinal("dispute_cost")).ToString()),
                                disputeCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_disputes")).ToString()),
                                percentChange = float.Parse(reader.GetValue(reader.GetOrdinal("percent_change")).ToString()),
                                //totalBadCallReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_reviewtime")).ToString()),
                                //totalBadCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_call_lenght")).ToString()),

                                complitedNotification = int.Parse(reader.GetValue(reader.GetOrdinal("notificationComplited")).ToString()),
                                totalPay = 0,
                                adjustedRate = 0,
                                callSpeed = 0,
                            };
                            //float  efficiency = (pinfo.totalCallTime - pinfo.totalBadCallTime) / (pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) * 100f;
                            pinfo.callSpeed = (pinfo.totalCallTime - pinfo.totalBadCallTime) / (pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) * 100;
                            if (pinfo.totalCallTime != 0)
                            {
                                if (pinfo.score > 0)
                                {
                                    pinfo.adjustedRate = (float)(pinfo.baseRate * (1 + pinfo.percentChange / 100));
                                }
                                else
                                {
                                    pinfo.adjustedRate = pinfo.baseRate;
                                }
                            }
                            else
                            {
                                pinfo.adjustedRate = pinfo.baseRate;
                            }
                            pinfo.totalPay = (float)Math.Round((decimal)(pinfo.adjustedRate * pinfo.totalReviewTime / 3600 + pinfo.complitedNotification * .4), 2);
                            pinfo.adjustedRate = (float)Math.Round(pinfo.adjustedRate, 2);
                            qaPaymentInfo.Add(pinfo);

                            ScorecardInfo scorecardInfo = new ScorecardInfo()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()
                            };
                            ScorecardPaymentInfo spinf = new ScorecardPaymentInfo() { scorecard = scorecardInfo, calibratorPaymentInfo = pinfo, weekEnd = pinfo.periodEnding };

                            if (qaweekInfo.Count == 0)
                            {
                                finalInfo.Add(spinf);
                            }
                            else
                            {

                                ScorecardPaymentInfo merged = new ScorecardPaymentInfo()
                                {
                                    calibratorPaymentInfo = spinf.calibratorPaymentInfo,
                                    scorecard = spinf.scorecard,
                                    weekEnd = spinf.weekEnd,
                                    qaPaymentInfo = (from qai in qaweekInfo
                                                     where
                              (qai.scorecard.scorecardId == spinf.scorecard.scorecardId && qai.weekEnd == spinf.weekEnd)
                                                     select qai.qaPaymentInfo).FirstOrDefault()

                                };
                                finalInfo.Add(merged);
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        myPay.weekEnds = new List<string>();
                        while (reader.Read())
                        {
                            myPay.weekEnds.Add(reader.GetFieldValue<string>(reader.GetOrdinal("endDate")));
                        }
                    }

                    finalInfo.AddRange(
                            (
                                from info in qaweekInfo
                                where !finalInfo.Any(f => f.scorecard.scorecardId == info.scorecard.scorecardId && f.weekEnd == info.weekEnd)
                                select info
                            ).ToList()
                        );


                    try
                    {
                        myPay.weeks = (from f in finalInfo group f by f.weekEnd into g select g.ToList()).ToList();
                    }
                    catch
                    {
                        return new MyPay();
                    }
                    return myPay;

                }
                catch
                {
                    return new MyPay();
                }
            }
        }
        /// <summary>
        /// SendPendingCallToStartOfQueue
        /// </summary>
        /// <param name="xId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IHttpActionResult))]
        [Route("dashboard/SendPendingCallToStartOfQueue")]
        [HttpPost]
        public IHttpActionResult SendPendingCallToStartOfQueue([FromBody]int xId)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
                {
                    sqlCon.Open();
                    string userName = "";
                    if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268
                        && HttpContext.Current.Request.UrlReferrer.Port == 51268 && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                    {
                        userName = "test321";// HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = HttpContext.Current.User.Identity.Name;
                    }
                    string sql = "update xcc_report_new set sort_order = 0 where  id=@id";
                    SqlCommand reply;

                    reply = new SqlCommand(sql, sqlCon);
                    reply.CommandTimeout = 60;
                    reply.Parameters.AddWithValue("id", xId);
                    reply.ExecuteNonQuery();
                    sqlCon.Close();
                    sqlCon.Dispose();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetQuestionInfo
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        [System.Web.Mvc.AsyncTimeout(duration: 10000)] //The timeout value in milliseconds.
        [System.Web.Mvc.HandleError(ExceptionType = typeof(TimeoutException),
                                   View = "TimeoutError")]
        [Route("dashboard/GetQuestionInfo")]
        [HttpPost]
        [ResponseType(typeof(QuestionInfo1))]
        public QuestionInfo1 GetQuestionInfo([FromBody]Filter filters, int qid)
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

#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetQuestionInfo]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type
                sqlComm.CommandTimeout = int.MaxValue;
                sqlComm.Parameters.AddWithValue("@qid", qid);
                sqlComm.Connection = sqlCon;
                var agregatedAnswer = new List<AgregatedAnswer>();
                List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
                List<CoachingQueueCallDetails> cqcd = new List<CoachingQueueCallDetails>();
                CoachingQueueResponceData cq = new CoachingQueueResponceData();
                var callShortInfo = new CallShortInfov2();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    QuestionInfo questionInfo = new QuestionInfo();


                    while (reader.Read())
                    {
                        questionInfo.questionName = reader.GetValue(reader.GetOrdinal("questionShortName")).ToString();
                        questionInfo.sectionName = reader.GetValue(reader.GetOrdinal("questionSectionName")).ToString();
                        questionInfo.isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString());
                        questionInfo.isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString());
                        questionInfo.questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString();
                        questionInfo.questionID = int.Parse(reader.GetValue(reader.GetOrdinal("qid")).ToString());

                    }
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            questionInfo.scorecard = new Scorecard()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()
                            };
                        }
                    }
                    var questionStatistic = new QuestionStatistic
                    {
                        answerList = new List<AnswerCommentStatList>()
                    };
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var qs = new AnswerCommentStatList()
                                {
                                    answerId = reader.GetFieldValue<int>(reader.GetOrdinal("answerId")),
                                    total = int.Parse(reader.GetValue(reader.GetOrdinal("Count")).ToString()),
                                    answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                                    // answerComent = (reader.GetValue(reader.GetOrdinal("otherAnswerText")).ToString()),
                                    isRightAnswer = bool.Parse(reader.GetValue(reader.GetOrdinal("isRightAnswer")).ToString()),
                                    commentText = (reader.GetValue(reader.GetOrdinal("commentText")).ToString()),
                                    answerCommentId = reader.GetFieldValue<int?>(reader.GetOrdinal("answerCommentsId"))
                                };
                                questionStatistic.answerList.Add(qs);
                            }
                            catch { }
                        }
                    }


                    var comments = new List<QuestionInfoAnswerComment>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {

                                var qs = new QuestionInfoAnswerComment()
                                {
                                    total = questionStatistic.answerList.Sum(a => a.total) - int.Parse(reader.GetValue(reader.GetOrdinal("count")).ToString()),
                                    commentText = reader.GetValue(reader.GetOrdinal("optionText")).ToString(),
                                    commentId = int.Parse(reader.GetValue(reader.GetOrdinal("option_id")).ToString()),
                                };
                                if (qs.total != 0)
                                {
                                    comments.Add(qs);
                                }

                            }
                            catch { }
                        }
                    }
                    if (reader.NextResult())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                agregatedAnswer.Add(new AgregatedAnswer()
                                {
                                    answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                                    count = int.Parse(reader.GetValue(reader.GetOrdinal("Count")).ToString()),
                                    answerId = int.Parse(reader.GetValue(reader.GetOrdinal("answerid")).ToString())
                                });
                            }
                        }
                        catch { }
                    }
                    questionInfo.simpleQuestionStat = questionStatistic;
                    QuestionInfo1 questionInfo1 = new QuestionInfo1();
                    questionInfo1 = new QuestionInfo1()
                    {
                        questionID = questionInfo.questionID,
                        questionName = questionInfo.questionName,
                        isComposite = questionInfo.isComposite,
                        isLinked = questionInfo.isLinked,
                        questionType = questionInfo.questionType,
                        sectionName = questionInfo.sectionName,
                        scorecard = questionInfo.scorecard,
                        total = questionInfo.total
                    };
                    questionInfo1.compositeQuestionStat = new CompositeQuestionStats();

                    if (questionInfo1.isComposite)
                    {
                        // questionInfo1.compositeQuestionStat = new CompositeQuestionStats();
                        questionInfo1.compositeQuestionStat.totalRight = questionInfo.simpleQuestionStat.answerList.Count(g => g.isRightAnswer);
                        questionInfo1.compositeQuestionStat.comments = (from a in questionInfo.simpleQuestionStat.answerList where a.isRightAnswer == true group a by new { a.commentText } into g select new QuestionInfoAnswerComment() { commentText = g.Key.commentText, total = g.Sum(a => a.total) }).ToList();
                        //questionInfo1.compositeQuestionStat.comments = questionInfo.simpleQuestionStat.answerList.Where(x=>x.isRightAnswer == true).Select(q=>new QuestionInfoAnswerComment{commentId = q.answerCommentId,commentText = q.commentText,total = agregatedAnswer.Sum(a => a.count) - q.total }).ToList();
                        var rightAnswerInfo =
                        (from a in questionInfo.simpleQuestionStat.answerList
                         where a.isRightAnswer
                         group a by new { a.answerText, a.total, a.answerId } into g
                         select new CompositeAnswerInfo() { answerText = g.Key.answerText, answerId = g.Key.answerId, totalCustomComments = g.Sum(_ => _.total) }).ToList();
                        if (rightAnswerInfo.Count > 0)
                        {
                            questionInfo1.compositeQuestionStat.rightAnswerInfo = new CompositeAnswerInfo()
                            {
                                answerText = rightAnswerInfo.First().answerText,
                                answerId = rightAnswerInfo.First().answerId,
                                totalCustomComments = rightAnswerInfo.Sum(a => a.totalCustomComments)
                            };
                        }
                        else
                        {
                            questionInfo1.compositeQuestionStat.rightAnswerInfo = new CompositeAnswerInfo();
                        }
                        try
                        {
                            questionInfo1.compositeQuestionStat.totalRight = questionInfo1.compositeQuestionStat.comments.Sum(a => a.total);
                        }
                        catch
                        {
                            questionInfo1.compositeQuestionStat.totalRight = questionInfo1.compositeQuestionStat.rightAnswerInfo.totalCustomComments;
                        }

                        var wrongAnswerInfo = (from a in questionInfo.simpleQuestionStat.answerList
                                               where !a.isRightAnswer
                                               group a by new { a.answerText, a.total, a.answerId } into g
                                               select new CompositeAnswerInfo() { answerText = g.Key.answerText, answerId = g.Key.answerId, totalCustomComments = g.Sum(_ => _.total) }).ToList();
                        if (wrongAnswerInfo.Count > 0)
                        {
                            questionInfo1.compositeQuestionStat.wrongAnswerInfo =
                                new CompositeAnswerInfo()
                                {
                                    answerText = wrongAnswerInfo.First().answerText,
                                    answerId = wrongAnswerInfo.First().answerId,
                                    totalCustomComments = wrongAnswerInfo.Sum(a => a.totalCustomComments)
                                };
                            questionInfo1.compositeQuestionStat.comments = comments;
                        }
                        else
                        {
                            questionInfo1.compositeQuestionStat.wrongAnswerInfo = new CompositeAnswerInfo();
                        }
                    }
                    else
                    {
                        questionInfo1.simpleQuestionStat = new SimpleQuestionStat();
                        if (questionInfo.simpleQuestionStat.answerList.Count != 0)
                        {
                            questionInfo1.simpleQuestionStat.answerList = (from a in questionInfo.simpleQuestionStat.answerList
                                                                           group a by new { a.isRightAnswer, a.answerText, a.answerId }
                                                                           into g
                                                                           select new SimpleAnswer()
                                                                           {
                                                                               answerText = g.Key.answerText,
                                                                               isRightAnswer = g.Key.isRightAnswer,
                                                                               answerId = g.Key.answerId,
                                                                               total = (from agr in agregatedAnswer
                                                                                        where agr.answerText == g.Key.answerText
                                                                                        select
                                                                                        agr.count).FirstOrDefault()
                                                                           }).ToList();
                            foreach (var lst in questionInfo1.simpleQuestionStat.answerList)
                            {
                                var scomments = (from b in questionInfo.simpleQuestionStat.answerList
                                                 where b.answerText == lst.answerText && b.isRightAnswer == lst.isRightAnswer
                                                 group b by new { b.commentText, b.total, b.answerCommentId }
                                                                           into g
                                                 select new QuestionInfoAnswerComment() { commentText = g.Key.commentText, commentId = g.Key.answerCommentId, total = g.Sum(_ => _.total) }).ToList();
                                lst.comments = scomments;
                                //lst.total = scomments.Sum(a => a.total);
                            }
                        }
                        else
                        {
                            questionInfo1.simpleQuestionStat.answerList = (from agr in agregatedAnswer
                                                                           select new SimpleAnswer()
                                                                           { answerText = agr.answerText, total = agr.count, isRightAnswer = true, comments = new List<QuestionInfoAnswerComment>() }
                            ).ToList();
                        }

                    }
                    //questionInfo1.simpleQuestionStat.answerList.Where(a=>a.comments==null);

                    questionInfo1.total = questionInfo.simpleQuestionStat.answerList.Sum(a => a.total);
                    questionInfo1.total = agregatedAnswer.Sum(a => a.count);
                    return questionInfo1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// GetSectionsInfo
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetSectionsInfo")]
        [HttpPost]
        [ResponseType(typeof(List<SectionInfoStat.ScorecardSectionInfo>))]
        public List<SectionInfoStat.ScorecardSectionInfo> GetSectionsInfo([FromBody]Filter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = HttpContext.Current.GetUserName();

#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetSectionScores]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
                List<CoachingQueueCallDetails> cqcd = new List<CoachingQueueCallDetails>();
                CoachingQueueResponceData cq = new CoachingQueueResponceData();
                var callShortInfo = new CallShortInfov2();
                try
                {

                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    var sectioInfoRaw = new List<SectionInfoRaw>();



                    try
                    {
                        sectioInfoRaw = SectionInfoRaw.Create(reader);
                    }
                    catch
                    {

                    }





                    var scorecardInfo = (from scr in sectioInfoRaw
                                         group scr by new { scr.scorecardId, scr.scorecardName } into scr
                                         select new SectionInfoStat.ScorecardSectionInfo()
                                         {
                                             scorecard = new Scorecard { scorecardName = scr.Key.scorecardName, scorecardId = scr.Key.scorecardId },
                                             sections = (from q in sectioInfoRaw
                                                         where q.scorecardId == scr.Key.scorecardId
                                                         group q by new { q.sectionId, q.sectionName, q.sectionOrder, q.scorecardId } into sections
                                                         orderby sections.Key.sectionOrder ascending
                                                         select new SectionInfoStat.Section()
                                                         {
                                                             sectionInfo = new SectionInfoStat.SectionInfo()
                                                             {
                                                                 sectionOrder = sections.Key.sectionOrder,
                                                                 sectionId = sections.Key.sectionId,
                                                                 sectionName = sections.Key.sectionName,
                                                             },
                                                             questions = (from ra in sectioInfoRaw
                                                                          where ra.scorecardId == sections.Key.scorecardId && ra.sectionId == sections.Key.sectionId
                                                                          orderby ra.qorder ascending
                                                                          select new SectionInfoStat.SectionQuestionDetail()
                                                                          {
                                                                              isComposite = ra.isComposite,
                                                                              isLinked = ra.isLinked,
                                                                              qId = ra.qId,
                                                                              questionShortName = ra.questionShortName,
                                                                              questionType = ra.questionType,
                                                                              totalRight = ra.totalRight,
                                                                              totalWrong = ra.totalWrong,
                                                                              questionOrder = ra.qorder,
                                                                          }).ToList()
                                                         }).ToList()
                                         }).ToList();
                    return scorecardInfo;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



        }

        /// <summary>
        /// ExportSections
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportSections")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic ExportSections([FromBody]Filter filters)
        {
            var userName = HttpContext.Current.GetUserName();
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportSectionsCode sectionsExport = new ExportSectionsCode();
                      sectionsExport.SectionsExport(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }

        /// <summary>
        /// GetCalibrationQuestionInfo
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        [Route("dashboard/GetCalibrationQuestionInfo")]
        [HttpPost]
        [ResponseType(typeof(QuestionInfo1))]
        public QuestionInfo1 GetCalibrationQuestionInfo([FromBody]Filter filters, int qid)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "AndrewLoewith";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetCalibratorQuestionInfo]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type
                sqlComm.CommandTimeout = 41;
                sqlComm.Parameters.AddWithValue("@qid", qid);
                sqlComm.Connection = sqlCon;
                var agregatedAnswer = new List<AgregatedAnswer>();
                List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
                List<CoachingQueueCallDetails> cqcd = new List<CoachingQueueCallDetails>();
                CoachingQueueResponceData cq = new CoachingQueueResponceData();
                var callShortInfo = new CallShortInfov2();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    QuestionInfo questionInfo = new QuestionInfo();


                    while (reader.Read())
                    {
                        questionInfo.questionName = reader.GetValue(reader.GetOrdinal("questionShortName")).ToString();
                        questionInfo.sectionName = reader.GetValue(reader.GetOrdinal("questionSectionName")).ToString();
                        questionInfo.isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString());
                        questionInfo.isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString());
                        questionInfo.questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString();
                        questionInfo.questionID = int.Parse(reader.GetValue(reader.GetOrdinal("qid")).ToString());

                    }
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            questionInfo.scorecard = new Scorecard()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()
                            };
                        }

                    }
                    var questionStatistic = new QuestionStatistic();
                    questionStatistic.answerList = new List<AnswerCommentStatList>();
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            try
                            {
                                var qs = new AnswerCommentStatList()
                                {
                                    total = int.Parse(reader.GetValue(reader.GetOrdinal("Count")).ToString()),
                                    answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                                    // answerComent = (reader.GetValue(reader.GetOrdinal("otherAnswerText")).ToString()),
                                    isRightAnswer = bool.Parse(reader.GetValue(reader.GetOrdinal("isRightAnswer")).ToString()),
                                    commentText = (reader.GetValue(reader.GetOrdinal("commentText")).ToString())
                                };
                                questionStatistic.answerList.Add(qs);
                            }
                            catch { }
                        }
                    }


                    var comments = new List<QuestionInfoAnswerComment>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var qs = new QuestionInfoAnswerComment()
                                {
                                    total = int.Parse(reader.GetValue(reader.GetOrdinal("count")).ToString()),
                                    commentText = reader.GetValue(reader.GetOrdinal("optionText")).ToString(),
                                };
                                comments.Add(qs);
                            }
                            catch { }
                        }
                    }
                    if (reader.NextResult())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                agregatedAnswer.Add(new AgregatedAnswer()
                                {
                                    answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                                    count = int.Parse(reader.GetValue(reader.GetOrdinal("Count")).ToString())
                                });
                            }
                        }
                        catch { }

                    }
                    questionInfo.simpleQuestionStat = questionStatistic;

                    QuestionInfo1 questionInfo1 = new QuestionInfo1();
                    questionInfo1 = new QuestionInfo1()
                    {
                        questionID = questionInfo.questionID,
                        questionName = questionInfo.questionName,
                        isComposite = questionInfo.isComposite,
                        isLinked = questionInfo.isLinked,
                        questionType = questionInfo.questionType,
                        sectionName = questionInfo.sectionName,
                        scorecard = questionInfo.scorecard,
                        total = questionInfo.total
                    };
                    questionInfo1.compositeQuestionStat = new CompositeQuestionStats();


                    if (questionInfo1.isComposite)
                    {
                        // questionInfo1.compositeQuestionStat = new CompositeQuestionStats();
                        questionInfo1.compositeQuestionStat.totalRight = questionInfo.simpleQuestionStat.answerList.Count(g => g.isRightAnswer);
                        questionInfo1.compositeQuestionStat.comments = (from a in questionInfo.simpleQuestionStat.answerList where a.isRightAnswer == true group a by new { a.commentText } into g select new QuestionInfoAnswerComment() { commentText = g.Key.commentText, total = g.Sum(a => a.total) }).ToList();

                        var rightAnswerInfo =
                        (from a in questionInfo.simpleQuestionStat.answerList
                         where a.isRightAnswer
                         group a by new { a.answerText, a.total } into g
                         select new CompositeAnswerInfo() { answerText = g.Key.answerText, totalCustomComments = g.Sum(_ => _.total) });

                        questionInfo1.compositeQuestionStat.rightAnswerInfo = new CompositeAnswerInfo() { answerText = rightAnswerInfo.FirstOrDefault().answerText, totalCustomComments = rightAnswerInfo.Sum(a => a.totalCustomComments) };

                        try
                        {
                            questionInfo1.compositeQuestionStat.totalRight = questionInfo1.compositeQuestionStat.comments.Sum(a => a.total);
                        }
                        catch
                        {
                            questionInfo1.compositeQuestionStat.totalRight = questionInfo1.compositeQuestionStat.rightAnswerInfo.totalCustomComments;
                        }

                        var wrongAnswerInfo = (from a in questionInfo.simpleQuestionStat.answerList
                                               where !a.isRightAnswer
                                               group a by new { a.answerText, a.total } into g
                                               select new CompositeAnswerInfo() { answerText = g.Key.answerText, totalCustomComments = g.Sum(_ => _.total) });
                        questionInfo1.compositeQuestionStat.wrongAnswerInfo =
                            new CompositeAnswerInfo()
                            {
                                answerText = wrongAnswerInfo.FirstOrDefault().answerText,
                                totalCustomComments = wrongAnswerInfo.Sum(a => a.totalCustomComments)
                            };
                        questionInfo1.compositeQuestionStat.comments = comments;

                    }
                    else
                    {
                        questionInfo1.simpleQuestionStat = new SimpleQuestionStat();
                        if (questionInfo.simpleQuestionStat.answerList.Count != 0)
                        {
                            questionInfo1.simpleQuestionStat.answerList = (from a in questionInfo.simpleQuestionStat.answerList
                                                                           group a by new { a.isRightAnswer, a.answerText }
                                                                           into g
                                                                           select new SimpleAnswer()
                                                                           {
                                                                               answerText = g.Key.answerText,
                                                                               isRightAnswer = g.Key.isRightAnswer,
                                                                               total = (from agr in agregatedAnswer
                                                                                        where agr.answerText == g.Key.answerText
                                                                                        select
                                                                                        agr.count).FirstOrDefault()
                                                                           }).ToList();
                            foreach (var lst in questionInfo1.simpleQuestionStat.answerList)
                            {
                                var scomments = (from b in questionInfo.simpleQuestionStat.answerList
                                                 where b.answerText == lst.answerText && b.isRightAnswer == lst.isRightAnswer
                                                 group b by new { b.commentText, b.total }
                                                                           into g
                                                 select new QuestionInfoAnswerComment() { commentText = g.Key.commentText, total = g.Sum(_ => _.total) }).ToList();
                                lst.comments = scomments;
                                lst.total = scomments.Sum(a => a.total);
                            }
                        }
                        else
                        {
                            questionInfo1.simpleQuestionStat.answerList = (from agr in agregatedAnswer
                                                                           select new SimpleAnswer()
                                                                           { answerText = agr.answerText, total = agr.count, isRightAnswer = true, comments = new List<QuestionInfoAnswerComment>() }
                            ).ToList();
                        }

                    }
                    //questionInfo1.simpleQuestionStat.answerList.Where(a=>a.comments==null);

                    questionInfo1.total = questionInfo.simpleQuestionStat.answerList.Sum(a => a.total);
                    questionInfo1.total = agregatedAnswer.Sum(a => a.count);
                    return questionInfo1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        /// <summary>
        /// getCalibrationSectionsInfo
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/getCalibrationSectionsInfo")]
        [HttpPost]
        [ResponseType(typeof(List<SectionInfoStat.ScorecardSectionInfo>))]
        public List<SectionInfoStat.ScorecardSectionInfo> getCalibrationSectionsInfo([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "andrewloewith";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetCalibratorSectionScores]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
                List<CoachingQueueCallDetails> cqcd = new List<CoachingQueueCallDetails>();
                CoachingQueueResponceData cq = new CoachingQueueResponceData();
                var callShortInfo = new CallShortInfov2();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    var sectioInfoRaw = new List<SectionInfoRaw>();


                    while (reader.Read())
                    {
                        try
                        {
                            sectioInfoRaw.Add(SectionInfoRaw.CreateRecord(reader));

                        }
                        catch
                        {

                        }
                    }



                    var scorecardInfo = (from scr in sectioInfoRaw
                                         group scr by new { scr.scorecardId, scr.scorecardName } into scr
                                         select new SectionInfoStat.ScorecardSectionInfo()
                                         {
                                             scorecard = new Scorecard { scorecardName = scr.Key.scorecardName, scorecardId = scr.Key.scorecardId },
                                             sections = (from q in sectioInfoRaw
                                                         where q.scorecardId == scr.Key.scorecardId
                                                         group q by new { q.sectionId, q.sectionName, q.sectionOrder, q.scorecardId } into sections
                                                         orderby sections.Key.sectionOrder ascending
                                                         select new SectionInfoStat.Section()
                                                         {
                                                             sectionInfo = new SectionInfoStat.SectionInfo()
                                                             {
                                                                 sectionOrder = sections.Key.sectionOrder,
                                                                 sectionId = sections.Key.sectionId,
                                                                 sectionName = sections.Key.sectionName,
                                                             },
                                                             questions = (from ra in sectioInfoRaw
                                                                          where ra.scorecardId == sections.Key.scorecardId && ra.sectionId == sections.Key.sectionId
                                                                          orderby ra.qorder ascending
                                                                          select new SectionInfoStat.SectionQuestionDetail()
                                                                          {
                                                                              isComposite = ra.isComposite,
                                                                              isLinked = ra.isLinked,
                                                                              qId = ra.qId,
                                                                              questionShortName = ra.questionShortName,
                                                                              questionType = ra.questionType,
                                                                              totalRight = ra.totalRight,
                                                                              totalWrong = ra.totalWrong,
                                                                              questionOrder = ra.qorder,
                                                                          }).ToList()
                                                         }).ToList()
                                         }).ToList();
                    return scorecardInfo;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        /// <summary>
        /// exportCallDetails
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dashboard/exportCallDetails")]
        [ResponseType(typeof(IHttpActionResult))]
#pragma warning disable IDE1006 // Naming Styles
        public dynamic exportCallDetails([FromBody]CallDetailsExportFilter filters)
#pragma warning restore IDE1006 // Naming Styles
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

            HostingEnvironment.QueueBackgroundWorkItem(
                ct =>
                {
                    var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    var cancellationToken = linkedTokenSource.Token;
                    return Task.Factory.StartNew(async () =>
                   {
                       await FileHelper.GenerateReport(filters, filters.columns, userName, "CallDetails");
                   }, cancellationToken);
                });

            return "success";
        }
        /// <summary>
        /// GetWebsiteStatistic
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/GetWebsiteStatistic")]
        [HttpPost]
        [ResponseType(typeof(WebSiteAverage))]
        public WebSiteAverage GetWebsiteStatistic([FromBody]Filter filters)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetWebSiteStatisticApi]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type
                sqlComm.CommandTimeout = int.MaxValue;
                sqlComm.Connection = sqlCon;

                WebSiteAverage webSiteAverage = new WebSiteAverage();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        webSiteAverage = new WebSiteAverage()
                        {
                            total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("total")).ToString()),
                            compliant = reader.IsDBNull(reader.GetOrdinal("compliant")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("compliant")).ToString()),
                            bad = reader.IsDBNull(reader.GetOrdinal("bad")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("bad")).ToString()),
                            nonCompliant = reader.IsDBNull(reader.GetOrdinal("nonCompliant")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("nonCompliant")).ToString()),
                        };

                    }
                    return webSiteAverage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        /// <summary>
        /// GetExportQueue
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        [Route("dashboard/GetExportQueue")]
        [HttpPost]
        [ResponseType(typeof(List<LoadedQueues>))]
        public List<LoadedQueues> GetExportQueue([FromBody]string moduleName)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                SqlCommand sqlComm = new SqlCommand();
                if (moduleName != null && moduleName != string.Empty)
                {
                    sqlComm = new SqlCommand("select q.id,q.[fileName], q.[exportDate],q.[fileUrl],q.[status],u.[userName],q.[exportType]  from  exportQueue q join userextrainfo u on u.id= q.fileowner where u.[username]='" + userName + "' and exportType = '" + moduleName + "'")
                    {
                        CommandTimeout = 41,
                        Connection = sqlCon
                    };
                }
                else
                {
                    sqlComm = new SqlCommand("select q.id,q.[fileName], q.[exportDate],q.[fileUrl],q.[status],u.[userName],q.[exportType]  from  exportQueue q join userextrainfo u on u.id= q.fileowner where u.[username]='" + userName + "'") //and exportType = '" + moduleName + "'")
                    {
                        CommandTimeout = 41,
                        Connection = sqlCon
                    };
                }

                List<LoadedQueues> loadedQueues = new List<LoadedQueues>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        loadedQueues.Add(new LoadedQueues()
                        {
                            fileOwner = reader.IsDBNull(reader.GetOrdinal("userName")) ? "" : (reader.GetValue(reader.GetOrdinal("userName")).ToString()),
                            date = reader.IsDBNull(reader.GetOrdinal("exportDate")) ? DateTime.Now : DateTime.Parse(reader.GetValue(reader.GetOrdinal("exportDate")).ToString()),
                            fileName = reader.IsDBNull(reader.GetOrdinal("fileName")) ? "" : reader.GetValue(reader.GetOrdinal("fileName")).ToString(),
                            url = reader.IsDBNull(reader.GetOrdinal("fileUrl")) ? "" : reader.GetValue(reader.GetOrdinal("fileUrl")).ToString(),
                            pending = reader.IsDBNull(reader.GetOrdinal("status")) || bool.Parse(reader.GetValue(reader.GetOrdinal("status")).ToString()),
                            id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            type = reader.IsDBNull(reader.GetOrdinal("exportType")) ? "" : (reader.GetValue(reader.GetOrdinal("exportType")).ToString())
                        });

                    }
                    return loadedQueues;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// DeleteFromExportQueue
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("dashboard/DeleteFromExportQueue")]
        [HttpPost]
        [ResponseType(typeof(List<LoadedQueues>))]
        public dynamic DeleteFromExportQueue([FromBody] DeleteExportQueueModel obj)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = HttpContext.Current.GetUserName();

                SqlCommand sqlComm =
                    new SqlCommand(
                        "select q.id,q.[fileName], q.[exportDate],q.[fileUrl],q.[status],u.[userName]  from  exportQueue q join userextrainfo u on u.id= q.fileowner where q.id =" +
                        obj.id + " and u.[username]='" + userName + "'")
                    {
                        CommandTimeout = 41,
                        Connection = sqlCon
                    };
                List<LoadedQueues> loadedQueues = new List<LoadedQueues>();
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();

                LoadedQueues record = new LoadedQueues();
                while (reader.Read())
                {
                    record = new LoadedQueues()
                    {
                        fileOwner = reader.IsDBNull(reader.GetOrdinal("userName")) ? "" : (reader.GetValue(reader.GetOrdinal("userName")).ToString()),
                        date = reader.IsDBNull(reader.GetOrdinal("exportDate")) ? DateTime.Now : DateTime.Parse(reader.GetValue(reader.GetOrdinal("exportDate")).ToString()),
                        fileName = reader.IsDBNull(reader.GetOrdinal("fileName")) ? "" : reader.GetValue(reader.GetOrdinal("fileName")).ToString(),
                        url = reader.IsDBNull(reader.GetOrdinal("fileUrl")) ? "" : reader.GetValue(reader.GetOrdinal("fileUrl")).ToString(),
                        pending = reader.IsDBNull(reader.GetOrdinal("status")) || bool.Parse(reader.GetValue(reader.GetOrdinal("status")).ToString()),
                        id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                    };
                }
                reader.Close();
                try
                {
                    if (File.Exists(Path.Combine(HostingEnvironment.MapPath(@"~\export\"), record.fileName))
                        && !HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost")
                        && !HttpContext.Current.Request.UrlReferrer.Host.Contains("dev")
                        )
                    {
                        File.Delete(Path.Combine(HostingEnvironment.MapPath(@"~\export\"), record.fileName));
                    }
                    var deleteCommand =
                        new SqlCommand("delete from exportQueue where id =" + record.id) { Connection = sqlCon };
                    deleteCommand.ExecuteReader();
                }
                catch (Exception)
                {
                    // ignored
                }

                return GetExportQueue((string)obj.moduleName);
            }
        }

        /// <summary>
        /// GetAvailableTableColumns
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetAvailableTableColumns")]
        [HttpPost]
        [ResponseType(typeof(SettingsLayer))]
        public async Task<List<AvailableTableColumns>> GetAvailableTableColumns()
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                SettingsLayer settings = new SettingsLayer();
                try
                {
                    return await settings.GetUserCollums(userName, sqlCon);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get Pending Calibrations for Scorecard pending calibration module
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetPendingCalibrations")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetPendingCalibrations(Filter filters)
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "papadmin";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                //Filter f = new Filter() { filters = filters.filters, range = filters.range };
                // SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetScorecardSummary]", userName);

                SqlCommand sqlComm = new SqlCommand();
                sqlComm.CommandType = CommandType.StoredProcedure;

                //sqlComm = new SqlCommand() { CommandTimeout = 60 };
                sqlComm.CommandText = "GetScorecardSummary";
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@appname", "");
                sqlComm.Connection = sqlCon;
                List<CalibrationPendingSummary> calibrationPendingSummary = new List<CalibrationPendingSummary>();
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                try
                {

                    while (reader.Read())
                    {
                        try
                        {
                            calibrationPendingSummary.Add(new CalibrationPendingSummary()
                            {
                                pending_calibrations = int.Parse(reader.GetValue(reader.GetOrdinal("Pending_Calibs")).ToString()),
                                pending_reviev_time = float.Parse(reader.GetValue(reader.GetOrdinal("pending_review_time")).ToString()),
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                oldestCall = DateTime.Parse(reader.GetValue(reader.GetOrdinal("oldest_call")).ToString()),
                                haveAccess = bool.Parse(reader.GetValue(reader.GetOrdinal("haveAccess")).ToString())
                            });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return calibrationPendingSummary;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// GetPendingCalibrationsInternal
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetPendingCalibrationsInternal")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetPendingCalibrationsInternal()
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "JacquieRamirez";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,

                    //sqlComm = new SqlCommand() { CommandTimeout = 60 };
                    CommandText = "[GetPendingCalibrationsInternal]"
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@appname", "");
                sqlComm.Connection = sqlCon;
                List<CalibrationPendingSummary> calibrationPendingSummary = new List<CalibrationPendingSummary>();
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                try
                {

                    while (reader.Read())
                    {
                        try
                        {
                            calibrationPendingSummary.Add(new CalibrationPendingSummary()
                            {
                                pending_calibrations = int.Parse(reader.GetValue(reader.GetOrdinal("Pending_Calibs")).ToString()),
                                pending_reviev_time = float.Parse(reader.GetValue(reader.GetOrdinal("pending_review_time")).ToString()),
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                oldestCall = DateTime.Parse(reader.GetValue(reader.GetOrdinal("oldest_call")).ToString()),
                                haveAccess = bool.Parse(reader.GetValue(reader.GetOrdinal("haveAccess")).ToString())
                            });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return calibrationPendingSummary;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// GetAMSummary
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetScorecardDetailedInfo")]
        [HttpPost]
        [ResponseType(typeof(List<AMSummaryModel>))]
        public dynamic GetScorecardDetailedInfo()
        {
            List<AMSummaryModel> aMSummaryModel = new List<AMSummaryModel>();
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandTimeout = int.MaxValue,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[GetAMSummary]"
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                //sqlComm.Parameters.AddWithValue("@AM", userName);
                sqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                aMSummaryModel = AMSummaryModel.Create(reader);
                return aMSummaryModel;
            }
        }

        /// <summary>
        /// GetQuchingQueueCustomColumns
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetQuchingQueueCustomColumns")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic GetQuchingQueueCustomColumns()
        {
            var list = new[]
            {
                new{id = 0,Name="Agent Name"},
                new{id = 1,Name = "Call Date"},
                new{id = 2,Name="Call Type"},
                new{id = 3,Name="Call Id" },
                new{id = 4,Name = "Call Review Status" },
                new{id = 5,Name= "Call Audio Url" },
                new{id = 6,Name="Call Audio Length"},
                new{id = 7,Name="Scorecard Id"},
                new{id = 8,Name="Scorecard Name"},
                new{id = 9,Name = "scorecardFailScore"},
                new{id = 10,Name = "Review Date"},
                new{id = 11,Name = "Reviewer User Role"},
                new{id = 12,Name="Reviewer Name"},
                new{id = 13,Name="Calibrator Id"},
                new{id = 14,Name = "CalibratorName"},
                new{id = 15,Name = "Missed Items Count"},
                new{id = 16,Name = "Agent Score" },
                new{id = 17,Name="Call Failed" },
                new{id = 18,Name = "Review Comments Present"},
                new{id = 19,Name = "Notification Comments Present"},
                new{id = 20,Name = "Agent Group"},
                new{id = 21,Name = "Campaign"},
                new{id = 22,Name = "Session Id"},
                new{id = 23,Name = "Profile Id"},
                new{id = 24,Name = "Prospect First Name"},
                new{id = 25,Name = "Prospect Last Name"},
                new{id = 26,Name = "Prospect Phone"},
                new{id = 27,Name = "Prospect Email"},
                new{id = 28,Name = "Calibration id"},
                new{id = 29,Name = "Was Edited"},
                new{id = 30,Name = "Notification Status"},
                new{id = 31,Name = "Notification Step"},
                new{id = 32,Name = "Notification ID"},
                new{id = 33,Name = "Is Owned Notification"},
                new{id = 34,Name = "Owned Notification"},
                new{id = 35,Name = "Assigned To Role"}
            };
            return list;
        }

        /// <summary>
        /// ExportQoachingQueue
        /// </summary>
        /// <param name="coachingQueueExportEndPointModel"></param>
        /// <returns></returns>
        [Route("dashboard/ExportQoachingQueue")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportQoachingQueue([FromBody] CoachingQueueExportEndPointModel coachingQueueExportEndPointModel)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "xl.lstevens";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportCoachingQueueCode exportCoachingQueueCode = new ExportCoachingQueueCode();
                      exportCoachingQueueCode.CoachingQueueExport(coachingQueueExportEndPointModel.Filter, userName, coachingQueueExportEndPointModel.columns);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");

        }
        /// <summary>
        /// Export Top Missed Points
        /// </summary>
        /// <param name="averageFilter"></param>
        /// <returns></returns>
        [Route("dashboard/ExportTopMissedPoints")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportTopMissedPoints([FromBody]AverageFilter averageFilter)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportTopMissedItemsCode exportTopMissedItemsCode = new ExportTopMissedItemsCode();
                      exportTopMissedItemsCode.TopMissedItemsExport(averageFilter, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        /// <summary>
        /// ExportMyPay
        /// </summary>
        /// <param name="weekEnd"></param>
        /// <returns></returns>
        [Route("dashboard/ExportMyPay")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic ExportMyPay([FromBody]string weekEnd)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "adrianlapaz";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportMyPayCode exportMyPayCode = new ExportMyPayCode();
                      exportMyPayCode.ExportMyPay(weekEnd, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        /// <summary>
        /// Export Top Qa Missed Points
        /// </summary>
        /// <param name="averageFilter"></param>
        /// <returns></returns>
        [Route("dashboard/ExportTopQaMissedPoints")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportTopQaMissedPoints([FromBody]AverageFilter averageFilter)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportTopQaMissedItemsCode exportTopQaMissedItemsCode = new ExportTopQaMissedItemsCode();
                      exportTopQaMissedItemsCode.ExportTopQaMissedItems(averageFilter, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }



        /// <summary>
        /// ExportTopCalibratorMissedPoints
        /// </summary>
        /// <param name="averageFilter"></param>
        /// <returns></returns>
        [Route("dashboard/ExportTopCalibratorMissedPoints")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportTopCalibratorMissedPoints([FromBody]AverageFilter averageFilter)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportTopCalibratorMissedItemsCode exportTopCalibratorMissedItemsCode = new ExportTopCalibratorMissedItemsCode();
                      exportTopCalibratorMissedItemsCode.ExportTopCalibratorMissedItems(averageFilter, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        /// <summary>
        /// ExportWebSiteStatistic
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportWebSiteStatistic")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportWebSiteStatistic([FromBody]Filter filters)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportWebSiteStatisticCode exportWebSiteStatisticCode = new ExportWebSiteStatisticCode();
                      exportWebSiteStatisticCode.ExportWebSiteStatistic(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }
        /// <summary>
        /// ExportGroupPerformance
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportGroupPerformance")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportGroupPerformance([FromBody]Filter filters)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportGroupPerformanceCode exportGroupPerformanceCode = new ExportGroupPerformanceCode();
                      exportGroupPerformanceCode.ExportGroupPerformance(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        /// <summary>
        /// ExportCampaignPerformance
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportCampaignPerformance")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportCampaignPerformance([FromBody]Filter filters)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportCampaignPerfomanceCode exportCampaignPerfomanceCode = new ExportCampaignPerfomanceCode();
                      exportCampaignPerfomanceCode.ExportCampaignPerformance(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }
        /// <summary>
        /// ExportCallsLeft
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportCallsLeft")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportCallsLeft([FromBody]Filter filters)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportCallsLeftCode exportCallsLeftCode = new ExportCallsLeftCode();
                      exportCallsLeftCode.ExporCallsLeft(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }





        /// <summary>
        /// CalibrationSectionExport
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("dashboard/ExportCalibrationSections")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic CalibrationSectionExport([FromBody]Filter filters)
        {

            var userName = HttpContext.Current.GetUserName();
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportCalibrationSection exportCallsLeftCode = new ExportCalibrationSection();
                      exportCallsLeftCode.CalibrationSectionExport(filters, userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        /// <summary>
        /// GetAvailableCoachingQueueColumns
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/GetAvailableCoachingQueueColumns")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic GetAvailableCoachingQueueColumns()
        {
            var response = new[]
                {
                    new { id = 1, value = "Agent Name"                   , @checked = false,isRequired = false, sortable = true},
                    new { id = 2, value = "Call Date"                    , @checked = false,isRequired = false, sortable = true},
                    new { id = 3, value = "Call Type"                    , @checked = false,isRequired = false, sortable = true},
                    new { id = 4, value = "Call Id"                      , @checked = false,isRequired = false, sortable = true},
                    new { id = 5, value = "Call Review Status"           , @checked = false,isRequired = false, sortable = true},
                    new { id = 6, value = "Call Audio Url"               , @checked = false,isRequired = false, sortable = true},
                    new { id = 7, value = "Call Audio Length"            , @checked = false,isRequired = false, sortable = true},
                    new { id = 8, value = "Scorecard Id"                 , @checked = false,isRequired = false, sortable = true},
                    new { id = 9, value = "Scorecard Name"               , @checked = false,isRequired = false, sortable = true},
                    new { id = 10,value = "Scorecard Fail Score"         , @checked = false,isRequired = false, sortable = true},
                    new { id = 11,value = "Review Date"                  , @checked = false,isRequired = false, sortable = true},
                    new { id = 12,value = "Reviewer User Role"           , @checked = false,isRequired = false, sortable = true},
                    new { id = 13,value = "Reviewer Name"                , @checked = false,isRequired = false, sortable = true},
                    new { id = 14,value = "Calibrator Id"                , @checked = false,isRequired = false, sortable = true},
                    new { id = 15,value = "Calibrator Name"              , @checked = false,isRequired = false, sortable = true},
                    new { id = 16,value = "Missed Items Count"           , @checked = false,isRequired = false, sortable = true},
                    new { id = 17,value = "Agent Score"                  , @checked = false,isRequired = false, sortable = true},
                    new { id = 18,value = "Call Failed"                  , @checked = false,isRequired = false, sortable = true},
                    new { id = 19,value = "Review Comments Present"      , @checked = false,isRequired = false, sortable = true},
                    new { id = 20,value = "Notification Comments Present", @checked = false,isRequired = false, sortable = true},
                    new { id = 21,value = "Agent Group"                  , @checked = false,isRequired = false, sortable = true},
                    new { id = 22,value = "Campaign"                     , @checked = false,isRequired = false, sortable = true},
                    new { id = 23,value = "Session Id"                   , @checked = false,isRequired = false, sortable = true},
                    new { id = 24,value = "Profile Id"                   , @checked = false,isRequired = false, sortable = true},
                    new { id = 25,value = "Prospect First Name"          , @checked = false,isRequired = false, sortable = true},
                    new { id = 26,value = "Prospect Last Name"           , @checked = false,isRequired = false, sortable = true},
                    new { id = 27,value = "Prospect Phone"               , @checked = false,isRequired = false, sortable = true},
                    new { id = 28,value = "Prospect Email"               , @checked = false,isRequired = false, sortable = true},
                    new { id = 29,value = "Calibration Id"               , @checked = false,isRequired = false, sortable = true},
                    new { id = 30,value = "Was Edited"                   , @checked = false,isRequired = false, sortable = true},
                    new { id = 31,value = "Notification Status"          , @checked = false,isRequired = false, sortable = true},
                    new { id = 32,value = "Notification Step"            , @checked = false,isRequired = false, sortable = true},
                    new { id = 33,value = "Notification Id"              , @checked = false,isRequired = false, sortable = true},
                    new { id = 34,value = "Is Owned Notification"        , @checked = false,isRequired = false, sortable = true},
                    new { id = 35,value = "Owned Notification"           , @checked = false,isRequired = false, sortable = true},
                    new { id = 36,value = "Assigned To Role"             , @checked = false,isRequired = false, sortable = true},
                    new { id = 37,value = "Missed Items"                 , @checked = false,isRequired = false, sortable = true},
                    new { id = 38,value = "Missed Items Comments"        , @checked = false,isRequired = false, sortable = true},
                };

            response = response.OrderBy(x => x.value).Select(x => x).ToArray();
            return response;
        }




        /// <summary>
        /// ExportScorecardSummaryCode : api for export Scorecard Summary Module ("ScorecardCounts" for export queue)
        /// </summary>
        /// <returns></returns>
        [Route("dashboard/ExportScorecardSummary")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public dynamic ExportScorecardSummaryCode()
        {

            var userName = HttpContext.Current.GetUserName();
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      ExportScorecardSummary export = new ExportScorecardSummary();
                      export.ExportScorecardSummaryCode(userName);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }


        ///// <summary>
        ///// ExportScorecardSummaryCode : api for export Scorecard Summary Module ("ScorecardCounts" for export queue)
        ///// </summary>
        ///// <returns></returns>
        //[Route("dashboard/GetAllRecords")]
        //[HttpPost]
        //[ResponseType(typeof(object))]
        //public dynamic GetAllRecords()
        //{

        //    string call_date = "2018-08-02 07:58:59.350";
        //    string appname = "edufficient";
        //    string use_review ="1";
        //    List<CallRecord> cr = new List<CallRecord>();
        //    bool rev_date = false;
        //    if (use_review == null)
        //        rev_date = false;
        //    switch (use_review)
        //    {
        //        case "1":
        //        case "true":
        //        case "True":
        //            {
        //                rev_date = true;
        //                break;
        //            }
        //    }
        //    try
        //    {
        //        CallRecordDetails objCallRecordDetails = new CallRecordDetails();
        //        List<CallRecord> objCallRecordList = new List<CallRecord>();
        //        objCallRecordList=objCallRecordDetails.GetCallRecord(call_date, rev_date, appname, use_review);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Ok("success");
        //}
    }
}