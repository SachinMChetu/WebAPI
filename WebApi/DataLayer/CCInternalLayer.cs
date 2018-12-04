using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using WebApi.DataLayerlo;
using WebApi.Entities;
using WebApi.Models.CCInternalAPI;
using WebApi.Models.CDService;
using WebApi.Models.DBModel;

namespace WebApi.DataLayer
{
    public class CCInternalLayer
    {


        /// <summary>
        /// GetCallRecord
        /// </summary>
        /// <param name="call_date"></param>
        /// <param name="rev_date"></param>
        /// <param name="appname"></param>
        /// <param name="use_review"></param>
        /// <returns></returns>
        public List<CallRecord> GetCallRecord(string call_date, bool rev_date, string appname, string use_review)
        {
            List<CallRecord> callRecordResData = new List<CallRecord>();
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    List<CallRecord> SubCallRecord = new List<CallRecord>();

                    ObjectResult<GetAllRecords_Result> objGetAllRecords = dataContext.GetAllRecords(Convert.ToDateTime(call_date), appname, Convert.ToInt32(use_review));

                    SubCallRecord = (from records in objGetAllRecords
                                     select new CallRecord
                                     {
                                         F_ID = records.F_ID.ToString(),
                                         review_ID = records.review_ID.ToString(),
                                         Comments = records.Comments,
                                         autofail = records.autofail.ToString(),
                                         reviewer = records.reviewer,
                                         appname = records.appname,
                                         total_score = records.total_score.ToString(),
                                         total_score_with_fails = records.total_score_with_fails.ToString(),
                                         call_length = records.call_length.ToString(),
                                         has_cardinal = "",
                                         fs_audio = records.fs_audio,
                                         week_ending_date = records.week_ending_date.ToString(),
                                         num_missed = records.num_missed.ToString(),
                                         missed_list = records.missed_list,
                                         call_made_date = records.call_made_date.ToString(),
                                         AGENT = records.AGENT,
                                         ANI = records.ANI,
                                         DNIS = records.DNIS,
                                         TIMESTAMP = records.TIMESTAMP,
                                         TALK_TIME = records.TALK_TIME,
                                         CALL_TIME = records.CALL_TIME.ToString(),
                                         CALL_TYPE = records.CALL_TYPE,
                                         leadid = records.leadid,
                                         AGENT_GROUP = records.AGENT_GROUP,
                                         Email = records.Email,
                                         City = records.City,
                                         State = records.State,
                                         Zip = records.Zip,
                                         Datacapturekey = records.Datacapturekey.ToString(),
                                         Datacapture = records.Datacapture.ToString(),
                                         Status = records.Status,
                                         Program = records.Program,
                                         X_ID = records.X_ID.ToString(),
                                         Datacapture_Status = records.Datacapture_Status,
                                         num_of_schools = records.num_of_schools,
                                         MAX_REVIEWS = records.MAX_REVIEWS.ToString(),
                                         review_started = records.review_started.ToString(),
                                         Number_of_Schools = records.Number_of_Schools,
                                         EducationLevel = records.EducationLevel,
                                         HighSchoolGradYear = records.HighSchoolGradYear,
                                         DegreeStartTimeframe = records.DegreeStartTimeframe,
                                         Expr3 = records.Expr3,
                                         First_Name = records.First_Name,
                                         Last_Name = records.Last_Name,
                                         address = records.address,
                                         phone = records.phone,
                                         call_date = records.call_date.ToString(),
                                         audio_link = records.audio_link,
                                         profile_id = records.profile_id,
                                         audio_user = "",
                                         audio_password = "",
                                         LIST_NAME = records.LIST_NAME,
                                         review_date = records.review_date.ToString(),
                                         CAMPAIGN = records.CAMPAIGN,
                                         DISPOSITION = records.DISPOSITION,
                                         bad_call = records.bad_call.ToString(),
                                         to_upload = "",
                                         SESSION_ID = records.SESSION_ID,
                                         agent_deviation = records.agent_deviation.ToString(),
                                         pass_fail = records.pass_fail,
                                         scorecard = records.scorecard.ToString(),
                                         uploaded = records.uploaded.ToString(),
                                         formatted_comments = records.formatted_comments,
                                         formatted_missed = records.formatted_missed,
                                         fileUrl = records.fileUrl,
                                         statusMessage = records.statusMessage,
                                         mediaId = "",
                                         requestStatus = "",
                                         fileStatus = "",
                                         response = "",
                                         review_time = "",
                                         wasEdited = records.wasEdited.ToString(),
                                         website = records.website.ToString(),
                                         pending_id = records.pending_id.ToString(),
                                         bad_call_reason = records.bad_call_reason,
                                         date_added = records.date_added.ToString(),
                                         calib_score = records.calib_score.ToString(),
                                         edited_score = records.edited_score.ToString(),
                                         compliance_sheet = records.compliance_sheet,
                                         scorecard_name = records.scorecard_name,
                                     }).ToList();

                    foreach (var scr in SubCallRecord)
                    {
                        int F_ID = Convert.ToInt32(scr.F_ID);
                        //DataTable qdt = Common.GetTable("select * from form_q_scores join questions on questions.id = form_q_scores.question_id   join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + dr["F_ID"].ToString() + " order by q_order, answer_order");
                        var scorecardResTemp = (from form_q_scores in dataContext.form_q_scores
                                                join questions in dataContext.Questions on form_q_scores.question_id equals questions.id
                                                join questionA in dataContext.question_answers on form_q_scores.question_answered equals questionA.id
                                                where form_q_scores.form_id == F_ID
                                                select new ScorecardResponse
                                                {
                                                    position = string.IsNullOrEmpty(form_q_scores.click_text) ? form_q_scores.q_position : form_q_scores.click_text,
                                                    question = questions.q_short_name,
                                                    result = questionA.answer_text,
                                                    QID = questions.id,
                                                    QAPoints = (int)questions.QA_points,
                                                    ViewLink = form_q_scores.view_link,
                                                    comments_allowed = (bool)questions.comments_allowed,
                                                    RightAnswer = (bool)questionA.right_answer,
                                                }).ToList();

                        scr.ScorecardResponses = new List<ScorecardResponse>();
                        scr.ScorecardResponses.AddRange(scorecardResTemp);

                        // DataTable cd_dt = Common.GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                        var clerkDataTemp = (from collected_data in dataContext.collected_data
                                             join sc_inputs in dataContext.sc_inputs on collected_data.value_id equals sc_inputs.id
                                             where collected_data.form_id == F_ID
                                             select new ClerkedData
                                             {
                                                 value = sc_inputs.value,
                                                 data = collected_data.value_data,
                                                 position = collected_data.value_position.ToString(),
                                                 ID = collected_data.value_id.ToString()
                                             }).ToList();
                        scr.ClerkedDataItems = new List<ClerkedData>();
                        scr.ClerkedDataItems.AddRange(clerkDataTemp);
                        callRecordResData.Add(scr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return callRecordResData;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ListenCall GetNextCall()
        {
            ListenCall objListenCall = new ListenCall();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                string username = HttpContext.Current.User.Identity.Name;
                string sessionId1 = HttpContext.Current.Session.SessionID;
                string sessionId = null;
                //if (HttpContext.Current.Session["session_id"].ToString() != null)
                //{
                //    sessionId = HttpContext.Current.Session["UserInfo"].ToString();
                //}
                //string username = "Joris";
                var getNextCallComplex = GetNextCallComplex(username, sessionId, 0, 0).ToList();
                if (getNextCallComplex.Equals(0))
                {
                    return objListenCall;
                }
                int scorecard_id = 1;
                var NextCallAPIQAQA = getNextCallComplex[0].Cast<getNextCallAPIQAQA_Result>();
                var dr = NextCallAPIQAQA.FirstOrDefault();
                List<School_X_Data_Result> school_X_Data = getNextCallComplex[1].Cast<School_X_Data_Result>().ToList();
                List<getotherformdata_Result> getotherformdata = getNextCallComplex[2].Cast<getotherformdata_Result>().ToList();
                List<rejection_reasons_Result> section_dt = getNextCallComplex[3].Cast<rejection_reasons_Result>().ToList();

                ListenData ld = new ListenData();
                if (dr != null)
                {
                    ld.address = dr.address;
                    ld.ANI = dr.ANI;
                    ld.appname = dr.appname;
                    //ld.audio_link = Common.GetAudioFileName(dr);
                    ld.call_date = dr.call_date.ToString();
                    ld.call_date = dr.call_date.ToString();
                    ld.CALL_TIME = dr.CALL_TIME.ToString();
                    ld.CALL_TYPE = dr.CALL_TYPE;
                    ld.CAMPAIGN = dr.CAMPAIGN;
                    ld.City = dr.City;
                    //ld.client_logo = dr.client_logo.ToString();
                    ld.DegreeStartTimeframe = dr.DegreeStartTimeframe;
                    ld.DISPOSITION = dr.DISPOSITION;
                    ld.DNIS = dr.DNIS;
                    ld.EducationLevel = dr.EducationLevel;
                    ld.Email = dr.Email;
                    ld.First_Name = dr.First_Name;
                    ld.HighSchoolGradYear = dr.HighSchoolGradYear;
                    ld.Last_Name = dr.Last_Name;
                    ld.leadid = dr.leadid;

                    ld.LIST_NAME = dr.LIST_NAME;
                    ld.phone = dr.phone;
                    ld.profile_id = dr.profile_id;
                    ld.Program = dr.Program;
                    ld.scorecard = dr.scorecard.ToString();
                    //ld.scorecard_name = dr.short_name.ToString();
                    ld.SESSION_ID = dr.SESSION_ID;
                    ld.State = dr.State;
                    ld.TIMESTAMP = dr.TIMESTAMP;
                    ld.website = dr.website;
                    ld.X_ID = dr.ID.ToString();
                    ld.Zip = dr.Zip;
                    ld.agent = dr.AGENT.ToString();
                    ld.must_review = Convert.ToInt32(dr.must_review);
                    scorecard_id = Convert.ToInt32(dr.scorecard);
                }
                List<SchoolItem> school_items = new List<SchoolItem>();
                foreach (var school_dr in school_X_Data)
                {
                    SchoolItem school_item = new SchoolItem();
                    school_item.AOI1 = school_dr.AOI1.ToString();
                    school_item.AOI2 = school_dr.AOI2;
                    school_item.College = school_dr.College;
                    school_item.DegreeOfInterest = school_dr.DegreeOfInterest;
                    school_item.L1_SubjectName = school_dr.L1_SubjectName;
                    school_item.L2_SubjectName = school_dr.L2_SubjectName;
                    school_item.Modality = school_dr.Modality;
                    school_item.Portal = school_dr.origin;
                    school_item.School = school_dr.School;
                    school_item.TCPA = school_dr.tcpa;
                    school_item.id = school_dr.id.ToString();
                    school_items.Add(school_item);
                }
                ld.SchoolData = school_items;
                List<OtherData> otherdata_items = new List<OtherData>();
                foreach (var school_dr in getotherformdata)
                {
                    OtherData otherdata_item = new OtherData();
                    otherdata_item.key = school_dr.data_key;
                    otherdata_item.label = school_dr.label;
                    otherdata_item.school = school_dr.school_name;
                    otherdata_item.type = school_dr.data_type;
                    otherdata_item.value = school_dr.data_value;
                    otherdata_item.id = school_dr.id.ToString();
                    otherdata_items.Add(otherdata_item);
                }
                ld.OtherData = otherdata_items;
                List<string> rej_list = new List<string>();
                foreach (var rej_dr in section_dt)
                {
                    rej_list.Add(rej_dr.reason.ToString());
                }
                ld.rejection_reasons = rej_list;
                objListenCall.ListenData = ld;
                getSCRecData gd = new getSCRecData();
                //if (dr.isQAQACard == false)
                //{

                if (gd.xcc_id != null)
                {
                    gd.scorecard_ID = scorecard_id.ToString();
                    gd.xcc_id = dr.ID.ToString();
                    objListenCall.Scorecard = GetScorecardRecordListen(gd.scorecard_ID, gd.xcc_id, username);
                }
                // }
                // else
                // {
                // gd.scorecard_ID = dr.qaqasc.ToString();
                // gd.xcc_id = dr.orig_ID.ToString();
                // }

                //objListenCall.ListenData.audio_merge = CDService.GetAvailableAudios(dr["ID"].ToString());
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                //if (HttpContext.Current.Session["session_id"].ToString() == "" | HttpContext.Current.Session["session_id"] == null)
                //{
                //Common.UpdateTable("update XCC_REPORT_NEW set review_started = dbo.getMTDate() where ID = " + dr.ID.ToString());
                if (dr != null)
                {
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == dr.ID).FirstOrDefault();
                    XCC_REPORT_NEW xccReportNew = new XCC_REPORT_NEW();
                    if (!object.Equals(isExist, null))
                    {
                        int x_id = isExist.ID;
                        xccReportNew = dataContext.XCC_REPORT_NEW.Find(x_id);
                        dataContext.Entry(xccReportNew).State = EntityState.Modified;
                        xccReportNew.review_started = mtdate;
                        dataContext.SaveChanges();
                    }
                    //}
                    //else
                    //{
                    //    //Common.UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" + username + "',dbo.getMTDate(), " + dr["ID"].ToString() + ",'listen'");
                    //    //HttpContext.Current.Session["session_id"] = null;
                    //    session_viewed objsession_viewed = new session_viewed();
                    //    // Save form_score3 data
                    //    objsession_viewed.session_id = dr.ID;
                    //    objsession_viewed.page_viewed = "listen";
                    //    objsession_viewed.date_viewed = mtdate;
                    //    objsession_viewed.agent = username;
                    //    dataContext.session_viewed.Add(objsession_viewed);
                    //    int result1 = dataContext.SaveChanges();
                    //}
                }
            }
            return objListenCall;
        }

        #region GetNextCallComplex
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sessionId"></param>
        /// <param name="scorecard"></param>
        /// <param name="xcc_Id"></param>
        /// <returns></returns>
        public List<IEnumerable> GetNextCallComplex(string username, string sessionId, Nullable<int> scorecard, Nullable<int> xcc_Id)
        {
            try
            {
                List<IEnumerable> result = new List<IEnumerable>();
                //second way to get multiple result set in entity
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var command = new SqlCommand()
                    {
                        CommandText = "[dbo].[getNextCallAPIQAQA]",
                        CommandType = CommandType.StoredProcedure
                    };

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@username",username),
                        new SqlParameter("@session_id ",sessionId),
                    };

                    command.Parameters.AddRange(parameters.ToArray());

                    result = dataContext.MultipleResults(command)
                                   .With<getNextCallAPIQAQA_Result>()
                                   .With<School_X_Data_Result>()
                                   .With<getotherformdata_Result>()
                                   .With<rejection_reasons_Result>()
                                   .Execute();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }
        #endregion GetNextCallComplex

        #region Public UpdateHeartbeat
        /// <summary>
        /// UpdateHeartbeat
        /// </summary>
        /// <param name="HB"></param>
        public void UpdateHeartbeat(Heatbeart HB)
        {
            int result = 0;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int xcc_Id = int.Parse(HB.xcc_id);

                var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == xcc_Id).FirstOrDefault();
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                XCC_REPORT_NEW xccReportNew = new XCC_REPORT_NEW();

