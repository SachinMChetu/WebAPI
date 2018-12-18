using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApi.Entities;
using WebApi.Models.CDService;

namespace WebApi.DataLayer
{
    /// <summary>
    /// CDServiceLayer
    /// </summary>
    public class CDServiceLayer
    {
        private Uri baseUri = new Uri("https://api.speechmatics.com/v1.0");

        // User ID: 10608 
        public string userID = System.Configuration.ConfigurationManager.AppSettings["userID"].ToString();
        // API Auth Token: ZGEzMDgyMGUtNzgxOC00NGMwLWFjMjMtMTZjZmMxMTFkN2Q2
        public string userToken = System.Configuration.ConfigurationManager.AppSettings["userToken"].ToString();
        /// <summary>
        /// getTranscriptById
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public String getTranscriptById(int jobID, string format)
        {
            NameValueCollection reqParams = new NameValueCollection();
            reqParams.Add("format", format);
            Uri uploadUri = createUserRelativeUri(String.Format("/jobs/{0}/transcript", jobID), reqParams);
            return getString(uploadUri);
        }

        /// <summary>
        /// createUserRelativeUri
        /// </summary>
        /// <param name="path"></param>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        private Uri createUserRelativeUri(String path, NameValueCollection requestParams = null)
        {
            if (requestParams == null)
            {
                requestParams = new NameValueCollection();
            }
            requestParams.Add("auth_token", userToken);
            String paramString = "?";
            foreach (string name in requestParams.Keys)
            {
                paramString += (name + Convert.ToString("=")) + requestParams[name] + "&";
            }
            return new Uri(baseUri, String.Format("/v1.0/user/{0}{1}{2}", userID.ToString(), path, paramString));
        }

        /// <summary>
        /// getSMdata
        /// </summary>
        /// <param name="sm_id"></param>
        /// <returns></returns>
        protected List<WordObject> getSMdata(string sm_id)
        {
            string resp = getTranscriptById(Convert.ToInt32(sm_id), "json");
            dynamic jResp = JsonConvert.DeserializeObject(resp);

            JArray words = jResp["words"];
            List<WordObject> wo = new List<WordObject>();
            for (int x = 0; x <= words.Count - 1; x++)
            {
                WordObject wo_object = new WordObject();
                wo_object.word = words[x]["name"].ToString();
                wo_object.position = Convert.ToInt32(words[x]["time"]);
                wo_object.confidence = Convert.ToInt32(words[x]["confidence"]);
            }
            return wo;
        }

        /// <summary>
        /// getChat
        /// </summary>
        /// <param name="session_id"></param>
        /// <returns></returns>
        public List<Chat> getChat(string session_id)
        {
            string url = "https://api.livechatinc.com/chats/" + session_id;
            WebClient client = new WebClient();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hello@callcriteria.com:eeb4009b7cd9fea2d7b60c6065129b09"));
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers.Add("X-API-Version", "2");
            string chat_data = client.DownloadString(url);

            ChatObject chat_oject = JsonConvert.DeserializeObject<ChatObject>(chat_data);
            List<Chat> wo = new List<Chat>();
            try
            {
                // Next
                foreach (var Message in chat_oject.events)
                {
                    Chat wo_object = new Chat();
                    wo_object.name = Message.author_name;
                    wo_object.date = Message.timestamp;
                    wo_object.type = Message.user_type;
                    wo_object.message = Message.text;
                    wo.Add(wo_object);
                }
            }
            catch (Exception ex)
            {
            }
            return wo;
        }

        /// <summary>
        /// getChatText
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        public List<WordObject> getChatText(string mediaid)
        {
            string url = "https://api.livechatinc.com/chats/" + mediaid;
            WebClient client = new WebClient();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("hello@callcriteria.com:eeb4009b7cd9fea2d7b60c6065129b09"));
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers.Add("X-API-Version", "2");
            string chat_data = client.DownloadString(url);

            ChatObject chat_oject = JsonConvert.DeserializeObject<ChatObject>(chat_data);
            List<WordObject> wo = new List<WordObject>();
            int x = 0;
            foreach (var Message in chat_oject.messages)
            {
                WordObject wo_object = new WordObject();
                wo_object.word = "<b>" + Message.author_name + "</b> (" + Message.user_type + "): " + Message.text + "<br>";
                wo_object.word = wo_object.word.Replace(" ", "&nbsp;");
                wo_object.position = x;
                wo_object.confidence = 100;
                x += 1;
                wo.Add(wo_object);
            }
            return wo;
        }


        /// <summary>
        /// getVBdata2
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        public List<WordObject> getVBdata2(string mediaid)
        {

            NameValueCollection nvc_header = new NameValueCollection();
            nvc_header.Add("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJlMzRkOTY1Mi04OTQwLTRkZTAtODUxNi05N2JlNmI4MmVhNTMiLCJ1c2VySWQiOiJhdXRoMHw1OGZmOTRmNTdhYWYwYjU3YmZhMWEzYzAiLCJvcmdhbml6YXRpb25JZCI6ImM1YTY4MjVjLTU0ZTEtMTFlNi1iZWI4LTllNzExMjhjYWU3NyIsImVwaGVtZXJhbCI6ZmFsc2UsImlhdCI6MTUyMzQwMDc5MTYwOCwiaXNzIjoiaHR0cDovL3d3dy52b2ljZWJhc2UuY29tIn0.eHoDDWQCWr8qkT6bI5yGe9oATjaSpela1gPwzcXLkNw");
            String media_id = mediaid;
            WebRequest web_request = null;
            WebResponse web_response = null;
            web_request = WebRequest.Create("https://apis.voicebase.com/v2-beta/media/" + mediaid + "/transcripts");
            web_request.Method = WebRequestMethods.Http.Get;
            web_request.Headers.Add(HttpRequestHeader.Authorization, "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJlMzRkOTY1Mi04OTQwLTRkZTAtODUxNi05N2JlNmI4MmVhNTMiLCJ1c2VySWQiOiJhdXRoMHw1OGZmOTRmNTdhYWYwYjU3YmZhMWEzYzAiLCJvcmdhbml6YXRpb25JZCI6ImM1YTY4MjVjLTU0ZTEtMTFlNi1iZWI4LTllNzExMjhjYWU3NyIsImVwaGVtZXJhbCI6ZmFsc2UsImlhdCI6MTUyMzQwMDc5MTYwOCwiaXNzIjoiaHR0cDovL3d3dy52b2ljZWJhc2UuY29tIn0.eHoDDWQCWr8qkT6bI5yGe9oATjaSpela1gPwzcXLkNw");
            web_request.ContentType = "application/json";
            string tmp = "";
            web_response = web_request.GetResponse();
            StreamReader reader = new StreamReader(web_response.GetResponseStream());
            tmp = reader.ReadToEnd();
            web_response.Close();
            web_response = null;
            web_request = null;
            string new_tmp = tmp;
            JObject d = JObject.Parse(new_tmp);

            JToken jp = d.SelectToken("transcripts.latest.words");
            List<WordObject> wo = new List<WordObject>();
            foreach (JToken vbti in jp)
            {
                WordObject wo_object = new WordObject();
                wo_object.word = vbti["w"].ToString().Replace("'", "`");
                wo_object.position = Convert.ToInt32(vbti["s"]) / 1000;
                wo_object.confidence = System.Convert.ToSingle(vbti["c"]);
                wo.Add(wo_object);
            }
            return wo;
        }

