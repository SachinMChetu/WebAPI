using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class ClerkedData
    {

        public string value { get; set; }
        public string position { get; set; }
        public string data { get; set; }
        public string ID { get; set; }
        public bool required { get; set; }

        public string value_data { get; set; }
        public string value_position { get; set; }
        public string value_id { get; set; }
    }
}