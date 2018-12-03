using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.RequestParams
{
    public class ScQAsRequiredRQ
    {
        public string appname { get; set; }
        public int scorecardId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

}