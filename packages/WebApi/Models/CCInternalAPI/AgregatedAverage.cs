using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    public class AgregatedAverage
    {
        public List<AvgScore> totalAvg;
        public List<AvgScore> filterAvg;
        public class AvgScore
        {
            public string avgScore;
            public string callDate;
        }
    }
}