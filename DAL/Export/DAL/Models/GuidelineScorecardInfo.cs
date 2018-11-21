
using System;
using System.Collections.Generic;
using System.Web;
using static DAL.Models.SectionInfoStat;

namespace DAL.Models
{
    public class GuidelineScorecardInfo
    {
        public GIScorecardInfo scorecardInfo { get; set; }
        public List<MetaDataItem> metaData { get; set; }//   xrn_close -- metadata updates
        public List<SchoolDataItem> schoolData { get; set; }//school_clone -- school, aoi1
        public List<OtherDataItem> otherData { get; set; }//other_clone key/value
    }

    public class OtherDataItem
    {
        public int? id { get; set; }
        public int? formId { get; set; }
        public string dataKey { get; set; }
        public string dataValue { get; set; }
        public string dataType { get; set; }
        public int? xccId { get; set; }
        public string schoolName { get; set; }
        public string label { get; set; }
    }

    public class SchoolDataItem
    {
        public int? id { get; set; }
        public string school { get; set; }
        public string AOI1 { get; set; }
        public string AOI2 { get; set; }
        public string L1_SubjectName { get; set; }
        public string L2_SubjectName { get; set; }
        public string Modality { get; set; }
        public int? xccId { get; set; }
        public string College { get; set; }
        public string DegreeOfInterest { get; set; }
        public string origin { get; set; }
        public string tcpa { get; set; }
    }

    public class QuestionTemplateItem
    {
        public int optionId { get; set; }
        public string optionText { get; set; }
        public bool @checked { get; set; }
    }

    public class CompositeQuestionInfo
    {
        public List<Answer> answers { get; set; }
        //public string answerText { get; set; }
        //public int answerId { get; set; }
        //public double? points { get; set; }
        //public bool isRightAnswer { get; set; }
        public List<QuestionTemplateItem> comments { get; set; }    
    }

    public class Answer
    {
        public string answerText { get; set; }
        public int answerId { get; set; }
        public double? points { get; set; }
        public bool isRightAnswer { get; set; }
        //public List<GLComment> comments { get; set; }
        public List<string> dropdownItems{get;set;}
    }

    public class GLComment
    {
        public string commentText { get; set; }
        public int commentId { get; set; }
        public int? points { get; set; }
        public bool @checked { get; set; }
        public int answerId { get; set; }
    }

    public class AnswerInfo
    {
        public Answer answer { get; set; }
        public List<GLComment> comments { get; set; }

        public bool commentRequired { get; set; }
        public bool isAnswered { get; set; }
        public string customComment { get; set; }
    }

    public class SimpleQuestionInfo

    {
        public bool singleComment { get; set; }
        public List<AnswerInfo> answers { get; set; }
    }

    public class MetaDataItem
    {
        public string name { get; set; }
        public dynamic value { get; set; }
    }
    public class UpdateMetadataPayload {
        public int id { get; set; }
        public List<MetaDataItem> metaDataItems { get; set; }
    }

    public class Instruction
    {
        public string answerText { get; set; }
        public string instructionText { get; set; }
        public bool wasEdited { get; set; }
    }

    public class FAQ
    {
        public string answerText { get; set; }
        public string questionText { get; set; }
        public bool wasEdited { get; set; }
    }

    public class GLQuestionInfo
    {
        public List<FAQ> faqs { get; set; } // need only on listen page (on guidlines maybe no) ???
        public List<Instruction> instructions { get; set; }
        public int? questionId { get; set; }
        public bool isWide { get; set; }
        public bool isComposite { get; set; }
        public bool singleComment { get; set; }
        public bool isLinked { get; set; } // only for "linkedAnswerId" and "linkedCommentId"
        public string questionType { get; set; } // "simple" or "dynamic" or "composite"
        public int? linkedAnswerId { get; set; } // answerId the question is linked to
        public int? linkedCommentId { get; set; } // commentId the question is linked to
        public MetaDataItem linkedMetaData { get; set; } // if questionType is "dynamic" should not be empty. Question appear when MetaDataItem from MetaData matching this object 
        public bool linkedVisible { get; set; } // question is visible until "linkedAnswerId" or "linkedCommentId" is checked
        public string qustionShortName { get; set; }
        public bool commentAllowed { get; set; }
        public SimpleQuestionInfo simpleQuestionInfo { get; set; }  // not empty if "isComposite" is "false" - questionType is "simple" or "dynamic"
        public CompositeQuestionInfo compositeQuestionInfo { get; set; } // not empty if "isComposite" is "true"
        public bool useQuestion { get; set; }
        public string optionVerb { get; set; }
        public string dropdownType{get;set;}
    }

    public class GlScorecard
    {
        public int scorecardId { get; set; }
        public string scorecardName { get; set; }
        public string scorecardApp { get; set; }
        public string scorecardType { get; set; } // "audio", "website"
    }


    public class GlSection
    {
        public SectionInfo sectionInfo { get; set; }
        public List<GLQuestionInfo> questions { get; set; }
    }

    public class GIScorecardInfo
    {
        public GlScorecard scorecard { get; set; }
        public List<GlSection> sections { get; set; }
    }

}










