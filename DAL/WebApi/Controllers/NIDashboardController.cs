using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class NIDashboardController : Controller
    {
        // GET: NIDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}