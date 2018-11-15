
Partial Class calibration_history
    Inherits System.Web.UI.Page

    Protected Sub gvHistory_PreRender(sender As Object, e As EventArgs) Handles gvHistory.PreRender
        If gvHistory.HeaderRow IsNot Nothing Then
            gvHistory.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub btnQAReport_Click(sender As Object, e As EventArgs) Handles btnQAReport.Click
        Dim domain_selected As String = ""
        For Each li As ListItem In cblDomainList.Items
            If li.Selected Then
                If domain_selected = "" Then
                    domain_selected = Trim(li.Text)
                Else
                    domain_selected &= "," & Trim(li.Text)
                End If

            End If
        Next

        dsHistory.SelectParameters("appnames").DefaultValue = domain_selected
      
    End Sub

    Protected Sub gvHistory_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim num_cols As Integer = 0
            Dim total_score As Single = 0
            For x As Integer = 2 To e.Row.Cells.Count - 1 Step 3
                Try
                    total_score += e.Row.Cells(x).Text
                    num_cols += 1
                    e.Row.Cells(x).Text = "<a href='view_calibration.aspx?reviewer=" & e.Row.Cells(1).Text & "&we_date=" & gvHistory.HeaderRow.Cells(x).Text & "'>" & e.Row.Cells(x).Text & "</a>"
                Catch ex As Exception
                    'Response.Write(e.Row.Cells(x).Text)
                    'Response.End()
                End Try

            Next
        End If
    End Sub
End Class
