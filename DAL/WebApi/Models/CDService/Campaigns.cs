using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Campaigns
    {
       public string start_date { get; set; }
        public string end_date { get; set; }
        public string scorecard { get; set; }
        public string group { get; set; }
    }
}