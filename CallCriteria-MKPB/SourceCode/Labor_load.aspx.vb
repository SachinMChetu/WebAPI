
Partial Class Labor_load
    Inherits System.Web.UI.Page

    Protected Sub gvQAReport_PreRender(sender As Object, e As EventArgs) Handles gvQAReport.PreRender
        If gvQAReport.HeaderRow IsNot Nothing Then
            gvQAReport.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub
End Class
