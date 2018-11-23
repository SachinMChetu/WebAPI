using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DropDownItemModel
    {
        public int id { get; set; }
        public int? questionId { get; set; }
        public string value { get; set; }
        public DateTime? dateAdded { get; set; }
        public bool? active { get; set; }
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int? itemOrder { get; set; }
    }
}
