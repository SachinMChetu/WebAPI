using System;
using System.Collections.Generic;

namespace DAL.Models
{//REMOVE AFTER MIGRATION TO V2

    public class AgentRanking
    {
        public List<AgentRankingInfo> info { get; set; }
        public decimal avg_score { get; set; }
        public DateTime end_date { get; set; }
        public DateTime start_date { get; set; }
        public string agentName { get; set; }
        public string agent { get; set; }
        public decimal pass_percent { get; set; }
        public int average_score { get; set; }
        public int ni_scorecard { get; set; }
        public DateTime agentEarlierCallDate { get; set; }
    }
    public class AgentRankingInfo
    {
        public string agent { get; set; }
        public string agent_block { get; set; }
    }


    //V2
    public class AgentMissedPoint
    {
        public string agentId { get; set; }
        public string questionShortName { get; set; }
        public int totalCalls { get; set; }
        public int missedCalls { get; set; }
        public bool isLinked { get; set; }
        public string questionType { get; set; }
        public bool isComposite  { get; set; }
        public int? questionId { get; set; }
    }

    public class Agent
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> groupNames { get; set; }
        public decimal? averageScore { get; set; }
        public int totalCalls { get; set; }
        public DateTime? earliestCallDate { get; set; }
        public List<AgentMissedPoint> top3MissedPoints { get; set; }
        public int? totalBadCalls { get; set; }
        public decimal? previousAverageScore { get; set; }
        public int missedCalls { get; set; }
        public List<string> missedQuestion { get; set; }
        public List<string> questionName { get; set; }
    }
    public class UserGroupInfo
    {
        public string username { get; set; }
        public string groupname { get; set; }
    }

    public class AgentRankingResponseData
    {
        public List<Agent> agents { get; set; }
    }

}



