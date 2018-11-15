Imports Common

Partial Class ClosedClientCalibrations
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=closed_client_calibrations.aspx")
        End If

        Dim access As String() = {"Admin", "QA Lead", "Calibrator", "Center Manager"}
        If Not access.Contains(Roles.GetRolesForUser().Single()) Then
            Response.Redirect("cd2.aspx")
        End If

        dsAppList.SelectParameters.Item("UserName").DefaultValue = User.Identity.Name
        dsCCC.SelectParameters.Item("UserName").DefaultValue = User.Identity.Name

    End Sub

    Protected Sub ddlAppList_SelectedIndexChanged(sender As Object, e As EventArgs)

        ddlScorecard.Items.Clear()

        If ddlAppList.SelectedIndex = 0 Then
            ddlScorecard.Enabled = False
        Else
            ddlScorecard.Enabled = True
            ddlScorecard.Items.Add(New ListItem("--Select Scorecard--", ""))
            ddlScorecard.DataSource = GetTable("SELECT Short_Name, Scorecards.ID FROM Scorecards JOIN UserApps ON User_Scorecard = Scorecards.ID WHERE UserName = '" & User.Identity.Name & "' AND Scorecards.AppName = '" & ddlAppList.SelectedValue & "'")
            ddlScorecard.DataBind()
        End If
    End Sub
    Protected Sub ddlScorecard_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
End Class
