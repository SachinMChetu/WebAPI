Imports Common
Partial Class CC_DM_center
    Inherits System.Web.UI.Page

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        dsSummary.DataBind()
    End Sub

    Private Sub CC_DM_center_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            date_from.Text = DateAdd(DateInterval.Day, -14, Today())
            date_to.Text = Today()
            dsSummary.DataBind()
        End If
    End Sub


    Private Sub ddlWE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWE.SelectedIndexChanged
        date_from.Text = DateAdd(DateInterval.Day, -6, CDate(ddlWE.SelectedValue)).ToShortDateString
        date_to.Text = CDate(ddlWE.SelectedValue).ToShortDateString
        dsSummary.DataBind()
    End Sub

    Private Sub gvTrain_DataBound(sender As Object, e As EventArgs) Handles gvTrain.DataBound
        Sum_Column(gvDetails, 0)
        Sum_Column(gvDetails, 1)
        Avg_Column(gvDetails, 2)
        Sum_Column(gvDetails, 3)
        Avg_Column(gvDetails, 4)
        Sum_Column(gvDetails, 5)

    End Sub
End Class
