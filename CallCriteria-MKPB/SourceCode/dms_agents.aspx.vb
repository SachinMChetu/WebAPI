Imports Common

Partial Class Q3M_team
    Inherits System.Web.UI.Page

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        Dim selRowIndex As Integer = DirectCast(DirectCast(sender, CheckBox).Parent.Parent, GridViewRow).RowIndex
        Dim cb As CheckBox = DirectCast(gvAgents.Rows(selRowIndex).FindControl("CheckBox1"), CheckBox)
        Dim id As Integer = gvAgents.DataKeys(selRowIndex).Value
        ' Find other checkbox using FindControl and check the
        If cb.Checked Then
            Updatetable("update dms_agents set special = 1 where id = " & id)
        Else
            Updatetable("update dms_agents set special = 0 where id = " & id)
        End If
    End Sub

    Private Sub gvAgents_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvAgents.RowUpdating

        'For Each newval As DictionaryEntry In e.NewValues
        '    Response.Write(newval.Key & " " & newval.Value & "<br>")
        'Next

        'For Each newval As DictionaryEntry In e.OldValues
        '    Response.Write(newval.Key & " " & newval.Value & "<br>")
        'Next


        UpdateTable("update xcc_report_new set agent_group = '" & e.NewValues("agent_group") & "' where agent = '" & e.OldValues("AGENT").ToString & "'")
        UpdateTable("update userextrainfo set user_group = '" & e.NewValues("agent_group") & "' where username = '" & e.OldValues("AGENT").ToString & "'")

    End Sub
End Class
