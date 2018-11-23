using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public  class School_X_Data_Result
    {
        public int id { get; set; }
        public string School { get; set; }
        public string AOI1 { get; set; }
        public string AOI2 { get; set; }
        public string L1_SubjectName { get; set; }
        public string L2_SubjectName { get; set; }
        public string Modality { get; set; }
        public Nullable<int> xcc_id { get; set; }
        public string College { get; set; }
        public string DegreeOfInterest { get; set; }
        public string origin { get; set; }
        public string tcpa { get; set; }
    }
}