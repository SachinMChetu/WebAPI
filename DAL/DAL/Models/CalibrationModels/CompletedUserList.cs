using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CompletedUserList
    {
        public int formId { get; set; }
        public string completedBy { get; set; }
        public int reviewTime { get; set; }
    }
}