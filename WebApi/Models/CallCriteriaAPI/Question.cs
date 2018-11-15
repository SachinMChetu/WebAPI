using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class Question
    {
        public string question;
        public string sectionID;
        public string order;
        public bool active;
        public string QuestionShort;
        public List<string> TemplateOptions;
        public List<Answer> answers;
        public List<FAQ> FAQs;
        public List<Instruction> instructions;
        public int QID;
        public string LinkedAnswer;
        public string LinkedComment;
        public bool comments_allowed;
        public int QAPoints;
    }
}