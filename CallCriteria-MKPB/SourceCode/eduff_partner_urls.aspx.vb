
Imports System.IO
Imports Common

Partial Class qs_partner_urls
    Inherits System.Web.UI.Page

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        gv_to_csv(gvURLS, "URLReport")

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Export_URLs.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        gvURLS.RenderControl(hw)
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

End Class
