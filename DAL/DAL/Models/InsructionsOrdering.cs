using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class InsructionsOrdering
    {
        public int questionId { get; set; }
        public List<InstructionModel> list { get; set; }
    }
}
