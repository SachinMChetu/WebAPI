using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AppModelWL
    {
        /// <summary>
        /// ID's of app or scorecard or notification or something else
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Name of app or scorecard or notification or something else
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// flag to show active inactive status
        /// </summary>
        public bool active { get; set; }
        /// <summary>
        /// small logi to show logo with scorecard
        /// </summary>
        public string logo { get; set; }

        public static AppModelWL CreateRecord(IDataRecord dataRecord)
        {
            return new AppModelWL
            {
                id = dataRecord.GetValueOrDefault<int?>("id"),
                name = dataRecord.Get<string>("name"),
                active = dataRecord.Get<bool>("active"),
                logo = dataRecord.Get<string>("logo")
            };
        }

        public static List<AppModelWL> Create(IDataReader reader)
        {
            var  list = new List<AppModelWL>();
            while (reader.Read())
            {
                try
                {
                    list.Add(CreateRecord(reader));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }
    }


}
