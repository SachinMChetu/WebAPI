using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ScorecardGroup
    {
        public int id { get; set; }
        public int scorecard { get; set; }
        public string groupName { get; set; }
        public DateTime dateadded { get; set; }
    }
}
