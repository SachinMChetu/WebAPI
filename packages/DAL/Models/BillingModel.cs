using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{

    public class BillingResponseData
    {
        public double? transcriptRate { get; set; }
        public int? minimumMinutes { get; set; }
        public double? budget { get; set; }
        public List<BillableRate> cpmBillableRate { get; set; }
        public List<BillingData> months { get; set; }


    }

    public class BillableRate
    {

        public double? rate { get; set; }
        public int? minutesFrom { get; set; }
        public int? minutesTo { get; set; }
    }

    public class BillingData
    {
        public DateTime date { get; set; }
        public double? currentBillableRate { get; set; }
        public int? billableTime { get; set; }
        public int? successfulTime { get; set; }
        public int? badCallsTime { get; set; }
        public int? transcriptMinutes { get; set; }
        public int? chatsReviewed { get; set; }
        public int? websitesReviewed { get; set; }
        public double? chatsCost { get; set; }
        public double? websitesCost { get; set; }
    }
}