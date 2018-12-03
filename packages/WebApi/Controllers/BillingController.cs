using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using System.Web.Hosting;
using System.Threading;
using System.Threading.Tasks;
using DAL.Export;

namespace WebApi.Controllers
{
    public class BillingController : ApiController
    {
        /// <summary>
        /// GetBillingInfoRequestData
        /// </summary>
        /// <returns></returns>
        [Route("Billing/GetBilling")]
        [HttpPost]
        [ResponseType(typeof(GuidelineScorecardInfo))]
        public BillingResponseData GetBillingInfoRequestData()
        {
            BillingLayer billingLayer = new BillingLayer();
           return billingLayer.GetBillingInfoRequestData();
        }
        /// <summary>
        /// ExportBilling
        /// </summary>
        /// <returns></returns>
        [Route("Billing/ExportBilling")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic ExportBilling([FromBody]DateTime? date)
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "winnie";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
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
                      ExportBillingCode exportBillingCode = new ExportBillingCode();
                      exportBillingCode.ExportBilling(userName, date);
                  }, cancellationToken);
              });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok("success");
        }
    }

}