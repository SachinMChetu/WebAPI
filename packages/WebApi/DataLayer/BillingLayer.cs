using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DAL.Models;

namespace WebApi.DataLayer
{
    public class BillingLayer
    {
        public BillingResponseData GetBillingInfoRequestData()
        {

            var brd = new BillingResponseData();

            using (var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName;
                if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                else
                    userName = HttpContext.Current.User.Identity.Name;


                var sqlComm = new SqlCommand();
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = "getAllBilling";
                sqlComm.Parameters.AddWithValue("@username", userName);

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

                return brd;
            }


        }

    }
}
