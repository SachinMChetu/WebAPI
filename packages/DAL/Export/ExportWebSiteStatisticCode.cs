using DAL.Code;
using DAL.GenericRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Export
{
    public class ExportWebSiteStatisticCode
    {
        public dynamic ExportWebSiteStatistic(Filter filters, string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetWebSiteStatisticApi]", userName);
                sqlComm.CommandTimeout = int.MaxValue;
                sqlComm.Connection = sqlCon;

                WebSiteAverage webSiteAverage = new WebSiteAverage();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        webSiteAverage = new WebSiteAverage()
                        {
                            total = reader.IsDBNull(reader.GetOrdinal("total")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("total")).ToString()),
                            compliant = reader.IsDBNull(reader.GetOrdinal("compliant")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("compliant")).ToString()),
                            bad = reader.IsDBNull(reader.GetOrdinal("bad")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("bad")).ToString()),
                            nonCompliant = reader.IsDBNull(reader.GetOrdinal("nonCompliant")) ? 0 : int.Parse(reader.GetValue(reader.GetOrdinal("nonCompliant")).ToString()),
                        };

                    }
                    //return webSiteAverage;
                    var propNames = new List<PropertieName>
                    {
                         new PropertieName { propName = "Total", propValue = "total", propPosition = 1 },
                        new PropertieName { propName = "Compliant", propValue = "compliant", propPosition = 2 },
                        new PropertieName { propName = "Non compliant", propValue = "nonCompliant", propPosition = 3 },
                        new PropertieName { propName = "Bad", propValue = "bad", propPosition = 4 }
                    };
                    List<ExportWebSiteStatisticModel> exportWebSiteStatistic = new List<ExportWebSiteStatisticModel>();
                    exportWebSiteStatistic.Add(new ExportWebSiteStatisticModel()
                    {
                        bad = webSiteAverage.bad,
                        compliant = webSiteAverage.compliant,
                        nonCompliant = webSiteAverage.nonCompliant,
                        total = webSiteAverage.total
                    });




                    ExportHelper.Export(propNames, exportWebSiteStatistic, "WebStat" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "WebSiteStatistic", userName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }
    }
}

