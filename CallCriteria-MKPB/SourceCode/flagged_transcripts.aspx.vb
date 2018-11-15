Imports Common
Imports System.IO

Partial Class flagged_transcripts
    Inherits System.Web.UI.Page

    Private Sub flagged_transcripts_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=flagged_transcripts.aspx")
        End If

        If Not IsPostBack Then
            dsSC.SelectParameters("username").DefaultValue = User.Identity.Name
            dsSC.DataBind()

            start_date.Text = Today.AddDays(-Today.Day + 1).ToShortDateString
            end_date.Text = Today.ToShortDateString
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=TranscriptsExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvPMComp.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString().Replace("<br>", ""))
        Response.Flush()
        Response.End()
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub

    Private Sub gvPMComp_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPMComp.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.DataItem("call_status") <> "Transcribed Only" Then
                'e.Row.Cells(11).Controls(1).Visible = False
                e.Row.Cells(10).Controls(1).Visible = False
            End If

            'Select Case e.Row.DataItem("max_reviews").ToString
            '    Case "99"
            '        e.Row.BackColor = Drawing.Color.LightGray
            '    Case "0"
            '        e.Row.BackColor = Drawing.Color.Gray
            '    Case "1", "2"
            '        e.Row.BackColor = Drawing.Color.Azure
            'End Select
        End If
    End Sub

    Private Sub gvPMComp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPMComp.RowCommand







        If IsNumeric(e.CommandArgument) Then

            Dim gvr As GridViewRow = e.CommandSource.parent.parent
            Dim lblFlag As HiddenField = gvr.FindControl("lblFlag")

            UpdateTable("update xcc_report_new set max_reviews = 0, must_review = 1, flagged_by = '" & User.Identity.Name & "',flagged_by_date = dbo.GetMTdate() where id = " & lblFlag.Value)
            gvPMComp.DataBind()



        End If

    End Sub

    Private Sub dsPMComp_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles dsPMComp.Selecting
        e.Command.CommandTimeout = 180
    End Sub
End Class
