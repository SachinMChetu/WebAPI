using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CallsInfo
    {
        public string appname { get; set; }
        public Scorecard scorecard { get; set; }
        public int id { get; set; }
    }
}