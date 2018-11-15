Imports Common

Partial Class ApplicantsData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim users As String() = {"admin", "tracy", "stacemoss", "jasonsalazar", "carlo", "ctatton"} 'ryan tracy stace jason carlo chad
        'If Not users.Contains(User.Identity.Name.ToLower) Then
        '    Response.Redirect("Default.aspx")
        'End If
        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("ApplicationForm.aspx")
        End If

        Dim access As String() = {"Admin"}
        If Not access.Contains(Roles.GetRolesForUser().Single()) Then
            Response.Redirect("ApplicationForm.aspx")
        End If
    End Sub

    Protected Sub gvReport_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(7).Text = "<span class='clickable answers' aid='" & e.Row.Cells(7).Text & "'>Answers</span>"
            e.Row.Cells(8).Text = "<a href='/docs/" & e.Row.Cells(8).Text & "'>Download</a>"
        End If
    End Sub

    Protected Sub gvQ_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQ.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = "<span class='clickable edit' qid='" & e.Row.Cells(0).Text & "'>Edit</span>"
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If q_edit.Text = "" Then Exit Sub
        Dim active As Integer
        If yes.Checked Then active = 1 Else active = 0

        If qid_edit.Value = "" Then
            UpdateTable("INSERT INTO ApplicantsQ SELECT '" & q_edit.Text.ToUpper & "', Active=" & active)
        Else
            UpdateTable("UPDATE ApplicantsQ SET Question='" & q_edit.Text.ToUpper & "', Active=" & active & " WHERE ID = " & qid_edit.Value)
        End If
        Response.Redirect("ApplicantsData.aspx")
    End Sub
End Class
