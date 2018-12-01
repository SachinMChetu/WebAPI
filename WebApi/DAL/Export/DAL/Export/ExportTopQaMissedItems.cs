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
    public class ExportTopQaMissedItemsCode
    {
        public dynamic ExportTopQaMissedItems(AverageFilter filters,string userName)
        {
            Filter f = new Filter() { filters = filters.filters, range = filters.range };

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                if (filters.filters.badCallsOnly == false)
                {
                    SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getTopqaMissedItemsJson_v2]", userName, filters.comparison);

                    sqlComm.Connection = sqlCon;

                    PageFiltersData pageFiltersData = new PageFiltersData();
                    sqlCon.Open();

                    TopMissedItemsResponseData topMissed = new TopMissedItemsResponseData() { missedItems = new List<MissedItem>() };
                    try
                    {
                        SqlDataReader reader = sqlComm.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                topMissed.missedItems.Add(new MissedItem()
                                {
                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    questionShortName = (reader.GetValue(reader.GetOrdinal("questionShortName")).ToString()),
                                    scorecardName = (reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missedCalls")).ToString()),
                                    questionSectionName = reader.GetValue(reader.GetOrdinal("sectionName")).ToString(),
                                    isComposite = bool.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()),
                                    isLinked = bool.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()),
                                    questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                    comparedMissedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedMissedCalls")).ToString()),
                                    comparedTotalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("comparedTotalCalls")).ToString()),
                                });
                            }
                            catch (Exception ex) { }
                        }
                        reader.NextResult();
                        List<MissedItemAgentInfo> lst = new List<MissedItemAgentInfo>();
                        while (reader.Read())
                        {
                            try
                            {
                                lst.Add(new MissedItemAgentInfo()
                                {

                                    questionId = int.Parse(reader.GetValue(reader.GetOrdinal("questionId")).ToString()),
                                    name = reader.GetValue(reader.GetOrdinal("reviewer")).ToString(),
                                    totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total_calls")).ToString()),
                                    missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_missed")).ToString()),
                                });
                            }
                            catch (Exception ex) { }
                        }
                        foreach (var item in topMissed.missedItems)
                        {
                            item.top3Agents = new List<MissedItemAgentInfo>();
                            item.top3Agents.AddRange((from v in lst where v.questionId == item.questionId select v).ToList());
                        }

                        //return topMissed;
                        List<TopMissedItemsExportModel> topMissedItemsExportModel = new List<TopMissedItemsExportModel>();
                        var propNames = new List<PropertieName>
                    {
                        new PropertieName { propName = "Missed Point", propValue = "questionShortName", propPosition = 1 },
                        new PropertieName { propName = "Section", propValue = "questionSectionName", propPosition = 2 },
                        new PropertieName { propName = "Scorecard", propValue = "scorecardName", propPosition = 3 },
                        new PropertieName { propName = "Number missed", propValue = "missedCalls", propPosition = 4 },
                        new PropertieName { propName = "Total calls", propValue = "totalCalls", propPosition = 5 },
                        new PropertieName { propName = "Occurrence", propValue = "occurrence", propPosition = 6 },
                        new PropertieName { propName = "Delta", propValue = "delta", propPosition = 7 },
                        new PropertieName { propName = "Top 3 agents", propValue = "top3Agents", propPosition = 8 }
                    };
                        foreach (var item in topMissed.missedItems)
                        {
                            List<string> topAgents = new List<string>();

                            foreach (var i in item.top3Agents)
                            {
                                if (item.questionId == i.questionId)
                                {
                                    topAgents.Add((new StringBuilder().Append(i.name + i.missedCalls + "/" + i.totalCalls + ";").ToString()));
                                }
                            }
                            if (item.comparedTotalCalls == 0)
                            {
                                topMissedItemsExportModel.Add(new TopMissedItemsExportModel
                                {
                                    questionShortName = item.questionShortName,
                                    questionSectionName = item.questionSectionName,
                                    scorecardName = item.scorecardName,
                                    missedCalls = item.missedCalls,
                                    totalCalls = item.totalCalls,
                                    occurrence = (float)Math.Round((float)((float)item.missedCalls / (float)item.totalCalls) * 100),
                                    delta = (float)Math.Round((float)((float)item.missedCalls / (float)item.totalCalls) * 100), //(item.comparedMissedCalls / item.comparedTotalCalls),
                                    top3Agents = ExportCodeHelper.GetCSVFromList(topAgents)
                                });
                            }
                            else
                            {
                                topMissedItemsExportModel.Add(new TopMissedItemsExportModel
                                {
                                    questionShortName = item.questionShortName,
                                    questionSectionName = item.questionSectionName,
                                    scorecardName = item.scorecardName,
                                    missedCalls = item.missedCalls,
                                    totalCalls = item.totalCalls,
                                    occurrence = (float)Math.Round(((float)item.missedCalls / (float)item.totalCalls) * 100),
                                    delta = (float)Math.Round(((float)((float)item.missedCalls / (float)item.totalCalls) * 100) - ((float)((float)item.comparedMissedCalls / (float)item.comparedTotalCalls) * 100)),//(item.missedCalls / item.totalCalls) * 100,
                                    top3Agents = ExportCodeHelper.GetCSVFromList(topAgents)
                                });
                            }

                        }
                        ExportHelper.Export(propNames, topMissedItemsExportModel, "TopQaMissed" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "TopQaMissedPoints", userName);

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
}
