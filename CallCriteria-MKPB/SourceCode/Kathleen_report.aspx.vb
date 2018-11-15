
Partial Class Kathleen_report
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GridView1.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            textbox1.SelectedDate = Today.ToShortDateString
            textbox2.SelectedDate = Today.ToShortDateString
        End If
    End Sub
End Class
