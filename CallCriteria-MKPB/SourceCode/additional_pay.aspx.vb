
Partial Class additional_pay
    Inherits System.Web.UI.Page

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        dsPay.Insert()
        gvPay.DataBind()
    End Sub

    Private Sub additional_pay_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsPay.InsertParameters("who_added").DefaultValue = User.Identity.Name


    End Sub
End Class
