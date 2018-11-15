Imports Common
Partial Class daily_work
    Inherits System.Web.UI.Page

    Protected Sub gvDaily_DataBound(sender As Object, e As EventArgs) Handles gvDaily.DataBound
        'Sum_Column(gvDaily, 1)
        'Sum_Column(gvDaily, 2)
        'Sum_Column(gvDaily, 3)
        Sum_Column(gvDaily, 4)
        Sum_Column(gvDaily, 5)
        Sum_Column(gvDaily, 6)
        Sum_Column(gvDaily, 7)
        Sum_Column(gvDaily, 8)
        Sum_Column(gvDaily, 9)
        Sum_Column(gvDaily, 10)
        Sum_Column(gvDaily, 11)
        Sum_Column(gvDaily, 12)



        If Not User.IsInRole("Admin") Then
            gvDaily.Columns(8).Visible = False
            gvDaily.Columns(6).Visible = False
        End If

        Dim new_now As DateTime = Now.AddHours(-6)

        Dim dom As Integer = Date.DaysInMonth(new_now.Year, new_now.Month)

        lblMTD.Text = "$" & (CInt(dom / new_now.Day * gvDaily.FooterRow.Cells(6).Text)).ToString & "/" & (CInt(dom / new_now.Day * gvDaily.FooterRow.Cells(5).Text)).ToString & " Pending Calls: " & gvDaily.FooterRow.Cells(11).Text

        gvDaily.FooterRow.Cells(5).Text &= " (" & CInt(dom / new_now.Day * gvDaily.FooterRow.Cells(5).Text) & ")"
        gvDaily.FooterRow.Cells(6).Text &= " ($" & CInt(dom / new_now.Day * gvDaily.FooterRow.Cells(6).Text) & ")"


    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load



        If User.IsInRole("Admin") Or User.IsInRole("QA Lead") Or User.IsInRole("Calibrator") Then
        Else
            Response.Redirect("login.aspx?ReturnURL=daily_work.aspx")
        End If

        dsDaily.SelectParameters("username").DefaultValue = User.Identity.Name

        If Request("email") = "yes" Then
            Dim admins() As String = Roles.FindUsersInRole("Admin", "")
        End If
    End Sub

    Private Sub dsDaily_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsDaily.Selecting
        e.Command.CommandTimeout = 180
    End Sub

    Private Sub ddlAM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAM.SelectedIndexChanged
        dsDaily.DataBind()
    End Sub
End Class
