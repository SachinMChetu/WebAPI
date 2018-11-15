Imports Common
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.IO

Partial Class direct_upload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Request("appname") IsNot Nothing And Request("date") IsNot Nothing Then
            GetList(Request("appname"), Request("date"))
        End If
    End Sub

    Protected Sub GetList(appname As String, req_date As String) 'Handles btnGetList.Click
        'Dim filename As String = Server.MapPath("docs/" & CDate(req_date).ToString("M_d_yyyy") & ".csv")
        'IO.File.WriteAllText(filename, "filename,file_date,file_directory" & vbCrLf)


        'lblResults.Text = ""
        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()


        'Remove delete for now
        Dim del As New SqlCommand("delete from WAV_DATA where file_date = '" & CDate(req_date).ToString("MM/dd/yyyy") & "' and appname = '" & appname.ToString & "'", cn)
        del.CommandTimeout = 600
        del.ExecuteNonQuery()

        del = New SqlCommand("delete from WAV_DATA where file_date < dateadd(dd,-30,dbo.getMTDate()) ", cn)
        del.CommandTimeout = 600
        del.ExecuteNonQuery()


        Dim settings_dt As DataTable = GetTable("select * from app_settings where appname = '" & appname.ToString & "'")


        Dim dir_list() As String = settings_dt.Rows(0).Item("recording_dirs").ToString.Split(";")


        Dim url_list() As String = settings_dt.Rows(0).Item("recording_url").ToString.Split(";")
        Dim user_list() As String = settings_dt.Rows(0).Item("recording_user").ToString.Split(";")
        Dim pwd_list() As String = settings_dt.Rows(0).Item("record_password").ToString.Split(";")

        Dim url_count As Integer = 0
        For Each url In url_list

            For x As Integer = 0 To dir_list.Length - 1
                Dim this_uri As Uri
                Try
                    this_uri = New Uri(url & dir_list(x) & "/" & CDate(req_date).ToString(settings_dt.Rows(0).Item("record_format")).Replace("/", "_") & "/")
                Catch ex As Exception

                    For Each url2 In url_list
                        Response.Write(url2 & " " & url_count & "<br>")
                    Next

                    Response.Write(url(url_count) & "/" & dir_list(x) & "/" & req_date.Replace("/", "_") & "/")
                    Response.End()
                End Try

                Dim tmp As String = ""

                Dim rec_count As Integer = 0

                Dim sql As String = ""


                Try

                   


                    If Left(url, 3) = "ftp" Then


                        'Response.Write(this_uri.AbsoluteUri)
                        'Response.End()



                        Dim request As FtpWebRequest = FtpWebRequest.Create(this_uri)
                        request.Method = WebRequestMethods.Ftp.ListDirectory

                        request.Credentials = New NetworkCredential(user_list(url_count), pwd_list(url_count))

                       
                        Dim ftp_response As FtpWebResponse = request.GetResponse()

                        Dim reader As New StreamReader(ftp_response.GetResponseStream())
                        tmp = reader.ReadToEnd()
                  
                        'Response.Write(tmp)
                        'Response.End()

                        reader.Close()

                        Dim files() As String = tmp.Split(Chr(13))

                        For Each file_obj In files
                            Dim attribs() As String = file_obj.Split(" ")



                            sql = "INSERT INTO [WAV_DATA] "
                            sql &= "(filename,file_date, file_directory, appname, audio_user, audio_password, session_id) VALUES ("

                            sql &= "'" & this_uri.AbsoluteUri.ToString & file_obj.Replace(Chr(10), "") & "',"

                            sql &= "'" & req_date & "','" & dir_list(x) & "','" & appname.ToString & "', '" & user_list(url_count) & "','" & pwd_list(url_count) & "',"
                            sql &= "'" & Trim(file_obj.Replace(".mp3", "").Replace(".gsm", "").Replace(".wav", "")) & "')"

                            Response.Write(sql & "<br>")
                            Response.Flush()
                            Try
                                Dim reply As New SqlCommand(sql, cn)
                                reply.ExecuteNonQuery()
                            Catch ex As Exception
                                Response.Write(ex.Message & "<br>")
                            End Try


                         


                            rec_count += 1

                        Next

                     

                    ElseIf Left(url, 4) = "sftp" Then


                        'Dim al As ArrayList = SFTPGetFiles(url, user_list(url_count), pwd_list(url_count), dir_list(x) & "/" & CDate(req_date).ToString("M_d_yyyy"))
                        Dim al As ArrayList = SFTPGetFiles(url, user_list(url_count), pwd_list(url_count), dir_list(x) & "/" & CDate(req_date).ToString(settings_dt.Rows(0).Item("record_format")))

                        For Each file_name As String In al

                            If file_name.IndexOf(".wav") > -1 Or (file_name.IndexOf(".mp3") > -1) Or (file_name.IndexOf(".gsm") > -1) Then

                                sql = "INSERT INTO [WAV_DATA] "
                                sql &= "(filename,file_date, file_directory, appname, audio_user, audio_password) VALUES ("
                                sql &= "'" & url & "/" & dir_list(x) & "/" & CDate(req_date).ToString(settings_dt.Rows(0).Item("record_format")) & "/" & file_name & "',"
                                sql &= "'" & req_date & "','" & dir_list(x) & "','" & appname.ToString & "', '" & user_list(url_count) & "','" & pwd_list(url_count) & "')"

                                Response.Write(sql & "<br>")
                                Dim reply As New SqlCommand(sql, cn)
                                reply.ExecuteNonQuery()

                               


                                rec_count += 1
                            End If
                        Next

                      


                    Else
                        Dim web_request As HttpWebRequest
                        Dim web_response As HttpWebResponse

                        web_request = HttpWebRequest.Create(this_uri)
                        web_request.Method = WebRequestMethods.Http.Get
                        web_request.Credentials = New NetworkCredential(user_list(url_count), pwd_list(url_count))
                        web_response = web_request.GetResponse()

                        'web_request.Timeout = 30000
                        Dim reader As New StreamReader(web_response.GetResponseStream())
                        tmp = reader.ReadToEnd()
                        web_response.Close()
                        'Response.Write(tmp)
                        'Response.End()


                        If tmp IsNot Nothing Then
                            Dim rec_list As New ArrayList
                            Dim str_start As Integer = 0
                            Dim str_end As Integer = 0

                            'Response.Write(tmp.ToString)
                            'Response.End()
                            Dim audios() As String = tmp.Split("<A HREF=")


                            For Each audio In audios
                                If UCase(audio).IndexOf(".WAV") > 0 Then
                                    'Response.Write(audio & "<BR>" & Chr(13) & Chr(13))
                                    str_start = audio.IndexOf(Chr(34)) - 1
                                    str_end = audio.LastIndexOf(Chr(34))
                                    sql = "INSERT INTO [WAV_DATA] "
                                    sql &= "(filename,file_date, file_directory, appname) VALUES ("

                                    If InStr(LCase(audio), "http") > 0 Or InStr(audio, "/" & CDate(req_date).ToString("M_d_yyyy") & "/") > 0 Then
                                        sql &= "'" & audio.Substring(str_start + 2, str_end - str_start - 1).Replace("%20", " ").Replace(Chr(34), "").Replace("/" & CDate(req_date).ToString("M_d_yyyy") & "/&dt=", "&dt=" & CDate(req_date).ToString("M_d_yyyy") & "&fl=") & "',"
                                        'sql &= "'" & audio & "',"
                                    Else
                                        sql &= "'/" & dir_list(x) & "/" & CDate(req_date).ToString("M_d_yyyy") & "/" & audio.Substring(str_start + 2, str_end - str_start - 2) & "',"
                                    End If
                                    sql &= "'" & req_date & "','" & dir_list(x) & "','" & appname.ToString & "');"
                                    Dim reply As New SqlCommand(sql, cn)
                                    reply.ExecuteNonQuery()
                                    rec_count += 1
                                    'Response.Write(sql & "<br>")
                                    'Response.Flush()
                                End If
                            Next

                        End If
                    End If




                    'Catch sex As SocketException
                    'lblResults.Text &= sex.Message & " - " & this_uri.AbsoluteUri & "(Socket)<br>"
                    ' Response.Write(sex.Message & " - " & this_uri.AbsoluteUri & "(Socket)<br>")
                    'Response.End()
                Catch ex As Exception
                    Response.Write(ex.Message & " - " & this_uri.AbsoluteUri & "<br>")
                    'Response.End()
                End Try



                'If rec_count > 0 Then
                '    lblResults.Text &= "Updated - " & rec_count & " added for " & dir_list(x) & ".<br>"
                'End If

                Select Case appname.ToString.ToLower
                    Case "esto", "estomesca"
                        UpdateTable2("update xcc_report_new set audio_link = filename, audio_user = wav_data.audio_user,  audio_password = wav_data.audio_password from WAV_DATA where filename like '%' + xcc_report_new.Agent +  '%' and filename like '%' + xcc_report_new.phone + '%'  and  audio_link is null and xcc_report_new.appname = '" & appname & "'  and WAV_DATA.appname = '" & appname & "' and file_date = convert(date, call_date)")
                    Case "contactics"
                        UpdateTable2("loadSRLData '" & CDate(req_date).ToString("MM/dd/yyyy") & "'")
                    Case Else
                        UpdateTable2("update wav_data set session_id =  left(REVERSE(SUBSTRING(REVERSE(filename),0,CHARINDEX('/',REVERSE(filename)))),32) where appname = '" & appname & "' and session_id is null")
                        UpdateTable2("update xcc_report_new set audio_link = filename from WAV_DATA with (nolock) where WAV_DATA.session_id = xcc_report_new.session_id and audio_link is null  and wav_data.file_date = '" & req_date & "'")

                End Select

                UpdateTable("exec dedupe '" & appname & "'")

                UpdateTable("exec updatescorecards")

            Next
            url_count += 1
        Next
        'GridView1.DataBind()

        cn.Close()
        cn.Dispose()


        Response.Write("Done")

    End Sub


End Class
