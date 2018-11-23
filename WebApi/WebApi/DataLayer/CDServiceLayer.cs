using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Security;
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
        public string userID = "10608";     // User ID: 10608 
        public string userToken = "ZGEzMDgyMGUtNzgxOC00NGMwLWFjMjMtMTZjZmMxMTFkN2Q2";     // API Auth Token: ZGEzMDgyMGUtNzgxOC00NGMwLWFjMjMtMTZjZmMxMTFkN2Q2

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


        //[OperationContract()]
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
        //[OperationContract()]
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
        //[OperationContract()]
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
        //[OperationContract()]
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
        //[OperationContract()]
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
        //[OperationContract()]
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
                if (drv.check_reason == "QA/QA Missed Items" & (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Account Manager")) & (drv.close_reason.IndexOf("Send to QA") > -1))
                {
                    ActionButton ab = new ActionButton();
                    ab.Action = "Reassign Notification";
                    ab.Endpoint = "ReassignNotification";
                    abs.Add(ab);
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
        //[OperationContract()]
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
        /// 
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        //[OperationContract()]
        public string GetNotificationSteps(string form_id) // List(Of ScorePerf)
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
        //[OperationContract()]
        public string GetNotificationStatus(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {

            string ret_data = "";
            int row_count = 0;
          
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var avg_dt = dataContext.Database.SqlQuery<NotificationStatus>(
                " "+ Messages.getNotificationStatus +" '" + start_date + "', '" + end_date + "',' " + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'"
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
        //[OperationContract()]
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
                    var isExist = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).FirstOrDefault();
                    UserExtraInfo tblUserExtraInfo = new UserExtraInfo();
                    if (isExist != null)
                    {
                        int Id = isExist.id;
                        tblUserExtraInfo = dataContext.UserExtraInfoes.Find(Id);
                        //dataContext.Entry(tblUserExtraInfo).State = EntityState.Modified;
                        //tblUserExtraInfo.q_order = value.Replace("'", "''");
                       //int result = dataContext.SaveChanges();
                    }
                }
            }
                return update_result.ToString();
        }

        ///// <summary>
        ///// GetAgents
        ///// </summary>
        ///// <param name="start_date"></param>
        ///// <param name="end_date"></param>
        ///// <param name="scorecard"></param>
        ///// <param name="group"></param>
        ///// <returns></returns>
        ////[OperationContract()]
        //public List<DBOptions> GetAgents(string start_date, string end_date, string scorecard, string group)
        //{
        //    List<DBOptions> mi_items = new List<DBOptions>();
        //    //DataTable sup_test = Common.GetTable("select user_group,user_role from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
        //    using (CC_ProdEntities dataContext = new CC_ProdEntities())
        //    {
        //        var sup_test = dataContext.UserExtraInfoes.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).FirstOrDefault();
        //        if (sup_test != null)
        //        {
        //            if (sup_test.user_role == "supervisor" & sup_test.user_group != "")
        //            {
        //                group = sup_test.user_group;
        //            }
        //        }
        //        string filter = "";
        //        string agentGroup = "";
        //        int scoreCard =0;
        //        string agent = "";
        //        if (group.IndexOf("'") == -1)
        //        {
        //            group = "'" + group + "'";
        //        }
        //        if (group != "'NOGROUP'")
        //        {
        //            agentGroup =  group;
        //        }
        //        if (scorecard != "0" & scorecard != "")
        //        {
        //            scoreCard =Convert.ToInt32(scorecard);
        //        }
        //        if (HttpContext.Current.User.IsInRole("Agent"))
        //        {
        //            agent += HttpContext.Current.User.Identity.Name.Replace("'", "''") ;
        //        }
        //        //DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
        //        var UserApp = dataContext.UserApps.Where(x => x.username == HttpContext.Current.User.Identity.Name.Replace("'", "''")).ToList();
        //        foreach (var dr in UserApp)
        //        {
        //        // DataTable stats_dt = Common.GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  where scorecard in (select user_scorecard from userapps where " +
        //        //"username =  '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + filter + " and agent is not null and agent != '' " +
        //       // "and max_reviews > -1 and call_date between '" + start_date + "' and '" + end_date + "'  " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + "  order by AGent");
        //       var xcc_REPORT_NEW = dataContext.XCC_REPORT_NEW.Where(x => x.AGENT_GROUP == agentGroup && x.scorecard ==scoreCard 
        //       && x.AGENT == agent && x.AGENT !=null && x.MAX_REVIEWS > -1 && x.call_date >=Convert.ToDateTime(start_date) && x.call_date <= Convert.ToDateTime(end_date)).OrderBy(x => x.AGENT).ToList();
        //            foreach (var dr in xcc_REPORT_NEW)
        //           {
        //             DBOptions _mi = new DBOptions();
        //            _mi.text = dr["AGent"].ToString();
        //            _mi.value = dr["AGent"].ToString();

        //            if (HttpContext.Current.Session["Agent"].ToString() == dr["Agent"].ToString())
        //            {
        //                _mi.selected = "selected";
        //            }
        //            else
        //            { 
        //                _mi.selected = "";
        //            }
        //            mi_items.Add(_mi);
        //        }
        //     }
        //    }
        //    return mi_items;
        //}

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
    /// <summary>
    /// 
    /// </summary>
    public class NotificationStatus
    {
        public string Reviewer { get; set; }
        public int? TN { get; set; }
        public int? AB { get; set; }
        public int? AA { get; set; }
        public int? AD { get; set; }
        public int? SB { get; set; }
        public int? SA { get; set; }
        public int? SD { get; set; }
        public int? QB { get; set; }
        public int? QA { get; set; }
        public int? QD { get; set; }
        public int? LB { get; set; }
        public int? LA { get; set; }
        public int? LD { get; set; }
        public int? TD { get; set; }

    }

}