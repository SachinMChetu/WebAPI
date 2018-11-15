
Imports System.IO
Imports Common

Partial Class dms_nb_report
    Inherits System.Web.UI.Page

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        gv_to_csv(gvNB, "NBReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=NBReport.xls")
        'Response.AddHeader("content-disposition", "attachment;filename=NBReport.xlsx")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvNB.RenderControl(hw)
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

    Private Sub dms_nb_report_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=dms_nb_report.aspx")
        End If

        dsSC.SelectParameters("username").DefaultValue = User.Identity.Name

    End Sub

    Private Sub gvNB_DataBound(sender As Object, e As EventArgs) Handles gvNB.DataBound
        lblRows.Text = gvNB.Rows.Count
    End Sub
End Class
