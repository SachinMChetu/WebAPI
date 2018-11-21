using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{

    public class AppsListModel
    {
        public string appName { get; set; }
        public List<ScorecardInfo> scorecards { get; set; }

        public AppsListModel()
        {
            scorecards = new List<ScorecardInfo>();
        }

        public static List<AppsListModel> Create(IDataReader reader)
        {
            List<AppsWithScorecards> list = new List<AppsWithScorecards>();
            List<AppsListModel> result = new List<AppsListModel>();
            List<AppInfo> appinfo = new List<AppInfo>();
            while (reader.Read())
            {
                try
                {
                    appinfo.Add(AppInfo.Create(reader));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    try
                    {
                        list.Add(AppsWithScorecards.Create(reader));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }


            foreach (var item in appinfo)
            {
                var rez = new AppsListModel
                {
                    appName = item.name,
                    scorecards = list.Where(x=>x.appName == item.name).Select(x=>x.scorecard).ToList()
                };
                result.Add(rez);
            }
            return result;

        }
    }


    public class AppsWithScorecards
    {
        public string appName { get; set; }
        //public int appId { get; set; }
        public ScorecardInfo scorecard { get; set; }

        public static AppsWithScorecards Create(IDataRecord record)
        {
            try
            {
                return new AppsWithScorecards
                {
                    appName = record.Get<string>("appname"),
                   // appId = record.Get<int>("id"),
                    scorecard = new ScorecardInfo
                    {
                        scorecardId = record.Get<int>("scorecardId"),
                        scorecardName = record.Get<string>("scorecardName")
                    }

                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }

    public class AppInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public static AppInfo Create(IDataRecord record)
        {
            return new AppInfo
            {
                id = record.Get<int>("id"),
                name = record.Get<string>("appname")
            };
        }
    }
}
