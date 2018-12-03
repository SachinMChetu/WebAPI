using DAL.Code;
using DAL.Models;
using DAL.Models.ConversionModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DAL.Extensions;

namespace WebApi.Controllers
{
    /// <summary>
    /// Conversion Controller
    /// </summary>
    public class ConversionController : ApiController
    {
        /// <summary>
        ///     Get the Total and Average Conversion statistics 
        /// </summary>
        /// <param name="filters"></param>       
        [Route("conversion/GetConversionStatsData")]
        [HttpPost]
        [ResponseType(typeof(ConversionTotalAndAvgData))]
        public ConversionTotalAndAvgData GetConversionStatsData([FromBody]Filter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
				var userName = HttpContext.Current.GetUserName();
				Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getConversionStatistics]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;                
                ConversionTotalAndAvgData totals = new ConversionTotalAndAvgData();

                sqlCon.Open();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    if (reader.Read())
                    {
                        totals = ConversionTotalAndAvgData.Create(reader);
                    }                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return totals;
            }

        }

        /// <summary>
        ///    Returns performance data for conversion page chart
        /// </summary>
        /// <param name="filters"></param>

        [Route("conversion/GetConversionPerformanceData")]
        [HttpPost]
        [ResponseType(typeof(ConversionRangeDataResponse))]
        public ConversionRangeDataResponse GetConversionPerformanceData([FromBody]Filter filters)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string userName = HttpContext.Current.GetUserName();

				Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getConversionPerformanceData]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type

                sqlComm.Connection = sqlCon;
				ConversionRangeDataResponse performanceData = new ConversionRangeDataResponse();               
                sqlCon.Open();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
					performanceData = ConversionRangeDataResponse.Create(reader);
				}
                catch (Exception ex)
                {
                    throw ex;
                }
                return performanceData;
            }                
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filters"></param>		
        [Route("conversion/GetChartData")]
        [HttpPost]
        [ResponseType(typeof(ConversionChartData))]
		public ConversionChartData GetChartData([FromBody]ConversionChartFilter filters)
        {			
			using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
				string userName = HttpContext.Current.GetUserName();
				Filter f = new Filter() { filters = filters.filters, range = filters.range };
#pragma warning disable CS0436 // Type conflicts with imported type
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(f, "[getConversionChartData]", userName);
#pragma warning restore CS0436 // Type conflicts with imported type
                if (filters.chartType != null && filters.chartType != "")
                {
                    sqlComm.Parameters.AddWithValue("@chartType", filters.chartType);
                }
                sqlComm.Parameters.AddWithValue("@pagerows", filters.paging.pagerows);
                sqlComm.Parameters.AddWithValue("@pagenum", filters.paging.pagenum);
                if (filters.sorting?.sortBy != null && filters.sorting.sortOrder != null && filters.sorting.sortBy != "" && filters.sorting.sortOrder != "")
                {

                    sqlComm.Parameters.AddWithValue("@OrderByColumn", filters.sorting.sortBy);
                    sqlComm.Parameters.AddWithValue("@sortOrder", (filters.sorting.sortOrder != "desc"));
                }

                sqlComm.Connection = sqlCon;
                ConversionChartData chartData = new ConversionChartData();
                sqlCon.Open();
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
					chartData = ConversionChartData.Create(reader);
 				}
                catch (Exception ex)
                {
                    throw ex;
                }
                return chartData;
            }
        }

    }   
}
