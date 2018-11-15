
Partial Class am_desktop
    Inherits System.Web.UI.Page

    Private Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        SqlDataSource1.DataBind()
        GridView1.DataBind()
    End Sub

    Private Sub am_desktop_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                DropDownList1.SelectedValue = User.Identity.Name
                SqlDataSource1.DataBind()
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
