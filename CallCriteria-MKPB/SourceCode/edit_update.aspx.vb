Imports Common
Imports System.Data

Partial Class edit_update
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("ID") IsNot Nothing Then
            If Request("FSID") <> "" Then
                UpdateTable("INSERT INTO [dbo].[form_q_score_changes]([form_id],[question_id],[changed_by],[changed_date],[approved]," &
               "[approved_by],[approved_date],[new_value]) select " & Request("id") & "," & Request("QID") & ",'" & User.Identity.Name & "',dbo.getMTDate()," &
                "1,'" & User.Identity.Name & "',dbo.getMTDate(),(select id from question_answers where question_id = " & Request("QID") & " and answer_text = '" & Request("ans") & "')")
            Else
                UpdateTable("INSERT INTO [dbo].[form_q_score_changes]([form_id],[changed_by],[changed_date],[approved]," &
               "[approved_by],[approved_date],[new_value]) select " & Request("id") & ",'" & User.Identity.Name & "',dbo.getMTDate()," &
                "1,'" & User.Identity.Name & "',dbo.getMTDate(),(select id from question_answers where question_id = " & Request("QID") & " and answer_text = '" & Request("ans") & "')")
            End If


            Dim Sql As String = "insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" & HttpContext.Current.User.Identity.Name & "', dbo.getMTDate(), '" & HttpContext.Current.User.Identity.Name & " changes ' + (select q_short_name from questions where question_id = " & Request("QID") & ")   + ' to ' + '" & Request("ans") & "', form_id, 'Call'"
            UpdateTable(Sql)




            Dim ans_dt As DataTable = GetTable("select * from form_q_scores where form_id = " & Request("id") & " and question_id = " & Request("QID"))
            If ans_dt.Rows.Count > 0 Then
                UpdateTable("update form_q_scores set question_answered =  (select id from question_answers where question_id = " & Request("QID") & " and answer_text = '" & Request("ans") & "') where id = (select max(ID) from form_q_scores where question_id = " & Request("QID") & " and form_id  = " & Request("id") & ")")
            Else
                UpdateTable("INSERT INTO [dbo].[form_q_scores]([form_id],[question_id],question_answered) select " & Request("id") & "," & Request("QID") & ",(select id from question_answers where question_id = " & Request("QID") & " and answer_text = '" & Request("ans") & "')")
            End If


            UpdateTable("UpdateScores " & Request("id"))
            UpdateTable("UpdateMissed " & Request("id"))
            UpdateTable("CreateNotifications " & Request("id") & ",'" & Session("appname") & "'")
            UpdateTable("declare @cali_id int;select @cali_id=id from calibration_form where original_form = " & Request("id") & ";exec updateCalibration @cali_id")

            UpdateTable("update form_score3 set wasedited = 1 where id = " & Request("id"))


        End If
    End Sub
End Class
