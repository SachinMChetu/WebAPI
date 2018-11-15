using System.Collections.Generic;

namespace DAL.Models
{

    //remove after v2
    public class TopMissedItems
    {
        public string numCalls { get; set; }
        public string shortName { get; set; }
        public string totalWrong { get; set; }
        public string percentQs { get; set; }
        public string qShortname { get; set; }
        public string questionId { get; set; }
        public string topMissed { get; set; }
    }
    //v2
    public class MissedItemAgentInfo
    {
        public int questionId { get; set; }
        public string name { get; set; }
        public int totalCalls { get; set; }// calls by agent within selected period where question is present
        public int missedCalls { get; set; }// calls by agent within selected period where he failed the question
    }

    public class MissedItem
    {
        public int comparedMissedCalls { get; set; }
        public int comparedTotalCalls { get; set; }

        public int questionId { get; set; }
        public string questionSectionName { get; set; }
        public string questionShortName { get; set; }
        public string scorecardName { get; set; }
        public int totalCalls { get; set; }
        public int missedCalls { get; set; }
        public bool isLinked { get; set; }
        public string questionType { get; set; }
        public bool isComposite { get; set; }
        public List<MissedItemAgentInfo> top3Agents { get; set; }
    }

    public class TopMissedItemsResponseData
    {
        public List<MissedItem> missedItems { get; set; }
    }






}