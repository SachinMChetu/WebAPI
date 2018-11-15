
Partial Class cs_spotcheck_report
    Inherits System.Web.UI.Page

    Private Sub cs_spotcheck_report_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtStart.Text = Today.AddDays(-Today.Day).AddHours(-7).ToShortDateString
            txtEnd.Text = Today.AddHours(-7).ToShortDateString
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dsPerf.DataBind()
        gvPerf.DataBind()
    End Sub

    Private Sub gvPerf_DataBound(sender As Object, e As EventArgs) Handles gvPerf.DataBound

        'If gvPerf.Columns.Count > 6 Then
        Common.Sum_Column(gvPerf, 1)
        Common.Sum_Column(gvPerf, 2)
        Common.Sum_Column(gvPerf, 3)
        Common.Avg_Weighted_Column(gvPerf, 4, 2)
        Common.Sum_Column(gvPerf, 5)
        Common.Sum_Column(gvPerf, 6)
        Common.Sum_Column(gvPerf, 7)
        Common.Avg_Weighted_Column(gvPerf, 8, 6)

        'Common.Avg_Weighted_Column(gvPerf, 6, 4)
        'End If
    End Sub

    Private Sub gvPerf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPerf.SelectedIndexChanged
        'Try

        'Catch ex As Exception

        'End Try


    End Sub

    Private Sub gvPerf_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPerf.RowCommand
        hdnSelectedQA.Value = gvPerf.Rows(e.CommandArgument).Cells(0).Text
        dsDetails.DataBind()
        gvDetails.DataBind()
    End Sub
End Class
