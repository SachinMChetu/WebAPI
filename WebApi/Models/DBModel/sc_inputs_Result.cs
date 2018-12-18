using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class sc_inputs_Result
    {
        public int id { get; set; }
        public Nullable<int> scorecard { get; set; }
        public string value { get; set; }
        public string notes { get; set; }
        public string value_type { get; set; }
        public Nullable<int> value_order { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public string added_by { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<bool> required_value { get; set; }
        public Nullable<bool> dash_visible { get; set; }
    }
}