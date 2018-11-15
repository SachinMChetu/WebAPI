using DAL.Models.ListenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// AddRecordData
    /// </summary>
    public class AddRecordData
    {
        public string SESSION_ID;
        public string AGENT;
        public string AGENT_NAME;
        public string DISPOSITION;
        public string CAMPAIGN;
        public string ANI;
        public string DNIS;
        public string TIMESTAMP;
        public string TALK_TIME;
        public string CALL_TIME;
        public string HANDLE_TIME;
        public string CALL_TYPE;
        public string LIST_NAME;
        public string leadid;
        public string AGENT_GROUP;
        public string HOLD_TIME;
        public string Email;
        public string City;
        public string State;
        public string Zip;
        public string Datacapturekey;
        public string Datacapture;
        public string Status;
        public string Program;
        public string Datacapture_Status;
        public string num_of_schools;
        public string EducationLevel;
        public string HighSchoolGradYear;
        public string DegreeStartTimeframe;
        public string appname;
        public string First_Name;
        public string Last_Name;
        public string address;
        public string phone;
        public string audio_link;
        public string sort_order;
        public string scorecard;
        public string call_date;
        public string Citizenship;
        public string Military;
        public string profile_id;
        public string website;
        public SchoolItem[] Schools;
        public audioFile[] audios;
        public OtherData[] OtherDataItems;
        public string Repost;
    }
}