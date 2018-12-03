using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Z.Dapper.Plus;
using System.Linq;
namespace DAL.GenericRepository
{
    //public static IEnumerable<dynamic> Query(this IDbConnection cnn, string sql, object param = null, SqlTransaction transaction = null, bool buffered = true);


    class DapperHelper
    {
        private static string constr = System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString;
        public static Func<DbConnection> ConnectionFactory = () => new SqlConnection(constr);

        static public T GetSingle<T>(string queryString)
        {
            using (var connection = new SqlConnection(constr))
            {
                connection.Open();
                dynamic data = connection.Query<T>(queryString).FirstOrDefault();
                return data;
            }
        }


        static public IEnumerable<T> GetList<T>(string queryString)
        {
            using (var connection = new SqlConnection(constr))
            {
                connection.Open();
                var data = connection.Query<T>(queryString);
                return data;

            }
        }

        static public IEnumerable<T> GetList<T>(string queryString, string param)
        {
            using (var connection = new SqlConnection(constr))
            {
                connection.Open();
                using (var multi = connection.QueryMultiple(constr,new { @param = param },commandType:CommandType.Text))
                {
                    var invoiceItems = multi.Read<T>();
                    return invoiceItems;
                }
            }
        }
    }
}
