using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class Answer
    {
        public string Answers;
        public bool RightAnswer;
        public int Points;
        public List<Comment> Comments;
        public int AnswerID;
        public int autoselect;
    }
}