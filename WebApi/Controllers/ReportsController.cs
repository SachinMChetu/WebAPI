using DAL.Code;
using DAL.DataLayer;
using DAL.Extensions;
using DAL.Layers;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Code;

namespace WebApi.Controllers
{
    /// <summary>
    /// ReportsController
    /// </summary>
    public class ReportsController : ApiController
    {
        #region Filters
        /// <summary>
        /// GetAllReportFilters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("Reports/GetAllReportFilters")]
        [HttpPost]
        [ResponseType(typeof(FiltersFormDataReports))]
        public dynamic GetAllReportfilters([FromBody]FilterReports filters)
        {
            var userName = HttpContext.Current.GetUserName();
            ReportsLayer reportsLayer = new ReportsLayer();
            var response = reportsLayer.GetAllReportsFilters(filters, userName);
            return response;
        }
        #endregion

        #region Agent reports
        /// <summary>
        /// GetAgentReport
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("Reports/GetAgentReport")]
        [HttpPost]
        [ResponseType(typeof(CallDetails))]
        public dynamic GetAgentReport([FromBody]AverageReportsFilters filters)
        {
            var userName = HttpContext.Current.GetUserName();
            ReportsLayer reportsLayer = new ReportsLayer();
            var response = reportsLayer.GetAgentReport(filters, userName);
            return response;
        }

        /// <summary>
        /// GetAgentDetailReport
        /// </summary>
        /// <returns></returns>
        [Route("Reports/GetAgentDetailReport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAgentDetailReport()
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// GetWeeklyAgentPerformance
        /// </summary>
        /// <returns></returns>
        [Route("Reports/GetWeeklyAgentPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetWeeklyAgentPerformance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetMonthlyAgentPerformance
        /// </summary>
        /// <returns></returns>
        [Route("Reports/GetMonthlyAgentPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetMonthlyAgentPerformance()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Group reports
        [Route("Reports/GetGroupPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetGroupPerformance()
        {
            throw new NotImplementedException();
        }



        [Route("Reports/GetCampaignPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetCampaignPerformance()
        {
            throw new NotImplementedException();
        }


        [Route("Reports/GetWeeklyGroupPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetWeeklyGroupPerformance()
        {
            throw new NotImplementedException();
        }


        [Route("Reports/GetMonthlyGroupPerformance")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetMonthlyGroupPerformance()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Scorecards reports

        [Route("Reports/GetScorecardDetailedReportWoW")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetScorecardDetailedReportWoW()
        {
            throw new NotImplementedException();
        }

        [Route("Reports/GetScorecardDetailedReportMoM")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetScorecardDetailedReportMoM()
        {
            throw new NotImplementedException();
        }

        [Route("Reports/BadCallsReport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic BadCallsReport()
        {
            throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// NOT READY YET, NEED MORE INFORMATION
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Route("Reports/SendReport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic SendReport([FromBody]List<User> users)
        {
            var userName = HttpContext.Current.GetUserName();

            List<string> emails = new List<string>();
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                for (var i = 0; i < users.Count; i++)
                {
                    var sqlComm = new SqlCommand("select top 1 email_address from UserExtraInfo where username = @userName")
                    {
                        CommandTimeout = 41,
                        Connection = sqlCon
                    };
                    sqlComm.Parameters.AddWithValue("@userName", users[i].userName);
                    sqlCon.Open();
                    var reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        emails.Add(reader.GetValue(reader.GetOrdinal("email_address")).ToString());
                    }
                    sqlCon.Close();
                }
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(
              ct =>
              {
                  var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
                  var cancellationToken = linkedTokenSource.Token;
                  return Task.Factory.StartNew(() =>
                  {
                      for (var i = 0; i < emails.Count; i++)
                      {
                          //if (emails[i] != string.Empty)
                          //{
                          //    using (MailMessage mail = new MailMessage("miko53254@gmail.com", emails[i]))
                          //    {
                          //        SmtpClient client = new SmtpClient
                          //        {
                          //            Port = 25,
                          //            DeliveryMethod = SmtpDeliveryMethod.Network,
                          //            UseDefaultCredentials = false,
                          //            Host = emails[i]
                          //        };
                          //        mail.Subject = "this is a test email.";
                          //        mail.Body = "this is my test email body";
                          //        client.Send(mail);
                          //    }
                          //}
                          var from = "";
                          var to = "";
                          var subject = "";
                          var body = "";
                          var smtpServer = "";
                          var login = "";
                          var password = "";
                          MailMessage mail = new MailMessage(from, to, subject, body);

                          SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                          {
                              Port = 587,//465
                              Credentials = new System.Net.NetworkCredential(login, password),
                              EnableSsl = true
                          };
                          smtpClient.Send(mail);
                      }
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }
 
            throw new NotImplementedException();
        }
    }
}