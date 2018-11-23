using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExportWebSiteStatisticModel
    {
        public int total { get; set; }
        public int compliant { get; set; }
        public int bad { get; set; }
        public int nonCompliant { get; set; }
    }
}
