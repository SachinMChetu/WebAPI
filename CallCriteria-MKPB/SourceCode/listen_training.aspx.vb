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

    Public isWebsite As Boolean = False
    Public myDelay As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?ReturnURL=Listen_training.aspx")
        End If

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
            myDelay = user_info_dt.Rows(0).Item("guideline_display").ToString
        Else
            data_rate = 0.05
        End If



        Dim dt2 As DataTable = GetTable("exec getMyNextTrainingCall '" & User.Identity.Name & "'")


        If dt2.Rows.Count = 0 Then

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "no_work", "alert('No calls to work.  Have the team lead check to see that the correct scorecard is selected and set to Trainee.');", True)
            Exit Sub 'Response.Redirect("default.aspx")
            'Response.Redirect("cd2.aspx")
        End If



        Dim cali_count As Integer
        If IsDBNull(dt2.Rows(0).Item("training_count")) Then
            cali_count = 0
        Else
            cali_count = dt2.Rows(0).Item("training_count")
        End If

        'Get user last 5 
        'Dim avg_dt As DataTable = GetTable("select isnull(avg(trainee_score),0), count(*) as number_scores, (select count(*) from vwTrain join sc_training_approvals on vwTrain.reviewer = sc_training_approvals.username and vwTrain.scorecard =  sc_training_approvals.sc_id  with (nolock) where reviewer = '" & User.Identity.Name & "' and sc_id = '" & dt2.Rows(0).Item("scorecard") & "') as total_tries from (select top " & cali_count & " trainee_score from vwTrain with (nolock) where reviewer = '" & User.Identity.Name & "' and sc_id = '" & dt2.Rows(0).Item("scorecard") & "' order by id desc) a")
        'Dim avg_dt As DataTable = GetTable("exec checkQATraining '" & User.Identity.Name & "'," & dt2.Rows(0).Item("scorecard") & "," & cali_count)

        'If avg_dt.Rows.Count > 0 Then


        Try
            If dt2.Rows(0).Item("total_tries") > (cali_count * 2.5) Then
                Email_Error(User.Identity.Name & " has tried " & dt2.Rows(0).Item("total_tries") & " To train And would be kicked off permanently from scorecard " & dt2.Rows(0).Item("scorecard") & ".", "chad@callcriteria.com, brian@callcriteria.com, ryan@callcriteria.com")
            End If
        Catch ex As Exception
            Email_Error(ex.Message)
        End Try



        lblAvgScore.Text = dt2.Rows(0).Item("trainee_avg_score")

        Dim pass_score As Integer = dt2.Rows(0).Item("pass_percent")
        If User.IsInRole("QA Lead") Or User.IsInRole("Calibrator") Then
            pass_score = dt2.Rows(0).Item("pass_percent") + 5 '90
        End If

        If ((dt2.Rows(0).Item("trainee_avg_score") >= pass_score) And (dt2.Rows(0).Item("number_scores") >= cali_count)) Then



            UpdateTable("update userapps set scorecard_role = 'QA' where user_scorecard = '" & dt2.Rows(0).Item("scorecard") & "' and username = '" & User.Identity.Name & "'")


            'Email.To.Add(New MailAddress("stace@datadrivendevelopment.com"))
            Dim Body As String = User.Identity.Name & " has passed in " & dt2.Rows(0).Item("short_name") & " with an avg score of " & dt2.Rows(0).Item("trainee_avg_score") & Chr(13) & Chr(13) & "<br>"

            Dim scores_dt As DataTable = GetTable("select * from vwTrain with (nolock) where reviewer = '" & User.Identity.Name & "' and scorecard = '" & dt2.Rows(0).Item("scorecard") & "' order by id")

            For Each score_dr As DataRow In scores_dt.Rows
                Body &= "http://app.callcriteria.com/review_training.aspx?ID=" & score_dr.Item("ID") & " - score:" & score_dr.Item("trainee_score") & "<br>"
            Next



            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn.Open()


            'If debug Then
            Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',   @recipients='ryan@callcriteria.com;stace@callcriteria.com;brian@callcriteria.com;chad@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
            reply2.Parameters.AddWithValue("Subject_text", "Trainee Passed " & dt2.Rows(0).Item("short_name"))
            reply2.Parameters.AddWithValue("Body", Body.ToString)

            reply2.CommandTimeout = 60
            reply2.ExecuteNonQuery()
            'End If

            cn.Close()
            cn.Dispose()


            Dim sc_dt As DataTable = GetTable("select id from sc_training_approvals where username = '" & User.Identity.Name & "' and sc_id = " & dt2.Rows(0).Item("scorecard").ToString)

            If sc_dt.Rows.Count = 0 Then
                UpdateTable("insert into sc_training_approvals(username, sc_id, sc_date, sc_by, retrain_date) select '" & User.Identity.Name & "'," & dt2.Rows(0).Item("scorecard").ToString & ",dbo.getMTDate(), 'Passed', dateadd(d, 14, dbo.getMTDate())")
            End If





            'Email_Error(User.Identity.Name & " has passed in " & Session("appname") & " with an avg score of " & avg_dt.Rows(0).Item("trainee_avg_score"), "tracy@callcriteria.com")
            Response.Redirect("Training_complete.aspx")
        End If

        lblNumberScores.Text = dt2.Rows(0).Item("number_scores")
        'End If



        If User.Identity.Name = "" Then
            Response.Redirect("login.aspx?ReturnURL=listen_training.aspx")
        End If

        'Disable button on click, still allow server processing
        btnSaveSession.Attributes.Add("onclientclick", "if(Page_ClientValidate('ValidationGroup')){this.disabled = true;" + ClientScript.GetPostBackEventReference(btnSaveSession, Nothing) + ";}")

        If Not IsPostBack Then
            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "listen collapsed-menu")

            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)





            If dt2.Rows.Count = 0 Then
                Response.Redirect("default.aspx")
            End If

            If dt2.Rows(0).Item("website").ToString <> "" Then
                isWebsite = True
            End If



            'hdnCampaign.Value = dt2.Rows(0).Item("campaign").ToString
            'hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString
            hdnCalibID.Value = dt2.Rows(0).Item("calib_id").ToString

            'Dim agentdt As DataTable = GetTable("select min(call_date) from xcc_report_new with (nolock) where call_date > dateadd(d,-30,dbo.getMTDate()) and agent = '" & dt2.Rows(0).Item("agent").ToString() & "'")
            'If agentdt.Rows.Count > 0 Then
            '    If Not IsDBNull(agentdt.Rows(0).Item(0)) Then
            '        If agentdt.Rows(0).Item(0) > DateAdd(DateInterval.Day, -14, Today) Then
            '            'rightHeader.Style.Add("background-color", "LightGreen")
            '        End If
            '    End If
            'End If


            TopDataBlock.top_data = dt2.DefaultView(0)




            Dim history_dt As DataTable = GetTable("select * from session_viewed where session_id = '" & dt2.Rows(0).Item("ID").ToString() & "'")
            If history_dt.Rows.Count > 0 Then
                'litOther.Text = litOther.Text & "<center>Session Viewed</center><br>"
                For Each dr As DataRow In history_dt.Rows()
                    'litOther.Text = litOther.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("agent") & "</span><strong>" & dr.Item("date_viewed") & "</strong></li>"
                Next
            End If
            hdnXCCID.Value = dt2.Rows(0).Item("review_ID").ToString
            hdnScorecard.Value = dt2.Rows(0).Item("scorecard").ToString
            hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString


            If dt2.Rows(0).Item("website").ToString() <> "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "show_website", "window.open('" & dt2.Rows(0).Item("website") & "','myWindow', 'width=800, height=600');", True)
            End If


            Dim client_dt As DataTable = GetTable("select * From app_settings with (nolock) where appname = '" & dt2.Rows(0).Item("appname").ToString & "'")



            If client_dt.Rows(0).Item("client_logo").ToString() = "" Then
                lblThisApp.Text = "Call from " & client_dt.Rows(0).Item("FullName").ToString()
            Else
                lblThisApp.Text = "<img src='" & client_dt.Rows(0).Item("client_logo").ToString() & "' />"
            End If

            'lblArea1.Text = dt2.Rows(0).Item("program").ToString
            'lblArea2.Text = dt2.Rows(0).Item("ID").ToString


            Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
            ' lblSession.Text = session_id.ToString



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


                            Dim txtWebsiteLink As TextBox = aRI.FindControl("txtWebsiteLink")
                            Dim chkWebsite As CheckBox = aRI.FindControl("chkWebsite")

                            If chkWebsite.Checked Then
                                UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, click_text,view_link ) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "','" & txtWebsiteLink.Text & "' )")
                            Else
                                UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, click_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtClickTime.Text & "')")
                            End If




                            Dim chkOptions As CheckBoxList = aRI.FindControl("chkOptions")


                            Dim answer_selected As Boolean = False

                            For Each cblItem As ListItem In chkOptions.Items
                                If cblItem.Selected Then

                                    answer_selected = True

                                    UpdateTable("insert into form_q_training_responses (question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & ",'" & cblItem.Value & "')")

                                End If
                            Next


                            Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                            Dim chkComment As CheckBox = aRI.FindControl("chkComment")
                            If chkComment.Checked Then
                                answer_selected = True
                                UpdateTable("insert into form_q_training_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherComment.Text.Replace("'", "''") & "')")
                            End If

                            If Not answer_selected Then
                                Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments with (nolock) where question_id = " & hdnQID.Value & " and answer_id = " & hdnAnswerID.Value)
                                If default_ans.Rows.Count > 0 Then
                                    UpdateTable("insert into form_q_training_responses ( question_id, form_id, answer_id) values(" & hdnQID.Value & "," & new_ID & "," & default_ans.Rows(0).Item("ID") & ")")
                                End If
                            End If


                            'If rblOptions.SelectedValue <> "" Then
                            '    If rblOptions.SelectedValue = "0" Then
                            '        Dim txtOtherComment As TextBox = aRI.FindControl("txtOtherComment")
                            '        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & txtOtherComment.Text.Replace("'", "''") & "')")
                            '    Else
                            '        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, answer_comment) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ",'" & rblOptions.SelectedValue & "')")
                            '    End If


                            'Else
                            '    'no selection, get the first default answer and save it as the answer comment
                            '    Dim default_ans As DataTable = GetTable("select top 1 * from answer_comments where question_id = " & hdnQID.Value & " and answer_id in (select id from question_answers where right_answer = 1 and question_id = " & hdnQID.Value & ")")
                            '    If default_ans.Rows.Count > 0 Then
                            '        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, answer_comment) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & "," & default_ans.Rows(0).Item("ID") & ")")
                            '    Else
                            '        UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered) values('" & total_sec & "'," & hdnQID.Value & "," & new_ID & ",0," & hdnAnswerID.Value & ")")
                            '    End If


                            'End If
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
                            UpdateTable("insert into form_q_training_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")
                            Response.Write("insert into form_q_training_responses (question_id, form_id,  answer_id, other_answer_text) values(" & hdnQID.Value & "," & new_ID & ",0,'" & txtOtherList.Text.Replace("'", "''") & "')")
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
                                    UpdateTable("insert into form_q_scores_options_training (option_pos, option_value, question_id, form_id, orig_id) values('" & total_sec & "','" & chkOption.Text.Replace("'", "''") & "'," & hdnQID.Value & "," & new_ID & "," & hdnOrigId.Value & ")")
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

                        If na_checked Then

                            UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers where question_id = " & hdnQID.Value & " and answer_text = 'NA'),'" & ContactInfo.Text & "')")
                        Else

                            If all_checked Then 'this has the time, was the answer, write to DB
                                'ClickTime from MM:SS to seconds
                                UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers where question_id = " & hdnQID.Value & " and answer_text = 'Yes'),'" & ContactInfo.Text & "')")
                            Else
                                UpdateTable("insert into form_q_scores_training (q_position, question_id, form_id, question_result, question_answered, other_answer_text) values('" & min_check & "'," & hdnQID.Value & "," & new_ID & ",0,(select id from question_answers where question_id = " & hdnQID.Value & " and answer_text = 'No'),'" & ContactInfo.Text & "')")
                            End If
                        End If

                    End If
                    ' Next
                End If


            Next

        Next

        UpdateTable("update form_score_training set calib_id = " & hdnCalibID.Value & " where id = " & new_ID)
        UpdateTable("updateCalibrationTraining " & new_ID) ' get effective calibration score
        UpdateTable("UpdateScoresTraining " & new_ID)
        UpdateTable("UpdateMissedTraining " & new_ID)

        Response.Redirect("review_training.aspx?ID=" & new_ID)



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


    Protected Sub rptAnswers_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)


        Dim txtOtherComment As TextBox = e.Item.FindControl("txtOtherComment")
        Dim chkComment As CheckBox = e.Item.FindControl("chkComment")
        txtOtherComment.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkComment.ClientID & "').checked = true;")

        Dim txtWebsiteLink As TextBox = e.Item.FindControl("txtWebsiteLink")
        Dim chkWebsite As CheckBox = e.Item.FindControl("chkWebsite")
        txtWebsiteLink.Attributes.Add("onkeyup", "console.log('keyup');document.getElementById('" & chkWebsite.ClientID & "').checked = true;")

        If Not isWebsite Then
            CType(e.Item.FindControl("chkWebsite"), CheckBox).Visible = False
            CType(e.Item.FindControl("txtWebsiteLink"), TextBox).Visible = False




        End If
    End Sub



    Protected Sub chkOptions_DataBound(sender As Object, e As EventArgs)
        Dim chk As CheckBoxList = sender
        'chk.Items.Add(New ListItem("", "0")) 
        If isWebsite Then
            ' CType()
        End If
    End Sub

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

