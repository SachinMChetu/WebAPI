Imports Common
Partial Class add_client_calibration
    Inherits System.Web.UI.Page

    Private Sub add_client_calibration_Load(sender As Object, e As EventArgs) Handles Me.Load
        dsNICards.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        UpdateTable("delete from cali_pending_client where assigned_to = '" & ddlSource.SelectedValue & "' and date_completed is null")
        dsOpenCals.DataBind()
        gvOpenCals.DataBind()
    End Sub

    Private Sub btnDeleteSelected_Click(sender As Object, e As EventArgs) Handles btnDeleteSelected.Click
        For Each gvr As GridViewRow In gvOpenCals.Rows
            Dim chkCopy As CheckBox = gvr.FindControl("chkCopy")
            Dim hdnThisID As HiddenField = gvr.FindControl("hdnThisID")
            If chkCopy.Checked Then
                UpdateTable("delete from cali_pending_client where id = " & hdnThisID.Value)
            End If
        Next


        lblResults.Text = "Updated."
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click

        For Each gvr As GridViewRow In gvOpenCals.Rows
            Dim chkCopy As CheckBox = gvr.FindControl("chkCopy")
            Dim hdnThisID As HiddenField = gvr.FindControl("hdnThisID")
            If chkCopy.Checked Then
                UpdateTable("insert into cali_pending_client(bad_value, form_id, review_type, appname, week_ending, assigned_to) select bad_value, form_id, review_type, appname, week_ending, '" & ddlTo.SelectedValue & "' from cali_pending_client where id = " & hdnThisID.Value)
            End If
        Next


        lblResults.Text = "Updated."

    End Sub

    Private Sub ddlSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSource.SelectedIndexChanged
        Dim appname As String = ddlSource.SelectedItem.Text
        appname = appname.Substring(appname.IndexOf("(") + 1)
        appname = Left(appname, Len(appname) - 1)
        'dsOpenCals.FilterExpression = "appname = '" & appname & "'"
        dsNICards.FilterExpression = "appname = '" & appname & "'"

        dsNICards.DataBind()
        ddlSource.DataBind()
        ddlTo.DataBind()

        dsOpenCals.DataBind()
        gvOpenCals.DataBind()
    End Sub
End Class
