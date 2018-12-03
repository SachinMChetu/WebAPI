using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        private const string BasicAuthResponseHeader = "WWW-Authenticate";

        /// <summary>
        /// 
        /// </summary>
        private const string BasicAuthResponseHeaderValue = "Basic";

        /// <summary>
        /// 
        /// </summary>
        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!object.Equals(HttpContext.Current.Session["UserInfo"], null))
            {
                HttpContext.Current.User = (CustomPrincipal)HttpContext.Current.Session["UserInfo"];
                return;
            }
            else
            {
                string strCredential = System.Web.HttpUtility.UrlDecode(actionContext.Request.RequestUri.Query);
                if (strCredential.Contains("userName") && strCredential.Contains("password"))
                {
                    string authInfo = strCredential.Split('=')[1].Split('&')[0] + ":" + strCredential.Split('=')[2];
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    actionContext.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(BasicAuthResponseHeaderValue, authInfo);
                    var authHeader = actionContext.Request.Headers.Authorization;
                    if (authHeader != null)
                    {
                        // Gets header parameters  
                        string authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                        string originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));

                        // Gets username and password  
                        string userName = originalString.Split(':')[0];
                        string password = originalString.Split(':')[1];


                        if (Membership.ValidateUser(userName, password))
                        {
                            var roles = Roles.GetRolesForUser(userName);
                            var guid = Guid.NewGuid().ToString();
                            CurrentUser = new CustomPrincipal(userName, roles, guid);
                           
                            HttpContext.Current.Session["UserInfo"] = CurrentUser;
                          
                            actionContext.Response =
                               actionContext.Request.CreateResponse(HttpStatusCode.OK,
                                  "User " + userName + " successfully authenticated");

                            return;
                        }
                    }
                }
                else
                {
                    actionContext.Response =
                               actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                                  "Username/password not valid, please enter valid credentials");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool IsAuthorized(HttpContext httpContext)
        {
            bool success = false;
            return success;
        }



        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}