
using System.Collections.Generic;
using DAL.Models.CalibrationModels;

namespace DAL.Models
{
    public class QuestionInfo
    {

        public int questionID { get; set; }
        public string questionName { get; set; }
        public string sectionName { get; set; }

        public bool isComposite { get; set; }
        public string questionType { get; set; }
        public bool isLinked { get; set; }

        public QuestionStatistic simpleQuestionStat { get; set; }
        public CompositeQuestionStatistic compositeQuestionStat { get; set; }
        public Scorecard scorecard { get; set; }
        public int total { get; set; }
        //public AgregatedAnswer agregatedAnswer { get; internal set; }
    }

    public class AgregatedAnswer
    {
        public int count { get; set; }
        public string answerText { get; set; }
        public int answerId { get; set; }
    }

    public class QuestionStatistic
    {
        public List<AnswerCommentStatList> answerList { get; set; }
    }
    public class CompositeQuestionStatistic
    {
        public List<CompositeQuestionStat> answerList { get; set; }
    }
    public class AnswerCommentStatList
    {
        public int? answerCommentId { get; set; }

        public string answerText { get; set; }
        public int total { get; set; }
        public bool isRightAnswer { get; set; }
        public string answerComent { get; set; }
        public string commentText { get; set; }
        public int answerId { get; set; }
    }

    public class CompositeQuestionStat
    {
        public int total { get; set; }
        public int checkedCount { get; set; }
        public int notCheckedCount { get; set; }
        public string answerText { get; set; }
    }




    public class QuestionInfo1
    {
        public int questionID { get; set; }
        public string questionName { get; set; }
        public string sectionName { get; set; }
        public bool isComposite { get; set; }
        public string questionType { get; set; }
        public bool isLinked { get; set; }
        public SimpleQuestionStat simpleQuestionStat { get; set; }
        public CompositeQuestionStats compositeQuestionStat { get; set; }
        public Scorecard scorecard { get; set; }
        public int total { get; set; }    }

    public class CompositeQuestionStats
    {
        public int totalRight { get; set; }
        public CompositeAnswerInfo rightAnswerInfo { get; set; }
        public CompositeAnswerInfo wrongAnswerInfo { get; set; }
        public List<QuestionInfoAnswerComment> comments { get; set; }

    }

    public class QuestionInfoAnswerComment
    {
        public string commentText { get; set; }
        public int? commentId { get; set; }
        public int total { get; set; }

    }

    public class CompositeAnswerInfo
    {
        public int totalCustomComments { get; set; }
        public string answerText { get; set; }
        public int  answerId { get; set; }
    }

    public class SimpleQuestionStat
    {
        public List<SimpleAnswer> answerList { get; set; }
    }
    public class SimpleComment
    {
        public List<CompositeQuestionStat> answerList { get; set; }
    }
    public class SimpleAnswer
    {
        public string answerText { get; set; }
        public int total { get; set; }
        public bool isRightAnswer { get; set; }
        public List<QuestionInfoAnswerComment> comments { get; set; }
        public int totalCustomComments { get; set; }
        public int answerId { get; set; }
    }


}