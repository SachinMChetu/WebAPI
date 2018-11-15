
Partial Class cs_tracking
    Inherits System.Web.UI.Page

    Private Sub ddlClient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlClient.SelectedIndexChanged
        refreshall()
    End Sub

    Private Sub ddlQA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlQA.SelectedIndexChanged
        refreshall()
    End Sub

    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        refreshall()
    End Sub

    Private Sub cs_tracking_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDate.Text = Today.ToShortDateString
            refreshall()
        End If
    End Sub

    Private Sub refreshall()

        'ddlQA.Items.Clear()
        'ddlQA.Items.Add(New ListItem("All", ""))
        'ddlClient.Items.Clear()
        'ddlClient.Items.Add(New ListItem("All", ""))


        'ddlQA.DataBind()

        'ddlClient.DataBind()
        dsPerf.DataBind()
        dsPending.DataBind()
        'gvPerf.DataBind()
    End Sub

    Private Sub ddlSC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSC.SelectedIndexChanged
        refreshall()
    End Sub
End Class
