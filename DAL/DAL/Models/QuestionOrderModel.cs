using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class QuestionOrderModel
    {
        public int questionId { get; set; }
        public int? questionOrder { get; set; }
    }

    public class QuestionOrdering
    {
       public int scorecardId { get; set; } 
       public List<QuestionOrderModel> questions { get; set; }
    }
}
