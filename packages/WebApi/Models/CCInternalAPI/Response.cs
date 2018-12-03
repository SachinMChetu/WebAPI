using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.CCInternalAPI
{
   public class Response
    {        
        public string Message { get; set; }
        public object Content { get; set; }
        public int Status { get; set; }
    }
}
