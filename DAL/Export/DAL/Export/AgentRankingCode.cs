using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;


namespace DAL.Export
{
    public class AgentRankingCode
    { 
        public dynamic AgentRankingExport(AverageFilter filters,string userName)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
               
                string p = Path.Combine(HostingEnvironment.MapPath(@"~\export\"));
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }

                Filter f = new Filter() { filters = filters.filters, range = filters.range };
                SqlCommand sqlComm = new SqlCommand();

                if(filters.filters.missedItems != null && filters.filters.missedItems.Count != 0)
                {
                    sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAgentRankingJson_v3_ExportForMissedPoints]", userName, filters.comparison);
                }
                else
                {
                    sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getAgentRankingJson_v2]", userName, filters.comparison);
                }
                

                sqlComm.Connection = sqlCon;
                var aRankingResponseData = new AgentRankingResponseData();



                List<AgentMissedPoint> agentRankingInfolst = new List<AgentMissedPoint>();
                List<Agent> agentRankinglst = new List<Agent>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {

                            agentRankingInfolst.Add(new AgentMissedPoint
                            {
                                agentId = reader.GetValue(reader.GetOrdinal("agent")).ToString(),
                                questionShortName = reader.GetValue(reader.GetOrdinal("q_short_name")).ToString(),
                                missedCalls = int.Parse(reader.GetValue(reader.GetOrdinal("missed")).ToString()),
                                totalCalls = int.Parse(reader.GetValue(reader.GetOrdinal("total")).ToString()),
                                isComposite = int.Parse(reader.GetValue(reader.GetOrdinal("hasTemplate")).ToString()) == 1,
                                isLinked = int.Parse(reader.GetValue(reader.GetOrdinal("isLinked")).ToString()) == 1,
                                questionType = reader.GetValue(reader.GetOrdinal("questionType")).ToString(),
                                questionId = reader.IsDBNull(reader.GetOrdinal("question_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("question_id")).ToString())
                            });
                        }
                    }
                    catch (Exception ex) { throw ex; }




                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var temp_ranking = new Agent()
                                {
                                    id = reader.GetValue(reader.GetOrdinal("agentID")).ToString(),
                                    name = reader.GetValue(reader.GetOrdinal("AgentName")).ToString(),
                                    groupNames = new List<string>(),
                                    averageScore =reader.IsDBNull(reader.GetOrdinal("averageScore"))? 0:decimal.Parse(reader.GetValue(reader.GetOrdinal("averageScore")).ToString()),
                                    previousAverageScore = reader.IsDBNull(reader.GetOrdinal("previousAverageScore")) ? 0 : decimal.Parse(reader.GetValue(reader.GetOrdinal("previousAverageScore")).ToString()),
                                    totalCalls = reader.IsDBNull(reader.GetOrdinal("totalCalls")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("totalCalls")).ToString()),
                                    totalBadCalls = reader.IsDBNull(reader.GetOrdinal("totalBadCalls")) ? (int?)null : (int?)reader.GetValue(reader.GetOrdinal("totalBadCalls")),
                                    earliestCallDate = reader.IsDBNull(reader.GetOrdinal("earlier")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("earlier")),
                                    // DateTime.Parse(reader.GetValue(reader.GetOrdinal("earlier")).ToString()),
                                };
                                temp_ranking.top3MissedPoints = (from val in agentRankingInfolst where val.agentId.Trim().Equals(temp_ranking.id.Trim()) select val).ToList();
                                agentRankinglst.Add(temp_ranking);
                            }
                            catch (Exception ex) { throw ex; }
                        }
                    }
                    List<UserGroupInfo> ugi = new List<UserGroupInfo>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                ugi.Add(new UserGroupInfo()
                                {
                                    groupname = reader.GetValue(reader.GetOrdinal("user_group")).ToString(),
                                    username = reader.GetValue(reader.GetOrdinal("Agent")).ToString()
                                });
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }

                    foreach (var a in agentRankinglst)
                    {
                        a.groupNames = (from val in ugi where a.name.Equals(val.username) select val.groupname).ToList();
                    }
                    //
                    #region Export
                    //EXPORT Realization
                    aRankingResponseData.agents = agentRankinglst;

                    var propNames = new List<PropertieName>();
                    PropertieName name = new PropertieName();
                    List<ExportAgentRanking> exportAgentRanking = new List<ExportAgentRanking>();
                    List<ExportAgentRankingTopMissedPoints> exportAgentRankingTopMissedPoints = new List<ExportAgentRankingTopMissedPoints>();
                    if (f.filters.missedItems != null && f.filters.missedItems.Count == 1)
                    {

                        propNames.Add(new PropertieName { propName = "Agent Name", propValue = "name", propPosition = 1 });
                        propNames.Add(new PropertieName { propName = "Missed Item", propValue = "questionName", propPosition = 2 });
                        propNames.Add(new PropertieName { propName = "Call Count", propValue = "callCount", propPosition = 3 });
                        propNames.Add(new PropertieName { propName = "Missed Calls", propValue = "missedCalls", propPosition = 4 });
                        propNames.Add(new PropertieName { propName = "Missed Percent", propValue = "missedPercent", propPosition = 5 });
                        //List<int> missed = new List<int>();
                        //int z = 0;
                        foreach (var item in aRankingResponseData.agents)
                        {
                            item.questionName = new List<string>();
                            foreach (var i in item.top3MissedPoints)
                            {
                                if (Convert.ToInt32(filters.filters.missedItems[0]) == i.questionId)
                                {
                                    //missed.Add(i.missedCalls);
                                    item.missedCalls = i.missedCalls;
                                    item.totalCalls = i.totalCalls;
                                    item.questionName.Add(i.questionShortName);
                                }
                            }
                            //z++;
                        }
                        foreach (var item in aRankingResponseData.agents)
                        {
                            exportAgentRankingTopMissedPoints.Add(new ExportAgentRankingTopMissedPoints
                            {
                                name = item.name,
                                questionName = item.questionName[0],
                                callCount = item.totalCalls,
                                missedCalls = item.missedCalls,
                                missedPercent = Math.Round((double)(item.missedCalls*100)/item.totalCalls)+"%" //-100 - ((item.averageScore * 100) / item.previousAverageScore)
                            });
                        }
                        ExportHelper.Export(propNames, exportAgentRankingTopMissedPoints, "AgentRanking " + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx", "AgentRanking",userName);

                    }
                    else
                    {
                        propNames.Add(new PropertieName { propName = "Agent Name", propValue = "name", propPosition = 1 });
                        propNames.Add(new PropertieName { propName = "Start Date", propValue = "startDate", propPosition = 2 });
                        propNames.Add(new PropertieName { propName = "Score", propValue = "score", propPosition = 3 });
                        propNames.Add(new PropertieName { propName = "Group Name", propValue = "groupName", propPosition = 4 });
                        propNames.Add(new PropertieName { propName = "Delta", propValue = "delta", propPosition = 5 });
                        propNames.Add(new PropertieName { propName = "Total Calls", propValue = "totalCalls", propPosition = 6 });
                        propNames.Add(new PropertieName { propName = "Top Missed Points", propValue = "top3Agents", propPosition = 7 });
                       
                       
                        foreach (var item in aRankingResponseData.agents)
                        {
                            List<string> topAgents = new List<string>();
                            foreach (var i in item.top3MissedPoints)
                            {
                                if (item.id == i.agentId)
                                {
                                    topAgents.Add((new StringBuilder().Append(i.questionShortName +", missed "+ i.missedCalls + " of " + i.totalCalls + ";").ToString()));
                                }
                            }
                            exportAgentRanking.Add(new ExportAgentRanking
                            {
                                name = item.name,
                                startDate = item.earliestCallDate,
                                score = item.averageScore,
                                groupName = ExportCodeHelper.GetCSVFromList(item.groupNames),
                                delta = ((item.averageScore - item.previousAverageScore) % 100) + "%",
                                totalCalls = item.totalCalls,
                                top3Agents = ExportCodeHelper.GetCSVFromList(topAgents)//GetCSVFromList(topAgents) as string
                            });
                        }

                        ExportHelper.Export(propNames, exportAgentRanking, "AgentRanking " + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx", "AgentRanking",userName);
                    }




                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
                return "success";
            }
        }
    }
}
