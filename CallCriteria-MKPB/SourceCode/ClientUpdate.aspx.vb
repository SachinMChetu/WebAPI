Imports System.Data.SqlClient

Imports Common

Partial Class ClientUpdate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=cd2.aspx")
        End If

        Dim access As String() = {"Admin", "QA Lead", "Calibrator"}
        If Not access.Contains(Roles.GetRolesForUser().Single()) Then
            Response.Redirect("cd2.aspx")
        End If
    End Sub

    Protected Sub gvReport_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(5).Text.Length > 50 Then
                e.Row.Cells(5).Text = e.Row.Cells(5).Text.Substring(0, 50) + "..."
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If txtSubject.Text = "" Or txtMsg.Text = "" Or hdnTos.Value = "" Then Exit Sub

        Dim users As String() = hdnTos.Value.Split(",")

        For Each user As String In users
            Dim sql As String = "INSERT INTO messaging (from_login,to_login,message_text,dateadded,subject) " &
                            "VALUES ('" & HttpContext.Current.User.Identity.Name & "','" & user & "','" & txtMsg.Text & "',dbo.getMTDate(),'" & txtSubject.Text & "')"

            UpdateTable(sql)
        Next


        Response.Redirect("ClientUpdate.aspx")

        'gvReport.DataBind()

        'txtTos.Text = ""
        'txtSubject.Text = ""
        'txtMsg.Text = ""
    End Sub
End Class
