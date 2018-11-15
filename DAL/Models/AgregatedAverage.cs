using System;
using System.Collections.Generic;

namespace DAL.Models
{

    //remove after v2 migration
    public class AgregatedAverage
    {
        public List<AvarageDayScore> totalAvg { get; set; }
        public List<AvarageDayScore> filterAvg { get; set; }
    }



    //v2 
    public class AvgScoreResponseData
    {
        public List<AvarageDayScore> averageScores { get; set; }

    }
    public class AvarageDayScore
    {
        public int averageScore { get; set; }
        public DateTime? date { get; set; }
    }
}