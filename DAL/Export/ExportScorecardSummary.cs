using DAL.Extensions;
using DAL.GenericRepository;
using DAL.Models;
using DAL.Models.ExportModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Export
{
    public class ExportScorecardSummary
    {
        public void ExportScorecardSummaryCode(string userName)
        {
            List<AMSummaryModel> aMSummaryModel = new List<AMSummaryModel>();
            
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                SqlCommand sqlComm = new SqlCommand
                {
                    CommandTimeout = int.MaxValue,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[GetAMSummary]"
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                //sqlComm.Parameters.AddWithValue("@AM", userName);
                sqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlDataReader reader = sqlComm.ExecuteReader();
                aMSummaryModel = AMSummaryModel.Create(reader);
                var propNames = new List<PropertieName>
                {
                    new PropertieName { propName = "Scorecard", propValue = "scorecardName", propPosition = 1 },
                    new PropertieName { propName = "Reviewed", propValue = "mtdCallsCompleted", propPosition = 2 },
                    new PropertieName { propName = "Mins", propValue = "minutesCompleted", propPosition = 3 },
                    new PropertieName { propName = "Pending", propValue = "pendingCalls", propPosition = 4 },
                    new PropertieName { propName = "No Audio", propValue = "missingAudioCalls", propPosition = 5 },
                    new PropertieName { propName = "Last loaded", propValue = "lastLoaded", propPosition = 6 },
                    new PropertieName { propName = "Last Reviewed", propValue = "lastReviewed", propPosition = 7 },
                    new PropertieName { propName = "Oldest Pending", propValue = "oldestPending", propPosition = 8 }
                };
                List<ExportScorecardCountsModel> export = new List<ExportScorecardCountsModel>();
                foreach (var item in aMSummaryModel)
                {
                    export.Add(new ExportScorecardCountsModel
                    {
                        scorecardName = item.scorecard.scorecardName,
                        mtdCallsCompleted = item.mtdCallsCompleted == null ? 0 : (int)item.mtdCallsCompleted,
                        minutesCompleted = item.minutesCompleted == null ? 0 : (int)item.minutesCompleted,
                        pendingCalls = item.pendingCalls == null ? 0 : (int)item.pendingCalls,
                        missingAudioCalls = item.missingAudioCalls == null ? 0 : (int)item.missingAudioCalls,
                        lastLoaded = item.lastLoaded,
                        lastReviewed = item.lastReviewed,
                        oldestPending = item.oldestPending
                    });
                }
                ExportHelper.Export(propNames, export, "ScorecardCounts " + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "ScorecardCounts", userName);

            }
        }
    }
}
