Imports System.Data.SqlClient
Imports System.Data
Imports Common


Partial Class Agent_Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            'If Request("agent") Is Nothing Then
            '    Response.Redirect("login.aspx?ReturnURL=default.aspx")
            'End If

            'If Not Roles.IsUserInRole("QA") Or Roles.IsUserInRole("Agent") Then
            '    Response.Redirect("login.aspx")
            'End If


            'Dim userProfile As ProfileCommon = ProfileCommon.Create(User.Identity.Name)

            'lblSupervisor.Text = userProfile.Supervisor

            hdnAgent.Value = User.Identity.Name 'Request("agent")

            Dim cn2 As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn2.Open()


            Dim replyNoOfreview = New SqlDataAdapter("select count(*) as num_left from dbo.XCC_REPORT_NEW b with (nolock) join userapps on userapps.appname = b.appname where ((MAX_REVIEWS >= 1)) and ((review_started < dateadd(mi,-45, dbo.getMTDate())) and (review_started is not null)) and username =  '" & User.Identity.Name & "' and b.AGENT = '" & hdnAgent.Value & "' ", cn2)
            Dim dtnoOfreview As New DataTable
            replyNoOfreview.Fill(dtnoOfreview)
            lblLeftToReview.Text = dtnoOfreview.Rows(0).Item(0)

            Dim total_worked_dt As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS with (nolock)  join XCC_REPORT_NEW with (nolock)  on XCC_ID = XCC_REPORT_NEW.ID where agent_id = '" & Request("Agent") & "' and review_date between convert(date, dbo.getMTDate()) and dbo.getMTDate() and xcc_report_new.appname = '" & Session("appname") & "'")
            lblWorkedRecords.Text = total_worked_dt.Rows(0).Item(0).ToString


            Dim toal_failed As DataTable = GetTable("select count(*) from dbo.XCC_RESULTS with (nolock)  join XCC_REPORT_NEW with (nolock)  on XCC_ID = XCC_REPORT_NEW.ID where  review_date between convert(varchar(10), dbo.getMTDate(),101) and dbo.getMTDate() and AutoFail is not null and xcc_report_new.appname = '" & Session("appname") & "' and XCC_REPORT_NEW.AGENT = '" & hdnAgent.Value & "'")
            lblTotalFailes.Text = toal_failed.Rows(0).Item(0).ToString

            Dim avg_score As DataTable = GetTable("select CAST(AVG(total_score_with_fails) AS DECIMAL(12,2)) from form_score3 with (nolock)  join XCC_REPORT_NEW with (nolock) on form_score3.review_ID = XCC_REPORT_NEW.ID where XCC_REPORT_NEW.AGENT = '" & hdnAgent.Value & "' and XCC_REPORT_NEW.call_date = '" & Now.ToShortDateString & "'")
            If avg_score.Rows.Count > 0 Then
                lblAvgScore.Text = avg_score.Rows(0).Item(0).ToString
            End If

        End If

    End Sub

    Protected Sub ApproveNotification(sender As Object, e As ImageClickEventArgs)

        Dim btn As ImageButton = sender

        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Common.UpdateTable("Update Notifications set acknowledged = 1, ack_date = dbo.getMTDate(), ack_by = '" & ack_by & "' where id = " & btn.CommandArgument)
        gvNotifications.DataBind()

    End Sub

End Class
