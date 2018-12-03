namespace DAL.Models
{
    public class GroupPerformance
    {
        public GroupInfo groupInfo { get; set; }
        public string scorecardName { get; set; }
        public PeriodPerformance currentPeriod { get; set; }
        public PeriodPerformance previousPeriod { get; set; }

    }
    public class GroupInfo
    {
        public string name { get; set; }
        public dynamic id { get; set; }
    }

    public class GroupInfoV2
    {
        public string name { get; set; }
        public dynamic id { get; set; }
        public bool updateOlderData { get; set; }
    }
}