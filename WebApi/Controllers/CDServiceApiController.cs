using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using WebApi.Models.CDService;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [BasicAuthenticationAttribute]
    [RoutePrefix("v2.4")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CDServiceApiController : ApiController
    {

        CDServiceLayer objCDServiceLayer = new CDServiceLayer();

        #region Public GetNotificationStatus
        /// <summary>
        /// GetNotificationStatus
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetNotificationStatus")]
        [HttpPost]
        public string GetNotificationStatus(string start_date, string end_date, string hdnAgentFilter, string filter_array = "") // List(Of ScorePerf)
        {
            string Message = string.Empty;
            try
            {
                Message = objCDServiceLayer.GetNotificationStatus(start_date, end_date, hdnAgentFilter, filter_array);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetNotificationStatus

        #region Public GetTranscriptID
        /// <summary>
        /// GetTranscriptID
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetTranscriptID")]
        [HttpPost]
        [ResponseType(typeof(TranscriptData))]
        public TranscriptData GetTranscriptID(string ID)
         {
            TranscriptData objTranscriptData = new TranscriptData();
            try
            {
                objTranscriptData = objCDServiceLayer.GetTranscriptID(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTranscriptData;
        }
        #endregion Public GetTranscriptID


        #region Public GetSpotCheckData
        /// <summary>
        /// GetSpotCheckData
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetSpotCheckData")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public string GetSpotCheckData(string start_date, string end_date, string scorecard, string appname, string team_lead)
        {
            string Message = string.Empty;
            try
            {
                Message = objCDServiceLayer.GetSpotCheckData( start_date,  end_date,  scorecard,  appname,  team_lead);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetSpotCheckData

        //#region Public GetActionButtons
        ///// <summary>
        ///// GetSpotCheckData
        ///// </summary>
        ///// <returns></returns>
        //[Route("CDService/GetActionButtons")]
        //[HttpPost]
        //[ResponseType(typeof(ActionButton))]
        //public List<ActionButton> GetActionButtons(string username, string f_id)
        //{
        //    List<ActionButton> objActionButton = new List<ActionButton>();
        //    try
        //    {
        //        objActionButton = objCDServiceLayer.GetActionButtons(username, f_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return objActionButton;
        //}
        //#endregion Public GetActionButtons

        #region Public getChat
        /// <summary>
        /// getChat
        /// </summary>
        /// <returns></returns>
        [Route("CDService/getChat")]
        [HttpPost]
        [ResponseType(typeof(Chat))]
        public List<Chat> getChat(string session_id)
        {
            List<Chat> objChat = new List<Chat>();
            try
            {
                objChat = objCDServiceLayer.getChat(session_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objChat;
        }
        #endregion Public getChat

        #region Public GetTranscript
        /// <summary>
        /// GetTranscript
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetTranscript")]
        [HttpPost]
        [ResponseType(typeof(TranscriptData))]
        public TranscriptData GetTranscript(string xcc_id)
        {
            TranscriptData objTranscriptData = new TranscriptData();
            try
            {
                objTranscriptData = objCDServiceLayer.GetTranscript(xcc_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTranscriptData;
        }
        #endregion Public GetTranscript

        //#region Public GetAvailableAudios
        ///// <summary>
        ///// GetNotificationSteps
        ///// </summary>
        ///// <returns></returns>
        //[Route("CDService/GetAvailableAudios")]
        //[HttpPost]
        //public List<string> GetAvailableAudios(string xcc_id)
        //{
        //    List<string> Message;
        //    try
        //    {
        //        Message = CDServiceLayer.GetAvailableAudios(xcc_id);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Message;
        //}
        //#endregion Public GetAvailableAudios

        #region Public GetNotificationSteps
        /// <summary>
        /// GetNotificationSteps
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetNotificationSteps")]
        [HttpPost]
        public string GetNotificationSteps(string form_id) 
        {
            string Message = string.Empty;
            try
            {
                Message = objCDServiceLayer.GetNotificationSteps(form_id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetNotificationSteps

        #region Public updateUserInfo
        /// <summary>
        /// updateUserInfo
        /// </summary>
        /// <returns></returns>
        [Route("CDService/updateUserInfo")]
        [HttpPost]
        public string updateUserInfo(string value, string field)
        {
            string Message = string.Empty;
            try
            {
                Message = objCDServiceLayer.updateUserInfo(value, field);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public updateUserInfo
    }
}
