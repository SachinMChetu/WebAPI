using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Configuration;
using System.Data;
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
                Email_Error(sql + "<br><br>" + ex.Message);
            }
            cn.Close();
            cn.Dispose();
            return dt;
        }
        #endregion

        #region Public UpdateTable
        /// <summary>
        /// UpdateTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        public static void UpdateTable(string sql, int debug = 0)
        {
            string sql_start = DateTime.Now.ToString();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            try
            {
                SqlCommand reply = new SqlCommand(sql, cn);
                reply.CommandTimeout = 60;
                reply.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("<!--" + sql + "-->" + Strings.Chr(13));
                Email_Error(sql + "<br><br>" + ex.Message);
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion


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
                withBlock.FileName = FFMPEG;
                withBlock.Arguments = arg;

                // start the process in a hidden window
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
            while (!myProcess.WaitForExit(1500) & num_tries < 20);
            return 0;
        }
        #endregion

        #region Public CheckAddBinPath
        /// <summary>
        /// CheckAddBinPath
        /// </summary>
        public static void CheckAddBinPath()
        {
            // find path to 'bin' folder
            var binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "bin" });
            // get current search path from environment
            var path__1 = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path__1.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path__1 = string.Join(Path.PathSeparator.ToString(), new string[] { path__1, binPath });
                Environment.SetEnvironmentVariable("PATH", path__1);
            }
        }
        #endregion

        #region Public ConvertWavToMp3
        /// <summary>
        /// ConvertWavToMp3
        /// </summary>
        /// <param name="wavFile"></param>
        /// <returns></returns>
        public static MemoryStream ConvertWavToMp3(NAudio.Wave.Wave32To16Stream wavFile)
        {
            CheckAddBinPath();

            using (var retMs = new MemoryStream())
            {
                using (var wtr = new NAudio.Lame.LameMP3FileWriter(retMs, wavFile.WaveFormat, 128))
                {
                    wavFile.CopyTo(wtr);
                    return retMs;
                }
            }
        }
        #endregion

        #region Public RunFFPROBE
        /// <summary>
        /// RunFFPROBE
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="OutResult"></param>
        /// <param name="ErrResult"></param>
        /// <returns></returns>
        public static int RunFFPROBE(string arg, ref string OutResult, ref string ErrResult)
        {
            Process myProcess = new Process();
            {
                var withBlock = myProcess.StartInfo;
                withBlock.FileName = FFPROBE;
                withBlock.Arguments = arg;

                // start the process in a hidden window
                withBlock.WindowStyle = ProcessWindowStyle.Hidden;
                withBlock.CreateNoWindow = true;
                withBlock.RedirectStandardOutput = true;
                withBlock.RedirectStandardError = true;
                withBlock.UseShellExecute = false;
            }
            myProcess.Start();
            // Attach to stdout and stderr.
            StreamReader std_out = myProcess.StandardOutput;
            StreamReader std_err = myProcess.StandardError;

            myProcess.WaitForExit(15000);
            // Return results
            OutResult = std_out.ReadToEnd();
            ErrResult = std_err.ReadToEnd();

            // Clean up.
            std_out.Close();
            std_err.Close();

            // In case process is stuck
            if (!myProcess.HasExited)
                myProcess.Kill();
            else
                myProcess.Close();
            return 0;
        }
        #endregion

        #region Public GetMediaDuration
        /// <summary>
        /// GetMediaDuration
        /// </summary>
        /// <param name="MediaFilename"></param>
        /// <returns></returns>
        public static double GetMediaDuration(string MediaFilename)
        {
            string output = "";
            string out_error = "";

            RunFFPROBE("-i " + Strings.Chr(34) + MediaFilename + Strings.Chr(34), ref output, ref out_error);
            int duration_loc = out_error.IndexOf("Duration:");
            string[] new_time = out_error.Substring(duration_loc + 11, 7).Split(':');
            try
            {
                TimeSpan ts = new TimeSpan(Convert.ToInt32(new_time[0]), Convert.ToInt32(new_time[1]), Convert.ToInt32(new_time[2]));
                return ts.TotalSeconds;
            }
            catch (Exception ex)
            {
                return 1;
            }
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

            int theID;

            try
            {
                theID = drv.X_ID;
            }
            catch (Exception ex)
            {
                theID = drv.F_ID;
            }

            DataTable sc_id = GetTable("select *, (select bypass from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "') as bypass from xcc_report_new  join scorecards  on xcc_report_new.scorecard = scorecards.id join app_settings  on app_settings.appname = xcc_report_new.appname where xcc_report_new.id = " + theID);
            if (sc_id.Rows.Count > 0)
            {
                if (sc_id.Rows[0]["bypass"].ToString() == "True" & sc_id.Rows[0]["onAWS"].ToString() == "True")

                    return "http://files.callcriteria.com" + sc_id.Rows[0]["audio_link"].ToString();

                if (sc_id.Rows[0]["onAWS"].ToString() == "True")
                {
                    string audio_link = sc_id.Rows[0]["audio_link"].ToString();
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);

                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();
                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = "callcriteriasingapore" + Strings.Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }

                if (sc_id.Rows[0]["review_type"].ToString() == "website")
                    return "/point1sec.mp3";

                if (sc_id.Rows[0]["stream_only"].ToString() == "True")
                    return sc_id.Rows[0]["audio_link"].ToString();
            }

            if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
            {
                try
                {
                    TimeSpan call_len = new TimeSpan(0, 0, Convert.ToInt32(GetMediaDuration("http://files.callcriteria.com" + drv.audio_link)) / 2);   // tlf.Properties.Duration
                    DateTime call_time = Convert.ToDateTime("12/30/1899") + call_len;
                    UpdateTable("update XCC_REPORT_NEW set call_time = '" + call_time.ToString() + "' where ID = (select review_id from form_score3  where ID = " + theID + ")");
                    UpdateTable("update form_score3 set call_length = '" + call_len.TotalSeconds + "' where ID = " + theID);
                    return "http://files.callcriteria.com" + drv.audio_link.ToString(); // already converted, send it back
                }
                catch (Exception ex)
                {
                    return "http://files.callcriteria.com" + drv.audio_link.ToString();
                }
            }

            // thinks it's downloaded, but file's not there, get the orignal file and download again
            if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
            {
                // Email_Error("trying to reload " & drv.Item("audio_link").ToString)
                // See if session ID has data
                DataTable wav_dt;
                wav_dt = GetTable("select * from wav_data  where session_id = '" + drv.SESSION_ID + "'");
                if (wav_dt.Rows.Count == 0)
                    wav_dt = GetTable("select * from wav_data  where filename like '%" + drv.SESSION_ID + "%'");

                if (wav_dt.Rows.Count > 0)
                    drv.audio_link = wav_dt.Rows[0]["filename"].ToString();
            }


            string session_id;

            if (drv.SESSION_ID.ToString().IndexOf(" ") > 0)

                session_id = Strings.Left(drv.SESSION_ID.ToString(), drv.SESSION_ID.ToString().IndexOf(" "));
            else
                session_id = drv.SESSION_ID.ToString();


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
                        Email_Error(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
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
                    RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
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
                    UpdateTable("update XCC_REPORT_NEW set audio_link = '" + this_filename + "' where ID = " + theID);
                    return this_filename;
                }
                else
                    return drv.audio_link.ToString();// already an mp3, file exists though doesn't match session ID
            }

            return "";
        }
        #endregion

        #region Public Email_Error
        /// <summary>
        /// Email_Error
        /// </summary>
        /// <param name="error_text"></param>
        /// <param name="also_email"></param>
        public static void Email_Error(string error_text, string also_email = "")
        {
            if (error_text == "Unexpected end of stream before frame complete" | error_text == "Thread was being aborted." | error_text == "Exception of type 'Tamir.SharpSsh.jsch.SftpException' was thrown.<br>173.10.20.190")
                return;
            // include logo
            StringBuilder body = new StringBuilder();
            body.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:Arial, sans-serif; font-size:12px;\"><tr><td colspan=\"2\" style=\"color:#FF0000; font-weight:bold;\">Please do not reply to this auto-generated email.</td></tr><tr><td colspan=\"2\">&nbsp;</td></tr>");

            body.Append("<tr><td colspan=\"2\"><img src=\"http://app.callcriteria.com/img/cc_words_logo.png\" alt=\"Call Criteria\" width=\"322\" height=\"50\"></td></tr><tr><td colspan=\"2\">&nbsp;</td></tr>");
            HttpContext current = System.Web.HttpContext.Current;
            body.Append("<tr><td>Source Page:</td><td>" + current.Request.Path + "</td></tr>");
            body.Append("<tr><td>Referrer:</td><td>" + current.Request.ServerVariables["HTTP_REFERER"] + "</td></tr>");
            body.Append("<tr><td>Name:</td><td>" + current.User.Identity.Name + "</td></tr>");
            body.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            body.Append("<tr><td colspan=\"2\" style=\"font-weight:bold;\">Error Information</td></tr>");
            body.Append("<tr><td>Browser:</td><td>" + current.Request.ServerVariables["HTTP_USER_AGENT"] + "</td></tr>");
            // body.Append("")
            try
            {
                body.Append("<tr><td>QueryString:</td><td>");
                string[] qs_string = current.Request.QueryString.AllKeys;
                foreach (string qs_str in qs_string)
                    body.Append(qs_str + " - " + current.Request.QueryString[qs_str].ToString() + ", ");
                if (Strings.Right(body.ToString(), 2) == ", ")
                    body.Remove(body.Length - 2, 2);
                body.Append("</td></tr>");
            }
            catch (Exception ex)
            {
            }

            try
            {
                body.Append("<tr><td>Session:</td><td>");
                for (var x = 0; x <= current.Session.Keys.Count - 1; x++)
                    body.Append(current.Session.Keys[x].ToString() + " - " + current.Session[current.Session.Keys[x].ToString()].ToString() + ", ");
                if (Strings.Right(body.ToString(), 2) == ", ")
                    body.Remove(body.Length - 2, 2);
                body.Append("</td></tr>");
            }
            catch (Exception ex)
            {
                body.Append("<tr><td>" + ex.Message + "</td></tr>");
            }

            body.Append("<tr><td>Date and Time:</td><td>" + DateTime.Now.ToString() + "</td></tr>");
            body.Append("<tr><td>Page:</td><td>" + current.Request.ServerVariables["URL"] + "</td></tr>");
            body.Append("<tr><td>Remote Address:</td><td>" + current.Request.ServerVariables["REMOTE_ADDR"] + "</td></tr>");
            body.Append("<tr><td>HTTP Referer:</td><td>" + current.Request.ServerVariables["HTTP_REFERER"] + "</td></tr>");

            body.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            body.Append("<tr><td colspan=\"2\" style=\"font-weight:bold;\">Actual Error</td></tr>");
            body.Append("<tr><td colspan=\"2\">" + error_text + "</td></tr>");
            body.Append("</table>");
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            try
            {
                // If debug Then
                SqlCommand reply2 = new SqlCommand("EXEC send_dbmail  @profile_name='General',  @copy_recipients=@CC,  @recipients='stace@callcriteria.com;chris@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn);
                reply2.Parameters.AddWithValue("Subject_text", "WEB SITE ERROR ");

                reply2.Parameters.AddWithValue("CC", also_email);
                reply2.Parameters.AddWithValue("Body", body.ToString());

                reply2.CommandTimeout = 60;
                reply2.ExecuteNonQuery();
            }
            // End If
            catch (Exception ex)
            {
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion

    }
}