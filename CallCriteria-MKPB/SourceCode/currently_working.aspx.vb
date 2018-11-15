
Partial Class currently_working
    Inherits System.Web.UI.Page

    Private Sub currently_working_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?ReturnURL=currently_working.aspx")
        End If
    End Sub
End Class
