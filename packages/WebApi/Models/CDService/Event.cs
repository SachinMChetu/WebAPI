using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Event
    {
        public string author_name { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public int timestamp { get; set; }
        public string agent_id { get; set; }
        public string user_type { get; set; }
        public string type { get; set; }
        public bool welcome_message { get; set; }
        public List<string> files { get; set; }
        public List<Detail> details { get; set; }
    }

}