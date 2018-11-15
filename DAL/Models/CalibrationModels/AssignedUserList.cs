using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class AssignedUserList
    {
        public int formId { get; set; }
        public string processed { get; set; }
        public string assigned { get; set; }
    }
}