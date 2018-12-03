using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// CallRecord
    /// </summary>
    public class CallRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public string F_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string review_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string autofail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string reviewer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string appname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string total_score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string total_score_with_fails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string call_length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string has_cardinal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string fs_audio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string week_ending_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string num_missed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string missed_list { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string call_made_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AGENT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ANI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DNIS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TIMESTAMP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TALK_TIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CALL_TIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CALL_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string leadid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AGENT_GROUP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Datacapturekey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Datacapture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string X_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Datacapture_Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string num_of_schools { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MAX_REVIEWS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string review_started { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Number_of_Schools { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EducationLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HighSchoolGradYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DegreeStartTimeframe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Expr3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string First_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Last_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string call_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string audio_link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string profile_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string audio_user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string audio_password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LIST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string review_date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CAMPAIGN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DISPOSITION { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string bad_call { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string to_upload { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SESSION_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string agent_deviation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string pass_fail {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public string scorecard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string uploaded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string formatted_comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string formatted_missed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string fileUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string statusMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string mediaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string requestStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string fileStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string response { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string review_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string wasEdited { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string pending_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string bad_call_reason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string date_added { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string calib_score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string edited_score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string compliance_sheet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string scorecard_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ScorecardResponse> ScorecardResponses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ClerkedData> ClerkedDataItems { get; set; }
        

        //public string F_ID;
        //public string review_ID;
        //public string Comments;
        //public string autofail;
        //public string reviewer;
        //public string appname;
        //public string total_score;
        //public string total_score_with_fails;
        //public string call_length;
        //public string has_cardinal;
        //public string fs_audio;
        //public string week_ending_date;
        //public string num_missed;
        //public string missed_list;
        //public string call_made_date;
        //public string AGENT;
        //public string ANI;
        //public string DNIS;
        //public string TIMESTAMP;
        //public string TALK_TIME;
        //public string CALL_TIME;
        //public string CALL_TYPE;
        //public string leadid;
        //public string AGENT_GROUP;
        //public string Email;
        //public string City;
        //public string State;
        //public string Zip;
        //public string Datacapturekey;
        //public string Datacapture;
        //public string Status;
        //public string Program;
        //public string X_ID;
        //public string Datacapture_Status;
        //public string num_of_schools;
        //public string MAX_REVIEWS;
        //public string review_started;
        //public string Number_of_Schools;
        //public string EducationLevel;
        //public string HighSchoolGradYear;
        //public string DegreeStartTimeframe;
        //public string Expr3;
        //public string First_Name;
        //public string Last_Name;
        //public string address;
        //public string phone;
        //public string call_date;
        //public string audio_link;
        //public string profile_id;
        //public string audio_user;
        //public string audio_password;
        //public string LIST_NAME;
        //public string review_date;
        //public string CAMPAIGN;
        //public string DISPOSITION;
        //public string bad_call;
        //public string to_upload;
        //public string SESSION_ID;
        //public string agent_deviation;
        //public string pass_fail;
        //public string scorecard;
        //public string uploaded;
        //public string formatted_comments;
        //public string formatted_missed;
        //public string fileUrl;
        //public string statusMessage;
        //public string mediaId;
        //public string requestStatus;
        //public string fileStatus;
        //public string response;
        //public string review_time;
        //public string wasEdited;
        //public string website;
        //public string pending_id;
        //public string bad_call_reason;
        //public string date_added;
        //public string calib_score;
        //public string edited_score;
        //public string compliance_sheet;
        //public string scorecard_name;
        //public List<ScorecardResponse> ScorecardResponses;
        //public List<ClerkedData> ClerkedDataItems;
    }
}