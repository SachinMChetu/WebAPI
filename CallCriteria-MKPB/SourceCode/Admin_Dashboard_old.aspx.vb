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
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        If Not Roles.IsUserInRole("Admin") And Not Roles.IsUserInRole("Supervisor") And Not Roles.IsUserInRole("Manager") And Not Roles.IsUserInRole("QA Lead") Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

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

            Recalc_Elements()

        End If




    End Sub


    'Protected Sub btnChangeDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeDate.Click
    '    dsMySessions.SelectParameters("selectedDate").DefaultValue = txt_date.Text
    '    gvUserSessions.DataBind()
    'End Sub


    Protected Sub ddlSupeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridViewRow = sender.parent.parent
        Dim userProfile As ProfileCommon = ProfileCommon.Create(row.Cells(3).Text)
        'Try
        userProfile.Supervisor = sender.SelectedValue
        userProfile.Save()

    End Sub

    Protected Sub ddlGroupChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridViewRow = sender.parent.parent
        Dim userProfile As ProfileCommon = ProfileCommon.Create(row.Cells(3).Text)
        'Try
        userProfile.Group = sender.SelectedValue
        userProfile.Save()

    End Sub

    Protected Sub Recalc_Elements()

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"

        ' Dim dsQAReport As SqlDataSource = CType(lvQAList.FindControl("dsQAReport"), SqlDataSource)

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

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

                



            End If
        End If


        If ddlGroup.SelectedValue <> "" Then
            litGroupFilter.Text = ddlGroup.SelectedValue
            agent_group_filter = " and AGENT_GROUP = '" & ddlGroup.SelectedValue & "' "
        Else
            litGroupFilter.Text = "All Groups"
        End If


        If ddlAgent.SelectedValue <> "" Then
            litGroupFilter.Text &= " " & ddlAgent.SelectedValue
            agent_group_filter &= " and AGENT = '" & ddlAgent.SelectedValue & "' "
        End If


        'check for already selected Agent 
        Dim agent_selected As String = ddlAgent.SelectedValue

        If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
        End If

        ddlAgent.DataBind()

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

        Dim fiftytoseventyRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','-100','80','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        'GetTable("select count(*) from form_score3 with (nolock)  join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where total_score > 50 and total_score <= 70 and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        hdn50to70.Text = If(fiftytoseventyRows.Rows.Count > 0, fiftytoseventyRows(0).Item(0).ToString(), "0")

        Dim seventytoeightyRows As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','80','90','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        'GetTable("select count(*) from form_score3 with (nolock)  join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where total_score > 70 and total_score <= 80 and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        hdn70to80.Text = If(seventytoeightyRows.Rows.Count > 0, seventytoeightyRows(0).Item(0).ToString(), "0")

        Dim eightytoninty As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','90','99','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        'GetTable("select count(*) from form_score3 with (nolock)  join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where total_score > 80 and total_score <= 90 and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        hdn80to90.Text = If(eightytoninty.Rows.Count > 0, eightytoninty(0).Item(0).ToString(), "0")

        Dim nintyplus As DataTable = GetTable("getRangeCount '" & txtStartDate.Text & "','" & txtEndDate.Text & "','99','200','" & Session("appname") & "','" & agent_group_filter.Replace("'", "''") & "'")
        'GetTable("select count(*) from form_score3 with (nolock)  join xcc_report_new with (nolock)  on form_score3.review_id = xcc_report_new.id where total_score > 90 and call_date between '" + Convert.ToDateTime(txtStartDate.Text) + "' and '" + Convert.ToDateTime(txtEndDate.Text) + " 11:59:59 pm' and form_score3.appname = '" & Session("appname") & "'" + agent_group_filter).Rows
        hdn90plus.Text = If(nintyplus.Rows.Count > 0, nintyplus(0).Item(0).ToString(), "0")



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


        Dim toal_failed As DataTable
        Select Case UCase(Session("appname"))
            Case "EDSOUP"
                'toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b join form_score3 f on f.review_id = b.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and has_cardinal = 1  and f.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
                toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b with (nolock)  join form_score3 f with (nolock)  on f.review_id = b.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and f.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
            Case Else
                toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b with (nolock)  join form_score3 f with (nolock)  on f.review_id = b.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and f.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
        End Select



        start_date = txtStartDate.Text
        end_date = txtEndDate.Text

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"


        Dim missed_dt As DataTable = GetTable("SELECT  top 5 case when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100  > 10 then 'red' " &
                                        "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 between 6 and 9 then 'yellow2' " &
                                        "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 between 3 and 6 then 'green' " &
                                        "when round(convert(float,SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100  < 3 then 'green2' end as div_color, " &
                                        "'" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, QUESTIONS.id, Questions.q_short_name, round(convert(float, " &
                                        "SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 as Percent_Qs, sum(q_wrong) as total_wrong " &
                                        "from q_summary_data with (nolock) join questions with (nolock)  ON q_summary_data.id = questions.id " &
                                        "WHERE     (q_summary_data.call_date BETWEEN '" & txtStartDate.Text & "' AND '" & txtEndDate.Text & " 11:59:59 pm') and questions.appname='" & Session("appname") & "' " &
                                        agent_group_filter &
                                        " GROUP BY Questions.q_short_name, QUESTIONS.id order by Percent_Qs desc")

        rptMissed.DataSource = missed_dt
        rptMissed.DataBind()

        Dim line_dt As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID  where call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " & agent_group_filter & " and form_score3.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")

        For Each dr As DataRow In line_dt.Rows
            line_graph_data &= ",['" & dr("CallDate") & "', " & dr("AvgScore") & "," & dr("NumReviewed") & "]" & Chr(13)
        Next


        Dim line_dt_30 As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID  where call_date between dateadd(dd,-30,dbo.getMTDate()) and dbo.getMTDate()  " & agent_group_filter & " and form_score3.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")
        For Each dr As DataRow In line_dt_30.Rows
            line_graph_data_30 &= ",['" & dr("CallDate") & "', " & dr("AvgScore") & "]" & Chr(13)
        Next

        Dim line_dt_60 As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID  where call_date between dateadd(dd,-60,dbo.getMTDate()) and dbo.getMTDate()  " & agent_group_filter & " and form_score3.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")
        For Each dr As DataRow In line_dt_60.Rows
            line_graph_data_60 &= ",['" & dr("CallDate") & "', " & dr("AvgScore") & "]" & Chr(13)
        Next



        Dim agent_dt As DataTable = GetTable("select  case when [Average Score]  between -200 and 79 then 'red' when [Average Score]  between 80 and 89 then 'yellow2' " &
                            "when [Average Score]  between 90 and 99 then 'green' when [Average Score]  = 100 then 'green2' end as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from " &
                            "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " &
                            "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " &
                            " left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                            "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and appname = '" & Session("appname") & "'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " &
                            "where form_score3.appname = '" & Session("appname") & "' " &
                            agent_group_filter &
                            " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " &
                            "group By XCC_REPORT_NEW.AGENT, agent_group   " &
                            "ORDER BY [Average Score] DESC)  a " &
                            "union all " &
                            "select case when [Average Score]  between -200 and 79 then 'red' when [Average Score]  between 80 and 89 then 'yellow2' " &
                            "when [Average Score]  between 90 and 99 then 'green' when [Average Score]  = 100 then 'green2' end as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from  " &
                            "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " &
                            "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " &
                            " left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                            "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and appname = '" & Session("appname") & "'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " &
                            "where form_score3.appname = '" & Session("appname") & "'  " &
                            agent_group_filter &
                            " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " &
                            "group By XCC_REPORT_NEW.AGENT, agent_group  " &
                            "ORDER BY [Average Score] asc) b  where 1=1 " &
                            agent_group_filter & " ORDER BY [Average Score] desc")



        gvTBAgents.DataSource = agent_dt
        gvTBAgents.DataBind()

        If Session("appname") = "edsoup" Then
            dsQADetails.SelectCommand = "select replace(missed_list,'/',',') as missed_list, form_score3.id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, " &
                                        "Call_length, num_missed,total_score,dnis,upper(left(form_score3.reviewer,1)) + substring(form_score3.reviewer,2,1000) as reviewer " &
                                        "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " &
                                        "left join (select isnull(min(notifications.dateadded),dbo.getMTDate()) as min_review, reviewer from form_score3 " &
                                        "left join notifications on notifications.form_id = form_score3.id where acknowledged is null group by reviewer) a  " &
                                        "on form_score3.reviewer = a.reviewer and form_score3.review_date > a.min_review  " &
                                        "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
        Else
            dsQADetails.SelectCommand = "select replace(missed_list,'/',',') as missed_list, form_score3.id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, " & _
                                    "Call_length, num_missed,total_score,dnis,upper(left(form_score3.reviewer,1)) + substring(form_score3.reviewer,2,1000) as reviewer " & _
                                    "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " & _
                                    "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
        End If



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
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        ddlAgent.Items.Clear()
        ddlAgent.Items.Add(New ListItem("All", ""))

        If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "'  order by AGent")
            ddlAgent.Visible = False
        End If

        If ddlGroup.SelectedIndex > 0 Then
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & txtStartDate.Text & "' and call_date <= '" & txtEndDate.Text & "' order by AGent")
            ddlAgent.Visible = True
        End If
        ddlAgent.DataBind()



        'Response.Write("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' order by AGent")
        'Response.End()

        If ddlGroup.SelectedValue <> "" Then
            Session("SelectedGroup") = ddlGroup.SelectedValue
        End If

        If Session("SelectedGroup") <> "" Then
            Try
                ddlGroup.SelectedValue = Session("SelectedGroup")
            Catch ex As Exception

            End Try

        End If



    End Sub

    Protected Sub gvQADetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQADetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(6).Text = "success" Then
                e.Row.Cells(6).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            Else
                e.Row.Cells(6).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
                e.Row.Attributes.Add("class", "fail-row")
            End If


            If e.Row.Cells(2).Text <> "&nbsp;" Then
                Dim seconds As Integer = CInt(e.Row.Cells(2).Text)
                Dim ts As New TimeSpan(0, 0, seconds)
                Dim s As String = String.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"))
                e.Row.Cells(2).Text = s
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


End Class
