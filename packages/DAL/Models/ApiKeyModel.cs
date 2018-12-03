using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApiKeyModel
    {
        public int id { get; set; }
        public string apiKey { get; set; }
        public string appname { get; set; }
        public bool active { get; set; }
        public DateTime dateAdded { get; set; }
    }
}
