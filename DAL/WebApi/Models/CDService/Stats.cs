using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Stats
    {
        public string CallsReviewed;
        public string CRCallDifference;
        public string CRDirection;

        public string CallsFailed;
        public string CFDifference;
        public string CFDirection;

        public string NumMinutes;
        public string NMDifference;
        public string NMDirection;

        public string NumAgents;
        public string NADifference;
        public string NADirection;
    }
}