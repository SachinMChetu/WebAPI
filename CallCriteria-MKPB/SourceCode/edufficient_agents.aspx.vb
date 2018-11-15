Imports Common
Imports System.Data

Partial Class edufficient_agents
    Inherits System.Web.UI.Page

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        Dim gvr As GridViewRow = sender.parent.parent
        Dim source As String = gvr.Cells(0).Text
        Dim dest As DropDownList = gvr.Cells(1).FindControl("ddlDest")

        Dim id As String = gvAgent.DataKeys(gvr.RowIndex).Value

        If id = 0 Then
            UpdateTable("insert into eduff_agents (source_name, destination_name) select '" & source & "','" & dest.SelectedValue & "'")
        Else
            UpdateTable("update eduff_agents set destination_name = '" & dest.SelectedValue & "' where id = " & id)
        End If

        UpdateTable("update xcc_report_new set agent = destination_name from eduff_agents where agent = source_name and agent in (select distinct agent from xcc_report_new where agent not in (select [sources company name] from eduff_cehe_names) and appname = 'edufficient') and appname = 'edufficient'")
        gvAgent.DataBind()
    End Sub

    Private Sub btnAddPartner_Click(sender As Object, e As EventArgs) Handles btnAddPartner.Click
        UpdateTable("insert into eduff_cehe_names ([sources company name]) select '" & txtNewPartner.Text & "'")
        gvAgent.DataBind()
    End Sub

    Private Sub btnCreateAgents_Click(sender As Object, e As EventArgs) Handles btnCreateAgents.Click
        Dim agent_dt As DataTable = GetTable("select * from eduff_cehe_names where [sources company name] not in (select username from [dbo].[aspnet_Users]) ")

        For Each dr As DataRow In agent_dt.Rows

            Dim mu As MembershipUser = Membership.CreateUser(dr("sources company name"), "education123")
            Roles.AddUserToRole(dr("sources company name"), "Agent")

            UpdateTable("insert into userextrainfo(username, user_role) select '" & dr("sources company name") & "','Agent'")

        Next

        Response.Write("updated")

    End Sub
End Class
