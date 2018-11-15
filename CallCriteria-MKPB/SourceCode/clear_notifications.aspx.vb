Imports Common
Imports System.Data.SqlClient
Imports System.Data


Partial Class clear_notifications
    Inherits System.Web.UI.Page

    Private Sub btnAddCC_Click(sender As Object, e As EventArgs) Handles btnAddCC.Click

        For Each gvr As GridViewRow In gvCallList.Rows
            If gvr.RowType = DataControlRowType.DataRow Then



                Dim chk As CheckBox = gvr.FindControl("chkUserThis")

                If chk.Checked Then

                    Dim hdnThisID As New HiddenField
                    hdnThisID.Value = gvCallList.DataKeys(gvr.RowIndex).Value

                    Dim selected_by As String = ""
                    selected_by = "Admin"
                    UpdateTable("insert into cali_pending_client (bad_value, form_id, reviewer, review_type, week_ending, appname, assigned_to) select '" & selected_by & " Selected','" & hdnThisID.Value & "',(select reviewer from form_score3 where id = " & hdnThisID.Value & "),'Client Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'(select appname from vwform where f_id = " & hdnThisID.Value & ")', userapps.username from userextrainfo join userapps on userextrainfo.username = userapps.username where user_role in ('Supervisor','Client','Manager') and user_scorecard = (select scorecard from vwForm where f_id = '" & hdnThisID.Value & "')")

                    Dim cal_dt As DataTable = GetTable("select count(*) from calibration_pending where form_id = " & hdnThisID.Value)

                    If cal_dt.Rows.Count > 0 Then
                        If cal_dt.Rows(0).Item(0).ToString = "0" Then
                            UpdateTable("insert into calibration_pending (bad_value, form_id, reviewer, review_type, week_ending, appname) select '" & selected_by & " Selected','" & hdnThisID.Value & "',(select reviewer from form_score3 where id = " & hdnThisID.Value & "),'" & selected_by & " Selected',convert(date, dateadd(d, 7 - datepart(weekday, dbo.getMTDate()), dbo.getMTDate())),'(select appname from vwform where f_id = " & hdnThisID.Value & ")'")
                        End If
                    End If

                End If
            End If

        Next


        lblAddCC.Text = "Updated."

        gvCallList.DataBind()

        'End If


    End Sub

    Private Sub btnAddQACal_Click(sender As Object, e As EventArgs) Handles btnAddQACal.Click
        UpdateTable("exec addCalibrationsFromQA '" & ddlQA.SelectedValue & "',  " & ddlSCQAC.SelectedValue & ", " & txtQAAdd.Text)
        lblAddQACal.Text = "Added."
    End Sub

    Private Sub btnClientDelete_Click(sender As Object, e As EventArgs) Handles btnClientDelete.Click
        UpdateTable("delete from cali_pending_client where id in (select cali_pending_client.id from cali_pending_client join vwForm on vwForm.f_id = form_id where scorecard = " & ddlSCCC.SelectedValue & " and ((assigned_to = '" & ddlClient.SelectedValue & "') or (assigned_to in (select username from userextrainfo where user_role =  '" & ddlCPC.SelectedValue & "') )) and date_completed is null)")
        ddlClient_SelectedIndexChanged(ddlSCCC, e)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        UpdateTable("exec [clearNotificationsSC] " & ddlSC.SelectedValue & ",'" & ddlROle.SelectedValue & "','" & txtClearStart.Text & "','" & txtClearEnd.Text & "','" & User.Identity.Name & "'")
        ddlROle_SelectedIndexChanged(ddlROle, e)
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        gvCallList.DataBind()
    End Sub

    Private Sub btnPendingDelete_Click(sender As Object, e As EventArgs) Handles btnPendingDelete.Click
        UpdateTable("delete from calibration_pending where sc_id = " & ddlSCC.SelectedValue & "  and date_completed is null")
        ddlSCC_SelectedIndexChanged(ddlSCC, e)
    End Sub

    Private Sub clear_notifications_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.IsInRole("Admin") Then
            Response.Redirect("default.aspx")
        End If



    End Sub

    Private Sub ddlClient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlClient.SelectedIndexChanged
        lblClientRows.Text = "0"
        If ddlClient.SelectedValue <> "" Then
            Dim dt As DataTable = GetTable("select count(*) from cali_pending_client join vwForm on vwForm.f_id = form_id where scorecard = " & ddlSCCC.SelectedValue & " and assigned_to = '" & ddlClient.SelectedValue & "' and date_completed is null")
            lblClientRows.Text = dt.Rows(0).Item(0)
        End If
    End Sub



    Private Sub ddlROle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlROle.SelectedIndexChanged, ddlSC.SelectedIndexChanged
        lblRows.Text = "0"
        Dim ddl As DropDownList = sender
        If ddl.SelectedValue <> "" Then
            Dim dt As DataTable
            If txtClearStart.Text <> "" Then
                dt = GetTable("select count(*) from vwFN where scorecard = " & ddlSC.SelectedValue & " and role = '" & ddlROle.SelectedValue & "' and date_closed is null and convert(date, date_created) between '" & txtClearStart.Text & "' and '" & txtClearEnd.Text & "'")
            Else
                dt = GetTable("select count(*) from vwFN where scorecard = " & ddlSC.SelectedValue & " and role = '" & ddlROle.SelectedValue & "' and date_closed is null")
            End If
            lblRows.Text = dt.Rows(0).Item(0)
        End If


    End Sub

    Private Sub ddlSCC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSCC.SelectedIndexChanged
        lblCRows.Text = "0"
        Dim ddl As DropDownList = sender
        If ddl.SelectedValue <> "" Then
            Dim dt As DataTable = GetTable("select count(*) from calibration_pending where sc_id = " & ddlSCC.SelectedValue & "  and date_completed is null")
            lblCRows.Text = dt.Rows(0).Item(0)
        End If
    End Sub

    Private Sub ddlSCCC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSCCC.SelectedIndexChanged
        dsClientList.DataBind()

        lblClientRows.Text = "0"
        If ddlCPC.SelectedValue <> "" Then
            Dim dt As DataTable = GetTable("select count(*) from cali_pending_client join vwForm on vwForm.f_id = form_id where scorecard = " & ddlSCCC.SelectedValue & " and assigned_to in (select username from userextrainfo where user_role =  '" & ddlCPC.SelectedValue & "') and date_completed is null")
            lblClientRows.Text = dt.Rows(0).Item(0)
        End If

    End Sub

    Private Sub ddlSCQACC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSCQACC.SelectedIndexChanged
        ddlQACA.DataBind()
    End Sub

    Private Sub txtAddCC_TextChanged(sender As Object, e As EventArgs) Handles txtAddCC.TextChanged
        gvCallList.DataBind()
    End Sub

    Private Sub btnRescore_Click(sender As Object, e As EventArgs) Handles btnRescore.Click
        Dim dt As DataTable = GetTable("select f_id from vwForm where scorecard = '" & ddlRescore.SelectedValue & "' and call_date between '" & txtRescoreStart.Text & "' and '" & txtRescoreEnd.Text & "' and bad_call is null and calib_score is null")
        lblRescore.Text = dt.Rows.Count & " updated."
        For Each dr In dt.Rows
            UpdateTable("exec postprocessquestions " & dr("f_id"))
        Next
    End Sub

    Private Sub btnRescoreCali_Click(sender As Object, e As EventArgs) Handles btnRescoreCali.Click
        Dim dt As DataTable = GetTable("select id from vwCF where scorecard = '" & ddlRescoreCali.SelectedValue & "' and review_date between '" & txtRescoreStartCali.Text & "' and '" & txtRescoreEndCali.Text & "'")
        lblRescoreCali.Text = dt.Rows.Count & " updated."
        For Each dr In dt.Rows
            UpdateTable("exec postprocessquestionscalib " & dr("id") & ", 0")
        Next
    End Sub

    Private Sub txtClearEnd_TextChanged(sender As Object, e As EventArgs) Handles txtClearEnd.TextChanged, txtClearStart.TextChanged
        ddlROle_SelectedIndexChanged(ddlROle, e)

    End Sub

    Private Sub ddlCPC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCPC.SelectedIndexChanged

        lblClientRows.Text = "0"
        If ddlCPC.SelectedValue <> "" Then
            Dim dt As DataTable = GetTable("select count(*) from cali_pending_client join vwForm on vwForm.f_id = form_id where scorecard = " & ddlSCCC.SelectedValue & " and assigned_to in (select username from userextrainfo where user_role =  '" & ddlCPC.SelectedValue & "') and date_completed is null")
            lblClientRows.Text = dt.Rows(0).Item(0)
        End If
    End Sub

    Private Sub btnMarkBad_Click(sender As Object, e As EventArgs) Handles btnMarkBad.Click
        If txtRejectReason.Text = "" Then
            lblMarkBad.Text = "Reject reason cannot be blank."
            Return
        End If
        Dim dt As DataTable
        If IsDate(txtMarkBadReviewStart.Text) And IsDate(txtMarkBadReviewEnd.Text) Then
            dt = GetTable("select f_id from vwForm where scorecard = '" & ddlMarkBadScorecard.SelectedValue & "' and review_date between '" & txtMarkBadReviewStart.Text & "' and '" & txtMarkBadReviewEnd.Text & "' and bad_call is null ")
        ElseIf IsDate(txtMarkBadCallDateStart.Text) And IsDate(txtMarkBadCallDateEnd.Text) Then
            dt = GetTable("select f_id from vwForm where scorecard = '" & ddlMarkBadScorecard.SelectedValue & "' and call_date between '" & txtMarkBadCallDateStart.Text & "' and '" & txtMarkBadCallDateEnd.Text & "' and bad_call is null ")
        Else
            lblMarkBad.Text = " No valid date ranges for review or call date."
            Return
        End If
        lblMarkBad.Text = dt.Rows.Count & " marked bad."
        For Each dr In dt.Rows
            MarkCallBad2(dr("f_id"), txtRejectReason.Text)
        Next
    End Sub

    Public Sub MarkCallBad2(f_id As Integer, reject_reason As String)

        Dim dt As DataTable = GetTable("select auto_accept_bad_call from xcc_report_new join scorecards on scorecards.id = scorecard and xcc_report_new.id = (select review_id from vwForm where f_id = " & f_id & ")")

        If dt.Rows(0).Item(0).ToString = "True" Then
            UpdateTable("update xcc_report_new set bad_call = 1, bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" & HttpContext.Current.User.Identity.Name & "',  bad_call_who='" & HttpContext.Current.User.Identity.Name & "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" & reject_reason.Replace("'", "''") & "' where id = (select review_id from vwForm where f_id = " & f_id & ")")
        Else
            UpdateTable("update xcc_report_new set bad_call = 1, bad_call_who='" & HttpContext.Current.User.Identity.Name & "',  bad_call_date=dbo.getMTDate(), max_reviews=1,  bad_call_reason='" & reject_reason.Replace("'", "''") & "' where id = (select review_id from vwForm where f_id = " & f_id & ")")
        End If

        UpdateTable("update vwForm set calib_score = null,  pass_fail='N/A',display_score=null, total_score= null, original_qa_score=null, missed_list = null, formatted_comments = null, formatted_missed = null where f_id = " & f_id)
        UpdateTable("delete from calibration_form where original_form = " & f_id)
        UpdateTable("delete from calibration_pending where form_id = " & f_id & " and date_completed is null")
        UpdateTable("delete from form_notifications where form_id = " & f_id & " and date_closed is null")

    End Sub
End Class
