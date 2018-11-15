
using ClosedXML.Excel;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace DAL.GenericRepository
{
    public static class ExportHelper
    {
        public static dynamic Export<T>(List<PropertieName> names, List<T> exportData, string fileName,string moduleName,string userName)
        {

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                string p = Path.Combine(HostingEnvironment.MapPath(@"~\export\"));
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }

                sqlCon.Open();
                string sql = "insert into exportQueue([fileowner], [fileNAme],[fileUrl],[exportDate],[status],[exportType]) OUTPUT inserted.id as id  select top 1 id ,'" + fileName
                    + "','webapi/export/" + fileName + "',getdate(),1 ,'" + moduleName + "' from userextrainfo where [username]='" + userName + "';";

                int recordId = 0;

                SqlCommand sqlInsertComm = new SqlCommand(sql, sqlCon)
                {
                    CommandText = sql
                };
                try
                {
                    SqlDataReader r = sqlInsertComm.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            recordId = int.Parse(r.GetValue(r.GetOrdinal("id")).ToString());
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                string updatesql = "update exportQueue set[status] = 0 where id =" + recordId;


                var workbook = new XLWorkbook();
                var tmp = fileName.Split('.');
                var tabName = tmp[0];
                var worksheet = workbook.Worksheets.Add(tabName);

                for (int i = 0; i < names.Count; i++)
                {
                    worksheet.Cell(1, names[i].propPosition).Value = names[i].propName;
                }
                int j = 1;
                foreach (var data in exportData)
                {

                    //  foreach (var a in data.GetType().GetProperties())
                    //{
                    if (data != null)
                    {
                        j++;
                        foreach (var name in names)
                        {
                            worksheet.Cell(j, name.propPosition).Value = GetPropValue(data, name.propValue);//
                        }
                    }

                    //}
                }
                workbook.Worksheets.Delete(tabName);
                workbook.AddWorksheet(worksheet);

                try
                {
                    var stream1 = new MemoryStream();

                    string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~\export\"), fileName);
                    using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        workbook.SaveAs(fileStream, new SaveOptions() { ValidatePackage = true });
                    }
                }
                catch (Exception ex) { throw ex; }

                try
                {
                    SqlCommand updateSql = new SqlCommand(updatesql, sqlCon);
                    SqlDataReader u = updateSql.ExecuteReader();
                }
                catch (Exception ex) { throw ex; }

            }
            return "success";

        }


        //public class pp1
        //{
        //    public string p1 { get; set; }
        //    public string p2 { get; set; }
        //    public string p3 { get; set; }
        //    public string p4 { get; set; }
        //}


        public static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }


        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

    }
}
