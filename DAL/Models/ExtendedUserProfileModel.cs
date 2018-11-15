using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{

    public class ExtendedUserProfileModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public int hoursPerWeek { get; set; }
        public int daysPerWeek { get; set; }
        public int prefStartHour { get; set; }
    }

}
