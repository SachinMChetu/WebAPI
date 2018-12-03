using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class SpotCheckDataModel
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string scorecard { get; set; }
        public string appname { get; set; }
        public string team_lead { get; set; }
    }
}