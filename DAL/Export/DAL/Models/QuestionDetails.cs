using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Models
{
    public class QuestionDetails
    {
        public int questionId { get; set; }
        public string questionShortName { get; set; }
        public int callId { get; set; }
        public AnswerDetails answer { get; set; }
        public List<AnswerComment> comments { get; set; }
        public string customComment { get; set; }
        public string questionSectionName { get; set; }
    }

    public class QuestionDetails_v2
    {
        public int callId { get; set; }
        public string questionShortName { get; set; }
        public string questionSectionName { get; set; }
        public string questionType { get; set; }
        public int questionId { get; set; }
        public bool isRightAnswer { get; set; }
        public bool isComposite { get; set; }
        public SimpleQuestionAnswer simpleQuestionAnswer { get; set; }
        public CompositeQuestionAnswer compositeQuestionAnswer { get; set; }
        public bool isLinked { get;  set; }

        //public QuestionDetails_v2()
        //{
        //    simpleQuestionAnswer = new SimpleQuestionAnswer();
        //    compositeQuestionAnswer = new CompositeQuestionAnswer();
        //}
        public static QuestionDetails_v2 CreateRecordCallDetails(IDataRecord record)
        {
            return new QuestionDetails_v2
            {
                callId = record.GetValueOrDefault<int>("callId"),
                questionShortName = record.GetValueOrDefault<string>("questionShortName"),
                questionId = record.GetValueOrDefault<int>("questionID"),
                questionSectionName = record.GetValueOrDefault<string>("questionSectionName"),
                questionType = record.GetValueOrDefault<string>("questionType"),
                isRightAnswer = record.GetValueOrDefault<bool>("isRightAnswer"),
                isComposite = record.GetValueOrDefault<bool>("hasTemplate"),
                isLinked = record.GetValueOrDefault<bool>("isLinked")
            };
        }
        public static List<QuestionDetails_v2> Create(IDataReader reader)
        {
            List<QuestionDetails_v2> result = new List<QuestionDetails_v2>();
            while (reader.Read())
            {
                try
                {
                    var r = CreateRecordCallDetails(reader);
                    r.questionType = ((string)r.questionType).ToLower();
                    if (!r.isComposite)
                    {
                        r.simpleQuestionAnswer = SimpleQuestionAnswer.Create(reader);
                    }
                    else
                    {
                        r.compositeQuestionAnswer = CompositeQuestionAnswer.Create(reader);
                    }
                    result.Add(r);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
            return result;
        }


    }

}