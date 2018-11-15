using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Layers
{
    public class ListenLayer
    {
        public dynamic GetNextCall()
        {
            var userName = HttpContext.Current.GetUserName();
            var session = HttpContext.Current.Session;
            return new NotImplementedException();
        }
    }
}
