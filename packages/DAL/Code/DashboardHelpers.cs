using DAL.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace DAL.Code
{
    public class DashboardHelpers
    {
        public static SqlCommand GetFiltersParameters(Filter filter, string spName, string userName, Period comparison = null)
        {

            var sqlComm = new SqlCommand();
            sqlComm.CommandTimeout = int.MaxValue;
            sqlComm.CommandText = spName;
            sqlComm.CommandType = CommandType.StoredProcedure;

            sqlComm.Parameters.AddWithValue("@userName", userName);
            if (filter != null)
            {
                if (filter.filters.teamLeads != null && filter.filters.teamLeads.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filter.filters.teamLeads)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@teamLeadIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
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
                if (filter.filters.agents != null && filter.filters.agents.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filter.filters.agents)
                    {
                        preparedLst.Append("'" + value.Replace(",", "<!@!>") + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@agentIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (filter.filters.QAs != null && filter.filters.QAs.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filter.filters.QAs)
                    {
                        preparedLst.Append("'" + value + "',");
                    }
                    sqlComm.Parameters.AddWithValue("@qaIDs", preparedLst.ToString().Trim(Convert.ToChar(",")));
                }
                if (filter.filters.missedItems != null && filter.filters.missedItems.Count > 0)
                {
                    var preparedLst = new StringBuilder();
                    foreach (var value in filter.filters.missedItems)
                    {
                        preparedLst.Append(("'" + (value + "',")));
                    }

                    if (preparedLst != null)
                    {
                        sqlComm.Parameters.AddWithValue("@missedItemsIDs", preparedLst.ToString().Trim(','));
                    }
                }
                try
                {
                    sqlComm.Parameters.AddWithValue("@badCallOnly", filter.filters.badCallsOnly);
                }
                catch { }

                if(filter.filters != null)
                {
                    sqlComm.Parameters.AddWithValue("@filterByReviewDate", filter.filters.filterByReviewDate);
                    sqlComm.Parameters.AddWithValue("@passedOnly", filter.filters.passedOnly);
                    sqlComm.Parameters.AddWithValue("@failed", filter.filters.failedOnly);
                    sqlComm.Parameters.AddWithValue("@reviewType", filter.filters.reviewType);
                    sqlComm.Parameters.AddWithValue("@missedBy", filter.filters.missedBy);
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

            if (comparison != null)
            {

                if (filter == null || comparison.start == null || comparison.start.Length < 4)
                {
                    sqlComm.Parameters.AddWithValue("@CompareStart", System.DateTime.Now.AddMonths(-1).AddDays(-14).ToString("d/M/YYYY"));
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@CompareStart", DateTime.Parse (comparison.start )); 
                }
                if (filter == null || comparison.end == null || comparison.end.Length < 4)
                {
                    sqlComm.Parameters.AddWithValue("@CompareEnd", System.DateTime.Now.AddMonths(-1).ToString("d/M/YYYY"));
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@CompareEnd", DateTime.Parse (comparison.end )); 
                }
            }

            return sqlComm;
        }
    }
}