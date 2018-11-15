Imports System.Data
Imports Common

Partial Class edit_scorecard
    Inherits System.Web.UI.Page

    Private Sub edit_scorecard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'dsMyApps.SelectParameters("username").DefaultValue = User.Identity.Name
        dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Private Sub ddlSC_DataBound(sender As Object, e As EventArgs) Handles ddlSC.DataBound

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=edit_notifications.aspx")

        End If

        If Not IsPostBack Then
            Try
                ddlSC.SelectedValue = Request("ID")
                dsNotifications.DataBind()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        dsNotifications.Insert()
        gvNotifications.DataBind()
    End Sub

    Private Sub btnNewProfile_Click(sender As Object, e As EventArgs) Handles btnNewProfile.Click
        UpdateTable("cloneNotificationFlow '" & txtNewProfile.Text & "'," & ddlFrom.SelectedValue)
        Dim dt As DataTable = GetTable("select max(id) from notification_profiles")
        Dim new_ID As String = dt.Rows(0).Item(0)

        ddlSC.Items.Clear()
        dsApps.DataBind()
        ddlSC.DataBind()
        Try
            ddlSC.SelectedValue = new_ID
            dsNotifications.DataBind()
        Catch ex As Exception

        End Try

        txtNewProfile.Text = ""
        'ddlFrom.SelectedValue = ""

    End Sub

    Private Sub ddlSC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSC.SelectedIndexChanged

        Dim dt As DataTable = GetTable("select * from notification_profiles where id = " & ddlSC.SelectedValue)

        If dt.Rows.Count > 0 Then
            txtProfile.Text = dt.Rows(0).Item("profile_description").ToString
        End If

        gvNotifications.DataBind()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateTable("update notification_profiles set profile_description = '" & txtProfile.Text.Replace("'", "''") & "' where id = " & ddlSC.SelectedValue)
    End Sub
End Class
