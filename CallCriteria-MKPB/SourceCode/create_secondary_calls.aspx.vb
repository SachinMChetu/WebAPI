Imports Common
Imports System.Data

Partial Class create_secondary_calls
    Inherits System.Web.UI.Page

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim dt As DataTable = GetTable("select top " & txtQuantity.Text & " review_id from vwForm where scorecard = " & ddlSC.SelectedValue & " and calib_score is null and wasedited is null order by review_id desc")

        For Each dr In dt.Rows
            UpdateTable("exec [createSecondaryCall] " & dr.item(0))
        Next

        lblResults.Text = txtQuantity.Text & " added."
        GridView1.DataBind()
    End Sub
End Class
