Imports System.IO
Imports Common

Partial Class All_Missed
    Inherits System.Web.UI.Page

    Dim agent_group_filter As String = ""

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not (User.IsInRole("Admin") Or User.IsInRole("Manager") Or User.IsInRole("Client") Or User.IsInRole("QA Lead") Or User.IsInRole("Supervisor")) Then Response.Redirect("default.aspx")
        If Not IsPostBack Then

            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "dashboard collapsed-menu")


            Dim totalDaysinMonth As Int32 = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)


            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                txtStartDate.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
                Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Else
                txtStartDate.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                txtEndDate.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
            Else
                txtEndDate.Text = Session("EndDate")
            End If

            If Request("start_date") Is Nothing Then
                txtStartDate.Text = Month(Today) & "/1/" & Year(Today)
            Else
                txtStartDate.Text = Request("start_date")
            End If

            If Request("end_date") Is Nothing Then
                txtEndDate.Text = Today.ToShortDateString
            Else
                txtEndDate.Text = Request("end_date")

            End If


            Recalc_Elements()

        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Protected Sub Recalc_Elements()
        Dim agent_group_filter As String = ""

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "
                'litGroupFilter.Text = userProfile.Group
            End If
        End If

        If ddlGroup.SelectedValue <> "" Then
            'litGroupFilter.Text = ddlGroup.SelectedValue
            agent_group_filter &= " and AGENT_group = '" & ddlGroup.SelectedValue & "' "
        End If

        If ddlAgent.SelectedValue <> "" Then
            'litGroupFilter.Text &= " " & ddlAgent.SelectedValue
            agent_group_filter &= " and AGENT = '" & ddlAgent.SelectedValue & "' "
        End If

        If ddlCampaign.SelectedValue <> "" Then
            'litGroupFilter.Text &= " " & ddlCampaign.SelectedValue
            agent_group_filter &= " and Campaign = '" & ddlCampaign.SelectedValue & "' "
        End If
        'check for already selected Agent 
        Dim agent_selected As String = ddlAgent.SelectedValue

        Response.Write("<!--" & agent_group_filter & "-->")

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        dsMissedPoint.SelectParameters("appname").DefaultValue = domain(0)

        dsMissedPoint.SelectParameters("agent_group_filter").DefaultValue = agent_group_filter
        dsMissedPoint.DataBind()
        'repeatMissedPoint.DataBind()
    End Sub

    Protected Sub btnAgentReport_Click(sender As Object, e As System.EventArgs) Handles btnApplyFilter.Click

        Recalc_Elements()
    End Sub

    Protected Sub btnSupeExportxp_Click(sender As Object, e As System.EventArgs) 'Handles btnSupeExportxp.Click



        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=AllMissed.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        pnlExcel.RenderControl(hw)
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

        txtStartDate.Text = previousStartDate.ToString("d")
        txtEndDate.Text = previousEndDate.ToString("d")

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton1.Click
        Dim currentStartDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
        Dim currentEndDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))

        txtStartDate.Text = currentStartDate.ToString("d")
        txtEndDate.Text = currentEndDate.ToString("d")
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click

        GV_to_CSV(repeatMissedPoint, "AllMissed")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=All_Missed.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)


        repeatMissedPoint.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub


    Protected Sub ddlAgent_DataBound(sender As Object, e As EventArgs) Handles ddlAgent.DataBound
        If ddlAgent.SelectedValue <> "" Then
            Session("SelectedAgent") = ddlAgent.SelectedValue
        End If

        If Session("SelectedAgent") <> "" Then
            Try
                ddlAgent.SelectedValue = Session("SelectedAgent")
            Catch ex As Exception

            End Try

        End If
    End Sub

    Protected Sub ddlAgent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAgent.SelectedIndexChanged
        Session("SelectedAgent") = ddlAgent.SelectedValue
    End Sub
    Protected Sub ddlCampaign_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCampaign.SelectedIndexChanged
        Session("SelectedCampaign") = ddlCampaign.SelectedValue
    End Sub


    Protected Sub ddlCampaign_DataBound(sender As Object, e As EventArgs) Handles ddlCampaign.DataBound
        If ddlCampaign.SelectedValue <> "" Then
            Session("SelectedCampaign") = ddlCampaign.SelectedValue
        End If

        If Session("SelectedCampaign") <> "" Then
            Try
                ddlCampaign.SelectedValue = Session("SelectedCampaign")
            Catch ex As Exception

            End Try

        End If
    End Sub


    Protected Sub ddlGroup_DataBound(sender As Object, e As EventArgs) Handles ddlGroup.DataBound


        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("Admin") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then

                ddlGroup.Visible = False
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "

                If ddlGroup.Items.Contains(New ListItem(userProfile.Group)) Then
                Else
                    ddlGroup.Items.Add(New ListItem(userProfile.Group))
                End If
                ddlGroup.SelectedValue = userProfile.Group
                ddlGroup.Enabled = False

                ddlAgent.Items.Clear()
                ddlAgent.Items.Add(New ListItem("All", ""))

                If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
                    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
                    'Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
                    'ddlAgent.Visible = False
                Else
                    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
                    'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
                    ddlAgent.Visible = True
                End If
                ddlAgent.DataBind()
            End If
        End If




        If Session("SelectedGroup") <> "" Then
            Try
                ddlGroup.SelectedValue = Session("SelectedGroup")
                ddlGroup_SelectedIndexChanged(sender, e)
                Recalc_Elements()
            Catch ex As Exception

            End Try

        End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        ddlAgent.Items.Clear()
        ddlAgent.Items.Add(New ListItem("All", ""))

        If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
            ' Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            'ddlAgent.Visible = False
        Else
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            ddlAgent.Visible = True
        End If

        'If ddlGroup.SelectedIndex > 0 Then
        '    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
        '    ddlAgent.Visible = True
        'End If
        ddlAgent.DataBind()

        Session("SelectedGroup") = ddlGroup.SelectedValue

        'Response.Write("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' order by AGent")
        'Response.End()





    End Sub



    Protected Sub dsCampaign_Selected(sender As Object, e As SqlDataSourceStatusEventArgs) Handles dsCampaign.Selected
        'Response.Write(e.AffectedRows)
        'Response.End()
        'repeatMissedPoint.DataSourceID = ""
        'repeatMissedPoint.DataSource = dsMissedPoint
        'repeatMissedPoint.DataBind()
    End Sub
End Class


