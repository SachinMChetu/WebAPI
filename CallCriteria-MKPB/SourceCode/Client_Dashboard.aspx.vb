Imports System.Data.SqlClient
Imports System.Data
Imports Common
Imports System.IO

Partial Class Admin_Dashboard
    Inherits System.Web.UI.Page

    Public start_date As String
    Public end_date As String

    Public agent_group_filter As String = ""

    Public line_graph_data As String = ""
    Public line_graph_data_30 As String = ""
    Public line_graph_data_60 As String = ""

    Public min_line_range As String = "-90"
    Public max_line_range As String = "0"

    Public min_date As Date
    Public max_date As Date

    Public data_rate As String

    Dim no_details As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            If user_info_dt.Rows(0).Item("speed_increment").ToString <> "" Then
                data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            Else
                data_rate = 0.05
            End If

        Else
            data_rate = 0.05
        End If


        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        If Not Roles.IsUserInRole("Admin") And Not Roles.IsUserInRole("Manager") And Not Roles.IsUserInRole("Supervisor") And Not Roles.IsUserInRole("QA Lead") And Not Roles.IsUserInRole("Client") Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        Session("appname") = domain(0)

        ',edsoup,edutrek
        'If "esto,estobk".IndexOf(domain(0)) > -1 Then
        '    Response.Redirect("cd2.aspx")
        'End If


        If Not IsPostBack Then

            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "dashboard collapsed-menu")

            Dim latest_dt As DataTable = GetTable("select max(call_date) from xcc_report_new where appname = '" & domain(0) & "'")
            If latest_dt.Rows.Count > 0 Then
                Try
                    litLatest.Text = CDate(latest_dt.Rows(0).Item(0)).ToShortDateString
                Catch ex As Exception

                End Try

            End If

            Dim client_dt As DataTable = GetTable("select * from app_settings where appname = '" & Session("appname") & "'")
            If client_dt.Rows.Count > 0 Then
                litClientName.Text = client_dt.Rows(0).Item("FullName").ToString
            End If

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

            'dsMySessions.SelectParameters("selectedDate").DefaultValue = DateAdd(DateInterval.Day, -1, Today).ToString("MM/dd/yyyy")
            'gvUserSessions.DataBind()


            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
            ddlAgent.DataBind()

            Recalc_Elements()

        End If




    End Sub


    Protected Sub Recalc_Elements()

        litGroupFilter.Text = "All Agents"

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "
                litGroupFilter.Text = userProfile.Group
            End If
        End If

        If ddlGroup.SelectedValue <> "" Then
            litGroupFilter.Text = ddlGroup.SelectedValue
            agent_group_filter &= " and AGENT_group = '" & ddlGroup.SelectedValue & "' "
        End If

        If ddlAgent.SelectedValue <> "" Then
            litGroupFilter.Text &= " " & ddlAgent.SelectedValue
            agent_group_filter &= " and AGENT = '" & ddlAgent.SelectedValue & "' "
        End If

        If ddlCampaign.SelectedValue <> "" Then
            litGroupFilter.Text &= " " & ddlCampaign.SelectedValue
            agent_group_filter &= " and Campaign = '" & ddlCampaign.SelectedValue & "' "
        End If
        'check for already selected Agent 
        Dim agent_selected As String = ddlAgent.SelectedValue


        hdnAgentFilter.Value = agent_group_filter

        ddlAgent.Items.Clear()
        ddlAgent.Items.Add(New ListItem("All Agents", ""))

        If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & " 11:59:59 pm'  order by AGent")
            'Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            'ddlAgent.Visible = False
        Else
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & " 11:59:59 pm' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            ddlAgent.Visible = True
        End If
        Try
            ddlAgent.DataBind()
        Catch ex As Exception

        End Try




        Dim fiftytoseventyRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','-100','80','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        hdn50to70.Text = If(fiftytoseventyRows.Rows.Count > 0, fiftytoseventyRows(0).Item(0).ToString(), "0")

        Dim seventytoeightyRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','80','90','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        hdn70to80.Text = If(seventytoeightyRows.Rows.Count > 0, seventytoeightyRows(0).Item(0).ToString(), "0")

        Dim eightytoninty As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','90','99','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        hdn80to90.Text = If(eightytoninty.Rows.Count > 0, eightytoninty(0).Item(0).ToString(), "0")

        Dim nintyplus As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','99','200','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        hdn90plus.Text = If(nintyplus.Rows.Count > 0, nintyplus(0).Item(0).ToString(), "0")

        Dim calls_total As Integer = CInt(hdn50to70.Text) + CInt(hdn70to80.Text) + CInt(hdn80to90.Text) + CInt(hdn90plus.Text)
        If calls_total > 0 Then
            hdn50to70.Text = hdn50to70.Text & " (" & CInt(hdn50to70.Text / calls_total * 100) & "%)"
            hdn70to80.Text = hdn70to80.Text & " (" & CInt(hdn70to80.Text / calls_total * 100) & "%)"
            hdn80to90.Text = hdn80to90.Text & " (" & CInt(hdn80to90.Text / calls_total * 100) & "%)"
            hdn90plus.Text = hdn90plus.Text & " (" & CInt(hdn90plus.Text / calls_total * 100) & "%)"
        End If




        'litStart.Text = txtStartDate.Text
        'litEnd.Text = txtEndDate.Text
        litStart2.Text = txtStartDate.Text
        litEnd2.Text = txtEndDate.Text
        'litStart3.Text = txtStartDate.Text
        'litEnd3.Text = txtEndDate.Text


        litBilledTime.Text = ""
        litNumberCalls.Text = ""

        Dim billed_dt As DataTable = GetTable("select [dbo].[ConvertTimeToHHMMSS](sum(case when call_length < 60 then 60 else call_length end),'s'), sum(case when call_length < 60 then 60 else call_length end)/60 * (select bill_rate from app_settings where appname = '" & Session("appname") & "') , count(*) as num_calls  from vwForm where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and appname = '" & Session("appname") & "' " & agent_group_filter)
        If billed_dt.Rows.Count > 0 And (User.IsInRole("Client") Or User.IsInRole("Admin")) Then
            litBilledTime.Text = billed_dt.Rows(0).Item(0).ToString
            litNumberCalls.Text = billed_dt.Rows(0).Item("num_calls").ToString
            Try
                litBilledAmount.Text = FormatCurrency(billed_dt.Rows(0).Item(1).ToString, 2)
            Catch ex As Exception

            End Try

        End If


        Dim dtAvgScore As DataTable = GetTable("SELECT  CAST(AVG(total_score_with_fails/top_score) * 100 AS DECIMAL(12,1)) as Counts, CAST(AVG(total_score/top_score) * 100 AS DECIMAL(12,1)) as CountsFails" +
                " FROM vwForm join app_settings on app_settings.appname = vwForm.appname  where " +
                " vwForm.appname = '" & Session("appname") & "' " +
                agent_group_filter +
                "and vwForm.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' ")



        lblAvgScore.Text = dtAvgScore.Rows(0).Item(1).ToString



        Dim toal_failed As DataTable
        Select Case UCase(Session("appname"))
            Case "EDSOUP"
                'toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b join form_score3 f on f.review_id = b.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and has_cardinal = 1  and f.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
                toal_failed = GetTable("select count(*) as num_left from vwForm  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
            Case Else
                toal_failed = GetTable("select count(*) as num_left from vwForm  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
        End Select



        start_date = txtStartDate.Text
        end_date = txtEndDate.Text


        Dim missed_sql As String = "select COUNT(*) as num_calls, case when convert(decimal (10,0),convert(float,sum(right_answer ^ 1))/convert(float,COUNT(*))* 100)  > 10 then 'red' " &
                    "when convert(decimal (10,0),convert(float,sum(right_answer ^ 1))/convert(float,COUNT(*))* 100) between 6 and 10 then 'yellow2' " &
                    "when convert(decimal (10,0),convert(float,sum(right_answer ^ 1))/convert(float,COUNT(*))* 100) between 3 and 6 then 'green' " &
                    "when convert(decimal (10,0),convert(float,sum(right_answer ^ 1))/convert(float,COUNT(*))* 100) < 3 then 'green2' end as div_color, " &
                    "sum(right_answer ^ 1) as total_wrong,convert(decimal (10,0),convert(float,sum(right_answer ^ 1))/convert(float,COUNT(*))* 100)  as Percent_Qs, " &
                    "form_q_scores.question_id, q_short_name  from form_q_scores " &
                    "join vwform on vwform.F_id = form_q_scores.form_id " &
                    "join Questions on Questions.id = form_q_scores.question_id " &
                    "join question_answers on question_answers.id = form_q_scores.question_answered "
        'If Session("appname") = "edsoup" Then
        '    missed_sql &= "left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from vwform  " & _
        '            "left join notifications on notifications.form_id = vwform.F_id where acknowledged is null  " & _
        '            "and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' and vwform.appname = '" & Session("appname") & "'  group by reviewer) a  " & _
        '            "on vwform.reviewer = a.reviewer and review_date < a.min_review  "
        'End If
        missed_sql &= "where call_made_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' " &
            "and vwform.appname = '" & Session("appname") & "' " &
            agent_group_filter &
            "group by form_q_scores.question_id, q_short_name " &
            "order by Percent_Qs desc "

        'Response.Write("<!--" & missed_sql & "-->")


        Dim missed_dt As DataTable = GetTable(missed_sql)


        rptMissed.DataSource = missed_dt
        rptMissed.DataBind()

        Dim line_dt As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score/top_score * 100)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from vwForm join app_settings on app_settings.appname = vwForm.appname  where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() " & agent_group_filter & " and vwForm.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")

        For Each dr As DataRow In line_dt.Rows
            Dim calldate As Date = dr("CallDate")
            Dim date_string As String = "new Date(" & Year(calldate) & ", " & Month(calldate) - 1 & ", " & Day(calldate) & ", 0, 0, 0)"
            line_graph_data &= ",[" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]" & Chr(13)
            'line_graph_data &= "data.addRow([" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]);" & Chr(13)
        Next

        'line_graph_data = Left(line_graph_data, Len(line_graph_data) - 2)

        Dim line_range_dt As DataTable = GetTable("select min(CONVERT(date, call_date)) as MinCallDate, " &
            "max(CONVERT(date, call_date)) as MaxCallDate " &
            "from vwForm where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() and vwForm.appname = '" & Session("appname") & "'" & agent_group_filter)

        If Not IsDBNull(line_range_dt.Rows(0).Item("MinCallDate")) Then
            min_date = line_range_dt.Rows(0).Item("MinCallDate")
            max_date = line_range_dt.Rows(0).Item("MaxCallDate")

            min_line_range = "new Date(" & min_date.Year & ", " & min_date.Month & ", " & min_date.Day & ")"
            max_line_range = "new Date(" & max_date.Year & ", " & max_date.Month & ", " & max_date.Day & ")"

        End If






        Dim agent_dt As DataTable = GetTable("select convert(varchar(10),Convert(decimal(10,2),[Average Score])) + '%' as avg_score,  case when [Average Score]  between -200 and 79 then 'red' when [Average Score]  between 80 and 89 then 'yellow2' " &
                            "when [Average Score]  between 90 and 99 then 'green' when [Average Score]  = 100 then 'green2' end as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & " 11:59:59 pm' as end_date, * from " &
                            "(SELECT  distinct XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score/top_score * 100)) as [Average Score] " &
                            "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " &
                            "join  app_settings on app_settings.appname = form_score3.appname  " &
                            " left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                            "left join form_notifications on form_notifications.form_id = form_score3.id where date_closed is null and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' and appname = '" & Session("appname") & "'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " &
                            "where form_score3.appname = '" & Session("appname") & "' " &
                            agent_group_filter &
                            " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' " &
                            "group By XCC_REPORT_NEW.AGENT, agent_group   " &
                            ") a ORDER BY [Average Score]")


        gvTBAgents.DataSource = agent_dt
        gvTBAgents.DataBind()



        If Session("applied_filter") IsNot Nothing Then
            If Session("applied_filter") <> "" Then
                agent_group_filter = agent_group_filter & Session("applied_filter")
            End If
        End If


        'If Session("appname") = "edsoup" Then
        '    dsQADetails.SelectCommand = "select agent,replace(missed_list,',',', ') as missed_list, convert(varchar(10), call_date, 101) as call_date, f_id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, " & _
        '                                "dbo.ConvertTimeToHHMMSS(Call_length,'s') as Call_length, num_missed,total_score,Isnull(phone, isnull(dnis, isnull(ani,''))) as dnis,upper(left(vwForm.reviewer,1)) + substring(vwForm.reviewer,2,1000) as reviewer, replace(replace(comments,char(13),''), char(10),'')  + dbo.getCannedComments(f_id) as Comments  " & _
        '                                "from vwForm   " & _
        '                                "left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 " & _
        '                                "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' group by reviewer) a  " & _
        '                                "on vwForm.reviewer = a.reviewer and vwForm.review_date > a.min_review  " & _
        '                                "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' and vwForm.appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
        'Else
        dsQADetails.SelectCommand = "select agent,replace(missed_list,',',', ') as missed_list,convert(varchar(10), call_date, 101) as call_date,f_id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, " & _
                                "dbo.ConvertTimeToHHMMSS(Call_length,'s') as Call_length, num_missed,total_score,Isnull(phone, isnull(dnis, isnull(ani,''))) as dnis,upper(left(vwForm.reviewer,1)) + substring(vwForm.reviewer,2,1000) as reviewer, replace(replace(comments,char(13),''), char(10),'')  + dbo.getCannedComments(f_id) as Comments " & _
                                "from vwForm  " & _
                                "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' and appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
        'End If


        gvQADetails.DataBind()

    End Sub

    'Protected Sub btnGo_Click(sender As Object, e As System.EventArgs) Handles btnGo.Click
    '    Recalc_Elements()

    'End Sub

    'Protected Sub btnPhoneNumber_Click(sender As Object, e As System.EventArgs) Handles btnPhoneNumber.Click
    '    If Not txtSearch.Text Is String.Empty Then
    '        Response.Redirect("Search.aspx?phone=" + txtSearch.Text)
    '    End If
    'End Sub

    'Protected Sub btnAgent_Click(sender As Object, e As System.EventArgs) Handles btnAgent.Click
    '    If Not txtSearch.Text Is String.Empty Then

    '        Response.Redirect("Search.aspx?Agent=" + txtSearch.Text)
    '    End If
    'End Sub

    'Protected Sub btnRecord_Click(sender As Object, e As System.EventArgs) Handles btnRecord.Click
    '    If Not txtSearch.Text Is String.Empty Then
    '        Response.Redirect("review_record.aspx?ID=" + txtSearch.Text)
    '    End If
    'End Sub

    Protected Sub repeatQAListy(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) 'Handles repeatQAList.ItemDataBound
        Dim drv As DataRowView = e.Item.DataItem
        Dim lbl As Label = e.Item.FindControl("lblTotalReview")
        'Dim total_mins As Integer = drv.Item("total_mins")
        Dim total_secs As Integer = drv.Item("total_secs")
        'Dim addl_mins As Integer = total_secs / 60
        Dim hours As Integer = Math.Truncate(total_secs / 3600) ' (total_mins + addl_mins) / 60
        Dim total_mins As Integer = Math.Truncate((total_secs - hours * 3600) / 60)

        Dim hours_string As String = Right("00" & hours, 2)
        Dim minutes_string As String = Right("00" & total_mins, 2)
        Dim seconds_string As String = Right("00" & (total_secs Mod 60), 2)

        If hours > 0 Then
            lbl.Text = hours_string & ":" & minutes_string & ":" & seconds_string
        Else
            lbl.Text = minutes_string & ":" & seconds_string
        End If

        lbl = e.Item.FindControl("ReviewHour")
        lbl.Text = FormatNumber(3600 / (drv.Item("total_secs") / drv.Item("Number Reviews")), 2)

    End Sub


    Protected Sub dsQAReport_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs)

        e.Command.CommandTimeout = 90

    End Sub


    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) 'Handles btnSearch.Click


        'If Not txtSearch.Text Is String.Empty Then
        '    Select Case ddlSearch.SelectedValue
        '        Case "Phone Number"
        '            Response.Redirect("Search.aspx?phone=" + txtSearch.Text)
        '        Case "Agent Name"
        '            Response.Redirect("Search.aspx?Agent=" + txtSearch.Text)
        '        Case "Record ID"
        '            Response.Redirect("review_record.aspx?ID=" + txtSearch.Text)
        '    End Select

        'End If


    End Sub

    Protected Sub btnApplyFilter_Click(sender As Object, e As EventArgs) Handles btnApplyFilter.Click
        Recalc_Elements()
    End Sub

    Protected Sub btnZeroData_Click(sender As Object, e As EventArgs) 'Handles btnZeroData.Click
        'AFL.StartDate = txtStartDate.Text
        'AFL.EndDate = txtEndDate.Text
        'AFL.Score_Range = "0"
        'AFL.UpdateView()

    End Sub



    Protected Sub gvQADetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQADetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(0).Text = "success" Then
                e.Row.Cells(0).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            Else
                e.Row.Cells(0).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
                e.Row.Attributes.Add("class", "fail-row")
            End If


            'If e.Row.Cells(3).Text <> "&nbsp;" Then
            '    Dim seconds As Integer = CInt(e.Row.Cells(3).Text)
            '    Dim ts As New TimeSpan(0, 0, seconds)
            '    Dim s As String = String.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"))
            '    e.Row.Cells(3).Text = s
            'End If





            Dim drv As DataRowView = e.Row.DataItem


            If Len(e.Row.Cells(8).Text) > 1 And Not no_details Then
                Dim replacement_text As String = ""
                Dim mq_list() As String = e.Row.Cells(8).Text.Split(",")
                For Each mq As String In mq_list

                    Dim q_pos As DataTable = GetTable("select case when q_position > call_length then call_length - 100 else q_position end as q_position, audio_link, " & _
                        "vwForm.F_id as form_id from vwForm join  form_q_scores on vwForm.F_id= form_q_scores.form_id " & _
                        "join Questions on questions.id = form_q_scores.question_id  " & _
                        "where vwForm.F_id = " & drv.Item("form_id").ToString & " and q_short_name='" & Trim(mq) & "'")

                    If q_pos.Rows.Count > 0 Then
                        If replacement_text <> "" Then
                            replacement_text &= ", <a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");'>" & mq & "</a>"
                        Else
                            replacement_text &= "<a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");'>" & mq & "</a>"
                        End If
                    End If
                Next
                e.Row.Cells(8).Text = replacement_text
            End If

        End If
        'If e.Row.RowType = DataControlRowType.Footer Then
        '    Dim 
        '    e.Row.Cells(0).Controls.Add()
        'End If

    End Sub

    Protected Sub btnExportDetails()

        no_details = True
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=DetailReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvQADetails.AllowPaging = False
        Recalc_Elements()
        gvQADetails.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Protected Sub gvQADetails_PreRender(sender As Object, e As EventArgs) Handles gvQADetails.PreRender
        If gvQADetails.HeaderRow IsNot Nothing Then
            gvQADetails.HeaderRow.TableSection = TableRowSection.TableHeader
        End If


        'If gvQADetails.FooterRow IsNot Nothing Then
        '    gvQADetails.FooterRow.TableSection = TableRowSection.TableFooter
        'End If

    End Sub


    Protected Sub gvQADetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvQADetails.PageIndexChanging
        Recalc_Elements()
    End Sub


    Protected Sub getPageNums(sender As Object, e As EventArgs)
        Dim bl As BulletedList = DirectCast(sender, BulletedList)
        Dim numPages As Integer = gvQADetails.PageSize

        If bl.Items.Count = 0 Then
            For i As Integer = 1 To numPages
                bl.Items.Add(New ListItem(("" & i), ("Page$" & i)))
                bl.Items(i - 1).Enabled = Not (gvQADetails.PageIndex = (i - 1))
            Next
        End If
    End Sub

    Protected Sub blPageNums_Click(sender As Object, args As EventArgs)
        'ListItem li = (ListItem)sender;
        'int page = Convert.ToInt32(li.Text) - 1;

        Dim bl As BulletedList = DirectCast(Page.FindControl("blPageNums"), BulletedList)
        gvQADetails.PageIndex = bl.SelectedIndex
    End Sub


    Protected Sub GridView1_RowCreated(sender As Object, e As GridViewRowEventArgs) 'Handles gvQADetails.RowCreated
        If e.Row.RowType = DataControlRowType.Pager Then
            Dim bl As New BulletedList
            bl.CssClass = "table-navigation"

            Dim index As Integer
            For index = 1 To gvQADetails.PageCount

                If index = gvQADetails.PageIndex + 1 Then
                    Dim li As New ListItem
                    li.Text = index.ToString
                    li.Attributes.Add("class", "selected-page")
                    Dim lbl As New Label()
                    lbl.Text = "<span style=""color:red;""> </span> <b style=""background-color: Silver;color:red;border-style:solid;border-color:silver;border-width:2px;padding:2px 2px 2px 2px"">" & index.ToString() & "</b> "
                    e.Row.Cells(0).Controls.Add(lbl)
                    bl.Items.Add(li)

                Else

                    Dim li As New ListItem
                    li.Text = index.ToString

                    Dim linkbutton As New LinkButton()
                    linkbutton.ID = "LinkPage" & index
                    linkbutton.CommandName = "Page"
                    linkbutton.CommandArgument = index.ToString()
                    linkbutton.Text = index.ToString()
                    linkbutton.CssClass = "link"
                    Dim sw As New StringWriter()
                    Dim hw As New HtmlTextWriter(sw)

                    linkbutton.RenderControl(hw)
                    li.Text = sw.ToString.Replace("&lt;", "<").Replace("&gt;", ">")
                    bl.Items.Add(li)

                End If

            Next
            e.Row.Cells(0).Controls.Add(bl)
        End If


        'Dim ddlPageSize As New DropDownList()
        'ddlPageSize.AutoPostBack = True
        'ddlPageSize.SelectedIndexChanged += New EventHandler(ThreadPageSizeList_SelectedIndexChanged)
        'ddlPageSize.Items.Clear()
        'Dim pageSizeOptions As Integer() = New Integer() {10, 20, 50, 100}

        'For i As Integer = 0 To pageSizeOptions.Length - 1
        '    ddlPageSize.Items.Add(pageSizeOptions(i).ToString())
        'Next

        'Dim item As ListItem = ddlPageSize.Items.FindByText(Me.PageSize.ToString())
        'If item IsNot Nothing Then
        '    ddlPageSize.SelectedIndex = ddlPageSize.Items.IndexOf(item)
        'End If

        'Dim pagerTable As Table = TryCast(e.Row.Cells(0).Controls(0), Table)
        'Dim cell As New TableCell()
        'cell.Controls.Add(New System.Web.UI.LiteralControl("Page Size:"))
        'cell.Controls.Add(ddlPageSize)
        'pagerTable.Rows(0).Cells.Add(cell)

    End Sub

    Protected Sub btnTopAgents()
        Response.Redirect("all_agents.aspx")
    End Sub


    Protected Sub txtStartDate_TextChanged(sender As Object, e As EventArgs) Handles txtStartDate.TextChanged
        Session("StartDate") = txtStartDate.Text
    End Sub

    Protected Sub txtEndDate_TextChanged(sender As Object, e As EventArgs) Handles txtEndDate.TextChanged
        Session("EndDate") = txtEndDate.Text
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
                ddlAgent.Items.Add(New ListItem("All Agents", ""))

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
        ddlAgent.Items.Add(New ListItem("All Agents", ""))

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


    Protected Sub btnViewAllCalls()
        Response.Redirect("expandedview.aspx")
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        gvQADetails.AllowPaging = False
        gvQADetails.DataBind()

        'Response.Clear()
        Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter()
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
        gvQADetails.RenderControl(htmlWrite)
        'Response.Write(stringWrite.ToString())
        Dim this_div As New HtmlGenericControl
        this_div.InnerHtml = stringWrite.ToString()
        Session("ctrl") = this_div
        ClientScript.RegisterStartupScript(Me.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>")
        gvQADetails.AllowPaging = True
        gvQADetails.DataBind()
    End Sub


    Protected Sub rptMissed_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptMissed.ItemCommand
        If e.CommandName = "Question" Then
            Session("applied_filter") = " and form_score3.id in (select form_id from dbo.GetFormListByShortName('" & e.CommandArgument & "','" & Session("appname") & "')) "
            Recalc_Elements()
            ClientScript.RegisterStartupScript(Me.GetType(), "refocus", "<script language=javascript>window.scrollTo(0, document.body.scrollHeight);</script>")
            gvQADetails.FooterRow.Focus()
        End If
    End Sub

    Protected Sub gvTBAgents_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTBAgents.RowCommand
        Session("applied_filter") = " and agent = '" & e.CommandArgument & "'"
        Recalc_Elements()
        ClientScript.RegisterStartupScript(Me.GetType(), "refocus", "<script language=javascript>window.scrollTo(0, document.body.scrollHeight);</script>")
        gvQADetails.FooterRow.Focus()
    End Sub

    Protected Sub lbSubmitPie_Click(sender As Object, e As EventArgs) Handles lbSubmitPie.Click
        Select Case hdnSelectedRange.Value
            Case "< 80"
                Session("applied_filter") = " and total_score < 80 "
                Recalc_Elements()
            Case "80-90"
                Session("applied_filter") = " and total_score >= 80 and total_score < 90"
                Recalc_Elements()
            Case "90-100"
                Session("applied_filter") = " and total_score >= 90 and total_score < 100"
                Recalc_Elements()
            Case "100"
                Session("applied_filter") = " and total_score >= 100"
                Recalc_Elements()
        End Select

    End Sub

End Class
