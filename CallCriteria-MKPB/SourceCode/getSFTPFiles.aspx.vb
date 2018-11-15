Imports System.Data
Imports System.IO
Imports Common

Partial Class getSFTPFiles
    Inherits System.Web.UI.Page

    Private Sub getSFTPFiles_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim dt As DataTable = GetTable("select audiodata.id as aud_id, * from xcc_report_new join audiodata on xcc_report_new.ID = final_xcc_id join app_settings on app_settings.appname = xcc_report_new.appname where file_name like 'sftp://%' and max_reviews = 0 order by audiodata.id desc")

        For Each drv In dt.Rows

            Dim theID As Integer

            Try
                theID = drv.Item("X_ID").ToString
            Catch ex As Exception
                theID = drv.Item("ID").ToString
            End Try


            Dim session_id As String

            If drv.Item("session_id").ToString.IndexOf(" ") > 0 Then
                session_id = Strings.Left(drv.Item("session_id").ToString, drv.Item("session_id").ToString.IndexOf(" "))
            Else
                session_id = Strings.Trim(drv.Item("session_id").ToString)
            End If

            If drv.Item("appname").ToString = "inside up" Then
                session_id = Strings.Trim(drv.Item("profile_id").ToString)
            End If

            Dim phone As String = drv.Item("phone").ToString

            Dim this_filename As String = drv.Item("audio_link").ToString
            Dim call_date As String = Strings.Trim(CDate(drv.Item("call_date").ToString).ToString.Substring(0, drv.Item("call_date").ToString.IndexOf(" ")).Replace("/", "_"))
            If Not System.IO.Directory.Exists("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\") Then
                System.IO.Directory.CreateDirectory("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\")
            End If

            If Strings.Left(drv.Item("audio_link").ToString, 4) = "http" Or Strings.Left(drv.Item("audio_link").ToString, 6) = "/audio" Or Strings.Left(drv.Item("audio_link").ToString, 3) = "ftp" Or Strings.Left(drv.Item("audio_link").ToString, 3) = "sft" Then
                this_filename = Strings.Trim(drv.Item("audio_link").ToString)
            Else
                this_filename = drv.Item("url_prefix") & Strings.Trim(drv.Item("audio_link").ToString)
            End If



            RW(this_filename)

            'TextBox2.Text = (this_filename )


            Dim file_ending As String
            If this_filename.IndexOf("?") > 0 Then
                file_ending = Strings.Mid(this_filename, this_filename.IndexOf("?") - 3, 4).ToLower
            Else
                file_ending = Strings.Right(this_filename, 4).ToLower
            End If




            Try
                Dim sftp_url As String() = drv("file_name").Split("/")
                Dim audio_port As String = "22"
                If sftp_url(2) = "sftp.moatcrm.com" Then
                Else
                    audio_port = drv.Item("audio_port").ToString
                End If

                'RW(sftp_url(2))
                'RW(audio_port)
                'RW(drv.Item("audio_user").ToString)
                'RW(drv.Item("audio_password").ToString)
                Dim sftp_new As New Renci.SshNet.SftpClient(sftp_url(2), audio_port, drv.Item("audio_user").ToString, drv.Item("audio_password").ToString)
                sftp_new.Connect()

                If Not IO.Directory.Exists("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\") Then
                    IO.Directory.CreateDirectory("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\")
                End If

                Dim dest_file As New System.IO.FileStream("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\" & sftp_url(sftp_url.Length - 1), FileMode.OpenOrCreate)

                sftp_new.DownloadFile(drv("file_name").Substring(Len(sftp_url(2)) + 7).Replace("%20", " "), dest_file)

                sftp_new.Disconnect()
                sftp_new.Dispose()


                dest_file.Close()
                dest_file.Dispose()

                RW("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\" & sftp_url(sftp_url.Length - 1) & " downloaded")
            Catch ex As Exception
                RW(ex.Message & "")
                'RW("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\" & session_id & file_ending & "")
                'RW(this_filename.Substring(Len(drv.Item("recording_url"))).Replace("%20", " "))


                Exit Sub 'Return ""
            End Try



            Dim max_wait As Integer = 0

            Do Until System.IO.File.Exists("\\64.111.27.113\d$\wwwroot\audio\" & drv.Item("appname").ToString & "\" & call_date & "\" & session_id & file_ending) Or max_wait = 100

                System.Threading.Thread.Sleep(100)
                max_wait += 1
            Loop

            UpdateTable("update audiodata set file_name = replace(file_name, 'sftp://184.70.40.130/home','/audio/" & drv("appname") & "') where  id = " & drv("aud_id").ToString)


            System.Threading.Thread.Sleep(150)

            Response.Flush()

        Next



    End Sub

    Protected Sub RW(thetext As String)
        Response.Write(thetext & "<br>")
    End Sub

End Class
