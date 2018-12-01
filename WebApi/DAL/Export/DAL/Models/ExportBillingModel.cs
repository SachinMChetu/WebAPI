using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExportBillingModel
    {
        public DateTime date { get; set; }
        public int? successfulTime { get; set; }
        public int? billableTime { get; set; }
        public double? currentBillableRate { get; set; }
        public int? badCallsTime { get; set; }
        public int? transcriptMinutes { get; set; }
        public int? chatsReviewed { get; set; }
        public int? websitesReviewed { get; set; }
    }
}
