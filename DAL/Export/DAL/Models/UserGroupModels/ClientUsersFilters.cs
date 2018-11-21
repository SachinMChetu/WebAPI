using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class ClientUsersFilters
    {
        public Pagination pagination { get; set; }
        public bool inactive{ get; set; }
        public string search{ get; set; }
    }
}
