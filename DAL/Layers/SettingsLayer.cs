using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;
using DAL.Extensions;
using DAL.Models.SettingsModels;

namespace DAL.Layers
{
    public class SettingsLayer
    {

        /// <summary>
        ///GetNotes method
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public List<NotesModel> GetNotes(DropdownInfo info)
        {
            try
            {
                return DapperHelper.GetList<NotesModel>(@"select * from app_Notes where appname ='" + info.name + "'").ToList();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
            return new List<NotesModel>();
        }

        /// <summary>
        /// InsertNotes method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public NotesModel InsertNotes(NotesModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"if(not exists(select * from app_Notes where id =" + model.id + ") )begin insert into app_Notes(note,appname) values('" + model.note + "' , '" + model.appName + "') end else begin update app_Notes set note = '" + model.note + "',appname = '" + model.appName + "' where app_Notes.id = " + model.id + " end", conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    var err = ex.Message;
                }


            }

            return model;
        }

        /// <summary>
        /// AddApplication method
        /// </summary>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        public AppSettings AddApplication(AppSettings appSettings)
        {

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string insert = "insert into app_settings (" +
                        "appname, " +
                        "contact_name , " +
                        "contact_phone ," +
                        "active," +
                        "first_noti ," +
                        "notification_profile ," +
                        "stream_only , " +
                        "recording_url ," +
                        "recording_user ," +
                        "record_password ," +
                        "record_format ," +
                        "recording_dirs ," +
                        "rejection_profile," +
                        "budget," +
                        "NA_affect," +
                        "show_section_score," +
                        "transcript_rate," +
                        "minimum_minutes" +
                        ") values (" +
                        "@appname," +
                        "@contactName, " +
                        "@contactPhone," +
                        "@active," +
                        "@firstNotificationAssigee," +
                        "@notificationProfileId," +
                        "@streamOnly," +
                        "@recordingUrl," +
                        "@recordingUser," +
                        "@recordPassword," +
                        "@recordFormat," +
                        "@recordingDirictories," +
                        "@rejectionProfile," +
                        "@budget," +
                        "@naAaffect," +
                        "@showSectionScore," +
                        "@transcriptRate," +
                        "@minimumMinutes" +
                        ")";


                    var reply = new SqlCommand(insert, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text,

                    };
                    reply.Parameters.AddWithValue("@appname", appSettings.appname);
                    reply.Parameters.AddWithValue("@contactName", appSettings.contactName == null ? (object)DBNull.Value : appSettings.contactName);
                    reply.Parameters.AddWithValue("@contactPhone", appSettings.contactPhone == null ? (object)DBNull.Value : appSettings.contactPhone);
                    reply.Parameters.AddWithValue("@active", appSettings.active);
                    reply.Parameters.AddWithValue("@firstNotificationAssigee", appSettings.firstNotificationAssigee == null ? (object)DBNull.Value : appSettings.firstNotificationAssigee);
                    reply.Parameters.AddWithValue("@notificationProfileId", appSettings.notificationProfileId == null ? (object)DBNull.Value : appSettings.notificationProfileId);
                    reply.Parameters.AddWithValue("@streamOnly", appSettings.streamOnly);
                    reply.Parameters.AddWithValue("@recordingUser", (appSettings.recordingUser) == null ? (object)DBNull.Value : appSettings.recordingUser);
                    reply.Parameters.AddWithValue("@naAaffect", (appSettings.naAffect == null) ? (object)DBNull.Value : appSettings.naAffect);
                    reply.Parameters.AddWithValue("@showSectionScore", appSettings.showSectionScore);
                    reply.Parameters.AddWithValue("@recordingUrl", appSettings.recordingUrl == null ? (object)DBNull.Value : appSettings.recordingUrl);
                    reply.Parameters.AddWithValue("@budget", appSettings.budget);
                    reply.Parameters.AddWithValue("@rejectionProfile", appSettings.rejectionProfile);
                    reply.Parameters.AddWithValue("@recordPassword", appSettings.recordPassword == null ? (object)DBNull.Value : appSettings.recordPassword);
                    reply.Parameters.AddWithValue("@recordFormat", appSettings.recordFormat == null ? (object)DBNull.Value : appSettings.recordFormat);
                    reply.Parameters.AddWithValue("@recordingDirictories", appSettings.recordingDirictories == null ? (object)DBNull.Value : appSettings.recordingDirictories);
                    reply.Parameters.AddWithValue("@transcriptRate", appSettings.transcriptRate);
                    reply.Parameters.AddWithValue("@minimumMinutes", appSettings.minimumMinutes);
                    conn.Open();
                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    var err = ex.Message;
                }
            }
            DropdownInfo info = new DropdownInfo
            {
                id = appSettings.id
            };
            return GetAppByName(info);
        }

        /// <summary>
        /// UpdateApplication method
        /// </summary>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        public AppSettings UpdateApplication(AppSettings appSettings)
        {

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string insert = "update app_settings set "
                        + "appname = @appname, "
                        + "contact_name = @contactName, "
                        + "contact_phone = @contactPhone, "
                        + "contact_email = @contactEmail,"
                        + "active = @active, "
                        + "first_noti = @firstNotificationAssigee, "
                        + "rejection_profile = @rejectionProfile, "
                        + "notification_profile = @notificationProfileId, "
                        + "stream_only = @streamOnly, "
                        + "recording_url = @recordingUrl, "
                        + "recording_user = @recordingUser, "
                        + "record_password = @recordPassword, "
                        + "record_format = @recordFormat, "
                        + "recording_dirs = @recordingDirictories, "
                        + "budget = @budget, "
                        + "NA_affect = @naAaffect, "
                        + "show_section_score = @showSectionScore," +
                        "transcript_rate = @transcriptRate,"
                        + "minimum_minutes = @minimumMinutes" +
                        " where id=@id";


                    var reply = new SqlCommand()
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text,

                    };
                    reply.Connection = conn;
                    reply.CommandText = insert;

                    reply.Parameters.AddWithValue("@id", appSettings.id);
                    reply.Parameters.AddWithValue("@appname", appSettings.appname == null ? (object)DBNull.Value : appSettings.appname);
                    reply.Parameters.AddWithValue("@contactName", appSettings.contactName == null ? (object)DBNull.Value : appSettings.contactName);
                    reply.Parameters.AddWithValue("@contactPhone", appSettings.contactPhone == null ? (object)DBNull.Value : appSettings.contactPhone);
                    reply.Parameters.AddWithValue("@active", appSettings.active);
                    reply.Parameters.AddWithValue("@firstNotificationAssigee", appSettings.firstNotificationAssigee == null ? (object)DBNull.Value : appSettings.firstNotificationAssigee);
                    reply.Parameters.AddWithValue("@notificationProfileId", appSettings.notificationProfileId == null ? (object)DBNull.Value : appSettings.notificationProfileId);
                    reply.Parameters.AddWithValue("@streamOnly", appSettings.streamOnly);
                    reply.Parameters.AddWithValue("@recordingUser", appSettings.recordingUser == null ? (object)DBNull.Value : appSettings.recordingUser);
                    reply.Parameters.AddWithValue("@naAaffect", (appSettings.naAffect == -1 || appSettings.naAffect == null) ? (object)DBNull.Value : appSettings.naAffect);
                    reply.Parameters.AddWithValue("@showSectionScore", appSettings.showSectionScore);
                    reply.Parameters.AddWithValue("@recordingUrl", appSettings.recordingUrl == null ? (object)DBNull.Value : appSettings.recordingUrl);
                    reply.Parameters.AddWithValue("@budget", appSettings.budget);
                    reply.Parameters.AddWithValue("@rejectionProfile", appSettings.rejectionProfile);
                    reply.Parameters.AddWithValue("@recordPassword", appSettings.recordPassword == null ? (object)DBNull.Value : appSettings.recordPassword);
                    reply.Parameters.AddWithValue("@recordFormat", appSettings.recordFormat == null ? (object)DBNull.Value : appSettings.recordFormat);
                    reply.Parameters.AddWithValue("@recordingDirictories", appSettings.recordingDirictories == null ? (object)DBNull.Value : appSettings.recordingDirictories);
                    reply.Parameters.AddWithValue("@contactEmail", appSettings.contactEmail == null ? (object)DBNull.Value : appSettings.contactEmail);
                    reply.Parameters.AddWithValue("@transcriptRate", appSettings.transcriptRate);
                    reply.Parameters.AddWithValue("@minimumMinutes", appSettings.minimumMinutes);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            DropdownInfo info = new DropdownInfo
            {
                id = appSettings.id
            };
            return GetAppByName(info);
        }

        /// <summary>
        /// DeleteApplication method
        /// </summary>
        /// <param name="appId"></param>
        public dynamic DeleteApplication(AppModelWL appModelWL)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from app_settings where id =" + appModelWL.id, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return GetAppListWithLogo();
        }
        /// <summary>
        /// DuplicateApp
        /// </summary>
        /// <param name="appModelWL"></param>
        /// <returns></returns>
        public dynamic DuplicateApp(AppModelWL appModelWL)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string insert = "insert into app_settings (" +
                        "appname, " +
                        "active,client_logo_small)values (" +
                        "@appname," +
                        "@active," +
                        "@client_logo_small)";


                    var reply = new SqlCommand(insert, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text,

                    };
                    reply.Parameters.AddWithValue("@appname", appModelWL.name);

                    reply.Parameters.AddWithValue("@active", appModelWL.active);

                    reply.Parameters.AddWithValue("@client_logo_small", appModelWL.logo == null ? (object)DBNull.Value : appModelWL.logo);
                    conn.Open();
                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    var err = ex.Message;
                }
            }
            return GetAppListWithLogo();
        }

        /// <summary>
        /// DeleteNotes method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteNotes(DropdownInfo info)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from app_Notes where id =" + info.id, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
            return "success";
        }

        /// <summary>
        /// DeleteAllNotes method
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string DeleteAllNotes(DropdownInfo info)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    SqlCommand reply = new SqlCommand();
                    if (info.id != null && info.id != 0)
                    {
                        reply = new SqlCommand(@"delete from app_Notes where id =" + info.id + "", conn)
                        {
                            CommandTimeout = int.MaxValue,
                            CommandType = CommandType.Text
                        };
                    }
                    else
                    {
                        reply = new SqlCommand(@"delete from app_Notes where appname ='" + info.name + "'", conn)
                        {
                            CommandTimeout = int.MaxValue,
                            CommandType = CommandType.Text
                        };
                    }


                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
            return "success";
        }

        /// <summary>
        /// GetBillingRate method
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public List<BillingRatesModel> GetBillingRate(DropdownInfo info)
        {
            List<BillingRatesModel> billing = new List<BillingRatesModel>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string userName;
                    if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                        userName = "test321";// HttpContext.Current.User.Identity.Name;
                    else
                        userName = HttpContext.Current.User.Identity.Name;



                    string comm = "";

                    comm = @"select * from billing_rates where appname ='" + info.name + "'";


                    var list = DapperHelper.GetList<BillingRatesModel>(comm).ToList();

                    return DapperHelper.GetList<BillingRatesModel>(comm).ToList();
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                }

                return new List<BillingRatesModel>();
            }

        }

        /// <summary>
        /// InsertBilingRate method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic InsertBilingRate(BillingRatesModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand reply = new SqlCommand();
                try
                {
                    string sql = @"if(not exists(select * from billing_rates where id = @id )) begin "
                  + "insert into billing_rates("
                  + "appname,"
                  + "start_minutes,"
                  + "end_minutes,"
                  + "rate,"
                  + "bill_type,"
                  + "scorecard_only"
                  + ")values("
                  + "@appname,"
                  + "@startminutes ,"
                  + "@endminutes ,"
                  + "@rate ,"
                  + "@billtype ,"
                  + "@scorecardonly)"
                  + "end else begin update billing_rates set appname = @appname"
                  + ",start_minutes = @startminutes"
                  + ",end_minutes = @endminutes"
                  + ",rate = @rate"
                  + ",bill_type = @billtype"
                  + ",scorecard_only = @scorecardonly"
                  + " where id = @id end";

                    reply = new SqlCommand(sql, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };

                    reply.Connection = conn;
                    reply.CommandText = sql;

                    reply.Parameters.AddWithValue("@appname", model.appname ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@startminutes", model.start_minutes);
                    reply.Parameters.AddWithValue("@endminutes", model.end_minutes);
                    reply.Parameters.AddWithValue("@rate", model.rate);
                    reply.Parameters.AddWithValue("@billtype", model.bill_type ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@scorecardonly", model.scorecard_only);
                    reply.Parameters.AddWithValue("@id", model.id);

                    conn.Open();

                    reply.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
            DropdownInfo dropdownInfo = new DropdownInfo();
            dropdownInfo.name = model.appname;
            return GetBillingRate(dropdownInfo);
        }

        /// <summary>
        /// DeleteBillingRate method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic DeleteBillingRate(ModelForDeleteBilling model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from billing_rates where id =" + model.billingId, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
            DropdownInfo dropdownInfo = new DropdownInfo();
            dropdownInfo.name = model.appName;
            return GetBillingRate(dropdownInfo);
        }

        /// <summary>
        /// GetApplication
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<DropdownInfo> GetAvailableApplicationList()
        {

            string select = @"select id, appname as name,cast(active as bit) as active from app_settings order by active desc,appname";

            var appInfoLst = DapperHelper.GetList<DropdownInfo>(select).ToList();

            return appInfoLst;
        }

        //public List<> GetPrefillinfo()
        //{

        //    string select = @"select id, appname as name,cast(active as bit) as active from app_settings order by active desc,appname";

        //    var appInfoLst = DapperHelper.GetList<DropdownInfo>(select).ToList();

        //    return appInfoLst;
        //}

        /// <summary>
        /// GetAvailableScorecardList method
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        public List<DropdownInfo> GetAvailableScorecardList(string appname)
        {


            string select = @"select id, Short_name as name,active from scorecards where appname = '" + appname + "' order by active desc";

            var appInfoLst = DapperHelper.GetList<DropdownInfo>(select).ToList();

            return appInfoLst;
        }

        /// <summary>
        /// GetAppByName method
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        public dynamic GetAppByName(DropdownInfo info)
        {
            AppSettings appInfoLst = new AppSettings();
            try
            {
                if (info.id != null && info.id != 0)
                {
                    string select = @"select id, "
                           + "appname, "
                           + "contact_name as contactName, "
                           + "contact_phone as contactPhone, "
                           + "contact_email as contactEmail, "
                           + "active, "
                           + "first_noti as firstNotificationAssigee, "
                           + "rejection_profile as rejectionProfile, "
                           + "notification_profile as notificationProfileId, "
                           + "stream_only as streamOnly, "
                           + "client_logo as logo, "
                           + "client_logo_small as smallLogo, "
                           + "recording_url as recordingUrl, "
                           + "recording_user recordingUser, "
                           + "record_password as recordPassword, "
                           + "record_format as recordFormat, "
                           + "recording_dirs as recordingDirictories, "
                           + "budget as budget, "
                           + "NA_affect as naAaffect, "
                           + "show_section_score as showSectionScore, "
                           + "transcript_rate as transcriptRate,"
                           + "minimum_minutes as minimumMinutes from app_settings  where id = " + info.id;

                    appInfoLst = DapperHelper.GetSingle<AppSettings>(select);

                }
                else
                {

                    string select = @"select top 1 id, "
                           + "appname, "
                           + "contact_name as contactName, "
                           + "contact_phone as contactPhone, "
                           + "contact_email as contactEmail, "
                           + "active, "
                           + "first_noti as firstNotificationAssigee, "
                           + "rejection_profile as rejectionProfile, "
                           + "notification_profile as notificationProfileId, "
                           + "stream_only as streamOnly, "
                           + "client_logo as logo, "
                           + "client_logo_small as smallLogo, "
                           + "recording_url as recordingUrl, "
                           + "recording_user recordingUser, "
                           + "record_password as recordPassword, "
                           + "record_format as recordFormat, "
                           + "recording_dirs as recordingDirictories, "
                           + "budget as budget, "
                           + "NA_affect as naAaffect, "
                           + "show_section_score as showSectionScore, "
                           + "transcript_rate as transcriptRate,"
                           + "minimum_minutes as minimumMinutes from app_settings  where appname = '" + info.name + "'";

                    appInfoLst = DapperHelper.GetSingle<AppSettings>(select);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appInfoLst;

        }



        /// <summary>
        /// UploadClientLogo method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public dynamic UploadClientLogo(FileUploaderSettinsModel file)
        {

            string path = Path.Combine(HostingEnvironment.MapPath(@"~/Logos/"), Path.GetFileName(file.fileName));
            string path2 = Path.Combine(@"/webApi/Logos/", Path.GetFileName(file.fileName));
            string FileName = Path.GetFileName(file.fileName);
            byte[] thePictureAsBytes = new byte[file.fileData.Length];
            try
            {
                //Uploading picture into file
                if (file != null && file.fileData.Length > 0)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }


                    using (FileStream fs = File.Create(path))
                    {
                        fs.Close();
                        using (FileStream fs2 = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                        {
                            FileName = Path.GetFileName(file.fileName);
                            thePictureAsBytes = new byte[file.fileData.Length];
                            //using (BinaryReader theReader = new BinaryReader(fs))
                            //{
                            //    thePictureAsBytes = theReader.ReadBytes(file.fileData.Length);
                            //}

                            fs2.Write(file.fileData, 0, file.fileData.Length);
                            fs2.Close();
                        }

                    }
                }
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
                {
                    try
                    {


                        var reply = new SqlCommand(@"if(exists(select * from app_settings where appname ='" + file.appname + "'))" +
                                              "begin " +
                                              " update app_settings set client_logo ='" + path2 +
                                              "'  where appname  = '" + file.appname + "' end", conn)
                        {
                            CommandTimeout = int.MaxValue,
                            CommandType = CommandType.Text
                        };

                        conn.Open();

                        reply.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //inserting file path into db


            DropdownInfo info = new DropdownInfo
            {
                name = file.appname
            };
            return GetAppByName(info);
        }
        /// <summary>
        /// UploadClientLogoSmall method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public dynamic UploadClientLogoSmall(FileUploaderSettinsModel file)
        {


            string path = Path.Combine(HostingEnvironment.MapPath(@"~/Logos/"), Path.GetFileName(file.fileName));
            string path2 = Path.Combine(@"/webApi/Logos/", Path.GetFileName(file.fileName));
            string FileName = Path.GetFileName(file.fileName);
            byte[] thePictureAsBytes = new byte[file.fileData.Length];
            try
            {
                //Uploading picture into file
                if (file != null && file.fileData.Length > 0)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }


                    using (FileStream fs = File.Create(path))
                    {
                        fs.Close();
                        using (FileStream fs2 = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                        {
                            FileName = Path.GetFileName(file.fileName);
                            thePictureAsBytes = new byte[file.fileData.Length];
                            //using (BinaryReader theReader = new BinaryReader(fs))
                            //{
                            //    thePictureAsBytes = theReader.ReadBytes(file.fileData.Length);
                            //}

                            fs2.Write(file.fileData, 0, file.fileData.Length);
                            fs2.Close();
                        }

                    }
                }
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
                {
                    try
                    {


                        var reply = new SqlCommand(@"if(exists(select * from app_settings where appname ='" + file.appname + "'))" +
                                              "begin " +
                                              " update app_settings set client_logo_small ='" + path2 +
                                              "' where appname  = '" + file.appname + "' end", conn)
                        {
                            CommandTimeout = int.MaxValue,
                            CommandType = CommandType.Text
                        };

                        conn.Open();

                        reply.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //inserting file path into db


            DropdownInfo info = new DropdownInfo
            {
                name = file.appname
            };
            return GetAppByName(info);

        }



        /// <summary>
        /// GetDefaultProfile method
        /// </summary>
        /// <returns></returns>
        public dynamic GetConfigurationProfile()
        {
            string select = @"select * from notification_profiles order by profile_name";
            return DapperHelper.GetList<DefaultNotificationProfileModel>(select).ToList();
        }
        /// <summary>
        /// GetRjectionProfile method
        /// </summary>
        /// <returns></returns>
        public dynamic GetRjectionProfile()
        {
            string select = @"select * from rejection_profiles order by profile_name";
            return DapperHelper.GetList<RejectionProfileModel>(select).ToList();
        }
        /// <summary>
        /// GetNaAffected method
        /// </summary>
        /// <returns></returns>
        public dynamic GetNaAffected()
        {
            var naAffected = new[]
               {
                    new { id = 0, Name = "No Score Reduction" },
                    new { id = 1, Name = "Reduce Total Score" },
                    new { id = -1,Name = "None"}
                };
            return naAffected;
        }
        /// <summary>
        /// GetFirstNotificationAssigee
        /// </summary>
        /// <returns></returns>
        public dynamic GetFirstNotificationAssigee()
        {
            var firstNotificationAffected = new[]
               {
                    new { id = 0, Name = "None" },
                    new { id = 1, Name = "Agent" },
                    new { id = -1,Name = "Supervisor"},
                    new {id = 2, Name = "Manager"}
                };
            return firstNotificationAffected;
        }



        /// <summary>
        /// Updating scorecard
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic UpdateScorecard(ScorecardSettingsModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }


                var oldScorecard = GetScorecardSettingsById(model.id);
                var variances = model.DetailedCompare(oldScorecard);

                if (variances.Count != 0)
                {
                    try
                    {
                        StringBuilder insertChanges = new StringBuilder();

                        insertChanges.Append(@"insert into scorecard_changes(scorecard,change,changed_by,changed_date) values ");
                        int count = 0;
                        foreach (var item in variances)
                        {
                            count++;
                            insertChanges.Append("("
                           + model.id + ", '"
                           + item.Prop.ToString() + " changed from: " + item.oldValue + " to: " + item.newValue + "',"
                           + "'" + userName + "',"
                           + "GetDate())");
                            if (count != variances.Count)
                                insertChanges.Append(",");


                        }
                        conn.Open();
                        SqlCommand SqlComm = new SqlCommand()
                        {
                            CommandTimeout = int.MaxValue,
                            CommandType = CommandType.Text
                        };
                        SqlComm.CommandText = insertChanges.ToString();
                        SqlComm.Connection = conn;


                        SqlComm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    conn.Close();
                }

                #region  Query
                try
                {

                    var command = @"update scorecards set [description]=@description ,"
                  + "short_name =@short_name  ,"
                  + "appname=@appname  ,"
                  //+ "date_added =@date_added  ,"
                  + "who_added=@who_added  ,"
                  + "active=@active ,"
                  + "review_type=@review_type  ,"
                  + "golden_user=@golden_user  ,"
                  + "calib_percent=@calib_percent, "
                  + "isCalibrated=@isCalibrated ,"
                  + "fail_score=@fail_score, "
                  + "team_lead=@team_lead  ,"
                  + "sc_sort=@sc_sort  ,"
                  + "training_count=@training_count, "
                  + "ni_scorecard=@ni_scorecard, "
                  + "transcribe=@transcribe ,"
                  + "score_type=@score_type  ,"
                  + "recal_percent=@recal_percent ,"
                  + "sectionless=@sectionless ,"
                  + "website_cost=@website_cost ,"
                  + "website_display=@website_display  ,"
                  + "min_transcript_count=@min_transcript_count, "
                  + "user_cant_dispute=@user_cant_dispute  ,"
                  + "max_speed=@max_speed ,"
                  + "min_cal=@min_cal ,"
                  + "num_cal_check=@num_cal_check ,"
                  + "import_type=@import_type  ,"
                  + "import_percent=@import_percent ,"
                  + "required_dispositions=@required_dispositions  ,"
                  + "min_call_length=@min_call_length, "
                  + "post_import_sp=@post_import_sp  ,"
                  + "pass_percent=@pass_percent ,"
                  + "cutoff_percent=@cutoff_percent ,"
                  + "cutoff_count=@cutoff_count ,"
                  + "import_agents=@import_agents ,"
                  + "keep_daily_calls=@keep_daily_calls ,"
                  + "hide_data=@hide_data  ,"
                  + "hide_school_data=@hide_school_data  ,"
                  + "sc_notification_score=@sc_notification_score ,"
                  + "cutoff_percent_avg=@cutoff_percent_avg ,"
                  + "scorecard_status=@scorecard_status  ,"
                  + "sc_notification_profile=@sc_notification_profile ,"
                  + "sc_profile=@sc_profile ,"
                  + "dedupe=@dedupe ,"
                  + "max_per_day=@max_per_day ,"
                  + "no_agent_login=@no_agent_login ,"
                  + "redact=@redact ,"
                  + "account_manager=@account_manager  ,"
                  + "email_failed=@email_failed ,"
                  + "show_custom_questions=@show_custom_questions ,"
                  + "onhold=@onhold ,"
                  + "retain_non_used_calls=@retain_non_used_calls ,"
                  + "max_call_length=@max_call_length ,"
                  + "meta_sort=@meta_sort  ,"
                  + "overwrite_group=@overwrite_group ,"
                  + "tango_calibrated=@tango_calibrated ,"
                  + "calib_role=@calib_role  ,"
                  + "qa_selected_role=@qa_selected_role  ,"
                  + "admin_selected_role=@admin_selected_role  ,"
                  + "client_selected_role=@client_selected_role  ,"
                  + "recalib_role=@recalib_role  ,"
                  + "manager_sees_supervisor=@manager_sees_supervisor ,"
                  + "rejection_profile=@rejection_profile ,"
                  + "tango_team_lead=@tango_team_lead  ,"
                  + "truncate_time=@truncate_time ,"
                  + "end_truncate_time=@end_truncate_time ,"
                  + "high_priority=@high_priority ,"
                  + "load_rate_15=@load_rate_15 ,"
                  + "load_rate_60=@load_rate_60 ,"
                  + "burn_rate_15=@burn_rate_15 ,"
                  + "burn_rate_60=@burn_rate_60 ,"
                  + "working_team=@working_team ,"
                  + "pending_queue=@pending_queue ,"
                  + "avg_review_time=@avg_review_time ,"
                  + "avg_call_length=@avg_call_length ,"
                  + "qa_qa_scorecard=@qa_qa_scorecard ,"
                  + "shift_end=@shift_end  ,"
                  + "shift_start=@shift_start  ,"
                  + "allow_others=@allow_others, "
                  + "isQAQACard=@isQAQACard ,"
                  + "calibration_floor=@calibration_floor ,"
                  + "call_turn_time=@call_turn_time ,"
                  + "auto_accept_bad_call=@auto_accept_bad_call ,"
                  //+ "allow_other_set=@allow_other_set  ,"
                  + "pay_type=@pay_type  ,"
                  + "qa_pay=@qa_pay  ,"
                  + "cal_spot_user_role=@cal_spot_user_role  ,"
                  + "dispute_base_percent=@dispute_base_percent"
                  + " where scorecards.id = @id;";



                    var reply = new SqlCommand(command, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };

                    reply.Parameters.AddWithValue("@id ", model.@id);
                    reply.Parameters.AddWithValue("@description", model.description ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@short_name", model.short_name ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@golden_user", model.golden_user ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@appname", model.appname ?? (object)DBNull.Value);
                    //reply.Parameters.AddWithValue("@date_added", model.date_added.ToShortDateString());
                    reply.Parameters.AddWithValue("@who_added", userName);
                    reply.Parameters.AddWithValue("@active", model.active);
                    reply.Parameters.AddWithValue("@review_type", model.review_type ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@isCalibrated", model.isCalibrated);
                    reply.Parameters.AddWithValue("@fail_score", model.fail_score);
                    reply.Parameters.AddWithValue("@team_lead", model.team_lead ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@sc_sort", model.sc_sort ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@training_count", model.training_count);
                    reply.Parameters.AddWithValue("@ni_scorecard", model.ni_scorecard);
                    reply.Parameters.AddWithValue("@transcribe", model.transcribe);
                    reply.Parameters.AddWithValue("@score_type", model.score_type);
                    reply.Parameters.AddWithValue("@recal_percent", model.recal_percent);
                    reply.Parameters.AddWithValue("@sectionless", model.sectionless);
                    reply.Parameters.AddWithValue("@website_cost", model.website_cost);
                    reply.Parameters.AddWithValue("@website_display", model.website_display ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@min_transcript_count", model.min_transcript_count);
                    reply.Parameters.AddWithValue("@user_cant_dispute", model.user_cant_dispute ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@max_speed", model.max_speed);
                    reply.Parameters.AddWithValue("@calib_percent", model.calib_percent);
                    reply.Parameters.AddWithValue("@min_cal", model.min_cal);
                    reply.Parameters.AddWithValue("@num_cal_check", model.num_cal_check);
                    reply.Parameters.AddWithValue("@import_type", model.import_type ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@import_percent", model.import_percent);
                    reply.Parameters.AddWithValue("@required_dispositions", model.required_dispositions ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@min_call_length", model.min_call_length);
                    reply.Parameters.AddWithValue("@post_import_sp", model.post_import_sp ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@pass_percent", model.pass_percent);
                    reply.Parameters.AddWithValue("@cutoff_percent", model.cutoff_percent);
                    reply.Parameters.AddWithValue("@cutoff_count", model.cutoff_count);
                    reply.Parameters.AddWithValue("@import_agents", model.import_agents);
                    reply.Parameters.AddWithValue("@keep_daily_calls", model.keep_daily_calls);
                    reply.Parameters.AddWithValue("@hide_data", model.hide_data ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@hide_school_data", model.hide_school_data ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@sc_notification_score", model.sc_notification_score);
                    reply.Parameters.AddWithValue("@cutoff_percent_avg", model.cutoff_percent_avg);
                    reply.Parameters.AddWithValue("@scorecard_status", model.scorecard_status ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@sc_notification_profile", model.sc_notification_profile);
                    reply.Parameters.AddWithValue("@sc_profile", model.sc_profile);
                    reply.Parameters.AddWithValue("@dedupe", model.dedupe);
                    reply.Parameters.AddWithValue("@max_per_day", model.max_per_day);
                    reply.Parameters.AddWithValue("@no_agent_login", model.no_agent_login);
                    reply.Parameters.AddWithValue("@redact", model.redact);
                    reply.Parameters.AddWithValue("@account_manager", model.account_manager ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@email_failed", model.email_failed);
                    reply.Parameters.AddWithValue("@show_custom_questions", model.show_custom_questions);
                    reply.Parameters.AddWithValue("@onhold", model.onhold);
                    reply.Parameters.AddWithValue("@retain_non_used_calls", model.retain_non_used_calls);
                    reply.Parameters.AddWithValue("@max_call_length", model.max_call_length);
                    reply.Parameters.AddWithValue("@meta_sort", model.meta_sort ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@overwrite_group", model.overwrite_group);
                    reply.Parameters.AddWithValue("@tango_calibrated", model.tango_calibrated);
                    reply.Parameters.AddWithValue("@calib_role", model.calib_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@qa_selected_role", model.qa_selected_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@admin_selected_role", model.admin_selected_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@client_selected_role", model.client_selected_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@recalib_role", model.recalib_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@manager_sees_supervisor", model.manager_sees_supervisor);
                    reply.Parameters.AddWithValue("@rejection_profile", model.rejection_profile);
                    reply.Parameters.AddWithValue("@tango_team_lead", model.tango_team_lead ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@truncate_time", model.truncate_time);
                    reply.Parameters.AddWithValue("@end_truncate_time", model.end_truncate_time);
                    reply.Parameters.AddWithValue("@high_priority", model.high_priority);
                    reply.Parameters.AddWithValue("@load_rate_15", model.load_rate_15);
                    reply.Parameters.AddWithValue("@load_rate_60", model.load_rate_60);
                    reply.Parameters.AddWithValue("@burn_rate_15", model.burn_rate_15);
                    reply.Parameters.AddWithValue("@burn_rate_60", model.burn_rate_60);
                    reply.Parameters.AddWithValue("@working_team", model.working_team);
                    reply.Parameters.AddWithValue("@pending_queue", model.pending_queue);
                    reply.Parameters.AddWithValue("@avg_review_time", model.avg_review_time);
                    reply.Parameters.AddWithValue("@avg_call_length", model.avg_call_length);
                    reply.Parameters.AddWithValue("@qa_qa_scorecard", model.qa_qa_scorecard);
                    reply.Parameters.AddWithValue("@shift_end", model.shift_end ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@shift_start", model.shift_start ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@allow_others", model.allow_others);
                    reply.Parameters.AddWithValue("@isQAQACard", model.isQAQACard);
                    reply.Parameters.AddWithValue("@calibration_floor", model.calibration_floor);
                    reply.Parameters.AddWithValue("@call_turn_time", model.call_turn_time);
                    reply.Parameters.AddWithValue("@auto_accept_bad_call", model.auto_accept_bad_call);
                    //reply.Parameters.AddWithValue("@allow_other_set", model.allow_other_set.ToShortDateString());
                    reply.Parameters.AddWithValue("@pay_type", model.pay_type ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@qa_pay", model.qa_pay ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@cal_spot_user_role", model.cal_spot_user_role ?? (object)DBNull.Value);
                    reply.Parameters.AddWithValue("@dispute_base_percent", model.dispute_base_percent);
                    conn.Open();
                    #endregion

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }


            }

            return GetScorecardSettingsById(model.id);
        }

        /// <summary>
        /// Adding new scorecard (with all props)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic AddNewScorecard(ScorecardSettingsModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                SqlCommand reply = new SqlCommand();
                try
                {
                    #region Query
                    var command = @"insert into scorecards("
                  + "[description],"//1
                  + "short_name,"///2
                  + "appname,"//3
                  + "date_added,"//4
                  + "who_added,"//5
                  + "active," +//6
                  "review_type," +//7
                  "golden_user," +//8
                  "calib_percent," +//9
                  "isCalibrated," +//10
                  "fail_score," +//11
                  "team_lead," +//12
                  "sc_sort," +//13
                  "training_count," +//14
                  "ni_scorecard," +//15
                  "transcribe," +//16
                  "score_type," +//17
                  "recal_percent," +//18
                  "sectionless," +//19
                  "website_cost," +//20
                  "website_display," +//21
                  "min_transcript_count," +//22
                  "user_cant_dispute," +//23
                  "max_speed," +//24
                  "min_cal," +//25
                  "num_cal_check," +//26
                  "import_type," +//27
                  "import_percent," +//28
                  "required_dispositions," +//29
                  "min_call_length," +//30
                  "post_import_sp," +//31
                  "pass_percent," +//32
                  "cutoff_percent," +//33
                  "cutoff_count," +//34
                  "import_agents," +//35
                  "keep_daily_calls," +//36
                  "hide_data," +//37
                  "hide_school_data," +//38
                  "sc_notification_score," +//39
                  "cutoff_percent_avg," +//40
                  "scorecard_status," +//41
                  "sc_notification_profile," +//42
                  "sc_profile," +//43
                  "dedupe," +//44
                  "max_per_day," +//45
                  "no_agent_login," +//46
                  "redact," +//47
                  "account_manager," +//48
                  "email_failed," +//49
                  "show_custom_questions," +//50
                  "onhold," +//51
                  "retain_non_used_calls," +//52
                  "max_call_length," +//53
                  "meta_sort," +//54
                  "overwrite_group," +//55
                  "tango_calibrated," +//56
                  "calib_role," +//57
                  "qa_selected_role," +//58
                  "admin_selected_role," +//59
                  "client_selected_role," +//60
                  "recalib_role," +//61
                  "manager_sees_supervisor," +//62
                  "rejection_profile," +//63
                  "tango_team_lead," +//64
                  "truncate_time," +//65
                  "end_truncate_time," +//66
                  "high_priority," +//67
                  "load_rate_15," +//68
                  "load_rate_60," +//69
                  "burn_rate_15," +//70
                  "burn_rate_60," +//71
                  "working_team," +//72
                  "pending_queue," +//73
                  "avg_review_time," +//74
                  "avg_call_length," +//75
                  "qa_qa_scorecard," +//76
                  "shift_end," +//77
                  "shift_start," +//78
                  "allow_others," +//79
                  "isQAQACard," +//80
                  "calibration_floor," +//81
                  "call_turn_time," +//82
                  "auto_accept_bad_call," +//83
                                           //"allow_other_set," +//84
                  "pay_type," +//85
                  "qa_pay," +//86
                  "cal_spot_user_role," +//87
                  "dispute_base_percent" +//88
                  ")values(" +
                  " @description " +//1
                  ",@short_name                      " +//2
                  ",@appname                         " +//3
                  ",GetDate()                      " +//4
                  ",@who_added                       " +//5
                  ",@active                          " +//6
                  ",@review_type                     " +//7
                  ",@golden_user                     " +//8
                  ",@calib_percent                   " +//9
                  ",@isCalibrated                    " +//10
                  ",@fail_score                      " +//11
                  ",@team_lead                       " +//12
                  ",@sc_sort                         " +//13
                  ",@training_count                  " +//14
                  ",@ni_scorecard                    " +//15
                  ",@transcribe                      " +//16
                  ",@score_type                      " +//17
                  ",@recal_percent                   " +//18
                  ",@sectionless                     " +//19
                  ",@website_cost                    " +//20
                  ",@website_display                 " +//21
                  ",@min_transcript_count            " +//22
                  ",@user_cant_dispute               " +//23
                  ",@max_speed                   " +//24
                  ",@min_cal                         " +//25
                  ",@num_cal_check                   " +//26
                  ",@import_type                     " +//27
                  ",@import_percent                  " +//28
                  ",@required_dispositions           " +//29
                  ",@min_call_length                 " +//30
                  ",@post_import_sp                  " +//31
                  ",@pass_percent                    " +//32
                  ",@cutoff_percent                  " +//33
                  ",@cutoff_count                    " +//34
                  ",@import_agents                   " +//35
                  ",@keep_daily_calls                " +//36
                  ",@hide_data                       " +//37
                  ",@hide_school_data                " +//38
                  ",@sc_notification_score           " +//39
                  ",@cutoff_percent_avg              " +//40
                  ",@scorecard_status                " +//41
                  ",@sc_notification_profile         " +//42
                  ",@sc_profile                      " +//43
                  ",@dedupe                          " +//44
                  ",@max_per_day                     " +//45
                  ",@no_agent_login                  " +//46
                  ",@redact                          " +//47
                  ",@account_manager                 " +//48
                  ",@email_failed                    " +//49
                  ",@show_custom_questions           " +//50
                  ",@onhold                          " +//51
                  ",@retain_non_used_calls           " +//52
                  ",@max_call_length                 " +//53
                  ",@meta_sort                       " +//54
                  ",@overwrite_group                 " +//55
                  ",@tango_calibrated                " +//56
                  ",@calib_role                      " +//57
                  ",@qa_selected_role                " +//58
                  ",@admin_selected_role             " +//59
                  ",@client_selected_role            " +//60
                  ",@recalib_role                    " +//61
                  ",@manager_sees_supervisor         " +//62
                  ",@rejection_profile               " +//63
                  ",@tango_team_lead                 " +//64
                  ",@truncate_time                   " +//65
                  ",@end_truncate_time               " +//66
                  ",@high_priority                   " +//67
                  ",@load_rate_15                    " +//68
                  ",@load_rate_60                    " +//69
                  ",@burn_rate_15                    " +//70
                  ",@burn_rate_60                    " +//71
                  ",@working_team                    " +//72
                  ",@pending_queue                   " +//73
                  ",@avg_review_time                 " +//74
                  ",@avg_call_length                 " +//75
                  ",@qa_qa_scorecard                 " +//76
                  ",@shift_end                       " +//77
                  ",@shift_start                     " +//78
                  ",@allow_others                    " +//79
                  ",@isQAQACard                      " +//80
                  ",@calibration_floor               " +//81
                  ",@call_turn_time                  " +//82
                  ",@auto_accept_bad_call            " +//83
                                                        // ",@allow_other_set                 " +//84
                  ",@pay_type                        " +//85
                  ",@qa_pay                          " +//86
                  ",@cal_spot_user_role              " +//87
                  ",@dispute_base_percent            );";//88




                    reply = new SqlCommand(command, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };

                    //reply.Parameters.AddWithValue("@id ", model.@id);//1
                    reply.Parameters.AddWithValue("@description", model.description ?? (object)DBNull.Value);//2
                    reply.Parameters.AddWithValue("@short_name", model.short_name ?? (object)DBNull.Value);//3
                    reply.Parameters.AddWithValue("@golden_user", model.golden_user ?? (object)DBNull.Value);//4
                    reply.Parameters.AddWithValue("@appname", model.appname ?? (object)DBNull.Value);//5
                    //reply.Parameters.AddWithValue("@date_added", model.date_added.ToShortDateString());//6
                    reply.Parameters.AddWithValue("@who_added", userName);//7
                    reply.Parameters.AddWithValue("@active", model.active);//8
                    reply.Parameters.AddWithValue("@review_type", model.review_type ?? (object)DBNull.Value);//9
                    reply.Parameters.AddWithValue("@isCalibrated", model.isCalibrated);//10
                    reply.Parameters.AddWithValue("@fail_score", model.fail_score);//11
                    reply.Parameters.AddWithValue("@team_lead", model.team_lead ?? (object)DBNull.Value);//12
                    reply.Parameters.AddWithValue("@sc_sort", model.sc_sort ?? (object)DBNull.Value);//13
                    reply.Parameters.AddWithValue("@training_count", model.training_count);//14
                    reply.Parameters.AddWithValue("@ni_scorecard", model.ni_scorecard);//15
                    reply.Parameters.AddWithValue("@transcribe", model.transcribe);//16
                    reply.Parameters.AddWithValue("@score_type", model.score_type);//17
                    reply.Parameters.AddWithValue("@recal_percent", model.recal_percent);//18
                    reply.Parameters.AddWithValue("@sectionless", model.sectionless);//19
                    reply.Parameters.AddWithValue("@website_cost", model.website_cost);//20
                    reply.Parameters.AddWithValue("@website_display", model.website_display ?? (object)DBNull.Value);//21
                    reply.Parameters.AddWithValue("@min_transcript_count", model.min_transcript_count);//22
                    reply.Parameters.AddWithValue("@user_cant_dispute", model.user_cant_dispute ?? (object)DBNull.Value);//23
                    reply.Parameters.AddWithValue("@max_speed", model.max_speed);//24
                    reply.Parameters.AddWithValue("@calib_percent", model.calib_percent);//25
                    reply.Parameters.AddWithValue("@min_cal", model.min_cal);//26
                    reply.Parameters.AddWithValue("@num_cal_check", model.num_cal_check);//27
                    reply.Parameters.AddWithValue("@import_type", model.import_type ?? (object)DBNull.Value);//28
                    reply.Parameters.AddWithValue("@import_percent", model.import_percent);//29
                    reply.Parameters.AddWithValue("@required_dispositions", model.required_dispositions ?? (object)DBNull.Value);//30
                    reply.Parameters.AddWithValue("@min_call_length", model.min_call_length);//31
                    reply.Parameters.AddWithValue("@post_import_sp", model.post_import_sp ?? (object)DBNull.Value);//32
                    reply.Parameters.AddWithValue("@pass_percent", model.pass_percent);//33
                    reply.Parameters.AddWithValue("@cutoff_percent", model.cutoff_percent);//34
                    reply.Parameters.AddWithValue("@cutoff_count", model.cutoff_count);//35
                    reply.Parameters.AddWithValue("@import_agents", model.import_agents);//36
                    reply.Parameters.AddWithValue("@keep_daily_calls", model.keep_daily_calls);//37
                    reply.Parameters.AddWithValue("@hide_data", model.hide_data ?? (object)DBNull.Value);//38
                    reply.Parameters.AddWithValue("@hide_school_data", model.hide_school_data ?? (object)DBNull.Value);//39
                    reply.Parameters.AddWithValue("@sc_notification_score", model.sc_notification_score);//40
                    reply.Parameters.AddWithValue("@cutoff_percent_avg", model.cutoff_percent_avg);//41
                    reply.Parameters.AddWithValue("@scorecard_status", model.scorecard_status ?? (object)DBNull.Value);//42
                    reply.Parameters.AddWithValue("@sc_notification_profile", model.sc_notification_profile);//43
                    reply.Parameters.AddWithValue("@sc_profile", model.sc_profile);//44
                    reply.Parameters.AddWithValue("@dedupe", model.dedupe);//45
                    reply.Parameters.AddWithValue("@max_per_day", model.max_per_day);//46
                    reply.Parameters.AddWithValue("@no_agent_login", model.no_agent_login);//47
                    reply.Parameters.AddWithValue("@redact", model.redact);//48
                    reply.Parameters.AddWithValue("@account_manager", model.account_manager ?? (object)DBNull.Value);//49
                    reply.Parameters.AddWithValue("@email_failed", model.email_failed);//50
                    reply.Parameters.AddWithValue("@show_custom_questions", model.show_custom_questions);//51
                    reply.Parameters.AddWithValue("@onhold", model.onhold);//52
                    reply.Parameters.AddWithValue("@retain_non_used_calls", model.retain_non_used_calls);//53
                    reply.Parameters.AddWithValue("@max_call_length", model.max_call_length);//54
                    reply.Parameters.AddWithValue("@meta_sort", model.meta_sort ?? (object)DBNull.Value);//55
                    reply.Parameters.AddWithValue("@overwrite_group", model.overwrite_group);//56
                    reply.Parameters.AddWithValue("@tango_calibrated", model.tango_calibrated);//57
                    reply.Parameters.AddWithValue("@calib_role", model.calib_role ?? (object)DBNull.Value);//58
                    reply.Parameters.AddWithValue("@qa_selected_role", model.qa_selected_role ?? (object)DBNull.Value);//59
                    reply.Parameters.AddWithValue("@admin_selected_role", model.admin_selected_role ?? (object)DBNull.Value);//60
                    reply.Parameters.AddWithValue("@client_selected_role", model.client_selected_role ?? (object)DBNull.Value);//61
                    reply.Parameters.AddWithValue("@recalib_role", model.recalib_role ?? (object)DBNull.Value);//62
                    reply.Parameters.AddWithValue("@manager_sees_supervisor", model.manager_sees_supervisor);//63
                    reply.Parameters.AddWithValue("@rejection_profile", model.rejection_profile);//64
                    reply.Parameters.AddWithValue("@tango_team_lead", model.tango_team_lead ?? (object)DBNull.Value);//65
                    reply.Parameters.AddWithValue("@truncate_time", model.truncate_time);//66
                    reply.Parameters.AddWithValue("@end_truncate_time", model.end_truncate_time);//67
                    reply.Parameters.AddWithValue("@high_priority", model.high_priority);//68
                    reply.Parameters.AddWithValue("@load_rate_15", model.load_rate_15);//69
                    reply.Parameters.AddWithValue("@load_rate_60", model.load_rate_60);//70
                    reply.Parameters.AddWithValue("@burn_rate_15", model.burn_rate_15);//71
                    reply.Parameters.AddWithValue("@burn_rate_60", model.burn_rate_60);//72
                    reply.Parameters.AddWithValue("@working_team", model.working_team);//73
                    reply.Parameters.AddWithValue("@pending_queue", model.pending_queue);//74
                    reply.Parameters.AddWithValue("@avg_review_time", model.avg_review_time);//75
                    reply.Parameters.AddWithValue("@avg_call_length", model.avg_call_length);//76
                    reply.Parameters.AddWithValue("@qa_qa_scorecard", model.qa_qa_scorecard);//77
                    reply.Parameters.AddWithValue("@shift_end", model.shift_end ?? (object)DBNull.Value);//78
                    reply.Parameters.AddWithValue("@shift_start", model.shift_start ?? (object)DBNull.Value);//79
                    reply.Parameters.AddWithValue("@allow_others", model.allow_others);//80
                    reply.Parameters.AddWithValue("@isQAQACard", model.isQAQACard);//81
                    reply.Parameters.AddWithValue("@calibration_floor", model.calibration_floor);//82
                    reply.Parameters.AddWithValue("@call_turn_time", model.call_turn_time);//83
                    reply.Parameters.AddWithValue("@auto_accept_bad_call", model.auto_accept_bad_call);//84
                    //reply.Parameters.AddWithValue("@allow_other_set", model.allow_other_set.ToShortDateString());//85
                    reply.Parameters.AddWithValue("@pay_type", model.pay_type ?? (object)DBNull.Value);//86
                    reply.Parameters.AddWithValue("@qa_pay", model.qa_pay ?? (object)DBNull.Value);//87
                    reply.Parameters.AddWithValue("@cal_spot_user_role", model.cal_spot_user_role ?? (object)DBNull.Value);//88
                    reply.Parameters.AddWithValue("@dispute_base_percent", model.dispute_base_percent);//89
                    conn.Open();
                    #endregion

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex) { throw ex; }
                return GetScorecardSettingsById(model.id);
            }
        }


        /// <summary>
        /// geting all scorecard settings for current scorecard by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScorecardSettingsModel GetScorecardSettingsById(int id)
        {
            string command = @"select * from scorecards where id =" + id;
            var mapper = DapperHelper.GetSingle<ScorecardSettingsModel>(command);
            return mapper;

        }
        /// <summary>
        /// deleting scorecard by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic DeleteScorecard(int id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from scorecards where id =" + id, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }
        /// <summary>
        /// geting scorecard changes for history of change by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ScorecardChangesModel> GetScorecardChanges(int id)
        {
            string command = @"select * from scorecard_changes where scorecard = " + id + " order by changed_date desc ";
            var mapper = DapperHelper.GetList<ScorecardChangesModel>(command).ToList();
            return mapper;
        }




        /// <summary>
        /// geting all scorecard notes for current scorecard by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ScorecardNotesModel> GetScorecardNotes(DropdownInfo info)
        {
            string command = @"select * from sc_notes where sc_id = " + info.id;
            var mapper = DapperHelper.GetList<ScorecardNotesModel>(command).ToList();
            return mapper;
        }
        /// <summary>
        /// deletes 1 scorecard note by notes id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public dynamic DeleteScorecardNote(ScorecardNotesModel info)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from sc_notes where id =" + info.id, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            DropdownInfo dropdownInfo = new DropdownInfo
            {
                id = info.sc_id
            };
            return GetScorecardNotes(dropdownInfo);
        }
        /// <summary>
        /// deleting all scorecard notes by sorecard id for current scorecard
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public dynamic DeleteAllScorecardNotes(ScorecardNotesModel info)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(@"delete from sc_notes where sc_id =" + info.sc_id, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            DropdownInfo dropdownInfo = new DropdownInfo
            {
                id = info.sc_id
            };
            return GetScorecardNotes(dropdownInfo);
        }
        /// <summary>
        /// Adding new scorecard note
        /// </summary>
        /// <param name="scorecardNotesModel"></param>
        /// <returns></returns>
        public dynamic AddNewScorecardNote(ScorecardNotesModel scorecardNotesModel)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"insert into sc_notes(note,sc_id)values(@note,@sc_id);";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@note ", scorecardNotesModel.note);
                    reply.Parameters.AddWithValue("@sc_id", scorecardNotesModel.sc_id);
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            DropdownInfo dropdownInfo = new DropdownInfo
            {
                id = scorecardNotesModel.sc_id
            };
            return GetScorecardNotes(dropdownInfo);
        }
        /// <summary>
        /// Udating existing scorecard note
        /// </summary>
        /// <param name="scorecardNotesModel"></param>
        /// <returns></returns>
        public dynamic UpdateScorecardNote(ScorecardNotesModel scorecardNotesModel)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @" update sc_notes set note = @note,sc_id = @sc_id where id = @id;";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = int.MaxValue,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@note ", scorecardNotesModel.note);
                    reply.Parameters.AddWithValue("@sc_id", scorecardNotesModel.sc_id);
                    reply.Parameters.AddWithValue("@id", scorecardNotesModel.id);
                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            DropdownInfo dropdownInfo = new DropdownInfo
            {
                id = scorecardNotesModel.sc_id
            };
            return GetScorecardNotes(dropdownInfo);
        }





        /// <summary>
        /// Gets users who is in user role Admin and Account Manager
        /// </summary>
        /// <returns></returns>
        public dynamic GetAccountManagerUsers()
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                List<string> accountManagers = new List<string>();
                string command = @"select Username from userextrainfo where user_role in ('Admin','Account Manager') order by username";
                try
                {

                    SqlCommand sqlComm = new SqlCommand(command);
                    sqlComm.CommandTimeout = 41;
                    sqlComm.Connection = sqlCon;
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        accountManagers.Add(reader.GetValue(reader.GetOrdinal("Username")).ToString());
                    }
                    return accountManagers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Gets users who is in user role QA Lead,Tango TL, Admin and Account Manager
        /// </summary>
        /// <returns></returns>
        public dynamic GetTLUsers()
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                List<string> accountManagers = new List<string>();
                string command = @" select distinct username from (select username from userextrainfo where user_role in ('QA Lead','Tango TL', 'Admin', 'Account Manager')" +
                                       "union all select distinct team_lead as username from scorecards) a where username != '' order by username";
                try
                {

                    SqlCommand sqlComm = new SqlCommand(command);
                    sqlComm.CommandTimeout = 41;
                    sqlComm.Connection = sqlCon;
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        accountManagers.Add(reader.GetValue(reader.GetOrdinal("username")).ToString());
                    }
                    return accountManagers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Gets users who is in user role QA Lead,Tango TL, Admin and Account Manager
        /// </summary>
        /// <returns></returns>
        public dynamic GetTangoTLUsers()
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                List<string> accountManagers = new List<string>();
                string command = @" select distinct username from (select username from userextrainfo where user_role in ('QA Lead','Tango TL', 'Admin', 'Account Manager')" +
                                       "union all select distinct tango_team_lead as username from scorecards union all select distinct team_lead as username from scorecards) a where username != '' order by username";
                try
                {

                    SqlCommand sqlComm = new SqlCommand(command);
                    sqlComm.CommandTimeout = 41;
                    sqlComm.Connection = sqlCon;
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        accountManagers.Add(reader.GetValue(reader.GetOrdinal("username")).ToString());
                    }
                    return accountManagers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Gets users who is in user role supervisor,client and manager
        /// </summary>
        /// <returns></returns>
        public dynamic GetGoldenUsers(DropdownInfo info)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                List<string> accountManagers = new List<string>();
                string command = @" select distinct userapps.username from userapps join userextrainfo on userapps.username = userextrainfo.username where userapps.appname = @appname and user_role in ('supervisor','client','manager') order by userapps.username";
                try
                {

                    SqlCommand sqlComm = new SqlCommand(command);
                    sqlComm.CommandTimeout = 41;
                    sqlComm.Connection = sqlCon;
                    sqlComm.Parameters.AddWithValue("@appname", info.name);
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        accountManagers.Add(reader.GetValue(reader.GetOrdinal("username")).ToString());
                    }
                    return accountManagers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }




        /// <summary>
        /// GetApiKey
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        public dynamic GetApiKey(string appname)
        {
            string select = @"SELECT * FROM [API_KEYS] WHERE ([appname] ='" + appname + "')";
            return DapperHelper.GetList<ApiKeyModel>(select).ToList();
        }
        /// <summary>
        /// AddApiKey
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        public dynamic AddApiKey(string appname)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"INSERT INTO [API_KEYS] ([appname], [active],[date_added] ) VALUES (@appname, 1,GetDate())";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@appname", appname);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetApiKey(appname);
        }

        /// <summary>
        /// UpdateApiKey
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic UpdateApiKey(ApiKeyModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"UPDATE [API_KEYS] SET  [active] = @active WHERE [id] = @id";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@active", model.active);
                    reply.Parameters.AddWithValue("@id", model.id);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetApiKey(model.appname);
        }
        /// <summary>
        /// DeleteApiKey
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic DeleteApiKey(ApiKeyModel model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"DELETE FROM [API_KEYS] WHERE [id] = @id";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@id", model.id);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetApiKey(model.appname);
        }






        /// <summary>
        /// AddExport
        /// </summary>
        /// <param name="appExportSettingsModel"></param>
        /// <returns></returns>
        public dynamic AddExport(AppExportSetting model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"INSERT INTO [app_specific_exports] ([appname], [field], [sp]) VALUES (@appname, @field, @sp)";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@appname", model.exportSettings.appname);
                    reply.Parameters.AddWithValue("@field", model.customClumns.columnName);
                    reply.Parameters.AddWithValue("@sp", model.exportSettings.sp);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetExport(model.exportSettings.appname);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        public dynamic GetExport(string appname)
        {
            try
            {
                AppExportSettingsModel appExportSettingsModel = new AppExportSettingsModel();
                List<ExportSettings> exportSettings = new List<ExportSettings>();
                List<ExportCustomClumns> exportCustoClumns = new List<ExportCustomClumns>();
                string command = "SELECT * FROM [app_specific_exports] WHERE ([appname] ='" + appname + "')";
                exportSettings = DapperHelper.GetList<ExportSettings>(command).ToList();
                string command2 = @"select   distinct id  as columnId," +
                                    "column_name as columnName," +
                                    "cast(case when sort_key is null then 0 else 1 end as bit) as sortable from  [available_columns] where column_name is not null";
                exportCustoClumns = DapperHelper.GetList<ExportCustomClumns>(command2).ToList();
                appExportSettingsModel.customClumns = exportCustoClumns;
                appExportSettingsModel.exportSettings = exportSettings;
                return appExportSettingsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// UpdateExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic UpdateExport(AppExportSetting model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"UPDATE [app_specific_exports] SET [field] = @field WHERE [id] = @id";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@field", model.customClumns.columnName);
                    reply.Parameters.AddWithValue("@id", model.exportSettings.id);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetExport(model.exportSettings.appname);
        }

        /// <summary>
        /// DeleteExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic DeleteExport(AppExportSetting model)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    string Sqlcommand = @"DELETE FROM [app_specific_exports] WHERE [id] = @id";
                    var reply = new SqlCommand(Sqlcommand, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@id", model.exportSettings.id);

                    conn.Open();

                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return GetExport(model.exportSettings.appname);
        }

        /// <summary>
        /// Get app logo method
        /// </summary>
        /// <returns></returns>
        public dynamic GetAppListWithLogo()
        {
            var userName = HttpContext.Current.GetUserName();
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sqlComm = new SqlCommand()
                {
                    CommandText = "[GetAvailableAppListSettings]",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    Connection = sqlCon
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                var reader = sqlComm.ExecuteReader();
                var response = AppModelWL.Create(reader);
                return response;
            }
        }

        /// <summary>
        /// GetAllScorecardList
        /// </summary>
        /// <returns></returns>
        public dynamic GetAllScorecardList()
        {
            string command = @"select id,isnull(short_name,'no name') as [name] from scorecards order by [name]";
            var mapper = DapperHelper.GetList<ScorecardsInfo>(command).ToList();
            return mapper;
        }
        /// <summary>
        /// GetQuestionSection
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public dynamic GetQuestionSection(ScorecardsInfo scorecards)
        {
            string command = @"select id,section,section_order,Descrip as descrip, appname,orig_id,serious,scorecard_id,max_score_loss from Sections where scorecard_id = " + scorecards.id + " order by section_order";
            var mapper = DapperHelper.GetList<SectionModel>(command).ToList();
            return mapper;
        }
        /// <summary>
        /// UpdateQuestionSection
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public dynamic UpdateSections(List<SectionModel> sectionModel)
        {
            var userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {

                    string command = @"update Sections set " +
               "section = @section," +
               "section_order = @sectionOrder" +
               //"Descrip = @descrip," +
               //"appname = @appname," +
               //"orig_id=@origId," +
               // "serious = @serious," +
               //"scorecard_id = @scorecardId," +
               //"max_score_loss = @maxScoreLoss" +
               " where id = @id";
                    foreach (var item in sectionModel)
                    {
                        SqlCommand SqlComm = new SqlCommand()
                        {
                            CommandType = CommandType.Text,
                            CommandTimeout = 60
                        };
                        SqlComm.CommandText = command;
                        SqlComm.Connection = sqlCon;
                        SqlComm.Parameters.AddWithValue("@id", item.id);
                        SqlComm.Parameters.AddWithValue("@section", item.section ?? (object)DBNull.Value);
                        SqlComm.Parameters.AddWithValue("@sectionOrder", item.section_order ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@descrip", item.descrip ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@appname", item.appname ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@origId", item.orig_id ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@serious", item.serious ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@scorecardId", item.scorecard_id ?? (object)DBNull.Value);
                        //SqlComm.Parameters.AddWithValue("@maxScoreLoss", item.max_score_loss ?? (object)DBNull.Value);
                        sqlCon.Open();
                        SqlComm.ExecuteNonQuery();
                        sqlCon.Close();

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ScorecardsInfo scorecards = new ScorecardsInfo
                {
                    id = (int)sectionModel[0].scorecard_id
                };
                return GetQuestionSection(scorecards);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scorecards"></param>
        /// <returns></returns>
        public dynamic GetQustionList(ScorecardsInfo scorecards)
        {
            #region Query
            string command = "";

            command = @"select questions.id," +
                         "questions.section," +
                         "questions.category," +
                         "questions.q_order," +
                         "questions.question," +
                         "case when isnull(questions.q_type,'')= '' then 'regular' else questions.q_type end as q_type," +
                         "questions.q_short_name," +
                         "questions.active," +
                         "questions.heading," +
                         "questions.[Order] as [order]," +
                         "questions.QuestionText as questionText," +
                         "questions.q_percent," +
                         "questions.appname," +
                         "questions.auto_yes," +
                         "questions.auto_no," +
                         "questions.agent_display," +
                         "questions.default_answer," +
                         "questions.orig_id," +
                         "questions.total_points," +
                         "questions.template," +
                         "questions.template_items," +
                         "questions.linked_question," +
                         "questions.email_wrong," +
                         "questions.campaign_specific," +
                         "questions.scorecard_id," +
                         "questions.QA_points as qaPoints," +
                         "questions.compliance," +
                         "questions.date_q_added," +
                         "questions.non_billable," +
                         "questions.comments_allowed," +
                         "questions.linked_answer," +
                         "questions.linked_comment," +
                         " cast(isnull(questions.client_visible,0)as bit) as client_visible," +
                         "questions.client_guideline_visible," +
                         "questions.Sectionless_Order as sectionlessOrder," +
                         "questions.linked_visible," +
                         "questions.client_dashboard_visible," +
                         "questions.pinned," +
                         "questions.pre_production," +
                         "questions.single_comment," +
                         "questions.points_paused," +
                         "questions.points_paused_date," +
                         "questions.full_width," +
                         "questions.wide_q," +
                         "questions.require_custom_comment," +
                         "questions.sentence," +
                         "questions.ddl_type," +
                         "questions.ddlQuery," +
                         "questions.options_verb," +
                         "Sections.section as sectionName," +
                         "questions.left_column_question from questions left join Sections on Sections.id = questions.section  where questions.scorecard_id =" + scorecards.id + "  order by questions.active desc,Sections.section_order,questions.q_order ";



            var mapper = DapperHelper.GetList<QuestionModel>(command);
            return mapper;
            #endregion
        }
        /// <summary>
        /// GetQuestionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetQuestionById(int id)
        {
            #region Query
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                var command = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                command = @"select id," +
                            "section," +
                            "category," +
                            "q_order," +
                            "question," +
                            "case when isnull(questions.q_type,'')= '' then 'regular' else questions.q_type end as q_type," +
                            "q_short_name," +
                            "active," +
                            "heading," +
                            "[Order] as [order]," +
                            "QuestionText as questionText," +
                            "q_percent," +
                            "appname," +
                            "auto_yes," +
                            "auto_no," +
                            "agent_display," +
                            "default_answer," +
                            "orig_id," +
                            "total_points," +
                            "template," +
                            "template_items," +
                            "linked_question," +
                            "email_wrong," +
                            "campaign_specific," +
                            "scorecard_id," +
                            "QA_points as qaPoints," +
                            "compliance," +
                            "date_q_added," +
                            "non_billable," +
                            "comments_allowed," +
                            "linked_answer," +
                            "linked_comment," +
                            "cast(isnull(client_visible,0)as bit) as client_visible," +
                            "client_guideline_visible," +
                            "Sectionless_Order as sectionlessOrder," +
                            "cast(linked_visible as bit) as linkedVisible," +
                            "client_dashboard_visible," +
                            "pinned," +
                            "pre_production," +
                            "single_comment," +
                            "points_paused," +
                            "points_paused_date," +
                            "full_width," +
                            "wide_q," +
                            "require_custom_comment," +
                            "sentence," +
                            "ddl_type," +
                            "ddlQuery," +
                            "options_verb," +
                            "left_column_question from questions where id =" + id;
                var mapper = DapperHelper.GetSingle<QuestionModel>(command);

                if (mapper.linked_question != null && mapper.linked_question != "0")
                {
                    string sql = "";
                    if(mapper.linked_question == "")
                    {
                        sql = @"select q_short_name as linkedQuestionName from questions where id = null";
                    }
                    else
                    {
                        sql = @"select q_short_name as linkedQuestionName from questions where id =" + int.Parse(mapper.linked_question);
                        
                    }
                   
                    mapper.linkedQuestionName = DapperHelper.GetSingle<string>(sql);
                }
                if (mapper.linked_answer != null && mapper.linked_answer != 0 )
                {
                    string sql2 = @"select answer_text as linkedAnswerText from question_answers where id = " + mapper.linked_answer + " order by answer_text";
                    mapper.linkedAnswerText = DapperHelper.GetSingle<string>(sql2);
                }

                if (mapper.linked_comment != null && mapper.linked_comment != 0)
                {
                    string sql3 = @"select  comment  + ' (' + answer_text + ')' as linkedCommentText from answer_comments join question_answers on question_answers.id = answer_id where answer_comments.id  = " + mapper.linked_comment + " order by answer_text desc ";
                    mapper.linkedCommentText = DapperHelper.GetSingle<string>(sql3);
                }




                return mapper;
            }
            #endregion
        }

        /// <summary>
        /// GetQuestionAnswer
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public dynamic GetQuestionAnswers(int questionId)
        {
            #region Query
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                var command = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                command = @"select id," +
                            "question_id as questionId," +
                            "answer_text as answerText," +
                            "answer_points as answerPoints," +
                            "isAutoFail," +
                            "autoselect," +
                            "right_answer as rightAnswer," +
                            "linked_answer as linkedAnswer," +
                            "old_answer_id as oldAnsewerId," +
                            "old_question_id as oldQuestionId," +
                            "acp_required as acpRequired," +
                            "answer_order as answerOrder," +
                            "answer_active as answeractive," +
                            "cs_text_returned as csTextReturned," +
                            "cs_id_returned as csIdReturned from question_answers where question_id =" + questionId;
                var mapper = DapperHelper.GetList<AnswerModel>(command).ToList();
                return mapper;
            }
            #endregion
        }
        /// <summary>
        /// GetAnswerById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetAnswerById(int id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                var command = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                command = command = @"select id," +
                            "question_id as questionId," +
                            "answer_text as answerText," +
                            "answer_points as answerPoints," +
                            "isAutoFail," +
                            "autoselect," +
                            "right_answer as rightAnswer," +
                            "linked_answer as linkedAnswer," +
                            "old_answer_id as oldAnsewerId," +
                            "old_question_id as oldQuestionId," +
                            "acp_required as acpRequired," +
                            "answer_order as answerOrder," +
                            "answer_active as answeractive," +
                            "cs_text_returned as csTextReturned," +
                            "cs_id_returned as csIdReturned from question_answers where id =" + id;
                var mapper = DapperHelper.GetSingle<AnswerModel>(command);
                return mapper;
            }
        }


        public dynamic UpdateAnswerV1(AnswerModel answerModel)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var userName = "";
                var command = "";
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                {
                    userName = "test calibrator";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                try
                {
                    var oldAnswer = (AnswerModel)GetAnswerById(answerModel.id);
                    var variances = answerModel.DetailedCompare(oldAnswer);
                    if (variances.Count != 0)
                    {
                        try
                        {
                            StringBuilder insertChanges = new StringBuilder();
                            insertChanges.Append(@"insert into guideline_updates(updated_by,updated_date,question_id,from_answer,to_answer)values ");
                            int count = 0;

                            foreach (var item in variances)
                            {
                                count++;
                                if (item.Prop == "answerText")
                                {
                                    insertChanges.Append("("
                                   + "'"
                                   + userName + "',"
                                   + "GetDate(),"
                                   + answerModel.questionId + ","
                                   + "'" + item.oldValue + "',"
                                   + "'" + item.newValue + "')"
                                   );
                                }

                            }
                            SqlCommand SqlComm = new SqlCommand()
                            {
                                CommandTimeout = 60,
                                CommandType = CommandType.Text
                            };
                            SqlComm.CommandText = insertChanges.ToString();
                            SqlComm.Connection = conn;


                            SqlComm.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //var rightAnswerChanged = DapperHelper.GetSingle<bool>("select right_answer from question_answers where id = "+ answerModel.id);

                //if(rightAnswerChanged != answerModel.rightAnswer)
                //{
                //     SqlCommand SqlComm = new SqlCommand()
                //            {
                //                CommandTimeout = 60,
                //                CommandType = CommandType.Text
                //            };
                //            SqlComm.CommandText = @"update form_q_scores set isRight = @isRight";
                //            SqlComm.Connection = conn;
                //sqlComm.Parameters.AddWithValue("@isRight", (object)answerModel.rightAnswer ?? DBNull.Value);

                //            SqlComm.ExecuteNonQuery();
                //            conn.Close();
                //}
                try
                {
                    command = command = @"update question_answers set question_id = @uestion_id," +
                         "answer_text = @answer_text," +
                         "answer_points = @answer_points," +
                         "isAutoFail = @isAutoFail," +
                         "autoselect = @autoselect," +
                         "right_answer = @right_answer," +
                         "linked_answer = @linked_answer," +
                         "old_answer_id = @old_answer_id," +
                         "old_question_id = @old_question_id," +
                         "acp_required = @acp_required," +
                         "answer_order = @answer_order," +
                         "answer_active = @answer_active," +
                         "cs_text_returned = @cs_text_returned," +
                         "cs_id_returned = @cs_id_returned where id = @id";


                    SqlCommand sqlComm = new SqlCommand(command, conn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    };
                    sqlComm.Parameters.AddWithValue("@id", (object)answerModel.id ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@answer_text", (object)answerModel.answerText ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@answer_points", (object)answerModel.answerPoints ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@isAutoFail", (object)answerModel.isAutoFail ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@autoselect", (object)answerModel.autoselect ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@right_answer", (object)answerModel.rightAnswer ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@linked_answer", (object)answerModel.linkedAnswer ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@old_answer_id", (object)answerModel.oldAnsewerId ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@old_question_id", (object)answerModel.oldQuestionId ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@acp_required", (object)answerModel.acpRequired ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@answer_order", (object)answerModel.answerOrder ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@answer_active", (object)answerModel.answeractive ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@cs_text_returned", (object)answerModel.csTextReturned ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@cs_id_returned", (object)answerModel.csIdReturned ?? DBNull.Value);
                    conn.Open();
                    sqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return GetAnswerById(answerModel.id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public dynamic UpdateQuestion(QuestionModel questionModel)
        {
            #region Query
            var userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "nataliaadmin";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var oldQuestion = (QuestionModel)GetQuestionById(questionModel.id);
                    var variances = questionModel.DetailedCompare(oldQuestion);
                    if (variances.Count != 0)
                    {
                        try
                        {
                            StringBuilder insertChanges = new StringBuilder();

                            int count = 0;
                            foreach (var item in variances)
                            {

                                if (item.Prop == "question")
                                {
                                    count++;
                                    if (count == 1)
                                    {
                                        insertChanges.Append(@"insert into guideline_updates(updated_by,updated_date,question_id,from_text,to_text)values ");
                                    }
                                    insertChanges.Append("("
                                   + "'"
                                   + userName + "',"
                                   + "GetDate(),"
                                   + questionModel.id + ","
                                   + "'" + item.oldValue + "',"
                                   + "'" + item.newValue + "'),"
                                   );
                                }

                            }
                            if (insertChanges.ToString() != string.Empty)
                            {
                                insertChanges.Remove(insertChanges.Length - 1, 1);
                            }

                            SqlCommand SqlComm = new SqlCommand()
                            {
                                CommandTimeout = 60,
                                CommandType = CommandType.Text
                            };
                            SqlComm.CommandText = insertChanges.ToString();
                            SqlComm.Connection = sqlCon;

                            if (insertChanges.ToString() != string.Empty)
                            {
                                sqlCon.Open();
                                SqlComm.ExecuteNonQuery();
                                sqlCon.Close();
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                #region Update query

                try
                {
                    string command = @"update questions set section = @section, " +//1
                             " category = @category," +//3
                             " q_order = @q_order," +//4
                             " question = @question," +//5
                             " q_type = @q_type," +//6
                             " q_short_name = @q_short_name," +//7
                             " active =  @active," +//8
                             " heading =@heading  ," +//9
                             " [Order]  = @order," +//10
                             " QuestionText = @QuestionText," +//11
                             " q_percent = @q_percent," +//12
                             " appname = @appname," +//13
                             " auto_yes = @auto_yes," +//14
                             " auto_no = @auto_no," +//15
                             " agent_display = @agent_display," +//16
                             " default_answer = @default_answer," +//17
                             " orig_id = @orig_id," +//18
                             " total_points = @total_points," +//19
                             " template =@template," +//20
                             " template_items = @template_items," +//21
                             " linked_question = @linked_question," +//22
                             " email_wrong = @email_wrong," +//23
                             " campaign_specific = @campaign_specific," +//24
                             " scorecard_id = @scorecard_id," +//25
                             " QA_points = @QA_points," +//26
                             " compliance = @compliance," +//27
                             " date_q_added = @date_q_added," +//28
                             " non_billable = @non_billable," +//29
                             " comments_allowed = @comments_allowed," +//30
                             " linked_answer = @linked_answer," +//31
                             " linked_comment = @linked_comment," +//32
                             " client_visible = cast(isnull(@client_visible,0)as int)," +//33
                             " client_guideline_visible = @client_guideline_visible," +//34
                             " Sectionless_Order = @Sectionless_Order," +//35
                             " linked_visible = @linked_visible," +//36
                             " client_dashboard_visible = @client_dashboard_visible," +//37
                             " pinned = @pinned," +//38
                             " pre_production = @pre_production," +//39
                             " single_comment = @single_comment," +//40
                             " points_paused = @points_paused," +//41
                             " points_paused_date = @points_paused_date," +//42
                             " full_width = @full_width," +//43
                             " wide_q = @wide_q," +//44
                             " require_custom_comment = @require_custom_comment," +//45
                             " sentence = @sentence," +//46
                             " ddl_type = @ddl_type," +//47
                             " ddlQuery = @ddlQuery," +//48
                             " options_verb = @options_verb," +//49
                             " left_column_question = @left_column_question where id = @id";//50
                    SqlCommand sqlComm = new SqlCommand(command, sqlCon)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    };
                    //sqlComm.CommandText = command;
                    sqlComm.Parameters.AddWithValue("@id", (object)questionModel.id ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@section", (object)questionModel.section ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@category", (object)questionModel.category ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@q_order", (object)questionModel.q_order ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@question", (object)questionModel.question ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@q_type", (object)questionModel.q_type ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@q_short_name", (object)questionModel.q_short_name ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@active", (object)questionModel.active ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@heading", (object)questionModel.heading ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@order", (object)questionModel.order ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@QuestionText", (object)questionModel.questionText ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@q_percent", (object)questionModel.q_percent ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@appname", (object)questionModel.appname ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@auto_yes", (object)questionModel.auto_yes ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@auto_no", (object)questionModel.auto_no ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@agent_display", (object)questionModel.agent_display ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@default_answer", (object)questionModel.default_answer ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@orig_id", (object)questionModel.orig_id ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@total_points", (object)questionModel.total_points ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@template", (object)questionModel.template ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@template_items", (object)questionModel.temlate_items ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@linked_question", questionModel.linked_question == null ?(object) DBNull.Value : questionModel.linked_question);
                    sqlComm.Parameters.AddWithValue("@email_wrong", (object)questionModel.email_wrong ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@campaign_specific", (object)questionModel.campaign_specific ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@scorecard_id", (object)questionModel.scorecard_id ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@QA_points", (object)questionModel.qaPoints ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@compliance", (object)questionModel.compliance ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@date_q_added", (object)questionModel.date_q_added ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@non_billable", (object)questionModel.non_billable ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@comments_allowed", (object)questionModel.comments_allowed ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@linked_answer", (object)questionModel.linked_answer ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@linked_comment", (object)questionModel.linked_comment ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@client_visible", (object)questionModel.client_visible ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@client_guideline_visible", (object)questionModel.client_guideline_visible ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@Sectionless_Order", (object)questionModel.sectionlessOrder ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@linked_visible", questionModel.linkedVisible);
                    sqlComm.Parameters.AddWithValue("@client_dashboard_visible", (object)questionModel.client_dashboard_visible ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@pinned", (object)questionModel.pinned ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@pre_production", (object)questionModel.pre_production ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@single_comment", (object)questionModel.single_comment ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@points_paused", (object)questionModel.points_paused ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@points_paused_date", (object)questionModel.points_paused_date ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@full_width", (object)questionModel.full_width ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@wide_q", (object)questionModel.wide_q ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@require_custom_comment", (object)questionModel.require_custom_comment ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@sentence", (object)questionModel.sentence ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@ddl_type", (object)questionModel.ddl_type ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@ddlQuery", (object)questionModel.ddlQuery ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@options_verb", (object)questionModel.options_verb ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@left_column_question", (object)questionModel.left_column_question ?? DBNull.Value);




                    sqlCon.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ScorecardsInfo scorecards = new ScorecardsInfo()
            {
                id = (int)questionModel.scorecard_id,
                isActiveQuestion = true
            };
            #endregion

            return GetQustionList(scorecards);
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public dynamic AddQuestion(QuestionModel questionModel)
        {
            #region Implementation
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                #region SqlCommand
                string command = @"insert into questions(" +
                              "section," +
                              "q_order," +
                              "question," +
                              "q_type," +
                              "q_short_name," +
                              "active," +
                              "QuestionText," +
                              "appname," +
                              "scorecard_id" +
                              ")values(" +
                              "@section," +
                              "@q_order," +
                              "@question," +
                              "@q_type," +
                              "@q_short_name," +
                              "@active," +
                              "@questionText," +
                              "@appname," +
                              "@scorecardId)";

                #endregion

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                SqlComm.CommandText = command;
                SqlComm.Connection = sqlCon;

                #region Params

                if (questionModel.section == null)
                {
                    SqlComm.Parameters.AddWithValue("@section", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@section", questionModel.section);
                }


                if (questionModel.q_order == null)
                {
                    SqlComm.Parameters.AddWithValue("@q_order", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@q_order", questionModel.q_order);
                }

                if (questionModel.question == null)
                {
                    SqlComm.Parameters.AddWithValue("@question", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@question", questionModel.question);
                }

                if (questionModel.q_type == null)
                {
                    SqlComm.Parameters.AddWithValue("@q_type", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@q_type", questionModel.q_type);
                }

                if (questionModel.q_short_name == null)
                {
                    SqlComm.Parameters.AddWithValue("@q_short_name", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@q_short_name", questionModel.q_short_name);
                }

                if (questionModel.active == null)
                {
                    SqlComm.Parameters.AddWithValue("@active", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@active", questionModel.active);
                }


                if (questionModel.questionText == null)
                {
                    SqlComm.Parameters.AddWithValue("@QuestionText", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@QuestionText", questionModel.questionText);
                }



                if (questionModel.appname == null)
                {
                    SqlComm.Parameters.AddWithValue("@appname", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@appname", questionModel.appname);
                }

                SqlComm.Parameters.AddWithValue("@scorecardId", questionModel.scorecard_id == null ? (object)DBNull.Value : questionModel.scorecard_id);


                #endregion

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
            }
            ScorecardsInfo scorecards = new ScorecardsInfo
            {
                id = (int)questionModel.scorecard_id
            };
            return GetQustionList(scorecards);
            #endregion
        }


        public dynamic PauseQaPoints(QuestionModel questionModel)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string command = @"update questions set points_paused = 1 where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                SqlComm.Parameters.AddWithValue("@id", questionModel.id);
                SqlComm.CommandText = command;
                SqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                string command2 = @"insert into update_mgmt_notes (note,date_created,who_created,update_id,status) Select (select q_short_name from questions where id = @id) + ' Points UnPaused',dbo.getMTdate(),@username, id, [status] From update_mgmt Where status <> 'Move Live' and scorecard = (select scorecard_id from questions where id = @id)";
                SqlCommand SqlComm2 = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                SqlComm2.Parameters.AddWithValue("@id", questionModel.id);
                SqlComm2.Parameters.AddWithValue("@username", userName);
                SqlComm2.CommandText = command2;
                SqlComm2.Connection = sqlCon;
                sqlCon.Open();
                SqlComm2.ExecuteNonQuery();
                sqlCon.Close();

                return GetQuestionById(questionModel.id);
            }
        }
        /// <summary>
        /// GetQuestionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        /// <summary>
        /// UnpauseQaPoints
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public dynamic UnpauseQaPoints(QuestionModel questionModel)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string command = @"update questions set points_paused = 0 where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                SqlComm.Parameters.AddWithValue("@id", questionModel.id);
                SqlComm.CommandText = command;
                SqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                string command2 = @"insert into update_mgmt_notes (note,date_created,who_created,update_id,status) Select (select q_short_name from questions where id = @id) + ' Points UnPaused',dbo.getMTdate(),@username, id, [status] From update_mgmt Where status <> 'Move Live' and scorecard = (select scorecard_id from questions where id = @id)";
                SqlCommand SqlComm2 = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                SqlComm2.Parameters.AddWithValue("@id", questionModel.id);
                SqlComm2.Parameters.AddWithValue("@username", userName);
                SqlComm2.CommandText = command2;
                SqlComm2.Connection = sqlCon;
                sqlCon.Open();
                SqlComm2.ExecuteNonQuery();
                sqlCon.Close();

                return GetQuestionById(questionModel.id);
            }
        }

        /// <summary>
        /// CloneQuestionToScorecard
        /// </summary>
        /// <param name="cloneModel"></param>
        /// <returns></returns>
        public dynamic CloneQuestionToScorecard(CloneModel cloneModel)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string command = @"[cloneQuestionToScorecard]";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = command,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@QID", cloneModel.QuestionModel.id);
                SqlComm.Parameters.AddWithValue("@deactivate", 0);
                SqlComm.Parameters.AddWithValue("@scorecard", cloneModel.Scorecards.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetQustionList(cloneModel.Scorecards);
            }
        }




        /// <summary>
        /// DedupeScorecard
        /// </summary>
        /// <param name="scorecards"></param>
        public void DedupeScorecard(ScorecardsInfo scorecards)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost"))
            {
                userName = "test321";
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60
                };
                try
                {
                    SqlComm.Parameters.AddWithValue("@scorecard", scorecards.id);
                    SqlComm.CommandText = "[dedupe_by_sc]";
                    SqlComm.Connection = sqlCon;
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                catch (Exception ex) { throw ex; }

            }
        }


        public dynamic UpdateMultipleQuestions(List<QuestionModel> questionModels)
        {
            try
            {
                foreach (var item in questionModels)
                {
                    UpdateQuestion(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return GetQustionList(new ScorecardsInfo() { id = (int)questionModels[0].scorecard_id });

        }


        public dynamic GetDropDownItemList(int questionId)
        {
            string sql = @"SELECT * FROM [dropDownItems] WHERE ([question_id] = @question_id) order by isnull(item_order,999)";

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@question_id", questionId);
                sqlCon.Open();
                List<DropDownItemModel> dropDownItemModelList = new List<DropDownItemModel>();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        dropDownItemModelList.Add(new DropDownItemModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            questionId = reader.IsDBNull(reader.GetOrdinal("question_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                            value = reader.GetValue(reader.GetOrdinal("value")).ToString(),
                            dateAdded = reader.IsDBNull(reader.GetOrdinal("date_added")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("date_added")).ToString()),
                            active = reader.IsDBNull(reader.GetOrdinal("active")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("active")).ToString()),
                            dateStart = reader.IsDBNull(reader.GetOrdinal("date_start")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("date_start")).ToString()),
                            dateEnd = reader.IsDBNull(reader.GetOrdinal("date_end")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("date_end")).ToString()),
                            itemOrder = reader.IsDBNull(reader.GetOrdinal("item_order")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("item_order")).ToString())
                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return dropDownItemModelList;

            }
        }


        public dynamic AddDropDownItem(DropDownItemModel dropDownItemModel)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = @"INSERT INTO [dropDownItems] ([value], [question_id], [date_added], [date_start], [date_end])" +
                                            "VALUES (@value, @question_id, dbo.getMTdate(), dbo.getMTdate(), @date_end)";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@value", dropDownItemModel.value);
                SqlComm.Parameters.AddWithValue("@question_id", dropDownItemModel.questionId);
                if (dropDownItemModel.dateEnd == null)
                {
                    SqlComm.Parameters.AddWithValue("@date_end", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@date_end", dropDownItemModel.dateStart);
                }
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetDropDownItemList((int)dropDownItemModel.questionId);
            }
        }


        public dynamic EditDropdownItem(DropDownItemModel dropDownItemModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = @"UPDATE [dropDownItems] SET " +
                    "[value] = @value, " +
                    "item_order = @item_order," +
                    "[date_start] = @date_start, " +
                    "[date_end] = @date_end " +
                    "WHERE [id] = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@value", dropDownItemModel.value);
                SqlComm.Parameters.AddWithValue("@item_order", dropDownItemModel.itemOrder == null ? (object)DBNull.Value : dropDownItemModel.itemOrder);
                if (dropDownItemModel.dateEnd == null)
                {
                    SqlComm.Parameters.AddWithValue("@date_start", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@date_start", dropDownItemModel.dateStart);
                }

                if (dropDownItemModel.dateEnd == null)
                {
                    SqlComm.Parameters.AddWithValue("@date_end", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@date_end", dropDownItemModel.dateEnd);
                }


                SqlComm.Parameters.AddWithValue("@id", dropDownItemModel.id);
                sqlCon.Open();
                try
                {
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return GetDropDownItemList((int)dropDownItemModel.questionId);
            }
        }


        public dynamic DeleteDropDownItem(DropDownItemModel dropDownItemModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = @"DELETE FROM [dropDownItems] WHERE [id] = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@id", dropDownItemModel.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetDropDownItemList((int)dropDownItemModel.questionId);
            }
        }


        public dynamic GetQuestionByIdSimple(int id)
        {
            var sql = @"select q_short_name, questions.id from questions join sections on sections.id = questions.section where active=1 and questions.id <> @ID and questions.scorecard_id = (select scorecard_id from questions where id = @ID) order by section_order, q_order ";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<QuestiomModelSimple> questiomModelSimples = new List<QuestiomModelSimple>();
                SqlComm.Parameters.AddWithValue("@ID", id);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    questiomModelSimples.Add(new QuestiomModelSimple
                    {
                        questionName = reader.GetValue(reader.GetOrdinal("q_short_name")).ToString(),
                        id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString())
                    });
                }

                return questiomModelSimples;
            }
        }


        public dynamic GetCommentsList(int id)
        {
            var sql = @"select  comment  + ' (' + answer_text + ')' as ddl_text,answer_comments.id   from answer_comments join question_answers on question_answers.id = answer_id where question_answers.question_id = @QID  order by answer_text desc ";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<LinkedCommentModel> questiomModelSimples = new List<LinkedCommentModel>();
                SqlComm.Parameters.AddWithValue("@QID", id);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    questiomModelSimples.Add(new LinkedCommentModel
                    {
                        ddlText = reader.GetValue(reader.GetOrdinal("ddl_text")).ToString(),
                        id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString())
                    });
                }

                return questiomModelSimples;
            }
        }

        public dynamic GetAnswerList(int id)
        {
            var sql = @"select answer_text, id from question_answers where question_id = @QID order by answer_text";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<LiskedAnswerModel> questiomModelSimples = new List<LiskedAnswerModel>();
                SqlComm.Parameters.AddWithValue("@QID", id);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    questiomModelSimples.Add(new LiskedAnswerModel
                    {
                        answerText = reader.GetValue(reader.GetOrdinal("answer_text")).ToString(),
                        id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString())
                    });
                }
                return questiomModelSimples;
            }
        }

        public dynamic GetCallData()
        {
            var sql = "SELECT distinct name FROM sys.columns c WHERE c.object_id = OBJECT_ID('xcc_report_new') order by name";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<string> names = new List<string>();
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    names.Add(reader.GetValue(reader.GetOrdinal("name")).ToString());
                }
                return names;
            }
        }


        public dynamic GetSchoolData()
        {
            var sql = "SELECT distinct name FROM sys.columns c WHERE c.object_id = OBJECT_ID('school_x_data') order by name";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<string> names = new List<string>();
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    names.Add(reader.GetValue(reader.GetOrdinal("name")).ToString());
                }
                return names;
            }
        }

        public dynamic GetFAQList(int Qid)
        {
            var sql = @"select * from q_faqs where question_id = @QID order by q_order";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<FAQsModel> fAQs = new List<FAQsModel>();
                SqlComm.Parameters.AddWithValue("@QID", Qid);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        fAQs.Add(new FAQsModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            questionId = int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                            questionText = reader.GetValue(reader.GetOrdinal("question_text")).ToString(),
                            questionAnswer = reader.GetValue(reader.GetOrdinal("question_answer")).ToString(),
                            qOrder = reader.IsDBNull(reader.GetOrdinal("q_order")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("q_order")).ToString()),
                            dateAdded = reader.IsDBNull(reader.GetOrdinal("dateadded")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("dateadded")).ToString()),
                            lastUpdateDate = reader.IsDBNull(reader.GetOrdinal("last_update_date"))? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("last_update_date")).ToString())
                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return fAQs;
            }
        }


        public dynamic DeleteFAQ(FAQsModel fAQsModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text, from_answer, to_answer) select @username, dbo.getMTDate(), @QID," +
                                            "(select question_text from q_faqs where ID=@ID), '**deleted**',  (select question_answer from q_faqs where ID=@ID), '**deleted**';" +
                                            "delete from q_faqs where id = @ID";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@ID", fAQsModel.id);
                SqlComm.Parameters.AddWithValue("@QID", fAQsModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetFAQList(fAQsModel.questionId);
            }
        }


        public dynamic UpdateFAQ(FAQsModel fAQsModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text, from_answer, to_answer) select @username, dbo.getMTDate(), @QID," +
                                            "(select question_text from q_faqs where ID=@ID), @question_text,  (select question_answer from q_faqs where ID=@ID),@question_answer;" +
                                            "update q_faqs set last_update_date=dbo.getMTDate(), question_text=@question_text, question_answer=@question_answer, q_order=@q_order where id = @ID";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@QID", fAQsModel.questionId);
                SqlComm.Parameters.AddWithValue("@ID", fAQsModel.id);
                SqlComm.Parameters.AddWithValue("@username", userName);
                SqlComm.Parameters.AddWithValue("@question_text", fAQsModel.questionText);
                SqlComm.Parameters.AddWithValue("@question_answer", fAQsModel.questionAnswer);
                if (fAQsModel.qOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@q_order", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@q_order", fAQsModel.qOrder);
                }

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetFAQList(fAQsModel.questionId);
            }
        }


        public dynamic GetInstructionsList(int Qid)
        {
            var sql = @"select * from q_instructions where question_id = @QID order by q_order";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<InstructionModel> instructions = new List<InstructionModel>();
                SqlComm.Parameters.AddWithValue("@QID", Qid);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        instructions.Add(new InstructionModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            questionId = reader.IsDBNull(reader.GetOrdinal("question_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                            questionText = reader.GetValue(reader.GetOrdinal("question_text")).ToString(),
                            answertype = reader.GetValue(reader.GetOrdinal("answer_type")).ToString(),
                            qOrder = reader.IsDBNull(reader.GetOrdinal("q_order")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("q_order")).ToString()),
                            dateAdded = reader.IsDBNull(reader.GetOrdinal("dateadded")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("dateadded")).ToString())

                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return instructions;
            }
        }

        public dynamic AddFAQ(FAQsModel fAQsModel)
        {
            string sql = @"insert into q_faqs (question_id,question_text,question_answer,q_order,dateadded,last_update_date)values(@qId,@qText,@qAnswer,@qOrder,dbo.GetMTDate(),dbo.GetMTDate())";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@qId", fAQsModel.questionId);
                SqlComm.Parameters.AddWithValue("@qText", fAQsModel.questionText);
                SqlComm.Parameters.AddWithValue("@qAnswer", fAQsModel.questionAnswer);
                if (fAQsModel.qOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@qOrder", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@qOrder", fAQsModel.qOrder);
                }

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetFAQList(fAQsModel.questionId);
            }
        }

        public dynamic DeleteInstruction(InstructionModel instructionModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID, " +
                                            "(select question_text from q_instructions where ID=@ID), '**deleted**';" +
                                            "delete from q_instructions where id = @ID";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@ID", instructionModel.id);
                SqlComm.Parameters.AddWithValue("@QID", instructionModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetInstructionsList((int)instructionModel.questionId);
            }
        }

        public dynamic AddInstruction(InstructionModel instructionModel)
        {
            string sql = @"insert into q_instructions (question_id,question_text,answer_type,q_order,dateadded) values(@qId,@qText,@answerType,@qOrder,GetDate())";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                if (instructionModel.questionId == null)
                {
                    SqlComm.Parameters.AddWithValue("@qId", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@qId", instructionModel.questionId);
                }


                if (instructionModel.questionText == null)
                {
                    SqlComm.Parameters.AddWithValue("@qText", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@qText", instructionModel.questionText);
                }


                if (instructionModel.answertype == null)
                {
                    SqlComm.Parameters.AddWithValue("@answerType", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answerType", instructionModel.answertype);
                }


                if (instructionModel.qOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@qOrder", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@qOrder", instructionModel.qOrder);
                }

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetInstructionsList((int)instructionModel.questionId);
            }
        }

        public dynamic UpdateInstruction(InstructionModel instructionModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID, " +
                                            "(select question_text from q_instructions where ID=@ID), @question_text; " +
                                            "update q_instructions set dateadded=dbo.getMTDate(),question_text=@question_text, q_order=@q_order, answer_type=@answer_type where id =@ID;";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@QID", instructionModel.questionId);
                SqlComm.Parameters.AddWithValue("@ID", instructionModel.id);
                SqlComm.Parameters.AddWithValue("@username", userName);
                SqlComm.Parameters.AddWithValue("@question_text", instructionModel.questionText);
                SqlComm.Parameters.AddWithValue("@answer_type", instructionModel.answertype);
                if (instructionModel.qOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@q_order", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@q_order", instructionModel.qOrder);
                }

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetInstructionsList((int)instructionModel.questionId);
            }
        }


        public dynamic ChangeQuestionsOrder(QuestionOrdering questionOrderModels)
        {


            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                foreach (var item in questionOrderModels.questions)
                {
                    string sql = @"update questions set q_order = @q_order where id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    SqlComm.Parameters.AddWithValue("@q_order", item.questionOrder);
                    SqlComm.Parameters.AddWithValue("@id", item.questionId);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                ScorecardsInfo scorecardInfo = new ScorecardsInfo
                {
                    id = questionOrderModels.scorecardId
                };
                return GetQustionList(scorecardInfo);

            }
        }

        public dynamic CahangeDropDownItemsOrdering(OrderingDropDown ordering)
        {

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                foreach (var item in ordering.list)
                {
                    string sql = @"update dropDownItems set item_order = @item_order where id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    SqlComm.Parameters.AddWithValue("@item_order", item.order);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }

                return GetDropDownItemList(ordering.questionId);

            }
        }


        public dynamic ChangeAnswerCommentsOrdering(AnswerCommentsOrdering ordering)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                foreach (var item in ordering.list)
                {
                    string sql = @"update answer_comments set ac_order = @item_order where id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    SqlComm.Parameters.AddWithValue("@item_order", item.answerOrder);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }

                return GetAnswerComments(ordering.questionId);

            }

        }


        public dynamic GetAnswerListFull(int qId)
        {
            var sql = @"select id,question_id,answer_text,answer_points,isAutoFail,autoselect,right_answer,acp_required,answer_order,answer_active,cs_text_returned,cs_id_returned from question_answers where question_id = @qID order by  answer_order";
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<AnswersModel> answers = new List<AnswersModel>();
                SqlComm.Parameters.AddWithValue("@QID", qId);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        answers.Add(new AnswersModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            questionId = reader.IsDBNull(reader.GetOrdinal("question_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                            answer = reader.GetValue(reader.GetOrdinal("answer_text")).ToString(),
                            points = reader.IsDBNull(reader.GetOrdinal("answer_points")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("answer_points")).ToString()),
                            isAutoFail = reader.IsDBNull(reader.GetOrdinal("isAutoFail")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("isAutoFail")).ToString()),
                            autoSelect = reader.IsDBNull(reader.GetOrdinal("autoselect")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("autoselect")).ToString()),
                            rightAnswer = reader.IsDBNull(reader.GetOrdinal("right_answer")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("right_answer")).ToString()),
                            commentRequired = reader.IsDBNull(reader.GetOrdinal("acp_required")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("acp_required")).ToString()),
                            answerOrder = reader.IsDBNull(reader.GetOrdinal("answer_order")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("answer_order")).ToString()),
                            answerActive = reader.IsDBNull(reader.GetOrdinal("answer_active")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("answer_active")).ToString()),
                            csText = reader.GetValue(reader.GetOrdinal("cs_text_returned")).ToString(),
                            csId = reader.IsDBNull(reader.GetOrdinal("cs_id_returned")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("cs_id_returned")).ToString())

                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return answers;
            }
        }



        public dynamic AddAnswer(AnswersModel answersModel)
        {
            string sql = @"INSERT INTO [question_answers] ([question_id], [answer_text], [answer_points], [isAutoFail]) VALUES (@question_id, @answer_text, @answer_points, @isAutoFail)";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@question_id", answersModel.questionId);
                SqlComm.Parameters.AddWithValue("@answer_text", answersModel.answer);
                if (answersModel.points == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_points", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_points", answersModel.points);
                }
                if (answersModel.isAutoFail == null)
                {
                    SqlComm.Parameters.AddWithValue("@isAutoFail", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@isAutoFail", answersModel.isAutoFail);
                }

                sqlCon.Open();
                SqlComm.ExecuteNonQuery();

                return GetAnswerListFull((int)answersModel.questionId);
            }
        }


        public dynamic UpdateAnswer(AnswersModel answersModel)
        {
            string sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID," +
                                            "'Answer: ' + (select answer_text + '/' + convert(varchar(10),answer_points) from question_answers where ID=@ID)  , 'Answer: ' + @answer_text  + '/' + convert(varchar(10),@answer_points);" +
                                            "UPDATE [question_answers] SET autoselect=@autoselect, [question_id] = @question_id, [answer_text] = @answer_text," +
                                                    "[answer_points] = @answer_points, acp_required=@acp_required, [isAutoFail] = @isAutoFail," +
                                                    "right_answer=@right_answer, answer_order=@answer_order, answer_active=@answer_active,cs_text_returned=@cs_text_returned," +
                                                    "cs_id_returned=@cs_id_returned WHERE [id] = @ID";
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@ID", answersModel.id);
                SqlComm.Parameters.AddWithValue("@QID", answersModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                if (answersModel.questionId == null)
                {
                    SqlComm.Parameters.AddWithValue("@question_id", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@question_id", answersModel.questionId);
                }

                if (answersModel.answer == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_text", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_text", answersModel.answer);
                }



                if (answersModel.points == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_points", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_points", answersModel.points);
                }


                if (answersModel.isAutoFail == null)
                {
                    SqlComm.Parameters.AddWithValue("@isAutoFail", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@isAutoFail", answersModel.isAutoFail);
                }


                if (answersModel.answerOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_order", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_order", answersModel.answerOrder);
                }


                if (answersModel.rightAnswer == null)
                {
                    SqlComm.Parameters.AddWithValue("@right_answer", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@right_answer", answersModel.rightAnswer);
                }

                if (answersModel.commentRequired == null)
                {
                    SqlComm.Parameters.AddWithValue("@acp_required", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@acp_required", answersModel.commentRequired);
                }




                if (answersModel.autoSelect == null)
                {
                    SqlComm.Parameters.AddWithValue("@autoselect", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@autoselect", answersModel.autoSelect);
                }


                if (answersModel.answerActive == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_active", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_active", answersModel.answerActive);
                }


                if (answersModel.csText == null)
                {
                    SqlComm.Parameters.AddWithValue("@cs_text_returned", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@cs_text_returned", answersModel.csText);
                }

                if (answersModel.csId == null)
                {
                    SqlComm.Parameters.AddWithValue("@cs_id_returned", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@cs_id_returned", answersModel.csId);
                }


                try
                {
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                return GetAnswerListFull((int)answersModel.questionId);
            }
        }


        public dynamic DeleteAnswer(AnswersModel answersModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID," +
                                            "'Answer: ' + (select answer_text from question_answers where ID=@ID), 'Answer: ' + '**deleted**';" +
            "DELETE FROM [question_answers] WHERE [id] = @id; delete from answer_comments where question_id = @ID";

            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@ID", answersModel.id);
                SqlComm.Parameters.AddWithValue("@QID", answersModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                return GetAnswerListFull((int)answersModel.questionId);
            }
        }




        public dynamic ChangeFAQOrder(OrderingFAQs fAQsModels)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                foreach (var item in fAQsModels.list)
                {
                    string sql = @"update q_faqs set q_order = @item_order where question_id = @Qid and id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    if (item.qOrder == null)
                    {
                        SqlComm.Parameters.AddWithValue("@item_order", DBNull.Value);
                    }
                    else
                    {
                        SqlComm.Parameters.AddWithValue("@item_order", item.qOrder);
                    }

                    SqlComm.Parameters.AddWithValue("@Qid", item.questionId);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }

                return GetFAQList(fAQsModels.questionId);

            }
        }




        public dynamic GetAnswerComments(int qId)
        {

            var sql = @"SELECT * FROM [answer_comments] WHERE ([question_id] = @question_id) and ((answer_id in (select id from question_answers where question_id = @question_id)) or (answer_id is null)) order by ac_active desc,  convert(int, isnull(ac_order,99))";

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<AnswerCommentModel> answersComments = new List<AnswerCommentModel>();
                SqlComm.Parameters.AddWithValue("@question_id", qId);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        answersComments.Add(new AnswerCommentModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            comment = reader.GetValue(reader.GetOrdinal("comment")).ToString(),
                            questionId = reader.IsDBNull(reader.GetOrdinal("question_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString()),
                            answerId = reader.IsDBNull(reader.GetOrdinal("answer_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("answer_id")).ToString()),
                            answerPoints = reader.IsDBNull(reader.GetOrdinal("comment_points")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("comment_points")).ToString()),
                            fixedPos = reader.IsDBNull(reader.GetOrdinal("univeral_postition")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("univeral_postition")).ToString()),
                            answerOrder = reader.IsDBNull(reader.GetOrdinal("ac_order")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("ac_order")).ToString()),
                            active = reader.IsDBNull(reader.GetOrdinal("ac_active")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("ac_active")).ToString()),
                            csText = reader.GetValue(reader.GetOrdinal("cs_text_returned")).ToString(),
                            csId = reader.IsDBNull(reader.GetOrdinal("cs_id_returned")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("cs_id_returned")).ToString()),
                            answerStatement = reader.GetValue(reader.GetOrdinal("answer_statement")).ToString()

                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return answersComments;
            }

        }


        public dynamic UpdateAnswerComment(AnswerCommentModel answerCommentModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID," +
                                            "'Answer Comment: ' + (select comment from answer_comments where ID=@ID), 'Answer Comment: ' + @comment;" +
                                            "UPDATE [answer_comments] SET comment_points=@comment_points, ac_order=@ac_order, ac_active=@ac_active,  [comment] = @comment, " +
                                            "[answer_id] = @answer_id, univeral_postition=@univeral_postition, cs_text_returned=@cs_text_returned, cs_id_returned=@cs_id_returned," +
                                            "answer_statement=@answer_statement WHERE [id] = @ID";
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@QID", answerCommentModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                SqlComm.Parameters.AddWithValue("@ID", answerCommentModel.id);
                SqlComm.Parameters.AddWithValue("@comment", answerCommentModel.comment);


                if (answerCommentModel.answerPoints == null)
                {
                    SqlComm.Parameters.AddWithValue("@comment_points", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@comment_points", answerCommentModel.answerPoints);
                }

                if (answerCommentModel.answerOrder == null)
                {
                    SqlComm.Parameters.AddWithValue("@ac_order", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@ac_order", answerCommentModel.answerOrder);
                }


                if (answerCommentModel.active == null)
                {
                    SqlComm.Parameters.AddWithValue("@ac_active", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@ac_active", answerCommentModel.active);
                }


                if (answerCommentModel.answerId == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_id", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_id", answerCommentModel.answerId);
                }


                if (answerCommentModel.fixedPos == null)
                {
                    SqlComm.Parameters.AddWithValue("@univeral_postition", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@univeral_postition", answerCommentModel.fixedPos);
                }

                SqlComm.Parameters.AddWithValue("@cs_text_returned", answerCommentModel.csText);

                if (answerCommentModel.csId == null)
                {
                    SqlComm.Parameters.AddWithValue("@cs_id_returned", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@cs_id_returned", answerCommentModel.csId);
                }


                if (answerCommentModel.answerStatement == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_statement", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_statement", answerCommentModel.answerStatement);
                }

                try
                {
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return GetAnswerComments((int)answerCommentModel.questionId);

            }
        }

        public dynamic AddAnswerComment(AnswerCommentModel answerCommentModel)
        {
            var sql = @"INSERT INTO [answer_comments] ([comment], [answer_id],[question_id]) VALUES (@comment, @answer_id,@questionId)";
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };


                SqlComm.Parameters.AddWithValue("@comment", answerCommentModel.comment);

                if (answerCommentModel.answerId == null)
                {
                    SqlComm.Parameters.AddWithValue("@answer_id", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@answer_id", answerCommentModel.answerId);
                }

                if (answerCommentModel.questionId == null)
                {
                    SqlComm.Parameters.AddWithValue("@questionId", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@questionId", answerCommentModel.questionId);
                }


                try
                {
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return GetAnswerComments((int)answerCommentModel.questionId);

            }
        }



        public dynamic DeleteAnswerComments(AnswerCommentModel answerCommentModel)
        {
            var sql = @"insert into guideline_updates (updated_by, updated_date, question_id, from_text, to_text) select @username, dbo.getMTDate(), @QID," +
                                            "'Answer Comment: ' + (select comment from answer_comments where ID=@ID), 'Answer Comment: ' + '**deleted**'; " +
                                            "DELETE FROM [answer_comments] WHERE [id] = @ID";
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@QID", answerCommentModel.questionId);
                SqlComm.Parameters.AddWithValue("@username", userName);
                SqlComm.Parameters.AddWithValue("@ID", answerCommentModel.id);

                try
                {
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return GetAnswerComments((int)answerCommentModel.questionId);

            }
        }

        public dynamic GetHistoryList(int qId)
        {
            var sql = @"select * from guideline_updates where question_id = @QID order by updated_date desc";

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                List<GuidelineHistoryModel> history = new List<GuidelineHistoryModel>();
                SqlComm.Parameters.AddWithValue("@QID", qId);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        history.Add(new GuidelineHistoryModel
                        {
                            id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            updatedBy = reader.GetValue(reader.GetOrdinal("updated_by")).ToString(),
                            updatedDate = reader.IsDBNull(reader.GetOrdinal("updated_date")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("updated_date")).ToString()),
                            questionId = reader.IsDBNull(reader.GetOrdinal("updated_date")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                            fromText = reader.GetValue(reader.GetOrdinal("from_text")).ToString(),
                            toText = reader.GetValue(reader.GetOrdinal("to_text")).ToString(),
                            fromAnswer = reader.GetValue(reader.GetOrdinal("from_answer")).ToString(),
                            toAnswer = reader.GetValue(reader.GetOrdinal("to_answer")).ToString(),

                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return history;
            }
        }



        public dynamic ChangeAnswerOrder(AnswerOrdering answerOrdering)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                foreach (var item in answerOrdering.list)
                {
                    string sql = @"update question_answers set answer_order = @item_order where question_id = @Qid and id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    if (item.answerOrder == null)
                    {
                        SqlComm.Parameters.AddWithValue("@item_order", DBNull.Value);
                    }
                    else
                    {
                        SqlComm.Parameters.AddWithValue("@item_order", item.answerOrder);
                    }
                    SqlComm.Parameters.AddWithValue("@Qid", item.questionId);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                return GetAnswerListFull(answerOrdering.questionId);
            }
        }


        public dynamic ChangeIstructionOrder(InsructionsOrdering ordering)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                foreach (var item in ordering.list)
                {
                    string sql = @"update q_instructions set q_order = @item_order where question_id = @Qid and id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };
                    SqlComm.Parameters.AddWithValue("@item_order", item.qOrder == null ? (object)DBNull.Value : item.qOrder);
                    SqlComm.Parameters.AddWithValue("@Qid", item.questionId);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                return GetInstructionsList(ordering.questionId);
            }
        }

        public dynamic MultipleCloneQuestionsToScorecard(List<CloneModel> multipleCloneModel)
        {
            foreach (var item in multipleCloneModel)
            {

                CloneQuestionToScorecard(item);
            }
            return GetQustionList(new ScorecardsInfo() { id = multipleCloneModel[0].Scorecards.id });
        }

        public dynamic CloneQuestion(QuestionModel model)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string command = @"[cloneQuestion]";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = command,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@QID", model.id);
                SqlComm.Parameters.AddWithValue("@deactivate", 0);
                
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetQustionList(new ScorecardsInfo() { id = model.scorecard_id });
            }
        }



        public dynamic GetAppScorecardList(Search search = null)
        {
            var userName = HttpContext.Current.GetUserName();
            List<AppsListModel> response = new List<AppsListModel>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = "[GetAppsWithScorecards]",
                    Connection = sqlCon
                };
                if (search?.columns != null && search.text != null && search.text != "" && search.columns.Count > 0)
                {
                    var preparedLst = new StringBuilder();

                    SqlComm.Parameters.AddWithValue("@searchstr", search.text);
                    foreach (var value in search.columns)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    SqlComm.Parameters.AddWithValue("@searchColumn", preparedLst.ToString());
                }
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                try
                {
                    response = AppsListModel.Create(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                return response;
            }
        }




        public dynamic ChangeSectionOrder(SectionOrderingModel ordering)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                foreach (var item in ordering.list)
                {
                    string sql = @"update sections set section_order = @item_order where id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };

                    SqlComm.Parameters.AddWithValue("@item_order", item.section_order == null ? (object)DBNull.Value : item.section_order);


                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                return GetQuestionSection(new ScorecardsInfo() { id = ordering.scorecard });

            }
        }


        public void AddNewSection(SectionModel section)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"insert into sections (section,section_order,appname,scorecard_id)" +
                               "values(@section,@section_order,@appname,@scorecard_id)";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@section", section.section);
                SqlComm.Parameters.AddWithValue("@section_order", section.section_order ?? (object)DBNull.Value);
                SqlComm.Parameters.AddWithValue("@appname", section.appname);
                SqlComm.Parameters.AddWithValue("@scorecard_id", section.scorecard_id ?? (object)DBNull.Value);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
            }
            //return GetQuestionSection(new ScorecardsInfo { id = section.id});
        }


        public void UpdateTemplateItem(TemplateItemModel template)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = "update question_options"
                            + " set"
                            + " option_text = @optionText,"
                            + " option_order = @option_order" +
                            " where id = @id and question_id = @qID ";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@qID", template.qID == null ? (object)DBNull.Value : template.qID);
                SqlComm.Parameters.AddWithValue("@id", template.id);
                SqlComm.Parameters.AddWithValue("@option_order", template.optionOrder == null ? (object)DBNull.Value : template.optionOrder);
                SqlComm.Parameters.AddWithValue("@optionText", template.optionText);
                sqlCon.Open();
                try
                {
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void DeleteTemplateItem(TemplateItemModel template)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = "delete from question_options where id = @id and question_id = @question_id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@question_id", template.qID);
                SqlComm.Parameters.AddWithValue("@id", template.id);
                sqlCon.Open();
                try
                {
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void AddTemplateItem(TemplateItemModel template)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = "INSERT INTO [question_options] ([question_id], [date_added], [date_start],[option_text],[option_order])" +
                                            "VALUES (@question_id, dbo.getMTdate(), dbo.getMTdate(),@optionText,@optionOrder)";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@question_id", template.qID);
                SqlComm.Parameters.AddWithValue("@optionText", template.optionText);
                SqlComm.Parameters.AddWithValue("@optionOrder", template.optionOrder);
                sqlCon.Open();
                try
                {
                    SqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public dynamic GetTemplateItems(int qId)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sql = "SELECT * FROM [question_options] WHERE ([question_id] = @question_id) order by isnull(option_order,999)";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@question_id", qId);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                var response = TemplateItemModel.Create(reader);
                return response;
            }
        }

        public dynamic ChangeTemplateItemOrder(TemlateItemOrdering ordering)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                foreach (var item in ordering.list)
                {
                    string sql = @"update question_options set option_order = @item_order where id = @id ";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };

                    SqlComm.Parameters.AddWithValue("@item_order", item.optionOrder == null ? (object)DBNull.Value : item.optionOrder);
                    SqlComm.Parameters.AddWithValue("@id", item.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                }
                return GetTemplateItems(ordering.questionId);

            }
        }

        public dynamic GetRuleItems(int qcID)
        {
            string sql = @"select id,rule_type as ruleType,rule_source as ruleSource,rule_operator as ruleOperator,rule_value as ruleValue,qc_id as qcId from question_rules where qc_id =" + qcID;
            var mapper = DapperHelper.GetList<RuleItemModel>(sql).ToList();
            return mapper;
        }

        public dynamic GetDynamicRuleItems(int qcID)
        {
            string sql = @"select id,rule_type as ruleType,rule_source as ruleSource,rule_operator as ruleOperator,rule_value as ruleValue,qc_id as qId from dynamic_question_rules where qc_id =" + qcID;
            var mapper = DapperHelper.GetList<RuleItemModel>(sql).ToList();
            return mapper;
        }

        public dynamic GetRulesCalc(int qID)
        {
            string sql = @"select id,qid as qID,rule_description as description,rule_active as active,q_answer as questionAnswerId,old_QID as oldQid,old_id as oldId from question_calcs where qid =" + qID;
            var mapper = DapperHelper.GetList<QuestionCalc>(sql).ToList();
            return mapper;
        }

        public dynamic DeleteRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
               
                    string sql = @"delete from question_rules where id = @id";
                    SqlCommand SqlComm = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60,
                        CommandText = sql,
                        Connection = sqlCon
                    };

                    
                    SqlComm.Parameters.AddWithValue("@id", calculatedRuleModel.id);
                    sqlCon.Open();
                    SqlComm.ExecuteNonQuery();
                    sqlCon.Close();
                
                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic UpdateRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string sql = @"update question_rules set rule_type=@rule_type, rule_source=@rule_source, rule_operator=@rule_operator, rule_value=@rule_value where id=@id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@rule_type", calculatedRuleModel.ruleType);
                SqlComm.Parameters.AddWithValue("@rule_source", calculatedRuleModel.ruleSource);
                SqlComm.Parameters.AddWithValue("@rule_operator", calculatedRuleModel.ruleOperator);
                SqlComm.Parameters.AddWithValue("@rule_value", calculatedRuleModel.ruleValue);
                SqlComm.Parameters.AddWithValue("@id", calculatedRuleModel.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic AddRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string sql = @"insert into question_rules (rule_type,rule_source,rule_operator,rule_value,qc_id) values(@rule_type,@rule_source,@rule_operator,@rule_value,@qc_id);";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@rule_type", calculatedRuleModel.ruleType);
                SqlComm.Parameters.AddWithValue("@rule_source", calculatedRuleModel.ruleSource);
                SqlComm.Parameters.AddWithValue("@rule_operator", calculatedRuleModel.ruleOperator);
                SqlComm.Parameters.AddWithValue("@rule_value", calculatedRuleModel.ruleValue);
                SqlComm.Parameters.AddWithValue("@qc_id", calculatedRuleModel.qcId == null ? (int?)null : calculatedRuleModel.qcId);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic GetCalculatedRules(int qID)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"GetCalculatedRules";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@qID", qID);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                var response = RuleModel.Create(reader);
                return response;
            }
        }

        public dynamic AddCalculatedRule(QuestionCalc questionCalc)
        {
            var userName = HttpContext.Current.GetUserName();
            int qcID = 0;
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"insert into question_calcs (qid,rule_description,rule_active,q_answer,old_QID,old_id) OUTPUT Inserted.ID " +
                    " values" +
                    "(" +
                    "@qid," +
                    "@rule_description," +
                    "@rule_active," +
                    "@q_answer," +
                    "@old_QID," +
                    "@old_id" +
                    ")";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@qid", questionCalc.qID == null ? (object)DBNull.Value : questionCalc.qID);
                SqlComm.Parameters.AddWithValue("@rule_description", questionCalc.description == null ? (object)DBNull.Value : questionCalc.description);
                SqlComm.Parameters.AddWithValue("@rule_active", questionCalc.active == null ? (object)DBNull.Value : questionCalc.active);
                SqlComm.Parameters.AddWithValue("@q_answer", questionCalc.questionAnswerId == null ? (object)DBNull.Value : questionCalc.questionAnswerId);
                SqlComm.Parameters.AddWithValue("@old_QID", questionCalc.oldQid == null ? (object)DBNull.Value : questionCalc.oldQid);
                SqlComm.Parameters.AddWithValue("@old_id", questionCalc.oldId == null ? (object)DBNull.Value : questionCalc.oldId);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                while (reader.Read())
                {
                    qcID = int.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString());
                }
                sqlCon.Close();
                return new RuleModel() { rule = GetCalculatedRuleItemById(qcID), ruleItems = new List<RuleItemModel>() };
            }
        }

        public dynamic GetCalculatedRuleItemById(int id)
        {
            string sql = @"select id,qid as qID,rule_description as description,rule_active as active,q_answer as questionAnswerId,old_QID as oldQid,old_id as oldId from question_calcs where id =" + id;
            var mapper = DapperHelper.GetSingle<QuestionCalc>(sql);
            return mapper;
        }

        public dynamic AddMultipleRuleItems(List<RuleItemModel> ruleItemModels)
        {
            foreach (var item in ruleItemModels)
            {
                AddRuleItem(item);
            }
            return GetCalculatedRules((int)ruleItemModels[0].qID);
        }

        public dynamic AddMultipleRuleItemsSimple(List<RuleItemModel> ruleItemModels)
        {
            foreach (var item in ruleItemModels)
            {
                AddRuleItem(item);
            }
            return GetRuleItems((int)ruleItemModels[0].qcId);
        }

        public dynamic UpdateCalculatedRule(QuestionCalc questionCalc)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"update question_calcs set " +
                    " qid = @qid," +
                    "rule_description = @rule_description," +
                    "rule_active = @rule_active," +
                    "q_answer = @q_answer," +
                    "old_QID = @old_QID," +
                    "old_id = @old_id " +
                    " where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@qid", questionCalc.qID == null ? (object)DBNull.Value : questionCalc.qID);
                SqlComm.Parameters.AddWithValue("@rule_description", questionCalc.description == null ? (object)DBNull.Value : questionCalc.description);
                SqlComm.Parameters.AddWithValue("@rule_active", questionCalc.active == null ? (object)DBNull.Value : questionCalc.active);
                SqlComm.Parameters.AddWithValue("@q_answer", questionCalc.questionAnswerId == null ? (object)DBNull.Value : questionCalc.questionAnswerId);
                SqlComm.Parameters.AddWithValue("@old_QID", questionCalc.oldQid == null ? (object)DBNull.Value : questionCalc.oldQid);
                SqlComm.Parameters.AddWithValue("@old_id", questionCalc.oldId == null ? (object)DBNull.Value : questionCalc.oldId);
                SqlComm.Parameters.AddWithValue("@id", questionCalc.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetCalculatedRules((int)questionCalc.qID);
            }
        }


        public dynamic DeleteCalculatedRule(RuleModel questionCalc)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"delete from question_calcs where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@id", questionCalc.rule.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                foreach (var item in questionCalc.ruleItems)
                {
                    DeleteRuleItem(item);
                }
                return GetCalculatedRules((int)questionCalc.rule.qID);
                
            }
        }



        public dynamic GetDynamicRules(int qID)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"GetDynamicRules";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@qID", qID);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                var response = RuleModel.Create(reader);
                return response;
            }
        }



        public dynamic DeleteDynamicRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string sql = @"delete from dynamic_question_rules where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };


                SqlComm.Parameters.AddWithValue("@id", calculatedRuleModel.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic UpdateDynamicRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string sql = @"update dynamic_question_rules set rule_type=@rule_type, rule_source=@rule_source, rule_operator=@rule_operator, rule_value=@rule_value where id=@id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@rule_type", calculatedRuleModel.ruleType);
                SqlComm.Parameters.AddWithValue("@rule_source", calculatedRuleModel.ruleSource);
                SqlComm.Parameters.AddWithValue("@rule_operator", calculatedRuleModel.ruleOperator);
                SqlComm.Parameters.AddWithValue("@rule_value", calculatedRuleModel.ruleValue);
                SqlComm.Parameters.AddWithValue("@id", calculatedRuleModel.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic AddDynamicRuleItem(RuleItemModel calculatedRuleModel)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                string sql = @"insert into dynamic_question_rules (rule_type,rule_source,rule_operator,rule_value,qc_id) values(@rule_type,@rule_source,@rule_operator,@rule_value,@qc_id);";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                SqlComm.Parameters.AddWithValue("@rule_type", calculatedRuleModel.ruleType);
                SqlComm.Parameters.AddWithValue("@rule_source", calculatedRuleModel.ruleSource);
                SqlComm.Parameters.AddWithValue("@rule_operator", calculatedRuleModel.ruleOperator);
                SqlComm.Parameters.AddWithValue("@rule_value", calculatedRuleModel.ruleValue);
                SqlComm.Parameters.AddWithValue("@qc_id", calculatedRuleModel.qcId == null ? (int?)null : calculatedRuleModel.qcId);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();

                return GetRuleItems((int)calculatedRuleModel.qcId);

            }
        }


        public dynamic GetLinkedItems(int qID)
        {
            var sql = @"select id, linked_parent_question as linkedParentQuestion,linked_to_question as linkedToQuestion,linked_type as linkedType,linked_item_id as linkedItemId,initially_visible as initialyVisible from linked_items where linked_parent_question ="+qID;
            var mapper = DapperHelper.GetList<LinkedItemModel>(sql);
            return mapper;
        }
        public dynamic GetLinkedItem(int id)
        {
            var sql = @"select id, linked_parent_question as linkedParentQuestion,linked_to_question as linkedToQuestion,linked_type as linkedType,linked_item_id as linkedItemId,initially_visible as initialyVisible from linked_items where id =" + id;
            var mapper = DapperHelper.GetSingle<LinkedItemModel>(sql);
            return mapper;
        }

        public dynamic AddLinkedItem(LinkedItemModel linkedItem)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"insert into linked_items (linked_parent_question,linked_to_question,linked_type,linked_item_id,initially_visible) values(@linked_parent_question,@linked_to_question,@linked_type,@linked_item_id,@initially_visible);";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@linked_parent_question", linkedItem.linkedParentQuestion == null ? (object)DBNull.Value : linkedItem.linkedParentQuestion);
                SqlComm.Parameters.AddWithValue("@linked_to_question", linkedItem.linkedToQuestion == null ? (object)DBNull.Value : linkedItem.linkedToQuestion);
                SqlComm.Parameters.AddWithValue("@linked_type", linkedItem.linkedType == null ? (object)DBNull.Value : linkedItem.linkedType);
                SqlComm.Parameters.AddWithValue("@linked_item_id", linkedItem.linkedItemId == null ? (object)DBNull.Value : linkedItem.linkedItemId);
                SqlComm.Parameters.AddWithValue("@initially_visible", linkedItem.initialyVisible == null ? (object)DBNull.Value : linkedItem.initialyVisible);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetLinkedItems((int)linkedItem.linkedParentQuestion);
            }
        }

        public dynamic UpdateLinkedItem(LinkedItemModel linkedItem)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var oldVariance = (LinkedItemModel)GetLinkedItem(linkedItem.id);
                var variances = linkedItem.DetailedCompare(oldVariance);
                if (variances.Count != 0)
                {
                    try
                    {
                        StringBuilder insertChanges = new StringBuilder();

                        int count = 0;
                        foreach (var item in variances)
                        {

                           
                                count++;
                                if (count == 1)
                                {
                                    insertChanges.Append(@"insert into UpdateHistorySettings(Property,FromValue,ToValue,DateUpdated,dynamic_id,UpdatedBy,UpdateItem)values ");
                                }
                                insertChanges.Append("("
                               + "'"
                               + item.Prop + "',"
                               + "'" + item.oldValue + "',"
                               + "'" + item.newValue + "',"
                               + "dbo.GetMTDate(),"
                               + linkedItem.id+ "," +
                               "'"+userName + "'," +
                               "'LinkedItem'),"
                               );
                            //UpdateHistorySettings
                            //table  UpdateItemsList for UpdateItem

                        }
                        if (insertChanges.ToString() != string.Empty)
                        {
                            insertChanges.Remove(insertChanges.Length - 1, 1);
                        }

                        SqlCommand SqlCo = new SqlCommand()
                        {
                            CommandTimeout = 60,
                            CommandType = CommandType.Text
                        };
                        SqlCo.CommandText = insertChanges.ToString();
                        SqlCo.Connection = sqlCon;

                        if (insertChanges.ToString() != string.Empty)
                        {
                            sqlCon.Open();
                            SqlCo.ExecuteNonQuery();
                            sqlCon.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }

                string sql = @"update linked_items set "+
                               " linked_parent_question = @linked_parent_question," +
                               "linked_to_question = @linked_to_question," +
                               "linked_type = @linked_type," +
                               "linked_item_id = @linked_item_id," +
                               "initially_visible = @initially_visible " +
                               " where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@linked_parent_question", linkedItem.linkedParentQuestion == null ? (object)DBNull.Value : linkedItem.linkedParentQuestion);
                SqlComm.Parameters.AddWithValue("@linked_to_question", linkedItem.linkedToQuestion == null ? (object)DBNull.Value : linkedItem.linkedToQuestion);
                SqlComm.Parameters.AddWithValue("@linked_type", linkedItem.linkedType == null ? (object)DBNull.Value : linkedItem.linkedType);
                SqlComm.Parameters.AddWithValue("@linked_item_id", linkedItem.linkedItemId == null ? (object)DBNull.Value : linkedItem.linkedItemId);
                SqlComm.Parameters.AddWithValue("@initially_visible", linkedItem.initialyVisible == null ? (object)DBNull.Value : linkedItem.initialyVisible);
                SqlComm.Parameters.AddWithValue("@id",linkedItem.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetLinkedItems((int)linkedItem.linkedParentQuestion);
            }
        }


        public dynamic DeleteLinkedItem(LinkedItemModel linkedItem)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"delete from linked_items where id = @id";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@id", linkedItem.id);
                sqlCon.Open();
                SqlComm.ExecuteNonQuery();
                sqlCon.Close();
                return GetLinkedItems((int)linkedItem.linkedParentQuestion);
            }
        }

        public dynamic GetSchoolDataItems()
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"SELECT distinct name FROM sys.columns c WHERE c.object_id = OBJECT_ID('school_x_data') order by name";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                List<string> items = new List<string>();
                while (reader.Read())
                {
                    items.Add(reader.GetValue(reader.GetOrdinal("name")).ToString());
                }


                return items;
            }
        }

        public dynamic GetCallDataItems()
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"SELECT distinct name FROM sys.columns c WHERE c.object_id = OBJECT_ID('xcc_report_new') order by name";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                List<string> items = new List<string>();
                while (reader.Read())
                {
                    items.Add(reader.GetValue(reader.GetOrdinal("name")).ToString());
                }


                return items;
            }
        }


        public dynamic GetOtherDataItems()
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"SELECT distinct name FROM sys.columns c where c.object_id  = OBJECT_ID('otherFormData') order by name";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };

                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                List<string> items = new List<string>();
                while (reader.Read())
                {
                    items.Add(reader.GetValue(reader.GetOrdinal("name")).ToString());
                }


                return items;
            }
        }


        public dynamic GetQuestionsForCalculatedRules(int scID)
        {
            var sql = @"select id,q_short_name as questionName from questions where scorecard_id =" + scID+ " and q_type != 'Calculated'";
            var mapper = DapperHelper.GetList<QuestiomModelSimple>(sql);
            return mapper;
        }

        public dynamic GetQuestionsWithAnswers(int scID)
        {
            var userName = HttpContext.Current.GetUserName();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string sql = @"[GetQuestionWithAnswersForCalculatedRules]";
                SqlCommand SqlComm = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    CommandText = sql,
                    Connection = sqlCon
                };
                SqlComm.Parameters.AddWithValue("@sc", scID);
                sqlCon.Open();
                var reader = SqlComm.ExecuteReader();
                List<QuestionInfoForCalcRules> questions = new List<QuestionInfoForCalcRules>();
                List<DAL.Models.SettingsModels.AnswerInfo> answers = new List<Models.SettingsModels.AnswerInfo>();
                List<QuestionsWithAnswerListModel> respose = new List<QuestionsWithAnswerListModel>();
                List<CommentInfoModel> comments = new List<CommentInfoModel>();
                while (reader.Read())
                {
                    questions.Add(new QuestionInfoForCalcRules()
                    {
                        questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                        questionName = reader.GetValue(reader.GetOrdinal("questionName")).ToString()
                    });
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        answers.Add(new Models.SettingsModels.AnswerInfo()
                        {
                            answerId = int.Parse(reader.GetValue(reader.GetOrdinal("answerId")).ToString()),
                            answerText = reader.GetValue(reader.GetOrdinal("answerText")).ToString(),
                            questionIdFromAnswer = int.Parse(reader.GetValue(reader.GetOrdinal("questionIdFromAnswer")).ToString())
                        });
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        comments.Add(new Models.SettingsModels.CommentInfoModel()
                        {
                            commentId = int.Parse(reader.GetValue(reader.GetOrdinal("commentId")).ToString()),
                            comment = reader.GetValue(reader.GetOrdinal("comment")).ToString(),
                            questionIdFromComment = int.Parse(reader.GetValue(reader.GetOrdinal("questionIdFromComment")).ToString())
                        });
                    }
                }


                for (int i = 0; i < questions.Count; i++)
                {
                    respose.Add(new QuestionsWithAnswerListModel()
                    {
                        question = new QuestionInfoForCalcRules()
                        {
                            questionId = questions[i].questionId,
                            questionName = questions[i].questionName
                        },
                        answers = new List<Models.SettingsModels.AnswerInfo>(),
                        comments = new List<CommentInfoModel>()
                    });
                }

                foreach (var item in respose)
                {
                    foreach (var q in answers)
                    {
                        if(q.questionIdFromAnswer == item.question.questionId)
                        {
                            item.answers.Add(q);
                        }
                    }
                }

                foreach (var item in respose)
                {
                    foreach (var c in comments)
                    {
                        if (c.questionIdFromComment == item.question.questionId)
                        {
                            item.comments.Add(c);
                        }
                    }
                }

                return respose;
            }
        }






    }
}

