Imports System.Data
Imports Common
Imports System.IO

Partial Class agent_summary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=all_agent_summary.aspx")
        End If


        'Response.Write("select review_date, id from form_score3 where id = (select min(form_id)  from notifications join form_score3 on form_score3.id = notifications.form_id  join XCC_REPORT_NEW on xcc_report_new.id = form_score3.review_ID   where acknowledged is null and form_score3.appname='" & Session("appname") & "' and agent = '" & Request("agent") & "')")
        If Not IsPostBack Then
            'Dim through_dt As DataTable = GetTable("select review_date, id from form_score3 with (nolock) where id = (select min(form_id)  from notifications with (nolock)  join form_score3 with (nolock)  on form_score3.id = notifications.form_id  join XCC_REPORT_NEW with (nolock)  on xcc_report_new.id = form_score3.review_ID   where acknowledged is null and form_score3.appname='" & Session("appname") & "' and agent = '" & Request("agent") & "')")
            'If through_dt.Rows.Count > 0 Then
            '    lblThroughDate.Text = "Data through " & through_dt.Rows(0).Item(0).ToString  '& " <a href='view_record2.aspx?ID=" & through_dt.Rows(0).Item(1).ToString & "'>(View Last Notification)</a>"
            'Else
            '    lblThroughDate.Text = "No open notifications."
            'End If

            Dim totalDaysinMonth As Int32 = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)

            If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
                txtAgentStart.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
                Session("StartDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).ToString("d")
            Else
                txtAgentStart.Text = Session("StartDate")
            End If

            If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
                txtAgentEnd.Text = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
                Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, totalDaysinMonth).ToString("d")
            Else
                txtAgentEnd.Text = Session("EndDate")
            End If

        End If

        'dsSummary.SelectParameters("agent").DefaultValue = Request("agent")

        If IsDate(txtAgentStart.Text) Then
            dsSummary.SelectParameters("startdate").DefaultValue = txtAgentStart.Text
        Else
            dsSummary.SelectParameters("startdate").DefaultValue = "1/1/2100"
        End If

        If IsDate(txtAgentEnd.Text) Then
            dsSummary.SelectParameters("enddate").DefaultValue = txtAgentEnd.Text
        Else
            dsSummary.SelectParameters("enddate").DefaultValue = "1/1/2100"
        End If


        dsSummary.DataBind()
        GridView1.DataBind()
    End Sub

    Protected Sub dsSummary_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsSummary.Selecting
        e.Command.CommandTimeout = 180
    End Sub

    Protected Sub btnAgentReport_Click(sender As Object, e As EventArgs) Handles btnAgentReport.Click
        GridView1.DataBind()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click

        GV_to_CSV(GridView1, "AllAgents")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Notifications.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)


        GridView1.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub GridView1_PreRender(sender As Object, e As EventArgs) Handles GridView1.PreRender
        If GridView1.HeaderRow IsNot Nothing Then
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub
End Class
