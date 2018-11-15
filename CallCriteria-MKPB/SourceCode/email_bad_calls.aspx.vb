Imports System.IO
Imports System.Data.SqlClient

Partial Class email_bad_calls
    Inherits System.Web.UI.Page

    Protected Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Email()
    End Sub

    Protected Sub Email()
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        GVBadCalls.RenderControl(hw)

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim email As New SqlCommand("EXEC send_dbmail @profile_name='General', @recipients=@Recipients, @copy_recipients=@CC, @subject=@Subject, @body=@Body, @body_format='HTML';", cn)
        email.Parameters.AddWithValue("Recipients", "brian@callcriteria.com")
        email.Parameters.AddWithValue("Subject", "Bad Calls Report - " & Now.AddDays(-1).Date)
        email.Parameters.AddWithValue("Body", sw.ToString)

        email.CommandTimeout = 60
        email.ExecuteNonQuery()

        cn.Close()
        cn.Dispose()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ''
    End Sub

End Class
