using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Models
{
   public class FileUploaderSettinsModel
    {
        public string fileName { get; set; }
        public byte[] fileData { get; set; }
        public string appname { get; set; }
        //public bool isSmall { get; set; }
    }
}
