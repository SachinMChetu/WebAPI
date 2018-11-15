using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using DAL.Code;
using DAL.DataLayer;
using DAL.Models;
using DAL;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DAL.Code
{
    public class FileHelper
    {
        public static async Task<dynamic> GenerateReport(CallDetailsExportFilter filters, List<string> columns, string userName, string moduleName)
        {
            // var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            string p = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~\export\"));
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                #region filters

                //var dbconn = new WebApi.Entities.Entities();

                SqlCommand sqlComm = new SqlCommand();

                sqlComm.Connection = sqlCon;
                if (filters.filters.pendingOnly == true) {
                    sqlComm.CommandText = "GetPendingCalls";
                        }
                else
                {
                    sqlComm.CommandText = "[getDetailDataArrayJson_mitemsV4]";
                }
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@userName", userName);
                if (filters.filters.scorecards != null && (filters.filters.scorecards.Count > 0))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.scorecards)
                    {
                        preparedLst.Append(("'" + (value + "',")));
                    }
                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@scorecardIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.filters.campaigns != null && ((filters.filters.campaigns.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.campaigns)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }
                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@campaignIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.filters.groups != null && (filters.filters.groups.Count > 0))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.groups)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@groupIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.filters.agents != null && ((filters.filters.agents.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.agents)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }

                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@agentIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.filters.QAs != null && ((filters.filters.QAs.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.QAs)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }
                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@qaIDs", preparedLst.ToString().Trim(','));
                    }
                }

                if (filters.filters.missedItems != null && ((filters.filters.missedItems.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.missedItems)
                    {
                        preparedLst.Append(((value + ",")));
                    }
                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@missedItemsIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.filters.teamLeads != null && ((filters.filters.teamLeads.Count > 0)))
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filters.filters.teamLeads)
                    {
                        preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                    }
                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@teamLeadIDs", preparedLst.ToString().Trim(','));
                    }
                }
                if (filters.sorting != null)
                {
                    if (filters.sorting.sortBy != null && filters.sorting.sortOrder != null && filters.sorting.sortBy != "" && filters.sorting.sortOrder != "")
                    {

                        sqlComm.Parameters.AddWithValue("@OrderByColumn", filters.sorting.sortBy);
                        sqlComm.Parameters.AddWithValue("@sortOrder", (filters.sorting.sortOrder == "desc") ? (false) : (true));
                    }
                }
              
                if (filters.search != null)//  @searchstr varchar(100)= '',
                {
                    if (filters.search.columns != null && filters.search.text != null && filters.search.text != "" && filters.search.columns.Count > 0)
                    {
                        var preparedLst = new StringBuilder();

                        sqlComm.Parameters.AddWithValue("@searchstr", filters.search.text);
                        foreach (var value in filters.search.columns)
                        {
                            preparedLst.Append(("'" + (value.Replace(",", "<!@!>") + "',")));
                        }

                        sqlComm.Parameters.AddWithValue("@searchColumn", preparedLst.ToString());
                    }
                }

                try
                {
                    sqlComm.Parameters.AddWithValue("@badCallOnly", filters.filters.badCallsOnly);
                }
                catch { }
                sqlComm.Parameters.AddWithValue("@failed", filters.filters.failedOnly);
                sqlComm.Parameters.AddWithValue("@missedBy", filters.filters.missedBy);

                sqlComm.Parameters.AddWithValue("@reviewType", filters.filters.reviewType);
                sqlComm.Parameters.AddWithValue("@filterByReviewDate", filters.filters.filterByReviewDate);
                sqlComm.Parameters.AddWithValue("@passedOnly", filters.filters.passedOnly);
                sqlComm.Parameters.AddWithValue("@pagerows", int.MaxValue);
                sqlComm.Parameters.AddWithValue("@pagenum", 1);
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
                //sqlComm.Parameters.AddWithValue("@Export", true);
                #endregion





                sqlComm.CommandTimeout = int.MaxValue;

                sqlCon.Open();
                sqlComm.CommandTimeout = int.MaxValue;
                string filename = "CallDetails " + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx";
                string sql = "insert into exportQueue([fileowner], [fileNAme],[fileUrl],[exportDate],[status],[exportType]) OUTPUT inserted.id as id  select top 1 id ,'" + filename
                    + "','webapi/export/" + filename + "',getdate(),1 ,'" + moduleName + "' from userextrainfo where [username]='" + userName + "';";

                int recordId = 0;

                SqlCommand sqlInsertComm = new SqlCommand(sql, sqlCon);

                sqlInsertComm.CommandText = sql;
                try
                {
                    SqlDataReader r = sqlInsertComm.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            recordId = int.Parse(r.GetValue(r.GetOrdinal("id")).ToString());
                        }
                        catch
                        {

                        }
                    }
                }
                catch 
                {

                }
                string updatesql = "update exportQueue set[status] = 0 where id =" + recordId;

                CallDetails callDetailsList = new CallDetails();
                SqlDataReader reader = sqlComm.ExecuteReader();
                //var watch2 = System.Diagnostics.Stopwatch.StartNew();
              
                    try
                    {
                     callDetailsList = CallDetails.Create(reader);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                try
                {
                    SettingsLayer settings = new SettingsLayer();

                    var customColumns = await settings.GetUserCollums(userName, sqlCon);

                    var workbook = new XLWorkbook();

                    var worksheet = workbook.Worksheets.Add("Calls");

                    int cell = 1;



                    //-  worksheet.Cell(1, cell).Value = "Review Status";
                    //  cell++;
                    foreach (var a in filters.columns)
                    {
                        if (a == "callId")
                        {
                            worksheet.Cell(1, cell).Value = "callId";
                            cell++;
                        }
                        if (a == ("callType"))
                        {
                            worksheet.Cell(1, cell).Value = "Type";
                            cell++;
                        }

                        if (a == ("callAudioLength"))
                        {
                            worksheet.Cell(1, cell).Value = "Duration";
                            cell++;
                        }
                        if (a == ("scorecardName"))
                        {
                            worksheet.Cell(1, cell).Value = "Scorecard";
                            cell++;
                        }
                        if (a == ("reviewDate"))
                        {
                            worksheet.Cell(1, cell).Value = "Review Date";
                            cell++;
                        }
                        if (a == ("receivedDate"))
                        {
                            worksheet.Cell(1, cell).Value = "Received Date";
                            cell++;
                        }
                        if (a == ("reviewerName"))
                        {
                            worksheet.Cell(1, cell).Value = "Reviewer";
                            cell++;
                        }
                        if (a == ("agentScore"))
                        {
                            worksheet.Cell(1, cell).Value = "Score";
                            cell++;
                        }
                        if (a == ("callDate"))
                        {
                            worksheet.Cell(1, cell).Value = "call Date";
                            cell++;
                        }
                        if (a == ("missedItemsCount"))
                        {
                            worksheet.Cell(1, cell).Value = "Missed Items count";
                            cell++;
                            worksheet.Cell(1, cell).Value = "Missed Items";
                            cell++;
                            worksheet.Cell(1, cell).Value = "Answer Comments";
                            cell++;
                        }
                        if (a == ("agentName"))
                        {
                            worksheet.Cell(1, cell).Value = "Agent";
                            cell++;
                        }
                        if (a == ("agentId"))
                        {
                            worksheet.Cell(1, cell).Value = "Agent Id";
                            cell++;
                        }
                        if (a == ("badCallReason"))
                        {
                            worksheet.Cell(1, cell).Value = "Bad Call Reason";
                            cell++;
                        }
                        if (a == ("sessionId"))
                        {
                            worksheet.Cell(1, cell).Value = "Session ID";
                            cell++;
                        }
                        if (a == ("prospectPhone"))
                        {
                            worksheet.Cell(1, cell).Value = "Phone";
                            cell++;
                        }
                        if (a == ("callReviewStatus"))
                        {
                            worksheet.Cell(1, cell).Value = "Review Status";
                            cell++;
                        }
                        if (a == ("callFailed"))
                        {
                            if (filters.filters.badCallsOnly != true)
                            {

                                worksheet.Cell(1, cell).Value = "Result";
                                cell++;

                            }
                            else
                            {
                                worksheet.Cell(1, cell).Value = "Bad call reason";
                                cell++;
                            }

                        }
                        if (a == ("agentGroup"))
                        {
                            worksheet.Cell(1, cell).Value = "Group";
                            cell++;
                        }
                        if (a == ("campaign"))
                        {
                            worksheet.Cell(1, cell).Value = "Campaign";
                            cell++;
                        }
                        if (a == ("prospectEmail"))
                        {
                            worksheet.Cell(1, cell).Value = "Email";
                            cell++;
                        }
                        if (a == ("prospectLastName"))
                        {
                            worksheet.Cell(1, cell).Value = "Last Name";
                            cell++;
                        }
                        if (a == ("prospectFirstName"))
                        {
                            worksheet.Cell(1, cell).Value = "First Name";
                            cell++;
                        }
                        if (a == ("profileId"))
                        {
                            worksheet.Cell(1, cell).Value = "Profile ID";
                            cell++;
                        }
                    
                        foreach (var ccol in customColumns)
                        {
                            var cname = (dynamic)ccol.value;

                            if (a == (dynamic)ccol.id)
                            {

                                worksheet.Cell(1, cell).Value = (dynamic)ccol.value;
                                cell++;
                            }
                        }
                    }




                    if (callDetailsList.calls.Count > 0)
                    {
                        int row = 1;
                        try

                        {
                            foreach (var call in callDetailsList.calls)
                            {

                                cell = 1;
                                row++;
                                //worksheet.Cell(row, cell).Value = call.systemData.callReviewStatus;
                                //cell++;
                                foreach (var a in filters.columns)
                                {
                                    if (a == ("callType"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.callType;
                                        cell++;
                                    }
                                    if (a == "callId")
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.callId;
                                        cell++;
                                    }
                                    if (a == "badCallReason")
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.badCallReason;
                                        cell++;
                                    }
                                    if (a == ("callAudioLength"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.callAudioLength;
                                        cell++;
                                    }
                                    if (a == ("scorecardName"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.scorecardName;
                                        cell++;
                                    }
                                    if (a == ("reviewDate"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.reviewDate;
                                        cell++;
                                    }
                                    if (a == ("receivedDate"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.receivedDate;
                                        cell++;
                                    }
                                    if (a == ("reviewerName"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.reviewerName;
                                        cell++;
                                    }
                                    if (a == ("agentScore"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.agentScore;
                                        cell++;
                                    }
                                    if (a == ("callDate"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.callDate;
                                        cell++;
                                    }
                                    if (a == ("missedItemsCount"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.missedItemsCount;
                                        cell++;
                                        worksheet.Cell(row, cell).Value = call.systemData.missedItemsList;
                                        cell++;
                                        worksheet.Cell(row, cell).Value = call.systemData.missedItemsCommentList;
                                        cell++;
                                    }
                                    if (a == ("agentName"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.agentName;
                                        cell++;
                                    }
                                    if (a == ("agentId"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.agentId;
                                        cell++;
                                    }
                                    if (a == ("sessionId"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.sessionId;
                                        cell++;
                                    }
                                    if (a == ("prospectPhone"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.prospectPhone;
                                        cell++;
                                    }
                                    if (a == ("callReviewStatus"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.systemData.callReviewStatus;
                                        cell++;
                                    }
                                
                                    if (a == "callFailed")
                                    {
                                        if (filters.filters.badCallsOnly != true)
                                        {
                                            worksheet.Cell(row, cell).Value = (bool)(call.systemData.callFailed) ? ("Fail") : ("Pass");
                                            cell++;
                                        }
                                        else
                                        {
                                            worksheet.Cell(row, cell).Value = (call.systemData.badCallReason);
                                            cell++;
                                        }
                                    }
                                    if (a == ("agentGroup"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.agentGroup;
                                        cell++;
                                    }
                                    if (a == ("campaign"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.campaign;
                                        cell++;
                                    }
                                    if (a == ("prospectEmail"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.prospectEmail;
                                        cell++;
                                    }
                                    if (a == ("prospectLastName"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.prospectLastName;
                                        cell++;
                                    }
                                    if (a == ("prospectFirstName"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.prospectFirstName;
                                        cell++;
                                    }
                                    if (a == ("profileId"))
                                    {
                                        worksheet.Cell(row, cell).Value = call.metaData.profileId;
                                        cell++;
                                    }
                                  
                                    foreach (var ccol in customColumns)
                                    {
                                        if (a == (dynamic)ccol.id)
                                        {
                                            try
                                            {
                                                worksheet.Cell(row, cell).Value = string.Empty;
                                                worksheet.Cell(row, cell).Value = ((IDictionary<string, object>)call.customData)[ccol.id];
                                            }
                                            catch (Exception)
                                            {
                                                //igore 
                                            }
                                            cell++;

                                        }
                                    }

                                }
                            }

                          
                            workbook.Worksheets.Delete("Calls");
                            workbook.AddWorksheet(worksheet);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        try
                        {
                            var stream1 = new MemoryStream();

                            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~\export\"), filename);
                            var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                            workbook.SaveAs(fileStream, new SaveOptions() { ValidatePackage = true });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        try
                        {
                            SqlCommand updateSql = new SqlCommand(updatesql, sqlCon);
                            SqlDataReader u = updateSql.ExecuteReader();
                        }
                        catch (Exception ex) { throw ex; }


                    }
                    // watch2.Stop();
                    // var elapsedMss = watch2.ElapsedMilliseconds;
                }
                catch (Exception ex) { throw ex; }
                // watch.Stop();

                // var elapsedMs = watch.ElapsedMilliseconds;
                return "";
            }
        }
                       
        }
    }


