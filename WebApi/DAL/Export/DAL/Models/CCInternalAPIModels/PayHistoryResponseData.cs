using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.CCInternalAPI;

namespace DAL.Models.CCInternalAPIModels
{
    public class PayHistoryResponseData
    {
        public List<PayHistory> PayHistories { get; set; }
    }
}
