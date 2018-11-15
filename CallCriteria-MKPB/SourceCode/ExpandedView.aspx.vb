Imports System.IO
Imports Common
Imports System.Data.SqlClient
Imports System.Data

Partial Class ExpandedView
    Inherits System.Web.UI.Page

    Dim pfl As ProfileCommon
    Dim header_cell As Integer = -1

    Dim agent_cell As Integer = -1
    Dim missed_cell As Integer = -1


    Public data_rate As String = ""

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
        Else
            data_rate = 0.05
        End If


        pfl = Profile.GetProfile(User.Identity.Name)

        If User.IsInRole("Admin") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("Calibrator") Or User.IsInRole("Qa Lead") Or User.IsInRole("Client") Then
        Else
            Response.Redirect("default.aspx")
        End If

        If Session("appname") Is Nothing Then
            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            Session("appname") = domain(0)
        End If

        If Not Page.IsPostBack Then

            If User.IsInRole("Client") Then
                ' QA_Weekly_div.Visible = False
            End If

            Dim totalDaysinMonth As Int32 = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)


            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                txtGroupStartxp.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
                Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Else
                txtGroupStartxp.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                txtgroupEndxp.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Else
                txtgroupEndxp.Text = Session("EndDate")
            End If


            If Request("filter") IsNot Nothing Then
                hdnExtraFilters.Value = Request("filter")
            End If

            If User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("QA Lead") Or User.IsInRole("Calibrator") Then

                ddlAgent.Items.Clear()

                ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & pfl.Group & "' and  appname = '" & Session("appname") & "' and convert(date, call_date) between '" & txtGroupStartxp.Text & "' and '" & txtgroupEndxp.Text & "' order by AGent")
                ddlAgent.DataBind()
                ddlAgent.Items.Insert(0, New ListItem("(Select)", ""))

                If ddlGroup.Items.Contains(New ListItem(pfl.Group)) Then
                Else
                    ddlGroup.Items.Add(New ListItem(pfl.Group))
                End If
                ddlGroup.SelectedValue = pfl.Group
                ddlGroup.Enabled = False

            Else
                ddlAgent.Items.Clear()
                ddlAgent.Items.Add(New ListItem("(Select)", ""))
                ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and convert(date, call_date) between '" & txtGroupStartxp.Text & "' and '" & txtgroupEndxp.Text & "'  order by AGent")
                ddlAgent.DataBind()
            End If

            If Request("filter") IsNot Nothing Then
                hdnFilter.Value = Request("filter")

                Dim filtered As NameValueCollection = New NameValueCollection(Request.QueryString)
                filtered.Remove("filter")

            End If

            ddlGroup.DataBind()
            If Session("SelectedGroup") <> "" Then
                ddlGroup.SelectedValue = Session("SelectedGroup")
            End If


            If Session("SelectedAgent") <> "" Then
                Try
                    ddlAgent.SelectedValue = Session("SelectedAgent")
                Catch ex As Exception

                End Try

            End If



            ' btnAgentGroupxp_Click(sender, e)


        End If



    End Sub
    Protected Sub btnAgentGroupxp_Click(sender As Object, e As System.EventArgs) Handles btnAgentGroupxp.Click


        Dim tz_offset = "0"

        Dim where_clause As String = " where fs.appname in (select appname from userapps where username = '" & User.Identity.Name & "') "
        If txtGroupStartxp.Text <> "" Then
            where_clause &= " and  convert(date, call_date) >= '" & txtGroupStartxp.Text & "' "
        End If

        If txtgroupEndxp.Text <> "" Then
            where_clause &= " and  convert(date, call_date) <= '" & txtgroupEndxp.Text & "' "
        End If

        If ddlGroup.SelectedValue <> "" Then
            where_clause &= " and Agent_group = '" & ddlGroup.SelectedValue & "' "
        End If

        If ddlAgent.SelectedValue <> "" Then
            where_clause &= " and Agent = '" & ddlAgent.SelectedValue & "' "
        End If


        If Request("ShortName") IsNot Nothing Then
            where_clause &= "and f_id in(select * from dbo.GetFormListByShortName('" & Request("shortname") & "','" & Session("appname") & "')) "
        End If

        'where_clause &= hdnExtraFilters.Value


        'Response.Write(where_clause)
        'Response.End()

        dsFormData.SelectParameters("where_clause").DefaultValue = where_clause

      

        If ddlGroup.SelectedValue <> "" Then
            litGroupFilter.Text = ddlGroup.SelectedValue
            'agent_group_filter = " and AGENT_GROUP = '" & ddlGroup.SelectedValue & "' "
        Else
            litGroupFilter.Text = "All Groups"
        End If


        If ddlAgent.SelectedValue <> "" Then
            litGroupFilter.Text &= " " & ddlAgent.SelectedValue
            'agent_group_filter &= " and AGENT = '" & ddlAgent.SelectedValue & "' "
        End If

        litDateRange.Text = txtGroupStartxp.Text & " - " & txtgroupEndxp.Text

    End Sub

    Protected Sub gvAgentGroupxp_DataBound(sender As Object, e As System.EventArgs) Handles gvAgentGroupxp.DataBound
        If gvAgentGroupxp.Rows.Count > 0 Then
            btnSupeExportxp.Visible = True
        Else
            btnSupeExportxp.Visible = False
        End If

        lblRows.Text = gvAgentGroupxp.Rows.Count & " Rows"
    End Sub

    Protected Sub gvAgentGroupxp_PreRender(sender As Object, e As EventArgs) Handles gvAgentGroupxp.PreRender
      
       
        If gvAgentGroupxp.HeaderRow IsNot Nothing Then
            gvAgentGroupxp.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub


    Protected Sub gvAgentGroupxp_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAgentGroupxp.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then

            If (User.IsInRole("Client") Or User.IsInRole("Supervisor")) Then
                e.Row.Cells(1).Visible = False
            End If

            Dim cell_count As Integer = 0
            For Each tc As TableCell In e.Row.Cells

                Dim hl As LinkButton = tc.Controls(0)

                If hl.Text = "id" Then
                    hl.Text = "Play"
                End If

                If hl.Text = "Comments" Then
                    header_cell = cell_count
                End If

                If hl.Text = "Agent" Then
                    agent_cell = cell_count
                End If

                If hl.Text = "Missed" Then
                    missed_cell = cell_count
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


            'e.Row.Cells(0).Text = "<a href='http://" & Request.ServerVariables("server_name") & "/review_record.aspx?ID=" & e.Row.Cells(0).Text & "'>" & e.Row.Cells(0).Text & "</a>"

            e.Row.Cells(0).Text = "<a class='play-option main-cta' href='#' onclick='javascript:window.open(" & Chr(34) & "http://" & Request.ServerVariables("server_name") & "/review_record.aspx?ID=" & e.Row.Cells(0).Text & Chr(34) & ", " & Chr(34) & "_blank" & Chr(34) & ");'><i class='fa fa-play'></i></a>"

            Try
                e.Row.Cells(3).Text = FormatNumber(e.Row.Cells(3).Text, 2)
                e.Row.Cells(4).Text = FormatNumber(e.Row.Cells(4).Text, 2)

            Catch ex As Exception

            End Try

            If header_cell <> -1 And Len(e.Row.Cells(header_cell).Text) > 1 Then

                Dim comments() As String = e.Row.Cells(header_cell).Text.Split("|")
                If comments.Length > 1 Then
                    e.Row.Cells(header_cell).Text = ""


                    For Each comment In comments
                        If Trim(comment) <> "" Then
                            e.Row.Cells(header_cell).Text &= "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(comment) & "</div></i></a>"
                        End If
                    Next

                    'If Trim(comments(0)) <> "" Then
                    '    e.Row.Cells(header_cell).Text = "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(comments(0)) & "</div></i></a>"
                    'End If
                    'If Trim(comments(1)) <> "" Then
                    '    e.Row.Cells(header_cell).Text &= "<a class='comments-trigger2' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(comments(1)) & "</div></i></a>"
                    'End If

                    'If Trim(comments(2)) <> "" Then
                    '    e.Row.Cells(header_cell).Text &= "<a class='comments-trigger2' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(comments(2)) & "</div></i></a>"
                    'End If

                Else
                    e.Row.Cells(header_cell).Text = "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(e.Row.Cells(header_cell).Text) & "</div></i></a>"
                End If




            End If

            If agent_cell <> -1 And Len(e.Row.Cells(agent_cell).Text) > 1 Then
                e.Row.Cells(agent_cell).Text = "<a href=" & Chr(34) & "expandedview.aspx?filter= and agent= '" & Trim(e.Row.Cells(agent_cell).Text) & "'" & Chr(34) & ">" & Trim(e.Row.Cells(agent_cell).Text) & "</a>"
            End If
            If Not User.IsInRole("QA") Then
                If missed_cell <> -1 And Len(e.Row.Cells(missed_cell).Text) > 1 Then
                    Dim replacement_text As String = ""
                    Dim mq_list() As String = e.Row.Cells(missed_cell).Text.Split(",")
                    For Each mq As String In mq_list

                        Dim q_pos As DataTable = GetTable("select case when q_position > call_length then call_length - 20 else q_position end as q_position, audio_link, form_score3.id as form_id from form_score3 join form_q_scores on form_score3.id= form_q_scores.form_id " & _
                            "join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID join Questions on questions.id = form_q_scores.question_id  " & _
                            "where form_score3.id = " & e.Row.DataItem("id").ToString & " and q_short_name='" & Trim(mq) & "'")

                        If q_pos.Rows.Count > 0 Then
                            'Response.Write("select q_position, audio_link from form_score3 join form_q_scores on form_score3.id= form_q_scores.form_id " & _
                            '    "join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID join Questions on questions.id = form_q_scores.question_id  " & _
                            '    "where form_score3.id = " & drv("form_id").ToString & " and q_short_name='" & mq & "'")
                            'Response.End()

                            If replacement_text <> "" Then
                                replacement_text &= ", <a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");event.stopPropagation();'>" & mq & "</a>"
                            Else
                                replacement_text &= "<a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");event.stopPropagation();'>" & mq & "</a>"
                            End If
                        End If
                    Next
                    e.Row.Cells(missed_cell).Text = replacement_text
                    'e.Row.Cells(5).Attributes.Add("onclick", "event.stopPropagation();")
                End If
            End If

            'Format Comments


            'If missed_cell <> -1 And Len(e.Row.Cells(missed_cell).Text) > 1 Then
            '    Dim new_href As String = ""
            '    Dim href_list() As String = e.Row.Cells(missed_cell).Text.Split(",")
            '    For Each href_item As String In href_list
            '        If new_href = "" Then
            '            new_href = "<a href=" & Chr(34) & "expandedview.aspx?filter= and fs.id in (select * from dbo.GetFormListByShortName('" & href_item & "','" & Session("appname") & "'))" & Chr(34) & ">" & href_item & "</a>"
            '        Else
            '            new_href = new_href & "," & "<a href=" & Chr(34) & "expandedview.aspx?filter= and fs.id in (select * from dbo.GetFormListByShortName('" & href_item & "','" & Session("appname") & "'))" & Chr(34) & ">" & href_item & "</a>"
            '        End If
            '    Next

            '    e.Row.Cells(missed_cell).Text = new_href
            'End If






        End If





    End Sub

    Protected Sub ddlGroup_DataBound(sender As Object, e As EventArgs) Handles ddlGroup.DataBound
        If Session("SelectedGroup") <> "" Then
            ddlGroup.SelectedValue = Session("SelectedGroup")
            ddlGroup_SelectedIndexChanged(sender, e)
            btnAgentGroupxp_Click(sender, e)
        End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged

        ddlAgent.Items.Clear()
        ddlAgent.Items.Add(New ListItem("ALL", ""))

        If ddlGroup.SelectedValue = "ALL" Or ddlGroup.SelectedValue = "" Then
            'agentHolder.Visible = False
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "' and convert(date, call_date) >= '" & txtGroupStartxp.Text & "' and convert(date, call_date) <= '" & txtgroupEndxp.Text & "'  order by AGent")
        End If

        If ddlGroup.SelectedIndex > 1 Then
            'agentHolder.Visible = True
            ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and convert(date, call_date) >= '" & txtGroupStartxp.Text & "' and convert(date, call_date) <= '" & txtgroupEndxp.Text & "'  order by AGent")
        End If
        ddlAgent.DataBind()

        Session("SelectedGroup") = ddlGroup.SelectedValue

        btnAgentGroupxp_Click(sender, e)

    End Sub

    Protected Sub btnSupeExportxp_Click(sender As Object, e As System.EventArgs) Handles btnSupeExportxp.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=ExpandedReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        btnAgentGroupxp_Click(sender, e)

        'For Each gvr As GridViewRow In gvAgentGroupxp.Rows
        '    If gvr.RowType = DataControlRowType.DataRow Then
        '        Dim hl As HyperLink = gvr.Cells(0).Controls(0)
        '        hl.NavigateUrl = "http://" & Request.ServerVariables("SERVER_NAME") & "/" & hl.NavigateUrl
        '    End If
        'Next
        'gvAgentGroupxp.Columns(0).Visible = False

        rptExpanded.DataSourceID = "dsFormData"
        rptExpanded.DataBind()
        rptExpanded.RenderControl(hw)
       
        'gvAgentGroupxp.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;} table{border-collapse:collapse;border:1px solid #FF0000;} table td{border:1px solid #FF0000;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString().Replace("<br>", "").Replace("|", ""))
        'Email_Error(sw.ToString())
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

    Protected Sub btnViewDetail_Click(sender As Object, e As System.EventArgs) Handles btnViewDetail.Click
        'Response.Redirect("ExpandedView_Detail.aspx?" & Request.QueryString.ToString)
        Response.Redirect("ExpandedView_Detail.aspx?StartDate=" & txtGroupStartxp.Text & "&EndDate=" & txtgroupEndxp.Text)

    End Sub

    Protected Sub gvAgentGroupxp_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAgentGroupxp.Sorting

    End Sub

    Protected Sub ddlAgent_DataBound(sender As Object, e As EventArgs) Handles ddlAgent.DataBound
        If ddlAgent.SelectedValue <> "" Then
            Session("SelectedAgent") = ddlAgent.SelectedValue
        End If

        If Session("SelectedAgent") <> "" Then
            Try
                ddlAgent.SelectedValue = Session("SelectedAgent")
            Catch ex As Exception

            End Try

        End If
    End Sub

    Protected Sub ddlAgent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAgent.SelectedIndexChanged
        Session("SelectedAgent") = ddlAgent.SelectedValue

        btnAgentGroupxp_Click(sender, e)
    End Sub

    Protected Sub btnAgree_Click(sender As Object, e As System.EventArgs)
        Dim ack_by As String = ""


        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If



        'Check to see if a notes only needs to be created
        Dim not_dt As DataTable = GetTable("select count(*) from form_notifications and date_closed is null where form_id = " & hdnOpenFormID.Value)
        If not_dt.Rows.Count > 0 Then
            If not_dt.Rows(0).Item(0).ToString = "0" Then
                UpdateTable("insert into Notifications (form_id, assigned_to, comment, dateadded, acknowledged, ack_date, ack_by, qa_who, qa_view) select '" & hdnOpenFormID.Value & "','NotesOnly','',dbo.getMTDate(),1,dbo.getMTDate(),'" & ack_by & "','" & ack_by & "',dbo.getMTDate()")
            End If
        End If


        Dim sql As String
        If txtComments.Text <> "" Then
            sql = "Update Notifications set acknowledged = 1, ack_date = dbo.getMTDate(), comment = isnull(comment,'') + @new_comments, ack_by = '" & ack_by & "', qa_view = dbo.getMTDate(), qa_who = '" & ack_by & "' where form_id = " & hdnOpenFormID.Value
        Else
            sql = "Update Notifications set acknowledged = 1, ack_date = dbo.getMTDate(), ack_by = '" & ack_by & "', qa_view = dbo.getMTDate(), qa_who = '" & ack_by & "' where form_id = " & hdnOpenFormID.Value
        End If

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()


        'Email_Error(sql)

        Dim reply As New SqlCommand(sql, cn)
        reply.CommandTimeout = 60
        'If txtComments.Text <> "" Then
        reply.Parameters.AddWithValue("new_comments", "<br>" & HttpContext.Current.User.Identity.Name & " (" & Now.ToString & ") - " & txtComments.Text)
        'End If
        reply.ExecuteNonQuery()
        cn.Close()
        cn.Dispose()

        Response.Redirect("expandedview.aspx?" & Request.QueryString.ToString)

    End Sub

    Protected Sub btnDisagree_Click(sender As Object, e As System.EventArgs)
        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If


        'Check to see if a notes only needs to be created
        Dim not_dt As DataTable = GetTable("select count(*) from Notifications where form_id = " & hdnOpenFormID.Value)
        If not_dt.Rows.Count > 0 Then
            If not_dt.Rows(0).Item(0).ToString = "0" Then
                UpdateTable("insert into Notifications (form_id, assigned_to, comment, dateadded, acknowledged, ack_date, ack_by, qa_who, qa_view) select '" & hdnOpenFormID.Value & "','NotesOnly','',dbo.getMTDate(),1,dbo.getMTDate(),'" & ack_by & "','" & ack_by & "',dbo.getMTDate()")
            End If
        End If

        Dim sql As String
        If txtComments.Text <> "" Then
            sql = "Update Notifications set acknowledged = 1, ack_date = dbo.getMTDate(), comment = isnull(comment,'') + @new_comments, ack_by = '" & ack_by & "' where form_id = " & hdnOpenFormID.Value
        Else
            sql = "Update Notifications set acknowledged = 1, ack_date = dbo.getMTDate(), ack_by = '" & ack_by & "' where form_id = " & hdnOpenFormID.Value
        End If

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        'Email_Error(sql)

        Dim reply As New SqlCommand(sql, cn)
        reply.CommandTimeout = 60
        'If txtComments.Text <> "" Then
        reply.Parameters.AddWithValue("new_comments", "<br><b>" & HttpContext.Current.User.Identity.Name & "</b> (" & Now.ToString & ") - " & txtComments.Text)
        'End If
        reply.ExecuteNonQuery()
        cn.Close()
        cn.Dispose()

        Response.Redirect("expandedview.aspx?" & Request.QueryString.ToString)
    End Sub

    Protected Sub btnRefer_Click(sender As Object, e As System.EventArgs)

        Dim sql As String
        If txtComments.Text <> "" Then
            sql = "Update Notifications set assigned_to = 'Manager', comment = isnull(comment,'') + @new_comments where form_id = " & hdnOpenFormID.Value
        Else
            sql = "Update Notifications set assigned_to = 'Manager' where form_id = " & hdnOpenFormID.Value
        End If

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim reply As New SqlCommand(sql, cn)
        reply.CommandTimeout = 60
        If txtComments.Text <> "" Then
            reply.Parameters.AddWithValue("new_comments", "<br><b>" & HttpContext.Current.User.Identity.Name & "</b> (" & Now.ToString & ") - " & txtComments.Text)
        End If
        reply.ExecuteNonQuery()
        cn.Close()
        cn.Dispose()

        Response.Redirect("My_Notifications.aspx")
    End Sub


    Protected Sub dsFormData_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsFormData.Selecting
        e.Command.CommandTimeout = 90
    End Sub
End Class
