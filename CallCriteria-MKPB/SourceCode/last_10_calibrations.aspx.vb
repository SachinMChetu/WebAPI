
Partial Class last_10_calibrations
    Inherits System.Web.UI.Page

    Private Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

        SqlDataSource2.SelectParameters(0).DefaultValue = GridView1.DataKeys(GridView1.SelectedRow.RowIndex).Values(0)
        SqlDataSource2.SelectParameters(1).DefaultValue = GridView1.DataKeys(GridView1.SelectedRow.RowIndex).Values(1)

        GridView2.DataBind()
    End Sub

    Private Sub GridView2_DataBound(sender As Object, e As EventArgs) Handles GridView2.DataBound
        For Each gvr As GridViewRow In GridView2.Rows
            Try
                gvr.Cells(0).Text = "<a href='review_record.aspx?ID=" & gvr.Cells(0).Text & "' target=_blank>" & gvr.Cells(0).Text & "</a>"
            Catch ex As Exception

            End Try
        Next
    End Sub
End Class
