using DAL.Layers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using DAL.Models;
using System.Web.Script.Serialization;
namespace DAL.DataLayer
{
    public class UserLayer
    {


        public async Task<AllUserSettings> GetAllUserSettings(string userName, string previousUser)
        {



            AllUserSettings allUserSettings = new AllUserSettings();

            SettingsLayer settings = new SettingsLayer();

            UserLayer userLayer = new UserLayer();
            //  var //stopwatch0 = new //stopwatch();
            //  var //stopwatch = new //stopwatch();
            //  var //stopwatch1 = new //stopwatch();
            //  var //stopwatch2 = new //stopwatch();
            //  //stopwatch0.Start();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                sqlCon.Open();

                //stopwatch.Start();

                allUserSettings.session = await userLayer.GetSessionAsync(userName, previousUser, sqlCon);

                //stopwatch.Stop();


                //stopwatch1.Start();
                allUserSettings.columns = await settings.GetUserCollums(userName, sqlCon);


                //stopwatch1.Stop();


                //stopwatch2.Start();
                allUserSettings.settings = await GetSavedUserSettings(new List<string>(), userName, sqlCon);

                //stopwatch2.Stop();


            }
            //stopwatch0.Stop();
            if (previousUser == null || previousUser == "")
            {
               await Task.Run(() =>
                 {
                     try
                     {
                         allUserSettings.settings = allUserSettings.settings.FindAll(a => a.name != "allUserSettings");
                         SavedUserSettings setting = new SavedUserSettings() { name = "allUserSettings", value = new JavaScriptSerializer() { MaxJsonLength = 16777216 }.Serialize(allUserSettings) };
                         SaveUserSettings(setting, userName);
                     }
                     catch
                     {

                     }
                 });
            }
            return allUserSettings;
        }


