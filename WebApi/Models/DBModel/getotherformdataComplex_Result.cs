using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getotherformdataComplex_Result
    {
        public int id { get; set; }
        public int xcc_id { get; set; }
        public string data_key { get; set; }
        public string data_value { get; set; }
        public string data_type { get; set; }
        public string school_name { get; set; }
        public string label { get; set; }
    }
}