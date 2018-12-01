using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CalibrationCallsInfo
    {
        public List<CalibrationCalls> pending { get; set; }
        public List<CalibrationCalls> completed { get; set; }
    }
}