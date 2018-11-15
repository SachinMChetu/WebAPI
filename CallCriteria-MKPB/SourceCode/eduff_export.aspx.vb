Imports Common
Imports System.Data
Imports System.Data.SqlClient

Partial Class eduff_export
    Inherits System.Web.UI.Page

    Private Sub eduff_export_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=eduff_export.aspx")
        End If

        If Not IsPostBack Then
            dsAgents.SelectParameters("username").DefaultValue = User.Identity.Name
            dsCampaigns.SelectParameters("username").DefaultValue = User.Identity.Name
            dsgroups.SelectParameters("username").DefaultValue = User.Identity.Name
            dsScorecard.SelectParameters("username").DefaultValue = User.Identity.Name
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click


        Dim filter As String = ""
        If ddlAgent.SelectedValue <> "" Then

            Dim agent_list As String = "'asdasdasdsadads'"
            For x = 0 To ddlAgent.Items.Count - 1
                If ddlAgent.Items.Item(x).Selected Then
                    agent_list &= ",'" & ddlAgent.Items.Item(x).Text & "'"
                End If

            Next

            filter &= " and vwForm.agent in (" & agent_list & ") "
        End If

        If ddlScorecard.SelectedValue <> "" Then
            filter &= " and vwForm.scorecard = '" & ddlScorecard.SelectedValue & "' "
        End If


        If ddlGroup.SelectedValue <> "" Then
            filter &= " and vwForm.agent_group = '" & ddlGroup.SelectedValue & "' "
        End If

        If ddlCampaign.SelectedValue <> "" Then
            filter &= " and vwForm.campaign = '" & ddlCampaign.SelectedValue & "' "
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
        reply.SelectCommand.Parameters.AddWithValue("start", txtStart.Text)
        reply.SelectCommand.Parameters.AddWithValue("end", txtEnd.Text)
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

            GV_to_CSV(gvDetails, "Edufficient Details Export")

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
End Class
