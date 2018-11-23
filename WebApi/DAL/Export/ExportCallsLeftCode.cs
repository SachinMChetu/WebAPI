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
    public class ExportCallsLeftCode
    {
        public dynamic ExporCallsLeft(Filter filters,string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                Filter f = new Filter() { filters = filters.filters, range = filters.range };
            SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "GetCallsLeftList", userName);

            sqlComm.Connection = sqlCon;
            var callsLeftLst = new List<CallsLeft>();
            var pendingCalls = new List<PendingCall>();
            try
            {
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ScorecardInfo sc = new ScorecardInfo()
                        {
                            scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                            scorecardName = reader.GetValue(reader.GetOrdinal("scorecardName")).ToString()
                        };
                        CallsLeft callsLeft = new CallsLeft()
                        {
                            badCalls = int.Parse(reader.GetValue(reader.GetOrdinal("badCalls")).ToString()),
                            pending = int.Parse(reader.GetValue(reader.GetOrdinal("pending")).ToString()),
                            pendingNotReady = int.Parse(reader.GetValue(reader.GetOrdinal("pending_not_ready")).ToString()),
                            pendingReady = int.Parse(reader.GetValue(reader.GetOrdinal("pending_ready")).ToString()),
                            reviewed = int.Parse(reader.GetValue(reader.GetOrdinal("reviewed")).ToString()),
                            callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("callDate")),
                            //DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),


                            scorecard = sc,
                            pendingCalls = new List<PendingCall>()
                        };
                        callsLeftLst.Add(callsLeft);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            PendingCall sc = new PendingCall()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                callDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
                                receiveDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("receiveDate")).ToString()),
                            };
                            pendingCalls.Add(sc);
                        }
                        catch { }
                    }

                }
                foreach (var call in callsLeftLst)
                {
                    call.pendingCalls = (from a in pendingCalls where a.scorecardId == call.scorecard.scorecardId && a.callDate == call.callDate select a).ToList();
                }

                    //return callsLeftLst;
                    var propNames = new List<PropertieName>
                    {
                         new PropertieName { propName = "Scorecard Name", propValue = "scorecardName", propPosition = 1 },
                        new PropertieName { propName = "Scorecard Id", propValue = "scorecardId", propPosition = 2 },
                        new PropertieName { propName = "Call date", propValue = "callDate", propPosition = 3 },
                        new PropertieName { propName = "Bad calls", propValue = "badCalls", propPosition = 4 },
                        new PropertieName { propName = "Reviewed calls", propValue = "reviewed", propPosition = 5 },
                        new PropertieName { propName = "Pending calls", propValue = "pendingCalls", propPosition = 6 }
                    };
                    List<ExportCallsLeftModel> exportCallsLeftModel = new List<ExportCallsLeftModel>();
                    foreach(var  i in callsLeftLst)
                    {
                        exportCallsLeftModel.Add(new ExportCallsLeftModel
                        {
                            scorecardName = i.scorecard.scorecardName,
                            scorecardId = i.scorecard.scorecardId,
                            callDate = i.callDate,
                            badCalls = i.badCalls,
                            reviewed = i.reviewed,
                            pendingCalls = i.pendingNotReady+"/"+i.pendingReady
                        });
                    }
                    ExportHelper.Export(propNames, exportCallsLeftModel, "CallsLeft" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx", "CallsLeft", userName);

                }
            catch (Exception ex)
            {
                throw ex;
            }
                return "succsess";
        }
    }
    }
}
