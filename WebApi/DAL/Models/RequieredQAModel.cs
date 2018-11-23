using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RequieredQAModel
    {
        public string appName { get; set; }
        public int? scorecardId { get; set; }
        public DateTime dayDate { get; set; }
        public List<WorkingPeriod> workingPeriods { get; set; }
    }
    public class WorkingPeriod
    {
        public int periodId { get; set; }
        public int required { get; set; }
        public int selected { get; set; }
    }
}
