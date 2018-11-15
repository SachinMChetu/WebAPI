using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Latest
    {
        public List<string> formats;
        public string engine;
        public List<Word> words;
        public string type;
    }
}