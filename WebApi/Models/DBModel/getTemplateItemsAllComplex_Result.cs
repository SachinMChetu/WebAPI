using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getTemplateItemsAllComplex_Result
    {
        public int id { get; set; }
        public int? question_id { get; set; }
        public string option_value { get; set; }
        public string value { get; set; }
    }
}