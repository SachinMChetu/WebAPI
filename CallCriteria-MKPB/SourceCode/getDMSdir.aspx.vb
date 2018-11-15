Imports Common
Imports System.IO
Imports System.Net
Imports System.Data.SqlClient
Imports System.Data

Partial Class getDMSdata
    Inherits System.Web.UI.Page
    Dim listen_dt As New DataTable
    Private Sub getDMSdata_Load(sender As Object, e As EventArgs) Handles Me.Load



        listen_dt.Columns.Add("filename", Type.GetType("System.String"))
        listen_dt.Columns.Add("call_date", Type.GetType("System.String"))
        listen_dt.Columns.Add("link", Type.GetType("System.String"))

        Try
            Dim web_request As HttpWebRequest
            Dim web_response As HttpWebResponse

            Dim dms_urls() As String = {"http://dms1.diphones.com/recordings/completed/", "http://107.0.160.54/completed/", "http://107.0.160.51/completed/", "http://107.0.160.53/completed/", "http://107.0.160.55/completed/"}

            For Each this_URL In dms_urls
                web_request = HttpWebRequest.Create(this_URL)
                web_request.Method = WebRequestMethods.Http.Get
                web_response = web_request.GetResponse()

                'web_request.Timeout = 30000
                Dim reader As New StreamReader(web_response.GetResponseStream())
                Dim tmp As String = reader.ReadToEnd()
                web_response.Close()



                tmp = Replace(Replace(Replace(Replace(tmp, "<", ""), ">", ""), "a  href", "a href"), """", "")
                tmp = Replace(tmp, "a href", "|")
                tmp = Replace(tmp, "=", "")

                Dim links() As String = tmp.Split("|")
                For Each link In links
                    If link.IndexOf("//a/tdtd alignright") > -1 Then

                        Dim dir As String = Left(link, link.IndexOf("/"))
                        Dim dir_date As String = link.Substring(link.IndexOf("//a/tdtd alignright") + 19, 12)
                        If Request("call_date") <> "" Then
                            If CDate(dir_date) >= CDate(Request("call_date")) Then
                                getDIRCalls(this_URL, dir, Format(CDate(Request("call_date")), "yyyy-MM-dd"))
                            End If
                        Else
                            getDIRCalls(this_URL, dir, Format(CDate(dir_date), "yyyy-MM-dd"))

                        End If

                        'Response.Write(dir & "  " & CDate(dir_date).ToShortDateString & "<br>")
                    End If
                Next


                Response.Write(this_URL & " - " & listen_dt.Rows.Count & "<br>")

                Using command = New SqlCommand("WAVDataInsert")
                    command.CommandType = CommandType.StoredProcedure
                    command.CommandTimeout = 600
                    'create your own data table
                    command.Parameters.Add(New SqlParameter("@WAV_Data", listen_dt))
                    RunSqlCommand(command)
                End Using

                Response.Flush()

                listen_dt.Rows.Clear()


            Next



            'this_URL = "http://107.0.160.55/completed/"

            'web_request = HttpWebRequest.Create(this_URL)
            'web_request.Method = WebRequestMethods.Http.Get
            'web_response = web_request.GetResponse()

            ''web_request.Timeout = 30000
            'reader = New StreamReader(web_response.GetResponseStream())
            'tmp = reader.ReadToEnd()
            'web_response.Close()



            'tmp = Replace(Replace(Replace(Replace(tmp, "<", ""), ">", ""), "a  href", "a href"), """", "")
            'tmp = Replace(tmp, "a href", "|")
            'tmp = Replace(tmp, "=", "")

            'links = tmp.Split("|")
            'For Each link In links
            '    If link.IndexOf("//a/tdtd alignright") > -1 Then

            '        Dim dir As String = Left(link, link.IndexOf("/"))
            '        Dim dir_date As String = link.Substring(link.IndexOf("//a/tdtd alignright") + 19, 12)
            '        If Request("call_date") <> "" Then
            '            If CDate(dir_date) >= CDate(Request("call_date")) Then
            '                getDIRCalls(this_URL, dir, Format(CDate(Request("call_date")), "yyyy-MM-dd"))
            '            End If
            '        Else
            '            getDIRCalls(this_URL, dir, Format(CDate(dir_date), "yyyy-MM-dd"))

            '        End If

            '        'Response.Write(dir & "  " & CDate(dir_date).ToShortDateString & "<br>")
            '    End If
            'Next


            ' Response.End()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


    End Sub

    Public Sub getDIRCalls(this_URL As String, subdir As String, dir_date As String)
        Try
            Dim web_request As HttpWebRequest
            Dim web_response As HttpWebResponse

            'Response.Write(this_uri.ToString & " - " & user_list(url_count) & ":" & pwd_list(url_count) & "<br>")

            'Response.Write(Request("URL") & subdir & "/" & dir_date)



            web_request = HttpWebRequest.Create(this_URL & subdir & "/" & dir_date)
            web_request.Method = WebRequestMethods.Http.Get
            web_response = web_request.GetResponse()

            'web_request.Timeout = 30000
            Dim reader As New StreamReader(web_response.GetResponseStream())
            Dim tmp As String = reader.ReadToEnd()
            web_response.Close()



            tmp = Replace(Replace(Replace(Replace(tmp, "<", ""), ">", ""), "a  href", "a href"), """", "")
            tmp = Replace(tmp, "a href", "|")
            tmp = Replace(tmp, "=", "")

            Dim links() As String = tmp.Split("|")
            For Each link In links

                If link.IndexOf(".mp3") > -1 Then

                    Dim short_link As String = Left(link, link.IndexOf(".mp3") + 4)
                    Response.Write(short_link & "<br>")

                    Dim listen_dr As DataRow = listen_dt.NewRow
                    listen_dr("filename") = this_URL & subdir & "/" & Format(CDate(dir_date), "yyyy-MM-dd") & "/" & short_link
                    listen_dr("call_date") = CDate(dir_date).ToShortDateString
                    listen_dr("link") = short_link

                    listen_dt.Rows.Add(listen_dr)
                End If
            Next

            'Response.End()



        Catch ex As Exception

        End Try
    End Sub
End Class
