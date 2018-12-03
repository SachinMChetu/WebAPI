using DAL.Layers;
using DAL.Models;
using DAL.Models.SettingsModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;


namespace WebApi.Controllers
{
    /// <summary>
    /// Settings Controller for Setting Page
    /// </summary>
    public class SettingsController : ApiController
    {
        /// <summary>
        /// GetAppList method
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetAppList")]
        [HttpPost]
        [ResponseType(typeof(DropdownInfo))]
        public dynamic GetAppList()
        {
            return new SettingsLayer().GetAvailableApplicationList();
        }
        /// <summary>
        /// GetScorecardList method
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/GetScorecardList")]
        [HttpPost]
        [ResponseType(typeof(DropdownInfo))]
        public dynamic GetScorecardList([FromBody] DropdownInfo info)
        {
            var a = new SettingsLayer().GetAvailableScorecardList(info.name);
            return a;
        }
        /// <summary>
        /// GetApplicationSetting method
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/GetApplicationSetting")]
        [HttpPost]
        [ResponseType(typeof(AppSettings))]
        public dynamic GetApplicationSetting([FromBody] DropdownInfo info)
        {
            var a = new SettingsLayer().GetAppByName(info);
            return a;
        }
        /// <summary>
        /// create new application
        /// </summary>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        [Route("Settings/CreateApplication")]
        [HttpPost]
        [ResponseType(typeof(AppSettings))]
        public dynamic CreateApplication([FromBody] AppSettings appSettings)
        {
            var a = new SettingsLayer().AddApplication(appSettings);
            return a;
        }
        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        [Route("Settings/UpdateApplication")]
        [HttpPost]
        [ResponseType(typeof(AppSettings))]
        public dynamic UpdateApplication([FromBody] AppSettings appSettings)
        {
            var a = new SettingsLayer().UpdateApplication(appSettings);
            return a;
        }
        /// <summary>
        /// delete app by id
        /// </summary>
        /// <param name="appModelWL"></param>
        /// <returns></returns>
        [Route("Settings/DeleteApplication")]
        [HttpPost]
        [ResponseType(typeof(AppModelWL))]
        public dynamic DeleteApplication([FromBody] AppModelWL appModelWL)
        {
            return new SettingsLayer().DeleteApplication(appModelWL);

        }
        // GET: Settings


        /// <summary>
        /// DuplicateApp
        /// </summary>
        /// <param name="appModelWL"></param>
        /// <returns></returns>
        [Route("Settings/DuplicateApp")]
        [HttpPost]
        [ResponseType(typeof(AppModelWL))]
        public dynamic DuplicateApp(AppModelWL appModelWL)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DuplicateApp(appModelWL);
        }

        /// <summary>
        /// get(settings) method
        /// </summary>
        /// <returns></returns>
        [Route("Settings/CreateNewApp")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic get()
        {
            return null;
        }


        /// <summary>
        /// GetbillingRates
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/GetNotes")]
        [HttpPost]
        [ResponseType(typeof(List<NotesModel>))]
        public IEnumerable<NotesModel> GetNotes([FromBody]DropdownInfo info)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.GetNotes(info);//appname
        }

