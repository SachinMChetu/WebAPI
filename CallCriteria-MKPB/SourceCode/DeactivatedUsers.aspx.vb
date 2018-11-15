Imports System.Data
Imports Common

Partial Class DeactivatedUsers
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
            Dim drv As DataRowView = e.Row.DataItem

            'Item can be deleted
            Dim btn As Button = e.Row.FindControl("btnReset")
            btn.CommandArgument = drv.Item("username")
        End If
    End Sub

    Protected Sub btn_Click(sender As Object, e As System.EventArgs)

        Dim btn As Button = sender

        UpdateTable("UPDATE UserExtraInfo SET lastActiveDate = dbo.getMTDate() WHERE username = '" & btn.CommandArgument & "'")

        Response.Redirect("DeactivatedUsers.aspx")

    End Sub

End Class
