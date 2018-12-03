using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CalibrationsPendingInfo
    {
        public int pendingCalibrations { get; set; }
        public decimal pendingReviewTime { get; set; }
        public DateTime oldestCall { get; set; }
        public Scorecard scorecard { get; set; }
    }
}