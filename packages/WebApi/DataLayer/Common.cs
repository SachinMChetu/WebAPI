using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApi.Entities;

namespace WebApi.DataLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class Common
    {
        #region Public Properties
        public static string FFPROBE = @"D:\home\site\wwwroot\ffmpeg\bin\ffprobe.exe";
        public static string FFMPEG = @"D:\home\site\wwwroot\ffmpeg\bin\ffmpeg.exe";
        #endregion
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="rethrow"></param>
        /// <returns></returns>
        public static bool RunSqlCommand(SqlCommand cmd, bool rethrow = true)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            try
            {
                cn.Open();
                cmd.Connection = cn;
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                if (rethrow)
                    throw ex;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return false;
        }

        #region Public GetTable
        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, int debug = 0)
        {
            string sql_start = DateTime.Now.ToString();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
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
                //Email_Error(sql + "<br><br>" + ex.Message);
            }
            cn.Close();
            cn.Dispose();
            return dt;
        }
        #endregion


        #region Public GetAudioFileName
        /// <summary>
        /// GetAudioFileName
        /// </summary>
        /// <param name="drv"></param>
        /// <returns></returns>
        public static string GetAudioFileName(getScorecardData_Result drv)
        {
            // If this scorecard is for website, just return the website

            int Id;

            try
            {
                Id = drv.X_ID;
            }
            catch (Exception ex)
            {
                Id = drv.F_ID;
            }

            //DataTable sc_id = GetTable("select *, (select bypass from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "') as bypass from xcc_report_new  join scorecards  on xcc_report_new.scorecard = scorecards.id join app_settings  on app_settings.appname = xcc_report_new.appname where xcc_report_new.id = " + Id);
            using (CC_ProdEntities dataContext = new CC_ProdEntities())
            {
                var sc_id = dataContext.GetAudioFileName(HttpContext.Current.User.Identity.Name, Id).FirstOrDefault();
                if (sc_id !=null)
                {
                    if (sc_id.bypass.ToString() == "True" & sc_id.onAWS.ToString() == "True")
                    { 
                        return "http://files.callcriteria.com" + sc_id.audio_link;
                    }
                    if (sc_id.onAWS.ToString() == "True")
                    {
                        string audio_link = sc_id.audio_link;
                        IAmazonS3 s3Client;
                        s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);

                        GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();
                        GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                        URL_REQ.BucketName = "callcriteriasingapore" + Strings.Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                        URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                        URL_REQ.Expires = DateTime.Now.AddHours(1);
                        return s3Client.GetPreSignedURL(URL_REQ);
                    }

                    if (sc_id.review_type == "website")
                    { 
                        return "/point1sec.mp3";
                    }
                    if (sc_id.stream_only.ToString() == "True")
                    {
                        return sc_id.audio_link;
                    }
                }

                if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
                {
                    //try
                    //{
                    //    TimeSpan call_len = new TimeSpan(0, 0, Convert.ToInt32(GetMediaDuration("http://files.callcriteria.com" + drv.audio_link)) / 2);   // tlf.Properties.Duration
                    //    DateTime call_time = Convert.ToDateTime("12/30/1899") + call_len;
                    //    UpdateTable("update XCC_REPORT_NEW set call_time = '" + call_time.ToString() + "' where ID = (select review_id from form_score3  where ID = " + Id + ")");
                    //    UpdateTable("update form_score3 set call_length = '" + call_len.TotalSeconds + "' where ID = " + Id);
                    //    return "http://files.callcriteria.com" + drv.audio_link.ToString(); // already converted, send it back
                    //}
                    //catch (Exception ex)
                    //{
                    //    return "http://files.callcriteria.com" + drv.audio_link.ToString();
                    //}
                }

                // thinks it's downloaded, but file's not there, get the orignal file and download again
                if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
                {
                    // Email_Error("trying to reload " & drv.Item("audio_link").ToString)
                    // See if session ID has data
                    //wav_dt = GetTable("select * from wav_data  where session_id = '" + drv.SESSION_ID + "'");
                    var wav_dt = dataContext.WAV_DATA.Where(x => x.session_id == drv.SESSION_ID).FirstOrDefault();
                    if (wav_dt == null)
                    {
                        //wav_dt = GetTable("select * from wav_data  where filename like '%" + drv.SESSION_ID + "%'");
                    }
                    if (wav_dt !=null)
                    {
                        drv.audio_link = wav_dt.filename;
                    }
                }

                string session_id;
                if (drv.SESSION_ID.ToString().IndexOf(" ") > 0)
                {
                    session_id = Strings.Left(drv.SESSION_ID.ToString(), drv.SESSION_ID.ToString().IndexOf(" "));
                }
                else
                { 
                    session_id = drv.SESSION_ID;
                }

                string phone = drv.phone.ToString();
                string this_filename = drv.audio_link.ToString();

                string call_date = drv.call_date.ToString().Substring(0, drv.call_date.ToString().IndexOf(" ")).Replace("/", "_");
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/")))
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/"));
                if (drv.audio_link.ToString() != "")
                {
                    if (Strings.Left(drv.audio_link.ToString(), 4) == "http" | Strings.Left(drv.audio_link.ToString(), 6) == "/audio" | Strings.Left(drv.audio_link.ToString(), 3) == "ftp")
                        this_filename = drv.audio_link.ToString().Replace(" ", "%20");
                    else
                        this_filename = drv.url_prefix + (drv.audio_link.ToString().Replace(" ", "%20"));


                    string file_ending = Strings.Right(this_filename, 4).ToLower();


                    if ((this_filename.IndexOf("@") > -1 | this_filename.IndexOf("http") > -1) & Strings.Left(this_filename, 3) != "ftp" & Strings.Left(this_filename, 4) != "sftp" & Strings.Left(this_filename, 6) != "/audio")
                    {
                        // Email_Error(this_filename)
                        System.Net.WebClient WebClient_down = new System.Net.WebClient();

                        WebClient_down.Credentials = new System.Net.NetworkCredential(drv.recording_user.ToString(), drv.record_password.ToString(), "");

                        try
                        {
                            WebClient_down.DownloadFile(this_filename, HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending));
                        }
                        catch (Exception ex)
                        {
                            HttpContext.Current.Response.Write("File not found, refresh page." + this_filename + " to " + HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                            //Email_Error(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                            HttpContext.Current.Response.Redirect("listen.aspx");
                        }


                        while (!!WebClient_down.IsBusy)
                            System.Threading.Thread.Sleep(100);
                    }

                    if (Strings.Left(this_filename, 6) == "ftp://")
                    {
                        WebClient ftpc = new WebClient();
                        ftpc.Credentials = new System.Net.NetworkCredential(drv.audio_user.ToString(), drv.audio_password.ToString(), "");

                        try
                        {
                            System.IO.File.Delete(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                        }
                        catch (Exception ex)
                        {
                        }

                        try
                        {
                            ftpc.DownloadFile(this_filename, @"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                            System.Threading.Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending + " " + ex.Message + "<br><br><br>");
                            HttpContext.Current.Response.End();
                        }

                        while (!!ftpc.IsBusy)
                            System.Threading.Thread.Sleep(100);
                    }

                    if (Strings.Left(this_filename, 7) == "sftp://")
                    {
                        try
                        {
                            Renci.SshNet.SftpClient sftp_new = new Renci.SshNet.SftpClient(drv.recording_user.ToString().Substring(7), Convert.ToInt32(drv.audio_link.ToString()), drv.audio_user.ToString(), drv.audio_password.ToString());
                            sftp_new.Connect();

                            System.IO.FileStream dest_file = new System.IO.FileStream(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending, FileMode.OpenOrCreate);

                            sftp_new.DownloadFile(this_filename.Substring(Strings.Len(drv.recording_user)).Replace("%20", " "), dest_file);
                            sftp_new.Disconnect();
                            sftp_new.Dispose();
                            dest_file.Close();
                            dest_file.Dispose();
                        }
                        catch (Exception ex)
                        {
                            HttpContext.Current.Response.Write(ex.Message + "<br>");
                            HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending + "<br>");
                            HttpContext.Current.Response.Write(this_filename.Substring(Strings.Len(drv.recording_user)).Replace("%20", " "));
                            HttpContext.Current.Response.End();
                        }
                        int max_wait = 0;
                        while (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending)) | max_wait == 100)
                        {
                            System.Threading.Thread.Sleep(100);
                            max_wait += 1;
                        }
                    }
                    if (file_ending == ".mp3" | file_ending == ".mp4")
                        this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending);
                    else
                    {
                        try
                        {
                            this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending);
                        }
                        catch (Exception ex)
                        {
                            return "";
                        }
                        string output = "";
                        string out_error = "";
                        string destination_file = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + ".mp3");
                        //RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
                        try
                        {
                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending));
                        }
                        catch (Exception ex)
                        {
                        }
                        file_ending = ".mp3";
                    }
                    this_filename = "/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending;
                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(this_filename)))
                    {
                        //UpdateTable("update XCC_REPORT_NEW set audio_link = '" + this_filename + "' where ID = " + Id);
                        var isExist = dataContext.XCC_REPORT_NEW.Where(x => x.ID == Id).FirstOrDefault();
                        XCC_REPORT_NEW tblXCC_REPORT_NEW = new XCC_REPORT_NEW();
                        if (isExist != null)
                        {
                            tblXCC_REPORT_NEW = dataContext.XCC_REPORT_NEW.Find(Id);
                            dataContext.Entry(tblXCC_REPORT_NEW).State = EntityState.Modified;
                            tblXCC_REPORT_NEW.bad_call_accepted = System.DateTime.Now;
                            tblXCC_REPORT_NEW.audio_link = this_filename;
                           int result = dataContext.SaveChanges();
                        }
                        return this_filename;
                    }
                    else
                        return drv.audio_link.ToString();// already an mp3, file exists though doesn't match session ID
                }
                return "";
            }

           
        }
        #endregion


    }
}