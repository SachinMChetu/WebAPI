using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// Answer
    /// </summary>
    public class Answer
    {
        public string Answers;
        public bool RightAnswer;
        public int Points;
        public List<Comment> Comments;
        public List<string> DropdownItems;
        public int AnswerID;
        public int autoselect;
        public bool acp_required;
        public bool RequireCustomComment;
    }

}