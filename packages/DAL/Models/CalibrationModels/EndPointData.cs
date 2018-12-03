using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class EndPointData
    {
            public List<int> ids { get; set; }
            public string side { get; set; } //"external"//"internal"
    }
}