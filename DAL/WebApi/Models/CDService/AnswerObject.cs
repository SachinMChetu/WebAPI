using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class AnswerObject
    {
        public int ID;
        public string Comment;
        public bool IsRight;
        public int Total;
        public int Count;
        public List<AnswerCallDetail> CallDetails;
    }
}