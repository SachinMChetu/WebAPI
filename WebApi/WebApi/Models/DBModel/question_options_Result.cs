using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class question_options_Result
    {
        public DateTime? date_end { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_added { get; set; }
        public int? option_order { get; set; }
        public int? question_id  { get; set; }
        public string option_text { get; set; }
        public int id { get; set; }
    }
}