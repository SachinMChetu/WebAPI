Imports System.Data
Imports Common

Partial Class calibration_status
    Inherits System.Web.UI.Page
    Dim danger_QAs As Integer = 0
    Dim tl_approve As Integer = 0
    Private Sub calibration_status_Load(sender As Object, e As EventArgs) Handles Me.Load

        If User.IsInRole("Admin") Or User.IsInRole("QA Lead") Then
        Else
            Response.Redirect("default.aspx")
        End If



    End Sub

    Private Sub rptdsScorecards_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptdsScorecards.ItemDataBound

    End Sub

    Private Sub rptdsScorecards_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles rptdsScorecards.ItemCreated
        If e.Item.ItemType = ListItemType.Item Then
            Dim dsCalibrations As SqlDataSource = e.Item.FindControl("dsCalibrations")
            If User.IsInRole("QA Lead") Then
                dsCalibrations.FilterExpression = "[Team Lead] = '" & User.Identity.Name & "'"
            End If
        End If
    End Sub
    Protected Sub btnRetrain_Click(sender As Object, e As EventArgs)
        Dim hdnScorecard As HiddenField = sender.parent.findcontrol("hdnScorecard")
        Dim lbl As Label = sender.parent.findcontrol("lblScorecard")
        Dim txtRetrain As TextBox = sender.parent.findcontrol("txtRetrain")
        UpdateTable("update sc_training_approvals set retrain_date = '" & txtRetrain.Text & "' where sc_id = '" & hdnScorecard.Value & "'")
        Email_Error(User.Identity.Name & " just updated all QAs retrain date for " & lbl.Text, "chad@callcriteria.com")
        Dim gv As GridView = sender.parent.findcontrol("gvCalibrations")
        gv.DataBind()

    End Sub

    Protected Sub gvCalibrations_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = e.Row.DataItem
            If drv("status").ToString = "TL Pending" Then
                CType(e.Row.FindControl("btnTLApprove"), Button).Visible = True
                CType(e.Row.FindControl("btnRetrain"), Button).Visible = False
                e.Row.Cells(10).BackColor = Drawing.Color.Red
                tl_approve += 1
                lblTL.Text = tl_approve
            End If
            If drv("role").ToString = "Trainee" Then
                CType(e.Row.FindControl("btnBypass"), Button).Visible = True
                CType(e.Row.FindControl("btnRetrain"), Button).Visible = False
            End If


            e.Row.Cells(2).BackColor = Drawing.Color.LightGray
            e.Row.Cells(3).BackColor = Drawing.Color.LightGray

            Try
                If CInt(e.Row.Cells(4).Text) < CInt(e.Row.Cells(7).Text) Then
                    e.Row.Cells(4).BackColor = Drawing.Color.Red
                    danger_QAs += 1
                    'lblFail.Text = danger_QAs
                End If

            Catch ex As Exception

            End Try

            'Email_Error(User.Identity.Name & " ")
        End If

    End Sub
    Protected Sub gvCalibrations_RowCommand(sender As Object, e As GridViewCommandEventArgs)


        Dim QA As String = DirectCast(DirectCast(e.CommandSource, Button).NamingContainer, GridViewRow).Cells(1).Text
        Dim hdn As HiddenField = DirectCast(DirectCast(e.CommandSource, Button).NamingContainer, GridViewRow).Parent.Parent.Parent.FindControl("hdnScorecard")
        Dim lbl As Label = DirectCast(DirectCast(e.CommandSource, Button).NamingContainer, GridViewRow).Parent.Parent.Parent.FindControl("lblScorecard")





        If e.CommandName = "Reset" Then

            UpdateTable("update sc_training_approvals set retrain_date = convert(date, dbo.getMTDate()), train_status = '' where username ='" & QA & "' and sc_id = '" & hdn.Value & "'")
            UpdateTable("update userapps set  scorecard_role = 'QA'  where username ='" & QA & "' and user_scorecard = '" & hdn.Value & "'")
            Email_Error(User.Identity.Name & " just updated " & QA & " retrain date and bypassed them for " & lbl.Text, "chad@callcriteria.com")
        End If
        If e.CommandName = "AllowTrain" Then

            UpdateTable("update sc_training_approvals set retrain_date = convert(date, dbo.getMTDate()), train_status = 'Retrain' where username ='" & QA & "' and sc_id = '" & hdn.Value & "'")
            UpdateTable("update userapps set  scorecard_role = 'Trainee'  where username ='" & QA & "' and user_scorecard = '" & hdn.Value & "'")
            Email_Error(User.Identity.Name & " just updated " & QA & " retrain date and set them to allow them to train them for " & lbl.Text, "chad@callcriteria.com")
        End If
        If e.CommandName = "RetainDate" Then

            UpdateTable("update sc_training_approvals set retrain_date = convert(date, dbo.getMTDate()),  train_status = ''  where username ='" & QA & "' and sc_id = '" & hdn.Value & "'")
            Email_Error(User.Identity.Name & " just updated " & QA & " retrain date for " & lbl.Text, "chad@callcriteria.com")
        End If

        Dim gv As GridView = DirectCast(DirectCast(e.CommandSource, Button).NamingContainer, GridViewRow).Parent.Parent
        gv.DataBind()

    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        Dim grv As GridViewRow = sender.parent.parent
        Dim drv As DataRowView = grv.DataItem

        Dim txtCutCount As TextBox = grv.FindControl("txtCutCount")
        Dim txtCutPercent As TextBox = grv.FindControl("txtCutPercent")
        Dim txtCutAvg As TextBox = grv.FindControl("txtCutAvg")




        UpdateTable("update scorecards set cutoff_percent_avg= " & IIf(txtCutAvg.Text = "", "null", txtCutAvg.Text) & ", cutoff_count = " & IIf(txtCutCount.Text = "", "null", txtCutCount.Text) & ", cutoff_percent = " & IIf(txtCutPercent.Text = "", "null", txtCutPercent.Text) & " where id = " & grv.Cells(2).Text)
        Dim gv As GridView = grv.Parent.Parent
        gv.DataBind()
    End Sub

    Private Sub ddlAppname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppname.SelectedIndexChanged
        dsScorecards.DataBind()
    End Sub
End Class
