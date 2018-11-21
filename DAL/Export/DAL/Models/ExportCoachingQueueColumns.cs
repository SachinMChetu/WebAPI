using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ExportCoachingQueueColumns
    {
        public string agentName { get; set; }
        public DateTime? callDate { get; set; }
        public float? agentScore { get; set; }
        public string assignedToRole { get; set; }
        public string callType { get; set; }
        public int? callId { get; set; }
        public string callFailed { get; set; }
        public float? callAudioLength { get; set; }
        public string callAudioUrl { get; set; }
        public int? cali_id { get; set; }
        public string calibratorName { get; set; }
        public string callReviewStatus { get; set; }
        public int? missedItemsCount { get; set; }
        public int? notificationId { get; set; }
        public DateTime? reviewDate { get; set; }
        public string reviewerUserRole { get; set; }
        public string reviewerName { get; set; }
        public string reviewCommentsPresent { get; set; }
        public string notificationCommentsPresent { get; set; }
        public float? scorecardFailScore { get; set; }
        public int? scorecardId { get; set; }
        public string scorecardName { get; set; }
        public string websiteUrl { get; set; }
        public string agentGroup { get; set; }
        public string campaign { get; set; }
        public string sessionId { get; set; }
        public string profileId { get; set; }
        public string prospectFirstName { get; set; }
        public string prospectLastName { get; set; }
        public string prospectPhone { get; set; }
        public string prospectEmail { get; set; }
        public string wasEdited { get; set; }
        public string notificationStatus { get; set; }
        public string notificationStep { get; set; }
        public string isOwnedNotification { get; set; }
        public string OwnedNotification { get; set; }
        public string calibratorId { get; set; }
        public string missedItemsList { get; set; }
        public string missedItemsCommentList { get; set; }


    }
}
