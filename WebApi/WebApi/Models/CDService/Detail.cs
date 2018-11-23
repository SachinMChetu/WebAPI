using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Detail
    {
        public string url { get; set; }
        public int size { get; set; }
        public string content_type { get; set; }
        public string content_encoding { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Thumbs thumbs { get; set; }
    }
}