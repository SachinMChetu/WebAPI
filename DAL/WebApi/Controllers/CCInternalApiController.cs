using DAL.Models.CCInternalAPIModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using WebApi.Models.CCInternalAPI;

namespace WebApi.Controllers
{

    /// <summary>
    /// CCInternalApiController
    /// </summary>
    [BasicAuthenticationAttribute]
    [RoutePrefix("v2.4")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CCInternalApiController : ApiController
    {
        CCInternalLayer objCCInternalLayer = new CCInternalLayer();
        private string status = "Success";
        private BaseResponse bres;

        #region Public AddExistingSchool
        /// <summary>
        /// AddExistingSchool
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/AddExistingSchool")]
        [HttpPost]
        [ResponseType(typeof(object))]
        [Description("Add more data to an existing record after it has been received and processed.")]
        public dynamic AddExistingSchool(AddExistingSchoolRequest AES)
        {
            //string SESSION_ID = "743042";
            string SESSION_ID = AES.SESSION_ID;
            SchoolItem[] Schools = AES.Schools;
            try
            {
                objCCInternalLayer.AddExistingSchool(Schools, SESSION_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok("success");
        }
        #endregion Public AddExistingSchool

        #region Public UpdateFaqsOrder
        /// <summary>
        /// UpdateFaqsOrder
        /// </summary>
        /// <param name="questionFaq"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateFaqsOrder")]
        [HttpPost]
        [ResponseType(typeof(QuestionFaq))]
        [Description("Changes order of questions for specific scorecard")]
        public string UpdateFaqsOrder(List<QuestionFaq> questionFaq)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.UpdateFaqsOrder(questionFaq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion UpdateFaqsOrder

        #region Public UpdateInstructionsOrder
        /// <summary>
        /// UpdateInstructionsOrder
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateInstructionsOrder")]
        [HttpPost]
        [ResponseType(typeof(InstructionQuestion))]
        public string UpdateInstructionsOrder(List<InstructionQuestion> instructions)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.UpdateInstructionsOrder(instructions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion UpdateFaqsOrder


        #region Public PostCalibration
        /// <summary>
        /// PostCalibration
        /// </summary>
        /// <param name="listenDataRequest"></param>
        /// <returns></returns>
        [Route("CCInternal/PostCalibration")]
        [HttpPost]
        [ResponseType(typeof(ListenDataRequest))]
        public string PostCalibration(ListenDataRequest listenDataRequest)
        {
            string Message = "";
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return Messages.Authenticated;
                }
                if (listenDataRequest.bad_reason == "Scorecard Mismatch")
                {
                    return "0";
                }
                Message = objCCInternalLayer.PostCalibration(listenDataRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion PostCalibration

        #region Public GetNextTraining
        /// <summary>
        /// GetNextTraining
        /// </summary>
        /// <param name="nextTrainingRequest"></param>
        /// <returns></returns>
        [Route("CCInternal/GetNextTraining")]
        [HttpPost]
        [ResponseType(typeof(NextTrainingRequest))]
        public TrainingCall GetNextTraining(NextTrainingRequest nextTrainingRequest)
        {
            TrainingCall objTrainingCall = new TrainingCall();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return objTrainingCall;
                }
                string userName = HttpContext.Current.User.Identity.Name;
                objTrainingCall = objCCInternalLayer.GetNextTraining(nextTrainingRequest, userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTrainingCall;
        }
        #endregion GetNextTraining

        #region Public GetNextCalibration
        /// <summary>
        /// GetNextCalibration
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/GetNextCalibration")]
        [HttpPost]
        public ListenCall GetNextCalibration()
        {
            ListenCall objListenCall = new ListenCall();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return objListenCall;
                }
                string userName = HttpContext.Current.User.Identity.Name;
                objListenCall = objCCInternalLayer.GetNextCalibration(userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objListenCall;
        }
        #endregion GetNextCalibration

        #region Public UploadFile
        /// <summary>
        /// UploadFile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("CCInternal/UploadFile")]
        [HttpPost]
        [ResponseType(typeof(Stream))]
        public ManipulateAudioResult UploadFile(Stream input)
        {
            ManipulateAudioResult manipulateAudioResult = new ManipulateAudioResult();
            try
            {
                manipulateAudioResult = objCCInternalLayer.UploadFile(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return manipulateAudioResult;
        }
        #endregion UploadFile

        #region Public Manipulateaudio
        /// <summary>
        /// Manipulateaudio
        /// </summary>
        /// <param name="maudio"></param>
        /// <returns></returns>
        [Route("CCInternal/Manipulateaudio")]
        [HttpPost]
        [ResponseType(typeof(Maudio))]
        public ManipulateAudioResult Manipulateaudio(Maudio maudio)
        {
            ManipulateAudioResult manipulateAudioResult = new ManipulateAudioResult();
            try
            {
                manipulateAudioResult = objCCInternalLayer.Manipulateaudio(maudio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return manipulateAudioResult;
        }
        #endregion Manipulateaudio


        #region Public UpdateClerkData
        /// <summary>
        /// UpdateClerkData
        /// </summary>
        /// <param name="updateClerkedData"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateClerkData")]
        [HttpPost]
        [ResponseType(typeof(UpdateClerkedData))]
        public string UpdateClerkData(List<UpdateClerkedData> updateClerkedData)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.UpdateClerkData(updateClerkedData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion UpdateClerkData

        #region Public UpdateOptions
        /// <summary>
        /// UpdateOptions
        /// </summary>
        /// <param name="uOData"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateOptions")]
        [HttpPost]
        [ResponseType(typeof(UOData))]
        public string UpdateOptions(List<UOData> uOData)
        {
            string Message = "";
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return Messages.Authenticated;
                }
                Message = objCCInternalLayer.UpdateOptions(uOData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion UpdateOptions

        #region Public UpdateDispute
        /// <summary>
        /// UpdateDispute
        /// </summary>
        /// <param name="updateDisputeData"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateDispute")]
        [HttpPost]
        [ResponseType(typeof(UpdateDisputeData))]
        public Models.CCInternalAPI.disputeResult UpdateDispute(UpdateDisputeData updateDisputeData)
        {
            disputeResult objdisputeResult = new disputeResult();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return objdisputeResult;
                }
                objdisputeResult = objCCInternalLayer.UpdateDispute(updateDisputeData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objdisputeResult;
        }
        #endregion UpdateDispute


        #region Public GetAllRecords
        /// <summary>
        /// GetAllRecords
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/GetAllRecords")]
        [HttpPost]
        [ResponseType(typeof(CallRecordResponseData))]
        public dynamic GetAllRecords(GetAllRecordData GARD)
        {
            CallRecordResponseData crrd = new CallRecordResponseData();

            string call_date = GARD.call_date;
            string appname = HttpContext.Current.Request["appname"];
            //string appname = "edufficient";
            string use_review = GARD.use_review;
            List<CallRecord> cr = new List<CallRecord>();
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
                crrd.CallRecords = new List<CallRecord>();
                crrd.CallRecords = objCCInternalLayer.GetCallRecord(call_date, rev_date, appname, use_review);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(crrd);
        }
        #endregion Public GetAllRecords

        #region Public UpdateQuestionsOrder
        /// <summary>
        /// UpdateQuestionsOrder
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateQuestionsOrder")]
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        [Description("Changes order of questions for specific scorecard")]
        public dynamic UpdateQuestionsOrder(List<ScorecardQuestion> questions)
        {
            try
            {
                bres = new BaseResponse(true, status, objCCInternalLayer.UpdateQuestionsOrder(questions));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(bres);
        }
        #endregion UpdateQuestionsOrder

        #region Public GetNextCall
        /// <summary>
        /// GetNextCall
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/GetNextCall")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public ListenCall GetNextCall() // List(Of CallRecord)
        {
            ListenCall objListenCall = new ListenCall();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return objListenCall;
                }
                objListenCall = objCCInternalLayer.GetNextCall();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return objListenCall;
        }
        #endregion Public GetNextCall

        #region Public UpdateHeartbeat
        /// <summary>
        /// UpdateHeartbeat
        /// </summary>
        /// <param name="HB"></param>
        [Route("CCInternal/UpdateHeartbeat")]
        [HttpPost]
        [ResponseType(typeof(object))]
        public object UpdateHeartbeat(Heatbeart HB)
        {
            try
            {
                objCCInternalLayer.UpdateHeartbeat(HB);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok("Success");
        }
        #endregion UpdateHeartbeat

        #region Public GetScorecardRequiredData
        /// <summary>
        /// GetScorecardRequiredData
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/GetScorecardRequiredData")]
        [HttpPost]
        [ResponseType(typeof(AllCallRecord))]
        public AllCallRecord GetScorecardRequiredData(SimpleID SI)
        {
            AllCallRecord objAllCallRecord = new AllCallRecord();
            string ID = SI.ID;
            return objAllCallRecord;
        }
        #endregion GetScorecardRequiredData

        #region Public SwitchUser
        /// <summary>
        /// SwitchUser 
        /// </summary>
        /// <param name="SU"></param>
        /// <returns></returns>
        [Route("CCInternal/SwitchUser")]
        [HttpPost]
        [ResponseType(typeof(ButtonActionResponseData))]
        public ButtonActionResponseData SwitchUser(SimpleUser SU)
        {
            ButtonActionResponseData btnActResData = new ButtonActionResponseData();
            try
            {
                btnActResData.ButtonAction = objCCInternalLayer.SwitchUser(SU);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return btnActResData;
        }
        #endregion SwitchUser

        #region Public SwitchUserBack
        /// <summary>
        /// SwitchUserBack
        /// </summary>
        /// <param name="SU"></param>
        /// <returns></returns>
        [Route("CCInternal/SwitchUserBack")]
        [HttpPost]
        [ResponseType(typeof(ButtonActionResponseData))]
        public ButtonActionResponseData SwitchUserBack(SimpleUser SU)
        {
            ButtonActionResponseData btnActResData = new ButtonActionResponseData();
            try
            {
                btnActResData.ButtonAction = objCCInternalLayer.SwitchUserBack(SU);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return btnActResData;
        }
        #endregion SwitchUserBack

        #region Public getPayWorksheet
        /// <summary>
        ///  getPayWorksheet
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/GetPayWorksheet")]
        [HttpPost]
        [ResponseType(typeof(PayworksheetResponseData))]
        public PayworksheetResponseData GetPayWorksheet(SimpleDate SI)
        {
            PayworksheetResponseData pwsResData = new PayworksheetResponseData();
            pwsResData.Payworksheets = new List<Payworksheet>();
            try
            {
                pwsResData.Payworksheets = objCCInternalLayer.getPayWorksheet(SI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pwsResData;

        }
        #endregion Public getPayWorksheet

        #region Public getPayHistory
        /// <summary>
        /// getPayHistory
        /// </summary>
        /// <param name="PO"></param>
        /// <returns></returns>
        [Route("CCInternal/GetPayHistory")]
        [HttpPost]
        [ResponseType(typeof(PayHistoryResponseData))]
        public PayHistoryResponseData GetPayHistory(payObj PO)
        {
            PayHistoryResponseData phResData = new PayHistoryResponseData();
            phResData.PayHistories = new List<PayHistory>();
            objCCInternalLayer.GetPayHistory(PO);
            return phResData;
        }
        #endregion getPayHistory


        #region Public getCalibrationHours
        /// <summary>
        /// getPayHistory
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/GetCalibrationHours")]
        [HttpPost]
        [ResponseType(typeof(CalibrationHoursResponseData))]
        public CalibrationHoursResponseData GetCalibrationHours()
        {
            CalibrationHoursResponseData chResData = new CalibrationHoursResponseData();
            try
            {
                chResData.CalibrationHoursList = objCCInternalLayer.getCalibrationHours();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chResData;
        }
        #endregion getCalibrationHours

        #region Public GetRecordID
        /// <summary>
        /// GetRecordID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/GetRecordID")]
        [HttpPost]
        [ResponseType(typeof(CalibrationHoursResponseData))]

        public AllCallRecord GetRecordID(SimpleID SI)
        {
            CalibrationHoursResponseData chResData = new CalibrationHoursResponseData();
            AllCallRecord scr = new AllCallRecord();
            try
            {

                string ID = SI.ID;
                string userName = HttpContext.Current.User.Identity.Name;

                scr = objCCInternalLayer.GetRecordID(ID, userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return scr;
        }
        #endregion GetRecordID

        #region Public AcceptAsBad
        /// <summary>
        /// AcceptAsBad
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/AcceptAsBad")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public string AcceptAsBad(SimpleID SI)
        {
            try
            {
                int x_id = Convert.ToInt32(SI.ID);
                string user = HttpContext.Current.User.Identity.Name;
                objCCInternalLayer.AcceptAsBad(x_id, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Updated.";
        }
        #endregion AcceptAsBad


        #region Public GetRecordXID
        /// <summary>
        /// GetRecordXID
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/GetRecordXID")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public AllCallRecord GetRecordXID(SimpleID SI)
        {
            AllCallRecord objAllCallRecord = new AllCallRecord();
            try
            {
                int ID = Convert.ToInt32(SI.ID);
                objAllCallRecord = objCCInternalLayer.GetRecordXID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAllCallRecord;
        }
        #endregion GetRecordXID


        /// <summary>
        /// GetUserData
        /// </summary>
        /// <returns></returns>
        [Route("CCInternal/GetUserData")]
        [HttpPost]
        [ResponseType(typeof(UserObject))]
        public UserObject GetUserData()
        {
            UserObject objUserObject = new UserObject();
            try
            {
                //if (!HttpContext.Current.User.Identity.IsAuthenticated)
                //{
                //    objUserObject.UserName = "Not Authenticated";
                //    return objUserObject;
                //}
                string username = "Courtney"; //HttpContext.Current.User.Identity.Name; //Courtney

                objUserObject = objCCInternalLayer.GetUserData(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objUserObject;
        }

        #region Public GetCallsLoaded
        /// <summary>
        /// GetCallsLoaded
        /// </summary>
        /// <param name="CL"></param>
        /// <returns></returns>
        [Route("CCInternal/GetCallsLoaded")]
        [HttpPost]
        [ResponseType(typeof(CallsLoaded))]
        public List<CallLoaded> GetCallsLoaded(CallsLoaded CL)
        {
            List<CallLoaded> objCallLoaded = new List<CallLoaded>();
            try
            {
                //string appname = HttpContext.Current.Request["appname"];
                string appname = "83bar";
                objCallLoaded = objCCInternalLayer.GetCallsLoaded(CL, appname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallLoaded;
        }
        #endregion GetCallsLoaded

        #region Public GetTrainingReview
        /// <summary>
        /// GetTrainingReview
        /// </summary>
        /// <param name="SI"></param>
        /// <returns></returns>
        [Route("CCInternal/GetTrainingReview")]
        [HttpPost]
        [ResponseType(typeof(ReviewSimpleID))]
        public TrainingCallRecord GetTrainingReview(ReviewSimpleID SI)
        {
            TrainingCallRecord objTrainingCallRecord = new TrainingCallRecord();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return objTrainingCallRecord;
                }
                objTrainingCallRecord = objCCInternalLayer.GetTrainingReview(SI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTrainingCallRecord;
        }
        #endregion GetTrainingReview

        #region Public UpdateSpotcheck
        /// <summary>
        /// UpdateSpotcheck
        /// </summary>
        /// <param name="SCD"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateSpotcheck")]
        [HttpPost]
        [ResponseType(typeof(SpotCheckData))]
        public ButtonAction UpdateSpotcheck(SpotCheckData SCD)
        {
            ButtonAction objButtonAction = new ButtonAction();
            List<CallLoaded> objCallLoaded = new List<CallLoaded>();
            try
            {
                objButtonAction = objCCInternalLayer.UpdateSpotcheck(SCD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion UpdateSpotcheck

        #region Public getScore
        /// <summary>
        /// getScore
        /// </summary>
        /// <param name="gsd"></param>
        /// <returns></returns>
        [Route("CCInternal/getScore")]
        [HttpPost]
        [ResponseType(typeof(getScoreData))]
        public List<SessionStatus> getScore(getScoreData gsd)
        {
            List<SessionStatus> objSessionStatus = new List<SessionStatus>();
            try
            {
                objSessionStatus = objCCInternalLayer.getScore(gsd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objSessionStatus;
        }
        #endregion getScore


        #region Public AddDispute
        /// <summary>
        /// AddDispute
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/AddDispute")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction AddDispute(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.AddDispute(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public AddDispute

        #region Public UpdateCallComment
        /// <summary>
        /// AddDispute
        /// </summary>
        /// <param name="updateComment"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateCallComment")]
        [HttpPost]
        [ResponseType(typeof(UpdateComment))]
        public string UpdateCallComment(UpdateComment updateComment)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.UpdateCallComment(updateComment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public UpdateCallComment


        #region Public DeleteComment
        /// <summary>
        /// DeleteComment
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        [Route("CCInternal/DeleteComment")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public string DeleteComment(SimpleID simpleID)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.DeleteComment(simpleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public DeleteComment

        #region Public DeleteCall
        /// <summary>
        /// DeleteComment
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/DeleteCall")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction DeleteCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.DeleteCall(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public DeleteCall

        #region Public ReassignNotification
        /// <summary>
        /// ReassignNotification
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/ReassignNotification")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction ReassignNotification(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.ReassignNotification(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public ReassignNotification

        #region Public RecreateCall
        /// <summary>
        /// RecreateCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/RecreateCall")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction RecreateCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.RecreateCall(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public RecreateCall

        #region Public HideCall
        /// <summary>
        /// HideCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/HideCall")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction HideCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.HideCall(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public HideCall

        #region Public ResetCall
        /// <summary>
        /// ResetCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/ResetCall")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction ResetCall(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.ResetCall(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public ResetCall

        #region Public AddCaliDispute
        /// <summary>
        /// ResetCall
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/AddCaliDispute")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction AddCaliDispute(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.AddCaliDispute(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public AddCaliDispute

        #region Public AddClientCalibration
        /// <summary>
        /// AddClientCalibration
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/AddClientCalibration")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction AddClientCalibration(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.AddClientCalibration(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public AddClientCalibration

        #region Public AddCalibration
        /// <summary>
        /// AddCalibration
        /// </summary>
        /// <param name="endPointData"></param>
        /// <returns></returns>
        [Route("CCInternal/AddCalibration")]
        [HttpPost]
        [ResponseType(typeof(EndPointData))]
        public ButtonAction AddCalibration(EndPointData endPointData)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.AddCalibration(endPointData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public AddCalibration

        #region Public UpdateMetaData
        /// <summary>
        /// UpdateMetaData
        /// </summary>
        /// <param name="updateAllItems"></param>
        /// <returns></returns>
        [Route("CCInternal/UpdateMetaData")]
        [HttpPost]
        [ResponseType(typeof(UpdateAllItems))]
        public string UpdateMetaData(UpdateAllItems updateAllItems)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.UpdateMetaData(updateAllItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public UpdateMetaData

        #region Public ChangeScorecardData
        /// <summary>
        /// ChangeScorecardData
        /// </summary>
        /// <param name="changeScorecardData"></param>
        /// <returns></returns>
        [Route("CCInternal/ChangeCallScorecard")]
        [HttpPost]
        [ResponseType(typeof(ChangeScorecardData))]
        public void ChangeCallScorecard(ChangeScorecardData changeScorecardData)
        {
            try
            {
                objCCInternalLayer.ChangeCallScorecard(changeScorecardData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Public ChangeScorecardData


        #region Public RemoveCalibrator
        /// <summary>
        /// RemoveCalibrator
        /// </summary>
        /// <param name="removeCalib"></param>
        /// <returns></returns>
        [Route("CCInternal/RemoveCalibrator")]
        [HttpPost]
        [ResponseType(typeof(RemoveCalib))]
        public string RemoveCalibrator(RemoveCalib removeCalib)
        {
            string message = "";
            int f_id = Convert.ToInt32(removeCalib.FID);
            string calibrator = removeCalib.calibrator;

            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return Messages.Authenticated;
                }
                message = objCCInternalLayer.RemoveCalibrator(f_id, calibrator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }
        #endregion Public RemoveCalibrator

        #region Public CompleteReview
        /// <summary>
        /// RemoveCalibrator
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        [Route("CCInternal/CompleteReview")]
        [HttpPost]
        [ResponseType(typeof(SimpleID))]
        public string CompleteReview(SimpleID simpleID)
        {
            string message = "";
            try
            {
                message = objCCInternalLayer.CompleteReview(simpleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }
        #endregion Public CompleteReview

        #region Public MarkCalibrationBad
        /// <summary>
        /// MarkCalibrationBad
        /// </summary>
        /// <param name="markCaliBad"></param>
        /// <returns></returns>
        [Route("CCInternal/MarkCalibrationBad")]
        [HttpPost]
        [ResponseType(typeof(MarkCaliBad))]
        public string MarkCalibrationBad(MarkCaliBad markCaliBad)
        {
            string message = "";
            try
            {
                message = objCCInternalLayer.MarkCalibrationBad(markCaliBad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }
        #endregion Public MarkCalibrationBad


        #region Public MarkCallBad2
        /// <summary>
        /// MarkCallBad2
        /// </summary>
        /// <param name="markBadCallData2"></param>
        /// <returns></returns>
        [Route("CCInternal/MarkCallBad2")]
        [HttpPost]
        [ResponseType(typeof(MarkBadCallData2))]
        public ButtonAction MarkCallBad2(MarkBadCallData2 markBadCallData2)
        {
            ButtonAction objButtonAction = new ButtonAction();
            try
            {
                objButtonAction = objCCInternalLayer.MarkCallBad2(markBadCallData2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objButtonAction;
        }
        #endregion Public MarkCalibrationBad


        #region Public MarkCallBad
        /// <summary>
        /// MarkCallBad2
        /// </summary>
        /// <param name="markBadCallData"></param>
        /// <returns></returns>
        [Route("CCInternal/MarkCallBad")]
        [HttpPost]
        [ResponseType(typeof(MarkBadCallData2))]
        public void MarkCallBad(MarkBadCallData markBadCallData)
        {
            try
            {
                objCCInternalLayer.MarkCallBad(markBadCallData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Public MarkCallBad


        #region Public GetScorecardRecordID
        /// <summary>
        /// GetScorecardRecordID
        /// </summary>
        /// <param name="simpleID"></param>
        /// <returns></returns>
        [Route("CCInternal/GetScorecardRecordID")]
        [HttpPost]
        [ResponseType(typeof(getSCRecData))]
        public CompleteScorecard GetScorecardRecordID(SimpleID simpleID)
        {
            CompleteScorecard objCompleteScorecard = new CompleteScorecard();

            try
            {
                objCompleteScorecard = objCCInternalLayer.GetScorecardRecordID(simpleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCompleteScorecard;
        }
        #endregion Public GetScorecardRecordID

        #region Public GetScorecardRecordListen
        /// <summary>
        /// GetScorecardRecordListen
        /// </summary>
        /// <param name="getSCRecData"></param>
        /// <returns></returns>
        [Route("CCInternal/GetScorecardRecordListen")]
        [HttpPost]
        [ResponseType(typeof(getSCRecData))]
        public CompleteScorecard GetScorecardRecordListen(getSCRecData getSCRecData)
        {
            CompleteScorecard objCompleteScorecard = new CompleteScorecard();
            string scorecard_ID = getSCRecData.scorecard_ID;
            string xcc_id = getSCRecData.xcc_id;
            if (!Information.IsNumeric(xcc_id))
            {
                xcc_id = "";
            }
            string username = HttpContext.Current.User.Identity.Name;

            try
            {
                objCompleteScorecard= objCCInternalLayer.GetScorecardRecordListen(scorecard_ID, xcc_id, username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCompleteScorecard;
        }
        #endregion Public GetScorecardRecordListen

        #region Public getCoachingQueueJson
        /// <summary>
        /// getCoachingQueueJson
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("CCInternal/getCoachingQueueJson")]
        [HttpPost]
        public List<CoachingQueue> getCoachingQueueJson(string filter)
        {
            List<CoachingQueue> coachingQueueLst = new List<CoachingQueue>();
            string username = HttpContext.Current.User.Identity.Name;

            try
            {
                coachingQueueLst = objCCInternalLayer.getCoachingQueueJson(username, filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return coachingQueueLst;
        }
        #endregion Public getCoachingQueueJson

        #region Public AddRecord
        /// <summary>
        /// AddRecord
        /// </summary>
        /// <param name="addRecordData"></param>
        /// <returns></returns>
        [Route("CCInternal/AddRecord")]
        [HttpPost]
        [ResponseType(typeof(AddRecordData))]
        public string AddRecord(AddRecordData addRecordData)
        {
            string Message = "";
            try
            {
                Message = objCCInternalLayer.AddRecord(addRecordData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Message;
        }
        #endregion Public AddRecord

    }
}

