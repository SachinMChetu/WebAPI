namespace DAL.Models
{
    public class AgregatedStatistics
    {
        public int totalCalls { get; set; }
        public int totalFailedCalls { get; set; }
        public int totalBadCalls { get; set; }
        public decimal totalCallsSeconds { get; set; }
        public int totalAgents { get; set; }
        public decimal avgAgentScore { get; set; }
        public int? totalPending { get; set; }
    }

    public class GetAgregatedStatisticsResponseData
    {
        public AgregatedStatistics rangeStats { get; set; }
        public AgregatedStatistics comparisonStats { get; set; }
    }
}