        /// <summary>
        /// InsertNote method
        /// </summary>
        /// <param name="notesModel"></param>
        /// <returns></returns>
        [Route("Settings/InsertNote")]
        [HttpPost]
        [ResponseType(typeof(NotesModel))]
        public NotesModel InsertNote([FromBody]NotesModel notesModel)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.InsertNotes(notesModel);
        }


        /// <summary>
        /// DeleteNote method
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/DeleteNote")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public string DeleteNote([FromBody]DropdownInfo info)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.DeleteNotes(info);//id
        }


        /// <summary>
        /// DeleteAllNotes method
        /// </summary>
        /// <returns></returns>
        [Route("Settings/ClearAllNotesByAppName")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteAllNotes([FromBody]DropdownInfo info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteAllNotes(info);//appname
        }


        /// <summary>
        /// GetBillingRate method
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        [Route("Settings/GetBillingRate")]
        [HttpPost]
        [ResponseType(typeof(List<BillingRatesModel>))]
        public List<BillingRatesModel> GetBillingRate([FromBody]DropdownInfo info)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.GetBillingRate(info);//appname
        }

        /// <summary>
        /// InsertBilingRate
        /// </summary>
        /// <param name="billingRatesModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateBilingRate")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic InsertBilingRate([FromBody]BillingRatesModel billingRatesModel)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.InsertBilingRate(billingRatesModel);
        }

        /// <summary>
        /// InsertBilingRate
        /// </summary>
        /// <param name="billingRatesModel"></param>
        /// <returns></returns>
        [Route("Settings/AddBilingRate")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddBilingRate([FromBody]BillingRatesModel billingRatesModel)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.InsertBilingRate(billingRatesModel);
        }


        /// <summary>
        /// DeleteBillingRate method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/DeleteBillingRate")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteBillingRate([FromBody]ModelForDeleteBilling model)
        {
            DAL.Layers.SettingsLayer settings = new DAL.Layers.SettingsLayer();
            return settings.DeleteBillingRate(model);//id
        }


        /// <summary>
        /// UploadLogo method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("Settings/UploadLogo")]
        [HttpPost]
        [ResponseType(typeof(HttpPostedFileBase))]
        public dynamic UploadClientLogo([FromBody]FileUploaderSettinsModel file)

        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UploadClientLogo(file);
        }




        /// <summary>
        /// UploadClientLogoSmall method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("Settings/UploadLogoSmall")]
        [HttpPost]
        [ResponseType(typeof(HttpPostedFileBase))]
        public dynamic UploadClientLogoSmall([FromBody]FileUploaderSettinsModel file)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UploadClientLogoSmall(file);
           
        }


        /// <summary>
        /// GetConfigurationProfile method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("Settings/GetConfigurationProfile")]
        [HttpPost]
        [ResponseType(typeof(List<DefaultNotificationProfileModel>))]
        public dynamic GetConfigurationProfile()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetConfigurationProfile();
        }



        /// <summary>
        /// GetConfigurationProfile method
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("Settings/GetRjectionProfile")]
        [HttpPost]
        [ResponseType(typeof(List<RejectionProfileModel>))]
        public dynamic GetRjectionProfile()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetRjectionProfile();
        }

        /// <summary>
        /// GetNaAffected method
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetNaAffected")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetNaAffected()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetNaAffected();
        }

        /// <summary>
        /// GetFirstNotificationAssigee method
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetFirstNotificationAssigee")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetFirstNotificationAssigee()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetFirstNotificationAssigee();
        }


        /// <summary>
        /// InsertScorecard method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/UpdateScorecardSettings")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateScorecard([FromBody]ScorecardSettingsModel model)
        {
            SettingsLayer settings = new SettingsLayer();
            return settings.UpdateScorecard(model);
        }




        /// <summary>
        /// InsertScorecard method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/AddNewScorecard")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddNewScorecard([FromBody]ScorecardSettingsModel model)
        {
            SettingsLayer settings = new SettingsLayer();
            return settings.AddNewScorecard(model);
        }


        /// <summary>
        /// geting all scorecard settings for current scorecard by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetScorecardSettingsById")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetScorecardSettingsById([FromBody]int id)
        {
            SettingsLayer settings = new SettingsLayer();
            return settings.GetScorecardSettingsById(id);
        }



        /// <summary>
        /// geting all scorecard notes for current scorecard by scorecard id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/GetScorecardNotesById")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetScorecardNotes([FromBody]DropdownInfo info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetScorecardNotes(info);
        }


        /// <summary>
        ///  deleting scorecard by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/DeleteScorecardById")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteScorecard([FromBody]int id)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteScorecard(id);
        }

        /// <summary>
        /// geting scorecard changes for history of change by scorecard id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetScorecardChanges")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public List<ScorecardChangesModel> GetScorecardChanges([FromBody]int id)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetScorecardChanges(id);
        }


        /// <summary>
        ///Deletes 1 scorecard note by notes id
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/DeleteScorecardNote")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteScorecardNote(ScorecardNotesModel info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteScorecardNote(info);
        }

        /// <summary>
        /// Deleting all scorecard notes by sorecard id for current scorecard
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/DeleteAllScorecardNote")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteAllScorecardNotesByScorecardId(ScorecardNotesModel info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteAllScorecardNotes(info);
        }

        /// <summary>
        /// Gets all account managers
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetAccountManagerUsers")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAccountManagerUsers()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetAccountManagerUsers();
        }



        /// <summary>
        /// Gets all TL
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetTLUsers")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetTLUsers()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetTLUsers();
        }


        /// <summary>
        /// Gets all Tango TL
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetTangoTLUsers")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetTangoTLUsers()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetTangoTLUsers();
        }



        /// <summary>
        /// Gets all Golden Users
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetGoldenUsers")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetGoldenUsers(DropdownInfo info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetGoldenUsers(info);
        }

        /// <summary>
        /// Adding new scorecard note
        /// </summary>
        /// <param name="scorecardNotesModel"></param>
        /// <returns></returns>
        [Route("Settings/AddNewScorecardNote")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddNewScorecardNote([FromBody]ScorecardNotesModel scorecardNotesModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.AddNewScorecardNote(scorecardNotesModel);
        }

        /// <summary>
        /// Update Scorecard Note
        /// </summary>
        /// <remarks>)</remarks>
        /// <param name="scorecardNotesModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateScorecardNote")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateScorecardNote([FromBody]ScorecardNotesModel scorecardNotesModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateScorecardNote(scorecardNotesModel);
        }

        /// <summary>
        /// method witch returns us api keys for current scorecard
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        [Route("Settings/GetApiKeys")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetApiKeys([FromBody]string appname)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetApiKey(appname);
        }
        /// <summary>
        /// AddApiKey
        /// </summary>
        /// <param name="appname"></param>
        /// <returns></returns>
        [Route("Settings/AddApiKey")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddApiKey([FromBody] string appname)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.AddApiKey(appname);
        }

        /// <summary>
        /// UpdateApiKey
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/UpdateApiKey")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateApiKey([FromBody] ApiKeyModel model)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateApiKey(model);
        }


        /// <summary>
        /// UpdateApiKey
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/DeleteApiKey")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteApiKey([FromBody] ApiKeyModel model)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteApiKey(model);
        }
        /// <summary>
        /// AddExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/AddExport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddExport([FromBody]AppExportSetting model)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.AddExport(model);
        }
        /// <summary>
        /// GetExport
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Settings/GetExport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetExport([FromBody]DropdownInfo info)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetExport(info.name);
        }
        /// <summary>
        /// UpdateExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/UpdateExport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateExport([FromBody]AppExportSetting model)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateExport(model);
        }
        /// <summary>
        /// DeleteExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Settings/DeleteExport")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DeleteExport([FromBody]AppExportSetting model)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.DeleteExport(model);
        }

        /// <summary>
        /// Get path of the loge by app id
        /// </summary>
        /// <param name="dropdownInfo"></param>
        /// <returns></returns>
        [Route("Settings/GetSmallLogo")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetSmallLogo([FromBody]DropdownInfo dropdownInfo)
        {

            #region GetLogoRealizataion
            string logo = "";
            string command = @"select isnull(client_logo_small,'') as logo from app_settings where id = @id";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                try
                {
                    var reply = new SqlCommand(command, conn)
                    {
                        CommandTimeout = 60,
                        CommandType = CommandType.Text
                    };
                    reply.Parameters.AddWithValue("@id", dropdownInfo.id);
                    conn.Open();
                    var reader = reply.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            logo = reader.GetValue(reader.GetOrdinal("logo")).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return logo;
            #endregion
        }


        /// <summary>
        /// GetAppListWithLogo
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetAppListWithLogo")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAppListWithLogo()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetAppListWithLogo();
        }


        /// <summary>
        /// GetAllScorecardList
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetAllScorecardList")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAllScorecardList()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetAllScorecardList();
        }

        /// <summary>
        /// GetQuestionSection
        /// </summary>
        /// <param name="scorecards"></param>
        /// <returns></returns>
        [Route("Settings/GetQuestionSection")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetQuestionSection([FromBody]ScorecardsInfo scorecards)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetQuestionSection(scorecards);
        }

        /// <summary>
        /// UpdateQuestionSection
        /// </summary>
        /// <param name="sectionModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateSections")]
        [HttpPost]
        [ResponseType(typeof(List<SectionModel>))]
        public dynamic UpdateSections([FromBody]List<SectionModel> sectionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateSections(sectionModel);
        }

        /// <summary>
        /// GetQustionList
        /// </summary>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        [Route("Settings/GetQuestionList")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetQustionList([FromBody]ScorecardsInfo scorecard)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetQustionList(scorecard);
        }
        /// <summary>
        /// UpdateQuestion
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateQuestion")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateQuestion([FromBody]QuestionModel questionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateQuestion(questionModel);
        }


        /// <summary>
        /// AddQuestion
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        [Route("Settings/AddQuestion")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic AddQuestion(QuestionModel questionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.AddQuestion(questionModel);
        }
        /// <summary>
        /// To pause qa points
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        [Route("Settings/PauseQaPoints")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic PauseQaPoints([FromBody]QuestionModel questionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.PauseQaPoints(questionModel);
        }
        /// <summary>
        /// GetQuestionById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetQuestionById")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetQuestionById([FromBody]int id)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetQuestionById(id);
        }
        /// <summary>
        /// To unpause qa points
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        [Route("Settings/UnPauseQaPoints")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UnPauseQaPoints([FromBody]QuestionModel questionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UnpauseQaPoints(questionModel);
        }
        /// <summary>
        /// CloneQuestionToScorecard
        /// </summary>
        /// <param name="cloneModel"></param>
        /// <returns></returns>
        [Route("Settings/CloneQuestionToScorecard")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionModel>))]
        public dynamic CloneQuestionToScorecard(CloneModel cloneModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.CloneQuestionToScorecard(cloneModel);
        }

        /// <summary>
        /// CloneQuestion
        /// </summary>
        /// <param name="cloneModel"></param>
        /// <returns></returns>
        [Route("Settings/CloneQuestion")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionModel>))]
        public dynamic CloneQuestion(QuestionModel cloneModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.CloneQuestion(cloneModel);
        }
        /// <summary>
        /// GetAnswerById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetAnswerById")]
        [HttpPost]
        [ResponseType(typeof(QuestionModel))]
        public dynamic GetAnswerById([FromBody]int id)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetAnswerById(id);
        }

        /// <summary>
        /// UpdateAnswer
        /// </summary>
        /// <param name="answerModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateAnswerV1")]
        [HttpPost]
        [ResponseType(typeof(QuestionModel))]
        public dynamic UpdateAnswerV1([FromBody]AnswerModel answerModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.UpdateAnswerV1(answerModel);
        }

        /// <summary>
        /// GetQuestionAnswers
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [Route("Settings/GetQuestionAnswers")]
        [HttpPost]
        [ResponseType(typeof(QuestionModel))]
        public dynamic GetQuestionAnswers([FromBody]int questionId)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            return settingsLayer.GetQuestionAnswers(questionId);
        }


 

        /// <summary>
        /// DedupeScorecard
        /// </summary>
        /// <param name="scorecards"></param>
        /// <returns></returns>
        [Route("Settings/DedupeScorecard")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic DedupeScorecard([FromBody]ScorecardsInfo scorecards)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            settingsLayer.DedupeScorecard(scorecards);
            return Ok("success");
        }
        /// <summary>
        /// UpdateMultipleQuestions
        /// </summary>
        /// <param name="questionModels"></param>
        /// <returns></returns>
        [Route("Settings/UpdateMultipleQuestions")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionModel>))]
        public dynamic UpdateMultipleQuestions([FromBody]List<QuestionModel> questionModels)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.UpdateMultipleQuestions(questionModels);
            return rez;
        }

        /// <summary>
        /// GetDropDownItemList
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [Route("Settings/GetDropDownItemList")]
        [HttpPost]
        [ResponseType(typeof(List<DropDownItemModel>))]
        public dynamic GetDropDownItemList([FromBody]int questionId)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetDropDownItemList(questionId);
            return rez;
        }

        /// <summary>
        /// AddDropDownItem
        /// </summary>
        /// <param name="dropDownItemModel"></param>
        /// <returns></returns>
        [Route("Settings/AddDropDownItem")]
        [HttpPost]
        [ResponseType(typeof(List<DropDownItemModel>))]
        public dynamic AddDropDownItem(DropDownItemModel dropDownItemModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.AddDropDownItem(dropDownItemModel);
            return rez;
        }

        /// <summary>
        /// EditDropdownItem
        /// </summary>
        /// <param name="dropDownItemModel"></param>
        /// <returns></returns>
        [Route("Settings/EditDropDownItem")]
        [HttpPost]
        [ResponseType(typeof(List<DropDownItemModel>))]
        public dynamic EditDropdownItem(DropDownItemModel dropDownItemModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.EditDropdownItem(dropDownItemModel);
            return rez;
        }

        /// <summary>
        /// DeleteDropDownItem
        /// </summary>
        /// <param name="dropDownItemModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteDropDownItem")]
        [HttpPost]
        [ResponseType(typeof(List<DropDownItemModel>))]
        public dynamic DeleteDropDownItem(DropDownItemModel dropDownItemModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.DeleteDropDownItem(dropDownItemModel);
            return rez;
        }

        /// <summary>
        /// GetQuestionByIdSimple
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetQuestionByIdSimple")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionModel>))]
        public dynamic GetQuestionByIdSimple([FromBody]int id)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetQuestionByIdSimple(id);
            return rez;
        }

        /// <summary>
        /// GetCommentsList
        /// </summary>
        /// <param name="Qid"></param>
        /// <returns></returns>
        [Route("Settings/GetCommentsList")]
        [HttpPost]
        [ResponseType(typeof(List<LinkedCommentModel>))]
        public dynamic GetCommentsList([FromBody]int Qid)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetCommentsList(Qid);
            return rez;
        }

        /// <summary>
        /// GetAnswerList
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Settings/GetAnswerList")]
        [HttpPost]
        [ResponseType(typeof(List<LiskedAnswerModel>))]
        public dynamic GetAnswerList([FromBody]int Qid)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetAnswerList(Qid);
            return rez;
        }
        /// <summary>
        /// GetCallData
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetCallData")]
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public dynamic GetCallData()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetCallData();
            return rez;
        }


        /// <summary>
        /// GetSchoolData
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetSchoolData")]
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public dynamic GetSchoolData()
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetSchoolData();
            return rez;
        }

        /// <summary>
        /// GetFAQList
        /// </summary>
        /// <param name="Qid"></param>
        /// <returns></returns>
        [Route("Settings/GetFAQList")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic GetFAQList([FromBody]int Qid)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetFAQList(Qid);
            return rez;
        }

        /// <summary>
        /// AddFAQ
        /// </summary>
        /// <param name="fAQsModel"></param>
        /// <returns></returns>
        [Route("Settings/AddFAQ")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic AddFAQ([FromBody]FAQsModel fAQsModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.AddFAQ(fAQsModel);
            return rez;
        }


        /// <summary>
        /// DeleteFAQ
        /// </summary>
        /// <param name="fAQsModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteFAQ")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic DeleteFAQ([FromBody]FAQsModel fAQsModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.DeleteFAQ(fAQsModel);
            return rez;
        }


        /// <summary>
        /// UpdateFAQ
        /// </summary>
        /// <param name="fAQsModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateFAQ")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic UpdateFAQ([FromBody]FAQsModel fAQsModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.UpdateFAQ(fAQsModel);
            return rez;
        }

        /// <summary>
        /// GetInstructionsList
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        [Route("Settings/GetInstructionsList")]
        [HttpPost]
        [ResponseType(typeof(List<InstructionModel>))]
        public dynamic GetInstructionsList([FromBody]int QID)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetInstructionsList(QID);
            return rez;
        }
        /// <summary>
        /// DeleteInstruction
        /// </summary>
        /// <param name="instructionModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteInstruction")]
        [HttpPost]
        [ResponseType(typeof(List<InstructionModel>))]
        public dynamic DeleteInstruction([FromBody]InstructionModel instructionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.DeleteInstruction(instructionModel);
            return rez;
        }

        /// <summary>
        /// UpdateInstruction
        /// </summary>
        /// <param name="instructionModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateInstruction")]
        [HttpPost]
        [ResponseType(typeof(List<InstructionModel>))]
        public dynamic UpdateInstruction([FromBody]InstructionModel instructionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.UpdateInstruction(instructionModel);
            return rez;
        }



        /// <summary>
        /// ChangeQuestionsOrder
        /// </summary>
        /// <param name="objectOrder"></param>
        /// <returns></returns>
        [Route("Settings/ChangeQuestionsOrder")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionModel>))]
        public dynamic ChangeQuestionsOrder([FromBody]QuestionOrdering objectOrder)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeQuestionsOrder(objectOrder);
            return rez;
        }

        /// <summary>
        /// CahangeDropDownItemsOrdering
        /// </summary>
        /// <param name="objectOrder"></param>
        /// <returns></returns>
        [Route("Settings/CahangeDropDownItemsOrdering")]
        [HttpPost]
        [ResponseType(typeof(List<DropDownItemModel>))]
        public dynamic CahangeDropDownItemsOrdering([FromBody]OrderingDropDown objectOrder)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.CahangeDropDownItemsOrdering(objectOrder);
            return rez;
        }


        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="instructionModel"></param>
        /// <returns></returns>
        [Route("Settings/AddInstruction")]
        [HttpPost]
        [ResponseType(typeof(List<InstructionModel>))]
        public dynamic AddInstruction([FromBody]InstructionModel instructionModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.AddInstruction(instructionModel);
            return rez;
        }


         /// <summary>
        /// ChangeIstructionOrder
        /// </summary>
        /// <param name="ordering"></param>
        /// <returns></returns>
        [Route("Settings/ChangeIstructionOrder")]
        [HttpPost]
        [ResponseType(typeof(List<InstructionModel>))]
        public dynamic ChangeIstructionOrder([FromBody]InsructionsOrdering ordering)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeIstructionOrder(ordering);
            return rez;
        }


        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetAnswerListFull")]
        [HttpPost]
        [ResponseType(typeof(List<AnswersModel>))]
        public dynamic GetAnswerListFull([FromBody]int qID)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetAnswerListFull(qID);
            return rez;
        }


        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="answersModel"></param>
        /// <returns></returns>
        [Route("Settings/AddAnswer")]
        [HttpPost]
        [ResponseType(typeof(List<AnswersModel>))]
        public dynamic AddAnswer([FromBody]AnswersModel answersModel )
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.AddAnswer(answersModel);
            return rez;
        }

        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="answersModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateAnswer")]
        [HttpPost]
        [ResponseType(typeof(List<AnswersModel>))]
        public dynamic UpdateAnswer([FromBody]AnswersModel answersModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.UpdateAnswer(answersModel);
            return rez;
        }




        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="answersModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteAnswer")]
        [HttpPost]
        [ResponseType(typeof(List<AnswersModel>))]
        public dynamic DeleteAnswer([FromBody]AnswersModel answersModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.DeleteAnswer(answersModel);
            return rez;
        }



        /// <summary>
        /// ChangeAnswerOrder
        /// </summary>
        /// <param name="answerOrdering"></param>
        /// <returns></returns>
        [Route("Settings/ChangeAnswerOrder")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic ChangeAnswerOrder([FromBody]AnswerOrdering  answerOrdering)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeAnswerOrder(answerOrdering);
            return rez;
        }



        /// <summary>
        /// AddInstruction
        /// </summary>
        /// <param name="orderingFAQs"></param>
        /// <returns></returns>
        [Route("Settings/ChangeFAQOrder")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic ChangeFAQOrder([FromBody]OrderingFAQs orderingFAQs)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeFAQOrder(orderingFAQs);
            return rez;
        }

        /// <summary>
        /// GetAnswerComments
        /// </summary>
        /// <param name="qId"></param>
        /// <returns></returns>
        [Route("Settings/GetAnswerComments")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic GetAnswerComments([FromBody]int qId)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetAnswerComments(qId);
            return rez;
        }

        /// <summary>
        /// UpdateAnswerComment
        /// </summary>
        /// <param name="answerCommentModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateAnswerComment")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic UpdateAnswerComment([FromBody]AnswerCommentModel  answerCommentModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.UpdateAnswerComment(answerCommentModel);
            return rez;
        }


        /// <summary>
        /// AddAnswerComment
        /// </summary>
        /// <param name="answerCommentModel"></param>
        /// <returns></returns>
        [Route("Settings/AddAnswerComment")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic AddAnswerComment([FromBody]AnswerCommentModel answerCommentModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.AddAnswerComment(answerCommentModel);
            return rez;
        }

        /// <summary>
        /// DeleteAnswerComments
        /// </summary>
        /// <param name="answerCommentModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteAnswerComments")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic DeleteAnswerComments([FromBody]AnswerCommentModel answerCommentModel)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.DeleteAnswerComments(answerCommentModel);
            return rez;
        }



        /// <summary>
        /// GetHistoryOfChanges
        /// </summary>
        /// <param name="qId"></param>
        /// <returns></returns>
        [Route("Settings/GetHistoryOfChanges")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic GetHistoryOfChanges([FromBody]int qId)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetHistoryList(qId);
            return rez;
        }

        /// <summary>
        /// GetAnswerTypes
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetAnswerType")]
        [HttpPost]
        [ResponseType(typeof(List<AnswerCommentModel>))]
        public dynamic GetAnswerType()
        {
            var types = new[]
                {
                    new { id = 0, Name = "Yes" },
                    new { id = 1, Name = "No" },
                    new { id = 2,Name = "NA"},
                    new {id = 3,Name = "Other"}
                };
            return types;
        }


        /// <summary>
        /// ChangeAnswerCommentsOrdering
        /// </summary>
        /// <param name="ordering"></param>
        /// <returns></returns>
        [Route("Settings/ChangeAnswerCommentsOrdering")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic ChangeAnswerCommentsOrdering([FromBody]AnswerCommentsOrdering ordering)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeAnswerCommentsOrdering(ordering);
            return rez;

        }

        /// <summary>
        /// CloneQuestionsToScorecard
        /// </summary>
        /// <param name="multipleCloneModel"></param>
        /// <returns></returns>
        [Route("Settings/CloneQuestionsToScorecard")]
        [HttpPost]
        [ResponseType(typeof(List<FAQsModel>))]
        public dynamic MultipleCloneQuestionsToScorecard([FromBody]List<CloneModel> multipleCloneModel)
        {
            try
            {
                SettingsLayer settingsLayer = new SettingsLayer();
                var rez = settingsLayer.MultipleCloneQuestionsToScorecard(multipleCloneModel);
                return rez;
            }
            catch
            {
               return Ok("Sorry this functionality is in progress");
            }
          
        }


        /// <summary>
        /// GetAppScorecardList
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("Settings/GetAppScorecardList")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic GetAppScorecardList([FromBody]Search search)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.GetAppScorecardList(search);
            return rez;
        }


        /// <summary>
        /// GetQuestionTypes
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetQuestionTypes")]
        [HttpPost]
        public dynamic GetQuestionTypes()
        {
            var types = new[]
               {
                    new { id = 0, Name = "Regular",info = "Basic Yes/No type format" },
                    new { id = 1, Name = "Count",info = "Number scale values where selectons can be averaged over a period of time for an overall score" },
                    new { id = 2,Name = "Calculated",info = "Non-QA selected question which calculatesat the time of submitting a call based on predefined rules being met" },
                    new {id = 3,Name = "Dynamic",info = "Meta data dependent where question populates/does not populate when predefined rules are met" },
                    new {id = 4,Name = "Dropdown",info = "Manually entered or imported list of dropdown selections to choose from" },
                    new {id = 5,Name = "YMM",info = "3-tiered dropdown that allows QA to individually select the year/make/model of a vihicle from a predetermined list" },
                    new {id = 6,Name = "DateTime",info = "Date Picker and Time dropdown that allows for specific date/time selection" },
                    new {id = 7,Name = "Date",info = "Date Picker thet allows for specific date selection" },
                };
            return types;
        }



        /// <summary>
        /// ChangeSectionOrder
        /// </summary>
        /// <param name="ordering"></param>
        /// <returns></returns>
        [Route("Settings/ChangeSectionOrder")]
        [HttpPost]
        [ResponseType(typeof(List<AppsListModel>))]
        public dynamic ChangeSectionOrder([FromBody]SectionOrderingModel ordering)
        {
            SettingsLayer settingsLayer = new SettingsLayer();
            var rez = settingsLayer.ChangeSectionOrder(ordering);
            return rez;
        }



        ///// <summary>
        ///// AddNewSection
        ///// </summary>
        ///// <param name="section"></param>
        ///// <returns></returns>
        //[Route("Settings/AddNewSection")]
        //[HttpPost]
        //[ResponseType(typeof(List<SectionModel>))]
        //public dynamic AddNewSection([FromBody]SectionModel section)
        //{
        //    SettingsLayer settingsLayer = new SettingsLayer();
        //    var rez = settingsLayer.AddNewSection(section);
        //    return rez;
        //}


        /// <summary>
        /// AddMultiplySections
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        [Route("Settings/AddMultiplySections")]
        [HttpPost]
        [ResponseType(typeof(List<SectionModel>))]
        public dynamic AddMultipleSections([FromBody]List<SectionModel> sections)
        {
            foreach (var item in sections)
            {
                SettingsLayer settingsLayer = new SettingsLayer();
                settingsLayer.AddNewSection(item);
            }
           
            return GetQuestionSection(new ScorecardsInfo { id = (int)sections[0].scorecard_id});
        }

        /// <summary>
        /// Update selected Template Item 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [Route("Settings/UpdateTemplateItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic UpdateTemplateItem([FromBody]TemplateItemModel template)
        {
            var layer = new SettingsLayer();
            layer.UpdateTemplateItem(template);
            return layer.GetTemplateItems((int)template.qID);
        }

         /// <summary>
        /// Delete selected template item
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [Route("Settings/DeleteTemplateItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic DeleteTemplateItem([FromBody]TemplateItemModel template)
        {
            var layer = new SettingsLayer();
            layer.DeleteTemplateItem(template);
            return layer.GetTemplateItems((int)template.qID);
        }

         /// <summary>
        /// Add new template item to question
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [Route("Settings/AddTemplateItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic AddTemplateItem([FromBody]TemplateItemModel template)
        {
            var layer = new SettingsLayer();
            layer.AddTemplateItem(template);
            return layer.GetTemplateItems((int)template.qID);
        }


         /// <summary>
        /// Gets all(list) templete items by question id
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetTemplateItems")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic GetTemplateItems([FromBody]int qID)
        {
            var layer = new SettingsLayer();
            return layer.GetTemplateItems(qID);
        }
        /// <summary>
        /// GetTemplateDropdownItems
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetTemplateDropdownItems")]
        [HttpPost]
        [ResponseType(typeof(object[]))]
        public dynamic GetTemplateDropdownItems()
        {
            return new[]
            {
                new {id = 0, value = "None"},
                new {id = 1, value = "Contact"},
                new {id = 2, value = "Schools"},
                new {id = 3, value = "Preferences"}
            };
        }

        /// <summary>
        /// Change Template Item Order
        /// </summary>
        /// <param name="ordering"></param>
        /// <returns></returns>
        [Route("Settings/ChangeTemplateItemOrder")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic ChangeTemplateItemOrder([FromBody]TemlateItemOrdering ordering)
        {
            var layer = new SettingsLayer();
            return layer.ChangeTemplateItemOrder(ordering);
        }


        /// <summary>
        /// GetCalculatedRules by question calc id
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetRuleItems")]
        [HttpPost]
        [ResponseType(typeof(List<RuleItemModel>))]
        public dynamic GetRuleItems([FromBody]int qcID)
        {
            var layer = new SettingsLayer();
            return layer.GetRuleItems(qcID);
        }

        /// <summary>
        /// DeleteCalculateRule
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic DeleteRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.DeleteRuleItem(calculatedRuleModel);
        }

        /// <summary>
        /// UpdateCalculatedRule
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic UpdateRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.UpdateRuleItem(calculatedRuleModel);
        }



        /// <summary>
        /// AddNewCalculatedRule
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/AddRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<TemplateItemModel>))]
        public dynamic AddRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.AddRuleItem(calculatedRuleModel);
        }



        /// <summary>
        /// GetCalculatedRules
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetCalculatedRules")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic GetCalculatedRules([FromBody]int qID)
        {
            var layer = new SettingsLayer();
            return layer.GetCalculatedRules(qID);
        }

        /// <summary>
        /// GetRuleOperatorList
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetRuleOperatorList")]
        [HttpPost]
        [ResponseType(typeof(List<object>))]
        public dynamic GetRuleOperatorList()
        {

            return new[]
            {
                new {id = 0,value = "(select)"},
                new {id = 1,value = "NA"},
                new {id = 2,value = "="},
                new {id = 3,value = ">"},
                new {id = 4,value = "<"},
                new {id = 5,value = "IN"},
                new {id = 6,value = "Like"},
                new {id = 7,value = "<>"},
                new {id = 8,value = "Not IN"},
                new {id = 9,value = "Not Like"},
                new {id = 10,value = "is null"},
                new {id = 11,value = "is not null"},
                new {id = 12,value = "is missing"}
            };
        }


        /// <summary>
        /// GetRuleTypeList
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetRuleTypeList")]
        [HttpPost]
        [ResponseType(typeof(List<object>))]
        public dynamic GetRuleTypeList()
        {

            return new[]
            {
                new {id = 0,value = "(select)"},
                new {id = 1,value = "Call Data"},
                new {id = 2,value = "School Data"},
                new {id = 3,value = "Other Data Item"},
                new {id = 4,value = "Other SQL"},
                new {id = 4,value = "Answer Comment"},
                new {id = 4,value = "Question Answer"},
            };
        }


        /// <summary>
        /// AddCalculatedRule
        /// </summary>
        /// <param name="questionCalc"></param>
        /// <returns></returns>
        [Route("Settings/AddCalculatedRule")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic AddCalculatedRule([FromBody]QuestionCalc questionCalc)
        {
            var layer = new SettingsLayer();
            return layer.AddCalculatedRule(questionCalc);
        }

        /// <summary>
        /// UpdateCalculatedRule
        /// </summary>
        /// <param name="questionCalc"></param>
        /// <returns></returns>
        [Route("Settings/UpdateCalculatedRule")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic UpdateCalculatedRule([FromBody]QuestionCalc questionCalc)
        {
            var layer = new SettingsLayer();
            return layer.UpdateCalculatedRule(questionCalc);
        }

        /// <summary>
        /// DeleteCalculatedRule
        /// </summary>
        /// <param name="questionCalc"></param>
        /// <returns></returns>
        [Route("Settings/DeleteCalculatedRule")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic DeleteCalculatedRule([FromBody]RuleModel questionCalc)
        {
            var layer = new SettingsLayer();
            return layer.DeleteCalculatedRule(questionCalc);
        }

        /// <summary>
        /// GetDynamicRuleItems
        /// </summary>
        /// <param name="qcId"></param>
        /// <returns></returns>
        [Route("Settings/GetDynamicRuleItems")]
        [HttpPost]
        [ResponseType(typeof(List<RuleItemModel>))]
        public dynamic GetDynamicRuleItems([FromBody]int qcId)
        {
            var layer = new SettingsLayer();
            return layer.GetDynamicRuleItems(qcId);
        }

        /// <summary>
        /// GetDynamicRules
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetDynamicRules")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic GetDynamicRules([FromBody]int qID)
        {
            var layer = new SettingsLayer();
            return layer.GetDynamicRules(qID);
        }

        /// <summary>
        /// DeleteDynamicRuleItem
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/DeleteDynamicRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic DeleteDynamicRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.DeleteDynamicRuleItem(calculatedRuleModel);
        }

        /// <summary>
        /// UpdateDynamicRuleItem
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/UpdateDynamicRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic UpdateDynamicRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.UpdateDynamicRuleItem(calculatedRuleModel);
        }



        /// <summary>
        /// AddDynamicRuleItem
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/AddDynamicRuleItem")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic AddDynamicRuleItem([FromBody]RuleItemModel calculatedRuleModel)
        {
            var layer = new SettingsLayer();
            return layer.AddDynamicRuleItem(calculatedRuleModel);
        }
        /// <summary>
        /// AddMultipleDynamicRuleItemsSimple - response : list of rule items only
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/AddMultipleDynamicRuleItemsSimple")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic AddDynamicRuleItemSimple([FromBody]List<RuleItemModel> calculatedRuleModels)
        {
            var layer = new SettingsLayer();
            foreach (var item in calculatedRuleModels)
            {
                layer.AddDynamicRuleItem(item);
            }
            return GetRuleItems((int)calculatedRuleModels[0].qcId);
        }
        /// <summary>
        /// AddDynamicRuleItem
        /// </summary>
        /// <param name="calculatedRuleModel"></param>
        /// <returns></returns>
        [Route("Settings/AddMultipleDynamicRuleItems")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic AddMultipleDynamicRuleItems([FromBody]List<RuleItemModel> calculatedRuleModels)
        {
            var layer = new SettingsLayer();
            foreach (var item in calculatedRuleModels)
            {
                layer.AddDynamicRuleItem(item);
            }
            return GetDynamicRules((int)calculatedRuleModels[0].qID);
        }


        /// <summary>
        /// GetRulesCalc
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetRulesCalc")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic GetRulesCalc([FromBody]int qID)
        {
            var layer = new SettingsLayer();
            return layer.GetRulesCalc(qID);
        }

        /// <summary>
        /// AddMultipleRuleItems - respose : List of whole things (rules with list of rule items) 
        /// </summary>
        /// <param name="ruleItemModels"></param>
        /// <returns></returns>
        [Route("Settings/AddMultipleRuleItems")]
        [HttpPost]
        [ResponseType(typeof(List<RuleModel>))]
        public dynamic AddMultipleRuleItems([FromBody]List<RuleItemModel> ruleItemModels)
        {
            var layer = new SettingsLayer();
            return layer.AddMultipleRuleItems(ruleItemModels);
        }


        /// <summary>
        /// AddMultipleRuleItemsSimple - respose : List of rule items only
        /// </summary>
        /// <param name="ruleItemModels"></param>
        /// <returns></returns>
        [Route("Settings/AddMultipleRuleItemsSimple")]
        [HttpPost]
        [ResponseType(typeof(List<RuleItemModel>))]
        public dynamic AddMultipleRuleItemsSimple([FromBody]List<RuleItemModel> ruleItemModels)
        {
            var layer = new SettingsLayer();
            return layer.AddMultipleRuleItemsSimple(ruleItemModels);
        }

        /// <summary>
        /// GetLinkedItems
        /// </summary>
        /// <param name="qID"></param>
        /// <returns></returns>
        [Route("Settings/GetLinkedItems")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic GetLinkedItems([FromBody]int qID)
        {
            var layer = new SettingsLayer();
            return layer.GetLinkedItems(qID);
        }

        /// <summary>
        /// AddLinkedItem
        /// </summary>
        /// <param name="linkedItem"></param>
        /// <returns></returns>
        [Route("Settings/AddLinkedItem")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic AddLinkedItem([FromBody]LinkedItemModel linkedItem)
        {
            var layer = new SettingsLayer();
            return layer.AddLinkedItem(linkedItem);
        }


        /// <summary>
        /// AddLinkedItems
        /// </summary>
        /// <param name="linkedItem"></param>
        /// <returns></returns>
        [Route("Settings/AddLinkedItems")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic AddLinkedItem([FromBody]List<LinkedItemModel> linkedItems)
        {
            var layer = new SettingsLayer();

            foreach (var item in linkedItems)
            {
                layer.AddLinkedItem(item);
            }
            return layer.GetLinkedItems((int)linkedItems[0].linkedParentQuestion);
        }

        /// <summary>
        /// UpdateLinkedItem
        /// </summary>
        /// <param name="linkedItem"></param>
        /// <returns></returns>
        [Route("Settings/UpdateLinkedItem")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic UpdateLinkedItem([FromBody]LinkedItemModel linkedItem)
        {
            var layer = new SettingsLayer();
            return layer.UpdateLinkedItem(linkedItem);
        }


        /// <summary>
        /// DeleteLinkedItem
        /// </summary>
        /// <param name="linkedItem"></param>
        /// <returns></returns>
        [Route("Settings/DeleteLinkedItem")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionCalc>))]
        public dynamic DeleteLinkedItem([FromBody]LinkedItemModel linkedItem)
        {
            var layer = new SettingsLayer();
            return layer.DeleteLinkedItem(linkedItem);
        }


        /// <summary>
        /// GetSchoolDataItems
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetSchoolDataItems")]
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public dynamic GetSchoolDataItems()
        {
            var layer = new SettingsLayer();
            return layer.GetSchoolDataItems();
        }


        /// <summary>
        /// GetCallDataItems
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetCallDataItems")]
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public dynamic GetCallDataItems()
        {
            var layer = new SettingsLayer();
            return layer.GetCallDataItems();
        }


        /// <summary>
        /// GetOtherDataItems
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetOtherDataItems")]
        [HttpPost]
        [ResponseType(typeof(List<string>))]
        public dynamic GetOtherDataItems()
        {
            //var layer = new SettingsLayer();
            //return layer.GetOtherDataItems();
            return new[]
            {
                new {id = 0,value = "xcc_id"},
                new {id = 1,value = "data_key"},
                new {id = 2,value = "data_value"}
            };
        }


        /// <summary>
        /// GetQuestionsWithoutCalculated
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetQuestionsForCalculatedRules")]
        [HttpPost]
        [ResponseType(typeof(List<QuestiomModelSimple>))]
        public dynamic GetQuestionsForCalculatedRules([FromBody]int scID)
        {
            var layer = new SettingsLayer();
            return layer.GetQuestionsForCalculatedRules(scID);
        }


        /// <summary>
        /// GetQuestionsWithAnswers by scorecard id "it's API for Calculated Rules,
        /// so the API is returning list of questions (without calculated ones) with list of their answers"
        /// </summary>
        /// <returns></returns>
        [Route("Settings/GetQuestionsWithAnswers")]
        [HttpPost]
        [ResponseType(typeof(List<QuestionsWithAnswerListModel>))]
        public dynamic GetQuestionsWithAnswers([FromBody]int scID)
        {
            var layer = new SettingsLayer();
            return layer.GetQuestionsWithAnswers(scID);
        }

    }
}