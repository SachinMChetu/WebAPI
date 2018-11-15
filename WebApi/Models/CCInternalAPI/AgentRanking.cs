using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{

    public class AgentRanking
    {
        public List<AgentRankingInfo> info;
        public string avg_score;
        public string end_date;
        public string start_date;
        public string agentName;
        public string agent;
        public string pass_percent;
        public string Average_score;
        public string ni_scorecard;
    }
}