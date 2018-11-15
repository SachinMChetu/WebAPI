<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">


    'Sub Application_BeginRequest(sender As Object, e As EventArgs)
    '    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*")
    '    'If HttpContext.Current.Request.HttpMethod = "OPTIONS" Then
    '    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PATCH, PUT, DELETE, OPTIONS")
    '    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token, Accept")
    '    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000")
    '    'HttpContext.Current.Response.[End]()
    '    'End If
    'End Sub


    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)



        'If System.Web.HttpContext.Current.User.Identity.Name <> "stacemoss2" Then

        Dim CurrentException As Exception = Server.GetLastError()
        Dim ErrorDetails As String = CurrentException.ToString()

        If ErrorDetails.ToUpper.IndexOf("VIEWSTATE") > 0 Then
            Exit Sub
        End If


        If System.Web.HttpContext.Current.Request.ServerVariables("HTTP_REFERER").ToUpper.IndexOf("LOCALHOST") > 0 Then
            Exit Sub
        End If

        Dim context As HttpContext = DirectCast(sender, HttpApplication).Context


        'include logo
        Dim body As New StringBuilder
        body.Append("<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""font-family:Arial, sans-serif; font-size:12px;""><tr><td colspan=""2"" style=""color:#FF0000; font-weight:bold;"">Please do not reply to this auto-generated email.</td></tr><tr><td colspan=""2"">&nbsp;</td></tr>")

        body.Append("<tr><td colspan=""2""><img src=""http://app.callcriteria.com/img/cc_words_logo.png"" alt=""Call Criteria"" width=""322"" height=""50""></td></tr><tr><td colspan=""2"">&nbsp;</td></tr>")

        Dim current As HttpContext = System.Web.HttpContext.Current
        body.Append("<tr><td>Source Page:</td><td>" & current.Request.Path & "</td></tr>")
        body.Append("<tr><td>Referrer:</td><td>" & current.Request.ServerVariables("HTTP_REFERER") & "</td></tr>")
        body.Append("<tr><td>Name:</td><td>" & current.User.Identity.Name & "</td></tr>")

        body.Append("<tr><td colspan=""2"">&nbsp;</td></tr>")
        body.Append("<tr><td colspan=""2"" style=""font-weight:bold;"">Error Information</td></tr>")
        body.Append("<tr><td>Browser:</td><td>" & current.Request.ServerVariables("HTTP_USER_AGENT") & "</td></tr>")
        body.Append("")
        Try
            body.Append("<tr><td>QueryString:</td><td>")
            Dim qs_string() As String = current.Request.QueryString.AllKeys
            For Each qs_str As String In qs_string
                body.Append(qs_str & " - " & current.Request.QueryString(qs_str).ToString & ", ")
            Next

            If Right(body.ToString, 2) = ", " Then
                body.Remove(body.Length - 2, 2)
            End If
            body.Append("</td></tr>")
        Catch ex As Exception

        End Try

        Try
            body.Append("<tr><td>Session:</td><td>")
            For x = 0 To current.Session.Keys.Count - 1
                body.Append(current.Session.Keys.Item(x).ToString & " - " & current.Session(current.Session.Keys.Item(x).ToString).ToString & ", ")
            Next
            body.Remove(body.Length - 2, 2)
            body.Append("</td></tr>")
        Catch ex As Exception
            body.Append("<tr><td>" & ex.Message & "</td></tr>")
        End Try

        body.Append("<tr><td>Date and Time:</td><td>" & Now().ToString & "</td></tr>")
        body.Append("<tr><td>Page:</td><td>" & current.Request.ServerVariables("URL") & "</td></tr>")
        body.Append("<tr><td>Remote Address:</td><td>" & current.Request.ServerVariables("REMOTE_ADDR") & "</td></tr>")
        body.Append("<tr><td>HTTP Referer:</td><td>" & current.Request.ServerVariables("HTTP_REFERER") & "</td></tr>")

        body.Append("<tr><td colspan=""2"">&nbsp;</td></tr>")
        body.Append("<tr><td colspan=""2"" style=""font-weight:bold;"">Actual Error</td></tr>")
        body.Append("<tr><td colspan=""2"">" & ErrorDetails & "</td></tr>")

        body.Append("</table>")




        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()



        'If debug Then
        Dim reply2 As New SqlCommand("EXEC msdb.dbo.sp_send_dbmail  @profile_name='General',  @copy_recipients =@CC,  @recipients='stace@callcriteria.com',  @subject='Query Result',  @body=@Body , @body_format = 'HTML' ;", cn)
        reply2.Parameters.AddWithValue("subject", "WEB SITE ERROR ")
        reply2.Parameters.AddWithValue("Body", body.ToString)

        reply2.CommandTimeout = 60
        reply2.ExecuteNonQuery()
        'End If

        cn.Close()
        cn.Dispose()




        ' End If
    End Sub

</script>