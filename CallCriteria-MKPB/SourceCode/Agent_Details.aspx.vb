Imports System.Data

Partial Class Agent_Details
    Inherits System.Web.UI.Page

    Protected Sub btnAgentReport_Click(sender As Object, e As System.EventArgs) Handles btnAgentReport.Click
        dsAvgQuestions.DataBind()
        gvAgentQuestions.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("agent") IsNot Nothing Then
                hdnAgentID.Value = Request("agent")
                lblAgentID.Text = Request("agent")
                dsAvgQuestions.DataBind()
            End If
        End If
    End Sub


    Protected Sub Ack_Item(sender As Object, e As System.EventArgs)
        Dim btn As Button = sender

        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Common.UpdateTable("Update form_notifications set date_closed = dbo.getMTDate(), closed_by = '" & ack_by & "' where id = " & btn.CommandArgument)
        gvComments.DataBind()
    End Sub


    Protected Sub gvAgentQuestions_DataBound(sender As Object, e As System.EventArgs) Handles gvAgentQuestions.DataBound
        lblTotal.Text = gvAgentQuestions.Rows.Count

        Dim gv As GridView = sender
        Dim total As Integer = 0
        Dim total2 As Integer = 0
        Dim total3 As Integer = 0
        Dim total4 As Integer = 0
        Dim total5 As Integer = 0
        Dim num_rows As Integer = 0
        For Each gvr As GridViewRow In gv.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                total += gvr.Cells(4).Text
                total2 += gvr.Cells(5).Text
                'total3 += gvr.Cells(2).Text
                num_rows += 1
            End If

        Next
        If num_rows > 0 Then
            gv.FooterRow.Cells(1).Text = "Averages:"
            gv.FooterRow.Cells(5).Text = CSng(CInt(total2 / num_rows * 100)) / 100
            gv.FooterRow.Cells(4).Text = CSng(CInt(total / num_rows * 100)) / 100
            'gv.FooterRow.Cells(2).Text = CSng(CInt(total3 / num_rows * 100)) / 100
        End If
    End Sub

    Protected Sub gvAgentQuestions_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAgentQuestions.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = e.Row.DataItem
            If drv.Item("real_total").ToString = "0" Then
                e.Row.BackColor = Drawing.Color.LightPink
            End If

            If drv.Item("num_serious").ToString <> "0" Then
                e.Row.BackColor = Drawing.Color.Orange
            End If

        End If
    End Sub

    Protected Sub gvAgentQuestions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvAgentQuestions.SelectedIndexChanged
        Dim gv As GridView = sender
        Response.Redirect("review_record.aspx?ID=" & gv.DataKeys(gv.SelectedRow.RowIndex).Value)
    End Sub
End Class
