using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Export
{
    public class ExportCampaignPerfomanceCode
    {
        public dynamic ExportCampaignPerformance(Filter filters , string userName) 
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
               
               
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "getCampaignPerfApi", userName);

                sqlComm.Connection = sqlCon;

                var gpl = new List<CampaignPerformance>();
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
                            Campaign campaign = new Campaign()
                            {
                                id = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                                name = reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                            };
                            gpl.Add(new CampaignPerformance
                            {
                                campaignInfo = campaign,
                                scorecardName = reader.GetValue(reader.GetOrdinal("scorecard_name")).ToString(),
                                currentPeriod = period,
                                previousPeriod = prviousPeriod
                            });
                        }
                        catch(Exception ex) { throw ex; }
                    };
                    //return gpl;
                    var propNames = new List<PropertieName>
                    {
                         new PropertieName { propName = "Campaign name", propValue = "name", propPosition = 1 },
                        new PropertieName { propName = "Scorecard name", propValue = "scorecardName", propPosition = 2 },
                        new PropertieName { propName = "Score current period", propValue = "currentScore", propPosition = 3 },
                        new PropertieName { propName = "Calls current period", propValue = "currentCalls", propPosition = 4 },
                        new PropertieName { propName = "Score previous period", propValue = "previousScore", propPosition = 5 },
                        new PropertieName { propName = "Calls previous period", propValue = "previousCalls", propPosition = 6 },
                        new PropertieName { propName = "Delta score", propValue = "delta", propPosition = 7 }
                    };
                    List<ExportCampaignPerformanceModel> exportCampaignPerformanceModels = new List<ExportCampaignPerformanceModel>();
                    foreach (var item in gpl)
                    {
                        exportCampaignPerformanceModels.Add(new ExportCampaignPerformanceModel
                        {
                            name = item.campaignInfo.name,
                            scorecardName = item.scorecardName,
                            currentScore = item.currentPeriod.score,
                            currentCalls = item.currentPeriod.callsCount,
                            previousCalls = item.previousPeriod.callsCount,
                            previousScore = item.previousPeriod.score,
                            delta = (item.currentPeriod.score - item.previousPeriod.score)/100+"%"
                        });
                    }
                    ExportHelper.Export(propNames, exportCampaignPerformanceModels, "CampaignPerformance" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "CampaignPerformance", userName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return "success";
            }
        }
    }
}
