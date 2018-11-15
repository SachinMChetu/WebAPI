Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Partial Class listen3
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Public myDelay As String = ""
    Public close_me As Boolean = False
    Public redirect_save As Boolean = True

    Public isWebsite As Boolean = False
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo  with (nolock)  where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
            myDelay = user_info_dt.Rows(0).Item("guideline_display").ToString
            'If user_info_dt.Rows(0).Item("presubmit").ToString = "True" Then
            '    btnSaveSession.Attributes.Add("onclick", "if(Page_ClientValidate(''))OpenInNewTab('listen.aspx');")
            '    close_me = True
            'End If
            If user_info_dt.Rows(0).Item("user_role").ToString = "QA Lead" Or user_info_dt.Rows(0).Item("user_role").ToString = "Calibrator" Or user_info_dt.Rows(0).Item("user_role").ToString = "Admin" Then
                btnSaveSessionPlus.Visible = True
            End If

        Else
            data_rate = 0.05
        End If


        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=listen.aspx")
        End If



        'If User.IsInRole("Trainee") Then
        '    Response.Redirect("listen_training.aspx")
        'End If



        'Disable button on click, still allow server processing
        'btnSaveSession.Attributes.Add("onclick", "alert('Testing button');if(Page_ClientValidate()){alert('Page Valid');$(this).toggle();}")

        If Not IsPostBack Then


            'btnSaveSession.Attributes.Add("onclick", "$(this).toggle();")

            Dim noti_check_dt As DataTable = GetTable("SELECT count(*) FROM vwFN with (nolock)   " &
                          "WHERE  date_closed is null and sup_override is null and role = 'QA' and reviewer = '" & User.Identity.Name & "'")

            If noti_check_dt.Rows.Count > 0 Then 'And User.IsInRole("Admin") Then
                If noti_check_dt.Rows(0).Item(0) > 0 Then
                    Response.Redirect("cd2.aspx?error=open_notes")
                    'ClientScript.RegisterStartupScript(Me.GetType(), "onclick", "<script language=javascript>alert('You have " & noti_check_dt.Rows(0).Item(0) & " open notifications.  You must clear them before you can work other calls. Go to Notifications now if you want to clear them.');</script>")
                End If
            End If



            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)


            Dim addl_sql As String = " And ((review_started < DateAdd(mi, -60, dbo.getMTDate())) Or (review_started Is null)) And max_reviews = 0 "

            If Request("force_app") IsNot Nothing Then
                addl_sql &= " and b.appname = '" & Request("force_app") & "' "
            End If

            If Request("force_sc") IsNot Nothing Then
                addl_sql &= " and b.scorecard = '" & Request("force_sc") & "' "
            End If

            If Request("session_id") IsNot Nothing Then
                addl_sql = " and b.session_id = '" & Request("session_id") & "' "
            End If

            If Request("phone") IsNot Nothing Then
                addl_sql = " and b.phone = '" & Request("phone") & "' "
            End If

            If Request("scorecard") IsNot Nothing Then
                addl_sql = " and b.scorecard= (select id from scorecards with (nolock)   where short_name = '" & Request("scorecard") & "' and active = 1) "
            End If

            Dim dt2 As DataTable

            If User.IsInRole("QA") Then

                Dim skip_dt As DataTable = GetTable("select top 1 xcc_report_new.id from session_viewed with(nolock) join xcc_report_new with(nolock) on xcc_report_new.id= session_viewed.session_id and page_viewed = 'listen' and max_reviews = 0 and scorecard in (select sc_id from sc_training_approvals  with (nolock)   where username =  '" & User.Identity.Name & "') and date_completed is null and session_viewed.agent = '" & User.Identity.Name & "'")


                If skip_dt.Rows.Count > 0 Then
                    addl_sql = " and b.id = '" & skip_dt.Rows(0).Item(0) & "' "
                End If
            End If

            Dim listen_sql As String = "select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock) " &
                           "join UserApps with  (nolock)  on UserApps.user_scorecard = b.scorecard  join app_settings with (nolock) on app_settings.appname = b.appname " &
                           "join scorecards with  (nolock)  on scorecards.id = b.scorecard " &
                           "where  ((b.scorecard in (select sc_id from sc_training_approvals  with (nolock)   where username =  '" & User.Identity.Name & "')) or (website is not null) or (scorecard_role in( 'QA Lead','Calibrator')) ) and  " &
                           "userapps.username = '" & User.Identity.Name & "'  " &
                           "and (audio_link is not null or website is not null) and session_id is not null and ((left(audio_link,6) = '/audio') or (stream_only = 1)) and scorecards.active = 1 and app_settings.active = 1 and isnull(scorecard_role,'') != 'Trainee' " & addl_sql & " order by user_priority,isnull(sort_order,1000),  MAX_REVIEWS,  " &
                           "case when scorecards.sc_sort = 'desc' then b.call_date  end desc, case when scorecards.sc_sort = 'asc' or scorecards.sc_sort is null then b.call_date end, review_started"

            'Response.Write(listen_sql)
            'Response.End()
            dt2 = GetTable(listen_sql)
            ' join scorecards on scorecards.id = b.scorecard and userapps.user_scorecard = scorecards.id
            'Response.Write("select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock)  join app_settings on app_settings.appname = b.appname join UserApps on UserApps.appname = b.appname   where max_reviews = 0 and ((review_started < dateadd(mi,-60, dbo.getMTDate())) or (review_started is null)) and username = '" & User.Identity.Name & "'  and audio_link is not null and app_settings.active = 1 and session_id is not null " & addl_sql & "  order by user_priority, MAX_REVIEWS, call_date, review_started, campaign")
            'Response.End()

            If dt2.Rows.Count > 0 Then
                If Request.QueryString.Count = 0 Then
                    UpdateTable("update XCC_REPORT_NEW set review_started = dbo.getMTDate() where ID = " & dt2.Rows(0).Item("ID").ToString)
                End If
            Else
                Response.Redirect("default.aspx")
            End If


            If dt2.Rows(0).Item("scorecard").ToString = "396" Then
                Response.Redirect("listen_vb.aspx")
            End If

            If dt2.Rows(0).Item("website").ToString <> "" Then
                isWebsite = True
            End If


            If dt2.Rows(0).Item("must_review").ToString = "true" Then
                btnBadCall.Visible = False
                txtOtherReason.Visible = False
                ddlDropReason.Visible = False
                lblOtherReason.Text = "call must be reviewed -- do not mark as bad."
                lblOtherReason.Font.Bold = True
            End If

            hdnCampaign.Value = dt2.Rows(0).Item("campaign").ToString
            hdnThisScorecard.Value = dt2.Rows(0).Item("scorecard").ToString
            hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString
            hdnCallLength.Value = dt2.Rows(0).Item("call_duration").ToString

            If dt2.Rows.Count > 0 Then
                'Personal
                litPersonal.Text = litPersonal.Text & getSidebarData("First Name", dt2.Rows(0).Item("First_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Last Name", dt2.Rows(0).Item("Last_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Email", dt2.Rows(0).Item("Email").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Education Level", dt2.Rows(0).Item("EducationLevel").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("High School Grad Year", dt2.Rows(0).Item("HighSchoolGradYear").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Degree Start Timeframe", dt2.Rows(0).Item("DegreeStartTimeframe").ToString())

                'Contact
                litPersonal.Text = litPersonal.Text & getSidebarData("Address", dt2.Rows(0).Item("address").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("City", dt2.Rows(0).Item("City").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("State", dt2.Rows(0).Item("State").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Zip", dt2.Rows(0).Item("Zip").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Phone", dt2.Rows(0).Item("Phone").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Website", dt2.Rows(0).Item("Website").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Session", dt2.Rows(0).Item("Session_id").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Campaign", dt2.Rows(0).Item("Campaign").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Call Type", dt2.Rows(0).Item("call_type").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Notes", dt2.Rows(0).Item("disposition").ToString())


                If dt2.Rows(0).Item("appname").ToString().ToLower() = "keypath" Then
                    litPersonal.Text = litPersonal.Text & getSidebarData("Agent", dt2.Rows(0).Item("agent").ToString())
                End If


                If isWebsite Then
                    litPersonal.Text = litPersonal.Text & getSidebarData("Agent", dt2.Rows(0).Item("agent").ToString())
                End If

                litPersonal.Text = litPersonal.Text & getSidebarData("Compliance Sheet", dt2.Rows(0).Item("compliance_sheet").ToString())
                Try
                    litPersonal.Text = litPersonal.Text & getSidebarData("Call Date", CDate(dt2.Rows(0).Item("call_date")).ToShortDateString)
                Catch ex As Exception

                End Try
                litPersonal.Text = litPersonal.Text & getSidebarData("Scorecard", dt2.Rows(0).Item("short_name").ToString())


                'Dim keyword_dt As DataTable = GetTable("select * from utterance_flags where scorecard = " & dt2.Rows(0).Item("scorecard").ToString())
                'If keyword_dt.Rows.Count > 0 Then
                '    litTextOnly.Text = "<b>Watch out for these keywords:</b> "
                '    divHiddenText.Visible = True
                '    For Each key_dr In keyword_dt.Rows
                '        litTextOnly.Text = litTextOnly.Text & key_dr("utterance").ToString & ", "
                '    Next
                '    litTextOnly.Text = litTextOnly.Text & "<br><hr></hr></br>"
                'End If

                If dt2.Rows(0).Item("transcript_flagged_reason").ToString() <> "" Then
                    divHiddenText.Visible = True
                    litTextOnly.Text = litTextOnly.Text & "Listen for these: " & dt2.Rows(0).Item("transcript_flagged_reason").ToString().Replace("<br>", " ")
                    'litTextOnly.Text = litTextOnly.Text & " <i class='fa fa-eye' onclick='show_transcript();' ></i>"
                    'litTextOnly.Text = litTextOnly.Text & "<div class='transcripttext' style='display:none'>" & dt2.Rows(0).Item("transcript").ToString() & "</div>"
                End If

                If dt2.Rows(0).Item("text_only").ToString() <> "" Then
                    litTextOnly.Text = litTextOnly.Text & dt2.Rows(0).Item("text_only").ToString()
                    divHiddenText.Visible = True
                End If




                'If dt2.Rows(0).Item("transcript").ToString() <> "" Then
                '    litTextOnly.Text = litTextOnly.Text & dt2.Rows(0).Item("transcript").ToString()
                '    divHiddenText.Visible = True
                'End If

                If dt2.Rows(0).Item("website").ToString() <> "" Then
                    ddlWhisper.SelectedValue = "0"
                    ClientScript.RegisterStartupScript(Me.GetType(), "show_website", "window.open('" & dt2.Rows(0).Item("website") & "','myWindow', 'width=800, height=600');", True)
                End If

                Dim od_dt As DataTable = GetTable("select distinct data_key, data_value from (select top 100 percent data_key, data_value from otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString & " and data_type <> 'School' and isnull(school_name,'') = ''   order by id) a")
                If od_dt.Rows.Count > 0 Then
                    divOtherCard.Visible = True
                    For Each dr As DataRow In od_dt.Rows
                        litOtherData.Text = litOtherData.Text & getSidebarData(dr("data_key").ToString(), dr("data_value").ToString().Replace(Chr(13), "<br>") & " ")
                    Next
                End If



                Dim sch_dt As DataTable = GetTable("select  distinct School,AOI1,AOI2,L1_SubjectName,L2_SubjectName,Modality,College,DegreeOfInterest,origin,tcpa from School_X_Data with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString)
                Dim sch_x As Integer = 1
                For Each sch_dr In sch_dt.Rows
                    litSchool.Text = litSchool.Text & " <tr><td class='school-name' colspan='2'>" & Replace(sch_dr.Item("School").ToString(), "'", "''") & "</td></tr>"

                    litSchool.Text = litSchool.Text & getSidebarData("College", sch_dr.Item("College").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Degree", sch_dr.Item("DegreeOfInterest").ToString())

                    'litSchool.Text = litSchool.Text & getSidebarData("School " & sch_x, sch_dr.Item("School").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 1", sch_dr.Item("AOI1").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 2", sch_dr.Item("AOI2").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 1", sch_dr.Item("L1_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 2", sch_dr.Item("L2_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Modality", sch_dr.Item("Modality").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Portal", sch_dr.Item("origin").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("TCPA", sch_dr.Item("tcpa").ToString())


                    Dim other_dt As DataTable = GetTable("select distinct data_key, data_value from (select top 100 percent data_key, data_value  from  otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString & " and school_name = '" & Replace(sch_dr.Item("School").ToString(), "'", "''") & "' and data_key not in (select val from  dbo.split((select hide_data from scorecards where id = '" & dt2.Rows(0).Item("scorecard") & "'),'|')) order by id) a")
                    For Each other_dr In other_dt.Rows
                        litSchool.Text = litSchool.Text & getSidebarData(other_dr.item("data_key").ToString(), other_dr.item("data_value").ToString().Replace("&#13;", "<br>") & " ")
                    Next

                    sch_x += 1
                Next


            Else
                ' liSchoolItem.Visible = False
            End If

            'Other
            'litOther.Text = litOther.Text & getSidebarData("CAMPAIGN", dt2.Rows(0).Item("CAMPAIGN").ToString())
            'litOther.Text = litOther.Text & getSidebarData("DATE", dt2.Rows(0).Item("call_date").ToString())

            'Dim history_dt As DataTable = GetTable("select * from session_viewed where session_id = '" & dt2.Rows(0).Item("ID").ToString() & "'")
            'If history_dt.Rows.Count > 0 Then
            '    'litOther.Text = litOther.Text & "<center>Session Viewed</center><br>"
            '    For Each dr As DataRow In history_dt.Rows()
            '        'litOther.Text = litOther.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("agent") & "</span><strong>" & dr.Item("date_viewed") & "</strong></li>"
            '    Next
            'End If
            hdnXCCID.Value = dt2.Rows(0).Item("ID").ToString
            hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString


            If dt2.Rows(0).Item("client_logo").ToString() = "" Then
                lblThisApp.Text = "Call from " & dt2.Rows(0).Item("FullName").ToString()
            Else
                lblThisApp.Text = "<img src='" & dt2.Rows(0).Item("client_logo").ToString() & "' />"
            End If


            'lblThisApp.Text = dt2.Rows(0).Item("appname").ToString
            'lblArea1.Text = dt2.Rows(0).Item("program").ToString
            'lblArea2.Text = dt2.Rows(0).Item("ID").ToString


            Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
            ' lblSession.Text = session_id.ToString

            five9update.record_ID = dt2.Rows(0).Item("ID").ToString

            If dt2.Rows(0).Item("stream_only").ToString = "True" Then
                audio_file = dt2.Rows(0).Item("audio_link").ToString
            Else
                audio_file = GetAudioFileName(dt2.Rows(0))
            End If

            'Dim this_filename As String = GetAudioFileName(dt2.Rows(0))


            UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" & User.Identity.Name & "',dbo.getMTDate(), " & dt2.Rows(0).Item("ID").ToString & ",'listen'")



        End If


        'End If


    End Sub

    Protected Sub btnSaveSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSession.Click, btnSaveSessionPlus.Click

        Dim btn As Button = sender
        If btn.ID = "btnSaveSession" Then
            save_data(0)
        Else
            save_data(1)
            'UpdateTable("exec copyFormToCalibration " & save_data())
        End If


        If close_me Then

            Response.Redirect("close.html")

            'ClientScript.RegisterStartupScript(Me.GetType(), "close_me", "window.close();", True)
            'Response.Flush()
            'Response.End()
        Else
            If chkStopWorking.Checked Then
                Response.Redirect("default.aspx")
            Else
                Response.Redirect("listen.aspx")
            End If
        End If




    End Sub

    Protected Function save_data(copy_to_cali As Integer) As String
        Dim total_score As Integer = 0

        Dim new_ID As String = ""

        Dim existing_dt As DataTable = GetTable("select count(*) from vwForm with (nolock)  where review_id = " & hdnXCCID.Value)



        Dim fail_all As Boolean = False
        Dim fail_list As New ArrayList

        'Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score3] (reviewer,session_id,review_date,[review_ID],Comments, appname, whisperID, QAwhisper) values 
        '    ('" & User.Identity.Name & "','" & lblSession.Value & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & Trim(txtComments.Text.Replace("'", "''")) & "','" & hdnThisApp.Value & "','" & hdnWhisper.Value & "','" & ddlWhisper.SelectedValue & "'); select @new_ID = scope_identity(); select @new_ID;"
        ''Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score3] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Value & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & Trim(txtComments.Text.Replace("'", "''")) & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"
        'Response.Write(sql)
        'Response.End()

        'If existing_dt.Rows(0).Item(0) = 0 Then 'if already reviewed don't create a new record
        '    Email_Error("Record duplicated - " & sql)
        'End If



        Dim listen_dt As New DataTable
        listen_dt.Columns.Add("reviewer", Type.GetType("System.String"))
        listen_dt.Columns.Add("session_id", Type.GetType("System.String"))
        listen_dt.Columns.Add("review_ID", Type.GetType("System.Int32"))
        listen_dt.Columns.Add("Comments", Type.GetType("System.String"))
        listen_dt.Columns.Add("appname", Type.GetType("System.String"))
        listen_dt.Columns.Add("whisperID", Type.GetType("System.Int32"))
        listen_dt.Columns.Add("QAwhisper", Type.GetType("System.Int32"))
        listen_dt.Columns.Add("qa_start", Type.GetType("System.Int32"))
        listen_dt.Columns.Add("qa_last_action", Type.GetType("System.Int32"))
        listen_dt.Columns.Add("call_length", Type.GetType("System.Single"))
        listen_dt.Columns.Add("copy_to_cali", Type.GetType("System.Int32"))

        Dim listen_dr As DataRow = listen_dt.NewRow
        listen_dr("reviewer") = User.Identity.Name
        listen_dr("session_id") = lblSession.Value
        listen_dr("review_ID") = hdnXCCID.Value
        listen_dr("Comments") = Trim(txtComments.Text)
        listen_dr("appname") = hdnThisApp.Value
        listen_dr("whisperID") = hdnWhisper.Value
        listen_dr("QAwhisper") = ddlWhisper.SelectedValue
        listen_dr("qa_start") = CInt(hdnFirstClientAction.Value / 1000)
        If hdnLastClientAction.Value <> "" Then
            listen_dr("qa_last_action") = CInt(hdnLastClientAction.Value / 1000)
        Else
            listen_dr("qa_last_action") = CInt(hdnFirstClientAction.Value / 1000)
        End If
        listen_dr("copy_to_cali") = copy_to_cali
        Try
            If hdnTotalCallLength.Value <> "NaN" Then
                listen_dr("call_length") = hdnTotalCallLength.Value
            End If
        Catch ex As Exception

        End Try




        listen_dt.Rows.Add(listen_dr)

        'Dim dt_new_ID_dt As DataTable = GetTable(sql)

        'If dt_new_ID_dt.Rows.Count = 0 Then
        '    dt_new_ID_dt = GetTable(sql) ' try again, same one
        'End If

        'new_ID = dt_new_ID_dt.Rows(0).Item(0).ToString


        'If hdnFirstClientAction.Value <> "" Then
        '    UpdateTable("update form_score3 set qa_start = dateadd(second, " & CInt(hdnFirstClientAction.Value / 1000) & ",'1/1/1970') where id = " & new_ID)
        'End If



        'If hdnLastClientAction.Value <> "" Then
        '    UpdateTable("update form_score3 set qa_last_action = dateadd(second, " & CInt(hdnLastClientAction.Value / 1000) & ",'1/1/1970') where id = " & new_ID)
        'End If


        Dim sc_dt As New DataTable
        sc_dt.Columns.Add("comment_who", Type.GetType("System.String"))
        sc_dt.Columns.Add("comment", Type.GetType("System.String"))
        sc_dt.Columns.Add("comment_type", Type.GetType("System.String"))
        sc_dt.Columns.Add("comment_pos", Type.GetType("System.String"))
        sc_dt.Columns.Add("comment_header", Type.GetType("System.String"))



        If txtComments.Text <> "" Then
            ' UpdateTable("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id) select reviewer, review_date, comments, 'Call', id from form_score3 where id =" & new_ID)

            ' Try
            Dim comment_title() As String = Request.Form.GetValues("comment_title")
            Dim more_comments() As String = Request.Form.GetValues("more_comments")
            Dim comment_time() As String = Request.Form.GetValues("comment_time")

            Dim comment_counter As Integer = 0

            For Each comm_title In more_comments
                If comm_title <> "" Then


                    'Response.Write(more_comments(comment_counter) & "<br>")
                    'Response.Write(comment_time(comment_counter) & "<br>")
                    'Response.Write(comment_title(comment_counter) & "<br>")
                    Dim sc_dr As DataRow = sc_dt.NewRow
                    sc_dr("comment_who") = User.Identity.Name
                    sc_dr("comment") = more_comments(comment_counter) & ""
                    sc_dr("comment_type") = "Call"
                    sc_dr("comment_pos") = comment_time(comment_counter) & ""
                    sc_dr("comment_header") = comment_title(comment_counter) & ""

                    sc_dt.Rows.Add(sc_dr)

                    'UpdateTable("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id, comment_pos, comment_header) select reviewer, review_date,'" & more_comments(comment_counter).Replace("'", "''") & "' , 'Call', id, '" & comment_time(comment_counter).Replace("'", "''") & "','" & comment_title(comment_counter).Replace("'", "''") & "' from form_score3 with  (nolock)  where id =" & new_ID)
                    'Response.Write("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id, comment_pos, comment_header) select reviewer, review_date,'" & more_comments(comment_counter).Replace("'", "''") & "' , 'Call', id, '" & comment_time(comment_counter).Replace("'", "''") & "','" & comment_title(comment_counter).Replace("'", "''") & "' from form_score3 where id =" & new_ID)
                End If
                comment_counter += 1
            Next

            ' Response.End()

            'Catch ex As Exception
            '    Email_Error("@ listen sys comment<br><br>" & ex.Message)
            'End Try
        End If

        Dim big_insert As String = ""

        Dim FQS_dt As New DataTable
        FQS_dt.Columns.Add("q_position", Type.GetType("System.String"))
        FQS_dt.Columns.Add("question_id", Type.GetType("System.Int32"))
        FQS_dt.Columns.Add("question_result", Type.GetType("System.Int32"))
        FQS_dt.Columns.Add("question_answered", Type.GetType("System.String"))
        FQS_dt.Columns.Add("click_text", Type.GetType("System.String"))
        FQS_dt.Columns.Add("other_answer_text", Type.GetType("System.String"))
        FQS_dt.Columns.Add("view_link", Type.GetType("System.String"))


        Dim FQR_dt As New DataTable
        FQR_dt.Columns.Add("question_id", Type.GetType("System.String"))
        FQR_dt.Columns.Add("answer_id", Type.GetType("System.Int32"))
        FQR_dt.Columns.Add("other_answer_text", Type.GetType("System.String"))

        Dim FQSO_dt As New DataTable
        FQSO_dt.Columns.Add("option_pos", Type.GetType("System.Int32"))
        FQSO_dt.Columns.Add("option_value", Type.GetType("System.String"))
        FQSO_dt.Columns.Add("question_id", Type.GetType("System.Int32"))
        FQSO_dt.Columns.Add("orig_id", Type.GetType("System.Int32"))


        For Each li As RepeaterItem In rptSections.Items


            'Find the questions in each item
            Dim rptQ As Repeater = li.FindControl("DataList1")
            For Each qRi As RepeaterItem In rptQ.Items
                Dim hdnQID As HiddenField = qRi.FindControl("hdnQID")
                Dim hdnHasOptions As HiddenField = qRi.FindControl("hdnHasOptions")

                If hdnHasOptions.Value <> "has-sub-items" Then ' Regular Yes/No
                    Dim rptAnswers As Repeater = qRi.FindControl("rptAnswers")
                    For Each aRI As RepeaterItem In rptAnswers.Items
                        Dim hdnAnswerID As HiddenField = aRI.FindControl("hdnAnswerID")
                        Dim txtClickTime As TextBox = aRI.FindControl("txtClickTime")

                        If txtClickTime.Text <> "" Then 'this has the time, was the answer, write to DB
                            'ClickTime from MM:SS to seconds
                            Dim times() As String = txtClickTime.Text.Split(":")
                            Dim total_sec As Integer = CInt(times(0)) * 60 + CInt(times(1))

                            Dim txtWebsiteLink As TextBox = aRI.FindControl("txtWebsiteLink")
                            Dim chkWebsite As CheckBox = aRI.FindControl("chkWebsite")

                            '  If chkWebsite.Checked Then

                            Dim FQS_dr As DataRow = FQS_dt.NewRow
                            FQS_dr("q_position") = total_sec
                            FQS_dr("question_id") = hdnQID.Value
                            FQS_dr("question_result") = 0
                            FQS_dr("question_answered") = hdnAnswerID.Value
                            FQS_dr("click_text") = txtClickTime.Text
                            If chkWebsite.Checked Then
                                FQS_dr("view_link") = txtWebsiteLink.Text
                            End If

                            FQS_dt.Rows.Add(FQS_dr)
                            'big_insert &= "insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, original_question_answered, click_text,view_link ) 
                            'values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "','" & txtWebsiteLink.Text & "' );"

                            '    Else
                            '        Dim FQS_dr As DataRow = FQS_dt.NewRow
                            '    FQS_dr("q_position") = total_sec
                            '    FQS_dr("question_id") = hdnQID.Value
                            '    FQS_dr("question_result") = 0
                            '    FQS_dr("question_answered") = hdnAnswerID.Value
                            '    FQS_dr("original_question_answered") = hdnAnswerID.Value
                            '    FQS_dr("click_text") = txtClickTime.Text
                            '    'FQS_dr("view_link") = txtWebsiteLink.Text

                            '    FQS_dt.Rows.Add(FQS_dr)
                            '    big_insert &= "insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, original_question_answered, click_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "');"
                            'End If


                            Dim chkOptions As CheckBoxList = aRI.FindControl("chkOptions")


                            Dim answer_selected As Boolean = False

                            For Each cblItem As ListItem In chkOptions.Items
                                If cblItem.Selected Then

                                    answer_selected = True

                                    Dim FQR_dr As DataRow = FQR_dt.NewRow
                                    FQR_dr("question_id") = hdnQID.Value
                                    FQR_dr("answer_id") = cblItem.Value
                                    FQR_dt.Rows.Add(FQR_dr)


                                    'big_insert &= "insert into form_q_responses (question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & ",'" & cblItem.Value & "');"

                                End If
                            Next


                            Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                            Dim chkComment As CheckBox = aRI.FindControl("chkComment")
                            If chkComment.Checked Then
                                answer_selected = True

                                Dim FQR_dr As DataRow = FQR_dt.NewRow
                                FQR_dr("question_id") = hdnQID.Value
                                FQR_dr("answer_id") = 0
                                FQR_dr("other_answer_text") = txtOtherComment.Text
                                FQR_dt.Rows.Add(FQR_dr)
                                'big_insert &= "insert into form_q_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherComment.Text.Replace("'", "''") & "');"
                            End If

                            If Not answer_selected Then
                                Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments  with (nolock)   where question_id = " & hdnQID.Value & " and answer_id = " & hdnAnswerID.Value & " order by isnull(ac_order,99)")
                                If default_ans.Rows.Count > 0 Then

                                    Dim FQR_dr As DataRow = FQR_dt.NewRow
                                    FQR_dr("question_id") = hdnQID.Value
                                    FQR_dr("answer_id") = default_ans.Rows(0).Item("ID")
                                    FQR_dt.Rows.Add(FQR_dr)
                                    'big_insert &= "insert into form_q_responses ( question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & "," & default_ans.Rows(0).Item("ID") & ");"
                                End If
                            End If


                            'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "))")


                        End If
                    Next
                End If

                If hdnHasOptions.Value = "has-sub-items" Then ' checklist

                    Dim rptContactList As Repeater = qRi.FindControl("rptContactList")
                    Dim all_checked As Boolean = True

                    If rptContactList IsNot Nothing Then

                        Dim min_check As Integer = 10000




                        Dim chkOtherList As CheckBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("chkOtherList")
                        If chkOtherList.Checked Then
                            Dim txtOtherList As TextBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("txtOtherList")

                            Dim FQR_dr As DataRow = FQR_dt.NewRow
                            FQR_dr("question_id") = hdnQID.Value
                            FQR_dr("answer_id") = 0
                            FQR_dr("other_answer_text") = txtOtherList.Text
                            FQR_dt.Rows.Add(FQR_dr)
                            'big_insert &= "insert into form_q_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "');"
                            'Response.Write("insert into form_q_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")
                        End If

                        Dim na_checked As Boolean = False

                        For Each CIRI As RepeaterItem In rptContactList.Items
                            Dim chkOption As CheckBox = CIRI.FindControl("chkOption")

                            If chkOption.Checked Then
                                Dim txtCheckTime As TextBox = CIRI.FindControl("txtCheckTime")
                                Dim hdnOrigId As HiddenField = CIRI.FindControl("hdnOrigId")

                                Dim times() As String = txtCheckTime.Text.Split(":")
                                Dim total_sec As Integer = 0
                                Try
                                    total_sec = CInt(times(0)) * 60 + CInt(times(1))
                                Catch ex As Exception

                                End Try

                                If total_sec < min_check And total_sec > 0 Then
                                    min_check = total_sec
                                End If

                                If chkOption.Text = "NA" Then
                                    na_checked = True
                                Else
                                    Dim FQSO_dr As DataRow = FQSO_dt.NewRow
                                    FQSO_dr("option_pos") = total_sec
                                    FQSO_dr("option_value") = chkOption.Text
                                    FQSO_dr("question_id") = hdnQID.Value
                                    FQSO_dr("orig_id") = hdnOrigId.Value
                                    FQSO_dt.Rows.Add(FQSO_dr)
                                    'big_insert &= "insert into form_q_scores_options (option_pos, option_value, question_id, form_id, orig_id) values('" & total_sec & "','" & chkOption.Text.Replace("'", "''") & "'," & hdnQID.Value & "," & new_ID & "," & hdnOrigId.Value & ");"
                                End If
                            Else
                                If chkOption.Text <> "NA" Then
                                    all_checked = False
                                End If
                            End If
                        Next

                        'Dim ContactInfo As HtmlGenericControl = rptContactList.FindControl("CIInfo")

                        Dim ContactInfo As Label = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("CIInfo")

                        'If ContactInfo IsNot Nothing Then

                        If min_check = 10000 Then min_check = 0



                        Dim txtWebsiteLink As TextBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("txtWebsiteLink")
                        Dim chkWebsite As CheckBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("chkWebsite")


                        Dim add_link As String = ""
                        Dim add_link_column As String = ""

                        If chkWebsite.Checked Then
                            add_link = ",'" & txtWebsiteLink.Text & "' "
                            add_link_column = ",view_link "
                        End If


                        If na_checked Then
                            Dim FQS_dr As DataRow = FQS_dt.NewRow
                            FQS_dr("q_position") = min_check
                            FQS_dr("question_id") = hdnQID.Value
                            FQS_dr("question_result") = 0
                            FQS_dr("question_answered") = "NA"
                            FQS_dr("other_answer_text") = ContactInfo.Text
                            If chkWebsite.Checked Then
                                FQS_dr("view_link") = txtWebsiteLink.Text
                            End If

                            FQS_dt.Rows.Add(FQS_dr)

                            '    big_insert &= "insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, original_question_answered, other_answer_text " & add_link_column & ") 
                            'values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers  with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'NA'),(select id from question_answers  with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'NA'),'" & ContactInfo.Text.Replace("'", "''") & "'" & add_link & ");"
                        Else
                            If all_checked Then 'this has the time, was the answer, write to DB
                                'ClickTime from MM:SS to seconds

                                Dim FQS_dr As DataRow = FQS_dt.NewRow
                                FQS_dr("q_position") = min_check
                                FQS_dr("question_id") = hdnQID.Value
                                FQS_dr("question_result") = 0
                                FQS_dr("question_answered") = "Yes"
                                FQS_dr("other_answer_text") = ContactInfo.Text
                                If chkWebsite.Checked Then
                                    FQS_dr("view_link") = txtWebsiteLink.Text
                                End If

                                FQS_dt.Rows.Add(FQS_dr)

                                'big_insert &= "insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, original_question_answered, other_answer_text " & add_link_column & ") values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers  with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),(select id from question_answers  with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & ContactInfo.Text.Replace("'", "''") & "'" & add_link & ");"
                            Else
                                Dim FQS_dr As DataRow = FQS_dt.NewRow
                                FQS_dr("q_position") = min_check
                                FQS_dr("question_id") = hdnQID.Value
                                FQS_dr("question_result") = 0
                                FQS_dr("question_answered") = "NO"
                                FQS_dr("other_answer_text") = ContactInfo.Text
                                If chkWebsite.Checked Then
                                    FQS_dr("view_link") = txtWebsiteLink.Text
                                End If

                                FQS_dt.Rows.Add(FQS_dr)
                                'big_insert &= "insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, original_question_answered, other_answer_text " & add_link_column & ") values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'No'),(select id from question_answers  with (nolock)   where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & ContactInfo.Text.Replace("'", "''") & "'" & add_link & ");"
                            End If
                        End If





                    End If
                    ' Next
                End If


            Next

        Next

        ' UpdateTable(big_insert)


        ' Save additional clerked data
        Dim CD_dt As New DataTable
        CD_dt.Columns.Add("value_id", Type.GetType("System.Int32"))
        CD_dt.Columns.Add("value_data", Type.GetType("System.String"))
        CD_dt.Columns.Add("value_position", Type.GetType("System.String"))


        Try
            For Each ri As RepeaterItem In rptClerked.Items
                Dim txtTextField As TextBox = ri.FindControl("txtTextField")
                Dim hdnValueID As HiddenField = ri.FindControl("hdnValueID")
                Dim hdnTimestamp As HiddenField = ri.FindControl("hdnTimestamp")

                Dim CD_dr As DataRow = CD_dt.NewRow

                CD_dr("value_id") = hdnValueID.Value
                CD_dr("value_data") = txtTextField.Text
                CD_dr("value_position") = hdnTimestamp.Value

                CD_dt.Rows.Add(CD_dr)
                'UpdateTable("insert into collected_data(form_id, value_id, value_data, value_position) select " & new_ID & "," & hdnValueID.Value & ",'" & txtTextField.Text.Replace("'", "''") & "','" & hdnTimestamp.Value & "'")

            Next
        Catch ex As Exception

        End Try



        Dim KW_dt As New DataTable
        KW_dt.Columns.Add("category", Type.GetType("System.String"))
        KW_dt.Columns.Add("keyword", Type.GetType("System.String"))
        KW_dt.Columns.Add("relevance", Type.GetType("System.Int32"))


        For Each key As String In Request.Form.Keys
            'Response.Write((key & Convert.ToString(": ")) + Request.Form(key) + "<br/>")
            If key.IndexOf("ctl00$ContentPlaceHolder1$cat_") > -1 Then
                'Response.Write((key & Convert.ToString(": ")) + Request.Form(key) + "<br/>")
                Dim kw_dr As DataRow = KW_dt.NewRow
                'has category
                Dim category As String = key.Replace("ctl00$ContentPlaceHolder1$cat_", "")

                kw_dr("category") = category
                kw_dr("keyword") = ""
                kw_dr("relevance") = Request.Form(key)
                KW_dt.Rows.Add(kw_dr)

            End If

            If key.IndexOf("ctl00$ContentPlaceHolder1$key_") > -1 Then
                'Response.Write((key & Convert.ToString(": ")) + Request.Form(key) + "<br/>")
                Dim kw_dr As DataRow = KW_dt.NewRow
                'has category
                Dim keyword As String = key.Replace("ctl00$ContentPlaceHolder1$key_", "")
                kw_dr("category") = ""
                kw_dr("keyword") = keyword
                kw_dr("relevance") = Request.Form(key)
                KW_dt.Rows.Add(kw_dr)

            End If

            'Response.Write((key & Convert.ToString(": ")) + Request.Form(key) + "<br/>")
        Next



        Using command = New SqlCommand("listenDataInsert")
            command.CommandType = CommandType.StoredProcedure
            'create your own data table
            command.Parameters.Add(New SqlParameter("@ListenInsert", listen_dt))
            command.Parameters.Add(New SqlParameter("@FQSInsert", FQS_dt))
            command.Parameters.Add(New SqlParameter("@FQRInsert", FQR_dt))
            command.Parameters.Add(New SqlParameter("@FQSOInsert", FQSO_dt))
            command.Parameters.Add(New SqlParameter("@SCInsert", sc_dt))
            command.Parameters.Add(New SqlParameter("@CDInsert", CD_dt))
            'command.Parameters.Add(New SqlParameter("@KeywordInsert", KW_dt))
            RunSqlCommand(command)
        End Using


        Dim all_fails As String = Join(fail_list.ToArray, ",")


        Dim final_query As String = ""


        Dim existing2_dt As DataTable = GetTable("select count(*) from vwForm where review_id = " & hdnXCCID.Value)

        If existing2_dt.Rows(0).Item(0) <> 1 Then
            'delete myself ?
            UpdateTable("exec dedupe_submits")
            'Email_Error("QA duped " & hdnXCCID.Value)
        End If

        Return new_ID


    End Function

    Protected Sub UpdateCallLengths()
        Dim dt As DataTable = GetTable("select * from XCC_REPORT_NEW with  (nolock)  join form_score3 with  (nolock)  on form_score3.review_id = xcc_report_new.id where left(audio_link,6) = '/audio' and call_length is null")

        For Each dr As DataRow In dt.Rows
            If IO.File.Exists(Server.MapPath(dr("audio_link"))) Then

                Dim call_lenth As Double = GetMediaDuration(Server.MapPath(dr("audio_link")))

                'Dim tlf As TagLib.File = TagLib.File.Create(HttpContext.Current.Server.MapPath(dr("audio_link")))
                Dim call_len As TimeSpan = New TimeSpan(0, 0, CInt(call_lenth / 2))
                Dim call_time As DateTime = CDate("12/30/1899") + call_len

                Try
                    UpdateTable("update XCC_REPORT_NEW set call_time = '" & call_time.ToString & "' where ID = " & dr("ID").ToString)
                    UpdateTable("update form_score3 set call_length = '" & CInt(call_lenth / 2) & "'  where  review_id = " & dr("ID").ToString)

                Catch ex As Exception

                End Try
            End If
        Next

    End Sub

    Public data_rate As String
    Public app_list As String


    Protected Sub FixList2(sender As Object, e As System.EventArgs)
        Dim ddl As DropDownList = sender
        CType(ddl.Parent.Parent.FindControl("hdnCount"), HiddenField).Value = ddl.Items.Count
        Select Case ddl.Items.Count

            Case 3 'Yes/No Only
                CType(ddl.Parent.Parent.FindControl("pnlSelect"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator2"), RequiredFieldValidator).Enabled = False
                CType(ddl.Parent.Parent.FindControl("pnlCheck"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator3"), RequiredFieldValidator).Enabled = False

            Case 2 'Check Timestamp
                CType(ddl.Parent.Parent.FindControl("pnlYesNo"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator1"), RequiredFieldValidator).Enabled = False
                CType(ddl.Parent.Parent.FindControl("pnlSelect"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator2"), RequiredFieldValidator).Enabled = False

            Case Is > 3 ' Drop down multiple
                CType(ddl.Parent.Parent.FindControl("pnlYesNo"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator1"), RequiredFieldValidator).Enabled = False
                CType(ddl.Parent.Parent.FindControl("pnlCheck"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("RequiredFieldValidator3"), RequiredFieldValidator).Enabled = False

        End Select
    End Sub
    Protected Sub FixList(sender As Object, e As RepeaterItemEventArgs) ' Handles gvQuestions.RowDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim rbl As RadioButtonList = e.Item.FindControl("RadioButtonList1")
            Dim hdn As HiddenField = e.Item.FindControl("hdnQTimestamp")
            Dim hdnQAnswer As HiddenField = e.Item.FindControl("hdnQAnswer")
            'rbl.Attributes.Add("onclick", "check_enable('" & hdn.ClientID & "'); getTimeStamp('" & hdn.ClientID & "');")

            Dim drv As DataRowView = e.Item.DataItem

            'Dim html_item As HtmlGenericControl = e.Item.FindControl("q_trigger")
            'html_item.Attributes.Add("class", drv.Item("starting_class"))
            'If drv.Item("starting_class") = "switch answer-no" Then
            '    hdnQAnswer.Value = "No"
            'End If


        End If



        'If drv.Item("default_answer").ToString <> "" Then
        '    rbl.SelectedValue = drv.Item("default_answer").ToString
        'End If


        'If drv.Item("serious").ToString = "True" Then
        '    rbl.Items(1).Selected = True
        'End If

        'If drv.Item("auto_yes").ToString = "True" Then
        '    rbl.Items(0).Selected = True
        '    rbl.Attributes.Clear()
        '    rbl.Enabled = False
        'End If

        'If drv.Item("auto_no").ToString = "True" Then
        '    rbl.Items(1).Selected = True
        '    rbl.Attributes.Clear()
        '    rbl.Enabled = False
        'End If


    End Sub
    'Protected Sub FixList(sender As Object, e As RepeaterItemEventArgs) ' Handles gvQuestions.RowDataBound


    '    Dim rbl As RadioButtonList = e.Item.FindControl("RadioButtonList1")
    '    Dim hdn As HiddenField = e.Item.FindControl("hdnQTimestamp")

    '    rbl.Attributes.Add("onclick", "check_enable('" & hdn.ClientID & "'); getTimeStamp('" & hdn.ClientID & "');")

    '    Dim drv As DataRowView = e.Item.DataItem
    '    If drv.Item("serious").ToString = "True" Then
    '        rbl.Items(1).Selected = True
    '    End If


    'End Sub

    Protected Sub CheckForPanel(sender As Object, e As RepeaterItemEventArgs)
        Dim rptitem As RepeaterItem = e.Item
        Dim lit As Literal = rptitem.FindControl("Literal1")
        Dim drv As DataRowView = rptitem.DataItem

        'If drv("Section") = "Verification" Then

        '    lit.Text = Session("verify")
        'End If

        'If drv("Section") = "Matching" Then
        '    lit.Text = Session("college")
        'End If
        'If drv("Section") = "Contact Information" Then
        '    lit.Text = Session("person_info")
        'End If
    End Sub

    Private Function getValueOrNA(p1 As String) As String

        If [String].IsNullOrEmpty(p1) Then
            Return "NA"
        Else
            Return p1
        End If
    End Function


    Protected Sub btnBadCall_Click(sender As Object, e As EventArgs) Handles btnBadCall.Click


        'Dim existing_dt As DataTable = GetTable("select count(*) from form_score3 where review_id = " & hdnXCCID.Value)

        'If existing_dt.Rows(0).Item(0) = 0 Then 'if already reviewed don't create a new record

        '    Dim fail_all As Boolean = False
        '    Dim fail_list As New ArrayList

        '    Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score3] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Text & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & txtComments.Text.Replace("'", "''") & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"

        '    Dim dt_new_ID_dt As DataTable = GetTable(sql)

        '    Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString


        'End If

        Try

            Dim reason_text As String = ""
            If txtOtherReason.Text <> "" Then
                reason_text = txtOtherReason.Text
            Else
                reason_text = ddlDropReason.SelectedValue
            End If


            UpdateTable("update xcc_report_new set bad_call = 1, bad_call_who='" & User.Identity.Name & "', bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" & reason_text.Replace("'", "''") & "' where id = " & hdnXCCID.Value)


            If chkStopWorking.Checked Then
                Response.Redirect("default.aspx")
            Else
                Response.Redirect("listen.aspx")
            End If
        Catch ex As Exception

            Email_Error(ex.Message)
        End Try




    End Sub



    Protected Function getSidebarData(lbl As String, value As String) As String
        If value <> "" Then
            'Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"

            Return "<tr><td class='info-label'>" & Replace(lbl, "_", " ") & "</td><td class='info-data'>" & value & "</td></tr>"

        Else
            Return ""
        End If

    End Function

    Protected Sub txtClickTime_DataBinding(sender As Object, e As EventArgs)
        Dim txt As TextBox = sender
        txt.Attributes.Add("readonly", "readonly")
    End Sub

    Protected Sub chkOption_DataBinding(sender As Object, e As EventArgs)
        Dim chk As CheckBox = sender
        Dim ri As DataRowView = sender.parent.dataitem
        chk.InputAttributes.Add("data-comment-name", ri("Value").ToString)
    End Sub

    Protected Sub txtCheckTime_DataBinding(sender As Object, e As EventArgs)
        Dim txt As TextBox = sender
        txt.Attributes.Add("readonly", "readonly")
    End Sub

    Protected Sub rblOptions_DataBound(sender As Object, e As EventArgs)
        Dim rbl As RadioButtonList = sender
        'rbl.Items.Add(New ListItem("", "0"))
        If isWebsite Then
            'rbl.Items.Add(New ListItem("", "-1"))
        End If
    End Sub

    Protected Sub chkOptions_DataBound(sender As Object, e As EventArgs)
        Dim chk As CheckBoxList = sender
        'chk.Items.Add(New ListItem("", "0")) 
        If isWebsite Then
            ' CType()
        End If
    End Sub
    Protected Sub rptAnswers_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)


        Dim drv As DataRowView = e.Item.DataItem



        Dim txtOtherComment As TextBox = e.Item.FindControl("txtOtherComment")
        Dim chkComment As CheckBox = e.Item.FindControl("chkComment")
        txtOtherComment.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkComment.ClientID & "').checked = true;")

        If drv("comments_allowed").ToString = "False" Then
            txtOtherComment.Visible = False
            chkComment.Visible = False
        End If


        Dim txtWebsiteLink As TextBox = e.Item.FindControl("txtWebsiteLink")
        Dim chkWebsite As CheckBox = e.Item.FindControl("chkWebsite")
        txtWebsiteLink.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkWebsite.ClientID & "').checked = true;")

        If Not isWebsite Then
            CType(e.Item.FindControl("chkWebsite"), CheckBox).Visible = False
            CType(e.Item.FindControl("txtWebsiteLink"), TextBox).Visible = False




        End If
    End Sub


    Private Sub rptClerked_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptClerked.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Then



            Dim ri As RepeaterItem = e.Item
            Dim drv As DataRowView = ri.DataItem
            If drv("required_value").ToString = "True" Then
                CType(ri.FindControl("rfvText"), RequiredFieldValidator).Enabled = True
            Else
                CType(ri.FindControl("rfvText"), RequiredFieldValidator).Enabled = False
            End If
            Dim txt As TextBox = ri.FindControl("txtTextField")
            ' txt.Attributes.Add("class", "offset-listen-textbox")

            Select Case drv("value_type")
                Case "Date"

                    txt.Attributes.Add("class", "hasdatePicker")
                    txt.Attributes.Add("placeholder", "MM/DD/YYYY")

                    ClientScript.RegisterStartupScript(Me.GetType(), "set_date_" & txt.ClientID, "$('#" & txt.ClientID & "').datepicker({dateFormat: 'mm/dd/yy'});", True)


            End Select

        End If
    End Sub

End Class

