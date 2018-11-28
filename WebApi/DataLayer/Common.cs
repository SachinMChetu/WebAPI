using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tamir.SharpSsh;
using WebApi.Entities;

namespace WebApi.DataLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class Common
    {
        #region Public Properties
        public static string FFPROBE = @"D:\home\site\wwwroot\ffmpeg\bin\ffprobe.exe";
        public static string FFMPEG = @"D:\home\site\wwwroot\ffmpeg\bin\ffmpeg.exe";
        #endregion

        #region Private Properties
        private static string bucketName;
        private static string keyName;
        private static IAmazonS3 client;
        #endregion 

        #region GetJson
        /// <summary>
        /// GetJson
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetJson(DataTable dt)
        {
            return new JavaScriptSerializer().Serialize(from dr in dt.Rows.Cast<DataRow>() select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]));
        }
        #endregion

        #region Public GetTable
        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable GetTable(SqlCommand cmd)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            try
            {
                cn.Open();
                cmd.Connection = cn;
                var rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                return dt;
            }
            catch (Exception ex)
            {
                Email_Error(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return new DataTable();
        }
        #endregion

        #region Public RunSqlCommands
        /// <summary>
        /// RunSqlCommands
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        public static string RunSqlCommands(SqlCommand[] cmds)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            SqlTransaction trans = null;
            try
            {
                cn.Open();
                trans = cn.BeginTransaction();
                foreach (var cmd in cmds)
                {
                    cmd.Connection = cn;
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return "Success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return ex.Message;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
                cn.Dispose();
            }
        }
        #endregion

        #region Public GetTables
        /// <summary>
        /// GetTables
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataSet GetTables(SqlCommand cmd)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString))
            {
                cn.Open();

                cmd.Connection = cn;
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

                adapter.Fill(ds);

                cn.Close();
            }

            return ds;
        }
        #endregion

        #region Public RunSqlCommand
        /// <summary>
        /// RunSqlCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="rethrow"></param>
        /// <returns></returns>
        public static bool RunSqlCommands(SqlCommand cmd, bool rethrow = true)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);

            try
            {
                cn.Open();
                cmd.Connection = cn;
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                if (rethrow)
                    throw ex;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }

            return false;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataSet getTables(SqlCommand cmd)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString))
            {
                cn.Open();
                cmd.Connection = cn;
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                adapter.Fill(ds);
                cn.Close();
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="rethrow"></param>
        /// <returns></returns>
        public static bool RunSqlCommand(SqlCommand cmd, bool rethrow = true)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            try
            {
                cn.Open();
                cmd.Connection = cn;
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                if (rethrow)
                    throw ex;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return false;
        }

        #region Public GetTable
        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, int debug = 0)
        {
            string sql_start = DateTime.Now.ToString();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);
            cn.Open();
            SqlDataAdapter reply = new SqlDataAdapter(sql, cn);
            reply.SelectCommand.CommandTimeout = 60;
            DataTable dt = new DataTable();
            try
            {
                reply.Fill(dt);
            }
            catch (Exception ex)
            {
                Email_Error(sql + "<br><br>" + ex.Message);
            }
            cn.Close();
            cn.Dispose();
            return dt;
        }
        #endregion

        #region Public UpdateTable
        /// <summary>
        /// UpdateTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        public static void UpdateTable(string sql, int debug = 0)
        {
            string sql_start = DateTime.Now.ToString();
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            try
            {
                SqlCommand reply = new SqlCommand(sql, cn);
                reply.CommandTimeout = 60;
                reply.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("<!--" + sql + "-->" + Strings.Chr(13));
                Email_Error(sql + "<br><br>" + ex.Message);
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion

        #region Public UpdateTable2
        /// <summary>
        /// UpdateTable2
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="debug"></param>
        public static void UpdateTable2(string sql, int debug = 0)
        {
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();
            SqlCommand reply = new SqlCommand(sql, cn);
            reply.CommandTimeout = 500;
            reply.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
        }
        #endregion

        #region Public WriteGVExcelWithNPOI
        /// <summary>
        /// WriteGVExcelWithNPOI
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="extension"></param>
        /// <param name="filename"></param>
        public static void WriteGVExcelWithNPOI(GridView gv, String extension, string filename)
        {
            IWorkbook workbook;
            if (extension == "xlsx")
                workbook = new XSSFWorkbook();
            else if (extension == "xls")
                workbook = new HSSFWorkbook();
            else
                throw new Exception("This format is not supported");
            ISheet sheet1 = workbook.CreateSheet("Sheet 1");
            // make a header row
            IRow row1 = sheet1.CreateRow(0);
            ICellStyle Style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Arial";
            font.Boldweight = (short)FontBoldWeight.Bold;

            Style.SetFont(font);
            int call_id_column = -1;
            for (int j = 0; j <= gv.HeaderRow.Cells.Count - 1; j++)
            {
                ICell cell = row1.CreateCell(j);
                String columnName = gv.HeaderRow.Cells[j].Text;
                if (columnName == "Call ID")
                    call_id_column = j;
                foreach (Control ctl in gv.HeaderRow.Cells[j].Controls)
                {
                    if ((ctl) is HyperLink)
                    {
                        HyperLink hl = (HyperLink)ctl;
                        columnName = hl.Text;
                    }
                    if ((ctl) is LinkButton)
                    {
                        LinkButton hl = (LinkButton)ctl;
                        columnName = hl.Text;
                    }
                }
                cell.SetCellValue(columnName);
            }

            // loops through data
            for (int i = 0; i <= gv.Rows.Count - 1; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j <= gv.Rows[i].Cells.Count - 1; j++)
                {
                    ICell cell = row.CreateCell(j);
                    string cell_text = Regex.Replace(WebUtility.HtmlDecode(gv.Rows[i].Cells[j].Text.Replace("Â", "").Replace("|", "")), "<.*?>", string.Empty).Replace("</a", "");
                    foreach (Control ctl in gv.Rows[i].Cells[j].Controls)
                    {
                        if ((ctl) is HyperLink)
                        {
                            HyperLink hl = (HyperLink)ctl;
                            cell_text = hl.Text;
                        }
                    }
                    if (j == call_id_column)
                        cell_text = "http://" + HttpContext.Current.Request.ServerVariables["server_name"] + "/review_record.aspx?ID=" + cell_text;
                    cell.SetCellValue(cell_text);
                }
            }

            using (var exportData = new MemoryStream())
            {
                HttpContext.Current.Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx")
                {
                    // xlsx file format
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename + ".xlsx"));
                    HttpContext.Current.Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")
                {
                    // xls file format
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename + ".xls"));
                    HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
                }
                HttpContext.Current.Response.End();
            }
        }
        #endregion

        #region Public WriteExcelWithNPOI
        /// <summary>
        /// WriteExcelWithNPOI
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="extension"></param>
        public static void WriteExcelWithNPOI(DataTable dt, String extension)
        {
            IWorkbook workbook;
            if (extension == "xlsx")
                workbook = new XSSFWorkbook();
            else if (extension == "xls")
                workbook = new HSSFWorkbook();
            else
                throw new Exception("This format is not supported");

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            // make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j <= dt.Columns.Count - 1; j++)
            {
                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            // loops through data
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                HttpContext.Current.Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx")
                {
                    // xlsx file format
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xlsx"));
                    HttpContext.Current.Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")
                {
                    // xls file format
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xls"));
                    HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
                }
                HttpContext.Current.Response.End();
            }
        }
        #endregion

        #region Public GV_to_CSV
        /// <summary>
        /// GV_to_CSV
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="filename"></param>
        public static void GV_to_CSV(GridView gv, string filename)
        {
            DataTable user_dt = GetTable("select export_type From userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "'");

            if (user_dt.Rows[0][0].ToString() == "Excel")
            {

                WriteGVExcelWithNPOI(gv, "xlsx", filename);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gv.RenderControl(hw);
                // style to format numbers to string
                string style = @"<style>.textmode{mso-number-format:\@;}</style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(Regex.Replace(WebUtility.HtmlDecode(sw.ToString().Replace("Â", "").Replace("&nbsp;", "")), "<.*?>", string.Empty));
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".csv");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "text/csv";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                int cell_count = 0;
                foreach (TableCell gvc in gv.HeaderRow.Cells)
                {
                    try
                    {
                        if (cell_count == gv.HeaderRow.Cells.Count - 1)
                            HttpContext.Current.Response.Output.Write(Regex.Replace(WebUtility.HtmlDecode(gvc.Text.Replace(",", "")), "<.*?>", string.Empty).Replace("</a", "") + "\n");
                        else
                            HttpContext.Current.Response.Output.Write(Regex.Replace(WebUtility.HtmlDecode(gvc.Text.Replace(",", "")), "<.*?>", string.Empty).Replace("</a", "") + ",");
                    }
                    catch (Exception ex)
                    {
                    }
                    cell_count = cell_count + 1;
                }
                foreach (GridViewRow gvr in gv.Rows)
                {
                    cell_count = 0;
                    foreach (TableCell gvc in gvr.Cells)
                    {
                        string fixed_text = gvc.Text.Replace(",", "");
                        fixed_text = fixed_text.Replace(Strings.Chr(13), '\0');
                        fixed_text = fixed_text.Replace(Strings.Chr(10), '\0');
                        fixed_text = fixed_text.Replace(Strings.Chr(9), '\0');
                        fixed_text = fixed_text.Replace("&nbsp;", "");
                        fixed_text = fixed_text.Replace("Â", "");
                        if (Strings.Len(fixed_text) > 0)
                            fixed_text = StripTagsCharArray(fixed_text);
                        foreach (Control ctl in gvc.Controls)
                        {
                            if ((ctl) is HyperLink)
                            {
                                HyperLink hl = (HyperLink)ctl;
                                fixed_text = hl.Text;
                            }
                        }
                        try
                        {
                            if (cell_count == gvr.Cells.Count - 1)
                                HttpContext.Current.Response.Output.Write(Regex.Replace(WebUtility.HtmlDecode(Strings.Replace(fixed_text, ",", "")), "<.*?>", string.Empty).Replace("</a", "") + "\n");
                            else
                                HttpContext.Current.Response.Output.Write(Regex.Replace(WebUtility.HtmlDecode(Strings.Replace(fixed_text, ",", "")), "<.*?>", string.Empty).Replace("</a", "") + ",");
                        }
                        catch (Exception ex)
                        {
                            if (cell_count == gvr.Cells.Count - 1)
                                HttpContext.Current.Response.Output.Write("\n");
                            else
                                HttpContext.Current.Response.Output.Write(",");
                        }
                        cell_count = cell_count + 1;
                    }
                }
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region Public StripTagsCharArray
        /// <summary>
        /// StripTagsCharArray
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length - 1 + 1];
            int arrayIndex = 0;
            bool inside = false;
            for (int i = 0; i <= source.Length - 1; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex += 1;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        #endregion

        #region Public RemoveHTMLTags
        /// <summary>
        /// RemoveHTMLTags
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHTMLTags(string content)
        {
            var cleaned = string.Empty;
            try
            {
                string textOnly = string.Empty;
                Regex tagRemove = new Regex("<[^>]*(>|$)");
                Regex compressSpaces = new Regex(@"[\s\r\n]+");
                textOnly = tagRemove.Replace(content, string.Empty);
                textOnly = compressSpaces.Replace(textOnly, " ");
                cleaned = textOnly;
            }
            catch
            {
            }
            return cleaned;
        }
        #endregion

        #region Public RunFFMPEG
        /// <summary>
        /// RunFFMPEG
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="OutResult"></param>
        /// <param name="ErrResult"></param>
        /// <returns></returns>
        public static int RunFFMPEG(string arg, ref string OutResult, ref string ErrResult)
        {
            Process myProcess = new Process();
            {
                var withBlock = myProcess.StartInfo;
                withBlock.FileName = FFMPEG;
                withBlock.Arguments = arg;

                // start the process in a hidden window
                withBlock.WindowStyle = ProcessWindowStyle.Hidden;
                withBlock.CreateNoWindow = true;
                withBlock.RedirectStandardOutput = false;
                withBlock.RedirectStandardError = false;
                withBlock.UseShellExecute = false;
            }
            myProcess.Start();
            int num_tries = 0;

            do
            {
                if (!myProcess.HasExited)
                {
                    // Refresh the current process property values.
                    myProcess.Refresh();
                    num_tries += 1;
                }
            }
            while (!myProcess.WaitForExit(1500) & num_tries < 20);
            return 0;
        }
        #endregion

        #region Public Sum_Time_Column
        /// <summary>
        /// Sum_Time_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Sum_Time_Column(GridView gv, int col_num)
        {
            TimeSpan total = new TimeSpan(0, 0, 0);

            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        try
                        {
                            string[] time_parts = gvr.Cells[col_num].Text.Split(':');
                            if (time_parts.Length == 3)
                            {
                                TimeSpan ts = new TimeSpan(Convert.ToInt32(time_parts[0]), Convert.ToInt32(time_parts[1]), Convert.ToInt32(time_parts[2]));
                                total += ts;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                gv.FooterRow.Cells[col_num].Text = System.Convert.ToString(total.TotalHours) + ":" + Strings.Right("0" + Math.Abs(total.Minutes), 2) + ":" + Strings.Right("0" + Math.Abs(total.Seconds), 2);
            }
        }
        #endregion

        #region Public Avg_Time_Column
        /// <summary>
        /// Avg_Time_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Avg_Time_Column(GridView gv, int col_num)
        {
            TimeSpan total = new TimeSpan(0, 0, 0);
            int count_to_avg = 0;
            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        string[] time_parts = gvr.Cells[col_num].Text.Split(':');
                        if (time_parts.Length == 3)
                        {
                            TimeSpan ts = new TimeSpan(Convert.ToInt32(time_parts[0]), Convert.ToInt32(time_parts[1]), Convert.ToInt32(time_parts[2]));
                            total += ts;
                            count_to_avg += 1;
                        }
                    }
                }
                try
                {
                    total = new TimeSpan(total.Ticks / count_to_avg);
                }
                catch (Exception ex)
                {
                }

                gv.FooterRow.Cells[col_num].Text = System.Convert.ToString(total.TotalHours) + ":" + Strings.Right("0" + Math.Abs(total.Minutes), 2) + ":" + Strings.Right("0" + Math.Abs(total.Seconds), 2);
            }
        }
        #endregion

        #region Public Sum_Column
        /// <summary>
        /// Sum_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Sum_Column(GridView gv, int col_num)
        {
            int total = 0;
            int total2 = 0;
            bool isTimestamp = false;
            string has_dollar = "";
            string has_percent = "";
            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        if (gvr.Cells[col_num].Text.IndexOf(":") > -1)
                        {
                            string[] pieces = gvr.Cells[col_num].Text.Split(':');

                            try
                            {
                                total += Convert.ToInt32(pieces[0]) * 3600 + Convert.ToInt32(pieces[1]) * 60 + Convert.ToInt32(pieces[2]);
                            }
                            catch (Exception ex)
                            {
                            }
                            isTimestamp = true;
                        }
                        else
                            try
                            {
                                if (gvr.Cells[col_num].Text.IndexOf("$") > -1)
                                    has_dollar = "$";
                                if (gvr.Cells[col_num].Text.IndexOf("%") > -1)
                                    has_percent = "%";
                                if (gvr.Cells[col_num].Text.IndexOf("/") > -1)
                                {
                                    string[] totals = gvr.Cells[col_num].Text.Split('/');
                                    total += Convert.ToInt32(totals[0]);
                                    total2 += Convert.ToInt32(totals[1]);
                                }
                                else
                                    total += Convert.ToInt32(gvr.Cells[col_num].Text);
                            }
                            catch (Exception ex)
                            {
                            }
                    }
                }
                if (isTimestamp)
                {
                    TimeSpan span = new TimeSpan(TimeSpan.TicksPerSecond * total);
                    gv.FooterRow.Cells[col_num].Text = (System.Convert.ToInt32(total / (double)3600)).ToString("00") + ":" + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00");
                }
                else if (total2 > 0)
                    gv.FooterRow.Cells[col_num].Text = has_dollar + total + has_percent + "/" + has_dollar + total2 + has_percent;
                else
                    gv.FooterRow.Cells[col_num].Text = has_dollar + total + has_percent;
            }
        }
        #endregion

        #region Public Avg_Weighted_Column
        /// <summary>
        /// Avg_Weighted_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        /// <param name="source_weight_col"></param>
        public static void Avg_Weighted_Column(GridView gv, int col_num, int source_weight_col)
        {
            float total = 0;
            float total_weight = 0;
            string add_percent = "";
            bool isTimestamp = false;
            int non_zero_rows = 0;

            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        if (gvr.Cells[col_num].Controls.Count > 0)
                        {
                            HyperLink hl = ((HyperLink)gvr.Cells[col_num].Controls[0]);
                            if (hl != null)
                            {
                                try
                                {
                                    total += Convert.ToInt64(hl.Text) * Convert.ToInt64(gvr.Cells[source_weight_col].Text) / 100;
                                    total_weight += Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                                    non_zero_rows += 1;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else if (gvr.Cells[col_num].Text.IndexOf(":") > -1)
                        {
                            string[] pieces = gvr.Cells[col_num].Text.Split(':');
                            total += (Convert.ToInt64(pieces[0]) * 3600 + Convert.ToInt64(pieces[1]) * 60 + Convert.ToInt64(pieces[2])) * Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                            total_weight += Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                            isTimestamp = true;
                        }
                        else
                        {
                            if (gvr.Cells[col_num].Text.IndexOf("%") > -1)
                                add_percent = "%";
                            if (IsNumeric(gvr.Cells[col_num].Text.Replace("%", "")))
                            {
                                total += Convert.ToInt64(gvr.Cells[col_num].Text.Replace("%", "")) * Convert.ToInt64(gvr.Cells[source_weight_col].Text) / 100;
                                total_weight += Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                                non_zero_rows += 1;
                            }
                            else if (gvr.Cells[col_num].Controls.Count > 0)
                            {
                                if ((gvr.Cells[col_num].Controls[0]) is HyperLink)
                                {
                                    HyperLink hl = (HyperLink)gvr.Cells[col_num].Controls[0];
                                    if (IsNumeric(hl.Text))
                                    {
                                        total += Convert.ToInt64(hl.Text) * Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                                        total_weight += Convert.ToInt64(gvr.Cells[source_weight_col].Text);
                                    }
                                }
                            }
                        }
                    }
                }
                if (isTimestamp)
                {
                    TimeSpan span = new TimeSpan(TimeSpan.TicksPerSecond * Convert.ToInt64(total) / Convert.ToInt64(total_weight));
                    gv.FooterRow.Cells[col_num].Text = (System.Convert.ToInt32(total / (double)total_weight / (double)3600)).ToString("00") + ":" + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00");
                }
                else
                    gv.FooterRow.Cells[col_num].Text = Strings.FormatPercent(total / (double)total_weight, 2) + add_percent;
            }
        }
        #endregion

        #region Public Avg_Column
        /// <summary>
        /// Avg_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Avg_Column(GridView gv, int col_num)
        {
            float total = 0;
            string add_percent = "";
            bool isTimestamp = false;
            int non_zero_rows = 0;
            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        if (gvr.Cells[col_num].Controls.Count > 0)
                        {
                            HyperLink hl = (HyperLink)gvr.Cells[col_num].Controls[0];
                            if (hl != null)
                            {
                                try
                                {
                                    total += Convert.ToInt64(hl.Text);
                                    non_zero_rows += 1;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else if (gvr.Cells[col_num].Text.IndexOf(":") > -1)
                        {
                            string[] pieces = gvr.Cells[col_num].Text.Split(':');
                            total += Convert.ToInt32(pieces[0]) * 3600 + Convert.ToInt32(pieces[1]) * 60 + Convert.ToInt32(pieces[2]);
                            isTimestamp = true;
                        }
                        else
                        {
                            if (gvr.Cells[col_num].Text.IndexOf("%") > -1)
                                add_percent = "%";
                            if (IsNumeric(gvr.Cells[col_num].Text.Replace("%", "")))
                            {
                                total += Convert.ToInt32(gvr.Cells[col_num].Text.Replace("%", ""));
                                non_zero_rows += 1;
                            }
                            else if (gvr.Cells[col_num].Controls.Count > 0)
                            {
                                if ((gvr.Cells[col_num].Controls[0]) is HyperLink)
                                {
                                    HyperLink hl = (HyperLink)gvr.Cells[col_num].Controls[0];
                                    if (IsNumeric(hl.Text))
                                        total += Convert.ToInt64(hl.Text);
                                }
                            }
                        }
                    }
                }
                if (isTimestamp)
                {
                    TimeSpan span = new TimeSpan(Convert.ToInt64(TimeSpan.TicksPerSecond * total));
                    gv.FooterRow.Cells[col_num].Text = (System.Convert.ToInt32(total / (double)3600)).ToString("00") + ":" + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00");
                }
                else if (total / (double)non_zero_rows > 1)
                    gv.FooterRow.Cells[col_num].Text = Strings.FormatNumber(total / (double)non_zero_rows, 2) + add_percent;
                else
                    gv.FooterRow.Cells[col_num].Text = Strings.FormatNumber(total / (double)non_zero_rows, 4) + add_percent;
            }
        }
        #endregion

        #region Public Weighted_Avg_Column
        /// <summary>
        /// Weighted_Avg_Column
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        /// <param name="weight_col"></param>
        public static void Weighted_Avg_Column(GridView gv, int col_num, int weight_col)
        {
            float total = 0;
            float weight_total = 0;
            bool has_percent = false;
            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        if (gvr.Cells[col_num].Text.IndexOf("%") > -1 & !has_percent)
                            has_percent = true;
                        if (IsNumeric(gvr.Cells[col_num].Text.Replace("%", "")))
                        {
                            total += Convert.ToInt64(gvr.Cells[col_num].Text.Replace("%", "")) * Convert.ToInt64(gvr.Cells[weight_col].Text.Replace("%", ""));
                            weight_total += Convert.ToInt64(gvr.Cells[weight_col].Text.Replace("%", ""));
                        }
                    }
                }
                if (has_percent)
                    gv.FooterRow.Cells[col_num].Text = Strings.FormatNumber(total / (double)weight_total, 2) + "%";
                else
                    gv.FooterRow.Cells[col_num].Text = Strings.FormatNumber(total / (double)weight_total, 2);
            }
        }
        #endregion

        #region Public Get_Max
        /// <summary>
        /// Get_Max
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Get_Max(GridView gv, int col_num)
        {
            DateTime max = Convert.ToDateTime("1/1/1900");

            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        try
                        {
                            DateTime col_numdate = Convert.ToDateTime(gvr.Cells[col_num].Text);
                            if (col_numdate > max)
                                max = Convert.ToDateTime(gvr.Cells[col_num].Text);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                gv.FooterRow.Cells[col_num].Text = max.ToShortDateString();
            }
        }
        #endregion

        #region Public Get_Min
        /// <summary>
        /// Get_Min
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="col_num"></param>
        public static void Get_Min(GridView gv, int col_num)
        {
            DateTime min = Convert.ToDateTime("1/1/2900");

            if (gv.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        try
                        {
                            DateTime col_numdate = Convert.ToDateTime(gvr.Cells[col_num].Text);
                            if (col_numdate < min)

                                min = Convert.ToDateTime(gvr.Cells[col_num].Text);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                gv.FooterRow.Cells[col_num].Text = min.ToShortDateString();
            }
        }
        #endregion

        #region Public CheckAddBinPath
        /// <summary>
        /// CheckAddBinPath
        /// </summary>
        public static void CheckAddBinPath()
        {
            // find path to 'bin' folder
            var binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "bin" });
            // get current search path from environment
            var path__1 = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path__1.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path__1 = string.Join(Path.PathSeparator.ToString(), new string[] { path__1, binPath });
                Environment.SetEnvironmentVariable("PATH", path__1);
            }
        }
        #endregion

        #region Public ConvertWavToMp3
        /// <summary>
        /// ConvertWavToMp3
        /// </summary>
        /// <param name="wavFile"></param>
        /// <returns></returns>
        public static MemoryStream ConvertWavToMp3(NAudio.Wave.Wave32To16Stream wavFile)
        {
            CheckAddBinPath();

            using (var retMs = new MemoryStream())
            {
                using (var wtr = new NAudio.Lame.LameMP3FileWriter(retMs, wavFile.WaveFormat, 128))
                {
                    wavFile.CopyTo(wtr);
                    return retMs;
                }
            }
        }
        #endregion

        #region Public ConcatMP3Files_new2
        /// <summary>
        /// ConcatMP3Files_new2
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="final_file_name"></param>
        public static void ConcatMP3Files_new2(string file1, string file2, string final_file_name)
        {
            if (file1 == final_file_name)
            {
                try
                {
                    System.IO.File.Copy(file1, file1.Replace(".mp3", "_temp.mp3"), false);
                    file1 = file1.Replace(".mp3", "_temp.mp3");
                }
                catch (Exception ex)
                {
                }
            }
            if (file2 == final_file_name)
            {
                try
                {
                    System.IO.File.Copy(file2, file2.Replace(".mp3", "_temp.mp3"), false);
                    file2 = file2.Replace(".mp3", "_temp.mp3");
                }
                catch (Exception ex)
                {
                }
            }
            string[] srcFileNames = new[] { file1, file2 };
            string destFileName = Strings.Replace(final_file_name, ".mp3", "_merge.mp3");
            using (Stream destStream = File.OpenWrite(destFileName))
            {
                foreach (string srcFileName in srcFileNames)
                {
                    using (Stream srcStream = File.OpenRead(srcFileName))
                    {
                        srcStream.CopyTo(destStream);
                    }
                }
            }


            NAudio.Wave.Mp3FileReader mpbacground = new NAudio.Wave.Mp3FileReader(file1);
            NAudio.Wave.Mp3FileReader mpMessage = new NAudio.Wave.Mp3FileReader(file2);

            // convert them into wave stream or decode the mp3 file
            NAudio.Wave.WaveStream background = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(mpbacground);
            NAudio.Wave.WaveStream message = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(mpMessage);

            var mixer = new NAudio.Wave.WaveMixerStream32();
            mixer.AutoStop = true;

            var messageOffset = background.TotalTime;
            var messageOffsetted = new NAudio.Wave.WaveOffsetStream(message, TimeSpan.FromSeconds(10), TimeSpan.Zero, message.TotalTime.Subtract(TimeSpan.FromSeconds(30)));

            var background32 = new NAudio.Wave.WaveChannel32(background);
            background32.PadWithZeroes = false;
            // set the volume of background file
            background32.Volume = 0.4F;
            // add stream into the mixer
            mixer.AddInputStream(background32);

            var message32 = new NAudio.Wave.WaveChannel32(messageOffsetted);
            message32.PadWithZeroes = false;
            // set the volume of 2nd mp3 song
            message32.Volume = 0.6F;
            mixer.AddInputStream(message32);
            var wave32 = new NAudio.Wave.Wave32To16Stream(mixer);
            // encode the wave stream into mp3
            var mp3Stream = ConvertWavToMp3(wave32);
            // write mp3 on the disk
            File.WriteAllBytes(Strings.Replace(final_file_name, ".mp3", "_merge.mp3"), mp3Stream.ToArray());
            return;
        }
        #endregion

        #region Public ConcatMP3Files
        /// <summary>
        /// ConcatMP3Files
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="final_file_name"></param>
        /// <returns></returns>
        public static string ConcatMP3Files(string file1, string file2, string final_file_name)
        {
            string output = "";
            string out_error = "";
            final_file_name = Strings.Replace(final_file_name, ".mp3", "_merge.mp3");
            RunFFMPEG("-i " + Strings.Chr(34) + "concat:" + file1 + "|" + file2 + Strings.Chr(34) + " -c copy " + final_file_name + " -y", ref output, ref out_error);
            return final_file_name;
        }
        #endregion

        #region Public ConcatMP3Files_new
        /// <summary>
        /// ConcatMP3Files_new
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="final_file_name"></param>
        public static void ConcatMP3Files_new(string file1, string file2, string final_file_name)
        {
            System.Threading.Thread.Sleep(1000);

            string output = "";
            string out_error = "";
            try
            {
                if (file1.IndexOf(@"\Audio") > -1)
                {
                    if (File.Exists(file1))
                    {
                        if (!File.Exists(file1.Replace(".mp3", "_working.mp3")))
                            File.Move(file1, file1.Replace(".mp3", "_working.mp3"));
                        else
                        {
                            File.Delete(file1.Replace(".mp3", "_working.mp3"));
                            File.Move(file1, file1.Replace(".mp3", "_working.mp3"));
                        }
                    }
                }
                if (file2.IndexOf(@"\Audio") > -1)
                {
                    if (File.Exists(file2))
                    {
                        if (!File.Exists(file2.Replace(".mp3", "_working.mp3")))
                            File.Move(file2, file2.Replace(".mp3", "_working.mp3"));
                        else
                        {
                            File.Delete(file2.Replace(".mp3", "_working.mp3"));
                            File.Move(file2, file2.Replace(".mp3", "_working.mp3"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            RunFFMPEG("-i " + Strings.Chr(34) + "concat:" + file1.Replace(".mp3", "_working.mp3") + "|" + file2.Replace(".mp3", "_working.mp3") + Strings.Chr(34) + " -c copy " + final_file_name + " -y", ref output, ref out_error);
            if (out_error != "")
            {
            }
            if (!File.Exists(final_file_name))
            {
                try
                {
                    File.Move(final_file_name.Replace(".mp3", "_working.mp3"), final_file_name);
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region Public ReGetFile
        /// <summary>
        /// ReGetFile
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="appname"></param>
        public static void ReGetFile(string FileName, string appname)
        {
            string this_session = FileName.Substring(FileName.LastIndexOf(@"\") + 1);
            this_session = Strings.Left(this_session, Strings.Len(this_session) - 4);
            DataTable file_dt = GetTable("select * from WAV_DATA  join XCC_REPORT_NEW  on XCC_REPORT_NEW.SESSION_ID = WAV_DATA.session_id join app_settings  on app_settings.appname = '" + appname + "' where wav_data.session_id = '" + this_session + "'");
            if (file_dt.Rows.Count > 0)
            {
                string call_date = (DateTime.Parse(file_dt.Rows[0]["call_date"].ToString()).ToString().Substring(0, file_dt.Rows[0]["call_date"].ToString().IndexOf(" ")).Replace("/", "_")).ToString().Trim();
                System.Net.WebClient WebClient_down = new System.Net.WebClient();
                WebClient_down.Credentials = new System.Net.NetworkCredential(file_dt.Rows[0]["recording_user"].ToString(), file_dt.Rows[0]["record_password"].ToString(), "");
                // Try
                if (System.IO.File.Exists(@"D:\wwwroot\audio\" + appname + @"\" + call_date + @"\" + this_session + ".wav"))
                    System.IO.File.Delete(@"D:\wwwroot\audio\" + appname + @"\" + call_date + @"\" + this_session + ".wav");
                WebClient_down.DownloadFile(file_dt.Rows[0]["filename"].ToString(), @"D:\wwwroot\audio\" + appname + @"\" + call_date + @"\" + this_session + ".wav");
                while (!!WebClient_down.IsBusy)
                    System.Threading.Thread.Sleep(100);
                string this_filename = @"d:\wwwroot\audio\" + appname + @"\" + call_date + @"\" + this_session + ".wav";
                string output = "";
                string out_error = "";
                string destination_file = @"D:\wwwroot\audio\" + appname + @"\" + call_date + @"\" + this_session + ".mp3";

                RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
            }
        }
        #endregion

        #region Public SendFTP
        /// <summary>
        /// SendFTP
        /// </summary>
        /// <param name="new_file_name"></param>
        /// <param name="local_file"></param>
        /// <param name="ftp_site"></param>
        /// <param name="ftp_login"></param>
        /// <param name="ftp_password"></param>
        /// <param name="thedomain"></param>
        /// <param name="ftp_port"></param>
        /// <returns></returns>
        public static string SendFTP(string new_file_name, string local_file, string ftp_site, string ftp_login, string ftp_password, string thedomain, string ftp_port = "21")
        {
            string value = "";
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftp_site + "/" + new_file_name);
            try
            {
                ftpClient.Credentials = new System.Net.NetworkCredential(ftp_login, ftp_password);
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                ftpClient.UseBinary = true;
                ftpClient.KeepAlive = true;
                System.IO.FileInfo fi = new System.IO.FileInfo(local_file);
                ftpClient.ContentLength = fi.Length;
                byte[] buffer = new byte[4097];
                int bytes = 0;
                int total_bytes = System.Convert.ToInt32(fi.Length);
                System.IO.FileStream fs = fi.OpenRead();
                System.IO.Stream rs = ftpClient.GetRequestStream();
                while (total_bytes > 0)
                {
                    bytes = fs.Read(buffer, 0, buffer.Length);
                    rs.Write(buffer, 0, bytes);
                    total_bytes = total_bytes - bytes;
                }
                // fs.Flush();
                fs.Close();
                rs.Close();
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                value = uploadResponse.StatusDescription;
                uploadResponse.Close();
            }
            catch (Exception ex)
            {
                Email_Error(ex.Message + "<br>" + local_file + " - " + "/upload/" + new_file_name + "<br>" + ftp_site);
                return "";
            }

            ftpClient = null;

            return value + "<br>";
        }
        #endregion

        #region Public SendSFTP
        /// <summary>
        /// SendSFTP
        /// </summary>
        /// <param name="new_file_name"></param>
        /// <param name="local_file"></param>
        /// <param name="ftp_site"></param>
        /// <param name="ftp_login"></param>
        /// <param name="ftp_password"></param>
        /// <param name="ftp_port"></param>
        /// <returns></returns>
        public static string SendSFTP(string new_file_name, string local_file, string ftp_site, string ftp_login, string ftp_password, string ftp_port = "22")
        {
            SftpClient sftp_new = new SftpClient(ftp_site, Convert.ToInt32(ftp_port), ftp_login, ftp_password);
            sftp_new.Connect();
            sftp_new.UploadFile(sftp_new.OpenRead(local_file), "/upload/" + new_file_name);
            sftp_new.Disconnect();
            sftp_new.Dispose();
            Sftp sftp = new Sftp(ftp_site, ftp_login, ftp_password);
            try
            {
                sftp.Connect(22);
                sftp.Put(local_file, "/upload/" + new_file_name);
                sftp.Close();
            }
            catch (Exception ex)
            {
                Email_Error(ex.Message + "<br>" + local_file + " - " + "/upload/" + new_file_name + "<br>" + ftp_site);
                return "";
            }
             
            sftp = null;
            return "Done";
        }
        #endregion

        #region Public SFTPGetFiles
        /// <summary>
        /// SFTPGetFiles
        /// </summary>
        /// <param name="ftp_site"></param>
        /// <param name="ftp_login"></param>
        /// <param name="ftp_password"></param>
        /// <param name="ftp_directory"></param>
        /// <param name="ftp_port"></param>
        /// <returns></returns>
        public static ArrayList SFTPGetFiles(string ftp_site, string ftp_login, string ftp_password, string ftp_directory, string ftp_port = "22")
        {

            IEnumerable<Renci.SshNet.Sftp.SftpFile> files = new List<Renci.SshNet.Sftp.SftpFile>();
            Renci.SshNet.SftpClient sftp_new = new Renci.SshNet.SftpClient(ftp_site.Substring(7), Convert.ToInt32(ftp_port), ftp_login, ftp_password);
            sftp_new.Connect();
            HttpContext.Current.Response.Write(ftp_directory + "<br>");
            files = sftp_new.ListDirectory(ftp_directory);
            HttpContext.Current.Response.Write(sftp_new.IsConnected + "<br>");
            ArrayList files2 = new ArrayList();
            foreach (var sftp_file in files)
            {
                files2.Add(sftp_file.Name);
                HttpContext.Current.Response.Write(sftp_file.Name + "<br>");
            }
            sftp_new.Disconnect();
            sftp_new.Dispose();

            return files2;
        }
        #endregion

        #region Public RunFFPROBE
        /// <summary>
        /// RunFFPROBE
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="OutResult"></param>
        /// <param name="ErrResult"></param>
        /// <returns></returns>
        public static int RunFFPROBE(string arg, ref string OutResult, ref string ErrResult)
        {
            Process myProcess = new Process();
            {
                var withBlock = myProcess.StartInfo;
                withBlock.FileName = FFPROBE;
                withBlock.Arguments = arg;

                // start the process in a hidden window
                withBlock.WindowStyle = ProcessWindowStyle.Hidden;
                withBlock.CreateNoWindow = true;
                withBlock.RedirectStandardOutput = true;
                withBlock.RedirectStandardError = true;
                withBlock.UseShellExecute = false;
            }
            myProcess.Start();
            // Attach to stdout and stderr.
            StreamReader std_out = myProcess.StandardOutput;
            StreamReader std_err = myProcess.StandardError;

            myProcess.WaitForExit(15000);
            // Return results
            OutResult = std_out.ReadToEnd();
            ErrResult = std_err.ReadToEnd();

            // Clean up.
            std_out.Close();
            std_err.Close();

            // In case process is stuck
            if (!myProcess.HasExited)
                myProcess.Kill();
            else
                myProcess.Close();
            return 0;
        }
        #endregion

        #region Public GetMediaDuration
        /// <summary>
        /// GetMediaDuration
        /// </summary>
        /// <param name="MediaFilename"></param>
        /// <returns></returns>
        public static double GetMediaDuration(string MediaFilename)
        {
            string output = "";
            string out_error = "";

            RunFFPROBE("-i " + Strings.Chr(34) + MediaFilename + Strings.Chr(34), ref output, ref out_error);
            int duration_loc = out_error.IndexOf("Duration:");
            string[] new_time = out_error.Substring(duration_loc + 11, 7).Split(':');
            try
            {
                TimeSpan ts = new TimeSpan(Convert.ToInt32(new_time[0]), Convert.ToInt32(new_time[1]), Convert.ToInt32(new_time[2]));
                return ts.TotalSeconds;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
        #endregion

        #region Public ValidateServerCertificate
        /// <summary>
        /// ValidateServerCertificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion

        #region Public GetAudioFileName
        /// <summary>
        /// GetAudioFileName
        /// </summary>
        /// <param name="drv"></param>
        /// <returns></returns>
        public static string GetAudioFileName(DataRow drv)
        {
            // If this scorecard is for website, just return the website

            int theID;

            try
            {
                theID = (int)drv["X_ID"];
            }
            catch (Exception ex)
            {
                theID = (int)drv["ID"];
            }

            DataTable sc_id = GetTable("select *, (select bypass from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "') as bypass from xcc_report_new  join scorecards  on xcc_report_new.scorecard = scorecards.id join app_settings  on app_settings.appname = xcc_report_new.appname where xcc_report_new.id = " + theID);
            if (sc_id.Rows.Count > 0)
            {
                if (sc_id.Rows[0]["bypass"].ToString() == "True" & sc_id.Rows[0]["onAWS"].ToString() == "True")

                    return "http://files.callcriteria.com" + sc_id.Rows[0]["audio_link"].ToString();

                if (sc_id.Rows[0]["onAWS"].ToString() == "True")
                {
                    string audio_link = sc_id.Rows[0]["audio_link"].ToString();
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);



                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();

                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = "callcriteriasingapore" + Strings.Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }

                if (sc_id.Rows[0]["review_type"].ToString() == "website")
                    return "/point1sec.mp3";

                if (sc_id.Rows[0]["stream_only"].ToString() == "True")
                    return sc_id.Rows[0]["audio_link"].ToString();
            }

            if (Strings.Left(drv["audio_link"].ToString(), 6) == "/audio" & (Strings.Right(drv["audio_link"].ToString(), 4) == ".mp3" | Strings.Right(drv["audio_link"].ToString(), 4) == ".mp4" | Strings.Right(drv["audio_link"].ToString(), 4) == ".gsm"))
            {
                try
                {
                    TimeSpan call_len = new TimeSpan(0, 0, Convert.ToInt32(GetMediaDuration("http://files.callcriteria.com" + drv["audio_link"])) / 2);   // tlf.Properties.Duration
                    DateTime call_time = Convert.ToDateTime("12/30/1899") + call_len;
                    UpdateTable("update XCC_REPORT_NEW set call_time = '" + call_time.ToString() + "' where ID = (select review_id from form_score3  where ID = " + theID + ")");
                    UpdateTable("update form_score3 set call_length = '" + call_len.TotalSeconds + "' where ID = " + theID);
                    return "http://files.callcriteria.com" + drv["audio_link"].ToString(); // already converted, send it back
                }
                catch (Exception ex)
                {
                    return "http://files.callcriteria.com" + drv["audio_link"].ToString();
                }
            }

            // thinks it's downloaded, but file's not there, get the orignal file and download again
            if (Strings.Left(drv["audio_link"].ToString(), 6) == "/audio" & (Strings.Right(drv["audio_link"].ToString(), 4) == ".mp3" | Strings.Right(drv["audio_link"].ToString(), 4) == ".mp4" | Strings.Right(drv["audio_link"].ToString(), 4) == ".gsm"))
            {
                // Email_Error("trying to reload " & drv.Item("audio_link").ToString)
                // See if session ID has data
                DataTable wav_dt;
                wav_dt = GetTable("select * from wav_data  where session_id = '" + drv["session_id"] + "'");
                if (wav_dt.Rows.Count == 0)
                    wav_dt = GetTable("select * from wav_data  where filename like '%" + drv["session_id"] + "%'");

                if (wav_dt.Rows.Count > 0)
                    drv["audio_link"] = wav_dt.Rows[0]["filename"].ToString();
            }


            string session_id;

            if (drv["session_id"].ToString().IndexOf(" ") > 0)

                session_id = Strings.Left(drv["session_id"].ToString(), drv["session_id"].ToString().IndexOf(" "));
            else
                session_id = drv["session_id"].ToString();


            string phone = drv["phone"].ToString();

            string this_filename = drv["audio_link"].ToString();


            string call_date = drv["call_date"].ToString().Substring(0, drv["call_date"].ToString().IndexOf(" ")).Replace("/", "_");
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/")))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/"));
            if (drv["audio_link"].ToString() != "")
            {
                if (Strings.Left(drv["audio_link"].ToString(), 4) == "http" | Strings.Left(drv["audio_link"].ToString(), 6) == "/audio" | Strings.Left(drv["audio_link"].ToString(), 3) == "ftp")
                    this_filename = drv["audio_link"].ToString().Replace(" ", "%20");
                else
                    this_filename = drv["url_prefix"] + (drv["audio_link"].ToString().Replace(" ", "%20"));


                string file_ending = Strings.Right(this_filename, 4).ToLower();


                if ((this_filename.IndexOf("@") > -1 | this_filename.IndexOf("http") > -1) & Strings.Left(this_filename, 3) != "ftp" & Strings.Left(this_filename, 4) != "sftp" & Strings.Left(this_filename, 6) != "/audio")
                {
                    // Email_Error(this_filename)
                    System.Net.WebClient WebClient_down = new System.Net.WebClient();

                    WebClient_down.Credentials = new System.Net.NetworkCredential(drv["recording_user"].ToString(), drv["record_password"].ToString(), "");

                    try
                    {
                        WebClient_down.DownloadFile(this_filename, HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending));
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("File not found, refresh page." + this_filename + " to " + HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                        Email_Error(HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                        HttpContext.Current.Response.Redirect("listen.aspx");
                    }


                    while (!!WebClient_down.IsBusy)
                        System.Threading.Thread.Sleep(100);
                }



                if (Strings.Left(this_filename, 6) == "ftp://")
                {
                    WebClient ftpc = new WebClient();
                    ftpc.Credentials = new System.Net.NetworkCredential(drv["audio_user"].ToString(), drv["audio_password"].ToString(), "");

                    try
                    {
                        System.IO.File.Delete(@"d:\wwwroot\audio\" + drv["appname"].ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        ftpc.DownloadFile(this_filename, @"d:\wwwroot\audio\" + drv["appname"].ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv["appname"].ToString() + @"\" + call_date + @"\" + session_id + file_ending + " " + ex.Message + "<br><br><br>");
                        HttpContext.Current.Response.End();
                    }

                    while (!!ftpc.IsBusy)
                        System.Threading.Thread.Sleep(100);
                }

                if (Strings.Left(this_filename, 7) == "sftp://")
                {
                    try
                    {
                        Renci.SshNet.SftpClient sftp_new = new Renci.SshNet.SftpClient(drv["recording_url"].ToString().Substring(7), Convert.ToInt32(drv["audio_port"].ToString()), drv["audio_user"].ToString(), drv["audio_password"].ToString());
                        sftp_new.Connect();

                        System.IO.FileStream dest_file = new System.IO.FileStream(@"d:\wwwroot\audio\" + drv["appname"].ToString() + @"\" + call_date + @"\" + session_id + file_ending, FileMode.OpenOrCreate);

                        sftp_new.DownloadFile(this_filename.Substring(Strings.Len(drv["recording_url"])).Replace("%20", " "), dest_file);
                        sftp_new.Disconnect();
                        sftp_new.Dispose();
                        dest_file.Close();
                        dest_file.Dispose();
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(ex.Message + "<br>");
                        HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv["appname"].ToString() + @"\" + call_date + @"\" + session_id + file_ending + "<br>");
                        HttpContext.Current.Response.Write(this_filename.Substring(Strings.Len(drv["recording_url"])).Replace("%20", " "));
                        HttpContext.Current.Response.End();
                    }
                    int max_wait = 0;
                    while (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending)) | max_wait == 100)
                    {
                        System.Threading.Thread.Sleep(100);
                        max_wait += 1;
                    }
                }
                if (file_ending == ".mp3" | file_ending == ".mp4")
                    this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending);
                else
                {
                    try
                    {
                        this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending);
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                    string output = "";
                    string out_error = "";
                    string destination_file = HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + ".mp3");
                    RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
                    try
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending));
                    }
                    catch (Exception ex)
                    {
                    }
                    file_ending = ".mp3";
                }
                this_filename = "/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending;
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(this_filename)))
                {
                    UpdateTable("update XCC_REPORT_NEW set audio_link = '" + this_filename + "' where ID = " + theID);
                    return this_filename;
                }
                else
                    return drv["audio_link"].ToString();// already an mp3, file exists though doesn't match session ID
            }

            return "";
        }
        #endregion

        #region Public GetAudioFileName
        /// <summary>
        /// GetAudioFileName
        /// </summary>
        /// <param name="drv"></param>
        /// <returns></returns>
        public static string GetAudioFileName(getScorecardData_Result drv)
        {
            // If this scorecard is for website, just return the website

            int theID;

            try
            {
                theID = drv.X_ID;
            }
            catch (Exception ex)
            {
                theID = drv.F_ID;
            }

            DataTable sc_id = GetTable("select *, (select bypass from userextrainfo where username = '" + HttpContext.Current.User.Identity.Name + "') as bypass from xcc_report_new  join scorecards  on xcc_report_new.scorecard = scorecards.id join app_settings  on app_settings.appname = xcc_report_new.appname where xcc_report_new.id = " + theID);
            if (sc_id.Rows.Count > 0)
            {
                if (sc_id.Rows[0]["bypass"].ToString() == "True" & sc_id.Rows[0]["onAWS"].ToString() == "True")

                    return "http://files.callcriteria.com" + sc_id.Rows[0]["audio_link"].ToString();

                if (sc_id.Rows[0]["onAWS"].ToString() == "True")
                {
                    string audio_link = sc_id.Rows[0]["audio_link"].ToString();
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);



                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();

                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = "callcriteriasingapore" + Strings.Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }

                if (sc_id.Rows[0]["review_type"].ToString() == "website")
                    return "/point1sec.mp3";

                if (sc_id.Rows[0]["stream_only"].ToString() == "True")
                    return sc_id.Rows[0]["audio_link"].ToString();
            }

            if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
            {
                try
                {
                    TimeSpan call_len = new TimeSpan(0, 0, Convert.ToInt32(GetMediaDuration("http://files.callcriteria.com" + drv.audio_link)) / 2);   // tlf.Properties.Duration
                    DateTime call_time = Convert.ToDateTime("12/30/1899") + call_len;
                    UpdateTable("update XCC_REPORT_NEW set call_time = '" + call_time.ToString() + "' where ID = (select review_id from form_score3  where ID = " + theID + ")");
                    UpdateTable("update form_score3 set call_length = '" + call_len.TotalSeconds + "' where ID = " + theID);
                    return "http://files.callcriteria.com" + drv.audio_link.ToString(); // already converted, send it back
                }
                catch (Exception ex)
                {
                    return "http://files.callcriteria.com" + drv.audio_link.ToString();
                }
            }

            // thinks it's downloaded, but file's not there, get the orignal file and download again
            if (Strings.Left(drv.audio_link.ToString(), 6) == "/audio" & (Strings.Right(drv.audio_link.ToString(), 4) == ".mp3" | Strings.Right(drv.audio_link.ToString(), 4) == ".mp4" | Strings.Right(drv.audio_link.ToString(), 4) == ".gsm"))
            {
                // Email_Error("trying to reload " & drv.Item("audio_link").ToString)
                // See if session ID has data
                DataTable wav_dt;
                wav_dt = GetTable("select * from wav_data  where session_id = '" + drv.SESSION_ID + "'");
                if (wav_dt.Rows.Count == 0)
                    wav_dt = GetTable("select * from wav_data  where filename like '%" + drv.SESSION_ID + "%'");

                if (wav_dt.Rows.Count > 0)
                    drv.audio_link = wav_dt.Rows[0]["filename"].ToString();
            }


            string session_id;

            if (drv.SESSION_ID.ToString().IndexOf(" ") > 0)

                session_id = Strings.Left(drv.SESSION_ID.ToString(), drv.SESSION_ID.ToString().IndexOf(" "));
            else
                session_id = drv.SESSION_ID.ToString();


            string phone = drv.phone.ToString();

            string this_filename = drv.audio_link.ToString();


            string call_date = drv.call_date.ToString().Substring(0, drv.call_date.ToString().IndexOf(" ")).Replace("/", "_");
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/")))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/"));
            if (drv.audio_link.ToString() != "")
            {
                if (Strings.Left(drv.audio_link.ToString(), 4) == "http" | Strings.Left(drv.audio_link.ToString(), 6) == "/audio" | Strings.Left(drv.audio_link.ToString(), 3) == "ftp")
                    this_filename = drv.audio_link.ToString().Replace(" ", "%20");
                else
                    this_filename = drv.url_prefix + (drv.audio_link.ToString().Replace(" ", "%20"));


                string file_ending = Strings.Right(this_filename, 4).ToLower();


                if ((this_filename.IndexOf("@") > -1 | this_filename.IndexOf("http") > -1) & Strings.Left(this_filename, 3) != "ftp" & Strings.Left(this_filename, 4) != "sftp" & Strings.Left(this_filename, 6) != "/audio")
                {
                    // Email_Error(this_filename)
                    System.Net.WebClient WebClient_down = new System.Net.WebClient();

                    WebClient_down.Credentials = new System.Net.NetworkCredential(drv.recording_user.ToString(), drv.record_password.ToString(), "");

                    try
                    {
                        WebClient_down.DownloadFile(this_filename, HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending));
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("File not found, refresh page." + this_filename + " to " + HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                        Email_Error(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending) + ex.Message);
                        HttpContext.Current.Response.Redirect("listen.aspx");
                    }


                    while (!!WebClient_down.IsBusy)
                        System.Threading.Thread.Sleep(100);
                }

                if (Strings.Left(this_filename, 6) == "ftp://")
                {
                    WebClient ftpc = new WebClient();
                    ftpc.Credentials = new System.Net.NetworkCredential(drv.audio_user.ToString(), drv.audio_password.ToString(), "");

                    try
                    {
                        System.IO.File.Delete(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        ftpc.DownloadFile(this_filename, @"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending + " " + ex.Message + "<br><br><br>");
                        HttpContext.Current.Response.End();
                    }

                    while (!!ftpc.IsBusy)
                        System.Threading.Thread.Sleep(100);
                }

                if (Strings.Left(this_filename, 7) == "sftp://")
                {
                    try
                    {
                        Renci.SshNet.SftpClient sftp_new = new Renci.SshNet.SftpClient(drv.recording_user.ToString().Substring(7), Convert.ToInt32(drv.audio_link.ToString()), drv.audio_user.ToString(), drv.audio_password.ToString());
                        sftp_new.Connect();

                        System.IO.FileStream dest_file = new System.IO.FileStream(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending, FileMode.OpenOrCreate);

                        sftp_new.DownloadFile(this_filename.Substring(Strings.Len(drv.recording_user)).Replace("%20", " "), dest_file);
                        sftp_new.Disconnect();
                        sftp_new.Dispose();
                        dest_file.Close();
                        dest_file.Dispose();
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(ex.Message + "<br>");
                        HttpContext.Current.Response.Write(@"d:\wwwroot\audio\" + drv.appname.ToString() + @"\" + call_date + @"\" + session_id + file_ending + "<br>");
                        HttpContext.Current.Response.Write(this_filename.Substring(Strings.Len(drv.recording_user)).Replace("%20", " "));
                        HttpContext.Current.Response.End();
                    }
                    int max_wait = 0;
                    while (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending)) | max_wait == 100)
                    {
                        System.Threading.Thread.Sleep(100);
                        max_wait += 1;
                    }
                }
                if (file_ending == ".mp3" | file_ending == ".mp4")
                    this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending);
                else
                {
                    try
                    {
                        this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending);
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                    string output = "";
                    string out_error = "";
                    string destination_file = HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + ".mp3");
                    RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
                    try
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending));
                    }
                    catch (Exception ex)
                    {
                    }
                    file_ending = ".mp3";
                }
                this_filename = "/audio/" + drv.appname.ToString() + "/" + call_date + "/" + session_id + file_ending;
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(this_filename)))
                {
                    UpdateTable("update XCC_REPORT_NEW set audio_link = '" + this_filename + "' where ID = " + theID);
                    return this_filename;
                }
                else
                    return drv.audio_link.ToString();// already an mp3, file exists though doesn't match session ID
            }

            return "";
        }
        #endregion

        #region Public UploadEsto
        /// <summary>
        /// UploadEsto
        /// </summary>
        /// <param name="drv"></param>
        /// <param name="theID"></param>
        /// <param name="call_date"></param>
        /// <param name="session_id"></param>
        /// <param name="file_ending"></param>
        /// <returns></returns>
        public static bool UploadEsto(DataRow drv, string theID, string call_date, string session_id, string file_ending)
        {
            // never been uploaded, find schools
            DataTable school_list_dt = GetTable("declare @allsch varchar(100); select @allsch=coalesce(@allsch,'') + origin from school_x_data  where xcc_id = " + theID + "; select @allsch");
            if (school_list_dt.Rows.Count > 0)
            {
                call_date = Strings.Replace(call_date, "/", "_");
                if (file_ending == ".mp3")
                {
                    string this_filename = HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending);
                    string output = "";
                    string out_error = "";
                    string destination_file = HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + ".WAV");
                    RunFFMPEG("-i " + Strings.Chr(34) + this_filename + Strings.Chr(34) + " -b:a 16k -y " + destination_file, ref output, ref out_error);
                    file_ending = ".WAV";
                }
                string full_list = "";
                try
                {
                    full_list = (string)school_list_dt.Rows[0][0];
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(theID + " - no schools<br>");
                    full_list = "MSCCHEG";
                }
                string send_result = "";
                switch (drv["appname"].ToString().ToLower())
                {
                    case "esto":
                        {
                            if (full_list.IndexOf("CC") > -1)
                            {
                                HttpContext.Current.Response.Write("sending esto CC<br>");
                                send_result = SendSFTP((DateTime)drv["call_date"] + "_" + drv["phone"].ToString() + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp-tcpa.quinstreet.com", "stomesca", "sT0m354@", "22") + " - esto/CC";
                            }
                            if (full_list.IndexOf("MS") > -1)
                            {
                                HttpContext.Current.Response.Write("sending esto MS<br>");
                                send_result = SendFTP(drv["phone"].ToString() + "_" + (DateTime)drv["call_date"] + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp://mediaspike.brickftp.com/", "estomes", "2967kWAQsxth", "21") + " - esto/MS";
                            }

                            if (full_list.IndexOf("HEG") > -1)
                            {
                                HttpContext.Current.Response.Write("sending esto HEG<br>");
                                send_result = SendFTP(drv["phone"].ToString() + "_" + (DateTime)drv["call_date"] + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp://ftp.higheredgrowth.com/", "estomes", "yMW$pgWymD9XcjUZ", "21");
                            }

                            break;
                        }

                    case "estobk":
                        {
                            if (full_list.IndexOf("CC") > -1)
                            {
                                HttpContext.Current.Response.Write("sending estobk CC<br>");
                                send_result = SendSFTP((DateTime)drv["call_date"] + "_" + drv["phone"].ToString() + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp-tcpa.quinstreet.com", "stomesnc", "sT0m35nC", "22") + " - estobk/CC";
                            }
                            if (full_list.IndexOf("MS") > -1)
                            {
                                HttpContext.Current.Response.Write("sending estobk MS<br>");
                                send_result = SendFTP(drv["phone"].ToString() + "_" + (DateTime)drv["call_date"] + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp://mediaspike.brickftp.com/", "estomes2", "2vmCus38", "21") + " - estobk/MS";
                            }

                            if (full_list.IndexOf("HEG") > -1)
                            {
                                HttpContext.Current.Response.Write("sending estobk HEG<br>");
                                send_result = SendFTP(drv["phone"].ToString() + "_" + (DateTime)drv["call_date"] + "_" + drv["agent"].ToString() + ".wav", HttpContext.Current.Server.MapPath("/audio/" + drv["appname"].ToString() + "/" + call_date + "/" + session_id + file_ending), "ftp://ftp.higheredgrowth.com/", "estomes", "yMW$pgWymD9XcjUZ", "21");
                            }
                            break;
                        }
                }

                HttpContext.Current.Response.Write(send_result + "<br>");
                UpdateTable("update xcc_report_new set uploaded = dbo.getMTDate() where id = " + theID);
            }

            return true;
        }
        #endregion

        #region Public getBlock
        /// <summary>
        /// getBlock
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getBlock(string lbl, string value)
        {
            if (value != "")
                return "<div><label>" + lbl + ":</label><span>" + value + "</span></div>";
            else
                return "";
        }
        #endregion

        #region Public getSidebarData
        /// <summary>
        /// getSidebarData
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getSidebarData(string lbl, string value)
        {
            if (value != "")
                return " <li><i class='fa fa-angle-right'></i><span>" + lbl + "</span><strong>" + value + "</strong></li>";
            else
                return "";
        }
        #endregion

        #region Public getSidebarData2
        /// <summary>
        /// getSidebarData2
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getSidebarData2(string lbl, string value)
        {
            if (value != "")
                return " <li><span>" + lbl + "</span> : <strong>" + value + "</strong></li>";
            else
                return "";
        }
        #endregion

        #region Public getSidebarData3
        /// <summary>
        /// getSidebarData3
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getSidebarData3(string lbl, string value)
        {
            if (value != "")
            {
                // Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"

                if (lbl == "Website")
                    return " <tr><td class='info-label'>" + lbl + "</td><td class='info-data'><a href='" + value + "' target=_blank>" + value + "</a></td></tr>";
                else
                    return " <tr><td class='info-label'>" + lbl + "</td><td class='info-data'>" + value + "</td></tr>";
            }
            else
                return "";
        }
        #endregion

        #region Public getSSchoolHeader
        /// <summary>
        /// getSSchoolHeader
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getSSchoolHeader(string lbl, string value)
        {
            if (value != "")
                return " <tr><td class='school-name' colspan='2'>" + value + "</td></tr>";
            else
                return "";
        }
        #endregion

        #region Public Email_Error
        /// <summary>
        /// Email_Error
        /// </summary>
        /// <param name="error_text"></param>
        /// <param name="also_email"></param>
        public static void Email_Error(string error_text, string also_email = "")
        {
            if (error_text == "Unexpected end of stream before frame complete" | error_text == "Thread was being aborted." | error_text == "Exception of type 'Tamir.SharpSsh.jsch.SftpException' was thrown.<br>173.10.20.190")
                return;
            // include logo
            StringBuilder body = new StringBuilder();
            body.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:Arial, sans-serif; font-size:12px;\"><tr><td colspan=\"2\" style=\"color:#FF0000; font-weight:bold;\">Please do not reply to this auto-generated email.</td></tr><tr><td colspan=\"2\">&nbsp;</td></tr>");

            body.Append("<tr><td colspan=\"2\"><img src=\"http://app.callcriteria.com/img/cc_words_logo.png\" alt=\"Call Criteria\" width=\"322\" height=\"50\"></td></tr><tr><td colspan=\"2\">&nbsp;</td></tr>");
            HttpContext current = System.Web.HttpContext.Current;
            body.Append("<tr><td>Source Page:</td><td>" + current.Request.Path + "</td></tr>");
            body.Append("<tr><td>Referrer:</td><td>" + current.Request.ServerVariables["HTTP_REFERER"] + "</td></tr>");
            body.Append("<tr><td>Name:</td><td>" + current.User.Identity.Name + "</td></tr>");
            body.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            body.Append("<tr><td colspan=\"2\" style=\"font-weight:bold;\">Error Information</td></tr>");
            body.Append("<tr><td>Browser:</td><td>" + current.Request.ServerVariables["HTTP_USER_AGENT"] + "</td></tr>");
            // body.Append("")
            try
            {
                body.Append("<tr><td>QueryString:</td><td>");
                string[] qs_string = current.Request.QueryString.AllKeys;
                foreach (string qs_str in qs_string)
                    body.Append(qs_str + " - " + current.Request.QueryString[qs_str].ToString() + ", ");
                if (Strings.Right(body.ToString(), 2) == ", ")
                    body.Remove(body.Length - 2, 2);
                body.Append("</td></tr>");
            }
            catch (Exception ex)
            {
            }

            try
            {
                body.Append("<tr><td>Session:</td><td>");
                for (var x = 0; x <= current.Session.Keys.Count - 1; x++)
                    body.Append(current.Session.Keys[x].ToString() + " - " + current.Session[current.Session.Keys[x].ToString()].ToString() + ", ");
                if (Strings.Right(body.ToString(), 2) == ", ")
                    body.Remove(body.Length - 2, 2);
                body.Append("</td></tr>");
            }
            catch (Exception ex)
            {
                body.Append("<tr><td>" + ex.Message + "</td></tr>");
            }

            body.Append("<tr><td>Date and Time:</td><td>" + DateTime.Now.ToString() + "</td></tr>");
            body.Append("<tr><td>Page:</td><td>" + current.Request.ServerVariables["URL"] + "</td></tr>");
            body.Append("<tr><td>Remote Address:</td><td>" + current.Request.ServerVariables["REMOTE_ADDR"] + "</td></tr>");
            body.Append("<tr><td>HTTP Referer:</td><td>" + current.Request.ServerVariables["HTTP_REFERER"] + "</td></tr>");

            body.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            body.Append("<tr><td colspan=\"2\" style=\"font-weight:bold;\">Actual Error</td></tr>");
            body.Append("<tr><td colspan=\"2\">" + error_text + "</td></tr>");
            body.Append("</table>");
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
            cn.Open();

            try
            {
                // If debug Then
                SqlCommand reply2 = new SqlCommand("EXEC send_dbmail  @profile_name='General',  @copy_recipients=@CC,  @recipients='stace@callcriteria.com;chris@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn);
                reply2.Parameters.AddWithValue("Subject_text", "WEB SITE ERROR ");

                reply2.Parameters.AddWithValue("CC", also_email);
                reply2.Parameters.AddWithValue("Body", body.ToString());

                reply2.CommandTimeout = 60;
                reply2.ExecuteNonQuery();
            }
            // End If
            catch (Exception ex)
            {
            }
            cn.Close();
            cn.Dispose();
        }
        #endregion

        #region Public UpperLeft
        /// <summary>
        /// UpperLeft
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string UpperLeft(string theString)
        {
            if (Strings.Len(theString) > 1)
                return Strings.Left(theString, 1).ToUpper() + Strings.Mid(theString, 2, Strings.Len(theString) - 1);
            if (Strings.Len(theString) == 1)
                return theString.ToUpper();
            return "";
        }
        #endregion

        #region Public Properties
        private const long BUFFER_SIZE = 4096;
        #endregion

        #region Public CopyStream
        /// <summary>
        /// CopyStream
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        private static void CopyStream(System.IO.FileStream inputStream, System.IO.Stream outputStream)
        {
            long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
            byte[] buffer = new byte[bufferSize - 1 + 1];
            int bytesRead = 0;
            long bytesWritten = 0;
            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
            while (bytesRead != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesWritten += bufferSize;
            }
        }
        #endregion 

        #region Public IsNumeric
        /// <summary>
        /// IsNumeric
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }
        #endregion
    }
}