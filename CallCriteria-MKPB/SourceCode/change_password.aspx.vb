Imports System.Data
Imports Common
Partial Class change_password
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Roles.IsUserInRole("Admin") Then
            Response.Redirect("default.aspx")
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim mu As MembershipUser = Membership.GetUser(TextBox1.Text)
        mu = Membership.GetUser(TextBox1.Text, False)
        If mu Is Nothing Then
            Label1.Text = "User does not exist"
            Exit Sub
        End If
        mu.ChangePassword(mu.ResetPassword(), TextBox2.Text)
        Label1.Text = "Changed"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        UpdateTable("exec changeUsername '" & txtOldUser.Text & "','" & txtNewUser.Text & "'")
        Label2.Text = "Changed"
    End Sub
    'Private Sub btnGo_Click2(sender As Object, e As EventArgs) 'Handles btnGo.Click

    '    Dim num_reset As Integer = 0
    '    'Response.Write("select distinct userapps.username from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard = '" & ddlApps.SelectedValue & "' and user_role = '" & ddlRole.SelectedValue & "'")
    '    Dim dt As DataTable = GetTable("select * from dmsuser1")
    '    For Each dr In dt.Rows

    '        'Response.Write(dr("username"))
    '        Try
    '            Dim mu As MembershipUser = Membership.GetUser(dr("username").ToString)
    '            mu = Membership.GetUser(dr("username").ToString, False)
    '            mu.ChangePassword(mu.ResetPassword(), dr("password").ToString)
    '            num_reset += 1

    '            Response.Write("changing " & dr("username").ToString & " to " & dr("password").ToString & "<br>")

    '        Catch ex As Exception

    '        End Try

    '    Next

    '    lblReset.Text = num_reset & " reset."
    '    Response.End()
    'End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click

        Dim num_reset As Integer = 0
        'Response.Write("select distinct userapps.username from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard = '" & ddlApps.SelectedValue & "' and user_role = '" & ddlRole.SelectedValue & "'")
        Dim dt As DataTable = GetTable("select distinct userapps.username from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard = '" & ddlApps.SelectedValue & "' and user_role = '" & ddlRole.SelectedValue & "'")
        For Each dr In dt.Rows

            'Response.Write(dr("username"))

            Dim mu As MembershipUser = Membership.GetUser(dr("username").ToString)
            mu = Membership.GetUser(dr("username").ToString, False)
            mu.ChangePassword(mu.ResetPassword(), txtNewPassword.Text)
            num_reset += 1

            Response.Write("changing " & dr("username").ToString & " to " & txtNewPassword.Text & "<br>")

        Next

        lblReset.Text = num_reset & " reset."
        Response.End()
    End Sub

    Private Sub btnChangeApp_Click(sender As Object, e As EventArgs) Handles btnChangeApp.Click
        Dim num_reset As Integer = 0
        'Response.Write("select distinct userapps.username from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard = '" & ddlApps.SelectedValue & "' and user_role = '" & ddlRole.SelectedValue & "'")
        Dim dt As DataTable = GetTable("select distinct userapps.username from userapps join userextrainfo on userextrainfo.username = userapps.username where appname = '" & ddlAppnames.SelectedValue & "' and user_role in ('Client','Supervisor','Manager','Agent','Partner')")
        For Each dr In dt.Rows

            'Response.Write(dr("username"))
            Try
                Dim mu As MembershipUser = Membership.GetUser(dr("username").ToString)
                mu = Membership.GetUser(dr("username").ToString, False)
                mu.ChangePassword(mu.ResetPassword(), txtAppPass.Text)
                num_reset += 1

                Response.Write("changing " & dr("username").ToString & " to " & txtNewPassword.Text & "<br>")

            Catch ex As Exception
                Response.Write(ex.Message & " : " & dr("username").ToString & "<br>")
            End Try

        Next

        lblAppResult.Text = num_reset & " reset."
        'Response.End()
    End Sub
End Class
