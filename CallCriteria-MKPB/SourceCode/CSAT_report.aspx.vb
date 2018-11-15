
Partial Class CSAT_report
    Inherits System.Web.UI.Page

    Private Sub CSAT_report_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?ReturnURL=csat_report.aspx")

        End If

        dsScorecards.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Private Sub btnGO_Click(sender As Object, e As EventArgs) Handles btnGO.Click
        dsCSAT.DataBind()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Common.GV_to_CSV(gvCSAT, "csat_export")
    End Sub

    Private Sub gvCSAT_DataBound(sender As Object, e As EventArgs) Handles gvCSAT.DataBound
        Common.Avg_Column(gvCSAT, 1)
        Common.Avg_Column(gvCSAT, 2)
        Common.Avg_Column(gvCSAT, 3)
        Common.Sum_Column(gvCSAT, 4)
    End Sub
End Class
