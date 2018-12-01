using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class CalibrationPendingSummary
    {
        public int pending_calibrations { get; set; }
        public double pending_reviev_time { get; set; }
        public int scorecardId { get; set; }
        public string scorecardName { get; set; }
        public DateTime oldestCall { get; set; }
        public bool haveAccess { get; set; }
    }
}