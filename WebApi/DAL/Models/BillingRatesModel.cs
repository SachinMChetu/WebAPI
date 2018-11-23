using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BillingRatesModel
    {
        public int id { get; set; }
        public string appname { get; set; }
        public int start_minutes { get; set; }
        public int end_minutes { get; set; }
        public float rate { get; set; }
        public string bill_type { get; set; }
        public int scorecard_only { get; set; }
    }
}
