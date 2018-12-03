using System.Collections.Generic;

namespace DAL.Models.Guidelines
{
    public class GuidelinesQHistoryInfo
    {       

        public int id { get; set; }
        public string questionName { get; set; }
        public List<QChanges> changeList { get; set; }
    }
}