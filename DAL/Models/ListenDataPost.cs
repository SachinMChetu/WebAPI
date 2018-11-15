using System.Collections.Generic;

namespace DAL.Models
{
    public class ListenDataPost
    {
        public string session_id{get;set;}
        public string review_ID{get;set;}
        public string Comments{get;set;}
        public string appname{get;set;}
        public string whisperID{get;set;}
        public string QAwhisper{get;set;}
        public string qa_start{get;set;}
        public string qa_last_action{get;set;}
        public string call_length{get;set;}
        public bool copy_to_cali{get;set;}
        public string active_time{get;set;}
    }
    public class FormQScores
    {
        public string q_position{get;set;}
        public string question_id{get;set;}
        public string question_answered{get;set;}
        public string click_text{get;set;}
        public string other_answer_text{get;set;}
        public string view_link{get;set;}
    }

    public class FormQResponses
    {
        public string question_id{get;set;}
        public string answer_id{get;set;}
        public string other_answer_text{get;set;}
    }

    public class FormQScoresOptions
    {

        public string option_pos{get;set;}
        public string option_value{get;set;}
        public string question_id{get;set;}
        public string orig_id{get;set;}
    }
    public class SystemComments
    {
        public string comment{get;set;}
        public string comment_pos{get;set;}
        public string comment_header{get;set;}
    }
    public struct ClerkedData
    {
       public string value{get;set;}
       public string position{get;set;}
       public string data{get;set;}
       public string ID{get;set;}
        public bool required{get;set;}
    }
    public class ListenDataRequest
    {
        public ListenDataPost LD{get;set;}
        public List<FormQScores> FQS{get;set;}
        public List<FormQResponses> FQR{get;set;}
        public List<FormQScoresOptions> FQSO{get;set;}
        public List<SystemComments> SC{get;set;}
        public List<ClerkedData> CD{get;set;}

        public bool is_practice{get;set;}
    }
}