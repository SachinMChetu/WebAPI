using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TopMissedItemsExportModel
    {
        public string questionShortName { get; set; }
        public string questionSectionName { get; set; }
        public string scorecardName { get; set; }
        public int missedCalls { get; set; }
        public int totalCalls { get; set; }
        public float occurrence { get; set; }
        public float delta { get; set; }
        public string top3Agents { get; set; }
    }
}
