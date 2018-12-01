using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SectionModel
    {
        public int id { get; set; }
        public string section { get; set; }
        public int? section_order { get; set; }
        public string descrip { get; set; }
        public string appname { get; set; }
        public int? orig_id { get; set; }
        public bool? serious { get; set; }
        public int? scorecard_id { get; set; }
        public float? max_score_loss { get; set; }
    }
}
