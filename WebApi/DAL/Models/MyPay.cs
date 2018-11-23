using System;
using System.Collections.Generic;


namespace DAL.Models
{
    public class MyPay
    {
        public List<List<ScorecardPaymentInfo>> weeks { get; set; }
        public DateTime startDate { get; set; }
        public List<string> weekEnds { get; set; }
    }
    public class WeekInfo
    {
        public List<ScorecardPaymentInfo> scorecardPaymentInfoList { get; set; }
        public DateTime weekEnd { get; set; }
    }

    public class ScorecardPaymentInfo
    {
        public PaymentInfo qaPaymentInfo { get; set; }
        public PaymentInfo calibratorPaymentInfo { get; set; }
        public ScorecardInfo scorecard { get; set; }
        public DateTime weekEnd { get; set; }
    }

    public class PaymentInfo
    {
        public string payType { get; set; }
        public string qaPay { get; set; }
        public int? numberCalls { get; set; }

        public DateTime periodEnding { get; set; }                      //db
        public float totalCallTime { get; set; }                        //db
        public float totalReviewTime { get; set; }                      //db
        public float totalBadCallTime { get; set; }                     //db
        public float totalBadCallReviewTime { get; set; }               //db
        public float baseRate { get; set; }                             //db
        public float adjustedRate { get; set; }                         //calc
        public float totalPay { get; set; }                             //calc
        public float callSpeed { get; set; }                            //calc
        public float disputeCost { get; set; }                          //db
        public int disputeCount { get; set; }                           //db
        public float score { get; set; }                                //db
        public float percentChange { get; set; }                        //db
        public float paymentRate { get; set; }
        public float calibrationCount { get; set; }
        public int complitedNotification { get;  set; }
        public float complitedСost { get { return 0.4f; } set { } }

        public DateTime startDate { get; set; }
    }
}
