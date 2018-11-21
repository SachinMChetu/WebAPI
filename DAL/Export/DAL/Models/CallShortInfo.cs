using System.Collections.Generic;

namespace DAL.Models
{
    public class CallShortInfo
    {
        public CallSystemData systemData { get; set; }
        public CallMetaData metaData { get; set; }
        public List<QuestionDetails> callMissedItems { get; set; }
        public NotificationInfo notificationInfo { get; set; }
    }

    public class CallShortInfov2
    {
        public CallSystemData systemData { get; set; }
        public CallMetaData metaData { get; set; }
        public List<QuestionDetails_v2> callMissedItems { get; set; }
        public NotificationInfo notificationInfo { get; set; }
    }
}