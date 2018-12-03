using System.Collections.Generic;

namespace DAL.Models
{
    public class AllUserSettings
    {
        public UserObject session { get; set; }
        public List<AvailableTableColumns> columns { get; set; }
        public List<SavedUserSettings> settings { get; set; }

    }
}