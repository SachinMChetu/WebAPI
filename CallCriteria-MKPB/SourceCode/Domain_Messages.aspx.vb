Imports Common

Partial Class Domain_Messages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            dsMessages.SelectParameters("username").DefaultValue = User.Identity.Name
            dsMessages.DataBind()
        End If
    End Sub

    Protected Sub gvMessages_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMessages.RowCommand
        UpdateTable("insert into DomainMessageAck(message_id, ack_by, ack_date) select " & gvMessages.DataKeys(e.CommandArgument).Value & " ,'" & User.Identity.Name & "',dbo.getMTDate()")
        dsMessages.DataBind()
        gvMessages.DataBind()
    End Sub
End Class
