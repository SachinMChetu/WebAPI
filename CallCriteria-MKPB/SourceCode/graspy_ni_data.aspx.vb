
Partial Class graspy_ni_data
    Inherits System.Web.UI.Page

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        dsAgents.DataBind()
    End Sub

    Private Sub gvAgents_DataBound(sender As Object, e As EventArgs) Handles gvAgents.DataBound
        For Each gvr As GridViewRow In gvAgents.Rows
            If gvr.Cells(2).Text.Replace("%", "") < 50 Then
                gvr.Cells(2).ForeColor = Drawing.Color.Red

            End If
            If gvr.Cells(4).Text.Replace("%", "") < 50 Then
                gvr.Cells(4).ForeColor = Drawing.Color.Red

            End If

            If gvr.Cells(5).Text.Replace("%", "") < 50 Then
                gvr.Cells(5).ForeColor = Drawing.Color.Red

            End If
        Next
    End Sub
End Class
