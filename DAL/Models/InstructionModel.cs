using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class InstructionModel
    {
        public int id { get; set; }
        public int? questionId { get; set; }
        public string questionText { get; set; }
        public string answertype { get; set; }
        public int? qOrder { get; set; }
        public DateTime? dateAdded { get; set; }
        
    }
}
