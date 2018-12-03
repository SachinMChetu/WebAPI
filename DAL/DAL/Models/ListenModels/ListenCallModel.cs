using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ListenModels
{
    public class ListenCallModel
    {
        public ListenData listenData { get; set; }
    }

    public class ListenData
    {
        public string clientLogo { get; set; }
        public string appname { get; set; }
        public long? ani { get; set; }
        public long? dnis { get; set; }
        public DateTime? timeStamp { get; set; }
        public DateTime? callTime { get; set; }
        public string callType { get; set; }
        public string leadId { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int? zip { get; set; }
        public string status { get; set; }
        public string program { get; set; }
        public int? xID { get; set; }
        public string educationLevel { get; set; }
        public int? highSchoolGradYear { get; set; }
        public string degreeStartTimeframe { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public DateTime? callDate { get; set; }
        public string audioLink { get; set; }
        public int profileId { get; set; }
        public string listName { get; set; }
        public string campaign { get; set; }
        public string disposition { get; set; }
        public string sessionId { get; set; }
        public string scorecard { get; set; }
        public string scorecardName { get; set; }
        public string website { get; set; }
        public string agent { get; set; }
        public bool? onHold { get; set; }
        public int? mustReview { get; set; }
        public bool? isRecal { get; set; }
        public List<string> rejectionReasons { get; set; }
        public List<string> audioMerge { get; set; }
        public List<SchoolItem> schoolData { get; set; }
        public List<OtherData> otherData { get; set; }
    }

    public class ComleteScorecard
    {
        public string scorecardName { get; set; }
        public int id { get; set; }
        public string appName { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string reviewType { get; set; }

    }

    public class SectionListen
    {
        public string section { get; set; }
        public string description { get; set; }
        public int? order { get; set; }
        public List<QuestionInfo> questions { get; set; }
    }

    public class ListenQuestion
    {
        public int id { get; set; }
        public int section { get; set; }
        public int category { get; set; }
        public int qOrder { get; set; }
        public string question { get; set; }
        public string qType { get; set; }
        public string qShortName { get; set; }
        public bool active { get; set; }
        public int heading { get; set; }
        public int order { get; set; }
        public string questionText { get; set; }
        public float qPercent { get; set; }
        public string appname { get; set; }
        public bool autoYes { get; set; }
        public bool autoNo { get; set; }
        public string agentDisplay { get; set; }
        public int defaultAnswer { get; set; }
        public int origId { get; set; }
        public float totalPoints { get; set; }
        public string template { get; set; }
        public string temlateItems { get; set; }
        public string linkedQuestion { get; set; }
        public bool emailWrong { get; set; }
        public string campaignSpecific { get; set; }
        public int scorecardId { get; set; }
        public float qaPoints { get; set; }
        public bool compliance { get; set; }
        public DateTime? dateQadded { get; set; }
        public bool nonBillable { get; set; }
        public bool commentsAllowed { get; set; }
        public int linkedAnswer { get; set; }
        public int linkedComment { get; set; }
        public bool clientVisible { get; set; }
        public bool clientQuideline_visible { get; set; }
        public int sectionlessOrder { get; set; }
        public bool linkedVisible { get; set; }
        public bool clientDashboard_visible { get; set; }
        public bool pinned { get; set; }
        public bool preProduction { get; set; }
        public bool singleComment { get; set; }
        public bool pointsPaused { get; set; }
        public DateTime? pointsPausedDate { get; set; }
        public bool fullWidth { get; set; }
        public bool wideQ { get; set; }
        public bool requireCustomComment { get; set; }
        public string sentence { get; set; }
        public string ddlType { get; set; }
        public string ddlQuery { get; set; }
        public string optionsVerb { get; set; }
        public bool leftColumnQuestion { get; set; }
        public string sectionName { get; set; }
        public string linkedQuestionName { get; set; }
        public string linkedAnswerText { get; set; }
        public string linkedCommentText { get; set; }
        public List<string> templateOptions { get; set; }

    }


    public class SchoolItem
    {
        public int id { get; set; }
        public string school { get; set; }
        public string college { get; set; }
        public string degreeOfInterest { get; set; }
        public string aoi1 { get; set; }
        public string aoi2 { get; set; }
        public string l1SubjectName { get; set; }
        public string l2SubjectName { get; set; }
        public string modality { get; set; }
        public string portal { get; set; }
        public string tcpa { get; set; } 
    }

    public class OtherData
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string school { get; set; }
        public string label { get; set; }
        public string id { get; set; }
    }



}
