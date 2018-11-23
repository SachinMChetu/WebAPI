using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// AgregatedStatistics
    /// </summary>
    public class AgregatedStatistics
    {
        public string totalCalls;
        public string totalFails;
        public string displayMinutes;
        public string totalAgents;
        public string badCalls;
        public string avgStat;
    }
}