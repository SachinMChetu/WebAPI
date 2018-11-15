Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net
Imports System.Net.Mail

Partial Class listen
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Response.Redirect("listen_train2.aspx")

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
        Else
            data_rate = 0.05
        End If

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        Session("appname") = domain(0)


        Dim app_dt As DataTable = GetTable("select * from app_settings where appname = '" & Session("appname") & "'")

        Dim cali_count As Integer
        If IsDBNull(app_dt.Rows(0).Item("calibration_count")) Then
            cali_count = 0
        Else
            cali_count = app_dt.Rows(0).Item("calibration_count")
        End If

        'Get user last 5 
        Dim avg_dt As DataTable = GetTable("select isnull(avg(total_score),0), count(*) as number_scores from (select top " & cali_count & " total_score from form_score_training where reviewer = '" & User.Identity.Name & "' and appname = '" & Session("appname") & "' order by id desc) a")

        If avg_dt.Rows.Count > 0 Then
            lblAvgScore.Text = avg_dt.Rows(0).Item(0)

            If avg_dt.Rows(0).Item(0) >= 90 And avg_dt.Rows(0).Item(1) = cali_count Then


                'Email.To.Add(New MailAddress("stace@datadrivendevelopment.com"))
                Dim Body As String = User.Identity.Name & " has passed in " & Session("appname") & " with an avg score of " & avg_dt.Rows(0).Item(0) & Chr(13) & Chr(13)

                Dim scores_dt As DataTable = GetTable("select * from form_score_training where reviewer = '" & User.Identity.Name & "' and appname = '" & Session("appname") & "' order by id")

                For Each score_dr As DataRow In scores_dt.Rows
                    Body &= "http://app.callcriteria.com/view_training.aspx?ID=" & score_dr.Item("ID") & " - score:" & score_dr.Item("total_score") & Chr(13)
                Next



                Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
                cn.Open()


                'If debug Then
                Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',  @copy_recipients=@CC,  @recipients='stace@callcriteria.com;carlo@callcriteria.com;tracy@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
                reply2.Parameters.AddWithValue("Subject_text", "Trainee Passed " & Session("appname"))
                reply2.Parameters.AddWithValue("Body", Body.ToString)

                reply2.CommandTimeout = 60
                reply2.ExecuteNonQuery()
                'End If

                cn.Close()
                cn.Dispose()

                'Email_Error(User.Identity.Name & " has passed in " & Session("appname") & " with an avg score of " & avg_dt.Rows(0).Item(0), "tracy@callcriteria.com")
                Response.Redirect("default.aspx")
            End If

            lblNumberScores.Text = avg_dt.Rows(0).Item(1)
        End If

        'If domain(0) <> "work" Then
        '    Response.Redirect("http://work.pointqa.com/listen.aspx")
        'End If

        'If Session("appname").ToString.ToLower = "work" Then
        '    Dim my_dt As DataTable = GetTable("declare @appnames varchar(1000);select @appnames = COALESCE(@appnames + ''',''', '') + appname  from userapps where username = '" & User.Identity.Name & "';select '''' + @appnames + ''''")
        '    app_list = my_dt.Rows(0).Item(0).ToString
        '    If app_list = "" Then
        '        Response.Redirect("Default.aspx")
        '    End If
        'Else
        '    app_list = "'" & Session("appname") & "'"
        'End If


        If User.Identity.Name = "" Then
            Response.Redirect("login.aspx?ReturnURL=listen.aspx")
        End If

        'Disable button on click, still allow server processing
        btnSaveSession.Attributes.Add("onclientclick", "if(Page_ClientValidate('ValidationGroup')){this.disabled = true;" + ClientScript.GetPostBackEventReference(btnSaveSession, Nothing) + ";}")

        If Not IsPostBack Then
            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "listen collapsed-menu")

            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

            litUserName.Text = User.Identity.Name
            litUserEmail.Text = mu.Email

            'Dim dt2 As DataTable = GetTable("select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock)  join app_settings on app_settings.appname = b.appname  where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-60, dbo.getMTDate())) or (review_started is null)) and b.appname  in (" & app_list & ")  and audio_link is not null and down_time < dateadd(minute,-30,dbo.getMTDate())  order by priority desc, MAX_REVIEWS, call_date, review_started")
            Dim dt2 As DataTable = GetTable("select top 1 calibration_form.id as calib_id,* from vwForm join app_settings on app_settings.appname = vwForm.appname  join calibration_form on calibration_form.original_form = f_ID " &
                "where  vwForm.appname in (select appname from userapps where username =  '" & User.Identity.Name & "') and calibration_form.id not in (select calib_id from form_score_training where reviewer = '" & User.Identity.Name & "') " &
                "and vwForm.reviewer <> '" & User.Identity.Name & "' order by selected desc, calibration_form.id desc")


            hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString
            hdnCalibID.Value = dt2.Rows(0).Item("calib_id").ToString

            If dt2.Rows.Count > 0 Then
                'Personal
                litPersonal.Text = litPersonal.Text & getSidebarData("First Name", dt2.Rows(0).Item("First_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Last Name", dt2.Rows(0).Item("Last_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Email", dt2.Rows(0).Item("Email").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Education Level", dt2.Rows(0).Item("EducationLevel").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("High School Grad Year", dt2.Rows(0).Item("HighSchoolGradYear").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Degree Start Timeframe", dt2.Rows(0).Item("DegreeStartTimeframe").ToString())

                'Contact
                litContact.Text = litContact.Text & getSidebarData("Address", dt2.Rows(0).Item("address").ToString())
                litContact.Text = litContact.Text & getSidebarData("City", dt2.Rows(0).Item("City").ToString())
                litContact.Text = litContact.Text & getSidebarData("State", dt2.Rows(0).Item("State").ToString())
                litContact.Text = litContact.Text & getSidebarData("Zip", dt2.Rows(0).Item("Zip").ToString())
                litContact.Text = litContact.Text & getSidebarData("Phone", dt2.Rows(0).Item("Phone").ToString())

                If dt2.Rows(0).Item("vertical").ToString = "Education" Then ' Only show schools for those with the right vertical
                    'School



                    Dim sch_dt As DataTable = GetTable("select * from School_X_Data where xcc_id = " & dt2.Rows(0).Item("X_ID").ToString)
                    Dim sch_x As Integer = 1
                    For Each sch_dr In sch_dt.Rows
                        litSchool.Text = litSchool.Text & getSidebarData("College " & sch_x, sch_dr.Item("College").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Degree of Interest " & sch_x, sch_dr.Item("DegreeOfInterest").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("School " & sch_x, sch_dr.Item("School").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Area of Interest 1 " & sch_x, sch_dr.Item("AOI1").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Area of Interest 2 " & sch_x, sch_dr.Item("AOI2").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Subject 1 " & sch_x, sch_dr.Item("L1_SubjectName").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Subject 2 " & sch_x, sch_dr.Item("L2_SubjectName").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Modality " & sch_x, sch_dr.Item("Modality").ToString())
                        litSchool.Text = litSchool.Text & getSidebarData("Affiliate " & sch_x, sch_dr.Item("origin").ToString())
                        sch_x += 1
                    Next



                Else
                    liSchoolItem.Visible = False


                End If

                'Other
                litOther.Text = litOther.Text & getSidebarData("CAMPAIGN", dt2.Rows(0).Item("CAMPAIGN").ToString())
                litOther.Text = litOther.Text & getSidebarData("DATE", dt2.Rows(0).Item("call_date").ToString())

                Dim history_dt As DataTable = GetTable("select * from session_viewed where session_id = '" & dt2.Rows(0).Item("ID").ToString() & "'")
                If history_dt.Rows.Count > 0 Then
                    litOther.Text = litOther.Text & "<center>Session Viewed</center><br>"
                    For Each dr As DataRow In history_dt.Rows()
                        litOther.Text = litOther.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("agent") & "</span><strong>" & dr.Item("date_viewed") & "</strong></li>"
                    Next
                End If
                hdnXCCID.Value = dt2.Rows(0).Item("ID").ToString
                hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString
                lblThisApp.Text = dt2.Rows(0).Item("appname").ToString
                'lblArea1.Text = dt2.Rows(0).Item("program").ToString
                'lblArea2.Text = dt2.Rows(0).Item("ID").ToString


                Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
                lblSession.Text = session_id.ToString

                five9update.record_ID = dt2.Rows(0).Item("X_ID").ToString

                Dim this_filename As String = GetAudioFileName(dt2.Rows(0))

                audio_file = this_filename

                'Response.Write("good")
                'Response.End()

                'UpdateTable("insert into session_viewed (agent, date_viewed, session_id) select '" & User.Identity.Name & "',dbo.getMTDate(), " & dt2.Rows(0).Item("ID").ToString)


            End If


        End If


    End Sub

    Protected Sub btnSaveSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSession.Click
        Dim total_score As Integer = 0



        Dim fail_all As Boolean = False
        Dim fail_list As New ArrayList

        Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score_training] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Text & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & txtComments.Text.Replace("'", "''") & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"

        Dim dt_new_ID_dt As DataTable = GetTable(sql)

        Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString

        For Each ri As Object In Request.Form
            'Response.Write(ri.ToString & "<br>")
            If ri.ToString.IndexOf("hdnQID") > 0 Then
                'Check for RBL version of that QID
                Dim QID As String = Request(ri.ToString).ToString
                Dim pos As String = ""
                Dim QAns As String = ""
                Dim theCount As Integer = Request(ri.ToString.Replace("hdnQID", "hdnCount")).ToString()


                Select Case theCount
                    Case 2
                        pos = Request(ri.ToString.Replace("hdnQID", "hdnQTimestamp3")).ToString()
                        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered) values('" & CInt(pos) & "'," & Request(ri.ToString) & "," & new_ID & ",0,-1)")

                    Case 3
                        pos = Request(ri.ToString.Replace("hdnQID", "hdnQTimestamp")).ToString()
                        QAns = Request(ri.ToString.Replace("hdnQID", "hdnQAnswer")).ToString
                        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered) values('" & pos & "'," & Request(ri.ToString) & "," & new_ID & ",0,(select id from question_answers where question_id = '" & Request(ri.ToString) & "' and answer_text = '" & QAns & "'))")

                    Case Is > 3
                        pos = Request(ri.ToString.Replace("hdnQID", "hdnQTimestamp2")).ToString()
                        QAns = Request(ri.ToString.Replace("hdnQID", "ddlQList")).ToString
                        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered) values('" & pos & "'," & Request(ri.ToString) & "," & new_ID & ",0,'" & QAns & "')")

                End Select


            End If

        Next

        'Dim all_fails As String = Join(fail_list.ToArray, ",")
        'UpdateTable("update [form_score3] set autofail = '" & all_fails & "' where id = " & new_ID)

        'UpdateTable("update [XCC_REPORT_NEW] set MAX_REVIEWS = case when MAX_REVIEWS is null then  1 else MAX_REVIEWS + 1 end where ID = '" & hdnXCCID.Value & "'")

        'UpdateTable("update session_viewed set  date_completed = dbo.getMTDate() where agent = '" & User.Identity.Name & "' and date_viewed > dateadd(minute, -30, dbo.getMTDate()) and session_id = '" & hdnXCCID.Value & "'")

        'sql = "insert into dbo.XCC_RESULTS (XCC_ID, total_score, agent_id, review_flag,review_comment, review_date, AutoFail, appname)"
        'sql &= " VALUES ("
        'sql &= "'" & hdnXCCID.Value & "',"
        'sql &= "'" & total_score & "',"
        'sql &= "'" & User.Identity.Name & "',"
        'sql &= "'" & IIf(total_score > 100, 0, 1) & "',"
        'sql &= "'',dbo.getMTDate()," '" & txtComments.Text.Replace("'", "''") & " don't add comments any more
        'sql &= "'" & all_fails & "',"
        'sql &= "'" & hdnThisApp.Value & "')"

        'UpdateTable(sql)
        UpdateTable("update form_score_training set calib_id = " & hdnCalibID.Value & " where id = " & new_ID)
        UpdateTable("updateCalibrationTraining " & new_ID) ' get effective calibration score
        UpdateTable("UpdateMissedTraining " & new_ID)
        'UpdateTable("CreateNotifications " & new_ID & ",'" & hdnThisApp.Value & "'")


        'UpdateCallLengths()

        'Try
        'UploadMP3(new_ID)
        'Catch ex As Exception
        '    Email_Error(new_ID & " - " & ex.Message)
        'End Try


        If chkStopWorking.Checked Then
            Response.Redirect("default.aspx")
        Else
            Response.Redirect("view_training.aspx?ID=" & new_ID)
            'Response.Redirect("http://work.pointqa.com/listen.aspx")
        End If



    End Sub


    Public data_rate As String
    Public app_list As String


    Protected Sub FixList2(sender As Object, e As System.EventArgs)
        Dim ddl As DropDownList = sender
        CType(ddl.Parent.Parent.FindControl("hdnCount"), HiddenField).Value = ddl.Items.Count
        Select Case ddl.Items.Count

            Case 3 'Yes/No Only
                CType(ddl.Parent.Parent.FindControl("pnlSelect"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("pnlCheck"), Panel).Visible = False
            Case 2 'Check Timestamp
                CType(ddl.Parent.Parent.FindControl("pnlYesNo"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("pnlSelect"), Panel).Visible = False
            Case Is > 3 ' Drop down multiple
                CType(ddl.Parent.Parent.FindControl("pnlYesNo"), Panel).Visible = False
                CType(ddl.Parent.Parent.FindControl("pnlCheck"), Panel).Visible = False
        End Select
    End Sub
    Protected Sub FixList(sender As Object, e As RepeaterItemEventArgs) ' Handles gvQuestions.RowDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim rbl As RadioButtonList = e.Item.FindControl("RadioButtonList1")
            Dim hdn As HiddenField = e.Item.FindControl("hdnQTimestamp")
            Dim hdnQAnswer As HiddenField = e.Item.FindControl("hdnQAnswer")
            'rbl.Attributes.Add("onclick", "check_enable('" & hdn.ClientID & "'); getTimeStamp('" & hdn.ClientID & "');")

            Dim drv As DataRowView = e.Item.DataItem

            Dim html_item As HtmlGenericControl = e.Item.FindControl("q_trigger")
            html_item.Attributes.Add("class", drv.Item("starting_class"))
            If drv.Item("starting_class") = "switch answer-no" Then
                hdnQAnswer.Value = "No"
            End If


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

End Class

