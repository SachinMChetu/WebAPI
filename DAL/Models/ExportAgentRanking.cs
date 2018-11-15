using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class ExportAgentRanking
    {
        public string name { get; set; }
        public DateTime? startDate { get; set; }
        public decimal? score { get; set; }
        public string groupName { get; set; }
        public string delta { get; set; }
        public int totalCalls { get; set; }
        public string top3Agents { get; set; }

    }
    public class ExportAgentRankingTopMissedPoints
    {
        public string name { get; set; }
        public string questionName { get; set; }
        public int callCount { get; set; }
        public int missedCalls { get; set; }
        public string missedPercent { get; set; }
    }
}