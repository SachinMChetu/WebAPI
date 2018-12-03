using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class Section
    {
        public string section;
        public string description;
        public string order;
        public List<Question> questions;
    }
}