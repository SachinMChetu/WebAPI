Imports Common
Partial Class edit_apps
    Inherits System.Web.UI.Page

    Protected Sub CheckBoxList1_DataBound(sender As Object, e As EventArgs) Handles CheckBoxList1.DataBound
        For Each li As ListItem In CheckBoxList1.Items
            If li.Value = 1 Then li.Selected = True Else li.Selected = False
        Next
    End Sub

    Protected Sub CheckBoxList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckBoxList1.SelectedIndexChanged
        UpdateTable("delete from UserApps where username = '" & Request("user") & "'")
        For Each li As ListItem In CheckBoxList1.Items
            If li.Selected Then
                UpdateTable("insert into UserApps (username, appname, dateadded, who_added) select '" & Request("user") & "','" & li.Text & "',dbo.getMTDate(), '" & User.Identity.Name & "'")
            End If
        Next

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.IsInRole("Admin") Then
            Response.Redirect("default.aspx")
        End If

        If Not IsPostBack Then
            lblUser.Text = Request("user")
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        For Each li As ListItem In ListBox2.Items

            If li.Selected Then
                UpdateTable("delete from UserApps where  username ='" & Request("user") & "' and appname = '" & li.Text & "'")
                UpdateTable("insert into UserApps (username, appname, dateadded, who_added) select '" & Request("user") & "','" & li.Text & "',dbo.getMTDate(), '" & User.Identity.Name & "'")
            End If
        Next

        ListBox1.DataBind()
        ListBox2.DataBind()
    End Sub

    Protected Sub btnAddAll_Click(sender As Object, e As EventArgs) Handles btnAddAll.Click
        For Each li As ListItem In ListBox2.Items
            UpdateTable("delete from UserApps where  username ='" & Request("user") & "' and appname = '" & li.Text & "'")
            UpdateTable("insert into UserApps (username, appname, dateadded, who_added) select '" & Request("user") & "','" & li.Text & "',dbo.getMTDate(), '" & User.Identity.Name & "'")
        Next

        ListBox1.DataBind()
        ListBox2.DataBind()
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        For Each li As ListItem In ListBox1.Items
            If li.Selected Then
                UpdateTable("delete from UserApps where  username ='" & Request("user") & "' and appname = '" & li.Text & "'")
            End If
        Next

        ListBox1.DataBind()
        ListBox2.DataBind()
    End Sub

    Protected Sub btnRemoveAll_Click(sender As Object, e As EventArgs) Handles btnRemoveAll.Click
        For Each li As ListItem In ListBox1.Items
            UpdateTable("delete from UserApps where  username ='" & Request("user") & "' and appname = '" & li.Text & "'")
        Next

        ListBox1.DataBind()
        ListBox2.DataBind()
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        UpdateTable("delete from UserApps where  username ='" & Request("user") & "'")
        Dim user_priority As Integer = 1
        For Each li As ListItem In ListBox1.Items
            UpdateTable("insert into UserApps (username, appname, dateadded, who_added, user_priority) select '" & Request("user") & "','" & li.Text & "',dbo.getMTDate(), '" & User.Identity.Name & "'," & user_priority)

            Response.Write("insert into UserApps (username, appname, dateadded, who_added, user_priority) select '" & Request("user") & "','" & li.Text & "',dbo.getMTDate(), '" & User.Identity.Name & "'," & user_priority & "<br>")
            user_priority += 1

        Next

        ListBox1.DataBind()
        ListBox2.DataBind()
    End Sub
End Class
