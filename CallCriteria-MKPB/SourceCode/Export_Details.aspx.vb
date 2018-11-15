Imports Common
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Partial Class Export_Details
    Inherits System.Web.UI.Page

    Dim comment_header As Integer = 0
    Dim missed_list_header As Integer = 0
    Dim call_id_header As Integer = 0
    Dim column_count As Integer = 0
    Dim blank_columns() As Integer


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load



        Dim filter As String = ""
        If Request("agent") <> "" And Request("agent") <> "undefined" Then
            filter &= " and vwForm.agent = '" & Request("agent") & "' "
        End If

        If Request("appname") <> "" And Request("appname") <> "undefined" Then
            filter &= " and vwForm.appname = '" & Request("appname") & "' "
        End If

        If Request("pass_fail") <> "" And Request("pass_fail") <> "undefined" Then
            filter &= " and vwForm.pass_fail = '" & Request("pass_fail") & "' "
        End If

        If Request("scorecard") <> "" And Request("scorecard") <> "undefined" Then
            filter &= " and vwForm.scorecard = '" & Request("scorecard") & "' "
        End If

        If Request("agent_group") <> "" And Request("agent_group") <> "undefined" Then
            filter &= " and vwForm.agent_group = '" & Request("agent_group") & "' "
        End If

        If Request("campaign") <> "" And Request("campaign") <> "undefined" Then
            filter &= " and vwForm.campaign = '" & Request("campaign") & "' "
        End If

        If Request("QID") <> "" And Request("QID") <> "undefined" Then
            filter &= " and vwForm.f_id in (select form_id from form_q_scores where question_id = " & Request("QID") & " and question_answered in (select id from question_answers where question_id = " & Request("QID") & " and right_answer = 0)  ) " ' "
        End If

        Dim this_user As String = HttpContext.Current.User.Identity.Name


        'start_date=11/23/2015&end_date=12/7/2015&agent_group=CEHE&Agent=undefined&QID=undefined

        'Response.Write(filter)
        'Response.End()


        Dim myRole As String = ""
        Dim user_roles() As String = Roles.GetRolesForUser(this_user)
        For Each role In user_roles
            myRole = role
        Next


        'If Request("agent") <> "" Then
        '    ' Email_Error(hdnAgentFilter) 
        '    ' and vwform.agent = 'Nyla Sophas' *
        '    Dim agent As String = Request("agent")
        '    this_user = agent
        '    myRole = "Agent"
        'End If



        Dim called_sp As String = "getDetailData"

        If HttpContext.Current.Request.Cookies.Item("filter") IsNot Nothing Then
            called_sp = "getDetailDataArray"
        End If


        Dim user_col_count_dt As DataTable = GetTable("select * from available_columns with (nolock) join user_columns with (nolock) on user_columns.column_id = available_columns.id where username = '" & HttpContext.Current.User.Identity.Name & "' order by col_order")
        If user_col_count_dt.Rows.Count > 0 Then

            called_sp = "getDetailDataCustom"

        End If


        'Response.Write("*" + HttpContext.Current.Request.Cookies.Item("filter").Value + "*")
        'Response.Write(called_sp)
        'Response.End()


        Dim dt As New DataTable

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim reply As New SqlDataAdapter(called_sp, cn)
        reply.SelectCommand.CommandType = CommandType.StoredProcedure

        reply.SelectCommand.Parameters.AddWithValue("username", User.Identity.Name)
        reply.SelectCommand.Parameters.AddWithValue("start", Request("start_date"))
        reply.SelectCommand.Parameters.AddWithValue("end", Request("end_date"))
        reply.SelectCommand.Parameters.AddWithValue("filter", filter)
        reply.SelectCommand.Parameters.AddWithValue("user", User.Identity.Name)
        reply.SelectCommand.Parameters.AddWithValue("user_role", myRole)
        reply.SelectCommand.Parameters.AddWithValue("pagenum", 1)

        If HttpContext.Current.Request.Cookies.Item("filter") IsNot Nothing Then
            reply.SelectCommand.Parameters.AddWithValue("filterarray", HttpContext.Current.Request.Cookies.Item("filter").Value)
        Else
            reply.SelectCommand.Parameters.AddWithValue("filterarray", "")
        End If
        reply.SelectCommand.Parameters.AddWithValue("pagerows", 100000)
        'reply.SelectCommand.CommandTimeout = 90
        reply.Fill(dt)

        'HttpContext.Current.Response.End()
        cn.Close()
        cn.Dispose()


        'If called_sp <> "getDetailDataArray" Then
        '    dt.Columns.Remove("Call Date")
        '    dt.Columns.Remove("Phone")

        'End If

        'dt.Columns.Remove("Session ID")
        dt.Columns.Remove("non_edit")
        dt.Columns.Remove("NotificationID")
        dt.Columns.Remove("notificationStep")
        dt.Columns.Remove("user_role")
        dt.Columns.Remove("cali_id")
        dt.Columns.Remove("play_btn_class")
        dt.Columns.Remove("OwnedNotification")
        dt.Columns.Remove("wasEdited")
        dt.Columns.Remove("website")
        dt.Columns.Remove("QA")

        If Not {"Admin", "QA Lead", "QA"}.Contains(Roles.GetRolesForUser(User.Identity.Name).Single) Then
            dt.Columns.Remove("Missed List")
        End If


        dt.Columns.Remove("bad_call_reason")
        dt.Columns.Remove("number")

        Try
            dt.Columns.Remove("timestamp")


        Catch ex As Exception

        End Try


        Try
            dt.Columns.Remove("efficiency")

        Catch ex As Exception

        End Try




        'Phone   QA		Call Date	Call ID			



        'dt = GetTable("getDetailData '" & Request("appname") & "','" & Request("start_date") & "','" & Request("end_date") & "','" & Replace(filter, "'", "''") & "','" & 1 & "','10000'") '
        gvDetails.DataSource = dt
        gvDetails.DataBind()





        If Request("print") = "" Then

            GV_to_CSV(gvDetails, "Export_Details")

            'Response.Clear()
            'Response.Buffer = True
            'Response.AddHeader("content-disposition", "attachment;filename=Export_Details.xls")
            'Response.Charset = ""
            'Response.ContentType = "application/vnd.ms-excel"
            'Dim sw As New StringWriter()
            'Dim hw As New HtmlTextWriter(sw)

            'gvDetails.RenderControl(hw)
            ''style to format numbers to string
            'Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
            'Response.Write(style)
            'Response.Output.Write(sw.ToString())
            'Response.Flush()
            'Response.End()

        End If





    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub


    Protected Sub gvDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim cell_count As Integer = 0
            For x = 0 To e.Row.Cells.Count - 1
                If e.Row.Cells(x).Text.ToUpper = "COMMENTS" Then
                    comment_header = x
                End If
                If e.Row.Cells(x).Text.ToUpper = "MISSED ITEMS" Then
                    missed_list_header = x
                    e.Row.Cells(x).Visible = False
                End If

                If e.Row.Cells(x).Text.ToUpper = "CALL ID" Then
                    call_id_header = x
                End If



                If x > 19 Then
                    e.Row.Cells(x).Visible = False
                End If
            Next


            column_count = e.Row.Cells.Count - 1

            blank_columns = New Integer(column_count) {}

            For x As Integer = 0 To column_count
                blank_columns(x) = 0
            Next

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'If e.Row.DataItem("pass_fail") = "success" Then
            '    e.Row.Cells(0).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            'Else
            '    e.Row.Cells(0).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
            '    e.Row.Attributes.Add("class", "fail-row")
            'End If


            For x As Integer = 0 To column_count
                If e.Row.Cells(x).Text <> "&nbsp;" Then
                    blank_columns(x) = 1
                    If x > 15 Then
                        e.Row.Cells(x).Visible = False
                    End If
                End If
            Next


            'If e.Row.Cells(comment_header).Text <> "&nbsp;" Then


            e.Row.Cells(comment_header).Text = StripTagsCharArray(WebUtility.HtmlDecode(Regex.Replace(e.Row.Cells(comment_header).Text, "<[^>]*(>|$)", String.Empty)).Replace("|", "").Replace("â€™", "'"))
            e.Row.Cells(missed_list_header).Text = StripTagsCharArray(WebUtility.HtmlDecode(Regex.Replace(e.Row.Cells(missed_list_header).Text, "<[^>]*(>|$)", String.Empty)).Replace("</a", ""))
            e.Row.Cells(missed_list_header).Visible = False

            'For x As Integer = 1 To column_count
            '    e.Row.Cells(x).Visible = False
            'Next


            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;br&gt;", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;b&gt;", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;/b&gt;", ""))

            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("<br>", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("<b>", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("</b>", ""))

            'End If


            Dim drv As DataRowView = e.Row.DataItem

            If call_id_header <> 0 Then
                e.Row.Cells(call_id_header).Text = "<a href='http://" & Request.ServerVariables("SERVER_NAME") & "/review_record.aspx?ID=" & drv.Item("call id").ToString & "'>View Call</a>"

            End If


        End If

    End Sub
End Class
