using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Extensions
{
	/// <summary>
	///		Getting the username here instead in every function ad nauseum
	/// </summary>
	public static class HttpUserNameExtension
	{
		/// <summary>
		///   Getting the username
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static string GetUserName(this HttpContext context)
		{
			string userName = "";
			if (context.Request.UrlReferrer != null && (context.Request.UrlReferrer.Host.Contains("localhost") && context.Request.UrlReferrer.Port == 51268))
			{
#if IS_CLB
				userName = "papadmin";
#else
				userName = "test321";
#endif
			}
			else
			{
				userName = context.User.Identity.Name;
			}
			return userName;
		}


    }
}