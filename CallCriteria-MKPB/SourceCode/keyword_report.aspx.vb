
Partial Class keyword_report
    Inherits System.Web.UI.Page

    Private Sub keyword_report_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsNICards.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub
End Class
