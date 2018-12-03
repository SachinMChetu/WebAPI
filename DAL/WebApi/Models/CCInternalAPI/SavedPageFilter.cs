using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    public class SavedPageFilter
    {
        public string startDate;
        public string endDate;
        public bool failedOnly;
        public List<string> scorecards;
        public List<string> campaigns;
        public List<string> groups;
        public List<string> agents;
        public List<string> QAs;
        public List<string> missedItems;
    }
}