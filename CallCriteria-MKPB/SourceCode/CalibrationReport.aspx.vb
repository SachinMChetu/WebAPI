Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Common

Partial Class CalibrationReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=cd2.aspx")
        End If

        Dim access As String() = {"Admin", "QA Lead", "Calibrator", "Center Manager", "Tango TL"}
        If Not access.Contains(Roles.GetRolesForUser().Single()) Then
            Response.Redirect("cd2.aspx")
        End If

        dsApps.SelectParameters("username").DefaultValue = User.Identity.Name
        dsQAs.SelectParameters("username").DefaultValue = User.Identity.Name
        dsCalibrators.SelectParameters("username").DefaultValue = User.Identity.Name
        dsSCSummary.SelectParameters("username").DefaultValue = User.Identity.Name
        dsSummary.SelectParameters("username").DefaultValue = User.Identity.Name
    End Sub




    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        GV_to_CSV(gvReport, "CalibrationReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=NotificationReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvReport.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub gvReport_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Text.Length > 60 Then
                e.Row.Cells(3).Text = e.Row.Cells(3).Text.Substring(0, 60) + "..."
            End If
            If e.Row.Cells(5).Text.Length > 60 Then
                e.Row.Cells(5).Text = e.Row.Cells(5).Text.Substring(0, 60) + "..."
            End If
        End If
    End Sub
    Protected Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim app_list As String = ""
        Dim dsAppList As String = "0"

        'If lbApps.SelectedValue <> "" Then
        app_list = " and vwForm.scorecard in (''"
        For Each li As ListItem In lbApps.Items
            If li.Selected Then
                app_list &= ",'" & li.Value & "'"
                dsAppList &= "," & li.Value
            End If
        Next
        app_list &= ") "
        'End If

        If app_list = " and vwForm.scorecard in ('') " Then app_list = ""

        app_list = Replace(app_list, "'',", "")


        If dsAppList <> "0" Then
            dsSummary.SelectParameters("scorecard_list").DefaultValue = dsAppList
        Else
            dsSummary.SelectParameters("scorecard_list").DefaultValue = "ALL"
        End If

        Dim user_dt As DataTable = GetTable("select * from userextrainfo where username = '" & User.Identity.Name & "'")




        'Dim top As String = ""
        'If Not Request.QueryString.HasKeys Then
        '    top = " TOP 500"
        'End If
        Dim summary As String = "SELECT convert(date, dateadd(d, -datepart(weekday, c.review_date) + 7, c.review_date)) as week_ending_date, short_name as Scorecard, reviewed_by, COUNT(*) AS processed, case when user_role = 'Calibrator' then num_cal_left else num_recal_left end AS pending,dbo.ConvertTimeToHHMMSS(SUM(DATEDIFF(s,c.review_started,c.review_date)),'s') AS reviewtime,dbo.ConvertTimeToHHMMSS(SUM(call_length),'s') AS calltime,convert(decimal(10,2), AVG(case when isrecal = 1 then null else c.total_score end)) AS avgscore,convert(decimal(10,2), AVG(case when isrecal = 1 then c.total_score else null end)) AS avgrecalscore FROM calibration_form c JOIN vwForm  ON f_id = c.original_form join scorecards on scorecards.ID = vwForm.scorecard left JOIN (SELECT sum( case when isrecal=1 then 1 else 0 end) AS num_recal_left, sum( case when isrecal=0 then 1 else 0 end) AS num_cal_left,  scorecard FROM calibration_pending join vwForm on vwForm.f_id = form_id WHERE date_completed IS NULL GROUP BY  scorecard ) x ON  x.scorecard = vwForm.scorecard join userextrainfo on userextrainfo.username = reviewed_by WHERE 1=1"
        Dim command As String = "SELECT  *, calibration_form.review_date as calib_date, vwForm.review_date as reviewed_date, ROUND(calibration_form.total_score,2) AS calib_score, vwForm.total_score AS form_score, agent_group FROM calibration_form JOIN vwForm ON f_id = calibration_form.original_form join scorecards on scorecards.id= vwForm.scorecard join userextrainfo on userextrainfo.username = reviewer WHERE 1=1"

        If rbRange.Checked Then
            summary &= " AND CONVERT(DATE, c.review_date) >= CONVERT(DATE, '" & date_from.Value & "')"
            command &= " AND CONVERT(DATE, calibration_form.review_date) >= CONVERT(DATE, '" & date_from.Value & "')"
            summary &= " AND CONVERT(DATE, c.review_date) < DATEADD(d,1,CONVERT(DATE, '" & date_to.Value & "'))" 
            command &= " AND CONVERT(DATE, calibration_form.review_date) < DATEADD(d,1,CONVERT(DATE, '" & date_to.Value & "'))"
        End If

        If rbWE.Checked Then
            summary &= " AND convert(date, dateadd(d, -datepart(weekday, c.review_date) + 7, c.review_date))= '" & ddlWEDate.SelectedValue & "' "
            command &= " AND vwForm.week_ending_date = '" & ddlWEDate.SelectedValue & "' "
            'ddlWEDate.SelectedValue = Request("wedate")
        End If

        If user_dt.Rows(0).Item("call_center").ToString <> "" Then
            command &= "and 1 = case when 1 is not null then case when userextrainfo.call_center = " & user_dt.Rows(0).Item("call_center").ToString & " then 1 else 0 end else 1 end"
        End If


        summary &= app_list
        command &= app_list

        summary &= " GROUP BY convert(date, dateadd(d, -datepart(weekday, c.review_date) + 7, c.review_date)), short_name, reviewed_by, num_recal_left, num_cal_left, user_role ORDER BY week_ending_date DESC, short_name, reviewed_by"
        command &= " ORDER BY calibration_form.review_date DESC, vwForm.review_date"


        'Response.Write(range.Value & "<br><br>")
        'Response.Write(date_from.Value & date_to.Value & "<br><br>")
        'Response.Write(summary & "<br><br>")
        'Response.Write(command)
        'Response.End()

        'dsSummary.SelectCommand = summary
        dsReport.SelectCommand = command

        If Request.QueryString.HasKeys AndAlso Request.QueryString("sendemail") IsNot Nothing Then
            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            Me.Master.FindControl("ContentPlaceHolder1").RenderControl(hw)
            'style to format numbers to string
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn.Open()


            Dim emails_dt As DataTable = GetTable("declare @all_emails varchar(2000); select  @all_emails = COALESCE(@all_emails + ';','') + isnull(email_address,'') from userextrainfo where user_role = 'Admin'; select @all_emails")


            'If debug Then
            Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',  @blind_copy_recipients=@CC,  @recipients='stace@callCriteria.com;jason@pointqa.com;carlo@callcriteria.com',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
            reply2.Parameters.AddWithValue("Subject_text", "Daily Calibration Report " + Date.Now.ToString("MM/dd/yyyy"))
            reply2.Parameters.AddWithValue("CC", "") 'emails_dt.Rows(0).Item(0))
            reply2.Parameters.AddWithValue("Body", sw.ToString())

            reply2.CommandTimeout = 60
            reply2.ExecuteNonQuery()
            'End If

            cn.Close()
            cn.Dispose()

            hw.Close()
            sw.Close()
            Response.End()
        End If

        gvSummary.DataBind()

    End Sub

    Private Sub dsSummary_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsSummary.Selecting
        e.Command.CommandTimeout = 240
    End Sub

    Private Sub dsReport_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsReport.Selecting
        e.Command.CommandTimeout = 240
    End Sub

    Private Sub dsSCSummary_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsSCSummary.Selecting
        e.Command.CommandTimeout = 240
    End Sub

    Private Sub gvReport_DataBound(sender As Object, e As EventArgs) Handles gvReport.DataBound

        Dim filter As String = " 1=1 "

        If ddlQAs.SelectedValue <> "" Then
            filter &= "and reviewer = '" & ddlQAs.SelectedValue & "' "
        End If

        If chkRecalOnly.Checked Then
            filter &= " and isrecal = 1 "
        End If

        dsReport.FilterExpression = filter
        dsReport.DataBind()

    End Sub

    Private Sub ddlWEDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWEDate.SelectedIndexChanged
        ddlQAs.Items.Clear()
        ddlQAs.Items.Add(New ListItem("(Select)", ""))
        ddlQAs.DataBind()
    End Sub

    Private Sub btnExport1_Click(sender As Object, e As EventArgs) Handles btnExport1.Click, btnExport2.Click, btnExport3.Click

        Dim btn As Button = sender
        Dim gv As GridView = FindControl(btn.CommandArgument)


        GV_to_CSV(gv, "CalibrationReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=NotificationReport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        Select Case btn.CommandArgument
            Case "gvSummary"
                gvSummary.RenderControl(hw)

            Case "gvScorecards"
                gvScorecards.RenderControl(hw)

            Case "gvCalibrators"
                gvCalibrators.RenderControl(hw)
        End Select

        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub
End Class
