
Partial Class admin_toolbox
    Inherits System.Web.UI.Page

    Private Sub admin_toolbox_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.IsInRole("Admin") Then
            Response.Redirect("default.aspx")
        End If
    End Sub
End Class
