using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using DAL.Models;
using DAL.Models.CalibrationModels;

namespace WebApi.Controllers
{
    public class CalibrationController : ApiController
    {
        /// <summary>
        /// SearchCalls method
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [Route("calibration/searchcalls")]
        [HttpPost]
        [ResponseType(typeof(CallDetailsResponseData))]
        //[Authorize(Roles = "getMyModules")]
        public CallDetailsResponseData SearchCalls([FromBody]string searchText)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            return calibrationLayer.SearchCalls(searchText);

        }

        /// <summary>
        /// SearchCallsByAppName
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("calibration/searchcallsbyAppName")]
        [HttpPost]
        [ResponseType(typeof(CallDetailsResponseData))]
        //[Authorize(Roles = "getMyModules")]
        public CallDetailsResponseData SearchCallsByAppName([FromBody]CalibrationPageSearch search)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            return calibrationLayer.SearchCallsByAppName(search);
        }

        /// <summary>
        /// AddCallsToCalibration method 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Route("calibration/AddCallsToCalibration")]
        [HttpPost]
        [ResponseType(typeof(IHttpActionResult))]
        public IHttpActionResult AddCallsToCalibration([FromBody]EndPointData action)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            calibrationLayer.AddCallsToCalibration(action);
            return Ok();
        }

        /// <summary>
        /// GetCalibrationQueue
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<CalibrationsPendingInfo>))]
        [Route("calibration/GetCalibrationQueue")]
        [HttpPost]
        public List<CalibrationsPendingInfo> GetCalibrationQueue([FromBody]string appName)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            return calibrationLayer.GetCalibrationQueue(appName);
        }


        /// <summary>
        /// GetCalibrationCalls
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IHttpActionResult))]
        [Route("calibration/GetCalibrationCalls")]
        [HttpPost]
        public CalibrationCallsInfo GetCalibrationCalls([FromBody]int scorecardId)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            return calibrationLayer.GetCalibrationCalls(scorecardId);
        }


        /// <summary>
        /// CompleteReview
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(IHttpActionResult))]
        [Route("calibration/CompleteReview")]
        [HttpPost]
        public IHttpActionResult CompleteReview([FromBody] int id)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            calibrationLayer.CompleteReview(id);
            return Ok();

        }


        /// <summary>
        /// DeleteReview
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(IHttpActionResult))]
        [Route("calibration/DeleteReview")]
        [HttpPost]
        public IHttpActionResult DeleteReview([FromBody] int id)
        {
            CalibrationLayer calibrationLayer = new CalibrationLayer();
            calibrationLayer.DeleteReview(id);
            return Ok();
        }
    }
}