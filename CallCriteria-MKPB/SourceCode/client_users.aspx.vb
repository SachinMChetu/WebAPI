Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Common
Partial Class client_users
    Inherits System.Web.UI.Page


    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserActive(username As String, isActive As String)

        If username = "" Or isActive = "" Then Exit Sub

        Dim isLockedOut As Integer = 0
        Dim roleFilter As String = ""
        Dim dt As DataTable = GetTable("SELECT user_role FROM UserExtraInfo WHERE username = '" & username & "'")
        If isActive = "true" Then
            If Trim(dt.Rows(0).Item(0).ToString) = "Inactive" Then
                roleFilter = ", user_role = 'QA' "
                If Roles.GetRolesForUser(username).Count > 0 Then
                    Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username))
                End If
                Roles.AddUserToRole(username, "QA")
            End If
            isActive = 1
        Else
            If Trim(dt.Rows(0).Item(0).ToString) = "QA" Then
                roleFilter = ", user_role = 'Inactive' "
                If Roles.GetRolesForUser(username).Count > 0 Then
                    Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username))
                End If
                Roles.AddUserToRole(username, "Inactive")
            End If
            isActive = 0
            isLockedOut = 1
        End If

        Dim sql As String
        sql = "UPDATE UserExtraInfo SET active = '" & isActive & "'" & roleFilter & " WHERE username = '" & username & "'"
        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim query As New SqlCommand(sql, cn)
        query.CommandTimeout = 60
        query.ExecuteNonQuery()

        'update [aspnet_Membership] set islockedout = 0 where userid in (select userID from  [aspnet_Users] where username  = 'carlo')
        sql = "UPDATE [aspnet_Membership] SET islockedout = '" & isLockedOut & "' WHERE userid in (select userID from  [aspnet_Users] where username = '" & username & "')"
        Dim query2 As New SqlCommand(sql, cn)
        query2.CommandTimeout = 60
        query2.ExecuteNonQuery()

        If Trim(dt.Rows(0).Item(0).ToString) = "QA" And isActive = 0 Then
            sql = "DELETE FROM UserApps WHERE username = '" & username & "'"
            Dim query3 As New SqlCommand(sql, cn)
            query3.CommandTimeout = 60
            query3.ExecuteNonQuery()
        End If

        cn.Close()
        cn.Dispose()

    End Sub





    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserGroup(username As String, group As String)

        UpdateTable("update userextrainfo set user_group = '" & group & "' where username =  '" & username & "'")

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserType(username As String, type As String)

        UpdateTable("update userextrainfo set user_role = '" & type & "' where username =  '" & username & "'")

        If Roles.GetRolesForUser(username).Count > 0 Then
            Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username))
        End If
        Roles.AddUserToRole(username, type)

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserFirst(username As String, type As String)

        'Why isn't this updating?
        UpdateTable("update userextrainfo set first_name = '" & type & "' where username =  '" & username & "'")

        'send JSON data back to $('#edit-first-done').click(), line 175 of client_users.js

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserLast(username As String, type As String)

        UpdateTable("update userextrainfo set last_name = '" & type & "' where username =  '" & username & "'")

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserPassword(username As String, newpassword As String)

        If username = "" Or newpassword = "" Then Exit Sub

        Dim mu As MembershipUser = Membership.GetUser(username)
        mu = Membership.GetUser(username, False)
        If mu Is Nothing Then
            Exit Sub
        End If
        mu.ChangePassword(mu.ResetPassword(), newpassword)

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Sub UpdateUserEmail(username As String, oldemail As String, newemail As String)

        If username = "" Or newemail = "" Then Exit Sub

        Dim sql As String
        sql = "UPDATE UserExtraInfo SET email_address = '" & newemail & "' WHERE username = '" & username & "'"

        UpdateTable(sql)

        'return Json(new {
        '	newemail = newemail,
        '	sql = sql
        '});

        'Response.Clear();
        'Response.ContentType = "application/json";
        'Response.Write(sql);
        'Response.Flush();
        'Response.End();


    End Sub

    Private Sub client_users_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=client_users.aspx")
        End If

        If User.IsInRole("Agent") Or User.IsInRole("QA") Or User.IsInRole("Calibrator") Then
            Response.Redirect("default.aspx")
        End If


        If Request.QueryString.HasKeys AndAlso Request.QueryString("exist") IsNot Nothing Then
            lblNote.Text = "Username already exists."
            lblNote.ForeColor = Drawing.Color.Red
            lblNote.Font.Bold = True
        Else
            lblNote.Text = ""
        End If

        dsMyUsers.SelectParameters("username").DefaultValue = User.Identity.Name
        dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
        dsGroup.SelectParameters("username").DefaultValue = User.Identity.Name
        dsScorecards.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub

    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click

        Dim existing As DataTable = GetTable("SELECT username FROM userExtraInfo WHERE username = '" & txtUserName.Text.Replace("'", "''") & "'")
        If existing.Rows.Count > 0 Then
            Response.Redirect("client_users.aspx?exist=true")
        End If

        'Try
        Dim this_mu As MembershipUser = Membership.CreateUser(txtUserName.Text, txtPassword.Text, txtEmail.Text)
            Roles.AddUserToRole(txtUserName.Text, ddlShareLevel.SelectedValue)

        'Catch ex As Exception

        'End Try
        UpdateTable("insert into userExtraInfo (username, user_role, starting_salary, startdate, email_address) select '" & txtUserName.Text.Replace("'", "''") & "','" & ddlShareLevel.SelectedValue & "', 1.00, dbo.getMTDate(),'" & txtEmail.Text & "'")
        UpdateTable("update userextrainfo  set startdate = a.min_date from (select min(review_date) as min_date, reviewer from form_score3 group by reviewer) a where username = a.reviewer and startdate is null ")


        For Each cb As ListItem In cblScorecards.Items
            If cb.Selected Then
                UpdateTable("insert into userapps(username, appname, dateadded, who_added, user_scorecard) select '" & txtUserName.Text & "','" & ddlInitialApp.SelectedValue & "', dbo.getMTDate(), '" & User.Identity.Name & "',(select top 1 id from scorecards where short_name = '" & cb.Text & "' and active = 1)")
            End If
        Next




        UpdateTable("insert into sc_update(reviewer, sc_id, date_reviewed) select '" & txtUserName.Text & "', scorecards.id, dbo.getMTDate() from scorecards join userapps on userapps.appname = scorecards.appname where userapps.username = '" & txtUserName.Text & "' and scorecards.active = 1")

        Response.Redirect("client_users.aspx")
    End Sub

    Private Sub gvMyUsers_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvMyUsers.RowUpdating
        If e.OldValues("password").ToString = "*****" Then
            If e.NewValues("password").ToString <> "*****" Then
                Dim mu As MembershipUser = Membership.GetUser(e.OldValues("username").ToString)
                mu = Membership.GetUser(e.OldValues("username").ToString, False)
                If mu Is Nothing Then
                    Exit Sub
                End If
                mu.ChangePassword(mu.ResetPassword(), e.NewValues("password").ToString)
            End If

        End If

        If e.OldValues("user_role").ToString <> e.NewValues("user_role").ToString Then
            Roles.RemoveUserFromRole(e.NewValues("username"), e.OldValues("user_role").ToString)
            Roles.AddUserToRole(e.NewValues("username"), e.NewValues("user_role").ToString)
        End If




        'Clear old values and rewrite



        Dim cbl As CheckBoxList = gvMyUsers.Rows(e.RowIndex).FindControl("cblScores")
        Dim hdn As HiddenField = gvMyUsers.Rows(e.RowIndex).FindControl("hdnThisUser")

        UpdateTable("delete From userapps where username = '" & hdn.Value & "'")

        For Each li As ListItem In cbl.Items
            If li.Selected Then
                UpdateTable("insert into userapps (username, appname, dateadded, user_scorecard) select '" & hdn.Value & "',(Select appname from scorecards where id = '" & li.Value & "'), dbo.getMTDate(), '" & li.Value & "'")
            End If
        Next


    End Sub
    Protected Sub cblScores_DataBinding(sender As Object, e As EventArgs)
        Dim hdn As HiddenField = sender.parent.parent.findcontrol("hdnThisUser")
        Dim cblScores As CheckBoxList = sender
        Dim current_dt As DataTable = GetTable("select * from userapps where username = '" & hdn.Value & "'")

        For Each dr As DataRow In current_dt.Rows
            For Each li As ListItem In cblScores.Items
                If dr("user_scorecard").ToString = li.Value Then
                    li.Selected = True
                End If
            Next
        Next
    End Sub



    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        gvMyUsers.Columns(0).Visible = False
        GV_to_CSV(gvMyUsers, "ClientUsersReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=ClientUsersExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvMyUsers.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Protected Sub btnGo_Click(sender As Object, e As EventArgs)
        'Dim alt_user As String = CType(lvAltUser.FindControl("new_user"), TextBox).Text

        'Dim user_dt As DataTable = GetTable("select * from userextrainfo   where username = '" & alt_user & "'")

        'If user_dt.Rows.Count > 0 Then

        '    UserImpersonation.ImpersonateUser(alt_user, "cd2.aspx")
        '    Response.Redirect("cd2.aspx")
        'Else
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "nouser", "alert('User does not exist');", True)
        'End If
    End Sub

    Private Sub gvMyUsers_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMyUsers.RowCommand


        'Response.Write(e.CommandArgument & " " & e.CommandName)
        'Response.End()
        If e.CommandName = "switchUser" Then
            UserImpersonation.ImpersonateUser(e.CommandArgument, "client_users.aspx")
            Response.Redirect("cd2.aspx")
        End If
    End Sub
    Protected Sub DropDownList2_DataBinding(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = sender

            Dim gvr As GridViewRow = sender.parent.parent

            ddl.SelectedValue = gvr.DataItem("user_group").ToString

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnUpdateGroup_Click(sender As Object, e As EventArgs) Handles btnUpdateGroup.Click
        UpdateTable("update userextrainfo set user_group = '" & txtNewGroup.Text & "' where username = '" & txtUserGroup.Text & "'")
        txtNewGroup.Text = ""
        txtUserGroup.Text = ""
        gvMyUsers.DataBind()
    End Sub
End Class
