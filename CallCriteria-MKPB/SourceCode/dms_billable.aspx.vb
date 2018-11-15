Imports Common
Partial Class dms_billable
    Inherits System.Web.UI.Page

    Public myRole As String = ""
    Private Sub dms_billable_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsSC.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub
End Class
