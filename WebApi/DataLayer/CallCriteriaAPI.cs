﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WebApi.Entities;
using WebApi.Models.CallCriteriaAPI;

namespace WebApi.DataLayer
{
    /// <summary>
    /// CallCriteriaAPI
    /// </summary>
    public class CallCriteriaAPI
    {

        /// <summary>
        /// GetCallRecord
        /// </summary>
        /// <param name="call_date"></param>
        /// <param name="rev_date"></param>
        /// <param name="appname"></param>
        /// <param name="use_review"></param>
        /// <returns></returns>
        public List<CallRecord> GetAllRecordsWithPending(DateTime call_date, bool rev_date, string appname, string use_review)
        {
            List<CallRecord> objCallRecord = new List<CallRecord>();
            try
            {
                using (CC_ProdEntities dataContext = new CC_ProdEntities())
                {
                    //SqlCommand reply = new SqlCommand("exec GetAllRecordsWithPending @call_date, @appname, @use_review", cn);
                    int useReview = 0;
                    if (use_review != "")
                    {
                        useReview = Convert.ToInt32(use_review);
                    }
                    CallRecord scr = new CallRecord();
                    ObjectResult<GetAllRecordsWithPending_Result> objGetAllRecords = dataContext.GetAllRecordsWithPending(call_date, appname, useReview);

                    var SubCallRecord = (from records in objGetAllRecords
                                         select new CallRecord
                                         {
                                             F_ID = records.F_ID.ToString(),
                                             review_ID = records.review_ID.ToString(),
                                             Comments = records.Comments,
                                             autofail = records.autofail.ToString(),
                                             reviewer = records.autofail.ToString(),
                                             appname = records.autofail.ToString(),
                                             total_score = records.autofail.ToString(),
                                             total_score_with_fails = records.autofail.ToString(),
                                             call_length = records.autofail.ToString(),
                                             has_cardinal = "",
                                             fs_audio = records.autofail.ToString(),
                                             week_ending_date = records.autofail.ToString(),
                                             num_missed = records.autofail.ToString(),
                                             missed_list = records.autofail.ToString(),
                                             call_made_date = records.autofail.ToString(),
                                             AGENT = records.autofail.ToString(),
                                             ANI = records.autofail.ToString(),
                                             DNIS = records.autofail.ToString(),
                                             TIMESTAMP = records.autofail.ToString(),
                                             TALK_TIME = records.autofail.ToString(),
                                             CALL_TIME = records.autofail.ToString(),
                                             CALL_TYPE = records.autofail.ToString(),
                                             leadid = records.autofail.ToString()
                                         }).ToList();

                    foreach (var Item in SubCallRecord)
                    {
                        if (Item.F_ID != "")
                        {
                            int FId = Convert.ToInt32(Item.F_ID);
                            List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                            //DataTable qdt = GetTable("select *,questions.id as q_id from form_q_scores join questions on questions.id = form_q_scores.question_id   join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + dr["F_ID"].ToString() + " order by q_order");
                            var ScoreResponse = (from form_q_scores in dataContext.form_q_scores
                                                 join questions in dataContext.Questions on form_q_scores.question_id equals questions.id
                                                 join questionA in dataContext.question_answers on form_q_scores.question_answered equals questionA.id
                                                 where form_q_scores.form_id == FId
                                                 select new ScorecardResponse
                                                 {
                                                     position = string.IsNullOrEmpty(form_q_scores.click_text) ? form_q_scores.q_position : form_q_scores.click_text,
                                                     question = questions.q_short_name.ToString(),
                                                     result = questionA.answer_text.ToString(),
                                                     QID = questions.id,
                                                     QAPoints = (int)questions.QA_points,
                                                     ViewLink = form_q_scores.view_link.ToString(),
                                                     comments_allowed = (bool)questions.comments_allowed,
                                                     RightAnswer = (bool)questionA.right_answer,
                                                     q_order = questions.q_order,
                                                 }).OrderBy(x => x.q_order).ToList();

                            if (ScoreResponse != null)
                            {
                                foreach (var item in ScoreResponse)
                                {
                                    //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                                    var formResponses = (from formresponses in dataContext.form_q_responses
                                                         join answercomments in dataContext.answer_comments on formresponses.answer_id equals answercomments.id
                                                         where formresponses.form_id == FId && formresponses.question_id == item.QID
                                                         select new { answercomments.comment, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                    if (formResponses.Count > 0)
                                    {
                                        List<string> ans_comment = new List<string>();
                                        foreach (var ans_dr in formResponses)
                                        {
                                            ans_comment.Add(ans_dr.comment);
                                        }
                                        item.QComments = ans_comment;
                                    }
                                    qrs.Add(item);
                                }
                            }
                            scr.ScorecardResponses = qrs;
                            List<ClerkedData> CDs = new List<ClerkedData>();
                            //DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                            var clerkDataTemp = (from collected_data in dataContext.collected_data
                                                 join sc_inputs in dataContext.sc_inputs on collected_data.value_id equals sc_inputs.id
                                                 where collected_data.form_id == FId
                                                 select new ClerkedData
                                                 {
                                                     value = sc_inputs.value,
                                                     data = collected_data.value_data,
                                                     position = collected_data.value_position.ToString(),
                                                     ID = collected_data.value_id.ToString()
                                                 }).ToList();
                            scr.ClerkedDataItems = new List<ClerkedData>();
                            scr.ClerkedDataItems.AddRange(clerkDataTemp);
                            scr.ClerkedDataItems = CDs;
                        }
                        objCallRecord.Add(scr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallRecord;
        }


        /// <summary>
        /// GetCallsLoaded
        /// </summary>
        /// <param name="callsLoaded"></param>
        /// <returns></returns>
        public List<CallLoaded> GetCallsLoaded(CallsLoaded callsLoaded)
        {
            string appname = HttpContext.Current.Request["appname"];
            List<CallLoaded> myCLs = new List<CallLoaded>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                if (Information.IsDate(callsLoaded.loaded_date))
                {
                    DateTime loadedDate = Convert.ToDateTime(callsLoaded.loaded_date);
                    //DataTable dt = GetTable("select * from xcc_report_new where call_date = '" + callsLoaded.loaded_date + "' and appname = '" + appname + "'");
                    var xccReportNew = dataContext.XCC_REPORT_NEW.Where(x => x.call_date == loadedDate && x.appname == appname).ToList();
                    foreach (var dr in xccReportNew)
                    {
                        CallLoaded call_loaded = new CallLoaded();
                        call_loaded.session_id = dr.SESSION_ID;
                        call_loaded.phone = dr.phone;
                        call_loaded.call_date = dr.call_date.ToString();
                        call_loaded.date_added = dr.date_added.ToString();
                        call_loaded.audio_link = dr.audio_link;
                        call_loaded.CC_ID = dr.ID.ToString();
                        if (dr.MAX_REVIEWS == 0)
                        {
                            call_loaded.status = "Pending";
                        }
                        if (dr.MAX_REVIEWS == 1)
                        {
                            call_loaded.status = "Worked";
                        }
                        if (dr.bad_call == 1)
                        {
                            call_loaded.status = "Bad Call";
                        }
                        myCLs.Add(call_loaded);
                    }
                }
            }
            return myCLs;
        }

        /// <summary>
        /// GetRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public AllCallRecord GetRecordID(SimpleID SI) 
        {
            int ID = 0;
            if (SI.ID !="")
            {
                ID =Convert.ToInt32(SI.ID);
            }
            AllCallRecord scr = new AllCallRecord();
            string username = HttpContext.Current.User.Identity.Name;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var isUser = dataContext.UserExtraInfoes.Where(x => x.username == username).FirstOrDefault();
                //SqlCommand reply = new SqlCommand("select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username", cn);
                string non_edit = "";
                bool wasEdited = false;
                UserObject objUserObject = new UserObject();
                if (username != "")
                {
                    non_edit = isUser.non_edit.ToString();
                    objUserObject.UserRole = isUser.user_role;
                    objUserObject.UserName = isUser.username;
                    objUserObject.SpeedInc = isUser.speed_increment.ToString();
                    objUserObject.StartImmediately = Convert.ToBoolean(isUser.calls_start_immediately);
                    scr.UserInfo = objUserObject;
                }

                string add_sql;
                if (HttpContext.Current.Request.QueryString["appname"] != null)
                {
                    add_sql = " and vwForm.appname = '" + HttpContext.Current.Request["appname"] + "'";
                }
                else
                { 
                    add_sql = " and vwForm.scorecard in (select user_scorecard from userapps where username= '" + username + "') ";
                }
                reply = new SqlCommand("select vwForm.*,isnull(client_logo, vwForm.appname) as client_logo, (select total_score from vwCF where active_cali = 1 and f_id = @id) as qa_cali_score  from vwForm join app_settings on vwForm.appname = app_settings.appname where f_id = @id" + add_sql, cn);
                var getvwForm = dataContext.getvwForm(ID, "", null).ToList();
              
                if (getvwForm.Count == 0)
                {
                    return scr;
                }
                var reviewId = getvwForm.FirstOrDefault();
                int review_id = Convert.ToInt32(reviewId.review_ID);
                List<ScorecardData> scds = new List<ScorecardData>();
                foreach (var dr in getvwForm)
                {
                    scr.F_ID = dr.F_ID.ToString();
                    scr.client_logo = dr.client_logo;
                    scr.review_ID = dr.review_ID.ToString();
                    scr.Comments = dr.Comments;
                    scr.autofail = dr.autofail;
                    scr.reviewer = dr.reviewer;
                    scr.appname = dr.appname;
                    scr.total_score = dr.display_score.ToString();
                    scr.total_score_with_fails = dr.display_score.ToString();
                    scr.call_length = dr.call_length.ToString();
                    scr.has_cardinal = ""; // dr("has_cardinal").ToString()
                    scr.fs_audio = dr.fs_audio;
                    scr.week_ending_date = dr.week_ending_date.ToString();
                    scr.num_missed = dr.num_missed.ToString();
                    scr.missed_list = dr.missed_list;
                    scr.call_made_date = dr.call_made_date.ToString();
                    scr.AGENT = dr.AGENT;
                    scr.ANI = dr.ANI;
                    scr.DNIS = dr.DNIS;
                    scr.TIMESTAMP = dr.TIMESTAMP;
                    scr.TALK_TIME = dr.TALK_TIME;
                    scr.CALL_TIME = dr.CALL_TIME.ToString();
                    scr.CALL_TYPE = dr.CALL_TYPE;
                    scr.leadid = dr.leadid;
                    scr.AGENT_GROUP = dr.AGENT_GROUP;
                    scr.Email = dr.Email;
                    scr.City = dr.City;
                    scr.State = dr.State;
                    scr.Zip = dr.Zip;
                    scr.Datacapturekey = dr.Datacapturekey.ToString();
                    scr.Datacapture = dr.Datacapture.ToString();
                    scr.Status = dr.Status;
                    scr.Program = dr.Program;
                    scr.X_ID = dr.X_ID.ToString();
                    scr.Datacapture_Status = dr.Datacapture_Status;
                    scr.num_of_schools = dr.num_of_schools;
                    scr.MAX_REVIEWS = dr.MAX_REVIEWS.ToString();
                    scr.review_started = dr.review_started.ToString();
                    scr.Number_of_Schools = dr.Number_of_Schools;
                    scr.EducationLevel = dr.EducationLevel;
                    scr.HighSchoolGradYear = dr.HighSchoolGradYear;
                    scr.DegreeStartTimeframe = dr.DegreeStartTimeframe;
                    scr.Expr3 = dr.Expr3;
                    scr.First_Name = dr.First_Name;
                    scr.Last_Name = dr.Last_Name;
                    scr.address = dr.address;
                    scr.phone = dr.phone;
                    scr.call_date = dr.call_date.ToString();
                    //scr.audio_link = Common.GetAudioFileName(dr);
                    scr.profile_id = dr.profile_id;
                    scr.audio_user = "";
                    scr.audio_password = "";
                    scr.LIST_NAME = dr.LIST_NAME;
                    scr.review_date = dr.review_date.ToString();
                    scr.CAMPAIGN = dr.CAMPAIGN;
                    scr.DISPOSITION = dr.DISPOSITION;
                    scr.bad_call = dr.bad_call.ToString();
                    scr.to_upload = "";
                    scr.SESSION_ID = dr.SESSION_ID;
                    scr.agent_deviation = "";
                    scr.pass_fail = dr.pass_fail;
                    scr.scorecard = dr.scorecard.ToString();
                    scr.scorecard_name = dr.Scorecard_name.ToString();
                    scr.uploaded = "";
                    scr.formatted_comments = dr.formatted_comments;
                    scr.formatted_missed = dr.formatted_missed;
                    scr.fileUrl = dr.fileUrl;
                    scr.statusMessage = dr.statusMessage;
                    scr.mediaId = ""; // dr("mediaId").ToString()
                    scr.requestStatus = ""; // dr("requestStatus").ToString()
                    scr.fileStatus = ""; // dr("fileStatus").ToString()
                    scr.response = "";
                    scr.review_time = "";
                    scr.wasEdited = dr.wasEdited.ToString();
                    scr.website = dr.website;
                    scr.pending_id = "";
                    scr.bad_call_reason = dr.bad_call_reason;
                    scr.date_added = dr.date_added.ToString();
                    scr.calib_score = dr.calib_score.ToString();
                    scr.edited_score = dr.edited_score.ToString();
                    scr.compliance_sheet = dr.compliance_sheet;
                    scr.editable = true;
                   if(dr.wasEdited == true)
                    { 
                        wasEdited = Convert.ToBoolean(dr.wasEdited);
                    }
                   else
                    {
                        wasEdited = false;
                    }
                    if ((objUserObject.UserRole == "QA" & (dr.calib_score.ToString() != "" | dr.edited_score.ToString() != "")) | non_edit == "True" | objUserObject.UserRole == "Agent")
                    {
                        scr.editable = false;
                    }
                    List<ClerkedData> CDs = new List<ClerkedData>();
                    //DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                    var clerkDataTemp = (from collected_data in dataContext.collected_data
                                         join sc_inputs in dataContext.sc_inputs on collected_data.value_id equals sc_inputs.id
                                         where collected_data.form_id == dr.F_ID
                                         select new ClerkedData
                                         {
                                             value = sc_inputs.value,
                                             data = collected_data.value_data,
                                             position = collected_data.value_position.ToString(),
                                             ID = collected_data.value_id.ToString()
                                         }).ToList();
                    scr.ClerkedDataItems = new List<ClerkedData>();
                    scr.ClerkedDataItems.AddRange(clerkDataTemp);
                    scr.ClerkedDataItems = CDs;
                   
                    ScorecardData scd = new ScorecardData();
                    UserObject scu = new UserObject();
                    scu.UserRole = "QA";
                    scu.UserTitle = "QA Response";
                    scd.ScorecardUser = scu;
                    CallScores cs = new CallScores();
                    if (dr.calib_score.ToString() == "" & dr.edited_score.ToString() == "")
                    {
                        cs.score = dr.display_score.ToString();
                    }
                    else
                    {
                        cs.score = Convert.ToString(Convert.IsDBNull(dr.original_qa_score) ? Convert.ToString(Convert.IsDBNull(dr.total_score) ? "N/A" : dr.total_score.ToString()) : dr.original_qa_score.ToString());
                    }
                    cs.reviewer = dr.reviewer;
                    cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                    if (!Information.IsDBNull(dr.qa_cali_score))
                    {
                        cs.calibrationscore = dr.qa_cali_score.ToString();
                    }
                    cs.role = "QA";
                    scd.CallScore = cs;

                    List<string> audio_list = new List<string>();
                    if (dr.phone.ToString() == "")
                    {
                        dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where (WAV_DATA.session_id = '" + dr["session_id"] + "')  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");
                    }
                    else
                    {
                        dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where ((WAV_DATA.session_id = '" + dr["session_id"] + "') or (WAV_DATA.filename like  '%" + dr["phone"] + "%' ))  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");
                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow audio_dr in dt.Rows)

                            audio_list.Add(ReverseMapPath(audio_dr["filename"].ToString()));
                    }
                    dt = GetTable("select * from audiodata where final_xcc_id = " + review_id);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow audio_dr in dt.Rows)

                            audio_list.Add(ReverseMapPath(audio_dr["file_name"].ToString()));
                    }
                    scr.audio_merge = audio_list;
                    DataTable section_dt = GetTable("exec getSections2 " + ID);
                    List<SectionData> sections = new List<SectionData>();
                    foreach (DataRow section_dr in section_dt.Rows)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr["section"].ToString();
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();

                        DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + dr["F_ID"].ToString() + ", " + section_dr["ID"].ToString() + ") left join (Select q_position, answer_text, form_q_scores.question_id, right_answer, view_link from form_q_scores join question_answers On question_answers.ID = form_q_scores.original_question_answered where form_id = " + dr["F_ID"].ToString() + ") a On  a.question_id = q_id  join questions On questions.ID = q_id  where active = 1 order by all_q_order");
                        foreach (DataRow qdr in qdt.Rows)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr["q_position"].ToString();
                            qr.question = qdr["q_short_name"].ToString();
                            qr.result = qdr["answer_text"].ToString();
                            qr.QID = Convert.ToInt32(qdr["q_id"]);
                            try
                            {
                                qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                            }
                            catch (Exception ex)
                            {
                            }
                            qr.QType = qdr["q_type"].ToString();
                            qr.ViewLink = qdr["view_link"].ToString();
                            qr.comments_allowed = Convert.ToBoolean(qdr["comments_allowed"]);
                            if (qdr["right_answer"].ToString() == "")
                                qr.RightAnswer = Convert.ToBoolean(0);
                            else
                                qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);

