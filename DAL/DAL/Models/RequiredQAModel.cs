using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class RequiredQAModel
    {
        public int id { get; set; }
        public string appName { get; set; }
        public int scorecardId { get; set; }
        public DateTime selectedDate { get; set; }
        public int hourId { get; set; }
    }
}