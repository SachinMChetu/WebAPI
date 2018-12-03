using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Progress
    {
        public string jobId;
        public string status;
        public string start;
        public string finish;
        public object tasks;
    }
}