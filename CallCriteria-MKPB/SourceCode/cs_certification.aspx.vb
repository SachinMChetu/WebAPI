
Imports System.Data

Partial Class cs_certification
    Inherits System.Web.UI.Page

    Private Sub gvdsCSStats_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvdsCSStats.RowCommand


        Dim username As String = gvdsCSStats.Rows(e.CommandArgument).Cells(2).Text.Trim()
        Dim scorecard As String = gvdsCSStats.Rows(e.CommandArgument).Cells(12).Text.Trim()


        If e.CommandName = "certify" Then
            Dim sql As String = "update userapps set scorecard_role = 'QA' where user_scorecard = " & scorecard & " and username ='" & username & "' and scorecard_role = 'Trainee'; "
            'sql &= "delete from userapps where username = '" & username & "' and user_scorecard = " & scorecard & " ; "
            'sql &= "insert into userapps(appname, username, user_scorecard, scorecard_role) select 'CallSource','" & username & "', " & scorecard & " , 'QA';"
            sql &= "insert into sc_training_approvals (username, sc_id, sc_date, sc_by) select '" & username.Replace("'", "''") & "','" & scorecard & " ',dbo.getMTDate(), '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "'; exec dedupeTrainingApprovals;"

            Common.UpdateTable(sql)
            gvdsCSStats.DataBind()

        End If

        If e.CommandName = "recertify" Then
            Dim sql As String = "delete from form_score_training where id in (select id from vwTrain where reviewer = '" & username & "' and scorecard = " & scorecard & ")"

            Common.UpdateTable(sql)
            gvdsCSStats.DataBind()

        End If


        If e.CommandName = "questions" Then
            Dim sql As String = "select * from form_score_training where reviewer ='" & username & "' and trainee_score = 55  and appname = 'Callsource' order by review_date desc "


            Dim dt As DataTable = Common.GetTable(sql)
            gvCSDetails.DataSource = dt
            gvCSDetails.DataBind()

        End If


        If e.CommandName = "all" Then
            Dim sql As String = "select * from form_score_training where reviewer ='" & username & "'  and appname = 'Callsource' order by review_date desc "


            Dim dt As DataTable = Common.GetTable(sql)
            gvCSDetails.DataSource = dt
            gvCSDetails.DataBind()

        End If

        If e.CommandName = "comments" Then
            Dim sql As String = "select * from form_score_training where reviewer ='" & username & "' and trainee_score = 80  and appname = 'Callsource'  order by review_date desc"

            Dim dt As DataTable = Common.GetTable(sql)
            gvCSDetails.DataSource = dt
            gvCSDetails.DataBind()

        End If


    End Sub

    Private Sub cs_certification_Load(sender As Object, e As EventArgs) Handles Me.Load
        'QAL TL Client AM
        If User.IsInRole("QALead") Or User.IsInRole("Tango TL") Or User.IsInRole("Admin") Or User.IsInRole("Account Manager") Then
        Else
            Response.Redirect("login.aspx?ReturnURL=cs_certification.aspx")
        End If



    End Sub
End Class
