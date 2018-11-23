using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CloneModel
    {
        public ScorecardsInfo Scorecards { get; set; }
        public QuestionModel QuestionModel { get; set; }
      
    }

    public class MultipleCloneModel
    {
        public int scorecardId { get; set; }
        public int sectionId { get; set; }
        public List<QuestionModel> list { get; set; }
    }
}
