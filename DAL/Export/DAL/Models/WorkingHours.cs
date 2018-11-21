using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class WorkingHours
    {
        public DateTime date{get;set;}
        public List<DaylyWorkinHour> daylyWorkinHours { get; set; }
    }

    public class DaylyWorkinHour{
        public int id { get; set; }
        public string start{ get; set; }
        public string end { get; set; }

    }
    
}