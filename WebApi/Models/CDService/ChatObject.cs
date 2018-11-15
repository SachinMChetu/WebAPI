using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class ChatObject
    {
        public string type { get; set; }
        public string id { get; set; }
        public List<object> tickets { get; set; }
        public string visitor_name { get; set; }
        public string visitor_id { get; set; }
        public string visitor_ip { get; set; }
        public Visitor visitor { get; set; }
        public List<Agent> agents { get; set; }
        public List<object> supervisors { get; set; }
        public string rate { get; set; }
        public int duration { get; set; }
        public string chat_start_url { get; set; }
        public List<int> group { get; set; }
        public string started { get; set; }
        public bool pending { get; set; }
        public List<object> tags { get; set; }
        public string timezone { get; set; }
        public List<Message> messages { get; set; }
        public List<PrechatSurvey> prechat_survey { get; set; }
        public List<Event> events { get; set; }
        public string engagement { get; set; }
        public int started_timestamp { get; set; }
        public int ended_timestamp { get; set; }
        public string ended { get; set; }
        public string referrer { get; set; }
        public List<PostchatSurvey> postchat_survey { get; set; }
    }
}