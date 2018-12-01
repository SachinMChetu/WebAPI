using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    class ExportGroupPerformanceModel
    {
        public string name { get; set; }
        public string scorecardName { get; set; }
        public float currentScore { get; set; }
        public int currentCalls { get; set; }
        public float previousScore { get; set; }
        public int previousCalls { get; set; }
        public string delta { get; set; }

    }
}
