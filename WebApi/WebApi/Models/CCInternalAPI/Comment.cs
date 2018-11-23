using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// Comment
    /// </summary>
    public class Comment
    {
        public string CommentText;
        public int CommentID;
        public float CommentPoints;
        public string CommentWho;
        public string CommentDate;
        public string NavigateQuestion;
        public string AnswerStatement;
    }

}