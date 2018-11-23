using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
  
    public class MgmtNotes
    {
        public string note { get; set; }
        public DateTime dateCreated { get; set; }
        public string whoCreated { get; set; }
        public int updateId { get; set; }
        public string status { get; set; }
    }
}
