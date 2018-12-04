using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class XCCNewReportModel
    {

        public string appname { get; set; }
        public string SessionId { get; set; }
        public string Agent { get; set; }
        public string DisPosition { get; set; }
        public string Campaign { get; set; }
        public string Ani { get; set; }
        public string Dnis { get; set; }
        public string TimeStamp { get; set; }
        public string TalkTime { get; set; }
        public string CallTime { get; set; }
        public string HandleTime { get; set; }
        public string CallType { get; set; }
        public string ListName { get; set; }
        public string Leadid { get; set; }
        public string AgentGroup { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime call_date { get; set; }
        public string Citizenship { get; set; }
        public string Military { get; set; }
        public string AGENT_NAME { get; set; }
        public string website { get; set; }
        public int? scorecard { get; set; }
        public string Zip { get; set; }
        public float Datacapturekey { get; set; }
        public float Datacapture { get; set; }
        public string Status { get; set; }
        public string Program { get; set; }
        public string Datacapture_Status { get; set; }
        public string num_of_schools { get; set; }
        public string EducationLevel { get; set; }
        public string HighSchoolGradYear { get; set; }
        public string DegreeStartTimeframe { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string audio_link { get; set; }
        public string profile_id { get; set; }
        public int? sort_order { get; set; }
    }
}