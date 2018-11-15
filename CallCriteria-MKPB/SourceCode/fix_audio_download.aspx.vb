Imports Common
Imports System.Data
Partial Class fix_audio_download
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim dt As DataTable = GetTable("select audio_link,* from vwForm where left(audio_link, 6) = 'ftp://' and max_reviews = 1 order by call_date desc")
        For Each dr In dt.Rows

            'Response.Write("/audio/" & dr.Item("appname").ToString & "/" & CDate(dr.Item("call_date")).ToString("M_d_yyyy") & "/" & dr.Item("session_id").ToString & ".mp3")
            'Response.End()

            If IO.File.Exists(Server.MapPath("/audio/" & dr.Item("appname").ToString & "/" & CDate(dr.Item("call_date")).ToString("M_d_yyyy") & "/" & dr.Item("session_id").ToString & ".mp3")) Then
                Response.Write("Updated " & dr("F_ID").ToString & " to /audio/" & dr.Item("appname").ToString & "/" & CDate(dr.Item("call_date")).ToString("M_d_yyyy") & "/" & dr.Item("session_id").ToString & ".mp3<br>")
                UpdateTable("update XCC_REPORT_NEW set audio_link = '/audio/" & dr.Item("appname").ToString & "/" & CDate(dr.Item("call_date")).ToString("M_d_yyyy") & "/" & dr.Item("session_id").ToString & ".mp3' where ID = " & dr("X_ID").ToString)
            Else
                GetAudioFileName(dr)
                Response.Write("No file for " & dr("F_ID").ToString & " to /audio/" & dr.Item("appname").ToString & "/" & CDate(dr.Item("call_date")).ToString("M_d_yyyy") & "/" & dr.Item("session_id").ToString & ".mp3<br>")
            End If

        Next
        Response.End()
    End Sub
End Class
