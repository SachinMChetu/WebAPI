using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
  public class BaseRequestModel
    {
        public double ClientTimeOffset { get; set; }
        public string Password { get; set; }
        public string UserID { get; set; }
    }
}
