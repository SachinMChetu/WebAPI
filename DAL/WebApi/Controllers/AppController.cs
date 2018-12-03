//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;

//namespace WebApi.Controllers
//{
//    public class AppController : Controller
//    {
//        // GET: Get
//        //[Route("app/home")]
//        //public async Task<ActionResult> Index()
//        //{
//        //    ViewBag.Title = "title";

//        //    ViewBag.uSettings = "title";

//        //    ViewBag.current_version = "title";
//        //    return View();
//        //}
//        [Route("app/home")]
//        public  InitialData Index()
//        {

//            InitialData initialData = new InitialData();
//            return View("index",initialData.GetInitialData("",""));

//        }

//        public class InitialData
//        {
//            public string title { get; set; }
//            public string uSettings { get; set; }
//            public string currentVersion { get; set; }
//            public string jsAppName { get; set; }

//            public  InitialData GetInitialData(string userName, string previousUser)
//            {
//                InitialData a = new InitialData();
//                a.title = "ettwet";



//                return a;
//            }


//        }
//    }
//}