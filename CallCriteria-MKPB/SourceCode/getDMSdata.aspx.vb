Imports Common
Imports System.IO
Imports System.Net
Imports System.Data.SqlClient

Partial Class getDMSdata
    Inherits System.Web.UI.Page

    Private Sub getDMSdata_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Dim web_request As HttpWebRequest
            Dim web_response As HttpWebResponse

            'Response.Write(this_uri.ToString & " - " & user_list(url_count) & ":" & pwd_list(url_count) & "<br>")




            web_request = HttpWebRequest.Create(Request("URL"))
            web_request.Method = WebRequestMethods.Http.Get
            web_response = web_request.GetResponse()

            'web_request.Timeout = 30000
            Dim reader As New StreamReader(web_response.GetResponseStream())
            Dim tmp As String = reader.ReadToEnd()
            web_response.Close()


            UpdateTable("truncate TABLE gvd_call_data")
            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn.Open()



            Dim reply As New SqlCommand("insert gvd_call_data(data) select @call_date", cn)

            reply.Parameters.AddWithValue("call_date", tmp)

            reply.ExecuteNonQuery()


            Response.Write(Len(tmp))

            Response.End()

        Catch ex As Exception

        End Try


    End Sub
End Class
