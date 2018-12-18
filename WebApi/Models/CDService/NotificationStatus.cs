using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class NotificationStatus
    {
        public string Reviewer { get; set; }
        public int? TN { get; set; }
        public int? AB { get; set; }
        public int? AA { get; set; }
        public int? AD { get; set; }
        public int? SB { get; set; }
        public int? SA { get; set; }
        public int? SD { get; set; }
        public int? QB { get; set; }
        public int? QA { get; set; }
        public int? QD { get; set; }
        public int? LB { get; set; }
        public int? LA { get; set; }
        public int? LD { get; set; }
        public int? TD { get; set; }

    }
}