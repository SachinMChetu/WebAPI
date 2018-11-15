using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Visitor
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string ip { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string timezone { get; set; }
    }
}