using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class AvailableTableColumns
    {
        public string id { get; set; }
        public string value { get; set; }
        public bool @checked { get; set; }
        public bool sortableValue { get; set; }
        public bool searchable { get; set; }
        public string type { get; set; }
    }
}