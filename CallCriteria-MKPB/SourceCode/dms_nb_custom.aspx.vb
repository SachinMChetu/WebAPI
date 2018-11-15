Imports System.IO
Imports Common
Partial Class dms_nb_custom
    Inherits System.Web.UI.Page

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        GV_to_CSV(gvNB, "NBReport")


    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Private Sub dms_nb_report_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=dms_nb_report.aspx")
        End If

        dsSC.SelectParameters("username").DefaultValue = User.Identity.Name

    End Sub

    Private Sub gvNB_DataBound(sender As Object, e As EventArgs) Handles gvNB.DataBound
        lblRows.Text = gvNB.Rows.Count
    End Sub
    Protected Sub ddlComments_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim hdnFID As HiddenField = sender.parent.findcontrol("hdnFID")
        Dim hdnQID As HiddenField = sender.parent.findcontrol("hdnQID")
        Dim ddlComments As DropDownList = sender.parent.findcontrol("ddlComments")

        If ddlComments.SelectedValue = "-1" Then
            UpdateTable("update vwForm set sort_order = -1 where f_id = " & hdnFID.Value)
        End If

        UpdateTable("insert into form_q_responses (form_id, question_id, answer_id) select " & hdnFID.Value & "," & hdnQID.Value & "," & ddlComments.SelectedValue)
        ' UpdateTable("insert into form_q_score_changes (form_id, question_id, answer_id) select " & hdnFID.Value & "," & hdnQID.Value & "," & ddlComments.SelectedValue)
        gvNB.DataBind()

    End Sub
End Class
