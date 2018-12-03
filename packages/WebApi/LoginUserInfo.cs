using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginUserInfo
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionModel"></param>
        public LoginUserInfo(SessionModel sessionModel)
        {
            Guid guid = new Guid();
            HttpContext.Current.Session[guid.ToString()] = sessionModel;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SessionModel
    {
        /// <summary>
        /// 
        /// </summary>
        public MembershipUser User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> UserRoles { get; set; }
    }
}