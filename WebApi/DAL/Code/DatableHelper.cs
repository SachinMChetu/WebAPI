using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DAL.Code
{
    public static class DataTableX

    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {            
            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }
    }
    public static class DataTableExtensions
    {
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            Regex regex = new Regex(@"<[^>]*>");
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    if (row[column].GetType().Name == "String")
                    {
                        var match = regex.Replace(row[column].ToString(), " ");
                        match = match.Replace("||", " ");
                        dic[column.ColumnName] = match;
                    }
                    else
                    {
                        dic[column.ColumnName] = row[column];
                    }
                   
                    
                }
            }
            return dynamicDt;
        }
    }
}