        /// <summary>
        /// getVBdata
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        protected List<WordObject> getVBdata(string mediaid)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("version", "1.1");
            nvc.Add("apikey", "06A3D84DfDCF94A55caf18D6DFF-A0F7D5F92C40679E9571ffd226DDD0e0d3f491697375906-1A30717875");
            nvc.Add("password", "srm2930");
            nvc.Add("action", "getTranscript");
            nvc.Add("format", "JSON");
            nvc.Add("mediaID", mediaid);
            String media_id = mediaid;
            WebRequest web_request = null;
            WebResponse web_response = null;
            web_request = WebRequest.Create("https://api.voicebase.com/services?version=1.1&apikey=06A3D84DfDCF94A55caf18D6DFF-A0F7D5F92C40679E9571ffd226DDD0e0d3f491697375906-1A30717875&password=srm2930&format=JSON&action=getTranscript&mediaID=" + mediaid);
            web_request.Method = WebRequestMethods.Http.Get;

            string tmp = "";
            try
            {
                web_response = web_request.GetResponse();
                StreamReader reader = new StreamReader(web_response.GetResponseStream());
                tmp = reader.ReadToEnd();
                web_response.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            web_response = null;
            web_request = null;

            string new_tmp = tmp.Replace("[redacted]", "redacted");
            JObject d = JObject.Parse(new_tmp);
            JToken jp = d.SelectToken("transcripts.latest.words");
            List<WordObject> wo = new List<WordObject>();
            foreach (var wordObj in jp)
            {
                try
                {
                    WordObject wo_object = new WordObject();
                    wo_object.word = wordObj["w"].ToString().Replace("'", "`");
                    wo_object.position = (float)wordObj["s"] / (float)1000.0;
                    wo_object.confidence = (float)wordObj["c"];
                    wo.Add(wo_object);
                }
                catch (Exception ex)
                {
                }
            }
            return wo;
        }

        /// <summary>
        /// GetTranscriptID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TranscriptData GetTranscriptID(string ID)
        {
            TranscriptData td = new TranscriptData();
            int Id = 0;
            if (ID != null)
            {
                Id = Convert.ToInt32(ID);
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable fid_dt = Common.GetTable("select review_id from vwFOrm where f_id = " + ID);
                var fid_dt = dataContext.vwForms.Where(x => x.F_ID == Id).FirstOrDefault();
                if (fid_dt == null)
                {
                    return td;
                }
                int? xcc_id = fid_dt.review_ID;
                if (!Information.IsNumeric(xcc_id))
                {
                    return td;
                }

                //DataTable form_dt = Common.GetTable("select sm_id, mediaId, scorecard, api_version from xcc_report_new where id = " + xcc_id);
                var form_dt = dataContext.XCC_REPORT_NEW.Where(x => x.ID == xcc_id).FirstOrDefault();
                if (form_dt != null)
                {
                    switch (Convert.ToInt32(form_dt.api_version))
                    {
                        case 1:
                            {
                                td.Words = getVBdata(form_dt.mediaId.ToString());
                                break;
                            }

                        case 2:
                            {
                                td.Words = getVBdata2(form_dt.mediaId.ToString());
                                break;
                            }
                        default:
                            {
                                return td;
                            }
                    }
                }
                List<KeywordObject> kw = new List<KeywordObject>();
                //DataTable kw_dt = Common.GetTable("select * from utterance_flags where scorecard = '" + form_dt.Rows[0][2].ToString() + "'");
                var kw_dt = dataContext.utterance_flags.Where(x => x.scorecard == form_dt.scorecard).ToList();
                foreach (var dr in kw_dt)
                {
                    KeywordObject kw_object = new KeywordObject();
                    kw_object.acceptedvariant = dr.accepted_variant;
                    kw_object.keyword = dr.Utterance;
                    kw_object.required = dr.utterance_type;
                    kw.Add(kw_object);
                }
                td.Keyword = kw;
            }
            return td;
        }

