Imports Common
Imports System.Data
Partial Class delete_reset_calls
    Inherits System.Web.UI.Page

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim call_list() As String = TextBox1.Text.Split(Chr(13))

        Dim dt As DataTable

        Dim review_ids As New List(Of String)

        For Each call_id In call_list
            Select Case ddlDataType.SelectedValue
                Case "Session ID"
                    dt = GetTable("select id from xcc_report_new where session_id = '" & call_id & "'")
                Case "F_ID"
                    dt = GetTable("select review_id from vwForm where f_id = " & call_id)
                Case "ID"
                    dt = GetTable("select id from xcc_report_new where id = '" & call_id & "'")
                Case "Phone"
                    dt = GetTable("select id from xcc_report_new where Phone = '" & call_id & "'")

            End Select


            If dt.Rows.Count > 0 Then

                Select Case ddlFunction.SelectedValue
                    Case "Reset Call"
                        UpdateTable("exec resetcall " & dt.Rows(0).Item(0).ToString)
                    Case "Mark Call Bad"
                        UpdateTable("exec resetcall " & dt.Rows(0).Item(0).ToString & ";update xcc_report_new set bad_call = 1, bad_call_reason = 'Known Bad', bad_call_date = dbo.getMTDate(), bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = 'System' where id =  " & dt.Rows(0).Item(0).ToString)
                    Case "Delete Call Completely"
                        UpdateTable("	declare @f_id int = (select f_id from vwForm where review_id = " & dt.Rows(0).Item(0).ToString & "); delete from form_q_scores where form_id = @f_id; delete from form_score3 where id = @f_id;	delete from  xcc_report_new where ID = " & dt.Rows(0).Item(0).ToString)
                End Select
            End If


        Next



        Label1.Text = "Updated/Deleted."

    End Sub
End Class
