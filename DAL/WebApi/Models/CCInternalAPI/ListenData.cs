using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{



    public class ListenData
    {
        public string client_logo;
        public string appname;
        public string ANI;
        public string DNIS;
        public string TIMESTAMP;
        public string CALL_TIME;
        public string CALL_TYPE;
        public string leadid;
        public string Email;
        public string City;
        public string State;
        public string Zip;
        public string Status;
        public string Program;
        public string X_ID;
        public string EducationLevel;
        public string HighSchoolGradYear;
        public string DegreeStartTimeframe;
        public string First_Name;
        public string Last_Name;
        public string address;
        public string phone;
        public string call_date;
        public string audio_link;
        public string profile_id;
        public string LIST_NAME;
        public string CAMPAIGN;
        public string DISPOSITION;
        public string SESSION_ID;
        public string scorecard;
        public string scorecard_name;
        public string website;
        public string agent;
        public bool onHold;
        public int must_review;

        public List<SchoolItem> SchoolData;
        public List<OtherData> OtherData;
        public bool isRecal;


        public List<string> rejection_reasons;

        public List<string> audio_merge;
    }


}