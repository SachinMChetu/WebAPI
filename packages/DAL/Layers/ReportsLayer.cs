using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;
using DAL.Models.ReportsModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Layers
{
    public class ReportsLayer
    {
        /// <summary>
        /// Get Filters for reports page
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public dynamic GetAllReportsFilters(FilterReports filters, string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {


                FilterReports f = new FilterReports() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = ReportsHelper.GetFiltersParameters(f, "[GetAllReportsFilters]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;

                FiltersFormDataReports pageFiltersData = new FiltersFormDataReports();
                sqlCon.Open();
                try
                {
                    SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable tblAgentgroup = ds.Tables[0];
                    DataTable tblAgentCampain = ds.Tables[1];
                    DataTable tblAgentAPP = ds.Tables[2];
                    DataTable tblAgentScorecard = ds.Tables[3];


                    DataTable tblRangeCount = ds.Tables[4];
                    List<FilterGroupValue> AgentGrouplLst = new List<FilterGroupValue>();
                    List<FilterCampainValue> AgentCampainLst = new List<FilterCampainValue>();
                    List<FilterAppValue> AgentAppLst = new List<FilterAppValue>();
                    //List<FilterAgentValue> AgentAgentsLst = new List<FilterAgentValue>();
                    List<FilterScorecardValue> AgentScorecardLst = new List<FilterScorecardValue>();
                    //List<FilterQAValue> AgentQALst = new List<FilterQAValue>();
                    //List<FilterTeamLeadValue> teamLeads = new List<FilterTeamLeadValue>();
                    RangeCalls rangeCalls = new RangeCalls();
                    //Dim AgentMissedItemsLst As List(Of PageFiltersData.FilterValue) = New List(Of PageFiltersData.FilterValue)

                    foreach (DataRow row in tblAgentgroup.Rows)
                    {
                        AgentGrouplLst.Add(new FilterGroupValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString()),
                            top3Agents = null
                        });
                    }
                    pageFiltersData.groups = AgentGrouplLst;

                    foreach (DataRow row in tblAgentCampain.Rows)
                    {
                        AgentCampainLst.Add(new FilterCampainValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString())
                        });
                    }
                    pageFiltersData.campaigns = AgentCampainLst;


                    foreach (DataRow row in tblAgentAPP.Rows)
                    {
                        AgentAppLst.Add(new FilterAppValue()
                        {
                            name = row[0].ToString(),
                            id = row[0].ToString(),
                            count = int.Parse(row[1].ToString())
                        });
                    }
                    pageFiltersData.apps = AgentAppLst;

                    foreach (DataRow row in tblAgentScorecard.Rows)
                    {
                        try
                        {
                            int _failScore = 0;
                            int.TryParse(row[3].ToString(), out _failScore);
                            AgentScorecardLst.Add(new FilterScorecardValue()
                            {
                                id = int.Parse(row[0].ToString()),
                                name = row[2].ToString(),
                                count = int.Parse(row[1].ToString()),
                                failScore = _failScore,
                            });
                        }
                        catch (Exception ex1)
                        {
                            throw ex1;
                        }

                    }
                    pageFiltersData.scorecards = AgentScorecardLst;





                    if (tblRangeCount.Rows.Count > 0)
                    {
                        rangeCalls.total = int.Parse(tblRangeCount.Rows[0][0].ToString());
                        rangeCalls.filtered = int.Parse(tblRangeCount.Rows[0][1].ToString());
                    }
                    pageFiltersData.rangeCalls = rangeCalls;


                    return pageFiltersData;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get data of agent report
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public dynamic GetAgentReport(AverageReportsFilters filters, string userName)
        {
            #region filters

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    Connection = sqlCon,

                    CommandText = "[GetAgentReport]",

                    CommandType = CommandType.StoredProcedure
                };

                sqlComm.Parameters.AddWithValue("@userName", userName);

                if (filters.filters.scorecards != null && (filters.filters.scorecards.Count > 0))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.scorecards)
                    {
                        preparedLst.Append(("'" + (value + "',")));
                    }

                    sqlComm.Parameters.AddWithValue("@scorecardIDs", preparedLst.ToString().Trim(','));
                }

                if (filters.filters.campaigns != null && ((filters.filters.campaigns.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.campaigns)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    sqlComm.Parameters.AddWithValue("@campaignIDs", preparedLst.ToString().Trim(','));
                }

                if (filters.filters.groups != null && (filters.filters.groups.Count > 0))
                {

                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.groups)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    sqlComm.Parameters.AddWithValue("@groupIDs", preparedLst.ToString().Trim(','));
                }

                sqlComm.Parameters.AddWithValue("@filterByReviewDate", filters.filters.filterByReviewDate);
                sqlComm.Parameters.AddWithValue("@pagerows", filters.paging.pagerows);
                sqlComm.Parameters.AddWithValue("@pagenum", filters.paging.pagenum);
                //sqlComm.Parameters.AddWithValue("@missedBy", filters.filters.missedBy);
                if (((filters.range == null) || (filters.range.start.Length < 4)))
                {
                    sqlComm.Parameters.AddWithValue("@Start", DateTime.Now.AddDays(-14));
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Start", DateTime.Parse(filters.range.start));
                }

                if (((filters.range.end == null) || (filters.range.end.Length < 4)))
                {
                    sqlComm.Parameters.AddWithValue("@end", DateTime.Now);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@end", DateTime.Parse(filters.range.end));
                }


                sqlComm.CommandTimeout = int.MaxValue;

                #endregion
                AgentReportModel agentReportModel = new AgentReportModel();
                agentReportModel.agentReports = new List<AgentReports>();
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        try
                        {
                            agentReportModel.agentReports.Add(new AgentReports
                            {
                                agentName = reader.GetValue(reader.GetOrdinal("AGENT")).ToString(),
                                scorecard = new ScorecardInfo
                                {
                                    scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scId")).ToString()),
                                    scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                                },
                                reviewId = reader.IsDBNull(reader.GetOrdinal("review_ID")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("review_ID")).ToString()),
                                reviewer = reader.GetValue(reader.GetOrdinal("reviewer")).ToString(),
                                agentGroup = reader.GetValue(reader.GetOrdinal("AGENT_GROUP")).ToString(),
                                campaign = reader.GetValue(reader.GetOrdinal("CAMPAIGN")).ToString(),
                                scorecardPassPercent = int.Parse(reader.GetValue(reader.GetOrdinal("pass_percent")).ToString()),
                                scorecardFailScore = int.Parse(reader.GetValue(reader.GetOrdinal("fail_score")).ToString()),
                                callTime = (int)Math.Round(double.Parse(reader.GetValue(reader.GetOrdinal("callTime")).ToString())),
                                agentScore = int.Parse(reader.GetValue(reader.GetOrdinal("score")).ToString())
                            });
                        }
                        catch (Exception ex) { throw ex; }

                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            agentReportModel.totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString());
                        }
                    }
                }
                catch (Exception ex) { throw ex; }

                return agentReportModel;
            }



        }




    }
}
