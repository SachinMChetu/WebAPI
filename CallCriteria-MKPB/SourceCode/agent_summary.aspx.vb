Imports System.Data
Imports System.IO
Imports Common

Partial Class agent_summary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        dsScorecard.SelectParameters("username").DefaultValue = User.Identity.Name



        If Not IsPostBack Then
            txtAgentStart.Text = DateAdd(DateInterval.Day, -30, Today()).ToShortDateString
            txtAgentEnd.Text = Today().ToShortDateString
        End If


        If IsDate(txtAgentStart.Text) Then
            dsSummary.SelectParameters("startdate").DefaultValue = txtAgentStart.Text
        Else
            dsSummary.SelectParameters("startdate").DefaultValue = "1/1/2100"
        End If

        If IsDate(txtAgentEnd.Text) Then
            dsSummary.SelectParameters("enddate").DefaultValue = txtAgentEnd.Text
        Else
            dsSummary.SelectParameters("enddate").DefaultValue = "1/1/2100"
        End If

        If IsDate(txtAgentStart.Text) Then
            dsQDetails.SelectParameters("startdate").DefaultValue = txtAgentStart.Text
        Else
            dsQDetails.SelectParameters("startdate").DefaultValue = "1/1/2100"
        End If

        If IsDate(txtAgentEnd.Text) Then
            dsQDetails.SelectParameters("enddate").DefaultValue = txtAgentEnd.Text
        Else
            dsQDetails.SelectParameters("enddate").DefaultValue = "1/1/2100"
        End If

        dsSummary.DataBind()
        dsQDetails.DataBind()
        fvSummary.DataBind()
        'gvQDetails.DataBind()
        rptDetails.DataBind()

    End Sub

    Protected Sub btnAll_Click(sender As Object, e As EventArgs) Handles btnAll.Click
        Response.Redirect("Missed_all_notes.aspx?agent=" & Request("agent"))
    End Sub

    Private Sub btnSupeExportxp_Click(sender As Object, e As EventArgs) Handles btnSupeExportxp.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=" & ddlAgents.SelectedValue & "_Report.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        'For Each ctl As Control In Controls
        '    Response.Write(ctl.ID & "<br>")
        'Next
        'Response.End()

        pnlAS.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub


    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Private Sub ddlScorecard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlScorecard.SelectedIndexChanged
        ddlAgents.Items.Clear()
        ddlAgents.Items.Add(New ListItem("(Select)", ""))
        dsAgents.DataBind()
        ddlAgents.DataBind()
    End Sub
End Class
