Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net

Partial Class listen3
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Public myDelay As String = ""

    Public isWebsite As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo with (nolock)  where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
            myDelay = user_info_dt.Rows(0).Item("guideline_display").ToString
        Else
            data_rate = 0.05
        End If



        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=calibrate_listen.aspx")
        End If



        'Disable button on click, still allow server processing
        btnSaveSession.Attributes.Add("onclientclick", "if(Page_ClientValidate()){this.disabled = true;" + ClientScript.GetPostBackEventReference(btnSaveSession, Nothing) + ";}")

        If Not IsPostBack Then


            UpdateTable("exec dedupeClientCali")

            'Dim dtDateTime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)

            'dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime()
            'Return dtDateTime;


            Dim dto As DateTimeOffset = New DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)

            Dim epoch As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Dim span As TimeSpan = (Now - epoch)


            hdnStartTime.Value = CInt(span.TotalSeconds).ToString

            'btnSaveSession.Attributes.Add("onclick", "$(this).toggle();")

            'release those that have been started, but not finished within 45 mins
            UpdateTable("update calibration_pending set date_started = null where date_started < dateadd(s, -45*60, dbo.getMTDate()) and date_completed is null")



            ''add lockout for calibrators for notifications
            'Dim noti_dt As DataTable = GetTable("exec getCoachingQueue '" & User.Identity.Name & "',''")
            'If noti_dt.Rows.Count > 0 And Not (User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "no_work", "alert('You have pending notifications - clear those first.');", True)
            '    Exit Sub 'Response.Redirect("default.aspx")
            'End If



            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            '
            Dim dt_next As DataTable
            dt_next = GetTable("exec getNextCalibration '" & User.Identity.Name & "'")

            'If Request("scorecard") <> "" Then
            '    dt_next = GetTable("select top 1 *, case when review_type in ('Admin Selected','Client Selected') then 1 else 0 end as cali_priority from calibration_pending left join calibration_form on calibration_form.original_form = calibration_pending.form_id join vwForm on vwform.f_id = form_id  where calibration_form.original_form is null and calibration_pending.reviewer <> '" & User.Identity.Name & "' and scorecard = '" & Request("scorecard") & "' and date_completed is null  and (( date_started is null) or (date_started < dateadd(minute, -45, dbo.getMTDate()))) order by cali_priority desc, calibration_pending.date_added desc") ' and week_ending = '" & week_ending & "'
            'End If


            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                dt_next = GetTable("select top 1 *, isnull(user_priority,10) as myPriority from cali_pending_client with (nolock)  join vwForm with (nolock)  on vwForm.f_id = form_id left join userapps with (nolock)  on userapps.username = assigned_to where assigned_to = '" & User.Identity.Name & "'  and user_scorecard = scorecard and date_completed is null order by myPriority")
            End If

            'hdnStartTime.Value = Now.ToString

            If dt_next.Rows.Count = 0 Then
                rptSections.Visible = False
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "no_work", "alert('No pending calibrations');", True)
                Exit Sub 'Response.Redirect("default.aspx")
            End If

            'Dim left_dt As DataTable = GetTable("select count(*) from calibration_pending where appname = '" & Request("appname") & "' and date_completed is null ")   'and week_ending = '" & week_ending & "'

            'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
            '    left_dt = GetTable("select count(*) from cali_pending_client where assigned_to = '" & User.Identity.Name & "' and date_completed is null")
            'End If

            ' litLeft.Text = left_dt.Rows(0).Item(0) & " left"


            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                UpdateTable("update cali_pending_client set who_processed = '" & User.Identity.Name & "', date_started = dbo.getMTDate() where id = " & dt_next.Rows(0).Item("id"))
                ' dsSections.SelectCommand = "select * from sections where id in (select section from questions where id in (select distinct question_id from form_q_scores where form_id = (select form_id from cali_pending_client  where id = @form_id))) order by section_order"
            Else
                UpdateTable("update calibration_pending set who_processed = '" & User.Identity.Name & "', date_started = dbo.getMTDate() where id = " & dt_next.Rows(0).Item("id"))
            End If



            rptSections.Visible = True
            Dim dt2 As DataTable = GetTable("select * from vwForm with (nolock) join app_settings  with (nolock) on app_settings.appname = vwForm.appname where vwForm.F_id = " & dt_next.Rows(0).Item("form_id"))




            hdnThisID.Value = dt_next.Rows(0).Item("id")

            If dt2.Rows.Count > 0 Then
                'UpdateTable("update XCC_REPORT_NEW set review_started = dbo.getMTDate() where ID = " & dt2.Rows(0).Item("ID").ToString)
            Else
                Response.Write(hdnThisID.Value)
                Response.End()

                Response.Redirect("default.aspx")
            End If






            Dim record_dt As DataTable = GetTable("select * from calibration_pending with (nolock)  join vwForm with (nolock)  on vwForm.f_id = form_id where calibration_pending.id = " & hdnThisID.Value)
            If record_dt.Rows.Count > 0 Then
                hdnCallLength.Value = record_dt.Rows(0).Item("call_length").ToString
                hdnThisAgent.Value = record_dt.Rows(0).Item("Agent").ToString
                hdnisrecal.Value = record_dt.Rows(0).Item("isrecal").ToString
                hdnThisFormID.Value = record_dt.Rows(0).Item("form_id").ToString


            End If
            hdnCampaign.Value = dt2.Rows(0).Item("campaign").ToString
            hdnThisScorecard.Value = dt2.Rows(0).Item("scorecard").ToString
            hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString


            Dim od_dt As DataTable = GetTable("select * from otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("X_ID").ToString & " and data_type <> 'School'")
            If od_dt.Rows.Count > 0 Then
                divOtherCard.Visible = True
                For Each dr As DataRow In od_dt.Rows
                    litOtherData.Text = litOtherData.Text & getSidebarData(dr("data_key").ToString(), dr("data_value").ToString())
                Next
            End If


            If dt2.Rows(0).Item("website").ToString <> "" Then
                isWebsite = True
            End If

            If dt2.Rows.Count > 0 Then
                'Personal
                litPersonal.Text = litPersonal.Text & getSidebarData("First Name", dt2.Rows(0).Item("First_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Last Name", dt2.Rows(0).Item("Last_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Email", dt2.Rows(0).Item("Email").ToString())

                litPersonal.Text = litPersonal.Text & getSidebarData("Website", "<a href='" & dt2.Rows(0).Item("Website").ToString() & "'>" & dt2.Rows(0).Item("Website").ToString() & "</a>")
                litPersonal.Text = litPersonal.Text & getSidebarData("Education Level", dt2.Rows(0).Item("EducationLevel").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("High School Grad Year", dt2.Rows(0).Item("HighSchoolGradYear").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Degree Start Timeframe", dt2.Rows(0).Item("DegreeStartTimeframe").ToString())

                'Contact
                litPersonal.Text = litPersonal.Text & getSidebarData("Address", dt2.Rows(0).Item("address").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("City", dt2.Rows(0).Item("City").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("State", dt2.Rows(0).Item("State").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Zip", dt2.Rows(0).Item("Zip").ToString())

                'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                '    litPersonal.Text = litPersonal.Text & getSidebarData("Phone", dt2.Rows(0).Item("Phone").ToString())
                'End If

                'litPersonal.Text = litPersonal.Text & getSidebarData("Session", dt2.Rows(0).Item("Session_id").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Campaign", dt2.Rows(0).Item("Campaign").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Call Date", CDate(dt2.Rows(0).Item("call_date")).ToShortDateString)
                litPersonal.Text = litPersonal.Text & getSidebarData("Call Type", dt2.Rows(0).Item("call_type").ToString())

                litPersonal.Text = litPersonal.Text & getSidebarData("Notes", dt2.Rows(0).Item("disposition").ToString())


                'If dt2.Rows(0).Item("text_only").ToString() <> "" Then
                '    litTextOnly.Text = dt2.Rows(0).Item("text_only").ToString()
                '    divHiddenText.Visible = True
                'End If


                Dim sch_dt As DataTable = GetTable("select distinct School,AOI1,AOI2,L1_SubjectName,L2_SubjectName,Modality,College,DegreeOfInterest,origin,tcpa from School_X_Data with (nolock)  where xcc_id = " & dt2.Rows(0).Item("X_ID").ToString)
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

                    Dim other_dt As DataTable = GetTable("select * from otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("x_ID").ToString & " and school_name = '" & Replace(sch_dr.Item("School").ToString(), "'", "''") & "'")
                    For Each other_dr In other_dt.Rows
                        litSchool.Text = litSchool.Text & getSidebarData(other_dr.item("data_key").ToString(), other_dr.item("data_value").ToString())
                    Next

                    sch_x += 1
                Next


            Else
                ' liSchoolItem.Visible = False
            End If


            If dt2.Rows(0).Item("website").ToString() <> "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "show_website", "window.open('" & dt2.Rows(0).Item("website") & "','myWindow', 'width=800, height=600');", True)
                litPersonal.Text = litPersonal.Text & getSidebarData("Agent", dt2.Rows(0).Item("Agent").ToString())
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
            hdnXCCID.Value = dt2.Rows(0).Item("X_ID").ToString
            hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString


            'lblThisApp.Text = dt2.Rows(0).Item("appname").ToString

            If dt2.Rows(0).Item("client_logo").ToString() = "" Then
                lblThisApp.Text = dt2.Rows(0).Item("FullName").ToString()
            Else
                lblThisApp.Text = "<img src='" & dt2.Rows(0).Item("client_logo").ToString() & "' />"
            End If


            'lblArea1.Text = dt2.Rows(0).Item("program").ToString
            'lblArea2.Text = dt2.Rows(0).Item("ID").ToString


            Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
            ' lblSession.Text = session_id.ToString

            'five9update.record_ID = dt2.Rows(0).Item("X_ID").ToString

            If dt2.Rows(0).Item("stream_only").ToString = "True" Then
                audio_file = dt2.Rows(0).Item("audio_link").ToString
            Else
                audio_file = GetAudioFileName(dt2.Rows(0))
            End If


            'Response.Write(audio_file)
            'Response.End()

            'Dim this_filename As String = GetAudioFileName(dt2.Rows(0))


            'UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" & User.Identity.Name & "',dbo.getMTDate(), " & dt2.Rows(0).Item("ID").ToString & ",'listen'")


        End If


        'End If


    End Sub

    Protected Sub btnSaveSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSession.Click
        Dim total_score As Integer = 0



        Dim fail_all As Boolean = False
        Dim fail_list As New ArrayList


        Dim form_test As DataTable = GetTable("select count(*) from calibration_form with (nolock)  where original_form in (select form_id from calibration_pending with (nolock)  where id = '" & hdnThisID.Value & "') and ((isrecal = 0) or (isrecal is null)) ")
        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
            form_test = GetTable("select count(*) from calibration_form_client where original_form in (select form_id from cali_pending_client with (nolock)  where id = '" & hdnThisID.Value & "') and reviewed_by = '" & User.Identity.Name & "'")
        End If


        If form_test.Rows.Count > 0 And (hdnisrecal.Value = "0" Or hdnisrecal.Value = "") Then
            If form_test.Rows(0).Item(0) > 0 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "nokey", "<script language=javascript>alert('Form already submitted');</script>")
                Exit Sub
            End If
        End If




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
        'listen_dr("session_id") = hdnThisID.Value
        listen_dr("review_ID") = hdnThisID.Value
        listen_dr("Comments") = Trim(txtComments.Text)
        'listen_dr("appname") = hdnThisApp.Value
        Try
            listen_dr("whisperID") = hdnWhisper.Value
            listen_dr("QAwhisper") = ddlWhisper.SelectedValue
        Catch ex As Exception

        End Try

        Try
            listen_dr("qa_start") = hdnStartTime.Value
        Catch ex As Exception

        End Try


        If hdnisrecal.Value <> "" Then
            listen_dr("copy_to_cali") = hdnisrecal.Value
        Else
            listen_dr("copy_to_cali") = 0
        End If

        listen_dt.Rows.Add(listen_dr)

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

                            'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered,  click_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "')")

                            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                ' UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos) select " & hdnQID.Value & "," & new_ID & "," & hdnAnswerID.Value & ",'" & total_sec & "'")


                                Dim FQS_dr As DataRow = FQS_dt.NewRow
                                FQS_dr("q_position") = total_sec
                                FQS_dr("question_id") = hdnQID.Value
                                FQS_dr("question_result") = 0
                                FQS_dr("question_answered") = hdnAnswerID.Value
                                FQS_dr("click_text") = txtClickTime.Text


                                FQS_dt.Rows.Add(FQS_dr)


                            Else
                                Dim txtWebsiteLink As TextBox = aRI.FindControl("txtWebsiteLink")
                                Dim chkWebsite As CheckBox = aRI.FindControl("chkWebsite")

                                'If chkWebsite.Checked Then
                                '    UpdateTable("insert into calibration_scores (q_pos, question_id, form_id, question_result, click_text,view_link ) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & "," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "','" & txtWebsiteLink.Text & "' );")
                                'Else
                                '    UpdateTable("insert into calibration_scores (q_pos, question_id, form_id, question_result, click_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & "," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "');")
                                'End If



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



                            End If






                            Dim chkOptions As CheckBoxList = aRI.FindControl("chkOptions")


                            Dim answer_selected As Boolean = False


                            Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                            Dim chkComment As CheckBox = aRI.FindControl("chkComment")
                            If chkComment.Checked Then
                                answer_selected = True

                                'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                '    UpdateTable("insert into form_c_responses_client (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherComment.Text.Replace("'", "''") & "')")
                                'Else
                                '    UpdateTable("insert into form_c_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherComment.Text.Replace("'", "''") & "')")
                                'End If

                                Dim FQR_dr As DataRow = FQR_dt.NewRow
                                FQR_dr("question_id") = hdnQID.Value
                                FQR_dr("answer_id") = 0
                                FQR_dr("other_answer_text") = txtOtherComment.Text
                                FQR_dt.Rows.Add(FQR_dr)

                            End If



                            For Each cblItem As ListItem In chkOptions.Items
                                If cblItem.Selected Then

                                    answer_selected = True

                                    'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                    '    UpdateTable("insert into form_c_responses_client (question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & ",'" & cblItem.Value & "')")
                                    'Else
                                    '    UpdateTable("insert into form_c_responses (question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & ",'" & cblItem.Value & "')")
                                    'End If

                                    Dim FQR_dr As DataRow = FQR_dt.NewRow
                                    FQR_dr("question_id") = hdnQID.Value
                                    FQR_dr("answer_id") = cblItem.Value
                                    'FQR_dr("other_answer_text") = 
                                    FQR_dt.Rows.Add(FQR_dr)


                                End If
                            Next


                            If Not answer_selected Then
                                Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments with (nolock)  where question_id = " & hdnQID.Value & " and answer_id = " & hdnAnswerID.Value)
                                If default_ans.Rows.Count > 0 Then

                                    'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                    '    UpdateTable("insert into form_c_responses_client ( question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & "," & default_ans.Rows(0).Item("ID") & ")")
                                    'Else
                                    '    UpdateTable("insert into form_c_responses ( question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & "," & default_ans.Rows(0).Item("ID") & ")")
                                    'End If

                                    Dim FQR_dr As DataRow = FQR_dt.NewRow
                                    FQR_dr("question_id") = hdnQID.Value
                                    FQR_dr("answer_id") = default_ans.Rows(0).Item("ID")
                                    FQR_dt.Rows.Add(FQR_dr)


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


                            'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                            '    UpdateTable("insert into form_c_responses_client (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")
                            'Else
                            '    UpdateTable("insert into form_c_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")

                            'End If

                            Dim FQR_dr As DataRow = FQR_dt.NewRow
                            FQR_dr("question_id") = hdnQID.Value
                            FQR_dr("answer_id") = 0
                            FQR_dr("other_answer_text") = txtOtherList.Text
                            FQR_dt.Rows.Add(FQR_dr)

                            'Response.Write("insert into form_c_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")
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

                                    'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                    '    UpdateTable("insert into form_c_scores_options_client (option_pos, option_value, question_id, form_id, orig_id) values('" & total_sec & "','" & chkOption.Text.Replace("'", "''") & "'," & hdnQID.Value & "," & new_ID & "," & hdnOrigId.Value & ")")
                                    'Else
                                    '    UpdateTable("insert into form_c_scores_options (option_pos, option_value, question_id, form_id, orig_id) values('" & total_sec & "','" & chkOption.Text.Replace("'", "''") & "'," & hdnQID.Value & "," & new_ID & "," & hdnOrigId.Value & ")")
                                    'End If

                                    Dim FQSO_dr As DataRow = FQSO_dt.NewRow
                                    FQSO_dr("option_pos") = total_sec
                                    FQSO_dr("option_value") = chkOption.Text
                                    FQSO_dr("question_id") = hdnQID.Value
                                    FQSO_dr("orig_id") = hdnOrigId.Value
                                    FQSO_dt.Rows.Add(FQSO_dr)


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




                        Dim txtWebsiteLink As TextBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("txtWebsiteLink")
                        Dim chkWebsite As CheckBox = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("chkWebsite")


                        Dim add_link As String = ""
                        Dim add_link_column As String = ""

                        If chkWebsite.Checked Then
                            add_link = ",'" & txtWebsiteLink.Text & "' "
                            add_link_column = ",view_link "
                        End If






                        If min_check = 10000 Then min_check = 0

                        If na_checked Then

                            'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                            '    UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos) select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'NA'),'" & min_check & "'")
                            'Else
                            '    UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos) select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'NA'),'" & min_check & "'")
                            'End If

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

                        Else

                            If all_checked Then 'this has the time, was the answer, write to DB
                                'ClickTime from MM:SS to seconds
                                'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                '    UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos " & add_link_column & ") select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & min_check & "'" & add_link)
                                'Else
                                '    UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos" & add_link_column & ") select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & min_check & "'" & add_link)
                                'End If

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

                                'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & ContactInfo.Text & "')")
                            Else

                                'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
                                '    UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos" & add_link_column & ") select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & min_check & "'" & add_link)
                                'Else
                                '    UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos" & add_link_column & ") select " & hdnQID.Value & "," & new_ID & ",(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & min_check & "'" & add_link)
                                'End If

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

                                'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & ContactInfo.Text & "')")
                            End If

                        End If


                    End If
                    ' Next
                End If


            Next

        Next




        Using command = New SqlCommand("calibDataInsert")
            command.CommandType = CommandType.StoredProcedure
            'create your own data table
            command.Parameters.Add(New SqlParameter("@ListenInsert", listen_dt))
            command.Parameters.Add(New SqlParameter("@FQSInsert", FQS_dt))
            command.Parameters.Add(New SqlParameter("@FQRInsert", FQR_dt))
            command.Parameters.Add(New SqlParameter("@FQSOInsert", FQSO_dt))
            'command.Parameters.Add(New SqlParameter("@KeywordInsert", KW_dt))
            RunSqlCommand(command)
        End Using



        'If hdnisrecal.Value = "1" Then
        'UpdateTable("update calibration_form set active_cali = 0 where original_form = (select form_id from calibration_pending with (nolock)  where id = '" & hdnThisID.Value & "')")
        'UpdateTable("update calibration_form set active_cali = 1 where id = " & new_ID)
        'End If

        'For Each ri As RepeaterItem In rptSections.Items
        '    For Each ri2 As RepeaterItem In CType(ri.FindControl("Repeater2"), Repeater).Items

        '        Dim QID As String = CType(ri2.FindControl("hdnQID"), HiddenField).Value
        '        Dim pos As String = CType(ri2.FindControl("hdnUpdateTime"), HiddenField).Value
        '        Dim QAns As String = CType(ri2.FindControl("ddlAnswers"), DropDownList).SelectedValue

        '        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
        '            UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos) select " & QID & "," & new_ID & "," & QAns & ",'" & pos & "'")
        '        Else
        '            UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos) select " & QID & "," & new_ID & "," & QAns & ",'" & pos & "'")
        '        End If

        '    Next
        'Next



        'If User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Then
        '    UpdateTable("update cali_pending_client set date_completed = dbo.getMTDate() where id = '" & hdnThisID.Value & "'")
        '    UpdateTable("update calibration_form_client set cali_dev = dbo.GetCaliDeviationClient(" & new_ID & ") where id = " & new_ID)
        '    UpdateTable("postProcessQuestionClientCalib '" & new_ID & "'")
        '    UpdateTable("updateCalibrationClient '" & new_ID & "'")
        'Else
        '    UpdateTable("update calibration_pending set date_completed = dbo.getMTDate() where id = '" & hdnThisID.Value & "'")
        '    UpdateTable("update calibration_form set cali_dev = dbo.GetCaliDeviation(" & new_ID & ") where id = " & new_ID)
        '    UpdateTable("exec postProcessQuestionsCalib '" & new_ID & "'")
        '    UpdateTable("updateCalibration '" & new_ID & "'")
        'End If

        ' Response.End()



        If chkStopWorking.Checked Then
            Response.Redirect("default.aspx")
        Else
            If Request("week_ending") IsNot Nothing Then
                Response.Redirect("calibrate_listen.aspx?week_ending=" & Request("week_ending") & "&scorecard=" & Request("scorecard"))
            Else
                Response.Redirect("calibrate_listen.aspx?scorecard=" & Request("scorecard"))
            End If

        End If



    End Sub



    Protected Sub btnSaveSession_Click2(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSaveSession.Click


        Dim total_score As Integer = 0

        Dim existing_dt As DataTable = GetTable("select count(*) from vwForm with (nolock)  where review_id = " & hdnXCCID.Value & " and max_reviews = 1")

        If existing_dt.Rows(0).Item(0) = 0 Then 'if already reviewed don't create a new record

            Dim fail_all As Boolean = False
            Dim fail_list As New ArrayList

            Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score3] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Value & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & Trim(txtComments.Text.Replace("'", "''")) & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"
            ''Response.Write(sql)
            ''Response.End()

            Dim dt_new_ID_dt As DataTable = GetTable(sql)

            Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString


            If txtComments.Text <> "" Then
                ' UpdateTable("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id) select reviewer, review_date, comments, 'Call', id from form_score3 where id =" & new_ID)

                ' Try
                Dim comment_title() As String = Request.Form.GetValues("comment_title")
                Dim more_comments() As String = Request.Form.GetValues("more_comments")
                Dim comment_time() As String = Request.Form.GetValues("comment_time")

                Dim comment_counter As Integer = 0

                For Each comm_title In more_comments
                    If comm_title <> "" Then
                        UpdateTable("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id, comment_pos, comment_header) select reviewer, review_date,'" & more_comments(comment_counter).Replace("'", "''") & "' , 'Call', id, '" & comment_time(comment_counter).Replace("'", "''") & "','" & comment_title(comment_counter).Replace("'", "''") & "' from form_score3 with (nolock)  where id =" & new_ID)
                        'Response.Write("insert into system_comments(comment_who, comment_date, comment, comment_type, comment_id, comment_pos, comment_header) select reviewer, review_date,'" & more_comments(comment_counter).Replace("'", "''") & "' , 'Call', id, '" & comment_time(comment_counter).Replace("'", "''") & "','" & comment_title(comment_counter).Replace("'", "''") & "' from form_score3 where id =" & new_ID)
                    End If
                    comment_counter += 1
                Next

                'Catch ex As Exception
                '    Email_Error("@ listen sys comment<br><br>" & ex.Message)
                'End Try
            End If



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

                                UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered,  click_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "')")

                                Dim chkOptions As CheckBoxList = aRI.FindControl("chkOptions")


                                Dim answer_selected As Boolean = False

                                For Each cblItem As ListItem In chkOptions.Items
                                    If cblItem.Selected Then

                                        answer_selected = True

                                        If cblItem.Value = "0" Then
                                            Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                                            UpdateTable("insert into form_q_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherComment.Text.Replace("'", "''") & "')")
                                        Else
                                            UpdateTable("insert into form_q_responses (question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & ",'" & cblItem.Value & "')")
                                        End If


                                    End If
                                Next


                                If Not answer_selected Then
                                    Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments with (nolock)  where question_id = " & hdnQID.Value & " and answer_id = " & hdnAnswerID.Value)
                                    If default_ans.Rows.Count > 0 Then
                                        UpdateTable("insert into form_q_responses ( question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & "," & default_ans.Rows(0).Item("ID") & ")")
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

                                    UpdateTable("insert into form_q_scores_options (option_pos, option_value, question_id, form_id, orig_id) values('" & total_sec & "','" & chkOption.Text.Replace("'", "''") & "'," & hdnQID.Value & "," & new_ID & "," & hdnOrigId.Value & ")")
                                Else
                                    all_checked = False
                                End If
                            Next

                            'Dim ContactInfo As HtmlGenericControl = rptContactList.FindControl("CIInfo")

                            Dim ContactInfo As Label = rptContactList.Controls(rptContactList.Controls.Count - 1).Controls(0).FindControl("CIInfo")

                            'If ContactInfo IsNot Nothing Then

                            If min_check = 10000 Then min_check = 0

                            If all_checked Then 'this has the time, was the answer, write to DB
                                'ClickTime from MM:SS to seconds
                                UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & ContactInfo.Text & "')")
                            Else
                                UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers with (nolock)  where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & ContactInfo.Text & "')")
                            End If



                        End If
                        ' Next
                    End If


                Next

            Next

            Dim all_fails As String = Join(fail_list.ToArray, ",")
            UpdateTable("update [form_score3] set autofail = '" & all_fails & "' where id = " & new_ID)

            UpdateTable("update [XCC_REPORT_NEW] set MAX_REVIEWS = case when MAX_REVIEWS is null then  1 else MAX_REVIEWS + 1 end where ID = '" & hdnXCCID.Value & "'")

            UpdateTable("update session_viewed set  date_completed = dbo.getMTDate() where agent = '" & User.Identity.Name & "' and date_viewed > dateadd(minute, -30, dbo.getMTDate()) and session_id = '" & hdnXCCID.Value & "'")

            sql = "insert into dbo.XCC_RESULTS (XCC_ID, total_score, agent_id, review_flag,review_comment, review_date, AutoFail, appname)"
            sql &= " VALUES ("
            sql &= "'" & hdnXCCID.Value & "',"
            sql &= "'" & total_score & "',"
            sql &= "'" & User.Identity.Name & "',"
            sql &= "'" & IIf(total_score > 100, 0, 1) & "',"
            sql &= "'',dbo.getMTDate()," '" & txtComments.Text.Replace("'", "''") & " don't add comments any more
            sql &= "'" & all_fails & "',"
            sql &= "'" & hdnThisApp.Value & "')"

            UpdateTable(sql)

            UpdateTable("UpdateScores " & new_ID)
            UpdateTable("UpdateMissed " & new_ID)
            UpdateTable("updateFormatMissed " & new_ID)
            UpdateTable("CreateNotifications " & new_ID & ",'" & hdnThisApp.Value & "'")


            UpdateCallLengths()


            'Check to see if an email is necessary 
            Dim client_dt As DataTable = GetTable("select * from vwForm  with (nolock) join app_settings with (nolock)  on app_settings.appname = vwForm.appname where f_id = " & new_ID)

            If client_dt.Rows.Count > 0 Then
                If client_dt.Rows(0).Item("pass_fail").ToString = "Fail" And client_dt.Rows(0).Item("email_on_fail").ToString = "1" And client_dt.Rows(0).Item("contact_email").ToString <> "" Then
                    Dim body As String = "<html><body><img src='http://app.callcriteria.com/img/cc_words_logo.png' alt='Call Criteria' width='322' height='50'><br>"
                    body &= "A call has been marked as failed.  Click <a href='http://app.callcriteria.com/review_record.aspx?ID=" & new_ID & ">here</a> to review the call."
                    body &= "</body></html>"


                    Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
                    cn.Open()



                    Try
                        'If debug Then
                        Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',   @recipients=@to,  @subject=@subject,  @body=@Body , @body_format = 'HTML' ;", cn)
                        reply2.Parameters.AddWithValue("subject", client_dt.Rows(0).Item("contact_email").ToString)
                        reply2.Parameters.AddWithValue("to", "Call Failed")
                        reply2.Parameters.AddWithValue("Body", body.ToString)

                        reply2.CommandTimeout = 60
                        reply2.ExecuteNonQuery()
                        'End If
                    Catch ex As Exception

                    End Try



                    cn.Close()
                    cn.Dispose()

                End If
            End If


        End If

        'do this twice since 

        UpdateTable("update userextrainfo set lastActiveDate = dbo.getMTDate() where username = '" & User.Identity.Name & "'")

        If chkStopWorking.Checked Then
            Response.Redirect("default.aspx")
        Else
            Response.Redirect("listen.aspx")
            'Response.Redirect("http://work.pointqa.com/listen3.aspx")
        End If



    End Sub

    Protected Sub UpdateCallLengths()
        Dim dt As DataTable = GetTable("select * from XCC_REPORT_NEW  with (nolock) join form_score3  with (nolock) on form_score3.review_id = xcc_report_new.id where left(audio_link,6) = '/audio' and call_length is null")

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


    End Sub

    Protected Sub CheckForPanel(sender As Object, e As RepeaterItemEventArgs)
        Dim rptitem As RepeaterItem = e.Item
        Dim lit As Literal = rptitem.FindControl("Literal1")
        Dim drv As DataRowView = rptitem.DataItem


    End Sub

    Private Function getValueOrNA(p1 As String) As String

        If [String].IsNullOrEmpty(p1) Then
            Return "NA"
        Else
            Return p1
        End If
    End Function


    Protected Sub btnBadCall_Click(sender As Object, e As EventArgs) 'Handles btnBadCall.Click


        'Dim existing_dt As DataTable = GetTable("select count(*) from form_score3 where review_id = " & hdnXCCID.Value)

        'If existing_dt.Rows(0).Item(0) = 0 Then 'if already reviewed don't create a new record

        '    Dim fail_all As Boolean = False
        '    Dim fail_list As New ArrayList

        '    Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score3] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Text & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & txtComments.Text.Replace("'", "''") & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"

        '    Dim dt_new_ID_dt As DataTable = GetTable(sql)

        '    Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString


        'End If

        'Try
        '    UpdateTable("update xcc_report_new set bad_call = 1, bad_call_who='" & User.Identity.Name & "',bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" & ddlDropReason.SelectedValue & "' where id = " & hdnXCCID.Value)


        '    If chkStopWorking.Checked Then
        '        Response.Redirect("default.aspx")
        '    Else
        '        Response.Redirect("listen.aspx")
        '    End If
        'Catch ex As Exception

        '    Email_Error(ex.Message)
        'End Try




    End Sub



    Protected Function getSidebarData(lbl As String, value As String) As String
        If value <> "" Then
            'Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"

            Return "<tr><td class='info-label'>" & lbl & "</td><td class='info-data'>" & value & "</td></tr>"

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
        rbl.Items.Add(New ListItem("", "0"))
    End Sub

    Protected Sub chkOptions_DataBound(sender As Object, e As EventArgs)
        'Dim chk As CheckBoxList = sender
        'chk.Items.Add(New ListItem("", "0"))
    End Sub
    Protected Sub rptAnswers_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim txtOtherComment As TextBox = e.Item.FindControl("txtOtherComment")
            Dim chkComment As CheckBox = e.Item.FindControl("chkComment")
            txtOtherComment.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkComment.ClientID & "').checked = true;")


            If Not isWebsite Then
                CType(e.Item.FindControl("chkWebsite"), CheckBox).Visible = False
                CType(e.Item.FindControl("txtWebsiteLink"), TextBox).Visible = False

            End If

            'Dim txtWebsiteLink As TextBox = e.Item.FindControl("txtWebsiteLink")
            'Dim chkWebsite As CheckBox = e.Item.FindControl("chkWebsite")
            'txtWebsiteLink.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkWebsite.ClientID & "').checked = true;")
        End If
    End Sub
End Class

