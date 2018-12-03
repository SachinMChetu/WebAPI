using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using DAL.Layers;
using System.Web.Http.Description;
using WebApi.RequestParams;
using DAL.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("v1.0")]
    [ApiExplorerSettings(IgnoreApi = false)]
#if DEBUG

#else
         [Authorize]
#endif
    public class SchedulingController : ApiController
    {
        // GET: Scheduling
        /// <summary>
        /// GetUserSettings
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<ExtendedUserProfileModel>))]
        [Route("scheduling/GetUserSettings")]
        [HttpPost]
        public dynamic GetUserSettings([FromBody]string userName)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();

            return schedulingLayer.GetSchedulingUserSettings(userName);
        }


        /// <summary>
        /// UpdateUserSettings
        /// </summary>
        /// <param name="UserSettings"></param>
        /// <returns></returns>
        [Route("scheduling/UpdateUserSettings")]
        [HttpPost]
        [ResponseType(typeof(IHttpActionResult))]
        public IHttpActionResult UpdateUserSettings([FromBody]ExtendedUserProfileModel UserSettings)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            schedulingLayer.UpdateSchedulingUserSettings(UserSettings.userId, UserSettings.userName, UserSettings.hoursPerWeek, UserSettings.daysPerWeek, UserSettings.prefStartHour);
            return Ok();
        }


        /// <summary>
        /// GetAllWorkingHoursAndDays
        /// </summary>
        /// <returns></returns>
        [Route("scheduling/getAvailablePeriods")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAllWorkingHoursAndDays()
        {
            var schedulingLayer = new SchedulingLayer();
            try
            {
                var d = new
                {
                    days = schedulingLayer.GetAvailableDaysPeriods(),
                    hours = schedulingLayer.GetAvailableDaysPeriods()
                };
                return d;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// SetUserWorkingHour
        /// </summary>
        /// <param name="workingHours"></param>
        /// <returns></returns>
        [Route("scheduling/SetUserWorkingHour")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic SetUserWorkingHour([FromBody]List<WorkingHours> workingHours)
        {
            var schedulingLayer = new SchedulingLayer();
            
            //schedulingLayer.u
            return workingHours;
        }

        /// <summary>
        /// GetRequieredQAs
        /// </summary>
        /// <param name="scQAsRequiredRQ"></param>
        /// <returns></returns>
        [Route("scheduling/GetRequiredQAs")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetRequiredQAs([FromBody]ScQAsRequiredRQ scQAsRequiredRQ)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            return schedulingLayer.GetRequieredQAs(scQAsRequiredRQ.appname, scQAsRequiredRQ.scorecardId, scQAsRequiredRQ.startDate, scQAsRequiredRQ.endDate);
        }

        /// <summary>
        /// UpdateRequieredQAsByScorecardId
        /// </summary>
        /// <param name="qAModel"></param>
        /// <returns></returns>
        [Route("scheduling/UpdateRequiredQAsByScorecardId")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateRequiredQAsByScorecardId([FromBody]List<DAL.Models.RequieredQAModel> qAModel)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            schedulingLayer.UpdateRequieredQAs(qAModel);
            return qAModel;
        }
        /// <summary>
        /// GetInitialInfo
        /// </summary>
        /// <returns></returns>
        [Route("scheduling/getInitialInfo")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetInitialInfo()
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
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            return schedulingLayer.GetInitialInfo(userName);
        }
        /// <summary>
        /// GetAvailableQAs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("scheduling/GetAvailableQAs")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetAvailableQAs([FromBody]DAL.Models.DateAppNameModel model)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            return schedulingLayer.GetAvailableQAs(model);
        }

        /// <summary>
        /// UpdateAvailableQAs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("scheduling/GetAvailableQAs")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateAvailableQAs([FromBody]DAL.Models.UpdateAvailableQAsModel model)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            schedulingLayer.UpdateAvailableQAs(model);
            return model;
        }

        /// <summary>
        /// GetSchedulingTimeShift method
        /// </summary>
        /// <param name="timeShiftId"></param>
        /// <returns></returns>
        [Route("scheduling/GetSchedulingTimeShift")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetSchedulingTimeShift([FromBody]int timeShiftId)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            return schedulingLayer.GetTimeShift(timeShiftId);
        }

        /// <summary>
        /// UpdateTimeShift
        /// </summary>
        /// <param name="modelShift"></param>
        /// <returns></returns>
        [Route("scheduling/UpdateTimeShift")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic UpdateTimeShift([FromBody]DAL.Models.TimeShift modelShift)
        {
            SchedulingLayer schedulingLayer = new SchedulingLayer();
            return schedulingLayer.UpdateTimeShift(modelShift);
            
        }

    }
}