using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ScorecardChangesModel
    {
        public int id { get; set; }
        public int scorecard { get; set; }
        public string change { get; set; }
        public string changed_by { get; set; }
        public DateTime changed_date { get; set; }
    }
}
