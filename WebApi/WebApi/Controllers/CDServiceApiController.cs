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
        [ResponseType(typeof(DetailCountvwForm))]
        public string GetNotificationStatus(DetailCountvwForm objNotificationStatusModel)
        {
            string Message = string.Empty;
            try
            {
                    string start_date = objNotificationStatusModel.start_date;
                    string end_date = objNotificationStatusModel.end_date;
                    string hdnAgentFilter = objNotificationStatusModel.hdnAgentFilter;
                    string filter_array = objNotificationStatusModel.filter_array?.ToString() ?? "";
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
        [ResponseType(typeof(SpotCheckDataModel))]
        public string GetSpotCheckData(SpotCheckDataModel objSpotCheckDataModel)
        {
            string Message = string.Empty;
            try
            {
                string start_date = objSpotCheckDataModel.start_date;
                string end_date = objSpotCheckDataModel.end_date;
                string scorecard = objSpotCheckDataModel.scorecard;
                string appname = objSpotCheckDataModel.appname;
                string team_lead = objSpotCheckDataModel.team_lead;
                Message = objCDServiceLayer.GetSpotCheckData( start_date,  end_date,  scorecard,  appname,  team_lead);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetSpotCheckData

        #region Public GetActionButtons
        /// <summary>
        /// GetSpotCheckData
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetActionButtons")]
        [HttpPost]
        [ResponseType(typeof(ActionButton))]
        public List<ActionButton> GetActionButtons(string username, string f_id)
        {
            List<ActionButton> objActionButton = new List<ActionButton>();
            try
            {
                objActionButton = objCDServiceLayer.GetActionButtons(username, f_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objActionButton;
        }
        #endregion Public GetActionButtons

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

        #region Public GetAvailableAudios
        /// <summary>
        /// GetNotificationSteps
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetAvailableAudios")]
        [HttpPost]
        public List<string> GetAvailableAudios(string xcc_id)
        {
            List<string> Message;
            try
            {
                Message = CDServiceLayer.GetAvailableAudios(xcc_id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetAvailableAudios

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
        [ResponseType(typeof(UpdateUserModel))]
        public string updateUserInfo(UpdateUserModel objUpdateUserModel)
        {
            string Message = string.Empty;
            try
            {
                string value = objUpdateUserModel.value;
                string field = objUpdateUserModel.field;
                Message = objCDServiceLayer.updateUserInfo(value, field);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public updateUserInfo

        #region Public GetAgents
        /// <summary>
        /// GetAgents
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetAgents")]
        [HttpPost]
        [ResponseType(typeof(Campaigns))]
        public List<DBOptions> GetAgents(Campaigns objCampaigns)
        {
            List<DBOptions> objDBOptions = new List<DBOptions>();
            try
            {
                string start_date = objCampaigns.start_date;
                string end_date = objCampaigns.start_date;
                string scorecard = objCampaigns.scorecard;
                string group = objCampaigns.group;
                objDBOptions = objCDServiceLayer.GetAgents(start_date, end_date, scorecard, group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDBOptions;
        }
        #endregion Public GetAgents

        #region Public GetQualityA
        /// <summary>
        /// GetQualityA
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetQualityA")]
        [HttpPost]
        [ResponseType(typeof(GroupsModel))]
        public List<DBOptions> GetQualityA(GroupsModel objGroupsModel)
        {
            List<DBOptions> objDBOptions = new List<DBOptions>();
            try
            {
                string start_date = objGroupsModel.start_date;
                string end_date = objGroupsModel.start_date;
                string scorecard = objGroupsModel.scorecard;
                objDBOptions = objCDServiceLayer.GetQualityA(start_date, end_date, scorecard);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDBOptions;
        }
        #endregion Public GetQualityA



        #region Public GetCampaigns
        /// <summary>
        /// GetCampaigns
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetCampaigns")]
        [HttpPost]
        [ResponseType(typeof(Campaigns))]
        public List<DBOptions> GetCampaigns(Campaigns objCampaigns)
        {
            List<DBOptions> objDBOptions = new List<DBOptions>();
            try
            {
                string start_date = objCampaigns.start_date;
                string end_date = objCampaigns.start_date;
                string scorecard = objCampaigns.scorecard;
                string group = objCampaigns.group;
                objDBOptions = objCDServiceLayer.GetCampaigns(start_date, end_date, scorecard, group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDBOptions;
        }
        #endregion Public GetCampaigns

        #region Public GetDetailCount
        /// <summary>
        /// GetDetailCount
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetDetailCount")]
        [HttpPost]
        [ResponseType(typeof(DetailCountvwForm))]
        public string GetDetailCount(DetailCountvwForm objDetailCountvwForm)
        {
            string Message = string.Empty;
            try
            {
              
                string start_date = objDetailCountvwForm.start_date;
                string end_date = objDetailCountvwForm.start_date;
                string hdnAgentFilter = objDetailCountvwForm.hdnAgentFilter?.ToString() ?? "";
                string filter_array = objDetailCountvwForm.filter_array?.ToString() ?? "";
                Message = objCDServiceLayer.GetDetailCount(start_date, end_date, hdnAgentFilter, filter_array);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetDetailCount


        #region Public GetAppnames
        /// <summary>
        /// GetAppnames
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetAppnames")]
        [HttpPost]
        public List<DBOptions> GetAppnames(string filter)
        {
            List<DBOptions> objDBOptions = new List<DBOptions>();
            try
            {
                objDBOptions = objCDServiceLayer.GetAppnames(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDBOptions;
        }
        #endregion Public GetAppnames

        #region Public UpdateScorecard
        /// <summary>
        /// GetAppnames
        /// </summary>
        /// <returns></returns>
        [Route("CDService/UpdateScorecard")]
        [HttpPost]
        public void UpdateScorecard(string scorecard)
        {
            try
            {
               objCDServiceLayer.UpdateScorecard(scorecard);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Public UpdateScorecard

        #region Public getUserInfo
        /// <summary>
        /// getUserInfo
        /// </summary>
        /// <returns></returns>
        [Route("CDService/getUserInfo")]
        [HttpGet]
        public UserInfo getUserInfo()
        {
            UserInfo objUserInfo = new UserInfo();
            try
            {
                objUserInfo = objCDServiceLayer.getUserInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objUserInfo;
        }
        #endregion Public getUserInfo

        #region Public GetGroups
        /// <summary>
        /// GetGroups
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetGroups")]
        [HttpPost]
        [ResponseType(typeof(GroupsModel))]
        public List<DBOptions> GetGroups(GroupsModel objGroupsModel)
        {
            List<DBOptions> objUserInfo = new List<DBOptions>();
            try
            {
                string start_date = objGroupsModel.start_date;
                string end_date = objGroupsModel.start_date;
                string scorecard = objGroupsModel.scorecard;
                objUserInfo = objCDServiceLayer.GetGroups( start_date,end_date, scorecard);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objUserInfo;
        }
        #endregion Public GetGroups

        #region Public GetDetails
        /// <summary>
        /// GetGroups
        /// </summary>
        /// <returns></returns>
        [Route("CDService/GetDetails")]
        [HttpPost]
        [ResponseType(typeof(DetailsModel))]
        public string GetDetails(DetailsModel objDetailsModel)
        {
            string Message = string.Empty;
            try
            {
                string start_date = objDetailsModel.start_date;
                string end_date = objDetailsModel.start_date;
                string pagenum = "1";
                string pagerows = "50";
                string Sort_statement = "";
                string rowstart = "0";
                string rowend = "0";
                string hdnAgentFilter = objDetailsModel.hdnAgentFilter;
                string filter_array = objDetailsModel.filter_array;
                Message = objCDServiceLayer.GetDetails(start_date, end_date, hdnAgentFilter, pagenum, pagerows, Sort_statement, rowstart, rowend, filter_array);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public GetDetails

    }
}
