using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApi.Entities;

namespace WebApi.DataLayer
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultipleResultSets
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storedProcedure"></param>
        /// <returns></returns>
        public static MultipleResultSetWrapper MultipleResults(this CC_ProdEntities db, SqlCommand storedProcedure)
        {
            return new MultipleResultSetWrapper(db, storedProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        public class MultipleResultSetWrapper
        {
            private readonly CC_ProdEntities _db;
            private readonly SqlCommand _storedProcedure;
            public List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>> _resultSets;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="db"></param>
            /// <param name="storedProcedure"></param>
            public MultipleResultSetWrapper(CC_ProdEntities db, SqlCommand storedProcedure)
            {
                _db = db;
                _storedProcedure = storedProcedure;
                _resultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <returns></returns>
            public MultipleResultSetWrapper With<TResult>()
            {
                _resultSets.Add((adapter, reader) => adapter
                    .ObjectContext
                    .Translate<TResult>(reader)
                    .ToList());

                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public List<IEnumerable> Execute()
            {
                var results = new List<IEnumerable>();

                using (var connection = _db.Database.Connection)
                {
                    connection.Open();
                    _storedProcedure.Connection = (SqlConnection)connection;
                    using (var reader = _storedProcedure.ExecuteReader())
                    {
                        var adapter = ((IObjectContextAdapter)_db);
                        foreach (var resultSet in _resultSets)
                        {
                            results.Add(resultSet(adapter, reader));
                            reader.NextResult();
                        }
                    }
                    return results;
                }
            }
        }

    }
}