using DAL.Code;
using DAL.Extensions;
using DAL.GenericRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace DAL.Export
{
    public class ExportCoachingQueueCode
    {
        public dynamic CoachingQueueExport(Filter filters, string userName, List<AvailableColumns> columns)
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
              
                sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getCoachingQueueJsonv2]", userName);

                List<ExportCoachingQueueColumns> exportCoachingQueueColumns = new List<ExportCoachingQueueColumns>();

                sqlComm.Connection = sqlCon;
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            exportCoachingQueueColumns.Add(new ExportCoachingQueueColumns
                            {
                                agentName = reader.IsDBNull(reader.GetOrdinal("agentName")) ? null : reader.GetValue(reader.GetOrdinal("agentName")).ToString(),
                                callDate = reader.IsDBNull(reader.GetOrdinal("callDate")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("callDate")).ToString()),
                                agentScore = reader.IsDBNull(reader.GetOrdinal("total_score")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("total_score")).ToString()),
                                assignedToRole = reader.IsDBNull(reader.GetOrdinal("AssignedToRole")) ? (string)null : reader.GetValue(reader.GetOrdinal("AssignedToRole")).ToString(),
                                callType = reader.IsDBNull(reader.GetOrdinal("calltype")) ? (string)null : reader.GetValue(reader.GetOrdinal("calltype")).ToString(),
                                callId = reader.IsDBNull(reader.GetOrdinal("callId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("callId")).ToString()),
                                callFailed = (reader.IsDBNull(reader.GetOrdinal("callFailed")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("callFailed")).ToString())) == true ? "Yes" : "No",
                                callAudioLength = reader.IsDBNull(reader.GetOrdinal("callAudioLength")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("callAudioLength")).ToString()),
                                callAudioUrl = reader.IsDBNull(reader.GetOrdinal("callAudioUrl")) ? (string)null : reader.GetValue(reader.GetOrdinal("callAudioUrl")).ToString(),
                                cali_id = reader.IsDBNull(reader.GetOrdinal("cali_id")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("cali_id")).ToString()),
                                calibratorName = reader.IsDBNull(reader.GetOrdinal("calibratorName")) ? (string)null : reader.GetValue(reader.GetOrdinal("calibratorName")).ToString(),
                                callReviewStatus = reader.IsDBNull(reader.GetOrdinal("callReviewStatus")) ? (string)null : reader.GetValue(reader.GetOrdinal("callReviewStatus")).ToString(),
                                missedItemsCount = reader.IsDBNull(reader.GetOrdinal("missedItemsCount")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("missedItemsCount")).ToString()),
                                notificationId = reader.IsDBNull(reader.GetOrdinal("NotificationID")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("NotificationID")).ToString()),
                                reviewDate = reader.IsDBNull(reader.GetOrdinal("reviewDate")) ? (DateTime?)null : DateTime.Parse(reader.GetValue(reader.GetOrdinal("reviewDate")).ToString()),
                                reviewerUserRole = reader.IsDBNull(reader.GetOrdinal("reviewerUserRole")) ? (string)null : reader.GetValue(reader.GetOrdinal("reviewerUserRole")).ToString(),
                                reviewerName = reader.IsDBNull(reader.GetOrdinal("reviewerName")) ? (string)null : reader.GetValue(reader.GetOrdinal("reviewerName")).ToString(),
                                reviewCommentsPresent = (reader.IsDBNull(reader.GetOrdinal("reviewCommentsPresent")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString())) == true ? "Yes" : "No",
                                notificationCommentsPresent = (reader.IsDBNull(reader.GetOrdinal("notificationCommentsPresent")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString())) == true ? "Yes" : "No",
                                scorecardFailScore = reader.IsDBNull(reader.GetOrdinal("scorecardFailScore")) ? (float?)null : float.Parse(reader.GetValue(reader.GetOrdinal("scorecardFailScore")).ToString()),
                                scorecardId = reader.IsDBNull(reader.GetOrdinal("scorecardId")) ? (int?)null : int.Parse(reader.GetValue(reader.GetOrdinal("scorecardId")).ToString()),
                                scorecardName = reader.IsDBNull(reader.GetOrdinal("scorecardName")) ? (string)null : reader.GetValue(reader.GetOrdinal("scorecardName")).ToString(),
                                websiteUrl = reader.IsDBNull(reader.GetOrdinal("websiteUrl")) ? (string)null : reader.GetValue(reader.GetOrdinal("websiteUrl")).ToString(),
                                agentGroup = reader.IsDBNull(reader.GetOrdinal("agentGroup")) ? (string)null : reader.GetValue(reader.GetOrdinal("agentGroup")).ToString(),
                                campaign = reader.IsDBNull(reader.GetOrdinal("campaign")) ? (string)null : reader.GetValue(reader.GetOrdinal("campaign")).ToString(),
                                sessionId = reader.IsDBNull(reader.GetOrdinal("sessionId")) ? (string)null : reader.GetValue(reader.GetOrdinal("sessionId")).ToString(),
                                profileId = reader.IsDBNull(reader.GetOrdinal("profileId")) ? (string)null : reader.GetValue(reader.GetOrdinal("profileId")).ToString(),
                                prospectFirstName = reader.IsDBNull(reader.GetOrdinal("prospectFirstName")) ? (string)null : reader.GetValue(reader.GetOrdinal("prospectFirstName")).ToString(),
                                prospectLastName = reader.IsDBNull(reader.GetOrdinal("prospectLastName")) ? (string)null : reader.GetValue(reader.GetOrdinal("prospectLastName")).ToString(),
                                prospectPhone = reader.IsDBNull(reader.GetOrdinal("prospectPhone")) ? (string)null : reader.GetValue(reader.GetOrdinal("prospectPhone")).ToString(),
                                prospectEmail = reader.IsDBNull(reader.GetOrdinal("prospectEmail")) ? (string)null : reader.GetValue(reader.GetOrdinal("prospectEmail")).ToString(),
                                wasEdited = (reader.IsDBNull(reader.GetOrdinal("wasEdit")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("wasEdit")).ToString())) == true ? "Yes" : "No",
                                notificationStatus = reader.IsDBNull(reader.GetOrdinal("notificationStatus")) ? (string)null : reader.GetValue(reader.GetOrdinal("notificationStatus")).ToString(),
                                notificationStep = reader.IsDBNull(reader.GetOrdinal("notificationStep")) ? (string)null : reader.GetValue(reader.GetOrdinal("notificationStep")).ToString(),
                                isOwnedNotification = (reader.IsDBNull(reader.GetOrdinal("isOwnedNotification")) ? (bool?)null : bool.Parse(reader.GetValue(reader.GetOrdinal("isOwnedNotification")).ToString())) == true ? "Yes" : "No",
                                OwnedNotification = (reader.IsDBNull(reader.GetOrdinal("OwnedNotification")) ? false : bool.Parse(reader.GetValue(reader.GetOrdinal("OwnedNotification")).ToString()))==true ? "Yes" : "No",
                                calibratorId = reader.IsDBNull(reader.GetOrdinal("calibratorId")) ? (string)null : reader.GetValue(reader.GetOrdinal("calibratorId")).ToString(),
                                missedItemsList = reader.IsDBNull(reader.GetOrdinal("missedItemsList")) ? (string)null : reader.GetValue(reader.GetOrdinal("missedItemsList")).ToString(),
                                missedItemsCommentList = reader.IsDBNull(reader.GetOrdinal("missedItemsCommentList")) ? (string)null : reader.GetValue(reader.GetOrdinal("missedItemsCommentList")).ToString()
                            });




                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    var propNames = new List<PropertieName>();
                    var j = 1;
                    foreach (var item in columns)
                    {
                        switch (item.id)
                        {
                            case 1:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "agentName", propPosition = j });
                                j++;
                                break;
                            case 2:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callDate", propPosition = j });
                                j++;
                                break;
                            case 3:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callType", propPosition = j });
                                j++;
                                break;
                            case 4:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callId", propPosition = j });
                                j++;
                                break;
                            case 5:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callReviewStatus", propPosition = j });
                                j++;
                                break;
                            case 6:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callAudioUrl", propPosition = j });
                                j++;
                                break;
                            case 7:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callAudioLength", propPosition = j });
                                j++;
                                break;
                            case 8:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "scorecardId", propPosition = j });
                                j++;
                                break;
                            case 9:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "scorecardName", propPosition = j });
                                j++;
                                break;
                            case 10:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "scorecardFailScore", propPosition = j });
                                j++;
                                break;
                            case 11:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "reviewDate", propPosition = j });
                                j++;
                                break;
                            case 12:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "reviewerUserRole", propPosition = j });
                                j++;
                                break;
                            case 13:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "reviewerName", propPosition = j });
                                j++;
                                break;
                            case 14:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "calibratorId", propPosition = j });
                                j++;
                                break;
                            case 15:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "calibratorName", propPosition = j });
                                j++;
                                break;
                            case 16:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "missedItemsCount", propPosition = j });
                                j++;
                                break;
                            case 17:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "agentScore", propPosition = j });
                                j++;
                                break;
                            case 18:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "callFailed", propPosition = j });
                                j++;
                                break;
                            case 19:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "reviewCommentsPresent", propPosition = j });
                                j++;
                                break;
                            case 20:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "notificationCommentsPresent", propPosition = j });
                                j++;
                                break;
                            case 21:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "agentGroup", propPosition = j });
                                j++;
                                break;
                            case 22:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "campaign", propPosition = j });
                                j++;
                                break;
                            case 23:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "sessionId", propPosition = j });
                                j++;
                                break;
                            case 24:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "profileId", propPosition = j });
                                j++;
                                break;
                            case 25:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "prospectFirstName", propPosition = j });
                                j++;
                                break;
                            case 26:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "prospectLastName", propPosition = j });
                                j++;
                                break;
                            case 27:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "prospectPhone", propPosition = j });
                                j++;
                                break;
                            case 28:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "prospectEmail", propPosition = j });
                                j++;
                                break;
                            case 29:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "cali_id", propPosition = j });
                                j++;
                                break;
                            case 30:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "wasEdit", propPosition = j });
                                j++;
                                break;
                            case 31:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "notificationStatus", propPosition = j });
                                j++;
                                break;
                            case 32:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "notificationStep", propPosition = j });
                                j++;
                                break;
                             case 33:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "notificationId", propPosition = j });
                                j++;
                                break;
                            case 34:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "isOwnedNotification", propPosition = j });
                                j++;
                                break;
                            case 35:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "OwnedNotification", propPosition = j });
                                j++;
                                break;
                            case 36:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "assignedToRole", propPosition = j });
                                j++;
                                break;
                            case 37:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "missedItemsList", propPosition = j });
                                j++;
                                break;
                            case 38:
                                propNames.Add(new PropertieName { propName = item.Name, propValue = "missedItemsCommentList", propPosition = j });
                                j++;
                                break;
                        }
                    }
                    ExportHelper.Export(propNames, exportCoachingQueueColumns, "CoachingQueue" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx","CoachingQueue", userName);

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
