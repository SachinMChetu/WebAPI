using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using DAL;
using DAL.Models;
using static DAL.GenericRepository.ExportHelper;

namespace WebApi.Controllers
{
    /*[EnableCors(origins: "http://fiddle.jshell.net", headers: "Access-Control-Allow-Credentials:*", methods: "*")]*/

#if DEBUG

#else
     [Authorize]
#endif
    public class SessiontestController : ApiController
    {
        /// <summary>
        /// UName
        /// </summary>
        /// <returns></returns>
        [Route("CORS-TEST")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public string UName()
        {
            return "CORS working"; ;
        }
        [Route("exporttest")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public dynamic exportdata()
        {
            string userName = "";
            if (HttpContext.Current.Request.UrlReferrer != null && (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268))
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            DAL.GenericRepository.ExportHelper.Export<string>(null, null, "testTab.xlsx","test",userName);
            return Ok();
        }

        /// <summary>
        /// GetUname
        /// </summary>
        /// <returns></returns>
        [Route("GetUname")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public string GetUname()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            var asss = HttpContext.Current.Request.Cookies.Count;
            string ccc = "";
            foreach (var c in HttpContext.Current.Request.Cookies)
            {
                ccc += c.ToString();
                ccc += ":";
                ccc += HttpContext.Current.Request.Cookies[c.ToString()].Value;
                ccc += "<br/>";
            }
            if (authCookie == null)
            {
                return asss.ToString();
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
            }

            return authTicket.Name;

        }
    }
}
