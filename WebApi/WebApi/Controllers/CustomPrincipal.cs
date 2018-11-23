using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomPrincipal: IPrincipal
    {
        /// <summary>
        /// 
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return UserRoles.Any(r => role.Contains(r));
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="roles"></param>
        public CustomPrincipal(string Username,  string[] roles, string guid)
        {
            Identity = new GenericIdentity(Username);
            UserRoles = roles;
            Guid = guid;
        }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] UserRoles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentGuid { get; set; }
    }
}