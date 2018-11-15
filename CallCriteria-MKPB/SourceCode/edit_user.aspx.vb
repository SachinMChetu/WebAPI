
Imports System.Data
Imports Common

Partial Class edit_user
    Inherits System.Web.UI.Page

    Protected Sub cblScores_DataBinding(sender As Object, e As EventArgs)
        Dim hdn As HiddenField = sender.parent.parent.findcontrol("hdnThisUser")
        Dim cblScores As CheckBoxList = sender
        Dim current_dt As DataTable = GetTable("select * from userapps where username = '" & Replace(Request("user"), "'", "''") & "'")

        For Each dr As DataRow In current_dt.Rows
            For Each li As ListItem In cblScores.Items
                If dr("user_scorecard").ToString = li.Value Then
                    li.Selected = True
                End If
            Next
        Next
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=client_users.aspx")
        End If
        dsMyUsers.SelectParameters("username").DefaultValue = User.Identity.Name
        dsGroup.SelectParameters("username").DefaultValue = User.Identity.Name
        dsScorecards.SelectParameters("username").DefaultValue = User.Identity.Name
        dsManager.SelectParameters("username").DefaultValue = User.Identity.Name


    End Sub

    Protected Sub FormView1_ItemUpdating(sender As Object, e As FormViewUpdateEventArgs) Handles FormView1.ItemUpdating
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

            Try
                Roles.RemoveUserFromRole(e.NewValues("username"), e.OldValues("user_role").ToString)
            Catch ex As Exception

            End Try

            Try
                Roles.AddUserToRole(e.NewValues("username"), e.NewValues("user_role").ToString)
            Catch ex As Exception

            End Try

        End If

        'Clear old values and rewrite



        Dim cbl As CheckBoxList = FormView1.FindControl("cblScores")
        Dim hdn As HiddenField = FormView1.FindControl("hdnThisUser")

        UpdateTable("delete From userapps where username = '" & hdn.Value & "'")

        For Each li As ListItem In cbl.Items
            If li.Selected Then
                UpdateTable("insert into userapps (username, appname, dateadded, user_scorecard) select '" & hdn.Value & "',(Select appname from scorecards where id = '" & li.Value & "'), dbo.getMTDate(), '" & li.Value & "'")
            End If
        Next

    End Sub

    Private Sub FormView1_ItemUpdated(sender As Object, e As FormViewUpdatedEventArgs) Handles FormView1.ItemUpdated
        'lblUpdated.Text = "Updated2."

        'update userextrainfo set non_edit=@non_edit, first_name =@first_name, manager=@manager, last_name =@last_name, email_address=@email_address, user_role=@user_role, user_group=@user_group where id=@ID
        Dim fv As FormView = sender
        Dim chkUpdateHistory As CheckBox = fv.FindControl("chkUpdateHistory")

        Dim DropDownList2 As DropDownList = fv.FindControl("DropDownList2")

        Dim agent_group As String = e.OldValues("user_group").ToString
        If DropDownList2.SelectedValue <> "" Then
            agent_group = DropDownList2.SelectedValue
        End If

        If chkUpdateHistory.Checked Then
            UpdateTable("update xcc_report_new set agent_group = '" & agent_group & "' where agent = '" & e.OldValues("username").ToString & "' and isnull(agent_group,'') <> '" & agent_group & "' and scorecard in (select user_scorecard from userapps where username = '" & User.Identity.Name & "') ")
        End If

        lblUpdated.Text = "<script>$(function() { window.parent.closeEditUserPopup(); window.parent.location.reload(); });</script>"
    End Sub
    Protected Sub DropDownList2_DataBound(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = sender

            'Dim gvr As FormViewRow = sender.parent.parent


            ddl.SelectedValue = FormView1.DataItem("user_group").ToString

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnSCGroupAdd_Click(sender As Object, e As EventArgs)
        Dim ddlSCForGroup As DropDownList = sender.findcontrol("ddlSCForGroup")
        Common.UpdateTable("insert into user_groups(userID, scorecard) select (select top 1 id from userextrainfo where username = '" & Request.QueryString("user") & "')," & ddlSCForGroup.SelectedValue)
        Dim dsGroups As SqlDataSource = sender.findcontrol("dsGroups")
        dsGroups.DataBind()
        Dim gvUserGroups As GridView = sender.findcontrol("gvUserGroups")
        gvUserGroups.DataBind()
    End Sub
End Class
