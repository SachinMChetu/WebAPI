using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExportCallsLeftModel
    {
        public string scorecardName { get; set; }
        public int scorecardId { get; set; }
        public DateTime? callDate { get; set; }
        public int badCalls { get; set; }
        public int reviewed { get; set; }
        public string pendingCalls { get; set; }
    }
}
