
Partial Class DMS_lists
    Inherits System.Web.UI.Page

    Private Sub btnADd_Click(sender As Object, e As EventArgs) Handles btnADd.Click
        SqlDataSource1.Insert()
        GridView1.DataBind()
    End Sub
End Class
