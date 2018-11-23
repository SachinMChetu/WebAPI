using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class CallsLeft
    {
        public int pending { get; set; }
        public int pendingReady { get; set; }
        public int pendingNotReady { get; set; }
        public int reviewed { get; set; }
        public int badCalls { get; set; }
        public DateTime? callDate { get; set; }
        public ScorecardInfo scorecard { get; set; }
        public List<PendingCall> pendingCalls { get; set; }
    }


    public class PendingCall
    {
        public DateTime receiveDate { get; set; }
        public int scorecardId { get; set; }
        public DateTime callDate { get; set; }
    }
  
}