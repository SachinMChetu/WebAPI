using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Services;

public partial class Completed_Calls : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
    static SqlCommand cmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    static SqlDataReader rd;
   
   
    public Completed_Calls()
    {
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        //var xx= User.Identity.Name;

        


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
            con.Close();
            return ex.Message;
        }
        finally
        {
            con.Close();
        }
    }
    public DataTable getdata(string qry)
    {        
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        cmd.Parameters.Clear();
        cmd.CommandText = qry;
        da.SelectCommand = cmd;
        da.Fill(dt);
        return dt;

    }
    public static string getsingledata(string qry)
    {
        
        try
        {
           
            con.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = qry;
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                string a = rd.GetValue(0).ToString();
                return a;
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            rd.Close();
            return ex.Message;
        }
        finally
        {
            rd.Close();
            con.Close();
        }
    }

    public void GridBind(int pageindex, string sortexp, string sortdirection, int rowsinpage)
    {
        int fromrow = ((pageindex - 1) * rowsinpage) + 1;
        //int fromrow = ((pageindex - 1) * 50) + 1;
        //if(sortexp==null)
        //{
        //    sortexp = "review_date";
        //}
        //if(sortdirection==null)
        //{
        //    sortdirection = "desc";
        //}
        var datefrom = txtDatefrom.Text.ToLower().ToString();
        var dateto = txtDateTo.Text.ToLower().ToString();
        var scorecard = txtscorecard.SelectedValue.Trim().ToLower(); 
        //var qa = txtqa.SelectedValue.Trim().ToLower(); 
        var qa = HiddenQAs.Value; 
        //var scorecardquest = txtscorecardquest.Text.Trim().ToLower(); 
        var efficnum = txtefficnum.Text.Trim().ToLower();
        //var missedpoints = txtmissedpoints.Text.Trim().ToLower();
        //var missedpoints = txtmissedpoints.SelectedValue.Trim().ToLower(); 
        var missedpoints = Hiddenmissed.Value;
       
        //string gridbindd = "select top 500 * from vwForm where max_reviews = 1";
        //string gridbindd = "select top 500 *,(select dbo.ConvertTimeToHHMMSS(call_length,'s')) as [call length],CAST(CAST((call_length * 100)/datediff(s, vwForm.review_started, vwForm.review_date) AS numeric(10,2)) AS varchar(10)) + '%'  as [Efficiency] from vwForm where max_reviews = 1";
        string gridbindd = ";WITH RecordsRN AS(select top 5000 *,(select dbo.ConvertTimeToHHMMSS(call_length,'s')) as [call length],CAST(CAST((call_length * 100)/datediff(s, vwForm.review_started, vwForm.review_date) AS numeric(10,2)) AS varchar(10)) + '%'  as [Efficiency],ROW_NUMBER() over(order by " + sortexp + " " + sortdirection + ") as Num  from vwForm where max_reviews = 1";
        string gridbind1 = "select count(*) from vwForm where max_reviews = 1";
        if (datefrom != "")
        {
            //gridbindd = gridbindd + " and CAST(review_started AS DATE)>=('" + datefrom + "')";
            gridbindd = gridbindd + " and CAST(review_date AS DATE)>=('" + datefrom + "')";
            gridbind1 = gridbind1 + " and CAST(review_date AS DATE)>=('" + datefrom + "')";
        }
        if (dateto != "")
        {
            //gridbindd = gridbindd + " and CAST(review_started AS DATE)<=('" + dateto + "')";
            gridbindd = gridbindd + " and CAST(review_date AS DATE)<=('" + dateto + "')";
            gridbind1 = gridbind1 + " and CAST(review_date AS DATE)<=('" + dateto + "')";
        }
        if (scorecard != "")
        {
            gridbindd = gridbindd + " and scorecard ='" + scorecard + "'";
            gridbind1 = gridbind1 + " and scorecard ='" + scorecard + "'";
        }

        if (qa != "")
        {
            gridbindd = gridbindd + " and reviewer='" + qa + "'";
            gridbind1 = gridbind1 + " and reviewer='" + qa + "'";
        }
        //if (scorecardquest != "")
        //{
        //    gridbindd = gridbindd + " and comments like '%" + scorecardquest + "%'";
        //}
        if (efficnum != "")
        {
            gridbindd = gridbindd + " and AGENT_GROUP = '" + efficnum + "'";
            gridbind1 = gridbind1 + " and AGENT_GROUP = '" + efficnum + "'";
        }
        //if(missedpoints!="")
        //{
        //    gridbindd = gridbindd + " and num_missed = '" + missedpoints + "'";
        //    gridbind1 = gridbind1 + " and num_missed = '" + missedpoints + "'";
        //}
        if (missedpoints != "")
        {            
            gridbindd = gridbindd + " and F_ID in (select form_id from form_q_scores where question_id =" + missedpoints + " and question_answered in (select id from question_answers where question_id =" + missedpoints + " and right_answer = 0))";
            gridbind1 = gridbind1 + " and F_ID in (select form_id from form_q_scores where question_id =" + missedpoints + " and question_answered in (select id from question_answers where question_id =" + missedpoints + " and right_answer = 0))";
        }
        //gridbindd = gridbindd + " )SELECT * FROM RecordsRN WHERE Num between " + fromrow + " AND (" + fromrow + "+49) ";
        gridbindd = gridbindd + " )SELECT * FROM RecordsRN WHERE Num between " + fromrow + " AND (" + fromrow + "+" + (rowsinpage - 1) + ") ";
        //gridbindd = gridbindd + " Order by review_date desc";
        DataTable dt = getdata(gridbindd);
        GridViewcall1.DataSource = dt;
        GridViewcall1.DataBind();
        //HiddenQAs.Value = null;
        //Hiddenmissed.Value = null;

        int recordCount = Int32.Parse(getsingledata(gridbind1));
        labelcount.Text = "Your search returns " + recordCount.ToString() + " values";
        if (recordCount>5000)
        {
            recordCount = 5000;
        }
        this.PopulatePager(recordCount, pageindex, rowsinpage);
        if (recordCount < 1000 && recordCount>0)
        {
            btnshowall.Visible = true;
        }
        else
        {
            btnshowall.Visible = false;

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(User.Identity.Name==""||User.Identity.Name==null)
        {
            Response.Redirect("login.aspx?ReturnURL=completed_calls.aspx");
        }
        if (!System.Web.Security.Roles.IsUserInRole("Admin") && !System.Web.Security.Roles.IsUserInRole("Manager") && !System.Web.Security.Roles.IsUserInRole("Supervisor") && !System.Web.Security.Roles.IsUserInRole("Calibrator") && !System.Web.Security.Roles.IsUserInRole("QA Lead") && !System.Web.Security.Roles.IsUserInRole("Client"))
        {
            Response.Redirect("login.aspx?ReturnURL=completed_calls.aspx");      
        }
        
        if(!IsPostBack)
        {

            dsMissedCalib.SelectParameters["username"].DefaultValue = User.Identity.Name;

            hdnIdusrname.Value = User.Identity.Name;
            //string scorecarddrop = "select short_name as scorecard_name, ID, appname from scorecards  where appname in (select appname from userapps where username = '"+User.Identity.Name+"') and active =1  order by appname, short_name";
            string scorecarddrop = "select short_name,ID from scorecards where not short_name ='' and active=1 order by short_name";
            //string scorecarddrop = "select * from scorecards join app_settings on app_settings.appname = scorecards.appname where scorecards.active = 1 and app_settings.active = 1 order by short_name";
            DataTable dts = getdata(scorecarddrop);
            txtscorecard.DataSource = dts;
            txtscorecard.DataBind();

            //string Qadrop = "Select distinct reviewer from vwform order by reviewer";
            //DataTable dtss = getdata(Qadrop);
            //txtqa.DataSource = dtss;
            //txtqa.DataBind();
            
            //string agentgroupbind = "select AGENT_GROUP from vwForm where AGENT_GROUP is not NULL";
            string agentgroupbind = "select distinct AGENT_GROUP from vwForm where (AGENT_GROUP is not NULL) and not AGENT_GROUP='' Order by AGENT_GROUP";
            DataTable dttt = getdata(agentgroupbind);
            txtefficnum.DataSource = dttt;
            txtefficnum.DataBind();

            //string missedcalls = "select distinct num_missed from vwform where not num_missed =''";
            //DataTable dtttt = getdata(missedcalls);
            //txtmissedpoints.DataSource = dtttt;
            //txtmissedpoints.DataBind();

            //string gridbindd = "select top 500 * from vwForm where max_reviews = 1";
            //string gridbindd = " select top 500 *,(select dbo.ConvertTimeToHHMMSS(call_length,'s')) as [call length],CAST(CAST((call_length * 100)/datediff(s, vwForm.review_started, vwForm.review_date) AS numeric(10,2)) AS varchar(10)) + '%'  as [Efficiency] from vwForm where max_reviews = 1 Order by review_date desc";
            //DataTable dt = getdata(gridbindd);
            //GridViewcall1.DataSource = dt;
            //GridViewcall1.DataBind();
            GridBind(1,"review_date","desc",50);
        }
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        GridBind(1,"review_date","desc",50);
    }
    protected void GridViewcall1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        if (row.RowType == DataControlRowType.DataRow)
        {
            row.Attributes["id"] = GridViewcall1.DataKeys[e.Row.RowIndex].Value.ToString();

        }
    }
    //protected void btnnotesubmit_Click(object sender, EventArgs e)
    //{
    //    var commentindividual = txtcommentsspotcheck.Text.Trim().ToLower();
    //    var hiddenfid = hdnfid.Value.ToString();
    //    string reviername = "select reviewer from vwForm where F_ID='" + hiddenfid + "'";
    //    string name = getsingledata(reviername);
    //    string qrystrng = "insert into spotcheck (f_id,checked_by,checked_date,disposition,notes) values('" + hiddenfid + "','" + name + "','" + DateTime.Now + "',' ','" + commentindividual + "')";
    //    //string qrystrng = "update spotcheck set notes='" + commentindividual + "' where f_id='"+hiddenfid+"'";
    //    manipulation(qrystrng);
    //}
    //protected void GridViewcall1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    string d = "delete from vwForm where max_reviews = 1 and F_ID='" + GridViewcall1.DataKeys[e.RowIndex].Value + "'";
    //    manipulation(d);
    //    GridBind();
    //}
    //[System.Web.Services.WebMethod]
    //public static void DeleteCalls(string isting)
    //{
    //    string[] Deleteids = (isting.TrimEnd(',')).Split(new Char[] { ',' });
    //    if (Deleteids != null)
    //    {
    //        foreach (var id in Deleteids)
    //        {
    //            SqlCommand comm = new SqlCommand("delete from vwForm where max_reviews = 1 and F_ID=@ID", con);
    //            comm.Parameters.AddWithValue("@ID", id);
    //            con.Open();
    //            comm.ExecuteNonQuery();
    //            con.Close();
    //        }
    //    }
    //}

    protected void GridViewcall1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewcall1.PageIndex = e.NewPageIndex;
        if (Session["SortedView"] != null)
        {
            GridViewcall1.DataSource = Session["SortedView"];
            GridViewcall1.DataBind();
        }
        else
        {
            GridBind(1, "review_date", "desc",50);
        }  
       
    }
    [System.Web.Services.WebMethod]
    public static void addspottable(string isting,string curtname,string creater)
    {
         
        if(isting!="")
        {
            string[] ids = (isting.TrimEnd(',')).Split(new Char[] { ',' });
            foreach(var id in ids)
            { 
                
                //string reviername = "select reviewer from vwForm where F_ID='" + id + "'";
                //string name = getsingledata(reviername);
                //cmd.CommandText = reviername;
                //con.Open();
                //string name = cmd.ExecuteScalar().ToString();
                //con.Close();
                string qrystrng = "insert into spotcheck (f_id,checked_by,checked_date,disposition,notes1) values('" + id + "','" + creater + "','" + DateTime.Now + "',' ','" + curtname + "')";
                manipulation(qrystrng);  
            }
                      
        }
        
    }
    [System.Web.Services.WebMethod]
    public static void addindividualspottable(string isting, string curtname, string notes)
    {

        if (isting != "")
        {


            string qrystrng = "insert into spotcheck (f_id,checked_by,checked_date,disposition,notes1) values('" + isting + "','" + curtname + "','" + DateTime.Now + "',' ','" + notes + "')";
            manipulation(qrystrng);           

        }
    }
    public SortDirection direction
    {
        get
        {
            if (ViewState["directionState"] == null)
            {
                ViewState["directionState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["directionState"];
        }
        set
        {
            ViewState["directionState"] = value;
        }
    }  

    protected void GridViewcall1_Sorting(object sender, GridViewSortEventArgs e)
    {

        
            string sortingDirection = string.Empty;
            if (direction == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
                sortingDirection = "Desc";

            }
            else
            {
                direction = SortDirection.Ascending;
                sortingDirection = "Asc";

            }
            var pageindexx = GridViewcall1.Rows.Count;
            //string gridbindd = " select top 500 *,(select dbo.ConvertTimeToHHMMSS(call_length,'s')) as [call length],CAST(CAST((call_length * 100)/datediff(s, vwForm.review_started, vwForm.review_date) AS numeric(10,2)) AS varchar(10)) + '%'  as [Efficiency] from vwForm where max_reviews = 1 Order by review_date desc";
        ///////////////////////   
        //var datefrom = txtDatefrom.Text.ToLower().ToString();
        //    var dateto = txtDateTo.Text.ToLower().ToString();
        //    var scorecard = txtscorecard.SelectedValue.Trim().ToLower();
        //    var qa = txtqa.SelectedValue.Trim().ToLower();
        //    //var scorecardquest = txtscorecardquest.Text.Trim().ToLower(); 
        //    var efficnum = txtefficnum.Text.Trim().ToLower();
        //    //var missedpoints = txtmissedpoints.Text.Trim().ToLower();
        //    var missedpoints = txtmissedpoints.SelectedValue.Trim().ToLower();

        //    //string gridbindd = "select top 500 * from vwForm where max_reviews = 1";
        //    string gridbindd = "select top 500 *,(select dbo.ConvertTimeToHHMMSS(call_length,'s')) as [call length],CAST(CAST((call_length * 100)/datediff(s, vwForm.review_started, vwForm.review_date) AS numeric(10,2)) AS varchar(10)) + '%'  as [Efficiency] from vwForm where max_reviews = 1";
        //    if (datefrom != "")
        //    {
        //        //gridbindd = gridbindd + " and CAST(review_started AS DATE)>=('" + datefrom + "')";
        //        gridbindd = gridbindd + " and CAST(review_date AS DATE)>=('" + datefrom + "')";
        //    }
        //    if (dateto != "")
        //    {
        //        //gridbindd = gridbindd + " and CAST(review_started AS DATE)<=('" + dateto + "')";
        //        gridbindd = gridbindd + " and CAST(review_date AS DATE)<=('" + dateto + "')";
        //    }
        //    if (scorecard != "")
        //    {
        //        gridbindd = gridbindd + " and scorecard ='" + scorecard + "'";
        //    }

        //    if (qa != "")
        //    {
        //        gridbindd = gridbindd + " and reviewer='" + qa + "'";
        //    }
        //    //if (scorecardquest != "")
        //    //{
        //    //    gridbindd = gridbindd + " and comments like '%" + scorecardquest + "%'";
        //    //}
        //    if (efficnum != "")
        //    {
        //        gridbindd = gridbindd + " and AGENT_GROUP = '" + efficnum + "'";
        //    }
        //    if (missedpoints != "")
        //    {
        //        gridbindd = gridbindd + " and num_missed = '" + missedpoints + "'";
        //    }
        //    gridbindd = gridbindd + " Order by "+e.SortExpression+" "+sortingDirection;
        ////////////////////
        //DataTable dt = getdata(gridbindd);
        //    //GridViewcall1.DataSource = dt;
        //    //GridViewcall1.DataBind();
        //    DataView sortedView = new DataView(dt);
        //    sortedView.Sort = e.SortExpression + " " + sortingDirection;
        //    Session["SortedView"] = sortedView;
        //    GridViewcall1.DataSource = sortedView;
        //    GridViewcall1.DataBind();
            GridBind(1, e.SortExpression, sortingDirection, pageindexx);
        
    }




    private void PopulatePager(int recordCount, int currentPage, int rowsinpage)
    {
        //double dblPageCount = (double)((decimal)recordCount / decimal.Parse("50"));
        double dblPageCount = (double)((decimal)recordCount / decimal.Parse(rowsinpage.ToString()));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 0)
        {
            pages.Add(new ListItem("First", "1", currentPage > 1));
            for (int i = 1; i <= pageCount; i++)
            {
                pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
            }
            pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
        }
        rptPager.DataSource = pages;
        rptPager.DataBind();
    }
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        this.GridBind(1, "review_date", "desc",50);
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.GridBind(pageIndex, "review_date", "desc",50);
    }
    [System.Web.Services.WebMethod]
    public static List<dropoptions> QaPopulate(string startdate, string enddate)
    {
        string qrry = "Select distinct reviewer from vwform where CAST(review_date AS DATE)>=('" + startdate + "') and CAST(review_date AS DATE)>=('" + enddate + "') order by reviewer";       
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        cmd.Parameters.Clear();
        cmd.CommandText = qrry;
        da.SelectCommand = cmd;
        da.Fill(dt);
        List<dropoptions> dropper = new List<dropoptions>();
        foreach (DataRow x in dt.Rows)
        {
            dropoptions optns = new dropoptions();
            optns.text = x["reviewer"].ToString();
            optns.value = x["reviewer"].ToString();
            dropper.Add(optns);
        }
        return dropper;

    }
    //[System.Web.Services.WebMethod]
    //public static List<dropoptions> missedpopulate(string startdate, string enddate, string username, string scorecard)
    //{
    //    string qrry = "";
    //    if (scorecard == "0000")
    //    {
    //        qrry = "select q_short_name + ' (' + scorecards.short_name + ')' as q_display , questions.ID from questions join userapps on userapps.user_scorecard = questions.scorecard_id join scorecards on scorecards.id = questions.scorecard_id where questions.active = 1 and username = '" + username + "' and scorecards.active = 1 and questions.id in(select question_id from form_q_scores join form_score3 on form_score3.id = form_id where convert(date,review_date) between '" + startdate + "' and '" + enddate + "' )  order by scorecards.appname, q_short_name  ";
    //    }
    //    else
    //    {
    //        qrry = "select q_short_name + ' (' + scorecards.short_name + ')' as q_display , questions.ID from questions join userapps on userapps.user_scorecard = " + scorecard + " join scorecards on scorecards.id = " + scorecard + " where questions.active = 1 and username = '" + username + "' and scorecards.active = 1 and questions.id in(select question_id from form_q_scores join form_score3 on form_score3.id = form_id where convert(date,review_date) between '" + startdate + "' and '" + enddate + "' )  order by scorecards.appname, q_short_name  ";
    //    }

    //    DataTable dt = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();
    //    cmd.Parameters.Clear();
    //    cmd.CommandText = qrry;
    //    da.SelectCommand = cmd;
    //    da.Fill(dt);
    //    List<dropoptions> dropper = new List<dropoptions>();
    //    foreach (DataRow x in dt.Rows)
    //    {
    //        dropoptions optns = new dropoptions();
    //        optns.text = x["q_display"].ToString();
    //        optns.value = x["id"].ToString();
    //        dropper.Add(optns);
    //    }
    //    return dropper;

    //}
    protected void btnshowall_Click(object sender, EventArgs e)
    {
        GridBind(1, "review_date", "desc", 1000);
    }
}
public class dropoptions
{
    public string text;
    public string value;
    public string selected;
}
