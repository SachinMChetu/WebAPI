Imports Common
Imports System.Data
Imports System.IO
Imports System.Net

Partial Class CD2
    Inherits System.Web.UI.Page


    Public myRole As String

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

    Public registered_callbacks As String = ""

    'Public red_sript_registered As Boolean = False

    Public prefilter As String = " and 1=1 "

    Protected Sub Recalc()

    End Sub



    'ClearGuidelines

    <System.Web.Services.WebMethod()>
    Public Shared Function ClearGuidelines() As String
        Dim resp As String = ""

        UpdateTable("resetUserGuidelines '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "'")

        Return ("Cleared")

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Sub SetFilter(filter As String)
        HttpContext.Current.Session("filter") = filter

        HttpContext.Current.Response.Cookies.Set(New HttpCookie("filter", filter))

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GetFilter() As String

        'Return HttpContext.Current.Session("filter")
        Return HttpContext.Current.Response.Cookies("filter").Value

    End Function


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Response.Redirect("/home/")


        Response.End()

        If Request("appname") IsNot Nothing Then
            Session("agent_appname") = Request("appname")
        End If


        Dim user_type As String = ""


        If Request("agent_login") IsNot Nothing Then




            If User.Identity.IsAuthenticated Then
                If User.Identity.Name <> Request("agent_login") Then
                    Session.Abandon()

                    If Request("appname") IsNot Nothing Then
                        Session("agent_appname") = Request("appname")
                    End If

                    FormsAuthentication.SignOut()
                    Response.Redirect("cd2.aspx?agent_login=" & Request("agent_login"))
                End If
            End If




            'Try
            user_type = Roles.GetRolesForUser(Request("agent_login")).Single()

            'Response.Write(user_type)
            'Response.End()

            If user_type.ToLower = "agent" Then
                'Dim no_login As DataTable = GetTable("select * from userapps join scorecards on userapps.user_scorecard = scorecards.id where no_agent_login = 1 and username = '" & Request("agent_login") & "'")
                Dim no_login As DataTable = GetTable("select count(distinct scorecards.appname) as num_apps, min(scorecards.appname) as first_app from userapps join scorecards on userapps.user_scorecard = scorecards.id where  username = '" & Request("agent_login") & "' and isnull(no_agent_login,0) <> 1 ")

                If no_login.Rows(0).Item("num_apps") = 1 Then


                    Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1,
                           Request("agent_login"),
                           DateTime.Now,
                           DateTime.Now.AddMonths(1),
                           False,
                           no_login.Rows(0).Item("first_app").ToString,
                           FormsAuthentication.FormsCookiePath)

                    ' Encrypt the ticket.
                    Dim encTicket As String = FormsAuthentication.Encrypt(ticket)

                    ' Create the cookie.
                    Response.Cookies.Add(New HttpCookie(FormsAuthentication.FormsCookieName, encTicket))



                    Response.Redirect("cd2.aspx")
                    'Catch ex As Exception

                    'End Try
                Else

                    If no_login.Rows(0).Item("num_apps") > 1 Then
                        Response.Redirect("set_app.aspx?agent_login=" & Request("agent_login"))
                    Else
                        Response.Redirect("login.aspx")
                    End If


                End If




            End If
        End If

        hdnHardFilters.Value = "username:" & Replace(User.Identity.Name, "'", "''")

        If user_type.ToLower = "agent" Then
            Dim id As FormsIdentity = CType(User.Identity, FormsIdentity)
            Dim ticket As FormsAuthenticationTicket = id.Ticket
            hdnHardFilters.Value = hdnHardFilters.Value & "|appname:" & ticket.UserData
        End If

        Dim user_info_dt As DataTable = GetTable("select * From userextrainfo left join userapps on userapps.username = UserExtraInfo.username left join app_settings on app_settings.appname = UserApps.appname where userextrainfo.username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "'")


        If user_info_dt.Rows.Count > 0 Then

            If user_info_dt.Rows(0).Item("dashboard").ToString = "new" And (User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("Agent")) Then
                Response.Redirect("/home")
            End If

            If user_info_dt.Rows(0).Item("no_dash").ToString = "True" And user_info_dt.Rows(0).Item("default_page").ToString <> "" Then
                Response.Redirect(user_info_dt.Rows(0).Item("default_page").ToString)
            End If



            If User.IsInRole("Supervisor") And user_info_dt.Rows(0).Item("user_group").ToString <> "" Then
                hdnFixedFilter.Value = " and vwform.agent_group = '" & user_info_dt.Rows(0).Item("user_group").ToString & "' "
                hdnHardFilters.Value = hdnHardFilters.Value & "|group:" & user_info_dt.Rows(0).Item("user_group").ToString
            End If

            If user_info_dt.Rows(0).Item("special_filter").ToString <> "" Then
                hdnFixedFilter.Value &= hdnFixedFilter.Value & user_info_dt.Rows(0).Item("special_filter").ToString
            End If

            If user_info_dt.Rows(0).Item("speed_increment").ToString <> "" Then
                data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            Else
                data_rate = 0.05
            End If

        Else
            data_rate = 0.05
        End If

        If Request("error") <> "" Then
            ClientScript.RegisterStartupScript(Me.GetType(), "alert_error", "<script>alert('You have open notifications. Please clear them from the coaching queue before continuing.');</script>")


        End If


        'Dim myRoles() As String = Roles.GetRolesForUser(User.Identity.Name)

        'Try
        '    myRole = myRoles(0)
        'Catch ex As Exception

        'End Try

        myRole = Replace(user_info_dt.Rows(0).Item("user_role"), " ", "_")


        If User.IsInRole("Agent") Then
            'hdnThisAgent.Value = User.Identity.Name
            hdnAgentFilter.Value = " and vwform.agent = '" & Replace(User.Identity.Name, "'", "''") & "' "
            hdnFixedFilter.Value = " and vwform.agent = '" & Replace(User.Identity.Name, "'", "''") & "'  "
            hdnHardFilters.Value = hdnHardFilters.Value & "|agent:" & Replace(User.Identity.Name, "'", "''")
            If Session("agent_appname") IsNot Nothing Then
                hdnHardFilters.Value = hdnHardFilters.Value & "|agent:" & Replace(User.Identity.Name, "'", "''")
            End If
            prefilter = " and vwform.agent = '" & Replace(User.Identity.Name, "'", "''") & "'  "
        End If

        If User.IsInRole("QA") Then
            'hdnThisAgent.Value = User.Identity.Name
            hdnHardFilters.Value = hdnHardFilters.Value & "|reviewer:" & Replace(User.Identity.Name, "'", "''")
            hdnAgentFilter.Value = " and vwform.reviewer = '" & Replace(User.Identity.Name, "'", "''") & "' "
            hdnFixedFilter.Value = " and vwform.reviewer = '" & Replace(User.Identity.Name, "'", "''") & "' "
        End If





        If User.IsInRole("Center Manager") Then

            Dim cc_dt As DataTable = GetTable("select call_center from userextrainfo with  (nolock)  where username = '" & Replace(User.Identity.Name, "'", "''") & "' ")
            If cc_dt.Rows.Count > 0 Then
                'hdnThisAgent.Value = User.Identity.Name
                hdnHardFilters.Value = hdnHardFilters.Value & "|call_center:" & cc_dt.Rows(0).Item("call_center").ToString
                hdnAgentFilter.Value = " and userextrainfo.call_center = '" & cc_dt.Rows(0).Item("call_center").ToString & "' "
                hdnFixedFilter.Value = " and userextrainfo.call_center = '" & cc_dt.Rows(0).Item("call_center").ToString & "' "
            End If
        End If




        If (User.Identity.Name = "" Or User.Identity.Name Is Nothing) Then 'And Request("agent") = "" Then
            Response.Redirect("login.aspx?ReturnURL=cd2.aspx")
        End If



        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        Session("appname") = domain(0)


        If Request.ServerVariables("SERVER_NAME").IndexOf("callcenterqa") > -1 Or Request.ServerVariables("SERVER_NAME").IndexOf("edufficient") > -1 Or Request.ServerVariables("SERVER_NAME").IndexOf("inquirycompliance") > -1 Then

            Dim csname1 As [String] = "swapColor"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript
            Dim cstext1 As New StringBuilder()
            cstext1.Append("<script type=text/javascript> refreshColor('#5c8b23','//app.callcriteria.com/img/edufficient/edufficient-logo.png','Edufficient'); </")
            cstext1.Append("script>")

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())

        ElseIf Request.ServerVariables("SERVER_NAME").IndexOf("thedmsgrp") > -1 Then
            Dim csname1 As [String] = "swapColor"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript

            Dim cstext1 As New StringBuilder()
            cstext1.Append("<script type=text/javascript> refreshColor('#51B5E0','//app.callcriteria.com/img/dms_wide.png','Digital Media Solutions'); </")
            cstext1.Append("script>")

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())


        ElseIf Request.ServerVariables("SERVER_NAME").IndexOf("callsource") > -1 Then
            Dim csname1 As [String] = "swapColor"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript

            Dim cstext1 As New StringBuilder()
            cstext1.Append("<script type=text/javascript> refreshColor('#51B5E0','//app.callcriteria.com/audio/CallSource/callsource.png','CallSource'); </")
            cstext1.Append("script>")

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())


        ElseIf Request.ServerVariables("SERVER_NAME").IndexOf("criticall") > -1 Then
            Dim csname1 As [String] = "swapColor"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript

            Dim cstext1 As New StringBuilder()
            cstext1.Append("<script type=text/javascript> refreshColor('#008080','//app.callcriteria.com/img/criticall_logo3.png','CritiCall Solutions'); </")
            cstext1.Append("script>")

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())


        Else

            Dim csname1 As [String] = "swapColor"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript

            If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
                Dim cstext1 As New StringBuilder()
                cstext1.Append("<script type=text/javascript> stdColor(); </")
                cstext1.Append("script>")

                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
            End If
        End If



        If Request.ServerVariables("SERVER_NAME").IndexOf("pointqa") > -1 Then


            Dim csname1 As [String] = "clearLogo"
            Dim cstype As Type = Me.[GetType]()

            Dim cs As ClientScriptManager = Page.ClientScript

            ' If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As New StringBuilder()
            cstext1.Append("<script type=text/javascript> clearLogo(); </")
            cstext1.Append("script>")

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())

        End If


        If Not IsPostBack Then


            Dim thisDate As Date = Today

            thisDate = DateAdd(DateInterval.Day, -14, Today)

            date1.Text = thisDate.ToString("d")
            Session("StartDate") = thisDate.ToString("d")

            date2.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")

            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                date1.Text = thisDate.ToString("d")
                Session("StartDate") = thisDate.ToString("d")
            Else
                date1.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                date2.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Else
                date2.Text = Session("EndDate")
            End If


            Dim user_modules As Integer = 0

            Dim my_role As String = Replace(user_info_dt.Rows(0).Item("user_role"), " ", "_")

            'Try
            '    my_role = Join(Roles.GetRolesForUser(User.Identity.Name), ",").Replace(" ", "_")
            'Catch ex As Exception
            '    If Request("agent") <> "" Then
            '        my_role = "Agent"
            '    End If
            'End Try



            'Get all modules available to add or delete -- + for add, - for delete
            dsModules.SelectParameters("username").DefaultValue = User.Identity.Name
            dsBilling.SelectParameters("username").DefaultValue = User.Identity.Name
            If (User.IsInRole("Client")) Then ' Or User.IsInRole("Admin")
                dsOpenBilling.SelectParameters("username").DefaultValue = User.Identity.Name
            End If

            UpdateTable("insert into userdash(username, controlname) select '" & Replace(User.Identity.Name, "'", "''") & "',moduleName from moduleList where " & my_role & " = 1 and moduleName not in (select controlname from userdash where username = '" & Replace(User.Identity.Name, "'", "''") & "')")


            Dim user_dt As DataTable = GetTable("select count(*) from userapps join app_settings on app_settings.appname = userapps.appname  join userextrainfo on userextrainfo.username = userapps.username where userapps.username = '" & Replace(User.Identity.Name, "'", "''") & "' and agent_summary_only = 1 and user_role ='Agent'")
            If user_dt.Rows.Count > 0 Then
                If user_dt.Rows(0).Item(0) > 0 Then
                    dd.Visible = False
                End If
            End If


            'setup user's preferences -- otherwise use stock AvgScore, Score Perf, Stats, Top Missed, Agent Ranking
            'Dim user_dash_dt As DataTable = GetTable("select * from moduleList  with  (nolock)  join userdash with  (nolock)  on moduleList.moduleName = UserDash.controlname  where  username = '" & User.Identity.Name & "'  order by controlorder")
            Dim user_dash_dt As DataTable = GetTable("select * from moduleList  with  (nolock)  join userdash with  (nolock)  on moduleList.moduleName = UserDash.controlname join userextrainfo on userdash.username = userextrainfo.username  where  userdash.username = '" & Replace(User.Identity.Name, "'", "''") & "' and modulename not in (select modulename from module_exceptions where add_remove = 0 and  [" & my_role.Replace(" ", "_") & "] = 1 and appname in (select appname from userapps where username = '" & User.Identity.Name & "'))  and 1 = case when userextrainfo.call_center is not null and moduleList.call_center = 0 then 0 else 1 end order by controlorder")
            'Dim user_dash_dt As DataTable = GetTable("[getMyModulesNew] '" & Replace(User.Identity.Name, "'", "''") & "',1")

            If user_dash_dt.Rows.Count = 1 Then



                Dim default_dt As DataTable = GetTable("Select * from moduleList  With  (nolock) where  [" & my_role.Replace(" ", "_") & "] = 1 And modulename Not In (Select modulename from module_exceptions where add_remove = 0 And  [" & my_role.Replace(" ", "_") & "] = 1 And appname In (Select appname from userapps where username = '" & User.Identity.Name & "'))  order by default_order")

                For Each dr In default_dt.Rows
                    Dim li As HtmlGenericControl = New HtmlGenericControl("li")
                    'li.Attributes.Add("draggable", "true")


                    Dim tm As New Control
                    tm = LoadControl(dr("moduleControlName"))
                    li.Controls.Add(tm)
                    li.Attributes.Add("class", dr("moduleWidth"))
                    registered_callbacks &= " try { " & dr("moduleFunction") & "} catch (err) {}" & vbCrLf
                    Select Case dr("moduleWidth")
                        Case "single"
                            user_modules += 1
                        Case "double"
                            user_modules += 2
                        Case "triple"
                            user_modules += 3
                    End Select

                    dashModules.Controls.Add(li)

                Next



            Else
                For Each dr As DataRow In user_dash_dt.Rows

                    If System.IO.File.Exists(Server.MapPath(dr("moduleControlName"))) Then
                        Dim li As HtmlGenericControl = New HtmlGenericControl("li")
                        li.Attributes.Add("draggable", "true")


                        Dim tm As New Control
                        tm = LoadControl(dr("moduleControlName"))
                        li.Controls.Add(tm)
                        li.Attributes.Add("class", dr("moduleWidth"))
                        registered_callbacks &= " try { " & dr("moduleFunction") & "} catch (err) {}" & vbCrLf
                        Select Case dr("moduleWidth")
                            Case "single"
                                user_modules += 1
                            Case "double"
                                user_modules += 2
                            Case "triple"
                                user_modules += 3
                        End Select


                        dashModules.Controls.Add(li)
                    End If
                Next
            End If



            hdnTotalModules.Value = user_modules

            Recalc_Elements()

            ' check for guidlines
            'litUpdateGuidlines

            'Dim guide_dt As DataTable = GetTable("select isnull(sum(case when isnull(date_reviewed,'1/1/2010') < max_date  or isnull(date_reviewed,'1/1/2010') < max_date_f  then 1 else '' end),0) as qu_class  from scorecards  left join (select * from sc_update where reviewer = '" & User.Identity.Name & "') a on a.sc_id = scorecards.id  left join (select max(dateadded) as max_date, scorecard_id from q_instructions join questions on questions.ID = q_instructions.question_id group by scorecard_id) b on b.scorecard_id = scorecards.id     left join (select max(dateadded) as max_date_f, scorecard_id from q_faqs join questions on questions.ID = q_faqs.question_id group by scorecard_id) c on c.scorecard_id = scorecards.id       where appname in (select appname from userapps where username = '" & User.Identity.Name & "') and active =1 ")
            Dim guide_dt As DataTable = GetTable("select isnull(sum(case when isnull(date_reviewed,'1/1/2010') < max_date  or isnull(date_reviewed,'1/1/2010') < max_date_f  then 1 else '' end),0) as qu_class  from scorecards  with  (nolock)  left join (select sc_id, max(date_reviewed) as date_reviewed from sc_update with  (nolock)  where reviewer = '" & Replace(HttpContext.Current.User.Identity.Name, "'", "''") & "' group by sc_id ) a on a.sc_id = scorecards.id  left join (select max(dateadded) as max_date, scorecard_id from q_instructions with  (nolock)  join questions with  (nolock)  on questions.ID = q_instructions.question_id group by scorecard_id) b on b.scorecard_id = scorecards.id     left join (select max(dateadded) as max_date_f, scorecard_id from q_faqs with  (nolock)  join questions with  (nolock)  on questions.ID = q_faqs.question_id group by scorecard_id) c on c.scorecard_id = scorecards.id  where scorecards.id in (select user_scorecard from userapps with  (nolock)  where username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "') and active =1 and onhold = 0 ")

            If guide_dt.Rows.Count > 0 And Not User.IsInRole("Agent") Then

                If guide_dt.Rows(0).Item(0) > 0 Then
                    showGuidlines.Attributes.Add("class", "updates-btn show")
                    litUpdateGuidlines.Text = "<div class='update'></div>"

                    ClientScript.RegisterStartupScript(Me.GetType(), "alert_guidelines", "show_guideline_popup();", True)
                End If

            End If
        End If




    End Sub


    Protected Sub Recalc_Elements()

        ''litGroupFilter.Text = "All Agents"

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("Client") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "
                hdnAgentFilter.Value &= " and AGENT_GROUP = '" & userProfile.Group & "' "
                hdnFixedFilter.Value &= " and AGENT_GROUP = '" & userProfile.Group & "' "
                hdnHardFilters.Value = "group:" & userProfile.Group
                'litGroupFilter.Text = userProfile.Group
            End If
        End If




        If (User.IsInRole("Client") Or User.IsInRole("Admin")) Then

            'Dim billed_dt As DataTable = GetTable("getBilling '" & User.Identity.Name & "'")
            'If billed_dt.Rows.Count > 0 Then

            '    For Each bill_dr In billed_dt.Rows

            '        Try
            '            'litLastBilled.Text = CDate(billed_dt.Rows(0).Item("max_bill_date")).ToString("MM/dd/yyyy")
            '        Catch ex As Exception

            '        End Try

            '        litPendTime.Text = bill_dr.Item("unpaidTime").ToString
            '        Try
            '            litPendAmount.Text = FormatCurrency(bill_dr.Item("unpaid_amount").ToString, 2)
            '        Catch ex As Exception

            '        End Try

            '        litPendNumberCalls.Text = bill_dr.Item("unpaid_calls").ToString
            '        Try
            '            'litBilledAmount.Text = FormatCurrency(billed_dt.Rows(0).Item("current_due_amount").ToString, 2)
            '        Catch ex As Exception

            '        End Try

            '        'If bill_dr.Item("over_due_amount").ToString <> "" Then
            '        '    Page.ClientScript.RegisterStartupScript(Me.GetType, "make_red", "$('.filterBtn2').css('background-color','red');", True)
            '        'End If

            '    Next

            'End If

        End If




        start_date = date1.Text
        end_date = date2.Text






    End Sub



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




    Protected Sub btnApplyFilter_Click(sender As Object, e As EventArgs) 'Handles lbApply.Click
        Recalc_Elements()
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


        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub



    Protected Sub btnTopAgents()
        Response.Redirect("all_agents.aspx")
    End Sub


    Protected Sub date1_TextChanged(sender As Object, e As EventArgs) Handles date1.TextChanged
        Session("StartDate") = date1.Text
        Recalc()
    End Sub

    Protected Sub date2_TextChanged(sender As Object, e As EventArgs) Handles date2.TextChanged
        Session("EndDate") = date2.Text
        Recalc()
    End Sub



    Protected Sub btnViewAllCalls()
        Response.Redirect("expandedview.aspx")
    End Sub

    Private Sub rptBilling_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptBilling.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Then
            Dim drv As DataRowView = e.Item.DataItem
            If drv("font_color") = "red" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, "make_red", "$('.filterBtn2').css('background-color','red');", True)
            End If
        End If

    End Sub



End Class
