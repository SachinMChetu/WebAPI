using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TemplateItemModel
    {
        public int id { get; set; }
        public string optionText { get; set; }
        public int? qID { get; set; }
        public DateTime? dateAdded { get; set; }
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public int? optionOrder { get; set; }

        public static TemplateItemModel CreateRecord(IDataRecord record)
        {
            return new TemplateItemModel
            {
                id = record.Get<int>("id"),
                optionText = record.Get<string>("option_text"),
                qID = record.GetValueOrDefault<int?>("question_id"),
                dateAdded = record.GetValueOrDefault<DateTime>("date_added"),
                dateStart = record.GetValueOrDefault<DateTime>("date_start"),
                dateEnd = record.GetValueOrDefault<DateTime>("date_end"),
                optionOrder = record.GetValueOrDefault<int?>("option_order")
            };
        }

        public static List<TemplateItemModel> Create(IDataReader reader)
        {
            var list = new List<TemplateItemModel>();
            while (reader.Read())
            {
                try
                {
                    list.Add(CreateRecord(reader));
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }
    }

}
