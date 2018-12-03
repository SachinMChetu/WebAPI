using System.Web.Http.Filters;
using System.Web.Mvc;
using WebApi.Controllers;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new BasicAuthenticationAttribute());
        }
    }
}
