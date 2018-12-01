using DAL.Models;

namespace DAL.Models
{
    public class CalibrationPageSearch
    {
        public string searchText { get; set; }
        public string appName { get; set; }
        public Pagination pagination { get; set; }
        public Period range { get; set; }
        public SortType sorting { get; set; }
        public CFilters filters { get; set; }

    }
  public class CFilters {
        public string appName { get; set; }
        public int  scorecardId { get; set; }
    }
}