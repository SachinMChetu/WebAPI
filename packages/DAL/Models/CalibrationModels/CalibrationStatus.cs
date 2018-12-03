using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CalibrationStatus
    {
        public int reviewed { get; set; }
        public int completed { get; set; }
        public List<string> assignedTo { get; set; }
        public List<string> whoProcessed { get; set; }
    }
}