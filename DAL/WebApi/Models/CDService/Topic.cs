using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Topic
    {
        public double score;
        public List<Keyword> keywords;
        public List<string> speakers;
        public string name;
        public List<object> similarCategories;
        public List<object> subcategories;
        public string type;
    }
}