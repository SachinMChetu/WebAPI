Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports Common

Partial Class app_owner_weekly_email
    Inherits System.Web.UI.Page

    Public app_name As String = ""

    Protected Sub form1_PreRender(sender As Object, e As EventArgs) Handles form1.PreRender

        If Request("email") Is Nothing Then

            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            form1.RenderControl(hw)
            'style to format numbers to string
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn.Open()


            Dim emails_dt As DataTable = GetTable("select * from app_settings where appname = '" & Request("appname") & "'")


            If gv.Rows.Count > 0 And emails_dt.Rows(0).Item("Contact_email").ToString <> "" Then
                'If debug Then
                Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',  @copy_recipients='stace@callCriteria.com;brian@callcriteria.com',  @recipients=@CC,  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
                reply2.Parameters.AddWithValue("Subject_text", "Weekly Agent Report")
                reply2.Parameters.AddWithValue("CC", emails_dt.Rows(0).Item("Contact_email"))
                reply2.Parameters.AddWithValue("Body", sw.ToString())

                reply2.CommandTimeout = 60
                reply2.ExecuteNonQuery()
                'End If

            End If


            cn.Close()
            cn.Dispose()


            hw.Close()
            sw.Close()
            Response.Write(sw.ToString)
            Response.End()
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim app_dt As DataTable = GetTable("select * from app_settings where appname = '" & Request("appname") & "'")
        app_name = app_dt.Rows(0).Item("FullName")
    End Sub
End Class
