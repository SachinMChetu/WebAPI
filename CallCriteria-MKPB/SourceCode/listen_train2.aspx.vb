Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net
Imports System.Net.Mail

Partial Class listen3
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Response.Redirect("Listen_training.aspx")

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?ReturnURL=Listen_train2.aspx")
        End If

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
        Else
            data_rate = 0.05
        End If

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        Session("appname") = domain(0)


        If Session("appname").ToString.ToLower = "work" Then
            Dim my_dt As DataTable = GetTable("declare @appnames varchar(1000);select @appnames = COALESCE(@appnames + ''',''', '') + appname  from userapps where username = '" & User.Identity.Name & "';select '''' + @appnames + ''''")
            app_list = my_dt.Rows(0).Item(0).ToString
            If app_list = "" Then
                Response.Redirect("Default.aspx")
            End If
        Else
            app_list = "'" & Session("appname") & "'"
        End If


        Dim dt2 As DataTable = GetTable("select top 1 calibration_form.id as calib_id,* from vwForm join calibration_form on calibration_form.original_form = f_ID " &
            " join UserApps on UserApps.user_scorecard = vwForm.scorecard " &
             "where  username =  '" & User.Identity.Name & "' and scorecard_role = 'Trainee' and calibration_form.id not in (select calib_id from form_score_training where reviewer = '" & User.Identity.Name & "') " &
             "and vwForm.reviewer <> '" & User.Identity.Name & "' order by user_priority, selected desc, calibration_form.id desc")

        If dt2.Rows.Count = 0 Then
            Response.Redirect("cd2.aspx")
        End If


        Session("appname") = dt2.Rows(0).Item("appname")

        Dim app_dt As DataTable = GetTable("select * from app_settings where appname = '" & dt2.Rows(0).Item("appname") & "'")

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
                    Body &= "http://app.callcriteria.com/review_training.aspx?ID=" & score_dr.Item("ID") & " - score:" & score_dr.Item("total_score") & Chr(13)
                Next



                Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
                cn.Open()


                'If debug Then
                Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',   @recipients='stace@callcriteria.com;carlo@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
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



        If User.Identity.Name = "" Then
            Response.Redirect("login.aspx?ReturnURL=listen.aspx")
        End If

        'Disable button on click, still allow server processing
        btnSaveSession.Attributes.Add("onclientclick", "if(Page_ClientValidate('ValidationGroup')){this.disabled = true;" + ClientScript.GetPostBackEventReference(btnSaveSession, Nothing) + ";}")

        If Not IsPostBack Then
            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "listen collapsed-menu")

            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

            'Dim dt2 As DataTable

            'If domain(0) = "work" Then
            '    dt2 = GetTable("select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock)  join app_settings on app_settings.appname = b.appname join UserApps on UserApps.appname = b.appname   where ((MAX_REVIEWS < 1) or (MAX_REVIEWS is null)) and ((review_started < dateadd(mi,-60, dbo.getMTDate())) or (review_started is null)) and username = '" & User.Identity.Name & "'  and audio_link is not null and down_time < dateadd(minute,-30,dbo.getMTDate()) and app_settings.active = 1 and session_id is not null  order by user_priority, MAX_REVIEWS, call_date, review_started, campaign")
            'Else
            '    dt2 = GetTable("select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock)  join app_settings on app_settings.appname = b.appname  where b.appname = '" & domain(0) & "' and app_settings.active = 1 and session_id is not null order by priority desc, MAX_REVIEWS, call_date, review_started, campaign")
            'End If




            If dt2.Rows.Count > 0 Then



                'check to use other template
                'If dt2.Rows(0).Item("listen_template").ToString <> "Yes/No New Template" Then
                '    Response.Redirect("listen.aspx")
                'End If

                'UpdateTable("update XCC_REPORT_NEW set review_started = dbo.getMTDate() where ID = " & dt2.Rows(0).Item("ID").ToString)
            Else
                Response.Redirect("default.aspx")
            End If


            hdnCampaign.Value = dt2.Rows(0).Item("campaign").ToString
            'hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString
            hdnCalibID.Value = dt2.Rows(0).Item("calib_id").ToString

            Dim agentdt As DataTable = GetTable("select min(call_date) from xcc_report_new where call_date > dateadd(d,-30,dbo.getMTDate()) and agent = '" & dt2.Rows(0).Item("agent").ToString() & "'")
            If agentdt.Rows.Count > 0 Then
                If Not IsDBNull(agentdt.Rows(0).Item(0)) Then
                    If agentdt.Rows(0).Item(0) > DateAdd(DateInterval.Day, -14, Today) Then
                        'rightHeader.Style.Add("background-color", "LightGreen")
                    End If
                End If
            End If


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
                ' litPersonal.Text = litPersonal.Text & getSidebarData("Session", dt2.Rows(0).Item("Session_id").ToString())



                Dim sch_dt As DataTable = GetTable("select * from School_X_Data where xcc_id = " & dt2.Rows(0).Item("X_ID").ToString)
                Dim sch_x As Integer = 1
                For Each sch_dr In sch_dt.Rows
                    litSchool.Text = litSchool.Text & " <tr><td class='school-name' colspan='2'>" & sch_dr.Item("School").ToString() & "</td></tr>"

                    litSchool.Text = litSchool.Text & getSidebarData("College", sch_dr.Item("College").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Degree", sch_dr.Item("DegreeOfInterest").ToString())

                    'litSchool.Text = litSchool.Text & getSidebarData("School " & sch_x, sch_dr.Item("School").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 1", sch_dr.Item("AOI1").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 2", sch_dr.Item("AOI2").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 1", sch_dr.Item("L1_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 2", sch_dr.Item("L2_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Modality", sch_dr.Item("Modality").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Affiliate", sch_dr.Item("origin").ToString())
                    sch_x += 1
                Next


            Else
                ' liSchoolItem.Visible = False
            End If

            'Other
            'litOther.Text = litOther.Text & getSidebarData("CAMPAIGN", dt2.Rows(0).Item("CAMPAIGN").ToString())
            'litOther.Text = litOther.Text & getSidebarData("DATE", dt2.Rows(0).Item("call_date").ToString())

            Dim history_dt As DataTable = GetTable("select * from session_viewed where session_id = '" & dt2.Rows(0).Item("ID").ToString() & "'")
            If history_dt.Rows.Count > 0 Then
                'litOther.Text = litOther.Text & "<center>Session Viewed</center><br>"
                For Each dr As DataRow In history_dt.Rows()
                    'litOther.Text = litOther.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("agent") & "</span><strong>" & dr.Item("date_viewed") & "</strong></li>"
                Next
            End If
            hdnXCCID.Value = dt2.Rows(0).Item("ID").ToString
            hdnScorecard.Value = dt2.Rows(0).Item("scorecard").ToString
            hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString





            Dim client_dt As DataTable = GetTable("select * From app_settings where appname = '" & dt2.Rows(0).Item("appname").ToString & "'")



            If client_dt.Rows(0).Item("client_logo").ToString() = "" Then
                lblThisApp.Text = "Call from " & client_dt.Rows(0).Item("FullName").ToString()
            Else
                lblThisApp.Text = "<img src='" & client_dt.Rows(0).Item("client_logo").ToString() & "' />"
            End If

            'lblArea1.Text = dt2.Rows(0).Item("program").ToString
            'lblArea2.Text = dt2.Rows(0).Item("ID").ToString


            Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
            ' lblSession.Text = session_id.ToString

            five9update.record_ID = dt2.Rows(0).Item("ID").ToString


            Dim this_filename As String = GetAudioFileName(dt2.Rows(0))



            audio_file = this_filename

            'Response.Write("good")
            'Response.End()

            ' UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" & User.Identity.Name & "',dbo.getMTDate(), " & dt2.Rows(0).Item("ID").ToString & ",'listen'")



        End If


        'End If


    End Sub

    Protected Sub btnSaveSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSession.Click


        Dim total_score As Integer = 0

        Dim sql As String = "declare @new_id int; INSERT INTO [dbo].[form_score_training] (reviewer,session_id,review_date,[review_ID],Comments, appname) values ('" & User.Identity.Name & "','" & lblSession.Value & "',dbo.getMTDate()," & hdnXCCID.Value & ",'" & txtComments.Text.Replace("'", "''") & "','" & hdnThisApp.Value & "'); select @new_ID = scope_identity(); select @new_ID;"

        Dim dt_new_ID_dt As DataTable = GetTable(sql)

        Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString



        Dim fail_all As Boolean = False
        Dim fail_list As New ArrayList


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
                            Dim rblOptions As RadioButtonList = aRI.FindControl("rblOptions")
                            If rblOptions.SelectedValue <> "" Then
                                If rblOptions.SelectedValue = "0" Then
                                    Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                                    UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtOtherComment.Text.Replace("'", "''") & "')")
                                Else
                                    UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, answer_comment) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & rblOptions.SelectedValue & "')")
                                End If


                            Else
                                'no selection, get the first default answer and save it as the answer comment
                                Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments where question_id = " & hdnQID.Value & " and answer_id in (select id from question_answers where right_answer = 1 and question_id = " & hdnQID.Value & ")")
                                If default_ans.Rows.Count > 0 Then
                                    UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, answer_comment) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "," & default_ans.Rows(0).Item("ID") & ")")
                                Else
                                    UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ")")
                                End If


                            End If
                            'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "))")


                        End If
                    Next
                End If



            Next

        Next

        UpdateTable("update form_score_training set calib_id = " & hdnCalibID.Value & " where id = " & new_ID)
        UpdateTable("updateCalibrationTraining " & new_ID) ' get effective calibration score
        UpdateTable("UpdateMissedTraining " & new_ID)

        If chkStopWorking.Checked Then
            Response.Redirect("default.aspx")
        Else
            Response.Redirect("review_training.aspx?ID=" & new_ID)

            'Response.Redirect("http://work.pointqa.com/listen3.aspx")
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
End Class

