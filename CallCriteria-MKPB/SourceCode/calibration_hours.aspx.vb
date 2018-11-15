
Partial Class calibration_hours
    Inherits System.Web.UI.Page

    Protected Sub gvHours_PreRender(sender As Object, e As System.EventArgs) Handles gvHours.PreRender
        If gvHours.HeaderRow IsNot Nothing Then
            gvHours.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub gvHours_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHours.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(5).Text = "$" & e.Row.Cells(5).Text
            e.Row.Cells(6).Text = e.Row.Cells(6).Text & "%"
        End If
    End Sub

    Private Sub btnQAReport_Click(sender As Object, e As EventArgs) Handles btnQAReport.Click
        gvHours.DataBind()
    End Sub
End Class
