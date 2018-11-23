using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{

    public class PageFiltersData
    {
        public List<FilterValue> scorecards;
        public List<FilterValue> campaigns;
        public List<FilterValue> groups;
        public List<FilterValue> agents;
        public List<FilterValue> QAs;
        public List<FilterValue> missedItems;
        public class FilterValue
        {
            public string key;
            public string value;
        }
    }
}