Imports System.IO
Imports Common
Imports System.Data.SqlClient
Imports System.Data

Partial Class ExpandedView
    Inherits System.Web.UI.Page
    Dim header_cell As Integer = -1


    Dim isExporting As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If User.IsInRole("Admin") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("QA Lead") Or User.IsInRole("Client") Then
        Else
            Response.Redirect("default.aspx")
        End If

        If Not Page.IsPostBack Then

            'If User.IsInRole("Client") Then
            '    QA_Weekly_div.Visible = False
            'End If


            dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
            dsApps.DataBind()

            If Request("StartDate") IsNot Nothing Then
                txtGroupStartxp.Text = Request("StartDate")
            Else
                txtGroupStartxp.Text = Month(Now) & "/1/" & Year(Now)
            End If

            If Request("EndDate") IsNot Nothing Then
                txtgroupEndxp.Text = Request("EndDate")
            Else
                txtgroupEndxp.Text = DateAdd(DateInterval.Day, 1, Today).ToShortDateString
            End If

            If Request("filter") IsNot Nothing Then
                hdnExtraFilters.Value = Request("filter")
            End If

            If User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("QA Lead") Or User.IsInRole("Calibrator") Then
                Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)

                If ddlAgentByGroupxp.Items.Contains(New ListItem(userProfile.Group)) Then
                Else
                    ddlAgentGroupxp.Items.Add(New ListItem(userProfile.Group))
                End If

                ddlAgentGroupxp.SelectedValue = userProfile.Group
                ddlAgentGroupxp.Enabled = False
                ddlAgentByGroupxp.Items.Clear()
                ddlAgentByGroupxp.Items.Add(New ListItem("(Select)", ""))

                ddlAgentByGroupxp.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & userProfile.Group & "' and  appname = '" & ddlApps.SelectedValue & "' order by AGent")

            Else
                ddlAgentByGroupxp.Items.Clear()
                ddlAgentByGroupxp.Items.Add(New ListItem("(Select)", ""))
                ddlAgentByGroupxp.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & ddlApps.SelectedValue & "' order by AGent")

            End If

            ddlAgentByGroupxp.DataBind()
            ' btnAgentGroupxp_Click(sender, e)

        End If
    End Sub
    Protected Sub btnAgentGroupxp_Click(sender As Object, e As System.EventArgs) Handles btnAgentGroupxp.Click

        Dim where_clause As String = " where fs.scorecard = '" & ddlApps.SelectedValue & "' "
        If txtGroupStartxp.Text <> "" Then
            where_clause &= " and  convert(date, call_date)  >= '" & txtGroupStartxp.Text & "' "
        End If

        If txtgroupEndxp.Text <> "" Then
            where_clause &= " and  convert(date, call_date)  <= '" & txtgroupEndxp.Text & "' "
        End If

        If User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("Calibrator") Or User.IsInRole("QA Lead") Then
            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                where_clause &= " and Agent_group = '" & userProfile.Group & "' "
            End If
        Else
            If ddlAgentGroupxp.SelectedValue <> "" Then
                where_clause &= " and Agent_group = '" & ddlAgentGroupxp.SelectedValue & "' "
            End If

        End If



        If ddlAgentByGroupxp.SelectedValue <> "" Then
            where_clause &= " and Agent = '" & ddlAgentByGroupxp.SelectedValue & "' "
        End If



        ' where_clause &= hdnExtraFilters.Value

        'Response.Write(where_clause)
        'Response.End()
        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)

        cn.Open()


        'Response.Write("<!-- " & where_clause & "-->")
        'Response.End()
        'Dim reply As New SqlDataAdapter("GetExpandedView", cn)



        Dim reply As New SqlDataAdapter("GetExpandedView_calib", cn)
        reply.SelectCommand.CommandType = Data.CommandType.StoredProcedure
        reply.SelectCommand.Parameters.AddWithValue("where_clause", where_clause)
        reply.SelectCommand.Parameters.AddWithValue("scorecard", ddlApps.SelectedValue)
        If (addTimes.Checked) Then
            reply.SelectCommand.Parameters.AddWithValue("add_times", 1)
        End If
        reply.SelectCommand.CommandTimeout = 480

        Dim dt As New DataTable
        reply.Fill(dt)
        cn.Close()
        cn.Dispose()
        gvAgentGroupxp.DataSource = dt
        gvAgentGroupxp.DataBind()

    End Sub

    Protected Sub gvAgentGroupxp_DataBound(sender As Object, e As System.EventArgs) Handles gvAgentGroupxp.DataBound

        If gvAgentGroupxp.Rows.Count > 0 Then
            btnSupeExportxp.Visible = True
        Else
            btnSupeExportxp.Visible = False
        End If


       

        lblRows.Text = gvAgentGroupxp.Rows.Count & " Rows"
    End Sub

    Protected Sub gvAgentGroupxp_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAgentGroupxp.RowDataBound


        If e.Row.RowType = DataControlRowType.Header Then

            If (User.IsInRole("Client") Or User.IsInRole("Supervisor")) Then
                e.Row.Cells(1).Visible = False
            End If
            Dim cell_count As Integer = 0
            For Each tc As TableCell In e.Row.Cells
                If tc.Text = "Comments" Then
                    header_cell = cell_count
                End If
                cell_count += 1
            Next
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            If (User.IsInRole("Client") Or User.IsInRole("Supervisor")) Then
                e.Row.Cells(1).Visible = False
            End If
            For x = 1 To e.Row.Cells.Count - 1
                Dim col As TableCell = e.Row.Cells(x)
                Dim encoded As String = col.Text
                col.Text = Context.Server.HtmlDecode(encoded)
            Next

            e.Row.Cells(0).Text = "<a href='http://" & Request.ServerVariables("server_name") & "/review_record.aspx?ID=" & e.Row.Cells(0).Text & "'>" & e.Row.Cells(0).Text & "</a>"

            e.Row.Cells(2).Width = 150

            If Not isExporting Then
                If header_cell <> -1 Then
                    If Len(e.Row.Cells(header_cell).Text) > 1 Then
                        e.Row.Cells(header_cell).Text = "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(e.Row.Cells(header_cell).Text) & "</div></i></a>"
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub ddlAgentGroupxp_DataBound(sender As Object, e As EventArgs) Handles ddlAgentGroupxp.DataBound
        If ddlAgentGroupxp.SelectedValue <> "" Then
            Session("SelectedAgent") = ddlAgentGroupxp.SelectedValue
        End If

        If Session("SelectedAgent") <> "" Then
            Try
                ddlAgentGroupxp.SelectedValue = Session("SelectedAgent")
            Catch ex As Exception

            End Try

        End If

        ddlAgentGroupxp.Items.Insert(0, New ListItem("(Select Group)", ""))
    End Sub


    Protected Sub ddlAgentGroupxp_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlAgentGroupxp.SelectedIndexChanged


        ddlAgentByGroupxp.Items.Clear()
        ddlAgentByGroupxp.Items.Add(New ListItem("(Select)", ""))

        If ddlAgentGroupxp.SelectedValue = "ALL" Or ddlAgentGroupxp.SelectedValue = "" Then
            ddlAgentByGroupxp.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & ddlApps.SelectedValue & "' order by AGent")
        End If

        If ddlAgentGroupxp.SelectedIndex > 1 Then
            ddlAgentByGroupxp.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlAgentGroupxp.SelectedValue & "' and  appname = '" & ddlApps.SelectedValue & "' order by AGent")
        End If
        ddlAgentByGroupxp.DataBind()



    End Sub

    Protected Sub btnSupeExportxp_Click(sender As Object, e As System.EventArgs) Handles btnSupeExportxp.Click
        isExporting = True
        btnAgentGroupxp_Click(sender, e)
        GV_to_CSV(gvAgentGroupxp, "ExpandedReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=ExpandedReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"


        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvAgentGroupxp.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString().Replace("<br>", "").Replace("<a", "<i").Replace("</a", "</i"))
        Response.Flush()
        Response.End()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub


    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton2.Click

        Dim previousStartDate As DateTime = New DateTime(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month, 1)
        Dim previousEndDate As DateTime = New DateTime(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month))

        txtGroupStartxp.Text = previousStartDate.ToString("d")
        txtgroupEndxp.Text = previousEndDate.ToString("d")
        btnAgentGroupxp_Click(sender, e)
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) 'Handles LinkButton1.Click
        Dim currentStartDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
        Dim currentEndDate As DateTime = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))

        txtGroupStartxp.Text = currentStartDate.ToString("d")
        txtgroupEndxp.Text = currentEndDate.ToString("d")
        btnAgentGroupxp_Click(sender, e)
    End Sub

    Protected Sub lbCurrentWeek_Click(sender As Object, e As System.EventArgs) 'Handles lbCurrentWeek.Click
        Dim currentStartDate As DateTime = DateAdd(DateInterval.Day, -Now.DayOfWeek, Now)
        Dim currentEndDate As DateTime = DateAdd(DateInterval.Day, 6, currentStartDate)

        txtGroupStartxp.Text = currentStartDate.ToString("d")
        txtgroupEndxp.Text = currentEndDate.ToString("d")
        btnAgentGroupxp_Click(sender, e)
    End Sub

    Protected Sub btnViewSummary_Click(sender As Object, e As System.EventArgs) Handles btnViewSummary.Click
        Response.Redirect("ExpandedView.aspx?" & Request.QueryString.ToString)
        'Response.Redirect("ExpandedView.aspx?StartDate=" & txtGroupStartxp.Text & "&EndDate=" & txtgroupEndxp.Text)
    End Sub

    Protected Sub btnAgentGroupxp_PreRender(sender As Object, e As System.EventArgs) Handles btnAgentGroupxp.PreRender
    
        If gvAgentGroupxp.HeaderRow IsNot Nothing Then
            gvAgentGroupxp.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub ddlAgentByGroupxp_DataBound(sender As Object, e As EventArgs) Handles ddlAgentByGroupxp.DataBound
        ddlAgentByGroupxp.Items.Insert(0, New ListItem("(Select Agent)", ""))
    End Sub

    Protected Sub ddlAgentByGroupxp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAgentByGroupxp.SelectedIndexChanged

        Session("SelectedAgent") = ddlAgentGroupxp.SelectedValue

    End Sub
End Class
