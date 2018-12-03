using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using DAL.Models;
using DAL.Models.Guidelines;

namespace WebApi.Controllers
{
    public class GuidelinesController : ApiController
    {
        // GET: Guidelines
        /// <summary>
        /// GetScorecardInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Guidelines/GuidelineScorecardInfo")]
        [HttpPost]
        [ResponseType(typeof(GuidelineScorecardInfo))]
        public dynamic GetScorecardInfo([FromBody]CallInfo info)
        {
            var guidelines = new GuidelinesLayer();
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            try
            {
                return guidelines.GetGuidelines(info.scorecardId, info.reviewId, info.listenId, userName);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Selected scorecard does not contain record with this ID");
            }
            catch (Exception)
            {
                return new GuidelineScorecardInfo();
            }

        }
        /// <summary>
        /// ResetScorecardInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Guidelines/ResetScorecardInfo")]
        [HttpPost]
        [ResponseType(typeof(GuidelineScorecardInfo))]
        public dynamic ResetScorecardInfo([FromBody]CallInfo info)
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            var guidelines = new GuidelinesLayer();
            try
            {
                return guidelines.GetGuidelines(info.scorecardId, info.reviewId, info.listenId, userName, 1);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Selected scorecard does not contain record with this ID");
            }
            catch (Exception)
            {
                return new GuidelineScorecardInfo();
            }

        }

        /// <summary>
        /// SaveSchoolDataInfo
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Route("Guidelines/SaveSchoolDataInfo")]
        [HttpPost]
        [ResponseType(typeof(ScorecardInfo))]
        public dynamic SaveSchoolDataInfo([FromBody]List<SchoolDataItem> payload)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.SaveSchoolDataInfo(payload);
        }

        /// <summary>
        /// UpdateMetadataInfo
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Route("Guidelines/UpdateMetadataInfo")]
        [HttpPost]
        [ResponseType(typeof(ScorecardInfo))]
        public dynamic UpdateMetadataInfo([FromBody]UpdateMetadataPayload payload)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.UpdateMetadataInfo(payload);
        }

        /// <summary>
        /// SaveOtherDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Guidelines/SaveOtherDataInfo")]
        [HttpPost]
        [ResponseType(typeof(ScorecardInfo))]
        public dynamic SaveOtherDataInfo([FromBody]List<OtherDataItem> info)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.SaveOtherDataInfo(info);
        }


        /// <summary>
        /// RemoveOtherDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Guidelines/RemoveOtherDataInfo")]
        [HttpPost]
        [ResponseType(typeof(ScorecardInfo))]
        public dynamic RemoveOtherDataInfo([FromBody]OtherDataItem info)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.RemoveOtherDataInfo(info);
        }


        /// <summary>
        /// RemoveSchoolDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("Guidelines/RemoveSchoolDataInfo")]
        [HttpPost]
        [ResponseType(typeof(ScorecardInfo))]
        public dynamic RemoveSchoolDataInfo([FromBody]SchoolDataItem info)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.RemoveSchoolDataInfo(info);
        }

        /// <summary>
        /// UpdateScorecardStatus
        /// </summary>
        /// <param name="scorecardID"></param>
        /// <returns></returns>
        [Route("Guidelines/UpdateScorecardStatus")]
        [HttpPost]
        public dynamic UpdateScorecardStatus([FromBody]int scorecardID)
        {

            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.UpdateScorecardStatus(scorecardID);
        }
        /// <summary>
        /// Returning history of question FAQ changes
        /// </summary>
        /// <param name="scorecardID"></param>
        /// <returns></returns>
        [Route("Guidelines/GetQuestionsHistory")]
        [ResponseType(typeof(List<QuestionHistory>))]
        [HttpPost]
        public dynamic GetQuestionsHistory([FromBody]int scorecardID)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.GetQuestionHistory(scorecardID);
        }
        /// <summary>
        /// Returning history of question FAQ changes 
        /// </summary>
        /// <param name="qId"></param>
        /// <returns></returns>
        [Route("Guidelines/GetQuestionsHistoryByQId")]
        [ResponseType(typeof(GuidelinesQHistoryInfo))]
        [HttpPost]
        public dynamic GetQuestionsHistoryByQId([FromBody]int qId)
        {
            GuidelinesLayer guidelinesLayer = new GuidelinesLayer();
            return guidelinesLayer.GetQuestionHistoryByQId(qId);
        }

    }
}