        /// <summary>
        /// GetSavedUserSettings
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public async Task<List<SavedUserSettings>> GetSavedUserSettings(List<string> names, string userName, SqlConnection sqlCon = null)
        {
            using (sqlCon)
            {
                if (sqlCon == null)
                {
#pragma warning disable CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement
                    sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
#pragma warning restore CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement
                    sqlCon.Open();
                }

                SqlCommand sqlComm = new SqlCommand
                {
                    Connection = sqlCon,

                    CommandText = "GetUserSettings",
                    CommandType = CommandType.StoredProcedure
                };

                sqlComm.Parameters.AddWithValue("@username", userName);

                if (names != null && names.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var val in names)
                    {
                        preparedLst.Append("'" + val + "',");
                    }

                    sqlComm.Parameters.AddWithValue("@Values", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Values", DBNull.Value);
                }

                var lst = new List<SavedUserSettings>();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    while (reader.Read())
                    {
                        lst.Add(new SavedUserSettings
                        {
                            name = reader.GetValue(reader.GetOrdinal("Name")).ToString(),
                            value = reader.GetValue(reader.GetOrdinal("Value")).ToString()
                        });
                    }

                    return await Task.Run(() => { return lst; });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        /// <summary>
        /// SaveUserSettings
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public string SaveUserSettings(SavedUserSettings setting, string userName = "")
        {
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager
                .ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                if (userName == "")
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var sqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "[SaveUserSettings]",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                sqlComm.Parameters.AddWithValue("@Value", setting.value);
                sqlComm.Parameters.AddWithValue("@Name", setting.name);

                sqlCon.Open();
                try
                {
                    if ((setting.name != string.Empty))
                    {
                        sqlComm.ExecuteReader();
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }

            return "Success";
        }

        /// <summary>
        /// GetSession
        /// </summary>
        /// <returns></returns>
        public async Task<UserObject> GetSessionAsync(string userName, string previousUser, SqlConnection sqlCon = null)
        {
            UserObject uo = new UserObject();


            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
                sqlCon.Open();
            }



            //var //stopwatch3 = new //stopwatch();
            //var //stopwatch2 = new //stopwatch();
            //var //stopwatch1 = new //stopwatch();
            //var //stopwatch = new //stopwatch();
            //var //stopwatch0 = new //stopwatch();
            ////stopwatch0.Start();
            if (userName != "")
            {
                ////stopwatch.Start();

                uo = await GetUserInfo(userName, sqlCon);
                uo.PreviousUser = previousUser;
                //stopwatch.Stop();
                //stopwatch1.Start();

                uo.UserLinks = await GetMenu(userName, sqlCon);
                //stopwatch1.Stop();

                //stopwatch2.Start();

                var allUserModules = await new SettingsLayer().GetModuleList(userName, sqlCon);
                //stopwatch2.Stop();

                uo.modules = allUserModules.userModuleConfigurable;
                uo.modulesRequired = allUserModules.userModuleAvailable;

                //stopwatch3.Start();


                var sclinks = await GetSClist(userName, sqlCon);
                //stopwatch3.Stop();

                sclinks = sclinks.OrderBy(x => x.scorcard_name).Select(x => x).ToList();
                var groupApps = (from a in sclinks group a by a.scorecard_appname into g select new Apps() { appname = g.Key, scorecards = g.Select(s => s).ToList() }).ToList();
                //groupApps = groupApps.OrderBy(x => x.scorecards == x.scorecards).Select(x=>x).ToList();
                uo.apps = groupApps;
                uo.scorecards = sclinks;


                uo.useNewDashboard = true;
            }
            //stopwatch0.Stop();

            return uo;

        }

        public async Task<UserObject> GetSession()
        {
            UserObject uo = new UserObject();
            string username = HttpContext.Current.User.Identity.Name;
            if (username == string.Empty)
            {
                username = "test321";
            }

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager
                .ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                cn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable dt = new DataTable();

                SqlCommand reply =
                    new SqlCommand(
                        "select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username = @username",
                        cn);

                reply.Parameters.AddWithValue("username", username);


                if (username != "")
                {

                    adapter.SelectCommand = reply;

                    adapter.Fill(dt);

                    uo.UserRole = dt.Rows[0]["user_role"].ToString();
                    uo.UserName = dt.Rows[0]["username"].ToString();
                    uo.SpeedInc = dt.Rows[0]["speed_increment"].ToString();
                    uo.StartImmediately = bool.Parse(dt.Rows[0]["calls_start_immediately"].ToString());


                    SqlCommand getMyMenuSqlComm = new SqlCommand();

                    cn.Close();
                    cn.Open();
                    getMyMenuSqlComm.Connection = cn;
                    getMyMenuSqlComm.CommandText = "[getMyMenu]";
                    getMyMenuSqlComm.CommandType = CommandType.StoredProcedure;
                    getMyMenuSqlComm.Parameters.AddWithValue("@userName", username);


                    SqlDataReader reader = getMyMenuSqlComm.ExecuteReader();
                    List<LinkList> links = new List<LinkList>();
                    while (reader.Read())
                    {
                        links.Add(new LinkList()
                        {
                            LinkText = reader.GetValue(reader.GetOrdinal("link")).ToString(),
                            LinkURL = reader.GetValue(reader.GetOrdinal("url")).ToString(),
                            LinkSpan = reader.GetValue(reader.GetOrdinal("span_data")).ToString()
                        });
                    }

                    uo.UserLinks = links;
                    SqlCommand getBadgessqlComm = new SqlCommand
                    {
                        Connection = cn,
                        CommandText = "[getBadges]",
                        CommandType = CommandType.StoredProcedure
                    };
                    getBadgessqlComm.Parameters.AddWithValue("@userName", username);


                    var getBadgesreader = getBadgessqlComm.ExecuteReader();
                    var agenda = "";
                    var guidelines = "";
                    var updateItems = "";
                    while (getBadgesreader.Read())
                    {
                        agenda = getBadgesreader[getBadgesreader.GetOrdinal("agenda")].ToString();
                        guidelines = getBadgesreader[getBadgesreader.GetOrdinal("guidelines")].ToString();
                        updateItems = getBadgesreader[getBadgesreader.GetOrdinal("update_items")].ToString();
                    }

                    foreach (var link in uo.UserLinks)
                    {
                        switch (link.LinkText)
                        {
                            case "agenda":
                                link.LinkSpan = agenda;
                                break;
                            case "Guidelines":
                                link.LinkSpan = guidelines;
                                break;
                            case "Update Mgmt":
                                link.LinkSpan = updateItems;
                                break;
                        }
                    }

                    var allUserModules = await new SettingsLayer().GetModuleList(username, cn);
                    uo.modules = allUserModules.userModuleConfigurable;
                    uo.modulesRequired = allUserModules.userModuleAvailable;



                    //Return uo
                    string userName;
                    if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") &&
                                                                            HttpContext.Current.Request.UrlReferrer.Port == 51268))
                    {
                        userName = "test321"; // HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = HttpContext.Current.User.Identity.Name;
                    }

                    var sclinks = await GetSClist(userName, cn);
                    sclinks = sclinks.OrderBy(x => x.scorcard_name).Select(x => x).ToList();
                    var groupApps = (from a in sclinks group a by a.scorecard_appname into g select new Apps() { appname = g.Key, scorecards = g.Select(s => s).ToList() }).ToList();
                    //groupApps = groupApps.OrderBy(x => x.scorecards == x.scorecards).Select(x=>x).ToList();
                    uo.apps = groupApps;
                    uo.scorecards = sclinks;

                    var logoSqlComm = new SqlCommand
                    {
                        Connection = cn,
                        CommandText =
                            "select top 1 client_logo_small from userextrainfo join userapps on userextrainfo.username = userapps.username join app_settings on app_settings.appname = userapps.appname where userapps.username = '" +
                            username.Replace("'","''") + "' and client_logo is not null",
                        CommandType = CommandType.Text
                    };



                    var getLogoAdapter = new SqlDataAdapter(logoSqlComm);
                    var logods = new DataSet();
                    getLogoAdapter.Fill(logods);
                    var logodt = logods.Tables[0];
                    //DataTable sc_dt = GetTable("select scorecards.id, short_name, scorecard_role, userapps.appname from userapps join scorecards on user_scorecard = scorecards.id where username = '" + username + "' and active = 1 order by short_name");
                    if (logodt.Rows.Count > 0)
                    {

                        foreach (DataRow logoDr in logodt.Rows)
                        {
                            uo.userClientLogo = logoDr["client_logo_small"].ToString();
                        }
                    }


                    var getDashboardUrlCommand = new SqlCommand
                    {
                        Connection = cn,
                        CommandText =
                            "select * From userextrainfo left join userapps on userapps.username = UserExtraInfo.username left join app_settings on app_settings.appname = UserApps.appname where userextrainfo.username =  '" +
                            username.Replace("'", "''") + "' ",
                        CommandType = CommandType.Text
                    };



                    var getDashboardAdapter =
                        new SqlDataAdapter(getDashboardUrlCommand);
                    var dashboardds = new DataSet();
                    getDashboardAdapter.Fill(dashboardds);
                    var dashboarddt = dashboardds.Tables[0];
                    //DataTable sc_dt = GetTable("select scorecards.id, short_name, scorecard_role, userapps.appname from userapps join scorecards on user_scorecard = scorecards.id where username = '" + username + "' and active = 1 order by short_name");
                    if (dashboarddt.Rows.Count > 0)
                    {

                        uo.useNewDashboard = false;
                        foreach (DataRow dashboardDr in dashboarddt.Rows)
                        {
                            if (dashboardDr["dashboard"].ToString() == "new")
                                uo.useNewDashboard = true;
                        }
                    }
                }

                cn.Close();
                cn.Dispose();
            }

