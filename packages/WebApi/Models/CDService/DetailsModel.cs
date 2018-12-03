using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class DetailsModel
    {
      public string start_date { get; set; }
       public string end_date { get; set; }
        public string hdnAgentFilter { get; set; }
        //public string pagenum { get; set; }
        //public string pagerows { get; set; }
        //public string Sort_statement { get; set; }
        //public string rowstart { get; set; }
        //public string rowend { get; set; }
        public string filter_array { get; set; }
    }
}