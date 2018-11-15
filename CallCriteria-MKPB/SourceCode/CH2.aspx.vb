Imports Common
Imports System.Data
Imports System.IO
Imports System.ServiceModel.Web
Imports System.ServiceModel

Partial Class CH2
    Inherits System.Web.UI.Page


    Public myRole As String

    Public start_date As String
    Public end_date As String

    Public agent_group_filter As String = ""

    Public line_graph_data As String = ""
    Public line_graph_data_30 As String = ""
    Public line_graph_data_60 As String = ""

    Public min_line_range As String = "-90"
    Public max_line_range As String = "0"

    Public min_date As Date
    Public max_date As Date

    Public data_rate As String

    Dim no_details As Boolean = False



    Protected Sub Recalc()

    End Sub




    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            If user_info_dt.Rows(0).Item("speed_increment").ToString <> "" Then
                data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            Else
                data_rate = 0.05
            End If

        Else
            data_rate = 0.05
        End If


        Dim myRoles() As String = Roles.GetRolesForUser(User.Identity.Name)

        Try
            myRole = myRoles(0)
        Catch ex As Exception

        End Try



        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        If Not Roles.IsUserInRole("Admin") And Not Roles.IsUserInRole("Manager") And Not Roles.IsUserInRole("Supervisor") And Not Roles.IsUserInRole("Calibrator") And Not Roles.IsUserInRole("QA Lead") And Not Roles.IsUserInRole("Client") Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        'Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        'Session("appname") = domain(0)



        If Not IsPostBack Then

            ' hdnAppname.Value = domain(0)

            hdnScorecard.Value = Request("scorecard")

            'Dim client_dt As DataTable = GetTable("select * from app_settings where appname = '" & Session("appname") & "'")
            'If client_dt.Rows.Count > 0 Then
            '    litClientName.Text = client_dt.Rows(0).Item("FullName").ToString
            'End If

            date1.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")

            date2.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")

            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                date1.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
                Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Else
                date1.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                date2.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            Else
                date2.Text = Session("EndDate")
            End If

            'dsMySessions.SelectParameters("selectedDate").DefaultValue = DateAdd(DateInterval.Day, -1, Today).ToString("MM/dd/yyyy")
            'gvUserSessions.DataBind()


            'ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "'  order by AGent")
            'ddlAgent.DataBind()


            dsModules.SelectParameters("username").DefaultValue = User.Identity.Name
            dsBilling.SelectParameters("username").DefaultValue = User.Identity.Name

            Recalc_Elements()

        End If




    End Sub



    Protected Sub Recalc_Elements()

        ''litGroupFilter.Text = "All Agents"

        If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

            Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            If userProfile.Group <> "" Then
                agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "
                'litGroupFilter.Text = userProfile.Group
            End If
        End If


        Dim billed_dt As DataTable = GetTable("getBilling '" & User.Identity.Name & "'")
        If billed_dt.Rows.Count > 0 And (User.IsInRole("Client") Or User.IsInRole("Admin")) Then
            'Try
            '    litLastBilled.Text = CDate(billed_dt.Rows(0).Item("max_bill_date")).ToString("MM/dd/yyyy")
            'Catch ex As Exception

            'End Try

            litPendTime.Text = billed_dt.Rows(0).Item("unpaidTime").ToString
            Try
                litPendAmount.Text = FormatCurrency(billed_dt.Rows(0).Item("unpaid_amount").ToString, 2)
            Catch ex As Exception

            End Try

            litPendNumberCalls.Text = billed_dt.Rows(0).Item("unpaid_calls").ToString

            If billed_dt.Rows(0).Item("over_due_amount").ToString <> "" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, "make_red", "$('.filterBtn2').css('background-color','red');", True)
            End If

            'Try
            '    litBilledAmount.Text = FormatCurrency(billed_dt.Rows(0).Item("current_due_amount").ToString, 2)
            'Catch ex As Exception

            'End Try

            'litBilledTime.Text = billed_dt.Rows(0).Item("current_due_time").ToString '
            'litBilledCalls.Text = billed_dt.Rows(0).Item("current_due_calls").ToString
            'If billed_dt.Rows(0).Item("over_due_amount").ToString <> "" Then
            '    overduerow.Visible = True

            '    Try
            '        litODAmount.Text = FormatCurrency(billed_dt.Rows(0).Item("over_due_amount").ToString, 2)
            '    Catch ex As Exception

            '    End Try


            '    litODTime.Text = billed_dt.Rows(0).Item("over_due_time").ToString
            '    litODnumber.Text = billed_dt.Rows(0).Item("over_due_calls").ToString
            'Else
            '    overduerow.Visible = False
            'End If



            '			

            'Try
            '    litBilledAmount.Text = FormatCurrency(billed_dt.Rows(0).Item(1).ToString, 2)
            'Catch ex As Exception

            'End Try

        End If

        'Dim toal_failed As DataTable
        'Select Case UCase(Session("appname"))
        '    Case "EDSOUP"
        '        'toal_failed = GetTable("select count(*) as num_left from dbo.XCC_REPORT_NEW b join form_score3 f on f.review_id = b.id  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and has_cardinal = 1  and f.appname = '" & Session("appname") & "' and call_date between '" & date1.Text & "' and '" & date2.Text & "'" & agent_group_filter)
        '        toal_failed = GetTable("select count(*) as num_left from vwForm  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and appname = '" & Session("appname") & "' and call_date between '" & date1.Text & "' and '" & date2.Text & "'" & agent_group_filter)
        '    Case Else
        '        toal_failed = GetTable("select count(*) as num_left from vwForm  where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and total_score = 0 and appname = '" & Session("appname") & "' and call_date between '" & date1.Text & "' and '" & date2.Text & "'" & agent_group_filter)
        'End Select



        start_date = date1.Text
        end_date = date2.Text


        'If Session("applied_filter") IsNot Nothing Then
        '    If Session("applied_filter") <> "" Then
        '        agent_group_filter = agent_group_filter & Session("applied_filter")
        '    End If
        'End If




    End Sub



    Protected Sub repeatQAListy(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) 'Handles repeatQAList.ItemDataBound
        Dim drv As DataRowView = e.Item.DataItem
        Dim lbl As Label = e.Item.FindControl("lblTotalReview")
        'Dim total_mins As Integer = drv.Item("total_mins")
        Dim total_secs As Integer = drv.Item("total_secs")
        'Dim addl_mins As Integer = total_secs / 60
        Dim hours As Integer = Math.Truncate(total_secs / 3600) ' (total_mins + addl_mins) / 60
        Dim total_mins As Integer = Math.Truncate((total_secs - hours * 3600) / 60)

        Dim hours_string As String = Right("00" & hours, 2)
        Dim minutes_string As String = Right("00" & total_mins, 2)
        Dim seconds_string As String = Right("00" & (total_secs Mod 60), 2)

        If hours > 0 Then
            lbl.Text = hours_string & ":" & minutes_string & ":" & seconds_string
        Else
            lbl.Text = minutes_string & ":" & seconds_string
        End If

        lbl = e.Item.FindControl("ReviewHour")
        lbl.Text = FormatNumber(3600 / (drv.Item("total_secs") / drv.Item("Number Reviews")), 2)

    End Sub


    Protected Sub dsQAReport_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs)

        e.Command.CommandTimeout = 90

    End Sub


    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) 'Handles btnSearch.Click


        'If Not txtSearch.Text Is String.Empty Then
        '    Select Case ddlSearch.SelectedValue
        '        Case "Phone Number"
        '            Response.Redirect("Search.aspx?phone=" + txtSearch.Text)
        '        Case "Agent Name"
        '            Response.Redirect("Search.aspx?Agent=" + txtSearch.Text)
        '        Case "Record ID"
        '            Response.Redirect("review_record.aspx?ID=" + txtSearch.Text)
        '    End Select

        'End If


    End Sub

    Protected Sub btnApplyFilter_Click(sender As Object, e As EventArgs) 'Handles lbApply.Click
        Recalc_Elements()
    End Sub

    Protected Sub btnZeroData_Click(sender As Object, e As EventArgs) 'Handles btnZeroData.Click
        'AFL.StartDate = date1.Text
        'AFL.EndDate = date2.Text
        'AFL.Score_Range = "0"
        'AFL.UpdateView()

    End Sub



    Protected Sub btnExportDetails()

        no_details = True
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=DetailReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        'gvQADetails.AllowPaging = False
        'Recalc_Elements()
        'gvQADetails.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub



    'Protected Sub gvQADetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvQADetails.PageIndexChanging
    '    Recalc_Elements()
    'End Sub




    'Protected Sub blPageNums_Click(sender As Object, args As EventArgs)
    '    'ListItem li = (ListItem)sender;
    '    'int page = Convert.ToInt32(li.Text) - 1;

    '    Dim bl As BulletedList = DirectCast(Page.FindControl("blPageNums"), BulletedList)
    '    gvQADetails.PageIndex = bl.SelectedIndex
    'End Sub


    Protected Sub btnTopAgents()
        Response.Redirect("all_agents.aspx")
    End Sub


    Protected Sub date1_TextChanged(sender As Object, e As EventArgs) Handles date1.TextChanged
        Session("StartDate") = date1.Text
        Recalc()
    End Sub

    Protected Sub date2_TextChanged(sender As Object, e As EventArgs) Handles date2.TextChanged
        Session("EndDate") = date2.Text
        Recalc()
    End Sub

    'Protected Sub ddlAgent_DataBound(sender As Object, e As EventArgs) Handles ddlAgent.DataBound
    '    If ddlAgent.SelectedValue <> "" Then
    '        Session("SelectedAgent") = ddlAgent.SelectedValue
    '    End If

    '    If Session("SelectedAgent") <> "" Then
    '        Try
    '            ddlAgent.SelectedValue = Session("SelectedAgent")
    '        Catch ex As Exception

    '        End Try

    '    End If
    'End Sub



    'Protected Sub ddlAgent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAgent.SelectedIndexChanged
    '    Session("SelectedAgent") = ddlAgent.SelectedValue
    'End Sub
    'Protected Sub ddlCampaign_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCampaign.SelectedIndexChanged
    '    Session("SelectedCampaign") = ddlCampaign.SelectedValue
    'End Sub


    'Protected Sub ddlCampaign_DataBound(sender As Object, e As EventArgs) Handles ddlCampaign.DataBound
    '    If ddlCampaign.SelectedValue <> "" Then
    '        Session("SelectedCampaign") = ddlCampaign.SelectedValue
    '    End If

    '    If Session("SelectedCampaign") <> "" Then
    '        Try
    '            ddlCampaign.SelectedValue = Session("SelectedCampaign")
    '        Catch ex As Exception

    '        End Try

    '    End If
    'End Sub

    'Protected Sub ddlGroup_DataBound(sender As Object, e As EventArgs) Handles ddlGroup.DataBound


    '    If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("Admin") Then

    '        Dim userProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
    '        If userProfile.Group <> "" Then

    '            ddlGroup.Visible = False
    '            agent_group_filter = " and AGENT_GROUP = '" & userProfile.Group & "' "

    '            If ddlGroup.Items.Contains(New ListItem(userProfile.Group)) Then
    '            Else
    '                ddlGroup.Items.Add(New ListItem(userProfile.Group))
    '            End If
    '            ddlGroup.SelectedValue = userProfile.Group
    '            ddlGroup.Enabled = False

    '            ddlAgent.Items.Clear()
    '            ddlAgent.Items.Add(New ListItem("All Agents", ""))

    '            If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
    '                ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "'  order by AGent")
    '                'Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '                'ddlAgent.Visible = False
    '            Else
    '                ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '                'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '                ddlAgent.Visible = True
    '            End If
    '            ddlAgent.DataBind()
    '        End If
    '    End If




    '    If Session("SelectedGroup") <> "" Then
    '        Try
    '            ddlGroup.SelectedValue = Session("SelectedGroup")
    '            ddlGroup_SelectedIndexChanged(sender, e)
    '            Recalc_Elements()
    '        Catch ex As Exception

    '        End Try

    '    End If
    'End Sub

    'Protected Sub ddlGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroup.SelectedIndexChanged
    '    ddlAgent.Items.Clear()
    '    ddlAgent.Items.Add(New ListItem("All Agents", ""))

    '    If ddlGroup.SelectedValue = "All" Or ddlGroup.SelectedValue = "" Then
    '        ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW]  with (nolock) where  appname = '" & Session("appname") & "'  and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "'  order by AGent")
    '        ' Email_Error("not filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '        'ddlAgent.Visible = False
    '    Else
    '        ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '        'Email_Error("filtered - SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '        ddlAgent.Visible = True
    '    End If

    '    'If ddlGroup.SelectedIndex > 0 Then
    '    '    ddlAgent.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' and call_date >= '" & date1.Text & "' and call_date <= '" & date2.Text & "' order by AGent")
    '    '    ddlAgent.Visible = True
    '    'End If
    '    ddlAgent.DataBind()

    '    Session("SelectedGroup") = ddlGroup.SelectedValue

    '    'Response.Write("SELECT distinct AGent FROM [XCC_REPORT_NEW] with (nolock)  where agent_group = '" & ddlGroup.SelectedValue & "' and  appname = '" & Session("appname") & "' order by AGent")
    '    'Response.End()





    'End Sub


    Protected Sub btnViewAllCalls()
        Response.Redirect("expandedview.aspx")
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    gvQADetails.AllowPaging = False
    '    gvQADetails.DataBind()

    '    'Response.Clear()
    '    Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter()
    '    Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
    '    gvQADetails.RenderControl(htmlWrite)
    '    'Response.Write(stringWrite.ToString())
    '    Dim this_div As New HtmlGenericControl
    '    this_div.InnerHtml = stringWrite.ToString()
    '    Session("ctrl") = this_div
    '    ClientScript.RegisterStartupScript(Me.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>")
    '    gvQADetails.AllowPaging = True
    '    gvQADetails.DataBind()
    'End Sub


    'Protected Sub rptMissed_ItemCommand(source As Object, e As RepeaterCommandEventArgs) 'Handles rptMissed.ItemCommand
    '    If e.CommandName = "Question" Then
    '        Session("applied_filter") = " and form_score3.id in (select form_id from dbo.GetFormListByShortName('" & e.CommandArgument & "','" & Session("appname") & "')) "
    '        Recalc_Elements()
    '        ClientScript.RegisterStartupScript(Me.GetType(), "refocus", "<script language=javascript>window.scrollTo(0, document.body.scrollHeight);</script>")
    '        gvQADetails.FooterRow.Focus()
    '    End If
    'End Sub

    'Protected Sub gvTBAgents_RowCommand(sender As Object, e As GridViewCommandEventArgs) 'Handles gvTBAgents.RowCommand
    '    Session("applied_filter") = " and agent = '" & e.CommandArgument & "'"
    '    Recalc_Elements()
    '    ClientScript.RegisterStartupScript(Me.GetType(), "refocus", "<script language=javascript>window.scrollTo(0, document.body.scrollHeight);</script>")
    '    gvQADetails.FooterRow.Focus()
    'End Sub

    'Protected Sub lbSubmitPie_Click(sender As Object, e As EventArgs) 'Handles lbSubmitPie.Click
    '    Select Case hdnSelectedRange.Value
    '        Case "< 80"
    '            Session("applied_filter") = " and total_score < 80 "
    '            Recalc_Elements()
    '        Case "80-90"
    '            Session("applied_filter") = " and total_score >= 80 and total_score < 90"
    '            Recalc_Elements()
    '        Case "90-100"
    '            Session("applied_filter") = " and total_score >= 90 and total_score < 100"
    '            Recalc_Elements()
    '        Case "100"
    '            Session("applied_filter") = " and total_score >= 100"
    '            Recalc_Elements()
    '    End Select

    'End Sub
    Public Class DBOptions
        Public value As String
        Public text As String
        Public selected As String
    End Class
    <OperationContract()>
    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json)>
    Public Function GetQAAgents(start_date As String, end_date As String, scorecard As String, group As String) As List(Of DBOptions)

        'start_date = "1/1/2015"
        'end_date = "3/1/2015"
        'appname = "edsoup"
        'group = "Edsoup"


        Dim mi_items As New List(Of DBOptions)

        Dim filter As String = ""

        If group <> "" Then
            filter &= " and agent_group ='" & group & "' "
        End If

        If scorecard <> "" Then
            filter &= " and scorecard ='" & scorecard & "' "
        End If


        If HttpContext.Current.User.IsInRole("Agent") Then
            filter &= " and agent ='" & HttpContext.Current.User.Identity.Name & "' "
        End If



        Dim stats_dt As DataTable

        stats_dt = GetTable("Select distinct reviewer from vwform and call_date >= '" & start_date & "' and call_date <= '" & end_date & "'")

        For Each dr In stats_dt.Rows
            Dim _mi As New DBOptions
            _mi.text = dr("reviewer").ToString
            _mi.value = dr("reviewer").ToString

            'If HttpContext.Current.Session("Agent") = dr("Agent").ToString Then
            '    _mi.selected = "selected"
            'Else
            '    _mi.selected = ""
            'End If

            mi_items.Add(_mi)

        Next


        Return mi_items

    End Function

End Class

