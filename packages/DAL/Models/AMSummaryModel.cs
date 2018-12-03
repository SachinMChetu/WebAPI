using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    /// <summary>
    /// AMSummaryModel
    /// </summary>
    public class AMSummaryModel
    {   public ScorecardInfo scorecard { get; set; }
        public string appname { get; set; }
        public int? mtdCallsCompleted { get; set; }
        public int? minutesCompleted { get; set; }
        public int? pendingCalls { get; set; }
        public int? missingAudioCalls { get; set; }
        public string lastLoaded { get; set; }
        public string lastReviewed { get; set; }
        public string oldestPending { get; set; }

        public AMSummaryModel()
        {
            scorecard = new ScorecardInfo();
        }
        private static AMSummaryModel CreateRecord(IDataRecord record)
        {
            var sc = new ScorecardInfo()
            {
                scorecardId = record.Get<int>("sc_id"),
                scorecardName = record.Get<string>("SCORECARD")
            };
            return new AMSummaryModel
            {
                scorecard = sc,
                appname = record.Get<string>("APPNAME"),
                mtdCallsCompleted = record.GetValueOrDefault<int?>("MTD CALLS COMPLETED"),
                minutesCompleted = record.GetValueOrDefault<int?>("MTD MINUTES PROCESSED"),
                pendingCalls = record.GetValueOrDefault<int?>("NUMBER PENDING REVIEW"),
                missingAudioCalls = record.GetValueOrDefault<int?>("NUMBER MISSING AUDIO"),
                lastLoaded = record.Get<string>("LAST LOADED DATE"),
                lastReviewed = record.Get<string>("LAST REVIEWED DATE"),
                oldestPending = record.Get<string>("OLDEST PENDING DATE"),
            };
        }

        public static List<AMSummaryModel> Create(IDataReader reader)
        {
            List<AMSummaryModel> respose = new List<AMSummaryModel>();
            while (reader.Read())
            {
                try
                {
                    respose.Add(CreateRecord(reader));
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
            return respose;
        }
    }
}