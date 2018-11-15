
Partial Class call_center_management
    Inherits System.Web.UI.Page



    Protected Sub btnNewCenter_Click(sender As Object, e As EventArgs) Handles btnNewCenter.Click
        dsCC.Insert()
        gvCC.DataBind()
    End Sub
End Class
