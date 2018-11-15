Imports System.Data
Imports System.Data.SqlClient
Imports Common

Partial Class CCMasterPage
    Inherits System.Web.UI.MasterPage
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FormsAuthentication.SignOut()
        Response.Redirect("login.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("appname") Is Nothing Then
            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            Session("appname") = domain(0)
        End If


        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            pnlHeader.Visible = False
            pnlSidebar.Visible = False
        End If

        If Not IsPostBack Then
            Response.AddHeader("Accept-Ranges", "1")
            Select Case Page.AppRelativeVirtualPath.Replace("~/", "").ToLower
                Case "app_settings.aspx"
                    app_settings_link.Attributes.Add("class", "you-are-here")
                Case "admin_dashboard.aspx"
                    default_link.Attributes.Add("class", "you-are-here")
                Case "users.aspx"
                    users_link.Attributes.Add("class", "you-are-here")
                Case "q_manager.aspx"
                    q_manager_link.Attributes.Add("class", "you-are-here")
                Case "uploads.aspx"
                    Uploads_link.Attributes.Add("class", "you-are-here")
                Case "category_Management.aspx"
                    'category_link.Attributes.Add("class", "you-are-here")
                Case "section_management.aspx"
                    section_link.Attributes.Add("class", "you-are-here")
                Case "reports.aspx"
                    reports_link.Attributes.Add("class", "you-are-here")
            End Select

        End If


        'If HttpContext.Current.User.IsInRole("Admin") Then
        '    nav.Visible = True
        'Else
        '    nav.Visible = False
        'End If

        If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
            'afterLoginDiv.Visible = True
            'beforeLoginDiv.Visible = False
            Dim role As String = String.Empty
            lblUserName.Text = System.Web.HttpContext.Current.User.Identity.Name
            Dim user As MembershipUser = Membership.GetUser(System.Web.HttpContext.Current.User.Identity.Name)



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
            ElseIf System.Web.HttpContext.Current.User.IsInRole("QA Lead") Then
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
            End If
            lblRole.Text = role
        Else
            'afterLoginDiv.Visible = False
            'beforeLoginDiv.Visible = True
        End If


        'If System.Web.HttpContext.Current.User.IsInRole("QA") Then
        '    hlMyReports.Visible = True
        'Else
        '    hlMyReports.Visible = False
        'End If



        Dim userProfile As ProfileCommon = ProfileCommon.Create(HttpContext.Current.User.Identity.Name)
        dsNotifications2.SelectParameters("reviewer").DefaultValue = HttpContext.Current.User.Identity.Name
        dsNotifications2.DataBind()


        If userProfile.Avatar <> "" Then
            imgAvatar.ImageUrl = "/audio/" & HttpContext.Current.User.Identity.Name & "/" & userProfile.Avatar
        Else
            imgAvatar.ImageUrl = "images/placeholders/Logo_no_words.png"
        End If


        Dim total_worked_dt2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS with (nolock)  join XCC_REPORT_NEW with (nolock)  on XCC_ID = XCC_REPORT_NEW.ID where  review_date between '" & Today.ToShortDateString & "' and '" & Now.ToString & "' and xcc_report_new.appname = '" & Session("appname") & "'")
        litWorkedRecords.Text = total_worked_dt2.Rows(0).Item(0).ToString

        Dim dtnoOfreview As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b  with (nolock)  left join form_score3 f  with (nolock)  on f.review_id = b.id  where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started > dateadd(mi,-45, dbo.getMTDate())) or (review_started is null)) and b.appname = '" & Session("appname") & "'")
        lblLeftToReview.Text = dtnoOfreview.Rows(0).Item(0)

        Dim total_worked_dt As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname = '" & Session("appname") & "'")
        lblWorkedRecords.Text = total_worked_dt.Rows(0).Item(0).ToString


        Dim toal_failed As DataTable = GetTable("select count(*) from form_score3 where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and total_score < 80 and appname = '" & Session("appname") & "'")
        lblTotalFailes.Text = toal_failed.Rows(0).Item(0).ToString

        Dim avg_score As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "' and xcc_report_new.appname = '" & Session("appname") & "'")
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
        If HttpContext.Current.User.IsInRole("Supervisor") Then

            Dim count_dt As DataTable = Common.GetTable("SELECT count(*) FROM [notifications] join form_score3 on notifications.form_id = form_score3.id join xcc_report_new on form_score3.review_id = xcc_report_new.id  WHERE  (acknowledged is null) and form_score3.appname='" & Session("appname") & "'  and assigned_to = 'Supervisor' ")
            If count_dt.Rows.Count > 0 Then
                litNotifications.Text = count_dt.Rows(0).Item(0)
                'Literal1.Text = "Notifications (" & count_dt.Rows(0).Item(0) & ")"
            End If
        End If

        If HttpContext.Current.User.IsInRole("Manager") Or HttpContext.Current.User.IsInRole("QA Lead") Then
            Dim count_dt As DataTable = Common.GetTable("SELECT count(*) FROM [notifications] join form_score3 on notifications.form_id = form_score3.id join xcc_report_new on form_score3.review_id = xcc_report_new.id join app_settings on app_settings.appname = form_score3.appname  WHERE  (acknowledged is null) and form_score3.appname='" & Session("appname") & "'  and assigned_to = 'Manager'") '(Agent_Group = '" & userProfile.Group & "' or agent_group = '' or agent_group is null)and app_settings.manager = '" & HttpContext.Current.User.Identity.Name & "'
            If count_dt.Rows.Count > 0 Then
                litNotifications.Text = count_dt.Rows(0).Item(0)
                'Literal1.Text = "Notifications (" & count_dt.Rows(0).Item(0) & ")"
            End If
        End If


        If HttpContext.Current.User.IsInRole("Admin") Then
            Dim count_dt As DataTable = Common.GetTable("SELECT count(*) FROM [notifications] join form_score3 on notifications.form_id = form_score3.id join xcc_report_new on form_score3.review_id = xcc_report_new.id  WHERE  (acknowledged is null) and form_score3.appname='" & Session("appname") & "'  ")
            If count_dt.Rows.Count > 0 Then
                litNotifications.Text = count_dt.Rows(0).Item(0)
                'Literal1.Text = "Notifications (" & count_dt.Rows(0).Item(0) & ")"
            End If
        End If

        If HttpContext.Current.User.IsInRole("Agent") Then
            Dim count_dt As DataTable = Common.GetTable("SELECT count(*) FROM [notifications] join form_score3 on notifications.form_id = form_score3.id join xcc_report_new on form_score3.review_id = xcc_report_new.id  WHERE  (acknowledged is null)  and form_score3.appname='" & Session("appname") & "'  and reviewer = '" & HttpContext.Current.User.Identity.Name & "'")
            If count_dt.Rows.Count > 0 Then
                litNotifications.Text = count_dt.Rows(0).Item(0)
                'Literal1.Text = "Notifications (" & count_dt.Rows(0).Item(0) & ")"
            End If


            Dim cn2 As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn2.Open()


            Dim dtnoOfreview2 As DataTable = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and appname = '" & Session("appname") & "' and b.AGENT = '" & Request("Agent") & "' ")
            lblLeftToReview.Text = dtnoOfreview2.Rows(0).Item(0)


            Dim total_worked_dt3 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where agent_id = '" & Request("Agent") & "' and review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and xcc_report_new.appname = '" & Session("appname") & "'")
            lblWorkedRecords.Text = total_worked_dt3.Rows(0).Item(0).ToString


            Dim toal_failed2 As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname = '" & Session("appname") & "' and XCC_REPORT_NEW.AGENT = '" & Request("Agent") & "'")
            lblTotalFailes.Text = toal_failed2.Rows(0).Item(0).ToString

            Dim avg_score2 As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 join XCC_REPORT_NEW  on form_score3.review_ID = XCC_REPORT_NEW.ID where  CONVERT(DATE,form_score3.review_date) = '" & Today.ToShortDateString & "'")
            If avg_score2.Rows.Count > 0 Then
                lblAvgScore.Text = avg_score2.Rows(0).Item(0).ToString
            Else
                lblAvgScore.Text = "0"
            End If


        End If

    End Sub


    Protected Sub dsNotifications2_Selected(sender As Object, e As SqlDataSourceStatusEventArgs) Handles dsNotifications2.Selected
        litMessages.Text = e.AffectedRows
    End Sub

End Class



