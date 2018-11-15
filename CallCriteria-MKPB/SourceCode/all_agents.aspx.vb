Imports System.IO
Imports System.Data
Imports Common

Partial Class all_agents
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If User.IsInRole("Agent") Or User.IsInRole("QA") Or User.Identity.Name = "" Then
            Response.Redirect("default.aspx")
        End If


        If User.IsInRole("Manager") Or User.IsInRole("Supervisor") Or User.IsInRole("QA Lead") Then
            Dim userProfile As ProfileCommon = ProfileCommon.Create(User.Identity.Name)
            dsAvgQuestions.FilterExpression = "[Agent Group] = '" & userProfile.Group & "' or [Agent Group] = '' or  [Agent Group] is null"
        End If

        If Not IsPostBack Then

            Dim totalDaysinMonth As Int32 = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)

            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                txtAgentStart.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
                Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Else
                txtAgentStart.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                txtAgentEnd.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
            Else
                txtAgentEnd.Text = Session("EndDate")
            End If


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

        txtAgentStart.Text = previousStartDate.ToString("d")
        txtAgentEnd.Text = previousEndDate.ToString("d")

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton1.Click
        Dim currentStartDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
        Dim currentEndDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))

        txtAgentStart.Text = currentStartDate.ToString("d")
        txtAgentEnd.Text = currentEndDate.ToString("d")
    End Sub

    Protected Sub gvAgentQuestions_PreRender(sender As Object, e As EventArgs) Handles gvAgentQuestions.PreRender
        If gvAgentQuestions.HeaderRow IsNot Nothing Then
            gvAgentQuestions.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Protected Sub ddlGroup_DataBound(sender As Object, e As EventArgs) Handles ddlGroup.DataBound

        If Session("SelectedGroup") <> "" Then
            Try
                ddlGroup.SelectedValue = Session("SelectedGroup")
            Catch ex As Exception

            End Try

        End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        If ddlGroup.SelectedIndex <> 0 Then
            Session("SelectedGroup") = ddlGroup.SelectedValue
            dsAvgQuestions.FilterExpression = "[Agent Group] = '" & ddlGroup.SelectedValue & "'"
        Else
            dsAvgQuestions.FilterExpression = ""
            Session("SelectedGroup") = ""
        End If

        Response.Write("<!--" & ddlGroup.SelectedValue & "-->")
        dsAvgQuestions.DataBind()
    End Sub

   
    Protected Sub gvAgentQuestions_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAgentQuestions.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            
            Dim missed_list() As String = e.Row.Cells(3).Text.Split(",")

            Dim new_list As String = ""
            For Each missed_item In missed_list
                new_list &= "<a href=" & Chr(34) & "/expandedview.aspx?filter= and fs.id in(select * from dbo.GetFormListByShortName('" & Trim(missed_item) & "','" & Session("appname") & "')) and agent = '" & e.Row.DataItem("Agent Name") & "'" & Chr(34) & ">" & Trim(missed_item) & "</a>, "
            Next

            new_list = Left(new_list, Len(new_list) - 2)

            e.Row.Cells(3).Text = new_list
        End If
    End Sub
End Class
