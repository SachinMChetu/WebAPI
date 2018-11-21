using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Models
{
    public class CompositeQuestionAnswer
    {
        public List<CompositeAnswerComment> comments { get; set; }
	    public string customComment{get;set;}
        public double? position { get;   set; }

        public CompositeQuestionAnswer()
        {
            comments = new List<CompositeAnswerComment>();
        }

        public static CompositeQuestionAnswer Create(IDataRecord reader)
        {
            try
            {
                return new CompositeQuestionAnswer
                {
                    customComment = reader.GetValueOrDefault<string>("answerText"),
                    position = reader.GetValueOrDefault<double?>("position")
                };
            }catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}