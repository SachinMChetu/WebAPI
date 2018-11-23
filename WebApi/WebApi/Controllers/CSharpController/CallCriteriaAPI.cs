using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using WebApi.DataLayer;
using WebApi.Models.CallCriteriaAPI;
using WebApi.Models.CDService;
using static WebApi.Controllers.CSharpController.CDService;

namespace WebApi.Controllers.CSharpController
{
    /// <summary>
    /// CallCriteriaAPI
    /// </summary>
    [ServiceContract(Namespace = "CallCriteriaAPI")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any, ConcurrencyMode = ConcurrencyMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CallCriteriaAPI
    {
        /// <summary>
        /// CallCriteriaAPI
        /// </summary>
        public CallCriteriaAPI()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Request.Url.ToString().IndexOf("/help") == -1)
                    ValidateKey();
                else
                {
                }
            }
        }

        /// <summary>
        /// ValidateKey
        /// </summary>
        private void ValidateKey()
        {
            if (HttpContext.Current.Request.ServerVariables["remote_addr"] == "52.90.147.233")
            {
                string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
                SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address");
                reply.Parameters.AddWithValue("raw_data", raw_post);
                reply.Parameters.AddWithValue("ip_address", HttpContext.Current.Request.ServerVariables["remote_addr"]);

                try
                {
                 Common.GetTable(reply);
                }
                catch (Exception ex)
                {
                }
            }

            string key = clean_string(HttpContext.Current.Request.QueryString["apikey"]) + clean_string(HttpContext.Current.Request.QueryString["api_key"]);
            string appname = clean_string(HttpContext.Current.Request.QueryString["appname"]);
            if (key == null | appname == null)
                throw new System.ServiceModel.Web.WebFaultException<string>("Invalid APIKEY Or Appname (missing)", HttpStatusCode.Forbidden);
            try
            {
                SqlCommand sq = new SqlCommand("select api_key from API_KEYS where api_key = @key and appname = @appname and active = 1");
                sq.Parameters.AddWithValue("key", key);
                sq.Parameters.AddWithValue("appname", appname);
                DataTable key_dt = Common.GetTable(sq);

                if (key_dt.Rows.Count == 0)
                    throw new WebFaultException<string>("Invalid APIKEY or Appname (not found)", HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>("Invalid APIKEY or Appname (" + ex.Message + ")", HttpStatusCode.Forbidden);
            }
        }

        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, int debug = 0)
        {
            string sql_start = DateTime.Now.ToString();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlDataAdapter reply = new SqlDataAdapter(sql, cn);
            reply.SelectCommand.CommandTimeout = 60;
            DataTable dt = new DataTable();
            try
            {
                reply.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            cn.Close();
            cn.Dispose();
            return dt;
        }

        /// <summary>
        /// AddExistingSchool
        /// </summary>
        /// <param name="AES"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        [Description("Add more data to an existing record after it has been received and processed.")]
        public string AddExistingSchool(AddExistingSchoolRequest AES)
        {
            string SESSION_ID = AES.SESSION_ID;
            SchoolItem[] Schools = AES.Schools;
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            foreach (SchoolItem si in Schools)
            {
                string sql_school = "insert into school_data(pending_id, school, college, degreeofinterest, AOI1, AOI2, L1_SubjectName, L2_SubjectName, Modality) values";
                sql_school += "((select top 1 id from xcc_report_new_pending where SESSION_ID = @SESSION_ID and appname = '" + HttpContext.Current.Request["appname"] + "'), @school, @college, @degreeofinterest, @AOI1, @AOI2, @L1_SubjectName, @L2_SubjectName, @Modality)";

                SqlCommand reply_school = new SqlCommand(sql_school, cn);
                reply_school.Parameters.AddWithValue("SESSION_ID", SESSION_ID);
                reply_school.Parameters.AddWithValue("AOI1", string.IsNullOrEmpty(si.AOI1) ? "" : si.AOI1);
                reply_school.Parameters.AddWithValue("AOI2", string.IsNullOrEmpty(si.AOI2) ? "" : si.AOI2);
                reply_school.Parameters.AddWithValue("College", string.IsNullOrEmpty(si.College) ? "" : si.College);
                reply_school.Parameters.AddWithValue("DegreeOfInterest", string.IsNullOrEmpty(si.DegreeOfInterest) ? "" : si.DegreeOfInterest);
                reply_school.Parameters.AddWithValue("L1_SubjectName", string.IsNullOrEmpty(si.L1_SubjectName) ? "" : si.L1_SubjectName);
                reply_school.Parameters.AddWithValue("L2_SubjectName", string.IsNullOrEmpty(si.L2_SubjectName) ? "" : si.L2_SubjectName);
                reply_school.Parameters.AddWithValue("Modality", string.IsNullOrEmpty(si.Modality) ? "" : si.Modality);
                reply_school.Parameters.AddWithValue("School", string.IsNullOrEmpty(si.School) ? "" : si.School);
                reply_school.Parameters.AddWithValue("Portal", string.IsNullOrEmpty(si.Portal) ? "" : si.Portal);
                reply_school.Parameters.AddWithValue("TCPA", string.IsNullOrEmpty(si.TCPA) ? "" : si.TCPA);
                try
                {
                    reply_school.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return "FAIL: " + ex.Message + " ";
                }
            }
            cn.Close();
            cn.Dispose();

            return "SUCCESS";
        }

        /// <summary>
        /// GetAllRecords
        /// </summary>
        /// <param name="GARD"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        [Description("Returns all records associated with a specific scorecard")]
        public List<CallRecord> GetAllRecords(GetAllRecordData GARD)
        {
            string call_date = GARD.call_date;
            string appname = HttpContext.Current.Request["appname"];
            string use_review = GARD.use_review;
            List<CallRecord> cr = new List<CallRecord>();

            bool rev_date = false;
            if (use_review == null)
                rev_date = false;
            switch (use_review)
            {
                case "1":
                case "true":
                case "True":
                    {
                        rev_date = true;
                        break;
                    }
            }

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand reply = new SqlCommand("exec GetAllRecords @call_date, @appname, @use_review", cn);
            reply.Parameters.AddWithValue("call_date", call_date);
            reply.Parameters.AddWithValue("use_review", rev_date);
            reply.Parameters.AddWithValue("appname", appname);
            adapter.SelectCommand = reply;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CallRecord scr = new CallRecord();
                scr.F_ID = dr["F_ID"].ToString();
                scr.review_ID = dr["review_ID"].ToString();
                scr.Comments = dr["Comments"].ToString();
                scr.autofail = dr["autofail"].ToString();
                scr.reviewer = dr["reviewer"].ToString();
                scr.appname = dr["appname"].ToString();
                scr.total_score = dr["display_score"].ToString();
                scr.total_score_with_fails = dr["display_score"].ToString();
                scr.call_length = dr["call_length"].ToString();
                scr.has_cardinal = dr["has_cardinal"].ToString();
                scr.fs_audio = dr["fs_audio"].ToString();
                scr.week_ending_date = dr["week_ending_date"].ToString();
                scr.num_missed = dr["num_missed"].ToString();
                scr.missed_list = dr["missed_list"].ToString();
                scr.call_made_date = dr["call_made_date"].ToString();
                scr.AGENT = dr["AGENT"].ToString();
                scr.ANI = dr["ANI"].ToString();
                scr.DNIS = dr["DNIS"].ToString();
                scr.TIMESTAMP = dr["TIMESTAMP"].ToString();
                scr.TALK_TIME = dr["TALK_TIME"].ToString();
                scr.CALL_TIME = dr["CALL_TIME"].ToString();
                scr.CALL_TYPE = dr["CALL_TYPE"].ToString();
                scr.leadid = dr["leadid"].ToString();
                scr.AGENT_GROUP = dr["AGENT_GROUP"].ToString();
                scr.Email = dr["Email"].ToString();
                scr.City = dr["City"].ToString();
                scr.State = dr["State"].ToString();
                scr.Zip = dr["Zip"].ToString();
                scr.Datacapturekey = dr["Datacapturekey"].ToString();
                scr.Datacapture = dr["Datacapture"].ToString();
                scr.Status = dr["Status"].ToString();
                scr.Program = dr["Program"].ToString();
                scr.X_ID = dr["X_ID"].ToString();
                scr.Datacapture_Status = dr["Datacapture_Status"].ToString();
                scr.num_of_schools = dr["num_of_schools"].ToString();
                scr.MAX_REVIEWS = dr["MAX_REVIEWS"].ToString();
                scr.review_started = dr["review_started"].ToString();
                scr.Number_of_Schools = dr["Number_of_Schools"].ToString();
                scr.EducationLevel = dr["EducationLevel"].ToString();
                scr.HighSchoolGradYear = dr["HighSchoolGradYear"].ToString();
                scr.DegreeStartTimeframe = dr["DegreeStartTimeframe"].ToString();
                scr.Expr3 = ""; 
                scr.First_Name = dr["First_Name"].ToString();
                scr.Last_Name = dr["Last_Name"].ToString();
                scr.address = dr["address"].ToString();
                scr.phone = dr["phone"].ToString();
                scr.call_date = dr["call_date"].ToString();
                scr.audio_link = dr["audio_link"].ToString();
                scr.profile_id = dr["profile_id"].ToString();
                scr.audio_user = ""; 
                scr.audio_password = ""; 
                scr.LIST_NAME = dr["LIST_NAME"].ToString();
                scr.review_date = dr["review_date"].ToString();
                scr.CAMPAIGN = dr["CAMPAIGN"].ToString();
                scr.DISPOSITION = dr["DISPOSITION"].ToString();
                scr.bad_call = dr["bad_call"].ToString();
                scr.to_upload = ""; 
                scr.SESSION_ID = dr["SESSION_ID"].ToString();
                scr.agent_deviation = ""; 
                scr.pass_fail = dr["pass_fail"].ToString();
                scr.scorecard = dr["scorecard"].ToString();
                scr.uploaded = ""; 
                scr.formatted_comments = dr["formatted_comments"].ToString();
                scr.formatted_missed = dr["formatted_missed"].ToString();
                scr.fileUrl = dr["fileUrl"].ToString();
                scr.statusMessage = dr["statusMessage"].ToString();
                scr.mediaId = ""; 
                scr.requestStatus = ""; 
                scr.fileStatus = ""; 
                scr.response = ""; 
                scr.review_time = ""; 
                scr.wasEdited = dr["wasEdited"].ToString();
                scr.website = dr["website"].ToString();
                scr.pending_id = dr["pending_id"].ToString();
                scr.bad_call_reason = dr["bad_call_reason"].ToString();
                scr.date_added = dr["date_added"].ToString();
                scr.calib_score = dr["calib_score"].ToString();
                scr.edited_score = dr["edited_score"].ToString();
                scr.compliance_sheet = dr["compliance_sheet"].ToString();
                scr.scorecard_name = dr["scorecard_name"].ToString();

                List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                DataTable qdt = GetTable("select *,questions.id as q_id from form_q_scores join questions on questions.id = form_q_scores.question_id   join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + dr["F_ID"].ToString() + " order by q_order");
                foreach (DataRow qdr in qdt.Rows)
                {
                    ScorecardResponse qr = new ScorecardResponse();
                    qr.position = qdr["click_text"].ToString();
                    qr.question = qdr["q_short_name"].ToString();
                    qr.result = qdr["answer_text"].ToString();
                    qr.QID =Convert.ToInt32( qdr["ID"]);
                    qr.position = qdr["q_position"].ToString();
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
                    qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);
                    DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                    if (ans_dt.Rows.Count > 0)
                    {
                        List<string> ans_comment = new List<string>();
                        foreach (DataRow ans_dr in ans_dt.Rows)
                            ans_comment.Add(ans_dr["comment"].ToString());
                        qr.QComments = ans_comment;
                    }
                    qrs.Add(qr);
                }
                scr.ScorecardResponses = qrs;
                List<ClerkedData> CDs = new List<ClerkedData>();
                DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                foreach (DataRow qdr in cd_dt.Rows)
                {
                    ClerkedData qr = new ClerkedData();
                    qr.value = qdr["value"].ToString();
                    qr.data = qdr["value_data"].ToString();
                    qr.position = qdr["value_position"].ToString();
                    qr.ID = qdr["value_id"].ToString();
                    CDs.Add(qr);
                }
                scr.ClerkedDataItems = CDs;
                cr.Add(scr);
            }
            cn.Close();
            cn.Dispose();
            return cr;
        }

        /// <summary>
        /// GetAllRecordsWithPending
        /// </summary>
        /// <param name="GARD"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        [Description("Returns all records associated with a specific scorecard")]
        public List<CallRecord> GetAllRecordsWithPending(GetAllRecordData GARD)
        {
            string call_date = GARD.call_date;
            string appname = HttpContext.Current.Request["appname"];
            string use_review = GARD.use_review;
            List<CallRecord> cr = new List<CallRecord>();
            bool rev_date = false;
            if (use_review == null)
                rev_date = false;
            switch (use_review)
            {
                case "1":
                case "true":
                case "True":
                    {
                        rev_date = true;
                        break;
                    }
            }
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand reply = new SqlCommand("exec GetAllRecordsWithPending @call_date, @appname, @use_review", cn);
            reply.Parameters.AddWithValue("call_date", call_date);
            reply.Parameters.AddWithValue("use_review", rev_date);
            reply.Parameters.AddWithValue("appname", appname);
            adapter.SelectCommand = reply;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CallRecord scr = new CallRecord();
                scr.F_ID = dr["F_ID"].ToString();
                scr.review_ID = dr["review_ID"].ToString();
                scr.Comments = dr["Comments"].ToString();
                scr.autofail = dr["autofail"].ToString();
                scr.reviewer = dr["reviewer"].ToString();
                scr.appname = dr["appname"].ToString();
                scr.total_score = dr["display_score"].ToString();
                scr.total_score_with_fails = dr["display_score"].ToString();
                scr.call_length = dr["call_length"].ToString();
                scr.has_cardinal = dr["has_cardinal"].ToString();
                scr.fs_audio = dr["fs_audio"].ToString();
                scr.week_ending_date = dr["week_ending_date"].ToString();
                scr.num_missed = dr["num_missed"].ToString();
                scr.missed_list = dr["missed_list"].ToString();
                scr.call_made_date = dr["call_made_date"].ToString();
                scr.AGENT = dr["AGENT"].ToString();
                scr.ANI = dr["ANI"].ToString();
                scr.DNIS = dr["DNIS"].ToString();
                scr.TIMESTAMP = dr["TIMESTAMP"].ToString();
                scr.TALK_TIME = dr["TALK_TIME"].ToString();
                scr.CALL_TIME = dr["CALL_TIME"].ToString();
                scr.CALL_TYPE = dr["CALL_TYPE"].ToString();
                scr.leadid = dr["leadid"].ToString();
                scr.AGENT_GROUP = dr["AGENT_GROUP"].ToString();
                scr.Email = dr["Email"].ToString();
                scr.City = dr["City"].ToString();
                scr.State = dr["State"].ToString();
                scr.Zip = dr["Zip"].ToString();
                scr.Datacapturekey = dr["Datacapturekey"].ToString();
                scr.Datacapture = dr["Datacapture"].ToString();
                scr.Status = dr["Status"].ToString();
                scr.Program = dr["Program"].ToString();
                scr.X_ID = dr["X_ID"].ToString();
                scr.Datacapture_Status = dr["Datacapture_Status"].ToString();
                scr.num_of_schools = dr["num_of_schools"].ToString();
                scr.MAX_REVIEWS = dr["MAX_REVIEWS"].ToString();
                scr.review_started = dr["review_started"].ToString();
                scr.Number_of_Schools = dr["Number_of_Schools"].ToString();
                scr.EducationLevel = dr["EducationLevel"].ToString();
                scr.HighSchoolGradYear = dr["HighSchoolGradYear"].ToString();
                scr.DegreeStartTimeframe = dr["DegreeStartTimeframe"].ToString();
                scr.Expr3 = dr["Expr3"].ToString();
                scr.First_Name = dr["First_Name"].ToString();
                scr.Last_Name = dr["Last_Name"].ToString();
                scr.address = dr["address"].ToString();
                scr.phone = dr["phone"].ToString();
                scr.call_date = dr["call_date"].ToString();
                scr.audio_link = dr["audio_link"].ToString();
                scr.profile_id = dr["profile_id"].ToString();
                scr.audio_user = ""; 
                scr.audio_password = ""; 
                scr.LIST_NAME = dr["LIST_NAME"].ToString();
                scr.review_date = dr["review_date"].ToString();
                scr.CAMPAIGN = dr["CAMPAIGN"].ToString();
                scr.DISPOSITION = dr["DISPOSITION"].ToString();
                scr.bad_call = dr["bad_call"].ToString();
                scr.to_upload = ""; // dr("to_upload").ToString()
                scr.SESSION_ID = dr["SESSION_ID"].ToString();
                scr.agent_deviation = dr["agent_deviation"].ToString();
                scr.pass_fail = dr["pass_fail"].ToString();
                scr.scorecard = dr["scorecard"].ToString();
                scr.uploaded = dr["uploaded"].ToString();
                scr.formatted_comments = dr["formatted_comments"].ToString();
                scr.formatted_missed = dr["formatted_missed"].ToString();
                scr.fileUrl = dr["fileUrl"].ToString();
                scr.statusMessage = dr["statusMessage"].ToString();
                scr.mediaId = ""; // dr("mediaId").ToString()
                scr.requestStatus = ""; // dr("requestStatus").ToString()
                scr.fileStatus = ""; // dr("fileStatus").ToString()
                scr.response = ""; // dr("response").ToString()
                scr.review_time = ""; // dr("review_time").ToString()
                scr.wasEdited = dr["wasEdited"].ToString();
                scr.website = dr["website"].ToString();
                scr.pending_id = ""; // dr("pending_id").ToString()
                scr.bad_call_reason = dr["bad_call_reason"].ToString();
                scr.date_added = dr["date_added"].ToString();
                scr.calib_score = dr["calib_score"].ToString();
                scr.edited_score = dr["edited_score"].ToString();
                scr.compliance_sheet = dr["compliance_sheet"].ToString();
                scr.scorecard_name = dr["scorecard_name"].ToString();
                if (scr.F_ID != "")
                {
                    List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                    DataTable qdt = GetTable("select *,questions.id as q_id from form_q_scores join questions on questions.id = form_q_scores.question_id   join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + dr["F_ID"].ToString() + " order by q_order");
                    foreach (DataRow qdr in qdt.Rows)
                    {
                        ScorecardResponse qr = new ScorecardResponse();
                        qr.position = qdr["click_text"].ToString();
                        qr.question = qdr["q_short_name"].ToString();
                        qr.result = qdr["answer_text"].ToString();
                        qr.QID = Convert.ToInt32(qdr["ID"]);
                        qr.position = qdr["q_position"].ToString();
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
                        qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);

                        DataTable ans_dt = GetTable("Select isnull(comment,other_answer_text) As comment, comment_points from form_q_responses left join answer_comments On form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " And form_q_responses.question_id = " + qdr["q_id"].ToString() + " order by isnull(ac_order,10000)");
                        if (ans_dt.Rows.Count > 0)
                        {
                            List<string> ans_comment = new List<string>();
                            foreach (DataRow ans_dr in ans_dt.Rows)
                                ans_comment.Add(ans_dr["comment"].ToString());
                            qr.QComments = ans_comment;
                        }
                        qrs.Add(qr);
                    }
                    scr.ScorecardResponses = qrs;
                    List<ClerkedData> CDs = new List<ClerkedData>();
                    DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                    foreach (DataRow qdr in cd_dt.Rows)
                    {
                        ClerkedData qr = new ClerkedData();
                        qr.value = qdr["value"].ToString();
                        qr.data = qdr["value_data"].ToString();
                        qr.position = qdr["value_position"].ToString();
                        qr.ID = qdr["value_id"].ToString();
                        CDs.Add(qr);
                    }
                    scr.ClerkedDataItems = CDs;
                }
                cr.Add(scr);
            }
            cn.Close();
            cn.Dispose();
            return cr;
        }

        /// <summary>
        /// GetRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public AllCallRecord GetRecordID(SimpleID SI) // AllCallRecord 'List(Of CallRecord)
        {
            string ID = SI.ID;
            AllCallRecord scr = new AllCallRecord();
            string username = HttpContext.Current.User.Identity.Name;
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand reply = new SqlCommand("select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username", cn);
            reply.Parameters.AddWithValue("username", username);
            string non_edit = "";
            bool wasEdited = false;
            UserObject uo = new UserObject();
            if (username != "")
            {
                adapter.SelectCommand = reply;
                adapter.Fill(dt);
                non_edit = dt.Rows[0]["non_edit"].ToString();
                uo.UserRole = dt.Rows[0]["user_role"].ToString();
                uo.UserName = dt.Rows[0]["username"].ToString();
                uo.SpeedInc = dt.Rows[0]["speed_increment"].ToString();
                uo.StartImmediately =Convert.ToBoolean( dt.Rows[0]["calls_start_immediately"]);
                scr.UserInfo = uo;
            }

            string add_sql;
            if (HttpContext.Current.Request.QueryString["appname"] != null)
                add_sql = " and vwForm.appname = '" + HttpContext.Current.Request["appname"] + "'";
            else
                add_sql = " and vwForm.scorecard in (select user_scorecard from userapps where username= '" + username + "') ";
            reply = new SqlCommand("select vwForm.*,isnull(client_logo, vwForm.appname) as client_logo, (select total_score from vwCF where active_cali = 1 and f_id = @id) as qa_cali_score  from vwForm join app_settings on vwForm.appname = app_settings.appname where f_id = @id" + add_sql, cn);
            reply.Parameters.AddWithValue("id", ID);
            adapter.SelectCommand = reply;
            dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count == 0)
                return scr;

            int review_id = Convert.ToInt32(dt.Rows[0]["review_ID"]);
            List<ScorecardData> scds = new List<ScorecardData>();
            foreach (DataRow dr in dt.Rows)
            {
                scr.F_ID = dr["F_ID"].ToString();
                scr.client_logo = dr["client_logo"].ToString();
                scr.review_ID = dr["review_ID"].ToString();
                scr.Comments = dr["Comments"].ToString();
                scr.autofail = dr["autofail"].ToString();
                scr.reviewer = dr["reviewer"].ToString();
                scr.appname = dr["appname"].ToString();
                scr.total_score = dr["display_score"].ToString();
                scr.total_score_with_fails = dr["display_score"].ToString();
                scr.call_length = dr["call_length"].ToString();
                scr.has_cardinal = ""; // dr("has_cardinal").ToString()
                scr.fs_audio = dr["fs_audio"].ToString();
                scr.week_ending_date = dr["week_ending_date"].ToString();
                scr.num_missed = dr["num_missed"].ToString();
                scr.missed_list = dr["missed_list"].ToString();
                scr.call_made_date = dr["call_made_date"].ToString();
                scr.AGENT = dr["AGENT"].ToString();
                scr.ANI = dr["ANI"].ToString();
                scr.DNIS = dr["DNIS"].ToString();
                scr.TIMESTAMP = dr["TIMESTAMP"].ToString();
                scr.TALK_TIME = dr["TALK_TIME"].ToString();
                scr.CALL_TIME = dr["CALL_TIME"].ToString();
                scr.CALL_TYPE = dr["CALL_TYPE"].ToString();
                scr.leadid = dr["leadid"].ToString();
                scr.AGENT_GROUP = dr["AGENT_GROUP"].ToString();
                scr.Email = dr["Email"].ToString();
                scr.City = dr["City"].ToString();
                scr.State = dr["State"].ToString();
                scr.Zip = dr["Zip"].ToString();
                scr.Datacapturekey = dr["Datacapturekey"].ToString();
                scr.Datacapture = dr["Datacapture"].ToString();
                scr.Status = dr["Status"].ToString();
                scr.Program = dr["Program"].ToString();
                scr.X_ID = dr["X_ID"].ToString();
                scr.Datacapture_Status = dr["Datacapture_Status"].ToString();
                scr.num_of_schools = dr["num_of_schools"].ToString();
                scr.MAX_REVIEWS = dr["MAX_REVIEWS"].ToString();
                scr.review_started = dr["review_started"].ToString();
                scr.Number_of_Schools = dr["Number_of_Schools"].ToString();
                scr.EducationLevel = dr["EducationLevel"].ToString();
                scr.HighSchoolGradYear = dr["HighSchoolGradYear"].ToString();
                scr.DegreeStartTimeframe = dr["DegreeStartTimeframe"].ToString();
                scr.Expr3 = dr["Expr3"].ToString();
                scr.First_Name = dr["First_Name"].ToString();
                scr.Last_Name = dr["Last_Name"].ToString();
                scr.address = dr["address"].ToString();
                scr.phone = dr["phone"].ToString();
                scr.call_date = dr["call_date"].ToString();
                scr.audio_link = Common.GetAudioFileName(dr);
                scr.profile_id = dr["profile_id"].ToString();
                scr.audio_user = ""; 
                scr.audio_password = ""; 
                scr.LIST_NAME = dr["LIST_NAME"].ToString();
                scr.review_date = dr["review_date"].ToString();
                scr.CAMPAIGN = dr["CAMPAIGN"].ToString();
                scr.DISPOSITION = dr["DISPOSITION"].ToString();
                scr.bad_call = dr["bad_call"].ToString();
                scr.to_upload = ""; 
                scr.SESSION_ID = dr["SESSION_ID"].ToString();
                scr.agent_deviation = ""; 
                scr.pass_fail = dr["pass_fail"].ToString();
                scr.scorecard = dr["scorecard"].ToString();
                scr.scorecard_name = dr["scorecard_name"].ToString();
                scr.uploaded = ""; 
                scr.formatted_comments = dr["formatted_comments"].ToString();
                scr.formatted_missed = dr["formatted_missed"].ToString();
                scr.fileUrl = dr["fileUrl"].ToString();
                scr.statusMessage = dr["statusMessage"].ToString();
                scr.mediaId = ""; // dr("mediaId").ToString()
                scr.requestStatus = ""; // dr("requestStatus").ToString()
                scr.fileStatus = ""; // dr("fileStatus").ToString()
                scr.response = "";
                scr.review_time = ""; 
                scr.wasEdited = dr["wasEdited"].ToString();
                scr.website = dr["website"].ToString();
                scr.pending_id = ""; 
                scr.bad_call_reason = dr["bad_call_reason"].ToString();
                scr.date_added = dr["date_added"].ToString();
                scr.calib_score = dr["calib_score"].ToString();
                scr.edited_score = dr["edited_score"].ToString();
                scr.compliance_sheet = dr["compliance_sheet"].ToString();
                scr.editable = true;
                try
                {
                    wasEdited =Convert.ToBoolean( dr["wasEdited"]);
                }
                catch (Exception ex)
                {
                    wasEdited = false;
                }
                if ((uo.UserRole == "QA" & (dr["calib_score"].ToString() != "" | dr["edited_score"].ToString() != "")) | non_edit == "True" | uo.UserRole == "Agent")
                    scr.editable = false;
                List<ClerkedData> CDs = new List<ClerkedData>();
                DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + dr["F_ID"].ToString());
                foreach (DataRow qdr in cd_dt.Rows)
                {
                    ClerkedData qr = new ClerkedData();
                    qr.value = qdr["value"].ToString();
                    qr.data = qdr["value_data"].ToString();
                    qr.position = qdr["value_position"].ToString();
                    qr.ID = qdr["value_id"].ToString();
                    CDs.Add(qr);
                }
                scr.ClerkedDataItems = CDs;
                ScorecardData scd = new ScorecardData();
                UserObject scu = new UserObject();
                scu.UserRole = "QA";
                scu.UserTitle = "QA Response";
                scd.ScorecardUser = scu;
                CallScores cs = new CallScores();
                if (dr["calib_score"].ToString() == "" & dr["edited_score"].ToString() == "")
                    cs.score = Convert.IsDBNull(dr["display_score"])? "N/A" : dr["display_score"].ToString();
                else
                    cs.score = Convert.ToString(Convert.IsDBNull(dr["original_qa_score"])? Convert.ToString( Convert.IsDBNull(dr["total_score"])? "N/A" : dr["total_score"].ToString()): dr["original_qa_score"].ToString());
                cs.reviewer = dr["reviewer"].ToString();
                cs.scoredate =Convert.ToDateTime(dr["review_date"]).ToShortDateString();
                if (!Information.IsDBNull(dr["qa_cali_score"]))
                    cs.calibrationscore = dr["qa_cali_score"].ToString();
                cs.role = "QA";
                scd.CallScore = cs;

                List<string> audio_list = new List<string>();
                if (dr["phone"].ToString() == "")
                    dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where (WAV_DATA.session_id = '" + dr["session_id"] + "')  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");
                else
                    dt = GetTable("select max(wav_data.ID) as WID, filename, url_prefix, recording_user, record_password, session_id  from WAV_DATA  join app_settings on WAV_DATA.appname = app_settings.appname where ((WAV_DATA.session_id = '" + dr["session_id"] + "') or (WAV_DATA.filename like  '%" + dr["phone"] + "%' ))  and app_settings.appname = '" + dr["appname"] + "' and  WAV_DATA.appname = '" + dr["appname"] + "' group by filename, url_prefix, recording_user, record_password, session_id");

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
                        qr.comments_allowed =Convert.ToBoolean( qdr["comments_allowed"]);
                        if (qdr["right_answer"].ToString() == "")
                            qr.RightAnswer =Convert.ToBoolean(0);
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
                cs.score = Convert.IsDBNull(dr["cali_form_score"])? "0": dr["cali_form_score"].ToString();
                cs.reviewer = dr["reviewed_by"].ToString();

                if (dr["user_role"].ToString() == "Calibrator")
                    cs.calibrationscore = dr["cal_recal_score"].ToString();
                else
                    cs.calibrationscore = dr["total_score"].ToString();
                cs.scoredate = Convert.ToDateTime( dr["review_date"]).ToShortDateString();
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
                        qr.QID =Convert.ToInt32(qdr["q_id"]);
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
                cs.score = Convert.IsDBNull(dr["total_score"])? "0" : dr["total_score"].ToString();
                cs.reviewer = dr["reviewed_by"].ToString();
                cs.scoredate = Convert.ToDateTime( dr["review_date"]).ToShortDateString();
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
                        qr.comments_allowed =Convert.ToBoolean( qdr["comments_allowed"]);
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
                    cs.score = Convert.IsDBNull(dr["total_score"])? "0" : dr["total_score"].ToString();
                    cs.reviewer = dr["username"].ToString();
                    cs.scoredate =Convert.ToDateTime( dr["review_date"]).ToShortDateString();
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
                            qr.QID =Convert.ToInt32( qdr["q_id"]);
                            try
                            {
                                qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                            }
                            catch (Exception ex)
                            {
                            }
                            qr.QType = qdr["q_type"].ToString();
                            qr.ViewLink = qdr["view_link"].ToString();
                            qr.comments_allowed =Convert.ToBoolean( qdr["comments_allowed"]);
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
            List<string> buttons = new List<string>(strButton.Split('|')) ;
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

            return scr;
        }


        /// <summary>
        /// GetCallsLoaded
        /// </summary>
        /// <param name="CL"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public List<CallLoaded> GetCallsLoaded(CallsLoaded CL)
        {
            string appname = HttpContext.Current.Request["appname"];
            List<CallLoaded> myCLs = new List<CallLoaded>();
            if (Information.IsDate(CL.loaded_date))
            {
                DataTable dt = GetTable("select * from xcc_report_new where call_date = '" + CL.loaded_date + "' and appname = '" + appname + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    CallLoaded call_loaded = new CallLoaded();
                    call_loaded.session_id = dr["session_id"].ToString();
                    call_loaded.phone = dr["phone"].ToString();
                    call_loaded.call_date = dr["call_date"].ToString();
                    call_loaded.date_added = dr["date_added"].ToString();
                    call_loaded.audio_link = dr["audio_link"].ToString();
                    call_loaded.CC_ID = dr["ID"].ToString();
                    if (dr["max_reviews"].ToString() == "0")
                        call_loaded.status = "Pending";
                    if (dr["max_reviews"].ToString() == "1")
                        call_loaded.status = "Worked";
                    if (dr["bad_call"].ToString() == "1")
                        call_loaded.status = "Bad Call";
                    myCLs.Add(call_loaded);
                }
              
            }
            return myCLs;
        }

        /// <summary>
        /// GetRecord
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public List<CallRecord> GetRecord(SimpleID SI)
        {
            string SESSION_ID = SI.ID;
            List<CallRecord> cr = new List<CallRecord>();

            if (SESSION_ID == "")
                return cr;
            Common.UpdateTable("insert into api_access (ip, value, value_type, posted_appname) select '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + SESSION_ID + "','session_id','" + HttpContext.Current.Request["appname"] + "'");
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string add_sql;
            if (HttpContext.Current.Request.QueryString["appname"] != null)
                add_sql = " and xcc_report_new.appname = '" + HttpContext.Current.Request["appname"] + "'";
            else
                add_sql = " and xcc_report_new.scorecard in (select user_scorecard from userapps where username= '" + HttpContext.Current.User.Identity.Name + "') ";
            SqlCommand reply = new SqlCommand("select form_score3.id as F_ID, xcc_report_new.id  as X_ID,  * from xcc_report_new left join form_score3 on review_id  = xcc_report_new.id  where xcc_report_new.session_id = @session_id and isnull(review_date, dbo.getMTdate()) > dateadd(d, -30, getdate()) and max_reviews = 1 " + add_sql, cn);

            if (HttpContext.Current.Request.QueryString["appname"] != null)
            {
                if (HttpContext.Current.Request.QueryString["appname"] == "inside up")
                    reply = new SqlCommand("select form_score3.id as F_ID,  xcc_report_new.id  as X_ID, * from xcc_report_new left join form_score3 on review_id  = xcc_report_new.id  where profile_ID = @session_id  and max_reviews = 1  and isnull(review_date, dbo.getMTdate()) > dateadd(d, -30, getdate()) " + add_sql, cn);
            }

            reply.Parameters.AddWithValue("session_id", SESSION_ID);
            adapter.SelectCommand = reply;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CallRecord scr = new CallRecord();
                scr.F_ID = dr["F_ID"].ToString();
                scr.review_ID = dr["review_ID"].ToString();
                scr.Comments = dr["Comments"].ToString();
                scr.autofail = dr["autofail"].ToString();
                scr.reviewer = dr["reviewer"].ToString();
                scr.appname = dr["appname"].ToString();
                scr.total_score = dr["display_score"].ToString();
                scr.total_score_with_fails = dr["display_score"].ToString();
                scr.call_length = dr["call_length"].ToString();
                scr.has_cardinal = ""; // dr("has_cardinal").ToString()
                scr.fs_audio = dr["fs_audio"].ToString();
                scr.week_ending_date = dr["week_ending_date"].ToString();
                scr.num_missed = dr["num_missed"].ToString();
                scr.missed_list = dr["missed_list"].ToString();
                scr.call_made_date = dr["call_made_date"].ToString();
                scr.AGENT = dr["AGENT"].ToString();
                scr.ANI = dr["ANI"].ToString();
                scr.DNIS = dr["DNIS"].ToString();
                scr.TIMESTAMP = dr["TIMESTAMP"].ToString();
                scr.TALK_TIME = dr["TALK_TIME"].ToString();
                scr.CALL_TIME = dr["CALL_TIME"].ToString();
                scr.CALL_TYPE = dr["CALL_TYPE"].ToString();
                scr.leadid = dr["leadid"].ToString();
                scr.AGENT_GROUP = dr["AGENT_GROUP"].ToString();
                scr.Email = dr["Email"].ToString();
                scr.City = dr["City"].ToString();
                scr.State = dr["State"].ToString();
                scr.Zip = dr["Zip"].ToString();
                scr.Datacapturekey = dr["Datacapturekey"].ToString();
                scr.Datacapture = dr["Datacapture"].ToString();
                scr.Status = dr["Status"].ToString();
                scr.Program = dr["Program"].ToString();
                scr.X_ID = dr["X_ID"].ToString();
                scr.Datacapture_Status = dr["Datacapture_Status"].ToString();
                scr.num_of_schools = dr["num_of_schools"].ToString();
                scr.MAX_REVIEWS = dr["MAX_REVIEWS"].ToString();
                scr.review_started = ""; // dr("review_started").ToString()
                scr.Number_of_Schools = dr["Number_of_Schools"].ToString();
                scr.EducationLevel = dr["EducationLevel"].ToString();
                scr.HighSchoolGradYear = dr["HighSchoolGradYear"].ToString();
                scr.DegreeStartTimeframe = dr["DegreeStartTimeframe"].ToString();

                scr.First_Name = dr["First_Name"].ToString();
                scr.Last_Name = dr["Last_Name"].ToString();
                scr.address = dr["address"].ToString();
                scr.phone = dr["phone"].ToString();
                scr.call_date = dr["call_date"].ToString();
                scr.audio_link = dr["audio_link"].ToString();
                scr.profile_id = dr["profile_id"].ToString();
                scr.audio_user = ""; // dr("audio_user").ToString()
                scr.audio_password = ""; // dr("audio_password").ToString()
                scr.LIST_NAME = dr["LIST_NAME"].ToString();
                scr.review_date = dr["review_date"].ToString();
                scr.CAMPAIGN = dr["CAMPAIGN"].ToString();
                scr.DISPOSITION = dr["DISPOSITION"].ToString();
                scr.bad_call = dr["bad_call"].ToString();
                scr.to_upload = ""; // dr("to_upload").ToString()
                scr.SESSION_ID = dr["SESSION_ID"].ToString();
                scr.agent_deviation = ""; // dr("agent_deviation").ToString()
                scr.pass_fail = dr["pass_fail"].ToString();
                scr.scorecard = dr["scorecard"].ToString();
                scr.uploaded = ""; // dr("uploaded").ToString()
                scr.formatted_comments = dr["formatted_comments"].ToString();
                scr.formatted_missed = dr["formatted_missed"].ToString();
                scr.fileUrl = ""; // dr("fileUrl").ToString()
                scr.statusMessage = dr["statusMessage"].ToString();
                scr.mediaId = ""; // dr("mediaId").ToString()
                scr.requestStatus = ""; // dr("requestStatus").ToString()
                scr.fileStatus = ""; // dr("fileStatus").ToString()
                scr.response = ""; // dr("response").ToString()
                scr.review_time = ""; // dr("review_time").ToString()
                scr.wasEdited = dr["wasEdited"].ToString();
                scr.website = dr["website"].ToString();
                scr.pending_id = ""; // dr("pending_id").ToString()
                scr.bad_call_reason = dr["bad_call_reason"].ToString();
                scr.date_added = dr["date_added"].ToString();
                scr.calib_score = dr["calib_score"].ToString();
                scr.edited_score = dr["edited_score"].ToString();
                scr.compliance_sheet = dr["compliance_sheet"].ToString();

                if (dr["F_ID"].ToString() == "")
                    scr.F_ID = "0";
                List<ScorecardResponse> qrs = new List<ScorecardResponse>();
                DataTable qdt;
                if (scr.wasEdited == "" & scr.calib_score != "")
                    qdt = GetTable("select * from calibration_scores join questions on questions.id = calibration_scores.question_id  join question_answers on question_answers.ID = calibration_scores.question_result where form_id = (select top 1 id from vwCF where f_id = " + scr.F_ID + " and active_cali = 1) order by questions.q_order");
                else
                    qdt = GetTable("select * from form_q_scores join questions on questions.id = form_q_scores.question_id  join question_answers on question_answers.ID = form_q_scores.question_answered where form_id = " + scr.F_ID + " order by questions.q_order");
                foreach (DataRow qdr in qdt.Rows)
                {
                    ScorecardResponse qr = new ScorecardResponse();
                    if (scr.wasEdited.ToString() == "" & scr.calib_score != "")
                        qr.position = qdr["q_pos"].ToString();
                    else
                        qr.position = qdr["q_position"].ToString();

                    qr.question = qdr["q_short_name"].ToString();
                    qr.result = qdr["answer_text"].ToString();
                    qr.QID = Convert.ToInt32(qdr["question_id"]);
                    qr.comments_allowed =Convert.ToBoolean(qdr["comments_allowed"]);
                    try
                    {
                        qr.QAPoints = Convert.ToInt32(qdr["QA_points"]);
                    }
                    catch (Exception ex)
                    {
                        qr.QAPoints = 0;
                    }
                    qr.ViewLink = qdr["view_link"].ToString();
                    qr.RightAnswer = Convert.ToBoolean(qdr["right_answer"]);
                    DataTable ans_dt;
                    if (scr.wasEdited == "" & scr.calib_score != "")
                        ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_c_responses left join answer_comments on form_c_responses.answer_id = answer_comments.id where form_c_responses.form_id = (select id from vwCF where f_id = " + dr["F_ID"].ToString() + " and active_cali = 1) and form_c_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");
                    else
                        ans_dt = GetTable("select isnull(comment,other_answer_text) as q_comment from form_q_responses left join answer_comments on form_q_responses.answer_id = answer_comments.id where form_q_responses.form_id = " + dr["F_ID"].ToString() + " and form_q_responses.question_id = " + qdr["question_id"].ToString() + " order by isnull(ac_order,10000)");

                    List<string> ans_comment = new List<string>();
                    if (ans_dt.Rows.Count > 0)
                    {
                        foreach (DataRow ans_dr in ans_dt.Rows)
                            ans_comment.Add(ans_dr["q_comment"].ToString());
                    }
                    qr.QComments = ans_comment;

                    qrs.Add(qr);
                }
                scr.ScorecardResponses = qrs;
                List<ClerkedData> CDs = new List<ClerkedData>();
                DataTable cd_dt = GetTable("select * from collected_data join sc_inputs on value_id = sc_inputs.id where form_id = " + scr.F_ID);

                foreach (DataRow qdr in cd_dt.Rows)
                {
                    ClerkedData qr = new ClerkedData();
                    qr.value = qdr["value"].ToString();
                    qr.data = qdr["value_data"].ToString();
                    qr.position = qdr["value_position"].ToString();
                    qr.ID = qdr["value_id"].ToString();
                    CDs.Add(qr);
                }
                scr.ClerkedDataItems = CDs;
                cr.Add(scr);
            }

            cn.Close();
            cn.Dispose();
            return cr;
        }

        /// <summary>
        /// getScore
        /// </summary>
        /// <param name="gsd"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public List<SessionStatus> getScore(getScoreData gsd)
        {
            string session_id = gsd.session_id;
            string username = gsd.username;

            List<SessionStatus> ss = new List<SessionStatus>();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            SqlDataAdapter reply = new SqlDataAdapter("Select * from xcc_report_new where session_id = @session_id And scorecard In (Select user_scorecard from userapps where username = @username)", cn);
            reply.SelectCommand.Parameters.AddWithValue("session_id", session_id);
            reply.SelectCommand.Parameters.AddWithValue("username", username);
            DataTable ss_dt = new DataTable();
            reply.Fill(ss_dt);
            foreach (DataRow dr in ss_dt.Rows)
            {
                SessionStatus ss_obj = new SessionStatus();
                DataTable vw_dt = GetTable("Select isnull(isnull(edited_score,calib_score),total_score) As theScore from vwForm where review_id = " + dr["id"].ToString());
                ss_obj.score = "N/A";
                switch (System.Convert.ToInt32(dr["max_reviews"]))
                {
                    case 0:
                        {
                            ss_obj.status = "RECEIVED";
                            break;
                        }

                    case 1:
                        {
                            ss_obj.status = "PROCESSED";
                            if (vw_dt.Rows.Count > 0)
                                ss_obj.score = vw_dt.Rows[0][0].ToString();
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

                if (dr["audio_link"].ToString() == "")
                    ss_obj.status = "WAITING/CONVERTING AUDIO";
                if (dr["bad_call"].ToString() == "1")
                    ss_obj.status = "BAD Call/UNABLE To SCORE - " + dr["bad_call_reason"].ToString();

                ss.Add(ss_obj);
            }
            return ss;
        }

       
        /// <summary>
        /// GetScorecardRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public CompleteScorecard GetScorecardRecordID(SimpleID SI)
        {
            DataTable fid_dt = GetTable("select review_id, scorecard from vwForm where f_id = " + SI.ID);

            string scorecard_ID = fid_dt.Rows[0]["scorecard"].ToString();
            string xcc_id = fid_dt.Rows[0]["review_id"].ToString();
            CompleteScorecard sc = new CompleteScorecard();

            if (!Information.IsNumeric(xcc_id))
                xcc_id = "";

            if (scorecard_ID == "")
                return sc;


            string add_sql;
            if (HttpContext.Current.Request.QueryString["appname"] != null)
                add_sql = " and appname = '" + HttpContext.Current.Request.QueryString["appname"] + "'";
            else
                add_sql = " and appname in (select appname from userapps where username= '" + HttpContext.Current.User.Identity.Name + "') ";
            DataTable sc_dt = GetTable("select * from scorecards where id = " + scorecard_ID + " " + add_sql);
            if (sc_dt.Rows.Count == 0)
            {
                sc.ScorecardName = "No data/No authorized.";
                return sc;
            }
            sc.ScorecardName = sc_dt.Rows[0]["short_name"].ToString();
            sc.Appname = sc_dt.Rows[0]["appname"].ToString();
            sc.Status = sc_dt.Rows[0]["scorecard_status"].ToString();
            sc.Description = sc_dt.Rows[0]["description"].ToString();
            List<Section> sec_list = new List<Section>();
            DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
            foreach (DataRow dr in section_dt.Rows)
            {
                Section sec = new Section();
                sec.section = dr["section"].ToString();
                sec.description = dr["Descrip"].ToString();
                DataTable q_dt;

                if (xcc_id != "")
                    q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                else
                    q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID);
                List<Question> ques_list = new List<Question>();
                foreach (DataRow drq in q_dt.Rows)
                {
                    Question ques = new Question();
                    ques.QuestionShort = drq["q_short_name"].ToString();
                    ques.question = drq["question"].ToString();
                    ques.LinkedAnswer = drq["linked_answer"].ToString();
                    ques.LinkedComment = drq["linked_comment"].ToString();
                    try
                    {
                        ques.QAPoints =Convert.ToInt32( drq["QA_points"]);
                    }
                    catch (Exception ex)
                    {
                        ques.QAPoints = 0;
                    }

                    ques.comments_allowed =Convert.ToBoolean( drq["comments_allowed"]);
                    ques.QID = Convert.ToInt32(drq["ID"]);

                    //if (drq["template"].ToString() == "Contact")
                        //ques.TemplateOptions = drq["template_items"].ToString().Split('|').ToList();
                    DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq["id"]);
                    List<Answer> ans_list = new List<Answer>();
                    foreach (DataRow dra in ans_dt.Rows)
                    {
                        Answer ans = new Answer();
                        ans.Answers = dra["answer_text"].ToString();
                        try
                        {
                            ans.Points = Convert.ToInt32(dra["answer_points"]);
                        }
                        catch (Exception ex)
                        {
                            ans.Points = 0;
                        }
                        ans.RightAnswer = Convert.ToBoolean(dra["right_answer"]);
                        if (dra["autoselect"].ToString() == "True")
                            ans.autoselect = 1;
                        else
                            ans.autoselect = 0;
                        ans.AnswerID = Convert.ToInt32(dra["ID"]);
                        DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                        List<Comment> cmt_list = new List<Comment>();

                        foreach (DataRow drcmt in cmt_dt.Rows)
                        {
                            Comment cmt = new Comment();
                            cmt.CommentText = drcmt["comment"].ToString();
                            cmt.CommentID = Convert.ToInt32(drcmt["ID"]);
                            cmt.CommentPoints = Convert.IsDBNull(drcmt["comment_points"])? new float():(float)drcmt["comment_points"];
                            cmt_list.Add(cmt);
                        }
                        ans.Comments = cmt_list;
                        ans_list.Add(ans);
                    }
                    ques.answers = ans_list;
                    List<Instruction> instr_list = new List<Instruction>();
                    DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                    foreach (DataRow drinst in instr_dt.Rows)
                    {
                        Instruction instr = new Instruction();
                        instr.InstructionText = drinst["question_text"].ToString();
                        instr_list.Add(instr);
                    }
                    ques.instructions = instr_list;

                    List<FAQ> faq_list = new List<FAQ>();

                    DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                    foreach (DataRow drfaq in faq_dt.Rows)
                    {
                        FAQ faq = new FAQ();
                        faq.QuestionText = drfaq["question_text"].ToString();
                        faq.AnswerText = drfaq["question_answer"].ToString();
                        faq_list.Add(faq);
                    }
                    ques.FAQs = faq_list;
                    ques_list.Add(ques);
                }
                sec.questions = ques_list;

                if (sec.questions.Count > 0)
                    sec_list.Add(sec);
            }

            if (xcc_id != "" & 1 == 0)
            {
                DataTable cq_dt = GetTable("Select  distinct data_key,label, school_name,data_type,data_value from otherformdata where xcc_id = '" + xcc_id + "' and data_type = 'customquestion'"); // 
                if (cq_dt.Rows.Count > 0)
                {
                    Section sec = new Section();
                    sec.section = "Custom Questions";
                    sec.description = "Custom Questions";
                    List<Question> ques_list = new List<Question>();
                    foreach (DataRow cq_dr in cq_dt.Rows)
                    {
                        Question ques = new Question();
                        ques.QuestionShort = cq_dr["data_key"].ToString();
                        ques.question = cq_dr["label"].ToString();
                        ques.LinkedAnswer = "";
                        ques.LinkedComment = "";
                        ques.QID = '-' + Convert.ToInt32(cq_dr["id"]);

                        List<Answer> ans_list = new List<Answer>();
                        Answer ans = new Answer();
                        ans.Answers = "Yes";
                        ans.Points = 0;
                        ans.RightAnswer =Convert.ToBoolean( "True");
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
                        ques.FAQs = new List<FAQ>();
                        ques.instructions = new List<Instruction>();
                        ques.answers = ans_list;
                        ques_list.Add(ques);
                    }
                    sec.questions = ques_list;
                    sec_list.Add(sec);
                }
            }

            sc.Sections = sec_list;
            DataTable clerk_dt = GetTable("Select * from sc_inputs where scorecard = " + scorecard_ID + " And active = 1 order by value_order");
            if (clerk_dt.Rows.Count > 0)
            {
                List<ClerkedData> cds = new List<ClerkedData>();
                foreach (DataRow clerk_item in clerk_dt.Rows)
                {
                    ClerkedData cd = new ClerkedData();
                    cd.value = clerk_item["value"].ToString();
                    cd.ID = clerk_item["id"].ToString();
                    try
                    {
                        cd.required =Convert.ToBoolean( clerk_item["required_value"]);
                    }
                    catch (Exception ex)
                    {
                    }
                    cds.Add(cd);
                }
                sc.ClerkData = cds;
            }

            return sc;
        }

        /// <summary>
        /// GetScorecardRecord
        /// </summary>
        /// <param name="gscd"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public CompleteScorecard GetScorecardRecord(getSCRecData gscd)
        {
            string scorecard_ID = gscd.scorecard_ID;
            string xcc_id = gscd.xcc_id;
            CompleteScorecard sc = new CompleteScorecard();

            if (!Information.IsNumeric(xcc_id))
                xcc_id = "";

            if (scorecard_ID == "")
                return sc;
            string add_sql;
            if (HttpContext.Current.Request.QueryString["appname"] != null)
                add_sql = " and appname = '" + HttpContext.Current.Request.QueryString["appname"] + "'";
            else
                add_sql = " and appname in (select appname from userapps where username= '" + HttpContext.Current.User.Identity.Name + "') ";
            DataTable sc_dt = GetTable("select * from scorecards where id = " + scorecard_ID + " " + add_sql);

            if (sc_dt.Rows.Count == 0)
            {
                sc.ScorecardName = "No data/No authorized.";
                return sc;
            }
            sc.ScorecardName = sc_dt.Rows[0]["short_name"].ToString();
            sc.Appname = sc_dt.Rows[0]["appname"].ToString();
            sc.Status = sc_dt.Rows[0]["scorecard_status"].ToString();
            sc.Description = sc_dt.Rows[0]["description"].ToString();
            List<Section> sec_list = new List<Section>();
            DataTable section_dt = GetTable("select sections.ID, sections.section, Descrip from sections where id in (select  section from questions  where scorecard_id = " + scorecard_ID + " and questions.active = 1) and  scorecard_id = " + scorecard_ID + " order by section_order");
            foreach (DataRow dr in section_dt.Rows)
            {
                Section sec = new Section();
                sec.section = dr["section"].ToString();
                sec.description = dr["Descrip"].ToString();
                DataTable q_dt;
                if (xcc_id != "")
                    q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID + "," + xcc_id);
                else
                    q_dt = GetTable("exec getListenQuestions " + dr["id"].ToString() + ", " + scorecard_ID);

                List<Question> ques_list = new List<Question>();
                foreach (DataRow drq in q_dt.Rows)
                {
                    Question ques = new Question();
                    ques.QuestionShort = drq["q_short_name"].ToString();
                    ques.question = drq["question"].ToString();
                    ques.LinkedAnswer = drq["linked_answer"].ToString();
                    ques.LinkedComment = drq["linked_comment"].ToString();
                    try
                    {
                        ques.QAPoints =Convert.ToInt32( drq["QA_points"]);
                    }
                    catch (Exception ex)
                    {
                        ques.QAPoints = 0;
                    }

                    ques.comments_allowed = Convert.ToBoolean(drq["comments_allowed"]);
                    ques.QID = Convert.ToInt32(drq["ID"]);

                    //if (drq["template"].ToString() == "Contact")
                        //ques.TemplateOptions = drq["template_items"].ToString().Split('|').ToList();

                    DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq["id"]);
                    List<Answer> ans_list = new List<Answer>();
                    foreach (DataRow dra in ans_dt.Rows)
                    {
                        Answer ans = new Answer();
                        ans.Answers = dra["answer_text"].ToString();
                        try
                        {
                            ans.Points = Convert.ToInt32(dra["answer_points"]);
                        }
                        catch (Exception ex)
                        {
                            ans.Points = 0;
                        }
                        ans.RightAnswer = Convert.ToBoolean(dra["right_answer"]);

                        if (dra["autoselect"].ToString() == "True")
                            ans.autoselect = 1;
                        else
                            ans.autoselect = 0;
                        ans.AnswerID = Convert.ToInt32(dra["ID"]);
                        DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                        List<Comment> cmt_list = new List<Comment>();
                        foreach (DataRow drcmt in cmt_dt.Rows)
                        {
                            Comment cmt = new Comment();
                            cmt.CommentText = drcmt["comment"].ToString();
                            cmt.CommentID = Convert.ToInt32(drcmt["ID"]);
                            cmt.CommentPoints = Convert.IsDBNull(drcmt["comment_points"])? new float() : (float)drcmt["comment_points"];
                            cmt_list.Add(cmt);
                        }

                        ans.Comments = cmt_list;
                        ans_list.Add(ans);
                    }
                    ques.answers = ans_list;
                    List<Instruction> instr_list = new List<Instruction>();
                    DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                    foreach (DataRow drinst in instr_dt.Rows)
                    {
                        Instruction instr = new Instruction();
                        instr.InstructionText = drinst["question_text"].ToString();
                        instr_list.Add(instr);
                    }
                    ques.instructions = instr_list;
                    List<FAQ> faq_list = new List<FAQ>();
                    DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                    foreach (DataRow drfaq in faq_dt.Rows)
                    {
                        FAQ faq = new FAQ();
                        faq.QuestionText = drfaq["question_text"].ToString();
                        faq.AnswerText = drfaq["question_answer"].ToString();
                        faq_list.Add(faq);
                    }
                    ques.FAQs = faq_list;
                    ques_list.Add(ques);
                }
                sec.questions = ques_list;
                if (sec.questions.Count > 0)
                    sec_list.Add(sec);
            }

            if (xcc_id != "" & 1 == 0)
            {
                DataTable cq_dt = GetTable("Select * from otherformdata where xcc_id = '" + xcc_id + "' and data_type = 'customquestion'"); // 
                if (cq_dt.Rows.Count > 0)
                {
                    Section sec = new Section();
                    sec.section = "Custom Questions";
                    sec.description = "Custom Questions";
                    List<Question> ques_list = new List<Question>();
                    foreach (DataRow cq_dr in cq_dt.Rows)
                    {
                        Question ques = new Question();
                        ques.QuestionShort = cq_dr["data_key"].ToString();
                        ques.question = cq_dr["label"].ToString();
                        ques.LinkedAnswer = "";
                        ques.LinkedComment = "";
                        ques.QID = '-' + Convert.ToInt32(cq_dr["id"]);
                        List<Answer> ans_list = new List<Answer>();

                        Answer ans = new Answer();
                        ans.Answers = "Yes";
                        ans.Points = 0;
                        ans.RightAnswer =Convert.ToBoolean( "True");
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

                        ques.FAQs = new List<FAQ>();
                        ques.instructions = new List<Instruction>();
                        ques.answers = ans_list;

                        ques_list.Add(ques);
                    }
                    sec.questions = ques_list;
                    sec_list.Add(sec);
                }
            }

            sc.Sections = sec_list;
            DataTable clerk_dt = GetTable("Select * from sc_inputs where scorecard = " + scorecard_ID + " And active = 1 order by value_order");
            if (clerk_dt.Rows.Count > 0)
            {
                List<ClerkedData> cds = new List<ClerkedData>();
                foreach (DataRow clerk_item in clerk_dt.Rows)
                {
                    ClerkedData cd = new ClerkedData();
                    cd.value = clerk_item["value"].ToString();
                    cd.ID =clerk_item["id"].ToString();
                    try
                    {
                        cd.required = Convert.ToBoolean(clerk_item["required_value"]);
                    }
                    catch (Exception ex)
                    {
                    }

                    cds.Add(cd);
                }
                sc.ClerkData = cds;
            }
            return sc;
        }

        /// <summary>
        /// GetScorecardRecordJ
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        public CompleteScorecard GetScorecardRecordJ(GCR SI)
        {
            string scorecard_ID = SI.scorecard_ID;
            CompleteScorecard sc = new CompleteScorecard();

            if (scorecard_ID == "")
                return sc;
            string add_sql;
            if (HttpContext.Current.Request.QueryString["appname"] != null)
                add_sql = " and appname = '" + HttpContext.Current.Request["appname"] + "'";
            else
                add_sql = " and appname in (select appname from userapps where username= '" + HttpContext.Current.User.Identity.Name + "') ";

            DataTable sc_dt = GetTable("Select * from scorecards where id = " + scorecard_ID + " " + add_sql);
            sc.ScorecardName = sc_dt.Rows[0]["short_name"].ToString();
            List<Section> sec_list = new List<Section>();
            DataTable section_dt = GetTable("Select sections.ID, sections.section, Descrip from sections where id In (Select  section from questions  where scorecard_id = " + scorecard_ID + " And questions.active = 1) And  scorecard_id = " + scorecard_ID + " order by section_order");
            foreach (DataRow dr in section_dt.Rows)
            {
                Section sec = new Section();
                sec.section = dr["section"].ToString();
                sec.description = dr["Descrip"].ToString();
                DataTable q_dt = GetTable("Select q_short_name, q.question,  q.ID FROM [Questions] q  join sections On sections.id  = q.section where  q.scorecard_id = " + scorecard_ID + " And q.section = " + dr["id"].ToString() + " And q.active = 1 order by q_order");

                List<Question> ques_list = new List<Question>();
                foreach (DataRow drq in q_dt.Rows)
                {
                    Question ques = new Question();
                    ques.QuestionShort = drq["q_short_name"].ToString();
                    ques.question = drq["question"].ToString();
                    ques.QID = Convert.ToInt32(drq["id"]);
                    DataTable ans_dt = GetTable("Select * from question_answers where question_id = " + drq["id"]);
                    List<Answer> ans_list = new List<Answer>();
                    foreach (DataRow dra in ans_dt.Rows)
                    {
                        Answer ans = new Answer();
                        ans.Answers = dra["answer_text"].ToString();
                        ans.Points = Convert.ToInt32(dra["answer_points"]);
                        ans.RightAnswer = Convert.ToBoolean(dra["right_answer"]);
                        if (dra["autoselect"].ToString() == "True")
                            ans.autoselect = 1;
                        else
                            ans.autoselect = 0;
                        ans.AnswerID = Convert.ToInt32(dra["id"]);
                        DataTable cmt_dt = GetTable("Select * from answer_comments where answer_id = " + dra["id"].ToString() + " order by isnull(ac_order,10000)");
                        List<Comment> cmt_list = new List<Comment>();
                        foreach (DataRow drcmt in cmt_dt.Rows)
                        {
                            Comment cmt = new Comment();
                            cmt.CommentText = drcmt["comment"].ToString();
                            cmt.CommentID = Convert.ToInt32(drcmt["ID"]);
                            cmt_list.Add(cmt);
                        }
                        ans.Comments = cmt_list;
                        ans_list.Add(ans);
                    }
                    ques.answers = ans_list;
                    List<Instruction> instr_list = new List<Instruction>();
                    DataTable instr_dt = GetTable("Select * from q_instructions where question_id = " + drq["id"]);
                    foreach (DataRow drinst in instr_dt.Rows)
                    {
                        Instruction instr = new Instruction();
                        instr.InstructionText = drinst["question_text"].ToString();
                        instr_list.Add(instr);
                    }
                    ques.instructions = instr_list;
                    List<FAQ> faq_list = new List<FAQ>();
                    DataTable faq_dt = GetTable("Select * from q_faqs where question_id = " + drq["id"] + " order by q_order");
                    foreach (DataRow drfaq in faq_dt.Rows)
                    {
                        FAQ faq = new FAQ();
                        faq.QuestionText = drfaq["question_text"].ToString();
                        faq.AnswerText = drfaq["question_answer"].ToString();
                        faq_list.Add(faq);
                    }
                    ques.FAQs = faq_list;
                    ques_list.Add(ques);
                }
                sec.questions = ques_list;
                sec_list.Add(sec);
            }
            sc.Sections = sec_list;
            return sc;
        }

        /// <summary>
        /// CreateScorecard
        /// </summary>
        /// <param name="SC"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        [Description("If necessary, use this to create a scorecard with your internal system.")]
        public string CreateScorecard(CompleteScorecard SC)
        {
            // Get user's appname via Request 
            string appname = HttpContext.Current.Request["appname"];
            DataTable dt = GetTable("Declare @new_ID int;  insert into scorecards (short_name, Description, appname,  date_added)  Select Case '" + SC.ScorecardName + "', '" + SC.Description + "', '" + appname + "', dbo.getMTDate(); select @new_id = @@identity; select @new_ID; ");
            string new_ID = dt.Rows[0][0].ToString();
            string sql = "";
            foreach (Section section in SC.Sections)
            {
                DataTable section_dt = GetTable("declare @new_ID int;  insert into sections (section, section_order, appname, scorecard_id)  Select Case '" + section.section + "', '" + section.order + "', '" + appname + "', " + new_ID + "; select @new_id = @@identity; select @new_ID; ");

                string new_section = dt.Rows[0][0].ToString();
                foreach (Question question in section.questions)
                {
                    sql = "declare @new_ID int;  ";
                    sql += "insert into questions (question, section,  q_order, q_short_name, active, appname,  scorecard_id)";
                    sql += "'" + question.question + "',";
                    sql += "'" + new_section + "',";
                    sql += "'" + question.order + "',";
                    sql += "'" + question.QuestionShort + "',";
                    sql += "'" + question.active + "',";
                    sql += "'" + appname + "',";
                    sql += "'" + new_ID + "';";
                    sql += "select @new_id = @@identity; select @new_ID;";
                    DataTable question_dt = GetTable(sql);
                    string new_question = dt.Rows[0][0].ToString();
                    foreach (Answer q_ans in question.answers)
                    {
                        sql = "declare @new_ID int;  ";
                        sql += "insert into question_answers (question_id, answer_text, answer_points,  autoselect, right_answer)";
                        sql += "'" + new_question + "',";
                        sql += "'" + q_ans.Answers + "',";
                        sql += "'" + q_ans.Points + "',";
                        sql += "'" + q_ans.autoselect + "',";
                        sql += "'" + q_ans.RightAnswer + "';";
                        sql += "select @new_id = @@identity; select @new_ID;";
                        DataTable answer_dt = GetTable(sql);
                        string new_answer = dt.Rows[0][0].ToString();
                        foreach (var ans_comm in q_ans.Comments)
                        {
                            sql = "declare @new_ID int;  ";
                            sql += "insert into answer_comments (comment, answer_id, question_id)";
                            sql += "'" + ans_comm.CommentText + "',";
                            sql += "'" + new_answer + "',";
                            sql += "'" + new_question + "';";
                            Common.UpdateTable(sql);
                        }
                    }
                    foreach (FAQ qfaq in question.FAQs)
                    {
                        sql = "declare @new_ID int;  ";
                        sql += "insert into q_faqs (question_id, question_text,  question_answer)";
                        sql += "'" + new_question + "'";
                        sql += "'" + qfaq.QuestionText + "',";
                        sql += "'" + qfaq.AnswerText + "';";
                        Common.UpdateTable(sql);
                    }
                    foreach (Instruction qinst in question.instructions)
                    {
                        sql = "declare @new_ID int;  ";
                        sql += "insert into q_instructions (question_id, question_text,  answer_type)";
                        sql += "'" + new_question + "'";
                        sql += "'" + qinst.InstructionText + "',";
                        sql += "'" + qinst.AnswerText + "';";

                        Common.UpdateTable(sql);
                    }
                }
            }
            return new_ID.ToString();
        }


        /// <summary>
        /// PostRecord
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "GET")]
        [Description("Use this to send us all data necessary to receive calls and meta-data associated with the call.")]
        public string PostRecord()
        {
            string[] xcc_list = new[] { "SESSION_ID", "AGENT", "DISPOSITION", "CAMPAIGN", "ANI", "DNIS", "TIMESTAMP", "TALK_TIME", "CALL_TIME", "HANDLE_TIME", "CALL_TYPE", "LIST_NAME", "leadid", "AGENT_GROUP", "DATE", "Email", "City", "State", "Zip", "Datacapturekey", "Datacapture", "Status", "Program", "Datacapture_Status", "num_of_schools", "Number_of_Schools", "EducationLevel", "HighSchoolGradYear", "DegreeStartTimeframe", "appname", "First_Name", "Last_Name", "address", "phone", "call_date", "audio_link", "profile_id", "AreaOfInterest", "ProgramsOfInterestType", "Citizenship", "DegreeOfInterest", "Gender", "Military", "secondphone", "agent_name", "scorecard", "Notes", "website", "hold_time" };
            string appname = clean_string(HttpContext.Current.Request.QueryString["appname"]);
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            string adr_xml = "";
            string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();
            SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
            reply.Parameters.AddWithValue("raw_data", HttpContext.Current.Request.Url.Query);
            reply.Parameters.AddWithValue("ip_address", HttpContext.Current.Request.ServerVariables["remote_addr"]);
            Common.UpdateTable("exec add_ip '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + appname + "'");
            try
            {
                reply.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Common.Email_Error(ex.Message);
                return "Post issue";
            }
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
                    reply.Parameters.AddWithValue(qi.ToString(), clean_string(HttpContext.Current.Request.QueryString[qi.ToString()]));
                }
            }
            sql += "[DATE],";
            @params += "dbo.getMTDate(),";
            sql = Strings.Left(sql, Strings.Len(sql) - 1) + ")values(" + Strings.Left(@params, Strings.Len(@params) - 1) + "); select @xcc_id = @@identity; select @xcc_id;";
            reply.CommandText = sql;
            string new_id = "0";
            try
            {
                new_id = reply.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return "FAIL: " + ex.Message;
            }
            foreach (var qi in HttpContext.Current.Request.QueryString.Keys)
            {
               string qii = clean_string(qi.ToString());
                int indx = xcc_list.ToString().IndexOf(qii);
                if (indx == -1)
                {
                    try
                    {
                        Common.UpdateTable("insert into otherFormDataPending(form_id, data_key, data_value) select " + new_id + ",'" + qi.ToString().Replace("'", "''") + "','" + HttpContext.Current.Request.QueryString[qi.ToString()].ToString().Replace("'", "''") + "'");
                    }
                    catch (Exception ex)
                    {
                        return "FAIL: " + ex.Message;
                    }
                }
            }
            cn.Close();
            cn.Dispose();
            return "SUCCESS";
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

        /// <summary>
        /// AddRecord
        /// </summary>
        /// <param name="ADR"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST")]
        [Description("Use this to send us all data necessary to receive calls and meta-data associated with the call.")]
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
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            string adr_xml = "";
            string raw_post = OperationContext.Current.RequestContext.RequestMessage.ToString();

            SqlCommand reply = new SqlCommand("insert into flatPost(raw_data, ip_address) Select @raw_data, @ip_address", cn);
            reply.Parameters.AddWithValue("raw_data", raw_post);
            reply.Parameters.AddWithValue("ip_address", HttpContext.Current.Request.ServerVariables["remote_addr"]);

            Common.UpdateTable("exec add_ip '" + HttpContext.Current.Request.ServerVariables["remote_addr"] + "','" + appname + "'");
            try
            {
                reply.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Common.Email_Error(ex.Message);
                return "Post issue";
            }
            if (appname != null)
            {
                if (appname.ToString().ToLower() == "inside up")
                {
                    profile_id = leadid;
                    leadid = null;
                }
            }
            if (clean_string(HttpContext.Current.Request["appname"]) != appname)
                return "Invalid appname/apikey to post data with.";

            string[] domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Split('.');
            string sql;
            string @params = "";
            if (appname != "")
                sql = "declare @xcc_id int; insert into xcc_report_new_pending(";
            else
            {
                sql = "declare @xcc_id int; insert into xcc_report_new_pending(appname,";
                @params = "'" + domain[0] + "',";
            }

            if (SESSION_ID != "")
            {
                sql += "SESSION_ID,";
                @params += "@SESSION_ID,";
                reply.Parameters.AddWithValue("SESSION_ID", SESSION_ID);
            }

            if (call_date != "")
            {
                sql += "call_date,";
                @params += "@call_date,";
                reply.Parameters.AddWithValue("call_date", call_date);
            }

            if (Citizenship != "")
            {
                sql += "Citizenship,";
                @params += "@Citizenship,";
                reply.Parameters.AddWithValue("Citizenship", Citizenship);
            }

            if (Military != "")
            {
                sql += "Military,";
                @params += "@Military,";
                reply.Parameters.AddWithValue("Military", Military);
            }

            if (scorecard != "" & Information.IsNumeric(scorecard))
            {
                sql += "scorecard,";
                @params += "@scorecard,";
                reply.Parameters.AddWithValue("scorecard", scorecard);
            }

            if (AGENT != "")
            {
                sql += "AGENT,";
                @params += "@AGENT,";
                reply.Parameters.AddWithValue("AGENT", AGENT);
            }

            if (AGENT_NAME != "")
            {
                sql += "AGENT_NAME,";
                @params += "@AGENT_NAME,";
                reply.Parameters.AddWithValue("AGENT_NAME", AGENT_NAME);
            }

            if (website != "")
            {
                sql += "website,";
                @params += "@website,";
                reply.Parameters.AddWithValue("website", website);
            }

            if (DISPOSITION != "")
            {
                sql += "DISPOSITION,";
                @params += "@DISPOSITION,";
                reply.Parameters.AddWithValue("DISPOSITION", DISPOSITION);
            }
            if (CAMPAIGN != "")
            {
                sql += "CAMPAIGN,";
                @params += "@CAMPAIGN,";
                reply.Parameters.AddWithValue("CAMPAIGN", CAMPAIGN);
            }
            if (ANI != "")
            {
                sql += "ANI,";
                @params += "@ANI,";
                reply.Parameters.AddWithValue("ANI", ANI);
            }
            if (DNIS != "")
            {
                sql += "DNIS,";
                @params += "@DNIS,";
                reply.Parameters.AddWithValue("DNIS", DNIS);
            }
            if (TIMESTAMP != "")
            {
                sql += "TIMESTAMP,";
                @params += "@TIMESTAMP,";
                reply.Parameters.AddWithValue("TIMESTAMP", TIMESTAMP);
            }
            if (TALK_TIME != "")
            {
                sql += "TALK_TIME,";
                @params += "@TALK_TIME,";
                reply.Parameters.AddWithValue("TALK_TIME", TALK_TIME.Replace("nn", "00"));
            }
            if (CALL_TIME != "")
            {
                sql += "CALL_TIME,";
                @params += "@CALL_TIME,";
                reply.Parameters.AddWithValue("CALL_TIME", CALL_TIME);
            }
            if (HANDLE_TIME != "")
            {
                sql += "HANDLE_TIME,";
                @params += "@HANDLE_TIME,";
                reply.Parameters.AddWithValue("HANDLE_TIME", HANDLE_TIME);
            }
            if (CALL_TYPE != "")
            {
                sql += "CALL_TYPE,";
                @params += "@CALL_TYPE,";
                reply.Parameters.AddWithValue("CALL_TYPE", CALL_TYPE);
            }
            if (LIST_NAME != "")
            {
                sql += "LIST_NAME,";
                @params += "@LIST_NAME,";
                reply.Parameters.AddWithValue("LIST_NAME", LIST_NAME);
            }
            if (leadid != "")
            {
                sql += "leadid,";
                @params += "@leadid,";
                reply.Parameters.AddWithValue("leadid", leadid);
            }
            if (AGENT_GROUP != "")
            {
                sql += "AGENT_GROUP,";
                @params += "@AGENT_GROUP,";
                reply.Parameters.AddWithValue("AGENT_GROUP", AGENT_GROUP);
            }
            if (HOLD_TIME != "")
            {
                sql += "HOLD_TIME,";
                @params += "@HOLD_TIME,";
                reply.Parameters.AddWithValue("HOLD_TIME", HOLD_TIME);
            }
            sql += "[DATE],";
            @params += "dbo.getMTDate(),";

            if (Email != "")
            {
                sql += "Email,";
                @params += "@Email,";
                reply.Parameters.AddWithValue("Email", Email);
            }
            if (City != "")
            {
                sql += "City,";
                @params += "@City,";
                reply.Parameters.AddWithValue("City", City);
            }
            if (State != "")
            {
                sql += "State,";
                @params += "@State,";
                reply.Parameters.AddWithValue("State", State);
            }
            if (Zip != "")
            {
                sql += "Zip,";
                @params += "@Zip,";
                reply.Parameters.AddWithValue("Zip", Zip);
            }
            if (Datacapturekey != "" & Information.IsNumeric(Datacapturekey))
            {
                sql += "Datacapturekey,";
                @params += "@Datacapturekey,";
                reply.Parameters.AddWithValue("Datacapturekey", Datacapturekey);
            }
            if (Datacapture != "" & Information.IsNumeric(Datacapture))
            {
                sql += "Datacapture,";
                @params += "@Datacapture,";
                reply.Parameters.AddWithValue("Datacapture", Datacapture);
            }
            if (Status != "")
            {
                sql += "Status,";
                @params += "@Status,";
                reply.Parameters.AddWithValue("Status", Status);
            }
            if (Program != "")
            {
                sql += "Program,";
                @params += "@Program,";
                reply.Parameters.AddWithValue("Program", Program);
            }
            if (Datacapture_Status != "")
            {
                sql += "Datacapture_Status,";
                @params += "@Datacapture_Status,";
                reply.Parameters.AddWithValue("Datacapture_Status", Datacapture_Status);
            }
            if (num_of_schools != "")
            {
                sql += "num_of_schools,";
                @params += "@num_of_schools,";
                reply.Parameters.AddWithValue("num_of_schools", num_of_schools);
            }
            if (EducationLevel != "")
            {
                sql += "EducationLevel,";
                @params += "@EducationLevel,";
                reply.Parameters.AddWithValue("EducationLevel", EducationLevel);
            }
            if (HighSchoolGradYear != "")
            {
                sql += "HighSchoolGradYear,";
                @params += "@HighSchoolGradYear,";
                reply.Parameters.AddWithValue("HighSchoolGradYear", HighSchoolGradYear);
            }
            if (DegreeStartTimeframe != "")
            {
                sql += "DegreeStartTimeframe,";
                @params += "@DegreeStartTimeframe,";
                reply.Parameters.AddWithValue("DegreeStartTimeframe", DegreeStartTimeframe);
            }

            if (appname != "")
            {
                sql += "appname,";
                @params += "@appname,";
                reply.Parameters.AddWithValue("appname", appname);
            }
            if (First_Name != "")
            {
                sql += "First_Name,";
                @params += "@First_Name,";
                reply.Parameters.AddWithValue("First_Name", First_Name);
            }
            if (Last_Name != "")
            {
                sql += "Last_Name,";
                @params += "@Last_Name,";
                reply.Parameters.AddWithValue("Last_Name", Last_Name);
            }
            if (address != "")
            {
                sql += "address,";
                @params += "@address,";
                reply.Parameters.AddWithValue("address", address);
            }
            if (phone != "")
            {
                sql += "phone,";
                @params += "@phone,";
                reply.Parameters.AddWithValue("phone", phone);
            }
            if (audio_link != "")
            {
                sql += "audio_link,";
                @params += "@audio_link,";
                reply.Parameters.AddWithValue("audio_link", audio_link);
            }

            if (profile_id != "")
            {
                sql += "profile_id,";
                @params += "@profile_id,";
                reply.Parameters.AddWithValue("profile_id", profile_id);
            }

            if (sort_order != "" & Information.IsNumeric(sort_order))
            {
                sql += "sort_order,";
                @params += "@sort_order,";
                reply.Parameters.AddWithValue("sort_order", sort_order);
            }
            sql = Strings.Left(sql, Strings.Len(sql) - 1) + ")values(" + Strings.Left(@params, Strings.Len(@params) - 1) + "); select @xcc_id = @@identity; select @xcc_id;";
            reply.CommandText = sql;
            // Return sql

            string new_id = "0";
            try
            {
                new_id =reply.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                return "FAIL: " + ex.Message + " " + phone;
            }
            if (Schools != null)
            {
                foreach (SchoolItem si in Schools)
                {
                    string sql_school = "insert into school_data(pending_id,";
                    string param_scure = " @pending_id,";

                    SqlCommand reply_school = new SqlCommand(sql_school, cn);
                    reply_school.Parameters.AddWithValue("pending_id", new_id);
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
                    if (si.TCPA.ToString() != "")
                    {
                        sql_school += "TCPA,";
                        param_scure += "@TCPA,";
                        reply_school.Parameters.AddWithValue("TCPA", si.TCPA.ToString());
                    }
                    if (AOI1.ToString() != "")
                    {
                        sql_school += "AOI1,";
                        param_scure += "@AOI1,";
                        reply_school.Parameters.AddWithValue("AOI1", AOI1.ToString());
                    }
                    if (Portal.ToString() != "")
                    {
                        sql_school += "origin,";
                        param_scure += "@Portal,";
                        reply_school.Parameters.AddWithValue("Portal",Portal.ToString());
                    }


                    if (AOI2.ToString() != "")
                    {
                        sql_school += "AOI2,";
                        param_scure += "@AOI2,";
                        reply_school.Parameters.AddWithValue("AOI2", AOI2.ToString());
                    }

                    if (College.ToString() != "")
                    {
                        sql_school += "College,";
                        param_scure += "@College,";
                        reply_school.Parameters.AddWithValue("College", College.ToString());
                    }

                    if (DegreeOfInterest.ToString() != "")
                    {
                        sql_school += "DegreeOfInterest,";
                        param_scure += "@DegreeOfInterest,";
                        reply_school.Parameters.AddWithValue("DegreeOfInterest", DegreeOfInterest.ToString());
                    }

                    if (L1_SubjectName.ToString() != "")
                    {
                        sql_school += "L1_SubjectName,";
                        param_scure += "@L1_SubjectName,";
                        reply_school.Parameters.AddWithValue("L1_SubjectName", L1_SubjectName.ToString());
                    }

                    if (L2_SubjectName.ToString() != "")
                    {
                        sql_school += "L2_SubjectName,";
                        param_scure += "@L2_SubjectName,";
                        reply_school.Parameters.AddWithValue("L2_SubjectName", L2_SubjectName.ToString());
                    }

                    if (Modality.ToString() != "")
                    {
                        sql_school += "Modality,";
                        param_scure += "@Modality,";
                        reply_school.Parameters.AddWithValue("Modality", Modality.ToString());
                    }

                    if (School.ToString() != "")
                    {
                        sql_school += "School,";
                        param_scure += "@School,";
                        reply_school.Parameters.AddWithValue("School", School.ToString());
                    }

                    reply_school.CommandText = Strings.Left(sql_school, Strings.Len(sql_school) - 1) + ") select " + Strings.Left(param_scure, Strings.Len(param_scure) - 1);
                    try
                    {
                        reply_school.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Common.Email_Error("FAIL: " + ex.Message + " " + Strings.Left(sql_school, Strings.Len(sql_school) - 1) + ") select " + Strings.Left(param_scure, Strings.Len(param_scure) - 1));
                        return "FAIL: " + ex.Message + " " + Strings.Left(sql_school, Strings.Len(sql_school) - 1) + ") select " + Strings.Left(param_scure, Strings.Len(param_scure) - 1);
                    }
                }
            }

            if (audios != null)
            {
                foreach (AudioFile af in audios)
                {
                    string order = object.Equals(af.order, null) ? "0" : af.order;

                    string file_date = object.Equals(af.file_date, null) ? DateTime.Now.ToShortDateString() : af.file_date;
                    string audio_file = af.audio_file.ToString();

                    if (appname == "inside up" & af.audio_file.ToString() != "")
                        audio_file = af.audio_file.Replace("%3A", ":").Replace("%2F", "/").Replace("+", "%20");
                    
                    try
                    {
                        Common.UpdateTable("insert into AudioData(file_name, file_date, file_order, pending_id) select '" + audio_file.ToString().Replace("'", "''") + "','" + file_date.ToString().Replace("nn", "00") + "','" + order.ToString() + "'," + new_id);
                    }
                    catch (Exception ex)
                    {
                        Common.Email_Error("FAIL: " + ex.Message + " " + "insert into AudioData(file_name, file_date, file_order, pending_id) select '" + Uri.UnescapeDataString(audio_file.ToString().Replace("'", "''")) + "','" + file_date.ToString().Replace("nn", "00") + "','" + order.ToString() + "'," + new_id);
                        return "FAIL: " + ex.Message;
                    }
                }
            }

            if (OtherDataItems != null)
            {
                foreach (OtherData od in OtherDataItems)
                {
                    // Get them in order and concatenate the audio.
                    string type = object.Equals(od.type, null) ? "0" : od.type;
                   
                    try
                    {
                        if (type == "")
                           type = "string";
                        Common.UpdateTable("insert into otherFormDataPending(form_id, data_key, data_value, data_type, school_name, label) select " + new_id + ",'" + od.key.ToString().Replace("'", "''") + "','" + od.value.ToString().Replace("'", "''") + "','" + type.ToString() + "','" + od.school.ToString().Replace("'", "''") + "','" + od.label.ToString().Replace("'", "''") + "'");
                    }
                    catch (Exception ex)
                    {
                        Common.Email_Error("FAIL: " + ex.Message + " " + "insert into otherFormDataPending(form_id, data_key, data_value, data_type) select " + new_id + ",'" + od.key.ToString() + "','" + od.value.ToString() + "','" + type.ToString() + "'");
                        return "FAIL: " + ex.Message;
                    }
                }
            }
            cn.Close();
            cn.Dispose();

            return "SUCCESS";
        }

        /// <summary>
        /// RunFFMPEG
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="OutResult"></param>
        /// <param name="ErrResult"></param>
        /// <returns></returns>
        public int RunFFMPEG(string arg, ref string OutResult, ref string ErrResult)
        {
            Process myProcess = new Process();
            {
                var withBlock = myProcess.StartInfo;
                withBlock.FileName = Common.FFMPEG;
                withBlock.Arguments = arg;
                withBlock.WindowStyle = ProcessWindowStyle.Hidden;
                withBlock.CreateNoWindow = true;
                withBlock.RedirectStandardOutput = false;
                withBlock.RedirectStandardError = false;
                withBlock.UseShellExecute = false;
            }

            myProcess.Start();
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
            while (!myProcess.WaitForExit(60000) & num_tries < 20);
            if (!myProcess.HasExited)
                myProcess.Kill();
            else
                myProcess.Close();
            return 0;
        }

        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static DataTable GetTable(SqlCommand cmd)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            try
            {
                cn.Open();
                cmd.Connection = cn;
                var rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);

                return dt;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return new DataTable();
        }

        public class ListenDataRequest
        {
            public ListenDataPost LD;
            public List<FormQScores> FQS;
            public List<FormQResponses> FQR;
            public List<FormQScoresOptions> FQSO;
            public List<SystemComments> SC;
            public List<ClerkedData> CD;
        }
        public class TrainingCall
        {
            public ListenCall LC;
            public List<training_item> training_items;
        }
       
        public class NoData
        {
        }

        public class MAudio
        {
            public string xcc_id;
            public List<string> audio_url;
        }

        public class UpdateClerkedData
        {
            public string form_id;
            public string value_id;
            public string value_data;
        }

        public class UOData
        {
            public string form_id;
            public string option_list;
            public string QID;
            public string website_link;
            public string custom_comment;
        }

        public class TrainingCallRecord
        {
            public AllCallRecord acr;
            public List<training_item> training_items;
            public bool passed_training;
        }

        public class ReviewSimpleID
        {
            public string review_id;
        }

        public class SpotCheckData
        {
            public string username;
            public string f_id;
            public string disposition;
            public string notes;
            public string review_time;
        }

      
        public class UQData
        {
            public string form_id;
            public string question_id;
            public string question_answer;
        }

        public class UQAData
        {
            public string form_id;
            public string check_list;
            public string id_list;
            public string optional_text;
            public string website_link;
            public string question_id;
        }


        public class EndPointData
        {
            public int ID;
            public string username;
        }

        public class ButtonAction
        {
            public string ActionResult;
            public string ActionTask;
            public string ActionRedirect;
        }

        public class ChangeScorecardData
        {
            public int x_id;
            public string new_scorecard;
        }

        public class MarkCaliBad
        {
            public int x_id;
        }

        public struct CommentData
        {
            public string comment_header;
            public string comment;
            public string comment_who;
            public string comment_date;
            public string comment_pos;
        }


        public class disputeResult
        {
            public bool dispute_complete;
            public int dispute_id;
            public string dispute_response;
            public string dispute_redirect;
        }

    }
}