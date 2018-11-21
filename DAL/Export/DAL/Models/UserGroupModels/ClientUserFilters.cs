using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class ClientUserFilters
    {
        public Pagination pagination { get; set; }
        public Filters filters { get; set; }
        public string searchText { get; set; }
        public bool hideInactive { get; set; }
        public SortType sorting { get; set; }
    }
    public class UserListFilters
    {
        public Pagination pagination { get; set; }
        public Filters filterItems { get; set; }
    }

    public class Filters
    {
       public List<string> filterGroups { get; set; }
       public List<string> filterScorecards { get; set; }
    }
}
