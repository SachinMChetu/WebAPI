
Partial Class Column_editor
    Inherits System.Web.UI.Page

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        dsColumns.Insert()
        gvColumns.DataBind()
    End Sub

    Private Sub Column_editor_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.IsInRole("Admin") Then
            Response.Redirect("login.aspx?ReturnURL=column_editor.aspx")
        End If
    End Sub
End Class
