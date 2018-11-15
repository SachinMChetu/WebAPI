Imports System.Data
Imports System.Data.SqlClient
Imports Common

Partial Class CCMasterPage
    Inherits System.Web.UI.MasterPage

    Public next_week As String
    Public app_list As String
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FormsAuthentication.SignOut()
        Response.Redirect("login.aspx")
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim role As String = String.Empty

        Dim theUser As String = String.Empty

        If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
            theUser = System.Web.HttpContext.Current.User.Identity.Name
            'afterLoginDiv.Visible = True
            'beforeLoginDiv.Visible = False

            lblUserName.Text = UpperLeft(theUser)
            Dim user As MembershipUser = Membership.GetUser(theUser)



            Dim lastDate As String = "NA"
            Try
                lastDate = user.LastLoginDate.ToString("d")
            Catch
                lastDate = "NA"

            End Try


            'lblLastLoginDate.Text = lastDate
            If System.Web.HttpContext.Current.User.IsInRole("QA") Then
                'lichart.Visible = False
                role = "QA"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("Supervisor") Then
                'lichart.Visible = True
                role = "Supervisor"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("Manager") Then
                'lichart.Visible = True
                role = "Manager"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("QA Lead") Or System.Web.HttpContext.Current.User.IsInRole("Calibrator") Then
                'lichart.Visible = True
                role = "QA Lead"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("Agent") Then
                'lichart.Visible = False
                role = "Agent"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("User") Then
                'lichart.Visible = False
                role = "User"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("Admin") Then
                'lichart.Visible = True
                role = "Admin"
            ElseIf System.Web.HttpContext.Current.User.IsInRole("Client") Then
                'lichart.Visible = True
                role = "Client"
            End If
            lblRole.Text = role
        Else
            'afterLoginDiv.Visible = False
            'beforeLoginDiv.Visible = True
        End If




        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        Session("appname") = domain(0)

        If Session("appname").ToString.ToLower = "work" Then
            Dim my_dt As DataTable = GetTable("declare @appnames varchar(1000);select @appnames = COALESCE(@appnames + ''',''', '') + appname  from userapps where username = '" & theUser & "';select '''' + isnull(@appnames,'edsoup') + ''''")
            app_list = my_dt.Rows(0).Item(0)
        Else
            app_list = "'" & Session("appname") & "'"
        End If


        If Request("agent") IsNot Nothing Then
            hlHome.NavigateUrl = "agent2_dashboard.aspx?agent=" & Request("agent")
        End If


        If Session("my_speed") IsNot Nothing Then
            Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & theUser & "'")
            Session("my_speed") = (user_info_dt.Rows(0).Item("speed_increment") / 100).ToString
        End If

        If Roles.IsUserInRole("Trainee") Then btnListen.HRef = "listen_training.aspx"

        'If Roles.IsUserInRole("Admin") Or Roles.IsUserInRole("QA Lead") Then
        '    btnCalListen.Visible = True
        'End If


        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            pnlHeader.Visible = False
            pnlSidebar.Visible = False
            pnlBlankHeader.Visible = True
        Else
            pnlHeader.Visible = True
            pnlSidebar.Visible = True
            pnlBlankHeader.Visible = False
        End If

        If Roles.IsUserInRole("Client") Then
            pnlSidebar.Visible = True
            pnlHeader.Visible = True
            messages.Visible = False
            events.Visible = False

            users_link.Visible = False
            Uploads_link.Visible = False
            'q_manager_link.Visible = False
            'section_link.Visible = False
            app_settings_link.Visible = False
        End If

        If Roles.IsUserInRole("QA") Or Roles.IsUserInRole("Trainee") Or Roles.IsUserInRole("Agent") Then
            liCallHist.Visible = False
            'reports_link.Visible = False
        End If

        If Roles.IsUserInRole("Agent") Then
            today_view.Visible = False
            search_view.Visible = False
        End If


        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("QA") Or Roles.IsUserInRole("Trainee") Or Roles.IsUserInRole("Agent") Then
            users_link.Visible = False
            Uploads_link.Visible = False
            'q_manager_link.Visible = False
            'section_link.Visible = False
            app_settings_link.Visible = False

        End If

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("Admin") Then
            liQA.Visible = True
        End If


        'reports_link.Visible = False

        If Roles.IsUserInRole("Client") Or Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("Agent") Then
            btnListen.Visible = False
        End If

        If Not IsPostBack Then
            Response.AddHeader("Accept-Ranges", "1")
            'Select Case Page.AppRelativeVirtualPath.Replace("~/", "").ToLower
            '    Case "app_settings.aspx"
            '        app_settings_link.Attributes.Add("class", "you-are-here")
            '    Case "admin_dashboard.aspx"
            '        default_link.Attributes.Add("class", "you-are-here")
            '    Case "users.aspx"
            '        users_link.Attributes.Add("class", "you-are-here")
            '        'Case "q_manager.aspx"
            '        '    q_manager_link.Attributes.Add("class", "you-are-here")
            '    Case "uploads.aspx"
            '        Uploads_link.Attributes.Add("class", "you-are-here")
            '    Case "category_Management.aspx"
            '        'category_link.Attributes.Add("class", "you-are-here")
            '        'Case "section_management.aspx"
            '        '    section_link.Attributes.Add("class", "you-are-here")
            '    Case "reports.aspx"
            '        'reports_link.Attributes.Add("class", "you-are-here")
            'End Select

        End If


        'If HttpContext.Current.User.IsInRole("Admin") Then
        '    nav.Visible = True
        'Else
        '    nav.Visible = False
        'End If


        'If System.Web.HttpContext.Current.User.IsInRole("QA") Then
        '    hlMyReports.Visible = True
        'Else
        '    hlMyReports.Visible = False
        'End If



        Dim userProfile As ProfileCommon = ProfileCommon.Create(theUser)
        'dsNotifications2.SelectParameters("reviewer").DefaultValue = theUser
        'dsNotifications2.DataBind()


        If userProfile.Avatar <> "" Then
            imgAvatar.ImageUrl = "/audio/" & theUser & "/" & userProfile.Avatar
        Else
            imgAvatar.ImageUrl = "images/placeholders/Logo_no_words.png"
        End If


        'Dim total_worked_dt2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS with (nolock)  join XCC_REPORT_NEW with (nolock)  on XCC_ID = XCC_REPORT_NEW.ID where  review_date between '" & Today.ToShortDateString & "' and '" & Now.ToString & "' and xcc_report_new.appname  in (" & app_list & ") ")
        'litWorkedRecords.Text = total_worked_dt2.Rows(0).Item(0).ToString


        Dim dtnoOfreview As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b  with (nolock)   where max_reviews = 0 and ((review_started < dateadd(mi,-45, dbo.getMTDate())) or (review_started is null)) and audio_link is not null and b.appname  in (" & app_list & ") ")
        lblLeftToReview.Text = dtnoOfreview.Rows(0).Item(0)
        If lblLeftToReview.Text = "0" Then
            btnListen.Visible = False
        End If
        litQALeft.Text = dtnoOfreview.Rows(0).Item(0)

        'litNotifications.Text = "0"

        If role = "QA" Then


            Dim total_worked_dt As DataTable = GetTable("select count(*), sum(case when reviewer = '" & theUser & "' then 1 else 0 end) as number_worked from form_score3 join XCC_REPORT_NEW on review_id  = XCC_REPORT_NEW.ID where review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname  in (" & app_list & ") ")
            lblWorkedRecords.Text = total_worked_dt.Rows(0).Item(0).ToString
            If total_worked_dt.Rows(0).Item(1).ToString <> "" Then
                litQAWorked.Text = total_worked_dt.Rows(0).Item(1).ToString
            End If

            Dim toal_failed As DataTable = GetTable("select count(*), sum(case when reviewer = '" & theUser & "' then 1 else 0 end) as number_failed  from form_score3 where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and total_score < 80 and appname  in (" & app_list & ") ")
            lblTotalFailes.Text = toal_failed.Rows(0).Item(0).ToString
            litQAFails.Text = toal_failed.Rows(0).Item(1).ToString


            dsComments.SelectCommand = "SELECT top 100 opened_by,AGENT,Agent_Group,role,total_score,comment,date_created,form_notifications.id as id,f_id as form_id, " &
            "dbo.GetMissedQuestionCount(f_id) as MissedQuestions, convert(varchar(10),call_date,101) as call_date FROM [form_notifications] " &
            "join vwForm on vwForm.f_id = form_notifications.form_id  " &
            "WHERE date_closed is null and role='QA' and sup_override is null  and reviewer = '" & userProfile.UserName & "' and vwForm.appname in (" & app_list & ") " &
            "ORDER BY [date_created], Agent"

            dsComments.DataBind()

        End If

        If role = "Admin" Or role = "QA Lead" Then
            dsComments.SelectCommand = "SELECT top 100 opened_by,AGENT,Agent_Group,role,total_score,comment,date_created,form_notifications.id as id,form_score3.id as form_id, " &
           "dbo.GetMissedQuestionCount(form_score3.id) as MissedQuestions, convert(varchar(10),call_date,101) as call_date FROM [form_notifications] join form_score3 on form_notifications.form_id = form_score3.id  " &
           "join xcc_report_new on form_score3.review_id = xcc_report_new.id  " &
           "WHERE date_closed is null and role='QA Lead'  and sup_override is null and form_score3.appname in (" & app_list & ")  " &
           "ORDER BY [date_created], Agent"

            dsComments.DataBind()


        End If


        Dim avg_score As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and xcc_report_new.appname  in (" & app_list & ") ")
        If avg_score.Rows.Count > 0 Then
            If IsDBNull(avg_score.Rows(0).Item(0)) Then
                lblAvgScore.Text = "0"
            Else
                lblAvgScore.Text = avg_score.Rows(0).Item(0).ToString
            End If

        Else
            lblAvgScore.Text = "0"
        End If



        'get # notifications litNotifications
        If role = "Supervisor" Then


            Dim group_specific As String = ""
            If userProfile.Group <> "" Then
                group_specific = " and agent_group = '" & userProfile.Group & "'  "
            End If

            If Session("SelectedGroup") IsNot Nothing Then
                group_specific = " and agent_group = '" & Session("SelectedGroup") & "'  "
            End If

            'Dim count_dt As DataTable = Common.GetTable("SELECT count(*) FROM [notifications] join form_score3 on notifications.form_id = form_score3.id join xcc_report_new on form_score3.review_id = xcc_report_new.id  WHERE  (acknowledged is null) and form_score3.appname in (" & app_list & ")  and assigned_to = 'Supervisor' " & group_specific)
            'If count_dt.Rows.Count > 0 Then
            '    litNotifications.Text = count_dt.Rows(0).Item(0)
            '    'Literal1.Text = "Notifications (" & count_dt.Rows(0).Item(0) & ")"
            'Else
            '    litNotifications.Text = "0"
            'End If

            dsComments.SelectCommand = "SELECT top 100 opened_by,AGENT,Agent_Group,role,total_score,comment,date_created,form_notifications.id as id,form_score3.id as form_id, " &
                   "dbo.GetMissedQuestionCount(form_score3.id) as MissedQuestions, convert(varchar(10),call_date,101) as call_date FROM [form_notifications] join form_score3 on form_notifications.form_id = form_score3.id  " &
                   "join xcc_report_new on form_score3.review_id = xcc_report_new.id  " &
                   "WHERE date_closed is null  and sup_override is null  and form_score3.appname in (" & app_list & ")  and role = 'Supervisor' " & group_specific & " " &
                   "ORDER BY [date_created], Agent"



            If userProfile.Group <> "" Then

                Dim dtnoOfreview2 As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b  with (nolock)  left join form_score3 f  with (nolock)  on f.review_id = b.id  where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) or (review_started is null))  and audio_link is not null  and b.appname  in (" & app_list & ")  and b.AGENT_group = '" & userProfile.Group & "' " & group_specific)
                lblLeftToReview.Text = dtnoOfreview2.Rows(0).Item(0)


                Dim total_worked_dt3 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where agent_group = '" & userProfile.Group & "' and review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname  in (" & app_list & ") " & group_specific)
                lblWorkedRecords.Text = total_worked_dt3.Rows(0).Item(0).ToString


                Dim toal_failed2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname  in (" & app_list & ")  and XCC_REPORT_NEW.AGENT_group = '" & userProfile.Group & "'" & group_specific)
                lblTotalFailes.Text = toal_failed2.Rows(0).Item(0).ToString

                Dim avg_score2 As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and XCC_REPORT_NEW.AGENT_group = '" & userProfile.Group & "'" & group_specific)
                If avg_score2.Rows.Count > 0 Then
                    lblAvgScore.Text = avg_score2.Rows(0).Item(0).ToString
                Else
                    lblAvgScore.Text = "0"
                End If
            End If

        End If

        If role = "Manager" Or role = "QA Lead" Then

            Dim dtnoOfreview2 As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) or (review_started is null))  and audio_link is not null  and appname  in (" & app_list & ") ")
            lblLeftToReview.Text = dtnoOfreview2.Rows(0).Item(0)


            Dim total_worked_dt3 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname  in (" & app_list & ") ")
            lblWorkedRecords.Text = total_worked_dt3.Rows(0).Item(0).ToString


            Dim toal_failed2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname  in (" & app_list & ") ")
            lblTotalFailes.Text = toal_failed2.Rows(0).Item(0).ToString

            Dim avg_score2 As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and xcc_report_new.appname  in (" & app_list & ") ")
            If avg_score2.Rows.Count > 0 Then
                lblAvgScore.Text = avg_score2.Rows(0).Item(0).ToString
            Else
                lblAvgScore.Text = "0"
            End If



        End If


        If role = "Admin" Or role = "Client" Then

            Dim dtnoOfreview2 As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) or (review_started is null))  and audio_link is not null  and appname  in (" & app_list & ") ")
            lblLeftToReview.Text = dtnoOfreview2.Rows(0).Item(0)


            Dim total_worked_dt3 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname  in (" & app_list & ") ")
            lblWorkedRecords.Text = total_worked_dt3.Rows(0).Item(0).ToString


            Dim toal_failed2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname  in (" & app_list & ") ")
            lblTotalFailes.Text = toal_failed2.Rows(0).Item(0).ToString

            Dim avg_score2 As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and xcc_report_new.appname  in (" & app_list & ") ")
            If avg_score2.Rows.Count > 0 Then
                lblAvgScore.Text = avg_score2.Rows(0).Item(0).ToString
            Else
                lblAvgScore.Text = "0"
            End If



        End If

        If role = "Agent" Then



            Dim dtnoOfreview2 As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) or (review_started is null))  and audio_link is not null  and appname  in (" & app_list & ")  and b.AGENT = '" & Request("Agent") & "' ")
            lblLeftToReview.Text = dtnoOfreview2.Rows(0).Item(0)


            Dim total_worked_dt3 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where agent_id = '" & Request("Agent") & "' and review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname  in (" & app_list & ") ")
            lblWorkedRecords.Text = total_worked_dt3.Rows(0).Item(0).ToString


            Dim toal_failed2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname  in (" & app_list & ")  and XCC_REPORT_NEW.AGENT = '" & Request("Agent") & "'")
            lblTotalFailes.Text = toal_failed2.Rows(0).Item(0).ToString

            Dim avg_score2 As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and xcc_report_new.appname  in (" & app_list & ") ")
            If avg_score2.Rows.Count > 0 Then
                lblAvgScore.Text = avg_score2.Rows(0).Item(0).ToString
            Else
                lblAvgScore.Text = "0"
            End If


        End If

    End Sub



    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

        UpdateTable("insert into searchedValues (who_searched, date_searched, value_searched) select '" & HttpContext.Current.User.Identity.Name & "', dbo.getMTDate(), '" & txtSearch.Text.Replace("'", "''") & "'")

        Response.Redirect("search.aspx?Value=" & txtSearch.Text)

    End Sub

    Protected Sub dsComments_Selected(sender As Object, e As SqlDataSourceStatusEventArgs) Handles dsComments.Selected
        litNotifications.Text = e.AffectedRows
    End Sub
End Class



