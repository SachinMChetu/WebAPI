using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.CCInternalAPI;
using WebApi.Models.CDService;

namespace WebApi.Models.CallCriteriaAPI
{
    public class AllCallRecord
    {
        public string client_logo;
        public string F_ID;
        public string review_ID;
        public string Comments;
        public string autofail;
        public string reviewer;
        public string appname;
        public string total_score;
        public string total_score_with_fails;
        public string call_length;
        public string has_cardinal;
        public string fs_audio;
        public string week_ending_date;
        public string num_missed;
        public string missed_list;
        public string call_made_date;
        public string AGENT;
        public string ANI;
        public string DNIS;
        public string TIMESTAMP;
        public string TALK_TIME;
        public string CALL_TIME;
        public string CALL_TYPE;
        public string leadid;
        public string AGENT_GROUP;
        public string Email;
        public string City;
        public string State;
        public string Zip;
        public string Datacapturekey;
        public string Datacapture;
        public string Status;
        public string Program;
        public string X_ID;
        public string Datacapture_Status;
        public string num_of_schools;
        public string MAX_REVIEWS;
        public string review_started;
        public string Number_of_Schools;
        public string EducationLevel;
        public string HighSchoolGradYear;
        public string DegreeStartTimeframe;
        public string Expr3;
        public string First_Name;
        public string Last_Name;
        public string address;
        public string phone;
        public string call_date;
        public string audio_link;
        public string profile_id;
        public string audio_user;
        public string audio_password;
        public string LIST_NAME;
        public string review_date;
        public string CAMPAIGN;
        public string DISPOSITION;
        public string bad_call;
        public string to_upload;
        public string SESSION_ID;
        public string agent_deviation;
        public string pass_fail;
        public string scorecard;
        public string uploaded;
        public string formatted_comments;
        public string formatted_missed;
        public string fileUrl;
        public string statusMessage;
        public string mediaId;
        public string requestStatus;
        public string fileStatus;
        public string response;
        public string review_time;
        public string wasEdited;
        public string website;
        public string pending_id;
        public string bad_call_reason;
        public string date_added;
        public string calib_score;
        public string edited_score;
        public string compliance_sheet;
        public List<ScorecardData> ScorecardData;
        public List<ClerkedData> ClerkedDataItems;
        public UserObject UserInfo;
        public List<CommentData> CallComments;

        public List<SchoolItem> SchoolData;
        public List<OtherData> OtherData;

        public bool editable;

        public List<DisputeData> Disputes;
        public List<string> dispute_buttons;

        public List<ActionButton> ActionButtons;
        public bool showSpotCheck;

        public string scorecard_name;

        public List<string> audio_merge;

        public List<SessionViews> sessions_viewed;
    }
}