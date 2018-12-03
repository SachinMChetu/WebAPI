using System.Collections.Generic;

namespace DAL.Models
{
    public class ForcedFilters
    {
        public List<string> campaign { get; set; }
        public List<string> group { get; set; }
        public List<string> qa { get; set; }
        public List<string> agent { get; set; }
        public List<string> teamLead { get; set; }

    }
}