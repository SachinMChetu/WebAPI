using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Models
{
    public class SimpleQuestionAnswer
    {
        public int answerId { get; set; }
        public string answerText { get; set; }
        public double? position { get; set; }
        public List<SimpleAnswerComment> comments { get; set; } // optional for some requests
        public dynamic customComment { get; set; }

        public SimpleQuestionAnswer()
        {
            comments = new List<SimpleAnswerComment>();
        }

        public static SimpleQuestionAnswer Create(IDataRecord dataReader)
        {
            try
            {
                return new SimpleQuestionAnswer
                {
                    answerId = dataReader.GetValueOrDefault<int>("answerId"),
                    answerText = dataReader.GetValueOrDefault<string>("answerText"),
                    position = dataReader.GetValueOrDefault<double?>("position")
                };
            }catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}