using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class callsource_post :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod(
       Description = "Converts F to C a temperature in " +
       "degrees Fahrenheit to a temperature in degrees Celsius.")]
    public string post_callsource(DataMapper dm, string username )
    {

        string sql = "declare @new_ID int;insert into xcc_report_new (campaign, agent, agent_name, DISPOSITION, agent_group,  appname, scorecard, call_date, timestamp, audio_link, phone, session_id, review_started)";

        sql += "select '" + dm.industry + "',";
        sql += "'" + dm.employee + "',";
        sql += "'" + dm.employeeID + "',";
        sql += "'" + dm.callstatus + "',";
        sql += "'" + dm.assignment + "',";
        sql += "'CallSource',";
        sql += "'524',";
        sql += "'" + DateTime.Today.AddHours(-6).ToShortDateString() + "',";
        sql += "'" + DateTime.Now.AddHours(-6).ToShortDateString() + "',";
        sql += "'" + dm.audio + "',";
        sql += "'" + dm.ani + "',";
        sql += "'" + dm.index + "',";

        sql += "'" + DateTime.Now.AddHours(-6).ToShortDateString() + "'; select @new_ID = scope_identity(); select @new_ID;";

        DataTable new_id = Common.GetTable(sql);

        if (new_id.Rows.Count > 0)
        {
            string x_id = new_id.Rows[0][0].ToString();

            string fs_sql = "declare @new_ID int;   INSERT INTO[dbo].[form_score3] (reviewer, session_id, review_date,[review_ID], appname) ";
            fs_sql += "select '" + username + "' ,";
            fs_sql += "'" + x_id + "' ,";
            fs_sql += "dbo.GetMTdate(),";
            fs_sql += "'" + x_id + "' ,";
            fs_sql += "'CallSource'; select @new_ID = scope_identity();select @new_ID;";

            DataTable fs_ds = Common.GetTable(fs_sql);

            if (fs_ds.Rows.Count > 0)
            {

                string f_id = fs_ds.Rows[0][0].ToString();

                for (int x = 0; x < dm.results.Count; x++)
                {
                    string fqs_sql = "insert into form_q_scores (q_position,form_id, question_id, question_answered, original_question_answered) ";
                    fqs_sql += "select 0,";
                    fqs_sql += f_id + ",";
                    fqs_sql += (x + 11770).ToString() + ",";
                    fqs_sql += "(select id from question_answers where question_id = " + (x + 11770).ToString() + " and answer_text = '" + dm.results[x] + "'),";
                    fqs_sql += "(select id from question_answers where question_id = " + (x + 11770).ToString() + " and answer_text = '" + dm.results[x] + "')";

                    Common.UpdateTable(fqs_sql);
                }
            }
        }
        return "Posted.";
    }


    public class DataMapper
    {
        public string industry;
        public string assignment;
        public string employee;
        public string employeeID;
        public string audio;
        public string callstatus;
        public string ani;
        public string index;
        public List<string> results;
    }

}