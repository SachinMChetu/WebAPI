Imports Common
Imports System.Data
Partial Class Call_time_update
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Response.Buffer = False


        Dim dt As DataTable = GetTable("select id as x_ID, audio_link from xcc_report_new where left(audio_link,6) = '/audio' and scorecard = 396 ") 'and review_date > dateadd(d,-15,dbo.getMTDate())")

        For Each dr As DataRow In dt.Rows
            If IO.File.Exists(Server.MapPath(dr("audio_link").replace("%20", " "))) Then

                Dim call_lenth As Double = GetMediaDuration(Server.MapPath(dr("audio_link").replace("%20", " ")))

                'Dim tlf As TagLib.File = TagLib.File.Create(HttpContext.Current.Server.MapPath(dr("audio_link")))
                Dim call_len As TimeSpan = New TimeSpan(0, 0, CInt(call_lenth))
                Dim call_time As DateTime = CDate("12/30/1899") + call_len

                'Try
                UpdateTable("update XCC_REPORT_NEW set call_time = '" & call_time.ToString & "' where ID = " & dr("x_ID").ToString)
                UpdateTable("update form_score3 set call_length = '" & CInt(call_lenth) & "'  where  review_id = " & dr("x_ID").ToString)
                Response.Write(dr("audio_link") & " " & call_time.ToString & "<br>")
                'Catch ex As Exception

                'End Try
            Else

                If IO.File.Exists(Server.MapPath(dr("audio_link").replace(".mp3", "_working.mp3"))) Then
                    response.write("working found = " & dr("audio_link").tostring & "<br>")
                    io.file.Move(Server.MapPath(dr("audio_link").replace(".mp3", "_working.mp3")), Server.MapPath(dr("audio_link")))
                End If

                response.write("Not found = " & dr("audio_link").tostring & "<br>")
            End If
        Next


        'Dim dt As DataTable = GetTable("select * from XCC_REPORT_NEW left join form_score3 on form_score3.review_id = XCC_REPORT_NEW.id where left(audio_link,6) = '/audio' and call_length is null")


        'For Each dr As DataRow In dt.Rows
        '    If IO.File.Exists(Server.MapPath(dr("audio_link"))) Then

        '        Dim call_lenth As Double = GetMediaDuration(Server.MapPath(dr("audio_link")))

        '        'Dim tlf As TagLib.File = TagLib.File.Create(HttpContext.Current.Server.MapPath(dr("audio_link")))
        '        Dim call_len As TimeSpan = New TimeSpan(0, 0, CInt(call_lenth / 2))
        '        Dim call_time As DateTime = CDate("12/30/1899") + call_len

        '        Try
        '            UpdateTable("update XCC_REPORT_NEW set call_time = '" & call_time.ToString & "' where ID = " & dr("ID").ToString)
        '            'UpdateTable("update form_score3 set call_length = '" & CInt(call_lenth / 2) & "'  where  review_id = " & dr("ID").ToString)
        '            Response.Write("update XCC_REPORT_NEW set call_time = '" & call_time.ToString & "' where ID = " & dr("ID").ToString & "<br>")
        '            'Response.Write(dr("call_time").ToString & " = " & call_time.ToString & "<br>")

        '            'Response.Write(dr("talk_time").ToString & " " & CInt(call_lenth / 120) & ":" & Math.Abs(CInt((call_lenth - CInt(call_lenth)) * 60)) & "<br>")
        '            Response.Flush()
        '        Catch ex As Exception
        '            Response.Write("update failed - " & ex.Message & "<br>")
        '            Response.Flush()
        '        End Try
        '    Else
        '        Response.Write("update failed - " & Server.MapPath(dr("audio_link")) & " not found<br>")
        '        Response.Flush()
        '    End If
        'Next

    End Sub
End Class
