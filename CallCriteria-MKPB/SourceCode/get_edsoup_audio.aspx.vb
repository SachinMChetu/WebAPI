Imports System.Data
Imports Common

Partial Class get_edsoup_audio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim appname As String = ""

        Dim date_dt As DataTable = GetTable("select * from xcc_report_new where xcc_report_new.audio_link is null and pending_id in (select pending_id from AudioData)")

        For Each date_dr As DataRow In date_dt.Rows

            Dim output As String = ""
            Dim out_error As String = ""

            Dim concats As String = ""

            Dim audio_dt As DataTable = GetTable("select * from audiodata join xcc_report_new on xcc_report_new.pending_id = audiodata.pending_id where xcc_report_new.pending_id = " & date_dr("pending_id") & "  and file_name <> ''  order by file_date")
            Dim new_call As String

            Dim session_id As String = ""

            Dim file_count As Integer = 0

            For Each dr As DataRow In audio_dt.Rows

                Try
                    Dim WebClient_down As New System.Net.WebClient()

                    new_call = CDate(dr("date")).ToShortDateString.Replace("/", "_")

                    If Not IO.Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" & dr("appname").ToString & "/" & new_call & "/")) Then
                        IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" & dr("appname").ToString & "/" & new_call & "/"))
                    End If

                    appname = dr("appname")

                    session_id = dr("session_id")

                    WebClient_down.DownloadFile(dr("file_name").replace("%40", "@").replace("+", " "), Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".wav"))

                    ' System.Threading.Thread.Sleep(5000)


                    'Dim max_wait As Integer = 0
                    'Do Until IO.File.Exists(Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".wav")) Or max_wait = 1000

                    '    System.Threading.Thread.Sleep(100)
                    '    max_wait += 1
                    'Loop


                    Do Until WebClient_down.IsBusy = False
                        System.Threading.Thread.Sleep(100)
                    Loop

                    System.Threading.Thread.Sleep(1000)

                    WebClient_down.Dispose()
                    WebClient_down = Nothing
                    'Dim incoming As String = Server.MapPath("/audio/edsoup/" & new_call & "/" & dr("file_name").replace("%40", "@").replace("+", " "))


                    RunFFMPEG("-i " & Chr(34) & Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".wav") & Chr(34) & " -b:a 13k -y " & Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".mp3"), output, out_error)


                    IO.File.Delete(Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".wav"))


                    If concats <> "" Then
                        concats &= "|" & Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".mp3")
                    Else
                        concats = Server.MapPath("/audio/" & dr("appname") & "/" & new_call & "/" & session_id & "_" & file_count & ".mp3")
                    End If

                    file_count += 1
                Catch ex As Exception

                End Try

                'Download each file


            Next

            Dim concat As String = "-i " & Chr(34) & "concat:" & concats & Chr(34) & " -c copy d:\wwwroot\Audio\" & appname & "\" & new_call & "\" & session_id & "_TEST.mp3 -y"
            RunFFMPEG(concat, output, out_error)


            For x As Integer = 0 To file_count - 1
                IO.File.Delete(Server.MapPath("/audio/" & appname & "/" & new_call & "/" & session_id & "_" & x & ".mp3"))
            Next

            Try
                IO.File.Move("d:\wwwroot\Audio\" & appname & "\" & new_call & "\" & session_id & "_TEST.mp3", "d:\wwwroot\Audio\" & appname & "\" & new_call & "\" & session_id & ".mp3")
                Response.Write("/audio/" & appname & "/" & new_call & "/" & session_id & ".mp3<br>")
            Catch ex As Exception
                Response.Write(ex.Message & "<br>")
            End Try

            Response.Write("update xcc_report_new set audio_link = '" & "/audio/" & appname & "/" & new_call & "/" & session_id & ".mp3' where pending_id = " & date_dr.Item("pending_id") & "<br>")

            Response.Flush()


            UpdateTable("update xcc_report_new set audio_link = '" & "/audio/" & appname & "/" & new_call & "/" & session_id & ".mp3' where pending_id = " & date_dr.Item("pending_id"))

        Next
    End Sub
End Class
