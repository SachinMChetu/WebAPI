using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.DataLayer;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.DataLayer;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Swashbuckle.Swagger;

namespace WebApi.Controllers
{

#if DEBUG

#else
        [Authorize]
#endif
    public class UserController : ApiController
    {
        /// <summary>
        /// GetSavedUserSettings
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        [Route("v2.1/user/GetSavedUserSettings")]
        [Route("v2/user/GetSavedUserSettings")]
        [Route("v1/user/GetSavedUserSettings")]
        [HttpPost]
        [ResponseType(typeof(List<SavedUserSettings>))]
        [Authorize]
        public async Task<List<SavedUserSettings>> GetSavedUserSettings([FromBody] List<string> names)
        {
            UserLayer userLayer = new UserLayer();


            string userName = HttpContext.Current.User.Identity.Name;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321"; // HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            string PreviousUser = null;
            try
            {
                PreviousUser = UserImpersonation.PrevUserName;
            }
            catch (Exception)
            {
                // ignored
            }

            return await userLayer.GetSavedUserSettings(names,userName);
        }

        /// <summary>
        /// SaveUserSettings
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        [Route("v2.1/user/SaveUserSettings")]
        [Route("v2/user/SaveUserSettings")]
        [Route("v1/user/SaveUserSettings")]
        [ResponseType(typeof(string))]
        [HttpPost]
        [Authorize]
        public string SaveUserSettings(SavedUserSettings setting)
        {
            UserLayer userLayer = new UserLayer();
            return userLayer.SaveUserSettings(setting);
        }

        /// <summary>
        /// GetSession
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(UserObject))]

        [Route("v2.1/user/GetSession")]
        [Route("v2/user/GetSession")]
        [Route("v1/user/GetSession")]
        [HttpPost]
        //   [Authorize]
        public async Task<UserObject> GetSession()
        {
            UserLayer userLayer = new UserLayer();
            UserObject uo = await userLayer.GetSession();
            try
            {
                uo.PreviousUser = UserImpersonation.PrevUserName;
            }
            catch (Exception)
            {

                // ignored
            }
            return uo;
        }
        [Route("v2.1/user/GetSessionAsync")]
        [Route("v2/user/GetSessionAsync")]
        [Route("v1/user/GetSessionAsync")]
        [HttpPost]
        //   [Authorize]
        public async Task<UserObject> GetSessionAsync()
        {
            UserLayer userLayer = new UserLayer();

            string userName = HttpContext.Current.User.Identity.Name;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321"; // HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            string previousUser = null;
            try
            {
                previousUser = UserImpersonation.PrevUserName;
            }
            catch (Exception)
            {
                // ignored
            }
            var uo= await userLayer.GetSessionAsync(userName, previousUser);
            uo.PreviousUser = previousUser;
            return uo;
        }


        /// <summary>
        /// GetCallsCount
        /// </summary>
        /// <returns></returns>
        [Route("v2.1/user/GetCallsCount")]
        [Route("v2/user/GetCallsCount")]
        [Route("v1/user/GetCallsCount")]
        [ResponseType(typeof(string))]
        [HttpPost]
        public CallsCountModel GetCallsCount()
        {
            UserLayer layer = new UserLayer();
            return layer.GetCallsCount();
        }

        /// <summary>
        /// GetCallsCount
        /// </summary>
        /// <returns></returns>
        [Route("v2.1/user/GetAllUserSettings")]
        [Route("v2/user/GetAllUserSettings")]
        [Route("v1/user/GetAllUserSettings")]
        [HttpPost]
        [ResponseType(typeof(List<AllUserSettings>))]

        public async Task<AllUserSettings> GetAllUserSettings()
        {
            UserLayer userLayer = new UserLayer();
            string userName = HttpContext.Current.User.Identity.Name;
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321"; // HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            string PreviousUser = null;
            try
            {
                 PreviousUser = UserImpersonation.PrevUserName;
            }
            catch (Exception)
            { 
                // ignored
            }

            var allsettings = await userLayer.GetAllUserSettings(userName, PreviousUser);


        
            return allsettings;
        }

        /// <summary>
        /// Geting CarQueryApi from web site by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Route("v2.1/user/GetCarQueryApi")]
        [Route("v2/user/GetCarQueryApi")]
        [Route("v1/user/GetCarQueryApi")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic GetCarQueryApi([FromBody]Url url)
        {
            string content;
            using (WebClient wc = new WebClient())
            {
                content = wc.DownloadString(url.url);
            }
            return content;
        }

    }
    /// <summary>
    /// Url
    /// </summary>
    public class Url
    {
        /// <summary>
        /// url
        /// </summary>
        public string url { get; set; }
    }
 
}