                            DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            if (ans_dt.Rows.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (DataRow ans_dr in ans_dt.Rows)
                                    ans_comment.Add(ans_dr["comment"].ToString());
                                qr.QComments = ans_comment;
                            }
                            DataTable temp_dt = GetTable("exec getTemplateItems " + ID + "," + qdr["q_id"].ToString());
                            if (temp_dt.Rows.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (DataRow temp_dr in temp_dt.Rows)
                                {
                                    CheckItems temp_item = new CheckItems();
                                    if (temp_dr["value"].ToString() == temp_dr["option_value"].ToString())
                                        temp_item.Checked = true;
                                    else
                                        temp_item.Checked = false;
                                    temp_item.Item = temp_dr["value"].ToString();
                                    temp_item.Position = temp_dr["option_pos"].ToString();
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
                dt = new DataTable();
                reply = new SqlCommand("Select *, (select recal_score from vwCF where isrecal = 1 and f_id = @id) as cal_recal_score from vwCF join userextrainfo On userextrainfo.username = reviewed_by where f_id = @id", cn);
                reply.Parameters.AddWithValue("id", ID);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    ScorecardData scd = new ScorecardData();
                    List<SectionData> sections = new List<SectionData>();
                    UserObject scu = new UserObject();
                    scu.UserRole = dr["user_role"].ToString();
                    scu.UserTitle = dr["user_role"] + " Response";
                    scd.ScorecardUser = scu;
                    CallScores cs = new CallScores();
                    cs.score = Convert.IsDBNull(dr["cali_form_score"]) ? "0" : dr["cali_form_score"].ToString();
                    cs.reviewer = dr["reviewed_by"].ToString();

                    if (dr["user_role"].ToString() == "Calibrator")
                        cs.calibrationscore = dr["cal_recal_score"].ToString();
                    else
                        cs.calibrationscore = dr["total_score"].ToString();
                    cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToShortDateString();
                    cs.role = dr["user_role"].ToString();
                    scd.CallScore = cs;
                    DataTable section_dt = GetTable("exec getSections2 " + ID);
                    foreach (DataRow section_dr in section_dt.Rows)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr["section"].ToString();
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();

                        DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + ID + "," + section_dr["ID"] + ") left join (select  q_pos, answer_text, calibration_scores.question_id, right_answer, view_link from calibration_scores join question_answers On question_answers.ID = calibration_scores.question_result where form_id = " + dr["ID"].ToString() + ") a on a.question_id = q_id join questions On questions.ID = q_id where active = 1 order by all_q_order");
                        foreach (DataRow qdr in qdt.Rows)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr["q_pos"].ToString();
                            qr.question = qdr["q_short_name"].ToString();
                            qr.result = qdr["answer_text"].ToString();
                            qr.QID = Convert.ToInt32(qdr["q_id"]);
                            qr.QType = qdr["q_type"].ToString();
                            try
                            {
                                qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                            }
                            catch (Exception ex)
                            {
                            }
                            qr.ViewLink = qdr["view_link"].ToString();
                            qr.comments_allowed = Convert.ToBoolean(qdr["comments_allowed"]);
                            if (qdr["right_answer"].ToString() == "")
                                qr.RightAnswer = Convert.ToBoolean(0);
                            else
                                qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);
                            DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As q_comment, comment_points from form_c_responses left join answer_comments On form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = " + dr["ID"].ToString() + " And form_c_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            if (ans_dt.Rows.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (DataRow ans_dr in ans_dt.Rows)
                                    ans_comment.Add(ans_dr["q_comment"].ToString());
                                qr.QComments = ans_comment;
                            }
                            DataTable temp_dt = GetTable("exec getCTemplateItems " + dr["ID"].ToString() + "," + qdr["q_id"].ToString());
                            if (temp_dt.Rows.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (DataRow temp_dr in temp_dt.Rows)
                                {
                                    CheckItems temp_item = new CheckItems();
                                    if (temp_dr["value"].ToString() == temp_dr["option_value"].ToString())
                                        temp_item.Checked = true;
                                    else
                                        temp_item.Checked = false;
                                    temp_item.Item = temp_dr["value"].ToString();
                                    temp_item.Position = temp_dr["option_pos"].ToString();
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
                dt = new DataTable();
                reply = new SqlCommand("Select * from calibration_form_client join userextrainfo On userextrainfo.username = reviewed_by where original_form = @id", cn);
                reply.Parameters.AddWithValue("id", ID);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    ScorecardData scd = new ScorecardData();
                    List<SectionData> sections = new List<SectionData>();
                    UserObject scu = new UserObject();
                    scu.UserRole = dr["user_role"] + " - Client Calib";
                    scu.UserTitle = dr["reviewed_by"].ToString();
                    CallScores cs = new CallScores();
                    cs.score = Convert.IsDBNull(dr["total_score"]) ? "0" : dr["total_score"].ToString();
                    cs.reviewer = dr["reviewed_by"].ToString();
                    cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToShortDateString();
                    cs.role = dr["user_role"].ToString();
                    scd.CallScore = cs;
                    scd.ScorecardUser = scu;
                    DataTable section_dt = GetTable("exec getSections2 " + ID);
                    foreach (DataRow section_dr in section_dt.Rows)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr["section"].ToString();
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                        DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + ID + "," + section_dr["ID"] + ") left join (select  q_pos, answer_text, calibration_scores_client.question_id, right_answer, view_link from calibration_scores_client join question_answers On question_answers.ID = calibration_scores_client.question_result where form_id = " + dr["ID"].ToString() + ") a on a.question_id = q_id join questions On questions.ID = q_id where active = 1 order by all_q_order");
                        foreach (DataRow qdr in qdt.Rows)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr["q_pos"].ToString();
                            qr.question = qdr["q_short_name"].ToString();
                            qr.result = qdr["answer_text"].ToString();
                            qr.QID = Convert.ToInt32(qdr["q_id"]);
                            qr.QType = qdr["q_type"].ToString();
                            try
                            {
                                qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                            }
                            catch (Exception ex)
                            {
                                qr.QAPoints = 0;
                            }
                            qr.ViewLink = qdr["view_link"].ToString();
                            qr.comments_allowed = Convert.ToBoolean(qdr["comments_allowed"]);
                            if (qdr["right_answer"].ToString() == "")
                                qr.RightAnswer = Convert.ToBoolean(0);
                            else
                                qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);
                            DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As q_comment from form_c_responses_client left join answer_comments On form_c_responses_client.answer_id = answer_comments.id where form_c_responses_client.form_id = " + dr["ID"].ToString() + " And form_c_responses_client.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            if (ans_dt.Rows.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (DataRow ans_dr in ans_dt.Rows)
                                    ans_comment.Add(ans_dr["q_comment"].ToString());
                                qr.QComments = ans_comment;
                            }
                            DataTable temp_dt = GetTable("exec getCTemplateItems " + dr["ID"].ToString() + "," + qdr["q_id"].ToString() + ", 1");
                            if (temp_dt.Rows.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (DataRow temp_dr in temp_dt.Rows)
                                {
                                    CheckItems temp_item = new CheckItems();
                                    if (temp_dr["value"].ToString() == temp_dr["option_value"].ToString())
                                        temp_item.Checked = true;
                                    else
                                        temp_item.Checked = false;
                                    temp_item.Item = temp_dr["value"].ToString();
                                    temp_item.Position = temp_dr["option_pos"].ToString();
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

                if (wasEdited)
                {
                    dt = new DataTable();
                    reply = new SqlCommand("select changed_date as review_date, changed_by, user_role, username, isnull(edited_score, (select total_score from vwForm where f_id = @f_id)) as total_score, form_id as f_id from form_q_score_changes join userextrainfo on changed_by = username where form_q_score_changes.id in (select max(id) from form_q_score_changes where form_id = @f_id group by changed_by) order by form_q_score_changes.id ", cn);
                    reply.Parameters.AddWithValue("f_id", ID);
                    adapter.SelectCommand = reply;
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        ScorecardData scd = new ScorecardData();
                        List<SectionData> sections = new List<SectionData>();
                        UserObject scu = new UserObject();
                        scu.UserRole = dr["user_role"].ToString();
                        scu.UserTitle = dr["username"].ToString();
                        CallScores cs = new CallScores();
                        cs.score = Convert.IsDBNull(dr["total_score"]) ? "0" : dr["total_score"].ToString();
                        cs.reviewer = dr["username"].ToString();
                        cs.scoredate = Convert.ToDateTime(dr["review_date"]).ToShortDateString();
                        cs.role = dr["user_role"].ToString();
                        scd.CallScore = cs;
                        scd.ScorecardUser = scu;
                        DataTable section_dt = GetTable("exec getSections2 " + ID);
                        foreach (DataRow section_dr in section_dt.Rows)
                        {
                            SectionData section_data = new SectionData();
                            section_data.SectionTitle = section_dr["section"].ToString();
                            List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                            DataTable qdt = GetTable("getEditorQuestions " + dr["F_ID"].ToString() + ", " + section_dr["ID"] + ",'" + dr["username"] + "'");
                            foreach (DataRow qdr in qdt.Rows)
                            {
                                ScorecardResponse qr = new ScorecardResponse();
                                qr.position = qdr["q_position"].ToString();
                                qr.question = qdr["q_short_name"].ToString();
                                qr.result = qdr["answer_text"].ToString();
                                qr.QID = Convert.ToInt32(qdr["q_id"]);
                                try
                                {
                                    qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                                }
                                catch (Exception ex)
                                {
                                }
                                qr.QType = qdr["q_type"].ToString();
                                qr.ViewLink = qdr["view_link"].ToString();
                                qr.comments_allowed = Convert.ToBoolean(qdr["comments_allowed"]);
                                if (qdr["right_answer"].ToString() == "")
                                    qr.RightAnswer = Convert.ToBoolean(0);
                                else
                                    qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);
                                DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                                if (ans_dt.Rows.Count > 0)
                                {
                                    List<string> ans_comment = new List<string>();
                                    foreach (DataRow ans_dr in ans_dt.Rows)
                                        ans_comment.Add(ans_dr["comment"].ToString());
                                    qr.QComments = ans_comment;
                                }
                                DataTable temp_dt = GetTable("exec getTemplateItems " + ID + "," + qdr["q_id"].ToString());
                                if (temp_dt.Rows.Count > 0)
                                {
                                    List<CheckItems> temp_items = new List<CheckItems>();
                                    foreach (DataRow temp_dr in temp_dt.Rows)
                                    {
                                        CheckItems temp_item = new CheckItems();
                                        if (temp_dr["value"].ToString() == temp_dr["option_value"].ToString())
                                            temp_item.Checked = true;
                                        else
                                            temp_item.Checked = false;
                                        temp_item.Item = temp_dr["value"].ToString();
                                        temp_item.Position = temp_dr["option_pos"].ToString();
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
                }
                scr.ScorecardData = scds;
                dt = new DataTable();
                reply = new SqlCommand("getSchoolDataWithPos @ID,  @xcc_id", cn);
                reply.Parameters.AddWithValue("xcc_id", review_id);
                reply.Parameters.AddWithValue("ID", ID);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                List<SchoolItem> school_items = new List<SchoolItem>();
                foreach (DataRow school_dr in dt.Rows)
                {
                    SchoolItem school_item = new SchoolItem();
                    school_item.AOI1 = school_dr["AOI1"].ToString();
                    school_item.AOI2 = school_dr["AOI2"].ToString();
                    school_item.College = school_dr["College"].ToString();
                    school_item.DegreeOfInterest = school_dr["DegreeOfInterest"].ToString();
                    school_item.L1_SubjectName = school_dr["L1_SubjectName"].ToString();
                    school_item.L2_SubjectName = school_dr["L2_SubjectName"].ToString();
                    school_item.Modality = school_dr["Modality"].ToString();
                    school_item.Portal = school_dr["origin"].ToString();
                    school_item.School = school_dr["School"].ToString();
                    school_item.TCPA = school_dr["TCPA"].ToString();
                    school_items.Add(school_item);
                }
                scr.SchoolData = school_items;
                dt = new DataTable();
                reply = new SqlCommand("exec getotherformdata @xcc_id", cn);
                reply.Parameters.AddWithValue("xcc_id", review_id);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                List<OtherData> otherdata_items = new List<OtherData>();
                foreach (DataRow school_dr in dt.Rows)
                {
                    OtherData otherdata_item = new OtherData();
                    otherdata_item.key = school_dr["data_key"].ToString();
                    otherdata_item.label = school_dr["label"].ToString();
                    otherdata_item.school = school_dr["school_name"].ToString();
                    otherdata_item.type = school_dr["data_type"].ToString();
                    otherdata_item.value = school_dr["data_value"].ToString();

                    otherdata_items.Add(otherdata_item);
                }
                scr.OtherData = otherdata_items;
                dt = new DataTable();
                reply = new SqlCommand("exec getCombinedComments @f_id ", cn);
                reply.Parameters.AddWithValue("f_id", ID);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                List<DisputeData> disputes = new List<DisputeData>();
                foreach (DataRow dispute_dr in dt.Rows)
                {
                    DisputeData dispute = new DisputeData();
                    dispute.closed = dispute_dr["date_closed"].ToString();
                    dispute.comment = dispute_dr["comment"].ToString();
                    dispute.created = dispute_dr["date_created"].ToString();
                    dispute.role = dispute_dr["role"].ToString() + " - " + dispute_dr["closed_by"].ToString();
                    dispute.user = dispute_dr["closed_by"].ToString();
                    dispute.id = Convert.ToInt32(dispute_dr["fn_id"]);

                    disputes.Add(dispute);
                }
                if (disputes.Count > 0)
                    scr.Disputes = disputes;
                Common.UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" + username + "',dbo.getMTDate(), (select review_id from form_score3 with  (nolock)  where id = " + ID + "),'review'");
                dt = new DataTable();
                reply = new SqlCommand("select * from session_viewed with  (nolock) join userextrainfo on userextrainfo.username = agent  where session_id = '" + review_id + "' order by date_viewed", cn);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                List<SessionViews> svs = new List<SessionViews>();
                foreach (DataRow comment_dr in dt.Rows)
                {
                    SessionViews sv = new SessionViews();
                    sv.view_action = comment_dr["page_viewed"].ToString();
                    sv.view_by = comment_dr["agent"].ToString();
                    sv.view_date = comment_dr["date_viewed"].ToString();
                    sv.view_role = comment_dr["user_role"].ToString();
                    svs.Add(sv);
                }
                if (svs.Count > 0)
                    scr.sessions_viewed = svs;
                CDService cdservice = new CDService();
                String strButton = cdservice.GetNotificationSteps(ID);
                List<string> buttons = new List<string>(strButton.Split('|'));
                if (buttons.Count > 0)
                    scr.dispute_buttons = buttons;
                List<ActionButton> abs = cdservice.GetActionButtons(username, ID);
                if (abs.Count > 0)
                    scr.ActionButtons = abs;
                dt = new DataTable();
                reply = new SqlCommand("Select count(*) from spotcheck where f_id = @id and checked_date is not null", cn);
                reply.Parameters.AddWithValue("id", ID);
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    scr.showSpotCheck = false;
                else if (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead"))
                    scr.showSpotCheck = true;
                else
                    scr.showSpotCheck = false;
                cn.Close();
                cn.Dispose();
            }
            return scr;
        }



        /// <summary>
        /// PostRecord
        /// </summary>
        /// <returns></returns>
        public string PostRecord()
        {
            string Message = "";
            string[] xcc_list = new[] { "SESSION_ID", "AGENT", "DISPOSITION", "CAMPAIGN", "ANI", "DNIS", "TIMESTAMP", "TALK_TIME", "CALL_TIME", "HANDLE_TIME", "CALL_TYPE", "LIST_NAME", "leadid", "AGENT_GROUP", "DATE", "Email", "City", "State", "Zip", "Datacapturekey", "Datacapture", "Status", "Program", "Datacapture_Status", "num_of_schools", "Number_of_Schools", "EducationLevel", "HighSchoolGradYear", "DegreeStartTimeframe", "appname", "First_Name", "Last_Name", "address", "phone", "call_date", "audio_link", "profile_id", "AreaOfInterest", "ProgramsOfInterestType", "Citizenship", "DegreeOfInterest", "Gender", "Military", "secondphone", "agent_name", "scorecard", "Notes", "website", "hold_time" };
            string appname = clean_string(HttpContext.Current.Request.QueryString["appname"]);
            string remoteAddr = HttpContext.Current.Request.ServerVariables["remote_addr"].ToString();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {

                string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
                //SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
                flatPost flatPost = new flatPost()
                {
                    raw_data = raw_post,
                    ip_address = remoteAddr,
                };
                dataContext.flatPosts.Add(flatPost);
                int result1 = dataContext.SaveChanges();
                //Common.UpdateTable("exec add_ip '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + appname + "'");
                int insert = dataContext.add_ip(remoteAddr, appname);

                string sql = "declare @xcc_id int; insert into xcc_report_new_pending(";
                string @params = "";
                foreach (var qi in HttpContext.Current.Request.QueryString.Keys)
                {
                    string qii = clean_string(qi.ToString());
                    int indx = xcc_list.ToString().IndexOf(qii);
                    if (indx > -1)
                    {
                        sql += "[" + qi + "],";
                        @params += "@" + qi + ",";
                        //reply.Parameters.AddWithValue(qi.ToString(), clean_string(HttpContext.Current.Request.QueryString[qi.ToString()]));
                    }
                }
                sql = Strings.Left(sql, Strings.Len(sql) - 1) + ")values(" + Strings.Left(@params, Strings.Len(@params) - 1) + "); select @xcc_id = @@identity; select @xcc_id;";
                string new_id = "0";
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                xcc_report_new_pending xccreportnew_pending = new xcc_report_new_pending()
                {
                    DATE = mtdate,
                    LIST_NAME = @params.ToString(),
                };
                dataContext.xcc_report_new_pending.Add(xccreportnew_pending);
                int result = dataContext.SaveChanges();

                foreach (var qi in HttpContext.Current.Request.QueryString.Keys)
                {
                    string qii = clean_string(qi.ToString());
                    int indx = xcc_list.ToString().IndexOf(qii);
                    if (indx == -1)
                    {
                        //Common.UpdateTable("insert into otherFormDataPending(form_id, data_key, data_value) select " + new_id + ",'" + qi.ToString().Replace("'", "''") + "','" + HttpContext.Current.Request.QueryString[qi.ToString()].ToString().Replace("'", "''") + "'");
                        otherFormDataPending formDataPending = new otherFormDataPending()
                        {
                            form_id = Convert.ToInt32(new_id),
                            data_key = qi.ToString().Replace("'", "''"),
                            data_value = HttpContext.Current.Request.QueryString[qi.ToString()].ToString().Replace("'", "''")
                        };
                        dataContext.otherFormDataPendings.Add(formDataPending);
                        int resultAudioDatas = dataContext.SaveChanges();
                        if (resultAudioDatas == 1)
                        {
                            Message = Messages.Insert;
                        }

                    }
                }
            }
            return Message; ;
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
                var userApps = dataContext.UserApps.Where(x => x.username == username).ToList();
                foreach (var item in userApps)
                {
                    var isxcc_report = dataContext.XCC_REPORT_NEW.Where(x => x.SESSION_ID == session_id && x.scorecard == item.user_scorecard).FirstOrDefault();
                    //SqlDataAdapter reply = new SqlDataAdapter("Select * from xcc_report_new where session_id = @session_id And scorecard In (Select user_scorecard from userapps where username = @username)", cn);
                    SessionStatus ss_obj = new SessionStatus();
                    var result = dataContext.vwForms.Where(x => x.review_ID == isxcc_report.ID).ToList();
                    //DataTable vw_dt = GetTable("Select isnull(isnull(edited_score,calib_score),total_score) As theScore from vwForm where review_id = " + dr["id"].ToString());
                    ss_obj.score = "N/A";
                    switch (isxcc_report.MAX_REVIEWS)
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
                    if (isxcc_report.audio_link.ToString() == "")
                    {
                        ss_obj.status = "WAITING/CONVERTING audio";
                    }
                    if (isxcc_report.bad_call.ToString() == "1")
                    {
                        ss_obj.status = "BAD Call/UNABLE To SCORE - " + isxcc_report.bad_call_reason;
                    }
                    objSessionStatus.Add(ss_obj);
                }
            }
            return objSessionStatus;
        }
        #endregion getScore


        /// <summary>
        /// GetScorecardRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public CompleteScorecard GetScorecardRecordID(SimpleID SI)
        {
            CompleteScorecard sc = new CompleteScorecard();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable fid_dt = GetTable("select review_id, scorecard from vwForm where f_id = " + SI.ID);
                int Id = Convert.ToInt32(SI.ID);
                var isform = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                if (isform != null)
                {
                    return sc;
                }
                int scorecard_ID = Convert.ToInt32(isform.scorecard);
                int xcc_id = Convert.ToInt32(isform.review_ID);
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
                var sc_dt = dataContext.scorecards.Where(x => x.id == scorecard_ID && x.appname == add_sql).FirstOrDefault();
                //DataTable sc_dt = GetTable("select * from scorecards where id = " + scorecard_ID + " " + add_sql);
                if (sc_dt != null)
                {
                    sc.ScorecardName = "No data/No authorized.";
                    return sc;
                }
                sc.ScorecardName = sc_dt.short_name;
                sc.Appname = sc_dt.appname;
                sc.Status = sc_dt.scorecard_status;
                sc.Description = sc_dt.description;

                List<WebApi.Models.CallCriteriaAPI.Section> sec_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                //DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
                var isSections = dataContext.Questions.Where(x => x.active == true && x.scorecard_id == scorecard_ID).ToList();
                foreach (var item in isSections)
                {
                    List<Models.CallCriteriaAPI.Question> ques_list = new List<Models.CallCriteriaAPI.Question>();
                    List<string> temp_list = new List<string>();
                    List<Models.CCInternalAPI.TemplateItem> objTemplateItem = new List<Models.CCInternalAPI.TemplateItem>();
                    var section_dt = dataContext.Sections.Where(x => x.id == item.section).ToList();
                    foreach (var item1 in section_dt)
                    {
                        WebApi.Models.CallCriteriaAPI.Section sec = new WebApi.Models.CallCriteriaAPI.Section();
                        sec.section = item1.section1.ToString();
                        sec.description = item1.Descrip.ToString();
                        if (xcc_id != 0)
                        {
                            var q_dt = dataContext.getListenQuestions(item1.id, scorecard_ID, xcc_id, null, "").ToList();
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                            //List<WebApi.Models.CallCriteriaAPI.Section> ques_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
                                ques.QuestionShort = drq.q_short_name;
                                ques.question = drq.question;
                                ques.LinkedAnswer = drq.linked_answer.ToString();
                                ques.LinkedComment = drq.linked_comment.ToString();

                                //ques.QAPoints = Convert.ToInt32(drq["QA_points"]);
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.id);

                                //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq.id);
                                var ans_dt = dataContext.question_answers.Where(x => x.question_id == drq.id).ToList();
                                List<Answer> ans_list = new List<Answer>();
                                foreach (var dra in ans_dt)
                                {
                                    Answer ans = new Answer();
                                    ans.Answers = dra.answer_text;
                                    ans.Points = Convert.ToInt32(dra.answer_points);
                                    ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                    if (dra.autoselect.ToString() == "True")
                                    {
                                        ans.autoselect = 1;
                                    }
                                    else
                                    {
                                        ans.autoselect = 0;
                                    }
                                    ans.AnswerID = Convert.ToInt32(dra.id);
                                    //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                                    var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                    List<Comment> cmt_list = new List<Comment>();

                                    foreach (var drcmt in isanswer_comments)
                                    {
                                        Comment cmt = new Comment();
                                        cmt.CommentText = drcmt.comment.ToString();
                                        cmt.CommentID = Convert.ToInt32(drcmt.id);
                                        cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                                        cmt_list.Add(cmt);
                                    }
                                    ans.Comments = cmt_list;
                                    ans_list.Add(ans);
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.id).ToList();
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;

                                List<FAQ> faq_list = new List<FAQ>();

                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.id).OrderBy(x => x.q_order).ToList();
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
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID);
                            var q_dt = dataContext.getListenQuestions(item1.id, scorecard_ID, null, null, "").ToList();
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                            //List<WebApi.Models.CallCriteriaAPI.Section> ques_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
                                ques.QuestionShort = drq.q_short_name;
                                ques.question = drq.question;
                                ques.LinkedAnswer = drq.linked_answer.ToString();
                                ques.LinkedComment = drq.linked_comment.ToString();

                                //ques.QAPoints = Convert.ToInt32(drq["QA_points"]);
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.id);

                                //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq.id);
                                var ans_dt = dataContext.question_answers.Where(x => x.question_id == drq.id).ToList();
                                List<Answer> ans_list = new List<Answer>();
                                foreach (var dra in ans_dt)
                                {
                                    Answer ans = new Answer();
                                    ans.Answers = dra.answer_text;
                                    ans.Points = Convert.ToInt32(dra.answer_points);
                                    ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                    if (dra.autoselect.ToString() == "True")
                                    {
                                        ans.autoselect = 1;
                                    }
                                    else
                                    {
                                        ans.autoselect = 0;
                                    }
                                    ans.AnswerID = Convert.ToInt32(dra.id);
                                    //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                                    var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                    List<Comment> cmt_list = new List<Comment>();

                                    foreach (var drcmt in isanswer_comments)
                                    {
                                        Comment cmt = new Comment();
                                        cmt.CommentText = drcmt.comment.ToString();
                                        cmt.CommentID = Convert.ToInt32(drcmt.id);
                                        cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                                        cmt_list.Add(cmt);
                                    }
                                    ans.Comments = cmt_list;
                                    ans_list.Add(ans);
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.id).ToList();
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;

                                List<FAQ> faq_list = new List<FAQ>();

                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.id).OrderBy(p => p.q_order).OrderBy(x => x.q_order).ToList();
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
                        //DataTable cq_dt = GetTable("Select  distinct data_key,label, school_name,data_type,data_value from otherformdata where xcc_id = '" + xcc_id + "' and data_type = 'customquestion'"); // 
                        var isotherformdata = dataContext.otherFormDatas.Where(x => x.xcc_id == xcc_id && x.data_type == "customquestion").Select(dt => new { dt.data_key, dt.label, dt.school_name, dt.data_type, dt.data_value }).Distinct().ToList();
                        if (isotherformdata.Count > 0)
                        {
                            WebApi.Models.CallCriteriaAPI.Section sec = new WebApi.Models.CallCriteriaAPI.Section();
                            sec.section = "Custom Questions";
                            sec.description = "Custom Questions";
                            List<WebApi.Models.CallCriteriaAPI.Question> ques_list1 = new List<WebApi.Models.CallCriteriaAPI.Question>();
                            foreach (var cq_dr in isotherformdata)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
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
                    //DataTable clerk_dt = GetTable("Select * from sc_inputs where scorecard = " + scorecard_ID + " And active = 1 order by value_order");
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

        /// <summary>
        /// GetScorecardRecord
        /// </summary>
        /// <param name="gscd"></param>
        /// <returns></returns>
        public CompleteScorecard GetScorecardRecord(getSCRecData gscd)
        {
            int scorecard_ID = 0;
            int xcc_id = 0;
            if (gscd.scorecard_ID != "")
            {
                scorecard_ID = Convert.ToInt32(gscd.scorecard_ID);
                scorecard_ID = Convert.ToInt32(gscd.xcc_id);
            }
            CompleteScorecard sc = new CompleteScorecard();
            if (scorecard_ID == 0)
            {
                return sc;
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
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
                if (sc_dt != null)
                {
                    sc.ScorecardName = "No data/No authorized.";
                    return sc;
                }
                sc.ScorecardName = sc_dt.short_name;
                sc.Appname = sc_dt.appname;
                sc.Status = sc_dt.scorecard_status;
                sc.Description = sc_dt.description;
                //DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
                List<WebApi.Models.CallCriteriaAPI.Section> sec_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                //DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
                var isSections = dataContext.Questions.Where(x => x.active == true && x.scorecard_id == scorecard_ID).ToList();
                foreach (var item in isSections)
                {
                    List<Models.CallCriteriaAPI.Question> ques_list = new List<Models.CallCriteriaAPI.Question>();
                    List<string> temp_list = new List<string>();
                    List<Models.CCInternalAPI.TemplateItem> objTemplateItem = new List<Models.CCInternalAPI.TemplateItem>();
                    var section_dt = dataContext.Sections.Where(x => x.id == item.section).ToList();
                    foreach (var item1 in section_dt)
                    {
                        WebApi.Models.CallCriteriaAPI.Section sec = new WebApi.Models.CallCriteriaAPI.Section();
                        sec.section = item1.section1.ToString();
                        sec.description = item1.Descrip.ToString();
                        if (xcc_id != 0)
                        {
                            var q_dt = dataContext.getListenQuestions(item1.id, scorecard_ID, xcc_id, null, "").ToList();
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                            //List<WebApi.Models.CallCriteriaAPI.Section> ques_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
                                ques.QuestionShort = drq.q_short_name;
                                ques.question = drq.question;
                                ques.LinkedAnswer = drq.linked_answer.ToString();
                                ques.LinkedComment = drq.linked_comment.ToString();

                                //ques.QAPoints = Convert.ToInt32(drq["QA_points"]);
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.id);

                                //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq.id);
                                var ans_dt = dataContext.question_answers.Where(x => x.question_id == drq.id).ToList();
                                List<Answer> ans_list = new List<Answer>();
                                foreach (var dra in ans_dt)
                                {
                                    Answer ans = new Answer();
                                    ans.Answers = dra.answer_text;
                                    ans.Points = Convert.ToInt32(dra.answer_points);
                                    ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                    if (dra.autoselect.ToString() == "True")
                                    {
                                        ans.autoselect = 1;
                                    }
                                    else
                                    {
                                        ans.autoselect = 0;
                                    }
                                    ans.AnswerID = Convert.ToInt32(dra.id);
                                    //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                                    var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                    List<Comment> cmt_list = new List<Comment>();

                                    foreach (var drcmt in isanswer_comments)
                                    {
                                        Comment cmt = new Comment();
                                        cmt.CommentText = drcmt.comment.ToString();
                                        cmt.CommentID = Convert.ToInt32(drcmt.id);
                                        cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                                        cmt_list.Add(cmt);
                                    }
                                    ans.Comments = cmt_list;
                                    ans_list.Add(ans);
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.id).ToList();
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;

                                List<FAQ> faq_list = new List<FAQ>();

                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.id).OrderBy(p => p.q_order).OrderBy(x => x.q_order).ToList();
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
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID);
                            var q_dt = dataContext.getListenQuestions(item1.id, scorecard_ID, null, null, "").ToList();
                            //q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                            //List<WebApi.Models.CallCriteriaAPI.Section> ques_list = new List<WebApi.Models.CallCriteriaAPI.Section>();
                            foreach (var drq in q_dt)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
                                ques.QuestionShort = drq.q_short_name;
                                ques.question = drq.question;
                                ques.LinkedAnswer = drq.linked_answer.ToString();
                                ques.LinkedComment = drq.linked_comment.ToString();

                                //ques.QAPoints = Convert.ToInt32(drq["QA_points"]);
                                ques.comments_allowed = Convert.ToBoolean(drq.comments_allowed);
                                ques.QID = Convert.ToInt32(drq.id);

                                //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq.id);
                                var ans_dt = dataContext.question_answers.Where(x => x.question_id == drq.id).ToList();
                                List<Answer> ans_list = new List<Answer>();
                                foreach (var dra in ans_dt)
                                {
                                    Answer ans = new Answer();
                                    ans.Answers = dra.answer_text;
                                    ans.Points = Convert.ToInt32(dra.answer_points);
                                    ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                    if (dra.autoselect.ToString() == "True")
                                    {
                                        ans.autoselect = 1;
                                    }
                                    else
                                    {
                                        ans.autoselect = 0;
                                    }
                                    ans.AnswerID = Convert.ToInt32(dra.id);
                                    //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                                    var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                    List<Comment> cmt_list = new List<Comment>();

                                    foreach (var drcmt in isanswer_comments)
                                    {
                                        Comment cmt = new Comment();
                                        cmt.CommentText = drcmt.comment.ToString();
                                        cmt.CommentID = Convert.ToInt32(drcmt.id);
                                        cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                                        cmt_list.Add(cmt);
                                    }
                                    ans.Comments = cmt_list;
                                    ans_list.Add(ans);
                                }
                                ques.answers = ans_list;
                                List<Instruction> instr_list = new List<Instruction>();
                                //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                                var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.id).ToList();
                                foreach (var drinst in isq_instructions)
                                {
                                    Instruction instr = new Instruction();
                                    instr.InstructionText = drinst.question_text;
                                    instr_list.Add(instr);
                                }
                                ques.instructions = instr_list;

                                List<FAQ> faq_list = new List<FAQ>();

                                //DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                                var isq_faqs = dataContext.q_faqs.Where(x => x.question_id == drq.id).OrderBy(p => p.q_order).OrderBy(x => x.q_order).ToList();
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
                        var isotherformdata = dataContext.otherFormDatas.Where(x => x.xcc_id == xcc_id && x.data_type == "customquestion").Select(dt => new { dt.data_key, dt.label, dt.school_name, dt.data_type, dt.data_value }).Distinct().ToList();
                        if (isotherformdata.Count > 0)
                        {
                            WebApi.Models.CallCriteriaAPI.Section sec = new WebApi.Models.CallCriteriaAPI.Section();
                            sec.section = "Custom Questions";
                            sec.description = "Custom Questions";
                            List<WebApi.Models.CallCriteriaAPI.Question> ques_list1 = new List<WebApi.Models.CallCriteriaAPI.Question>();
                            foreach (var cq_dr in isotherformdata)
                            {
                                WebApi.Models.CallCriteriaAPI.Question ques = new WebApi.Models.CallCriteriaAPI.Question();
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
                    //DataTable clerk_dt = GetTable("Select * from sc_inputs where scorecard = " + scorecard_ID + " And active = 1 order by value_order");
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

        /// <summary>
        /// GetScorecardRecordJ
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public CompleteScorecard GetScorecardRecordJ(GCR SI)
        {
            int scorecard_ID=0;
            CompleteScorecard sc = new CompleteScorecard();
            if (SI.scorecard_ID == "")
            {
                return sc;
            }
            else
            {
                scorecard_ID =Convert.ToInt32(SI.scorecard_ID);
            }
            List<Models.CallCriteriaAPI.Section> sec_list = new List<Models.CallCriteriaAPI.Section>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
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
                var sc_dt = dataContext.scorecards.Where(x => x.id == scorecard_ID && x.appname == add_sql).FirstOrDefault();
                //DataTable sc_dt = GetTable("Select * from scorecards where id = " + scorecard_ID + " " + add_sql);
                if (sc_dt !=null)
                {
                    sc.ScorecardName = sc_dt.short_name;
                }
                //DataTable section_dt = GetTable("Select sections.ID, sections.section, Descrip from sections where id In (Select  section from questions  where scorecard_id = " + scorecard_ID + " And questions.active = 1) And  scorecard_id = " + scorecard_ID + " order by section_order");
                var isSections = dataContext.Questions.Where(x => x.active == true && x.scorecard_id == scorecard_ID).ToList();
                foreach (var item in isSections)
                {
                    List<Models.CallCriteriaAPI.Question> ques_list = new List<Models.CallCriteriaAPI.Question>();
                    List<string> temp_list = new List<string>();
                    List<Models.CCInternalAPI.TemplateItem> objTemplateItem = new List<Models.CCInternalAPI.TemplateItem>();
                    var section_dt = dataContext.Sections.Where(x => x.id == item.section && x.scorecard_id == scorecard_ID).OrderBy(x => x.section_order).ToList();
                    foreach (var item1 in section_dt)
                    {

                        Models.CallCriteriaAPI.Section sec = new Models.CallCriteriaAPI.Section();
                        sec.section = item1.section1;
                        sec.description = item1.Descrip;
                        //DataTable q_dt = GetTable("Select q_short_name, q.question,  q.ID FROM [Questions] q  join sections On sections.id  = q.section where  q.scorecard_id = " + scorecard_ID + " And q.section = " + dr["id"].ToString() + " And q.active = 1 order by q_order");


                        var questionsSections = (from questions in dataContext.Questions
                                                join section in dataContext.Sections on questions.section equals section.id
                                                where questions.scorecard_id == scorecard_ID && questions.section == item1.id && questions.active ==true
                                                 select new { questions.q_short_name, questions.question1, questions.id }).ToList();

                        foreach (var drq in questionsSections)
                        {
                            Models.CallCriteriaAPI.Question ques = new Models.CallCriteriaAPI.Question();
                            ques.QuestionShort = drq.q_short_name;
                            ques.question = drq.question1;
                            ques.QID = Convert.ToInt32(drq.id);
                            //DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq.id);
                            var ans_dt = dataContext.question_answers.Where(x => x.question_id == drq.id).ToList();
                            List<Answer> ans_list = new List<Answer>();
                            foreach (var dra in ans_dt)
                            {
                                Answer ans = new Answer();
                                ans.Answers = dra.answer_text;
                                ans.Points = Convert.ToInt32(dra.answer_points);
                                ans.RightAnswer = Convert.ToBoolean(dra.right_answer);
                                if (dra.autoselect.ToString() == "True")
                                {
                                    ans.autoselect = 1;
                                }
                                else
                                {
                                    ans.autoselect = 0;
                                }
                                ans.AnswerID = dra.id;
                                //DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                                var isanswer_comments = dataContext.answer_comments.Where(x => x.answer_id == dra.id).OrderBy(p => p.ac_order == null || p.ac_order == 10000).ToList();
                                List<Comment> cmt_list = new List<Comment>();

                                foreach (var drcmt in isanswer_comments)
                                {
                                    Comment cmt = new Comment();
                                    cmt.CommentText = drcmt.comment.ToString();
                                    cmt.CommentID = Convert.ToInt32(drcmt.id);
                                    cmt.CommentPoints = Convert.ToInt32(drcmt.comment_points);
                                    cmt_list.Add(cmt);
                                }
                                ans.Comments = cmt_list;
                                ans_list.Add(ans);
                            }
                            ques.answers = ans_list;
                           
                            List<Instruction> instr_list = new List<Instruction>();
                            var isq_instructions = dataContext.q_instructions.Where(x => x.question_id == drq.id).ToList();
                            //DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                            foreach (var drinst in isq_instructions)
                            {
                                Instruction instr = new Instruction();
                                instr.InstructionText = drinst.question_text.ToString();
                                instr_list.Add(instr);
                            }
                            ques.instructions = instr_list;
                            List<FAQ> faq_list = new List<FAQ>();
                            var faq_dt = dataContext.q_faqs.Where(x => x.question_id == drq.id).OrderBy(p => p.q_order).ToList();
                            foreach (var drfaq in faq_dt)
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
                        sec_list.Add(sec);
                    }
                }
            }
            sc.Sections = sec_list;
            return sc;
        }

        /// <summary>
        /// clean_string
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
    }
}

