using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ChangeINOrder : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
    static SqlCommand cmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    SqlDataReader rd;

    public ChangeINOrder()
    {
       
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string ids = Request.QueryString["ids"];
        string idnum = Request.QueryString["idnum"];
        string[] catid = (ids.TrimEnd(',')).Split(new Char[] { ',' });
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        if (catid != null)
        {
            //count += ((pageno - 1) * 10);
            foreach (var id in catid)
            {
                if (id != "")
                {

                    int Id = Convert.ToInt32(id);
                    string updateorder = "update userapps set user_priority='1' where user= (select username from userextrainfo where ID='" + Id + "') and user_scorecard ='" + idnum + "'";
                    manipulation(updateorder);

                }
            }
            string gridbind = "select  userextrainfo.id, userextrainfo.username, userapps.user_priority, user_role from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard ='" + idnum + "' and user_role = 'QA' and active=1 order by user_priority  ";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.Parameters.Clear();
            cmd.CommandText = gridbind;
            da.SelectCommand = cmd;
            da.Fill(dt);
            str.Append("<tr><th scope='col' style='color:White;background-color:#41637C;'>IN</th></tr>");



            foreach (DataRow dr in dt.Rows)
            {
                str.Append("<tr id='" + dr["id"] + "'><td>" + dr["username"] + " (" + dr["user_priority"] + ")</td></tr>");




            }

        }
        Response.Write(str);
    }
    public static string manipulation(string qry)
    {
        try
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);

            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;
            cmd.ExecuteNonQuery();
            return "success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            con.Close();
        }
    }
}