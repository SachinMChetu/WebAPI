
Partial Class Link_management
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SqlDataSource1.Insert()
        GridView1.DataBind()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SqlDataSource2.Insert()
        GridView2.DataBind()
    End Sub

    Private Sub Link_management_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.IsInRole("Admin") Then
            Response.Redirect("default.aspx")
        End If
    End Sub


End Class
