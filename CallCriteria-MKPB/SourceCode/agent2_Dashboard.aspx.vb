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


        If Request("agent") Is Nothing And Not User.IsInRole("Agent") Then
            Response.Redirect("login.aspx")
        End If

        'If Session("appname") Is Nothing Then
        '    Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        '    Session("appname") = domain(0)
        'End If


        Response.Redirect("cd2.aspx")

        If Request("agent") IsNot Nothing Then
            hdnThisAgent.Value = Request("agent")
            Session("thisAgent") = Request("agent")
            'Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, "agent", DateTime.Now, DateTime.Now.AddMinutes(30), False, String.Empty, FormsAuthentication.FormsCookiePath)
            FormsAuthentication.SetAuthCookie("agent", True)
        Else
            If User.IsInRole("Agent") Then
                hdnThisAgent.Value = User.Identity.Name
            End If
        End If


        If Not IsPostBack Then

            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "dashboard collapsed-menu")


            Dim totalDaysinMonth As Int32 = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)

            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                txtStartDate.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Else
                txtStartDate.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                txtEndDate.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
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


    Protected Sub Recalc_Elements()

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"

        ' Dim dsQAReport As SqlDataSource = CType(lvQAList.FindControl("dsQAReport"), SqlDataSource)


        Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
        If userProfile.Group <> "" Then

            agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "

        End If

        agent_group_filter &= " and xcc_report_new.agent = '" & hdnThisAgent.Value & "' "


        'Response.Write("<!--" & agent_group_filter & "-->")

        'If dsQAReport IsNot Nothing Then
        '    If ddlGroup.SelectedValue <> "" Then
        '        dsQAReport.FilterExpression = agent_group_filter.Replace("and", "")
        '        dsQAReport.DataBind()
        '        Response.Write("<!--" & agent_group_filter & "-->")
        '    Else
        '        dsQAReport.FilterExpression = ""
        '        dsQAReport.DataBind()
        '        Response.Write("<!--" & agent_group_filter & "-->")
        '    End If
        '    dsQAReport.DataBind()
        'End If

        'dsQAReport.DataBind()

        hdnAgentFilter.Value = agent_group_filter

        AFL.Agent = hdnThisAgent.Value

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
                toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW  with (nolock)  join form_score3  with (nolock)  on form_score3.review_id = xcc_report_new.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and form_score3.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
            Case Else
                toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW  with (nolock)  join form_score3  with (nolock)  on form_score3.review_id = xcc_report_new.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and form_score3.appname = '" & Session("appname") & "' and call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'" & agent_group_filter)
        End Select



        start_date = txtStartDate.Text
        end_date = txtEndDate.Text

        'txtEndDate.Text = txtEndDate.Text & " 11:59:59 pm"


        Dim missed_dt As DataTable
        '= GetTable("SELECT  top 5 '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, QUESTIONS.id, Questions.q_short_name, round(convert(float, " & _
        '                                        "SUM (q_wrong))/convert(float,sum(q_right + q_wrong)),2) * 100 as Percent_Qs, sum(q_wrong) as total_wrong " & _
        '                                        "from q_summary_data with (nolock) join questions with (nolock)  ON q_summary_data.id = questions.id " & _
        '                                        "WHERE     (q_summary_data.call_date BETWEEN '" & txtStartDate.Text & "' AND '" & txtEndDate.Text & " 11:59:59 pm') and questions.appname='" & Session("appname") & "' " & _
        '                                        agent_group_filter & _
        '                                        " GROUP BY Questions.q_short_name, QUESTIONS.id order by Percent_Qs desc")


        missed_dt = GetTable("select '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, " &
        "COUNT(*), count(min_answers.id) as total_wrong, convert(int,convert(float,COUNT(min_answers.id))/convert(float,COUNT(*)) * 100) as percent_qs, questions.q_short_name, questions.id as id from " &
        "form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID " &
        "join form_q_scores with (nolock) on form_q_scores.form_id = form_score3.id " &
        "join questions with (nolock) on questions.id = form_q_scores.question_id  " &
        "left join min_answers with (nolock) on min_answers.question_id = questions.id and min_answers.id = form_q_scores.question_answered  " &
        "where call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & " 11:59:59 pm' " & agent_group_filter &
        "group by questions.q_short_name, questions.id " &
        "having COUNT(min_answers.id) > 0 " &
        "order by convert(float,COUNT(min_answers.id))/convert(float,COUNT(*)) desc ")

        rptMissed.DataSource = missed_dt
        rptMissed.DataBind()

        'Dim line_dt As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID  where call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " & agent_group_filter & " and form_score3.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")

        'For Each dr As DataRow In line_dt.Rows
        '    line_graph_data &= ",['" & dr("CallDate") & "', " & dr("AvgScore") & "," & dr("NumReviewed") & "]" & Chr(13)
        'Next


        Dim line_dt As DataTable = GetTable("select convert(decimal(10,2),AVG(total_score)) as AvgScore, count(*) as NumReviewed, CONVERT(date, call_date) as CallDate from form_score3 with (nolock) join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID  where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() " & agent_group_filter & " and form_score3.appname = '" & Session("appname") & "'  group by CONVERT(date, call_date) order by CONVERT(date, call_date) ")

        For Each dr As DataRow In line_dt.Rows
            Dim calldate As Date = dr("CallDate")
            Dim date_string As String = "new Date(" & Year(calldate) & ", " & Month(calldate) - 1 & ", " & Day(calldate) & ", 0, 0, 0)"
            line_graph_data &= ",[" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]" & Chr(13)
            'line_graph_data &= "data.addRow([" & date_string & ", " & dr("AvgScore") & "," & dr("NumReviewed") & "]);" & Chr(13)
        Next

        'line_graph_data = Left(line_graph_data, Len(line_graph_data) - 2)

        Dim line_range_dt As DataTable = GetTable("select min(CONVERT(date, call_date)) as MinCallDate, " &
            "max(CONVERT(date, call_date)) as MaxCallDate " &
            "from form_score3 with (nolock)  " &
            "join XCC_REPORT_NEW with (nolock) on XCC_REPORT_NEW.ID = form_score3.review_ID   " &
            "where call_date between dateadd(dd, -90, dbo.getMTDate()) and dbo.getMTDate() and form_score3.appname = '" & Session("appname") & "' " & agent_group_filter)



        Try
            min_date = line_range_dt.Rows(0).Item("MinCallDate")
            max_date = line_range_dt.Rows(0).Item("MaxCallDate")
        Catch ex As Exception
            min_date = txtStartDate.Text
            max_date = txtEndDate.Text
        End Try


        min_line_range = "new Date(" & min_date.Year & ", " & min_date.Month & ", " & min_date.Day & ")"
        max_line_range = "new Date(" & max_date.Year & ", " & max_date.Month & ", " & max_date.Day & ")"



        'Dim agent_dt As DataTable = GetTable("select 'green' as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from " & _
        '                    "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " & _
        '                    "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " & _
        '                    " join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 " & _
        '                    "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and appname = 'edsoup'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " & _
        '                    "where form_score3.appname = '" & Session("appname") & "' " & _
        '                    agent_group_filter &
        '                    " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " & _
        '                    "group By XCC_REPORT_NEW.AGENT, agent_group   " & _
        '                    "ORDER BY [Average Score] DESC)  a " & _
        '                    "union all " & _
        '                    "select 'red'  as div_color, '" & txtStartDate.Text & "' as start_date, '" & txtEndDate.Text & "' as end_date, * from  " & _
        '                    "(SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName,AGENT, agent_group, convert(int,avg(form_score3.total_score)) as [Average Score] " & _
        '                    "FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID  " & _
        '                    " join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 " & _
        '                    "left join notifications on notifications.form_id = form_score3.id where acknowledged is null and appname = 'edsoup'  group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date < a.min_review " & _
        '                    "where form_score3.appname = '" & Session("appname") & "'  " & _
        '                    agent_group_filter &
        '                    " and XCC_REPORT_NEW.call_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' " & _
        '                    "group By XCC_REPORT_NEW.AGENT, agent_group  " & _
        '                    "ORDER BY [Average Score] asc) b  where 1=1 " & _
        '                    " ORDER BY [Average Score] desc")



        'gvTBAgents.DataSource = agent_dt
        'gvTBAgents.DataBind()

        'If Session("appname") = "edsoup" Then
        '    dsQADetails.SelectCommand = "select *, missed_list, form_score3.id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, replace(replace(comments,char(13),''''), char(10),'''')  + dbo.getCannedComments(form_score3.id) as all_comments  " & _
        '                                      "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " & _
        '                                      "left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 " & _
        '                                        "left join form_notifications on form_notifications.form_id = form_score3.id where date_closed is null group by reviewer) a  " & _
        '                                        "on form_score3.reviewer = a.reviewer and form_score3.review_date > a.min_review  " & _
        '                                      "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
        'Else
        dsQADetails.SelectCommand = "select *, missed_list, form_score3.id as form_id, case when total_score >= 80 then 'success' else 'fail' end as pass_fail, replace(replace(comments,char(13),''''), char(10),'''')  + dbo.getCannedComments(form_score3.id) as all_comments  " & _
                                "from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id  " & _
                                "where review_date between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "' and form_score3.appname='" & Session("appname") & "' " & agent_group_filter & " order by review_date desc"
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

    Protected Sub gvQADetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvQADetails.PageIndexChanging
        Recalc_Elements()
    End Sub



    Protected Sub gvQADetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQADetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(6).Text = "success" Then
                e.Row.Cells(6).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            Else
                e.Row.Cells(6).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
                e.Row.Attributes.Add("class", "fail-row")
            End If


            If e.Row.Cells(3).Text <> "&nbsp;" Then
                Dim seconds As Integer = CInt(e.Row.Cells(3).Text)
                Dim ts As New TimeSpan(0, 0, seconds)
                Dim s As String = String.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"))
                e.Row.Cells(3).Text = s
            End If

            'Dim pfl As ProfileCommon = Profile.GetProfile(e.Row.DataItem("reviewer"))
            'Dim img As Image = e.Row.FindControl("imgAvatar")
            'If pfl.Gender = "F" Then
            '    img.ImageUrl = "images/female-avatar.png"
            'Else
            '    img.ImageUrl = "images/male-avatar.png"
            'End If


            Dim drv As DataRowView = e.Row.DataItem
            If drv.Item("all_comments").ToString <> "" Then
                Dim lit As Literal = e.Row.FindControl("Literal3")
                lit.Text = drv.Item("all_comments").ToString
                '"<a class='comments-trigger' style='padding:10px;' href='#'><i class='fa fa-file'><div class='full-question-tooltip'  style='overflow:visible;dislay:block;z-index:1000;'>" & Trim(drv.Item("all_comments").ToString) & "</div></i></a>"
            End If

        End If
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

    'Protected Sub getPageNums(sender As Object, e As EventArgs)
    '    Dim bl As BulletedList = DirectCast(sender, BulletedList)
    '    Dim numPages As Integer = gvQADetails.PageSize

    '    If bl.Items.Count = 0 Then
    '        For i As Integer = 1 To numPages
    '            bl.Items.Add(New ListItem(("" & i), ("Page$" & i)))
    '            bl.Items(i - 1).Enabled = Not (gvQADetails.PageIndex = (i - 1))
    '        Next
    '    End If
    'End Sub

    'Protected Sub blPageNums_Click(sender As Object, args As EventArgs)
    '    'ListItem li = (ListItem)sender;
    '    'int page = Convert.ToInt32(li.Text) - 1;

    '    Dim bl As BulletedList = DirectCast(Page.FindControl("blPageNums"), BulletedList)
    '    gvQADetails.PageIndex = bl.SelectedIndex
    'End Sub


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

    Protected Sub btnMyNotifications_Click(sender As Object, e As EventArgs) Handles btnMyNotifications.Click
        Response.Redirect("agent_notifications.aspx?agent=" & hdnThisAgent.Value)
    End Sub

End Class
