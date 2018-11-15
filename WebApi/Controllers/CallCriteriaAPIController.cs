using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using WebApi.Models.CallCriteriaAPI;

namespace WebApi.Controllers
{
    /// <summary>
    /// CCInternalApiController
    /// </summary>
    [BasicAuthenticationAttribute]
    [RoutePrefix("v2.4")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CallCriteriaAPIController : ApiController
    {
        CallCriteriaAPI objCallCriteriaAPI = new CallCriteriaAPI();

        #region Public GetAllRecordsWithPending
        /// <summary>
        /// GetAllRecords
        /// </summary>
        /// <returns></returns>
        [Route("CallCriteria/GetAllRecordsWithPending")]
        [HttpPost]
        [ResponseType(typeof(GetAllRecordData))]
        public List<CallRecord> GetAllRecordsWithPending(GetAllRecordData GARD)
        {
            List<CallRecord> objCallRecord = new List<CallRecord>();
            DateTime call_date =new DateTime();
            string appname = HttpContext.Current.Request["appname"];
            string use_review = GARD.use_review;
            if(GARD.use_review !="")
            {
                 call_date =Convert.ToDateTime(GARD.call_date);
            }
            bool rev_date = false;
            if (use_review == null)
                rev_date = false;
            switch (use_review)
            {
                case "1":
                case "true":
                case "True":
                    {
                        rev_date = true;
                        break;
                    }
            }
            try
            {
                objCallRecord = objCallCriteriaAPI.GetAllRecordsWithPending(call_date, rev_date, appname, use_review);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallRecord;
        }
        #endregion Public GetAllRecordsWithPending
    }
}
