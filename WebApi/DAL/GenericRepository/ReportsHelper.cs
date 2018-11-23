using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.GenericRepository
{
    public class ReportsHelper
    {
        public static SqlCommand GetFiltersParameters(FilterReports filter, string spName, string userName, Period comparison = null)
        {

            var sqlComm = new SqlCommand();
            sqlComm.CommandTimeout = int.MaxValue;
            sqlComm.CommandText = spName;
            sqlComm.CommandType = CommandType.StoredProcedure;
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                sqlComm.Parameters.AddWithValue("@userName", userName);
                if (filter != null)
                {

                    if (filter.filters.scorecards != null && filter.filters.scorecards.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter.filters.scorecards)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sqlComm.Parameters.AddWithValue("@scorecardIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }
                    if (filter.filters.campaigns != null && filter.filters.campaigns.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter.filters.campaigns)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sqlComm.Parameters.AddWithValue("@campaignIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }

                    if (filter.filters.groups != null && filter.filters.groups.Count > 0)
                    {
                        var preparedLst = new StringBuilder();
                        foreach (var value in filter.filters.groups)
                        {
                            preparedLst.Append("'" + value + "',");
                        }
                        sqlComm.Parameters.AddWithValue("@groupIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                    }




                    if (filter.filters != null)
                    {
                        sqlComm.Parameters.AddWithValue("@filterByReviewDate", filter.filters.filterByReviewDate);
                    }

                }
                if (filter == null || filter.range.start == null || filter.range.start.Length < 4)
                {
                    sqlComm.Parameters.AddWithValue("@Start", DateTime.Now.AddDays(-14));
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Start", DateTime.Parse(filter.range.start));
                }
                if (filter == null || filter.range.end == null || filter.range.end.Length < 4)
                {
                    sqlComm.Parameters.AddWithValue("@end", DateTime.Now);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@end", DateTime.Parse(filter.range.end));
                }
                return sqlComm;
            }
            //if (comparison != null)
            //{

            //    if (filter == null || comparison.start == null || comparison.start.Length < 4)
            //    {
            //        sqlComm.Parameters.AddWithValue("@CompareStart", System.DateTime.Now.AddMonths(-1).AddDays(-14).ToString("d/M/YYYY"));
            //    }
            //    else
            //    {
            //        sqlComm.Parameters.AddWithValue("@CompareStart", DateTime.Parse(comparison.start));
            //    }
            //    if (filter == null || comparison.end == null || comparison.end.Length < 4)
            //    {
            //        sqlComm.Parameters.AddWithValue("@CompareEnd", System.DateTime.Now.AddMonths(-1).ToString("d/M/YYYY"));
            //    }
            //    else
            //    {
            //        sqlComm.Parameters.AddWithValue("@CompareEnd", DateTime.Parse(comparison.end));
            //    }
            //}

          
        }
    }
}
