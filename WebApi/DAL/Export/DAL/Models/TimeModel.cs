using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TimeModel
    {
        public int id { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }

    }

}
