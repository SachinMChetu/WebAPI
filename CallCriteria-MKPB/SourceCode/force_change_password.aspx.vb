
Partial Class force_change_password
    Inherits System.Web.UI.Page

    Protected Sub ChangePassword1_ChangedPassword(sender As Object, e As EventArgs) Handles ChangePassword1.ChangedPassword
        Response.Redirect("cd2.aspx")
    End Sub
End Class
