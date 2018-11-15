
Partial Class clerked_data
    Inherits System.Web.UI.Page

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        dsItems.Insert()
        gvItems.DataBind()
    End Sub

    Private Sub clerked_data_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsItems.InsertParameters("added_by").DefaultValue = User.Identity.Name
    End Sub
End Class