                heatbeat_data heartBeatData = new heatbeat_data();
                if (!object.Equals(isExist, null))
                {
                    //Common.UpdateTable("update xcc_report_new set heartbeat_who = '" + HB.username + "', last_heartbeat = dbo.getMTdate() where id = " + HB.xcc_id);
                    dataContext.Entry(xccReportNew).State = EntityState.Modified;
                    xccReportNew.heartbeat_who = HB.username;
                    xccReportNew.last_heartbeat = mtdate;
                    result = dataContext.SaveChanges();

                    //Common.UpdateTable("insert into heatbeat_data (who, x_id) select '" + HB.username + "'," + HB.xcc_id);
                    dataContext.Entry(heartBeatData).State = EntityState.Modified;
                    heartBeatData.who = HB.username;
                    heartBeatData.who = HB.xcc_id;
                    result = dataContext.SaveChanges();
                }
            }
        }
        #endregion UpdateHeartbeat


        #region Public SwitchUser
        /// <summary>
        /// SwitchUser 
        /// </summary>
        /// <param name="SU"></param>
        /// <returns></returns>
        public ButtonAction SwitchUser(SimpleUser SU)
        {
            ButtonAction ba = new ButtonAction();
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                string alt_user = SU.username;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //DataTable user_dt = GetTable("select * from userextrainfo   where username = '" + alt_user.Replace("'", "''") + "' and (userextrainfo.active!=0 and user_role!='inactive')");
                    var result = dataContext.UserExtraInfoes.Where(x => x.username == alt_user.Replace("'", "''") && x.active != 0 && x.user_role != "inactive").ToList();
                    if (result.Count > 0)
                    {
                        //HttpCookie cookie = HttpContext.Current.Request.Cookies["filter"];
                        //if (cookie != null)
                        //{
                        //    cookie.Expires = DateTime.Now.AddYears(-1);
                        //  HttpContext.Current.Response.Cookies.Add(cookie);
                        //}
                        ba.ActionRedirect = "dashboard";
                        ba.ActionResult = "Success";
                        ba.ActionTask = "Redirect";
                    }
                    else
                    {
                        ba.ActionRedirect = "";
                        ba.ActionResult = "Alert";
                        ba.ActionTask = "User does not exist";
                    }
                }
            }
            return ba;
        }
        #endregion SwitchUser

        #region Public SwitchUserBack
        /// <summary>
        /// SwitchUserBack
        /// </summary>
        /// <param name="SU"></param>
        /// <returns></returns>
        public ButtonAction SwitchUserBack(SimpleUser SU)
        {
            ButtonAction ba = new ButtonAction();
            ba.ActionRedirect = "dashboard";
            ba.ActionResult = "Success";
            ba.ActionTask = "Redirect";
            return ba;
        }
        #endregion SwitchUserBack


        #region Public getPayWorksheet
        /// <summary>
        ///  getPayWorksheet
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Payworksheet> getPayWorksheet(SimpleDate SI)
        {
            List<Payworksheet> pw = new List<Payworksheet>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var result = dataContext.Database.SqlQuery<getPay3SCW_Result>(
              " exec getPay3SCW '" + SI.WeekEnding + "', '" + "" + "','" + null + "'"
              ).ToList();

                foreach (var dr in result)
                {
                    Payworksheet pw_item = new Payworksheet();
                    pw_item.appname = dr.appname;
                    //pw_item.app_difficulty = dr.app_difficulty.ToString() ;
                    pw_item.@base = dr.Base.ToString();
                    pw_item.calibration_score = dr.calibration_score.ToString();
                    pw_item.calltime = dr.calltime.ToString();
                    pw_item.cal_percent = dr.cal_percent.ToString();
                    pw_item.deduct = dr.deduct.ToString();
                    pw_item.efficiency = dr.efficiency.ToString();
                    pw_item.eff_percent = dr.eff_percent.ToString();

                    pw_item.num_calibrations = dr.num_calibrations.ToString();
                    pw_item.num_calls = dr.num_calls.ToString();
                    pw_item.num_disputes = dr.num_disputes.ToString();
                    pw_item.reviewer = dr.reviewer.ToString();
                    pw_item.reviewtime = dr.reviewtime.ToString();
                    pw_item.scorecard = dr.scorecard.ToString();
                    pw_item.sc_date = dr.sc_date.ToString();
                    pw_item.short_name = dr.short_name;
                    pw_item.startdate = dr.startdate.ToString();
                    pw_item.websites = dr.websites.ToString();
                    pw_item.website_pay = dr.website_pay.ToString();
                    pw.Add(pw_item);
                }
            }
            return pw;
        }
        #endregion Public getPayWorksheet

        #region Public getPayHistory
        /// <summary>
        /// getPayHistory
        /// </summary>
        /// <param name="PO"></param>
        /// <returns></returns>
        public List<PayHistory> GetPayHistory(payObj PO)
        {
            List<PayHistory> ch = new List<PayHistory>();

            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var result = dataContext.getPayHistory(PO.name).ToList();
                if (result.Any())
                {
                    var finalresult = (from res in result
                                       select new PayHistory
                                       {
                                           WeekEnding = res.week_ending.ToShortDateString(),
                                           Name = res.reviewer,
                                           Email = res.email_address,
                                           TotalPay = res.total_pay.ToString(),
                                           Scorecard = res.short_name
                                       }).ToList();
                    ch.AddRange(finalresult);
                }
            }
            return ch;
        }
        #endregion getPayHistory 

        #region getCalibrationHours
        /// <summary>
        /// getCalibrationHours
        /// </summary>
        /// <returns></returns>
        public List<CalibrationHours> getCalibrationHours()
        {
            List<CalibrationHours> ch = new List<CalibrationHours>();

            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var result = dataContext.getCalibrationHours().ToList();
                if (result.Any())
                {
                    var finalresult = (from res in result
                                       select new CalibrationHours
                                       {
                                           CalibPay = res.Calib_Pay,
                                           Calibrations = res.C__Calibrations.ToString(),
                                           NotifCompleted = res.Notif_Completed.ToString(),
                                           NotifPay = res.Notif_Pay,
                                           OffShiftHours = res.Off_Shift_Hours.ToString(),
                                           QALead = res.QA_Lead,
                                           Recal = res.C__Recal.ToString(),
                                           RecalPay = res.Recal_Pay__.ToString(),
                                           RecalScore = res.Recal_Score.ToString(),
                                           ReviewTime = res.Review_Time.ToString(),
                                           Scorecard = res.Scorecard.ToString(),
                                           Speed = res.Speed,
                                           TotalPay = res.Total_Pay,
                                           WebsitesReviewed = res.Websites_Reviewed.ToString(),
                                           WeekEnding = res.Week_Ending,
                                           email = res.email_address,
                                       }).ToList();
                    ch.AddRange(finalresult);
                }
            }
            return ch;

        }
        #endregion getCalibrationHours

        #region Public GetRecordID
        /// <summary>
        /// GetRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public AllCallRecord GetRecordID(string SI, string userName)
        {
            bool bad_call = false;
            List<ScorecardData> scorecardDataList = new List<ScorecardData>();
            AllCallRecord scr = new AllCallRecord();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int si = Convert.ToInt32(SI);
                var result = dataContext.getAllCallScorecards(si, userName).ToList();
                foreach (var item in result)
                {

                    switch (item.call_type.ToString())
                    {
                        case "CSCall":
                            {
                                scorecardDataList.Add(getRecordSCCSCall(item.call_id.ToString(), userName));
                                break;
                            }

                        case "Call":
                            {
                                scorecardDataList.Add(getRecordSCCall(item.call_id.ToString(), userName));
                                break;
                            }

                        case "Calibration":
                            {
                                //scorecardDataList.Add(getRecordSCCalibration(item.call_id.ToString(), userName));
                                break;
                            }

                        case "ClientCalibration":
                            {
                                //scorecardDataList.Add(getRecordSCClientCalibration(item.call_id.ToString(), userName));
                                break;
                            }

                        case "EditedCall":
                            {
                                //scorecardDataList.Add(getRecordSCEdited(item.call_id.ToString(), userName, item.call_username.ToString()));
                                break;
                            }
                    }
                }
                scr.ScorecardData = scorecardDataList;
                var resultScoredData = dataContext.getScorecardData(si).ToList();
                int review_id = 0;
                if (resultScoredData.Count > 0)
                {
                    review_id = Convert.ToInt32(resultScoredData.FirstOrDefault().review_ID);
                }
                if (!resultScoredData.Any())
                {
                    return scr;
                }
                foreach (var res in resultScoredData)
                {

                    scr.F_ID = res.F_ID.ToString();
                    scr.client_logo = res.client_logo.ToString();
                    scr.review_ID = res.review_ID.ToString();
                    scr.Comments = res.Comments.ToString();
                    scr.autofail = res.autofail.ToString();
                    scr.reviewer = res.reviewer.ToString();
                    scr.appname = res.appname.ToString();
                    scr.total_score = res.total_score.ToString();
                    scr.total_score_with_fails = res.total_score_with_fails.ToString();
                    scr.call_length = res.call_length.ToString();
                    scr.has_cardinal = res.has_cardinal.ToString();
                    scr.fs_audio = res.fs_audio;
                    scr.week_ending_date = res.week_ending_date.ToString();
                    scr.num_missed = res.num_missed.ToString();
                    scr.missed_list = res.missed_list.ToString();
                    scr.call_made_date = res.call_made_date.ToString();
                    scr.AGENT = res.AGENT.ToString();
                    scr.ANI = res.ANI;
                    scr.DNIS = res.DNIS;
                    scr.TIMESTAMP = res.TIMESTAMP;
                    scr.TALK_TIME = res.TALK_TIME;
                    scr.CALL_TIME = res.CALL_TIME.ToString();
                    scr.CALL_TYPE = res.CALL_TYPE;
                    scr.leadid = res.leadid;
                    scr.AGENT_GROUP = res.AGENT_GROUP;
                    scr.Email = res.Email;
                    scr.Email = res.Email;
                    scr.City = res.City;
                    scr.State = res.State;
                    scr.Zip = res.Zip;
                    scr.Datacapturekey = res.Datacapturekey.ToString();
                    scr.Datacapture = res.Datacapture.ToString();
                    scr.Status = res.Status;
                    scr.Program = res.Program;
                    scr.X_ID = res.X_ID.ToString();
                    scr.Datacapture_Status = res.Datacapture_Status;
                    scr.num_of_schools = res.num_of_schools;
                    scr.MAX_REVIEWS = res.MAX_REVIEWS.ToString();
                    scr.review_started = res.review_started.ToString();
                    scr.Number_of_Schools = res.Number_of_Schools;
                    scr.EducationLevel = res.EducationLevel;
                    scr.HighSchoolGradYear = res.HighSchoolGradYear;
                    scr.HighSchoolGradYear = res.HighSchoolGradYear;
                    scr.DegreeStartTimeframe = res.DegreeStartTimeframe;
                    scr.Expr3 = res.Expr3;
                    scr.First_Name = res.First_Name;
                    scr.First_Name = res.First_Name;
                    scr.Last_Name = res.Last_Name;
                    scr.address = res.address;
                    scr.phone = res.phone;
                    scr.call_date = res.call_date.ToString();
                    scr.audio_link = Common.GetAudioFileName(res);
                    scr.profile_id = res.profile_id;
                    scr.audio_user = "";
                    scr.audio_password = "";
                    scr.LIST_NAME = res.LIST_NAME;
                    scr.review_date = res.review_date.ToString();
                    scr.CAMPAIGN = res.CAMPAIGN;
                    scr.DISPOSITION = res.DISPOSITION;
                    scr.bad_call = res.bad_call.ToString();
                    scr.to_upload = "";
                    scr.SESSION_ID = res.SESSION_ID;
                    scr.agent_deviation = res.agent_deviation.ToString();
                    scr.pass_fail = res.pass_fail;
                    scr.scorecard = res.scorecard.ToString();
                    scr.scorecard_name = res.Scorecard_name;
                    scr.uploaded = "";
                    scr.formatted_comments = res.formatted_comments;
                    scr.formatted_missed = res.formatted_missed;
                    scr.fileUrl = res.fileUrl;
                    scr.statusMessage = res.statusMessage;
                    scr.mediaId = "";
                    scr.requestStatus = "";
                    scr.fileStatus = "";
                    scr.response = "";
                    scr.review_time = "";
                    //scr.wasEdited = res.wasEdited;

                    scr.website = CheckForS3(res.website);
                    scr.pending_id = "";
                    scr.bad_call_reason = res.bad_call_reason;
                    scr.date_added = res.date_added.ToString();
                    scr.calib_score = res.calib_score.ToString();
                    scr.edited_score = res.edited_score.ToString();
                    scr.compliance_sheet = res.compliance_sheet;
                    scr.notification_status = res.notification_status;
                    scr.review_type = res.review_type;
                    scr.editable = true;
                    string non_edit = "";

                    var isExist = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Trim()).Select(s => s.non_edit).FirstOrDefault();

                    //DataTable sup_dt = GetTable("select isnull(non_edit,0) as non_edit from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "'");
                    // Try
                    bool nonEdit = isExist.HasValue ? true : false;
                    if (nonEdit)
                        scr.editable = false;
                    if ((HttpContext.Current.User.IsInRole("QA") & (res.calib_score.ToString() != "" | res.edited_score.ToString() != "")) | non_edit == "True" | (HttpContext.Current.User.IsInRole("Agent")))
                        scr.editable = false;

                    if ((HttpContext.Current.User.IsInRole("QA") & userName != res.reviewer))
                        scr.editable = false;

                    if ((HttpContext.Current.User.IsInRole("calibrator") & userName == res.reviewer))
                        scr.editable = false;
                    if (bad_call)
                        scr.editable = false;
                    var clerkDataTemp = (from collected_data in dataContext.collected_data
                                         join sc_inputs in dataContext.sc_inputs on collected_data.value_id equals sc_inputs.id
                                         where collected_data.form_id == res.F_ID
                                         select new ClerkedData
                                         {
                                             value = sc_inputs.value,
                                             data = collected_data.value_data,
                                             position = collected_data.value_position.ToString(),
                                             ID = collected_data.value_id.ToString()
                                         }).ToList();
                    scr.ClerkedDataItems = new List<ClerkedData>();
                    scr.ClerkedDataItems.AddRange(clerkDataTemp);
                    //DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + res.F_ID.ToString());
                    List<string> audio_list = CDServiceLayer.GetAvailableAudios(review_id.ToString());
                    scr.audio_merge = audio_list;
                }

                var resultSchoolDataWithPos = dataContext.getSchoolDataWithPos(si, review_id).ToList();
                List<SchoolItem> school_items = new List<SchoolItem>();
                foreach (var school_dr in resultSchoolDataWithPos)
                {
                    SchoolItem school_item = new SchoolItem();
                    school_item.AOI1 = school_dr.AOI1;
                    school_item.AOI2 = school_dr.AOI2;
                    school_item.College = school_dr.College;
                    school_item.DegreeOfInterest = school_dr.DegreeOfInterest;
                    school_item.L1_SubjectName = school_dr.L1_SubjectName;
                    school_item.L2_SubjectName = school_dr.L2_SubjectName;
                    school_item.Modality = school_dr.Modality;
                    school_item.Portal = school_dr.origin;
                    school_item.School = school_dr.School;
                    school_item.TCPA = school_dr.tcpa;
                    school_item.id = school_dr.ID;
                    school_items.Add(school_item);
                }
                scr.SchoolData = school_items;

                var resultOtherData = dataContext.getotherformdata(review_id).ToList();
                List<OtherData> otherdata_items = new List<OtherData>();
                foreach (var school_dr in resultOtherData)
                {
                    OtherData otherdata_item = new OtherData();
                    otherdata_item.key = school_dr.data_key;
                    otherdata_item.label = school_dr.label;
                    otherdata_item.school = school_dr.school_name;
                    otherdata_item.type = school_dr.data_type;
                    otherdata_item.value = school_dr.data_value;
                    otherdata_item.id = school_dr.id.ToString();
                    otherdata_items.Add(otherdata_item);
                }

                scr.OtherData = otherdata_items;
                var resultCombinedComments = dataContext.getCombinedComments(si, Roles.GetRolesForUser(userName).Single()).ToList();
                List<DisputeData> disputes = new List<DisputeData>();
                foreach (var dispute_dr in resultCombinedComments)
                {
                    DisputeData dispute = new DisputeData();
                    dispute.closed = dispute_dr.date_closed.ToString();
                    dispute.comment = dispute_dr.comment;
                    dispute.created = dispute_dr.date_created.ToString();
                    dispute.role = dispute_dr.role + " - " + dispute_dr.closed_by;
                    dispute.user = dispute_dr.closed_by;
                    dispute.id = dispute_dr.fn_id;
                    disputes.Add(dispute);
                }

                if (disputes.Count > 0)
                {
                    scr.Disputes = disputes;
                }
                //Common.UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" + username + "',dbo.getMTDate(), (select review_id from form_score3 with  (nolock)  where id = " + ID + "),'review'");
                var resultform_score = dataContext.form_score3.Where(x => x.id == si).FirstOrDefault();
                session_viewed objsession_viewed = new session_viewed();
                if (resultform_score == null)
                {
                    int sessionId = Convert.ToInt32(resultform_score.session_id);
                    // Save form_score3 data
                    objsession_viewed.session_id = sessionId;
                    objsession_viewed.page_viewed = "review";
                    objsession_viewed.agent = userName;
                    dataContext.session_viewed.Add(objsession_viewed);
                    int result1 = dataContext.SaveChanges();

                }

                //reply = new SqlCommand("select * from session_viewed with  (nolock) join userextrainfo on userextrainfo.username = agent  where session_id = '" + review_id + "' order by date_viewed");
                var clerksession_viewed = (from session_viewed in dataContext.session_viewed
                                           join userExtraInfoes in dataContext.UserExtraInfoes on session_viewed.agent equals userExtraInfoes.username
                                           where session_viewed.session_id == review_id
                                           select new SessionViews
                                           {
                                               view_action = session_viewed.page_viewed,
                                               view_by = session_viewed.agent,
                                               view_date = session_viewed.date_viewed.ToString(),
                                               view_role = userExtraInfoes.user_role
                                           }).ToList();
                scr.sessions_viewed = new List<SessionViews>();
                scr.sessions_viewed.AddRange(clerksession_viewed);

                if (scr.sessions_viewed.Count > 0)
                {
                    scr.sessions_viewed = clerksession_viewed;
                }
                CDServiceLayer cdservice = new CDServiceLayer();
                List<string> rej_list = new List<string>();
                int scoreCard = 0;
                if (scr.scorecard != null)
                {
                    scoreCard = Convert.ToInt32(scr.scorecard);
                }
                var resultRejectionReasons = dataContext.getRejectionReasons(scoreCard).ToList();

                foreach (var res in resultRejectionReasons)
                {
                    rej_list.Add(res.reason);
                }
                // DataTable rej_dt = GetTable("select * from rejection_reasons where profile_id = (select isnull(scorecards.rejection_profile,isnull(app_settings.rejection_profile, 1)) from app_settings join scorecards on scorecards.appname = app_settings.appname where scorecards.id = " + scr.scorecard + ")");
                scr.rejection_reasons = rej_list;
                List<string> buttons = cdservice.GetNotificationSteps(si.ToString()).Split('|').ToList();
                if (buttons.Count > 0)
                    scr.dispute_buttons = buttons;
                CDServiceLayer objCDService = new CDServiceLayer();
                List<ActionButton> abs = objCDService.GetActionButtons(userName, si.ToString());
                if (abs.Count > 0)
                {
                    scr.ActionButtons = abs;
                }
                var resultspotcheck = dataContext.spotchecks.Where(x => x.f_id == si && x.checked_date == null).ToList();
                //reply = new SqlCommand("Select * from spotcheck where f_id = @id and checked_date is null");
                if (resultspotcheck.Count > 0)
                {
                    if (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Tango TL"))
                    {
                        scr.showSpotCheck = true;
                        scr.spotCheckReason = resultspotcheck.FirstOrDefault().check_reason;
                    }
                    else
                    {
                        scr.showSpotCheck = false;
                    }
                    if ("'QAL_MichelleNoriega','John Charles Depano','JaniceRecamadas','cc_dc_jonelsim', 'CC_DC_IreneMacasero'".ToUpper().IndexOf(userName.ToUpper()) > 0 & HttpContext.Current.User.IsInRole("QA"))
                    {
                        scr.showSpotCheck = true;
                        scr.spotCheckReason = resultspotcheck.FirstOrDefault().check_reason;
                    }
                }
                else
                    scr.showSpotCheck = false;

                var resultcalipendingclient = dataContext.cali_pending_client.Where(x => x.form_id == si && x.date_completed == null).ToList();
                //reply = new SqlCommand("select assigned_to from cali_pending_client where date_completed is null and form_id = @id");
                List<string> incom_list = new List<string>();

                foreach (var calipendingclient in resultcalipendingclient)
                {
                    incom_list.Add(calipendingclient.assigned_to);
                    scr.incomplete_calibrators = incom_list;
                }
                return scr;
            }
        }
        #endregion GetRecordID 

        #region Private getRecordSCCSCall
        /// <summary>
        /// getRecordSCCSCall
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private ScorecardData getRecordSCCSCall(string f_id, string username)
        {

            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int fId = 0;
                if (f_id != "" && f_id != null)
                {
                    fId = Convert.ToInt32(f_id);
                }
                var getPopulatedScorecard = getRecordSCCSCall1(fId, username).ToList();
                //SqlCommand sq = new SqlCommand("getCompletedSCCSCall");
                ScorecardData scd = populateScorecard(getPopulatedScorecard);

                app_settings_Result dr = getPopulatedScorecard[4].Cast<app_settings_Result>().FirstOrDefault();
                //DataRow dr = ds.Tables[4].Rows[0];

                UserObject scu = new UserObject();
                scu.UserRole = "QA";
                scu.UserTitle = "QA Response";
                scu.isGolden = false;
                scd.ScorecardUser = scu;
                CallScores cs = new CallScores();

                if (dr.edited_score.ToString() == "")
                {
                    cs.score = Convert.IsDBNull(dr.display_score) ? "N/A" : dr.display_score.ToString();
                }
                else
                {
                    cs.score = Convert.IsDBNull(dr.original_qa_score) ? (Convert.IsDBNull(dr.total_score) ? "N/A" : dr.total_score.ToString()) : dr.original_qa_score.ToString();
                }
                cs.reviewer = dr.reviewer.ToString();
                cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                if (!Convert.IsDBNull(dr.qa_cali_score))
                {
                    cs.calibrationscore = dr.qa_cali_score.ToString();
                }
                cs.role = "QA";
                scd.CallScore = cs;
                return scd;
            }
        }
        #endregion  getRecordSCCSCall

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<IEnumerable> getRecordSCCSCall1(int f_id, string username)
        {
            try
            {
                List<IEnumerable> result = new List<IEnumerable>();
                //second way to get multiple result set in entity
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var command = new SqlCommand()
                    {
                        CommandText = "[dbo].[getCompletedSCCSCall]",
                        CommandType = CommandType.StoredProcedure
                    };

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@f_id",f_id),
                        new SqlParameter("@username",username)

                    };

                    command.Parameters.AddRange(parameters.ToArray());

                    result = dataContext.MultipleResults(command)
                                   .With<getCompletedSCCSCall_Result>()
                                   .With<form_q_scores_Result>()
                                   .With<vwFOrm_Result>()
                                   .With<getTemplateItemsAll_Result>()
                                   .With<app_settings_Result>()

                                   .Execute();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }


        #region Private populateScorecard
        /// <summary>
        /// populateScorecard
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private ScorecardData populateScorecard(List<IEnumerable> ds)
        {
            ScorecardData scd = new ScorecardData();
            //DataTable section_dt = ds.Tables[0];
            //DataTable qdt = ds.Tables[1];
            //DataTable ans_dt = ds.Tables[2];
            //DataTable temp_dt = ds.Tables[3];
            List<getCompletedSCCSCall_Result> section_dt = ds[0].Cast<getCompletedSCCSCall_Result>().ToList();
            List<form_q_scores_Result> qdt = ds[1].Cast<form_q_scores_Result>().ToList();
            List<vwFOrm_Result> ans_dt = ds[2].Cast<vwFOrm_Result>().ToList();
            List<getTemplateItemsAll_Result> temp_dt = ds[3].Cast<getTemplateItemsAll_Result>().ToList();
            List<SectionData> sections = new List<SectionData>();
            foreach (var section_dr in section_dt)
            {
                SectionData section_data = new SectionData();
                section_data.SectionTitle = section_dr.section;
                List<ScorecardResponse> qrs = new List<ScorecardResponse>();

                //filteredq_dt = qdt.Select("section_id = " + section_dr["ID"]);
                var qdt1 = qdt.Where(x => x.section_id == section_dr.ID).ToList();
                foreach (var qdr in qdt1)
                {
                    ScorecardResponse qr = new ScorecardResponse();
                    qr.position = qdr.q_position;
                    qr.question = qdr.q_short_name;
                    qr.result = qdr.answer_text;
                    qr.QID = Convert.ToInt32(qdr.q_id);
                    qr.sentence = qdr.sentence;
                    qr.OptionVerb = qdr.options_verb;
                    qr.LeftColumnQuestion = Convert.ToBoolean(qdr.left_column_question);
                    qr.QAPoints = Convert.ToInt32(qdr.QA_points);

                    qr.QType = qdr.q_type;
                    qr.ViewLink = qdr.view_link;
                    qr.comments_allowed = Convert.ToBoolean(qdr.comments_allowed);
                    if (qdr.right_answer.ToString() == "")
                    {
                        qr.RightAnswer = true;
                    }
                    else
                    {
                        qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                    }
                    List<Comment> cmt_list = new List<Comment>();
                    try
                    {
                        //filteredans_dt = ans_dt.Select("question_id = '" + qdr.q_id.ToString() + "'");
                        var filteredans_dt = ans_dt.Where(x => x.question_id == qdr.q_id).ToList();
                        if (filteredans_dt.Count() > 0)
                        {
                            List<string> ans_comment = new List<string>();
                            foreach (var ans_dr in filteredans_dt)
                            {
                                ans_comment.Add(ans_dr.comment);
                                Comment cmt = new Comment();

                                //cmt.CommentWho = ans_dr.changed_by.ToString();
                                //cmt.CommentDate = ans_dr.changed_date.ToString();

                                cmt.CommentText = ans_dr.comment;
                                cmt.CommentID = Convert.ToInt32(ans_dr.id);
                                //cmt.CommentPoints = Convert.IsDBNull(ans_dr["comment_points"]) ? new float() : (float)(ans_dr["comment_points"]);

                                //cmt.NavigateQuestion = ans_dr["origin_qid"].ToString();
                                cmt_list.Add(cmt);
                            }
                            qr.QComments = ans_comment;
                            qr.SCRComments = cmt_list;
                        }

                        //DataRow[] filteredtemp_dt;
                        //filteredtemp_dt = temp_dt.Select("question_id = " + qdr["q_id"]);
                        var filteredtemp_dt = temp_dt.Where(x => x.question_id == qdr.q_id).ToList();
                        if (filteredtemp_dt.Count() > 0)
                        {
                            List<CheckItems> temp_items = new List<CheckItems>();
                            foreach (var temp_dr in filteredtemp_dt)
                            {
                                CheckItems temp_item = new CheckItems();
                                if (temp_dr.value.Trim() == temp_dr.option_value.Trim())
                                {
                                    temp_item.Checked = true;
                                }
                                else
                                {
                                    temp_item.Checked = false;
                                }
                                temp_item.Item = temp_dr.value;
                                temp_item.Position = temp_dr.option_pos.ToString();
                                temp_item.ID = temp_dr.id.ToString();
                                temp_items.Add(temp_item);
                            }
                            qr.QTemplate = temp_items;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    qrs.Add(qr);
                }
                section_data.QList = qrs;
                sections.Add(section_data);
            }
            scd.Sections = sections;

            return scd;
        }
        #endregion populateScorecard

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<IEnumerable> getRecordSCCallComplexType(int f_id, string username)
        {
            try
            {
                List<IEnumerable> result = new List<IEnumerable>();
                //second way to get multiple result set in entity
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var command = new SqlCommand()
                    {
                        CommandText = "[dbo].[getCompletedSCCSCall]",
                        CommandType = CommandType.StoredProcedure
                    };

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@f_id",f_id),
                        new SqlParameter("@username",username)

                    };

                    command.Parameters.AddRange(parameters.ToArray());

                    result = dataContext.MultipleResults(command)
                                   .With<getCompletedSCCSCall_Result>()
                                   .With<form_q_scores_Result>()
                                   .With<vwFOrm_Result>()
                                   .With<getTemplateItemsAll_Result>()
                                   .With<app_settings_Result>()
                                   .Execute();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        #region Private getRecordSCCall
        /// <summary>
        /// getRecordSCCall
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private ScorecardData getRecordSCCall(string f_id, string username)
        {
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int fId = 0;
                if (f_id != "" && f_id != null)
                {
                    fId = Convert.ToInt32(f_id);
                }
                var getPopulatedScorecard = getRecordSCCallComplexType(fId, username).ToList();
                ScorecardData scd = populateScorecard(getPopulatedScorecard);
                app_settings_Result dr = getPopulatedScorecard[4].Cast<app_settings_Result>().FirstOrDefault();
                //SqlCommand sq = new SqlCommand("getCompletedSCCall");
                //DataRow dr = ds.Tables[4].Rows[0];
                UserObject scu = new UserObject();
                scu.UserRole = "QA";
                scu.UserTitle = "QA Response";
                scu.isGolden = false;
                scd.ScorecardUser = scu;
                CallScores cs = new CallScores();
                if (dr.calib_score.ToString() == "" & dr.edited_score.ToString() == "")
                {

                    cs.score = Convert.IsDBNull(dr.display_score) ? "N/A" : dr.display_score.ToString();
                }
                else
                {
                    cs.score = Convert.IsDBNull(dr.original_qa_score) ? (Convert.IsDBNull(dr.total_score) ? "N/A" : dr.total_score.ToString()) : dr.original_qa_score.ToString();
                }
                cs.reviewer = dr.reviewer.ToString();
                cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                if (!Convert.IsDBNull(dr.qa_cali_score))
                {
                    cs.calibrationscore = dr.qa_cali_score.ToString();
                }
                cs.role = "QA";
                scd.CallScore = cs;
                return scd;
            }
        }
        #endregion  getRecordSCCall


        //#region Private getRecordSCCall
        ///// <summary>
        ///// getRecordSCCalibration
        ///// </summary>
        ///// <param name="f_id"></param>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //private ScorecardData getRecordSCCalibration(string f_id, string username)
        //{
        //    SqlCommand sq = new SqlCommand("getCompletedSCCalibration");
        //    sq.CommandType = CommandType.StoredProcedure;
        //    sq.Parameters.AddWithValue("id", f_id);
        //    sq.Parameters.AddWithValue("username", username);
        //    DataSet ds = Common.getTables(sq);
        //    ScorecardData scd = populateScorecard(ds);
        //    DataRow dr = ds.Tables[4].Rows[0];
        //    UserObject scu = new UserObject();
        //    if (dr["active_cali"].ToString() == "1" & Convert.ToInt32(dr["golden_count"]) == 0)
        //        scu.isGolden = true;
        //    else
        //        scu.isGolden = false;

        //    if (dr["user_role"].ToString() == "Calibrator")
        //        scu.UserTitle = "Calibrator";
        //    else
        //        scu.UserTitle = "Recalibrator";
        //    scu.UserRole = dr["user_role"] + " Response";
        //    scd.ScorecardUser = scu;
        //    CallScores cs = new CallScores();
        //    cs.score = Convert.IsDBNull(dr["cali_form_score"]) ? "0" : dr["cali_form_score"].ToString();
        //    cs.reviewer = dr["reviewed_by"].ToString();
        //    if (dr["user_role"].ToString() == "Calibrator")
        //        cs.calibrationscore = dr["cal_recal_score"].ToString();
        //    else
        //        cs.calibrationscore = dr["total_score"].ToString();
        //    cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToString();
        //    cs.role = dr["user_role"].ToString();
        //    scd.CallScore = cs;
        //    return scd;
        //}
        //#endregion  getRecordSCCall

        //#region Private getRecordSCClientCalibration
        ///// <summary>
        ///// getRecordSCClientCalibration
        ///// </summary>
        ///// <param name="f_id"></param>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //private ScorecardData getRecordSCClientCalibration(string f_id, string username)
        //{
        //    SqlCommand sq = new SqlCommand("getCompletedSCClientCalibration");
        //    sq.CommandType = CommandType.StoredProcedure;
        //    sq.Parameters.AddWithValue("id", f_id);
        //    sq.Parameters.AddWithValue("username", username);
        //    DataSet ds = Common.getTables(sq);
        //    ScorecardData scd = populateScorecard(ds);
        //    DataRow dr = ds.Tables[4].Rows[0];
        //    UserObject scu = new UserObject();

        //    scu.UserRole = dr["user_role"] + " - Client Calib";
        //    scu.UserTitle = dr["reviewed_by"].ToString();
        //    if (dr["golden"] == dr["reviewed_by"])
        //        scu.isGolden = true;
        //    else
        //        scu.isGolden = false;
        //    CallScores cs = new CallScores();
        //    cs.score = Convert.IsDBNull(dr["total_score"]) ? "0" : dr["total_score"].ToString();
        //    cs.calibrationscore = Convert.IsDBNull(dr["cali_form_score"]) ? "0" : dr["cali_form_score"].ToString();
        //    cs.reviewer = dr["reviewed_by"].ToString();
        //    cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToShortDateString();
        //    cs.role = dr["user_role"].ToString();
        //    scd.CallScore = cs;
        //    scd.ScorecardUser = scu;
        //    return scd;
        //}
        //#endregion getRecordSCClientCalibration

        #region Private getRecordSCEdited
        /// <summary>
        /// getRecordSCEdited
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="username"></param>
        /// <param name="call_username"></param>
        /// <returns></returns>
        //private ScorecardData getRecordSCEdited(string f_id, string username, string call_username)
        //{
        //    SqlCommand sq = new SqlCommand("getCompletedSCEdited");
        //    sq.CommandType = CommandType.StoredProcedure;
        //    sq.Parameters.AddWithValue("f_id", f_id);
        //    sq.Parameters.AddWithValue("username", username);
        //    sq.Parameters.AddWithValue("call_username", call_username);

        //    DataSet ds = Common.getTables(sq);
        //    ScorecardData scd = populateScorecard(ds);
        //    DataRow dr = ds.Tables[4].Rows[0];
        //    UserObject scu = new UserObject();
        //    scu.UserRole = dr["user_role"].ToString();
        //    scu.UserTitle = dr["username"] + " (Edit)";

        //    CallScores cs = new CallScores();
        //    cs.score = Convert.IsDBNull(dr["total_score"]) ? "0" : dr["total_score"].ToString();
        //    cs.reviewer = dr["username"].ToString();
        //    cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToShortDateString();
        //    cs.role = dr["user_role"].ToString();
        //    scd.CallScore = cs;
        //    scd.ScorecardUser = scu;

        //    return scd;
        //}
        #endregion  getRecordSCEdited

        #region Private CheckForS3
        /// <summary>
        /// CheckForS3
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private string CheckForS3(string link)
        {
            if (link != null)
            {
                if (Strings.Left(link.ToLower(), 3) == "s3:")
                {
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.USWest2);
                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();
                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = Strings.Left(link, link.LastIndexOf("/")).Replace("s3://", "");
                    URL_REQ.Key = link.Substring(link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }
                else
                {
                    return link;
                }
            }
            else
            {
                return link;
            }
        }
        #endregion  CheckForS3

        /// <summary>
        /// AcceptAsBad
        /// </summary>
        /// <param name="x_id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string AcceptAsBad(int x_id, string user)
        {
            try
            {
                int result = 0;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //Common.UpdateTable("update xcc_report_new set bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" + user + "' where id = '" + x_id + "'");
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == x_id).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(x_id);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.bad_call_accepted = System.DateTime.Now;
                        tblXCC_REPORT_NEW.bad_call_accepted_who = user;
                        result = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("exec makeBlankForm '" + x_id + "'");

                    //var isExist1 = dataContext.makeBlankForm(x_id);

                    //Common.UpdateTable("update vwForm set review_date = bad_call_date, calib_score = null, total_score=null, total_score_with_fails = null, display_score = null,  pass_fail='N/A',original_qa_score=null where review_id = '" + x_id + "'");
                    var isExist2 = dataContext.vwForms.Where(x => x.review_ID == x_id).FirstOrDefault();
                    var tblscore = dataContext.form_score3.Where(x => x.review_ID == x_id).FirstOrDefault();
                    form_score3 tblscore3 = new form_score3();
                    if (tblscore != null)
                    {
                        tblscore3 = dataContext.form_score3.Find(tblscore.id);
                        dataContext.Entry(tblscore3).State = EntityState.Modified;
                        tblscore3.review_date = System.DateTime.Now;
                        tblscore3.calib_score = null;
                        tblscore3.total_score = null;
                        tblscore3.total_score_with_fails = null;
                        tblscore3.display_score = null;
                        tblscore3.pass_fail = "N/A";
                        tblscore3.original_qa_score = null;
                        int resultform_score3 = dataContext.SaveChanges();
                    }

                    //Common.UpdateTable("insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" + user + "', dbo.getMTDate(), 'Call marked as bad: ' + (select top 1 bad_call_reason from xcc_report_new with  (nolock)  where id = " + x_id + "),(select top 1 f_id from vwForm with  (nolock)  where review_id  = " + x_id + "),'Call'");
                    ObjectParameter ReturnedValue = new ObjectParameter("ReturnValue", typeof(int));
                    int ReturnedValue1 = dataContext.insertSystemComments(x_id, user, ReturnedValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Messages.Updated;
        }


        /// <summary>
        /// GetRecordXID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AllCallRecord GetRecordXID(int ID)
        {
            AllCallRecord scr = new AllCallRecord();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                string username = HttpContext.Current.User.Identity.Name;
                string appname = HttpContext.Current.Request.QueryString["appname"];
                //SqlCommand reply = new SqlCommand("select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username", cn);
                //reply.Parameters.AddWithValue("username", username);
                string non_edit = "";
                var isUserExtraInfoes = dataContext.UserExtraInfoes.Where(x => x.username == username).FirstOrDefault();
                UserObject objUserObject = new UserObject();
                if (isUserExtraInfoes != null)
                {
                    non_edit = isUserExtraInfoes.non_edit.ToString();
                    objUserObject.UserRole = isUserExtraInfoes.user_role;
                    objUserObject.UserName = isUserExtraInfoes.username;
                    objUserObject.SpeedInc = isUserExtraInfoes.speed_increment.ToString();
                    objUserObject.StartImmediately = Convert.ToBoolean(isUserExtraInfoes.calls_start_immediately);
                    scr.UserInfo = objUserObject;
                }

                var isxcc_report_new = dataContext.getXccReportNew(ID, appname, username).ToList();
                //reply = new SqlCommand("select xcc_report_new.*, xcc_report_new.id as X_ID,xcc_report_new.id as review_id, recording_user,record_password,url_prefix,  isnull(client_logo, xcc_report_new.appname) as client_logo, 0 as qa_cali_score   from xcc_report_new join app_settings on xcc_report_new.appname = app_settings.appname where xcc_report_new.id =  @id" + add_sql, cn);

                if (isxcc_report_new.Count == 0)
                {
                    return scr;
                }
                int review_id = Convert.ToInt32(isxcc_report_new.FirstOrDefault().review_id);
                List<ScorecardData> scds = new List<ScorecardData>();

                foreach (var Res in isxcc_report_new)
                {
                    // 
                    scr.F_ID = "";
                    scr.client_logo = Res.client_logo;
                    scr.review_ID = Res.review_id.ToString();
                    scr.appname = Res.appname.ToString();
                    scr.call_length = Res.call_duration.ToString();
                    scr.AGENT = Res.AGENT;
                    scr.ANI = Res.ANI;
                    scr.DNIS = Res.DNIS;
                    scr.TIMESTAMP = Res.TIMESTAMP;
                    scr.TALK_TIME = Res.TALK_TIME;
                    scr.CALL_TIME = Res.CALL_TIME.ToString();
                    scr.CALL_TYPE = Res.CALL_TYPE;
                    scr.leadid = Res.leadid;
                    scr.AGENT_GROUP = Res.AGENT_GROUP;
                    scr.Email = Res.Email.ToString();
                    scr.City = Res.City.ToString();
                    scr.State = Res.State.ToString();
                    scr.Zip = Res.Zip.ToString();
                    scr.Datacapturekey = Res.Datacapturekey.ToString();
                    scr.Datacapture = Res.Datacapture.ToString();
                    scr.Status = Res.Status;
                    scr.Program = Res.Program;
                    scr.X_ID = ID.ToString();
                    scr.Datacapture_Status = Res.Datacapture_Status;
                    scr.num_of_schools = Res.num_of_schools;
                    scr.MAX_REVIEWS = Res.MAX_REVIEWS.ToString();
                    scr.Number_of_Schools = Res.Number_of_Schools;
                    scr.EducationLevel = Res.EducationLevel;
                    scr.HighSchoolGradYear = Res.HighSchoolGradYear;
                    scr.DegreeStartTimeframe = Res.DegreeStartTimeframe;
                    scr.First_Name = Res.First_Name;
                    scr.Last_Name = Res.Last_Name;
                    scr.address = Res.address;
                    scr.phone = Res.phone;
                    scr.call_date = Res.call_date.ToString();
                    //scr.audio_link = Common.GetAudioFileName(dr);
                    scr.profile_id = Res.profile_id;
                    scr.audio_user = Res.audio_user;
                    scr.audio_password = Res.audio_password;
                    scr.LIST_NAME = Res.LIST_NAME;
                    scr.CAMPAIGN = Res.CAMPAIGN;
                    scr.DISPOSITION = Res.DISPOSITION;
                    scr.bad_call = Res.bad_call.ToString();
                    scr.SESSION_ID = Res.SESSION_ID;
                    scr.scorecard = Res.scorecard.ToString();
                    scr.fileUrl = Res.fileUrl;
                    scr.statusMessage = Res.statusMessage;
                    scr.mediaId = Res.mediaId;
                    scr.requestStatus = Res.requestStatus;
                    scr.fileStatus = Res.fileStatus;
                    scr.response = Res.response;
                    scr.website = Res.website;
                    scr.pending_id = Res.pending_id.ToString();
                    scr.bad_call_reason = Res.bad_call_reason;
                    scr.date_added = Res.date_added.ToString();
                    scr.editable = true;
                    List<string> audio_list = new List<string>();

                    scr.ScorecardData = scds;
                    var isSchoolDataWithPos = dataContext.getSchoolDataWithPos(ID, review_id).ToList();
                    // reply = new SqlCommand("getSchoolDataWithPos @ID,  @xcc_id", cn);

                    List<SchoolItem> objSchoolItems = new List<SchoolItem>();
                    foreach (var school_dr in isSchoolDataWithPos)
                    {
                        SchoolItem schoolItem = new SchoolItem();
                        schoolItem.AOI1 = school_dr.AOI1;
                        schoolItem.AOI2 = school_dr.AOI2;
                        schoolItem.College = school_dr.College;
                        schoolItem.DegreeOfInterest = school_dr.DegreeOfInterest;
                        schoolItem.L1_SubjectName = school_dr.L1_SubjectName;
                        schoolItem.L2_SubjectName = school_dr.L2_SubjectName;
                        schoolItem.Modality = school_dr.Modality;
                        schoolItem.Portal = school_dr.origin;
                        schoolItem.School = school_dr.School;
                        schoolItem.TCPA = school_dr.tcpa;
                        schoolItem.id = school_dr.ID;
                        objSchoolItems.Add(schoolItem);
                    }
                    scr.SchoolData = objSchoolItems;

                    //reply = new SqlCommand("exec getotherformdata @xcc_id", cn);
                    var isOtherFormDatas = dataContext.getotherformdata(review_id).ToList();
                    List<OtherData> objOtherdataItems = new List<OtherData>();
                    foreach (var school_dr in isOtherFormDatas)
                    {
                        OtherData otherdata_item = new OtherData();
                        otherdata_item.key = school_dr.data_key;
                        otherdata_item.label = school_dr.label;
                        otherdata_item.school = school_dr.school_name;
                        otherdata_item.type = school_dr.data_type;
                        otherdata_item.value = school_dr.data_value;
                        otherdata_item.id = school_dr.id.ToString();
                        objOtherdataItems.Add(otherdata_item);
                    }
                    scr.OtherData = objOtherdataItems;
                    var isCombinedComments = dataContext.getCombinedComments(ID, username).ToList();
                    //reply = new SqlCommand("select * from session_viewed with  (nolock) join userextrainfo on userextrainfo.username = agent  where session_id = '" + review_id + "' order by date_viewed", cn);
                    var clerksession_viewed = (from session_viewed in dataContext.session_viewed
                                               join userExtraInfoes in dataContext.UserExtraInfoes on session_viewed.agent equals userExtraInfoes.username
                                               where session_viewed.session_id == review_id
                                               select new SessionViews
                                               {
                                                   view_action = session_viewed.page_viewed,
                                                   view_by = session_viewed.agent,
                                                   view_date = session_viewed.date_viewed.ToString(),
                                                   view_role = userExtraInfoes.user_role
                                               }).ToList();
                    scr.sessions_viewed = new List<SessionViews>();
                    scr.sessions_viewed.AddRange(clerksession_viewed);

                    if (clerksession_viewed.Count > 0)
                        scr.sessions_viewed = clerksession_viewed;
                }
            }
            return scr;
        }


        #region Public GetUserData
        /// <summary>
        /// GetUserData
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserObject GetUserData(string userName)
        {
            UserObject objUserObject = new UserObject();
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //SqlCommand reply = new SqlCommand("select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username", cn);
                    var isUserExtraInfo = dataContext.UserExtraInfoes.Where(x => x.username == userName).FirstOrDefault();
                    string nonEdit = "";
                    if (isUserExtraInfo != null)
                    {
                        nonEdit = isUserExtraInfo.non_edit.ToString();
                        objUserObject.UserRole = isUserExtraInfo.user_role.ToString();
                        objUserObject.UserName = isUserExtraInfo.username.ToString();
                        objUserObject.SpeedInc = isUserExtraInfo.speed_increment.ToString();
                        objUserObject.StartImmediately = Convert.ToBoolean(isUserExtraInfo.calls_start_immediately);
                        //DataTable links_dt =Common.GetTable("exec getMyMenu '" + userName + "'");
                        var isLinks = dataContext.getMyMenu(userName).ToList();
                        if (isLinks.Count > 0)
                        {
                            List<LinkList> links = new List<LinkList>();
                            foreach (var link in isLinks)
                            {
                                LinkList ll = new LinkList();
                                ll.LinkText = link.link.ToString();
                                ll.LinkURL = link.url.ToString();
                                ll.LinkSpan = link.span_data.ToString();
                                links.Add(ll);
                            }
                            objUserObject.UserLinks = links;
                        }
                        var isModule = dataContext.getMyModules(userName).ToList();
                        //DataTable module_dt = GetTable("exec getMyModules '" + userName + "'");
                        if (isModule.Count > 0)
                        {
                            List<UserModule> ml = new List<UserModule>();
                            foreach (var item in isModule)
                            {
                                UserModule um = new UserModule();
                                if (item.plus_minus.ToString() == "minus")
                                {
                                    um.module_active = true;
                                }
                                else
                                {
                                    um.module_active = false;
                                }
                                um.module_function = item.moduleFunction.ToString();
                                um.module_order = Convert.ToInt32(item.controlorder);
                                um.module_title = item.moduleName.ToString();
                                um.module_width = item.moduleWidth.ToString();
                                ml.Add(um);
                            }
                            objUserObject.modules = ml;
                        }
                        List<MyScorecards> linksList = new List<MyScorecards>();
                        var clerksession_viewed = (from UserApps in dataContext.UserApps
                                                   join scorecards in dataContext.scorecards on UserApps.user_scorecard equals scorecards.id
                                                   where UserApps.username == userName
                                                   select new MyScorecards
                                                   {
                                                       scorecard = scorecards.id,
                                                       scorcard_name = scorecards.short_name,
                                                       scorecard_role = UserApps.scorecard_role,
                                                       scorecard_appname = scorecards.appname
                                                   }).ToList();

                        linksList = new List<MyScorecards>();
                        linksList.AddRange(clerksession_viewed);
                        // DataTable sc_dt =Common.GetTable("select scorecards.id, short_name, scorecard_role, userapps.appname from userapps join scorecards on user_scorecard = scorecards.id where username = '" + userName + "' and active = 1 order by short_name");
                    }
                }
            return objUserObject;
        }
        #endregion GetUserData

        #region GetCallsLoaded
        /// <summary>
        /// GetCallsLoaded
        /// </summary>
        /// <param name="CL"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public List<CallLoaded> GetCallsLoaded(CallsLoaded CL, string appName)
        {
            List<CallLoaded> myCLs = new List<CallLoaded>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                if (Information.IsDate(CL.loaded_date))
                {
                    DateTime loadedDate = Convert.ToDateTime(CL.loaded_date);
                    var isXCCREPORT_NEW = dataContext.XCC_REPORT_NEW.Where(x => x.call_date == loadedDate && x.appname == appName).ToList();
                    //DataTable dt = GetTable("select * from xcc_report_new where call_date = '" + CL.loaded_date + "' and appname = '" + appName + "'");
                    foreach (var item in isXCCREPORT_NEW)
                    {
                        CallLoaded call_loaded = new CallLoaded();
                        call_loaded.session_id = item.SESSION_ID.ToString();
                        call_loaded.phone = item.phone.ToString();
                        call_loaded.call_date = item.call_date.ToString();
                        call_loaded.date_added = item.date_added.ToString();
                        call_loaded.audio_link = item.audio_link.ToString();
                        call_loaded.CC_ID = item.ID.ToString();
                        if (item.MAX_REVIEWS.ToString() == "0")
                        {
                            call_loaded.status = "Pending";
                        }
                        if (item.MAX_REVIEWS.ToString() == "1")
                        {
                            call_loaded.status = "Worked";
                        }
                        if (item.bad_call.ToString() == "1")
                        {
                            call_loaded.status = "Bad Call";
                        }
                        myCLs.Add(call_loaded);
                    }
                }
            }
            return myCLs;
        }

        #endregion GetCallsLoaded

        #region GetCallsLoaded
        /// <summary>
        /// GetTrainingReview
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public TrainingCallRecord GetTrainingReview(ReviewSimpleID SI)
        {
            int ID = 0;
            if (SI.review_id != "")
            {
                ID = Convert.ToInt32(SI.review_id);
            }
            TrainingCallRecord tcr = new TrainingCallRecord();
            AllCallRecord scr = new AllCallRecord();
            string username = HttpContext.Current.User.Identity.Name;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var isUserExtraInfoes = dataContext.UserExtraInfoes.Where(x => x.username == username).ToList();
                //SqlCommand reply = new SqlCommand("select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username", cn);

                //reply = new SqlCommand("select vwTrain.*,isnull(client_logo, vwTrain.appname) as client_logo, (select total_score from vwCF where active_cali = 1 and f_id = @id) as qa_cali_score  from vwTrain join app_settings on vwTrain.appname = app_settings.appname where vwTrain.id = @id", cn);
                var isgetvwTrain = dataContext.getvwTrain(ID).ToList();
                if (isgetvwTrain.Count == 0)
                {
                    return tcr;
                }
                int calib_id = 0;
                int review_id = Convert.ToInt32(isgetvwTrain.FirstOrDefault().review_ID);
                int f_id = 0;
                string reviewer = "";
                string scorecard = "";
                List<ScorecardData> scds = new List<ScorecardData>();
                foreach (var item in isgetvwTrain)
                {
                    scr.F_ID = item.F_ID.ToString();
                    scr.client_logo = item.client_logo.ToString();
                    scr.review_ID = item.review_ID.ToString();
                    scr.Comments = item.Comments;
                    scr.autofail = item.autofail;
                    scr.reviewer = item.reviewer;
                    reviewer = item.reviewer;
                    scr.appname = item.appname;
                    scr.total_score = item.trainee_score.ToString();
                    scr.total_score_with_fails = item.trainee_score.ToString();
                    scr.call_length = item.call_length.ToString();
                    scr.has_cardinal = item.has_cardinal.ToString();
                    scr.fs_audio = item.fs_audio;
                    scr.week_ending_date = item.week_ending_date.ToString();
                    scr.num_missed = item.num_missed.ToString();
                    scr.missed_list = item.missed_list;
                    scr.call_made_date = item.call_made_date.ToString();
                    scr.AGENT = item.AGENT;
                    scr.ANI = item.ANI;
                    scr.DNIS = item.DNIS;
                    scr.CALL_TIME = item.CALL_TIME.ToString();
                    scr.CALL_TYPE = item.CALL_TYPE;
                    scr.Email = item.Email;
                    scr.City = item.City;
                    scr.State = item.State;
                    scr.Zip = item.Zip;
                    scr.X_ID = item.X_ID.ToString();
                    scr.EducationLevel = item.EducationLevel;
                    scr.HighSchoolGradYear = item.HighSchoolGradYear;
                    scr.DegreeStartTimeframe = item.DegreeStartTimeframe;
                    scr.First_Name = item.First_Name;
                    scr.Last_Name = item.Last_Name;
                    scr.address = item.address;
                    scr.phone = item.phone;
                    scr.call_date = item.call_date.ToString();
                    //scr.audio_link = Common.GetAudioFileName(dr);
                    scr.profile_id = item.profile_id;
                    scr.audio_user = item.audio_user;
                    scr.audio_password = item.audio_password;
                    scr.review_date = item.review_date.ToString();
                    scr.CAMPAIGN = item.CAMPAIGN;
                    scr.DISPOSITION = item.DISPOSITION;
                    scr.SESSION_ID = item.session_id;
                    scr.scorecard = item.scorecard.ToString();
                    scorecard = item.scorecard.ToString();
                    scr.scorecard_name = item.Scorecard_name;
                    scr.website = item.website;
                    scr.calib_score = item.calib_score.ToString();
                    calib_id = Convert.ToInt32(item.calib_id);
                    scr.editable = false;
                    ScorecardData scd = new ScorecardData();
                    UserObject scu = new UserObject();
                    scu.UserRole = "Trainee";
                    scu.UserTitle = "Trainee Response";
                    scd.ScorecardUser = scu;
                    CallScores cs = new CallScores();
                    cs.score = Convert.IsDBNull(item.trainee_score.ToString()) ? "0" : item.trainee_score.ToString();
                    cs.reviewer = item.reviewer;
                    cs.scoredate = Convert.ToDateTime(item.review_date).ToShortDateString();
                    cs.role = "Trainee";
                    scd.CallScore = cs;
                    f_id = item.F_ID;
                    //DataTable section_dt = GetTable("exec getSections2 " + f_id);
                    var isgetSections2 = dataContext.getSections2(f_id).ToList();
                    List<SectionData> sections = new List<SectionData>();
                    foreach (var section_dr in isgetSections2)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr.section;
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                        var isProAllClientQuestions = dataContext.getProAllClientQuestions(item.F_ID, section_dr.ID, Convert.ToInt32(ID), username).ToList();
                        //DataTable qdt = GetTable("select * from  dbo.[getAllClientQuestions](" + dr["F_ID"].ToString() + "," + section_dr.ID + ",'" + username + "') left join (select q_position, answer_text, form_q_scores_training.question_id, right_answer, view_link from form_q_scores_training join question_answers on question_answers.ID = form_q_scores_training.question_answered where form_id = " + ID + ") a on  a.question_id = q_id  join questions on questions.ID = q_id  where active = 1 order by all_q_order");
                        foreach (var item1 in isProAllClientQuestions)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = item1.q_position;
                            qr.question = item1.q_short_name;
                            qr.result = item1.answer_text;
                            qr.QID = Convert.ToInt32(item1.q_id);
                            qr.QType = item1.q_type;
                            qr.OptionVerb = item1.options_verb;
                            qr.LeftColumnQuestion = Convert.ToBoolean(item1.left_column_question);
                            qr.QAPoints = Convert.ToInt32(item1.QA_points);

                            qr.ViewLink = item1.view_link;
                            qr.comments_allowed = Convert.ToBoolean(item1.comments_allowed);
                            if (item1.right_answer.ToString() == "")
                            {
                                qr.RightAnswer = true;
                            }
                            else
                            {
                                qr.RightAnswer = Convert.ToBoolean(item1.right_answer.ToString());
                            }
                            List<Comment> cmt_list = new List<Comment>();
                            var isFormTrainingResponses = dataContext.getFormTrainingResponses(ID, item1.q_id).ToList();
                            //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points,isnull(answer_comments.id,0) as id  from form_q_training_responses left join answer_comments On form_q_training_responses.answer_id = answer_comments.id where form_q_training_responses.form_id = " + ID + " And form_q_training_responses.question_id = " + item1["q_id"].ToString() + " order by isnull(ac_order,10000)");

                            if (isFormTrainingResponses.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (var ans_dr in isFormTrainingResponses)
                                {
                                    ans_comment.Add(ans_dr.comment);
                                    Comment cmt = new Comment();
                                    cmt.CommentText = ans_dr.comment;
                                    cmt.CommentID = Convert.ToInt32(ans_dr.id);
                                    cmt.CommentPoints = Convert.IsDBNull(ans_dr.comment_points) ? new float() : (float)ans_dr.comment_points;
                                    cmt_list.Add(cmt);
                                }
                                qr.QComments = ans_comment;
                                qr.SCRComments = cmt_list;
                            }
                        }
                        section_data.QList = qrs;
                        sections.Add(section_data);
                    }
                    scd.Sections = sections;
                    scds.Add(scd);
                }

                var clerkDataTemp = (from vwCFs in dataContext.vwCFs
                                     join UserExtraInfoes in dataContext.UserExtraInfoes on vwCFs.reviewed_by equals UserExtraInfoes.username
                                     where vwCFs.id == calib_id
                                     select new { vwCFs.id, vwCFs.cali_form_score, vwCFs.reviewed_by, vwCFs.recal_score, vwCFs.review_date, vwCFs.total_score, UserExtraInfoes.user_role }
                                     ).ToList();
                //reply = new SqlCommand("Select * from vwCF join userextrainfo On userextrainfo.username = reviewed_by where vwCF.id = @id", cn);
                foreach (var dr in clerkDataTemp)
                {
                    ScorecardData scd = new ScorecardData();
                    List<SectionData> sections = new List<SectionData>();
                    UserObject scu = new UserObject();
                    scu.UserRole = dr.user_role.ToString();
                    scu.UserTitle = dr.user_role + " Response";
                    scd.ScorecardUser = scu;
                    CallScores cs = new CallScores();
                    cs.score = Convert.IsDBNull(dr.cali_form_score.ToString()) ? "0" : dr.cali_form_score.ToString();
                    cs.reviewer = dr.reviewed_by.ToString();
                    if (dr.user_role.ToString() == "QA Lead")
                    {
                        cs.calibrationscore = dr.recal_score.ToString();
                    }
                    else
                    {
                        cs.calibrationscore = dr.total_score.ToString();
                    }
                    cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                    cs.role = dr.user_role;
                    scd.CallScore = cs;
                    var isgetSections2 = dataContext.getSections2(Convert.ToInt32(f_id)).ToList();
                    //DataTable section_dt = GetTable("exec getSections2 " + f_id);
                    foreach (var section_dr in isgetSections2)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr.section;
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                        var getAllClientQuestions = dataContext.getAllClientQuestionsReview(f_id, dr.id, section_dr.ID, username).ToList();
                        //DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + f_id + ", " + section_dr["ID"] + ",'" + username + "') left join (Select  q_pos, answer_text, calibration_scores.question_id, right_answer, view_link from calibration_scores join question_answers On question_answers.ID = calibration_scores.question_result where form_id = " + dr["ID"].ToString() + ") a On a.question_id = q_id join questions On questions.ID = q_id where active = 1 order by all_q_order");
                        foreach (var qdr in getAllClientQuestions)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr.q_pos.ToString();
                            qr.question = qdr.q_short_name;
                            qr.result = qdr.answer_text;
                            qr.QID = Convert.ToInt32(qdr.q_id);
                            qr.OptionVerb = qdr.options_verb;
                            qr.LeftColumnQuestion = Convert.ToBoolean(qdr.left_column_question);
                            if (Convert.ToInt32(qdr.QA_points) != 0)
                            {
                                qr.QAPoints = Convert.ToInt32(qdr.QA_points);
                            }
                            else
                            {
                                qr.QAPoints = 0;
                            }
                            qr.QType = qdr.q_type;
                            qr.ViewLink = qdr.view_link;
                            qr.comments_allowed = Convert.ToBoolean(qdr.comments_allowed);
                            if (qdr.right_answer.ToString() == "")
                            {
                                qr.RightAnswer = true;
                            }
                            else
                            {
                                qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                            }
                            List<Comment> cmt_list = new List<Comment>();
                            //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As q_comment, comment_points,isnull(answer_comments.id,0) as id  from form_c_responses left join answer_comments On form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = " + dr["ID"].ToString() + " And form_c_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            var getFormResponses = dataContext.getFormResponses(dr.id, qdr.q_id).ToList();
                            if (getFormResponses.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (var ans_dr in getFormResponses)
                                {
                                    ans_comment.Add(ans_dr.q_comment);
                                    Comment cmt = new Comment();
                                    cmt.CommentText = ans_dr.q_comment;
                                    cmt.CommentID = Convert.ToInt32(ans_dr.id);
                                    if (ans_dr.comment_points != 0)
                                    {
                                        cmt.CommentPoints = Convert.ToInt32(ans_dr.comment_points);
                                    }
                                    else
                                    {
                                        cmt.CommentPoints = 0;
                                    }
                                    cmt_list.Add(cmt);
                                }
                                qr.QComments = ans_comment;
                                qr.SCRComments = cmt_list;
                            }

                            //DataTable temp_dt = GetTable("exec getCTemplateItems " + dr["ID"].ToString() + "," + qdr["q_id"].ToString());
                            var getCTemplateItems = dataContext.getCTemplateItems(dr.id, qdr.q_id, null).ToList();
                            if (getCTemplateItems.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (var temp_dr in getCTemplateItems)
                                {
                                    CheckItems temp_item = new CheckItems();
                                    if (temp_dr.value == temp_dr.option_value)
                                    {
                                        temp_item.Checked = true;
                                    }
                                    else
                                    {
                                        temp_item.Checked = false;
                                    }
                                    temp_item.Item = temp_dr.value;
                                    temp_item.Position = temp_dr.option_pos;
                                    temp_item.ID = temp_dr.id.ToString();
                                    temp_items.Add(temp_item);
                                }
                                qr.QTemplate = temp_items;
                            }
                            qrs.Add(qr);
                        }
                        section_data.QList = qrs;
                        sections.Add(section_data);
                    }
                    scd.Sections = sections;
                    scds.Add(scd);
                }
                scr.ScorecardData = scds;

                //reply = new SqlCommand("Select * from school_x_data where xcc_id = @xcc_id ", cn);
                var schoolXdata = dataContext.School_X_Data.Where(x => x.xcc_id == review_id).ToList();
                List<SchoolItem> school_items = new List<SchoolItem>();
                foreach (var school_dr in schoolXdata)
                {
                    SchoolItem school_item = new SchoolItem();
                    school_item.AOI1 = school_dr.AOI1;
                    school_item.AOI2 = school_dr.AOI2;
                    school_item.College = school_dr.College;
                    school_item.DegreeOfInterest = school_dr.DegreeOfInterest;
                    school_item.L1_SubjectName = school_dr.L1_SubjectName;
                    school_item.L2_SubjectName = school_dr.L2_SubjectName;
                    school_item.Modality = school_dr.Modality;
                    school_item.Portal = school_dr.origin;
                    school_item.School = school_dr.School;
                    school_item.TCPA = school_dr.tcpa;
                    school_item.id = school_dr.id.ToString();
                    school_items.Add(school_item);
                }
                scr.SchoolData = school_items;

                var getotherformdata = dataContext.getotherformdata(review_id).ToList();
                List<OtherData> otherdata_items = new List<OtherData>();
                foreach (var school_dr in getotherformdata)
                {
                    OtherData otherdata_item = new OtherData();
                    otherdata_item.key = school_dr.data_key;
                    otherdata_item.label = school_dr.label;
                    otherdata_item.school = school_dr.school_name;
                    otherdata_item.type = school_dr.data_type;
                    otherdata_item.value = school_dr.data_value;
                    otherdata_item.id = school_dr.id.ToString();
                    otherdata_items.Add(otherdata_item);
                }
                scr.OtherData = otherdata_items;
                int scoreCard1 = 0;
                if (scorecard != "")
                {
                    scoreCard1 = Convert.ToInt32(scorecard);
                }
                if (SI.is_practice)
                {
                    //reply = new SqlCommand("Select id, total_score, review_date, trainee_score, is_practice from vwtrain where reviewer = '" + reviewer + "' and scorecard = '" + scorecard + "'" + sql + " order by review_date desc", cn);
                    var vtrain = dataContext.vwTrains.Where(x => x.reviewer == reviewer && x.scorecard == scoreCard1 && x.is_practice == true).OrderByDescending(x => x.review_date).ToList();
                    List<training_item> training_items = new List<training_item>();
                    foreach (var school_dr in vtrain)
                    {
                        training_item ti = new training_item();
                        ti.id = school_dr.id.ToString();
                        ti.score = school_dr.trainee_score.ToString();
                        ti.score_date = school_dr.review_date.ToString();
                        ti.is_practice = Convert.ToBoolean(school_dr.is_practice);
                        training_items.Add(ti);
                    }
                    tcr.training_items = training_items;
                }
                else
                {
                    //string sql = " and (is_practice = 0 or is_practice IS NULL) ";
                    //reply = new SqlCommand("Select id, total_score, review_date, trainee_score, is_practice from vwtrain where reviewer = '" + reviewer + "' and scorecard = '" + scorecard + "'" + sql + " order by review_date desc", cn);
                    var vtrain = dataContext.vwTrains.Where(x => x.reviewer == reviewer && x.scorecard == scoreCard1 && (x.is_practice == false || x.is_practice == null)).OrderByDescending(x => x.review_date).ToList();
                    List<training_item> training_items = new List<training_item>();
                    foreach (var school_dr in vtrain)
                    {
                        training_item ti = new training_item();
                        ti.id = school_dr.id.ToString();
                        ti.score = school_dr.trainee_score.ToString();
                        ti.score_date = school_dr.review_date.ToString();
                        ti.is_practice = Convert.ToBoolean(school_dr.is_practice);
                        training_items.Add(ti);
                    }
                    tcr.training_items = training_items;
                }

                int scoreCard = 0;
                if (scorecard != "")
                {
                    scoreCard = Convert.ToInt32(scorecard);
                }
                //DataTable pass_dt = GetTable("select case when avg(trainee_score)>= (select pass_percent from scorecards where id=" + scorecard + ") and count(*) >  (select  isnull(training_count,10) from scorecards where id=" + scorecard + ") then 1 else 0 end  from (select top (select isnull(training_count,10) from scorecards where id=" + scorecard + ") trainee_score from vwTrain where reviewer = '" + reviewer + "' order by id desc) a");
                var getPassedTraining = dataContext.getPassedTraining(scoreCard, reviewer).Count();
                if (getPassedTraining == 1)
                {
                    tcr.passed_training = true;
                }
                else
                {
                    tcr.passed_training = false;
                }
                scr.showSpotCheck = false;
                tcr.acr = scr;
            }
            return tcr;
        }
        #endregion GetTrainingReview

        /// <summary>
        /// UpdateSpotcheck
        /// </summary>
        /// <param name="SCD"></param>
        /// <returns></returns>
        public ButtonAction UpdateSpotcheck(SpotCheckData SCD)
        {
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                ButtonAction bojButtonAction = new ButtonAction();
                bojButtonAction.ActionRedirect = "dashboard";
                bojButtonAction.ActionResult = "Success";
                bojButtonAction.ActionTask = "Redirect";
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    bojButtonAction.ActionResult = "Not Authenticated";
                    bojButtonAction.ActionRedirect = "dashboard";
                    bojButtonAction.ActionTask = "Redirect";
                    return bojButtonAction;
                }
                string username = SCD.username;
                int f_id = 0;
                if (SCD.f_id != "")
                {
                    f_id = Convert.ToInt32(SCD.f_id);
                }
                string disposition = SCD.disposition;
                string notes = SCD.notes;
                int review_time = 0;
                if (SCD.review_time != "")
                {
                    review_time = Convert.ToInt32(SCD.review_time);
                }
                if ((notes == "" | notes == "Comments required for Spot Check!") & disposition != "Agree with Score")
                {
                    notes = "Comments required for Spot Check!";
                    bojButtonAction.ActionRedirect = "";
                    bojButtonAction.ActionResult = "Comments required for Spot Check!";
                    bojButtonAction.ActionTask = "AlertUser";
                    return bojButtonAction;
                }

                int thisSpot = 0;
                var isSpot = dataContext.spotchecks.Where(x => x.f_id == f_id && x.checked_date == null).FirstOrDefault();
                //DataTable spot_dt = GetTable("select * From spotcheck where f_id = " + f_id + " and checked_date is null");
                if (isSpot != null)
                {
                    thisSpot = isSpot.id;
                }
                //Common.UpdateTable("update questions set q_order = " + q.order + " where id = " + q.id);
                var isspotcheckUpdate = dataContext.spotcheckUpdate(thisSpot, f_id, username, notes, disposition, review_time).ToList();

                var isNextSpot = dataContext.getMySpotCheck(HttpContext.Current.User.Identity.Name).FirstOrDefault();
                //DataTable next_spot_dt = GetTable("exec getMySpotCheck '" + HttpContext.Current.User.Identity.Name + "'");
                if (isNextSpot != null)
                {
                    bojButtonAction.ActionResult = "Not Authenticated";
                    bojButtonAction.ActionRedirect = "review/" + isNextSpot.f_id;
                    bojButtonAction.ActionTask = "Redirect";
                    return bojButtonAction;
                }
                else
                {
                    bojButtonAction.ActionResult = "";
                    bojButtonAction.ActionRedirect = "dashboard";
                    bojButtonAction.ActionTask = "Redirect";
                    return bojButtonAction;
                }
            }
        }

        #region Public getScore
        /// <summary>
        /// getScore
        /// </summary>
        /// <param name="gsd"></param>
        /// <returns></returns>
        public List<SessionStatus> getScore(getScoreData gsd)
        {
            string session_id = gsd.session_id;
            string username = gsd.username;
            List<SessionStatus> objSessionStatus = new List<SessionStatus>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var userApps = dataContext.UserApps.Where(x => x.username == username).FirstOrDefault();
                int? userScorecard = 0;
                if (userApps != null)
                {
                    userScorecard = userApps.user_scorecard;
                }
                var isxcc_report = dataContext.XCC_REPORT_NEW.Where(x => x.SESSION_ID == session_id && x.scorecard == userScorecard).ToList();
                //SqlDataAdapter reply = new SqlDataAdapter("Select * from xcc_report_new where session_id = @session_id And scorecard In (Select user_scorecard from userapps where username = @username)", cn);

                foreach (var dr in isxcc_report)
                {
                    SessionStatus ss_obj = new SessionStatus();
                    var result = dataContext.vwForms.Where(x => x.review_ID == dr.ID).ToList();
                    //DataTable vw_dt = GetTable("Select isnull(isnull(edited_score,calib_score),total_score) As theScore from vwForm where review_id = " + dr["id"].ToString());
                    ss_obj.score = "N/A";
                    switch (dr.MAX_REVIEWS)
                    {
                        case 0:
                            {
                                ss_obj.status = "RECEIVED";
                                break;
                            }
                        case 1:
                            {
                                ss_obj.status = "PROCESSED";
                                if (result.Count > 0)
                                    ss_obj.score = result.SingleOrDefault().total_score.ToString();
                                break;
                            }

                        case 10:
                            {
                                ss_obj.status = "WAITING For DATA";
                                break;
                            }

                        case 99:
                            {
                                ss_obj.status = "WAITING For TRANSCRIPTION";
                                break;
                            }
                    }
                    if (dr.audio_link.ToString() == "")
                    {
                        ss_obj.status = "WAITING/CONVERTING audio";
                    }
                    if (dr.bad_call.ToString() == "1")
                    {
                        ss_obj.status = "BAD Call/UNABLE To SCORE - " + dr.bad_call_reason;
                    }
                    objSessionStatus.Add(ss_obj);
                }
            }
            return objSessionStatus;
        }
        #endregion getScore

        #region Public AddDispute
        /// <summary>
        /// AddDispute
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction AddDispute(EndPointData endPointData)
        {
            int Id = endPointData.ID;
            string userName = endPointData.username;
            string destNotification = string.Empty;
            if (HttpContext.Current.User.IsInRole("Agent"))
            {
                destNotification = "Supervisor";
            }
            else
            {
                destNotification = "Calibrator";
            }
            ButtonAction ba = new ButtonAction();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                // Common.UpdateTable("INSERT INTO [form_notifications] (role, [date_created], [form_id], opened_by) select (select user_role from userextrainfo where username = '" + userName + "'),  dbo.getMTDate(), " + Id + ", '" + userName + "'");
                var resultUserExtraInfoes = dataContext.UserExtraInfoes.Where(x => x.username == userName).FirstOrDefault();
                form_notifications form_notifications = new form_notifications();
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                if (resultUserExtraInfoes == null)
                {
                    // Save form_notifications data
                    form_notifications.role = resultUserExtraInfoes.user_role;
                    form_notifications.date_created = Date;
                    form_notifications.form_id = Id;
                    form_notifications.opened_by = userName;
                    dataContext.form_notifications.Add(form_notifications);
                    int result1 = dataContext.SaveChanges();
                }
                ba.ActionRedirect = "";
                ba.ActionResult = "Added Notification";
                ba.ActionTask = "AlertUser";
                return ba;
            }
        }
        #endregion Public AddDispute

        #region Public UpdateCallComment
        /// <summary>
        /// UpdateCallComment
        /// </summary>
        /// <param name="updateComment"></param>
        /// <returns></returns>
        public string UpdateCallComment(UpdateComment updateComment)
        {
            try
            {
                int result = 0;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    int Id = Convert.ToInt32(updateComment.ID.Trim());
                    //Common.UpdateTable("update system_comments set comment = '" + UC.comment.Replace("'", "''") + "' where id = " + UC.ID);
                    var isExist = dataContext.system_comments.Where(x => x.id == Id).FirstOrDefault();
                    system_comments tblSystemComments = new system_comments();
                    if (isExist != null)
                    {
                        tblSystemComments = dataContext.system_comments.Find(Id);
                        dataContext.Entry(tblSystemComments).State = EntityState.Modified;
                        tblSystemComments.comment = updateComment.comment.Replace("'", "''");
                        result = dataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Messages.Updated;
        }
        #endregion Public AddDispute

        #region Public DeleteComment
        /// <summary>
        /// DeleteComment
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        public string DeleteComment(SimpleID simpleID)
        {
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    int Id = 0;
                    if (simpleID.ID.Trim() != "")
                    {
                        Id = Convert.ToInt32(simpleID.ID.Trim());
                    }

                    //Common.UpdateTable("delete from form_notifications where id = " + SI.ID);
                    var isExist = dataContext.form_notifications.Where(x => x.id == Id).FirstOrDefault();
                    form_notifications tblformNotifications = new form_notifications();
                    if (isExist != null)
                    {
                        dataContext.form_notifications.Remove(isExist);
                        int result = dataContext.SaveChanges();
                    }
                    return Messages.Deleted;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion Public DeleteComment

        #region Public DeleteCall
        /// <summary>
        /// DeleteCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction DeleteCall(EndPointData endPointData)
        {
            try
            {
                ButtonAction objButtonAction = new ButtonAction();
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    string userName = endPointData.username;
                    int Id = endPointData.ID;
                    //Common.UpdateTable("exec deleteOneCallCompletely " + ID);
                    var isDelete = dataContext.deleteOneCallCompletely(Id);
                    objButtonAction.ActionRedirect = "dashboard";
                    objButtonAction.ActionResult = "Success";
                    objButtonAction.ActionTask = "Redirect";
                }
                return objButtonAction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Public DeleteCall

        #region Public ReassignNotification
        /// <summary>
        /// ReassignNotification
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction ReassignNotification(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            objButtonAction.ActionRedirect = "";
            objButtonAction.ActionResult = "Reassigned";
            objButtonAction.ActionTask = "AlertUser";
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                objButtonAction.ActionResult = "Not Authenticated";
                objButtonAction.ActionRedirect = "dashboard";
                objButtonAction.ActionTask = "Redirect";
                return objButtonAction;
            }
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    int Id = endPointData.ID;
                    var isExist = dataContext.ReassignNotification(Id);
                    //Common.UpdateTable("exec ReassignNotification " + ID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public ReassignNotification

        #region Public RecreateCall
        /// <summary>
        /// RecreateCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction RecreateCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            objButtonAction.ActionRedirect = "dashboard";
            objButtonAction.ActionResult = "Success";
            objButtonAction.ActionTask = "Redirect";

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                objButtonAction.ActionResult = "Not Authenticated";
                objButtonAction.ActionRedirect = "dashboard";
                objButtonAction.ActionTask = "Redirect";
                return objButtonAction;
            }
            int Id = endPointData.ID;
            string username = endPointData.username;
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var isvwForms = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                    //DataTable app_data = GetTable("select review_id, audio_link From vwFOrm where f_id = " + ID);
                    if (isvwForms != null)
                    {
                        int x_id = isvwForms.X_ID;
                    }
                    string audio_link = isvwForms.audio_link;
                    var isDuplicateCall = dataContext.duplicateCall(Id, audio_link);
                    //Common.UpdateTable("exec duplicateCall " + ID + ",'" + audio_link + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public RecreateCall

        #region Public HideCall
        /// <summary>
        /// HideCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction HideCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            objButtonAction.ActionRedirect = "dashboard";
            objButtonAction.ActionResult = "Success";
            objButtonAction.ActionTask = "Redirect";
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                objButtonAction.ActionResult = "Not Authenticated";
                objButtonAction.ActionRedirect = "dashboard";
                objButtonAction.ActionTask = "Redirect";
                return objButtonAction;
            }
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //Common.UpdateTable("update form_score3 set client_visible = 0 where id =" + Id);
                    int Id = endPointData.ID;
                    var isExist = dataContext.form_score3.Where(x => x.id == Id).FirstOrDefault();
                    form_score3 tblFormScore = new form_score3();
                    if (isExist != null)
                    {
                        tblFormScore = dataContext.form_score3.Find(Id);
                        dataContext.Entry(tblFormScore).State = EntityState.Modified;
                        tblFormScore.client_visible = true;
                        int result = dataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public HideCall

        #region Public ResetCall
        /// <summary>
        /// ResetCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction ResetCall(EndPointData endPointData)
        {
            ButtonAction buttonAction = new ButtonAction();
            try
            {
                buttonAction.ActionRedirect = "dashboard";
                buttonAction.ActionResult = "Success";
                buttonAction.ActionTask = "Redirect";
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    buttonAction.ActionResult = "Not Authenticated";
                    buttonAction.ActionRedirect = "dashboard";
                    buttonAction.ActionTask = "Redirect";
                    return buttonAction;
                }
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {

                    int Id = endPointData.ID;
                    //Common.UpdateTable("update system_comments set comment = '" + UC.comment.Replace("'", "''") + "' where id = " + UC.ID);
                    var isExist = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                    int x_id = isExist.X_ID;
                    if (isExist != null)
                    {
                        var isResetcall = dataContext.resetCall(x_id);
                        //Common.UpdateTable("exec resetcall " + x_id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return buttonAction;
        }
        #endregion Public ResetCall

        #region Public AddCaliDispute
        /// <summary>
        /// AddCaliDispute
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction AddCaliDispute(EndPointData endPointData)
        {
            int Id = endPointData.ID;
            string username = endPointData.username;
            ButtonAction objButtonAction = new ButtonAction();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable app_data = GetTable("select appname From vwFOrm where f_id = " + ID);
                var resultvwForm = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                string appName = string.Empty;
                if (resultvwForm.appname != null)
                {
                    appName = resultvwForm.appname;
                }
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                var getWeekDate = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getWeekDate()");
                DateTime weekEnding = getWeekDate.AsEnumerable().First();
                //Common.UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, isrecal) select 'QA Selected','" + ID + "','" + username + "','QA Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "','" + username + "', 1");
                calibration_pending calibration_pending = new calibration_pending();
                // Save form_notifications data
                calibration_pending.bad_value = "QA Selected";
                calibration_pending.form_id = Id;
                calibration_pending.reviewer = username;
                calibration_pending.review_type = "QA Selected";
                calibration_pending.week_ending = weekEnding;
                calibration_pending.appname = appName;
                calibration_pending.cp_who_added = username;
                calibration_pending.isRecal = 1;
                dataContext.calibration_pending.Add(calibration_pending);
                int result1 = dataContext.SaveChanges();
                objButtonAction.ActionRedirect = "";
                objButtonAction.ActionResult = Messages.AddedNotification;
                objButtonAction.ActionTask = Messages.AlertUser;
                return objButtonAction;
            }
        }
        #endregion Public AddCaliDispute

        #region Public AddClientCalibration
        /// <summary>
        /// AddClientCalibration
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction AddClientCalibration(EndPointData endPointData)
        {
            string selected_by = "";
            int result = 0;
            int Id = endPointData.ID;
            string username = endPointData.username;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable app_data = GetTable("select appname, scorecard From vwFOrm where f_id = " + ID);
                var resultvwForm = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                string appname = string.Empty;
                int scorecard = 0;
                if (resultvwForm != null)
                {
                    appname = resultvwForm.appname.ToString();
                    scorecard = Convert.ToInt32(resultvwForm.scorecard);
                }
                selected_by = Messages.Client;
                //Common.UpdateTable("insert into cali_pending_client (bad_value, form_id, reviewer, review_type, week_ending, appname, assigned_to, cpc_who_added) select '" + selected_by + " Selected','" + ID + "',(select reviewer from form_score3 where id = " + ID + "),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "', userapps.username,'" + username + "' from userextrainfo join userapps on userextrainfo.username = userapps.username where user_role in ('Supervisor','Client','Manager', 'Client Calibrator') and non_calib = 0 and user_scorecard = (select scorecard from vwForm where f_id = '" + ID + "')");
                var resultinsertCaliPending = dataContext.insertCaliPendingClient(Id, selected_by, appname, username, null, 0);
                //Common.UpdateTable("insert into cali_pending_client (bad_value, form_id, reviewer, review_type, week_ending, appname, assigned_to, cpc_who_added) select '" + selected_by + " Selected','" + Id + "',(select reviewer from form_score3 where id = " + Id + "),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "', userapps.username,'" + username + "' from userextrainfo join userapps on userextrainfo.username = userapps.username where user_role in ('Supervisor','Client','Manager', 'Client Calibrator') and non_calib = 0 and user_scorecard = (select scorecard from vwForm where f_id = '" + Id + "')");
                var resultcalibration_pending = dataContext.calibration_pending.Where(x => x.form_id == Id && x.isRecal == 1).ToList();
                var count = resultcalibration_pending.Count();
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                var getWeekDate = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getWeekDate()");
                DateTime weekEnding = getWeekDate.AsEnumerable().First();
                var resultformScore = dataContext.form_score3.Where(x => x.id == Id).FirstOrDefault();
                //DataTable cal_dt = GetTable("select count(*) from calibration_pending where form_id = " + Id + " and isrecal = 1");
                if (count == 0)
                {
                    //Common.UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, client_start, isrecal, sc_id) select '" + selected_by + " Selected','" + Id + "',(select reviewer from form_score3 where id = " + Id + "),'" + selected_by + " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "','" + username + "', dbo.getMTDate(),1,'" + scorecard + "'");
                    calibration_pending tblcalibration_pending = new calibration_pending();
                    if (resultformScore == null)
                    {
                        // Save form_notifications data
                        tblcalibration_pending.bad_value = selected_by + Messages.Selected;
                        tblcalibration_pending.form_id = Id;
                        tblcalibration_pending.reviewer = resultformScore.reviewer;
                        tblcalibration_pending.review_type = Messages.Selected;
                        tblcalibration_pending.week_ending = weekEnding;
                        tblcalibration_pending.appname = appname;
                        tblcalibration_pending.cp_who_added = username;
                        tblcalibration_pending.client_start = Date;
                        tblcalibration_pending.sc_id = scorecard;
                        tblcalibration_pending.isRecal = 1;
                        dataContext.calibration_pending.Add(tblcalibration_pending);
                        result = dataContext.SaveChanges();
                    }
                }
                else
                {
                    //Common.UpdateTable("update calibration_pending set client_start = dbo.getMTDate() where id = (select top 1 id from calibration_pending where form_id = " + Id + " and (select count(*) from calibration_pending where client_start is not null and form_id = " + Id + ") = 0 order by id desc)");
                    var resultcalibration = dataContext.calibration_pending.Where(x => x.id == Id).FirstOrDefault();
                    var resultcalibrationCount = dataContext.calibration_pending.Where(x => x.id == Id && x.client_start != null).OrderByDescending(x => x.id).ToList();
                    int count1 = resultcalibrationCount.Count();
                    var query = dataContext.calibration_pending.FirstOrDefault(p => p.form_id == Id);
                    calibration_pending tblcalibration = new calibration_pending();
                    if (resultcalibrationCount != null && query != null)
                    {
                        tblcalibration = dataContext.calibration_pending.Find(resultcalibration.id);
                        dataContext.Entry(tblcalibration).State = EntityState.Modified;
                        tblcalibration.client_start = Date;
                        result = dataContext.SaveChanges();
                    }

                }
                ButtonAction objButtonAction = new ButtonAction();
                objButtonAction.ActionRedirect = "";
                objButtonAction.ActionResult = Messages.Added;
                objButtonAction.ActionTask = Messages.AlertUser;
                return objButtonAction;
            }
        }
        #endregion Public AddClientCalibration

        #region Public AddCalibration
        /// <summary>
        /// AddCalibration
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        public ButtonAction AddCalibration(EndPointData endPointData)
        {
            int Id = endPointData.ID;
            string username = endPointData.username;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable app_data = GetTable("select appname, scorecard From vwFOrm where f_id = " + ID);
                var resultvwForm = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                string appname = string.Empty;
                int scorecard = 0;
                if (resultvwForm != null)
                {
                    appname = resultvwForm.appname.ToString();
                    scorecard = Convert.ToInt32(resultvwForm.scorecard);
                }
                string selected_by = "";
                selected_by = Messages.Admin;

                //DataTable cal_dt = GetTable("select count(*) from calibration_pending where form_id = " + ID + " and isrecal = 1");
                var resultcalibration_pending = dataContext.calibration_pending.Where(x => x.form_id == Id && x.isRecal == 1).ToList();
                var count = resultcalibration_pending.Count();

                if (count == 0)
                {
                    var resultinsertCaliPending = dataContext.insertCaliPendingClient(Id, selected_by, appname, username, scorecard, 1);
                    //Common.UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, isrecal, sc_id) select '" + selected_by + " Selected','" + ID + "',(select reviewer from form_score3 where id = " + ID + "),'" + selected_by + " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "','" + username + "', 1,'" + scorecard + "'");
                }
                else
                {
                    var resultcalibrationCount = dataContext.calibration_pending.Where(x => x.form_id == Id && x.isRecal == 0).ToList();
                    var count1 = resultcalibration_pending.Count();
                    //cal_dt = GetTable("select count(*) from calibration_pending where form_id = " + ID + " and isrecal = 0");
                    if (count1 == 0)
                    {
                        var resultinsertCaliPending = dataContext.insertCaliPendingClient(Id, selected_by, appname, username, scorecard, 0);
                        //Common.UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname, cp_who_added, isrecal, sc_id) select '" + selected_by + " Selected','" + ID + "',(select reviewer from form_score3 where id = " + ID + "),'" + selected_by + " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" + appname + "','" + username + "', 0,'" + scorecard + "'");
                    }
                    else
                    {
                        //Common.UpdateTable("update calibration_pending set client_start = dbo.getMTDate() where id = (select top 1 id from calibration_pending where form_id = " + ID + " and (select count(*) from calibration_pending where client_start is not null and form_id = " + ID + ") = 0 order by id desc)");
                        var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                        DateTime Date = dateQuery.AsEnumerable().First();
                        var resultcalibrationCountAdmin = dataContext.calibration_pending.Where(x => x.form_id == Id && x.client_start != null).OrderByDescending(x => x.id).ToList();
                        int countAdmin = resultcalibrationCountAdmin.Count();
                        var query = dataContext.calibration_pending.FirstOrDefault(p => p.form_id == Id);
                        var resultcalibration = dataContext.calibration_pending.Where(x => x.id == Id && countAdmin == 0).FirstOrDefault();
                        calibration_pending tblcalibration = new calibration_pending();
                        if (resultcalibrationCountAdmin != null && query != null)
                        {
                            tblcalibration = dataContext.calibration_pending.Find(resultcalibration.id);
                            dataContext.Entry(tblcalibration).State = EntityState.Modified;
                            tblcalibration.client_start = Date;
                            int result = dataContext.SaveChanges();
                        }
                    }
                }
                ButtonAction buttonAction = new ButtonAction();
                buttonAction.ActionRedirect = "";
                buttonAction.ActionResult = Messages.Added;
                buttonAction.ActionTask = Messages.AlertUser;
                return buttonAction;
            }
        }
        #endregion Public AddCalibration

        #region Public UpdateMetaData
        /// <summary>
        /// UpdateMetaData
        /// </summary>
        /// <param name="updateAllItems"></param>
        /// <returns></returns>
        public string UpdateMetaData(UpdateAllItems updateAllItems)
        {
            string ret_resp = "";
            int result = 0;
            foreach (UpdateMetaDataItem UMDI in updateAllItems.UMDIList)
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    int Id = Convert.ToInt32(UMDI.id.Trim());
                    if (UMDI.table == "otherformdata")
                    {
                        //Common.UpdateTable("update " + UMDI.table + " set data_value = '" + UMDI.value.Replace("'", "''") + "' where id = " + UMDI.id);
                        var isExist = dataContext.otherFormDatas.Where(x => x.id == Id).FirstOrDefault();
                        otherFormData tblotherFormDatas = new otherFormData();
                        if (isExist != null)
                        {
                            tblotherFormDatas = dataContext.otherFormDatas.Find(Id);
                            dataContext.Entry(tblotherFormDatas).State = EntityState.Modified;
                            tblotherFormDatas.data_value = UMDI.value.Replace("'", "''");
                            result = dataContext.SaveChanges();
                        }
                    }
                    else
                    {
                        if (UMDI.table == "xcc_report_new")
                        {
                            //Common.UpdateTable("insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" + HttpContext.Current.User.Identity.Name + "', dbo.getMTDate(), '<strong>" + UMDI.key + "</strong> changed to <strong>" + UMDI.value.Replace("'", "''") + "</strong>',(select f_id from vwForm  where review_id = " + UMDI.id + "), 'Call'");
                            system_comments tblsystem_comments = new system_comments();
                            var isvwForm = dataContext.vwForms.Where(x => x.review_ID == Id).FirstOrDefault();
                            var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                            DateTime Date = dateQuery.AsEnumerable().First();
                            if (isvwForm != null)
                            {
                                // Save system_comments data
                                tblsystem_comments.comment_who = HttpContext.Current.User.Identity.Name;
                                tblsystem_comments.comment_date = Date;
                                tblsystem_comments.comment = "<strong>" + UMDI.key + "</strong> changed to <strong>" + UMDI.value.Replace("'", "''") + " </strong >";
                                tblsystem_comments.comment_id = isvwForm.F_ID;
                                tblsystem_comments.comment_type = "Call";
                                dataContext.system_comments.Add(tblsystem_comments);
                                result = dataContext.SaveChanges();
                            }
                        }
                        else
                        {

                            //Common.UpdateTable("update " + UMDI.table + " set [" + UMDI.key + "] = '" + UMDI.value.Replace("'", "''") + "' where id = " + UMDI.id);
                        }

                    }
                    foreach (int removeitem in updateAllItems.removeother)
                    {
                        //Common.UpdateTable("delete from otherFormData where id = " + removeitem);
                        var isExist = dataContext.otherFormDatas.Where(x => x.id == removeitem).FirstOrDefault();
                        otherFormData tblotherFormDatas = new otherFormData();
                        if (isExist != null)
                        {
                            dataContext.otherFormDatas.Remove(isExist);
                            result = dataContext.SaveChanges();

                        }
                        if (ret_resp == "")
                        {
                            ret_resp = Messages.Updated;
                        }

                    }
                }
            }
            return ret_resp;
        }
        #endregion Public UpdateMetaData

        #region Public ChangeCallScorecard
        /// <summary>
        /// ChangeCallScorecard
        /// </summary>
        /// <param name="changeScorecardData"></param>
        public void ChangeCallScorecard(ChangeScorecardData changeScorecardData)
        {
            try
            {
                int result = 0;
                int x_id = changeScorecardData.x_id;
                int new_scorecard = Convert.ToInt32(changeScorecardData.new_scorecard);
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return;
                }
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //Common.UpdateTable("update xcc_report_new set scorecard='" + new_scorecard + "' where id = " + x_id);
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == x_id).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(isExist.ID);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.scorecard = new_scorecard;
                        result = dataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Public ChangeCallScorecard

        #region Public RemoveCalibrator
        /// <summary>
        /// RemoveCalibrator
        /// </summary>
        /// <param name="f_id"></param>
        /// <param name="calibrator"></param>
        /// <returns></returns>
        public string RemoveCalibrator(int f_id, string calibrator)
        {

            //Common.UpdateTable("delete from [cali_pending_client] where form_id = " + f_id + " and date_completed is null and assigned_to = '" + calibrator + "'");
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var isExist = dataContext.cali_pending_client.Where(x => x.form_id == f_id && x.date_completed != null && x.assigned_to == calibrator).FirstOrDefault();
                    cali_pending_client tblQuestion = new cali_pending_client();
                    if (isExist != null)
                    {
                        dataContext.cali_pending_client.Remove(isExist);
                        int result = dataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Messages.Deleted;
        }
        #endregion Public RemoveCalibrator

        #region Public CompleteReview
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        public string CompleteReview(SimpleID simpleID)
        {
            int f_id = 0;
            if (simpleID.ID != "")
            {
                f_id = Convert.ToInt32(simpleID.ID);
            }
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return "";
            }
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                    DateTime Date = dateQuery.AsEnumerable().First();
                    //Common.UpdateTable("update calibration_pending set client_review_completed = dbo.getMTDate(), client_who_closed = '" + HttpContext.Current.User.Identity.Name + "' where form_id = " + f_id);
                    var isExist = dataContext.calibration_pending.Where(x => x.form_id == f_id).FirstOrDefault();
                    calibration_pending tblcalibrationPending = new calibration_pending();
                    if (isExist != null)
                    {
                        tblcalibrationPending = dataContext.calibration_pending.Find(isExist.id);
                        tblcalibrationPending.client_review_completed = Date;
                        tblcalibrationPending.client_who_closed = HttpContext.Current.User.Identity.Name;
                        dataContext.Entry(tblcalibrationPending).State = EntityState.Modified;
                        dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("delete from [cali_pending_client] where form_id = " + f_id + " and date_completed is null");
                    var iscaliPendingClient = dataContext.cali_pending_client.Where(x => x.form_id == f_id && x.date_completed == null).FirstOrDefault();
                    cali_pending_client tblcali_pending_client = new cali_pending_client();
                    if (iscaliPendingClient != null)
                    {
                        dataContext.cali_pending_client.Remove(iscaliPendingClient);
                        dataContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Messages.Updated;
        }
        #endregion Public CompleteReview

        #region Public CompleteReview
        /// <summary>
        /// MarkCalibrationBad
        /// </summary>
        /// <param name="mcb"></param>
        /// <returns></returns>
        public string MarkCalibrationBad(MarkCaliBad mcb)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return Messages.Authenticated;
            }
            int x_id = mcb.x_id;
            //Common.UpdateTable("declare @f_id int = (select form_id from calibration_pending where id = " + x_id + "); exec [markExistingCallBad] @f_id");
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var isExist = dataContext.calibration_pending.Where(x => x.form_id == x_id).FirstOrDefault();

                    if (isExist != null)
                    {
                        var getList = dataContext.markExistingCallBad(isExist.form_id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Messages.Updated;
        }
        #endregion Public CompleteReview

        #region Public MarkCallBad2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="markBadCallData2"></param>
        /// <returns></returns>
        public ButtonAction MarkCallBad2(MarkBadCallData2 markBadCallData2)
        {
            int x_id = markBadCallData2.ID;
            string reject_reason = markBadCallData2.bad_reason;
            ButtonAction objButtonAction = new ButtonAction();
            objButtonAction.ActionRedirect = "review/" + markBadCallData2.ID;
            objButtonAction.ActionResult = Messages.Updated;
            objButtonAction.ActionTask = Messages.Redirect;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return objButtonAction;
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                List<ScorecardData> scds = new List<ScorecardData>();
                var isvwForm = dataContext.vwForms.Where(x => x.F_ID == x_id).FirstOrDefault();
                int Idform = 0;
                if (isvwForm != null)
                {
                    Idform = isvwForm.X_ID;
                }
                var scorecardResTemp = (from XCC_REPORT_NEW in dataContext.XCC_REPORT_NEW
                                        join scorecards in dataContext.scorecards on XCC_REPORT_NEW.scorecard equals scorecards.id
                                        where XCC_REPORT_NEW.ID == Idform
                                        select (scorecards.auto_accept_bad_call)).ToList();
                //DataTable dt = GetTable("select auto_accept_bad_call from xcc_report_new join scorecards on scorecards.id = scorecard and xcc_report_new.id = (select review_id from vwForm where f_id = " + x_id + ")");
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                if (scorecardResTemp.Equals(1))
                {
                    //Common.UpdateTable("update xcc_report_new set bad_call = 1, bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" + HttpContext.Current.User.Identity.Name + "',  bad_call_who='" + HttpContext.Current.User.Identity.Name + "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" + reject_reason.Replace("'", "''") + "' where id = (select review_id from vwForm where f_id = " + x_id + ")");
                    int reviewId = 0;
                    if (isvwForm != null)
                    {
                        reviewId =Convert.ToInt32(isvwForm.review_ID);
                    }
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == reviewId).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(isExist.ID);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.bad_call = 1;
                        tblXCC_REPORT_NEW.bad_call_accepted = Date;
                        tblXCC_REPORT_NEW.bad_call_accepted_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_date = Date;
                        tblXCC_REPORT_NEW.MAX_REVIEWS = 1;
                        tblXCC_REPORT_NEW.bad_call_reason = reject_reason.Replace("'", "''");
                        int result = dataContext.SaveChanges();
                    }
                }
                else
                {
                    //Common.UpdateTable("update xcc_report_new set bad_call = 1, bad_call_who='" + HttpContext.Current.User.Identity.Name + "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" + reject_reason.Replace("'", "''") + "' where id = (select review_id from vwForm where f_id = " + x_id + ")");
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == isvwForm.review_ID).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(isExist.ID);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.bad_call = 1;
                        tblXCC_REPORT_NEW.bad_call_date = Date;
                        tblXCC_REPORT_NEW.bad_call_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_date = Date;
                        tblXCC_REPORT_NEW.MAX_REVIEWS = 1;
                        tblXCC_REPORT_NEW.bad_call_reason = reject_reason.Replace("'", "''");
                        int result = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("update vwForm set calib_score = null,  pass_fail='N/A',display_score=null, total_score= null, original_qa_score=null, missed_list = null, formatted_comments = null, formatted_missed = null where f_id = " + x_id);
                    var isExist2 = dataContext.vwForms.Where(x => x.review_ID == x_id).FirstOrDefault();
                    var tblscore = dataContext.form_score3.Where(x => x.review_ID == x_id).FirstOrDefault();
                    form_score3 tblscore3 = new form_score3();
                    if (isExist != null)
                    {
                        tblscore3 = dataContext.form_score3.Find(tblscore.id);
                        dataContext.Entry(tblscore3).State = EntityState.Modified;
                        tblscore3.calib_score = null;
                        tblscore3.total_score = null;
                        tblscore3.total_score_with_fails = null;
                        tblscore3.display_score = null;
                        tblscore3.pass_fail = "N/A";
                        tblscore3.original_qa_score = null;
                        tblscore3.missed_list = null;
                        tblscore3.formatted_comments = null;
                        tblscore3.formatted_missed = null;
                        int resultform_score3 = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("delete from calibration_form where original_form = " + x_id);
                    var iscalibrationForm = dataContext.calibration_form.Where(x => x.original_form == x_id).FirstOrDefault();
                    calibration_form tblcalibration_form = new calibration_form();
                    if (iscalibrationForm != null)
                    {
                        dataContext.calibration_form.Remove(iscalibrationForm);
                        dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("delete from calibration_pending where form_id = " + x_id + " and date_completed is null");
                    var iscalibration_pending = dataContext.calibration_pending.Where(x => x.form_id == x_id && x.date_completed == null).FirstOrDefault();
                    calibration_pending tblcalibration_pending = new calibration_pending();
                    if (iscalibration_pending != null)
                    {
                        dataContext.calibration_pending.Remove(iscalibration_pending);
                        dataContext.SaveChanges();
                    }
                    // Common.UpdateTable("delete from form_notifications where form_id = " + x_id + " and date_closed is null");
                    var isform_notifications = dataContext.form_notifications.Where(x => x.form_id == x_id && x.date_closed == null).FirstOrDefault();
                    form_notifications tblform_notifications = new form_notifications();
                    if (isform_notifications != null)
                    {
                        dataContext.form_notifications.Remove(isform_notifications);
                        dataContext.SaveChanges();
                    }
                }
            }
            return objButtonAction;
        }
        #endregion Public MarkCallBad2

        #region Public MarkCallBad
        /// <summary>
        /// MarkCallBad
        /// </summary>
        /// <param name="markBadCallData"></param>
        public void MarkCallBad(MarkBadCallData markBadCallData)
        {
            int x_id = markBadCallData.x_id;
            string reject_reason;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return;
            }

            if (markBadCallData.reject_reason != null)
            {
                reject_reason = markBadCallData.reject_reason;
            }
            else
            {
                reject_reason = "Known Bad";
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                DateTime startdate1 = Convert.ToDateTime("1/1/1970");
                DateTime startdate = startdate1.AddSeconds(double.Parse(markBadCallData.qa_start));
                DateTime enddate1 = Convert.ToDateTime("1/1/1970");
                DateTime enddate = enddate1.AddSeconds(double.Parse(markBadCallData.qa_last_action));
                int review_time = int.Parse(markBadCallData.qa_last_action) - int.Parse(markBadCallData.qa_start);
                //DataTable dt = GetTable("select auto_accept_bad_call from xcc_report_new join scorecards on scorecards.id = scorecard and xcc_report_new.id = " + x_id);
                var scorecardResTemp = (from XCC_REPORT_NEW in dataContext.XCC_REPORT_NEW
                                        join scorecards in dataContext.scorecards on XCC_REPORT_NEW.scorecard equals scorecards.id
                                        where XCC_REPORT_NEW.ID == x_id
                                        select (scorecards.auto_accept_bad_call)).FirstOrDefault();

                if (scorecardResTemp.Equals(1))
                {
                    //Common.UpdateTable("update xcc_report_new set bad_call = 1, bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who ='" + HttpContext.Current.User.Identity.Name + "',  bad_call_who='" + HttpContext.Current.User.Identity.Name + "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" + reject_reason.Replace("'", "''") + "' where id = " + x_id);
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == x_id).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(isExist.ID);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.bad_call = 1;
                        tblXCC_REPORT_NEW.bad_call_accepted = Date;
                        tblXCC_REPORT_NEW.bad_call_accepted_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_date = Date;
                        tblXCC_REPORT_NEW.MAX_REVIEWS = 1;
                        tblXCC_REPORT_NEW.bad_call_reason = reject_reason.Replace("'", "''");
                        int result = dataContext.SaveChanges();
                    }
                }
                else
                {

                    //Common.UpdateTable("update xcc_report_new set bad_call = 1,  bad_call_who='" + HttpContext.Current.User.Identity.Name + "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" + reject_reason.Replace("'", "''") + "' where id = " + x_id);
                    var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == x_id).FirstOrDefault();
                    XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                    if (isExist != null)
                    {
                        tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(isExist.ID);
                        dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                        tblXCC_REPORT_NEW.bad_call = 1;
                        tblXCC_REPORT_NEW.bad_call_who = HttpContext.Current.User.Identity.Name;
                        tblXCC_REPORT_NEW.bad_call_date = Date;
                        tblXCC_REPORT_NEW.MAX_REVIEWS = 1;
                        tblXCC_REPORT_NEW.bad_call_reason = reject_reason.Replace("'", "''");
                        int result = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("update vwForm set calib_score = null, total_score=null, total_score_with_fails = null, display_score = null,  pass_fail='N/A',original_qa_score=null,   qa_start = '" + startdate.ToString() + "', qa_last_action='" + enddate + "', review_time = '" + review_time + "' where review_id = " + x_id);
                    var tblscore = dataContext.form_score3.Where(x => x.review_ID == x_id).FirstOrDefault();
                    form_score3 tblscore3 = new form_score3();
                    if (tblscore != null)
                    {
                        tblscore3 = dataContext.form_score3.Find(tblscore.id);
                        dataContext.Entry(tblscore3).State = EntityState.Modified;
                        tblscore3.calib_score = null;
                        tblscore3.total_score = null;
                        tblscore3.total_score_with_fails = null;
                        tblscore3.display_score = null;
                        tblscore3.pass_fail = "N/A";
                        tblscore3.original_qa_score = null;
                        tblscore3.qa_start = startdate;
                        tblscore3.qa_last_action = enddate;
                        tblscore3.review_time = review_time;
                        int resultform_score3 = dataContext.SaveChanges();
                    }

                    //Common.UpdateTable("delete from calibration_form where original_form in (select f_id from vwForm where review_id = " + x_id + ")");
                    var isform = dataContext.vwForms.Where(x => x.review_ID == x_id).FirstOrDefault();
                    int f_Id = 0;
                    if (!object.Equals(isform, null))
                    {
                        f_Id = isform.F_ID;
                    }
                    var iscalibration_form = dataContext.calibration_form.Where(x => x.original_form == f_Id).FirstOrDefault();
                    calibration_form tblcalibration_form = new calibration_form();
                    if (iscalibration_form != null)
                    {
                        dataContext.calibration_form.Remove(iscalibration_form);
                        dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("delete from calibgetScoreration_pending where form_id in (select f_id from vwForm where review_id = " + x_id + ") and date_completed is null");
                    //Common.UpdateTable("delete from calibration_pending where form_id in (select f_id from vwForm where review_id = " + x_id + ") and date_completed is null");
                    var iscalibration_pending = dataContext.calibration_pending.Where(x => x.form_id == f_Id && x.date_completed == null).FirstOrDefault();
                    calibration_pending tblcalibration_pending = new calibration_pending();
                    if (iscalibration_pending != null)
                    {
                        dataContext.calibration_pending.Remove(tblcalibration_pending);
                        dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("delete from form_notifications where form_id in (select f_id from vwForm where review_id = " + x_id + ") and date_closed is null");
                    var isform_notifications = dataContext.form_notifications.Where(x => x.form_id == f_Id && x.date_closed == null).FirstOrDefault();
                    form_notifications tblform_notifications = new form_notifications();
                    if (isform_notifications != null)
                    {
                        dataContext.form_notifications.Remove(isform_notifications);
                        dataContext.SaveChanges();
                    }
                }
            }
        }
        #endregion Public MarkCallBad

        /// <summary>
        /// GetScorecardRecordID
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        public CompleteScorecard GetScorecardRecordID(SimpleID simpleID)
        {
            CompleteScorecard sc = new CompleteScorecard();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int Id = 0;
                if (simpleID.ID != null)
                {
                    Id = Convert.ToInt32(simpleID.ID);
                }
                //DataTable fid_dt = GetTable("select review_id, scorecard from vwForm where f_id = " + simpleID.ID);
                var isform = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                if (object.Equals(isform, null))
                {
                    return sc;
                }
                int scorecard_ID = Convert.ToInt32(isform.scorecard);
                int xcc_id = Convert.ToInt32(isform.review_ID);

                string username = HttpContext.Current.User.Identity.Name;
                //var getPopulatedScorecard = dataContext.getPopulatedScorecard(Convert.ToInt32(scorecard_ID), Convert.ToInt32(xcc_id), username, 1).ToList();
                //SqlCommand scorecard_sq = new SqlCommand("getPopulatedScorecard");
                //DataSet ds = Common.getTables();
                var getPopulatedScorecard = GetPopulatedScorecard(scorecard_ID, xcc_id, username, 1).ToList();
                sc = populateScorecardData(getPopulatedScorecard, xcc_id.ToString());
                if (object.Equals(sc, null))
                {
                    return sc;
                }
                if (!Information.IsNumeric(xcc_id))
                {
                    xcc_id = 0;
                }
                if (scorecard_ID == 0)
                {
                    return sc;
                }
                string add_sql;
                if (HttpContext.Current.Request.QueryString["appname"] != null)
                {
                    add_sql = HttpContext.Current.Request.QueryString["appname"];
                }
                else
                {
                    var add_sql1 = dataContext.UserApps.Where(x => x.username == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                    add_sql = add_sql1.appname;
                    //add_sql = " and appname in (select appname from userapps where username= '" + HttpContext.Current.User.Identity.Name + "') ";
                }
                //DataTable sc_dt = GetTable("select * from scorecards where id = " + scorecard_ID + " " + add_sql);
                var sc_dt = dataContext.scorecards.Where(x => x.id == scorecard_ID && x.appname == add_sql).FirstOrDefault();
                if (sc_dt == null)
                {
                    sc.ScorecardName = "No data/No authorized.";
                    return sc;
                }
                sc.ScorecardName = sc_dt.short_name;
                sc.Appname = sc_dt.appname;
                sc.Status = sc_dt.scorecard_status;
                sc.Description = sc_dt.description;

                List<WebApi.Models.CCInternalAPI.Section> sec_list = new List<WebApi.Models.CCInternalAPI.Section>();
                //DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
                var isSections = dataContext.Questions.Where(x => x.active == true && x.scorecard_id == scorecard_ID).ToList();
                foreach (var item in isSections)
                {
                    List<Models.CCInternalAPI.Question> ques_list = new List<Models.CCInternalAPI.Question>();
                    List<string> temp_list = new List<string>();
                    List<TemplateItem> objTemplateItem = new List<TemplateItem>();
                    var section_dt = dataContext.Sections.Where(x => x.id == item.section).ToList();
                    foreach (var item1 in section_dt)
                    {
                        WebApi.Models.CCInternalAPI.Section sec = new WebApi.Models.CCInternalAPI.Section();
                        sec.section = item1.section1;
                        sec.description = item1.Descrip;
                        if (xcc_id != 0)
                        {
                            var q_dt = dataContext.getListenQuestions_SectionLess(item1.id, scorecard_ID, xcc_id, 1, HttpContext.Current.User.Identity.Name).ToList();
                            //q_dt = GetTable("exec [getListenQuestions_SectionLess] " + item1["id"].ToString() + ", " + scorecard_ID + "," + xcc_id + ", @username='" + HttpContext.Current.User.Identity.Name + "'");
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CCInternalAPI.Question ques = new WebApi.Models.CCInternalAPI.Question();
                                ques.QuestionShort = Strings.Replace(Strings.Replace(drq.q_short_name.ToString(), "[", @"\["), "]", @"\]");
                                ques.question = drq.question.ToString();
                                ques.LinkedAnswer = Convert.IsDBNull(drq.linked_answer) ? new int?() : (int)drq.linked_answer;
                                ques.LinkedComment = Convert.IsDBNull(drq.linked_comment) ? new int?() : (int)drq.linked_comment;
                                ques.LinkedVisible = Convert.ToBoolean(drq.linked_visible);
                                ques.SingleComment = Convert.ToBoolean(drq.single_comment);
                                ques.WideQuestion = Convert.ToBoolean(drq.wide_q);
                                ques.RequireCustomComment = Convert.ToBoolean(drq.require_custom_comment);
                                //ques.QAPoints =drq.QA_points;
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.ID);
                                if (drq.template.ToString() == "Contact")
                                {
                                    var isquetionoptions = dataContext.question_options.Where(x => x.question_id == drq.ID && (x.date_end == null || System.DateTime.Now <= System.DateTime.Now)).OrderBy(p => p.option_order).ToList();
                                    //DataTable ti_dt = GetTable("select option_text, id from question_options where  isnull(date_end, getdate()) <= getdate() and  question_id = " + drq.ID + " order by isnull(option_order, 99)");
                                    foreach (var ti_dr in isquetionoptions)
                                    {
                                        if (ti_dr.option_text.ToString() != "")
                                        {
                                            TemplateItem titem = new TemplateItem();
                                            temp_list.Add(ti_dr.option_text);
                                            titem.option_text = ti_dr.option_text;
                                            titem.option_id = Convert.ToInt32(ti_dr.id);
                                            objTemplateItem.Add(titem);
                                        }
                                    }
                                    ques.TemplateItems = objTemplateItem;
                                    ques.TemplateOptions = temp_list;
                                }
                                List<Answer> ans_list = new List<Answer>();
                                var form_q_scores = dataContext.form_q_scores.Where(x => x.form_id == Id).ToList();
                                foreach (var itemf in form_q_scores)
                                {
                                    var isquestion_answers = dataContext.question_answers.Where(x => x.question_id == drq.ID && (x.answer_active == true || x.question_id == itemf.question_answered)).OrderBy(p => p.answer_order).ToList();
                                    //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq["id"] + " and (answer_active=1 or question_answers.id in (select question_answered from form_q_scores where form_id = " + simpleID.ID + "))  order by answer_order");
                                    foreach (var dra in isquestion_answers)
                                    {
                                        Answer ans = new Answer();
                                        ans.Answers = dra.answer_text.ToString();
                                        if (dra.answer_points != 0)
                                        {
                                            ans.Points = Convert.ToInt32(dra.answer_points);
                                        }
                                        else
                                        {
                                            ans.Points = 0;
                                        }
                                        ans.RequireCustomComment = Convert.ToBoolean(dra.custom_comment_required);
                                        ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                        if (dra.autoselect.ToString() == "True")
                                        {
                                            ans.autoselect = 1;
                                        }
                                        else
                                        {
                                            ans.autoselect = 0;
                                        }
                                        ans.AnswerID = Convert.ToInt32(dra.id.ToString());
                                        var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id && x.ac_active == true).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                        //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + "  and ac_active = 1  order by isnull(ac_order,10000)");
                                        List<Comment> cmt_list = new List<Comment>();
                                        foreach (var drcmt in isanswer_comments)
                                        {
                                            Comment cmt = new Comment();
                                            cmt.CommentText = drcmt.comment;
                                            cmt.CommentID = Convert.ToInt32(drcmt.id);
                                            cmt.CommentPoints = Convert.IsDBNull(drcmt.comment_points) ? new float() : (float)drcmt.comment_points;
                                            cmt_list.Add(cmt);
                                        }
                                        ans.Comments = cmt_list;
                                        ans_list.Add(ans);
                                    }
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.ID).ToList();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;
                                List<FAQ> faq_list = new List<FAQ>();
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.ID).OrderBy(p => p.q_order).ToList();
                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                foreach (var drfaq in isq_faqs)
                                {
                                    FAQ faq = new FAQ();
                                    faq.QuestionText = drfaq.question_text;
                                    faq.AnswerText = drfaq.question_answer;
                                    faq_list.Add(faq);
                                }
                                ques.FAQs = faq_list;
                                ques_list.Add(ques);
                            }
                            sec.questions = ques_list;
                            if (sec.questions.Count > 0)
                            {
                                sec_list.Add(sec);
                            }
                        }
                        else
                        {
                            //q_dt = GetTable("exec [getListenQuestions_SectionLess] " + item1["id"].ToString() + ", " + scorecard_ID + ", @username='" + HttpContext.Current.User.Identity.Name + "'");
                            var q_dt = dataContext.getListenQuestions_SectionLess(item1.id, scorecard_ID, null, null, HttpContext.Current.User.Identity.Name).ToList();
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CCInternalAPI.Question ques = new WebApi.Models.CCInternalAPI.Question();
                                ques.QuestionShort = Strings.Replace(Strings.Replace(drq.q_short_name, "[", @"\["), "]", @"\]");
                                ques.question = drq.question.ToString();
                                ques.LinkedAnswer = Convert.IsDBNull(drq.linked_answer) ? new int?() : (int)drq.linked_answer;
                                ques.LinkedComment = Convert.IsDBNull(drq.linked_comment) ? new int?() : (int)drq.linked_comment;
                                ques.LinkedVisible = Convert.ToBoolean(drq.linked_visible);
                                ques.SingleComment = Convert.ToBoolean(drq.single_comment);
                                ques.WideQuestion = Convert.ToBoolean(drq.wide_q);
                                ques.RequireCustomComment = Convert.ToBoolean(drq.require_custom_comment);
                                //ques.QAPoints =drq.QA_points;
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.ID);
                                if (drq.template == "Contact")
                                {

                                    var isquetionoptions = dataContext.question_options.Where(x => x.question_id == drq.ID && (x.date_end == null || System.DateTime.Now <= System.DateTime.Now)).OrderBy(p => p.option_order).ToList();
                                    //DataTable ti_dt = GetTable("select option_text, id from question_options where  isnull(date_end, getdate()) <= getdate() and  question_id = " + drq.ID + " order by isnull(option_order, 99)");
                                    foreach (var ti_dr in isquetionoptions)
                                    {
                                        if (ti_dr.option_text != "")
                                        {
                                            TemplateItem titem = new TemplateItem();
                                            temp_list.Add(ti_dr.option_text);
                                            titem.option_text = ti_dr.option_text;
                                            titem.option_id = Convert.ToInt32(ti_dr.id);
                                            objTemplateItem.Add(titem);
                                        }
                                    }
                                    ques.TemplateItems = objTemplateItem;
                                    ques.TemplateOptions = temp_list;
                                }
                                List<Answer> ans_list = new List<Answer>();
                                int formId = 0;
                                if (simpleID.ID == "")
                                {
                                    formId = Convert.ToInt32(simpleID.ID);
                                }
                                var form_q_scores = dataContext.form_q_scores.Where(x => x.form_id == formId).ToList();
                                foreach (var itemf in form_q_scores)
                                {
                                    var isquestion_answers = dataContext.question_answers.Where(x => x.question_id == drq.ID && (x.answer_active == true || x.question_id == itemf.question_answered)).OrderBy(p => p.answer_order).ToList();
                                    //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq["id"] + " and (answer_active=1 or question_answers.id in (select question_answered from form_q_scores where form_id = " + simpleID.ID + "))  order by answer_order");

                                    foreach (var dra in isquestion_answers)
                                    {
                                        Answer ans = new Answer();
                                        ans.Answers = dra.answer_text;
                                        if (dra.answer_points != 0)
                                        {
                                            ans.Points = Convert.ToInt32(dra.answer_points);
                                        }
                                        else
                                        {
                                            ans.Points = 0;
                                        }
                                        ans.RequireCustomComment = Convert.ToBoolean(dra.custom_comment_required);
                                        ans.RightAnswer = Convert.ToBoolean(dra.right_answer);

                                        if (dra.autoselect.ToString() == "True")
                                        {
                                            ans.autoselect = 1;
                                        }
                                        else
                                        {
                                            ans.autoselect = 0;
                                        }
                                        ans.AnswerID = Convert.ToInt32(dra.id.ToString());
                                        var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id && x.ac_active == true).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                        //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + "  and ac_active = 1  order by isnull(ac_order,10000)");
                                        List<Comment> cmt_list = new List<Comment>();
                                        foreach (var drcmt in isanswer_comments)
                                        {
                                            Comment cmt = new Comment();
                                            cmt.CommentText = drcmt.comment;
                                            cmt.CommentID = Convert.ToInt32(drcmt.id);
                                            cmt.CommentPoints = Convert.IsDBNull(drcmt.comment_points) ? new float() : (float)drcmt.comment_points;
                                            cmt_list.Add(cmt);
                                        }
                                        ans.Comments = cmt_list;
                                        ans_list.Add(ans);
                                    }
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.ID).ToList();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;
                                List<FAQ> faq_list = new List<FAQ>();
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.ID).OrderBy(p => p.q_order).ToList();
                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                foreach (var drfaq in isq_faqs)
                                {
                                    FAQ faq = new FAQ();
                                    faq.QuestionText = drfaq.question_text;
                                    faq.AnswerText = drfaq.question_answer;
                                    faq_list.Add(faq);
                                }
                                ques.FAQs = faq_list;
                                ques_list.Add(ques);
                            }
                            sec.questions = ques_list;
                            if (sec.questions.Count > 0)
                            {
                                sec_list.Add(sec);
                            }
                        }
                    }
                    if (xcc_id != 0)
                    {
                        var isotherformdata = dataContext.otherFormDatas.Where(x => x.xcc_id == Convert.ToInt32(xcc_id) && x.data_type == "customquestion").Select(dt => new { dt.data_key, dt.label, dt.school_name, dt.data_type, dt.data_value }).Distinct().ToList();
                        //DataTable isotherformdata = GetTable("Select  distinct data_key,label, school_name,data_type,data_value from otherformdata where xcc_id = '" + xcc_id + "' and data_type = 'customquestion'"); // 
                        if (isotherformdata.Count > 0)
                        {
                            WebApi.Models.CCInternalAPI.Section sec = new WebApi.Models.CCInternalAPI.Section();
                            sec.section = "Custom Questions";
                            sec.description = "Custom Questions";
                            List<WebApi.Models.CCInternalAPI.Question> ques_list1 = new List<WebApi.Models.CCInternalAPI.Question>();
                            foreach (var cq_dr in isotherformdata)
                            {
                                WebApi.Models.CCInternalAPI.Question ques = new WebApi.Models.CCInternalAPI.Question();
                                ques.QuestionShort = cq_dr.data_key;
                                ques.question = cq_dr.label;
                                //ques.QID = '-' + Convert.ToInt32(cq_dr.id);
                                List<Answer> ans_list = new List<Answer>();
                                Answer ans = new Answer();
                                ans.Answers = "Yes";
                                ans.Points = 0;
                                ans.RightAnswer = true;
                                ans.autoselect = 0;
                                ans.AnswerID = -1;
                                List<Comment> cmt_list = new List<Comment>();
                                Comment cmt = new Comment();
                                cmt.CommentText = "Agent did ask this custom question";
                                cmt.CommentID = -1;
                                cmt_list.Add(cmt);
                                ans.Comments = cmt_list;
                                ans_list.Add(ans);
                                ans = new Answer();
                                ans.Answers = "No";
                                ans.Points = 0;
                                ans.RightAnswer = false;
                                ans.autoselect = 0;
                                ans.AnswerID = -2;
                                cmt_list = new List<Comment>();
                                cmt = new Comment();
                                cmt.CommentText = "Agent did NOT ask this custom question";
                                cmt.CommentID = -2;
                                cmt_list.Add(cmt);
                                ans.Comments = cmt_list;
                                ans_list.Add(ans);
                                ques.FAQs = new List<FAQ>();
                                ques.instructions = new List<Instruction>();
                                ques.answers = ans_list;
                                ques_list1.Add(ques);
                            }
                            sec.questions = ques_list1;
                            sec_list.Add(sec);
                        }

                    }
                    sc.Sections = sec_list;
                    var isscInputs = dataContext.sc_inputs.Where(x => x.scorecard == scorecard_ID && x.active == true).OrderBy(p => p.value_order).ToList();
                    //DataTable clerk_dt = GetTable("Select * from sc_inputs where scorecard = " + scorecard_ID + " And active = 1 order by value_order");
                    if (isscInputs.Count > 0)
                    {
                        List<ClerkedData> cds = new List<ClerkedData>();
                        foreach (var clerk_item in isscInputs)
                        {
                            ClerkedData objClerkedData = new ClerkedData();
                            objClerkedData.value = clerk_item.value;
                            objClerkedData.ID = clerk_item.id.ToString();
                            objClerkedData.required = Convert.ToBoolean(clerk_item.required_value);
                            cds.Add(objClerkedData);
                        }
                        sc.ClerkData = cds;
                    }
                }
                return sc;
            }
        }

        #region Public GetScorecardRecordListen
        /// <summary>
        /// GetScorecardRecordListen
        /// </summary>
        /// <param name="scorecard_ID"></param>
        /// <param name="xcc_id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public CompleteScorecard GetScorecardRecordListen(string scorecard_ID, string xcc_id, string username)
        {
            CompleteScorecard objCompleteScorecard = new CompleteScorecard();
            try
            {
                //username = "B_JessicaArroyo";
                //var getPopulatedScorecard = dataContext.getPopulatedScorecard(Convert.ToInt32(scorecard_ID), Convert.ToInt32(xcc_id), username, 0).ToList();
                int xccId = 0;
                if (xcc_id != "")
                {
                    xccId = Convert.ToInt32(xcc_id);
                }
                int scoreCardId = 0;
                if (xcc_id != "")
                {
                    scoreCardId = Convert.ToInt32(scorecard_ID);
                }
                var getPopulatedScorecard = GetPopulatedScorecard(scoreCardId, xccId, username, 0).ToList();
                objCompleteScorecard = populateScorecardData(getPopulatedScorecard, xcc_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCompleteScorecard;
        }
        #endregion Public GetScorecardRecordListen

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="xcc_id"></param>
        /// <returns></returns>
        private CompleteScorecard populateScorecardData(List<IEnumerable> ds, string xcc_id)
        {
            CompleteScorecard sc = new CompleteScorecard();
            var getPopulatedScorecard = ds[0].Cast<getPopulatedScorecard_Result>();
            var sc_dt = getPopulatedScorecard.FirstOrDefault();

            List<section_order_Result> section_dt = ds[1].Cast<section_order_Result>().ToList();
            List<getListenQuestionsAll_Result> q_dt = ds[2].Cast<getListenQuestionsAll_Result>().ToList();
            List<question_options_Result> ti_dt = ds[3].Cast<question_options_Result>().ToList();
            List<question_answers_Result> ans_dt = ds[4].Cast<question_answers_Result>().ToList();
            List<q_instructions> instr_dt = ds[5].Cast<q_instructions>().ToList();
            List<q_faqs> faq_dt = ds[6].Cast<q_faqs>().ToList();
            List<getotherformdata_Result> cq_dt = ds[7].Cast<getotherformdata_Result>().ToList();
            List<sc_inputs> clerk_dt = ds[8].Cast<sc_inputs>().ToList();
            List<answer_comments> cmt_dt = ds[9].Cast<answer_comments>().ToList();

            string other_sort_order = string.Empty;
            if (sc_dt.meta_sort == "Alphbetical")
            {
                other_sort_order = " order by data_key";
            }
            sc.ScorecardName = sc_dt.short_name;
            sc.Appname = sc_dt.appname;
            sc.Status = sc_dt.scorecard_status;
            sc.Description = sc_dt.description;
            sc.ReviewType = sc_dt.review_type;

            List<WebApi.Models.CCInternalAPI.Section> sec_list = new List<WebApi.Models.CCInternalAPI.Section>();
            //DataTable section_dt = ds.Tables[1];
            //DataTable q_dt = ds.Tables[2];
            //DataTable ti_dt = ds.Tables[3];
            //DataTable ans_dt = ds.Tables[4];
            //DataTable instr_dt = ds.Tables[5];
            //DataTable faq_dt = ds.Tables[6];
            //DataTable cq_dt = ds.Tables[7];
            //DataTable clerk_dt = ds.Tables[8];
            //DataTable cmt_dt = ds.Tables[9];
            //DataTable ddl_dt = ds.Tables[10];
            foreach (var item in section_dt)
            {
                WebApi.Models.CCInternalAPI.Section sec = new WebApi.Models.CCInternalAPI.Section();
                sec.section = item.section;
                sec.description = item.descrip;
                List<WebApi.Models.CCInternalAPI.Question> ques_list = new List<WebApi.Models.CCInternalAPI.Question>();

                //if (item.ID.ToString() == "0")
                //{
                //    filteredq_dt = q_dt.Select("1=1", "q_order");
                //}
                //else
                //{
                //    filteredq_dt = q_dt.Select("section = " + item.id, "q_order");
                //}
                foreach (var drq in q_dt)
                {
                    WebApi.Models.CCInternalAPI.Question ques = new WebApi.Models.CCInternalAPI.Question();
                    ques.QuestionShort = drq.q_short_name;
                    ques.question = drq.question;
                    ques.LinkedAnswer = drq.linked_answer;
                    ques.LinkedComment = drq.linked_comment;

                    ques.LinkedVisible = Convert.ToBoolean(drq.linked_visible);
                    ques.SingleComment = Convert.ToBoolean(drq.single_comment);
                    ques.WideQuestion = Convert.ToBoolean(drq.wide_q);
                    ques.RequireCustomComment = Convert.ToBoolean(drq.require_custom_comment);
                    ques.QuestionType = drq.q_type;
                    if (drq.q_order.ToString() != "")
                    {
                        ques.order = drq.q_order.ToString();
                    }
                    ques.DropDownType = drq.ddl_type;
                    ques.LeftColumnQuestion = Convert.ToBoolean(drq.left_column_question);
                    ques.OptionVerb = drq.options_verb;
                    ques.sentence = drq.sentence;
                    if (drq.ddl_type == "API")
                    {
                        ques.DropDownType = drq.ddlQuery;
                    }
                    //ques.QAPoints = Convert.ToInt32(drq.QA_points);
                    ques.QAPoints = 0;
                    ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                    ques.QID = Convert.ToInt32(drq.id);
                    if (drq.template.ToString() == "Contact")
                    {
                        List<string> tem_list = new List<string>();
                        List<TemplateItem> ti = new List<TemplateItem>();
                        //DataRow[] filteredti_dt = ti_dt.Select("question_id = " + drq.id);
                        foreach (var ti_dr in ti_dt)
                        {
                            TemplateItem titem = new TemplateItem();
                            tem_list.Add(ti_dr.option_text);
                            titem.option_text = ti_dr.option_text;
                            titem.option_id = Convert.ToInt32(ti_dr.id);
                            ti.Add(titem);
                        }
                        ques.TemplateOptions = tem_list;
                        ques.TemplateItems = ti;
                    }
                    List<Answer> ans_list = new List<Answer>();

                    //var filteredans_dt = ans_dt.Select("question_id = " + drq["id"]);
                    var filteredans_dt = ans_dt.Where(x => x.question_id == drq.id).ToList();
                    foreach (var dra in filteredans_dt)
                    {
                        Answer ans = new Answer();
                        ans.Answers = dra.answer_text;
                        ans.acp_required = Convert.ToBoolean(dra.acp_required);
                        if (dra.answer_points != 0)
                        {
                            ans.Points = Convert.ToInt32(dra.answer_points);
                        }
                        else
                        {
                            ans.Points = 0;
                        }
                        ans.RequireCustomComment = Convert.ToBoolean(dra.custom_comment_required);
                        ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                        if (dra.autoselect == true)
                        {
                            ans.autoselect = 1;
                        }
                        else
                        {
                            ans.autoselect = 0;
                        }
                        ans.AnswerID = dra.id;
                        //DataRow[] filteredcmt_dt = cmt_dt.Select("answer_id = " + dra["id"]);
                        var filteredcmt_dt = cmt_dt.Where(x => x.question_id == dra.id).ToList();
                        List<Comment> cmt_list = new List<Comment>();
                        foreach (var drcmt in filteredcmt_dt)
                        {
                            Comment cmt = new Comment();
                            cmt.CommentText = drcmt.comment;
                            cmt.CommentID = Convert.ToInt32(drcmt.id);
                            cmt.AnswerStatement = drcmt.answer_statement;
                            cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                            cmt_list.Add(cmt);
                        }

                        // Comments from dropdowns are simply answer comments presented in drop down form for YES answer only
                        if (dra.answer_text == "Yes")
                        {
                            // DataRow[] filteredddl_dt = ddl_dt.Select("question_id = " + drq["id"]);
                            // List<string> ddlList = new List<string>();
                            // foreach (var drcmt in filteredddl_dt)
                            // {
                            //    ddlList.Add(drcmt["value"].ToString());
                            // }
                            // ans.DropdownItems = ddlList;
                        }
                        ans.Comments = cmt_list;
                        ans_list.Add(ans);
                    }
                    //        ques.answers = ans_list;
                    var filteredinstr_dt = instr_dt.Where(x => x.question_id == drq.id).ToList();
                    List<Instruction> instr_list = new List<Instruction>();
                    foreach (var drinst in filteredinstr_dt)
                    {

                        Instruction instr = new Instruction();
                        instr.InstructionText = drinst.question_text;
                        instr_list.Add(instr);
                    }
                    ques.instructions = instr_list;
                    var filteredfaq_dt = faq_dt.Where(x => x.question_id == drq.id).ToList();
                    //DataRow[] filteredfaq_dt = faq_dt.Select("question_id = " + drq["id"]);
                    List<FAQ> faq_list = new List<FAQ>();
                    foreach (var drfaq in filteredfaq_dt)
                    {
                        FAQ faq = new FAQ();
                        faq.QuestionText = drfaq.question_text;
                        faq.AnswerText = drfaq.question_answer;
                        faq_list.Add(faq);
                    }
                    ques.FAQs = faq_list;
                    ques_list.Add(ques);
                }
                sec.questions = ques_list;

                if (sec.questions.Count > 0)
                {
                    sec_list.Add(sec);
                }
                // }

                if (xcc_id != "" & sc_dt.show_custom_questions.ToString() == "True")
                {
                    if (cq_dt.Count > 0)
                    {
                        //Models.CCInternalAPI.Section sec = new Models.CCInternalAPI.Section();
                        sec.section = "Custom Questions";
                        sec.description = "Custom Questions";
                        //List<WebApi.Models.CCInternalAPI.Question> ques_list = new List<WebApi.Models.CCInternalAPI.Question>();
                        foreach (var cq_dr in cq_dt)
                        {
                            WebApi.Models.CCInternalAPI.Question ques = new WebApi.Models.CCInternalAPI.Question();
                            ques.QuestionShort = cq_dr.data_key;
                            ques.question = cq_dr.label;

                            ques.QID = '-' + Convert.ToInt32(cq_dr.id);
                            List<FAQ> faq_list = new List<FAQ>();
                            FAQ faq = new FAQ();
                            faq.QuestionText = "Verify the Agent asked the question: ";
                            faq.AnswerText = cq_dr.label;
                            faq_list.Add(faq);
                            faq = new FAQ();
                            faq.QuestionText = "Extra Info: ";
                            faq.AnswerText = "School: " + cq_dr.school_name + "<br>" + "Answer: " + cq_dr.data_value;
                            faq_list.Add(faq);

                            ques.FAQs = faq_list;
                            List<Answer> ans_list = new List<Answer>();
                            Answer ans = new Answer();
                            ans.Answers = "Yes";
                            ans.Points = 0;
                            ans.RightAnswer = Convert.ToBoolean("True");
                            ans.autoselect = 0;
                            ans.AnswerID = -1;
                            List<Comment> cmt_list = new List<Comment>();
                            Comment cmt = new Comment();
                            cmt.CommentText = "Agent did ask this custom question";
                            cmt.CommentID = -1;
                            cmt_list.Add(cmt);
                            ans.Comments = cmt_list;
                            ans_list.Add(ans);
                            ans = new Answer();
                            ans.Answers = "No";
                            ans.Points = 0;
                            ans.RightAnswer = Convert.ToBoolean("False");
                            ans.autoselect = 0;
                            ans.AnswerID = -2;
                            cmt_list = new List<Comment>();
                            cmt = new Comment();
                            cmt.CommentText = "Agent did NOT ask this custom question";
                            cmt.CommentID = -2;
                            cmt_list.Add(cmt);
                            ans.Comments = cmt_list;
                            ans_list.Add(ans);
                            ques.instructions = new List<Instruction>();
                            ques.answers = ans_list;
                            ques_list.Add(ques);
                        }
                        sec.questions = ques_list;
                        sec_list.Add(sec);
                    }
                }
                sc.Sections = sec_list;
                if (clerk_dt.Count > 0)
                {
                    List<ClerkedData> cds = new List<ClerkedData>();
                    foreach (var clerk_item in clerk_dt)
                    {
                        ClerkedData cd = new ClerkedData();
                        cd.value = clerk_item.value;
                        cd.ID = clerk_item.id.ToString();
                        try
                        {
                            cd.required = Convert.ToBoolean(clerk_item.required_value);
                        }
                        catch (Exception ex)
                        {
                        }
                        cds.Add(cd);
                    }
                    sc.ClerkData = cds;
                }
            }
            return sc;
        }

        #region GetpoulatedScoreCard
        /// <summary>
        /// GetpoulatedScoreCard
        /// </summary>
        /// <param name="scorecard"></param>
        /// <param name="xcc_id"></param>
        /// <param name="username"></param>
        /// <param name="show_calc"></param>
        /// <returns></returns>
        public List<IEnumerable> GetPopulatedScorecard(Nullable<int> scorecard, Nullable<int> xcc_id, string username, Nullable<int> show_calc)
        {
            try
            {
                List<IEnumerable> result = new List<IEnumerable>();
                //second way to get multiple result set in entity
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    var command = new SqlCommand()
                    {
                        CommandText = "[dbo].[getPopulatedScorecard]",
                        CommandType = CommandType.StoredProcedure
                    };

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@scorecard",scorecard),
                        new SqlParameter("@xcc_id",xcc_id),
                        new SqlParameter("@username",username),
                        new SqlParameter("@show_calc",show_calc)
                    };

                    command.Parameters.AddRange(parameters.ToArray());

                    result = dataContext.MultipleResults(command)
                                   .With<getPopulatedScorecard_Result>()
                                   .With<section_order_Result>()
                                   .With<getListenQuestionsAll_Result>()
                                   .With<question_options_Result>()
                                   .With<question_answers_Result>()
                                   .With<q_instructions>()
                                   .With<q_faqs>()
                                   .With<getotherformdata_Result>()
                                   .With<sc_inputs>()
                                   .With<answer_comments>()
                                   .Execute();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        #endregion

        #region Public getCoachingQueueJson
        /// <summary>
        /// getCoachingQueueJson
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CoachingQueue> getCoachingQueueJson(string userName, string filter)
        {
            List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {

                    filter = string.Empty;
                    var result = dataContext.Database.SqlQuery<getCoachingQueueJson_Result>(
                    " exec getCoachingQueueJson @username='" + userName + "',@filter= '" + filter + "'").ToList();
                    var item = (from resut in result
                                select new CoachingQueue
                                {
                                    NotificationID = resut.NotificationID,
                                    agent = resut.agent,
                                    total_score = resut.total_score,
                                    call_date = resut.call_date,
                                    dateadded = resut.dateadded,
                                    notificationStep = resut.notificationStep,
                                    form_id = resut.form_id.ToString(),
                                    form_id_plus = resut.form_id_plus,
                                    first_error = resut.first_error,
                                    OwnedNotification = resut.OwnedNotification,
                                    AllComments = resut.AllComments,
                                }).ToList();
                    coachingQueueLst.AddRange(item);
                }
                return coachingQueueLst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Public getCoachingQueueJson

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirty_string"></param>
        /// <returns></returns>
        private string clean_string(string dirty_string)
        {
            if (dirty_string != null)
                return dirty_string.Replace("+", " ").Replace("%20", " ");
            else
                return dirty_string;
        }

        #region Public AddRecord
        /// <summary>
        /// AddRecord
        /// </summary>
        /// <param name="addRecordData"></param>
        /// <returns></returns>
        public string AddRecord(AddRecordData addRecordData)
        {
            string Message = "";
            int result = 0;
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    string SESSION_ID = clean_string(addRecordData.SESSION_ID);
                    string AGENT = clean_string(addRecordData.AGENT);
                    string AGENT_NAME = clean_string(addRecordData.AGENT_NAME);
                    string DISPOSITION = clean_string(addRecordData.DISPOSITION);
                    string CAMPAIGN = clean_string(addRecordData.CAMPAIGN);
                    string ANI = clean_string(addRecordData.ANI);
                    string DNIS = clean_string(addRecordData.DNIS);
                    string TIMESTAMP = clean_string(addRecordData.TIMESTAMP);
                    string TALK_TIME = clean_string(addRecordData.TALK_TIME);
                    string CALL_TIME = clean_string(addRecordData.CALL_TIME);
                    string HANDLE_TIME = clean_string(addRecordData.HANDLE_TIME);
                    string CALL_TYPE = clean_string(addRecordData.CALL_TYPE);
                    string LIST_NAME = clean_string(addRecordData.LIST_NAME);
                    string leadid = clean_string(addRecordData.leadid);
                    string AGENT_GROUP = clean_string(addRecordData.AGENT_GROUP);
                    string HOLD_TIME = clean_string(addRecordData.HOLD_TIME);
                    string Email = clean_string(addRecordData.Email);
                    string City = clean_string(addRecordData.City);
                    string State = clean_string(addRecordData.State);
                    string Zip = clean_string(addRecordData.Zip);
                    string Datacapturekey = clean_string(addRecordData.Datacapturekey);
                    string Datacapture = clean_string(addRecordData.Datacapture);
                    string Status = clean_string(addRecordData.Status);
                    string Program = clean_string(addRecordData.Program);
                    string Datacapture_Status = clean_string(addRecordData.Datacapture_Status);
                    string num_of_schools = clean_string(addRecordData.num_of_schools);
                    string EducationLevel = clean_string(addRecordData.EducationLevel);
                    string HighSchoolGradYear = clean_string(addRecordData.HighSchoolGradYear);
                    string DegreeStartTimeframe = clean_string(addRecordData.DegreeStartTimeframe);
                    string appname = clean_string(addRecordData.appname);
                    string First_Name = clean_string(addRecordData.First_Name);
                    string Last_Name = clean_string(addRecordData.Last_Name);
                    string address = clean_string(addRecordData.address);
                    string phone = clean_string(addRecordData.phone);
                    string audio_link = clean_string(addRecordData.audio_link);
                    string sort_order = clean_string(addRecordData.sort_order);
                    string scorecard = clean_string(addRecordData.scorecard);
                    string call_date = clean_string(addRecordData.call_date);
                    string Citizenship = clean_string(addRecordData.Citizenship);
                    string Military = clean_string(addRecordData.Military);
                    string profile_id = clean_string(addRecordData.profile_id);
                    string website = clean_string(addRecordData.website);
                    SchoolItem[] Schools = addRecordData.Schools;
                    audioFile[] audios = addRecordData.audios;
                    OtherData[] OtherDataItems = addRecordData.OtherDataItems;
                    string Repost = addRecordData.Repost;
                    string adr_xml = "";
                    var stringwriter = new System.IO.StringWriter();
                    var serializer = new XmlSerializer(addRecordData.GetType());
                    serializer.Serialize(stringwriter, addRecordData);
                    adr_xml = stringwriter.ToString();

                    string raw_post = HttpContext.Current.Request.QueryString.ToString();
                    //string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
                    //SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
                    flatPost flatPost = new flatPost()
                    {
                        raw_data = raw_post,
                        ip_address = HttpContext.Current.Request.ServerVariables["remote_addr"],
                    };
                    dataContext.flatPosts.Add(flatPost);
                    int result1 = dataContext.SaveChanges();
                    if (appname != null)
                    {
                        if (appname.ToString().ToLower() == "inside up")
                        {
                            profile_id = leadid;
                            leadid = null;
                        }
                    }
                    if (clean_string(HttpContext.Current.Request.QueryString["appname"]) != appname)
                    {
                        return Messages.InvalidAppname;
                    }
                    DateTime callDate = new DateTime();
                    int scoreCard = 0;
                    if (call_date != "")
                    {
                        callDate = Convert.ToDateTime(call_date);
                    }
                    else
                    {
                        callDate = System.DateTime.Now;
                    }
                    if (scorecard != "")
                    {
                        scoreCard = Convert.ToInt32(scorecard);
                    }
                    var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                    DateTime Date = dateQuery.AsEnumerable().First();
                    double DataCapturekey;
                    if (Datacapturekey != "" & Information.IsNumeric(Datacapturekey))
                    {
                        DataCapturekey = Convert.ToDouble(Datacapturekey);
                    }
                    else
                    {
                        DataCapturekey = 0.0;
                    }
                    double DataCapture;
                    if (Datacapture != "")
                    {
                        DataCapture = Convert.ToDouble(Datacapture);
                    }
                    else
                    {
                        DataCapture = 0.0;
                    }
                    int sortOrder = 0;
                    if (sort_order != "")
                    {
                        sortOrder = Convert.ToInt32(sort_order);
                    }
                    else
                    {
                        sortOrder = 0;
                    }
                    int new_id = 0;
                    xcc_report_new_pending report_new_pending = new xcc_report_new_pending()
                    {
                        SESSION_ID = string.IsNullOrEmpty(SESSION_ID) ? "NULL" : SESSION_ID,
                        call_date = callDate,
                        Citizenship = (string.IsNullOrEmpty(Citizenship) ? "NULL" : Citizenship),
                        Military = (string.IsNullOrEmpty(Military) ? "NULL" : Military),
                        scorecard = scoreCard,
                        AGENT = (string.IsNullOrEmpty(AGENT) ? "NULL" : AGENT),
                        agent_name = (string.IsNullOrEmpty(AGENT_NAME) ? "NULL" : AGENT_NAME),
                        website = (string.IsNullOrEmpty(website) ? "NULL" : website),
                        DISPOSITION = (string.IsNullOrEmpty(DISPOSITION) ? "NULL" : DISPOSITION),
                        CAMPAIGN = (string.IsNullOrEmpty(CAMPAIGN) ? "NULL" : CAMPAIGN),
                        ANI = (string.IsNullOrEmpty(ANI) ? "NULL" : ANI),
                        DNIS = (string.IsNullOrEmpty(DNIS) ? "NULL" : DNIS),
                        TIMESTAMP = (string.IsNullOrEmpty(TIMESTAMP) ? "NULL" : TIMESTAMP),
                        TALK_TIME = (string.IsNullOrEmpty(TALK_TIME) ? "NULL" : TALK_TIME),
                        CALL_TIME = (string.IsNullOrEmpty(CALL_TIME) ? "NULL" : CALL_TIME),
                        HANDLE_TIME = (string.IsNullOrEmpty(HANDLE_TIME) ? "NULL" : HANDLE_TIME),
                        CALL_TYPE = (string.IsNullOrEmpty(CALL_TYPE) ? "NULL" : CALL_TYPE),
                        LIST_NAME = (string.IsNullOrEmpty(LIST_NAME) ? "NULL" : LIST_NAME),
                        leadid = (string.IsNullOrEmpty(leadid) ? "NULL" : leadid),
                        AGENT_GROUP = (string.IsNullOrEmpty(AGENT_GROUP) ? "NULL" : AGENT_GROUP),
                        hold_time = (string.IsNullOrEmpty(HOLD_TIME) ? "NULL" : HOLD_TIME),
                        DATE = Date,
                        Email = (string.IsNullOrEmpty(Email) ? "NULL" : Email),
                        City = (string.IsNullOrEmpty(City) ? "NULL" : City),
                        State = (string.IsNullOrEmpty(State) ? "NULL" : State),
                        Zip = (string.IsNullOrEmpty(Zip) ? "NULL" : Zip),
                        Datacapturekey = DataCapturekey,
                        Datacapture = DataCapture,
                        Status = (string.IsNullOrEmpty(Status) ? "NULL" : Status),
                        Program = (string.IsNullOrEmpty(Program) ? "NULL" : Program),
                        Datacapture_Status = (string.IsNullOrEmpty(Datacapture_Status) ? "NULL" : Datacapture_Status),
                        num_of_schools = (string.IsNullOrEmpty(num_of_schools) ? "NULL" : num_of_schools),
                        EducationLevel = (string.IsNullOrEmpty(EducationLevel) ? "NULL" : EducationLevel),
                        HighSchoolGradYear = (string.IsNullOrEmpty(HighSchoolGradYear) ? "NULL" : HighSchoolGradYear),
                        DegreeStartTimeframe = (string.IsNullOrEmpty(DegreeStartTimeframe) ? "NULL" : DegreeStartTimeframe),
                        appname = (string.IsNullOrEmpty(appname) ? "NULL" : appname),
                        First_Name = (string.IsNullOrEmpty(First_Name) ? "NULL" : First_Name),
                        Last_Name = (string.IsNullOrEmpty(Last_Name) ? "NULL" : Last_Name),
                        address = (string.IsNullOrEmpty(address) ? "NULL" : address),
                        phone = (string.IsNullOrEmpty(phone) ? "NULL" : phone),
                        audio_link = (string.IsNullOrEmpty(audio_link) ? "NULL" : audio_link),
                        profile_id = (string.IsNullOrEmpty(profile_id) ? "NULL" : profile_id),
                        sort_order = sortOrder
                    };
                    dataContext.xcc_report_new_pending.Add(report_new_pending);
                    result = dataContext.SaveChanges();
                    new_id = report_new_pending.ID;
                    if (result == 1)
                    {
                        Message = Messages.Insert;
                    }

                    //sql = "declare @xcc_id int; insert into xcc_report_new_pending(";
                    //sql = Strings.Left(sql, Strings.Len(sql) - 1) + ")values(" + Strings.Left(@params, Strings.Len(@params) - 1) + "); select @xcc_id = @@identity; select @xcc_id;";
                    //reply.CommandText = sql;

                    if (Schools != null)
                    {
                        foreach (SchoolItem si in Schools)
                        {
                            school_data schoolData = new school_data()
                            {
                                AOI1 = object.Equals(si.AOI1, null) ? "0" : si.AOI1.Replace("+", " ").Replace("%20", " "),
                                AOI2 = object.Equals(si.AOI2, null) ? "0" : si.AOI2.Replace("+", " ").Replace("%20", " "),
                                College = object.Equals(si.College, null) ? "0" : si.College.Replace("+", " ").Replace("%20", " "),
                                DegreeOfInterest = object.Equals(si.DegreeOfInterest, null) ? "0" : si.DegreeOfInterest.Replace("+", " ").Replace("%20", " "),
                                L1_SubjectName = object.Equals(si.L1_SubjectName, null) ? "0" : si.L1_SubjectName.Replace("+", " ").Replace("%20", " "),
                                Modality = object.Equals(si.Modality, null) ? "0" : si.Modality.Replace("+", " ").Replace("%20", " "),
                                School = object.Equals(si.School, null) ? "0" : si.School.Replace("+", " ").Replace("%20", " "),
                                //Portal = object.Equals(si.Portal, null) ? "0" : si.Portal,
                                TCPA = object.Equals(si.TCPA, null) ? "0" : si.TCPA.Replace("+", " ").Replace("%20", " "),
                            };
                            dataContext.school_data.Add(schoolData);
                            int resultschoolData = dataContext.SaveChanges();
                            if (resultschoolData == 1)
                            {
                                Message = Messages.Insert;
                            }
                            //string sql_school = "insert into school_data(pending_id,";
                            //string param_scure = " @pending_id,";
                            // SqlCommand reply_school = new SqlCommand(sql_school, cn);
                            //reply_school.Parameters.AddWithValue("pending_id", new_id);
                        }
                    }

                    if (audios != null)
                    {
                        foreach (audioFile af in audios)
                        {
                            string order = object.Equals(af.order, null) ? "0" : af.order;
                            string file_date = object.Equals(af.file_date, null) ? DateTime.Now.ToShortDateString() : af.file_date;
                            string audio_file = af.audio_file.ToString();
                            if (appname == "inside up" & audio_file != "")
                            {
                                audio_file = af.audio_file.Replace("%3A", ":").Replace("%2F", "/").Replace("+", "%20");
                            }
                            // Get them in order and concatenate the audio.
                            //Common.UpdateTable("insert into audioData(file_name, file_date, file_order, pending_id) select '" + audio_file.ToString().Replace("'", "''") + "','" + file_date.ToString().Replace("nn", "00") + "','" + order.ToString() + "'," + new_id);
                            AudioData audioData = new AudioData()
                            {
                                file_name = audio_file.ToString().Replace("'", "''"),
                                file_date = file_date.ToString().Replace("nn", "00"),
                                file_order = Convert.ToInt32(order),
                                pending_id = new_id
                            };
                            dataContext.AudioDatas.Add(audioData);
                            int resultAudioDatas = dataContext.SaveChanges();
                            if (resultAudioDatas == 1)
                            {
                                Message = Messages.Insert;
                            }
                        }
                    }
                    if (OtherDataItems != null)
                    {
                        foreach (OtherData od in OtherDataItems)
                        {
                            // Get them in order and concatenate the audio.
                            string type = object.Equals(od.type, null) ? "0" : od.type;
                            otherFormDataPending formDataPending = new otherFormDataPending()
                            {
                                form_id = new_id,
                                data_key = od.key.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " "),
                                data_value = od.value.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " "),
                                data_type = type.ToString().Replace("+", " ").Replace("%20", " "),
                                school_name = od.school.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " ")
                            };
                            dataContext.otherFormDataPendings.Add(formDataPending);
                            int resultAudioDatas = dataContext.SaveChanges();
                            if (resultAudioDatas == 1)
                            {
                                Message = Messages.Insert;
                            }
                            //Common.UpdateTable("insert into otherFormDataPending(form_id, data_key, data_value, data_type, school_name, label) select " + new_id + ",'" + od.key.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " ") + "','" + od.value.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " ") + "','" + type.ToString().Replace("+", " ").Replace("%20", " ") + "','" + od.school.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " ") + "','" + od.label.ToString().Replace("'", "''").Replace("+", " ").Replace("%20", " ") + "'");
                        }
                    }
                    return Message;
                }
            }
            catch (Exception ex)
            {
                return Message = Messages.Fail; ;
            }
        }
        #endregion  AddRecord


        #region Public RunFFMPEG
        /// <summary>
        /// RunFFMPEG
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="OutResult"></param>
        /// <param name="ErrResult"></param>
        /// <returns></returns>
        public static int RunFFMPEG(string arg, ref string OutResult, ref string ErrResult)
        {
            Process myProcess = new Process();
            {
                var withBlock = myProcess.StartInfo;
                withBlock.FileName = Common.FFMPEG;
                withBlock.Arguments = arg;
                // start the process in a hidden window
                withBlock.WindowStyle = ProcessWindowStyle.Normal;
                withBlock.CreateNoWindow = false;
                withBlock.RedirectStandardOutput = false;
                withBlock.RedirectStandardError = false;
                withBlock.UseShellExecute = false;
            }
            string Result = "";
            try
            {
                myProcess.Start();
                Result = myProcess.StandardError.ReadToEnd();
                myProcess.WaitForExit();
                myProcess.Close();
            }
            catch (Exception ex)
            {
                Result = string.Empty;
            }
            int num_tries = 0;
            do
            {
                if (!myProcess.HasExited)
                {
                    // Refresh the current process property values.
                    myProcess.Refresh();
                    num_tries += 1;
                }
            }
            while (!myProcess.WaitForExit(600000) & num_tries < 20);
            return 0;
        }
        #endregion RunFFMPEG


        /// <summary>
        /// AddExistingSchool
        /// </summary>
        /// <param name="Schools"></param>
        /// <param name="SESSION_ID"></param>
        /// <returns></returns>
        public string AddExistingSchool(SchoolItem[] Schools, string SESSION_ID)
        {
            try
            {
                int result = 0;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //SchoolItem[] Schools1 ={ new SchoolItem { School = "113", AOI1 = "uiu" } }; 
                    if (Schools != null && Schools.Count() > 0)
                    {
                        foreach (var si in Schools)
                        {
                            string appname = HttpContext.Current.Request["appname"].ToString();
                            var xcc_report_new_pending = dataContext.xcc_report_new_pending.Where(x => x.SESSION_ID == SESSION_ID && x.appname == appname).FirstOrDefault();//  appname=HttpContext.Current.Request["appname"]
                            school_data school_Data = new school_data()
                            {
                                pending_id = Convert.ToInt32(xcc_report_new_pending.SESSION_ID),
                                AOI1 = (string.IsNullOrEmpty(si.AOI1) ? "NULL" : si.AOI1),
                                AOI2 = (string.IsNullOrEmpty(si.AOI2) ? "NULL" : si.AOI2),
                                College = (string.IsNullOrEmpty(si.College) ? "NULL" : si.College),
                                DegreeOfInterest = (string.IsNullOrEmpty(si.DegreeOfInterest) ? "NULL" : si.DegreeOfInterest),
                                L1_SubjectName = (string.IsNullOrEmpty(si.L1_SubjectName) ? "NULL" : si.L1_SubjectName),
                                L2_SubjectName = (string.IsNullOrEmpty(si.L2_SubjectName) ? "NULL" : si.L2_SubjectName),
                                Modality = (string.IsNullOrEmpty(si.Modality) ? "NULL" : si.Modality),
                                School = (string.IsNullOrEmpty(si.School) ? "NULL" : si.School),
                                TCPA = (string.IsNullOrEmpty(si.TCPA) ? "NULL" : si.TCPA)
                            };
                            dataContext.school_data.Add(school_Data);
                            result = dataContext.SaveChanges();
                        }
                        if (result == 1)
                        {
                            return Messages.Insert;
                        }
                        else
                        {
                            return Messages.Fail;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Messages.Fail + ex.Message + " ";
            }
            return Messages.Insert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questions"></param>
        public string UpdateQuestionsOrder(List<ScorecardQuestion> questions)
        {

            try
            {
                int result = 0;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    foreach (var item in questions)
                    {
                        int Id = Convert.ToInt32(item.id.Trim());
                        //Common.UpdateTable("update questions set q_order = " + q.order + " where id = " + q.id);
                        var isExist = dataContext.Questions.Where(x => x.id == Id).FirstOrDefault();
                        Entities.Question tblQuestion = new Entities.Question();
                        if (isExist != null)
                        {
                            tblQuestion = dataContext.Questions.Find(Id);
                            dataContext.Entry(tblQuestion).State = EntityState.Modified;
                            tblQuestion.q_order = Convert.ToInt32(item.order);
                            result = dataContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Messages.Updated + questions.Count.ToString();
        }

        #region Public UpdateFaqsOrder
        /// <summary>
        /// UpdateFaqsOrder
        /// </summary>
        /// <param name="faqs"></param>
        /// <returns></returns>
        public string UpdateFaqsOrder(List<QuestionFaq> faqs)
        {
            try
            {
                int result = 0;
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    foreach (QuestionFaq item in faqs)
                    {
                        int Id = 0;
                        if (item.id.Trim() != "")
                        {
                            Id = Convert.ToInt32(item.id.Trim());
                        }
                        //Common.UpdateTable("update q_faqs set q_order = " + f.order + " where id = " + f.id);
                        var isExist = dataContext.q_faqs.Where(x => x.id == Id).FirstOrDefault();
                        q_faqs tblqfaqs = new q_faqs();
                        if (isExist != null)
                        {
                            tblqfaqs = dataContext.q_faqs.Find(Id);
                            dataContext.Entry(tblqfaqs).State = EntityState.Modified;
                            tblqfaqs.q_order = Convert.ToInt32(item.order);
                            result = dataContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Messages.Fail + ex.Message;
            }
            return Messages.Updated + faqs.Count.ToString();
        }
        #endregion Public UpdateFaqsOrder

        #region Public UpdateInstructionsOrder
        /// <summary>
        /// UpdateInstructionsOrder
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public string UpdateInstructionsOrder(List<InstructionQuestion> instructions)
        {
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    foreach (InstructionQuestion item in instructions)
                    {
                        //Common.UpdateTable("update q_instructions set q_order = " + item.order + " where id = " + item.id);
                        //str += "update q_instructions set q_order = " + item.order + " where id = " + item.id;
                        int Id = 0;
                        if (item.id.Trim() != "")
                        {
                            Id = Convert.ToInt32(item.id.Trim());
                        }
                        var isExist = dataContext.q_instructions.Where(x => x.id == Id).FirstOrDefault();
                        q_instructions tblq_instructions = new q_instructions();
                        if (isExist != null)
                        {
                            tblq_instructions = dataContext.q_instructions.Find(Id);
                            dataContext.Entry(tblq_instructions).State = EntityState.Modified;
                            tblq_instructions.q_order = Convert.ToInt32(item.order);
                            int result = dataContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Messages.Fail + ex.Message;
            }
            return Messages.Updated + instructions.Count.ToString();
        }
        #endregion UpdateInstructionsOrder

        #region Public PostCalibration
        /// <summary>
        /// PostCalibration
        /// </summary>
        /// <param name="listenDataRequest"></param>
        /// <returns></returns>
        public string PostCalibration(ListenDataRequest listenDataRequest)
        {

            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    ListenDataPost LD = listenDataRequest.LD;
                    List<FormQScores> FQS = listenDataRequest.FQS;
                    List<FormQResponses> FQR = listenDataRequest.FQR;
                    List<FormQScoresOptions> FQSO = listenDataRequest.FQSO;
                    List<ClerkedData> CD = listenDataRequest.CD;
                    List<SystemComments> SC = listenDataRequest.SC;
                    List<CommentData> Comments = listenDataRequest.Comments;
                    DataTable listen_dt = new DataTable();
                    listen_dt.Columns.Add("reviewer", Type.GetType("System.String"));
                    listen_dt.Columns.Add("session_id", Type.GetType("System.String"));
                    listen_dt.Columns.Add("review_ID", Type.GetType("System.Int32"));
                    listen_dt.Columns.Add("Comments", Type.GetType("System.String"));
                    listen_dt.Columns.Add("appname", Type.GetType("System.String"));
                    listen_dt.Columns.Add("whisperID", Type.GetType("System.Int32"));
                    listen_dt.Columns.Add("QAwhisper", Type.GetType("System.Int32"));
                    listen_dt.Columns.Add("qa_start", Type.GetType("System.Int32"));
                    listen_dt.Columns.Add("qa_last_action", Type.GetType("System.Int32"));
                    listen_dt.Columns.Add("call_length", Type.GetType("System.Single"));
                    listen_dt.Columns.Add("copy_to_cali", Type.GetType("System.Int32"));

                    DataRow listen_dr = listen_dt.NewRow();
                    listen_dr["reviewer"] = HttpContext.Current.User.Identity.Name;
                    listen_dr["session_id"] = LD.session_id;
                    listen_dr["review_ID"] = LD.review_ID;

                    if (Comments.Count > 0)
                    {
                        listen_dr["Comments"] = "";
                        foreach (CommentData sc_comment in Comments)
                            listen_dr["Comments"] = listen_dr["Comments"] + sc_comment.comment;
                    }
                    else
                    {
                        listen_dr["Comments"] = LD.Comments;
                    }
                    listen_dr["appname"] = LD.appname;
                    listen_dr["whisperID"] = Interaction.IIf(Information.IsNumeric(LD.whisperID), LD.whisperID, 0);
                    listen_dr["QAwhisper"] = Interaction.IIf(Information.IsNumeric(LD.QAwhisper), LD.QAwhisper, 0);
                    listen_dr["qa_start"] = LD.qa_start;
                    listen_dr["qa_last_action"] = LD.qa_last_action;
                    listen_dr["call_length"] = LD.call_length;
                    listen_dr["copy_to_cali"] = LD.copy_to_cali;
                    listen_dt.Rows.Add(listen_dr);

                    DataTable FQS_dt = new DataTable();
                    FQS_dt.Columns.Add("q_position", Type.GetType("System.String"));
                    FQS_dt.Columns.Add("question_id", Type.GetType("System.Int32"));
                    FQS_dt.Columns.Add("question_result", Type.GetType("System.Int32"));
                    FQS_dt.Columns.Add("question_answered", Type.GetType("System.String"));
                    FQS_dt.Columns.Add("click_text", Type.GetType("System.String"));
                    FQS_dt.Columns.Add("other_answer_text", Type.GetType("System.String"));
                    FQS_dt.Columns.Add("view_link", Type.GetType("System.String"));

                    foreach (var fqs_item in FQS)
                    {
                        DataRow FQS_dr = FQS_dt.NewRow();
                        FQS_dr["q_position"] = fqs_item.q_position;
                        FQS_dr["question_id"] = fqs_item.question_id;
                        FQS_dr["question_result"] = 0;
                        FQS_dr["question_answered"] = fqs_item.question_answered;
                        FQS_dr["click_text"] = fqs_item.click_text;
                        FQS_dr["view_link"] = fqs_item.view_link;
                        FQS_dt.Rows.Add(FQS_dr);
                    }

                    DataTable FQR_dt = new DataTable();
                    FQR_dt.Columns.Add("question_id", Type.GetType("System.String"));
                    FQR_dt.Columns.Add("answer_id", Type.GetType("System.Int32"));
                    FQR_dt.Columns.Add("other_answer_text", Type.GetType("System.String"));

                    foreach (var fqr_item in FQR)
                    {
                        DataRow FQR_dr = FQR_dt.NewRow();
                        FQR_dr["question_id"] = fqr_item.question_id;
                        FQR_dr["answer_id"] = fqr_item.answer_id;
                        FQR_dr["other_answer_text"] = fqr_item.other_answer_text;

                        FQR_dt.Rows.Add(FQR_dr);
                    }
                    DataTable FQSO_dt = new DataTable();
                    FQSO_dt.Columns.Add("option_pos", Type.GetType("System.Int32"));
                    FQSO_dt.Columns.Add("option_value", Type.GetType("System.String"));
                    FQSO_dt.Columns.Add("question_id", Type.GetType("System.Int32"));
                    FQSO_dt.Columns.Add("orig_id", Type.GetType("System.Int32"));

                    foreach (var fqso_item in FQSO)
                    {
                        DataRow FQSO_dr = FQSO_dt.NewRow();
                        FQSO_dr["option_pos"] = fqso_item.option_pos;
                        FQSO_dr["option_value"] = fqso_item.option_value;
                        FQSO_dr["question_id"] = fqso_item.question_id;
                        FQSO_dr["orig_id"] = fqso_item.orig_id;
                        FQSO_dt.Rows.Add(FQSO_dr);
                    }

                    using (var command = new SqlCommand("calibDataInsert"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 90;
                        // create your own data table
                        command.Parameters.Add(new SqlParameter("@ListenInsert", listen_dt));
                        command.Parameters.Add(new SqlParameter("@FQSInsert", FQS_dt));
                        command.Parameters.Add(new SqlParameter("@FQRInsert", FQR_dt));
                        command.Parameters.Add(new SqlParameter("@FQSOInsert", FQSO_dt));
                        Common.RunSqlCommand(command, true);
                    }
                    var isvwCF = dataContext.vwCFs.OrderByDescending(p => p.id).ToList();
                    //DataTable dt = GetTable("select top 1 f_id from vwCF order by id desc");
                    var fId = isvwCF.Take(1).FirstOrDefault();
                    return fId.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion PostCalibration


        #region Public GetNextTraining
        /// <summary>
        /// GetNextTraining
        /// </summary>
        /// <param name="nextTrainingRequest"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public TrainingCall GetNextTraining(NextTrainingRequest nextTrainingRequest, string username)
        {
            TrainingCall objTrainingCall = new TrainingCall();
            //string listen_sql = "exec getMyNextTrainingCall '" + username + "'";
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var getMyNextTrainingCall = dataContext.getMyNextTrainingCall(username).ToList();

                //DataTable dt2 = GetTable(listen_sql);
                var firstRows = getMyNextTrainingCall.Take(1).FirstOrDefault();
                if (getMyNextTrainingCall.Count == 0)
                {
                    return objTrainingCall;
                }
                int cali_count;
                if (Information.IsDBNull(firstRows.training_count))
                {
                    cali_count = 0;
                }
                else
                {
                    cali_count = Convert.ToInt32(firstRows.training_count);
                }
                int scoreCard1 = Convert.ToInt32(firstRows.scorecard);
                //DataTable checkQATraining = GetTable("exec checkQATraining '" + username + "'," + dt2.Rows[0]["scorecard"] + "," + cali_count);
                var checkQATraining = dataContext.checkQATraining(username, scoreCard1, cali_count).FirstOrDefault();

                int pass_score = Convert.ToInt32(firstRows.pass_percent);
                if (HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Calibrator") | HttpContext.Current.User.IsInRole("Recalibrator"))
                {
                    pass_score = Convert.ToInt32(firstRows.pass_percent) + 5;// 90
                }
                if ((((int)checkQATraining.trainee_avg_score >= pass_score) & ((int)checkQATraining.number_scores >= cali_count)))
                {
                    int scoreCard = Convert.ToInt32(firstRows.scorecard);
                    var isExist = dataContext.UserApps.Where(x => x.user_scorecard == scoreCard && x.username == username).FirstOrDefault();
                    //Common.UpdateTable("update userapps set scorecard_role = 'QA' where user_scorecard = '" + dt2.Rows[0]["scorecard"] + "' and username = '" + username + "'");
                    UserApp tblUserApps = new UserApp();
                    if (isExist != null)
                    {
                        int Id = isExist.id;
                        tblUserApps = dataContext.UserApps.Find(Id);
                        dataContext.Entry(tblUserApps).State = EntityState.Modified;
                        tblUserApps.scorecard_role = "QA";
                        int result = dataContext.SaveChanges();
                    }

                    string Body = username + " has passed in " + firstRows.short_name + " with an avg score of " + checkQATraining.trainee_avg_score + Strings.Chr(13) + Strings.Chr(13) + "<br>";
                    //DataTable scores_dt = GetTable("select * from vwTrain where reviewer = '" + username + "' and scorecard = '" + dt2.Rows[0]["scorecard"] + "' order by id desc");
                    var scores_dt = dataContext.vwTrains.Where(x => x.reviewer == username && x.scorecard == scoreCard).OrderByDescending(p => p.id).ToList();
                    foreach (var score_dr in scores_dt)
                    {
                        ObjectParameter ReturnedValue = new ObjectParameter("ReturnValue", typeof(int));
                        Body += "http://app.callcriteria.com/review_training.aspx?ID=" + score_dr.id + " - score:" + score_dr.trainee_score + "<br>";
                        var send_dbmail = dataContext.send_dbmail("General", "ryan@callcriteria.com; stace @callcriteria.com; brian @callcriteria.com; chad @callcriteria.com", "", "", "Trainee Passed " + firstRows.short_name, Body.ToString(), "HTML", "", "", "", "", "", false, "", false, 256, "", false, false, false, false, ReturnedValue, "", "").ToString();
                        //SqlCommand reply2 = new SqlCommand("EXEC send_dbmail  @profile_name='General',   @recipients='ryan@callcriteria.com;stace@callcriteria.com;brian@callcriteria.com;chad@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn);
                        //reply2.Parameters.AddWithValue("Subject_text", "Trainee Passed " + dt2.Rows[0]["short_name"]);
                        //reply2.Parameters.AddWithValue("Body", Body.ToString());
                    }
                    //DataTable sc_dt = GetTable("select id from sc_training_approvals where username = '" + username + "' and sc_id = " + dt2.Rows[0]["scorecard"].ToString());
                    var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                    DateTime mtdate = dateQuery.AsEnumerable().First();
                    var sc_dt = dataContext.sc_training_approvals.Where(x => x.username == username && x.sc_id == scoreCard).FirstOrDefault();
                    if (sc_dt != null)
                    {
                        //Common.UpdateTable("insert into sc_training_approvals(username, sc_id, sc_date, sc_by, retrain_date) select '" + username + "'," + dt2.Rows[0]["scorecard"].ToString() + ",dbo.getMTDate(), 'Passed', dateadd(d, 14, dbo.getMTDate())");
                        sc_training_approvals scTrainingApprovals = new sc_training_approvals()
                        {
                            username = username,
                            sc_id = Convert.ToInt32(firstRows.scorecard),
                            sc_date = mtdate,
                            sc_by = "Passed",
                            retrain_date = mtdate.AddDays(14)
                        };
                        dataContext.sc_training_approvals.Add(scTrainingApprovals);
                        int resultsc_training_approvals = dataContext.SaveChanges();
                    }
                    //return objTrainingCall;
                }

                int review_id = 0;
                string f_id = "";
                string reviewer = "";
                string scorecard = "";
                int scorecard_id = 1;
                foreach (var dr in getMyNextTrainingCall)
                {
                    ListenData ld = new ListenData();
                    ld.address = dr.address;
                    ld.ANI = dr.ANI;
                    ld.appname = dr.appname;
                    //ld.audio_link = Common.GetAudioFileName(dr);
                    ld.call_date = dr.call_date.ToString();
                    ld.CALL_TIME = dr.call_time.ToString();
                    ld.CALL_TYPE = dr.call_type;
                    ld.CAMPAIGN = dr.campaign;
                    ld.City = dr.city;
                    ld.client_logo = dr.client_logo;
                    ld.DegreeStartTimeframe = dr.DegreeStartTimeframe;
                    ld.DISPOSITION = dr.Disposition;
                    ld.DNIS = dr.DNIS;
                    ld.EducationLevel = dr.EducationLevel;
                    ld.Email = dr.Email;
                    ld.First_Name = dr.First_Name;
                    ld.HighSchoolGradYear = dr.HighSchoolGradYear;
                    ld.Last_Name = dr.Last_Name;
                    ld.leadid = dr.review_id.ToString();
                    ld.phone = dr.phone;
                    ld.profile_id = dr.profile_id;
                    ld.scorecard = dr.scorecard.ToString();
                    ld.scorecard_name = dr.short_name;
                    ld.SESSION_ID = dr.session_id.ToString();
                    ld.State = dr.state;
                    ld.website = dr.website;
                    ld.X_ID = dr.calib_id.ToString();
                    ld.Zip = dr.zip;
                    ld.agent = dr.agent;
                    scorecard_id = Convert.ToInt32(dr.scorecard);
                    review_id = Convert.ToInt32(dr.review_id);
                    f_id = dr.f_id.ToString();
                    reviewer = username;
                    scorecard = dr.scorecard.ToString();
                    //SqlCommand reply = new SqlCommand("select user_role from userextrainfo where username = @username", cn);
                    var userextrain = dataContext.UserExtraInfoes.Where(x => x.username == username).ToList();
                    var schoolData = dataContext.School_X_Data.Where(x => x.xcc_id == review_id).ToList();
                    //reply = new SqlCommand("select * from school_x_data where xcc_id = @xcc_id ", cn);
                    List<SchoolItem> school_items = new List<SchoolItem>();
                    foreach (var school_dr in schoolData)
                    {
                        SchoolItem school_item = new SchoolItem();
                        school_item.AOI1 = school_dr.AOI1;
                        school_item.AOI2 = school_dr.AOI2;
                        school_item.College = school_dr.College;
                        school_item.DegreeOfInterest = school_dr.DegreeOfInterest;
                        school_item.L1_SubjectName = school_dr.L1_SubjectName;
                        school_item.L2_SubjectName = school_dr.L2_SubjectName;
                        school_item.Modality = school_dr.Modality;
                        school_item.Portal = school_dr.origin;
                        school_item.School = school_dr.School;
                        school_item.TCPA = school_dr.tcpa;
                        school_item.id = school_dr.id.ToString();
                        school_item.id = school_dr.id.ToString();
                        school_items.Add(school_item);
                    }
                    ld.SchoolData = school_items;
                    if (review_id != 0)
                    {
                        var getotherformdata = dataContext.getotherformdata(review_id).ToList();
                        //reply = new SqlCommand("exec getotherformdata @xcc_id", cn);
                        List<OtherData> otherdata_items = new List<OtherData>();
                        foreach (var school_dr in getotherformdata)
                        {
                            OtherData otherdata_item = new OtherData();
                            otherdata_item.key = school_dr.data_key;
                            otherdata_item.label = school_dr.label;
                            otherdata_item.school = school_dr.school_name;
                            otherdata_item.type = school_dr.data_type;
                            otherdata_item.value = school_dr.data_value;
                            otherdata_item.id = school_dr.xcc_id.ToString();
                            otherdata_items.Add(otherdata_item);
                        }
                        ld.OtherData = otherdata_items;
                        objTrainingCall.LC.ListenData = ld;
                    }
                    //string sql_data = " and (is_practice = 0 or is_practice IS NULL) ";
                    if (nextTrainingRequest != null)
                    {

                        if (nextTrainingRequest.is_practice)
                        {
                            int scoreCard = Convert.ToInt32(scorecard);
                            //sql_data = " and is_practice = 1 ";
                            var vwTrain1 = dataContext.vwTrains.Where(x => x.reviewer == reviewer && x.scorecard == scoreCard && x.is_practice == true).OrderByDescending(p => p.review_date).ToList();
                            List<training_item> training_items1 = new List<training_item>();
                            foreach (var school_dr in vwTrain1)
                            {
                                training_item ti = new training_item();
                                ti.id = school_dr.id.ToString();
                                ti.score = school_dr.trainee_score.ToString();
                                ti.score_date = school_dr.review_date.ToString();
                                training_items1.Add(ti);
                            }
                            objTrainingCall.training_items = training_items1;
                        }
                    }
                    else
                    {
                        int scoreCard = Convert.ToInt32(scorecard);
                        //reply = new SqlCommand("Select id, trainee_score, review_date from vwtrain where reviewer = '" + reviewer + "' and scorecard = '" + scorecard + "'" + sql_data + " order by review_date desc", cn);
                        var vwTrain = dataContext.vwTrains.Where(x => x.reviewer == reviewer && x.scorecard == scoreCard && x.is_practice == false && x.is_practice == null).OrderByDescending(p => p.review_date).ToList();
                        List<training_item> training_items = new List<training_item>();
                        foreach (var school_dr in vwTrain)
                        {
                            training_item ti = new training_item();
                            ti.id = school_dr.id.ToString();
                            ti.score = school_dr.trainee_score.ToString();
                            ti.score_date = school_dr.review_date.ToString();
                            training_items.Add(ti);
                        }
                        objTrainingCall.training_items = training_items;
                    }
                    string scorecardId = scorecard_id.ToString();
                    string xcc_id = review_id.ToString();
                    objTrainingCall.LC.Scorecard = GetScorecardRecordListen(scorecardId, xcc_id, "");
                }
            }
            return objTrainingCall;
        }

        #endregion GetNextTraining

        #region GetNextCalibration
        /// <summary>
        /// GetNextCalibration
        /// </summary>
        /// <returns></returns>
        public ListenCall GetNextCalibration(string username)
        {

            ListenCall objListenCall = new ListenCall();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                string scoreCardId = "";
                if (HttpContext.Current.Request["scorecard"] != null)
                {
                    scoreCardId = HttpContext.Current.Request["scorecard"].ToString();
                }
                else
                {
                    scoreCardId = "Any";
                }
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                //SqlCommand sq_cali = new SqlCommand("getNextCalibration");
                var getNextCalibration = dataContext.getNextCalibration(username, scoreCardId).ToList();
                int scorecard_id = 1;
                foreach (var dr in getNextCalibration)
                {
                    if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("Manager") | HttpContext.Current.User.IsInRole("Client Calibrator"))
                    {
                        var isExist = dataContext.cali_pending_client.Where(x => x.id == dr.id).FirstOrDefault();
                        cali_pending_client tblQuestion = new cali_pending_client();
                        if (isExist != null)
                        {
                            int Id = isExist.id;
                            tblQuestion = dataContext.cali_pending_client.Find(Id);
                            dataContext.Entry(tblQuestion).State = EntityState.Modified;
                            tblQuestion.who_processed = username;
                            tblQuestion.date_started = mtdate;
                            int result = dataContext.SaveChanges();
                        }
                        //Common.UpdateTable("update cali_pending_client set who_processed = '" + username + "', date_started = dbo.getMTDate() where id = " + dr["ID"].ToString());
                    }
                    else
                    {
                        //Common.UpdateTable("update calibration_pending set who_processed = '" + username + "', date_started = dbo.getMTDate() where id = " + dr["ID"].ToString());
                        var isExist = dataContext.calibration_pending.Where(x => x.id == dr.id).FirstOrDefault();
                        calibration_pending tblcalibration_pending = new calibration_pending();
                        if (isExist != null)
                        {
                            int Id = isExist.id;
                            tblcalibration_pending = dataContext.calibration_pending.Find(Id);
                            dataContext.Entry(tblcalibration_pending).State = EntityState.Modified;
                            tblcalibration_pending.who_processed = username;
                            tblcalibration_pending.date_started = mtdate;
                            int result = dataContext.SaveChanges();
                        }
                    }
                    ListenData ld = new ListenData();
                    ld.address = dr.address;
                    ld.ANI = dr.ANI;
                    ld.appname = dr.appname;
                    //ld.audio_link = Common.GetAudioFileName(dr);
                    ld.call_date = dr.call_date.ToString(); ;
                    ld.CALL_TIME = dr.CALL_TIME.ToString();
                    ld.CALL_TYPE = dr.CALL_TYPE;
                    ld.CAMPAIGN = dr.CAMPAIGN;
                    ld.City = dr.City;
                    ld.client_logo = dr.client_logo;
                    ld.DegreeStartTimeframe = dr.DegreeStartTimeframe;
                    ld.DISPOSITION = dr.DISPOSITION;
                    ld.DNIS = dr.DNIS;
                    ld.EducationLevel = dr.EducationLevel;
                    ld.Email = dr.Email;
                    ld.First_Name = dr.First_Name;
                    ld.HighSchoolGradYear = dr.HighSchoolGradYear;
                    ld.Last_Name = dr.Last_Name;
                    ld.leadid = dr.leadid;
                    ld.LIST_NAME = dr.LIST_NAME;
                    ld.phone = dr.phone;
                    ld.profile_id = dr.profile_id;
                    ld.Program = dr.Program;
                    ld.scorecard = dr.scorecard.ToString();
                    ld.scorecard_name = dr.short_name;
                    ld.SESSION_ID = dr.SESSION_ID;
                    ld.State = dr.State;
                    ld.TIMESTAMP = dr.TIMESTAMP;
                    ld.website = dr.website;
                    ld.X_ID = dr.review_ID.ToString();
                    ld.Zip = dr.Zip;
                    ld.isRecal = true;

                    scorecard_id = Convert.ToInt32(dr.scorecard);
                    ld.agent = dr.AGENT;

                    //SqlCommand reply = new SqlCommand("select user_role from userextrainfo where username = @username", cn);
                    var isUser = dataContext.UserExtraInfoes.Where(x => x.username == username).ToList();

                    //reply = new SqlCommand("select * from school_x_data where xcc_id = @xcc_id ", cn);
                    var isschool_x_data = dataContext.School_X_Data.Where(x => x.xcc_id == dr.review_ID).ToList();
                    List<SchoolItem> school_items = new List<SchoolItem>();
                    foreach (var school_dr in isschool_x_data)
                    {
                        SchoolItem school_item = new SchoolItem();
                        school_item.AOI1 = school_dr.AOI1;
                        school_item.AOI2 = school_dr.AOI2;
                        school_item.College = school_dr.College;
                        school_item.DegreeOfInterest = school_dr.DegreeOfInterest;
                        school_item.L1_SubjectName = school_dr.L1_SubjectName;
                        school_item.L2_SubjectName = school_dr.L2_SubjectName;
                        school_item.Modality = school_dr.Modality;
                        school_item.Portal = school_dr.origin;
                        school_item.School = school_dr.School;
                        school_item.TCPA = school_dr.tcpa;
                        school_item.id = school_dr.id.ToString();
                        school_items.Add(school_item);
                    }

                    ld.SchoolData = school_items;
                    var getotherformdata = dataContext.getotherformdata(dr.review_ID).ToList();
                    //reply = new SqlCommand("exec getotherformdata @xcc_id", cn);
                    List<OtherData> otherdata_items = new List<OtherData>();
                    foreach (var school_dr in getotherformdata)
                    {
                        OtherData otherdata_item = new OtherData();
                        otherdata_item.key = school_dr.data_key;
                        otherdata_item.label = school_dr.label;
                        otherdata_item.school = school_dr.school_name;
                        otherdata_item.type = school_dr.data_type;
                        otherdata_item.value = school_dr.data_value;
                        otherdata_item.id = school_dr.id.ToString(); ;
                        otherdata_items.Add(otherdata_item);
                    }

                    ld.OtherData = otherdata_items;
                    objListenCall.ListenData = ld;
                    getSCRecData gd = new getSCRecData();
                    gd.scorecard_ID = scorecard_id.ToString();
                    gd.xcc_id = dr.review_ID.ToString();

                    objListenCall.Scorecard = GetScorecardRecordListen(gd.scorecard_ID, gd.xcc_id, "");
                    //Common.UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" + username + "',dbo.getMTDate(), " + dr["review_ID"].ToString() + ",'calibration listen'");
                    session_viewed objsession_viewed = new session_viewed();
                    // Save form_score3 data
                    objsession_viewed.session_id = dr.review_ID;
                    objsession_viewed.page_viewed = "calibration listen";
                    objsession_viewed.date_viewed = mtdate;
                    objsession_viewed.agent = username;
                    dataContext.session_viewed.Add(objsession_viewed);
                    int result1 = dataContext.SaveChanges();
                }
            }
            return objListenCall;
        }
        #endregion GetNextCalibration

        /// <summary>
        /// UploadFile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ManipulateAudioResult UploadFile(Stream input)
        {

            var Type = HttpContext.Current.Request.Headers["Content-Type"].ToString();
            //var Type = WebOperationContext.Current.IncomingRequest.Headers["Content-Type"];
            // Now we want to strip the boundary out of the Content-Type, currently the string
            // looks Like: "multipart/form-data; boundary=---------------------124123qase124"
            var boundary = Type.Substring(Type.IndexOf("=") + 1);
            MultiformUploadRequest MR = new MultiformUploadRequest(input, boundary);
            Maudio MA = new Maudio();
            MA.xcc_id = MR.choosenFiles.xcc_id;
            MA.audio_url = MR.audio_url;
            return Manipulateaudio(MA);
        }

        #region Public Manipulateaudio
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maudio"></param>
        /// <returns></returns>
        public ManipulateAudioResult Manipulateaudio(Maudio maudio)
        {
            int xcc_id = Convert.ToInt32(maudio.xcc_id);
            ManipulateAudioResult manipulateAudioResult = new ManipulateAudioResult();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable xccReport = GetTable("select appname, call_date from xcc_report_new where id=" + maudio.xcc_id);
                var xccReport = dataContext.AudioDatas.Where(x => x.final_xcc_id == xcc_id).FirstOrDefault();
                if (xccReport != null)
                {
                    int Id = xccReport.id;
                    //Common.UpdateTable("update audiodata set final_xcc_id = final_xcc_id  * -1 where final_xcc_id = " + maudio.xcc_id + ";update xcc_report_new set onAWS = 0 where id = " + MA.xcc_id);
                    var isExist = dataContext.AudioDatas.Where(x => x.id == Id).FirstOrDefault();
                    AudioData tblAudioDatas = new AudioData();
                    if (isExist != null)
                    {
                        tblAudioDatas = dataContext.AudioDatas.Find(Id);
                        dataContext.Entry(tblAudioDatas).State = EntityState.Modified;
                        tblAudioDatas.final_xcc_id = -1 + isExist.final_xcc_id;
                        int result = dataContext.SaveChanges();
                    }
                }
                var xccReport_new = dataContext.XCC_REPORT_NEW.Where(x => x.ID == xcc_id).FirstOrDefault();
                XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                if (xccReport_new != null)
                {
                    tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(xccReport_new.ID);
                    dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                    tblXCC_REPORT_NEW.onAWS = false;
                    int result = dataContext.SaveChanges();
                }

                string try_sql = "";
                int file_count = 1;
                foreach (var audio in maudio.audio_url)
                {
                    var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                    DateTime mtdate = dateQuery.AsEnumerable().First();
                    if (Strings.Left(audio, 4) == "http" | Strings.Left(audio, 4) == "sftp" | Strings.Left(audio, 4) == "ftp:" | Strings.Left(audio, 4) == @"\\64" | Strings.Left(audio, 4) == "//64" | Strings.Left(audio, 3) == @"d:\")
                    {
                        //Common.UpdateTable("insert into audiodata (file_name, file_date, file_order,pending_id, final_xcc_id) select '" + audio + "',convert(date, dbo.getMTDate())," + file_count + "," + MA.xcc_id + "," + MA.xcc_id);
                        AudioData audioData = new AudioData()
                        {
                            file_name = audio,
                            file_date = mtdate.ToString(),
                            file_order = file_count,
                            pending_id = xcc_id,
                            final_xcc_id = xcc_id
                        };
                        dataContext.AudioDatas.Add(audioData);
                        int resultAudioDatas = dataContext.SaveChanges();
                    }
                    else
                    {
                        //Common.UpdateTable("insert into audiodata (file_name, file_date, file_order,pending_id, final_xcc_id) select '" + HttpContext.Current.Server.MapPath("/") + audio + "',convert(date, dbo.getMTDate())," + file_count + "," + MA.xcc_id + "," + MA.xcc_id);
                        AudioData audioData = new AudioData()
                        {
                            file_name = HttpContext.Current.Server.MapPath("/") + audio,
                            file_date = mtdate.ToString(),
                            file_order = file_count,
                            pending_id = xcc_id,
                            final_xcc_id = xcc_id
                        };
                        dataContext.AudioDatas.Add(audioData);
                        int resultAudioDatas = dataContext.SaveChanges();
                    }
                    file_count += 1;
                }

                //DataTable date_dt = GetTable("select xcc_report_new.*, record_format, recording_user, record_password from xcc_report_new join app_settings on app_settings.appname = xcc_report_new.appname where xcc_report_new.id = " + MA.xcc_id);
                var clerksession_viewed = (from XCCREPORT_NEW in dataContext.XCC_REPORT_NEW
                                           join appSettings in dataContext.app_settings on XCCREPORT_NEW.appname equals appSettings.appname
                                           where XCCREPORT_NEW.ID == xcc_id  //19364093
                                           select new XccReportNew
                                           {
                                               ID = XCCREPORT_NEW.ID,
                                               appname = XCCREPORT_NEW.appname,
                                               SESSION_ID = XCCREPORT_NEW.SESSION_ID,
                                               record_format = appSettings.record_format,
                                               recording_user = appSettings.recording_user,
                                               record_password = appSettings.record_password,
                                           }).ToList();
                //lect new { XCCREPORT_NEW, appSettings.record_format, appSettings.recording_user, appSettings.record_password }).ToList();
                if (clerksession_viewed.Count > 0)
                {
                    manipulateAudioResult = assembleItem(clerksession_viewed);

                    if (manipulateAudioResult.IsSuccess & File.Exists(HttpContext.Current.Server.MapPath(manipulateAudioResult.Audio)))
                    {
                        string existingBucketName = "callcriteriasingapore";
                        Amazon.S3.Transfer.TransferUtility fileTransferUtility = new Amazon.S3.Transfer.TransferUtility(new AmazonS3Client("AKIAJSJTF2UX2FU3TIZA", "xZoV9fLa2O2qpwlv2hUJ26XJYXhTak25JI1PceXZ", Amazon.RegionEndpoint.APSoutheast1));
                        string file_source = HttpContext.Current.Server.MapPath(manipulateAudioResult.Audio);
                        string file_dest = existingBucketName + manipulateAudioResult.Audio.Substring(0, manipulateAudioResult.Audio.LastIndexOf("/"));
                        fileTransferUtility.Upload(file_source, file_dest);
                        //Common.UpdateTable("update xcc_report_new set audio_link = '" + manipulateAudioResult.Audio + "', onAWS = 1 where id = " + MA.xcc_id);
                        var xcReport_new = dataContext.XCC_REPORT_NEW.Where(x => x.ID == xcc_id).FirstOrDefault();
                        XCC_REPORT_NEW tblXREPORT_NEW = new XCC_REPORT_NEW();
                        if (xccReport_new != null)
                        {
                            tblXREPORT_NEW = dataContext.XCC_REPORT_NEW.Find(xccReport_new.ID);
                            dataContext.Entry(tblXREPORT_NEW).State = EntityState.Modified;
                            tblXREPORT_NEW.audio_link = manipulateAudioResult.Audio;
                            tblXREPORT_NEW.onAWS = true;
                            int result = dataContext.SaveChanges();
                        }
                        else
                        {
                            //Common.UpdateTable("update xcc_report_new set audio_link = '" + manipulateAudioResult.Audio + "', onAWS = 0 where id = " + MA.xcc_id);
                            if (xccReport_new != null)
                            {
                                tblXREPORT_NEW = dataContext.XCC_REPORT_NEW.Find(xccReport_new.ID);
                                dataContext.Entry(tblXREPORT_NEW).State = EntityState.Modified;
                                tblXREPORT_NEW.audio_link = manipulateAudioResult.Audio;
                                tblXREPORT_NEW.onAWS = false;
                                int result = dataContext.SaveChanges();
                            }
                        }
                    }
                }
                return manipulateAudioResult;
            }

        }
        #endregion Manipulateaudio

        #region Public UpdateClerkData
        /// <summary>
        /// UpdateClerkData
        /// </summary>
        /// <param name="updateClerkedData"></param>
        /// <returns></returns>
        public string UpdateClerkData(List<UpdateClerkedData> updateClerkedData)
        {
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                foreach (var UCD in updateClerkedData)
                {
                    //Common.UpdateTable("update collected_data set value_data = '" + UCD.value_data.Replace("'", "''") + "' where form_id = " + UCD.form_id + " and value_id = " + UCD.value_id);
                    int Id = Convert.ToInt32(UCD.form_id.Trim());
                    int valueId = Convert.ToInt32(UCD.value_id.Trim());
                    var isExist = dataContext.collected_data.Where(x => x.form_id == Id && x.value_id == valueId).FirstOrDefault();
                    collected_data tblcollected_data = new collected_data();
                    if (isExist != null)
                    {
                        tblcollected_data = dataContext.collected_data.Find(isExist.id);
                        dataContext.Entry(tblcollected_data).State = EntityState.Modified;
                        tblcollected_data.value_data = UCD.value_data.Replace("'", "''");
                        int result = dataContext.SaveChanges();
                    }
                }
            }
            return Messages.Updated;
        }
        #endregion UpdateClerkData

        #region Public UpdateOptions
        /// <summary>
        /// UpdateOptions
        /// </summary>
        /// <param name="uOData"></param>
        /// <returns></returns>
        public string UpdateOptions(List<UOData> uOData)
        {

            string Message = "";
            int form_id = 0;
            int QID = 0;
            string new_answer = "No";
            string name = HttpContext.Current.User.Identity.Name;
            DateTime approved_date = DateTime.Now;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                foreach (var UOD_item in uOData)
                {

                    if (UOD_item.form_id != "")
                    {
                        form_id = Convert.ToInt32(UOD_item.form_id);
                    }

                    if (UOD_item.QID != "")
                    {
                        QID = Convert.ToInt32(UOD_item.QID);
                    }
                    string option_list = UOD_item.option_list;
                    string custom_comment = UOD_item.custom_comment;
                    string website_link = UOD_item.website_link;
                    string[] options = option_list.Split('|');
                    if (HttpContext.Current.User.IsInRole("QA"))
                    {
                        //Common.UpdateTable("delete from form_q_scores_options where form_id = " + form_id + " and question_id = " + QID);
                        var isExist = dataContext.form_q_scores_options.Where(x => x.form_id == form_id && x.question_id == QID).ToList();
                        if (isExist != null)
                        {
                            dataContext.form_q_scores_options.RemoveRange(isExist);
                            dataContext.SaveChanges();
                        }
                    }

                    var isreviewDate = dataContext.vwForms.Where(x => x.F_ID == form_id).ToList();
                    var reviewDate = isreviewDate.FirstOrDefault().review_date;
                    //DataTable reviewDate = GetTable("select review_date from vwForm where f_id = " + form_id);
                    DateTime? review_date = reviewDate;
                    foreach (var option_string in options)
                    {
                        string[] option_data = option_string.Split(':');
                        if (HttpContext.Current.User.IsInRole("QA"))
                        {
                            if (option_data.Length == 2)
                            {
                                //Common.UpdateTable("insert into form_q_scores_options(option_value, option_pos,question_id, form_id) select '" + option_data[0].Replace("'", "''") + "','" + option_data[1] + "'," + QID + "," + form_id);
                                form_q_scores_options formScoresOptions = new form_q_scores_options()
                                {
                                    option_value = option_data[0].Replace("'", "''"),
                                    option_pos = Convert.ToDouble(option_data[1]),
                                    question_id = Convert.ToInt32(QID),
                                    form_id = form_id
                                };
                                dataContext.form_q_scores_options.Add(formScoresOptions);
                                int resultScoresOptions = dataContext.SaveChanges();
                                if (resultScoresOptions == 1)
                                {
                                    Message = Messages.Insert;
                                }
                            }
                            if (option_data.Length == 1)
                            {
                                //Common.UpdateTable("insert into form_q_scores_options(option_value, question_id, form_id) select '" + option_data[0].Replace("'", "''") + "'," + QID + "," + form_id);
                                form_q_scores_options formScoresOptions = new form_q_scores_options()
                                {
                                    option_value = option_data[0].Replace("'", "''"),
                                    question_id = Convert.ToInt32(QID),
                                    form_id = form_id
                                };
                                dataContext.form_q_scores_options.Add(formScoresOptions);
                                int resultScoresOptions = dataContext.SaveChanges();
                                if (resultScoresOptions == 1)
                                {
                                    Message = Messages.Insert;
                                }
                            }
                        }
                        else
                        {
                            if (option_data.Length == 2)
                            {

                                //Common.UpdateTable("insert into form_q_scores_options_changes(option_value, option_pos,question_id, form_id, changed_by, changed_date, new_value) select '" + option_data[0].Replace("'", "''") + "','" + option_data[1] + "'," + QID + "," + form_id + ",'" + name + "','" + approved_date + "','" + option_data[0].Replace("'", "''") + "'");
                                form_q_scores_options_changes formQcoresOptions_changes = new form_q_scores_options_changes()
                                {
                                    option_value = option_data[0].Replace("'", "''"),
                                    option_pos = Convert.ToDouble(option_data[1]),
                                    question_id = Convert.ToInt32(QID),
                                    form_id = form_id,
                                    changed_by = name,
                                    changed_date = approved_date,
                                    new_value = option_data[0].Replace("'", "''")
                                };
                                dataContext.form_q_scores_options_changes.Add(formQcoresOptions_changes);
                                int resultScoresOptions = dataContext.SaveChanges();
                                if (resultScoresOptions == 1)
                                {
                                    Message = Messages.Insert;
                                }
                            }
                            if (option_data.Length == 1)
                            {
                                //Common.UpdateTable("insert into form_q_scores_options_changes(option_value, question_id, form_id, changed_by, changed_date, new_value) select '" + option_data[0].Replace("'", "''") + "'," + QID + "," + form_id + ",'" + name + "','" + approved_date + "','" + option_data[0].Replace("'", "''") + "'");
                                form_q_scores_options_changes formQcoresOptions_changes = new form_q_scores_options_changes()
                                {
                                    option_value = option_data[0].Replace("'", "''"),
                                    question_id = Convert.ToInt32(QID),
                                    form_id = form_id,
                                    changed_by = name,
                                    changed_date = approved_date,
                                    new_value = option_data[0].Replace("'", "''")
                                };
                                dataContext.form_q_scores_options_changes.Add(formQcoresOptions_changes);
                                int resultScoresOptions = dataContext.SaveChanges();
                                if (resultScoresOptions == 1)
                                {
                                    Message = Messages.Insert;
                                }
                            }
                        }
                    }
                    if (HttpContext.Current.User.IsInRole("QA"))
                    {
                        if (website_link != "")
                        {
                            //Common.UpdateTable("update form_q_scores set view_link = '" + website_link + "' where question_id = " + QID + " and  form_id = " + form_id);
                            var isExist = dataContext.form_q_scores.Where(x => x.form_id == form_id && x.question_id == QID).FirstOrDefault();
                            form_q_scores tblform_q_scores = new form_q_scores();
                            if (isExist != null)
                            {
                                var Id = isExist.id;
                                tblform_q_scores = dataContext.form_q_scores.Find(Id);
                                dataContext.Entry(tblform_q_scores).State = EntityState.Modified;
                                tblform_q_scores.view_link = website_link;
                                int resultS = dataContext.SaveChanges();
                            }
                        }
                    }
                    else if (website_link != "")
                    {
                        if (!HttpContext.Current.User.IsInRole("QA"))
                        {

                            var isselect = dataContext.question_answers.Where(x => x.question_id == QID && x.answer_text == new_answer && x.answer_active == true).FirstOrDefault();
                            //Common.UpdateTable("INSERT INTO [dbo].[form_q_score_changes]([form_id],[question_id],[changed_by],[changed_date],[approved]," + "[approved_by],[approved_date],[new_value],view_link) Select " + form_id + ", " + QID + ",'" + name + "',dbo.getMTDate()," + "1,'" + name + "',dbo.getMTDate(),(select id from question_answers  where question_id = " + QID + " and answer_text = '" + new_answer + "' and answer_active = 1),'" + website_link + "'");
                            form_q_score_changes formscore_changes = new form_q_score_changes()
                            {
                                form_id = Convert.ToInt32(form_id),
                                question_id = Convert.ToInt32(QID),
                                changed_by = name,
                                changed_date = mtdate,
                                approved_by = name,
                                approved_date = mtdate,
                                new_value = isselect.id.ToString(),
                                view_link = website_link
                            };
                            dataContext.form_q_score_changes.Add(formscore_changes);
                            int resultScoresOptions = dataContext.SaveChanges();
                            if (resultScoresOptions == 1)
                            {
                                Message = Messages.Insert;
                            }
                        }
                    }

                    if (HttpContext.Current.User.IsInRole("QA"))
                    {
                        if (custom_comment != "")
                        {

                            //Common.UpdateTable("insert into form_q_responses(question_id, form_id, answer_id,other_answer_text) Select " + QID + ", " + form_id + ", 0 ,'" + custom_comment.Replace("'", "''") + "' ");
                            form_q_responses formresponses = new form_q_responses()
                            {
                                form_id = form_id,
                                question_id = Convert.ToInt32(QID),
                                answer_id = 0,
                                other_answer_text = custom_comment.Replace("'", "''")

                            };
                            dataContext.form_q_responses.Add(formresponses);
                            int resultR = dataContext.SaveChanges();
                            if (resultR == 1)
                            {
                                Message = Messages.Insert;
                            }
                        }
                    }
                    else if (custom_comment != "")
                    {
                        //Common.UpdateTable("insert into form_q_response_changes(question_id, form_id, changed_by, changed_date, new_value,other_answer_text) Select " + QID + ", " + form_id + ",'" + name + "','" + approved_date + "', 0 ,'" + custom_comment.Replace("'", "''") + "' ");
                        form_q_response_changes formresponse_changes = new form_q_response_changes()
                        {
                            form_id = form_id,
                            question_id = Convert.ToInt32(QID),
                            changed_by = name,
                            changed_date = approved_date,
                            new_value = 0,
                            other_answer_text = custom_comment.Replace("'", "''")

                        };
                        dataContext.form_q_response_changes.Add(formresponse_changes);
                        int result1 = dataContext.SaveChanges();
                        if (result1 == 1)
                        {
                            Message = Messages.Insert;
                        }
                    }
                    int num_opions_received = options.Length;

                    var questionOptions = dataContext.question_options.Where(x => x.question_id == QID && (x.date_end >= review_date && x.date_start <= review_date) && x.date_end == null).ToList().Count();
                    //DataTable q_dt = GetTable("select count(*) from question_options where question_id = " + QID + " and '" + review_date + "' between  date_start and isnull(date_end,'1/1/2099') ");
                    int options_avail = Convert.ToInt32(questionOptions);

                    if (options_avail == num_opions_received)
                    {
                        new_answer = "Yes";
                    }
                    else
                    {
                        new_answer = "No";
                    }
                    if (HttpContext.Current.User.IsInRole("QA"))
                    {
                        var questionOption = dataContext.question_answers.Where(x => x.answer_text == new_answer && x.question_id == QID && x.answer_active == true).FirstOrDefault();
                        //Common.UpdateTable("update form_q_scores set question_answered = (select id from question_answers where answer_text = '" + new_answer + "' and question_id = " + QID + " and answer_active = 1), original_question_answered = (select id from question_answers where answer_text = '" + new_answer + "' and question_id = " + QID + " and answer_active = 1) where form_id = " + form_id + " and question_id = " + QID);
                        var isExist2 = dataContext.form_q_scores.Where(x => x.form_id == form_id && x.question_id == QID).FirstOrDefault();
                        form_q_scores tblform_q_scores = new form_q_scores();
                        if (isExist2 != null)
                        {
                            var Id = isExist2.id;
                            tblform_q_scores = dataContext.form_q_scores.Find(Id);
                            dataContext.Entry(tblform_q_scores).State = EntityState.Modified;
                            tblform_q_scores.question_answered = questionOption.id;
                            tblform_q_scores.original_question_answered = questionOption.id;
                            dataContext.SaveChanges();
                        }
                    }
                    else
                    {
                        //Common.UpdateTable("update form_q_scores set question_answered = (select id from question_answers where answer_text = '" + new_answer + "' and question_id = " + QID + " and answer_active = 1) where form_id = " + form_id + " and question_id = " + QID);
                        var questionOption = dataContext.question_answers.Where(x => x.answer_text == new_answer && x.question_id == QID && x.answer_active == true).FirstOrDefault();
                        var isExist2 = dataContext.form_q_scores.Where(x => x.form_id == form_id && x.question_id == QID).FirstOrDefault();
                        form_q_scores tblform_q_scores = new form_q_scores();
                        if (isExist2 != null)
                        {
                            var Id = isExist2.id;
                            tblform_q_scores = dataContext.form_q_scores.Find(Id);
                            dataContext.Entry(tblform_q_scores).State = EntityState.Modified;
                            tblform_q_scores.question_answered = questionOption.id;
                            tblform_q_scores.original_question_answered = questionOption.id;
                            dataContext.SaveChanges();
                        }

                        //Common.UpdateTable("INSERT INTO [dbo].[form_q_score_changes]([form_id],[question_id],[changed_by],[changed_date],[approved]," + "[approved_by],[approved_date],[new_value],view_link) Select " + form_id + ", " + QID + ",'" + name + "',dbo.getMTDate()," + "1,'" + name + "',dbo.getMTDate(),(select id from question_answers  where question_id = " + QID + " and answer_text = '" + new_answer + "' and answer_active = 1),'" + website_link + "'");
                        var isselect = dataContext.question_answers.Where(x => x.question_id == QID && x.answer_text == new_answer && x.answer_active == true).FirstOrDefault();
                        form_q_score_changes formscore_changes = new form_q_score_changes()
                        {
                            form_id = form_id,
                            question_id = Convert.ToInt32(QID),
                            changed_by = name,
                            changed_date = mtdate,
                            approved_by = name,
                            approved_date = mtdate,
                            new_value = isselect.id.ToString(),
                            view_link = website_link
                        };
                        dataContext.form_q_score_changes.Add(formscore_changes);
                        int resultScoresOptions = dataContext.SaveChanges();
                        if (resultScoresOptions == 1)
                        {
                            Message = Messages.Insert;
                        }
                    }
                    //Common.UpdateTable("insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" + name + "', dbo.getMTDate(), '<strong>' + (select q_short_name from questions where id =  " + QID + ")   +  '</strong> changed to <strong>' + '" + new_answer + "</strong>'," + form_id + ", 'Call'");
                    system_comments tblsystem_comments = new system_comments();
                    var isQuestions = dataContext.Questions.Where(x => x.id == QID).FirstOrDefault().q_short_name;
                    if (isQuestions == null || isQuestions != null)
                    {
                        // Save system_comments data
                        tblsystem_comments.comment_who = name;
                        tblsystem_comments.comment_date = mtdate;
                        tblsystem_comments.comment = isQuestions;
                        tblsystem_comments.comment_id = form_id;
                        tblsystem_comments.comment_type = "Call";
                        dataContext.system_comments.Add(tblsystem_comments);
                        int result = dataContext.SaveChanges();
                    }
                }

                if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Calibrator") | HttpContext.Current.User.IsInRole("Recalibrator") | HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("Manager"))
                {
                    //Common.UpdateTable("update form_score3 set edited_score = total_score, wasedited = 1 where id = " + form_id);
                    var isExist = dataContext.form_score3.Where(x => x.id == form_id).FirstOrDefault();
                    form_score3 tblform_score3 = new form_score3();
                    if (isExist != null)
                    {
                        var Id = isExist.id;
                        tblform_score3 = dataContext.form_score3.Find(Id);
                        dataContext.Entry(tblform_score3).State = EntityState.Modified;
                        tblform_score3.edited_score = isExist.total_score;
                        tblform_score3.wasEdited = true;
                        int result = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("update form_q_scores_options set option_id = question_options.id from question_options where option_value = option_text and question_options.question_id = form_q_scores_options.question_id and option_id is null and form_id = " + form_id);
                    var scorecardResTemp = (from formscores_options in dataContext.form_q_scores_options
                                            join questionOptions in dataContext.question_options on formscores_options.question_id equals questionOptions.question_id
                                            where formscores_options.form_id == form_id
                                            select new { formscores_options, questionOptions.question_id, questionOptions.option_text, questionOptions.id }).ToList();
                    foreach (var item in scorecardResTemp)
                    {
                        var isExist2 = dataContext.form_q_scores_options.Where(x => x.id == item.formscores_options.id && x.question_id == item.question_id && x.option_value == item.option_text && x.option_id == null).FirstOrDefault();
                        form_q_scores_options tblformscores_options = new form_q_scores_options();
                        if (isExist2 != null)
                        {
                            var Id = isExist.id;
                            tblformscores_options = dataContext.form_q_scores_options.Find(Id);
                            dataContext.Entry(tblform_score3).State = EntityState.Modified;
                            tblformscores_options.option_id = item.id;
                            int result = dataContext.SaveChanges();
                        }
                    }
                    //Common.UpdateTable("[updateAllComments] " + form_id);
                    var updateAllComments = dataContext.updateAllComments(form_id).ToString();
                    var postProcessQuestions = dataContext.postProcessQuestions(form_id, name);
                    //Common.UpdateTable("postProcessQuestions " + form_id + ",'" + name + "'");
                    var updateMissed = dataContext.UpdateMissed(form_id);
                    //Common.UpdateTable("UpdateMissed " + form_id);
                    var createNotifications = dataContext.CreateNotifications(form_id, "Test");
                    //Common.UpdateTable("CreateNotifications " + form_id + ",'Test'");

                    var formscoreChanges = (from formscore_changes in dataContext.form_q_score_changes
                                            join vWForm in dataContext.vwForms on formscore_changes.form_id equals vWForm.F_ID
                                            where formscore_changes.form_id == form_id && formscore_changes.edited_score == null
                                            select new { formscore_changes, vWForm.edited_score }).FirstOrDefault();

                    var isExist3 = dataContext.form_q_scores_options.Where(x => x.form_id == formscoreChanges.formscore_changes.form_id).FirstOrDefault();
                    form_q_score_changes tblform_q_scores_options = new form_q_score_changes();
                    if (isExist3 != null)
                    {
                        tblform_q_scores_options = dataContext.form_q_score_changes.Find(isExist3.id);
                        dataContext.Entry(tblform_q_scores_options).State = EntityState.Modified;
                        tblform_q_scores_options.edited_score = formscoreChanges.formscore_changes.edited_score;
                        int result = dataContext.SaveChanges();
                    }
                    //Common.UpdateTable("update form_q_score_changes set edited_score = vwForm.edited_score from vwForm where form_id = " + form_id + " and f_id = " + form_id + "  and form_q_score_changes.edited_score is null"); // and changed_date > dateadd(s, -60, dbo.getMTDate())")

                    //Common.UpdateTable("update form_q_scores_options_changes set option_id = question_options.id from question_options where question_options.question_id  = form_q_scores_options_changes.question_id and option_text = option_value and option_id is null");
                    var postprocesSquestions = dataContext.postProcessQuestions(form_id, name);
                    //Common.UpdateTable("postprocessquestions " + form_id + ",'" + HttpContext.Current.User.Identity.Name + "'");
                }
                return Messages.Updated;
            }
        }
        #endregion UpdateOptions

        #region Public UpdateDispute
        /// <summary>
        /// UpdateDispute
        /// </summary>
        /// <param name="updateDisputeData"></param>
        /// <returns></returns>
        public Models.CCInternalAPI.disputeResult UpdateDispute(UpdateDisputeData updateDisputeData)
        {
            int noti_id = updateDisputeData.noti_id;
            string noti_button = updateDisputeData.noti_button;
            string noti_notes = updateDisputeData.noti_notes;
            bool noti_override = updateDisputeData.noti_override;
            int form_id = 0;
            if (updateDisputeData.form_id != "")
            {
                form_id = Convert.ToInt32(updateDisputeData.form_id);
            }
            Models.CCInternalAPI.disputeResult disp_res = new Models.CCInternalAPI.disputeResult();
            string ack_by = HttpContext.Current.User.Identity.Name.Replace("'", "''");
            string role = Roles.GetRolesForUser(ack_by).Single();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                string from = "";
                if (noti_id != 0)
                {
                    //DataTable from_dt = GetTable("select role from vwFN where fn_id = " + noti_id + " and date_closed is null");
                    var isvwFN = dataContext.vwFNs.Where(x => x.FN_ID == noti_id && x.date_closed == null).FirstOrDefault();
                    if (isvwFN != null)
                    {
                        from = isvwFN.role;
                    }
                }
                if (noti_id == 0)
                {
                    //sql = "insert into form_notifications (date_created,date_closed,closed_by,close_reason, comment, form_id, role) select dbo.getMTDate(), dbo.getMTDate(), '" + ack_by + "',  'Update',   @new_comments, " + form_id + ",(Select user_role from userextrainfo   where username = '" + ack_by + "'); ";
                    var isUser = dataContext.UserExtraInfoes.Where(x => x.username == ack_by).FirstOrDefault();
                    string Role = "";
                    if (isUser == null)
                    {
                        Role = null;
                    }
                    else
                    {
                        Role = isUser.user_role;
                    }
                    form_notifications form_notifications = new form_notifications();
                    if (isUser == null || isUser != null)
                    {
                        // Save form_notifications data
                        form_notifications.date_created = mtdate;
                        form_notifications.date_closed = mtdate;
                        form_notifications.closed_by = ack_by;
                        form_notifications.close_reason = "Update";
                        form_notifications.comment = ack_by;
                        form_notifications.form_id = form_id;
                        form_notifications.role = Role;
                        dataContext.form_notifications.Add(form_notifications);
                        int result1 = dataContext.SaveChanges();
                    }
                }
                else if (noti_button != "Notes Only" & noti_button != "Update")
                {

                    //sql = "update form_notifications set date_closed = dbo.getMTDate() ,closed_by = '" + ack_by + "',close_reason = 'Updated', comment = @new_comments where id = " + noti_id + " and date_closed is null;";
                    var isExist = dataContext.form_notifications.Where(p => p.id == noti_id && p.date_closed == null).FirstOrDefault();
                    form_notifications formNotifications = new form_notifications();
                    if (isExist == null)
                    {
                        dataContext.Entry(formNotifications).State = EntityState.Modified;
                        formNotifications.date_closed = mtdate;
                        formNotifications.closed_by = ack_by;
                        formNotifications.close_reason = "Update";
                        formNotifications.comment = noti_notes;
                        int result = dataContext.SaveChanges();
                    }
                }
                switch (noti_button)
                {
                    case "Supervisor":
                        {

                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Supervisor',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            disp_res.dispute_complete = false;
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Supervisor";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            break;
                        }

                    case "Agent":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Agent',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            disp_res.dispute_complete = false;
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Agent";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            break;
                        }

                    case "Calibrator/Dispute":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Calibrator',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Calibrator/Dispute";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();

                            //sql += "delete from calibration_pending where form_id = " + form_id + " and date_completed is null ;";
                            var isExist = dataContext.calibration_pending.Where(x => x.form_id == form_id && x.date_completed == null).FirstOrDefault();
                            if (isExist != null)
                            {
                                dataContext.calibration_pending.Remove(isExist);
                                int result = dataContext.SaveChanges();
                            }
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Tango Lead":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Tango TL',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Tango TL";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();

                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "QA":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "QA";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Team Lead":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA Lead',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "QA Lead";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Account Manager":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Account Manager',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Account Manager";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Manager":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Manager',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Manager";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Center Manager":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Center Manager',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Center Manager";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Admin":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Admin',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Admin";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Client":
                        {
                            //sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Client',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "';";
                            form_notifications form_notifications = new form_notifications();
                            // Save form_notifications data
                            form_notifications.date_created = mtdate;
                            form_notifications.opened_by = ack_by;
                            form_notifications.form_id = form_id;
                            form_notifications.role = "Client";
                            dataContext.form_notifications.Add(form_notifications);
                            int result1 = dataContext.SaveChanges();
                            disp_res.dispute_complete = false;
                            break;
                        }

                    case "Notes Only":
                        {
                            //sql = "insert into form_notifications (date_created,date_closed,closed_by,close_reason, comment, form_id, role) select dbo.getMTDate(), dbo.getMTDate(), '" + ack_by + "',  'Update',   @new_comments, " + form_id + ",(Select user_role from userextrainfo   where username = '" + ack_by + "'); ";
                            var isUser = dataContext.UserExtraInfoes.Where(x => x.username == ack_by).FirstOrDefault();
                            form_notifications form_notifications = new form_notifications();
                            if (isUser == null || isUser != null)
                            {
                                // Save form_notifications data
                                form_notifications.date_created = mtdate;
                                form_notifications.date_closed = mtdate;
                                form_notifications.closed_by = ack_by;
                                form_notifications.close_reason = "Update";
                                form_notifications.comment = noti_notes;
                                form_notifications.form_id = form_id;
                                form_notifications.role = isUser.user_role;
                                dataContext.form_notifications.Add(form_notifications);
                                int result1 = dataContext.SaveChanges();
                            }
                            disp_res.dispute_complete = true;
                            break;
                        }
                }

                disp_res.dispute_response = "";

                if (disp_res.dispute_complete == false)
                {

                    //sql += "insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" + ack_by + "', dbo.getMTDate(), 'Notification moved from <strong>" + from + "</strong> to <strong>" + noti_button + "</strong>'," + form_id + ", 'Call';";
                    system_comments tblsystem_comments = new system_comments();
                    // Save system_comments data
                    tblsystem_comments.comment_who = ack_by;
                    tblsystem_comments.comment_date = mtdate;
                    tblsystem_comments.comment = "'Notification moved from <strong>" + from + "</strong> to <strong>" + noti_button + "</strong>',";
                    tblsystem_comments.comment_id = form_id;
                    tblsystem_comments.comment_type = "Call";
                    dataContext.system_comments.Add(tblsystem_comments);
                    int result = dataContext.SaveChanges();
                }
                if (noti_button == "Agenda Item")
                {
                    disp_res.dispute_redirect = "agenda_items.aspx";
                }
            }
            return disp_res;
        }
        #endregion UpdateDispute




        #region Public assembleItem
        /// <summary>
        /// assembleItem
        /// </summary>
        /// <param name="xccReportNew"></param>
        /// <returns></returns>
        public ManipulateAudioResult assembleItem(List<XccReportNew> xccReportNew)
        {

            var firstRows = xccReportNew.FirstOrDefault();
            int Id = 0;
            string appname = "";
            if (firstRows != null)
            {
                Id = firstRows.ID;
                appname = firstRows.appname;
            }
            ManipulateAudioResult MAResult = new ManipulateAudioResult();
            string server_root = HttpContext.Current.Server.MapPath("/");
            string output = "";
            string out_error = "";
            string concats = "";
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var audioDt = dataContext.GetXccReportNewAudioData(Id).ToList();
                //DataTable audioDt = GetTable(@"select  distinct  xcc_report_new.ID as theID, recording_user, record_password,audio_port,  file_order, isnull(failed_downloads, 0) as failed_downloads,isnull(record_format,'MM_dd_yyyy') as record_format,  call_date,  case when replace(file_name,'/','\') not like '%\%' then isnull(recording_url,'') + file_name else file_name end as file_name, isnull(recording_url,'') + replace(file_name,'/','\') as local_file, xcc_report_new.appname, session_id, isnull(format([call_date],'MM/dd/yyyy'),case when isdate(file_date) = 1 then file_date else null end) as theDate, profile_ID from audiodata  join xcc_report_new  on xcc_report_new.id = audiodata.final_xcc_id join app_settings on app_settings.appname = xcc_report_new.appname  where xcc_report_new.id = " + date_dr["id"] + "  and file_name <> '' order by file_order, isnull(failed_downloads, 0)"); // order by file_date")

                string new_call = Convert.ToDateTime(firstRows.call_date).ToString(firstRows.record_format.ToString());

                string session_id = firstRows.ID.ToString();
                string fileExists = HttpContext.Current.Server.MapPath("/audio/concats_" + firstRows.ID + ".txt");
                if (File.Exists(fileExists))
                {
                    File.Delete(HttpContext.Current.Server.MapPath("/audio/concats_" + firstRows.ID + ".txt"));
                }
                //LocalizeAudio LA = new LocalizeAudio();
                //List<LocalizeAudio.LocalResult> LAresult = LA.MakeLocal(audioDt);

                string concat = "-i " + Strings.Chr(34) + "concat:" + concats + Strings.Chr(34) + " -c copy " + concats + Strings.Chr(34) + server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST.mp3" + concats + Strings.Chr(34) + " -y";
                concat = "-f concat -safe " + Strings.Chr(34) + "0" + Strings.Chr(34) + " -i " + Strings.Chr(34) + server_root + @"\audio\concats_" + firstRows.ID + ".txt" + Strings.Chr(34) + " -c copy " + Strings.Chr(34) + server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST2.mp3" + Strings.Chr(34) + " -y";
                RunFFMPEG(concat, ref output, ref out_error);
                int x = 0;
                while (!System.IO.File.Exists(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST2.mp3") | x > 90)
                {
                    System.Threading.Thread.Sleep(500);
                    x = x + 1;
                }
                string ff_string = "-i  " + Strings.Chr(34) + server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST2.mp3" + Strings.Chr(34) + " -b:a 16k  " + Strings.Chr(34) + server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST.mp3" + Strings.Chr(34);
                RunFFMPEG(ff_string, ref output, ref out_error);
                x = 0;
                while (!System.IO.File.Exists(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST.mp3") | x > 90)
                {
                    System.Threading.Thread.Sleep(500);
                    x = x + 1;
                }
                File.Delete(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST2.mp3");
                //foreach (var LR in LAresult)
                //{
                //    try
                //    {
                //        File.Delete(LR.local_audio);
                //    }
                //    catch (Exception ex)
                //    {
                //        HttpContext.Current.Response.Write(LR.local_audio + "<br>");
                //    }
                //}
                File.Delete(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + ".mp3");
                File.Move(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + "_TEST.mp3", server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + ".mp3");
                if (System.IO.File.Exists(server_root + @"\audio\" + appname + @"\" + new_call + @"\" + session_id + ".mp3"))
                {
                    var isExist = dataContext.XCC_REPORT_NEW.Where(p => p.ID == Id).FirstOrDefault();
                    XCC_REPORT_NEW XccReportNew = new XCC_REPORT_NEW();

                    if (isExist == null)
                    {
                        //Common.UpdateTable("update xcc_report_new set  audio_link = '" + "/audio/" + appname + "/" + new_call + "/" + session_id + ".mp3', recreate_call = null, onAWS = 0 where id = " + date_dr["id"]);
                        dataContext.Entry(XccReportNew).State = EntityState.Modified;
                        XccReportNew.audio_link = "/audio/" + appname + "/" + new_call + "/" + session_id + ".mp3";
                        XccReportNew.recreate_call = null;
                        XccReportNew.onAWS = false;
                        int result = dataContext.SaveChanges();
                    }
                }
                else
                {
                    //Common.UpdateTable("update xcc_report_new set failed_downloads=isnull(failed_downloads,0) + 1 where id = " + date_dr["id"]);
                    var isExist = dataContext.XCC_REPORT_NEW.Where(p => p.ID == Id).FirstOrDefault();
                    XCC_REPORT_NEW XccReportNew = new XCC_REPORT_NEW();
                    if (isExist == null)
                    {
                        dataContext.Entry(XccReportNew).State = EntityState.Modified;
                        XccReportNew.failed_downloads = Convert.IsDBNull(firstRows.failed_downloads) ? new int() : (int)(firstRows.failed_downloads);
                        int result = dataContext.SaveChanges();
                    }
                }
                MAResult.IsSuccess = true;
                MAResult.Audio = "/audio/" + appname + "/" + new_call + "/" + session_id + ".mp3";
                return MAResult;
            }
        }
        #endregion  assembleItem

    }
}