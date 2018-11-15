
Partial Class Filter_editor
    Inherits System.Web.UI.Page

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        dsFilters.Insert()
        GridView1.DataBind()
    End Sub
End Class
