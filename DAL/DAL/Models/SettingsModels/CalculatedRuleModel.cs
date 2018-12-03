using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.SettingsModels
{
    public class RuleItemModel
    {
        public int id { get; set; }
        public string ruleType { get; set; }
        public string ruleSource { get; set; }
        public string ruleOperator { get; set; }
        public string ruleValue { get; set; }
        public int? qcId { get; set; }
        public int? qID { get; set; }


        public static RuleItemModel Create(IDataRecord record)
        {
            return new RuleItemModel
            {
                id = record.Get<int>("ruleID"),
                ruleType = record.Get<string>("rule_type"),
                ruleSource = record.Get<string>("rule_source"),
                ruleOperator = record.Get<string>("rule_operator"),
                ruleValue = record.Get<string>("rule_value"),
                qcId = record.GetValueOrDefault<int?>("questionCalcId"),
            };
        }
    }
}
