using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TimeShift
    {
        public int shiftId { get; set; }
        public string shiftName { get; set; }//???

        public List<int> hourID { get; set; } 

    }
}
