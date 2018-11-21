using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AvailableQAsModel
    {
        public string userName { get; set; }
        public string appName { get; set; }
        public string scorecardName { get; set; }
        public int scorecardId { get; set; }
        public int assignedHours { get; set; }
    }
    public class UpdateAvailableQAsModel
    {
        public string userName { get; set; }
        public DateTime selectedDate { get; set; }
        public int hourId { get; set; }
    }
}
