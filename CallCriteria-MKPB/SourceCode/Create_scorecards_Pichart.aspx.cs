using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;


public partial class Create_scorecards_Pichart : System.Web.UI.Page
{


    static SqlConnection contion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
    static SqlCommand cmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    SqlDataReader rd;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.Name == "" || User.Identity.Name == null)
        {
            Response.Redirect("login.aspx?ReturnURL=Create_scorecards_Pichart.aspx");
        }
        if (!System.Web.Security.Roles.IsUserInRole("Admin") && !System.Web.Security.Roles.IsUserInRole("Manager") && !System.Web.Security.Roles.IsUserInRole("Supervisor") && !System.Web.Security.Roles.IsUserInRole("Calibrator") && !System.Web.Security.Roles.IsUserInRole("QA Lead") && !System.Web.Security.Roles.IsUserInRole("Client"))
        {
            Response.Redirect("login.aspx?ReturnURL=Create_scorecards_Pichart.aspx");
        }
        if (!IsPostBack)
        {
            DropDownList3.Items.Clear();
            for (int i = 2014; i <= DateTime.Now.Year; i++)
            {
                DropDownList3.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            Gridfill();
        }
    }
    public Create_scorecards_Pichart()
    {
       
        cmd.Connection = contion;
        cmd.CommandType = CommandType.Text;
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

    public void Gridfill()
    {
        string dropval = scrrcrdid.Value;
        string mnth = "03";
        string year = "2016";
        string gridbind = "";
        //if (dropval == "All")
        //{
        //    //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";

        //    gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
        //}
        //else
        //{
        //    //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where agent like '%" + dropval + "%' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";
        //    gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent ='" + dropval + "' and total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where agent = '" + dropval + "' and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and agent is not null and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
        //}
        ////////////
        if (dropval == "All" && hdnscorecardvalue.Value == "All")
        {

            gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
        }
        else
        {
            gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80";
            if (dropval != "All")
            {
                gridbind = gridbind + " and agent ='" + dropval + "'";
            }
            if (hdnscorecardvalue.Value != "All")
            {
                gridbind = gridbind + "and scorecard='" + hdnscorecardvalue.Value + "'";
            }
            gridbind = gridbind + " and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80";
            if (dropval != "All")
            {
                gridbind = gridbind + " and agent = '" + dropval + "'";
            }
            if (hdnscorecardvalue.Value != "All")
            {
                gridbind = gridbind + " and scorecard='" + hdnscorecardvalue.Value + "'";
            }
            gridbind = gridbind + "and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "'";
            if (dropval != "All")
            {
                gridbind = gridbind + " and agent = '" + dropval + "'";
            }
            if (hdnscorecardvalue.Value != "All")
            {
                gridbind = gridbind + " and scorecard='" + hdnscorecardvalue.Value + "'";
            }            
            gridbind = gridbind+" and agent is not null and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
        }
        ///////////////
        DataTable dt = getdata(gridbind);
        //GridViewcomplains.DataSource = dt;
        //GridViewcomplains.DataBind();

        //string gg = "select reviewer,call_date,num_missed,audio_link,First_Name,Last_Name,Call_Date from vwform where agent ='Academix Direct Inc.' and total_score>80 and appname='edufficient' and month(call_date)='03' and year(call_date)='2016'";
        string gg = "select F_id,reviewer,agent,scorecardname=(select distinct s.short_name from xcc_report_new x join scorecards s on s.id = x.scorecard where s.id=v.scorecard),agent_group,CONVERT(CHAR(10),call_date,101) as call_date,website,num_missed,audio_link,First_Name,Last_Name,CONVERT(CHAR(10),review_date,101) as review_date,pass_fail, missed_list,total_score from vwForm v where agent='Academix Direct Inc.' and datepart(month, call_date) = '03' and datepart(year, call_date) ='2016' and appname = 'edufficient'";
        DataTable dtt = getdata(gg);
        GridView2complains.DataSource = dtt;
        GridView2complains.DataBind();

    }

    [WebMethod]
    public static List<Datass> GetData(string dropval, string mnth, string yrval, string scorecarddropdown)
    {
        
         SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";
        //if (dropval == "All")
        //{
        //    cmdstr = "select 'Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + yrval + "'  and appname = 'edufficient' union select 'Non Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=80  and year(call_date)='" + yrval + "' and appname = 'edufficient'";

        //}
        //else
        //{
        //    cmdstr = "select 'Compliant Pages',COUNT(reviewer) from  vwform where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + yrval + "'  and appname = 'edufficient'  union select 'Non Compliant Pages',COUNT(reviewer) from  vwform where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=80  and year(call_date)='" + yrval + "' and appname = 'edufficient'";
        //}
        ///////////////
        
        if (dropval == "All" && scorecarddropdown == "All")
        {

            //cmdstr = "select 'Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + yrval + "'  and appname = 'edufficient' union select 'Non Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=80  and year(call_date)='" + yrval + "' and appname = 'edufficient'";

            cmdstr = "select isnull(sum(case when pass_fail = 'Pass' then 1 else 0 end),0) as 'Compliant Pages',isnull(sum(case when pass_fail = 'Fail' then 1 else 0 end),0) as 'Non Compliant Pages' from vwForm where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) ='" + yrval + "' and appname = 'edufficient'";

        }
        else
        {
            cmdstr = "select isnull(sum(case when pass_fail = 'Pass' then 1 else 0 end),0) as 'Compliant Pages',isnull(sum(case when pass_fail = 'Fail' then 1 else 0 end),0) as 'Non Compliant Pages' from vwForm where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) ='" + yrval + "' and appname = 'edufficient'";


            if (dropval != "All")
            {
                cmdstr = cmdstr + " and agent = '" + dropval + "'";
            }
            if (scorecarddropdown != "All")
            {
                cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
            }
            //cmdstr = "select 'Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "'";

           
            //if (dropval != "All")
            //{
            //    cmdstr = cmdstr + " and agent = '" + dropval + "'";
            //}
            //if (scorecarddropdown != "All")
            //{
            //    cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
            //}
            //cmdstr = cmdstr + " and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + yrval + "' and appname = 'edufficient'  union select 'Non Compliant Pages',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "'";
            //if (dropval != "All")
            //{
            //    cmdstr = cmdstr + " and agent = '" + dropval + "'";
            //}
            //if (scorecarddropdown != "All")
            //{
            //    cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
            //}
            //cmdstr = cmdstr + " and reviewer is not null and review_time is not null and total_score is not null and total_score<=80  and year(call_date)='" + yrval + "' and appname = 'edufficient'";
           
        }
      ////////////////////
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
        List<Datass> dataList = new List<Datass>();
        string cat = "";
        int val = 0;
        foreach (DataRow dtRow in dt.Rows)
        {
            foreach (DataColumn dc in dt.Columns)
            {
               
                cat = dc.ColumnName;
                val = Convert.ToInt32(dtRow[dc]);
                dataList.Add(new Datass(cat, val));                
               
            }
        }
        return dataList;
    }

    [WebMethod]
    public static List<Datass> GetDatainfraction(string dropval, string mnth, string yrval, string scorecarddropdown)
    {

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";
        //if(dropval=="All")
        //{
        //    cmdstr = "select 'Major Infraction',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=-20 and year(call_date)='" + yrval + "' and appname = 'edufficient' union select 'Minor Infraction',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>-20  and year(call_date)='" + yrval + "' and appname = 'edufficient'";
           
        //}
        //else{
        //    cmdstr = "select 'Major Infraction',COUNT(reviewer) from  vwform where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=-20 and year(call_date)='" + yrval + "' and appname = 'edufficient'  union select 'Minor Infraction',COUNT(reviewer) from  vwform where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>-20  and year(call_date)='" + yrval + "' and appname = 'edufficient' ";
        //}

        ///////////////

        if (dropval == "All" && scorecarddropdown == "All")
        {

            //cmdstr = "select 'Major Infraction',COUNT(reviewer) from  vwform v where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and (select min(answer_points) from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered where f_id =v.f_id) <=-20 and year(call_date)='" + yrval + "' and appname = 'edufficient' union select 'Minor Infraction',COUNT(reviewer) from  vwform v where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and (select min(answer_points) from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered where f_id =v.f_id) >-20  and year(call_date)='" + yrval + "' and appname = 'edufficient'";

            cmdstr = @"select case when answer_points <= -20 then 'Major Infraction' else 'Minor Infraction' end,count(*) from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1 and right_answer = 0
       and vwForm.appname='edufficient' and month(vwForm.call_date)='" + mnth + @"' and year(vwForm.call_date)='" + yrval + @"'
      group by case when answer_points <= -20 then 'Major Infraction' else 'Minor Infraction' end";
            

        }
        else
        {
            cmdstr = @"select case when answer_points <= -20 then 'Major Infraction' else 'Minor Infraction' end, count(*) from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1 and right_answer = 0
       and vwForm.appname='edufficient' and month(vwForm.call_date)='" + mnth + @"' and year(vwForm.call_date)='" + yrval + "'";

            if (dropval != "All")
            {
                cmdstr = cmdstr + " and agent = '" + dropval + "'";
            }
            if (scorecarddropdown != "All")
            {
                cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
            }
            cmdstr = cmdstr + " group by case when answer_points <= -20 then 'Major Infraction' else 'Minor Infraction' end";

        }
        ////////////////////
       
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
        List<Datass> dataList = new List<Datass>();
        string cat = "";
        int val = 0;
        foreach (DataRow dr in dt.Rows)
        {
            cat = dr[0].ToString();
            val = Convert.ToInt32(dr[1]);
            dataList.Add(new Datass(cat, val));
        }
        return dataList;
    }


    [WebMethod]
    public static List<Datass> GetDatainfractiondetails(string dropval, string mnth, string yrval, string scorecarddropdown)
    {

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";
        //if (dropval == "All")
        //{
        //    //cmdstr = "SELECT distinct Questions.q_short_name,count(Questions.q_short_name)as countdt FROM Questions  join  vwform on  vwform.scorecard=Questions.scorecard_id where month(vwform.call_date)='" + mnth + "' or year(call_date)='" + yrval + "' and Questions.q_short_name='programs' or Questions.q_short_name='city' or Questions.q_short_name='state' or Questions.q_short_name='disclosures' or Questions.q_short_name='Accreditation' or Questions.q_short_name='Locations' or Questions.q_short_name='Year of High School Graduation' or Questions.q_short_name='content' or Questions.q_short_name='cell phone' group by Questions.q_short_name";

        //    cmdstr = "SELECT distinct short_name,count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where  month(vwform.call_date)='" + mnth + "' and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name ";
        //}
        //else
        //{
        //    //cmdstr = " select  distinct state, count (state) as CountOf  from  vwform where agent like '%" + dropval + "%' and month(call_date)='" + mnth + "' or year(call_date)='" + yrval + "' group by state ";

        //    cmdstr = "SELECT distinct short_name,count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where vwform.agent = '" + dropval + "' and month(vwform.call_date)='" + mnth + "' and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name";
        //}


        ///////////////
        //previousmodification

        //if (dropval == "All" && scorecarddropdown == "All")
        //{

        //    cmdstr = "SELECT distinct short_name,count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where  month(vwform.call_date)='" + mnth + "' and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name ";

        //}
        //else
        //{
        //    cmdstr = "SELECT distinct short_name,count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where month(vwform.call_date)='" + mnth + "'";



        //    if (dropval != "All")
        //    {
        //        cmdstr = cmdstr + " and vwform.agent = '" + dropval + "'";
        //    }
        //    if (scorecarddropdown != "All")
        //    {
        //        cmdstr = cmdstr + " and vwform.scorecard='" + scorecarddropdown + "'";
        //    }
        //    cmdstr = cmdstr + " and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name";
           
        //}
        ////////////////////
        if (dropval == "All" && scorecarddropdown == "All")
        {

            //cmdstr = "select q_short_name,count(*) from vwform v join Questions q on q.scorecard_id=v.scorecard where v.appname='edufficient' and month(call_date)='" + mnth + "' and year(call_date)='" + yrval + "' group by q.q_short_name";
            cmdstr = @"select upper(q_short_name),count(*) as totlacnt from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1  and right_answer = 0 
       and vwForm.appname='edufficient' and month(vwForm.call_date)='" + mnth + @"' and year(vwForm.call_date)='" + yrval + @"' 
       and Questions.section not in (select id from sections where section = 'Compliance Check - Fields')
      group by q_short_name union  select 'FIELD' as q_short_name, 
        isnull((select distinct SUM(COUNT(q_short_name)) OVER() AS total_count from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1  and right_answer = 0 
       and vwForm.appname='edufficient' and month(vwForm.call_date)='" + mnth + @"' and year(vwForm.call_date)='" + yrval + @"' 
       and Questions.section in (select id from sections where section = 'Compliance Check - Fields')
      group by q_short_name),0)  as totlacnt ";


        }
        else
        {
            cmdstr = @"select upper(q_short_name),count(*) as totlacnt from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1  and right_answer = 0 
       and vwForm.appname='edufficient' and month(call_date)='" + mnth + "' and Questions.section not in (select id from sections where section = 'Compliance Check - Fields')";
            if (dropval != "All")
            {
                cmdstr = cmdstr + " and vwForm.agent = '" + dropval + "'";
            }
            if (scorecarddropdown != "All")
            {
                cmdstr = cmdstr + " and vwForm.scorecard='" + scorecarddropdown + "'";
            }
            cmdstr = cmdstr + " and year(call_date)='" + yrval + "' group by q_short_name";
           
            cmdstr = cmdstr + @" union  select 'FIELD' as q_short_name,
            isnull((select distinct SUM(COUNT(q_short_name)) OVER() AS total_count from vwForm join form_q_scores on form_q_scores.form_id = f_id join question_answers on question_answers.id = form_q_scores.question_answered
      join Questions on Questions.id=question_answers.question_id
       where Questions.active=1  and right_answer = 0 
       and vwForm.appname='edufficient' and month(call_date)='" + mnth + "' and Questions.section in (select id from sections where section = 'Compliance Check - Fields')";
            if (dropval != "All")
            {
                cmdstr = cmdstr + " and vwForm.agent = '" + dropval + "'";
            }
            if (scorecarddropdown != "All")
            {
                cmdstr = cmdstr + " and vwForm.scorecard='" + scorecarddropdown + "'";
            }
            cmdstr = cmdstr + " and year(call_date)='" + yrval + "' group by q_short_name),0)  as totlacnt";
        }
        ///////////////////
       
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
        List<Datass> dataList = new List<Datass>();
        string cat = "";
        int val = 0;
        
        foreach (DataRow dr in dt.Rows)
        {
            
            cat = dr[0].ToString();
            val = Convert.ToInt32(dr[1]);
            if (dt.Rows.Count == 1 && val==0 && cat=="FIELD")
            {
                return dataList;
            }
            dataList.Add(new Datass(cat, val));
        }
        return dataList;
    }

    protected void GridViewcomplains_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        if (row.RowType == DataControlRowType.DataRow)
        {
            row.Attributes["ID"] = GridViewcomplains.DataKeys[e.Row.RowIndex].Value.ToString();

        }
    }

    [WebMethod]
    public static List<Data1> GetDatabardetails(string dropval, string mnth, string yrval, string scorecarddropdown)
    {

        SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        con1.Open();
        string cmdstr = "";
        //if (dropval == "All")
        //{
        //    //cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient'),noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
        //    cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";

        //}
        //else
        //{
        //    //cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent = '" + dropval + "'and  total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and  total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient'),noncomplaint=(select COUNT(reviewer) from vwform  where agent = '" + dropval + "'and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where agent = '" + dropval + "'and month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
        //    cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent = '" + dropval + "'and  total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and  total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient'),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where agent = '" + dropval + "'and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where agent = '" + dropval + "'and month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";

        //}

        /////////////////////
        if (dropval == "All" && scorecarddropdown == "All")
        {

            //cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
            cmdstr = "select agent, sum(case when pass_fail = 'Pass' then 1 else 0 end) as pass,sum(case when pass_fail = 'Fail' then 1 else 0 end) as fail, max(review_date) as date_last_checked from vwForm where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and appname = 'edufficient' group by agent";


        }
        else
        {
        //    cmdstr = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80";



        //    if (dropval != "All")
        //    {
        //        cmdstr = cmdstr + " and agent = '" + dropval + "'";
        //    }
        //    if (scorecarddropdown != "All")
        //    {
        //        cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
        //    }
        //    cmdstr = cmdstr + " and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and  total_score is not null and year(call_date)='" + yrval + "' and appname = 'edufficient'),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform where";
        //    if (dropval != "All")
        //    {
        //        cmdstr = cmdstr + " and agent = '" + dropval + "'";
        //    }
        //    if (scorecarddropdown != "All")
        //    {
        //        cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
        //    }
        //    cmdstr = cmdstr + " and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "'";
        //    if (dropval != "All")
        //    {
        //        cmdstr = cmdstr + " and agent = '" + dropval + "'";
        //    }
        //    if (scorecarddropdown != "All")
        //    {
        //        cmdstr = cmdstr + " and scorecard='" + scorecarddropdown + "'";
        //    }
        //    cmdstr = cmdstr + " and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
        }
        /////////////////////////

        SqlCommand cmd = new SqlCommand(cmdstr, con1);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds1);
        dt1 = ds1.Tables[0];
        List<Data1> dataList1 = new List<Data1>();
        int cat = 0;
        int val = 0;
        foreach (DataRow dr1 in dt1.Rows)
        {
            cat = Convert.ToInt32(dr1[1]);
            val = Convert.ToInt32(dr1[2]);
            dataList1.Add(new Data1(cat, val));
        }
        return dataList1;
    }
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}
    protected void Button1_Click1(object sender, EventArgs e)
    {
        
        var TotalHeight = 20;
        System.Drawing.Image img1 = null;
        string filepath1 = "";
        byte[] imageBytes1 = Convert.FromBase64String(Hiddengraphimage1.Value.Replace("data:image/png;base64,", ""));
        var ms1 = new MemoryStream(imageBytes1);
         img1 = System.Drawing.Image.FromStream(ms1);
        filepath1 = Server.MapPath("~/ChartImage/") + "graph1.png";
        img1.Save(filepath1);
        TotalHeight += img1.Height;

        System.Drawing.Image img2 = null;
        string filepath2 = "";
        byte[] imageBytes2 = Convert.FromBase64String(Hiddengraphimage2.Value.Replace("data:image/png;base64,", ""));
        var ms2 = new MemoryStream(imageBytes2);
        img2 = System.Drawing.Image.FromStream(ms2);
        filepath2 = Server.MapPath("~/ChartImage/") + "graph2.png";
        img2.Save(filepath2);
        TotalHeight += img2.Height;

        System.Drawing.Image img3 = null;
        string filepath3 = "";
        byte[] imageBytes3 = Convert.FromBase64String(Hiddengraphimage3.Value.Replace("data:image/png;base64,", ""));
        var ms3 = new MemoryStream(imageBytes3);
        img3 = System.Drawing.Image.FromStream(ms3);
        filepath3 = Server.MapPath("~/ChartImage/") + "graph3.png";
        img3.Save(filepath3);
        TotalHeight += img3.Height;

        if (scrrcrdid.Value != "All" && hdnscorecardvalue.Value == "All")
        {
            System.Drawing.Image img4 = null;
            string filepath4 = "";
            byte[] imageBytes4 = Convert.FromBase64String(Hiddengraphimage4.Value.Replace("data:image/png;base64,", ""));
            var ms4 = new MemoryStream(imageBytes4);
            img4 = System.Drawing.Image.FromStream(ms4);
            filepath4 = Server.MapPath("~/ChartImage/") + "graph4.png";
            img4.Save(filepath4);
            TotalHeight += img4.Height;
        }

        System.Drawing.Image img5 = null;
        string filepath5 = "";
        byte[] imageBytes5 = Convert.FromBase64String(gridviewdetails.Value.Replace("data:image/png;base64,", ""));
        var ms5 = new MemoryStream(imageBytes5);
        img5 = System.Drawing.Image.FromStream(ms5);
        filepath5 = Server.MapPath("~/ChartImage/") + "grid.png";
        img5.Save(filepath5);
        TotalHeight += img5.Height;
        

        var imagedtls1 = Hiddengraphimage1details.Value;
        var imagedtls2 = Hiddengraphimage2details.Value;
        var imagedtls3 = Hiddengraphimage3details.Value;
        string monthdroptext="";
        if (DropDownList2.SelectedValue=="01")
        {
            monthdroptext = "January";
        }
        else if (DropDownList2.SelectedValue == "02")
        {
            monthdroptext = "February";
        }
        else if (DropDownList2.SelectedValue == "03")
        {
            monthdroptext = "March";
        }
        else if (DropDownList2.SelectedValue == "04")
        {
            monthdroptext = "April";
        }
        else if (DropDownList2.SelectedValue == "05")
        {
            monthdroptext = "May";
        }
        else if (DropDownList2.SelectedValue == "06")
        {
            monthdroptext = "June";
        }
        else if (DropDownList2.SelectedValue == "07")
        {
            monthdroptext = "July";
        }
        else if (DropDownList2.SelectedValue == "08")
        {
            monthdroptext = "August";
        }
        else if (DropDownList2.SelectedValue == "09")
        {
            monthdroptext = "September";
        }
        else if (DropDownList2.SelectedValue == "10")
        {
            monthdroptext = "October";
        }
        else if (DropDownList2.SelectedValue == "11")
        {
            monthdroptext = "November";
        }
        else if (DropDownList2.SelectedValue == "12")
        {
            monthdroptext = "December";
        }

        string emailbody = "<html><head></head><body><p style='font-size: 24px;font-weight: bold;'><img src='http://partner.callcriteria.com/images/edufficient.png' width='161'/> EDUFFICIENT GROUP COMPLIANCE REPORT</p><table border='0' cellpadding='0' cellspacing='0' style='width:100%;'><tr style='background-color:black;height:35px;color:white'><td style='width:70%;'>Campaign Reference: <b style='font-size: 18px;font-weight: bold;'>";
        var nameall="";
          var keycode ="";
        if (hdnscorecardtext.Value == "All")
           {
            nameall=" &nbsp;";
           }
        else
        {
            //nameall=scrrcrddrop.SelectedValue;
            nameall = hdnscorecardtext.Value;
        }
        emailbody += nameall + "</b></td><td style='text-align: right;width: 30%;'>Report Month: " + monthdroptext + "&nbsp;" + DropDownList3.SelectedValue + "</td></tr><tr style='background-color:black;height:29px;color:white'><td>Scorecard ID: " + hdnscorecardvalue.Value + "</td><td></td></tr><tr style='height:15px;'><td colspan='2'></td></tr><tr><td colspan='2'><p style='font-size: 24px;font-weight: bold;'>PAGE COMPLIANCE SNAPSHOT: OVERALL</p></td></tr><tr><td style='width:30%'>" + Hiddengraphimage1details.Value + "</td><td style='height: 330px;width:80%;'><img src=\"" + Server.MapPath("~/ChartImage/graph1.png") + "\"  /></td></tr>";
        emailbody += "<tr><td colspan='2'><p style='font-size: 24px;font-weight: bold;'>PAGE COMPLIANCE SNAPSHOT: INFRACTION CATEGORY</p></td></tr><tr><td style='width:20%'>" + Hiddengraphimage2details.Value + "</td><td style='height: 330px;width:80%;'><img src=\"" + Server.MapPath("~/ChartImage/graph2.png") + "\"  /></td></tr>";
        emailbody += "<tr><td colspan='2'><p style='font-size: 24px;font-weight: bold;'>PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL</p></td></tr><tr><td style='width:20%'>" + Hiddengraphimage3details.Value + "</td><td style='height:330px;width:80%;'><img src=\"" + Server.MapPath("~/ChartImage/graph3.png") + "\"  /></td></tr>";
        if (scrrcrdid.Value != "All" && hdnscorecardtext.Value == "All")
        {
            emailbody += "<tr><td colspan='2'><p style='font-size: 24px;font-weight: bold;'>PAGE COMPLIANCE SNAPSHOT: SCORECARD DETAIL</p></td></tr><tr><td style='width:20%'>" + Hiddengraphimage4details.Value + "</td><td style='height: 330px;width:80%;'><img src=\"" + Server.MapPath("~/ChartImage/graph4.png") + "\"  /></td></tr>";
        }

        emailbody += "</table><p style='font-size: 24px;font-weight: bold;'>PAGE COMPLIANCE SNAPSHOT: PARTNER SUMMARIES</p>";
        

       
            if (hdnscorecardtext.Value == "All" && scrrcrdid .Value != "All")
            {
            }
        else
            {
                emailbody += "<table id='colorkeys'style='width:100%;'><tr><td style='background-color:#3399CC;width:2%;'>&nbsp;&nbsp;</td><td style='width:98%;'>COMPLIANT PAGES</td></tr><tr><td style='background-color:#80C65A;width:2%;'>&nbsp;&nbsp;</td><td style='width:98%;'>NON COMPLIANT PAGES</td></tr></table>";

            }
        

          emailbody += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'><tr style='background-color:black;height:29px;color:white'><td style='width:70%;'>Campaign Reference: <b style='font-size: 18px;font-weight: bold;'>" + nameall + "</b></td><td style='text-align: right;width:30%;'>Report Month: " + monthdroptext + "&nbsp;" + DropDownList3.SelectedValue + "</td></tr><tr style='background-color:black;height:29px;color:white'><td>Scorecard ID: " + hdnscorecardvalue.Value + "</td><td></td></tr></table>";
        emailbody += "<img style='width:1500px;' src=\"" + Server.MapPath("~/ChartImage/grid.png") + "\"  /></body></html>";
        
        string path = Server.MapPath("~/ChartImage");

        ////////////////
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //HtmlForm frm = new HtmlForm();
        //GridViewcomplains.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Style.Add("text-decoration", "none");
        //frm.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        //frm.Style.Add("font-size", "42px");
        //frm.Controls.Add(GridViewcomplains);
        //frm.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
       
        ///////////////////

        //export to grid with image<--start-->
        
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //GridViewcomplains.AllowPaging = false;
        //GridViewcomplains.DataBind();
        //GridViewcomplains.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
         

        //<---end--->

        StringReader html = new StringReader(emailbody);
        
        //using (Document document = new Document(PageSize.A0 , 25, 25, 25, 25))
       
        using (Document document = new Document())
        {
            document.SetPageSize(new iTextSharp.text.Rectangle(600f, TotalHeight));
            //var document = new iTextSharp.text.Document(PageSize.A4, 0, 0, 0, 0);
            //document.PageSize = PageSize.A4;

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path + "/" + hdnscorecardtext.Value + "-" + monthdroptext + ".pdf", FileMode.Create));
            document.Open();
            //document.SetMargins(0, 0, 0, 0);

            XMLWorkerHelper.GetInstance().ParseXHtml(
              writer, document, html
            );

          //  HTMLWorker htmlparser = new HTMLWorker(document);
          //  htmlparser.Parse(sr);

          //  XMLWorkerHelper.GetInstance().ParseXHtml(
          //  writer, document, sr
          //);
            
           

        }
     

         //var LoginUserId = 0;
         //  if (Request.IsAuthenticated)
         //      LoginUserId = Convert.ToInt32(User.Identity.Name);          
           var file = hdnscorecardtext.Value + "-" + monthdroptext + ".pdf";
           if (!String.IsNullOrEmpty(file))
           {
               var filename = file;
               var contenttype = "application/pdf";
               Response.Clear();
               Response.ContentType = contenttype;
               Response.AddHeader("content-disposition", "attachment;filename="+file);
               Response.AddHeader("Pragma", "public");
               Response.Cache.SetCacheability(HttpCacheability.NoCache);
               string filePath = Server.MapPath("~/ChartImage/" + file);
               Response.TransmitFile(filePath);
               Response.End();
           }
           //////////////////
           string[] filePaths = Directory.GetFiles(Server.MapPath("~/ChartImage/"));
           foreach (string fp in filePaths)
           {
               FileInfo fi = new FileInfo(fp);
               try
               {
                   if (fi.Exists)
                   {
                       fi.Delete();
                   }
               }
               catch
               {

               }
           }
        /////////////////
        //prev update
        //Response.ContentType = "application/pdf";

        //Response.AddHeader("content-disposition", "attachment;filename=Export.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //HtmlForm frm = new HtmlForm();
        //GridViewcomplains.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Style.Add("text-decoration", "none");
        //frm.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        //frm.Style.Add("font-size", "8px");
        //frm.Controls.Add(GridViewcomplains);
        //frm.RenderControl(hw);

        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();      
       
    }   
   
    
    [WebMethod]
    public static string Getgridonclick(string dropval, string mnth, string yrval, string qshortname, string scorecarddropdown)
    {
        string query = "";
        if(qshortname=="")
        {
            //if (dropval == "All")
            //{
            //    //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";

            //    query = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
            //}
            //else
            //{
            //    //query = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent ='BackToLearn Media LLC' and total_score>80 and month(call_date)='03' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='2016' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where agent = 'BackToLearn Media LLC' and total_score<=80 and  month(call_date)='03' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='2016' and appname = 'edufficient') from  vwform w where agent = 'BackToLearn Media LLC' and month(call_date)='03' and agent is not null and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='2016' and appname = 'edufficient' order by agent";
            //    //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where agent like '%" + dropval + "%' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";
            //    query = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent ='" + dropval + "' and total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where agent = '" + dropval + "' and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and agent is not null and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
            //}
            ////////////////////////
            if (dropval == "All" && scorecarddropdown == "All")
            {

                //query = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";
                query = "select agent, sum(case when pass_fail = 'Pass' then 1 else 0 end) as pass,sum(case when pass_fail = 'Fail' then 1 else 0 end) as fail,CONVERT(CHAR(10),max(review_date),101) as date_last_checked from vwForm where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and appname = 'edufficient' group by agent";
            }
            else
            {
                /////////////
                if (dropval != "All" && scorecarddropdown == "All")
                {
                    //query = "select reviewer,CONVERT(CHAR(10),call_date,101) as call_date,num_missed,audio_link,First_Name,Last_Name,CONVERT(CHAR(10),Call_Date,101) as Call_Date from vwform where agent ='" + dropval + "' and total_score>80 and appname='edufficient' and month(call_date)='" + mnth + "' and year(call_date)='" + yrval + "'";
                    query = "select F_id,reviewer,agent,scorecardname=(select distinct s.short_name from xcc_report_new x join scorecards s on s.id = x.scorecard where s.id=v.scorecard),agent_group,CONVERT(CHAR(10),call_date,101) as call_date,website,num_missed,audio_link,First_Name,Last_Name,CONVERT(CHAR(10),review_date,101) as review_date,pass_fail, missed_list,total_score from vwForm v where agent='" + dropval + "' and datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) ='" + yrval + "' and appname = 'edufficient'";

                }else
                {
                    ////////////////////
                    //query = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80";
                    query = "select agent, sum(case when pass_fail = 'Pass' then 1 else 0 end) as pass,sum(case when pass_fail = 'Fail' then 1 else 0 end) as fail,CONVERT(CHAR(10),max(review_date),101) as date_last_checked from vwForm where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and appname = 'edufficient' ";
                    if (dropval != "All")
                    {
                        query = query + " and agent ='" + dropval + "'";
                    }
                    if (scorecarddropdown != "All")
                    {
                        query = query + " and scorecard='" + scorecarddropdown + "'";
                    }
                    query = query + "group by agent";

                    //if (dropval != "All")
                    //{
                    //    query = query + " and agent ='" + dropval + "'";
                    //}
                    //if (scorecarddropdown != "All")
                    //{
                    //    query = query + " and scorecard='" + scorecarddropdown + "'";
                    //}
                    //query = query + " and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient'),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80";
                    //if (dropval != "All")
                    //{
                    //    query = query + " and agent = '" + dropval + "'";
                    //}
                    //if (scorecarddropdown != "All")
                    //{
                    //    query = query + " and scorecard='" + scorecarddropdown + "'";
                    //}
                    //query = query + " and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "'";
                    //if (dropval != "All")
                    //{
                    //    query = query + " and agent = '" + dropval + "'";
                    //}
                    //if (scorecarddropdown != "All")
                    //{
                    //    query = query + " and scorecard='" + scorecarddropdown + "'";
                    //}
                    //query = query + " and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' order by agent";

                }
               
            }
            /////////////////////
        }else
        {
            //previous update//
            //query = " select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w join scorecards on w.scorecard=scorecards.id where month(w.call_date)='" + mnth + "' and w.agent is not null and w.reviewer is not null and w.review_time is not null and w.total_score is not null  and year(w.call_date)='" + yrval + "' and w.appname = 'edufficient' and scorecards.short_name='" + qshortname + "'order by agent";
             query = " select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + yrval + "' and appname = 'edufficient' ),CONVERT(CHAR(5),week_ending_date,101) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null and year(call_date)='" + yrval + "' and appname = 'edufficient') from  vwform w join scorecards on w.scorecard=scorecards.id join Questions on Questions.scorecard_id=w.scorecard where month(w.call_date)='" + mnth + "' and w.agent is not null and w.reviewer is not null and w.review_time is not null and w.total_score is not null  and year(w.call_date)='" + yrval + "' and w.appname = 'edufficient' and Questions.q_short_name='" + qshortname + "'order by agent";
                      
        }
        //string query = " select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='"+mnth+"' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='"+yrval+"' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='"+mnth+"' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null and year(call_date)='"+yrval+"' and appname = 'edufficient') from  vwform w join scorecards on w.scorecard=scorecards.id where month(w.call_date)='"+mnth+"' and w.agent is not null and w.reviewer is not null and w.review_time is not null and w.total_score is not null  and year(w.call_date)='"+yrval+"' and w.appname = 'edufficient' and scorecards.short_name='"+qshortname+"'order by agent";
        SqlCommand cmd = new SqlCommand(query);
        return GetData(cmd).GetXml();
    }
    private static DataSet GetData(SqlCommand cmd)
    {
        string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    return ds;

                }
            }
        }
    }



    [WebMethod]
    public static string Getgridoninfractgraphclick(string dropval, string mnth, string yrval, string qshortname, string scorecarddropdown)
    {
        string query = "";
        //if (qshortname == "Minor Infraction")
        //{
        //    if (dropval == "All" && scorecarddropdown == "All")
        //    {

        //        query = "select distinct agent,, from  vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score<=-20 and year(call_date)='" + yrval + "' and appname = 'edufficient'";

        //    }
        //    else
        //    {
        //        query = "select 'Major Infraction',COUNT(reviewer) from  vwform where month(call_date)='" + mnth + "'";


        //        if (dropval != "All")
        //        {
        //            query = query + " and agent = '" + dropval + "'";
        //        }
        //        if (scorecarddropdown != "All")
        //        {
        //            query = query + " and scorecard='" + scorecarddropdown + "'";
        //        }
        //        query = query + " and reviewer is not null and review_time is not null and total_score is not null and total_score<=-20 and year(call_date)='" + yrval + "' and appname = 'edufficient'  union select 'Minor Infraction',COUNT(reviewer) from  vwform where  month(call_date)='" + mnth + "'";
        //        if (dropval != "All")
        //        {
        //            query = query + " and agent = '" + dropval + "'";
        //        }
        //        if (scorecarddropdown != "All")
        //        {
        //            query = query + " and scorecard='" + scorecarddropdown + "'";
        //        }
        //        query = query + " and reviewer is not null and review_time is not null and total_score is not null and total_score>-20 and year(call_date)='" + yrval + "' and appname = 'edufficient'";

        //    }
        //}
        //else if (qshortname == "Major Infraction")
        //{
        //    query = "";
        //}
        SqlCommand cmd = new SqlCommand(query);
        return GetData(cmd).GetXml();
    }

     [WebMethod]
    public static List<Datass> Getgraphfour(string dropval, string mnth, string yrval, string scorecarddropdown)
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";
        if (dropval == "All")
        {
            //cmdstr = "SELECT distinct Questions.q_short_name,count(Questions.q_short_name)as countdt FROM Questions  join  vwform on  vwform.scorecard=Questions.scorecard_id where month(vwform.call_date)='" + mnth + "' or year(call_date)='" + yrval + "' and Questions.q_short_name='programs' or Questions.q_short_name='city' or Questions.q_short_name='state' or Questions.q_short_name='disclosures' or Questions.q_short_name='Accreditation' or Questions.q_short_name='Locations' or Questions.q_short_name='Year of High School Graduation' or Questions.q_short_name='content' or Questions.q_short_name='cell phone' group by Questions.q_short_name";

            cmdstr = "SELECT distinct upper(short_name),count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where  month(vwform.call_date)='" + mnth + "' and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name ";
        }
        else
        {
            //cmdstr = " select  distinct state, count (state) as CountOf  from  vwform where agent like '%" + dropval + "%' and month(call_date)='" + mnth + "' or year(call_date)='" + yrval + "' group by state ";

            cmdstr = "SELECT distinct upper(short_name),count(short_name)as countdt FROM vwform  join scorecards  on  vwform.scorecard=scorecards.id where vwform.agent = '" + dropval + "' and month(vwform.call_date)='" + mnth + "' and year(vwform.call_date)='" + yrval + "' and reviewer is not null and review_time is not null and total_score is not null and scorecards.appname = 'edufficient' group by short_name";
        }
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
        List<Datass> dataList = new List<Datass>();
        string cat = "";
        int val = 0;
        foreach (DataRow dr in dt.Rows)
        {
            cat = dr[0].ToString();
            val = Convert.ToInt32(dr[1]);
            dataList.Add(new Datass(cat, val));
        }
        return dataList;
    }

    ///////////////

    //protected void scrrcrddrop_SelectedIndexChanged1(object sender, EventArgs e)
    //{
    //    //string dropval=scrrcrddrop.SelectedValue.ToString();
    //    //string mnth=DropDownList2.SelectedValue.ToString();
    //    //string yrval=DropDownList3.SelectedValue.ToString();
    //    //string qshortname="";
    //    //Getgridonclick(dropval,mnth,yrval,qshortname);
    //    string dropval = scrrcrddrop.SelectedValue.ToString();
    //    string mnth = DropDownList2.SelectedValue.ToString();
    //    string year = DropDownList3.SelectedValue.ToString();
    //    string gridbind = "";
    //    if (dropval == "All")
    //    {
    //        //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";

    //        gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where month(call_date)='" + mnth + "' and agent is not null and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
    //    }
    //    else
    //    {
    //        //gridbind = "select agent,COUNT(reviewer) as 'complaintpage', CONVERT(CHAR(7),week_ending_date,111) as week_ending_date from vwform where agent like '%" + dropval + "%' and month(call_date)='" + mnth + "' and reviewer is not null and review_time is not null and total_score is not null and total_score>80 and year(call_date)='" + year + "' group by agent,week_ending_date";
    //        gridbind = "select distinct agent,complaint=(select COUNT(agent) from vwform where agent ='" + dropval + "' and total_score>80 and month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' ),CONVERT(CHAR(7),week_ending_date,111) as week_ending_date,noncomplaint=(select COUNT(reviewer) from vwform  where agent = '" + dropval + "' and total_score<=80 and  month(call_date)='" + mnth + "' and agent=w.agent and reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient') from  vwform w where agent = '" + dropval + "' and month(call_date)='" + mnth + "' and agent is not null and  reviewer is not null and review_time is not null and total_score is not null  and year(call_date)='" + year + "' and appname = 'edufficient' order by agent";
    //    }

    //    DataTable dt = getdata(gridbind);
    //    GridViewcomplains.DataSource = dt;
    //    GridViewcomplains.DataBind();

    //}


    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList3.DataBind();
    }

    [WebMethod]
    public static List<string> bindagentdropdown(string mnth, string yrval, string scorecarddropdown)
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";
        

        if(scorecarddropdown=="All")
        {
            cmdstr = "select distinct agent from vwform where agent is not null and agent !='' and datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and appname = 'edufficient' order by agent";
        }
        else
        {
            cmdstr = "select distinct agent from xcc_report_new where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and scorecard = '" + scorecarddropdown + "'";
        }
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        dt = ds.Tables[0];
        List<string> dataList = new List<string>();
        string cat = "";
       
        foreach (DataRow dr in dt.Rows)
        {
            cat = dr[0].ToString();
           
            dataList.Add(cat);
        }
        return dataList;

    }

    [WebMethod]
    public static List<Datass> bindscorecarddropdown(string mnth, string yrval) 
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["estomes2ConnectionString"].ConnectionString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        con.Open();
        string cmdstr = "";

        cmdstr = "select distinct short_name, scorecards.ID from xcc_report_new join scorecards on scorecards.id = scorecard join userapps on userapps.user_scorecard = scorecards.id where datepart(month, call_date) = '" + mnth + "' and datepart(year, call_date) = '" + yrval + "' and scorecards.appname = 'edufficient' and userapps.username = '" + HttpContext.Current.User.Identity.Name + "' order by short_name";
 
        SqlCommand cmd = new SqlCommand(cmdstr, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        
        dt = ds.Tables[0];
        List<Datass> dataList = new List<Datass>();
        string cat = "";
        int val = 0;
        foreach (DataRow dr in dt.Rows)
        {
            cat = dr[0].ToString();
            val = Convert.ToInt32(dr[1]);
            dataList.Add(new Datass(cat, val));
        }
        return dataList;

    }

}

public class Data1
{
    public int firstvalue = 0;
    public int secondvalue = 0;
    public Data1(int valuenew1, int valuenew2)
    {
        firstvalue = valuenew1;
        secondvalue = valuenew2;

    }
}
public class Datass
{
    public string ColumnName = "";
    public int Value = 0;
    public Datass(string columnName, int value)
    {
        ColumnName = columnName;
        Value = value;
    }
}