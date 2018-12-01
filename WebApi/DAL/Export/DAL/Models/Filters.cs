using System.Collections.Generic;

namespace DAL.Models
{

    public class CallDetailsFilter
    {
        public Filters filters { get; set; }
        public Period range { get; set; }
        public Period comparison { get; set; }
        public Pagination paging { get; set; }
        public SortType sorting { get; set; }
        public Search search { get; set; }
    }
    public class CallDetailsExportFilter
    {
        public Filters filters { get; set; }
        public Period range { get; set; }
        public Period comparison { get; set; }
        public Pagination paging { get; set; }
        public SortType sorting { get; set; }
        public List<string> columns { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string text { get; set; }
        public List<string> columns { get; set; }
    }

    public class SortType
    {
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
    }

    public class Filter
    {
        public Filters filters { get; set; }
        public Period range { get; set; }

    }
    public class AverageReportsFilters
    {
        public ReportsF filters { get; set; }
        public Period range { get; set; }
        public Pagination paging { get; set; }
    }
    public class FilterReports
    {
        public ReportsF filters { get; set; }
        public Period range { get; set; }
    }

    public class ConversionChartFilter
    {
        public Filters filters { get; set; }
        public Period range { get; set; }
        public Pagination paging { get; set; }
        public string chartType { get; set; }
        public SortType sorting { get; set; }
    }

    public class AverageFilter
    {
        public Filters filters { get; set; }
        public Period range { get; set; }
        public Period comparison { get; set; }        
    }

    public class Period
    {
        public string end { get; set; }
        public string start { get; set; }
    }
    public class FilterDates
    {
        public string range { get; set; }
        public string rangeCustomStart { get; set; }
        public string rangeCustomEnd { get; set; }
        public string comparison { get; set; }
        public string comparisonCustomStart { get; set; }
        public string comparisonCustomEnd { get; set; }
    }
    public class Filters
    {
        public List<string> scorecards { get; set; }
        public List<string> groups { get; set; }
        public List<string> agents { get; set; }
        public List<string> campaigns { get; set; }
        public List<string> QAs { get; set; }
        public List<string> missedItems { get; set; }
        public List<string> teamLeads { get; set; }
        public List<int> answerIds { get; set; }
		public List<int> commentIds { get; set; }
        public List<int> compositeAnswerIds { get; set; }
        public List<int> compositeCommentIds { get; set; }
        public bool failedOnly { get; set; }
		public bool badCallsOnly { get; set; }
		public bool passedOnly { get; set; }
        public bool filterByReviewDate { get; set; }
        public string missedBy { get; set; }
        public string reviewType { get; set; }
        public bool pendingOnly { get; set; }
        public List<string> conversionFilters { get; set; }
        public bool isConversion { get; set; } = false;
    }

    public class ReportsF
    {
        public List<string> scorecards { get; set; }
        public List<string> groups { get; set; }
        public List<string> campaigns { get; set; }
        public bool filterByReviewDate { get; set; }
    }
    public class Pagination
    {
        public int pagenum { get; set; }
        public int pagerows { get; set; }
    }
    public class CalibrationPendingFilters
    {
       public CalibrationPendingF filters { get; set; }
       public Period range { get; set; }
     
    }
    public class CalibrationPendingF
    {
        public List<string> scorecards { get; set; }
        public List<string> groups { get; set; }
        public List<string> agents { get; set; }
        public List<string> campaigns { get; set; }
        public List<string> QAs { get; set; }
        public List<string> teamLeads { get; set; }
    }

}