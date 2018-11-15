Imports Common

Partial Class Counts
    Inherits System.Web.UI.Page

    Protected Sub gvCounts_DataBound(sender As Object, e As System.EventArgs) Handles gvCounts.DataBound, gvCOunts2.DataBound

        Dim gv As GridView = sender
        For x = 2 To 10
            Sum_Column(gv, x)
        Next
        Sum_Time_Column(gv, 11)
        Avg_Column(gv, 12)
        Avg_Column(gv, 13)
        Avg_Column(gv, 14)
        Sum_Time_Column(gv, 15)

        If gv.Rows.Count > 0 Then
            For Each gvr As GridViewRow In gv.Rows


                If gvr.RowType = DataControlRowType.DataRow Then
                    'If gvr.Cells(1).Text > 0 Then
                    '    gvr.Cells(4).Text = FormatNumber(gvr.Cells(4).Text / gvr.Cells(1).Text, 2)
                    '    gvr.Cells(5).Text = FormatNumber(gvr.Cells(5).Text / gvr.Cells(1).Text, 2)
                    'End If

                    Try

                        If gvr.Cells(2).Text - gvr.Cells(4).Text - gvr.Cells(5).Text - gvr.Cells(6).Text - gvr.Cells(9).Text <> 0 Then
                            gvr.Cells(4).ForeColor = Drawing.Color.Blue
                        End If
                        If gvr.Cells(5).Text <> "0" Then
                            gvr.Cells(5).Text = "<a href='bad_call_report.aspx' style='color:red'>" & gvr.Cells(5).Text & "</a>"
                        End If
                    Catch ex As Exception

                    End Try

                End If



                If gvr.RowType = DataControlRowType.Footer Then
                    'If gvr.Cells(1).Text > 0 Then
                    '    gvr.Cells(4).Text = FormatNumber(gvr.Cells(4).Text / gvr.Cells(1).Text, 2)
                    '    gvr.Cells(5).Text = FormatNumber(gvr.Cells(5).Text / gvr.Cells(1).Text, 2)
                    'End If

                    Try

                        Dim time_pieces() As String = Split(gvr.Cells(10).Text, ":")

                        Dim total_sec As Integer = time_pieces(0) * 3600 + time_pieces(1) * 60 + time_pieces(2)

                        gvr.Cells(14).Text = total_sec / gvr.Cells(4).Text


                    Catch ex As Exception

                    End Try

                End If


            Next
        End If
    End Sub


    Protected Sub gvCounts_DataBound2(sender As Object, e As System.EventArgs) Handles gvReview.DataBound

        Dim gv As GridView = sender
        Sum_Column(gv, 2)
        Sum_Column(gv, 3)

        For x = 6 To 12
            Sum_Column(gv, x)
        Next
        Sum_Time_Column(gv, 13)
        Avg_Column(gv, 14)
        Avg_Column(gv, 15)
        Avg_Column(gv, 16)
        Sum_Time_Column(gv, 17)

        If gv.Rows.Count > 0 Then
            For Each gvr As GridViewRow In gv.Rows


                If gvr.RowType = DataControlRowType.DataRow Then
                    'If gvr.Cells(1).Text > 0 Then
                    '    gvr.Cells(4).Text = FormatNumber(gvr.Cells(4).Text / gvr.Cells(1).Text, 2)
                    '    gvr.Cells(5).Text = FormatNumber(gvr.Cells(5).Text / gvr.Cells(1).Text, 2)
                    'End If

                    Try

                        If gvr.Cells(2).Text - gvr.Cells(6).Text - gvr.Cells(7).Text - gvr.Cells(8).Text - gvr.Cells(11).Text <> 0 Then
                            gvr.Cells(6).ForeColor = Drawing.Color.Blue
                        End If
                        If gvr.Cells(7).Text <> "0" Then
                            gvr.Cells(7).Text = "<a href='bad_call_report.aspx' style='color:red'>" & gvr.Cells(7).Text & "</a>"
                        End If
                    Catch ex As Exception

                    End Try

                End If



                If gvr.RowType = DataControlRowType.Footer Then
                    'If gvr.Cells(1).Text > 0 Then
                    '    gvr.Cells(4).Text = FormatNumber(gvr.Cells(4).Text / gvr.Cells(1).Text, 2)
                    '    gvr.Cells(5).Text = FormatNumber(gvr.Cells(5).Text / gvr.Cells(1).Text, 2)
                    'End If

                    Try

                        Dim time_pieces() As String = Split(gvr.Cells(12).Text, ":")

                        Dim total_sec As Integer = time_pieces(0) * 3600 + time_pieces(1) * 60 + time_pieces(2)

                        gvr.Cells(14).Text = total_sec / gvr.Cells(6).Text


                    Catch ex As Exception

                    End Try

                End If


            Next
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=counts.aspx")
        End If

        If User.IsInRole("Admin") Then
            GridView1.Visible = True
            GridView2.Visible = True
        Else
            GridView1.Visible = False
            GridView2.Visible = False
        End If

        If Not IsPostBack Then
            txtAgentStart.Text = DateAdd(DateInterval.Day, -7, Today).ToShortDateString
            txtAgentEnd.Text = Today.ToShortDateString

            If Request("start") <> "" Then
                txtAgentStart.Text = Request("start")
            End If

            If Request("end") <> "" Then
                txtAgentEnd.Text = Request("end")
            End If




            If {"Client", "Manager", "Supervisor"}.Contains(Roles.GetRolesForUser(User.Identity.Name).Single) Then
                dsApps.SelectCommand = "select scorecards.appname + ' (' + short_name + ')' as appname, scorecards.ID  from scorecards with (nolock)   join app_settings  with (nolock)   on app_settings.appname = scorecards.appname where app_settings.active=1 and scorecards.active = 1 and scorecards.id in (select user_scorecard from userapps where username = '" & User.Identity.Name & "') order by scorecards.appname "
                dsApps.DataBind()

            End If

            If Request("appname") <> "" Then
                ddlApps.DataBind()
                ddlApps.SelectedValue = Request("appname")
                dsCounts.DataBind()
            End If

            If Request("ID") <> "" Then
                ddlApps.DataBind()
                ddlApps.SelectedValue = Request("ID")
                dsCounts.DataBind()
            End If

        End If

    End Sub

    Protected Sub ddlApps_DataBound(sender As Object, e As EventArgs) Handles ddlApps.DataBound
       
    End Sub

    Protected Sub ddlApps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApps.SelectedIndexChanged
        Response.Redirect("counts.aspx?ID=" & ddlApps.SelectedValue)
        'dsCounts.DataBind()
    End Sub

    Private Sub dsCounts_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsCounts.Selecting, dsCOunts2.Selecting
        e.Command.CommandTimeout = 900
    End Sub

    Private Sub gvCOunts2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCOunts2.RowDataBound, gvCounts.RowDataBound, gvReview.RowDataBound
        If {"Client", "Manager", "Supervisor"}.Contains(Roles.GetRolesForUser(User.Identity.Name).Single) Then

            For x = 14 To e.Row.Cells.Count - 1
                e.Row.Cells(x).Visible = False
            Next
        End If

    End Sub

    Private Sub btnAgentReport_Command(sender As Object, e As CommandEventArgs) Handles btnAgentReport.Command

    End Sub

    Private Sub btnAgentReport_Click(sender As Object, e As EventArgs) Handles btnAgentReport.Click
        dsCounts.DataBind()
        dsCOunts2.DataBind()

        GridView1.DataBind()
        GridView2.DataBind()
    End Sub
End Class
