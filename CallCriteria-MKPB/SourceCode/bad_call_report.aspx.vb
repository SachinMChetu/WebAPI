Imports System.IO
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Common
Imports System.Data.SqlClient

Partial Class bad_call_report
    Inherits System.Web.UI.Page

    Protected Sub btnApplyFilter_Click(sender As Object, e As System.EventArgs) Handles btnApplyFilter.Click
        dsBadCalls.DataBind()
        GVBadCalls.DataBind()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click

        GV_to_CSV(GVBadCalls, "BadCallReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=BadCallReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        GVBadCalls.RenderControl(hw)
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


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If User.IsInRole("Admin") Or User.IsInRole("QA Lead") Or User.IsInRole("Tango TL") Then

        Else
            Response.Redirect("default.aspx")
        End If


        If Not IsPostBack Then
            hdnIdusrname.Value = User.Identity.Name
            Dim thisDate As Date = Today
            thisDate = DateAdd(DateInterval.Day, -7, Today)


            txtStartDate.Text = thisDate.ToString("d")

            txtEndDate.Text = DateAdd(DateInterval.Day, 1, Today()).ToShortDateString

            'If Session("StartDate") Is Nothing Or Session("StartDate") = "" Then
            '    txtStartDate.Text = thisDate.ToString("d")
            '    Session("StartDate") = thisDate.ToString("d")
            'Else
            '    txtStartDate.Text = Session("StartDate")
            'End If

            'If Session("EndDate") Is Nothing Or Session("EndDate") = "" Then
            '    txtEndDate.Text = DateAdd(DateInterval.Day, 1, Today()).ToShortDateString
            '    Session("EndDate") = New DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToString("d")
            'Else
            '    txtEndDate.Text = Session("EndDate")
            'End If

        End If
    End Sub

    Protected Sub GVBadCalls_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GVBadCalls.RowCommand
        If e.CommandName = "Reset" Then
            ' UpdateTable("update xcc_report_new set max_reviews = 0, onAWS = 0, review_started = null, bad_call = null, bad_call_date=null, bad_call_reason=null, bad_call_who=null, bad_call_accepted=null, bad_call_accepted_who=null where id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")
            UpdateTable("exec resetcall '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")
            GVBadCalls.DataBind()
        End If

        If e.CommandName = "Recreate" Then
            UpdateTable("update xcc_report_new set  onAWS = 0, max_reviews = 0, recreate_call = 1 where id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")
            GVBadCalls.DataBind()
        End If

        If e.CommandName = "Accept as Bad" Then



            UpdateTable("update xcc_report_new set bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" + User.Identity.Name + "' where id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")
            UpdateTable("exec makeBlankForm '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")
            UpdateTable("update vwForm set review_date = bad_call_date, pass_fail = 'N/A' where review_id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")

            Dim dr As DataRow = GetTable("select * from xcc_report_new where id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'").Rows(0)

            Try
                Dim call_length As Single = GetMediaDuration(Server.MapPath(dr("audio_link").ToString))


                UpdateTable("update vwForm set call_length = '" & call_length & "' where review_id = '" + GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() + "'")

            Catch ex As Exception

            End Try


            UpdateTable("insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" & User.Identity.Name & "', dbo.getMTDate(), 'Call marked as bad: ' + (select top 1 bad_call_reason from xcc_report_new with  (nolock)  where id = " & GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() & "),(select top 1 f_id from vwForm with  (nolock)  where review_id  = " & GVBadCalls.DataKeys(e.CommandArgument).Value.ToString() & "),'Call'")


            GVBadCalls.DataBind()
        End If
    End Sub

    Protected Sub ddlApps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApps.SelectedIndexChanged
        dsBadCalls.DataBind()
    End Sub

    Protected Sub GVBadCalls_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GVBadCalls.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes("id") = GVBadCalls.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim drv As DataRowView = e.Row.DataItem
            If drv("recreate_call").ToString = "True" Then
                e.Row.Cells(2).Text = ""
            End If

        End If

    End Sub

    <System.Web.Services.WebMethod>
    Public Shared Sub multipleasbad(isting As String, creater As String)

        If isting <> "" Then
            Dim ids As String() = (isting.TrimEnd(","c)).Split(New [Char]() {","c})

            Dim bcl_dt As New DataTable
            bcl_dt.Columns.Add("id", Type.GetType("System.Int32"))
            bcl_dt.Columns.Add("call_length", Type.GetType("System.Single"))

            For Each id As String In ids


                Dim bcl_dr As DataRow = bcl_dt.NewRow
                bcl_dr("ID") = id

                'UpdateTable("update xcc_report_new set bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" + creater + "' where id = '" + id + "'")





                'UpdateTable("update xcc_report_new set bad_call_accepted = dbo.getMTDate(), bad_call_accepted_who = '" + HttpContext.Current.User.Identity.Name + "' where id = '" + id + "'")
                'UpdateTable("insert into form_score3(review_date, review_id,comments, reviewer, appname, week_ending_date, pass_fail, formatted_comments, review_time) select dbo.getMTDate()," + id + ", 'Placeholder for bad call',bad_call_who, appname, Convert(date, dateadd(d, -datepart(weekday, bad_call_date) + 7, bad_call_date)), 'Fail', 'Bad Call', datediff(s, review_started, bad_call_date) from xcc_report_new where id = " + id)
                'UpdateTable("update vwForm Set review_date = bad_call_date, pass_fail = 'N/A' where review_id = '" + id + "'")

                Dim dr As DataRow = GetTable("select * from xcc_report_new where id = '" + id + "'").Rows(0)

                Try
                    Dim call_length As Single = GetMediaDuration(HttpContext.Current.Server.MapPath(dr("audio_link").ToString))

                    bcl_dr("call_length") = call_length
                    'UpdateTable("update vwForm set call_length = '" & call_length & "' where review_id = '" + id + "'")

                Catch ex As Exception

                End Try
                bcl_dt.Rows.Add(bcl_dr)

                'UpdateTable("insert into system_comments (comment_who, comment_date, comment, comment_id, comment_type) select '" & HttpContext.Current.User.Identity.Name & "', dbo.getMTDate(), 'Call marked as bad: ' + (select top 1 bad_call_reason from xcc_report_new with  (nolock)  where id = " & id & "),(select top 1 f_id from vwForm with  (nolock)  where review_id  = " & id & "),'Call'")





            Next


            Using command = New SqlCommand("markManyBadCall")
                command.CommandType = CommandType.StoredProcedure
                'create your own data table
                command.Parameters.Add(New SqlParameter("@BadCallIDList", bcl_dt))
                command.Parameters.Add(New SqlParameter("@username", HttpContext.Current.User.Identity.Name))
                RunSqlCommand(command)
            End Using



        End If

    End Sub

    Private Sub GVBadCalls_DataBound(sender As Object, e As EventArgs) Handles GVBadCalls.DataBound
        lblRows.Text = GVBadCalls.Rows.Count
    End Sub
End Class
