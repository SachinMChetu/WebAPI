using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DAL.Models;
using System.Linq;
//using AutoMapper;

namespace DAL.Layers
{
    public class SchedulingLayer
    {
        /// <summary>
        /// GetAvailableDaysPeriods
        /// </summary>
        /// <returns></returns>
        public dynamic GetAvailableDaysPeriods()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString;
            using (var sqlCon = new SqlConnection(connectionString))
            {

                var gethourSqlComm = new SqlCommand("select * from [SchedulingTimeSheet]");
                gethourSqlComm.CommandTimeout = 60;
                gethourSqlComm.Connection = sqlCon;
                gethourSqlComm.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(gethourSqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                var hourList = new List<TimeModel>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TimeSpan start = dr.Field<TimeSpan>("startTime");
                    TimeSpan end = dr.Field<TimeSpan>("endTime");

                    hourList.Add(new TimeModel()
                    {
                        id = dr.Field<int>("id"),
                        start = start,
                        end = end
                    }
                    );

                }
                return hourList;
            }

        }


        /// <summary>
        /// GetAvailableHoursPeriods
        /// </summary>
        /// <returns></returns>
        public dynamic GetAvailableHoursPeriods()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString;
            using (var sqlCon = new SqlConnection(connectionString))
            {
                var getDaysSqlComm = new SqlCommand("select * from [SchedulingDaysSheet]");
                getDaysSqlComm.CommandTimeout = 60;
                getDaysSqlComm.CommandType = CommandType.Text;
                getDaysSqlComm.Connection = sqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(getDaysSqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                var daysList = new List<DayModel>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    daysList.Add(new DayModel()
                    {
                        id = dr.Field<int>("id"),
                        day = dr.Field<string>("day")
                    }
                    );

                }
                return daysList;
            }
           
        }


        /// <summary>
        /// GetUserWorkingHours
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public dynamic GetUserWorkingHours(string userName)
        {


            var connectionString = ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString;
            using (var sqlCon = new SqlConnection(connectionString))
            {
                var getDaysSqlComm = new SqlCommand("select * from [SchedulingDaysSheet]");
                getDaysSqlComm.CommandTimeout = 60;
                getDaysSqlComm.CommandType = CommandType.Text;
                getDaysSqlComm.Connection = sqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(getDaysSqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                var daysList = new List<DayModel>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    daysList.Add(new DayModel()
                    {
                        id = dr.Field<int>("id"),
                        day = dr.Field<string>("day")
                    }
                    );

                }
                return daysList;
            }

        }
        /// <summary>
        /// GetUserSettingsRealization method
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public dynamic GetSchedulingUserSettings(string userName)
        {
            List<ExtendedUserProfileModel> userStettings = new List<ExtendedUserProfileModel>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetUserProfile"
                };
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlComm.Connection = sqlCon;
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            ExtendedUserProfileModel extendedUserProfileModel = new ExtendedUserProfileModel
                            {
                                userId = int.Parse(reader.GetValue(reader.GetOrdinal("userId")).ToString()),
                                userName = reader.GetValue(reader.GetOrdinal("userName")).ToString(),
                                hoursPerWeek = int.Parse(reader.GetValue(reader.GetOrdinal("hoursWeek")).ToString()),
                                daysPerWeek = int.Parse(reader.GetValue(reader.GetOrdinal("daysWeek")).ToString()),
                                prefStartHour = int.Parse(reader.GetValue(reader.GetOrdinal("prefStartHour")).ToString())
                            };

                            userStettings.Add(extendedUserProfileModel);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    return userStettings;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// GetInitialInfo
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public InitialData GetInitialInfo(string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                var sqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "[getSchedulingInitialInfo]",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                InitialData initialData = new InitialData();
                List<TimeModel> periods = new List<TimeModel>();
                while (reader.Read())
                {
                    periods.Add(new TimeModel()
                    {
                        id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                        start = TimeSpan.Parse(reader.GetValue(reader.GetOrdinal("startTime")).ToString()),
                        end = TimeSpan.Parse(reader.GetValue(reader.GetOrdinal("endTime")).ToString())

                    });
                }
                List<App> apps = new List<App>();
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        string appName = reader.GetValue(reader.GetOrdinal("appname")).ToString();
                        if (apps.Where(a => a.app == appName).Select(a => a).Count() == 0)
                        {
                            apps.Add(new App()
                            {
                                app = appName,
                                scorecards = new List<ScorecardsInfo>()
                            });
                        }
                        foreach (var a in apps)
                        {
                            if (a.app == appName)
                            {
                                a.scorecards.Add(new ScorecardsInfo()
                                {
                                    id = int.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
                                    name = reader.GetValue(reader.GetOrdinal("name")).ToString()
                                });
                            }

                        }

                    }
                }
                initialData.periods = periods;
                initialData.apps = apps;
                return initialData;
            }
        }

        /// <summary>
        /// UpdateUserSettingsRealization method
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="hoursPerWeek"></param>
        /// <param name="daysPerWeek"></param>
        /// <param name="prefStartHour"></param>
        /// <returns></returns>
        public dynamic UpdateSchedulingUserSettings(int userId, string userName, int hoursPerWeek, int daysPerWeek, int prefStartHour)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                var sqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "[UpdateExtendedUserProfile]",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@userId", userId);
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlComm.Parameters.AddWithValue("@hoursPerWeek", hoursPerWeek);
                sqlComm.Parameters.AddWithValue("@daysPerWeek", daysPerWeek);
                sqlComm.Parameters.AddWithValue("@prefStartHour", prefStartHour);

                sqlCon.Open();
                try
                {
                    sqlComm.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
                return "success";
            }
        }

        /// <summary>
        /// GetRequieredQAs Realization
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        public dynamic GetRequieredQAs(string appName, int scorecardId, DateTime startDate, DateTime endDate)
        {
            var listQAModel = new List<RequieredQAModel>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetRequieredQAs"
                };
                sqlComm.Parameters.AddWithValue("@appname", appName);
                sqlComm.Parameters.AddWithValue("@scorecardId", scorecardId);

                sqlComm.Parameters.AddWithValue("@startDate", startDate);
                sqlComm.Parameters.AddWithValue("@endDate", endDate);
                sqlComm.Connection = sqlCon;
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            appName = reader.GetValue(reader.GetOrdinal("appname")).ToString();
                            var dayDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("date")).ToString());
                            if (listQAModel.Where(a => a.dayDate == dayDate).Select(a => a).Count() == 0)
                            {
                                listQAModel.Add(new RequieredQAModel()
                                {
                                    appName = appName,
                                    scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                    dayDate = dayDate,
                                    workingPeriods = new List<WorkingPeriod>()
                                });
                            }
                            foreach (var a in listQAModel)
                            {
                                if (a.dayDate == dayDate)
                                {
                                    a.workingPeriods.Add(new WorkingPeriod()
                                    {
                                        periodId = int.Parse(reader.GetValue(reader.GetOrdinal("hourId")).ToString()),
                                        required = int.Parse(reader.GetValue(reader.GetOrdinal("requiredQAs")).ToString()),
                                        selected = int.Parse(reader.GetValue(reader.GetOrdinal("selectedQAs")).ToString())
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    return listQAModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// UpdateRequieredQAs
        /// </summary>
        /// <param name="requieredQAModel"></param>
        /// <returns></returns>
        public string UpdateRequieredQAs(List<RequieredQAModel> list)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                for (var i = 0; i < list.Count; i++)
                {

                    foreach (var item in list[i].workingPeriods)
                    {
                        var sqlComm = new SqlCommand
                        {
                            Connection = sqlCon,
                            CommandText = "[UpdateRequieredQAsByScorecardId]",
                            CommandType = CommandType.StoredProcedure
                        };
                        sqlComm.Parameters.AddWithValue("@appname", list[i].appName);
                        sqlComm.Parameters.AddWithValue("@scorecardId", list[i].scorecardId);
                        sqlComm.Parameters.AddWithValue("@date", list[i].dayDate);
                        sqlComm.Parameters.AddWithValue("@hourId", item.periodId);
                        sqlComm.Parameters.AddWithValue("@requiered", item.periodId);
                        sqlCon.Open();
                        try
                        {
                            sqlComm.ExecuteReader();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);

                        }
                    }



                }
                return "success";
            }
        }

        /// <summary>
        /// GetAvailableQAs
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public dynamic GetAvailableQAs(DateAppNameModel model)
        {
            
            var listQAModel = new List<AvailableQAsModel>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetAvailableQAs"
                };
                sqlComm.Parameters.AddWithValue("@appname", model.appName);
                sqlComm.Parameters.AddWithValue("@start", model.startDate);
                sqlComm.Connection = sqlCon;
                try
                {
                    sqlCon.Open();
                    IDataReader reader = sqlComm.ExecuteReader();
            
                    while (reader.Read())
                    {
                        try
                        {

                            listQAModel.Add(new AvailableQAsModel()
                            {
                                userName = reader.GetValue(reader.GetOrdinal("UserName")).ToString(),
                                appName = reader.GetValue(reader.GetOrdinal("appname")).ToString(),
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                                assignedHours = int.Parse(reader.GetValue(reader.GetOrdinal("assignedHours")).ToString())
                            });


                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    return listQAModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// UpdateAvailableQAs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic UpdateAvailableQAs(UpdateAvailableQAsModel model)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                var sqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "[UpdateExtendedUserProfile]",
                    CommandType = CommandType.StoredProcedure
                };
                sqlComm.Parameters.AddWithValue("@userName", model.userName);
                sqlComm.Parameters.AddWithValue("@selectedDate", model.selectedDate);
                sqlComm.Parameters.AddWithValue("@assignedHours", model.hourId);

                sqlCon.Open();
                try
                {
                    sqlComm.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
                return "success";
            }
        }

        /// <summary>
        /// GetTimeShift method
        /// </summary>
        /// <param name="timeShiftId"></param>
        /// <returns></returns>
        public dynamic GetTimeShift(int timeShiftId)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                List<TimeShift> shifts = new List<TimeShift>();
                TimeShift timeShift =new TimeShift();
                var sqlCommand = new SqlCommand
                {
                    Connection = con,
                    CommandText = "GetSchedulingTimeShift",
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@id", timeShiftId);
                con.Open();
                try
                {
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            shifts.Add(new TimeShift
                            {
                                shiftId = int.Parse(reader.GetValue(reader.GetOrdinal("shiftId")).ToString()),
                                shiftName = reader.GetValue(reader.GetOrdinal("shiftName")).ToString(),
                                hourID = new List<int>()
                            });
                            foreach (var item in shifts)
                            {
                                item.hourID.Add(int.Parse(reader.GetValue(reader.GetOrdinal("hourID")).ToString()));
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    return shifts;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// UpdateTimeShift method
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
        public dynamic UpdateTimeShift(TimeShift shift)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                var sqlComm = new SqlCommand
                {
                    Connection = sqlCon,
                    CommandText = "UpdateSchedulingTimeShift",
                    CommandType = CommandType.StoredProcedure
                };
                foreach (var item in shift.hourID)
                {
                    sqlComm.Parameters.AddWithValue("@timeShiftId", shift.shiftId);
                    sqlComm.Parameters.AddWithValue("@hourId", item);
                }
               
                sqlCon.Open();
                try
                {
                    sqlComm.ExecuteReader();
                }
                catch (Exception)
                {
                    throw;
                }
                return shift;
            }

            }



    }

}
