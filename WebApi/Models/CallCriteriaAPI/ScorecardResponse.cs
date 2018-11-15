using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class ScorecardResponse
    {


        public string question { get; set; }
        public string result { get; set; }
        public string comments { get; set; }
        public string position { get; set; }
        public int QID { get; set; }
        public bool RightAnswer { get; set; }
        public int QAPoints { get; set; }

        public string QType { get; set; }
        public string ViewLink { get; set; }

        public List<string> QComments { get; set; }
        public List<CheckItems> QTemplate  { get; set; }

        public bool comments_allowed { get; set; }

        public int q_id { get; set; }
        public int? q_order { get; set; }
    }
}