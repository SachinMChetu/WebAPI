using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class ListenDataPost
    {
        public string session_id;
        public string review_ID;
        public string Comments;
        public string appname;
        public string whisperID;
        public string QAwhisper;
        public string qa_start;
        public string qa_last_action;
        public string call_length;
        public bool copy_to_cali;
        public string active_time;
    }
}