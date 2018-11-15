
Partial Class high_priority_scorecards
    Inherits System.Web.UI.Page

    Private Sub high_priority_scorecards_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnUR=high_priority_scorecards.aspx")
        End If
    End Sub
End Class
