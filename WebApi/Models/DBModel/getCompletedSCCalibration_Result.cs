using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getCompletedSCCalibration_Result
    {
        public int? active_cali { get; set; }
        public double? recal_score { get; set; }
        public int? golden_count { get; set; }
        public string user_role { get; set; }
        public string golden { get; set; }
        public decimal? total_score { get; set; }
        public DateTime? review_date { get; set; }
        public string reviewed_by { get; set; }
    }
}