
Imports System.IO
Imports Common
Partial Class NotificationsReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=cd2.aspx")
        End If

        Dim access As String() = {"Admin", "QA Lead", "Calibrator", "Client", "Supervisor", "Manager"}
        If Not access.Contains(Roles.GetRolesForUser().Single()) Then
            Response.Redirect("cd2.aspx")
        End If

        If Not IsPostBack Then
            from.Text = DateAdd(DateInterval.Day, -14, Today())
            [to].Text = Today()

            dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
            dsSummary.SelectParameters("username").DefaultValue = User.Identity.Name
            dsUsers.SelectParameters("username").DefaultValue = User.Identity.Name
            dsReport.SelectParameters("username").DefaultValue = User.Identity.Name
            dsSuper.SelectParameters("username").DefaultValue = User.Identity.Name
            dsAppname.SelectParameters("username").DefaultValue = User.Identity.Name
            dsDisputes.DataBind()
        End If

        'recalc()
    End Sub

    Protected Sub recalc()
        Dim addl_sql As String = "1=1 "
        If from.Text <> "" Then
            addl_sql &= " and review_date >= '" & from.Text & "' "
        End If

        If [to].Text <> "" Then
            addl_sql &= " and review_date <= '" & [to].Text & "' "
        End If

        If ddlApps.SelectedValue <> "" Then
            addl_sql &= " and scorecard = '" & ddlApps.SelectedValue & "' "
        End If

        If ddlAppname.SelectedValue <> "" Then
            addl_sql &= " and appname = '" & ddlAppname.SelectedValue & "' "
        End If


        dsReport.SelectCommand = "SELECT TOP 500 vwFN.appname, F_ID, short_name, wasEdited, role, call_date, AGENT, AGENT_group, missed_list, comment, date_created, opened_by, date_closed, closed_by, close_reason, reviewer FROM vwFN join scorecards on scorecards.id = scorecard where " & addl_sql & "  and scorecard in (select user_scorecard from userapps where username=@username)  ORDER BY date_created DESC"
        dsReport.SelectCommand = "Select TOP 500 appname, vwForm.F_ID,scorecard_name,  wasEdited, call_date, AGENT, AGENT_group, phone, missed_list, reviewer, all_comments From vwForm Join (Select f_id, all_comments = STUFF((   SELECT ', ' + '(' + role + isnull( ' - ' + closed_by,'') + ') ' + isnull(comment,'--Open--')  + ' ' + format(date_created,'MM/dd/yyyy') + '-' + isnull(format(date_closed, 'MM/dd/yyyy'),'') + char(10)  FROM dbo.vwFN WHERE f_id = x.f_id  order by isnull(date_closed, '1/1/2100') for XML PATH(''), TYPE).value('.[1]', 'nvarchar(max)'), 1, 2, '') From dbo.vwFN AS x Group By f_id) b on b.f_id = vwForm.f_id Where " & addl_sql & " and scorecard In (Select user_scorecard from userapps where username=@username) ORDER BY f_id DESC"
        'Response.Write(dsReport.SelectCommand)
        'Response.End()
        dsReport.DataBind()
        gvReport.DataBind()
    End Sub


    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        gv_to_csv(gvReport, "Notificationreport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=NotificationReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvReport.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub


    'Private Sub ddlApps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApps.SelectedIndexChanged
    '    If ddlApps.SelectedValue = "" Then
    '        dsReport.SelectCommand = "SELECT TOP 500 appname, F_ID, call_date, AGENT, AGENT_group, missed_list, comment, date_created, opened_by, date_closed, closed_by, close_reason, reviewer FROM vwFN ORDER BY date_created DESC"
    '    Else
    '        dsReport.SelectCommand = "SELECT TOP 500 appname, F_ID, call_date, AGENT, AGENT_group, missed_list, comment, date_created, opened_by, date_closed, closed_by, close_reason, reviewer FROM vwFN where appname = '" & ddlApps.SelectedValue & "' ORDER BY date_created DESC"

    '    End If

    'End Sub

    Private Sub NotificationsReport_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

    End Sub

    Private Sub dsReport_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsReport.Selecting
        e.Command.CommandTimeout = 90
    End Sub

    Private Sub dsSummary_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsSummary.Selecting
        e.Command.CommandTimeout = 90
    End Sub

    Private Sub dsSuper_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsSuper.Selecting
        e.Command.CommandTimeout = 90
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        dsDisputes.DataBind()
        gvDisputes.DataBind()
    End Sub
End Class
