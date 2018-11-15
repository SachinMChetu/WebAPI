Imports System.IO
Imports Common

Partial Class Agent_Report
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If User.IsInRole("Agent") Or User.IsInRole("QA") Or User.Identity.Name = "" Then
            Response.Redirect("default.aspx")
        End If
        If Not IsPostBack Then
            If Request("start_date") Is Nothing Then
                txtAgentStart.Text = Month(Today) & "/1/" & Year(Today)
            Else
                txtAgentStart.Text = Request("start_date")
            End If

            If Request("end_date") Is Nothing Then
                txtAgentEnd.Text = Today.ToShortDateString
            Else
                txtAgentEnd.Text = Request("end_date")

            End If

            If User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("QA Lead") Then

                Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
                If userProfile.Group <> "" Then
                    dsAgentList2.FilterExpression = "AGENT_GROUP = '" & userProfile.Group & "'"
                    dsAgentList2.DataBind()
                End If
            End If

            If Request("Agent") IsNot Nothing Then
                ddlAgentList2.SelectedValue = Request("Agent")
            End If
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Protected Sub gvAgentQuestions_DataBound(sender As Object, e As System.EventArgs) Handles gvAgentQuestions.DataBound
        Dim gv As GridView = sender
        Dim total As Integer = 0
        Dim total2 As Integer = 0
        Dim total3 As Integer = 0
        Dim total4 As Integer = 0
        Dim total5 As Integer = 0
        Dim num_rows As Integer = 0
        For Each gvr As GridViewRow In gv.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                total += gvr.Cells(3).Text
                total2 += gvr.Cells(4).Text
                num_rows += 1
            End If

        Next
        If num_rows > 0 Then
            gv.FooterRow.Cells(1).Text = "Averages:"
            gv.FooterRow.Cells(4).Text = CSng(CInt(total2 / num_rows * 100)) / 100
            gv.FooterRow.Cells(3).Text = CSng(CInt(total / num_rows * 100)) / 100
        End If
    End Sub

    Protected Sub btnAgentReport_Click(sender As Object, e As System.EventArgs) Handles btnAgentReport.Click
        gvAgentQuestions.DataBind()
    End Sub

    Protected Sub btnSupeExportxp_Click(sender As Object, e As System.EventArgs) Handles btnSupeExportxp.Click

        GV_to_CSV(gvAgentQuestions, "Agent")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Agent.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvAgentQuestions.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton2.Click

        Dim previousStartDate As DateTime = New DateTime(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month, 1)
        Dim previousEndDate As DateTime = New DateTime(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month))

        txtAgentStart.Text = previousStartDate.ToString()
        txtAgentEnd.Text = previousEndDate.ToString("d")
        dsAvgQuestions.DataBind()
        'btnAgentReport_Click(sender, e)
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton1.Click
        Dim currentStartDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
        Dim currentEndDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))

        txtAgentStart.Text = currentStartDate.ToString("d")
        txtAgentEnd.Text = currentEndDate.ToString("d")
        dsAvgQuestions.DataBind()
        'btnAgentReport_Click(sender, e)
    End Sub
End Class
