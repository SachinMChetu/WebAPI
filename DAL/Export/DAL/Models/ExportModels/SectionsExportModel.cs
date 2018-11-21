using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ExportModels
{
    public class SectionsExportModel
    {
        public string scorecardName { get; set; }
        public string sectionName { get; set; }
        public string questionName { get; set; }
        public string isLinked { get; set; }
        public int totalRight { get; set; }
        public int totalWrong { get; set; }
        public int total { get; set; }
        public double rightScore { get; set; }
        public double wrongScore { get; set; }
    }
}
