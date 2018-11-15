using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExportMyPayModel
    {
        public DateTime weekEnd { get; set; }
        public string scorecardName { get; set; }
        public int? scorecardId { get; set; }
        //QaPay
        public string payType { get; set; }
        public string qaPay { get; set; }
        public int? numberCalls { get; set; }

        public DateTime periodEnding { get; set; }                      //db
        public float? totalCallTime { get; set; }                        //db
        public float? totalReviewTime { get; set; }                      //db
        public float? totalBadCallTime { get; set; }                     //db
        public float? totalBadCallReviewTime { get; set; }               //db
        public float? baseRate { get; set; }                             //db
        public float? adjustedRate { get; set; }                         //calc
        public float? totalPay { get; set; }                             //calc
        public float? callSpeed { get; set; }                            //calc
        public float? disputeCost { get; set; }                          //db
        public int? disputeCount { get; set; }                           //db
        public float? score { get; set; }                                //db
        public float? percentChange { get; set; }                        //db
        public float? paymentRate { get; set; }
        public float? calibrationCount { get; set; }
        public int? complitedNotification { get; set; }
        public float? complitedСost { get { return 0.4f; } set { } }

        public DateTime? startDate { get; set; }

        //CalibratorPay
        public string CpayType { get; set; }
        public string CqaPay { get; set; }
        public int? CnumberCalls { get; set; }

        public DateTime? CperiodEnding { get; set; }                      //db
        public float? CtotalCallTime { get; set; }                        //db
        public float? CtotalReviewTime { get; set; }                      //db
        public float? CtotalBadCallTime { get; set; }                     //db
        public float? CtotalBadCallReviewTime { get; set; }               //db
        public float? CbaseRate { get; set; }                             //db
        public float? CadjustedRate { get; set; }                         //calc
        public float? CtotalPay { get; set; }                             //calc
        public float? CcallSpeed { get; set; }                            //calc
        public float? CdisputeCost { get; set; }                          //db
        public int? CdisputeCount { get; set; }                           //db
        public float? Cscore { get; set; }                                //db
        public float? CpercentChange { get; set; }                        //db
        public float? CpaymentRate { get; set; }
        public float? CcalibrationCount { get; set; }
        public int? CcomplitedNotification { get; set; }
        public float? CcomplitedСost { get { return 0.4f; } set { } }

        public DateTime? CstartDate { get; set; }
    }
}
