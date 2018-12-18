using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getRecordSCEdited_Result
    {

      
        public string changed_by { get; set; }
        public DateTime? review_date { get; set; }
        public string user_role { get; set; }
        public string username { get; set; }
        public Nullable<double> total_score { get; set; }
        public Nullable<int> f_id { get; set; }
    }
}