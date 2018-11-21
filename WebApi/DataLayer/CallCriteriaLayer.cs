using Microsoft.VisualBasic;
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
    public class CallCriteriaLayer
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
                    ObjectResult<GetAllRecordsWithPending_Result> objGetAllRecords = dataContext.GetAllRecordsWithPending(call_date, "revlive", useReview);

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

                    objCallRecord.AddRange(SubCallRecord);
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
                            scr.ScorecardResponses = new List<ScorecardResponse>();
                            scr.ScorecardResponses.AddRange(ScoreResponse);
                            if (ScoreResponse != null)
                            {
                                foreach (var item in ScoreResponse)
                                {
                                    //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                                    var formResponses = (from formresponses in dataContext.form_q_responses
                                                         join answercomments in dataContext.answer_comments on formresponses.answer_id equals answercomments.id
                                                         where formresponses.form_id == FId && formresponses.question_id == item.QID
                                                         select new { answercomments.comment, formresponses.other_answer_text, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                    if (formResponses.Count > 0)
                                    {
                                        List<string> ans_comment = new List<string>();
                                        foreach (var ans_dr in formResponses)
                                        {
                                            ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
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
                    var xccReportNew = dataContext.XCC_REPORT_NEW.Where(x => x.call_date == loadedDate && x.appname == "callsource").ToList();
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
            if (SI.ID != "")
            {
                ID = Convert.ToInt32(SI.ID);
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

                string AppName = null;
                string userName = null;
                if (HttpContext.Current.Request.QueryString["appname"] != null)
                {
                    AppName = HttpContext.Current.Request["appname"];
                }
                else
                {
                    userName = username;
                }
                //reply = new SqlCommand("select vwForm.*,isnull(client_logo, vwForm.appname) as client_logo, (select total_score from vwCF where active_cali = 1 and f_id = @id) as qa_cali_score  from vwForm join app_settings on vwForm.appname = app_settings.appname where f_id = @id" + add_sql, cn);
                var getvwForm = dataContext.getvwForm(ID, "vs", userName).ToList();

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
                    if (dr.wasEdited == true)
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
                        //dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where (WAV_DATA.session_id = '" + dr["session_id"] + "')  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");
                        string SESSIONId = dr.SESSION_ID;
                        var waVDATA = (from wAVDATA in dataContext.WAV_DATA
                                       join appSettings in dataContext.app_settings on wAVDATA.appname equals appSettings.appname
                                       where wAVDATA.session_id == SESSIONId && appSettings.appname == dr.appname && wAVDATA.appname == dr.appname
                                       select new
                                       {
                                           wAVDATA.ID,
                                           wAVDATA.filename,
                                           appSettings.url_prefix,
                                           appSettings.recording_user,
                                           appSettings.record_password,
                                           wAVDATA.session_id
                                       }).OrderBy(x => new { x.filename, x.ID, x.url_prefix, x.recording_user, x.record_password, x.session_id }).ToList();
                        if (waVDATA.Count > 0)
                        {
                            foreach (var audio_dr in waVDATA)
                            {

                                //audio_list.Add(ReverseMapPath(audio_dr.filename));
                            }
                        }
                    }
                    else
                    {
                        //dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where ((WAV_DATA.session_id = '" + dr["session_id"] + "') or (WAV_DATA.filename like  '%" + dr["phone"] + "%' ))  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");
                        string SESSIONId = dr.SESSION_ID;
                        var waVDATA = (from wAVDATA in dataContext.WAV_DATA
                                       join appSettings in dataContext.app_settings on wAVDATA.appname equals appSettings.appname
                                       where (wAVDATA.session_id == SESSIONId || wAVDATA.filename == dr.phone) && appSettings.appname == dr.appname && wAVDATA.appname == dr.appname
                                       select new
                                       {
                                           wAVDATA.ID,
                                           wAVDATA.filename,
                                           appSettings.url_prefix,
                                           appSettings.recording_user,
                                           appSettings.record_password,
                                           wAVDATA.session_id
                                       }).OrderBy(x => new { x.filename, x.ID, x.url_prefix, x.recording_user, x.record_password, x.session_id }).ToList();
                        if (waVDATA.Count > 0)
                        {
                            foreach (var audio_dr in waVDATA)
                            {

                                //audio_list.Add(ReverseMapPath(audio_dr.filename));
                            }
                        }
                    }
                    var audioData = dataContext.AudioDatas.Where(x => x.final_xcc_id == review_id).ToList();
                    //dt = GetTable("select * from audiodata where final_xcc_id = " + review_id);
                    if (audioData.Count > 0)
                    {
                        foreach (var audio_dr in audioData)
                        {
                            //audio_list.Add(ReverseMapPath(audio_dr.file_name));
                        }
                    }
                    scr.audio_merge = audio_list;
                    var section_dt = dataContext.getSections2(ID).ToList();
                    //DataTable section_dt = GetTable("exec getSections2 " + ID);
                    List<SectionData> sections = new List<SectionData>();
                    foreach (var section_dr in section_dt)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr.section;
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();

                        //DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + dr["F_ID"].ToString() + ", " + section_dr["ID"].ToString() + ") left join (Select q_position, answer_text, form_q_scores.question_id, right_answer, view_link from form_q_scores join question_answers On question_answers.ID = form_q_scores.original_question_answered where form_id = " + dr["F_ID"].ToString() + ") a On  a.question_id = q_id  join questions On questions.ID = q_id  where active = 1 order by all_q_order");
                        var isProAllClientQuestions = dataContext.getProAllClientQuestions(dr.F_ID, section_dr.ID, Convert.ToInt32(ID), username).ToList();
                        foreach (var qdr in isProAllClientQuestions)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr.q_position;
                            qr.question = qdr.q_short_name;
                            qr.result = qdr.answer_text;
                            qr.QID = Convert.ToInt32(qdr.q_id);
                            if (qdr.QA_points != null)
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
                            if (qdr.right_answer == null)
                            {
                                qr.RightAnswer = Convert.ToBoolean(0);
                            }
                            else
                            {
                                qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                            }
                            //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id " +
                            // "where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            var objCompanieML = (from formresponses in dataContext.form_q_responses
                                                 join answercomments in dataContext.answer_comments on formresponses.answer_id equals answercomments.id into SJ
                                                 from answercomments in SJ.DefaultIfEmpty()
                                                 where formresponses.form_id == dr.F_ID && formresponses.question_id == qdr.q_id
                                                 select new { answercomments.comment, formresponses.other_answer_text, answercomments.comment_points, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                            if (objCompanieML.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (var ans_dr in objCompanieML)
                                {
                                    ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                }
                                qr.QComments = ans_comment;
                            }
                            //DataTable temp_dt = GetTable("exec getTemplateItems " + ID + "," + qdr["q_id"].ToString());
                            var temp_dt = dataContext.getCTemplateItems(ID, qdr.q_id, null).ToList();
                            if (temp_dt.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (var temp_dr in temp_dt)
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
                //dt = new DataTable();
                //reply = new SqlCommand("Select *, (select recal_score from vwCF where isrecal = 1 and f_id = @id) as cal_recal_score from vwCF join userextrainfo On userextrainfo.username = reviewed_by where f_id = @id", cn);
                var dt = dataContext.GetuserexTrainfo(ID).ToList();
                foreach (var dr in dt)
                {
                    ScorecardData scd = new ScorecardData();
                    List<SectionData> sections = new List<SectionData>();
                    UserObject scu = new UserObject();
                    scu.UserRole = dr.user_role.ToString();
                    scu.UserTitle = dr.user_role + " Response";
                    scd.ScorecardUser = scu;
                    CallScores cs = new CallScores();
                    cs.score = dr.cali_form_score.ToString();
                    cs.reviewer = dr.reviewed_by;
                    if (dr.user_role == "Calibrator")
                    {
                        cs.calibrationscore = dr.cal_recal_score.ToString();
                    }
                    else
                    {
                        cs.calibrationscore = dr.total_score.ToString();
                    }
                    cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                    cs.role = dr.user_role;
                    scd.CallScore = cs;
                    //DataTable section_dt = GetTable("exec getSections2 " + ID);
                    var section_dt = dataContext.getSections2(ID).ToList();
                    foreach (var section_dr in section_dt)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr.section;
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();

                        //DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + ID + "," + section_dr["ID"] + ") left join (select  q_pos, answer_text, calibration_scores.question_id, right_answer, view_link from calibration_scores join question_answers On question_answers.ID = calibration_scores.question_result where form_id = " + dr["ID"].ToString() + ") a on a.question_id = q_id join questions On questions.ID = q_id where active = 1 order by all_q_order");
                        var qdt = dataContext.getAllClientQuestionsReview(ID, dr.id, section_dr.ID, null).ToList();
                        foreach (var qdr in qdt)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr.q_pos.ToString();
                            qr.question = qdr.q_short_name.ToString();
                            qr.result = qdr.answer_text.ToString();
                            qr.QID = Convert.ToInt32(qdr.q_id);
                            qr.QType = qdr.q_type;
                            if (qdr.QA_points != null)
                            {
                                qr.QAPoints = Convert.ToInt32(qdr.QA_points);
                            }
                            else
                            {
                                qr.QAPoints = 0;
                            }
                            qr.ViewLink = qdr.view_link;
                            qr.comments_allowed = Convert.ToBoolean(qdr.comments_allowed);
                            if (qdr.right_answer.ToString() == "")
                            {
                                qr.RightAnswer = Convert.ToBoolean(0);
                            }
                            else
                            {
                                qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                            }
                            //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As q_comment, comment_points from form_c_responses left join answer_comments On form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = " + dr.ID.ToString() + " And form_c_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            var objCompanieML = (from form_cResponses in dataContext.form_c_responses
                                                 join answercomments in dataContext.answer_comments on form_cResponses.answer_id equals answercomments.id into SJ
                                                 from answercomments in SJ.DefaultIfEmpty()
                                                 where form_cResponses.form_id == dr.id && form_cResponses.question_id == qdr.q_id
                                                 select new { answercomments.comment, form_cResponses.other_answer_text, answercomments.comment_points, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                            if (objCompanieML.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (var ans_dr in objCompanieML)
                                {
                                    ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                }
                                qr.QComments = ans_comment;
                            }
                            //DataTable temp_dt = GetTable("exec getCTemplateItems " + dr["ID"].ToString() + "," + qdr["q_id"].ToString());
                            var temp_dt = dataContext.getCTemplateItems(dr.id, qdr.q_id, null).ToList();
                            if (temp_dt.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (var temp_dr in temp_dt)
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
                //reply = new SqlCommand("Select * from calibration_form_client join userextrainfo On userextrainfo.username = reviewed_by where original_form = @id", cn);
                var calibrationClient = (from calibrationclient in dataContext.calibration_form_client
                                         join UserExtra in dataContext.UserExtraInfoes on calibrationclient.reviewed_by equals UserExtra.username
                                         where calibrationclient.original_form == ID
                                         select new { calibrationclient.id, UserExtra.user_role, calibrationclient.reviewed_by, calibrationclient.total_score, calibrationclient.review_date }).ToList();

                foreach (var dr in calibrationClient)
                {
                    ScorecardData scd = new ScorecardData();
                    List<SectionData> sections = new List<SectionData>();
                    UserObject scu = new UserObject();
                    scu.UserRole = dr.user_role + " - Client Calib";
                    scu.UserTitle = dr.reviewed_by;
                    CallScores cs = new CallScores();
                    cs.score = Convert.IsDBNull(dr.total_score) ? "0" : dr.total_score.ToString();
                    cs.reviewer = dr.reviewed_by;
                    cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                    cs.role = dr.user_role;
                    scd.CallScore = cs;
                    scd.ScorecardUser = scu;
                    //DataTable section_dt = GetTable("exec getSections2 " + ID);
                    var section_dt = dataContext.getSections2(ID).ToList();
                    foreach (var section_dr in section_dt)
                    {
                        SectionData section_data = new SectionData();
                        section_data.SectionTitle = section_dr.section;
                        List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                        //DataTable qdt = GetTable("Select * from  dbo.[getAllClientQuestions](" + ID + "," + section_dr["ID"] + ") left join (select  q_pos, answer_text, calibration_scores_client.question_id, right_answer, view_link from calibration_scores_client join question_answers On question_answers.ID = calibration_scores_client.question_result where form_id = " + dr["ID"].ToString() + ") a on a.question_id = q_id join questions On questions.ID = q_id where active = 1 order by all_q_order");
                        var qdt = dataContext.getAllClientQuestionsReview(ID, dr.id, section_dr.ID, null).ToList();
                        foreach (var qdr in qdt)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            qr.position = qdr.q_pos.ToString();
                            qr.question = qdr.q_short_name;
                            qr.result = qdr.answer_text;
                            qr.QID = Convert.ToInt32(qdr.q_id);
                            qr.QType = qdr.q_type;
                            if (qdr.QA_points != null)
                            {
                                qr.QAPoints = Convert.ToInt32(qdr.QA_points);
                            }
                            else
                            {
                                qr.QAPoints = 0;
                            }
                            qr.ViewLink = qdr.view_link;
                            qr.comments_allowed = Convert.ToBoolean(qdr.comments_allowed);
                            if (qdr.right_answer.ToString() == "")
                            {
                                qr.RightAnswer = Convert.ToBoolean(0);
                            }
                            else
                            {
                                qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                            }
                            //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As q_comment from form_c_responses_client left join answer_comments On form_c_responses_client.answer_id = answer_comments.id where form_c_responses_client.form_id = " + dr["ID"].ToString() + " And form_c_responses_client.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                            var formresponsesClient = (from form_pResponses in dataContext.form_c_responses_client
                                                       join answercomments in dataContext.answer_comments on form_pResponses.answer_id equals answercomments.id into SJ
                                                       from answercomments in SJ.DefaultIfEmpty()
                                                       where form_pResponses.form_id == dr.id && form_pResponses.question_id == qdr.q_id
                                                       select new { answercomments.comment, form_pResponses.other_answer_text, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();

                            if (formresponsesClient.Count > 0)
                            {
                                List<string> ans_comment = new List<string>();
                                foreach (var ans_dr in formresponsesClient)
                                {
                                    ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                    qr.QComments = ans_comment;
                                }
                            }
                            //DataTable temp_dt = GetTable("exec getCTemplateItems " + dr["ID"].ToString() + "," + qdr["q_id"].ToString() + ", 1");
                            var temp_dt = dataContext.getCTemplateItems(dr.id, qdr.q_id, 1).ToList();
                            if (temp_dt.Count > 0)
                            {
                                List<CheckItems> temp_items = new List<CheckItems>();
                                foreach (var temp_dr in temp_dt)
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
                    //reply = new SqlCommand("select changed_date as review_date, changed_by, user_role, username, isnull(edited_score, (select total_score from vwForm where f_id = @f_id)) as total_score, form_id as f_id from form_q_score_changes join userextrainfo on changed_by = username where form_q_score_changes.id in (select max(id) from form_q_score_changes where form_id = @f_id group by changed_by) order by form_q_score_changes.id ", cn);
                    var GetformQScoreChanges = dataContext.GetformQScoreChanges(ID).ToList();
                    foreach (var dr in GetformQScoreChanges)
                    {
                        ScorecardData scd = new ScorecardData();
                        List<SectionData> sections = new List<SectionData>();
                        UserObject scu = new UserObject();
                        scu.UserRole = dr.user_role;
                        scu.UserTitle = dr.username;
                        CallScores cs = new CallScores();
                        cs.score = dr.total_score.ToString();
                        cs.reviewer = dr.username;
                        cs.scoredate = Convert.ToDateTime(dr.review_date).ToShortDateString();
                        cs.role = dr.user_role;
                        scd.CallScore = cs;
                        scd.ScorecardUser = scu;
                        //DataTable section_dt = GetTable("exec getSections2 " + ID);
                        var section_dt = dataContext.getSections2(ID).ToList();
                        foreach (var section_dr in section_dt)
                        {
                            SectionData section_data = new SectionData();
                            section_data.SectionTitle = section_dr.section;
                            List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                            //DataTable qdt = GetTable("getEditorQuestions " + dr["F_ID"].ToString() + ", " + section_dr["ID"] + ",'" + dr["username"] + "'");
                            var qdt = dataContext.getEditorQuestions(dr.f_id, section_dr.ID, dr.username).ToList();
                            foreach (var qdr in qdt)
                            {
                                ScorecardResponse qr = new ScorecardResponse();
                                qr.position = qdr.q_position;
                                qr.question = qdr.q_short_name;
                                qr.result = qdr.answer_text;
                                qr.QID = qdr.q_id;
                                if (qdr.QA_points != null)
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
                                    qr.RightAnswer = Convert.ToBoolean(0);
                                }
                                else
                                {
                                    qr.RightAnswer = Convert.ToBoolean(qdr.right_answer);
                                }
                                //DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                                var objCompanieML = (from form_pResponses in dataContext.form_q_responses
                                                     join answercomments in dataContext.answer_comments on form_pResponses.answer_id equals answercomments.id into SJ
                                                     from answercomments in SJ.DefaultIfEmpty()
                                                     where form_pResponses.form_id == dr.f_id && form_pResponses.question_id == qdr.q_id
                                                     select new { answercomments.comment, form_pResponses.other_answer_text, answercomments.comment_points, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                if (objCompanieML.Count > 0)
                                {
                                    List<string> ans_comment = new List<string>();
                                    foreach (var ans_dr in objCompanieML)
                                    {
                                        ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                        qr.QComments = ans_comment;
                                    }

                                }
                                //DataTable temp_dt = GetTable("exec getTemplateItems " + ID + "," + qdr["q_id"].ToString());
                                var temp_dt = dataContext.getCTemplateItems(ID, qdr.q_id, null).ToList();
                                if (temp_dt.Count > 0)
                                {
                                    List<CheckItems> temp_items = new List<CheckItems>();
                                    foreach (var temp_dr in temp_dt)
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
                //reply = new SqlCommand("getSchoolDataWithPos @ID,  @xcc_id", cn);
                var resultSchoolDataWithPos = dataContext.getSchoolDataWithPos(ID, review_id).ToList();
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
                    school_items.Add(school_item);
                }
                scr.SchoolData = school_items;
                //reply = new SqlCommand("exec getotherformdata @xcc_id", cn);
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
                    otherdata_items.Add(otherdata_item);
                }
                scr.OtherData = otherdata_items;
                //reply = new SqlCommand("exec getCombinedComments @f_id ", cn);
                var isCombinedComments = dataContext.getCombinedComments(ID, null).ToList();
                List<DisputeData> disputes = new List<DisputeData>();
                foreach (var dispute_dr in isCombinedComments)
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
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                if (disputes.Count > 0)
                {
                    scr.Disputes = disputes;
                }
                //Common.UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" + username + "',dbo.getMTDate(), (select review_id from form_score3 with  (nolock)  where id = " + ID + "),'review'");
                var resultform_score = dataContext.form_score3.Where(x => x.id == ID).FirstOrDefault();
                session_viewed objsession_viewed = new session_viewed();
                if (resultform_score != null)
                {
                    int sessionId = Convert.ToInt32(resultform_score.session_id);
                    // Save form_score3 data
                    objsession_viewed.session_id = sessionId;
                    objsession_viewed.page_viewed = "review";
                    objsession_viewed.date_viewed = mtdate;
                    objsession_viewed.agent = username;
                }
                dataContext.session_viewed.Add(objsession_viewed);
                int result1 = dataContext.SaveChanges();

                //reply = new SqlCommand("select * from session_viewed with  (nolock) join userextrainfo on userextrainfo.username = agent  where session_id = '" + review_id + "' order by date_viewed", cn);
                var clerksession_viewed = (from session_viewed in dataContext.session_viewed
                                           join userExtraInfoes in dataContext.UserExtraInfoes on session_viewed.agent equals userExtraInfoes.username
                                           where session_viewed.session_id == review_id
                                           select new SessionViews
                                           {
                                               view_action = session_viewed.page_viewed,
                                               view_by = session_viewed.agent,
                                               view_date = session_viewed.date_viewed.ToString(),
                                               view_role = userExtraInfoes.user_role,
                                               date_viewed = session_viewed.date_viewed
                                           }).OrderBy(x => x.date_viewed).ToList();
                scr.sessions_viewed = new List<SessionViews>();
                scr.sessions_viewed.AddRange(clerksession_viewed);
                if (scr.sessions_viewed.Count > 0)
                {
                    scr.sessions_viewed = clerksession_viewed;
                }
                //CDService cdservice = new CDService();
                //String strButton = cdservice.GetNotificationSteps(ID);
                //List<string> buttons = new List<string>(strButton.Split('|'));
                //if (buttons.Count > 0)
                //{
                //    scr.dispute_buttons = buttons;
                //}
                //List<Models.CDService.ActionButton> abs = cdservice.GetActionButtons(username, ID);
                //if (abs.Count > 0)
                //{
                //    scr.ActionButtons = abs;
                //}
                //reply = new SqlCommand("Select count(*) from spotcheck where f_id = @id and checked_date is not null", cn);
                var spotcheck = dataContext.spotchecks.Where(x => x.f_id == ID && x.checked_date != null).Count();
                if (spotcheck > 0)
                {
                    scr.showSpotCheck = false;
                }
                else if (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead"))
                {
                    scr.showSpotCheck = true;
                }
                else
                {
                    scr.showSpotCheck = false;
                }

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

                string page = HttpContext.Current.Request.QueryString.ToString();
                //string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
                //SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
                flatPost flatPost = new flatPost()
                {
                    raw_data = page,
                    ip_address = remoteAddr,
                };
                dataContext.flatPosts.Add(flatPost);
                int result1 = dataContext.SaveChanges();
                //Common.UpdateTable("exec add_ip '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + appname + "'");
                int insert = dataContext.add_ip(remoteAddr, appname);

                string sql = "declare @xcc_id int; insert into xcc_report_new_pending(";
                string @params = "";

                Dictionary<string, string> xccRpt = new Dictionary<string, string>();
                foreach (var qi in HttpContext.Current.Request.QueryString.Keys)
                {
                    string qii = clean_string(qi.ToString());
                    int indx = xcc_list.ToString().IndexOf(qii);
                    if (indx > -1)
                    {
                        //sql += "[" + qi + "],";
                        //@params += "@" + qi + ",";
                        xccRpt.Add(qi.ToString(), qi.ToString());
                        //reply.Parameters.AddWithValue(qi.ToString(), clean_string(HttpContext.Current.Request.QueryString[qi.ToString()]));
                    }
                }

                XCCNewReport xcrpt = new XCCNewReport();
                xcrpt.SessionId = xccRpt.ContainsKey("SESSION_ID") ? xccRpt["SESSION_ID"] : string.Empty;
                xcrpt.Agent = xccRpt.ContainsKey("AGENT") ? xccRpt["AGENT"] : string.Empty;
                xcrpt.DisPosition = xccRpt.ContainsKey("DISPOSITION") ? xccRpt["DISPOSITION"] : string.Empty;
                xcrpt.Campaign = xccRpt.ContainsKey("CAMPAIGN") ? xccRpt["CAMPAIGN"] : string.Empty;
                xcrpt.Ani = xccRpt.ContainsKey("ANI") ? xccRpt["ANI"] : string.Empty;
                xcrpt.Dnis = xccRpt.ContainsKey("DNIS") ? xccRpt["DNIS"] : string.Empty;
                xcrpt.TimeStamp = xccRpt.ContainsKey("TIMESTAMP") ? xccRpt["TIMESTAMP"] : string.Empty;
                xcrpt.TalkTime = xccRpt.ContainsKey("TALK_TIME") ? xccRpt["TALK_TIME"] : string.Empty;
                xcrpt.HandleTime = xccRpt.ContainsKey("HANDLE_TIME") ? xccRpt["HANDLE_TIME"] : string.Empty;
                xcrpt.CallType = xccRpt.ContainsKey("CALL_TYPE") ? xccRpt["CALL_TYPE"] : string.Empty;
                xcrpt.ListName = xccRpt.ContainsKey("LIST_NAME") ? xccRpt["LIST_NAME"] : string.Empty;
                xcrpt.Leadid = xccRpt.ContainsKey("leadid") ? xccRpt["leadid"] : string.Empty;
                xcrpt.AgentGroup = xccRpt.ContainsKey("AGENT_GROUP") ? xccRpt["AGENT_GROUP"] : string.Empty;
                xcrpt.Date = Convert.ToDateTime(xccRpt.ContainsKey("DATE") ? xccRpt["DATE"] : string.Empty);
                xcrpt.Email = xccRpt.ContainsKey("Email") ? xccRpt["Email"] : string.Empty;
                xcrpt.City = xccRpt.ContainsKey("City") ? xccRpt["City"] : string.Empty;
                sql = Strings.Left(sql, Strings.Len(sql) - 1) + ")values(" + Strings.Left(@params, Strings.Len(@params) - 1) + "); select @xcc_id = @@identity; select @xcc_id;";
                string new_id = "0";
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                xcc_report_new_pending xccreportnew_pending = new xcc_report_new_pending()
                {
                    DATE = mtdate,
                    SESSION_ID = xcrpt.SessionId,
                    AGENT = xcrpt.Agent,
                    DISPOSITION = xcrpt.DisPosition,
                    CAMPAIGN = xcrpt.Campaign,
                    ANI = xcrpt.Ani,
                    DNIS = xcrpt.Dnis,
                    TIMESTAMP = xcrpt.TimeStamp,
                    TALK_TIME = xcrpt.TalkTime,
                    HANDLE_TIME = xcrpt.HandleTime,
                    CALL_TYPE = xcrpt.CallType,
                    LIST_NAME = xcrpt.ListName,
                    leadid = xcrpt.Leadid,
                    AGENT_GROUP = xcrpt.AgentGroup,
                    Email = xcrpt.Email,
                    City = xcrpt.City,
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

        /// <summary>
        /// GetRecord
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<CallRecord> GetRecord(SimpleID SI)
        {
            int SESSION_ID = 0;
            if (SI.ID != "")
            {
                SESSION_ID = Convert.ToInt32(SI.ID);
            }
            List<CallRecord> objCallRecord = new List<CallRecord>();

            if (SESSION_ID == 0)
            {
                return objCallRecord;
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //Common.UpdateTable("insert into api_access (ip, value, value_type, posted_appname) select '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + SESSION_ID + "','session_id','" + HttpContext.Current.Request["appname"] + "'");
                string Ip = HttpContext.Current.Request.ServerVariables["remote_addr"];
                string AppName = HttpContext.Current.Request["appname"];
                api_access tblapi_access = new api_access()
                {
                    ip = Ip,
                    value = SESSION_ID.ToString(),
                    posted_appname = AppName,
                    value_type = "session_id"
                };
                dataContext.api_access.Add(tblapi_access);
                int result1 = dataContext.SaveChanges();
                string AppNameQ = "inside up";//HttpContext.Current.Request.QueryString["appname"];
                string UserName = null;
                if (AppNameQ != null)
                {
                    AppNameQ = "inside up";//HttpContext.Current.Request["appname"];
                }
                else
                {
                    UserName = HttpContext.Current.User.Identity.Name;
                }
                //SqlCommand reply = new SqlCommand("select form_score3.id as F_ID, xcc_report_new.id  as X_ID,  * from xcc_report_new left join form_score3 on review_id  = xcc_report_new.id  where xcc_report_new.session_id = @session_id and isnull(review_date, dbo.getMTdate()) > dateadd(d, -30, getdate()) and max_reviews = 1 " + add_sql, cn);
                var GetRecordxccReportNew = dataContext.GetRecordxccReportNew(SESSION_ID, AppNameQ, UserName).ToList();
                foreach (var dr in GetRecordxccReportNew)
                {
                    CallRecord scr = new CallRecord();
                    scr.F_ID = dr.F_ID.ToString();
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
                    scr.review_started = "";
                    scr.Number_of_Schools = dr.Number_of_Schools;
                    scr.EducationLevel = dr.EducationLevel;
                    scr.HighSchoolGradYear = dr.HighSchoolGradYear;
                    scr.DegreeStartTimeframe = dr.DegreeStartTimeframe;

                    scr.First_Name = dr.First_Name;
                    scr.Last_Name = dr.Last_Name;
                    scr.address = dr.address;
                    scr.phone = dr.phone;
                    scr.call_date = dr.call_date.ToString();
                    scr.audio_link = dr.audio_link;
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
                    scr.uploaded = "";
                    scr.formatted_comments = dr.formatted_comments;
                    scr.formatted_missed = dr.formatted_missed;
                    scr.fileUrl = "";
                    scr.statusMessage = dr.statusMessage;
                    scr.mediaId = "";
                    scr.requestStatus = "";
                    scr.fileStatus = "";
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
                    if (dr.F_ID.ToString() == "")
                    {
                        scr.F_ID = "0";
                    }
                    List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                    DataTable qdt;
                    if (scr.wasEdited == "" & scr.calib_score != "")
                    {
                        //qdt = GetTable("select * from calibration_scores join questions on questions.id = calibration_scores.question_id  join question_answers on question_answers.ID = calibration_scores.question_result where form_id = (select top 1 id from vwCF where f_id = " + scr.F_ID + " and active_cali = 1) order by questions.q_order");
                        var vwCFID = dataContext.vwCFs.Where(x => x.F_ID == dr.F_ID && x.active_cali == 1).FirstOrDefault();
                        int f_ID = 0;
                        if (vwCFID != null)
                        {
                            f_ID = Convert.ToInt32(vwCFID.id);
                        }
                        var ScoreResponse = (from calibrationScores in dataContext.calibration_scores
                                             join questions in dataContext.Questions on calibrationScores.question_id equals questions.id
                                             join questionA in dataContext.question_answers on calibrationScores.question_result equals questionA.id
                                             where calibrationScores.form_id == f_ID
                                             select new ScorecardResponse
                                             {
                                                 position = calibrationScores.q_pos.ToString(),
                                                 question = questions.q_short_name,
                                                 result = questionA.answer_text,
                                                 QID = questions.id,
                                                 QAPoints = (int)questions.QA_points,
                                                 ViewLink = calibrationScores.view_link,
                                                 comments_allowed = (bool)questions.comments_allowed,
                                                 RightAnswer = (bool)questionA.right_answer,
                                                 q_order = questions.q_order,
                                             }).OrderBy(x => x.q_order).ToList();
                        scr.ScorecardResponses = new List<ScorecardResponse>();
                        scr.ScorecardResponses.AddRange(ScoreResponse);

                        foreach (var qdr in ScoreResponse)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            var vwCF = dataContext.vwCFs.Where(x => x.F_ID == dr.F_ID && x.active_cali == 1).FirstOrDefault();
                            if (scr.wasEdited == "" & scr.calib_score != "")
                            {
                                //ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_c_responses left join answer_comments on form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = (select id from vwCF where f_id = " + dr["F_ID"].ToString() + " and active_cali = 1) and form_c_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");
                                if (vwCF != null)
                                {
                                    var objCompanie = (from form_cResponses in dataContext.form_c_responses
                                                       join answercomments in dataContext.answer_comments on form_cResponses.answer_id equals answercomments.id into SJ
                                                       from answercomments in SJ.DefaultIfEmpty()
                                                       where form_cResponses.form_id == vwCF.id && form_cResponses.question_id == qdr.q_id
                                                       select new { answercomments.comment, form_cResponses.other_answer_text, answercomments.comment_points, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                    List<string> ans_comment = new List<string>();
                                    if (objCompanie.Count > 0)
                                    {
                                        foreach (var ans_dr in objCompanie)
                                        {
                                            ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                        }
                                    }
                                    qr.QComments = ans_comment;
                                    qrs.Add(qr);
                                }
                            }
                            else
                            {
                                //ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_q_responses left join answer_comments on form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " and form_q_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");
                                var objCompanie = (from form_cResponses in dataContext.form_q_responses
                                                   join answercomments in dataContext.answer_comments on form_cResponses.answer_id equals answercomments.id into SJ
                                                   from answercomments in SJ.DefaultIfEmpty()
                                                   where form_cResponses.form_id == dr.F_ID && form_cResponses.question_id == qdr.QID
                                                   select new { answercomments.comment, form_cResponses.other_answer_text, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                List<string> ans_comment = new List<string>();
                                if (objCompanie.Count > 0)
                                {
                                    foreach (var ans_dr in objCompanie)
                                    {
                                        ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                    }
                                }
                                qr.QComments = ans_comment;
                                qrs.Add(qr);
                            }
                        }
                    }
                    else
                    {
                        //qdt = GetTable("select * from form_q_scores join questions on questions.id = form_q_scores.question_id  join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + scr.F_ID + " order by questions.q_order");
                        int f_ID = 0;
                        if (scr.F_ID != null)
                        {
                            f_ID = Convert.ToInt32(scr.F_ID);
                        }
                        var ScoreResponse = (from form_q_scores in dataContext.form_q_scores
                                             join questions in dataContext.Questions on form_q_scores.question_id equals questions.id
                                             join questionA in dataContext.question_answers on form_q_scores.question_answered equals questionA.id
                                             where form_q_scores.form_id == f_ID
                                             select new ScorecardResponse
                                             {
                                                 position = form_q_scores.q_position,
                                                 question = questions.q_short_name.ToString(),
                                                 result = questionA.answer_text.ToString(),
                                                 QID = questions.id,
                                                 QAPoints = (int)questions.QA_points,
                                                 ViewLink = form_q_scores.view_link.ToString(),
                                                 comments_allowed = (bool)questions.comments_allowed,
                                                 RightAnswer = (bool)questionA.right_answer,
                                                 q_order = questions.q_order,
                                             }).OrderBy(x => x.q_order).ToList();
                        scr.ScorecardResponses = new List<ScorecardResponse>();
                        scr.ScorecardResponses.AddRange(ScoreResponse);

                        foreach (var qdr in ScoreResponse)
                        {
                            ScorecardResponse qr = new ScorecardResponse();
                            var vwCF = dataContext.vwCFs.Where(x => x.F_ID == dr.F_ID && x.active_cali == 1).FirstOrDefault();
                            if (scr.wasEdited == "" & scr.calib_score != "")
                            {
                                //ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_c_responses left join answer_comments on form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = (select id from vwCF where f_id = " + dr["F_ID"].ToString() + " and active_cali = 1) and form_c_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");
                                if (vwCF != null)
                                {
                                    var objCompanie = (from form_cResponses in dataContext.form_c_responses
                                                       join answercomments in dataContext.answer_comments on form_cResponses.answer_id equals answercomments.id into SJ
                                                       from answercomments in SJ.DefaultIfEmpty()
                                                       where form_cResponses.form_id == vwCF.id && form_cResponses.question_id == qdr.q_id
                                                       select new { answercomments.comment, form_cResponses.other_answer_text, answercomments.comment_points, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                    List<string> ans_comment = new List<string>();
                                    if (objCompanie.Count > 0)
                                    {
                                        foreach (var ans_dr in objCompanie)
                                        {
                                            ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                        }
                                    }
                                    qr.QComments = ans_comment;
                                    qrs.Add(qr);
                                }
                            }
                            else
                            {
                                //ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_q_responses left join answer_comments on form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " and form_q_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");
                                var objCompanie = (from form_cResponses in dataContext.form_q_responses
                                                   join answercomments in dataContext.answer_comments on form_cResponses.answer_id equals answercomments.id into SJ
                                                   from answercomments in SJ.DefaultIfEmpty()
                                                   where form_cResponses.form_id == dr.F_ID && form_cResponses.question_id == qdr.QID
                                                   select new { answercomments.comment, form_cResponses.other_answer_text, answercomments.ac_order }).OrderBy(x => x.ac_order).ToList();
                                List<string> ans_comment = new List<string>();
                                if (objCompanie.Count > 0)
                                {
                                    foreach (var ans_dr in objCompanie)
                                    {
                                        ans_comment.Add(ans_dr.comment + ans_dr.other_answer_text);
                                    }
                                }
                                qr.QComments = ans_comment;
                                qrs.Add(qr);
                            }
                        }
                    }
                    scr.ScorecardResponses = qrs;
                    int f_Id = 0;
                    if (scr.F_ID != null)
                    {
                        f_Id = Convert.ToInt32(scr.F_ID);
                    }
                    List<ClerkedData> CDs = new List<ClerkedData>();
                    var clerkDataTemp = (from collected_data in dataContext.collected_data
                                         join sc_inputs in dataContext.sc_inputs on collected_data.value_id equals sc_inputs.id
                                         where collected_data.form_id == f_Id
                                         select new ClerkedData
                                         {
                                             value = sc_inputs.value,
                                             data = collected_data.value_data,
                                             position = collected_data.value_position.ToString(),
                                             ID = collected_data.value_id.ToString()
                                         }).ToList();
                    scr.ClerkedDataItems = new List<ClerkedData>();
                    scr.ClerkedDataItems.AddRange(clerkDataTemp);
                    //scr.ClerkedDataItems = CDs;
                    objCallRecord.Add(scr);
                }
                return objCallRecord;
            }
        }

        #region Public getScore
        /// <summary>
        /// getScore
        /// </summary>
        /// <param name="gsd"></param>
        /// <returns></returns>
        public List<SessionStatus> GetScore(getScoreData gsd)
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
                    if (isxcc_report != null)
                    {
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
            }
            return objSessionStatus;
        }
        #endregion getScore


        /// <summary>
        /// AddRecord
        /// </summary>
        /// <param name="ADR"></param>
        /// <returns></returns>
        public string AddRecord(AddRecordData ADR)
        {
            string SESSION_ID = clean_string(ADR.SESSION_ID);
            string AGENT = clean_string(ADR.AGENT);
            string AGENT_NAME = clean_string(ADR.AGENT_NAME);
            string DISPOSITION = clean_string(ADR.DISPOSITION);
            string CAMPAIGN = clean_string(ADR.CAMPAIGN);
            string ANI = clean_string(ADR.ANI);
            string DNIS = clean_string(ADR.DNIS);
            string TIMESTAMP = clean_string(ADR.TIMESTAMP);
            string TALK_TIME = clean_string(ADR.TALK_TIME);
            string CALL_TIME = clean_string(ADR.CALL_TIME);
            string HANDLE_TIME = clean_string(ADR.HANDLE_TIME);
            string CALL_TYPE = clean_string(ADR.CALL_TYPE);
            string LIST_NAME = clean_string(ADR.LIST_NAME);
            string leadid = clean_string(ADR.leadid);
            string AGENT_GROUP = clean_string(ADR.AGENT_GROUP);
            string HOLD_TIME = clean_string(ADR.HOLD_TIME);
            string Email = clean_string(ADR.Email);
            string City = clean_string(ADR.City);
            string State = clean_string(ADR.State);
            string Zip = clean_string(ADR.Zip);
            string Datacapturekey = clean_string(ADR.Datacapturekey);
            string Datacapture = clean_string(ADR.Datacapture);
            string Status = clean_string(ADR.Status);
            string Program = clean_string(ADR.Program);
            string Datacapture_Status = clean_string(ADR.Datacapture_Status);
            string num_of_schools = clean_string(ADR.num_of_schools);
            string EducationLevel = clean_string(ADR.EducationLevel);
            string HighSchoolGradYear = clean_string(ADR.HighSchoolGradYear);
            string DegreeStartTimeframe = clean_string(ADR.DegreeStartTimeframe);
            string appname = clean_string(ADR.appname);
            string First_Name = clean_string(ADR.First_Name);
            string Last_Name = clean_string(ADR.Last_Name);
            string address = clean_string(ADR.address);
            string phone = clean_string(ADR.phone);
            string audio_link = clean_string(ADR.audio_link);
            string sort_order = clean_string(ADR.sort_order);
            string scorecard = clean_string(ADR.scorecard);
            string call_date = clean_string(ADR.call_date);
            string Citizenship = clean_string(ADR.Citizenship);
            string Military = clean_string(ADR.Military);
            string profile_id = clean_string(ADR.profile_id);
            string website = clean_string(ADR.website);
            SchoolItem[] Schools = ADR.Schools;
            AudioFile[] audios = ADR.audios;
            OtherData[] OtherDataItems = ADR.OtherDataItems;
            string Repost = ADR.Repost;
            string Message = string.Empty;
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
                //SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
                string remoteAddr = HttpContext.Current.Request.ServerVariables["remote_addr"];
                string page = HttpContext.Current.Request.QueryString.ToString();
                flatPost flatPost = new flatPost()
                {
                    raw_data = page,
                    ip_address = remoteAddr,
                };
                dataContext.flatPosts.Add(flatPost);
                int result1 = dataContext.SaveChanges();

                //Common.UpdateTable("exec add_ip '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + appname + "'");
                int insert = dataContext.add_ip(remoteAddr, appname);
                if (appname != null)
                {
                    if (appname.ToString().ToLower() == "inside up")
                    {
                        profile_id = leadid;
                        leadid = null;
                    }
                }
                if (clean_string(HttpContext.Current.Request["appname"]) != appname)
                {
                    //return "Invalid appname/apikey to post data with.";
                }
                string[] domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Split('.');
                XCCNewReport xcrpt = new XCCNewReport();
                if (appname != "")
                {
                    //sql = "declare @xcc_id int; insert into xcc_report_new_pending(";
                }
                else
                {
                    //sql = "declare @xcc_id int; insert into xcc_report_new_pending(appname,";
                    xcrpt.appname = domain[0];
                }

                if (SESSION_ID != "")
                {
                    xcrpt.SessionId = SESSION_ID;
                }

                if (call_date != "")
                {
                    xcrpt.call_date = Convert.ToDateTime(call_date);
                }

                if (Citizenship != "")
                {
                    xcrpt.Citizenship = Citizenship;
                }

                if (Military != "")
                {
                    xcrpt.Military = Military;
                }

                if (scorecard != "" & Information.IsNumeric(scorecard))
                {
                    xcrpt.scorecard = Convert.ToInt32(scorecard);
                }

                if (AGENT != "")
                {
                    xcrpt.Agent = AGENT;
                }

                if (AGENT_NAME != "")
                {
                    xcrpt.AGENT_NAME = AGENT_NAME;
                }

                if (website != "")
                {
                    xcrpt.website = website;
                }
                if (DISPOSITION != "")
                {
                    xcrpt.DisPosition = DISPOSITION;
                }
                if (CAMPAIGN != "")
                {
                    xcrpt.Campaign = CAMPAIGN;
                }
                if (ANI != "")
                {
                    xcrpt.Ani = ANI;
                }
                if (DNIS != "")
                {
                    xcrpt.Dnis = DNIS;
                }
                if (TIMESTAMP != "")
                {
                    xcrpt.TimeStamp = TIMESTAMP;
                }
                if (TALK_TIME != "")
                {
                    xcrpt.TalkTime = TALK_TIME.Replace("nn", "00");
                }
                if (CALL_TIME != "")
                {
                    xcrpt.CallTime = CALL_TIME;
                }
                if (HANDLE_TIME != "")
                {
                    xcrpt.HandleTime = HANDLE_TIME;
                }
                if (CALL_TYPE != "")
                {
                    xcrpt.CallType = CALL_TYPE;
                }
                if (LIST_NAME != "")
                {
                    xcrpt.ListName = LIST_NAME;
                }
                if (leadid != "")
                {
                    xcrpt.Leadid = leadid;
                }
                if (AGENT_GROUP != "")
                {
                    xcrpt.AgentGroup = AGENT_GROUP;
                }

                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime mtdate = dateQuery.AsEnumerable().First();
                if (Email != "")
                {
                    xcrpt.Email = Email;
                }
                if (City != "")
                {
                    xcrpt.City = City;
                }
                if (State != "")
                {
                    xcrpt.State = State;
                }
                if (Zip != "")
                {
                    xcrpt.Zip = Zip;
                }
                if (Datacapturekey != "" & Information.IsNumeric(Datacapturekey))
                {
                    xcrpt.Datacapturekey = Convert.ToInt32(Datacapturekey);
                }
                if (Datacapture != "" & Information.IsNumeric(Datacapture))
                {
                    xcrpt.Datacapture = Convert.ToInt32(Datacapture);
                }
                if (Status != "")
                {
                    xcrpt.Status = Status;
                }
                if (Program != "")
                {
                    xcrpt.Program = Program;
                }
                if (Datacapture_Status != "")
                {
                    xcrpt.Datacapture_Status = Datacapture_Status;
                }
                if (num_of_schools != "")
                {
                    xcrpt.num_of_schools = num_of_schools;
                }
                if (EducationLevel != "")
                {
                    xcrpt.EducationLevel = EducationLevel;
                }
                if (HighSchoolGradYear != "")
                {
                    xcrpt.HighSchoolGradYear = HighSchoolGradYear;
                }
                if (DegreeStartTimeframe != "")
                {
                    xcrpt.DegreeStartTimeframe = DegreeStartTimeframe;
                }

                if (appname != "")
                {
                    xcrpt.appname = appname;
                }
                if (First_Name != "")
                {
                    xcrpt.First_Name = First_Name;
                }
                if (Last_Name != "")
                {
                    xcrpt.Last_Name = Last_Name;
                }
                if (audio_link != "")
                {
                    xcrpt.audio_link = audio_link;
                }

                if (profile_id != "")
                {
                    xcrpt.profile_id = profile_id;
                }

                if (sort_order != "" & Information.IsNumeric(sort_order))
                {
                    xcrpt.sort_order = Convert.ToInt32(sort_order);
                }
                int new_id = 0;
                xcc_report_new_pending xccreportnew_pending = new xcc_report_new_pending()
                {
                    DATE = mtdate,
                    SESSION_ID = xcrpt.SessionId,
                    AGENT = xcrpt.Agent,
                    DISPOSITION = xcrpt.DisPosition,
                    CAMPAIGN = xcrpt.Campaign,
                    ANI = xcrpt.Ani,
                    DNIS = xcrpt.Dnis,
                    TIMESTAMP = xcrpt.TimeStamp,
                    TALK_TIME = xcrpt.TalkTime,
                    HANDLE_TIME = xcrpt.HandleTime,
                    CALL_TYPE = xcrpt.CallType,
                    LIST_NAME = xcrpt.ListName,
                    leadid = xcrpt.Leadid,
                    AGENT_GROUP = xcrpt.AgentGroup,
                    Email = xcrpt.Email,
                    City = xcrpt.City,
                    State = xcrpt.State,
                    Zip = xcrpt.Zip,
                    Datacapturekey = xcrpt.Datacapturekey,
                    Datacapture = xcrpt.Datacapture,
                    Status = xcrpt.Status,
                    Program = xcrpt.Program,
                    Datacapture_Status = xcrpt.Datacapture_Status,
                    num_of_schools = xcrpt.num_of_schools,
                    EducationLevel = xcrpt.EducationLevel,
                    HighSchoolGradYear = xcrpt.HighSchoolGradYear,
                    DegreeStartTimeframe = xcrpt.DegreeStartTimeframe,
                    appname = xcrpt.appname,
                    First_Name = xcrpt.First_Name,
                    Last_Name = xcrpt.Last_Name,
                    call_date = xcrpt.call_date,
                    audio_link = xcrpt.audio_link,
                    profile_id = xcrpt.profile_id,
                };
               
                dataContext.xcc_report_new_pending.Add(xccreportnew_pending);
                int id = dataContext.SaveChanges();
                new_id = xccreportnew_pending.ID;
                if (new_id > 0)
                {
                    Message = Messages.Insert;
                }
                if (Schools != null)
                {
                    foreach (SchoolItem si in Schools)
                    {
                        //string sql_school = "insert into school_data(pending_id,";
                        //string param_scure = " @pending_id,";
                        SchoolData objSchoolData = new SchoolData();
                        string AOI1 = object.Equals(si.AOI1, null) ? "0" : si.AOI1;
                        string AOI2 = object.Equals(si.AOI2, null) ? "0" : si.AOI2;
                        string College = object.Equals(si.College, null) ? "0" : si.College;
                        string DegreeOfInterest = object.Equals(si.DegreeOfInterest, null) ? "0" : si.DegreeOfInterest;
                        string L1_SubjectName = object.Equals(si.L1_SubjectName, null) ? "0" : si.L1_SubjectName;
                        string L2_SubjectName = object.Equals(si.L2_SubjectName, null) ? "0" : si.L2_SubjectName;
                        string Modality = object.Equals(si.Modality, null) ? "0" : si.Modality;
                        string School = object.Equals(si.School, null) ? "0" : si.School;
                        string Portal = object.Equals(si.Portal, null) ? "0" : si.Portal;
                        string TCPA = object.Equals(si.TCPA, null) ? "0" : si.TCPA;

                        if (TCPA.ToString() != "")
                        {
                            objSchoolData.TCPA = si.TCPA;
                        }
                        if (new_id > 1)
                        {
                            objSchoolData.pending_id = new_id;
                        }
                        if (AOI1.ToString() != "")
                        {
                            objSchoolData.TCPA = si.AOI1;
                        }
                        if (Portal.ToString() != "")
                        {
                            objSchoolData.Portal = si.Portal;
                        }


                        if (AOI2.ToString() != "")
                        {
                            objSchoolData.AOI2 = si.AOI2;
                        }

                        if (College.ToString() != "")
                        {
                            objSchoolData.College = si.College;
                        }

                        if (DegreeOfInterest.ToString() != "")
                        {
                            objSchoolData.DegreeOfInterest = si.DegreeOfInterest;
                        }

                        if (L1_SubjectName.ToString() != "")
                        {
                            objSchoolData.L1_SubjectName = si.L1_SubjectName;
                        }

                        if (L2_SubjectName.ToString() != "")
                        {
                            objSchoolData.L2_SubjectName = si.L2_SubjectName;
                        }
                        if (Modality.ToString() != "")
                        {
                            objSchoolData.Modality = si.Modality;
                        }

                        if (School.ToString() != "")
                        {
                            objSchoolData.School = si.School;
                        }
                        school_data schoolData = new school_data()
                        {
                            AOI1 = objSchoolData.AOI1,
                            AOI2 = objSchoolData.AOI2,
                            College = objSchoolData.College,
                            DegreeOfInterest = objSchoolData.DegreeOfInterest,
                            L1_SubjectName = objSchoolData.L1_SubjectName,
                            L2_SubjectName = objSchoolData.L2_SubjectName,
                            Modality = objSchoolData.Modality,
                            School = objSchoolData.School,
                            TCPA = objSchoolData.TCPA,
                            pending_id = objSchoolData.pending_id,
                        };
                        dataContext.school_data.Add(schoolData);
                        int resultschoolData = dataContext.SaveChanges();
                        if (resultschoolData == 1)
                        {
                            Message = Messages.Insert;
                        }
                    }
                }
                if (audios != null)
                {
                    foreach (AudioFile af in audios)
                    {
                        string order = object.Equals(af.order, null) ? "0" : af.order;
                        string file_date = object.Equals(af.file_date, null) ? DateTime.Now.ToShortDateString() : af.file_date;
                        string audio_file = af.audio_file;

                        if (appname == "inside up" & af.audio_file.ToString() != "")
                        {
                            audio_file = af.audio_file.Replace("%3A", ":").Replace("%2F", "/").Replace("+", "%20");
                        }
                        //Common.UpdateTable("insert into AudioData(file_name, file_date, file_order, pending_id) select '" + audio_file.ToString().Replace("'", "''") + "','" + file_date.ToString().Replace("nn", "00") + "','" + order.ToString() + "'," + new_id);
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
                        if (type == "")
                        {
                            type = "string";
                        }
                        //Common.UpdateTable("insert into otherFormDataPending(form_id, data_key, data_value, data_type, school_name, label) select " + new_id + ",'" + od.key.ToString().Replace("'", "''") + "','" + od.value.ToString().Replace("'", "''") + "','" + type.ToString() + "','" + od.school.ToString().Replace("'", "''") + "','" + od.label.ToString().Replace("'", "''") + "'");
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
                        //Common.Email_Error("FAIL: " + ex.Message + " " + "insert into otherFormDataPending(form_id, data_key, data_value, data_type) select " + new_id + ",'" + od.key.ToString() + "','" + od.value.ToString() + "','" + type.ToString() + "'");
                        //return "FAIL: " + ex.Message;
                    }
                }
                return Message = Messages.Insert;
            }
        }

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
                int Id = 0;
                if (SI.ID != "")
                {
                    Id = Convert.ToInt32(SI.ID);
                }
                var isform = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                if (isform == null)
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
                var sc_dt = dataContext.scorecards.Where(x => x.id == scorecard_ID && x.appname == "vsrto").FirstOrDefault();
                //DataTable sc_dt = GetTable("select * from scorecards where id = " + scorecard_ID + " " + add_sql);
                if (sc_dt == null)
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
                        sec.section = item1.section1;
                        sec.description = item1.Descrip;
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
                string add_sql = "";
                if (HttpContext.Current.Request.QueryString["appname"] != null)
                {
                    add_sql = HttpContext.Current.Request.QueryString["appname"];
                }
                else
                {
                    var add_sql1 = dataContext.UserApps.Where(x => x.username == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                    if (add_sql1 != null)
                    {
                        add_sql = add_sql1.appname;
                    }
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
            int scorecard_ID = 0;
            CompleteScorecard sc = new CompleteScorecard();
            if (SI.scorecard_ID == "")
            {
                return sc;
            }
            else
            {
                scorecard_ID = Convert.ToInt32(SI.scorecard_ID);
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
                if (sc_dt != null)
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
                                                 where questions.scorecard_id == scorecard_ID && questions.section == item1.id && questions.active == true
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

/// <summary>
/// 
/// </summary>
public class SchoolData
{
    public int pending_id { get; set; }
    public string AOI1 { get; set; }
    public string AOI2 { get; set; }
    public string College { get; set; }
    public string DegreeOfInterest { get; set; }
    public string L1_SubjectName { get; set; }
    public string L2_SubjectName { get; set; }
    public string Modality { get; set; }
    public string School { get; set; }
    public string Portal { get; set; }
    public string TCPA { get; set; }
}

/// <summary>
/// 
/// </summary>
public class XCCNewReport
{

    public string appname { get; set; }
    public string SessionId { get; set; }
    public string Agent { get; set; }
    public string DisPosition { get; set; }
    public string Campaign { get; set; }
    public string Ani { get; set; }
    public string Dnis { get; set; }
    public string TimeStamp { get; set; }
    public string TalkTime { get; set; }
    public string CallTime { get; set; }
    public string HandleTime { get; set; }
    public string CallType { get; set; }
    public string ListName { get; set; }
    public string Leadid { get; set; }
    public string AgentGroup { get; set; }
    public DateTime Date { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public DateTime call_date { get; set; }
    public string Citizenship { get; set; }
    public string Military { get; set; }
    public string AGENT_NAME { get; set; }
    public string website { get; set; }
    public int? scorecard { get; set; }
    public string Zip { get; set; }
    public float Datacapturekey { get; set; }
    public float Datacapture { get; set; }
    public string Status { get; set; }
    public string Program { get; set; }
    public string Datacapture_Status { get; set; }
    public string num_of_schools { get; set; }
    public string EducationLevel { get; set; }
    public string HighSchoolGradYear { get; set; }
    public string DegreeStartTimeframe { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string audio_link { get; set; }
    public string profile_id { get; set; }
    public int? sort_order { get; set; }
}