using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderingDropDown
    {
        public int questionId { get; set; }
        public List<DropDownItemSimple> list { get; set; }
    }

    public class DropDownItemSimple
    {
        public int id { get; set; }
        public int order { get; set; }
    }


    public class OrderingFAQs
    {
        public int questionId { get; set; }
        public List<FAQsModel> list { get; set; }
    }

    public class AnswerOrdering
    {
        public int questionId { get; set; }
        public List<AnswerModel> list { get; set; }
    }


    public class SectionOrderingModel
    {
        public int scorecard { get; set; }
        public List<SectionModel> list { get; set; }
    }

    public class TemlateItemOrdering
    {
        public int questionId { get; set; }
        public List<TemplateItemModel> list { get; set; }
    }

    public class AnswerCommentsOrdering
    {
        public int questionId { get; set; }
        public List<AnswerCommentModel> list { get; set; }
    }
}
