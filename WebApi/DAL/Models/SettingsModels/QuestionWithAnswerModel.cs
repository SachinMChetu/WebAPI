using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.SettingsModels
{
    public class QuestionsWithAnswerModel
    {
        public int questionId { get; set; }
        public string questionName { get; set; }
        public int answerId { get; set; }
        public string answerText { get; set; }
        public int questionIdFromAnswer { get; set; }
    }
    public class QuestionInfoForCalcRules
    {
        public int questionId { get; set; }
        public string questionName { get; set; }
    }

    public class CommentInfoModel
    {
        public int commentId { get; set; }
        public string comment { get; set; }
        public int questionIdFromComment { get; set; }
    }
    public class AnswerInfo
    {
        public int answerId { get; set; }
        public string answerText { get; set; }
        public int questionIdFromAnswer { get; set; }
    }
    public class QuestionsWithAnswerListModel
    {
        public QuestionInfoForCalcRules question { get; set; }
        public List<AnswerInfo> answers { get; set; }
        public List<CommentInfoModel> comments { get; set; }
    }
}
