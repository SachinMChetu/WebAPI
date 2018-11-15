Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net

Public Class ER2
    Inherits System.Web.UI.Page

    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Public data_rate As String
    Public download_id As String
    Public play_option As String = "true"


    'Public Shared Sub UpdateTable(out As String) 
    '    HttpContext.Current.Response.Write(out & "<br>")
    'End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateQAAnswers(ByVal id As Integer, ByVal qid As Integer, ByVal name As String, ByVal answer As String) As UpdateAnswerResponse

        Dim uar As New UpdateAnswerResponse

        Dim msg As String = "Successfully updated TL response."
        Try
            UpdateTable("INSERT INTO [dbo].[form_c_score_changes_client]([form_id],[question_id],[changed_by],[changed_date],[approved]," &
             "[approved_by],[approved_date],[new_value]) select " & id & ",(select question_id from calibration_scores_client where id = " & qid & "),'" & name & "',dbo.getMTDate()," &
             "1,'" & name & "',dbo.getMTDate(),(select id from question_answers where question_id = (select question_id from calibration_scores_client where id = " & qid & ") and answer_text = '" & answer & "')")

            Dim Sql As String = "insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" & name & "', dbo.getMTDate(), '<strong>' + (select q_short_name from questions where id = (select question_id from calibration_scores_client where id = " & qid & "))   +  '</strong> changed to <strong>' + '" & answer & "</strong>'," & id & ", 'Client Calibration'"
            'Error when executing this query
            UpdateTable(Sql)

            Dim ans_dt As DataTable = GetTable("select * from calibration_scores_client where id = " & qid)
            If ans_dt.Rows.Count > 0 Then
                UpdateTable("update calibration_scores_client set question_result =  (select id from question_answers where question_id = (select question_id from calibration_scores_client where id = " & qid & ") and answer_text = '" & answer & "') where id = " & qid)
            Else
                UpdateTable("INSERT INTO [dbo].[calibration_scores_client]([form_id],[question_id],question_result) select " & id & ",(select question_id from calibration_scores_client where id = " & qid & "),(select id from question_answers where question_id = (select question_id from calibration_scores_client where id = " & qid & ") and answer_text = '" & answer & "')")
            End If

            UpdateTable("exec updateCalibrationClient " & id)




            uar.success = True
            Dim rr_dt As DataTable = GetTable("select convert(decimal(10,1), total_score) as total_score,(select q_short_name from questions where id = (select question_id from calibration_scores_client where id = " & qid & ")) as q_changed, (select right_answer from question_answers where question_id = (select question_id from calibration_scores_client where id = " & qid & ") and answer_text = '" & answer & "') as right_wrong from calibration_form_client where id = " & id)

            If rr_dt.Rows.Count > 0 Then
                uar.score = rr_dt.Rows(0).Item("total_score")
                uar.right_answer = rr_dt.Rows(0).Item("right_wrong")
                uar.added_note = "<tr class='data-row' data-section='101' unselectable='on' style='-webkit-user-select: none;'><td class='emptyBox'></td><td class='question'><strong>" & rr_dt.Rows(0).Item("q_changed") & "</strong> changed to <strong>" & answer & "</strong></td><td class='response'>" & name & "</td><td>" & Now & "</td><td class='td-play'><button onclick='jumpPos();return false;'><div></div></button></td></tr>"
            End If



        Catch ex As Exception
            msg = "An error occured while saving QA response." + ex.Message
            uar.success = False
        End Try

        Return uar

        'Return id.ToString & "|" & qid.ToString & "|" & name & "|" & answer
    End Function

    Public Class UpdateAnswerResponse
        Public success As Boolean
        Public right_answer As Integer
        Public score As Single
        Public added_note As String
    End Class


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        download_id = Request("ID")

        If User.IsInRole("Agent") Then
            Response.Redirect(Request.ServerVariables("HTTP_REFERER"))
        End If

        If Not User.Identity.IsAuthenticated And Request("agent") Is Nothing Then
            Response.Redirect("Login.aspx?default.aspx")
        End If

        'If User.IsInRole("QA") Or (Not User.Identity.IsAuthenticated And Request("agent") IsNot Nothing) Then 'User.IsInRole("Client") Or  removed
        '    hlEdit.Visible = False
        '    'hlEdit.NavigateUrl = "edit_record.aspx?ID=" & Request("ID")
        '    'fvFORMData.FindControl("NotificationControl").Visible = False
        'Else
        '    'hlEdit.NavigateUrl = "/edit_record.aspx?ID=" & Request("ID")
        'End If




        If Not User.IsInRole("Admin") Then
            lbAddCalibration.Visible = False
            lbAddDispute.Visible = False
        End If

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            play_option = user_info_dt.Rows(0).Item("calls_start_immediately").ToString
        Else
            data_rate = 0.05
        End If


        If Session("appname") Is Nothing Then
            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            Session("appname") = domain(0)
        End If

        If Not IsPostBack Then


            'Try
            '    Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

            '    litUserName.Text = User.Identity.Name
            '    litUserEmail.Text = mu.Email

            '    Dim pfl As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            '    If pfl IsNot Nothing Then
            '        If pfl.Avatar <> "" Then
            '            imgAvatar.ImageUrl = "/audio/" & User.Identity.Name & "/" & pfl.Avatar
            '        End If
            '    End If

            'Catch ex As Exception

            'End Try

            'UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" & User.Identity.Name & "',dbo.getMTDate(), (select review_id from calibration_form where id = " & Request("ID") & "),'review'")


            If Request("ID") IsNot Nothing Then
                hdnThisID.Value = Request("ID")
                Dim record_dt As DataTable = GetTable("select * from calibration_form_client join vwForm on vwForm.f_id=calibration_form_client.original_form  where calibration_form_client.id = " & Request("ID"))
                If record_dt.Rows.Count > 0 Then
                    hdnCallLength.Value = record_dt.Rows(0).Item("call_length").ToString
                    hdnThisAgent.Value = record_dt.Rows(0).Item("Agent").ToString
                    hdnFormID.Value = record_dt.Rows(0).Item("F_ID").ToString
                Else
                    Response.Redirect("default.aspx")

                End If
            End If




            'fvFORMData.FindControl("NotificationControl").Visible = False

        End If
    End Sub

    Protected Sub fvFORMData_DataBound(sender As Object, e As System.EventArgs) Handles fvFORMData.DataBound
        Dim lbl As Label = fvFORMData.FindControl("lblPlayer")




        Dim drv As DataRowView = fvFORMData.DataItem

        If drv Is Nothing Then
            Response.Redirect("default.aspx")
        End If


        If drv.Item("client_logo").ToString() = "" Then
            lblAppname.Text = drv.Item("FullName").ToString()
        Else
            lblAppname.Text = "<img src='" & drv.Item("client_logo").ToString() & "' />"
        End If



        If DateDiff(DateInterval.Hour, drv("review_date"), Now) < 2 And drv("reviewer") = User.Identity.Name Then
            hlEdit.Visible = True
            'hlEdit.NavigateUrl = "edit_record.aspx?ID=" & Request("ID")
        End If



        litUserName.Text = drv.Item("agent").ToString()


        'If Not User.Identity.IsAuthenticated And Request("agent") IsNot Nothing Then
        '    fvFORMData.FindControl("NotificationControl").Visible = False
        'End If


        Dim pfl As ProfileCommon = Profile.GetProfile(drv.Item("reviewer").ToString())
        If pfl IsNot Nothing Then
            If pfl.Avatar <> "" Then
                ' imgAvatar.ImageUrl = "/audio/" & drv.Item("reviewer").ToString() & "/" & pfl.Avatar
            End If
        End If


        'Dim agentdt As DataTable = GetTable("select min(call_date) from xcc_report_new where call_date > dateadd(d,-30,dbo.getMTDate()) and agent = '" & drv.Item("agent").ToString() & "'")
        'If agentdt.Rows.Count > 0 Then
        '    If Not IsDBNull(agentdt.Rows(0).Item(0)) Then
        '        If agentdt.Rows(0).Item(0) > DateAdd(DateInterval.Day, -14, Today) Then
        '            ' rightHeader.Style.Add("background-color", "LightGreen")
        '        End If
        '    End If
        'End If


        hdnCallLength2.Value = drv.Item("call_length").ToString

        hdnThisApp.Value = drv.Item("appname").ToString


        litAgentName.Text = drv.Item("agent").ToString
        litQAName.Text = drv.Item("reviewed_by").ToString

        litPersonal.Text = litPersonal.Text & getSidebarData3("First", UpperLeft(drv.Item("First_Name").ToString()))
        litPersonal.Text = litPersonal.Text & getSidebarData3("Last", UpperLeft(drv.Item("Last_Name").ToString()))
        litPersonal.Text = litPersonal.Text & getSidebarData3("Email", drv.Item("Email").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("Phone", drv.Item("phone").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("Education Level", drv.Item("EducationLevel").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("High School Grad Year", drv.Item("HighSchoolGradYear").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("Degree Start Timeframe", drv.Item("DegreeStartTimeframe").ToString())

        'Contact
        litPersonal.Text = litPersonal.Text & getSidebarData3("Address", drv.Item("address").ToString())
        If drv.Item("State").ToString() <> "" Then
            litPersonal.Text = litPersonal.Text & getSidebarData3("", drv.Item("City").ToString() & ", " & drv.Item("State").ToString() & " " & drv.Item("Zip").ToString())
        End If
        litPersonal.Text = litPersonal.Text & getSidebarData3("ANI", drv.Item("ANI").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("DNIS", drv.Item("DNIS").ToString())

        If drv.Item("vertical").ToString = "Education" Then ' Only show schools for those with the right vertical

            'School

            Dim sch_dt As DataTable = GetTable("getSchoolDataWithPos " & drv.Item("f_id").ToString & ",  " & drv.Item("review_id").ToString)
            Dim sch_x As Integer = 1
            For Each sch_dr In sch_dt.Rows

                litSchool.Text = litSchool.Text & getSchoolSidebarHeader(sch_dr.Item("School").ToString(), sch_dr.Item("sch_pos").ToString())

                litSchool.Text = litSchool.Text & getSchoolSidebarData("College", sch_dr.Item("College").ToString(), sch_dr.Item("col_pos").ToString())
                litSchool.Text = litSchool.Text & getSchoolSidebarData("Degree", sch_dr.Item("DegreeOfInterest").ToString(), sch_dr.Item("deg_pos").ToString())

                'litSchool.Text = litSchool.Text & getSidebarData("School " & sch_x, sch_dr.Item("School").ToString())
                litSchool.Text = litSchool.Text & getSchoolSidebarData("Area 1", sch_dr.Item("AOI1").ToString(), sch_dr.Item("aoi_pos").ToString())
                'litSchool.Text = litSchool.Text & getSchoolSidebarData("Area 2", sch_dr.Item("AOI2").ToString(), sch_dr.Item("College").ToString())
                litSchool.Text = litSchool.Text & getSchoolSidebarData("Subject 1", sch_dr.Item("L1_SubjectName").ToString(), sch_dr.Item("sub_pos").ToString())
                litSchool.Text = litSchool.Text & getSchoolSidebarData("Modality", sch_dr.Item("Modality").ToString(), sch_dr.Item("mod_pos").ToString())
                litSchool.Text = litSchool.Text & getSidebarData("Affiliate", sch_dr.Item("origin").ToString())
                sch_x += 1
            Next


        Else
            liSchoolItem.Visible = False
        End If


        Dim prefs_dt As DataTable = GetTable("select dbo.getTemplateText2(f_id, questions.id) as pref_data, * from vwForm join questions on questions.appname = vwForm.appname  where template='Preferences' and f_id = " & drv.Item("f_id").ToString)


        For Each prefs_dr In prefs_dt.Rows
            litPrefs.Text = litPrefs.Text & getSidebarData(prefs_dr.Item("q_short_name").ToString(), prefs_dr.Item("pref_data").ToString())

        Next


        prefs_dt = GetTable("select * from form_c_scores_options where form_id = " & drv.Item("id").ToString & " and question_id in (select id from questions where template='Preferences')")

        For Each prefs_dr In prefs_dt.Rows
            getSidebarData(prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_pos").ToString())

        Next


        'lblSession.Text = drv.Item("Session_ID").ToString().Replace("()", "")

        'Other
        litOther.Text = litOther.Text & getSidebarData3("CAMPAIGN", drv.Item("CAMPAIGN").ToString())
        litOther.Text = litOther.Text & getSidebarData3("DATE", drv.Item("call_date").ToString())
        litOther.Text = litOther.Text & getSidebarData3("Session", drv.Item("Session_ID").ToString())
        litOther.Text = litOther.Text & getSidebarData3("Profile ID", drv.Item("profile_id").ToString())

        Dim history_dt As DataTable = GetTable("select * from session_viewed where session_id = '" & drv.Item("review_id").ToString() & "'")
        If history_dt.Rows.Count > 0 Then
            'litOther.Text = litOther.Text & "Session Viewed<br>"
            For Each dr As DataRow In history_dt.Rows()
                'litsession.Text = litsession.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("agent") & "</span><strong>" & dr.Item("date_viewed") & " - " & dr.Item("page_viewed") & "</strong></li>"
                litsession.Text = litsession.Text & getSidebarData3(dr.Item("agent"), dr.Item("date_viewed") & " - " & dr.Item("page_viewed"))
            Next
        End If

        'Dim edit_dt As DataTable = GetTable("select changed_by, changed_date, answer_text, q_short_name  from form_q_score_changes join question_answers on question_answers.id = new_value join questions on questions.id = question_answers.question_id  where form_id = '" & hdnThisID.Value & "'")
        'If edit_dt.Rows.Count > 0 Then
        '    litOther.Text = litOther.Text & "<br>Edit Info<br>"
        '    For Each dr As DataRow In edit_dt.Rows()
        '        litOther.Text = litOther.Text & " <li><i class='fa fa-angle-right'></i><span>" & dr.Item("changed_by") & "</span><strong>" & dr.Item("changed_date") & " changed " & dr.Item("q_short_name") & " to " & dr.Item("answer_text") & "</strong></li>"
        '    Next
        'End If


        'If drv.Item("comments").ToString <> "" Then
        '    Dim litComments As Literal = rptSections.Controls(rptSections.Controls.Count - 1).Controls(0).FindControl("litComments")
        '    litComments.Text = drv.Item("comments").ToString
        'Else
        '    Dim footer_tr As HtmlTableRow = rptSections.Controls(rptSections.Controls.Count - 1).Controls(0).FindControl("footer_tr")
        '    footer_tr.visible = False
        'End If


        If drv.Item("missed_list").ToString = "" Then
            spanMissed.Visible = False
        End If

        Dim ml() As String = drv.Item("missed_list").ToString.Split(",")
        For Each mlitem In ml
            If litJumpQ.Text = "" Then
                litJumpQ.Text = "<a href=""javascript:jumpToQuestion('" & mlitem & "');"">" & mlitem & "</a>"
            Else
                litJumpQ.Text &= ", <a href=""javascript:jumpToQuestion('" & mlitem & "');"">" & mlitem & "</a>"
            End If

        Next


        litScore.Text = drv.Item("total_score").ToString

        Dim this_filename As String = GetAudioFileName(drv.Row)

        audio_file = this_filename


        If User.IsInRole("Client") Then
            'fvFORMData.FindControl("NotificationControl").Visible = False
        End If

    End Sub

    Protected Sub CheckRow(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = e.Row.DataItem

            If drv.Item("real_result").ToString = "Wrong" Then
                e.Row.BackColor = Drawing.Color.Orange
            End If

        End If
    End Sub

    'Protected Sub gvQuestions_PreRender(sender As Object, e As EventArgs) Handles gvQuestions.PreRender
    '    If gvQuestions.HeaderRow IsNot Nothing Then
    '        gvQuestions.HeaderRow.TableSection = TableRowSection.TableHeader
    '    End If
    'End Sub

    'Protected Sub ShowAll(sender As Object, e As EventArgs)
    '    hdnFilter.Value = ""
    '    hdnFilter_ValueChanged(sender, e)
    'End Sub

    'Protected Sub ShowRight(sender As Object, e As EventArgs)
    '    hdnFilter.Value = "Right"
    '    hdnFilter_ValueChanged(sender, e)
    'End Sub

    'Protected Sub ShowWrong(sender As Object, e As EventArgs)
    '    hdnFilter.Value = "Wrong"
    '    hdnFilter_ValueChanged(sender, e)
    'End Sub

    'Protected Sub hdnFilter_ValueChanged(sender As Object, e As EventArgs) Handles hdnFilter.ValueChanged
    '    If hdnFilter.Value = "" Then
    '        dsQuestions.FilterExpression = Nothing
    '    Else
    '        dsQuestions.FilterExpression = "real_result = '" & hdnFilter.Value & "'"
    '    End If

    '    Select Case hdnFilter.Value
    '        Case ""
    '            AllTab.Attributes.Add("class", "selected-tab")
    '            RightTab.Attributes.Add("class", "")
    '            WrongTab.Attributes.Add("class", "")
    '        Case "Right"
    '            AllTab.Attributes.Add("class", "")
    '            RightTab.Attributes.Add("class", "selected-tab")
    '            WrongTab.Attributes.Add("class", "")
    '        Case "Wrong"
    '            AllTab.Attributes.Add("class", "")
    '            RightTab.Attributes.Add("class", "")
    '            WrongTab.Attributes.Add("class", "selected-tab")
    '    End Select

    '    dsQuestions.DataBind()
    'End Sub

    Protected Sub gvQuestions_RowDataBound(sender As Object, e As RepeaterItemEventArgs) 'Handles gvQuestions.RowDataBound
        If hdnCallLength.Value <> "" Then
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                'Add div for audio if the answer is right or wrong
                Dim position As Integer = 0

                If hdnCallLength.Value > 0 And e.Item.DataItem("q_pos").ToString <> "" Then
                    position = CInt(e.Item.DataItem("q_pos") / hdnCallLength.Value * 100)
                End If

                If e.Item.DataItem("bad-response").ToString = "" Then
                    litSliderPoints.Text &= " <div class='warning-part' style='left: " & position & "%;' title='" & e.Item.DataItem("q_short_name") & "'><span></span><a href='#' class='listen-from-here' style='background: none repeat scroll 0% 0% rgb(154, 190, 46);'>&#x2714</a></div>" & Chr(13)
                Else
                    litSliderPoints.Text &= " <div class='warning-part' style='left: " & position & "%;' title='" & e.Item.DataItem("q_short_name") & "'><span></span><a href='#' class='listen-from-here'>!</a></div>" & Chr(13)
                End If


                Dim ddlAnswers As DropDownList = CType(e.Item.FindControl("ddlAnswers"), DropDownList)

                'SetddlAnswers(ddlAnswers)

                'ddlAnswers.SelectedValue = e.Item.DataItem("answer_text").ToString()

                ddlAnswers.Attributes.Add("onchange", String.Format("process(this, '{0}','{1}', '{2}');", e.Item.DataItem("ID").ToString(), User.Identity.Name, e.Item.DataItem("form_id").ToString())) 'HttpContext.Current.User.Identity.Name

            End If
        End If
    End Sub

    'Protected Sub SetddlAnswers(ByVal ddl As DropDownList)
    '    Dim l As New List(Of String())() From {New String() {"Yes", "Yes"}, New String() {"No", "No"}, New String() {"Na", "Na"}}

    '    For Each s As String() In l
    '        ddl.Items.Add(New ListItem(s(1), s(0)))
    '    Next
    'End Sub

    'Protected Sub ibEdit_Click(sender As Object, e As ImageClickEventArgs) Handles ibEdit.Click
    '    Response.Redirect("edit_record.aspx?ID=" & Request("ID"))
    'End Sub


    Protected Sub lbAddCalibration_Click(sender As Object, e As System.EventArgs) Handles lbAddCalibration.Click


        'see if there is one to replace first
        Dim cal_dt As DataTable = GetTable("select top 1 * from calibration_pending where appname =  and review_type = 'random 4%' order by id desc")
        If cal_dt.Rows.Count > 0 Then
            ' UpdateTable("delete from calibration_pending where id = " & cal_dt.Rows(0).Item("ID"))
            UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname) select 'Admin Selected','" & hdnThisID.Value & "',(select reviewer from calibration_form where id = " & hdnThisID.Value & "),'Admin Selected','" & cal_dt.Rows(0).Item("week_ending") & "','" & Session("appname") & "'")
            'Response.Write("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending) select 'Admin Selected','" & hdnThisID.Value & "',(select reviewer from calibration_form where id = " & hdnThisID.Value & "),'Admin Selected','" & cal_dt.Rows(0).Item("week_ending") & "'")
            'Response.End()
            lbAddCalibration.Visible = False
        End If
    End Sub

    Protected Sub lbAddDispute_Click(sender As Object, e As System.EventArgs) Handles lbAddDispute.Click
        UpdateTable("INSERT INTO [notifications] ([assigned_to], [dateadded], [acknowledged], [form_id], [ack_date], opened_by, ack_by) select 'QA',  dbo.getMTDate(), 1, " & hdnThisID.Value & ", dbo.getMTDate(), (select reviewer from calibration_form where ID = " & hdnThisID.Value & "),'Client'")
        lbAddDispute.Visible = False

    End Sub

    Protected Function getSidebarData(lbl As String, value As String, Optional start_pos As String = "") As String
        If value <> "" Then
            'Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"

            If start_pos <> "" Then
                Dim position As Integer
                position = CInt(CInt(start_pos) / hdnCallLength2.Value * 100)

            End If

            Return "<tr><td class='info-label'>" & lbl & "</td><td class='info-data'>" & value & "</td></tr>"

        Else
            Return ""
        End If

    End Function

    Protected Function getSchoolSidebarData(lbl As String, value As String, start_pos As String) As String
        If value <> "" Then
            Dim position As Integer

            If start_pos <> "" Then

                position = CInt(CInt(start_pos) / hdnCallLength2.Value * 100)

                Return "<tr><td class='info-label'>" & lbl & "</td><td class='info-data'><a onclick='jumpPos(" & start_pos & ");return false;'><i style='cursor: pointer'>" & value & "</i></a></td></tr>"
            Else
                Return "<tr><td class='info-label'>" & lbl & "</td><td class='info-data' style='color:#888'>" & value & "</td></tr>"
            End If


        Else
            Return ""
        End If

    End Function



    Protected Function getSchoolSidebarHeader(value As String, start_pos As String) As String
        If value <> "" Then
            Dim position As Integer

            If start_pos <> "" Then

                position = CInt(CInt(start_pos) / hdnCallLength2.Value * 100)

                Return " <tr><td class='school-name' colspan='2'><a onclick='jumpPos(" & start_pos & ");return false;'><i style='cursor: pointer'>" & value & "</i></a></td></tr>"
            Else
                Return " <tr><td class='school-name' colspan='2'  style='color:#888'>" & value & "</td></tr>"

            End If


        Else
            Return ""
        End If

    End Function

    Protected Sub rptContact_PreRender(sender As Object, e As EventArgs)
        Dim myRepeater As Repeater = DirectCast(sender, Repeater)
        Dim counter As Integer = myRepeater.Items.Count
        If counter = 0 Then
            myRepeater.Visible = False
        End If
    End Sub


    Protected Sub rptComments_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim drv As DataRowView = e.Item.DataItem
            Dim position As Integer

            Try


                'comment_header
                'comment_pos


                If drv.Item("click_pos") <> "" Then

                    position = CInt(CInt(drv.Item("click_pos")) / CInt(hdnCallLength.Value) * 100)
                    litSliderPoints.Text &= "<div class='warning-part' style='left: " & CInt(CInt(drv.Item("click_pos")) / CInt(hdnCallLength.Value) * 100) & "%;' title='" & drv.Item("comment_header").ToString & "'><span></span><a href='#' class='listen-from-here' style='background: none repeat scroll 0% 0% rgb(102,178,255);'>&#x2724</a></div>" & Chr(13)

                End If
            Catch ex As Exception
                Response.Write("<!--" & position & "-->")
            End Try

        End If

    End Sub

    Private Sub hlEdit_Click(sender As Object, e As EventArgs) Handles hlEdit.Click
        Response.Redirect("edit_record.aspx?ID=" & Request("ID"))
    End Sub

    Private Sub btnDldAudio_Click(sender As Object, e As EventArgs) Handles btnDldAudio.Click
        Response.Redirect("download_audio.aspx?ID=" & Request("ID"))
    End Sub

    Private Sub btnDldCall_Click(sender As Object, e As EventArgs) Handles btnDldCall.Click
        Response.Redirect("download_call.aspx?ID=" & Request("ID"))
    End Sub


End Class