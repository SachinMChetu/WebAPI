﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Thumbs
    {
        public int width { get; set; }
        public string path { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }
}