using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class GuidelineHistoryModel
    {
        public int id { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public int? questionId { get; set; }
        public string fromText { get; set; }
        public string toText { get; set; }
        public string fromAnswer { get; set; }
        public string toAnswer { get; set; }
    }
}
