using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Models
{
    public class CoachingQueue
    {
        public string notificationID { get; set; }
        public string agent { get; set; }
        public string total_score { get; set; }
        public string call_date { get; set; }
        public string dateadded { get; set; }
        public string notificationStep { get; set; }
        public string form_id { get; set; }
        public string form_id_plus { get; set; }
        public string first_error { get; set; }
        public string ownedNotification { get; set; }
        public string allComments { get; set; }
    }
    public class CoachingQueueResponceData
    {
        public List<CoachingQueueCallDetails> calls { get; set; }
        public int totalNotification { get; set; }
    }
    public class CoachingQueueCallDetails
    {
        public CallSystemData systemData { get; set; }
        public string metaDataAgentName { get; set; }
        public DateTime? metaDataCallDate { get; set; }
        public dynamic customData { get; set; }
        public CallMetaData metaData { get; set; }
    }
    public class CallSystemData
    {
        public bool externalAdded { get; set; }
        public bool inernalAdded { get; set; }

        public DateTime? receivedDate { get; set; }
         
        public int? callId { get; set; }
        public string callType { get; set; }// "call" or "website"
        public string callReviewStatus { get; set; } // "pending" or "reviewed" or "calibrated" or "edited" or "bad" or "disqualified"
        public string callAudioUrl { get; set; }
        public double? callAudioLength { get; set; }// seconds
        public string websiteUrl { get; set; }
        public int? scorecardId { get; set; }
        public string scorecardName { get; set; }
        public double? scorecardFailScore { get; set; } // for "reviewed" or "calibrated" or "edited"
        public DateTime? reviewDate { get; set; }
        public string reviewerUserRole { get; set; }
        public string reviewerName { get; set; } // QA
        public string calibratorId { get; set; }
        public string calibratorName { get; set; }
        public double? agentScore { get; set; }
        public bool callFailed { get; set; }
        public int? missedItemsCount { get; set; }
        public bool reviewCommentsPresent { get; set; }
        public bool notificationCommentsPresent { get; set; }
        public int notificationId { get; set; }
        public string notificationStatus { get; set; } // "none" or "notification" or "dispute"
        public bool isNotificationOwner { get; set; }    // for "bad" or "disqualified"
        public string badCallReason { get; set; }
        public List<CallMissedItem> missedItems;
        public int? xId { get; set; }
        public string assignedToRole { get; set; }
        public string missedItemsList { get;  set; }
        public string missedItemsCommentList { get; set; }
        public ScorecardInfo scorecardInfo;
        public bool wasEdited { get; set; }
        public int? scoreChanged { get; set; }
        public bool audioStatus { get; set; }

        public CallSystemData()
        {
            missedItems = new List<CallMissedItem>();
            scorecardInfo = new ScorecardInfo();
            missedItems = new List<CallMissedItem>();
        }

        public static CallSystemData Create(IDataRecord reader)
        {
            return new CallSystemData
            {
                callId = reader.GetValueOrDefault<int?>("callId"),
                callType = reader.Get<string>("calltype"),
                callReviewStatus = reader.Get<string>("callReviewStatus"),
                  callAudioUrl = null,
                callAudioLength = reader.GetValueOrDefault<double?>("callAudioLength"),
                websiteUrl = reader.Get<string>("websiteUrl"),
                scorecardId = reader.GetValueOrDefault<int?>("scorecardId"),
                scorecardName = reader.Get<string>("scorecardName"),
                scorecardFailScore = reader.GetValueOrDefault<double?>("scorecardFailScore"),
                receivedDate = reader.GetValueOrDefault<DateTime?>("receivedDate"),
                reviewDate = reader.GetValueOrDefault<DateTime?>("reviewDate"),
                reviewerUserRole = reader.Get<string>("reviewerUserRole"),
                reviewerName = reader.Get<string>("reviewerName"),
                calibratorId = reader.Get<string>("calibratorId"),
                calibratorName = reader.Get<string>("calibratorName"),
                missedItemsCount = reader.GetValueOrDefault<int?>("missedItemsCount"),
                agentScore = reader.GetValueOrDefault<double?>("agentScore") == null ? (double?)null : Math.Round((double)reader.GetValueOrDefault<double?>("agentScore")),
                callFailed = reader.Get<bool>("callFailed"),//((reader.GetValue(reader.GetOrdinal("callFailed")).ToString() != "Pass")),
                reviewCommentsPresent = (reader.Get<string>("reviewCommentsPresent")!="0"),//((reader.GetValue(reader.GetOrdinal("reviewCommentsPresent")).ToString() != "0")),
                notificationCommentsPresent = (reader.Get<string>("notificationCommentsPresent") != "0"),//((reader.GetValue(reader.GetOrdinal("notificationCommentsPresent")).ToString() != "0")),
                notificationStatus = reader.Get<string>("notificationStatus") == null ? "non" : reader.Get<string>("notificationStatus"),
                isNotificationOwner = reader.Get<bool>("OwnedNotification"),
                badCallReason = reader.Get<string>("badCallReason"),
                scoreChanged = reader.GetValueOrDefault<int?>("scoreChanged"),
                audioStatus = reader.Get<bool>("audioReady"),
                missedItemsList = reader.Get<string>("missedItemsList"),
                missedItemsCommentList = reader.Get<string>("missedItemsCommentList"),
                wasEdited = reader.Get<bool>("wasEdit")
            };
        }
    }

}