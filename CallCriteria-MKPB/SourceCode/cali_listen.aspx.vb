Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net

Partial Class listen
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""

    Public time_left As String = ""

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



        For Each ri As Object In Request.Form
            'Response.Write(ri.ToString & "<br>")
            If ri.ToString.IndexOf("hdnQID") > 0 Then
                'Check for RBL version of that QID
                Dim QID As String = Request(ri.ToString).ToString
                Dim pos As String = Request(ri.ToString.Replace("hdnQID", "hdnQTimestamp")).ToString
                Dim QAns As String = Request(ri.ToString.Replace("hdnQID", "hdnQAnswer")).ToString

                Response.Write("*" & QID & " " & pos & " " & QAns & "*<br>")
                If QAns <> "" Then

                    'UpdateTable("insert into form_q_scores (q_position, question_id, form_id, question_result, question_answered) values('" & pos & "'," & Request(ri.ToString) & "," & new_ID & ",0,(select id from question_answers where question_id = '" & Request(ri.ToString) & "' and answer_text = '" & QAns & "'))")
                    'Try

                    If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
                        UpdateTable("insert into calibration_scores_client(question_id,form_id,question_result,q_pos) select " & Request(ri.ToString) & "," & new_ID & "," & QAns & ",'" & pos & "'")
                    Else
                        UpdateTable("insert into calibration_scores(question_id,form_id,question_result,q_pos) select " & Request(ri.ToString) & "," & new_ID & "," & QAns & ",'" & pos & "'")
                    End If


                    'Catch ex As Exception
                    '    UpdateTable("insert into calibration_scores(question_id,form_id,question_result) select " & Request(ri.ToString) & "," & new_ID & "," & QAns)
                    '    Email_Error("insert into calibration_scores(question_id,form_id,question_result,q_pos) select " & Request(ri.ToString) & "," & new_ID & ",(select id from question_answers where question_id = '" & Request(ri.ToString) & "' and answer_text = '" & QAns & ",'" & pos & "')")
                    'End Try


                End If



            End If

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
                Response.Redirect("cali_listen.aspx?week_ending=" & Request("week_ending") & "&appname=" & Request("appname"))
            Else
                Response.Redirect("cali_listen.aspx?appname=" & Request("appname"))
            End If

        End If



    End Sub

    Public data_rate As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'If Session("appname") Is Nothing Then
        '    Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        '    Session("appname") = domain(0)
        'End If


        Response.Redirect("calibrate_record.aspx?appname=" & Request("appname"))

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
        Else
            data_rate = 0.05
        End If


        If User.Identity.Name = "" Then
            Response.Redirect("login.aspx?ReturnURL=cali_listen.aspx")
        End If




        If Not IsPostBack Then

            'release those that have been started, but not finished within 45 mins
            UpdateTable("update calibration_pending set date_started = null where date_started < dateadd(s, -45*60, dbo.getMTDate()) and date_completed is null")


            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "listen collapsed-menu")

            Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

            litUserName.Text = User.Identity.Name
            litUserEmail.Text = mu.Email


            Dim week_ending As String = DateAdd(DateInterval.Day, -1 - Today.DayOfWeek, Today).ToShortDateString

            time_left = DateDiff(DateInterval.Second, Now, DateAdd(DateInterval.Day, 7 - Today.DayOfWeek, Today))

            Dim dt_next As DataTable = GetTable("select top 1 * from calibration_pending where reviewer <> '" & User.Identity.Name & "' and appname = '" & Request("appname") & "' and date_completed is null  and (( date_started is null) or (date_started < dateadd(minute, -45, dbo.getMTDate()))) order by week_ending") ' and week_ending = '" & week_ending & "'


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

            litLeft.Text = left_dt.Rows(0).Item(0) & " left"


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
                'litContact.Text = litContact.Text & getSidebarData("Phone", dt2.Rows(0).Item("Phone").ToString())

                'School



                Dim sch_dt As DataTable = GetTable("select * from School_X_Data where xcc_id = " & dt2.Rows(0).Item("review_ID").ToString)
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
                    sch_x += 1
                Next


                'Other
                litOther.Text = litOther.Text & getSidebarData("CAMPAIGN", dt2.Rows(0).Item("CAMPAIGN").ToString())
                litOther.Text = litOther.Text & getSidebarData("DATE", dt2.Rows(0).Item("call_date").ToString())

                hdnXCCID.Value = dt2.Rows(0).Item("review_ID").ToString
                'lblArea1.Text = dt2.Rows(0).Item("program").ToString
                'lblArea2.Text = dt2.Rows(0).Item("ID").ToString

                'UpdateTable("insert into session_viewed (agent, date_viewed, session_id) select '" & User.Identity.Name & "',dbo.getMTDate(), " & dt2.Rows(0).Item("ID").ToString)



                Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
                lblSession.Text = session_id.ToString

                Dim this_filename As String = GetAudioFileName(dt2.Rows(0))

                audio_file = this_filename

                'Response.Write("good")
                'Response.End()




            End If


        End If


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

    Protected Sub btnSkip_Click(sender As Object, e As EventArgs) Handles btnSkip.Click

        UpdateTable("update calibration_pending set date_completed = dbo.getMTDate(), skip_reason='" & ddlSkip.SelectedValue & "', skipped = 1 where id = '" & hdnThisID.Value & "'")
        Email_Error("Calibration " & hdnThisID.Value & " skipped because of " & ddlSkip.SelectedValue, "tracy@callcriteria.com")
        Response.Redirect("calibration_listen.aspx")
    End Sub

    Protected Sub ddlAnswer_DataBinding(sender As Object, e As EventArgs)
        For Each li As ListItem In sender.items
            If li.Text = "Yes" Then li.Attributes.CssStyle.Item("color") = "Green"
            If li.Text = "No" Then li.Attributes.CssStyle.Item("color") = "Red"
        Next
    End Sub

    Protected Sub rptSections_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles rptSections.ItemCreated

        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            Dim ds As SqlDataSource = e.Item.FindControl("dsQuestions")
            ds.SelectCommand = "SELECT * FROM [Questions]  join sections on sections.id  = q.section  where q.section=@section and q.id in (select distinct question_id from form_q_scores where form_id = (select form_id from cali_pending_client  where id = @form_id)) order by q_order"

        End If


    End Sub
End Class

