using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DateAppNameModel
    {
        public string appName { get; set; }
        public DateTime startDate { get; set; }       
        public List<int> workingPeriods { get; set; }
    }
}
