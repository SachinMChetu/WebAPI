using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class DetailCountvwForm
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string hdnAgentFilter { get; set; }
        public string filter_array { get; set; }
    }
}