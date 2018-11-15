using DAL.GenericRepository;
using DAL.Models;
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
    public class ExportBillingCode
    {
        public dynamic ExportBilling(string userName, DateTime? date = null)
        {
            var brd = new BillingResponseData();

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                var sqlComm = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[getAllBillingForExport]"
                };
                sqlComm.Parameters.AddWithValue("@username", userName);
                if (date != null)
                {
                    sqlComm.Parameters.AddWithValue("@month", date.Value.Month);
                    sqlComm.Parameters.AddWithValue("@year", date.Value.Year);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@month", 0);
                    sqlComm.Parameters.AddWithValue("@year", 0);
                }
               
                sqlComm.Connection = sqlCon;

                sqlCon.Open();
                var sda = new SqlDataAdapter(sqlComm);
                var ds = new DataSet();
                sda.Fill(ds);
                brd.months = new List<BillingData>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                //try
                {

                    var billingData = new BillingData()
                    {
                        date = new DateTime(dr.Field<int>("bill_year"), dr.Field<int>("bill_month"), 1),
                        currentBillableRate = dr.Field<double?>("rate"),
                        billableTime = dr.Field<int?>("billable_time"),
                        successfulTime = dr.Field<int?>("successful_time"),
                        badCallsTime = dr.Field<int?>("bad_call_length"),
                        transcriptMinutes = dr.Field<int?>("transcript_time"),
                        chatsReviewed = dr.Field<int?>("chats_reviewed"),
                        websitesReviewed = dr.Field<int?>("websites_reviewed"),
                        chatsCost = dr.Field<double?>("chat_cost"),
                        websitesCost = dr.Field<double?>("website_cost")

                    };
                    brd.months.Add(billingData);
                }
                //catch (Exception)
                //{

                //    //throw ex;
                //}

                brd.cpmBillableRate = new List<BillableRate>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    var billingRate = new BillableRate()
                    {
                        rate = dr.Field<double?>("rate"),
                        minutesFrom = dr.Field<int?>("start_minutes"),
                        minutesTo = dr.Field<int?>("end_minutes")

                    };
                    brd.cpmBillableRate.Add(billingRate);
                }

                brd.transcriptRate = (double?)ds.Tables[2].Rows[0].ItemArray[0];
                brd.minimumMinutes = (int?)ds.Tables[2].Rows[0].ItemArray[1];
                brd.budget = (double?)ds.Tables[2].Rows[0].ItemArray[2];


                //return brd;
                try
                {
                    List<ExportBillingModel> exportBillingModel = new List<ExportBillingModel>();
                    var propNames = new List<PropertieName>
                    {
                        new PropertieName { propName = "Date", propValue = "date", propPosition = 1 },
                        new PropertieName { propName = "Successful time", propValue = "successfulTime", propPosition = 2 },
                        new PropertieName { propName = "Billable time", propValue = "billableTime", propPosition = 3 },
                        new PropertieName { propName = "Current billable rate", propValue = "currentBillableRate", propPosition = 4 },
                        new PropertieName { propName = "Bad calls time", propValue = "badCallsTime", propPosition = 5 },
                        new PropertieName { propName = "Transcript minutes", propValue = "transcriptMinutes", propPosition = 6 },
                        new PropertieName { propName = "Chats reviwed", propValue = "chatsReviewed", propPosition = 7 },
                        new PropertieName { propName = "Websites reviewed", propValue = "websitesReviewed", propPosition = 8 }
                };
                    foreach (var item in brd.months)
                    {
                        exportBillingModel.Add(new ExportBillingModel
                        {
                            date = item.date,
                            successfulTime = item.successfulTime,
                            billableTime = item.billableTime,
                            currentBillableRate = item.currentBillableRate,
                            badCallsTime = item.badCallsTime,
                            transcriptMinutes = item.transcriptMinutes,
                            chatsReviewed = item.chatsReviewed,
                            websitesReviewed = item.websitesReviewed
                        });
                    }
                    ExportHelper.Export(propNames, exportBillingModel, "Billing" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx", "Billing", userName);
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
