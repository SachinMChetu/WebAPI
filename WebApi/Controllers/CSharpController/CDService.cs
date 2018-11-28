using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI;
using WebApi.Models.CDService;
using WebApi.DataLayer;

namespace WebApi.Controllers.CSharpController
{

    /// <summary>
    /// CDService
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [ServiceContract(Namespace = "http://webservices.callcriteria.com/")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CDService
    {

        /// <summary>
        /// GetAgentAvgs
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetAgentAvgs(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {
            DataTable daily_avg = Common.GetTable("exec GetAgentAvgs '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "', '" + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");
            string graph_data = "[";
            foreach (DataRow dr in daily_avg.Rows)
                graph_data += "[new Date('" + dr["call_date"] + "')," + dr["avg_score"] + "],";
            if (Strings.Right(graph_data, 1) == ",")
                graph_data = Strings.Left(graph_data, Strings.Len(graph_data) - 1);

            graph_data += "]";

            return graph_data;
        }

        /// <summary>
        /// GetScorePerf
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetScorePerf(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {
            DataTable daily_avg = Common.GetTable("exec [GetScorePerf] '" + start_date + "','" + end_date + "' ,'" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "',' " + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array + "'");
            string graph_data = "[";

            foreach (DataRow dr in daily_avg.Rows)
                graph_data += "[new Date('" + dr["call_date"] + "')," + dr["avg_score"] + "],";

            if (Strings.Right(graph_data, 1) == ",")
                graph_data = Strings.Left(graph_data, Strings.Len(graph_data) - 1);

            graph_data += "]";
            return graph_data;
        }

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

        /// <summary>
        /// createUserRelativeUri
        /// </summary>
        /// <param name="path"></param>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        private Uri createUserRelativeUri(String path, NameValueCollection requestParams = null)
        {
            if (requestParams == null)
                requestParams = new NameValueCollection();
            requestParams.Add("auth_token", userToken);
            String paramString = "?";
            foreach (string name in requestParams.Keys)
                paramString += (name + Convert.ToString("=")) + requestParams[name] + "&";
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
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
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
        /// Deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            if (json != "")
            {
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                ms.Dispose();
            }
            return obj;
        }

        /// <summary>
        /// GetTranscriptID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public TranscriptData GetTranscriptID(string ID)
        {
            DataTable fid_dt = Common.GetTable("select review_id from vwFOrm where f_id = " + ID);
            TranscriptData td = new TranscriptData();

            if (fid_dt.Rows.Count == 0)
                return td;
            string xcc_id = fid_dt.Rows[0][0].ToString();
            if (!Information.IsNumeric(xcc_id))
                return td;

            DataTable form_dt = Common.GetTable("select sm_id, mediaId, scorecard, api_version from xcc_report_new where id = " + xcc_id);
            if (form_dt.Rows.Count > 0)
            {
                switch (Convert.ToInt32(form_dt.Rows[0][3]))
                {
                    case 1:
                        {
                            td.Words = getVBdata(form_dt.Rows[0][1].ToString());
                            break;
                        }

                    case 2:
                        {
                            td.Words = getVBdata2(form_dt.Rows[0][1].ToString());
                            break;
                        }
                    default:
                        {
                            return td;
                        }
                }
            }

            List<KeywordObject> kw = new List<KeywordObject>();
            DataTable kw_dt = Common.GetTable("select * from utterance_flags where scorecard = '" + form_dt.Rows[0][2].ToString() + "'");
            foreach (DataRow dr in kw_dt.Rows)
            {
                KeywordObject kw_object = new KeywordObject();
                kw_object.acceptedvariant = dr["accepted_variant"].ToString();
                kw_object.keyword = dr["utterance"].ToString();
                kw_object.required = dr["utterance_type"].ToString();
                kw.Add(kw_object);
            }

            td.Keyword = kw;
            return td;
        }

        /// <summary>
        /// GetQANotifications
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQANotifications(string start_date, string end_date, string scorecard)
        {
            DataTable dt = Common.GetTable(" exec GetQANotifications '" + start_date + "','" + end_date + "','" + scorecard + "'");
            try
            {
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// GetSupervisorNotifications
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetSupervisorNotifications(string start_date, string end_date, string scorecard)
        {
            DataTable dt = Common.GetTable(" exec GetSupervisorNotifications '" + start_date + "','" + end_date + "','" + scorecard + "'");
            try
            {
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// GetSpotCheckData2
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetSpotCheckData2(string start_date, string end_date, string scorecard)
        {
            DataTable dt = Common.GetTable(" exec getSpotCheckData '" + start_date + "','" + start_date + "','" + scorecard + "'");

            return dt.Rows[0][0].ToString();
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
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        public string GetSpotCheckData(string start_date, string end_date, string scorecard, string appname, string team_lead)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand();

                sqlComm.Connection = sqlCon;
                sqlComm.CommandText = "[getSpotCheckData]";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("scorecard", scorecard);
                sqlComm.Parameters.AddWithValue("start", start_date);
                sqlComm.Parameters.AddWithValue("end", end_date);
                sqlComm.Parameters.AddWithValue("appname", appname);
                sqlComm.Parameters.AddWithValue("tl", team_lead);
                sqlCon.Open();
                string json = "";

                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                    json = json + reader[0];
                return json;
            }
        }


        /// <summary>
        /// LoadDMSVendors
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<vendor_data> LoadDMSVendors()
        {
            List<vendor_data> vd = new List<vendor_data>();

            DataTable dt = Common.GetTable("select vendor, short_name from dms_vendor_list left join scorecards on scorecards.id = scorecard order by vendor");
            List<string> vendors = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                vendor_data vd_item = new vendor_data();
                vd_item.vendor = dr[0].ToString();
                vd_item.scorecard = dr[1].ToString();
                vd.Add(vd_item);
            }
            return vd;
        }

        /// <summary>
        /// DeleteDMSVendors
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string DeleteDMSVendors(string vendor)
        {
            DataTable dt = Common.GetTable("delete from dms_vendor_list where vendor = '" + vendor + "'");

            return "Deleted";
        }

        /// <summary>
        /// AddDMSVendors
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string AddDMSVendors(string vendor, string scorecard)
        {
            DataTable dt = Common.GetTable("insert into dms_vendor_list (vendor, scorecard) select  '" + vendor + "', '" + scorecard + "'");

            return "Added";
        }

        /// <summary>
        /// GetTranscript
        /// </summary>
        /// <param name="xcc_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public TranscriptData GetTranscript(string xcc_id)
        {
            TranscriptData td = new TranscriptData();
            // Return td
            if (!Information.IsNumeric(xcc_id))
                return td;

            DataTable form_dt = Common.GetTable("select sm_id, mediaId, scorecard, api_version from xcc_report_new where id = " + xcc_id);
            if (form_dt.Rows.Count > 0)
            {
                switch (Convert.ToInt32(form_dt.Rows[0][3]))
                {
                    case 1:
                        {
                            td.Words = getVBdata(form_dt.Rows[0][1].ToString());
                            break;
                        }

                    case 2:
                        {
                            try
                            {
                                td.Words = getVBdata2(form_dt.Rows[0][1].ToString());
                            }
                            catch (Exception ex)
                            {
                            }

                            break;
                        }

                    case -2:
                        {
                            // Try
                            td.Words = getChatText(form_dt.Rows[0][1].ToString());
                            break;
                        }

                    default:
                        {
                            return td;
                        }
                }
                try
                {
                    List<KeywordObject> kw = new List<KeywordObject>();
                    DataTable kw_dt = Common.GetTable("select * from utterance_flags where scorecard = '" + form_dt.Rows[0][2].ToString() + "'");
                    if (kw_dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in kw_dt.Rows)
                        {
                            KeywordObject kw_object = new KeywordObject();
                            kw_object.acceptedvariant = dr["accepted_variant"].ToString();
                            kw_object.keyword = dr["utterance"].ToString();
                            kw_object.required = dr["utterance_type"].ToString();
                            kw.Add(kw_object);
                        }
                    }

                    td.Keyword = kw;
                }
                catch (Exception ex)
                {
                }
            }
            return td;
        }


        /// <summary>
        /// GetActionButtons
        /// </summary>
        /// <param name="username"></param>
        /// <param name="f_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<ActionButton> GetActionButtons(string username, string f_id)
        {
            List<ActionButton> abs = new List<ActionButton>();
            DataTable f_dt = Common.GetTable("exec GetActionButtonsData " + f_id);
            DataRow drv = f_dt.Rows[0];
            bool show_edit = true;

            if (HttpContext.Current.User.IsInRole("Agent"))
                show_edit = false;

            if (HttpContext.Current.User.IsInRole("QA"))
            {
                if (drv["calib_score"].ToString() != "" | drv["edited_score"].ToString() != "")
                    show_edit = false;
            }
            if ((drv["website"].ToString() != "" & HttpContext.Current.User.IsInRole("QA Lead")))
            {
                ActionButton ab = new ActionButton();
                ab = new ActionButton();
                ab.Action = "Reset Call";
                ab.Endpoint = "ResetCall";
                abs.Add(ab);
            }

            if (drv["check_reason"].ToString() == "QA/QA Missed Items" & (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead") | HttpContext.Current.User.IsInRole("Account Manager")) & (drv["close_reason"].ToString().IndexOf("Send to QA") > -1))
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

                if (drv["bad_call"].ToString() == "")
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

                if (drv["bad_call"].ToString() == "")
                {
                    ab = new ActionButton();
                    ab.Action = "Mark Call Bad";
                    ab.Endpoint = "MarkCallBad2";
                    abs.Add(ab);
                }
            }

            if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager") | HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("Account Manager"))
            {
                if (drv["bad_call"].ToString() != "" & drv["bca_date"].ToString() == "")
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
                if (drv["bad_call"].ToString() == "")
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

            if (drv["bad_call"].ToString() == "" & drv["wasEdited"].ToString() == "")
            {
                // already is in calibration, don't show button
                DataTable cal_dt = Common.GetTable("Select count(*) from calibration_pending where form_id = " + f_id);
                if (cal_dt.Rows.Count > 0)
                {
                    if (!(HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("QA")) & Convert.ToInt32(cal_dt.Rows[0][0]) == 0)
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Add Calibration";
                        ab.Endpoint = "AddCalibration";
                        abs.Add(ab);
                    }
                }
                // already is in calibration, don't show button
                DataTable c_cal_dt = Common.GetTable("Select count(*) from cali_pending_client   where form_id = " + f_id);
                if (Convert.ToInt32(c_cal_dt.Rows[0][0]) == 0)
                {
                    if ((HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Manager")) & Convert.ToInt32(c_cal_dt.Rows[0][0]) == 0)
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Add Calibration";
                        ab.Endpoint = "AddClientCalibration";
                        abs.Add(ab);
                    }

                    if (HttpContext.Current.User.IsInRole("Admin") & Convert.ToInt32(c_cal_dt.Rows[0][0]) == 0)
                    {
                        ActionButton ab = new ActionButton();
                        ab.Action = "Add Client Calibration";
                        ab.Endpoint = "AddClientCalibration";
                        abs.Add(ab);
                    }
                }
                // already is in notifications, don't show button
                DataTable cali_dt = Common.GetTable("Select count(*) as num_recal, (select count(*) from vwCF where f_id = " + f_id + " and isrecal = 0) as num_cals from calibration_pending   where form_id = " + f_id + " And isrecal = 1");
                if (Convert.ToInt32(cali_dt.Rows[0]["num_recal"]) == 0 && Convert.ToInt32(cali_dt.Rows[0]["num_cals"]) == 1 & HttpContext.Current.User.IsInRole("QA"))
                {
                    ActionButton ab = new ActionButton();
                    ab.Action = "Add Recalibration";
                    ab.Endpoint = "AddCaliDispute";
                    abs.Add(ab);
                }
            }

            DataTable client_list = Common.GetTable("exec [getClientCalibrations] '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow cdr in client_list.Rows)
            {
                if (cdr["form_id"].ToString() == f_id)
                {
                    ActionButton ab = new ActionButton();
                    ab.Action = "Complete Review";
                    ab.Endpoint = "CompleteReview";
                    abs.Add(ab);
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
        /// GetBadCalls
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public BadCallListResponse GetBadCalls(string date_start, string date_end)
        {

            string sql = "exec [getUserScorecards] '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + date_start + "','" + date_end + "'";
            BadCallListResponse BCLR = new BadCallListResponse();
            DataTable calls_left = Common.GetTable(sql);
            List<BadCallInfo> badCallsListResult = new List<BadCallInfo>();
            foreach (DataRow dr in calls_left.Rows)
            {
                if (dr["bad_calls"].ToString() != "")
                {
                    var badCallsList = getBadCallsList(dr["short_name"].ToString(), date_start, date_end);
                    badCallsListResult.AddRange(badCallsList);
                }
            }

            BCLR.data = badCallsListResult;
            return BCLR;
        }

        /// <summary>
        /// getBadCallsList
        /// </summary>
        /// <param name="short_name"></param>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <returns></returns>
        public List<BadCallInfo> getBadCallsList(string short_name, string date_start, string date_end)
        {
            List<BadCallInfo> badCallInfoList = new List<BadCallInfo>();
            string sql = "select *, xcc_report_new.id as x_id from xcc_report_new  join scorecards on scorecards.id = xcc_report_new.scorecard join userapps on userapps.user_scorecard= xcc_report_new.scorecard where bad_call is not null  and scorecards.short_name = '" + short_name + "' and bad_call_accepted is null and username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' order by convert(date, call_date) desc";
            DataTable calls_left = Common.GetTable(sql);
            string json = "";

            foreach (DataRow dr in calls_left.Rows)
            {
                json += "{\"id\":\"" + dr["x_id"] + "\",\"scorecard\":\"" + short_name + "\",\"call_date\":\"" + dr["call_date"] + "\",\"phone\":\"" + dr["phone"] + "\",\"bad_call_reason\":\"" + dr["bad_call_reason"] + "\",\"audio_link\":\"" + dr["audio_link"] + "\"},";
                var callInfo = new BadCallInfo();
                callInfo.id = dr["x_id"].ToString();
                callInfo.scorecard = short_name;
                // callInfo.f_id = dr("f_id")
                callInfo.call_date = dr["call_date"].ToString();
                callInfo.phone = dr["phone"].ToString();
                callInfo.bad_call_reason = dr["bad_call_reason"].ToString();
                callInfo.audio_link = dr["audio_link"].ToString();
                callInfo.agent = dr["agent"].ToString();
                callInfo.agentGroup = dr["agent_group"].ToString();
                badCallInfoList.Add(callInfo);
            }

            if (Strings.Right(json, 1) == ",")
                json = Strings.Left(json, Strings.Len(json) - 1);
            return badCallInfoList;
        }


        /// <summary>
        /// GetAvailableAudios
        /// </summary>
        /// <param name="xcc_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public static List<string> GetAvailableAudios(string xcc_id) // List(Of ScorePerf)
        {
            List<string> audios = new List<string>();

            DataTable dt = Common.GetTable("exec GetAudioFiles " + xcc_id);
            foreach (DataRow dr in dt.Rows)
                audios.Add(ReverseMapPath(dr["filename"].ToString()));
            return audios;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetNotificationSteps(string form_id) // List(Of ScorePerf)
        {
            string dest_list = "Notes Only";
            string[] @internal = new[] { "QA", "Calibrator", "Recalibrator", "Center Manager", "Tango TL", "QA Lead", "Team Lead", "Account Manager", "Admin" };
            string[] external = new[] { "Agent", "Supervisor", "Manager", "Client", "Account Manager", "Admin" };

            string myRole = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).ToString();

            DataTable notify_dt = Common.GetTable("select * from VWFN where date_closed  is null and f_id = " + form_id);
            bool editable = true;
            foreach (DataRow dr in notify_dt.Rows)
            {
                // if there are open notifications, prevent lesser users from accessing them                
                if (Array.IndexOf(@internal, dr["role"].ToString()) > -1)
                {
                    if ((Array.IndexOf(@internal, myRole) >= Array.IndexOf(@internal, dr["role"].ToString())))
                    {
                        editable = true;
                        break;
                    }
                    else
                        editable = false;
                }

                if (Array.IndexOf(external, dr["role"].ToString()) > -1)
                {
                    if (Array.IndexOf(external, myRole) >= Array.IndexOf(external, dr["role"].ToString()))
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
                DataTable dt = Common.GetTable("select * from [dbo].[notification_steps] where profile_id = (select isnull(isnull(sc_profile, setting_profile),1) from vwForm join app_settings on app_settings.appname = vwForm.appname join scorecards on scorecards.id = vwForm.scorecard where f_id = " + form_id + ") and role = '" + myRole + "'");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["admin"].ToString() == "1")
                        dest_list = dest_list + "|Admin";

                    if (dt.Rows[0]["account_manager"].ToString() == "1")
                        dest_list = dest_list + "|Account Manager";
                    if (dt.Rows[0]["tango_tl"].ToString() == "1")
                        dest_list = dest_list + "|Tango Lead";

                    if (dt.Rows[0]["client"].ToString() == "1")
                        dest_list = dest_list + "|Client";
                    if (dt.Rows[0]["Manager"].ToString() == "1" | HttpContext.Current.User.Identity.Name == "QALVia")
                        dest_list = dest_list + "|Manager";

                    if (dt.Rows[0]["Supervisor"].ToString() == "1")
                        dest_list = dest_list + "|Supervisor";
                    if (dt.Rows[0]["Agent"].ToString() == "1")
                        dest_list = dest_list + "|Agent";
                    if (dt.Rows[0]["QA"].ToString() == "1")
                        dest_list = dest_list + "|QA";
                    if (dt.Rows[0]["Calibrator"].ToString() == "1")
                        dest_list = dest_list + "|Calibrator/Dispute";
                    if (dt.Rows[0]["TL"].ToString() == "1")
                        dest_list = dest_list + "|Team Lead";
                    if (dt.Rows[0]["CM"].ToString() == "1")
                        dest_list = dest_list + "|Center Manager";
                    dest_list = dest_list + "|Close It";
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
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetNotificationStatus(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {

            string ret_data = "";
            int row_count = 0;
            DataTable avg_dt = Common.GetTable("exec getNotificationStatus '" + start_date + "', '" + end_date + "',' " + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");
            foreach (DataRow dr in avg_dt.Rows)
            {
                if (row_count == 0)
                    ret_data += "<tr style='font-weight:bold'>";
                else
                    ret_data += "<tr>";

                ret_data += "<td>" + dr["Reviewer"].ToString() + "</td>";
                ret_data += "<td>" + dr["TN"].ToString() + "</td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AB\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["AB"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AA\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["AA"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"AD\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["AD"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SB\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["SB"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SA\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["SA"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"SD\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["SD"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"QB\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["QB"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"QA\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["QA"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LD\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["LB"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LA\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["LA"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"LD\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["LD"].ToString() + "</span></td>";
                ret_data += "<td><span style='cursor:pointer'  onclick='pop_notif_detail(\"TD\",\"" + dr["Reviewer"].ToString() + "\")';>" + dr["TD"].ToString() + "</span></td>";
                ret_data += "</tr>";

                row_count += 1;
            }
            return ret_data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getSCSectionStats(string sc_id, string start_date, string end_date, string filter_array)
        {

            string ret_data = "";
            int row_count = 0;
            DataTable avg_dt = Common.GetTable("exec getSCSectionStats '" + sc_id + "','" + start_date + "', '" + end_date + "', '" + filter_array + "'");
            string previous_section = "";

            foreach (DataRow dr in avg_dt.Rows)
            {
                if (previous_section != dr["section"].ToString())
                {
                    ret_data += "<tr data-datapoint='" + dr["top5"].ToString() + "'><td class='qname'><a onclick=\"addArrayFilter('Section','" + dr["id"].ToString() + "', '" + dr["section"].ToString() + "');\"><strong>" + dr["section"].ToString() + "</strong></a></td><td align='right' class='qscore' data-callscount='" + dr["section_right"].ToString() + " of " + dr["total_calls"].ToString() + " Calls'><strong>" + Strings.FormatNumber(Convert.ToInt32( dr["section_right"]) * 100 / (double)dr["total_calls"], 1) + "</strong></td></tr>";
                    previous_section = dr["section"].ToString();
                }

                ret_data += "<tr data-datapoint='" + dr["top5"].ToString() + "'>";
                ret_data += "<td class='qname'><a data-QID='" + dr["qid"].ToString() + "' onclick=\"addArrayFilter('Missed','" + dr["qid"].ToString() + "', '" + dr["q_short_name"].ToString() + "');\">" + dr["q_short_name"].ToString() + "</a></td>";
                ret_data += "<td align='right' class='qscore' data-callscount='" + dr["num_right"].ToString() + " of " + dr["total_calls"].ToString() + " Calls'>" + Strings.FormatNumber(Convert.ToInt32(dr["num_right"]) * 100 / (double)dr["total_calls"], 1) + "</td></tr>";
            }
            return ret_data;
        }

        /// <summary>
        /// GetQStats
        /// </summary>
        /// <param name="q_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<AnswerObject> GetQStats(string q_id, string start_date, string end_date, string hdnAgentFilter = "", string filter_array = "")
        {
            if (hdnAgentFilter == null)
                hdnAgentFilter = "";
            if (filter_array == null)
                filter_array = "";
            List<AnswerObject> answers = new List<AnswerObject>();

            if (string.IsNullOrEmpty(q_id))
                return answers;
            DataTable avg_dt = Common.GetTable("exec getQStats '" + q_id + "','" + start_date + "', '" + end_date + "', '" + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");

            foreach (DataRow dr in avg_dt.Rows)
                answers.Add(new AnswerObject()
                {
                    ID = -1,
                    Comment = dr["answer_text"].ToString(),
                    IsRight = dr["right_answer"].ToString() == "True",
                    Count = Convert.ToInt32(dr["count_q"]),
                    Total = Convert.ToInt32(dr["total_q"])
                });

            avg_dt = Common.GetTable("exec getQRStats '" + q_id + "','" + start_date + "', '" + end_date + "', '" + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");

            foreach (DataRow dr in avg_dt.Rows)

                answers.Add(new AnswerObject()
                {
                    ID = Convert.ToInt32(dr["id"]),
                    Comment = dr["comment"].ToString(),
                    IsRight = dr["right_answer"].ToString() == "True",
                    Count = Convert.ToInt32(dr["count_q"]),
                    Total = Convert.ToInt32(dr["total_q"]),
                    CallDetails = GetCallDetailsList(start_date, end_date, Convert.ToInt32(dr["id"]))
                });

            return answers;
        }

        /// <summary>
        /// GetCallDetailsList
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<AnswerCallDetail> GetCallDetailsList(string startDate, string endDate, int id)
        {
            List<AnswerCallDetail> detailsList = new List<AnswerCallDetail>();

            DataTable detailsDT;
            detailsDT = Common.GetTable("SELECT F_ID, IsNull(Phone,'') AS Phone, LeadID, IsNull(Agent,'') AS Agent FROM vwForm WHERE CONVERT(DATE, Review_Date) BETWEEN '" + startDate + "' AND '" + endDate + "' AND F_ID IN (SELECT Form_ID FROM Form_Q_Responses WHERE Answer_ID = " + id + ")");
            foreach (DataRow detailsDR in detailsDT.Rows)
                detailsList.Add(new AnswerCallDetail()
                {
                    ReviewID = Convert.ToInt32(detailsDR["F_ID"]),
                    Phone = detailsDR["Phone"].ToString(),
                    LeadID = detailsDR["LeadID"].ToString(),
                    Agent = detailsDR["Agent"].ToString()
                });
            return detailsList;
        }

        /// <summary>
        /// GetAverages
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetAverages(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {

            DateTime datTime = new DateTime();
            string month = DateInterval.Month.ToString();
            int one=-1;
            string fullstart_date = month + one + start_date;
            string fullend_date = month + one + end_date;
            int avg_dt1 = 0;
            string prior_start_date = datTime.AddDays(Convert.ToDouble(fullstart_date)).ToShortDateString();
            string prior_end_date = datTime.AddDays(Convert.ToDouble(fullend_date)).ToShortDateString();

            DataTable avg_dt = Common.GetTable("exec [GetAverages] '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");
            if (avg_dt.Rows.Count == 2)
                avg_dt1 = Convert.ToInt32(avg_dt.Rows[0][0]) - Convert.ToInt32(avg_dt.Rows[1][0]);
            return avg_dt.Rows[0][0] + ":" + avg_dt1.ToString();
        }

        /// <summary>
        /// UpdateNotification2
        /// </summary>
        /// <param name="noti_id"></param>
        /// <param name="noti_response"></param>
        /// <param name="noti_button"></param>
        /// <param name="noti_notes"></param>
        /// <param name="noti_override"></param>
        /// <param name="noti_step"></param>
        /// <param name="form_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public disputeResult UpdateNotification2(int noti_id, string noti_response, string noti_button, string noti_notes, bool noti_override, string noti_step, string form_id, string question_id)
        {
            disputeResult disp_res = new disputeResult();
            string sql = "";
            string ack_by = HttpContext.Current.User.Identity.Name;

            string role = Roles.GetRolesForUser(ack_by).Single();
            if (noti_button != "Notes Only" & noti_button != "Update")
            {
                if (noti_notes != "")
                    sql = "update form_notifications set date_closed = dbo.getMTDate(), closed_by = '" + ack_by + "', close_reason = 'Updated', comment = @new_comments where id = " + noti_id + " and date_closed is null and ((role = '" + role + "') or ('Admin'  = '" + role + "') or ('Client'  = '" + role + "')); ";
                else
                    sql = "update form_notifications set date_closed = dbo.getMTDate(), closed_by = '" + ack_by + "', close_reason = 'Updated', comment = @new_comments where id = " + noti_id + " and date_closed is null and ((role = '" + role + "') or ('Admin'  = '" + role + "') or ('Client'  = '" + role + "')); ";
            }
            else if (noti_notes != "")
                sql = "insert into form_notifications (date_created,date_closed,closed_by,close_reason, comment, form_id, role) select dbo.getMTDate(), dbo.getMTDate(), '" + ack_by + "',  'Update',   @new_comments, " + form_id + ",(Select user_role from userextrainfo   where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'); ";

            if (noti_button == "Ack Sup Override")
                sql += "delete from form_notifications where id = " + noti_id + "; ";
            switch (noti_button)
            {
                case "Supervisor":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Supervisor',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "Agent":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Agent',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "Calibrator":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Calibrator',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "QA":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "Team Lead":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'QA Lead',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "Account Manager":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Admin',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }

                case "Manager":
                    {
                        sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by) select 'Manager',  dbo.getMTDate(), " + form_id + ", '" + ack_by + "'";
                        sql += "; update form_notifications set close_reason = 'Sent to " + noti_button + "' where id = " + noti_id;
                        disp_res.dispute_complete = false;
                        break;
                    }
            }

            disp_res.dispute_response = sql;
            if (sql != "")
            {
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
                cn.Open();
                SqlCommand reply = new SqlCommand(sql, cn);
                reply.CommandTimeout = 60;
                reply.Parameters.AddWithValue("new_comments", noti_notes.ToString());
                reply.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
            }
            if (noti_button == "Agenda Item")
                disp_res.dispute_redirect = "agenda_items.aspx";
            return disp_res;
        }

        /// <summary>
        /// UpdateNotification
        /// </summary>
        /// <param name="noti_id"></param>
        /// <param name="noti_response"></param>
        /// <param name="noti_button"></param>
        /// <param name="noti_notes"></param>
        /// <param name="noti_override"></param>
        /// <param name="noti_step"></param>
        /// <param name="form_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public disputeResult UpdateNotification(int noti_id, string noti_response, string noti_button, string noti_notes, bool noti_override, string noti_step, string form_id, string question_id)
        {
            disputeResult disres = new disputeResult();
            string ack_by = "";
            ack_by = HttpContext.Current.User.Identity.Name;
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + ack_by + "'");

            if (user_dt.Rows.Count > 0)
                noti_step = user_dt.Rows[0]["user_role"].ToString();

            string sql = "";
            string addl_sql = "";
            if (noti_override == true)
                addl_sql = " sup_override = 1, ";

            disres.dispute_complete = true;
            if (noti_step == "Ack Sup Override")
                noti_override = true;
            string prev_step = " with ";
            switch (noti_step)
            {
                case "Supervisor":
                    {
                        prev_step += "QA";
                        break;
                    }

                case "QA":
                    {
                        prev_step += "Supervisor";
                        break;
                    }

                case "QA Lead":
                case "Calibrator":
                    {
                        prev_step += "Supervisor";
                        break;
                    }

                case "Agent":
                    {
                        prev_step += "QA";
                        break;
                    }
            }
            switch (noti_response)
            {
                case "Agree":
                    {
                        sql = "update form_notifications set role = (select user_role from Userextrainfo where username = '" + ack_by + "'), comment =  @new_comments, date_closed = dbo.getMTDate(), closed_by = '" + ack_by + "', close_reason  = '" + noti_response + "' where id = " + noti_id + "; ";
                        if (noti_notes == "")
                            sql += "INSERT INTO [form_notifications] ([role], [date_created], date_closed,  [form_id],  opened_by, closed_by, comment, close_reason, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + ack_by + "','" + noti_step + " AGREED " + prev_step + "','Agree','" + question_id + "'; ";
                        else
                            noti_notes = noti_step + " AGREED " + prev_step + " - " + noti_notes;
                        if (noti_step == "QA Lead" | noti_step == "Calibrator")
                        {
                            disres.dispute_complete = false;
                            noti_step = "QAL to QA";
                            noti_response = "Disagree";
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + question_id + "'";
                        }

                        break;
                    }

                case "Disagree":
                    {
                        sql = "update form_notifications set  role = (select user_role from Userextrainfo where username = '" + ack_by + "'), comment =  @new_comments, date_closed = dbo.getMTDate(), closed_by = '" + ack_by + "', close_reason  = '" + noti_response + "' where id = " + noti_id + "; ";
                        if (noti_notes == "")
                            sql += "INSERT INTO [form_notifications] ([role], [date_created], date_closed,  [form_id],  opened_by, closed_by, comment, close_reason, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + ack_by + "','" + noti_step + " DISAGREED " + prev_step + "','Disagree','" + question_id + "'; ";
                        else
                            noti_notes = noti_step + " DISAGREED " + prev_step + " - " + noti_notes;
                        disres.dispute_complete = false;
                        break;
                    }

                case "Notes Only":
                    {
                        Common.UpdateTable("INSERT INTO [form_notifications] ([role], [date_created], [date_closed], closed_by,  [form_id],  opened_by,  comment, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(),  dbo.getMTDate(),'" + ack_by + "'," + form_id + ", '" + ack_by + "','" + noti_notes.Replace("'", "''") + "','" + question_id + "'; ");
                        disres.dispute_complete = false;
                        break;
                    }
            }
            if (((noti_response == "Disagree") | ((noti_id == 0) & noti_button == "New")) & !noti_override)
            {
                switch (noti_step)
                {
                    case "Supervisor":
                    case "Client":
                        {
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + question_id + "'; Select @@identity;";
                            break;
                        }

                    case "QA Lead":
                    case "Calibrator":
                        {
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + question_id + "'; Select @@identity;";
                            break;
                        }

                    case "Agent":
                        {
                            sql += "INSERT INTO [form_notifications] ([role], [date_created],  [form_id],  opened_by, question_id) select  (select user_role from Userextrainfo where username = '" + ack_by + "'),  dbo.getMTDate(), " + form_id + ", '" + ack_by + "','" + question_id + "'; Select @@identity;";
                            break;
                        }
                }
            }
            // Return sql
            disres.dispute_response = sql;
            int result_dt = 0;
            if (sql != "")
            {
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
                cn.Open();

                SqlCommand reply = new SqlCommand(sql, cn);
                reply.CommandTimeout = 60;
                if (noti_response != "")
                    reply.Parameters.AddWithValue("new_comments", noti_notes);
                result_dt = Convert.ToInt32(reply.ExecuteScalar());

                cn.Close();
                cn.Dispose();
            }


            if (noti_override == false & noti_response == "Disagree" & ((noti_step == "Supervisor") | (noti_step == "Client")))
            {
                DataTable noti_dt = Common.GetTable("select form_id from form_notifications where id = " + noti_id);
                if (noti_dt.Rows.Count > 0)
                {
                }
            }

            if (noti_id == 0)
                disres.dispute_id = result_dt;
            else
                disres.dispute_id = noti_id;
            Common.UpdateTable("exec updateAllComments " + form_id);

            return disres;
        }


        /// <summary>
        /// CreateComment
        /// </summary>
        /// <param name="noti_response"></param>
        /// <param name="noti_id"></param>
        protected void CreateComment(string noti_response, string noti_id)
        {
            if (noti_response != "")
            {
                string sql;
                sql = "insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id) select '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "', dbo.getMTDate(), @new_comments, 'Notification', " + noti_id;
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
                cn.Open();

                SqlCommand reply = new SqlCommand(sql, cn);
                reply.CommandTimeout = 60;
                if (noti_response != "")
                    reply.Parameters.AddWithValue("new_comments", noti_response);
                reply.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
            }
        }

        /// <summary>
        /// GetStats
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public WebApi.Models.CDService.Stats GetStats(string start_date, string end_date, string hdnAgentFilter, string filter_array = "")
        {
            DataTable stats_dt = Common.GetTable("getTodayStats '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + filter_array + "'");

            WebApi.Models.CDService.Stats _stats = new WebApi.Models.CDService.Stats();
            // row 0 is now, row 1 is compare
            _stats.CallsReviewed = stats_dt.Rows[0]["total_calls"].ToString();
            _stats.CRCallDifference = Convert.ToString(Convert.ToInt32(stats_dt.Rows[0]["total_calls"]) - Convert.ToInt32(stats_dt.Rows[1]["total_calls"]));

            if (Convert.ToInt32(stats_dt.Rows[0]["total_calls"]) > Convert.ToInt32(stats_dt.Rows[1]["total_calls"]))
                _stats.CRDirection = "positive";
            else
                _stats.CRDirection = "negative";

            _stats.CallsFailed = stats_dt.Rows[0]["total_fails"].ToString();
            _stats.CFDifference = Convert.ToString(Convert.ToInt32(stats_dt.Rows[0]["total_fails"]) - Convert.ToInt32(stats_dt.Rows[1]["total_fails"]));
            if (Convert.ToInt32(stats_dt.Rows[0]["total_fails"]) > Convert.ToInt32(stats_dt.Rows[1]["total_fails"]))
                _stats.CFDirection = "positive";
            else
                _stats.CFDirection = "negative";

            _stats.NumMinutes = Convert.ToString(Convert.ToInt32(stats_dt.Rows[0]["total_minutes"]));
            _stats.NMDifference = Convert.ToString(Convert.ToInt32(stats_dt.Rows[0]["total_minutes"]) - Convert.ToInt32(stats_dt.Rows[1]["total_minutes"]));
            if (Convert.ToInt32(stats_dt.Rows[0]["total_minutes"]) > Convert.ToInt32(stats_dt.Rows[1]["total_minutes"]))
                _stats.NMDirection = "positive";
            else
                _stats.NMDirection = "negative";

            _stats.NumAgents = Convert.ToString(stats_dt.Rows[0]["total_agents"]);
            _stats.NADifference = Convert.ToString(Convert.ToInt32(stats_dt.Rows[0]["total_agents"]) - Convert.ToInt32(stats_dt.Rows[1]["total_agents"]));
            if (Convert.ToInt32(stats_dt.Rows[0]["total_agents"]) > Convert.ToInt32(stats_dt.Rows[1]["total_agents"]))
                _stats.NADirection = "positive";
            else
                _stats.NADirection = "negative";

            return _stats;
        }
        /// <summary>
        /// GetMyCalibrations
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetMyCalibrations(string username)
        {
            string user_roles = string.Join(", ", Roles.GetRolesForUser(username));
            DataTable cali_dt = new DataTable();
            switch (user_roles)
            {
                case "Client":
                case "Supervisor":
                case "Manager":
                    {
                        cali_dt = Common.GetTable("select dbo.ConvertTimeToHHMMSS(sum(call_length),'s') as calltime, count(*) as num_calls, scorecard_name, scorecard from cali_pending_client join vwForm on vwForm.F_ID = form_id  where assigned_to = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' and cali_pending_client.date_completed is null group by scorecard_name, scorecard");
                        break;
                    }

                default:
                    {
                        cali_dt = Common.GetTable("exec getMyCalibrationList '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                        break;
                    }
            }
            string ret_string = "";
            TimeSpan total_time = new TimeSpan(0, 0, 0);
            if (cali_dt != null)
            {
                if (cali_dt.Rows.Count > 0)
                    ret_string = "<tr><td><strong>Listen</strong></td><td colspan='2'><a href='/calibrate/' target='_blank'><img src='/img/small_play.png' height=30></a></td></tr>";
                foreach (DataRow dr in cali_dt.Rows)
                {
                    string[] call_item_pieces = dr["calltime"].ToString().Split(':');
                    TimeSpan temp_ts = new TimeSpan(Convert.ToInt32(call_item_pieces[0]), Convert.ToInt32(call_item_pieces[1]), Convert.ToInt32(call_item_pieces[2]));
                    total_time = total_time.Add(temp_ts);
                    ret_string += "<tr><td>" + dr["scorecard_name"].ToString() + "</td><td>" + dr["num_calls"].ToString() + "</td><td>" + dr["calltime"].ToString() + "</td></tr>";
                }

                DataTable time_left_dt = Common.GetTable("select avg(datediff(s, calibration_form.review_started, calibration_form.review_date)/call_length) as avg_speed from vwFOrm join calibration_form on calibration_form.original_form = vwForm.f_id  where call_length > 0 and call_length is not null and calibration_form.reviewed_by = '" + username + "' and calibration_form.review_date > DATEADD(d, -30, getdate())");
                if (time_left_dt.Rows.Count > 0)
                {
                    if (!Information.IsDBNull(time_left_dt.Rows[0][0]))
                        total_time = TimeSpan.FromTicks(Convert.ToInt32(total_time.Ticks) * Convert.ToInt32(time_left_dt.Rows[0][0]));
                }
            }

            if (user_roles == "QA Lead" | user_roles == "Admin")
            {
                cali_dt = Common.GetTable("exec getAllCalibrationList '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
                if (cali_dt != null)
                {
                    if (cali_dt.Rows.Count > 0)
                    {
                        ret_string += "<tr><td colspan='3'><hr></td></tr>";
                        ret_string += "<tr><td colspan='3'>All Pending Calibrations</td></tr>";
                    }

                    foreach (DataRow dr in cali_dt.Rows)
                        ret_string += "<tr><td>" + dr["scorecard_name"].ToString() + "</td><td>" + dr["num_calls"].ToString() + "</td><td>" + dr["calltime"].ToString() + "</td></tr>";
                }
            }
            return System.Convert.ToString(total_time.TotalHours) + ":" + total_time.ToString(@"mm\:ss") + "|" + ret_string;
        }

        /// <summary>
        /// GetSearchResults
        /// </summary>
        /// <param name="test_string"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<search_results> GetSearchResults(string test_string)
        {
            // exec SearchAutoComplete '700','stacemoss'
            List<search_results> sr_items = new List<search_results>();
            DataTable sr_dt = Common.GetTable("exec SearchAutoComplete '" + test_string.Replace("'", "''") + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow dr in sr_dt.Rows)
            {
                search_results _sr_item = new search_results();
                _sr_item.label = dr["item_type"].ToString();
                _sr_item.value = dr["suggested"].ToString();
                _sr_item.form_id = dr["form_id"].ToString();
                sr_items.Add(_sr_item);
            }

            return sr_items;
        }



        /// <summary>
        /// GetActiveQA
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<ActiveQAStats> GetActiveQA(string start_date, string end_date)
        {
            List<ActiveQAStats> mi_items = new List<ActiveQAStats>();
            DataTable stats_dt;
            stats_dt = Common.GetTable("exec getActiveQA '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                ActiveQAStats _mi = new ActiveQAStats();
                _mi.reviewer = dr["reviewer"].ToString();
                _mi.number_calls = dr["number_calls"].ToString();
                _mi.short_name = dr["short_name"].ToString();
                _mi.Last_reviewed = dr["Last_reviewed"].ToString();
                _mi.days_active = dr["days_active"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetAgentRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="q_short_name"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<AgentRanks> GetAgentRanking(string start_date, string end_date, string hdnAgentFilter, string q_short_name = "", string filter_array = "")
        {


            List<AgentRanks> mi_items = new List<AgentRanks>();
            DataTable stats_dt;
            if (q_short_name != "")
                stats_dt = Common.GetTable("getAgentRanking '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + filter_array.Replace("'", "''") + "','" + q_short_name + "'");
            else
                stats_dt = Common.GetTable("getAgentRanking '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + filter_array.Replace("'", "''") + "'");

            foreach (DataRow dr in stats_dt.Rows)
            {
                AgentRanks _mi = new AgentRanks();
                _mi.avg_score = dr["avg_score"].ToString();
                _mi.agent_name = dr["agentname"].ToString();
                _mi.div_color = dr["div_color"].ToString();
                _mi.top3 = dr["top3"].ToString();
                _mi.ni_scorecard = dr["ni_scorecard"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }



        /// <summary>
        /// getNotificationData
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public NotificationData getNotificationData(string form_id)
        {
            DataTable nd_dt = Common.GetTable("select total_score, agent, phone, review_date,  dbo.[GetFormattedMQ] (" + form_id + ") as missed_blob, [dbo].[getNotificationComments] (" + form_id + ") as comments from vwForm where F_ID = " + form_id);
            NotificationData notidata = new NotificationData();
            if (nd_dt.Rows.Count > 0)
            {
                notidata.agent = nd_dt.Rows[0]["agent"].ToString();
                notidata.total_score = nd_dt.Rows[0]["total_score"].ToString();
                notidata.phone = nd_dt.Rows[0]["phone"].ToString();
                notidata.review_date = nd_dt.Rows[0]["review_date"].ToString();
                notidata.Missed_blob = nd_dt.Rows[0]["Missed_blob"].ToString();
                notidata.comments = nd_dt.Rows[0]["comments"].ToString();
            }
            return notidata;
        }



        /// <summary>
        /// getAppsStatus
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<AppStatus> getAppsStatus(string start_date, string end_date)
        {

            List<AppStatus> mi_items = new List<AppStatus>();
            DataTable stats_dt = Common.GetTable("getAppsStatus '" + start_date + "','" + end_date + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                AppStatus _mi = new AppStatus();
                _mi.appname = dr["appname"].ToString();
                _mi.total_loaded = dr["total_loaded"].ToString();
                _mi.pending = dr["pending"].ToString();
                _mi.need_audio = dr["need_audio"].ToString();
                _mi.bad_calls = dr["bad_calls"].ToString();
                _mi.Priority = dr["Priority"].ToString();

                _mi.Last_Loaded_Date = Convert.ToDateTime(dr["Last Loaded Date"]).ToShortDateString();
                _mi.avg_score = dr["avg_score"].ToString();
                _mi.std_dev = dr["std_dev"].ToString();
                _mi.number_loaded = dr["number_loaded"].ToString();
                _mi.call_date = dr["call_date"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }


        /// <summary>
        /// GetPay
        /// </summary>
        /// <param name="username"></param>
        /// <param name="week_ending"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetPay(string username, string week_ending, string scorecard)
        {
            if (scorecard == "undefined")
                scorecard = "ALL";
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            DataTable stats_dt;
            if (user_dt.Rows[0]["user_role"].ToString() == "Calibrator")
                stats_dt = Common.GetTable("getPay2SCCalib '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + week_ending + "','" + scorecard + "'");
            else
                stats_dt = Common.GetTable("getPay2SC '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + week_ending + "','" + scorecard + "'");
            string ret_val = "";

            float total_pay = 0;
            string base_pay = "0";
            string start_date = "";
            int num_by_qal = 0;
            if (user_dt.Rows[0]["user_role"].ToString() == "Calibrator")
            {
                foreach (DataRow dr in stats_dt.Rows)
                {
                    base_pay = dr["base"].ToString();
                    start_date = dr["startdate"].ToString();
                    try
                    {
                        num_by_qal = Convert.ToInt32(num_by_qal + Convert.ToInt32(dr["num_by_qal"]));
                    }
                    catch (Exception ex)
                    {
                        num_by_qal = num_by_qal + 0;
                    }
                    if (dr["short_name"].ToString() != "")
                    {
                        ret_val += "<tr><td>Scorecard</td><td>" + dr["short_name"].ToString() + "</td></tr>";
                        ret_val += "<tr><td>Review Time</td><td>" + dr["reviewtime"].ToString() + "</td></tr>";
                        ret_val += "<tr><td>Calibration Score</td><td><a href='view_recalibration.aspx?reviewer=" + HttpContext.Current.User.Identity.Name + "&we_date=" + week_ending + "&scorecard=" + dr["scorecard"] + "' target=_blank>" + Strings.FormatNumber(dr["calibration_score"].ToString(), 2) + "</a></td></tr>";
                        ret_val += "<tr><td>Adjust Hourly Rate</td><td>" + Strings.FormatCurrency(Convert.ToInt32(dr["base"]) * (100 + Convert.ToInt32(dr["cal_percent"])) / (double)100) + "</td></tr>";

                        float new_base = (float)(dr["base"]) * (100 + Convert.ToInt32(dr["cal_percent"])) / 100;
                        string[] rev_time = dr["reviewtime"].ToString().Split(':');
                        if (rev_time.Length == 3)
                        {
                            ret_val += "<tr><td>Pay</td><td>" + Strings.FormatCurrency(new_base * (Convert.ToInt32(rev_time[0]) + Convert.ToInt32(rev_time[1]) / 60 + Convert.ToInt32(rev_time[2]) / 3600) + Convert.ToInt32(dr["websites"]) * Convert.ToInt32(dr["website_pay"])) + "</td></tr>";
                            total_pay += new_base * (Convert.ToInt32(rev_time[0]) + Convert.ToInt32(rev_time[1]) / 60 + Convert.ToInt32(rev_time[2]) / 3600) + Convert.ToInt32(dr["websites"]) * Convert.ToInt32(dr["website_pay"]);
                        }
                        else
                            ret_val += "<tr><td>Pay</td><td>$0.00</td></tr>";
                        ret_val += "<tr><td colspan=2>&nbsp;</td></tr>";
                    }
                }

                stats_dt = Common.GetTable("getPay2SC '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + week_ending + "','" + scorecard + "'");

                if (stats_dt.Rows.Count > 0)
                {
                    ret_val += "<tr><td colspan=2>QA Pay</td></tr>";
                    ret_val += "<tr><td colspan=2><hr></td></tr>";
                }
            }
            foreach (DataRow dr in stats_dt.Rows)
            {
                base_pay = dr["base"].ToString();
                start_date = dr["startdate"].ToString();
                ret_val += "<tr><td>Scorecard</td><td>" + dr["short_name"].ToString() + "</td></tr>";
                ret_val += "<tr><td>Review Time</td><td>" + dr["reviewtime"].ToString() + "</td></tr>";
                ret_val += "<tr><td>Call Speed</td><td>" + dr["efficiency"].ToString() + "</td></tr>";
                ret_val += "<tr><td>Calibration Score</td><td><a href='view_calibration.aspx?reviewer=" + HttpContext.Current.User.Identity.Name + "&we_date=" + week_ending + "&scorecard=" + dr["scorecard"] + "' target=_blank>" + Strings.FormatNumber(dr["calibration_score"].ToString(), 2) + "</a></td></tr>";

                float new_base = 0;
                float efficiency = 100;
                if (Convert.ToInt32(dr["efficiency"]) > 100)
                    efficiency = (((float)dr["efficiency"] - 100) / 2 + 100) / 100;
                else
                    efficiency = (float)dr["efficiency"] / 100;

                if (Information.IsNumeric(dr["efficiency"].ToString()))
                    new_base = (float)dr["base"] * (100 + (float)dr["cal_percent"]) * efficiency / 100;

                ret_val += "<tr><td>Adjust Hourly Rate</td><td>" + Strings.FormatCurrency(new_base, 2) + "</td></tr>";

                ret_val += "<tr><td>Disputes</td><td><a href='ValidDisputes.aspx?wedate=" + week_ending + "&qa=" + HttpContext.Current.User.Identity.Name + "' target=_blank>" + dr["num_disputes"].ToString() + "</a></td></tr>";
                ret_val += "<tr><td>Disputes Deduction</td><td>" + Strings.FormatCurrency(dr["dispute_cost"]) + "</td></tr>";
                string[] rev_time = dr["reviewtime"].ToString().Split(':');
                if (rev_time.Length == 3)
                {
                    ret_val += "<tr><td>Pay</td><td>" + Strings.FormatCurrency(new_base * (Convert.ToInt32(rev_time[0]) + Convert.ToInt32(rev_time[1]) / (double)60 + Convert.ToInt32(rev_time[2]) / (double)3600) - Convert.ToInt32(dr["dispute_cost"]) + Convert.ToInt32(dr["websites"]) * Convert.ToInt32(dr["website_pay"]) + "</td></tr>");
                    total_pay += new_base * (Convert.ToInt32(rev_time[0]) + Convert.ToInt32(rev_time[1]) / 60 + Convert.ToInt32(rev_time[2]) / 3600) - Convert.ToInt32(dr["dispute_cost"]) + Convert.ToInt32(dr["websites"]) * Convert.ToInt32(dr["website_pay"]);
                }
                else
                    ret_val += "<tr><td>Pay</td><td>$0.00</td></tr>";
                ret_val += "<tr><td colspan=2>&nbsp;</td></tr>";
            }

            string header = "<table class='detailsTable' style='font:smaller;'><tr><td><strong>Summary</strong></td></tr>";
            header += "<tr><td>Start Date</td><td>" + start_date + "</td></tr>";
            header += "<tr><td>Base Pay</td><td>" + Strings.FormatCurrency(base_pay) + "</td></tr>";
            header += "<tr><td>Total Pay</td><td>" + Strings.FormatCurrency(total_pay + num_by_qal * 0.4) + "</td></tr>";
            header += "<tr><td><strong>Details</strong></td></tr>";
            ret_val = header + ret_val;

            if (user_dt.Rows[0]["user_role"].ToString() == "Calibrator")
            {
                ret_val += "<tr><td>Notification Pay</td></tr>";
                ret_val += "<tr><td>Pay Per Notificaiton</td><td>$0.40</td></tr>";
                ret_val += "<tr><td>Notification Completed</td><td>" + num_by_qal + "</td></tr>";
                ret_val += "<tr><td>Total Pay</td><td>" + Strings.FormatCurrency(num_by_qal * 0.4) + "</td></tr>";
            }
            ret_val += "</table>";
            return ret_val;
        }



        /// <summary>
        /// GetTrainTopMissed
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="agent"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<MissedItems> GetTrainTopMissed(string start_date, string end_date, string hdnAgentFilter, string agent, string filter_array = "")
        {

            List<MissedItems> mi_items = new List<MissedItems>();
            DataTable stats_dt = Common.GetTable("getTopMissedTrain '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + filter_array.Replace("'", "''") + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                MissedItems _mi = new MissedItems();
                _mi.num_calls = dr["num_calls"].ToString();
                _mi.div_color = dr["div_color"].ToString();
                _mi.Percent_Qs = dr["Percent_Qs"].ToString();
                _mi.q_short_name = dr["q_short_name"].ToString();
                _mi.top_missed = dr["top_missed"].ToString();
                _mi.total_wrong = dr["total_wrong"].ToString();
                _mi.QID = dr["question_id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetQATopMissed
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="agent"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<MissedItems> GetQATopMissed(string start_date, string end_date, string hdnAgentFilter, string agent, string filter_array = "")
        {
            List<MissedItems> mi_items = new List<MissedItems>();
            DataTable stats_dt = Common.GetTable("getTopMissedCalibWithQA_stripped '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + filter_array.Replace("'", "''") + "'");

            foreach (DataRow dr in stats_dt.Rows)
            {
                MissedItems _mi = new MissedItems();
                _mi.num_calls = dr["num_calls"].ToString();
                _mi.div_color = dr["div_color"].ToString();
                _mi.Percent_Qs = dr["Percent_Qs"].ToString();
                _mi.q_short_name = dr["q_short_name"].ToString();
                _mi.top_missed = dr["top_missed"].ToString();
                _mi.total_wrong = dr["total_wrong"].ToString();
                _mi.QID = dr["question_id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetTopMissed
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="agent"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<MissedItems> GetTopMissed(string start_date, string end_date, string hdnAgentFilter, string agent, string filter_array = "")
        {

            List<MissedItems> mi_items = new List<MissedItems>();
            DataTable stats_dt = Common.GetTable("getMissedWithReps '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + hdnAgentFilter.Replace("'", "''") + "','" + agent + "','" + filter_array + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                MissedItems _mi = new MissedItems();
                _mi.num_calls = dr["num_calls"].ToString();
                _mi.div_color = dr["div_color"].ToString();
                _mi.Percent_Qs = dr["Percent_Qs"].ToString();
                _mi.q_short_name = dr["q_short_name"].ToString();
                _mi.top_missed = dr["top_missed"].ToString();
                _mi.total_wrong = dr["total_wrong"].ToString();
                _mi.QID = dr["question_id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }



        /// <summary>
        /// clearDash
        /// </summary>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void clearDash()
        {
            Common.UpdateTable("delete from UserDash where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
        }

        /// <summary>
        /// deDupeDash
        /// </summary>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void deDupeDash()
        {
            Common.UpdateTable("exec dedupedash '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
        }

        /// <summary>
        /// updateDash
        /// </summary>
        /// <param name="controlname"></param>
        /// <param name="controlorder"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void updateDash(string controlname, string controlorder)
        {
            Common.UpdateTable("updateDash @username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "', @controlname='" + controlname + "', @controlorder='" + controlorder + "'");
        }

        /// <summary>
        /// RemoveDash
        /// </summary>
        /// <param name="controlname"></param>
        /// <param name="controlorder"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void RemoveDash(string controlname, string controlorder)
        {
            Common.UpdateTable("delete from userdash where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' and controlname='" + controlname + "'");
        }

        /// <summary>
        /// GetIndicators
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetIndicators(string form_id)
        {

            string div_response = "";
            DataTable call_dt = Common.GetTable("select * from form_score3 where id = " + form_id);
            string call_length = "";

            if (call_dt.Rows.Count > 0)
                call_length = call_dt.Rows[0]["call_length"].ToString();
            DataTable stats_dt;
            stats_dt = Common.GetTable("select q_short_name,q_position,case when right_answer = 0 then 'bad-response' else '' end as [bad-response] from dbo.form_q_scores fs join [Questions] q on fs.question_ID = q.ID join question_answers qa on qa.ID = question_answered where form_ID = " + form_id);
            // each answer
            foreach (DataRow dr in stats_dt.Rows)
            {
                int position = 0;

                if (Convert.ToInt32(call_length) > 0 & dr["q_position"].ToString() != "")
                    position = System.Convert.ToInt32(dr["q_position"]) / Convert.ToInt32(call_length) * 100;

                if (dr["bad-response"].ToString() == "")
                    div_response += " <div class='warning-part' style='left: " + position + "%;' title='" + dr["q_short_name"] + "'><span></span><a onclick='jumpPos(" + dr["q_position"] + ");' class='listen-from-here' style='background: none repeat scroll 0% 0% rgb(154, 190, 46);'>&#x2714</a></div>" + Strings.Chr(13);
                else
                    div_response += " <div class='warning-part' style='left: " + position + "%;' title='" + dr["q_short_name"] + "'><span></span><a onclick='jumpPos(" + dr["q_position"] + ");' class='listen-from-here'>!</a></div>" + Strings.Chr(13);
            }
            stats_dt = Common.GetTable("select option_value, option_pos from form_q_scores_options where form_id = " + form_id + " and question_id in (select id from questions where template='Preferences')");
            // each template answer
            foreach (DataRow dr in stats_dt.Rows)
            {
                int position = 0;
                if (Convert.ToInt32(call_length) > 0 & dr["option_pos"].ToString() != "")
                    position = System.Convert.ToInt32(dr["option_pos"]) / Convert.ToInt32(call_length) * 100;
                div_response += " <div class='warning-part' style='left: " + position + "%;' title='" + dr["option_value"].ToString() + "'><span></span><a onclick='jumpPos(" + dr["option_pos"] + ");' class='listen-from-here' style='cursor: pointer;background: none repeat scroll 0% 0% rgb(102,178,255);'>&#x2724</a></div>" + Strings.Chr(13);
            }

            stats_dt = Common.GetTable("select option_value, option_pos from form_q_scores_options where form_id = " + form_id + " and question_id in (select id from questions where template='Preferences')");
            // each template answer
            foreach (DataRow dr in stats_dt.Rows)
            {
                int position = 0;
                if (Convert.ToInt32(call_length) > 0 & dr["option_pos"].ToString() != "")
                    position = System.Convert.ToInt32(dr["option_pos"]) / Convert.ToInt32(call_length) * 100;
                div_response += " <div class='warning-part' style='left: " + position + "%;' title='" + dr["option_value"].ToString() + "'><span></span><a onclick='jumpPos(" + dr["option_pos"] + ");' class='listen-from-here' style='cursor: pointer;background: none repeat scroll 0% 0% rgb(102,178,255);'>&#x2724</a></div>" + Strings.Chr(13);
            }
            stats_dt = Common.GetTable("select comment_pos, comment_header from system_comments where comment_id = " + form_id);
            // each template answer
            foreach (DataRow dr in stats_dt.Rows)
            {
                int position = 0;

                if (Convert.ToInt32(call_length) > 0 & dr["comment_pos"].ToString() != "")
                {
                    string[] comment_times = dr["comment_pos"].ToString().Split(':');
                    position = System.Convert.ToInt32(Convert.ToInt32(comment_times[0]) * 60 + Convert.ToInt32(comment_times[1]) / Convert.ToInt32(call_length) * 100);
                    div_response += "<div class='warning-part' style='left: " + position + "%;' title='" + dr["comment_header"].ToString() + "'><span></span><a onclick='jumpPos(" + (Convert.ToInt32(comment_times[0]) * 60 + Convert.ToInt32(comment_times[1])) + ");' class='listen-from-here' style='cursor: pointer;background: none repeat scroll 0% 0% rgb(102,178,255);'>&#x2724</a></div>" + Strings.Chr(13);
                }
            }
            return div_response;
        }

        /// <summary>
        /// GetFormQuestions
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetFormQuestions(string form_id)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable q_list = Common.GetTable("select q_short_name, question_id from form_q_scores join questions on questions.id = form_q_scores.question_ID join sections on sections.id = questions.section where form_id = " + form_id + " order by section_order, q_order");
            foreach (DataRow dr in q_list.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["q_short_name"].ToString();
                _mi.value = dr["question_id"].ToString();

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetMyScorecards
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetMyScorecards()
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable q_list = Common.GetTable("select short_name, scorecards.id from scorecards join userapps on userapps.user_scorecard = scorecards.id where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' and active = 1 order by short_name");
            foreach (DataRow dr in q_list.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["short_name"].ToString();
                _mi.value = dr["id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetMyQuestions
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetMyQuestions(string start_date, string end_date)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable q_list = Common.GetTable("exec getMyQuestions '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "', '" + start_date + "', '" + end_date + "'");
            foreach (DataRow dr in q_list.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["q_display"].ToString();
                _mi.value = dr["id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetGroups
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetGroups(string start_date, string end_date, string scorecard)
        {

            string my_group = "";
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable stats_dt;
            if (scorecard == "0" | scorecard == "")
                scorecard = " in (select user_scorecard from userapps  where username =  '" + HttpContext.Current.User.Identity.Name.Replace("'", "''").Replace("'", "''") + "') ";
            else
                scorecard = " in (" + scorecard + ") ";

            DataTable sup_test = Common.GetTable("select user_group,user_role from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''").Replace("'", "''") + "'");
            if (sup_test.Rows.Count > 0)
            {
                if (sup_test.Rows[0]["user_role"].ToString() == "supervisor" & sup_test.Rows[0]["user_group"].ToString() != "")
                {
                    scorecard = scorecard + " and agent_group = '" + sup_test.Rows[0]["user_group"].ToString() + "' ";
                    my_group = sup_test.Rows[0]["user_group"].ToString();
                }
            }

            if (HttpContext.Current.User.IsInRole("Agent"))
                scorecard += " and agent ='" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' ";
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            stats_dt = Common.GetTable("SELECT distinct agent_group FROM [XCC_REPORT_NEW]  where scorecard " + scorecard + " and call_date between '" + start_date + "' and '" + end_date + "'  " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + " order by agent_group");
            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["agent_group"].ToString();
                _mi.value = dr["agent_group"].ToString();

                if (my_group == dr["agent_group"].ToString())
                    _mi.selected = "selected";
                else
                    _mi.selected = "";

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// getWhisperCounts
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<WhisperData> getWhisperCounts()
        {
            List<WhisperData> mi_items = new List<WhisperData>();
            DataTable user_dt = Common.GetTable("exec getWhisperCounts '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            foreach (DataRow dr in user_dt.Rows)
            {
                WhisperData _mi = new WhisperData();
                _mi.QA = "<a href='view_strikes.aspx?reviewer=" + dr["QA"].ToString() + "' target=_blank>" + dr["QA"].ToString() + "</a>";
                _mi.Strikes = dr["Strikes"].ToString();
                _mi.LastReviewDate = dr["LastReviewDate"].ToString();

                mi_items.Add(_mi);
            }
            user_dt = Common.GetTable("exec getWhisperCountsC '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            if (user_dt.Rows.Count > 0)
            {
                WhisperData _mi_int = new WhisperData();
                _mi_int.QA = "<hr>";
                _mi_int.Strikes = "<hr>";
                _mi_int.LastReviewDate = "<hr>";
                mi_items.Add(_mi_int);
            }
            foreach (DataRow dr in user_dt.Rows)
            {
                WhisperData _mi = new WhisperData();
                _mi.QA = "<a href='view_strikes.aspx?Calibrator=" + dr["Calibrator"].ToString() + "' target=_blank>" + dr["Calibrator"].ToString() + "</a>";
                _mi.Strikes = dr["Strikes"].ToString();
                _mi.LastReviewDate = dr["LastReviewDate"].ToString();

                mi_items.Add(_mi);
            }
            return mi_items;
        }



        /// <summary>
        /// GetAgents
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetAgents(string start_date, string end_date, string scorecard, string group)
        {
            DataTable sup_test = Common.GetTable("select user_group,user_role from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            if (sup_test.Rows.Count > 0)
            {
                if (sup_test.Rows[0]["user_role"].ToString() == "supervisor" & sup_test.Rows[0]["user_group"].ToString() != "")
                    group = sup_test.Rows[0]["user_group"].ToString();
            }
            List<DBOptions> mi_items = new List<DBOptions>();

            string filter = "";
            if (group.IndexOf("'") == -1)
                group = "'" + group + "'";
            if (group != "'NOGROUP'")
                filter += " and agent_group in (" + group + ") ";

            if (scorecard != "0" & scorecard != "")
                filter += " and scorecard in (" + scorecard + ") ";

            if (HttpContext.Current.User.IsInRole("Agent"))
                filter += " and agent ='" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' ";
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            DataTable stats_dt;
            stats_dt = Common.GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  where scorecard in (select user_scorecard from userapps where username =  '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + filter + " and agent is not null and agent != '' and max_reviews > -1 and call_date between '" + start_date + "' and '" + end_date +
                "'  " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + "  order by AGent");

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["AGent"].ToString();
                _mi.value = dr["AGent"].ToString();

                if (HttpContext.Current.Session["Agent"].ToString() == dr["Agent"].ToString())
                    _mi.selected = "selected";
                else
                    _mi.selected = "";

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetAppnames
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetAppnames(string filter)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            filter = Strings.Replace(filter, "undefined", "");
            string app_filter = "";
            if (HttpContext.Current.Session["agent_appname"] != null)
                app_filter = HttpContext.Current.Session["agent_appname"].ToString();

            DataTable stats_dt;
            stats_dt = Common.GetTable("exec getMyAppnames '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + filter.Replace("'", "''") + "','" + app_filter + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["scorecard"].ToString();
                _mi.value = dr["ID"].ToString();

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetSCStats
        /// </summary>
        /// <param name="scorecard"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetSCStats(string scorecard, string start_date, string end_date)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable stats_dt;
            stats_dt = Common.GetTable("exec GetSCStats " + scorecard + ",'" + start_date + "','" + end_date + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["comment"].ToString();
                _mi.value = dr["num_objections"].ToString();

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetOtherDataKeys
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <param name="filterarray"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetOtherDataKeys(string date_start, string date_end, string filterarray = "")
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable stats_dt;

            SqlCommand sq = new SqlCommand("getOtherDataKeys");
            sq.CommandType = CommandType.StoredProcedure;
            sq.Parameters.AddWithValue("username", HttpContext.Current.User.Identity.Name);
            sq.Parameters.AddWithValue("date_start", date_start);
            sq.Parameters.AddWithValue("filterarray", filterarray);
            sq.Parameters.AddWithValue("date_end", date_end);
            stats_dt = Common.GetTable(sq);

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["data_key"].ToString();
                _mi.value = dr["data_key"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetOtherDataValues
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <param name="data_key"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetOtherDataValues(string date_start, string date_end, string data_key)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable stats_dt;
            stats_dt = Common.GetTable("select distinct data_value from otherformdata where xcc_id in (select id from xcc_report_new where scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and date_added between '" + date_start + "' and '" + date_end + "') and data_key = '" + data_key + "' and isnull(data_value,'') <> ''   order by data_value");

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["data_value"].ToString();
                _mi.value = dr["data_value"].ToString();
                mi_items.Add(_mi);
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
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetCampaigns(string start_date, string end_date, string scorecard, string group)
        {
            List<DBOptions> mi_items = new List<DBOptions>();

            string filter = "";

            if (group.IndexOf("'") == -1)
                group = "'" + group + "'";

            if (group != "'NOGROUP'")
                filter += " and agent_group in (" + group + ") ";

            if (scorecard != "0" & scorecard != "")
                filter += " and scorecard in (" + scorecard + ") ";

            if (HttpContext.Current.User.IsInRole("Agent"))
                filter += " and agent ='" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' ";

            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            DataTable stats_dt;
            stats_dt = Common.GetTable("SELECT distinct Campaign FROM [XCC_REPORT_NEW]  where  scorecard in (select user_scorecard from userapps where username =  '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + filter + " and call_date between '" + start_date + "' and '" + end_date + "' " + Strings.Replace(Strings.Replace(user_dt.Rows[0]["special_filter"].ToString(), "''", "'"), "vwform", "xcc_report_new") + " and campaign is not null  order by campaign");

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["Campaign"].ToString();
                _mi.value = dr["Campaign"].ToString();

                if (HttpContext.Current.Session["Campaign"].ToString() == dr["Campaign"].ToString())
                    _mi.selected = "selected";
                else
                    _mi.selected = "";

                mi_items.Add(_mi);
            }
            return mi_items;
        }



        /// <summary>
        /// GetDetailCount
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetDetailCount(string start_date, string end_date, string hdnAgentFilter = "", string filter_array = "")
        {

            if (hdnAgentFilter == null)
                hdnAgentFilter = "";

            if (filter_array == null)
                filter_array = "";
            DataTable dt;

            dt = Common.GetTable("getDetailDataCount '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + filter_array.Replace("'", "''") + "'");

            return dt.Rows[0][0].ToString();
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
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetDetails(string start_date, string end_date, string hdnAgentFilter, string pagenum = "1", string pagerows = "50", string Sort_statement = "", string rowstart = "0", string rowend = "0", string filter_array = "")
        {
            GridView gvQADetails = new GridView();
            gvQADetails.AutoGenerateColumns = false;

            gvQADetails.RowCreated += GridView1_RowCreated;
            gvQADetails.RowDataBound += gvQADetails_RowDataBound;

            string called_sp = "getDetailData";

            if (filter_array != "")
                called_sp = "getDetailDataArray";

            addField(ref gvQADetails, "CALL ID");
            DataTable user_col_count_dt = Common.GetTable("select * from available_columns join user_columns on user_columns.column_id = available_columns.id where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' order by col_order");
            if (user_col_count_dt.Rows.Count == 0)
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
                    addField(ref gvQADetails, "Efficiency");
            }
            else
            {
                foreach (DataRow dr in user_col_count_dt.Rows)
                {
                    if (dr["Column_required"].ToString() == "False" | dr["Column_required"].ToString() == "")
                        addField(ref gvQADetails, dr["Column_name"].ToString().Replace("[", "").Replace("]", ""));
                }

                called_sp = "getDetailDataCustom";
            }

            gvQADetails.UseAccessibleHeader = false;
            string this_user = HttpContext.Current.User.Identity.Name;
            if (Sort_statement == "undefined" | Strings.Trim(Sort_statement) == "order by [] desc")
                Sort_statement = "";

            string myRole = "";
            string[] user_roles = Roles.GetRolesForUser(this_user);
            foreach (var role in user_roles)
                myRole = role;

            if (hdnAgentFilter.IndexOf("and vwform.agent =") > -1 & HttpContext.Current.User.Identity.Name.ToLower() == "agent")
            {

                this_user = "agent";
                myRole = "Agent";
            }
            DataTable dt;
            if (Convert.ToInt32(pagenum) == 0 | Convert.ToInt32(pagenum) == -1)
            {
                if (Convert.ToInt32(pagenum) == 0)
                    dt = Common.GetTable(called_sp + " '" + this_user.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user.Replace("'", "''") + "','" + myRole + "','1','1','" + Sort_statement + "','" + rowstart + "','" + rowend + "','" + filter_array.Replace("'", "''") + "'");
                else
                    dt = Common.GetTable(called_sp + " '" + this_user.Replace("'", "''") + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user.Replace("'", "''") + "','" + myRole + "','" + 1 + "','10000','" + Sort_statement + "','" + rowstart + "','" + rowend + "','" + filter_array.Replace("'", "''") + "'");
            }
            else if (filter_array != "")
                dt = Common.GetTable(called_sp + " '" + this_user + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user + "','" + myRole + "','" + pagenum + "','" + pagerows + "','" + Sort_statement + "','" + rowstart + "','" + rowend + "','" + filter_array.Replace("'", "''") + "'");
            else
                dt = Common.GetTable(called_sp + " '" + this_user + "','" + start_date + "','" + end_date + "','" + Strings.Replace(hdnAgentFilter, "'", "''") + "','" + this_user + "','" + myRole + "','" + pagenum + "','" + pagerows + "','" + Sort_statement + "','" + rowstart + "','" + rowend + "'");

            gvQADetails.DataSource = dt;
            gvQADetails.DataBind();

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (Convert.ToInt32(pagenum) == 0 | Convert.ToInt32(pagenum) == -1)
            {
                try
                {
                    gvQADetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gvQADetails.HeaderRow.RenderControl(hw);
                }
                catch (Exception ex)
                {
                }

                if (Convert.ToInt32(pagenum) == -1)
                {
                    foreach (GridViewRow gvr in gvQADetails.Rows)
                        gvr.RenderControl(hw);
                }
            }
            else
                foreach (GridViewRow gvr in gvQADetails.Rows)
                    gvr.RenderControl(hw);
            return sw.ToString();
        }

        /// <summary>
        /// GridView1_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e) // Handles gvQADetails.RowCreated
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
                int cell_count = 0;
                for (var x = 0; x <= e.Row.Cells.Count - 1; x++)
                {
                    if (e.Row.Cells[x].Text == "COMMENTS")
                        comment_header = x;
                    if (e.Row.Cells[x].Text == "MISSED ITEMS")
                        missed_list_header = x;

                    if (e.Row.Cells[x].Text == "CALL ID")
                        call_id_header = x;

                    if (e.Row.Cells[x].Text == "RESULT")
                        call_result_header = x;
                }

                cols_with_data = new int[e.Row.Cells.Count + 1];
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (var x = 0; x <= e.Row.Cells.Count - 1; x++)
                {
                    if (e.Row.Cells[x].Text != "&nbsp;")
                        cols_with_data[x] = cols_with_data[x] + 1;
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
                    e.Row.Cells[call_id_header].Attributes.Add("data-text", sort_class);
                try
                {
                    if (dr["Result"].ToString() == "Pass")
                        e.Row.Cells[call_result_header].Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>";

                    if (dr["Result"].ToString() == "N/A")
                        e.Row.Cells[call_result_header].Text = "<span class='final-result' title='" + dr["bad_call_reason"].ToString().Replace("'", "").Replace("\"", "") + "' style='color:darkgray'>N/A &nbsp;&nbsp;<i class='fa fa-question-circle'></i></span>";

                    if (dr["Result"].ToString() == "Fail")
                    {
                        dr[call_result_header] = "<span class='final-result' " + sort_class + ">FAIL <i class='fa fa-times'></i></span>";
                        e.Row.Attributes.Add("class", "fail-row");
                    }
                }
                catch (Exception ex)
                {
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
                                dr[comment_header] += "<i class='fa fa-file comment" + comment_id + "'></i><span style='white-space: normal;'>" + Strings.Trim(comment.Replace("&lt;br&gt;", "<br>")) + "</span>";
                            comment_id += 1;
                        }
                    }
                    else
                        dr[comment_header] = "<i class='fa fa-file comment1'></i><span style='white-space: normal;'>" + dr[comment_header].ToString().Replace("&lt;br&gt;", "<br>").Trim() + "</span>";
                }

                string noti_owned = "1";
                if (dr["non_edit"].ToString() == "1")
                    noti_owned = "0";

                if (dr["NotificationID"].ToString() != "" & dr["Notificationstep"].ToString() != "")
                    dr[comment_header] += " <img class='noti-click yellow-ex-mark noti-click" + dr["OwnedNotification"].ToString() + "' src='img/yellow_exclamation.png' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Open " + dr["Notificationstep"].ToString() + " Notification' data-notiid='" + dr["NotificationID"].ToString() + "' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='" + dr["Notificationstep"].ToString() + "' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";
                else
                {
                    if (HttpContext.Current.User.IsInRole("Client") | HttpContext.Current.User.IsInRole("Supervisor") | HttpContext.Current.User.IsInRole("Admin"))
                        dr[comment_header] += " <img class='noti-click yellow-ex-mark' src='img/yellow-plus.PNG' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Create Notification' data-source='callDetails' data-notiid='0' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='Supervisor' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";


                    try
                    {
                        if (HttpContext.Current.User.IsInRole("Agent") & dr["agent"].ToString() == HttpContext.Current.User.Identity.Name)
                            dr[comment_header] += " <img class='noti-click yellow-ex-mark' src='img/yellow-plus.PNG' alt='Open " + dr["Notificationstep"].ToString() + " Notification' title='Create Notification' data-source='callDetails' data-notiid='0' data-formid='" + dr["Call ID"].ToString() + "' data-notiowned='" + noti_owned + "' data-notistep='Agent' data-phone='" + dr["phone"].ToString() + "' onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"),\"\",\"" + dr["Call ID"].ToString() + "\");'>";
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //DataRowView drv = e.Row.DataItem(;
                if (dr["play_btn_class"].ToString() == "")
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button'><div></div></button></a>";
                else
                    // If HttpContext.Current.User.IsInRole("Agent") Then
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button'  class='cali_class' title='Calibrated Call'><div></div></button></a>";


                if (dr["wasEdited"].ToString() == "True" & dr["play_btn_class"].ToString() == "")
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "' target='_blank'><button type='button' class='edit_class'  title='Edited Call'><div></div></button></a>";

                if (dr["website"].ToString() != "")
                    dr[call_id_header] = "<a href='review/" + dr["call id"].ToString() + "'  target='_blank'><button type='button' class='website_class'  title='Website Call'><div></div></button></a>";
                e.Row.Attributes.Add("class", "playBtn");
                dr[call_id_header].ToString();
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
        /// UpdateUserEmail
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldemail"></param>
        /// <param name="newemail"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateUserEmail(string username, string oldemail, string newemail)
        {
            if (username == "" | newemail == "")
                return;

            string sql;
            sql = "UPDATE UserExtraInfo SET email_address = '" + newemail + "' WHERE username = '" + username + "'";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            SqlCommand query = new SqlCommand(sql, cn);
            query.CommandTimeout = 60;
            query.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
        }

        /// <summary>
        /// UpdateUserActive
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isActive"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateUserActive(string username, string isActive)
        {
            if (username == "" | isActive == "")
                return;

            int isLockedOut = 0;
            string roleFilter = "";
            DataTable dt = Common.GetTable("SELECT user_role FROM UserExtraInfo WHERE username = '" + username + "'");
            if (isActive == "true")
            {
                if (Strings.Trim(dt.Rows[0][0].ToString()) == "Inactive")
                {
                    roleFilter = ", user_role = 'QA' ";
                    string[] userName = Roles.GetRolesForUser(username);
                    if (userName.Count() > 0)
                        Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
                    Roles.AddUserToRole(username, "QA");
                }
                isActive = "1";
            }
            else
            {
                if (Strings.Trim(dt.Rows[0][0].ToString()) == "QA")
                {
                    roleFilter = ", user_role = 'Inactive' ";
                    if (Roles.GetRolesForUser(username).Count() > 0)
                        Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
                    Roles.AddUserToRole(username, "Inactive");
                }
                isActive = "0";
                isLockedOut = 1;
            }

            string sql;
            sql = "UPDATE UserExtraInfo SET active = '" + isActive + "'" + roleFilter + " WHERE username = '" + username + "'";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            SqlCommand query = new SqlCommand(sql, cn);
            query.CommandTimeout = 60;
            query.ExecuteNonQuery();
            sql = "UPDATE [aspnet_Membership] SET islockedout = '" + isLockedOut + "' WHERE userid in (select userID from  [aspnet_Users] where username = '" + username + "')";
            SqlCommand query2 = new SqlCommand(sql, cn);
            query2.CommandTimeout = 60;
            query2.ExecuteNonQuery();

            if (Strings.Trim(dt.Rows[0][0].ToString()) == "QA" & isActive == "0")
            {
                sql = "DELETE FROM UserApps WHERE username = '" + username + "'";
                SqlCommand query3 = new SqlCommand(sql, cn);
                query3.CommandTimeout = 60;
                query3.ExecuteNonQuery();
            }
            cn.Close();
            cn.Dispose();
        }

        /// <summary>
        /// UpdateBadCallNotes
        /// </summary>
        /// <param name="session_id"></param>
        /// <param name="notes"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateBadCallNotes(string session_id, string notes)
        {
            if (session_id == "")
                return;

            string sql;
            sql = "UPDATE xcc_report_new SET Notes = '" + notes + "' WHERE SESSION_ID = '" + session_id + "'";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlCommand query = new SqlCommand(sql, cn);
            query.CommandTimeout = 60;
            query.ExecuteNonQuery();

            cn.Close();
            cn.Dispose();
        }

        /// <summary>
        /// AcceptBadCall
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string AcceptBadCall(string form_id)
        {
            Common.UpdateTable("update xcc_report_new set bad_call_accepted = 1, bad_call_accepted_who = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' where id = " + form_id);
            return "Updated.";
        }

        /// <summary>
        /// ResetBadCall
        /// </summary>
        /// <param name="form_id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string ResetBadCall(string form_id)
        {

            Common.UpdateTable("exec resetcall " + form_id);
            return "Updated.";
        }

        /// <summary>
        /// getMyMissingCalls
        /// </summary>
        /// <param name="short_name"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getMyMissingCalls(string short_name)
        {
            string sql = "select *, xcc_report_new.id as x_id from xcc_report_new  join scorecards on scorecards.id = scorecard join userapps on userapps.appname= xcc_report_new.appname where audio_link is null and short_name = '" + short_name + "' and username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'";
            DataTable calls_left = Common.GetTable(sql);
            string json = "[";
            foreach (DataRow dr in calls_left.Rows)
                json += "{\"id\":\"" + dr["x_id"] + "\",\"call_date\":\"" + dr["call_date"] + "\",\"phone\":\"" + dr["phone"] + "\",\"agent_group\":\"" + dr["agent_group"] + "\",\"agent\":\"" + dr["agent"] + "\"},";

            if (Strings.Right(json, 1) == ",")
                json = Strings.Left(json, Strings.Len(json) - 1);

            json += "]";

            return json;
        }

        /// <summary>
        /// deleteMissingCalls
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string deleteMissingCalls(string id)
        {

            Common.UpdateTable("delete from xcc_report_new where id  =" + id);
            return "";
        }


        /// <summary>
        /// getMyBadCalls
        /// </summary>
        /// <param name="short_name"></param>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getMyBadCalls(string short_name, string date_start, string date_end)
        {
            string sql = "select *, xcc_report_new.id as x_id from xcc_report_new  join scorecards on scorecards.id = xcc_report_new.scorecard join userapps on userapps.user_scorecard= xcc_report_new.scorecard where bad_call is not null and bad_call_date between '" + date_start + "' and dateadd(d, 1, '" + date_end + "') and scorecards.short_name = '" + short_name + "' and bad_call_accepted is null and username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'";

            List<bad_call> bcs = new List<bad_call>();

            DataTable calls_left = Common.GetTable(sql);

            foreach (DataRow dr in calls_left.Rows)
            {
                bad_call bc = new bad_call();
                bc.audio_link = dr["audio_link"].ToString();
                bc.phone = dr["phone"].ToString();
                bc.id = Convert.ToInt32(dr["id"]);
                bc.call_date = dr["call_date"].ToString();
                bc.bad_call_reason = dr["bad_call_reason"].ToString();
                bcs.Add(bc);
            }
            return new JavaScriptSerializer().Serialize(bcs);
        }

        /// <summary>
        /// getWebsiteStats
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getWebsiteStats(string date_start, string date_end)
        {
            DataTable dt = Common.GetTable("exec getWebsiteStats '" + date_start + "','" + date_end + "', '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            string resp = "<tr style='font-weight:bold'><td colspan=2>General Stats</td></tr>";
            if (dt.Rows.Count > 0)
            {
                resp += "<tr><td>Compliant</td><td>" + dt.Rows[0]["pass_percent"] + "%</td></tr>";
                resp += "<tr><td>Non-compliant</td><td>" + dt.Rows[0]["failed_percent"] + "%</td></tr>";
                resp += "<tr><td>Bad</td><td>" + dt.Rows[0]["bad_percent"] + "%</td></tr>";
            }

            DataTable user_special_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            if (user_special_dt.Rows[0]["special_filter"].ToString() != "")
                dt = Common.GetTable("select count(*) as total, sum(max_reviews) as completed, sum(case when pass_fail = 'Pass' then 1 else 0 end) as passed, sum(case when max_reviews = 0 then 1 else 0 end) as pending, campaign, Agent  from vwform  where scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') " + user_special_dt.Rows[0]["special_filter"].ToString() + " and date_added between '" + date_start + "' and '" + date_end + "' and website is not null group by campaign, agent");
            else
                dt = Common.GetTable("select count(*) as total, sum(max_reviews) as completed, sum(case when pass_fail = 'Pass' then 1 else 0 end) as passed, sum(case when max_reviews = 0 then 1 else 0 end) as pending, campaign, Agent  from vwform  where scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and date_added between '" + date_start + "' and '" + date_end + "' and website is not null group by campaign, agent");


            if (dt.Rows.Count > 0)
                resp += "<tr style='font-weight:bold'><td>Partner</td><td>Campaign</td><td>Total</td><td>Done</td><td>Compliant</td><td>Pend</td></tr>";
            foreach (DataRow dr in dt.Rows)
                resp += "<tr><td>" + dr["agent"].ToString() + "</td><td>" + dr["Campaign"].ToString() + "</td><td>" + dr["total"].ToString() + "</td><td>" + dr["Completed"].ToString() + "</td><td>" + dr["passed"].ToString() + "</td><td>" + dr["pending"].ToString() + "</td></tr>";

            return resp;
        }

        /// <summary>
        /// AddSpotcheck
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <param name="assigned_to"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string AddSpotcheck(string date_start, string date_end, string filter, string count, string assigned_to)
        {
            string sql_range = "";
            if (count != "0")
                sql_range = " top " + count + " ";

            if (assigned_to == "" | assigned_to == "Me")
                assigned_to = HttpContext.Current.User.Identity.Name.Replace("'", "''");
            Common.UpdateTable("insert into spotcheck (f_id, assigned_to) select " + sql_range + " f_id, '" + assigned_to + "' from vwform where call_date between '" + date_start + "' and '" + date_end + "' " + filter + " and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and f_id not in (select f_id from spotcheck) order by f_id desc");

            return "Done.";
        }

        /// <summary>
        /// GetCallsLeft
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_end"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetCallsLeft(string date_start, string date_end)
        {
            string sql = "exec getCallList '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + date_start + "','" + date_end + "'";

            DataTable calls_left = Common.GetTable(sql);
            string json = "<tr style='font-weight:bold; text-align: center;'><td>Scorecard</td><td>Ready</td><td>No Audio</td><td>Bad Calls</td></tr>";
            foreach (DataRow dr in calls_left.Rows)
            {
                string bgcolor = "";
                if (dr["scorecard_role"].ToString() == "Trainee")
                    bgcolor = "style='color:lightgray;'";
                if (dr["train_status"].ToString() == "TL Pending")
                    bgcolor = "style='color:red;'";

                json += "<tr " + bgcolor + "><td>" + dr["short_name"] + "</td><td>" + dr["call_with_audio"] + "</td><td><span style='cursor:pointer' onclick='pop_missing_calls(\"" + dr["short_name"] + "\");'>" + (Convert.ToInt32( dr["calls"]) - Convert.ToInt32(dr["call_with_audio"])) + "</span></td><td><span style='cursor:pointer' onclick='pop_bad_calls(\"" + dr["short_name"] + "\");'>" + dr["bad_calls"] + "</span></td></tr>";
            }

            json += "<tr><td colspan=5><hr></td></tr>";
            json += "<tr style='font-weight:bold; text-align: center;'><td>Call Date</td><td title='Completed/Loaded'>Calls</td><td>Bad</td><td>Scorecard</td></tr>";
            DataTable call_date_dt = Common.GetTable("select count(*) as number_calls, sum(case when max_reviews = 1 then 1 else 0 end) as completed_calls, sum(bad_call) as bad_calls, format(convert(date, call_date),'M/d/yy') as call_date, short_name  from xcc_report_new join scorecards on scorecards.id = xcc_report_new.scorecard where call_date between '" + date_start + "' and '" + date_end + "' and xcc_report_new.scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and bad_call_accepted is null  group by convert(date, call_date), short_name order by convert(date, call_date) desc, short_name");
            foreach (DataRow dr in call_date_dt.Rows)

                json += "<tr><td>" + dr["call_date"] + "</td><td>" + dr["completed_calls"] + "/" + dr["number_calls"] + "</td><td><span style='cursor:pointer' onclick='pop_bad_calls(\"" + dr["short_name"] + "\");'>" + dr["bad_calls"] + "</span></td><td>" + dr["short_name"] + "</td></tr>";

            return json;
        }

        /// <summary>
        /// GetQAApps
        /// </summary>
        /// <param name="username"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQAApps(string username, string from)
        {
            if (username == "")
                return "";

            DataTable apps = new DataTable();
            if (from == "ddlQAs")
                apps = Common.GetTable("SELECT * FROM Userapps a JOIN UserExtraInfo e ON a.username = e.username WHERE a.username = '" + username + "' ORDER BY user_priority");
            else
                apps = Common.GetTable("select userapps.appname + ' (' + short_name + ')' as scorecard, scorecards.id from userapps join userapps on userapps.user_scorecard= xcc_report_new.scorecard WHERE username = '" + username + "' order by userapps.appname");
            string json = "[";
            foreach (DataRow dr in apps.Rows)
            {
                if (from == "ddlQAs")
                    json += "{\"username\":\"" + dr["username"] + "\",\"role\":\"" + dr["user_role"] + "\",\"appname\":\"" + dr["appname"] + "\",\"user_priority\":\"" + dr["user_priority"] + "\",\"max_per_day\":\"" + dr["max_per_day"] + "\"},";
                else
                    json += "{\"appname\":\"" + dr["appname"] + "\"},";
            }

            if (Strings.Right(json, 1) == ",")
                json = Strings.Left(json, Strings.Len(json) - 1);

            json += "]";
            return json;
        }

        /// <summary>
        /// DeleteUserApps
        /// </summary>
        /// <param name="username"></param>
        /// <param name="appname"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void DeleteUserApps(string username, string appname)
        {
            if (username == "" | appname == "")
                return;

            string sql;
            sql = "DELETE FROM UserApps WHERE username = '" + username + "' AND appname = '" + appname + "'";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            SqlCommand query = new SqlCommand(sql, cn);
            query.CommandTimeout = 60;
            query.ExecuteNonQuery();

            cn.Close();
            cn.Dispose();
        }

        /// <summary>
        /// AddUserApps
        /// </summary>
        /// <param name="username"></param>
        /// <param name="appname"></param>
        /// <param name="quota"></param>
        /// <param name="role"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void AddUserApps(string username, string appname, string quota, string role)
        {
            if (username == "" | appname == "")
                return;

            string sql;
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            if (role == "")
                return;
            else
            {
                if (Roles.GetRolesForUser(username).Count() > 0)
                    Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
                Roles.AddUserToRole(username, role);
                sql = "UPDATE UserExtraInfo SET user_role='" + role + "' WHERE username='" + username + "'";
                SqlCommand query2 = new SqlCommand(sql, cn);
                query2.CommandTimeout = 60;
                query2.ExecuteNonQuery();
            }
            if (quota == "")
                quota = "null";
            else
                quota = "'" + quota + "'";
            sql = "INSERT INTO UserApps (username,appname,dateadded,who_added,user_priority,trained_date,max_per_day)  VALUES ('" + username + "','" + appname + "',dbo.getMTDate(),'" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "',null,null," + quota + ")";
            SqlCommand query = new SqlCommand(sql, cn);
            query.CommandTimeout = 60;
            query.ExecuteNonQuery();

            cn.Close();
            cn.Dispose();
        }

        /// <summary>
        /// UpdateUserAppPriority
        /// </summary>
        /// <param name="username"></param>
        /// <param name="appset"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateUserAppPriority(string username, string appset)
        {
            if (username == "" | appset == "")
                return;
            string sql;
            string[] apps = appset.Split(',');
            string[] app;
            foreach (string item in apps)
            {
                app = item.Split('=');
                sql = "UPDATE UserApps SET user_priority = " + app[1] + "WHERE username = '" + username + "' AND appname = '" + app[0] + "'";
                Common.UpdateTable(sql);
            }
        }

        /// <summary>
        /// UpdateReport
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string UpdateReport()
        {
            DataTable apps = Common.GetTable("SELECT DISTINCT * FROM Userapps a JOIN UserExtraInfo e ON a.username = e.username WHERE user_role = 'QA' OR user_role = 'Trainee' AND active = 1 ORDER BY a.username, appname");

            string json = "[";

            foreach (DataRow dr in apps.Rows)
                json += "{\"username\":\"" + dr["username"] + "\",\"role\":\"" + dr["user_role"] + "\",\"appname\":\"" + dr["appname"] + "\",\"user_priority\":\"" + dr["user_priority"] + "\",\"max_per_day\":\"" + dr["max_per_day"] + "\"},";

            if (Strings.Right(json, 1) == ",")
                json = Strings.Left(json, Strings.Len(json) - 1);
            json += "]";
            return json;
        }

        /// <summary>
        /// SaveCoachingLog
        /// </summary>
        /// <param name="scorecard"></param>
        /// <param name="QA"></param>
        /// <param name="TL"></param>
        /// <param name="Reason"></param>
        /// <param name="ActionID"></param>
        /// <param name="DiscussionPoints"></param>
        /// <param name="ID"></param>
        /// <param name="form_id"></param>
        /// <param name="Owner"></param>
        /// <param name="Action"></param>
        /// <param name="Timeline"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void SaveCoachingLog(string scorecard, string QA, string TL, string Reason, string ActionID, string DiscussionPoints, string ID, string form_id, string Owner, string Action, string Timeline)
        {
            if (QA == "" | TL == "" | ActionID == "")
                return;

            string sql;
            if (ID == "" | ID == "0" | ID == "undefined")
            {
                sql = "INSERT INTO CoachingLog SELECT '" + QA + "', '" + TL + "', dbo.getMTDate(), dbo.getMTDate(), '" + Reason.Replace("'", "''") + "', " + ActionID + ", '" + DiscussionPoints.Replace("'", "''") + "', null, null, null, null, '" + scorecard + "';";
                sql += "declare @newID int; select @newID = @@Identity; ";

                sql += "INSERT INTO ActionPlans SELECT @newID, '" + Owner + "', '" + Action.Replace("'", "''") + "', '" + Timeline.Replace("'", "''") + "'";
            }
            else
                sql = "UPDATE CoachingLog SET QA = '" + QA + "',scorecard = '" + scorecard + "', TL = '" + TL + "', ModifiedDate = dbo.getMTDate(), Reason = '" + Reason.Replace("'", "''") + "', ActionID = " + ActionID + ", DiscussionPoints = '" + DiscussionPoints.Replace("'", "''") + "' WHERE ID = " + ID;

            Common.UpdateTable(sql);
        }

        /// <summary>
        /// UpdateCoachingReport
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<CoachingReport> UpdateCoachingReport(string user, string role)
        {
            if (user == "" & role == "")
                return null;

            DataTable apps;
            if (role == "QA")
                apps = Common.GetTable("SELECT ISNULL(first_name + ISNULL(' ' + last_name,''),u.username) AS fullname, c.ID as ID, * FROM CoachingLog c JOIN UserExtraInfo u ON c.QA = u.username left join scorecards on scorecards.id = c.scorecard  WHERE QA = '" + user + "' OR first_name + ' ' + last_name = '" + user + "' ORDER BY CoachingDate DESC");
            else if (role == "QA Lead" | role == "Calibrator")
                apps = Common.GetTable("SELECT ISNULL(first_name + ISNULL(' ' + last_name,''),u.username) AS fullname, c.ID as ID, * FROM CoachingLog c JOIN UserExtraInfo u ON c.QA = u.username left join scorecards on scorecards.id = c.scorecard  WHERE TL = '" + user + "' OR first_name + ' ' + last_name = '" + user + "' ORDER BY CoachingDate DESC");
            else
                apps = Common.GetTable("SELECT ISNULL(first_name + ISNULL(' ' + last_name,''),u.username) AS fullname, c.ID as ID, * FROM CoachingLog c JOIN UserExtraInfo u ON c.QA = u.username left join scorecards on scorecards.id = c.scorecard  ORDER BY CoachingDate");

            List<CoachingReport> sr_items = new List<CoachingReport>();
            foreach (DataRow dr in apps.Rows)
            {
                CoachingReport report = new CoachingReport();
                report.fullname = dr["fullname"].ToString();
                report.ID = dr["ID"].ToString();
                report.TL = dr["TL"].ToString();
                report.Scorecard = dr["short_name"].ToString();
                report.CoachingDate = dr["CoachingDate"].ToString().Split(' ')[0];
                report.QASigned = dr["QASigned"].ToString().Split(' ')[0];
                report.TLSigned = dr["TLSigned"].ToString().Split(' ')[0];
                report.ManagerSigned = dr["ManagerSigned"].ToString().Split(' ')[0];
                sr_items.Add(report);
            }

            return sr_items;
        }



        /// <summary>
        /// GetCoachingLog
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<CoachingReport> GetCoachingLog(int ID)
        {
            if (ID < 0)
                return null;

            DataTable log = Common.GetTable("SELECT ISNULL(first_name + ISNULL(' ' + last_name,''),u.username) AS fullname,* FROM CoachingLog c JOIN UserExtraInfo u ON c.QA = u.username WHERE c.id = '" + ID + "'");

            List<CoachingReport> sr_items = new List<CoachingReport>();
            foreach (DataRow dr in log.Rows)
            {
                CoachingReport report = new CoachingReport();
                report.fullname = dr["fullname"].ToString();
                report.TL = dr["TL"].ToString();
                report.CoachingDate = dr["CoachingDate"].ToString().Split(' ')[0];
                report.Reason = dr["Reason"].ToString();
                report.ActionID = dr["ActionID"].ToString();
                report.DiscussionPoints = dr["DiscussionPoints"].ToString();
                report.QAResponse = dr["QAResponse"].ToString();
                report.Scorecard = dr["Scorecard"].ToString();
                sr_items.Add(report);
            }
            return sr_items;
        }

        /// <summary>
        /// AddActionPlans
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="Action"></param>
        /// <param name="Timeline"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<ActionPlans> AddActionPlans(string Owner, string Action, string Timeline, int ID)
        {
            if (ID < 1)
                return null;

            if (Owner.Length > 0 & Action.Length > 0 & Timeline.Length > 0)
            {
                string sql = "INSERT INTO ActionPlans SELECT " + ID + ", '" + Owner + "', '" + Action.Replace("'", "''") + "', '" + Timeline.Replace("'", "''") + "'";

                Common.UpdateTable(sql);
            }

            DataTable log = Common.GetTable("SELECT * FROM ActionPlans WHERE CoachingLogID = " + ID);
            List<ActionPlans> sr_items = new List<ActionPlans>();
            foreach (DataRow dr in log.Rows)
            {
                ActionPlans plan = new ActionPlans();
                plan.ID = dr["ID"].ToString();
                plan.Owner = dr["Owner"].ToString();
                plan.Action = dr["Action"].ToString();
                plan.Timeline = dr["Timeline"].ToString().Split(' ')[0];
                sr_items.Add(plan);
            }
            return sr_items;
        }



        /// <summary>
        /// DeleteActionPlan
        /// </summary>
        /// <param name="ID"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void DeleteActionPlan(string ID)
        {
            if (ID == "")
                return;

            string sql;
            sql = "DELETE FROM ActionPlans WHERE ID = '" + ID + "'";
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// UpdateSign
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Role"></param>
        /// <param name="QAResponse"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateSign(int ID, string Role, string QAResponse)
        {
            if (ID < 0 | Role == "")
                return;

            string sql;
            if (Role == "QA")
                sql = "UPDATE CoachingLog SET QASigned = dbo.getMTDate(), QAResponse = '" + QAResponse.Replace("'", "''") + "', ModifiedDate = dbo.getMTDate() WHERE ID = " + ID;
            else if (Role == "QA Lead" | Role == "Calibrator")
                sql = "UPDATE CoachingLog SET TLSigned = dbo.getMTDate(), ModifiedDate = dbo.getMTDate() WHERE ID = " + ID;
            else if (Role == "Calibrator")
                sql = "UPDATE CoachingLog SET TLSigned = dbo.getMTDate(), ModifiedDate = dbo.getMTDate() WHERE ID = " + ID;
            else
                sql = "UPDATE CoachingLog SET ManagerSigned = dbo.getMTDate(), ModifiedDate = dbo.getMTDate() WHERE ID = " + ID;
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// ResetTLSign
        /// </summary>
        /// <param name="id"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void ResetTLSign(string id)
        {
            if (id == "")
                return;

            string sql = "UPDATE CoachingLog SET TLSigned = null WHERE id = " + id;
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// GetQAData
        /// </summary>
        /// <param name="QA"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQAData(string QA)
        {
            DataTable data = Common.GetTable("SELECT startdate, starting_salary FROM UserExtraInfo WHERE username = '" + QA + "'");

            string json = "[{\"start_date\":\"" + data.Rows[0][0] + "\",\"start_salary\":\"" + data.Rows[0][1] + "\"}]";
            return json;
        }

        /// <summary>
        /// UpdateQAStart
        /// </summary>
        /// <param name="username"></param>
        /// <param name="start_date"></param>
        /// <param name="start_salary"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateQAStart(string username, string start_date, string start_salary)
        {
            if (start_date == "" & start_salary == "")
                return;
            if (start_date == "")
                start_date = "null";
            else
                start_date = "'" + start_date + "'";
            if (start_salary == "")
                start_salary = "0";
            string sql = "UPDATE UserExtraInfo SET startdate = " + start_date + ", starting_salary = '" + start_salary + "' WHERE username = '" + username + "'";
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// ClientUpdate
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<ClientUpdateData> ClientUpdate(string username)
        {
            DataTable messaging = Common.GetTable("getMyMessages '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            List<ClientUpdateData> list = new List<ClientUpdateData>();

            foreach (DataRow dr in messaging.Rows)
            {
                ClientUpdateData data = new ClientUpdateData();
                data.id = dr["id"].ToString();
                data.dateadded = dr["dateadded"].ToString();
                data.dateaddedtime = dr["dateadded"].ToString();
                data.dateclosed = dr["dateclosed"].ToString();
                data.from_login = dr["from_login"].ToString();
                data.subject = dr["subject"].ToString();
                data.message_text = dr["message_text"].ToString();
                list.Add(data);
            }

            return list;
        }

        /// <summary>
        /// ClientUpdateAck
        /// </summary>
        /// <param name="id"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void ClientUpdateAck(string id)
        {
            if (id == "")
                return;

            string sql = "UPDATE messaging SET dateclosed = dbo.getMTDate() WHERE id = " + id;
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// GetWorstDeviation
        /// </summary>
        /// <param name="username"></param>
        /// <param name="appname"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="agent_group"></param>
        /// <param name="agent"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<WorstDeviation> GetWorstDeviation(string username, string appname, string start_date, string end_date, string agent_group, string agent, string campaign)
        {
            DataTable table = Common.GetTable("getTop50Deviations '" + username + "','" + appname + "','" + start_date + "','" + end_date + "','" + agent_group + "','" + agent + "','" + campaign + "'");

            List<WorstDeviation> list = new List<WorstDeviation>();
            foreach (DataRow dr in table.Rows)
            {
                WorstDeviation data = new WorstDeviation();
                data.total_dev = dr["total_dev"].ToString();
                data.form_id = dr["form_id"].ToString();
                data.appname = dr["appname"].ToString();
                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// UpdateFormScore3
        /// </summary>
        /// <param name="username"></param>
        /// <param name="formid"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateFormScore3(string username, string formid)
        {
            if (username == "" & formid == "")
                return;
            string sql = "UPDATE form_score3 SET deviation_reviewed = dbo.getMTDate(), deviation_reviewed_by = '" + username + "' WHERE id = " + formid;
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// GetClientCalibrations
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetClientCalibrations()
        {
            string ret_val = "";

            DataTable wk_summary = Common.GetTable("getClientWeeklySummary '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            if (wk_summary.Rows.Count > 0)
            {
                ret_val = "<hr><table width='100%' class='detailsTable'><thead><tr><td>Week</td><td># Cals</td><td>Cal Done</td><td># Reviews</td></tr></thead><tbody>";
                foreach (DataRow dr in wk_summary.Rows)
                {
                    string bk_color = "white";
                    ret_val += "<tr>";
                    ret_val += "<td>" + dr["week_ending"] + "</td>";
                    ret_val += "<td><a target=_blank href='ClientCalibrationReport.aspx?week_ending=" + dr["week_ending"] + "'>" + dr["number_assigned"] + "</a></td><td>" + dr["number_completed"] + "</td><td>" + dr["number_reviews_completed"] + "</td></tr>";
                }
                ret_val += "</tbody></table>";
            }
            ret_val += "<hr>";
            DataTable client_list = Common.GetTable("exec [getClientCalibrations] '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            if (client_list.Rows.Count > 0)
            {
                ret_val += "<table width='100%' class='detailsTable'><thead><tr><td>Scorecard</td><td>Date Added</td><td>Reviews</td><td></td></tr></thead><tbody>";
                foreach (DataRow dr in client_list.Rows)
                {
                    string bk_color = "white";

                    ret_val += "<tr>";
                    ret_val += "<td>" + dr["short_name"];
                    if (dr["phone"].ToString() != "")
                        ret_val += "<br>(" + dr["phone"] + ")";
                    ret_val += "</td>";
                    ret_val += "<td>" + Convert.ToDateTime(dr["dateadded"]).ToShortDateString() + "</td>";
                    ret_val += "<td>" + dr["real_num_completed"] + "/" + dr["real_num_reviews"] + "</td><td><a href='review/" + dr["form_id"].ToString() + "' target=_blank><img src='/img/small_play.PNG' height='15' style='position: relative; top: 2px;'></a></td></tr>";
                }
                ret_val += "</tbody></table>";
            }

            return ret_val;
        }

        /// <summary>
        /// GetUserAppsByWeekEnding
        /// </summary>
        /// <param name="user"></param>
        /// <param name="wedate"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetUserAppsByWeekEnding(string user, string wedate)
        {
            DataTable apps = Common.GetTable("SELECT DISTINCT scorecard, short_name FROM vwForm join scorecards on scorecards.id = vwForm.scorecard WHERE week_ending_date = '" + wedate + "' and reviewer = '" + user + "' ORDER BY short_name");

            string json = "[";

            foreach (DataRow dr in apps.Rows)
                json += "{\"ID\":\"" + dr["scorecard"] + "\",\"scorecard\":\"" + dr["short_name"] + "\"},";

            if (Strings.Right(json, 1) == ",")
                json = Strings.Left(json, Strings.Len(json) - 1);

            json += "]";
            return json;
        }

        /// <summary>
        /// getCoachingQueue
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getCoachingQueue(string filter)
        {
            DataTable user_info = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");

            if (user_info.Rows.Count > 0)
            {
                if (user_info.Rows[0]["user_group"].ToString() != "")
                    filter += " and AGENT_GROUP = '" + user_info.Rows[0]["user_group"].ToString() + "' ";
            }
            string sql = "exec getCoachingQueue '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + filter.Replace("'", "''") + " '";

            DataTable client_list = Common.GetTable(sql);
            string ret_val = "";
            foreach (DataRow dr in client_list.Rows)
            {
                string[] comments = dr["AllComments"].ToString().Split('|');
                int comment_id = 1;
                string all_comments = "";
                foreach (var comment in comments)
                {
                    if (Strings.Trim(comment) != "")
                        all_comments += "<i class='fa fa-file comment" + comment_id + "'></i><span style='white-space:normal'>" + Strings.Trim(comment.Replace("&lt;br&gt;", "<br>")) + "</span>";
                    comment_id += 1;
                }
                string noti_owned = "1";
                if (dr["non_edit"].ToString() != "")
                    noti_owned = "0";
                string splat_color = "yellow";
                if (dr["notificationStep"].ToString() == "Manager")
                    splat_color = "green";

                ret_val += "<tr>";
                ret_val += "<td>" + dr["agent"] + "</td><td>" + dr["call_date"] + "</td><td class='popup-comments'>" + all_comments + "</td><td>";
                ret_val += "<img src='img/" + splat_color + "_exclamation.png' class='noti-click' title='Open " + dr["notificationStep"] + " Notification' data-notiowned='" + noti_owned + "'";
                ret_val += "data-notiid='" + dr["ID"] + "' data-notiso='" + dr["sup_override"].ToString() + "'  data-notistep='" + dr["notificationStep"] + "' data-phone='" + dr["phone"] + "' data-formid='" + dr["form_id"] + "'";
                ret_val += "onclick='pop_notification($(this).attr(\"data-notiid\"),$(this).attr(\"data-notistep\"), \"" + dr["first_error"] + "\",\"" + dr["form_id"] + "\",\"" + dr["sup_override"].ToString() + "\");'>";
                ret_val += "</td><td valign='middle'>";
                ret_val += "<a href='review/" + dr["form_id"] + "' target='_blank'>";
                ret_val += "<img src='/img/small_play.PNG' height='15' style='position: relative; top: 2px;'>";
                ret_val += "</a>";
                ret_val += "</td>";
                ret_val += "</tr>";
            }

            return ret_val;
        }

        /// <summary>
        /// getCoachingEscQueue
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getCoachingEscQueue()
        {
            string sql = "exec getCoachingEscQueue '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'";
            DataTable client_list = Common.GetTable(sql);

            string ret_val = "";
            if (HttpContext.Current.User.IsInRole("Admin") | HttpContext.Current.User.IsInRole("QA Lead"))
            {
                foreach (DataRow dr in client_list.Rows)
                {
                    string[] comments = dr["AllComments"].ToString().Split('|');
                    int comment_id = 1;
                    string all_comments = "";
                    foreach (var comment in comments)
                    {
                        if (Strings.Trim(comment) != "")
                            all_comments += "<i class='fa fa-file comment" + comment_id + "'></i><span style='white-space:normal'>" + Strings.Trim(comment.Replace("&lt;br&gt;", "<br>")) + "</span>";
                        comment_id += 1;
                    }
                    ret_val += "<tr>";
                    ret_val += "<td>" + dr["agent"] + "</td><td>" + dr["call_date"] + "</td><td class='popup-comments'>" + all_comments + "</td><td>";
                    ret_val += "<a href='manage_notification.aspx?ID=" + dr["form_id"] + "' target='_blank'><img src='img/green_exclamation.png' class='noti-click'></a>";
                    ret_val += "</td><td valign='middle'>";
                    ret_val += "<a href='review/" + dr["form_id"] + "' target='_blank'>";
                    ret_val += "<img src='/img/small_play.PNG' height='15' style='position: relative; top: 2px;'>";
                    ret_val += "</a>";
                    ret_val += "</td>";
                    ret_val += "</tr>";
                }
            }

            return ret_val + "<tr><td colspan=10><hr></td></tr>";
        }

        /// <summary>
        /// GetCampaignPerf
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetCampaignPerf(string start_date, string end_date, string filter_array = "")
        {
            SqlCommand sql = new SqlCommand("getCampaignPerf");
            sql.CommandType = CommandType.StoredProcedure;

            sql.Parameters.AddWithValue("start_date", start_date);
            sql.Parameters.AddWithValue("end_date", end_date);
            sql.Parameters.AddWithValue("username", HttpContext.Current.User.Identity.Name.Replace("'", "''"));
            sql.Parameters.AddWithValue("filterarray", filter_array);

            DataTable client_list = Common.GetTable(sql);
            string ret_val = "";
            foreach (DataRow dr in client_list.Rows)
            {
                ret_val += "<tr>";
                ret_val += "<td>" + dr["avg_score"] + "</td><td>" + dr["num_calls"] + "</td><td>" + dr["campaign"] + "</td>";
                ret_val += "</td>";
                ret_val += "</tr>";
            }
            return ret_val;
        }

        /// <summary>
        /// GetGroupPerf
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetGroupPerf(string start_date, string end_date)
        {
            string sql = "select convert(varchar(10), convert(decimal(10,2), avg(isnull(isnull(edited_score,calib_score),vwForm.total_score)))) + '%' as avg_score, count(*) as num_calls, agent_group  from vwForm  join userapps  on userapps.user_scorecard = vwForm.scorecard where call_date between '" + start_date + "' and '" + end_date + "' and username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' group by agent_group order by avg(isnull(isnull(edited_score,calib_score),vwForm.total_score))";

            DataTable client_list = Common.GetTable(sql);
            string ret_val = "";
            foreach (DataRow dr in client_list.Rows)
            {
                ret_val += "<tr>";
                ret_val += "<td>" + dr["avg_score"] + "</td><td>" + dr["num_calls"] + "</td><td>" + dr["agent_group"] + "</td>";
                ret_val += "</td>";
                ret_val += "</tr>";
            }
            return ret_val;
        }

        /// <summary>
        /// UpdateUserPassword
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newpassword"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateUserPassword(string username, string newpassword)
        {
            if (username == "" | newpassword == "")
                return;

            MembershipUser mu = Membership.GetUser(username);
            mu = Membership.GetUser(username, false);
            if (mu == null)
                return;
            mu.ChangePassword(mu.ResetPassword(), newpassword);
        }

        /// <summary>
        /// UpdateUserMaxPerDay
        /// </summary>
        /// <param name="qa"></param>
        /// <param name="appname"></param>
        /// <param name="maxperday"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateUserMaxPerDay(string qa, string appname, string maxperday)
        {
            if (qa == "" | appname == "")
                return;
            if (maxperday == "")
                maxperday = "null";

            string sql = "UPDATE Userapps SET max_per_day = " + maxperday + " WHERE username = '" + qa + "' AND appname = '" + appname + "'";
            Common.UpdateTable(sql);
        }

        /// <summary>
        /// Escalate
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="escalatedby"></param>
        /// <param name="comments"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void Escalate(string fid, string escalatedby, string comments)
        {
            if (fid == "" | escalatedby == "")
                return;

            string sql = "UPDATE form_notifications SET escalated = dbo.getMTDate(), escalated_by = '" + escalatedby + "', comment = '" + comments.Replace("'", "''") + "' WHERE id = " + fid;

            Common.UpdateTable(sql);
        }

        /// <summary>
        /// getUserInfo
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public UserInfo getUserInfo()
        {
            UserInfo ui = new UserInfo();
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            if (user_dt.Rows.Count > 0)
            {
                DataRow dr = user_dt.Rows[0];
                ui.username = HttpContext.Current.User.Identity.Name;
                ui.SpeedInc = dr["speed_increment"].ToString();
                ui.first_name = dr["first_name"].ToString();
                ui.last_name = dr["last_name"].ToString();
                ui.phone = dr["phone_number"].ToString();
                ui.ImmediatePlay = Convert.ToBoolean(dr["calls_start_immediately"]);
                ui.bypass = Convert.ToBoolean(dr["bypass"]);
                ui.email = dr["email_address"].ToString();
                ui.guideline_display = dr["guideline_display"].ToString();
                ui.presubmit = dr["presubmit"].ToString();
                ui.export_type = dr["export_type"].ToString();
            }

            return ui;
        }

        /// <summary>
        /// updateUserInfo
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string updateUserInfo(string value, string field)
        {
            bool update_result = Convert.ToBoolean("True");
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
                Common.UpdateTable("update userextrainfo set " + field + " = '" + value.Replace("'", "''") + "' where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            return update_result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQACalibScore(string start_date, string end_date) // , hdnAgentFilter As String
        {

            DateTime datTime = new DateTime();
            string month = DateInterval.Month.ToString();
            int one = -1;
            string fullstart_date = month + one + start_date;
            string fullend_date = month + one + end_date;
            string prior_start_date = datTime.AddDays(Convert.ToDouble(fullstart_date)).ToShortDateString();
            string prior_end_date = datTime.AddDays(Convert.ToDouble(fullend_date)).ToShortDateString();

            DataTable avg_dt = Common.GetTable("SELECT ISNULL(ROUND(AVG(total_score),0),0) as avg_score FROM calibration_form WHERE CONVERT(DATE, review_date) BETWEEN '" + start_date + "' AND '" + end_date + "' UNION ALL SELECT ISNULL(ROUND(AVG(total_score),0),0) as avg_score FROM calibration_form WHERE CONVERT(DATE, review_date) BETWEEN '" + prior_start_date + "' AND '" + prior_end_date + "'");

            if (avg_dt.Rows.Count == 2)
                return Convert.ToString(avg_dt.Rows[0][0].ToString() + ":" + Convert.ToString(Convert.ToInt32(avg_dt.Rows[0][0]) - Convert.ToInt32(avg_dt.Rows[1][0])));

            return "";
        }

        /// <summary>
        /// GetQADisputeScore
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQADisputeScore(string start_date, string end_date) // , hdnAgentFilter As String
        {
            DateTime datTime = new DateTime();
            string month = DateInterval.Month.ToString();
            int one = -1;
            string fullstart_date = month + one + start_date;
            string fullend_date = month + one + end_date;
            string prior_start_date = datTime.AddDays(Convert.ToDouble(fullstart_date)).ToShortDateString();
            string prior_end_date = datTime.AddDays(Convert.ToDouble(fullend_date)).ToShortDateString();
            DataTable avg_dt = Common.GetTable("SELECT COUNT(*) FROM form_notifications JOIN form_score3 ON form_score3.id = form_notifications.form_id WHERE close_reason = 'Agree' AND sup_override IS NULL AND role = 'QA' AND CONVERT(DATE, review_date) BETWEEN '" + start_date + "' AND '" + end_date + "' UNION ALL SELECT COUNT(*) FROM form_notifications JOIN form_score3 ON form_score3.id = form_notifications.form_id WHERE close_reason = 'Agree' AND sup_override IS NULL AND role = 'QA' AND CONVERT(DATE, review_date) BETWEEN '" + prior_start_date + "' AND '" + prior_end_date + "'");

            if (avg_dt.Rows.Count == 2)
                return Convert.ToString(avg_dt.Rows[0][0].ToString() + ":" + Convert.ToString(Convert.ToInt32(avg_dt.Rows[0][0]) - Convert.ToInt32(avg_dt.Rows[1][0])));

            return "";
        }

        /// <summary>
        /// GetQAEffScore
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetQAEffScore(string start_date, string end_date) // , hdnAgentFilter As String
        {
            // string prior_start_date = DateTime.DateAdd(DateInterval.Month, -1, (DateTime)start_date).ToShortDateString();
            // string prior_end_date = DateTime.DateAdd(DateInterval.Month, -1, (DateTime)end_date).ToShortDateString();
            DateTime datTime = new DateTime();
            string month = DateInterval.Month.ToString();
            int one = -1;
            string fullstart_date = month + one + start_date;
            string fullend_date = month + one + end_date;
            string prior_start_date = datTime.AddDays(Convert.ToDouble(fullstart_date)).ToShortDateString();
            string prior_end_date = datTime.AddDays(Convert.ToDouble(fullend_date)).ToShortDateString();
            DataTable avg_dt = Common.GetTable("select convert(decimal(10,2),(convert(decimal(10,2),(sum(convert(float,call_length))/3600)/(sum(convert(float,datediff(s, review_started, review_date)))/3600) * 100))) as efficiency from form_score3 join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID where CONVERT(DATE, review_date) BETWEEN '" + start_date + "' AND '" + end_date + "' union all select convert(decimal(10,2),(convert(decimal(10,2),(sum(convert(float,call_length))/3600)/(sum(convert(float,datediff(s, review_started, review_date)))/3600) * 100))) as efficiency from form_score3 join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID where CONVERT(DATE, review_date) BETWEEN '" + prior_start_date + "' AND '" + prior_end_date + "'");

            if (avg_dt.Rows.Count == 2)

                return Convert.ToString(avg_dt.Rows[0][0].ToString() + ":" + Convert.ToString(Convert.ToInt32(avg_dt.Rows[0][0]) - Convert.ToInt32(avg_dt.Rows[1][0])));
            return "";
        }

        /// <summary>
        /// GetQACalibRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QACalibRanking> GetQACalibRanking(string start_date, string end_date)
        {
            List<QACalibRanking> mi_items = new List<QACalibRanking>();

            DataTable stats_dt = Common.GetTable("exec getQACalibRanking2 '" + start_date + "', '" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                QACalibRanking _mi = new QACalibRanking();
                _mi.avg_score = dr["calib_score"].ToString();
                _mi.qa = dr["reviewer"].ToString();
                _mi.div_color = "";
                _mi.top3 = "";

                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetQADisputeRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QADisputeRanking> GetQADisputeRanking(string start_date, string end_date)
        {
            List<QADisputeRanking> mi_items = new List<QADisputeRanking>();

            DataTable stats_dt = Common.GetTable("SELECT reviewer, COUNT(*) AS dispute FROM form_notifications JOIN form_score3 ON form_score3.id = form_notifications.form_id WHERE close_reason = 'Agree' AND sup_override IS NULL AND role = 'QA' AND CONVERT(DATE, review_date) BETWEEN '" + start_date + "' AND '" + end_date + "' GROUP BY reviewer ORDER BY dispute DESC, reviewer");
            foreach (DataRow dr in stats_dt.Rows)
            {
                QADisputeRanking _mi = new QADisputeRanking();
                _mi.dispute = dr["dispute"].ToString();
                _mi.qa = dr["reviewer"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }


        /// <summary>
        /// GetQAEffRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QAEffRanking> GetQAEffRanking(string start_date, string end_date)
        {
            List<QAEffRanking> mi_items = new List<QAEffRanking>();
            DataTable stats_dt = Common.GetTable("select reviewer, convert(decimal(10,2),(convert(decimal(10,2),(sum(convert(float,call_length))/3600)/(sum(convert(float,datediff(s, review_started, review_date)))/3600) * 100))) as efficiency from form_score3 join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID where CONVERT(DATE, review_date) BETWEEN '" + start_date + "' AND '" + end_date + "' GROUP BY reviewer ORDER BY efficiency");
            foreach (DataRow dr in stats_dt.Rows)
            {
                QAEffRanking _mi = new QAEffRanking();
                _mi.efficiency = dr["efficiency"].ToString();
                _mi.qa = dr["reviewer"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }


        /// <summary>
        /// GetTLCalibRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<TLCalibRanking> GetTLCalibRanking(string start_date, string end_date)
        {
            List<TLCalibRanking> mi_items = new List<TLCalibRanking>();
            DataTable stats_dt = Common.GetTable("SELECT reviewed_by, ROUND(AVG(c.total_score),0) AS avg_score FROM calibration_form c JOIN form_score3 f ON f.id = c.original_form WHERE CONVERT(DATE, f.review_date) between '" + start_date + "' and '" + end_date + "' GROUP BY reviewed_by ORDER BY avg_score");

            foreach (DataRow dr in stats_dt.Rows)
            {
                TLCalibRanking _mi = new TLCalibRanking();
                _mi.avg_score = dr[("avg_score")].ToString();
                _mi.tl = dr["reviewed_by"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }

        /// <summary>
        /// GetTLNotifRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<TLNotifRanking> GetTLNotifRanking(string start_date, string end_date)
        {
            List<TLNotifRanking> mi_items = new List<TLNotifRanking>();
            DataTable stats_dt = Common.GetTable("exec getNotifBySC '" + start_date + "','" + end_date + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                TLNotifRanking _mi = new TLNotifRanking();
                _mi.notif = dr["totalopen"].ToString();
                _mi.appname = dr["appname"].ToString();
                _mi.ag = dr["open_agent"].ToString();
                _mi.su = dr["open_supervisor"].ToString();
                _mi.qa = dr["open_QA"].ToString();
                _mi.tl = dr["open_tl"].ToString();

                mi_items.Add(_mi);
            }

            return mi_items;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="hdnAgentFilter"></param>
        /// <param name="filter_array"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QAStats> GetQAStats(string start_date, string end_date, string hdnAgentFilter, string filter_array = "")
        {
            List<QAStats> mi_items = new List<QAStats>();
            DataTable stats_dt = Common.GetTable("exec [getQAStats] '" + start_date + "','" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "', '" + hdnAgentFilter.Replace("'", "''") + "', '" + filter_array.Replace("'", "''") + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                QAStats _mi = new QAStats();
                _mi.result = dr["result"].ToString();
                _mi.label = dr["label"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }



        /// <summary>
        /// GetCLCalibRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<CLCalibRanking> GetCLCalibRanking(string start_date, string end_date)
        {
            List<CLCalibRanking> mi_items = new List<CLCalibRanking>();

            DataTable stats_dt = Common.GetTable("getCLCalibRanking '" + start_date + "', '" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow dr in stats_dt.Rows)
            {
                CLCalibRanking _mi = new CLCalibRanking();
                _mi.calib = dr["cal_score"].ToString();
                _mi.cl = dr["appname"].ToString();
                _mi.num_calib = dr["num_calib"].ToString();
                _mi.total = dr["total"].ToString();
                _mi.percent_completed = dr["percent_completed"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }


        /// <summary>
        /// GetCLDisputeRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<CLDisputeRanking> GetCLDisputeRanking(string start_date, string end_date)
        {
            List<CLDisputeRanking> mi_items = new List<CLDisputeRanking>();
            DataTable stats_dt = Common.GetTable("select count(*) as disputes, appname from vwFN where date_closed between '" + start_date + "' and '" + end_date + "' and close_reason = 'Agree' and role = 'QA' group by appname order by disputes desc, appname");
            foreach (DataRow dr in stats_dt.Rows)
            {
                CLDisputeRanking _mi = new CLDisputeRanking();
                _mi.disputes = dr["disputes"].ToString();
                _mi.cl = dr["appname"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }


        /// <summary>
        /// GetCLEffRanking
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<CLEffRanking> GetCLEffRanking(string start_date, string end_date)
        {
            List<CLEffRanking> mi_items = new List<CLEffRanking>();

            DataTable stats_dt = Common.GetTable("select convert(decimal(10,2),(convert(decimal(10,2),(sum(convert(float,call_length))/3600)/(sum(convert(float,datediff(s, review_started, review_date)))/3600) * 100))) as eff, appname from vwForm where call_length > 0 and review_date between '" + start_date + "' and '" + end_date + "' group by appname order by eff, appname");

            foreach (DataRow dr in stats_dt.Rows)
            {
                CLEffRanking _mi = new CLEffRanking();
                _mi.eff = dr["eff"].ToString();
                _mi.cl = dr["appname"].ToString();

                mi_items.Add(_mi);
            }

            return mi_items;
        }

        /// <summary>
        /// GetQnA
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QnA> GetQnA(string aid)
        {
            if (aid == "")
                return null;
            List<QnA> QnAs = new List<QnA>();

            DataTable dt = Common.GetTable("SELECT q.Question, a.Answer FROM ApplicantsQnA a INNER JOIN ApplicantsQ q ON a.QuestionID = q.ID WHERE ApplicantID = " + aid);
            foreach (DataRow dr in dt.Rows)
            {
                QnA QnA = new QnA();
                QnA.q = dr["Question"].ToString();
                QnA.a = dr["Answer"].ToString();
                QnAs.Add(QnA);
            }

            return QnAs;
        }

        /// <summary>
        /// UpdateScorecard
        /// </summary>
        /// <param name="scorecard"></param>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public void UpdateScorecard(string scorecard)
        {
            Common.UpdateTable("delete from sc_update where reviewer = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "' and sc_id = '" + scorecard + "'");
            Common.UpdateTable("insert into sc_update (reviewer, sc_id, date_reviewed) select '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "','" + scorecard + "', dbo.getMTDate()");
        }

        /// <summary>
        /// GetQAScores
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<QAScores> GetQAScores(string start_date, string end_date)
        {
            if (start_date == "" | end_date == "")
                return null;
            List<QAScores> list = new List<QAScores>();
            DataTable dt = Common.GetTable("getQAScores '" + start_date + "', '" + end_date + "','" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "'");
            foreach (DataRow dr in dt.Rows)
            {
                QAScores result = new QAScores();
                result.today = dr["today"].ToString();
                result.lastmonth = dr["lastmonth"].ToString();
                if (System.Convert.ToInt32(result.today) == System.Convert.ToInt32(result.lastmonth))
                    result.direction = "";
                else if (System.Convert.ToInt32(result.today) > System.Convert.ToInt32(result.lastmonth))
                    result.direction = "positive";
                else
                    result.direction = "negative";

                list.Add(result);
            }

            return list;
        }



        /// <summary>
        /// GetLastLoginDate
        /// </summary>
        /// <param name="qa"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string GetLastLoginDate(string qa)
        {
            DataTable user_dt = Common.GetTable("select * from userextrainfo where username =  '" + qa + "'");

            if (user_dt.Rows.Count > 0)
                return user_dt.Rows[0]["lastactivedate"].ToString();
            else
                return "";
        }

        /// <summary>
        /// ResetLastLoginDate
        /// </summary>
        /// <param name="qa"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string ResetLastLoginDate(string qa)
        {
            Common.UpdateTable("update userextrainfo set lastactivedate = dbo.getMTDate() where username =  '" + qa + "'");

            DataTable user_dt = Common.GetTable("select * from userextrainfo where username =  '" + qa + "'");

            if (user_dt.Rows.Count > 0)
                return user_dt.Rows[0]["lastactivedate"].ToString();
            else
                return "";
        }

        /// <summary>
        /// GetQualityA
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetQualityA(string start_date, string end_date, string scorecard)
        {

            List<DBOptions> mi_items = new List<DBOptions>();

            DataTable stats_dt;
            stats_dt = Common.GetTable("Select distinct reviewer from vwform  where call_date between '" + start_date + "' and '" + end_date + "'  order by reviewer");
            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["reviewer"].ToString();
                _mi.value = dr["reviewer"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetNonBillable
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetNonBillable(string start_date, string end_date, string scorecard)
        {
            List<DBOptions> mi_items = new List<DBOptions>();
            DataTable stats_dt;

            stats_dt = Common.GetTable("exec getNonBilllable " + scorecard);

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["comment"].ToString();
                _mi.value = dr["id"].ToString();
                mi_items.Add(_mi);
            }
            return mi_items;
        }

        /// <summary>
        /// GetManagers
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public List<DBOptions> GetManagers(string start_date, string end_date, string scorecard)
        {
            List<DBOptions> mi_items = new List<DBOptions>();
            DataTable stats_dt;

            if (scorecard != "")
                stats_dt = Common.GetTable("select distinct manager from vwForm join userextrainfo on agent = username where scorecard = '" + scorecard + "'  and call_date between '" + start_date + "' and '" + end_date + "' and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and manager is not null order by manager");
            else
                stats_dt = Common.GetTable("select distinct manager from vwForm join userextrainfo on agent = username where call_date between '" + start_date + "' and '" + end_date + "' and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') and manager is not null order by manager");

            foreach (DataRow dr in stats_dt.Rows)
            {
                DBOptions _mi = new DBOptions();
                _mi.text = dr["manager"].ToString();
                _mi.value = dr["manager"].ToString();
                mi_items.Add(_mi);
            }

            return mi_items;
        }

        /// <summary>
        /// getNotifDetails
        /// </summary>
        /// <param name="detail_type"></param>
        /// <param name="username"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string getNotifDetails(string detail_type, string username, string start_date, string end_date, string filter)
        {
            string sql = "select * from vwFN where ";
            switch (detail_type)
            {
                case "AB":
                    {
                        sql += "date_closed is null and role = 'Agent' and agent = '" + username + "' ";
                        break;
                    }

                case "AA":
                    {
                        sql += "date_closed is not null and role = 'Agent' and agent = '" + username + "'  and close_reason = 'Agree' ";
                        break;
                    }

                case "AD":
                    {
                        sql += "date_closed Is null And role = 'Agent' and agent = '" + username + "' and close_reason = 'Disagree' and sup_override is null  ";
                        break;
                    }

                case "SB":
                    {
                        sql += "date_closed is null and role = 'Supervisor' and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') ";
                        break;
                    }

                case "SA":
                    {
                        sql += "date_closed is not null and role = 'Supervisor' and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "')  and close_reason = 'Agree' ";
                        break;
                    }

                case "SD":
                    {
                        sql += "date_closed is not null and role = 'Supervisor' and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "')  and close_reason = 'Disagree' and sup_override is null  ";
                        break;
                    }

                case "QB":
                    {
                        sql += "date_closed is null and role = 'QA' and reviewer = '" + username + "' ";
                        break;
                    }

                case "QA":
                case "TD":
                    {
                        sql += "date_closed is not null and role = 'QA' and reviewer = '" + username + "'  and close_reason = 'Agree' ";
                        break;
                    }

                case "QD":
                    {
                        sql += "date_closed Is not null And role = 'QA' and reviewer = '" + username + "' and close_reason = 'Disagree' and sup_override is null  ";
                        break;
                    }

                case "LB":
                    {
                        sql += "date_closed is null and role in ('QA Lead','Calibrator') and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "') ";
                        break;
                    }

                case "LA":
                    {
                        sql += "date_closed is not null and role in ('QA Lead','Calibrator') and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "')  and close_reason = 'Agree' ";
                        break;
                    }

                case "LD":
                    {
                        sql += "date_closed is not null and role in ('QA Lead','Calibrator') and scorecard in (select user_scorecard from userapps where username = '" + HttpContext.Current.User.Identity.Name.Replace("'", "''") + "')  and close_reason = 'Disagree' and sup_override is null  ";
                        break;
                    }
            }
            sql = sql + filter;
            sql += " and review_date between '" + start_date + "' and '" + end_date + "' ";

            DataTable noti_dt = Common.GetTable(sql);
            string ret = "<table>";
            foreach (DataRow dr in noti_dt.Rows)
            {
                ret += "<tr><td valign='top'><img src='/img/small_play.PNG' height='15' onclick='window.location.href=\"review/" + dr["f_id"] + "\"';></td>";
                ret += "<td valign='top'>" + dr["call_date"] + "</td>";
                ret += "<td>" + dr["formatted_comments"].ToString().Replace("|", "<br>") + "</td></tr><tr><td colspan=3><hr></td></tr>";
            }
            ret += "</table>";
            return ret;
        }

        /// <summary>
        /// post_callsource
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string post_callsource(DataMapper dm, string username)
        {
            string sql = "declare @new_ID int;insert into xcc_report_new (campaign, agent, agent_name, DISPOSITION, agent_group,  appname, scorecard, call_date, timestamp, audio_link, phone, session_id, review_started)";

            sql += "select '" + dm.industry + "',";
            sql += "'" + dm.employee + "',";
            sql += "'" + dm.employeeID + "',";
            sql += "'" + dm.callstatus + "',";
            sql += "'" + dm.assignment + "',";
            sql += "'CallSource',";
            sql += "'524',";
            sql += "'" + DateTime.Today.AddHours(-6).ToShortDateString() + "',";
            sql += "'" + DateTime.Now.AddHours(-6).ToShortDateString() + "',";
            sql += "'" + dm.audio + "',";
            sql += "'" + dm.ani + "',";
            sql += "'" + dm.index + "',";

            sql += "'" + DateTime.Now.AddHours(-6).ToShortDateString() + "'; select @new_ID = scope_identity(); select @new_ID;";
            DataTable new_id = Common.GetTable(sql);
            if (new_id.Rows.Count > 0)
            {
                string x_id = new_id.Rows[0][0].ToString();
                string fs_sql = "declare @new_ID int;   INSERT INTO[dbo].[form_score3] (reviewer, session_id, review_date,[review_ID], appname) ";
                fs_sql += (Convert.ToString("select '") + username) + "' ,";
                fs_sql += (Convert.ToString("'") + x_id) + "' ,";
                fs_sql += "dbo.GetMTdate(),";
                fs_sql += (Convert.ToString("'") + x_id) + "' ,";
                fs_sql += "'CallSource'; select @new_ID = scope_identity();select @new_ID;";

                DataTable fs_ds = Common.GetTable(fs_sql);
                if (fs_ds.Rows.Count > 0)
                {
                    string f_id = fs_ds.Rows[0][0].ToString();
                    for (int x = 0; x <= dm.results.Count - 1; x++)
                    {
                        string fqs_sql = "insert into form_q_scores (q_position,form_id, question_id, question_answered, original_question_answered) ";
                        fqs_sql += "select 0,";
                        fqs_sql += f_id + Convert.ToString(",");
                        fqs_sql += (x + 11770).ToString() + ",";
                        fqs_sql += "(select id from question_answers where question_id = " + (x + 11770).ToString() + " and answer_text = '" + dm.results[x] + "'),";
                        fqs_sql += "(select id from question_answers where question_id = " + (x + 11770).ToString() + " and answer_text = '" + dm.results[x] + "')";
                        Common.UpdateTable(fqs_sql);
                    }
                }
            }
            return "Posted.";
        }
    }
}
