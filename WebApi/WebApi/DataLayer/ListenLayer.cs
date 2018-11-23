using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DAL.Models;

namespace WebApi.DataLayer
{
    public class ListenLayer
    {
        /// <summary>
        /// PostListen
        /// </summary>
        /// <param name="LDR"></param>
        /// <returns></returns>
        public string PostListen(ListenDataRequest LDR)
        {
            string user = "cehe_webqa2";


            ListenDataPost LD = LDR.LD;
            List<FormQScores> FQS = LDR.FQS;
            List<FormQResponses> FQR = LDR.FQR;
            List<FormQScoresOptions> FQSO = LDR.FQSO;

            List<SystemComments> SC = LDR.SC;
            List<ClerkedData> CD = LDR.CD;


            DataTable listen_dt = new DataTable();
            listen_dt.Columns.Add("reviewer", Type.GetType("System.String"));
            listen_dt.Columns.Add("session_id", Type.GetType("System.String"));
            listen_dt.Columns.Add("review_ID", Type.GetType("System.Int32"));
            listen_dt.Columns.Add("Comments", Type.GetType("System.String"));
            listen_dt.Columns.Add("appname", Type.GetType("System.String"));
            listen_dt.Columns.Add("whisperID", Type.GetType("System.Int32"));
            listen_dt.Columns.Add("QAwhisper", Type.GetType("System.Int32"));
            listen_dt.Columns.Add("qa_start", Type.GetType("System.Int32"));
            listen_dt.Columns.Add("qa_last_action", Type.GetType("System.Int32"));
            listen_dt.Columns.Add("call_length", Type.GetType("System.Single"));
            listen_dt.Columns.Add("copy_to_cali", Type.GetType("System.Int32"));

            int whisperID = 0;
            int QAwhisper = 0;
            int.TryParse(LD.whisperID, out whisperID);
            int.TryParse(LD.QAwhisper, out QAwhisper);
            DataRow listen_dr = listen_dt.NewRow();
            listen_dr["reviewer"] = user;
            listen_dr["session_id"] = LD.session_id;
            listen_dr["review_ID"] = LD.review_ID;
            listen_dr["Comments"] = LD.Comments;
            listen_dr["appname"] = LD.appname;
            listen_dr["whisperID"] = whisperID;
            listen_dr["QAwhisper"] = QAwhisper;
            listen_dr["qa_start"] = LD.qa_start;
            listen_dr["qa_last_action"] = LD.qa_last_action;
            listen_dr["call_length"] = LD.call_length;
            if (LD.copy_to_cali == false | LD.copy_to_cali == false | LD.copy_to_cali == false)
            {
                listen_dr["copy_to_cali"] = 0;
            }
            else
            {
                listen_dr["copy_to_cali"] = 1;
            }

            listen_dt.Rows.Add(listen_dr);

            DataTable sc_dt = new DataTable();
            sc_dt.Columns.Add("comment_who", Type.GetType("System.String"));
            sc_dt.Columns.Add("comment", Type.GetType("System.String"));
            sc_dt.Columns.Add("comment_type", Type.GetType("System.String"));
            sc_dt.Columns.Add("comment_pos", Type.GetType("System.String"));
            sc_dt.Columns.Add("comment_header", Type.GetType("System.String"));


            foreach (var sc_item in SC)
            {
                DataRow sc_dr = sc_dt.NewRow();
                sc_dr["comment_who"] = user;
                sc_dr["comment"] = sc_item.comment;
                sc_dr["comment_type"] = "Call";
                if (sc_item.comment_pos != "")
                {
                    sc_dr["comment_pos"] = sc_item.comment_pos;
                }
                sc_dr["comment_header"] = sc_item.comment_header;

                sc_dt.Rows.Add(sc_dr);

            }


            DataTable FQS_dt = new DataTable();
            FQS_dt.Columns.Add("q_position", Type.GetType("System.String"));
            FQS_dt.Columns.Add("question_id", Type.GetType("System.Int32"));
            FQS_dt.Columns.Add("question_result", Type.GetType("System.Int32"));
            FQS_dt.Columns.Add("question_answered", Type.GetType("System.String"));
            FQS_dt.Columns.Add("click_text", Type.GetType("System.String"));
            FQS_dt.Columns.Add("other_answer_text", Type.GetType("System.String"));
            FQS_dt.Columns.Add("view_link", Type.GetType("System.String"));

            foreach (var fqs_item in FQS)
            {
                DataRow FQS_dr = FQS_dt.NewRow();
                FQS_dr["q_position"] = fqs_item.q_position;
                FQS_dr["question_id"] = fqs_item.question_id;
                FQS_dr["question_result"] = 0;
                FQS_dr["question_answered"] = fqs_item.question_answered;
                FQS_dr["click_text"] = fqs_item.click_text;
                FQS_dr["view_link"] = fqs_item.view_link;

                FQS_dt.Rows.Add(FQS_dr);
            }


            DataTable FQR_dt = new DataTable();
            FQR_dt.Columns.Add("question_id", Type.GetType("System.String"));
            FQR_dt.Columns.Add("answer_id", Type.GetType("System.Int32"));
            FQR_dt.Columns.Add("other_answer_text", Type.GetType("System.String"));

            foreach (var fqr_item in FQR)
            {
                DataRow FQR_dr = FQR_dt.NewRow();
                FQR_dr["question_id"] = fqr_item.question_id;
                FQR_dr["answer_id"] = fqr_item.answer_id;
                FQR_dr["other_answer_text"] = fqr_item.other_answer_text;

                FQR_dt.Rows.Add(FQR_dr);
            }

            DataTable FQSO_dt = new DataTable();
            FQSO_dt.Columns.Add("option_pos", Type.GetType("System.Int32"));
            FQSO_dt.Columns.Add("option_value", Type.GetType("System.String"));
            FQSO_dt.Columns.Add("question_id", Type.GetType("System.Int32"));
            FQSO_dt.Columns.Add("orig_id", Type.GetType("System.Int32"));

            foreach (var fqso_item in FQSO)
            {
                DataRow FQSO_dr = FQSO_dt.NewRow();
                FQSO_dr["option_pos"] = fqso_item.option_pos;
                FQSO_dr["option_value"] = fqso_item.option_value;
                FQSO_dr["question_id"] = fqso_item.question_id;
                FQSO_dr["orig_id"] = fqso_item.orig_id;
                FQSO_dt.Rows.Add(FQSO_dr);
            }

            // Save additional clerked data
            DataTable CD_dt = new DataTable();
            CD_dt.Columns.Add("value_id", Type.GetType("System.Int32"));
            CD_dt.Columns.Add("value_data", Type.GetType("System.String"));
            CD_dt.Columns.Add("value_position", Type.GetType("System.String"));

            foreach (var cd_item in CD)
            {
                DataRow CD_dr = CD_dt.NewRow();

                CD_dr["value_id"] = cd_item.ID;
                CD_dr["value_data"] = cd_item.data;
                CD_dr["value_position"] = cd_item.position;

                CD_dt.Rows.Add(CD_dr);
            }
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString);


            using (var command = new SqlCommand("listenDataInsert"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 90;
                //create your own data table
                command.Parameters.Add(new SqlParameter("@ListenInsert", listen_dt));
                command.Parameters.Add(new SqlParameter("@FQSInsert", FQS_dt));
                command.Parameters.Add(new SqlParameter("@FQRInsert", FQR_dt));
                command.Parameters.Add(new SqlParameter("@FQSOInsert", FQSO_dt));
                command.Parameters.Add(new SqlParameter("@SCInsert", sc_dt));
                command.Parameters.Add(new SqlParameter("@CDInsert", CD_dt));
                //command.Parameters.Add(New SqlParameter("@KeywordInsert", KW_dt))
                command.Connection = sqlCon;
                sqlCon.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";

            //DataTable dt = GetTable("select top 1 f_id from vwForm where review_id = '" + LD.review_ID + "' order by f_id desc");

            //return dt.Rows(0).Item(0).ToString;

        }
    }
}