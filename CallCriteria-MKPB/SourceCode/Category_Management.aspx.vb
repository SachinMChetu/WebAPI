
Partial Class Category_Management
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not User.IsInRole("Admin") Then Response.Redirect("default.aspx")

        If Not IsPostBack Then
            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "management collapsed-menu")

        End If

    End Sub

    Protected Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
        dsCategories2.Insert()
        gvCategories.DataBind()
    End Sub

End Class
