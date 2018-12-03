using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class vwFOrm_Result
    {
        public string comment { get; set; }
        public int comment_points { get; set; }
        public int? id { get; set; }
        public int? question_id { get; set; }
    }
}