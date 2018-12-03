using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ReportsModels
{
    public class AgentReportModel
    {
      public List<AgentReports> agentReports { get; set; }
      public int totalCalls { get; set; }
    }
    public class AgentReports
    {
        public string agentName { get; set; }
        public ScorecardInfo scorecard { get; set; }
        public int? reviewId { get; set; }
        public string reviewer { get; set; }
        public string agentGroup { get; set; }
        public string campaign { get; set; }
        public int scorecardPassPercent { get; set; }
        public int scorecardFailScore { get; set; }
        public int callTime { get; set; }
        public int agentScore { get; set; }
    }
}
