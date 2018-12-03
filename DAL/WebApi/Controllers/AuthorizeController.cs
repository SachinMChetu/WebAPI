using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using WebApi.DataLayer;
using WebApi.Models;
using WebApi.Models.CCInternalAPI;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("v2.4")]
    public class AuthorizeController : ApiController
    {

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
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("Authorize/Login")]
        [HttpPost]
        public dynamic Login(string userName, string password)
        {

            LoginInfo objLogin = new LoginInfo();
            Response objResponse = new Response();
            if (userName != null && password != null)
            {
                try
                {
                    objLogin.UserName = userName;
                    objLogin.Password = password;
                    MembershipUser user = Membership.GetUser(objLogin.UserName);
                    bool userValidate = Membership.ValidateUser(objLogin.UserName, objLogin.Password);
                    BaseRequestModel objBaseRequest = new BaseRequestModel();

                    if (user != null && user.IsLockedOut)
                    {
                        objResponse.Message = Messages.UserLockedMessage;
                    }
                    if (user != null && userValidate == true)
                    {
                        //Roles.AddUserToRole(userName,"Admin");
                        //bool isUserInRole = Roles.IsUserInRole("Admin");
                        string[] rolesForUser = Roles.GetRolesForUser(objLogin.UserName);
                        if (rolesForUser.Length > 0)
                        {
                            var UserName = user.UserName;
                            //var Password = user.GetPassword;
                            if (UserName == objLogin.UserName)
                            {

                                //SessionModel sessionModel = new SessionModel();
                                //sessionModel.User = user;
                                //sessionModel.UserRoles = rolesForUser.ToList();
                                //CurrentUser = new CustomPrincipal(userName, rolesForUser);
                                //HttpContext.Current.User = CurrentUser;
                               // HttpContext.Current.Session["User"] = CurrentUser;

                                //LoginUserInfo usr = new LoginUserInfo(sessionModel);

                                //HttpClient client = new HttpClient();
                                //string authInfo = objLogin.UserName + ":" + objLogin.Password;
                                //authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

                                objResponse.Message = Messages.LoginSuccess;
                                objResponse.Status = 1;

                            }
                            else
                            {
                                objResponse.Message = Messages.LoginFailure;
                                objResponse.Status = 0;
                            }
                        }
                        else
                        {
                            objResponse.Message = Messages.InvalidCredentials;
                            objResponse.Status = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objResponse.Message = Messages.LoginFailure;
                    objResponse.Status = 0;
                }
            }
            return objResponse;
        }
        public dynamic LogOut()
        {
            throw new NotImplementedException();
        }
        public dynamic ChangePassword()
        {
            throw new NotImplementedException();
        }
    }
}