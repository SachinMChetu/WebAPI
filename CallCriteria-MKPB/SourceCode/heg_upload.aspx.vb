Imports Common
Imports System.Data
Imports System.Net
Partial Class heg_upload
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim calls_dt As DataTable = GetTable("select xcc_report_new.id, audio_link, phone1, call_date from heg_missing2 join xcc_report_new on ((xcc_report_new.dnis = heg_missing2.phone1) or (xcc_report_new.ani = heg_missing2.phone1)) where left(audio_link, 6) = '/audio'")

        For Each dr As DataRow In calls_dt.Rows
            Dim files() As String = dr("audio_link").ToString.Split("/")
            Response.Write(dr("audio_link").ToString & "<br>")
            Response.Flush()

            Dim output As String = ""
            Dim out_error As String = ""

            Dim call_date As String = CDate(dr("call_date")).ToShortDateString.Replace("/", "_")

            RunFFMPEG("-i " & Chr(34) & Server.MapPath(dr("audio_link").ToString) & Chr(34) & "  -ar 8000 -b:a 8000 -y  " & Server.MapPath("/audio/HEG/" & dr("phone1") & ".wav"), output, out_error)


            If out_error = "" Then
                UpdateTable("update xcc_report_new set to_upload = 0 where id = " & dr("id").ToString)
            End If

            'Response.Write(output & "<br>")
            'Response.Flush()


            'System.Threading.Thread.Sleep(5000)

            'Dim ftpClient As FtpWebRequest = DirectCast(FtpWebRequest.Create("ftp://higheredgrowth.smartfile.com" & "/" & dr("phone1") & ".wav"), FtpWebRequest)
            'ftpClient.Credentials = New System.Net.NetworkCredential("estomes", "FRAvdlBnHP")
            'ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile
            'ftpClient.UseBinary = True
            'ftpClient.KeepAlive = True
            'Dim fi As New System.IO.FileInfo(Server.MapPath(dr("audio_link")))
            'ftpClient.ContentLength = fi.Length
            'Dim buffer As Byte() = New Byte(4096) {}
            'Dim bytes As Integer = 0
            'Dim total_bytes As Integer = CInt(fi.Length)
            'Dim fs As System.IO.FileStream = fi.OpenRead()
            'Dim rs As System.IO.Stream = ftpClient.GetRequestStream()
            'While total_bytes > 0
            '    bytes = fs.Read(buffer, 0, buffer.Length)
            '    rs.Write(buffer, 0, bytes)
            '    total_bytes = total_bytes - bytes
            'End While
            ''fs.Flush();
            'fs.Close()
            'rs.Close()
            'Dim uploadResponse As FtpWebResponse = DirectCast(ftpClient.GetResponse(), FtpWebResponse)
            'Dim value As String = uploadResponse.StatusDescription
            'uploadResponse.Close()

            'SendFTP(files(files.Length - 1), Server.MapPath(dr("audio_link")), "ftp://higheredgrowth.smartfile.com", "estomes", "FRAvdlBnHP", "")

        Next

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim file_dt As DataTable = GetTable("select phoneDay, filename from heg_fond3 join wav_data on wav_data.session_id = heg_missing2.[session id] ")
        For Each dr As DataRow In file_dt.Rows
            Dim WebClient_down As New System.Net.WebClient()

            WebClient_down.Credentials = New System.Net.NetworkCredential("ryan", "Ot32at515", "")
            WebClient_down.DownloadFile("http://www.directboard2.net" & dr("filename"), Server.MapPath("/audio/HEG3/" & dr("phoneDay") & ".wav"))

            Do Until Not WebClient_down.IsBusy
                System.Threading.Thread.Sleep(100)
            Loop

            Response.Write("http://www.directboard2.net" & dr("filename") & "<br>")
            Response.Flush()

        Next
    End Sub
End Class