        /// <summary>
        /// GetSpotCheckData
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <param name="appname"></param>
        /// <param name="team_lead"></param>
        /// <returns></returns>
        public string GetSpotCheckData(string start_date, string end_date, string scorecard, string appname, string team_lead)
        {
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                DateTime startDate = new DateTime();
                if (start_date != null)
                {
                    startDate = Convert.ToDateTime(start_date);
                }
                DateTime endDate = new DateTime();
                if (start_date != null)
                {
                    endDate = Convert.ToDateTime(end_date);
                }
                int scoreCard = 0;
                if (scorecard != null)
                {
                    scoreCard = Convert.ToInt32(scorecard);
                }

                //var result = dataContext.Database.SqlQuery<JsonStringResult>("exe getSpotCheckData @start, @end, @scorecard, @appname, @tl",
                //    new SqlParameter("@start", startDate.Date.ToShortDateString()),
                //    new SqlParameter("@end", endDate.Date.ToShortDateString()),
                //    new SqlParameter("@scorecard", scoreCard),
                //    new SqlParameter("@appname", appname),
                //    new SqlParameter("@tl", team_lead)).ToList();


                var result1 = dataContext.Database.SqlQuery<JsonStringResult>("exec getSpotCheckData @start = {0}, @end = {1}, @scorecard = {2}, @appname = {3}, @tl = {4}",
                   startDate.Date.ToShortDateString(),
                   endDate.Date.ToShortDateString(),
                   scoreCard,
                   appname,
                   team_lead).FirstOrDefault();

                //var command = new SqlCommand()
                //{
                //    CommandText = "[dbo].[getSpotCheckData]",
                //    CommandType = CommandType.StoredProcedure
                //};

                //var parameters = new List<SqlParameter>
                //    {
                //        new SqlParameter("@start",startDate.Date.ToShortDateString()),
                //        new SqlParameter("@end",endDate.Date.ToShortDateString()),

                //         new SqlParameter("@scorecard",scoreCard),
                //        new SqlParameter("@appname",appname),
                //        new SqlParameter("@tl",team_lead)
                //    };

                //command.Parameters.AddRange(parameters.ToArray());

                // var result = dataContext.MultipleResults(command).Execute();

                //sqlComm.CommandText = "[getSpotCheckData]";
                //var dt=dataContext.getSpotCheckData(startDate,endDate, scoreCard,appname,team_lead)
                string json = "";
                //while (reader.Read())
                //json = json + reader[0];
                return json;
            }
        }


        /// <summary>
        /// GetSpotCheckData2
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        public string GetSpotCheckData2(string start_date, string end_date, string scorecard)
        {
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                DateTime startDate = new DateTime();
                if (start_date != null)
                {
                    startDate = Convert.ToDateTime(start_date);
                }
                DateTime? endDate = new DateTime();
                if (start_date != null)
                {
                    endDate = Convert.ToDateTime(end_date);
                }
                int scoreCard = 0;
                if (start_date != null)
                {
                    scoreCard = Convert.ToInt32(scorecard);
                }
                //DataTable dt = Common.GetTable(" exec getSpotCheckData '" + start_date + "','" + start_date + "','" + scorecard + "'");
                //var dt=dataContext.getSpotCheckData(startDate,endDate, scoreCard,null,null)
                return "";
            }
        }

        /// <summary>
        /// GetTranscript
        /// </summary>
        /// <param name="xcc_id"></param>
        /// <returns></returns>
        public TranscriptData GetTranscript(string xcc_id)
        {
            TranscriptData objTranscriptData = new TranscriptData();

            if (!Information.IsNumeric(xcc_id))
            {
                return objTranscriptData;
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable form_dt = Common.GetTable("select sm_id, mediaId, scorecard, api_version from xcc_report_new where id = " + xcc_id);
                int xccId = Convert.ToInt32(xcc_id);
                var form_dt = dataContext.XCC_REPORT_NEW.Where(x => x.ID == xccId).FirstOrDefault();
                if (form_dt != null)
                {
                    switch (Convert.ToInt32(form_dt.api_version))
                    {
                        case 1:
                            {
                                objTranscriptData.Words = getVBdata(form_dt.mediaId.ToString());
                                break;
                            }

                        case 2:
                            {
                                try
                                {
                                    objTranscriptData.Words = getVBdata2(form_dt.mediaId.ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                                break;
                            }

                        case -2:
                            {
                                // Try
                                objTranscriptData.Words = getChatText(form_dt.mediaId.ToString());
                                break;
                            }
                        default:
                            {
                                return objTranscriptData;
                            }
                    }
                    try
                    {
                        List<KeywordObject> kw = new List<KeywordObject>();
                        //DataTable kw_dt = Common.GetTable("select * from utterance_flags where scorecard = '" + form_dt.Rows[0][2].ToString() + "'");
                        var kw_dt = dataContext.utterance_flags.Where(x => x.scorecard == form_dt.scorecard).ToList();
                        if (kw_dt.Count > 0)
                        {
                            foreach (var dr in kw_dt)
                            {
                                KeywordObject kw_object = new KeywordObject();
                                kw_object.acceptedvariant = dr.accepted_variant;
                                kw_object.keyword = dr.Utterance;
                                kw_object.required = dr.utterance_type;
                                kw.Add(kw_object);
                            }
                        }
                        objTranscriptData.Keyword = kw;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return objTranscriptData;
        }


        /// <summary>
        /// GetActionButtons
        /// </summary>
        /// <param name="username"></param>
        /// <param name="f_id"></param>
        /// <returns></returns>
        public List<ActionButton> GetActionButtons(string username, string f_id)
        {
            List<ActionButton> abs = new List<ActionButton>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int fId = 0;
                if (f_id != "")
                {
                    fId = Convert.ToInt32(f_id);
                }
                //DataTable f_dt1 = Common.GetTable("exec GetActionButtonsData " + f_id);
                var drv = dataContext.GetActionButtonsData(fId).FirstOrDefault();
                bool show_edit = true;

                if (HttpContext.Current.User.IsInRole("Agent"))
                {
                    show_edit = false;
                }
                if (HttpContext.Current.User.IsInRole("QA"))
                {
                    if (drv.calib_score.ToString() != "" | drv.edited_score.ToString() != "")
                    {
                        show_edit = false;
                    }
                }
                if ((drv.website != "" & HttpContext.Current.User.IsInRole("QA Lead")))
                {
                    ActionButton ab = new ActionButton();
                    ab = new ActionButton();
                    ab.Action = "Reset Call";
                    ab.Endpoint = "ResetCall";
                    abs.Add(ab);
                }
                if (drv.check_reason != null && drv.close_reason != null)
                {
                    if (drv.check_reason == "QA/QA Missed Items" & (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Account Manager")) & (drv.close_reason.IndexOf("Send to QA") > -1))
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Reassign Notification";
                        ab.Endpoint = "ReassignNotification";
                        abs.Add(ab);
                    }
                }
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    ActionButton ab = new ActionButton();
                    ab.Action = "Delete Call";
                    ab.Endpoint = "DeleteCall";
                    abs.Add(ab);
                    if (drv.bad_call.ToString() == "")
                    {
                        ab = new ActionButton();
                        ab.Action = "Mark Call Bad";
                        ab.Endpoint = "MarkCallBad2";
                        abs.Add(ab);
                    }

                    ab = new ActionButton();
                    ab.Action = "Recreate Call";
                    ab.Endpoint = "RecreateCall";
                    abs.Add(ab);
                    ab = new ActionButton();
                    ab.Action = "Reset Call";
                    ab.Endpoint = "ResetCall";
                    abs.Add(ab);

                    ab = new ActionButton();
                    ab.Action = "Hide Call From Client";
                    ab.Endpoint = "HideCall";
                    abs.Add(ab);
                }

                if (HttpContext.Current.User.IsInRole("QALead") | HttpContext.Current.User.IsInRole("Calibrator") | HttpContext.Current.User.IsInRole("Recalibrator"))
                {
                    ActionButton ab = new ActionButton();
                    if (drv.bad_call.ToString() == "")
                    {
                        ab = new ActionButton();
                        ab.Action = "Mark Call Bad";
                        ab.Endpoint = "MarkCallBad2";
                        abs.Add(ab);
                    }
                }

                if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager") | HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("Account Manager"))
                {
                    if (drv.bad_call.ToString() != "" & drv.bca_date.ToString() == "")
                    {
                        ActionButton ab = new ActionButton();
                        ab = new ActionButton();
                        ab.Action = "Accept As Bad";
                        ab.Endpoint = "AcceptAsBad";
                        abs.Add(ab);
                    }
                }
                if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager"))
                {
                    ActionButton ab = new ActionButton();
                    if (drv.bad_call.ToString() == "")
                    {
                        ab = new ActionButton();
                        ab.Action = "Mark Call Bad";
                        ab.Endpoint = "MarkCallBad2";
                        abs.Add(ab);
                    }

                    ab = new ActionButton();
                    ab.Action = "Recreate Call";
                    ab.Endpoint = "RecreateCall";
                    abs.Add(ab);
                }

                if (drv.bad_call.ToString() == "" & drv.wasEdited.ToString() == "")
                {
                    // already is in calibration, don't show button
                    //DataTable cal_dt = Common.GetTable("Select count(*) from calibration_pending where form_id = " + f_id);
                    var cal_dt = dataContext.calibration_pending.Where(x => x.form_id == fId).Count();
                    if (cal_dt > 0)
                    {
                        if (!(HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("QA")) & Convert.ToInt32(cal_dt) == 0)
                        {
                            ActionButton ab = new ActionButton();
                            ab.Action = "Add Calibration";
                            ab.Endpoint = "AddCalibration";
                            abs.Add(ab);
                        }
                    }
                    // already is in calibration, don't show button
                    //DataTable c_cal_dt = Common.GetTable("Select count(*) from cali_pending_client   where form_id = " + f_id);
                    var c_cal_dt = dataContext.cali_pending_client.Where(x => x.form_id == fId).Count();
                    if (c_cal_dt == 0)
                    {
                        if ((HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager")) & Convert.ToInt32(c_cal_dt) == 0)
                        {
                            ActionButton ab = new ActionButton();
                            ab.Action = "Add Calibration";
                            ab.Endpoint = "AddClientCalibration";
                            abs.Add(ab);
                        }

                        if (HttpContext.Current.User.IsInRole("Admin") && c_cal_dt == 0)
                        {
                            ActionButton ab = new ActionButton();
                            ab.Action = "Add Client Calibration";
                            ab.Endpoint = "AddClientCalibration";
                            abs.Add(ab);
                        }
                    }
                    // already is in notifications, don't show button
                    //DataTable cali_dt = Common.GetTable("Select count(*) as num_recal, (select count(*) from vwCF where f_id = " + f_id + " and isrecal = 0) as num_cals from calibration_pending   where form_id = " + f_id + " And isrecal = 1");
                    var vwCF = dataContext.vwCFs.Where(x => x.F_ID == fId && x.isRecal == 1).Count();
                    var cali_dt = dataContext.calibration_pending.Where(x => x.form_id == fId && x.isRecal == 1).Count();
                    if (cali_dt == 0 && cali_dt == 1 & HttpContext.Current.User.IsInRole("QA"))
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Add Recalibration";
                        ab.Endpoint = "AddCaliDispute";
                        abs.Add(ab);
                    }
                }

                //DataTable client_list = Common.GetTable("exec [getClientCalibrations] '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                string userName = HttpContext.Current.User.Identity.Name.Replace("'", "''");
                var client_list = dataContext.getClientCalibrations(userName, null, null).ToList();
                foreach (var cdr in client_list)
                {
                    if (cdr.Form_ID == fId)
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Complete Review";
                        ab.Endpoint = "CompleteReview";
                        abs.Add(ab);
                    }
                }
            }
            return abs;
        }


        /// <summary>
        /// ReverseMapPath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReverseMapPath(string path)
        {
            string res;
            try
            {
                path = Strings.Replace(path.ToLower(), "d:/home/site/wwwroot/", "/");
                path = Strings.Replace(path.ToLower(), @"\\64.111.27.109\d$\wwwroot\", "http://files.callcriteria.com/");
                path = Strings.Replace(path.ToLower(), @"\\64.111.27.109\incoming\DDD\", "http://files.callcriteria.com/incoming/DDD/");
                path = Strings.Replace(path.ToLower(), "d:/wwwroot/audio/", "http://files.callcriteria.com/audio/");
                path = Strings.Replace(path.ToLower(), @"\", "/");
            }
            catch (Exception ex)
            {
                return path;
            }
            return path;
        }

        /// <summary>
        /// GetAvailableAudios
        /// </summary>
        /// <param name="xcc_id"></param>
        /// <returns></returns>
        public static List<string> GetAvailableAudios(string xcc_id) // List(Of ScorePerf)
        {
            List<string> audios = new List<string>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable dt = Common.GetTable("exec GetAudioFiles " + xcc_id);
                if (xcc_id != "")
                {
                    int xccId = Convert.ToInt32(xcc_id);
                    var GetAudioFiles = dataContext.GetAudioFiles(xccId).ToList();
                    foreach (var dr in GetAudioFiles)
                    {
                        audios.Add(ReverseMapPath(dr.filename));
                    }
                }
            }
            return audios;
        }

        /// <summary>
        /// GetNotificationSteps
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        public string GetNotificationSteps(string form_id)
        {
            string dest_list = "Notes Only";
            string[] @internal = new[] { "QA", "Calibrator", "Recalibrator", "Center Manager", "Tango TL", "QA Lead", "Team Lead", "Account Manager", "Admin" };
            string[] external = new[] { "Agent", "Supervisor", "Manager", "Client", "Account Manager", "Admin" };
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                if (form_id != "")
                {
                    int formId = Convert.ToInt32(form_id);
                    string myRole = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).ToString();
                    //DataTable notify_dt = Common.GetTable("select * from VWFN where date_closed  is null and f_id = " + form_id);
                    var notify_dt = dataContext.vwFNs.Where(x => x.date_closed == null && x.F_ID == formId).ToList();
                    bool editable = true;
                    foreach (var dr in notify_dt)
                    {
                        // if there are open notifications, prevent lesser users from accessing them                
                        if (Array.IndexOf(@internal, dr.role) > -1)
                        {
                            if ((Array.IndexOf(@internal, myRole) >= Array.IndexOf(@internal, dr.role)))
                            {
                                editable = true;
                                break;
                            }
                            else
                            {
                                editable = false;
                            }
                        }

                        if (Array.IndexOf(external, dr.role) > -1)
                        {
                            if (Array.IndexOf(external, myRole) >= Array.IndexOf(external, dr.role))
                            {
                                editable = true;
                                break;
                            }
                            else
                                editable = false;
                        }
                    }
                    if (editable)
                    {
                        //DataTable dt = Common.GetTable("select * from [dbo].[notification_steps] where profile_id = (select isnull(isnull(sc_profile, setting_profile),1) from vwForm join app_settings on app_settings.appname = vwForm.appname join scorecards on scorecards.id = vwForm.scorecard where f_id = " + form_id + ") and role = '" + myRole + "'");
                        var dt = dataContext.Getnotification_steps(myRole, formId).FirstOrDefault();
                        if (dt != null)
                        {
                            if (dt.admin == true)
                            {
                                dest_list = dest_list + "|Admin";
                            }
                            if (dt.account_manager == true)
                            {
                                dest_list = dest_list + "|Account Manager";
                            }
                            if (dt.tango_tl == true)
                            {
                                dest_list = dest_list + "|Tango Lead";
                            }
                            if (dt.client == true)
                            {
                                dest_list = dest_list + "|Client";
                            }
                            if (dt.manager == true | HttpContext.Current.User.Identity.Name == "QALVia")
                            {
                                dest_list = dest_list + "|Manager";
                            }
                            if (dt.supervisor == true)
                            {
                                dest_list = dest_list + "|Supervisor";
                            }
                            if (dt.agent == true)
                            {
                                dest_list = dest_list + "|Agent";
                            }
                            if (dt.QA == true)
                            {
                                dest_list = dest_list + "|QA";
                            }
                            if (dt.calibrator == true)
                            {
                                dest_list = dest_list + "|Calibrator/Dispute";
                            }
                            if (dt.TL == true)
                            {
                                dest_list = dest_list + "|Team Lead";
                            }
                            if (dt.CM == true)
                            {
                                dest_list = dest_list + "|Center Manager";
                            }
                            dest_list = dest_list + "|Close It";
                        }
                    }
                }
            }
            return dest_list;
        }

        /// <summary>
        /// GetNotificationStatus
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>

        public string GetNotificationStatus(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {

            string ret_data = "";
            int row_count = 0;

            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var avg_dt = dataContext.Database.SqlQuery<NotificationStatus>(
                " " + Messages.getNotificationStatus + " '" + start_date + "', '" + end_date + "',' " + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'"
                ).ToList();

                foreach (var dr in avg_dt)
                {
                    if (row_count == 0)
                    {
                        ret_data += "<tr style='font-weight:bold'>";
                    }
                    else
                    {
                        ret_data += "<tr>";
                    }

                    ret_data += "<td>" + dr.Reviewer + "</td>";
                    ret_data += "<td>" + dr.TN + "</td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AB\",\"" + dr.Reviewer + "\")';>" + dr.AB + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AA\",\"" + dr.Reviewer + "\")';>" + dr.AA + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AD\",\"" + dr.Reviewer + "\")';>" + dr.AD + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SB\",\"" + dr.Reviewer + "\")';>" + dr.SB + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SA\",\"" + dr.Reviewer + "\")';>" + dr.SA + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SD\",\"" + dr.Reviewer + "\")';>" + dr.SD + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"QB\",\"" + dr.Reviewer + "\")';>" + dr.QB + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"QA\",\"" + dr.Reviewer + "\")';>" + dr.QA + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LD\",\"" + dr.Reviewer + "\")';>" + dr.LB + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LA\",\"" + dr.Reviewer + "\")';>" + dr.LA + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LD\",\"" + dr.Reviewer + "\")';>" + dr.LD + "</span></td>";
                    ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"TD\",\"" + dr.Reviewer + "\")';>" + dr.TD + "</span></td>";
                    ret_data += "</tr>";
                    row_count += 1;
                }
            }
            return ret_data;
        }


        /// <summary>
        /// updateUserInfo
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string updateUserInfo(string value, string field)
        {
            bool update_result = Convert.ToBoolean("True");
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                if (field == "password")
                {
                    string[] pass_pieces = value.Split('|');
                    try
                    {
                        if (pass_pieces[1] == pass_pieces[2])
                        {
                            MembershipUser mu = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                            update_result = mu.ChangePassword(pass_pieces[0], pass_pieces[1]);
                        }
                    }
                    catch (Exception ex)
                    {
                        update_result = Convert.ToBoolean(ex.Message);
                    }
                }
                else
                {
                    //Common.UpdateTable("update userextrainfo set " + field + " = '" + value.Replace("'", "''") + "' where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                    var isExist = dataContext.UpdateUserExtraInfo(field, value.Replace("'", "''"), HttpContext.Current.User.Identity.Name.Replace("'", "''"));
                }
            }
            return update_result.ToString();
        }

        /// <summary>
        /// GetAgents
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<DBOptions> GetAgents(string start_date, string end_date, string scorecard, string group)
        {
            List<DBOptions> mi_items = new List<DBOptions>();
            //DataTable sup_test = Common.GetTable("select user_group,user_role from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var sup_test = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).FirstOrDefault();
                if (sup_test != null)
                {
                    if (sup_test.user_role == "supervisor" & sup_test.user_group != "")
                    {
                        group = sup_test.user_group;
                    }
                }
                string agentGroup = "";
                int scoreCard = 0;
                string agent = "";
                string UserName = HttpContext.Current.User.Identity.Name.Replace("'", "''");
                if (group.IndexOf("'") == -1)
                {
                    group = "'" + group + "'";
                }
                if (group != "'NOGROUP'")
                {
                    agentGroup = group;
                }
                if (scorecard != "0" & scorecard != "")
                {
                    scoreCard = Convert.ToInt32(scorecard);
                }
                if (HttpContext.Current.User.IsInRole("Agent"))
                {
                    agent = HttpContext.Current.User.Identity.Name.Replace("'", "''");
                }
                //DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                var user_dt = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).FirstOrDefault();
                // DataTable stats_dt = Common.GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  where scorecard in (select user_scorecard from userapps where " +
                //"username =  '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + filter + " and agent is not null and agent != '' " +
                // "and max_reviews > -1 and call_date between '" + start_date + "' and '" + end_date + "'  " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + "  order by AGent");
                var xcc_REPORT_NEW = dataContext.GetCDServiceXccReportNew(agentGroup, scoreCard, UserName, agent, end_date, end_date, user_dt.special_filter, "Agent").ToList();
                foreach (var dr in xcc_REPORT_NEW)
                {
                    DBOptions _mi = new DBOptions();
                    _mi.text = dr.AGent;
                    _mi.value = dr.AGent;

                    if (HttpContext.Current.Session["Agent"].ToString() == dr.AGent)
                    {
                        _mi.selected = "selected";
                    }
                    else
                    {
                        _mi.selected = "";
                    }
                    mi_items.Add(_mi);
                }
            }
            return mi_items;
        }


        /// <summary>
        /// GetDetails
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagerows"></param>
        /// <param name="Sort_statement"></param>
        /// <param name="rowstart"></param>
        /// <param name="rowend"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        public string GetDetails(string start_date, string end_date, string hdnAgentFilter, string pagenum = "1", string pagerows = "50", string Sort_statement = "", string rowstart = "0", string rowend = "0", string filter_array = "")
        {
            GridView gvQADetails = new GridView();
            gvQADetails.AutoGenerateColumns = false;

            gvQADetails.RowCreated += GridView1_RowCreated;
            gvQADetails.RowDataBound += gvQADetails_RowDataBound;

            string called_sp = "getDetailData";
            if (filter_array != "")
            {
                called_sp = "getDetailDataArray";
            }
            StringWriter sw = new StringWriter();
            addField(ref gvQADetails, "CALL ID");
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable user_col_count_dt = Common.GetTable("select * from available_columns join user_columns on user_columns.column_id = available_columns.id where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' order by col_order");
                var AvailableColumns = (from availableColumns in dataContext.available_columns
                                        join userColumns in dataContext.user_columns on availableColumns.id equals userColumns.column_id
                                        where userColumns.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")
                                        select new { availableColumns.column_required, availableColumns.column_name, userColumns.col_order }).OrderBy(x => x.col_order).ToList();

                if (AvailableColumns.Count == 0)
                {
                    addField(ref gvQADetails, "RESULT");
                    addField(ref gvQADetails, "COMMENTS");
                    addField(ref gvQADetails, "AGENT");
                    addField(ref gvQADetails, "CAMPAIGN");
                    addField(ref gvQADetails, "REVIEW DATE");
                    addField(ref gvQADetails, "PHONE");
                    addField(ref gvQADetails, "CALL DATE");
                    addField(ref gvQADetails, "CALL LENGTH");
                    addField(ref gvQADetails, "SCORE");
                    addField(ref gvQADetails, "# MISSED");
                    addField(ref gvQADetails, "MISSED ITEMS");
                    addField(ref gvQADetails, "Session ID");
                    if (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead"))
                    {
                        addField(ref gvQADetails, "Efficiency");
                    }
                }
                else
                {
                    foreach (var dr in AvailableColumns)
                    {
                        if (dr.column_required.ToString() == "False" | dr.column_required.ToString() == "")
                        {
                            addField(ref gvQADetails, dr.column_name.Replace("[", "").Replace("]", ""));
                        }
                    }

                    called_sp = "getDetailDataCustom";
                }

                gvQADetails.UseAccessibleHeader = false;
                string this_user = HttpContext.Current.User.Identity.Name;
                if (Sort_statement == "undefined" | Strings.Trim(Sort_statement) == "order by [] desc")
                {
                    Sort_statement = "";
                }
                string myRole = "";
                string[] user_roles = Roles.GetRolesForUser(this_user);
                foreach (var role in user_roles)
                {
                    myRole = role;
                }
                if (hdnAgentFilter.IndexOf("and vwform.agent =") > -1 & HttpContext.Current.User.Identity.Name.ToLower() == "agent")
                {
                    this_user = "agent";
                    myRole = "Agent";
                }
                DataTable dt;
                if (Convert.ToInt32(pagenum) == 0 || Convert.ToInt32(pagenum) == -1)
                {
                    if (Convert.ToInt32(pagenum) == 0)
                    {
                        dt = Common.GetTable(called_sp + " '" + this_user.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user.Replace("'", "''") + "','" + myRole + "','1','1','" + Sort_statement + "','" + rowstart + "','" + rowend + "','" + filter_array.Replace("'", "''") + "'");

                    }
                    else
                    {
                        dt = Common.GetTable(called_sp + " '" + this_user.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user.Replace("'", "''") + "','" + myRole + "','" + 1 + "','10000','" + Sort_statement + "','" + rowstart + "','" + rowend + "','" + filter_array.Replace("'", "''") + "'");
                    }
                }
                else if (filter_array != "")
                {
                    dt = Common.GetTable(called_sp + " '" + this_user + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user + "','" + myRole + "','" + pagenum + "','" + pagerows + "','" + Sort_statement + "','" + rowstart + "','" + rowend + "'");
                }
                else
                {
                    dt = Common.GetTable(called_sp + " '" + this_user + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user + "','" + myRole + "','" + pagenum + "','" + pagerows + "','" + Sort_statement + "','" + rowstart + "','" + rowend + "'");
                }
                gvQADetails.DataSource = dt;
                gvQADetails.DataBind();

                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (Convert.ToInt32(pagenum) == 0 | Convert.ToInt32(pagenum) == -1)
                {

                    gvQADetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gvQADetails.HeaderRow.RenderControl(hw);
                    if (Convert.ToInt32(pagenum) == -1)
                    {
                        foreach (GridViewRow gvr in gvQADetails.Rows)
                            gvr.RenderControl(hw);
                    }
                }
                else
                    foreach (GridViewRow gvr in gvQADetails.Rows)
                    {
                        gvr.RenderControl(hw);
                    }
            }
            return sw.ToString();
        }

        /// <summary>
        /// GridView1_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gvQADetails = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                BulletedList bl = new BulletedList();
                bl.CssClass = "table-navigation";
                int index;
                for (index = 1; index <= gvQADetails.PageCount; index++)
                {
                    if (index == gvQADetails.PageIndex + 1)
                    {
                        ListItem li = new ListItem();
                        li.Text = index.ToString();
                        li.Attributes.Add("class", "selected-page");
                        Label lbl = new Label();
                        lbl.Text = "<span style=\"color:red;\"> </span> <b style=\"background-color: Silver;color:red;border-style:solid;border-color:silver;border-width:2px;padding:2px 2px 2px 2px\">" + index.ToString() + "</b> ";
                        e.Row.Cells[0].Controls.Add(lbl);
                        bl.Items.Add(li);
                    }
                    else
                    {
                        ListItem li = new ListItem();
                        li.Text = index.ToString();
                        LinkButton linkbutton = new LinkButton();
                        linkbutton.ID = "LinkPage" + index;
                        linkbutton.CommandName = "Page";
                        linkbutton.CommandArgument = index.ToString();
                        linkbutton.Text = index.ToString();
                        linkbutton.CssClass = "link";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter hw = new HtmlTextWriter(sw);

                        linkbutton.RenderControl(hw);
                        li.Text = sw.ToString().Replace("&lt;", "<").Replace("&gt;", ">");
                        bl.Items.Add(li);
                    }
                }
                e.Row.Cells[0].Controls.Add(bl);
            }
        }

        /// <summary>
        /// addField
        /// </summary>
        /// <param name="gvQADetails"></param>
        /// <param name="fieldname"></param>
        protected void addField(ref GridView gvQADetails, string fieldname)
        {
            var bfield = new BoundField();
            bfield.HeaderText = fieldname;
            bfield.DataField = fieldname;
            bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            bfield.HtmlEncode = false;
            gvQADetails.Columns.Add(bfield);
        }


        /// <summary>
        /// GetQualityA
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        public List<DBOptions> GetQualityA(string start_date, string end_date, string scorecard)
        {

            List<DBOptions> mi_items = new List<DBOptions>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                DateTime startDate = new DateTime();
                if (start_date != "")
                {
                    startDate = Convert.ToDateTime(start_date);
                }
                DateTime etartDate = new DateTime();
                if (end_date != "")
                {
                    etartDate = Convert.ToDateTime(end_date);
                }
                //DataTable stats_dt;
                //stats_dt = Common.GetTable("Select distinct reviewer from vwform  where call_date between '" + start_date + "' and '" + end_date + "'  order by reviewer");
                var stats_dt = (from x in dataContext.vwForms
                                where x.call_date >= startDate && x.call_date <= etartDate
                                select new { x.reviewer }).Distinct().ToList();
                foreach (var dr in stats_dt)
                {
                    DBOptions _mi = new DBOptions();
                    _mi.text = dr.reviewer;
                    _mi.value = dr.reviewer;
                    mi_items.Add(_mi);
                }
            }
            return mi_items;
        }

        /// <summary>
        /// GetCampaigns
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<DBOptions> GetCampaigns(string start_date, string end_date, string scorecard, string group)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            string agentGroup = "";
            int scoreCard = 0;
            string agent = "";
            string UserName = HttpContext.Current.User.Identity.Name.Replace("'", "''");
            if (group.IndexOf("'") == -1)
            {
                group = "'" + group + "'";
            }
            if (group != "'NOGROUP'")
            {
                agentGroup = group;
            }
            if (scorecard != "0" & scorecard != "")
            {
                scoreCard = Convert.ToInt32(scorecard);
            }
            if (HttpContext.Current.User.IsInRole("Agent"))
            {
                agent = HttpContext.Current.User.Identity.Name.Replace("'", "''");
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

                //stats_dt = Common.GetTable("SELECT distinct Campaign FROM [XCC_REPORT_NEW]  where  scorecard in (select user_scorecard from userapps where username =  " +"'" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + filter + " and call_date between '" + start_date + "' and '" + end_date + "' " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + " and campaign is not null  order by campaign");
                var user_dt = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).FirstOrDefault();
                var xcc_REPORT_NEW = dataContext.GetCDServiceXccReportNew(agentGroup, scoreCard, UserName, agent, end_date, end_date, user_dt.special_filter, null).ToList();
                foreach (var dr in xcc_REPORT_NEW)
                {
                    foreach (var dr1 in xcc_REPORT_NEW)
                    {
                        DBOptions _mi = new DBOptions();
                        _mi.text = dr1.Campaign.ToString();
                        _mi.value = dr.Campaign.ToString();

                        if (HttpContext.Current.Session["Campaign"].ToString() == dr.Campaign.ToString())
                        {
                            _mi.selected = "selected";
                        }
                        else
                        {
                            _mi.selected = "";
                        }
                        mi_items.Add(_mi);
                    }
                }
                return mi_items;
            }
        }

        /// <summary>
        /// GetDetailCount
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        public string GetDetailCount(string start_date, string end_date, string hdnAgentFilter = "", string filter_array = "")
        {

            if (hdnAgentFilter == null)
            {
                hdnAgentFilter = "";
            }
            if (filter_array == null)
            {
                filter_array = "";
            }
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                //dt = Common.GetTable("getDetailDataCount '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + filter_array.Replace("'", "''") + "'");
                var avg_dt = dataContext.Database.SqlQuery<int>(
                   " exec getDetailDataCount '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + filter_array.Replace("'", "''") + "'"
                   ).FirstOrDefault();

                return avg_dt.ToString();
            }
        }

        /// <summary>
        /// GetGroups
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        public List<DBOptions> GetGroups(string start_date, string end_date, string scorecard)
        {

            string my_group = "";
            List<DBOptions> mi_items = new List<DBOptions>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {

                string userName = HttpContext.Current.User.Identity.Name.Replace("'", "''");
                int scoreCard = 0;
                if (scorecard != null && scorecard != "")
                {
                    scoreCard = Convert.ToInt32(scorecard);
                }
                var stats_dt = dataContext.GetCDGetGroups(scoreCard, userName, start_date, end_date).ToList();
                foreach (var dr in stats_dt)
                {
                    DBOptions _mi = new DBOptions();
                    _mi.text = dr.agent_group;
                    _mi.value = dr.agent_group;

                    if (my_group == dr.agent_group)
                    {
                        _mi.selected = "selected";
                    }
                    else
                    {
                        _mi.selected = "";
                    }

                    mi_items.Add(_mi);
                }
            }
            return mi_items;
        }

        /// <summary>
        /// GetAppnames
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DBOptions> GetAppnames(string filter)
        {
            List<DBOptions> mi_items = new List<DBOptions>();
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                filter = Strings.Replace(filter, "undefined", "");
                string app_filter = "";
                if (HttpContext.Current.Session["agent_appname"] != null)
                {
                    app_filter = HttpContext.Current.Session["agent_appname"].ToString();
                }
                //stats_dt = Common.GetTable("exec getMyAppnames '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + filter.Replace("'", "''") + "','" + app_filter + "'");
                var stats_dt = dataContext.getMyAppnames(HttpContext.Current.User.Identity.Name.Replace("'", "''"), filter.Replace("'", "''"), app_filter).ToList();
                foreach (var dr in stats_dt)
                {
                    DBOptions _mi = new DBOptions();
                    _mi.text = dr.scorecard;
                    _mi.value = dr.ID.ToString();
                    mi_items.Add(_mi);
                }
                return mi_items;
            }
        }

        /// <summary>
        /// UpdateScorecard
        /// </summary>
        /// <param name="scorecard"></param>
        public void UpdateScorecard(string scorecard)
        {

            //Common.UpdateTable("delete from sc_update where reviewer = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' and sc_id = '" + scorecard + "'");
            //Common.UpdateTable("insert into sc_update (reviewer, sc_id, date_reviewed) select '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + scorecard + "', dbo.getMTDate()");
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                int Id = Convert.ToInt32(scorecard.Trim());
                var isExist = dataContext.sc_update.Where(x => x.reviewer == HttpContext.Current.User.Identity.Name.Replace("'", "''") && x.sc_id == Id).FirstOrDefault();
                if (isExist != null)
                {
                    dataContext.sc_update.Remove(isExist);
                    int result = dataContext.SaveChanges();
                }
                var dateQuery = dataContext.Database.SqlQuery<DateTime>("SELECT dbo.getMTdate()");
                DateTime Date = dateQuery.AsEnumerable().First();
                sc_update tblsc_update = new sc_update();
                if (HttpContext.Current.User.Identity.Name.Replace("'", "''") != null)
                {

                    tblsc_update.reviewer = HttpContext.Current.User.Identity.Name.Replace("'", "''");
                    tblsc_update.sc_id = Id;
                    tblsc_update.date_reviewed = Date;
                    dataContext.sc_update.Add(tblsc_update);
                    int Result = dataContext.SaveChanges();
                }

            }
        }

        /// <summary>
        /// getUserInfo
        /// </summary>
        /// <returns></returns>
        public UserInfo getUserInfo()
        {

            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                UserInfo ui = new UserInfo();
                //DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                var user_dt = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Trim()).FirstOrDefault();
                if (user_dt != null)
                {
                    ui.username = HttpContext.Current.User.Identity.Name;
                    ui.SpeedInc = user_dt.speed_increment.ToString();
                    ui.first_name = user_dt.first_name;
                    ui.last_name = user_dt.last_name;
                    ui.phone = user_dt.phone_number;
                    ui.ImmediatePlay = Convert.ToBoolean(user_dt.calls_start_immediately);
                    ui.bypass = Convert.ToBoolean(user_dt.bypass);
                    ui.email = user_dt.email_address;
                    ui.guideline_display = user_dt.guideline_display.ToString();
                    ui.presubmit = user_dt.presubmit.ToString();
                    ui.export_type = user_dt.export_type.ToString();
                }
                return ui;
            }
        }

        private int comment_header = 0;
        private int missed_list_header = 0;
        private int call_id_header = 0;
        private int call_result_header = 0;
        private int[] cols_with_data;
        /// <summary>
        /// gvQADetails_RowDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvQADetails_RowDataBound(object sender, GridViewRowEventArgs e) // Handles gvQADetails.RowDataBound
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (var x = 0; x <= e.Row.Cells.Count - 1; x++)
                {
                    if (e.Row.Cells[x].Text == "COMMENTS")
                    {
                        comment_header = x;
                    }
                    if (e.Row.Cells[x].Text == "MISSED ITEMS")
                    {
                        missed_list_header = x;
                    }
                    if (e.Row.Cells[x].Text == "CALL ID")
                    {
                        call_id_header = x;
                    }
                    if (e.Row.Cells[x].Text == "RESULT")
                    {
                        call_result_header = x;
                    }
                }

                cols_with_data = new int[e.Row.Cells.Count + 1];
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (var x = 0; x <= e.Row.Cells.Count - 1; x++)
                {
                    if (e.Row.Cells[x].Text != "&nbsp;")
                    {
                        cols_with_data[x] = cols_with_data[x] + 1;
                    }
                }

                bool isCalib = false;
                bool isedited = false;
                string sort_class = "";
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (dr["play_btn_class"].ToString() != "")
                {
                    isCalib = true;
                    sort_class += "calib";
                }

                if (dr["wasEdited"].ToString() != "")
                {
                    isedited = true;
                    sort_class += "edit";
                }
                if (sort_class != "")
                {
                    e.Row.Cells[call_id_header].Attributes.Add("data-text", sort_class);
                }
                if (dr["Result"].ToString() == "Pass")
                {
                    e.Row.Cells[call_result_header].Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>";
                }
                if (dr["Result"].ToString() == "N/A")
                {
                    e.Row.Cells[call_result_header].Text = "<span class='final-result' title='" + dr["bad_call_reason"].ToString().Replace("'", "").Replace("\"", "") + "' style='color:darkgray'>N/A &nbsp;&nbsp;<i class='fa fa-question-circle'></i></span>";
                }
                if (dr["Result"].ToString() == "Fail")
                {
                    dr[call_result_header] = "<span class='final-result' " + sort_class + ">FAIL <i class='fa fa-times'></i></span>";
                    e.Row.Attributes.Add("class", "fail-row");
                }

                if (dr[comment_header].ToString() != "&nbsp;" & (dr[comment_header].ToString().Trim()) != "-")
                {
                    e.Row.Attributes.Add("class", "popup-comments");
                    dr[comment_header].ToString();
                    string[] comments = dr[comment_header].ToString().Split('|');
                    if (comments.Length > 1)
                    {
                        dr[comment_header] = "";
                        int comment_id = 1;
                        foreach (var comment in comments)
                        {
                            if (Strings.Trim(comment) != "")
                            {
                                dr[comment_header] += "<i class='fa fa-file comment" + comment_id + "'></i><span style='white-space: normal;'>" + Strings.Trim(comment.Replace("&lt;br&gt;", "<br>")) + "</span>";
                            }
                            comment_id += 1;
                        }
                    }
                    else
                    {
                        dr[comment_header] = "<i class='fa fa-file comment1'></i><span style='white-space: normal;'>" + dr[comment_header].ToString().Replace("&lt;br&gt;", "<br>").Trim() + "</span>";
                    }
                }

                string noti_owned = "1";
                if (dr["non_edit"].ToString() == "1")
                {
                    noti_owned = "0";
                }
                if (dr["NotificationID"].ToString() != "" & dr["Notificationstep"].ToString() != "")
                {
                    dr[comment_header] += " <img class='noti-click yellow-ex-mark noti-click" + dr["OwnedNotification"].ToString() + "' src='img/yellow_exclamation.png' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Open " + dr["Notificationstep"].ToString() + " Notification' data-notiid='" + dr["NotificationID"].ToString() + "' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='" + dr["Notificationstep"].ToString() + "' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";
                }
                else
                {
                    if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("Admin"))
                    {
                        dr[comment_header] += " <img class='noti-click yellow-ex-mark' src='img/yellow-plus.PNG' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Create Notification' data-source='callDetails' data-notiid='0' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='Supervisor' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";
                    }
                    if (HttpContext.Current.User.IsInRole("Agent") & dr["agent"].ToString() == HttpContext.Current.User.Identity.Name)
                    {
                        dr[comment_header] += " <img class='noti-click yellow-ex-mark' src='img/yellow-plus.PNG' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Create Notification' data-source='callDetails' data-notiid='0' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='Agent' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";
                    }
                }

                if (dr["play_btn_class"].ToString() == "")
                {
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button'><div></div></button></a>";
                }
                else
                {
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button'  class='cali_class' title='Calibrated Call'><div></div></button></a>";
                }

                if (dr["wasEdited"].ToString() == "True" & dr["play_btn_class"].ToString() == "")
                {
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button' class='edit_class'  title='Edited Call'><div></div></button></a>";
                }
                if (dr["website"].ToString() != "")
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "'  target='_blank'><button type='button' class='website_class'  title='Website Call'><div></div></button></a>";
                e.Row.Attributes.Add("class", "playBtn");
                dr[call_id_header].ToString();
            }
        }

        /// <summary>
        /// getString
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string getString(Uri uri)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }
    }




    public class JsonStringResult
    {
        public string JsonString { get; set; }
    }
}