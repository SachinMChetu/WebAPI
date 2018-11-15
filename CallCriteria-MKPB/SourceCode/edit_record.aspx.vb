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

    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateQAComments(ByVal id As String, check_list As String, id_list As String, optional_text As String, website_link As String) As String
        Dim resp As String = ""

        'UpdateTable("delete from form_q_responses where id in (select form_q_responses.id from form_q_responses join form_q_scores on form_q_scores.form_id = form_q_responses.form_id and form_q_scores.question_id = form_q_responses.question_id  where form_q_scores.id = " & id & " and answer_id <> 0 )")


        Dim check_index As Integer = 0

        Dim checks() As String = check_list.Split("|")
        Dim existing() As String = id_list.Split("|")

        For Each check_item In checks

            Try
                Dim check_data() As String = check_item.Split(":")


                If check_data.Length > 1 Then

                    If existing(check_index) <> "undefined" And check_data(0) <> "0" And existing(check_index) <> "0" Then
                        UpdateTable("delete from form_q_responses where id = " & existing(check_index))
                    End If

                    If check_data(1) = "false" And check_data(0) = "0" And existing(check_index) <> "undefined" And existing(check_index) <> "0" Then
                        UpdateTable("delete from form_q_responses where id = " & existing(check_index))
                    End If


                    If check_data(1) = "true" And check_data(0) <> "0" And check_data(0) <> "-2" Then
                        UpdateTable("insert into form_q_responses(question_id, form_id, answer_id) select question_id, form_id," & check_data(0) & " from form_q_scores with (nolock) where id = " & id)
                    End If

                    'If new_comment <> "" And new_comment <> "0" Then
                    '   Updatetable ("insert into form_q_responses(question_id, form_id, answer_id) select question_id, form_id," & new_comment & " from form_q_scores where id = " & id)
                    'End If

                    If check_data(1) = "true" And check_data(0) = "0" And existing(check_index) = "-1" Then
                        UpdateTable("insert into form_q_responses(question_id, form_id, answer_id, other_answer_text) select question_id, form_id," & check_data(0) & ", '" & optional_text.Replace("'", "''") & "' from form_q_scores with (nolock)  where id = " & id)
                    End If

                    If check_data(0) = "-2" And website_link <> "undefined" Then
                        UpdateTable("update form_q_scores set view_link = '" & website_link & "' where id = " & id)
                    End If





                End If
            Catch ex As Exception

            End Try


            check_index += 1
        Next

        Dim reload_dt As DataTable = GetTable("select isnull(comment, other_answer_text) as comment from form_q_responses with (nolock)  join answer_comments  with (nolock) on form_q_responses.answer_id = answer_comments.id where form_q_responses.id in (select form_q_responses.id from form_q_responses with (nolock)  join form_q_scores with (nolock)  on form_q_scores.form_id = form_q_responses.form_id and form_q_scores.question_id = form_q_responses.question_id where form_q_scores.id = " & id & ")")
        For Each dr In reload_dt.Rows
            resp &= dr("comment") & " "
        Next

        If optional_text <> "" Then
            resp &= optional_text
        End If


        Dim this_call_dt As DataTable = GetTable("select ni_scorecard from vwForm with (nolock)  join scorecards with (nolock)  on scorecards.id = scorecard where f_id =  " & id)


        Try
            If this_call_dt.Rows(0).Item(0).ToString = "1" Then
                UpdateTable("exec updateNIPassFail " & id)
            End If
        Catch ex As Exception

        End Try




        Return resp

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetAnswerOptions(ByVal id As String) As String
        Dim resp As String = ""
        Dim page_url As String = HttpContext.Current.Request.ServerVariables("http_referer").ToString
        Dim f_id As String = page_url.Substring(page_url.IndexOf("=") + 1)

        'If QID/form_ID mismatch, the ID was from calibration -- go get the real form_id and swap it out.
        Dim cal_test_dt As DataTable = GetTable("exec cleanQIDEdit " & f_id & "," & id)
        id = cal_test_dt.Rows(0).Item(0)


        Dim answers_dt As DataTable = GetTable("select *, isnull(existing_id,0) as exist_id, isnull(id,answer_id) as theID, isnull(comment,other_answer_text) as comment_text, case when b.answer_id is not null then ' checked ' else '' end as check_val from " &
            "(select answer_comments.id, comment from answer_comments with (nolock)  join form_q_scores with (nolock)  on form_q_scores.question_answered = answer_id where form_q_scores.id = " & id & ") a " &
            "full outer join (select form_q_responses.answer_id,  form_q_responses.id as existing_id, form_q_responses.other_answer_text from form_q_responses with (nolock)  join form_q_scores with (nolock)  on form_q_scores.form_id = form_q_responses.form_id and form_q_scores.question_id = form_q_responses.question_id   " &
            "where form_q_scores.id =" & id & ") b  " &
            "on a.id = b.answer_id ")

        For Each dr As DataRow In answers_dt.Rows
            resp &= "<input type ='checkbox' data-existingid='" & dr("exist_id") & "' value='" & dr("theID") & "' " & dr("check_val") & " > " & dr("comment_text") & "<br>"
        Next


        resp &= "<input type ='checkbox' id='chkOther' value='0' data-existingid='-1'> <input type='text' placeholder='Other comment...' id='other-text-comment' onkeyup='$(""#chkOther"").prop(""checked"", true);' ><br>"

        Dim check_web_dt As DataTable = GetTable("select website, view_link from form_q_scores with (nolock)  join vwForm with (nolock)  on vwForm.f_id = form_q_scores.form_id where form_q_scores.id = " & id)

        If check_web_dt.Rows.Count > 0 Then
            If check_web_dt.Rows(0).Item(0).ToString <> "" Then
                'Has website, allow for web checkbox
                Dim checked As String = ""

                If check_web_dt.Rows(0).Item(1).ToString <> "" Then
                    checked = "checked"
                End If
                resp &= "<input type ='checkbox' id'chkWebLink' " & checked & " value='-2' data-existingid='-2'> <input type='text' placeholder='Website Link...' id='website-link'  onkeyup='$(""#chkWebLink"").prop(""checked"", true);'  value='" & check_web_dt.Rows(0).Item(1).ToString & "'> <br>"
            End If
        End If

        Return resp

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateQAAnswers(ByVal id As Integer, ByVal qid As Integer, ByVal name As String, ByVal answer As String) As UpdateAnswerResponse

        Dim uar As New UpdateAnswerResponse


        'If QID/form_ID mismatch, the ID was from calibration -- go get the real form_id and swap it out.
        Dim cal_test_dt As DataTable = GetTable("exec cleanQIDEdit " & id & "," & qid)

        Try
            qid = cal_test_dt.Rows(0).Item(0)

            Dim msg As String = "Successfully updated QA response."
            Try

                Dim Sql As String
                If Not HttpContext.Current.User.IsInRole("QA") Then
                    UpdateTable("INSERT INTO [dbo].[form_q_score_changes]([form_id],[question_id],[changed_by],[changed_date],[approved]," &
                     "[approved_by],[approved_date],[new_value]) Select " & id & ",(Select question_id from form_q_scores where id = " & qid & "),'" & name & "',dbo.getMTDate()," &
                     "1,'" & name & "',dbo.getMTDate(),(select id from question_answers with (nolock)  where question_id = (select question_id from form_q_scores with (nolock)  where id = " & qid & ") and answer_text = '" & answer & "')")

                    Sql = "insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" & name & "', dbo.getMTDate(), '<strong>' + (select q_short_name from questions where id = (select question_id from form_q_scores with (nolock)  where id = " & qid & "))   +  '</strong> changed to <strong>' + '" & answer & "</strong>'," & id & ", 'Call'"
                    'Error when executing this query
                    UpdateTable(Sql)
                End If

                Dim ans_dt As DataTable = GetTable("select * from form_q_scores where id = " & qid)
                If ans_dt.Rows.Count > 0 Then
                    UpdateTable("update form_q_scores set question_answered =  (select id from question_answers with (nolock)  where question_id = (select question_id from form_q_scores where id = " & qid & ") and answer_text = '" & answer & "') where id = " & qid)
                    If HttpContext.Current.User.IsInRole("QA") Or HttpContext.Current.User.IsInRole("Admin") Then 'Or HttpContext.Current.User.IsInRole("QA Lead") Or HttpContext.Current.User.IsInRole("Calibrator")  Then
                        UpdateTable("update form_q_scores set original_question_answered = question_answered  where id  = " & qid)
                    End If
                Else
                    UpdateTable("INSERT INTO [dbo].[form_q_scores]([form_id], [question_id], question_answered) Select " & id & ",(Select question_id from form_q_scores with (nolock)  where id = " & qid & "),(Select id from question_answers with (nolock)  where question_id = (Select question_id from form_q_scores where id = " & qid & ") And answer_text = '" & answer & "')")
                End If


                Dim sc_dt As DataTable = GetTable("select count(*) from form_notifications where form_id = (select form_id from form_q_scores with (nolock)  where id = " & qid & ") and role in ('QA Lead','Calibrator') and date_closed is null")
                If sc_dt.Rows.Count = 0 Then
                    If sc_dt.Rows(0).Item(0) = 0 And Not HttpContext.Current.User.IsInRole("QA") Then
                        UpdateTable("insert into form_notifications( [role], comment, date_created, form_id, opened_by) select 'Calibrator','Call Edited', dbo.getMTDate(), (select form_id from form_q_scores with (nolock)  where id = " & qid & "),'" & HttpContext.Current.User.Identity.Name & "'")
                    End If
                End If

                UpdateTable("postProcessQuestions " & id)
                UpdateTable("UpdateScores " & id)
                UpdateTable("UpdateMissed " & id)
                UpdateTable("CreateNotifications " & id & ",'Test'")
                UpdateTable("declare @cali_id int;select @cali_id=id from calibration_form with (nolock)  where original_form = " & id & ";exec updateCalibration @cali_id")


                If HttpContext.Current.User.IsInRole("Client") Or HttpContext.Current.User.IsInRole("Supervisor") Or HttpContext.Current.User.IsInRole("QA Lead") Or HttpContext.Current.User.IsInRole("Calibrator") Or HttpContext.Current.User.IsInRole("Admin") Or HttpContext.Current.User.IsInRole("Manager") Then
                    UpdateTable("update form_score3 set edited_score = total_score, wasedited = 1 where id = " & id)
                End If

                Dim open_dt As DataTable = GetTable("select count(*) from vwFN where f_id = " & id)
                If open_dt.Rows.Count > 0 Then
                    If open_dt.Rows(0).Item(0) = 0 Then 'has had notifications before
                        If Not HttpContext.Current.User.IsInRole("QA") Then
                            UpdateTable("insert into form_notifications(role, date_created,form_id, opened_by)  select 'Calibrator',  dbo.getMTDate(),  form_score3.id, reviewer from form_score3 with (nolock)  join app_settings with (nolock)  on app_settings.appname = form_score3.appname where form_score3.id = " & id)
                        End If
                    End If
                End If



                uar.success = True
                Dim rr_dt As DataTable = GetTable("select convert(decimal(10,1), isnull(total_score,0)) as total_score,(select q_short_name from questions with (nolock)  where id = (select question_id from form_q_scores with (nolock)  where id = " & qid & ")) as q_changed, (select right_answer from question_answers with (nolock)  where question_id = (select question_id from form_q_scores with (nolock)  where id = " & qid & ") and answer_text = '" & answer & "') as right_wrong from form_score3 where id = " & id)

                If rr_dt.Rows.Count > 0 Then
                    uar.score = rr_dt.Rows(0).Item("total_score")
                    uar.right_answer = rr_dt.Rows(0).Item("right_wrong")
                    uar.added_note = "<tr class='data-row' data-section='101' unselectable='on' style='-webkit-user-select: none;'><td class='emptyBox'></td><td class='question'><strong>" & rr_dt.Rows(0).Item("q_changed") & "</strong> changed to <strong>" & answer & "</strong></td><td class='response'>" & name & "</td><td>" & Now & "</td><td class='td-play'><button onclick='jumpPos();return false;'><div></div></button></td></tr>"
                End If



            Catch ex As Exception
                msg = "An error occured while saving QA response." + ex.Message
                Email_Error("An error occured while saving QA response." + ex.Message)
                uar.success = False
            End Try

            UpdateTable("UpdateScores " & id)
        Catch ex As Exception

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


        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx?ReturnURL=edit_record.aspx?ID=" & Request("ID"))
        End If

        download_id = Request("ID")

        If User.IsInRole("Agent") Then
            Response.Redirect(Request.ServerVariables("HTTP_REFERER"))
        End If

        'already is in calibration, don't show button
        Dim cal_dt As DataTable = GetTable("select count(*) from calibration_pending with (nolock)  where form_id = " & Request("ID"))
        If cal_dt.Rows.Count > 0 Then
            If cal_dt.Rows(0).Item(0) > 0 Then
                lbAddCalibration.Visible = False
            End If
        End If

        'already is in calibration, don't show button
        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            Dim c_cal_dt As DataTable = GetTable("select count(*) from cali_pending_client with (nolock)  where form_id = " & Request("ID"))
            If c_cal_dt.Rows.Count > 0 Then
                If c_cal_dt.Rows(0).Item(0) = 0 Then
                    lbAddCalibration.Visible = True
                End If
            End If
        End If


        'already is in notifications, don't show button
        Dim noti_dt As DataTable = GetTable("select count(*) from form_notifications with (nolock)  where date_closed is null and  form_id = " & Request("ID"))
        If noti_dt.Rows.Count > 0 Then
            If noti_dt.Rows(0).Item(0) > 0 Then
                lbAddDispute.Visible = False
            End If
        End If

        If Not User.IsInRole("Admin") Then
            lbAddCalibration.Visible = False
            lbAddDispute.Visible = False
        End If

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo with (nolock)  where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then

            If User.IsInRole("Supervisor") And user_info_dt.Rows(0).Item("non_edit").ToString.ToLower = "true" Then
                Response.Redirect(Request.ServerVariables("HTTP_REFERER"))
            End If


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

            UpdateTable("insert into session_viewed (agent, date_viewed, session_id, page_viewed) select '" & User.Identity.Name & "',dbo.getMTDate(), (select top 1 review_id from form_score3 with (nolock)  where id = " & Request("ID") & "),'review'")


            If Request("ID") IsNot Nothing Then
                hdnThisID.Value = Request("ID")
                Dim record_dt As DataTable = GetTable("select * from vwForm with (nolock)  where vwForm.F_id = " & Request("ID"))
                If record_dt.Rows.Count > 0 Then
                    hdnCallLength.Value = record_dt.Rows(0).Item("call_length").ToString
                    hdnThisAgent.Value = record_dt.Rows(0).Item("Agent").ToString
                Else
                    Response.Redirect("default.aspx")

                End If
            End If

            Try
                UpdateTable("insert into FormViewed (who_viewed, form_id, date_viewed) select '" & User.Identity.Name & "'," & Request("ID") & ", dbo.getMTDate()")
            Catch ex As Exception

            End Try




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



        If drv.Item("website").ToString <> "" Then
            pnlCall.Visible = False
            pnlWeb.Visible = True
            litWebsite.Text = "<iframe src='" & drv.Item("website").ToString & "' width='900px' height='500px'></iframe>"
            hdnIsWebsite.Value = "1"
        End If




        litUserName.Text = drv.Item("agent").ToString()


        'If Not User.Identity.IsAuthenticated And Request("agent") IsNot Nothing Then
        '    fvFORMData.FindControl("NotificationControl").Visible = False
        'End If


        'Dim pfl As ProfileCommon = Profile.GetProfile(drv.Item("reviewer").ToString())
        'If pfl IsNot Nothing Then
        '    If pfl.Avatar <> "" Then
        '        ' imgAvatar.ImageUrl = "/audio/" & drv.Item("reviewer").ToString() & "/" & pfl.Avatar
        '    End If
        'End If


        'Dim agentdt As DataTable = GetTable("select min(call_date) from xcc_report_new with (nolock)  where call_date > dateadd(d,-30,dbo.getMTDate()) and agent = '" & drv.Item("agent").ToString() & "'")
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


        If IsDBNull(drv.Item("first_name")) Or (drv.Item("rev_first").ToString & " " & drv.Item("rev_last").ToString = " ") Then
            litQAName.Text = drv.Item("reviewer").ToString
        Else

            litQAName.Text = drv.Item("rev_first").ToString & " " & drv.Item("rev_last").ToString
        End If


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


        Dim prefs_dt As DataTable = GetTable("select dbo.getTemplateText2(f_id, questions.id) as pref_data, * from vwForm with (nolock)  join questions with (nolock)  on questions.appname = vwForm.appname  where template='Preferences' and f_id = " & drv.Item("f_id").ToString)


        For Each prefs_dr In prefs_dt.Rows
            litPrefs.Text = litPrefs.Text & getSidebarData(prefs_dr.Item("q_short_name").ToString(), prefs_dr.Item("pref_data").ToString())

        Next


        prefs_dt = GetTable("select * from form_q_scores_options with (nolock)  where form_id = " & drv.Item("f_id").ToString & " and question_id in (select id from questions with (nolock)  where template='Preferences')")

        For Each prefs_dr In prefs_dt.Rows
            getSidebarData(prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_value").ToString(), prefs_dr.Item("option_pos").ToString())

        Next


        'lblSession.Text = drv.Item("Session_ID").ToString().Replace("()", "")

        'Other
        litOther.Text = litOther.Text & getSidebarData3("CAMPAIGN", drv.Item("CAMPAIGN").ToString())
        litOther.Text = litOther.Text & getSidebarData3("DATE", drv.Item("call_date").ToString())
        litOther.Text = litOther.Text & getSidebarData3("Session", drv.Item("Session_ID").ToString())
        litOther.Text = litOther.Text & getSidebarData3("Profile ID", drv.Item("profile_id").ToString())

        Dim history_dt As DataTable = GetTable("select * from session_viewed with (nolock)  where session_id = '" & drv.Item("review_id").ToString() & "' order by date_viewed ")
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


        litScore.Text = drv.Item("real_score").ToString

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

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim ddlAnswers As DropDownList = CType(e.Item.FindControl("ddlAnswers"), DropDownList)

            ddlAnswers.Attributes.Add("onchange", String.Format("process(this, '{0}','{1}', '{2}');", e.Item.DataItem("FS_ID").ToString(), User.Identity.Name, e.Item.DataItem("f_id").ToString())) 'HttpContext.Current.User.Identity.Name
            If hdnCallLength.Value <> "" Then

                'Add div for audio if the answer is right or wrong 
                Dim position As Integer = 0

                If hdnCallLength.Value > 0 And e.Item.DataItem("q_position").ToString <> "" Then
                    position = CInt(e.Item.DataItem("q_position") / hdnCallLength.Value * 100)
                End If

                If e.Item.DataItem("bad-response").ToString = "" Then
                    litSliderPoints.Text &= " <div class='warning-part' style='left: " & position & "%;' title='" & e.Item.DataItem("q_short_name") & "'><span></span><a href='#' class='listen-from-here' style='background: none repeat scroll 0% 0% rgb(154, 190, 46);'>&#x2714</a></div>" & Chr(13)
                Else
                    litSliderPoints.Text &= " <div class='warning-part' style='left: " & position & "%;' title='" & e.Item.DataItem("q_short_name") & "'><span></span><a href='#' class='listen-from-here'>!</a></div>" & Chr(13)
                End If



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
        'Dim cal_dt As DataTable = GetTable("select top 1 * from calibration_pending where review_type = 'random 4%' order by id desc")
        'If cal_dt.Rows.Count > 0 Then
        ' UpdateTable("delete from calibration_pending where id = " & cal_dt.Rows(0).Item("ID"))
        Dim selected_by As String = ""
        If User.IsInRole("Client") Or User.IsInRole("Supervisor") Then
            selected_by = "Client"
            UpdateTable("insert into cali_pending_client (bad_value, form_id, reviewer, review_type, week_ending, appname, assigned_to) select '" & selected_by & " Selected','" & hdnThisID.Value & "',(select reviewer from form_score3 with (nolock)  where id = " & hdnThisID.Value & "),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" & hdnThisApp.Value & "', userapps.username from userextrainfo with (nolock)  join userapps with (nolock)  on userextrainfo.username = userapps.username where user_role in ('Supervisor','Client') and appname = '" & hdnThisApp.Value & "'")
        End If
        If User.IsInRole("Admin") Then
            selected_by = "Admin"
        End If

        Dim cal_dt As DataTable = GetTable("select count(*) from calibration_pending where form_id = " & Request("ID"))

        If cal_dt.Rows.Count > 0 Then
            If cal_dt.Rows(0).Item(0).ToString = "0" Then
                UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname) select '" & selected_by & " Selected','" & hdnThisID.Value & "',(select reviewer from form_score3 where id = " & hdnThisID.Value & "),'" & selected_by & " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'" & hdnThisApp.Value & "'")
            End If
        End If

        'Response.Write("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending) select 'Admin Selected','" & hdnThisID.Value & "',(select reviewer from form_score3 where id = " & hdnThisID.Value & "),'Admin Selected','" & cal_dt.Rows(0).Item("week_ending") & "'")
        'Response.End()
        lbAddCalibration.Visible = False
        'End If
    End Sub

    Protected Sub lbAddDispute_Click(sender As Object, e As System.EventArgs) Handles lbAddDispute.Click
        UpdateTable("INSERT INTO [notifications] ([assigned_to], [dateadded], [acknowledged], [form_id], [ack_date], opened_by, ack_by) select 'QA',  dbo.getMTDate(), 1, " & hdnThisID.Value & ", dbo.getMTDate(), (select reviewer from form_score3 with (nolock)  where ID = " & hdnThisID.Value & "),'Client'")
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

                    position = CInt(CInt(drv.Item("click_pos")) / hdnCallLength.Value * 100)
                    litSliderPoints.Text &= "<div class='warning-part' style='left: " & CInt(CInt(drv.Item("click_pos")) / hdnCallLength.Value * 100) & "%;' title='" & drv.Item("comment_header").ToString & "'><span></span><a href='#' class='listen-from-here' style='background: none repeat scroll 0% 0% rgb(102,178,255);'>&#x2724</a></div>" & Chr(13)

                End If
            Catch ex As Exception
                Response.Write("<!--" & position & "-->")
            End Try

        End If

    End Sub



    Private Sub btnDldAudio_Click(sender As Object, e As EventArgs) Handles btnDldAudio.Click
        Response.Redirect("download_audio.aspx?ID=" & Request("ID"))
    End Sub

    Private Sub btnDldCall_Click(sender As Object, e As EventArgs) Handles btnDldCall.Click
        Response.Redirect("download_call.aspx?ID=" & Request("ID"))
    End Sub

    Private Sub btnDldAudio_Command(sender As Object, e As CommandEventArgs) Handles btnDldAudio.Command

    End Sub
End Class