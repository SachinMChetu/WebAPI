using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// Question
    /// </summary>
    public class Question
    {
        public string question;
        public string sectionID;
        public string order;
        public bool active;
        public string QuestionShort;
        public List<string> TemplateOptions;

        public List<TemplateItem> TemplateItems;
        public List<Answer> answers;
        public List<FAQ> FAQs;
        public List<Instruction> instructions;
        public int QID;
        public Nullable<int> LinkedAnswer;
        public Nullable<int> LinkedComment;
        public bool comments_allowed;
        public int QAPoints;
        public bool LinkedVisible;
        public bool SingleComment;
        public bool WideQuestion;
        public bool RequireCustomComment;
        public string DropDownType;
        public string DropDownEndpoint;
        public bool LeftColumnQuestion;
        public string OptionVerb;
        public string sentence;
        public string QuestionType;
    }
}