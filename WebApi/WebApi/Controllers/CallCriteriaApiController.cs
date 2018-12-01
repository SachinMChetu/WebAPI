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
    public class CallCriteriaApiController : ApiController
    {
        CallCriteriaLayer objCallCriteriaAPI = new CallCriteriaLayer();

        #region Public GetAllRecordsWithPending
        /// <summary>
        /// GetAllRecordsWithPending
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


        #region Public GetCallsLoaded
        /// <summary>
        /// GetCallsLoaded
        /// </summary>
        /// <param name="callsLoaded"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetCallsLoaded")]
        [HttpPost]
        [ResponseType(typeof(CallsLoaded))]
        public List<CallLoaded> GetCallsLoaded(CallsLoaded callsLoaded)
        {
            List<CallLoaded> objCallLoaded = new List<CallLoaded>();
            try
            {
                objCallLoaded = objCallCriteriaAPI.GetCallsLoaded(callsLoaded);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallLoaded;
        }
        #endregion Public GetCallsLoaded

        #region Public GetRecordID
        /// <summary>
        /// GetRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetRecordID")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public AllCallRecord GetRecordID(SimpleID SI)
        {
            AllCallRecord scr = new AllCallRecord();
            try
            {
                scr = objCallCriteriaAPI.GetRecordID(SI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return scr;
        }
        #endregion Public GetRecordID

        #region Public PostRecord
        /// <summary>
        /// PostRecord
        /// </summary>
        /// <returns></returns>
        [Route("CallCriteria/PostRecord")]
        [HttpGet]
        public string PostRecord()
        {
            string Message = "";
            try
            {
                Message = objCallCriteriaAPI.PostRecord();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public PostRecord

        #region Public GetRecord
        /// <summary>
        /// GetRecord
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetRecord")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public List<CallRecord> GetRecord(SimpleID SI)
        {
            List<CallRecord> objCallRecord = new List<CallRecord>();
            try
            {
                objCallRecord = objCallCriteriaAPI.GetRecord(SI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallRecord;
        }
        #endregion Public GetRecord

        #region Public AddRecord
        /// <summary>
        /// AddRecord
        /// </summary>
        /// <param name="ADR"></param>
        /// <returns></returns>
        [Route("CallCriteria/AddRecord")]
        [HttpPost]
        [ResponseType(typeof(AddRecordData))]
        public string AddRecord(AddRecordData ADR)
        {
            string Message = string.Empty;
            try
            {
                Message = objCallCriteriaAPI.AddRecord(ADR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public AddRecord


        #region Public GetScore
        /// <summary>
        /// GetScore
        /// </summary>
        /// <param name="getScoreData"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetScore")]
        [HttpPost]
        [ResponseType(typeof(getScoreData))]
        public List<SessionStatus> GetScore(getScoreData getScoreData)
        {
            List<SessionStatus> objSessionStatus = new List<SessionStatus>();
            try
            {
                objSessionStatus = objCallCriteriaAPI.GetScore(getScoreData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objSessionStatus;
        }
        #endregion Public GetScore

        #region Public GetScorecardRecordID
        /// <summary>
        /// GetScorecardRecordID
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetScorecardRecordID")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public CompleteScorecard GetScorecardRecordID(SimpleID simpleID)
        {
            CompleteScorecard bojCompleteScorecard = new CompleteScorecard();
            try
            {
                bojCompleteScorecard = objCallCriteriaAPI.GetScorecardRecordID(simpleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bojCompleteScorecard;
        }
        #endregion Public GetScorecardRecordID

        #region Public GetScorecardRecord
        /// <summary>
        /// GetScorecardRecord
        /// </summary>
        /// <param name="getSCRecData"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetScorecardRecord")]
        [HttpPost]
        [ResponseType(typeof(getSCRecData))]
        public CompleteScorecard GetScorecardRecord(getSCRecData getSCRecData)
        {
            CompleteScorecard bojCompleteScorecard = new CompleteScorecard();
            try
            {
                bojCompleteScorecard = objCallCriteriaAPI.GetScorecardRecord(getSCRecData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bojCompleteScorecard;
        }
        #endregion Public GetScorecardRecord


        #region Public GetScorecardRecordJ
        /// <summary>
        /// GetScorecardRecordJ
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CallCriteria/GetScorecardRecordJ")]
        [HttpPost]
        [ResponseType(typeof(GCR))]
        public CompleteScorecard GetScorecardRecordJ(GCR SI)
        {
            CompleteScorecard bojCompleteScorecard = new CompleteScorecard();
            try
            {
                bojCompleteScorecard = objCallCriteriaAPI.GetScorecardRecordJ(SI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bojCompleteScorecard;
        }
        #endregion Public GetScorecardRecordJ
    }
}
