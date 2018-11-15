Imports Common
Imports System.Data
Imports System.Net
Imports Tamir.SharpSsh

Partial Class convert_audio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("ID") IsNot Nothing Then
            Dim dt2 As DataTable = GetTable("select top 1 * from vwForm  with (nolock) join app_settings with (nolock)  on app_settings.appname = vwForm.appname  where f_id = " & Request("ID"))
            Dim this_filename As String = GetAudioFileName(dt2.Rows(0))
            Response.Write(this_filename)
            Response.End()
        End If

        If Request("WID") IsNot Nothing Then
            Dim dt2 As DataTable = GetTable("select * from WAV_DATA  with (nolock) join app_settings with (nolock)  on app_settings.appname = WAV_DATA.appname  where WAV_DATA.id = " & Request("WID"))
            Dim this_filename As String = GetAudioFile(dt2.Rows(0), Request("row_id"))
            Response.Write(this_filename)
            Response.End()
        End If

    End Sub

    Protected Function GetAudioFile(ByVal drv As DataRow, row_id As String) As String


        Dim session_id As String

        If drv.Item("session_id").ToString.IndexOf(" ") > 0 Then
            session_id = Left(drv.Item("session_id").ToString, drv.Item("session_id").ToString.IndexOf(" "))
        Else
            session_id = Trim(drv.Item("session_id").ToString)
        End If

        session_id = session_id & "_" & row_id

        Dim this_filename As String = drv.Item("filename").ToString



        Dim file_date As String

        Try
            file_date = Trim(CDate(drv.Item("file_date").ToString).ToString.Substring(0, drv.Item("file_date").ToString.IndexOf(" ")).Replace("/", "_"))
        Catch ex As Exception
            file_date = Today.ToShortDateString.Replace("/", "_")
        End Try


        If Not IO.Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/")) Then
            IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/"))
        End If




        Dim file_ending As String = Right(this_filename, 4).ToLower


        If (this_filename.IndexOf("@") > -1 Or this_filename.IndexOf("http") > -1) And Left(this_filename, 3) <> "ftp" And Left(this_filename, 4) <> "sftp" And Left(this_filename, 6) <> "/audio" Then 'Needs downloading first - http and/or still has @ sign 
            'Email_Error(this_filename)
            Dim WebClient_down As New System.Net.WebClient()

            WebClient_down.Credentials = New System.Net.NetworkCredential(drv.Item("recording_user").ToString, drv.Item("record_password").ToString, "")

            Try
                WebClient_down.DownloadFile(this_filename, HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending))
            Catch ex As Exception

                'HttpContext.Current.Response.Write("File not found, refresh page." & this_filename & " to " & HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending) & ex.Message)
                Email_Error(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending) & ex.Message)
                'HttpContext.Current.Response.End()

            End Try


            Do Until Not WebClient_down.IsBusy
                System.Threading.Thread.Sleep(100)
            Loop



        End If



        If Left(this_filename, 6) = "ftp://" Then 'Needs downloading first - ftp and/or still has @ sign 


            Dim ftpc As New WebClient
            ftpc.Credentials = New System.Net.NetworkCredential(drv.Item("audio_user").ToString, drv.Item("audio_password").ToString, "")

            Try
                IO.File.Delete("d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending)
            Catch ex As Exception

            End Try

            Try
                ftpc.DownloadFile(this_filename, "d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending)
                System.Threading.Thread.Sleep(500)
            Catch ex As Exception
                HttpContext.Current.Response.Write("d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending & " " & ex.Message & "<br><br><br>")
                HttpContext.Current.Response.End()
            End Try

            Do Until Not ftpc.IsBusy
                System.Threading.Thread.Sleep(100)
            Loop


        End If


        If Left(this_filename, 2) = "\\" Then 'Needs downloading first - ftp and/or still has @ sign 
            'Email_Error(this_filename.Substring(Len(drv.Item("recording_url")) + 1).Replace("%20", " "))
            Try
                IO.File.Copy(Replace(this_filename, "\\64.111.27.113\d$", "d:"), HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending))
            Catch ex As Exception

            End Try


            Dim WebClient_down As New System.Net.WebClient()

            'WebClient_down.Credentials = New System.Net.NetworkCredential("administrator", "Mars2930!", "")



            Try
                WebClient_down.DownloadFile(Replace(this_filename, "\\64.111.27.113\d$", "d:"), HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending))
            Catch ex As Exception

                'HttpContext.Current.Response.Write("File not found, refresh page." & this_filename & " to " & HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending) & ex.Message)
                'Email_Error(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending) & ex.Message)
                'HttpContext.Current.Response.End()

            End Try

            Dim max_wait As Integer = 0

            Do Until IO.File.Exists(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending)) Or max_wait = 100

                System.Threading.Thread.Sleep(100)
                max_wait += 1
            Loop
            System.Threading.Thread.Sleep(150)
        End If




        If Left(this_filename, 7) = "sftp://" Then 'Needs downloading first - ftp and/or still has @ sign 
            'Email_Error(this_filename.Substring(Len(drv.Item("recording_url")) + 1).Replace("%20", " "))

            Dim sftp As Sftp
            sftp = New Sftp(drv.Item("recording_url").ToString.Substring(7), drv.Item("audio_user").ToString, drv.Item("audio_password").ToString)
            If drv.Item("audio_port").ToString <> "" Then
                sftp.Connect(drv.Item("audio_port").ToString)
            Else
                sftp.Connect(22)
            End If


            'HttpContext.Current.Response.Write(drv.Item("recording_url").ToString.Substring(7) & "-" & drv.Item("audio_user").ToString & "-" & drv.Item("audio_password").ToString & "<br>")
            'HttpContext.Current.Response.Write(this_filename.Substring(Len(drv.Item("recording_url")) + 1).Replace("2014", "14") & " to " & "d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending)
            'HttpContext.Current.Response.End()



            Try
                sftp.Get(this_filename.Substring(Len(drv.Item("recording_url"))).Replace("%20", " "), "d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending)
                'sftp.Get(this_filename.Replace("%20", " "), "d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending)

            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.Message & "<br>")
                HttpContext.Current.Response.Write("d:\wwwroot\audio\" & drv.Item("appname").ToString & "\" & file_date & "\" & session_id & file_ending & "<br>")
                HttpContext.Current.Response.Write(this_filename.Substring(Len(drv.Item("recording_url"))).Replace("%20", " "))

                HttpContext.Current.Response.End()

            End Try

            System.Threading.Thread.Sleep(1500)


            Dim max_wait As Integer = 0

            Do Until IO.File.Exists(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending)) Or max_wait = 100

                System.Threading.Thread.Sleep(100)
                max_wait += 1
            Loop
            System.Threading.Thread.Sleep(1500)
        End If


        If file_ending = ".mp3" Or file_ending = ".mp4" Then
            this_filename = HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending)
        Else

            this_filename = HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending)

            Dim output As String = ""
            Dim out_error As String = ""
            Dim destination_file As String = HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & ".mp3")

            'Email_Error("-i " & Chr(34) & this_filename & Chr(34) & " -b:a 13k -y " & destination_file)

            RunFFMPEG("-i " & Chr(34) & this_filename & Chr(34) & " -b:a 13k -y " & destination_file, output, out_error)

            'HttpContext.Current.Response.Write("<!--" & "-i " & Chr(34) & this_filename & Chr(34) & " " & destination_file & "-->")
            Try
                IO.File.Delete(HttpContext.Current.Server.MapPath("/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending))
            Catch ex As Exception

            End Try
            file_ending = ".mp3"

        End If

        this_filename = "/audio/" & drv.Item("appname").ToString & "/" & file_date & "/" & session_id & file_ending
        If IO.File.Exists(HttpContext.Current.Server.MapPath(this_filename)) Then
            Return this_filename
        Else
            Return "this_filename - " & this_filename
        End If

        Return ""


    End Function


End Class
