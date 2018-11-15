using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class import_fluent :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        FileUpload1.SaveAs(Server.MapPath("/docs/" + FileUpload1.FileName));

        IWorkbook workbook;
        using (FileStream stream = new FileStream(Server.MapPath("/docs/" + FileUpload1.FileName), FileMode.Open, FileAccess.Read))
        {
            workbook = new HSSFWorkbook(stream);
        }

        ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet
        DataTable dt = new DataTable(sheet.SheetName);

        // write header row
        IRow headerRow = sheet.GetRow(0);
        foreach (ICell headerCell in headerRow)
        {
            dt.Columns.Add(headerCell.ToString());
        }

        // write the rest
        int rowIndex = 0;
        foreach (IRow row in sheet)
        {
            // skip header row
            if (rowIndex++ == 0) continue;
            DataRow dataRow = dt.NewRow();
            dataRow.ItemArray = row.Cells.Select(c => c.ToString()).ToArray();
            dt.Rows.Add(dataRow);
        }

        SqlCommand sq = new SqlCommand("finishFluentImport");
        sq.CommandType = CommandType.StoredProcedure;
        sq.Parameters.AddWithValue("fd", dt);

        DataSet ds = getDataSet(sq);

        Response.Redirect("counts.aspx?appname=627");
        //gvTest.DataSource = ds.Tables[0];
        //gvTest.DataBind();

    }


    public DataSet getDataSet(SqlCommand sq)
    {
        DataSet ds = new DataSet();
        SqlConnection cn = new SqlConnection("Server=tcp:callcriteriadb.database.windows.net,1433;Initial Catalog=CC_Prod;Persist Security Info=False;User ID=cc_admin;Password=SJQc9M89em5Gmf55;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;");
        cn.Open();

        try
        {

            sq.Connection = cn;
            SqlDataAdapter sda = new SqlDataAdapter(sq);

            sda.Fill(ds);


        }

        catch (Exception ex)
        {

        }

        cn.Close();
        cn.Dispose();

        return ds;
    }


}