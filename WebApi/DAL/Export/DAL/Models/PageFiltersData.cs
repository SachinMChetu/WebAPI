using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class PageFiltersData
    {
        public List<FilterValue> scorecards { get; set; }
        public List<FilterValue> campaigns { get; set; }
        public List<FilterValue> groups { get; set; }
        public List<FilterValue> agents { get; set; }
        public List<FilterValue> QAs { get; set; }
        public List<FilterValue> missedItems { get; set; }
        public RangeCalls rangeCalls { get; set; }
        public List<DayCalls> dayCalls { get; set; }
    }

    public class FilterValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //  public List<AdditionalInfo> additionalInfo { get; set; }
    }

    public class AdditionalInfo
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class RangeCalls
    {
        public int total { get; set; }
        public int filtered { get; set; }
    }

    public class DayCalls
    {
        public int total { get; set; }
        public int filtered { get; set; }
        public DateTime day { get; set; }
    }

    public class FiltersFormData
    {
        public List<FilterScorecardValue> scorecards { get; set; }
        public List<FilterCampainValue> campaigns { get; set; }
        public List<FilterGroupValue> groups { get; set; }
        public List<FilterAgentValue> agents { get; set; }
        public List<FilterQAValue> QAs { get; set; }
        public List<FilterMissedValue> missedItems { get; set; }
        public List<FilterTeamLeadValue> teamLeads { get; set; }
        public RangeCalls rangeCalls { get; set; }
        public List<DayCalls> dayCalls { get; set; }
        public bool failedOnly { get; set; }
        public bool badCallsOnly { get; set; }
        public bool passedOnly { get; set; }
    }

    public class FiltersFormDataReports
    {
        public List<FilterScorecardValue> scorecards { get; set; }
        public List<FilterCampainValue> campaigns { get; set; }
        public List<FilterGroupValue> groups { get; set; }  
        public List<FilterAppValue> apps { get; set; }
        public RangeCalls rangeCalls { get; set; }
        public List<DayCalls> dayCalls { get; set; }
    }

    public class FilterMissedValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
    }

    public class FilterGroupValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
        public List<AdditionalGroupsInfo> top3Agents { get; set; }
    }

    public class AdditionalGroupsInfo
    {
        public string agentName { get; set; }
        public string agentId { get; set; }
    }

    public class FilterAgentValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
        public AgentGroupInfo agentGroup { get; set; }
    }

    public class AgentGroupInfo
    {
        public string groupName { get; set; }
        public string groupId { get; set; }
    }

    public class FilterCampainValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
    }

    public class FilterScorecardValue
    {
        public int failScore { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public int count { get; set; }
    }
    public class FilterAppValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
    }

    public class FilterTeamLeadValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
        public List<AdditionalTeamLeadInfo> top3QAs { get; set; }
    }

    public class AdditionalTeamLeadInfo
    {
        public string qaName { get; set; }
        public string qaId { get; set; }
    }

    public class FilterQAValue
    {
        public string name { get; set; }
        public string id { get; set; }
        public int count { get; set; }
        public List<AdditionalQAInfo> qaTeam { get; set; }
    }

    public class AdditionalQAInfo
    {
        public string teamLeadName { get; set; }
        public string teamLeadId { get; set; }
    }

}