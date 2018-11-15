using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
   public class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public bool RememberMe { get; set; }
    }
}