            return uo;
        }



        /// <summary>
        /// GetCallsCount method
        /// </summary>
        /// <returns></returns>
        public CallsCountModel GetCallsCount()
        {
            CallsCountModel callsCount = new CallsCountModel();

            string userName = HttpContext.Current.User.Identity.Name;
            string userRole = "";
            if (userName == "")
            {
                userName = "cic_chat_qa2"; ;
            }

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {


                SqlCommand command = new SqlCommand(
                         "select user_role, non_edit,speed_increment, calls_start_immediately, username from userextrainfo where username ='" + userName + "'",
                         sqlCon);
                sqlCon.Open();
                SqlDataReader reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.HasRows)
                    {
                        userRole = reader2.GetValue(reader2.GetOrdinal("user_role")).ToString();
                    }
                }

                string comm = @"if ('" + userRole + "' =  ('QA')) begin declare @today date = convert(date, dbo.getMTdate()) select  sum(review_time) as [time],count(*) as [count]  from vwForm with(nolock) where reviewer = @username and review_date > DATEADD(dd, -(DATEPART(dw, @today)-1), @today) end";

                var reply = new SqlCommand(comm, sqlCon)
                {
                    CommandTimeout = 60,
                    CommandType = CommandType.Text
                };
                reply.Parameters.AddWithValue("@username", userName);

                SqlDataReader reader = reply.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        callsCount = new CallsCountModel()
                        {
                            time = reader.IsDBNull(reader.GetOrdinal("time")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("time")).ToString()),
                            count = int.Parse(reader.GetValue(reader.GetOrdinal("count")).ToString())
                        };
                    }
                }
            }
            return callsCount;
        }

        public async Task<UserObject> GetUserInfo(string userName, SqlConnection cn)
        {

            {


                UserObject uo = new UserObject();



                SqlCommand reply = new SqlCommand("select  user_role, non_edit,speed_increment, calls_start_immediately, username, " +
                    "(select top 1 client_logo_small from userapps join app_settings on app_settings.appname = userapps.appname where userapps.username = @username and client_logo is not null)as client_logo_small " +
                    "from userextrainfo where username = @username", cn);

                reply.Parameters.AddWithValue("username", userName);

                SqlDataReader reader = reply.ExecuteReader();
                List<LinkList> links = new List<LinkList>();
                while (reader.Read())
                {

                    uo.userClientLogo = reader.GetValue(reader.GetOrdinal("client_logo_small")).ToString();
                    uo.UserRole = reader.GetValue(reader.GetOrdinal("user_role")).ToString();
                    uo.UserName = reader.GetValue(reader.GetOrdinal("username")).ToString();
                    uo.SpeedInc = reader.GetValue(reader.GetOrdinal("speed_increment")).ToString();
                    uo.StartImmediately = (reader.GetValue(reader.GetOrdinal("calls_start_immediately")).ToString() == "True");					
				}


                return await Task.Run(() => { return uo; });
            }
        }


        public async Task<List<LinkList>> GetMenu(string userName, SqlConnection cn)
        {

            {
                SqlCommand getMyMenuSqlComm = new SqlCommand();


                getMyMenuSqlComm.Connection = cn;
                getMyMenuSqlComm.CommandText = "[getMyMenu]";
                getMyMenuSqlComm.CommandType = CommandType.StoredProcedure;
                getMyMenuSqlComm.Parameters.AddWithValue("@userName", userName);

                SqlDataReader reader = getMyMenuSqlComm.ExecuteReader();
                List<LinkList> links = new List<LinkList>();
                while (reader.Read())
                {
                    links.Add(new LinkList()
                    {
                        LinkText = reader.GetValue(reader.GetOrdinal("link")).ToString(),
                        LinkURL = reader.GetValue(reader.GetOrdinal("url")).ToString(),
                        LinkSpan = reader.GetValue(reader.GetOrdinal("span_data")).ToString()
                    });
                }

                SqlCommand getBadgessqlComm = new SqlCommand
                {
                    Connection = cn,
                    CommandText = "[getBadges]",
                    CommandType = CommandType.StoredProcedure
                };
                getBadgessqlComm.Parameters.AddWithValue("@userName", userName);

                var getBadgesreader = getBadgessqlComm.ExecuteReader();
                var agenda = "";
                var guidelines = "";
                var updateItems = "";
                var calibrations = "";
                while (getBadgesreader.Read())
                {
                    agenda = getBadgesreader[getBadgesreader.GetOrdinal("agenda")].ToString();
                    guidelines = getBadgesreader[getBadgesreader.GetOrdinal("guidelines")].ToString();
                    updateItems = getBadgesreader[getBadgesreader.GetOrdinal("update_items")].ToString();
                    calibrations = getBadgesreader[getBadgesreader.GetOrdinal("calibrations")].ToString();
                }
                foreach (var link in links)
                {
                    switch (link.LinkText)
                    {
                        case "agenda":
                            link.LinkSpan = agenda;
                            break;
                        case "Guidelines":
                            link.LinkSpan = guidelines;
                            break;
                        case "Update Mgmt":
                            link.LinkSpan = updateItems;
                            break;
                        case "Calibration":
                            link.LinkSpan = calibrations;
                            break;
                    }
                }

                return await Task.Run(() =>
                {
                    return links;
                });
            }
        }

        public async Task<List<MyScorecards>> GetSClist(string userName, SqlConnection sqlCon)
        {
            var sclinks = new List<MyScorecards>();
            {
                var sCsqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "[GetAvailableUserScorecardList]",
                    CommandType = CommandType.StoredProcedure
                };
                sCsqlComm.Parameters.AddWithValue("@userName", userName);
                var getScorecardsAdapter = new SqlDataAdapter(sCsqlComm);
                var scorecardsDs = new DataSet();
                getScorecardsAdapter.Fill(scorecardsDs);
                var scDt = scorecardsDs.Tables[0];


                //DataTable sc_dt = GetTable("select scorecards.id, short_name, scorecard_role, userapps.appname from userapps join scorecards on user_scorecard = scorecards.id where username = '" + username + "' and active = 1 order by short_name");
                if (scDt.Rows.Count > 0)
                {
                    var apps = new List<Apps>();

                    foreach (DataRow link in scDt.Rows)
                    {

                        var ll = new MyScorecards
                        {
                            scorecard = int.Parse(link["scorecard_id"].ToString()),
                            scorcard_name = link["short_name"].ToString(),
                            scorecard_role = link["scorecard_role"].ToString(),
                            scorecard_appname = link["appname"].ToString(),
                            accountManager = link["account_manager"].ToString(),
                            appId = int.Parse(link["app_stetingsId"].ToString()),
                            isNew = bool.Parse(link["IsNew"].ToString())
                        };


                        sclinks.Add(ll);
                    }


                }
                return await Task.Run(() => { return sclinks; });
            }
        }
        public ForcedFilters GetUserForsedFilters(string userName, SqlConnection sqlConn)
        {
            //  var a ="Campaign", "Group", "QA", "Agent", "TeamLead" 
            sqlConn.Open();
            var ff = new ForcedFilters();
            ff.agent = new List<string>();
            ff.campaign = new List<string>();
            ff.group = new List<string>();
            ff.qa = new List<string>();
            ff.teamLead = new List<string>();
            var sCsqlComm = new SqlCommand
            {
                Connection = sqlConn,
                CommandText = "select * from forced_filters where username=@userName",
                CommandType = CommandType.Text
            };
            sCsqlComm.Parameters.AddWithValue("@userName", userName);
            SqlDataReader reader = sCsqlComm.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetFieldValue<string>(reader.GetOrdinal("filter_type")) == "Agent") { ff.agent.Add(reader.GetFieldValue<string>(reader.GetOrdinal("filter_value"))); }
                    if (reader.GetFieldValue<string>(reader.GetOrdinal("filter_type")) == "Campaign") { ff.campaign.Add(reader.GetFieldValue<string>(reader.GetOrdinal("filter_value"))); }
                    if (reader.GetFieldValue<string>(reader.GetOrdinal("filter_type")) == "Group") { ff.group.Add(reader.GetFieldValue<string>(reader.GetOrdinal("filter_value"))); }
                    if (reader.GetFieldValue<string>(reader.GetOrdinal("filter_type")) == "QA") { ff.qa.Add(reader.GetFieldValue<string>(reader.GetOrdinal("filter_value"))); }
                    if (reader.GetFieldValue<string>(reader.GetOrdinal("filter_type")) == "TeamLead") { ff.teamLead.Add(reader.GetFieldValue<string>(reader.GetOrdinal("filter_value"))); }
                }
            }
            sqlConn.Close();
            return ff;
        }
    }
}
