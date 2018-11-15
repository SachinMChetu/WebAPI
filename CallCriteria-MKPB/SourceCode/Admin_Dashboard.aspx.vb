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


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        If Not Roles.IsUserInRole("Admin") And Not Roles.IsUserInRole("Supervisor") And Not Roles.IsUserInRole("Manager") And Not Roles.IsUserInRole("QA Lead") Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        ' Response.Redirect("client_dashboard.aspx")

        If Session("appname") Is Nothing Then
            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            Session("appname") = domain(0)
        End If

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

            'dsMySessions.SelectParameters("selectedDate").DefaultValue = DateAdd(DateInterval.Day, -1, Today).ToString("MM/dd/yyyy")
            'gvUserSessions.DataBind()

            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
            ' Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            ddlAgent.DataBind()


            Recalc_Elements()

        End If


    End Sub

    Protected Sub Recalc_Elements()

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"

        ' Dim dsQAReport As SqlDataSource = CType(lvQAList.FindControl("dsQAReport"), SqlDataSource)

        litGroupFilter.Text = "All Groups"

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "
                litGroupFilter.Text = userProfile.Group
            End If
        End If


        If ddlGroup.SelectedValue <> "" Then
            litGroupFilter.Text = ddlGroup.SelectedValue
            agent_group_filter = " and AGENT_GROUP = '" & ddlGroup.SelectedValue & "' "
        End If


        If ddlAgent.SelectedValue <> "" Then
            litGroupFilter.Text &= " " & ddlAgent.SelectedValue
            agent_group_filter &= " and AGENT = '" & ddlAgent.SelectedValue & "' "
        End If


        'check for already selected Agent 
        Dim agent_selected As String = ddlAgent.SelectedValue

        'ddlAgent.Items.Clear()
        'ddlAgent.Items.Add(New ListItem("All", ""))

        'If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
        '    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
        'Else
        '    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' and agent_group = '" & ddlGroup.SelectedValue & "'  order by AGent")
        'End If

        'ddlAgent.DataBind()

        Try
            ddlAgent.SelectedValue = agent_selected
        Catch ex As Exception

        End Try



        hdnAgentFilter.Value = agent_group_filter

        'getRangeCount '1/1/2014','1/15/2014','0','50','edsoup',''

        'Dim zeroDataRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','-100','0','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        ''GetTable("select count(*) from form_score3 with (nolock) join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where (total_score is null or total_score <= 0) and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        'hdnzeroData.Text = If(zeroDataRows.Rows.Count > 0, zeroDataRows(0).Item(0).ToString(), "0")

        'Dim zerotofiftyDataRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','-100','0','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        ''GetTable("select count(*) from form_score3 with (nolock)  join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where total_score <= 50 and total_score is not null and total_score > 0 and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        'hdnless50.Text = If(zerotofiftyDataRows.Rows.Count > 0, zerotofiftyDataRows(0).Item(0).ToString(), "0")

        Dim fiftytoseventyRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','-400','80','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
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


        litStart.Text = txtStartDate.Text
        litEnd.Text = txtEndDate.Text
        litStart2.Text = txtStartDate.Text
        litEnd2.Text = txtEndDate.Text
        'litStart3.Text = txtStartDate.Text
        'litEnd3.Text = txtEndDate.Text




        Dim dtAvgScore As DataTable = GetTable("SELECT  CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) as Counts, CAST(AVG(total_score) AS DECIMAL(12,2)) as CountsFails" +
                " FROM form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  where " +
                " form_score3.appname = '" & Session("appname") & "' " +
                agent_group_filter +
                "and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' ")

        lblAvgScore.Text = dtAvgScore.Rows(0).Item(0).ToString


        start_date = txtStartDate.Text
        end_date = txtEndDate.Text

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"


        'Dim missed_dt As DataTable = GetTable("SELECT  top 5 case when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100  > 10 then 'red' " & _
        '                                "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 between 6 and 10 then 'yellow2' " & _
        '                                "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 between 3 and 6 then 'green' " & _
        '                                "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100  < 3 then 'green2' end as div_color, " & _
        '                                "'" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, QUESTIONS.id, Questions.q_short_name, round(convert(float, " & _
        '                                "SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 as Percent_Qs, sum(q_wrong) as total_wrong " & _
        '                                "from q_summary_data with (nolock) join questions with (nolock)  ON q_summary_data.id = questions.id " & _
        '                                "WHERE     (q_summary_data.call_date BETWEEN '" & txtStartDate.Text & "' AND '" & txtEndDate.Text & " 11:59:59 pm') and questions.appname='" & Session("appname") & "' " & _
        '                                agent_group_filter & _
        '                                " GROUP BY Questions.q_short_name, QUESTIONS.id order by Percent_Qs desc")


        Dim missed_sql As String = "select top 5 case when convert(decimal (10,1),convert(float,COUNT(min_answers.id))/convert(float,COUNT(*))* 100)  > 10 then 'red' " & _
            "when convert(decimal (10,1),convert(float,COUNT(min_answers.id))/convert(float,COUNT(*))* 100) between 6 and 10 then 'yellow2' " & _
            "when convert(decimal (10,1),convert(float,COUNT(min_answers.id))/convert(float,COUNT(*))* 100) between 3 and 6 then 'green' " & _
            "when convert(decimal (10,1),convert(float,COUNT(min_answers.id))/convert(float,COUNT(*))* 100) < 3 then 'green2' end as div_color, " & _
            "COUNT(min_answers.id) as total_wrong,convert(decimal (10,1),convert(float,COUNT(min_answers.id))/convert(float,COUNT(*))* 100)  as Percent_Qs, form_q_scores.question_id, q_short_name  from form_q_scores " & _
            "left join max_answers on max_answers.id = form_q_scores.question_answered " & _
            "left join min_answers on min_answers.id = form_q_scores.question_answered " & _
            "join form_score3 on form_score3.id = form_q_scores.form_id " & _
            "join xcc_report_new on form_score3.review_id = xcc_report_new.id " & _
            "join Questions on Questions.id = form_q_scores.question_id "
        'If Session("appname") = "edsoup" Then
        '    missed_sql &= "left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3  " & _
        '            "left join notifications on notifications.form_id = form_score3.id where acknowledged is null  " & _
        '            "and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and appname = '" & Session("appname") & "'  group by reviewer) a  " & _
        '            "on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review  "
        'End If
        missed_sql &= "where call_made_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " &
            "and form_score3.appname = '" & Session("appname") & "' " &
            agent_group_filter &
            "group by form_q_scores.question_id, q_short_name " &
            "order by COUNT(min_answers.id) desc "

        Dim missed_dt As DataTable = GetTable(missed_sql)

        rptMissed.DataSource = missed_dt
        rptMissed.DataBind()

        Dim line_dt As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from vwForm  where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() " & agent_group_filter & " and vwForm.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")
        line_graph_data = ""
        For Each dr As DataRow In line_dt.Rows
            Dim calldate As Date = dr("CallDate")
            Dim date_string As String = "new Date(" & Year(calldate) & ", " & Month(calldate) - 1 & ", " & Day(calldate) & ", 0, 0, 0)"
            line_graph_data &= ",[" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]" & Chr(13)
            'line_graph_data &= "data.addRow([" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]);" & Chr(13)
        Next

        'line_graph_data = Left(line_graph_data, Len(line_graph_data) - 2)

        Dim line_range_dt As DataTable = GetTable("select min(CONVERT(date, call_date)) as MinCallDate, " &
            "max(CONVERT(date, call_date)) as MaxCallDate " &
            "from vwForm  " &
            "where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() and vwForm.appname = '" & Session("appname") & "' " & agent_group_filter)



        Try
            min_date = line_range_dt.Rows(0).Item("MinCallDate")
            max_date = line_range_dt.Rows(0).Item("MaxCallDate")
        Catch ex As Exception
            min_date = txtStartDate.Text
            max_date = txtEndDate.Text
        End Try


        min_line_range = "new Date(" & min_date.Year & ", " & min_date.Month & ", " & min_date.Day & ")"
        max_line_range = "new Date(" & max_date.Year & ", " & max_date.Month & ", " & max_date.Day & ")"



        Dim agent_dt As DataTable = GetTable("select  case when [Average Score]  < 80 then 'red' when [Average Score]  between 80 and 89 then 'yellow2' " &
                            "when [Average Score]  between 90 and 99 then 'green' when [Average Score]  = 100 then 'green2' end as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from " &
                            "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " &
                            "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " &
                            " left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                            "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and appname = '" & Session("appname") & "'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " &
                            "where form_score3.appname = '" & Session("appname") & "' " &
                            agent_group_filter &
                            " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " &
                            "group By XCC_REPORT_NEW.AGENT, agent_group   " &
                            "ORDER BY [Average Score] DESC)  a " &
                            "union all " &
                            "select case when [Average Score]  < 80 then 'red' when [Average Score]  between 80 and 89 then 'yellow2' " &
                            "when [Average Score]  between 90 and 99 then 'green' when [Average Score]  = 100 then 'green2' end as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from  " &
                            "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " &
                            "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " &
                            " left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                            "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and appname = '" & Session("appname") & "'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " &
                            "where form_score3.appname = '" & Session("appname") & "'  " &
                            agent_group_filter &
                            " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " &
                            "group By XCC_REPORT_NEW.AGENT, agent_group  " &
                            "ORDER BY [Average Score] asc) b  where 1=1 " &
                            agent_group_filter & " ORDER BY [Average Score] desc")


        gvTBAgents.DataSource = agent_dt
        gvTBAgents.DataBind()

        'If Session("appname") = "edsoup" Then
        '    dsQADetails.SelectCommand = "select agent,missed_list, form_score3.id as form_id, case when total_score >= fail_score then 'success' else 'fail' end as pass_fail, " & _
        '                                "Call_length, num_missed,total_score,dnis,upper(left(form_score3.reviewer,1)) + substring(form_score3.reviewer,2,1000) as reviewer " & _
        '                                "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " & _
        '                                "left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " & _
        '                                "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' group by reviewer) a  " & _
        '                                "on form_score3.reviewer = a.reviewer and form_score3.review_date > a.min_review  " & _
        '                                "join app_settings on app_settings.appname = form_score3.appname " & _
        '                                "where call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by call_date desc"
        'Else
        dsQADetails.SelectCommand = "select agent,missed_list, form_score3.id as form_id, case when total_score >= fail_score then 'success' else 'fail' end as pass_fail, " & _
                                "Call_length, num_missed,total_score,dnis,upper(left(form_score3.reviewer,1)) + substring(form_score3.reviewer,2,1000) as reviewer " & _
                                "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " & _
                                "join app_settings on app_settings.appname = form_score3.appname " & _
                                "where call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by call_date desc"
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

    Protected Sub repeatQAList_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) 'Handles repeatQAList.ItemDataBound
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
        AFL.StartDate = txtStartDate.Text
        AFL.EndDate = txtEndDate.Text
        AFL.Score_Range = "0"
        AFL.UpdateView()

    End Sub

    Protected Sub ddlGroup_DataBound(sender As Object, e As EventArgs) Handles ddlGroup.DataBound

        Session("SelectedGroup") = ddlGroup.SelectedValue

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
                    ddlAgent.Visible = False
                Else
                    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
                    'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
                    ddlAgent.Visible = True
                End If
                ddlAgent.DataBind()
            End If
        End If


        'If Session("SelectedGroup") <> "" Then
        '    Try
        '        ddlGroup.SelectedValue = Session("SelectedGroup")
        '        ddlGroup_SelectedIndexChanged(sender, e)
        '        Recalc_Elements()
        '    Catch ex As Exception

        '    End Try

        'End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        ddlAgent.Items.Clear()
        ddlAgent.Items.Add(New ListItem("All", ""))

        If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
            ' Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            ddlAgent.Visible = False
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

    Protected Sub gvQADetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQADetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(7).Text = "success" Then
                e.Row.Cells(7).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            Else
                e.Row.Cells(7).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
                e.Row.Attributes.Add("class", "fail-row")
            End If


            If e.Row.Cells(3).Text <> "&nbsp;" Then
                Dim seconds As Integer = CInt(e.Row.Cells(3).Text)
                Dim ts As New TimeSpan(0, 0, seconds)
                Dim s As String = String.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"))
                e.Row.Cells(3).Text = s
            End If

            Dim pfl As ProfileCommon = Profile.GetProfile(e.Row.DataItem("reviewer"))
            Dim img As Image = e.Row.FindControl("imgAvatar")

            If pfl.Avatar <> "" Then
                img.ImageUrl = "/Audio/" & e.Row.DataItem("reviewer") & "/" & pfl.Avatar
            Else
                If pfl.Gender = "F" Then
                    img.ImageUrl = "images/female-avatar.png"
                Else
                    img.ImageUrl = "images/male-avatar.png"
                End If
            End If

            If e.Row.Cells(5).Text <> "&nbsp;" Then

                '/expandedview.aspx?filter= and fs.id in(select * from dbo.GetFormListByShortName('E-mail Address','gvd'))

                Dim missed_list() As String = e.Row.Cells(5).Text.Split(",")

                Dim new_list As String = ""
                For Each missed_item In missed_list
                    new_list &= "<a href=" & Chr(34) & "/expandedview.aspx?filter= and fs.id in(select * from dbo.GetFormListByShortName('" & missed_item & "','" & Session("appname") & "')) and fs.reviewer = '" & e.Row.DataItem("reviewer") & "'" & Chr(34) & ">" & missed_item & "</a>, "
                Next

                new_list = Left(new_list, Len(new_list) - 2)

                e.Row.Cells(5).Text = new_list
            
            End If

        End If


        'If e.Row.RowType = DataControlRowType.Footer Then
        '    Dim 
        '    e.Row.Cells(0).Controls.Add()
        'End If

    End Sub

    Protected Sub btnExportDetails()
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=QAReport.xls")
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

    Protected Sub btnViewAllCalls()
        Response.Redirect("expandedview.aspx?filter= and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'")
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



    Protected Sub gvQADetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvQADetails.PageIndexChanging
        Recalc_Elements()
    End Sub

    Protected Sub gvQADetails_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvQADetails.Sorting
        Recalc_Elements()
    End Sub
End Class
