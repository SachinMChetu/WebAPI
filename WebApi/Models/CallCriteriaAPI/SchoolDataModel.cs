﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class SchoolDataModel
    {
        public int pending_id { get; set; }
        public string AOI1 { get; set; }
        public string AOI2 { get; set; }
        public string College { get; set; }
        public string DegreeOfInterest { get; set; }
        public string L1_SubjectName { get; set; }
        public string L2_SubjectName { get; set; }
        public string Modality { get; set; }
        public string School { get; set; }
        public string Portal { get; set; }
        public string TCPA { get; set; }
    }
}