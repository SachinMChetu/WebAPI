using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class AppStatus
    {
        public string appname;
        public string total_loaded;
        public string pending;
        public string need_audio;
        public string bad_calls;
        public string Priority;
        public string Last_Loaded_Date;
        public string avg_score;
        public string std_dev;
        public string number_loaded;
        public string call_date;
    }
}