using DAL.Extensions;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using DAL.Models.ListenModels;

namespace WebApi.Controllers
{
    public class ListenController : ApiController
    {
        /// <summary>
        /// PostListen
        /// </summary>
        /// <param name="LDR"></param>
        /// <returns></returns>
        public string PostListen(ListenDataRequest LDR)
        {
            ListenLayer listenLayer = new ListenLayer();
            return listenLayer.PostListen(LDR);
        }



        [Route("listen/GetAutoComplete")]
        [HttpPost]
        public List<string> GetAutoComplete([FromBody]string question_id, string string_match)
        {
            List<string> ac_data = new List<string>();

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                //getAutoCompleteData
                var sqlComm = new SqlCommand
                {
                    CommandText = "getAutoCompleteData",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@question_id", question_id);
                sqlComm.Parameters.AddWithValue("@string_match", string_match);
                sqlComm.Connection = sqlCon;

                SqlDataAdapter sda = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ac_data.Add(dr.Field<string>(0));
                }

            }

            return ac_data;
        }

        /// <summary>
        ///   Allows files to be uploaded, requires multipart form data post, file + f_id.
        /// </summary>
        /// <returns></returns>
        [Route("listen/uploaduserfile")]
        [HttpPost]
        public async Task<int> Upload()
        {
            string userName = HttpContext.Current.GetUserName();
            if (userName.Length == 0) return -1;

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var posted_filename = "";

            var provider = new MultipartMemoryStreamProvider();
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                if (file.Headers.ContentDisposition.FileName != null)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    posted_filename = filename;
                    var buffer = await file.ReadAsByteArrayAsync();
                    files.Add(filename, buffer);
                }
                else
                {
                    foreach (NameValueHeaderValue p in file.Headers.ContentDisposition.Parameters)
                    {
                        string name = p.Value;
                        if (name.StartsWith("\"") && name.EndsWith("\"")) name = name.Substring(1, name.Length - 2);
                        string value = await file.ReadAsStringAsync();
                        attributes.Add(name, value);
                    }
                }
            }

            if (!attributes.ContainsKey("f_id")) return -1;
            int f_id = 0;
            var desc = "";

            if (!Int32.TryParse(attributes["f_id"], out f_id)) return -1;
            if (attributes.ContainsKey("description"))
                desc = attributes["description"];
            var fileList = new List<UploadUserCallDocuments>();
            var returnid = -1;
            foreach (var file in files)
            {
                var userFile = new UploadUserCallDocuments()
                {
                    fileName = file.Key,
                    f_ID = f_id,
                    description = desc

				};

                if (file.Key.IndexOf(".") > -1)
                    userFile.AddFile(file.Value.ToArray(), file.Key.Substring(file.Key.IndexOf(".")));
                else
                    userFile.AddFile(file.Value.ToArray(), file.Key);

                await userFile.UploadtoAWS();
				returnid = userFile.ID;
			}
			return returnid;
		}

		/// <summary>
		///    Deletes file
		/// </summary>
		/// <param name="userDoc">guid for s3 and ID for record</param>
		/// <returns></returns>
		[Route("listen/deleteuserfile")]
		[HttpPost]
		public IHttpActionResult Delete([FromBody] baseUserCallDocuments userDoc)
		{
			string userName = HttpContext.Current.GetUserName();
			if (userName.Length == 0) return Unauthorized();
			if (userDoc.s3Key.Length == 0) return BadRequest();
			var userFile = new UploadUserCallDocuments() {
				s3Key = userDoc.s3Key,
				ID = userDoc.ID
			};
			userFile.Delete();
			return Ok();
		}

		/// <summary>
		///  Get if app allows document upload and gets available documents for a F_ID
        /// </summary>
        /// <param name="f_id"></param>
        /// <returns></returns>
        [Route("list/getuserfiles")]
        [HttpPost]
        public AllUserCallDocuments GetAllUserDocumentsForCall([FromBody]int f_id)
        {
            var docs = new AllUserCallDocuments();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var cmd = new SqlCommand("GetReviewUserFiles", con);
                cmd.Parameters.AddWithValue("@f_id", f_id);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                docs = AllUserCallDocuments.Create(reader);
            }
            return docs;
        }

        /// <summary>
		///  Search Get Available Agent Names for
		/// </summary>

        /// <param name="reviewId"></param>
		/// <returns></returns>
		[Route("list/GetAvailableAgentName")]
        [HttpPost]
        public List<string> GetAvailableAgentName([FromBody] int reviewId)
        {
            List<string> agentNames = new List<string>();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                var cmd = new SqlCommand("[getAvailableAgentNames1]", con);
                // cmd.Parameters.AddWithValue("@agentName", agentName);
                cmd.Parameters.AddWithValue("@reviewId", reviewId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                List<string> unames = new List<string>();
                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    agentNames.Add(dr[0].ToString());
                }

            }
            return agentNames;
        }

        /// <summary>
        /// API witch used to update agent on selected call at review page
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("list/ChangeRecordAgentName")]
        [HttpPost]
        public dynamic UpdateAgentName([FromBody]SimpleObject obj)
        {

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = @"update vwForm set AGENT = @agent,agent_name = @agent where F_ID = @reviewId";

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = con
                };
                try
                {
                    SqlComm.Parameters.AddWithValue("@agent", obj.value);
                    SqlComm.Parameters.AddWithValue("@reviewId", obj.id);
                    con.Open();
                    SqlComm.ExecuteNonQuery();
                    con.Close();
                    return GetAvailableAgentName(obj.id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }



    }
}
