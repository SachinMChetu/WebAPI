﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// CompleteScorecard
    /// </summary>
    public class CompleteScorecard
    {
        public string ScorecardName;
        public string Appname;
        public string ID;
        public string Status;
        public string Description;
        public List<Section> Sections;
        public List<ClerkedData> ClerkData;
        public string ReviewType;
    }

}