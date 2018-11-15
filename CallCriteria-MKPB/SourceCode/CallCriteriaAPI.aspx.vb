
Partial Class CallCriteriaAPI
    Inherits System.Web.UI.Page

    Private Sub CallCriteriaAPI_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsMyApps.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub
End Class
