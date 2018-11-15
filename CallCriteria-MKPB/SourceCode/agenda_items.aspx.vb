
Partial Class agenda_items
    Inherits System.Web.UI.Page

    Private Sub agenda_items_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.IsInRole("Admin") Then
            btnAddItem.Visible = False
        End If

        dsAgenda.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Response.Redirect("create_agenda.aspx")
    End Sub
End Class
