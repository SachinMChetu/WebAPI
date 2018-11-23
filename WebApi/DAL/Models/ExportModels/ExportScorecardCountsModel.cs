using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ExportModels
{
    public class ExportScorecardCountsModel
    {
        
        public string lastLoaded { get; set; }//6
        public string lastReviewed { get; set; }//7
        public int minutesCompleted { get; set; }//3
        public int missingAudioCalls { get; set; }//5
        public int mtdCallsCompleted { get; set; }//2
        public string oldestPending { get; set; }//8
        public int pendingCalls { get; set; }//4
       
        public string scorecardName { get; set; }//1
    }
}
