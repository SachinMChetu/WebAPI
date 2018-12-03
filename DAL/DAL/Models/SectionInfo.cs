using System;
using System.Collections.Generic;
using System.Data;
using DAL.Extensions;
using DAL.Models.CalibrationModels;
//using static DAL.Controllers.CalibrationController;

namespace DAL.Models
{
    /*Response:
Section = {
	sectionName:    String
    questions:      [QuestionInfo]
}
ScorecardInfo = {
	scorecard:    Scorecard,
	sections:     [Section]
}
GetSectionsInfo = [ScorecardInfo]*/

    public class SectionInfoStat
    {
        //public List<ScorecardSectionInfo> ScorecardInfo { get; set; }

        public class ScorecardSectionInfo
        {
            public Scorecard scorecard { get; set; }
            public List<Section> sections { get; set; }
        }
        public class Section
        {
            public SectionInfo sectionInfo { get; set; }
            public List<SectionQuestionDetail> questions { get; set; }
        }
        public class SectionInfo
        {
            public int? sectionOrder { get; set; }
            public int sectionId { get; set; }
            public string sectionName { get; set; }
        }
        public class SectionQuestionDetail
        {
            public int qId { get; set; }
            public string questionShortName { get; set; }
            public int totalRight { get; set; }
            public int totalWrong { get; set; }
            public bool isComposite { get; set; }
            public bool isLinked { get; set; }
            public string questionType { get; set; }
            public int? questionOrder { get; set; }
        }
    }
    public class SectionInfoRaw
    {
        public int qId { get; set; }
        public string questionShortName { get; set; }
        public int totalRight { get; set; }
        public int totalWrong { get; set; }
        public bool isComposite { get; set; }
        public bool isLinked { get; set; }
        public string questionType { get; set; }
        public string scorecardName { get; set; }
        public int scorecardId { get; set; }
        public int? sectionOrder { get; set; }
        public string sectionName { get; set; }
        public int sectionId { get; set; }
        public int? qorder { get;  set; }

        public static SectionInfoRaw CreateRecord(IDataRecord dataRecord)
        {
            return new SectionInfoRaw
            {
                qId = dataRecord.Get<int>("qid"),
                questionShortName = dataRecord.Get<string>("questionShortName"),
                sectionName = dataRecord.Get<string>("sectionName"),
                isComposite = dataRecord.Get<bool>("hasTemplate"),
                isLinked = dataRecord.Get<bool>("isLinked"),
                questionType = dataRecord.Get<string>("questionType"),
                scorecardId = dataRecord.Get<int>("scorecardId"),
                scorecardName = dataRecord.Get<string>("scorecardName"),
                sectionId = dataRecord.Get<int>("sectionId"),
                sectionOrder = dataRecord.GetValueOrDefault<int?>("sectionOrder"),
                totalWrong = dataRecord.Get<int>("totalWrong"),
                totalRight = dataRecord.Get<int>("totalRight"),
                qorder = dataRecord.GetValueOrDefault<int?>("qorder"),
            };
        }


        public static List<SectionInfoRaw> Create(IDataReader reader)
        {
            List<SectionInfoRaw> result = new List<SectionInfoRaw>();
            while (reader.Read())
            {
                try
                {
                    var r = CreateRecord(reader);
                    r.qorder = r.qorder == null ? -1 : r.qorder;
                    r.sectionOrder = r.sectionOrder == null ? -1 : r.sectionOrder;
                    result.Add(r);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
            return result;
        }
    }
}