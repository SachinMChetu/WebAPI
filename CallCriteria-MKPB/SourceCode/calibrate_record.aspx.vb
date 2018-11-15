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




    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        download_id = Request("ID")

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?default.aspx")
        End If


        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            play_option = user_info_dt.Rows(0).Item("calls_start_immediately").ToString
        Else
            data_rate = 0.05
        End If

        If Not IsPostBack Then

            hdnStartTime.Value = Now.ToString

            'release those that have been started, but not finished within 45 mins
            UpdateTable("update calibration_pending set date_started = null where date_started < dateadd(s, -45*60, dbo.getMTDate()) and date_completed is null")


            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "listen collapsed-menu")

            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

            litUserName.Text = User.Identity.Name


            'Dim week_ending As String = DateAdd(DateInterval.Day, -1 - Today.DayOfWeek, Today).ToShortDateString

            'time_left = DateDiff(DateInterval.Second, Now, DateAdd(DateInterval.Day, 7 - Today.DayOfWeek, Today))

            Dim dt_next As DataTable = GetTable("select top 1 *, case when review_type in ('Admin Selected','Client Selected') then 1 else 0 end as cali_priority from calibration_pending where reviewer <> '" & User.Identity.Name & "' and appname = '" & Request("appname") & "' and date_completed is null  and (( date_started is null) or (date_started < dateadd(minute, -45, dbo.getMTDate()))) order by cali_priority, date_added desc") ' and week_ending = '" & week_ending & "'


            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
                dt_next = GetTable("select top 1 * from cali_pending_client where assigned_to = '" & User.Identity.Name & "' and date_completed is null ")
            End If

            hdnStartTime.Value = Now.ToString

            If dt_next.Rows.Count = 0 Then
                rptSections.Visible = False
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "no_work", "alert('No pending calibrations');", True)
                Exit Sub 'Response.Redirect("default.aspx")
            End If

            Dim left_dt As DataTable = GetTable("select count(*) from calibration_pending where appname = '" & Request("appname") & "' and date_completed is null ")   'and week_ending = '" & week_ending & "'

            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
                left_dt = GetTable("select count(*) from cali_pending_client where assigned_to = '" & User.Identity.Name & "' and date_completed is null")
            End If

            ' litLeft.Text = left_dt.Rows(0).Item(0) & " left"


            If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
                UpdateTable("update cali_pending_client set who_processed = '" & User.Identity.Name & "', date_started = dbo.getMTDate() where id = " & dt_next.Rows(0).Item("id"))
                dsSections.SelectCommand = "select * from sections where id in (select section from questions where id in (select distinct question_id from form_q_scores where form_id = (select form_id from cali_pending_client  where id = @form_id))) order by section_order"
            Else
                UpdateTable("update calibration_pending set who_processed = '" & User.Identity.Name & "', date_started = dbo.getMTDate() where id = " & dt_next.Rows(0).Item("id"))
            End If




            rptSections.Visible = True
            Dim dt2 As DataTable = GetTable("select * from vwForm  join app_settings on app_settings.appname = vwForm.appname where vwForm.F_id = " & dt_next.Rows(0).Item("form_id"))




            hdnThisID.Value = dt_next.Rows(0).Item("id")

            If dt2.Rows.Count > 0 Then
                'UpdateTable("update XCC_REPORT_NEW set review_started = dbo.getMTDate() where ID = " & dt2.Rows(0).Item("ID").ToString)
            Else
                Response.Write(hdnThisID.Value)
                Response.End()

                Response.Redirect("default.aspx")
            End If






            Dim record_dt As DataTable = GetTable("select * from calibration_pending join vwForm on vwForm.f_id = form_id where calibration_pending.id = " & hdnThisID.Value)
            If record_dt.Rows.Count > 0 Then
                hdnCallLength.Value = record_dt.Rows(0).Item("call_length").ToString
                hdnThisAgent.Value = record_dt.Rows(0).Item("Agent").ToString
                hdnThisFormID.Value = record_dt.Rows(0).Item("form_id").ToString

            End If



            'fvFORMData.FindControl("NotificationControl").Visible = False

        End If
    End Sub


    Protected Sub btnSaveSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSession.Click
        Dim total_score As Integer = 0




        Dim fail_all As Boolean = False
        Dim fail_list As New ArrayList


        Dim form_test As DataTable = GetTable("select count(*) from calibration_form where original_form in (select form_id from calibration_pending where id = '" & hdnThisID.Value & "')")
        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            form_test = GetTable("select count(*) from calibration_form_client where original_form in (select form_id from cali_pending_client where id = '" & hdnThisID.Value & "')")
        End If


        If form_test.Rows.Count > 0 Then
            If form_test.Rows(0).Item(0) > 0 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "nokey", "<script language=javascript>alert('Form already submitted');</script>")
                Exit Sub
            End If
        End If


        Dim dt_new_ID_dt As DataTable = GetTable("declare @new_id int; insert into calibration_form (original_form, reviewed_by,review_date, calibration_comment, review_started) select  (select form_id from calibration_pending where id = '" & hdnThisID.Value & "'),'" & User.Identity.Name & "',dbo.getMTDate(),'" & txtComments.Text.Replace("'", "''") & "','" & hdnStartTime.Value & "' ; select @new_ID = scope_identity(); select @new_ID;")

        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            dt_new_ID_dt = GetTable("declare @new_id int; insert into calibration_form_client (original_form, cpc_id, reviewed_by,review_date, calibration_comment, review_started) select  (select form_id from cali_pending_client where id = '" & hdnThisID.Value & "')," & hdnThisID.Value & ",'" & User.Identity.Name & "',dbo.getMTDate(),'" & txtComments.Text.Replace("'", "''") & "','" & hdnStartTime.Value & "' ; select @new_ID = scope_identity(); select @new_ID;")
        End If

        Dim new_ID As String = dt_new_ID_dt.Rows(0).Item(0).ToString


        For Each ri As RepeaterItem In rptSections.Items
            For Each ri2 As RepeaterItem In CType(ri.FindControl("Repeater2"), Repeater).Items

                Dim QID As String = CType(ri2.FindControl("hdnQID"), HiddenField).Value
                Dim pos As String = CType(ri2.FindControl("hdnUpdateTime"), HiddenField).Value
                Dim QAns As String = CType(ri2.FindControl("ddlAnswers"), DropDownList).SelectedValue

                If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
                    UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos) select " & QID & "," & new_ID & "," & QAns & ",'" & pos & "'")
                Else
                    UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos) select " & QID & "," & new_ID & "," & QAns & ",'" & pos & "'")
                End If

            Next
        Next



        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            UpdateTable("update cali_pending_client set date_completed = dbo.getMTDate() where id = '" & hdnThisID.Value & "'")
            UpdateTable("update calibration_form_client set cali_dev = dbo.GetCaliDeviationClient(" & new_ID & ") where id = " & new_ID)
            UpdateTable("updateCalibrationClient '" & new_ID & "'")
        Else
            UpdateTable("update calibration_pending set date_completed = dbo.getMTDate() where id = '" & hdnThisID.Value & "'")
            UpdateTable("update calibration_form set cali_dev = dbo.GetCaliDeviation(" & new_ID & ") where id = " & new_ID)
            UpdateTable("updateCalibration '" & new_ID & "'")
        End If





        If chkStopWorking.Checked Then
            Response.Redirect("default.aspx")
        Else
            If Request("week_ending") IsNot Nothing Then
                Response.Redirect("calibrate_record.aspx?week_ending=" & Request("week_ending") & "&appname=" & Request("appname"))
            Else
                Response.Redirect("calibrate_record.aspx?appname=" & Request("appname"))
            End If

        End If



    End Sub



    Protected Sub fvFORMData_DataBound(sender As Object, e As System.EventArgs) Handles fvFORMData.DataBound
        Dim lbl As Label = fvFORMData.FindControl("lblPlayer")




        Dim drv As DataRowView = fvFORMData.DataItem

        If drv Is Nothing Then
            Response.Redirect("default.aspx")
        End If


        lblAppname.Text = drv.Item("FullName").ToString()
        'If gvQuestions.FooterRow IsNot Nothing Then
        '    gvQuestions.FooterRow.Cells(3).Text = "Total Score: "
        '    gvQuestions.FooterRow.Cells(4).Text = drv.Item("total_score").ToString()
        '    gvQuestions.FooterRow.Cells(4).Font.Bold = True
        'End If


        litUserName.Text = drv.Item("agent").ToString()


        'If Not User.Identity.IsAuthenticated And Request("agent") IsNot Nothing Then
        '    fvFORMData.FindControl("NotificationControl").Visible = False
        'End If


        hdnCallLength2.Value = drv.Item("call_length").ToString

        hdnThisApp.Value = drv.Item("appname").ToString


        litAgentName.Text = drv.Item("agent").ToString
        litQAName.Text = drv.Item("reviewer").ToString

        litPersonal.Text = litPersonal.Text & getSidebarData3("First", UpperLeft(drv.Item("First_Name").ToString()))
        litPersonal.Text = litPersonal.Text & getSidebarData3("Last", UpperLeft(drv.Item("Last_Name").ToString()))
        litPersonal.Text = litPersonal.Text & getSidebarData3("Email", drv.Item("Email").ToString())
        'litPersonal.Text = litPersonal.Text & getSidebarData3("Phone", drv.Item("phone").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("Education Level", drv.Item("EducationLevel").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("High School Grad Year", drv.Item("HighSchoolGradYear").ToString())
        litPersonal.Text = litPersonal.Text & getSidebarData3("Degree Start Timeframe", drv.Item("DegreeStartTimeframe").ToString())

        'Contact
        litPersonal.Text = litPersonal.Text & getSidebarData3("Address", drv.Item("address").ToString())
        If drv.Item("State").ToString() <> "" Then
            litPersonal.Text = litPersonal.Text & getSidebarData3("", drv.Item("City").ToString() & ", " & drv.Item("State").ToString() & " " & drv.Item("Zip").ToString())
        End If
        litPersonal.Text = litPersonal.Text & getSidebarData3("Call Date", CDate(drv.Item("call_date")).ToString("MM/dd/yyyy"))
        'litPersonal.Text = litPersonal.Text & getSidebarData3("DNIS", drv.Item("DNIS").ToString())

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


        prefs_dt = GetTable("select * from form_q_scores_options where form_id = " & drv.Item("f_id").ToString & " and question_id in (select id from questions where template='Preferences')")

        For Each prefs_dr In prefs_dt.Rows
            getSidebarData(prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_pos").ToString())

        Next





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

                If hdnCallLength.Value > 0 And e.Item.DataItem("q_position").ToString <> "" Then
                    position = CInt(e.Item.DataItem("q_position") / hdnCallLength.Value * 100)
                End If

                Dim ddlAnswers As DropDownList = CType(e.Item.FindControl("ddlAnswers"), DropDownList)

                'SetddlAnswers(ddlAnswers)

                'ddlAnswers.SelectedValue = e.Item.DataItem("answer_text").ToString()

                'ddlAnswers.Attributes.Add("onchange", String.Format("process(this, '{0}','{1}', '{2}');", e.Item.DataItem("ID").ToString(), User.Identity.Name, e.Item.DataItem("form_id").ToString())) 'HttpContext.Current.User.Identity.Name
                ddlAnswers.Attributes.Add("onchange", "updateTime(this);")

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

    Protected Sub ddlAnswer_DataBinding(sender As Object, e As EventArgs)
        For Each li As ListItem In sender.items
            If li.Text = "Yes" Then li.Attributes.CssStyle.Item("color") = "Green"
            If li.Text = "No" Then li.Attributes.CssStyle.Item("color") = "Red"
        Next
    End Sub

End Class