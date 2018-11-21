namespace DAL.Models
{
    public class CampaignPerformance
    {
        public Campaign campaignInfo { get; set; }
        public string scorecardName { get; set; }
        public PeriodPerformance currentPeriod { get; set; }
        public PeriodPerformance previousPeriod { get; set; }
    }
    public class Campaign
    {
        public string name { get; set; }
        public string id { get; set; }
    }
}