using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Export
{
    public class ExportGroupPerformanceCode
    {
        public dynamic ExportGroupPerformance(Filter filters,string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "getGroupPerfAPI", userName);

                sqlComm.Connection = sqlCon;

                var gpl = new List<GroupPerformance>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            PeriodPerformance period = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("avg_score")).ToString())
                            };
                            PeriodPerformance prviousPeriod = new PeriodPerformance()
                            {
                                callsCount = int.Parse(reader.GetValue(reader.GetOrdinal("prev_num_calls")).ToString()),
                                score = float.Parse(reader.GetValue(reader.GetOrdinal("prev_avg_score")).ToString())
                            };
                            GroupInfo groupInfo = new GroupInfo() { id = reader.GetValue(reader.GetOrdinal("agent_group")).ToString(), name = reader.GetValue(reader.GetOrdinal("agent_group")).ToString() };
                            gpl.Add(new GroupPerformance
                            {
                                groupInfo = groupInfo,
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                currentPeriod = period,
                                previousPeriod = prviousPeriod
                            });
                        }
                        catch 
                        {

                        }
                    };
                    //return gpl;
                    var propNames = new List<PropertieName>
                    {
                         new PropertieName { propName = "Group name", propValue = "name", propPosition = 1 },
                        new PropertieName { propName = "Scorecard name", propValue = "scorecardName", propPosition = 2 },
                        new PropertieName { propName = "Score current period", propValue = "currentScore", propPosition = 3 },
                        new PropertieName { propName = "Calls current period", propValue = "currentCalls", propPosition = 4 },
                        new PropertieName { propName = "Score previous period", propValue = "previousScore", propPosition = 5 },
                        new PropertieName { propName = "Calls previous period", propValue = "previousCalls", propPosition = 6 },
                        new PropertieName { propName = "Delta score", propValue = "delta", propPosition = 7 }
                    };
                    List<ExportGroupPerformanceModel> exportGroupPerformanceModels = new List<ExportGroupPerformanceModel>();
                    foreach (var item in gpl)
                    {
                        exportGroupPerformanceModels.Add(new ExportGroupPerformanceModel
                        {
                            name = item.groupInfo.name,
                            scorecardName = item.scorecardName,
                            currentScore = item.currentPeriod.score,
                            currentCalls = item.currentPeriod.callsCount,
                            previousCalls = item.previousPeriod.callsCount,
                            previousScore = item.previousPeriod.score,
                            delta = (item.currentPeriod.score - item.previousPeriod.score) + "%"
                        });
                    }
                    ExportHelper.Export(propNames, exportGroupPerformanceModels, "GroupPerformance" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "GroupPerformance", userName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return "success";

        }
    }
}
