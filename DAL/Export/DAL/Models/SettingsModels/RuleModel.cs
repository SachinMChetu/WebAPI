using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.SettingsModels
{

   

    public class RuleModel
    {
        public QuestionCalc rule { get; set; }
        public List<RuleItemModel> ruleItems { get; set; }


        public RuleModel()
        {
            ruleItems = new List<RuleItemModel>();
            
        }

      
        public static List<RuleModel> Create(IDataReader reader)
        {
            List<RuleModel> rules = new List<RuleModel>();
            List<RuleItemModel> ruleItems = new List<RuleItemModel>();
            List<QuestionCalc> calc = new List<QuestionCalc>();
            while (reader.Read())
            {
                calc.Add(QuestionCalc.CreateRecord(reader));
            }

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    try
                    {
                        ruleItems.Add(RuleItemModel.Create(reader));
                    }catch(Exception ex)
                    {
                        throw ex;
                    }
                    
                }
            }
            foreach (var item in calc)
            {
                RuleModel ruleModel = new RuleModel();
                ruleModel.rule = item;
                foreach (var ruleItem in ruleItems)
                {
                    if(ruleItem.qcId == item.id)
                    {
                        try
                        {
                            ruleModel.ruleItems.Add(ruleItem);
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                        
                    }
                }
                rules.Add(ruleModel);
            }
            return rules;
        }
    }


    public class QuestionCalc
    {
        public int id { get; set; }
        public int? qID { get; set; }
        public string description { get; set; }
        public bool? active { get; set; }
        public int? questionAnswerId { get; set; }
        public int? oldQid { get; set; }
        public int? oldId { get; set; }

        public static QuestionCalc CreateRecord(IDataRecord record)
        {
            return new QuestionCalc
            {
                id = record.Get<int>("id"),
                qID = record.GetValueOrDefault<int?>("qid"),
                description = record.Get<string>("rule_description"),
                active = record.GetValueOrDefault<bool?>("rule_active"),
                questionAnswerId = record.GetValueOrDefault<int?>("q_answer"),
                oldQid = record.GetValueOrDefault<int?>("old_QID"),
                oldId = record.GetValueOrDefault<int?>("old_id")
            };
        }
    }
}
