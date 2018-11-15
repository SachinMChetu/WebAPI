using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class rejection_reasons_Result
    {
       
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int? profile_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
            public string reason { get; set; }
       
    }
}