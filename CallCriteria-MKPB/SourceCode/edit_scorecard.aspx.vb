
Partial Class edit_scorecard
    Inherits System.Web.UI.Page

    Private Sub edit_scorecard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=edit_scorecard.aspx")
        End If

        If Not IsPostBack Then
            If User.IsInRole("Admin") Then
                dsMyApps.SelectCommand = "Select appname from app_settings order by appname"
            End If
        End If

        dsMyApps.SelectParameters("username").DefaultValue = User.Identity.Name
        dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Private Sub ddlSC_DataBound(sender As Object, e As EventArgs) Handles ddlSC.DataBound

        If ddlSC.SelectedValue <> "" Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If

        If Not IsPostBack Then
            Try
                ddlSC.SelectedValue = Request("ID")
                'dsNotifications.DataBind()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        dsNotes.Insert()
        gvNotes.DataBind()
    End Sub

    Protected Sub btnAddEmail_Click(sender As Object, e As EventArgs)
        Dim dsEmails As SqlDataSource = sender.parent.findcontrol("dsEmails")
        Dim gvEmails As GridView = sender.parent.findcontrol("gvEmails")
        dsEmails.Insert()
        gvEmails.DataBind()
    End Sub



    Private Sub btnDedupe_Click(sender As Object, e As EventArgs) Handles btnDedupe.Click
        Common.UpdateTable("exec dedupe_session_sc " & ddlSC.SelectedValue)
        lblDedupe.Text = "Deduped."
    End Sub

    Private Sub ddlSC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSC.SelectedIndexChanged
        lblDedupe.Text = ""
        gvChanges.DataBind()
    End Sub

    Private Sub FormView1_ItemUpdating(sender As Object, e As FormViewUpdateEventArgs) Handles FormView1.ItemUpdating
        For x = 0 To e.NewValues.Count - 1

            Try

                If e.OldValues(x) <> e.NewValues(x) Then
                    'Response.Write("insert into scorecard_changes (scorecard, change, changed_by, changed_date) select '" & ddlSC.SelectedValue & "','" & e.NewValues.Keys(x) & " changed from " & e.OldValues(x) & " to " & e.NewValues(x) & "','" & User.Identity.Name & "',dbo.GetMTdate()<br>")
                    Common.UpdateTable("insert into scorecard_changes (scorecard, change, changed_by, changed_date) select '" & ddlSC.SelectedValue & "','" & e.NewValues.Keys(x) & " changed from " & e.OldValues(x) & " to " & e.NewValues(x) & "','" & User.Identity.Name & "',dbo.GetMTdate()")
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

        Next
        'Response.End()
    End Sub
End Class
