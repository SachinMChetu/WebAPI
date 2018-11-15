Imports System.Data
Imports Common

Partial Class check_missing_audio
    Inherits System.Web.UI.Page

    Private Sub btnGO_Click(sender As Object, e As EventArgs) Handles btnGO.Click

        Literal1.Text = ""

        Dim file_dt As DataTable = GetTable("select * from xcc_report_new where max_reviews = 0 and audio_link is not null and left(audio_link,6) = '/audio'")

        For Each dr In file_dt.Rows
            Try
                If Not IO.File.Exists(Server.MapPath(dr("audio_link"))) Then
                    Literal1.Text &= "update xcc_report_new set audio_link = null where id = " & dr("ID") & " --" & dr("audio_link") & " " & dr("appname") & "<br>"
                End If
            Catch ex As Exception
                Literal1.Text &= "update xcc_report_new set audio_link = null where id = " & dr("ID") & " --" & dr("audio_link") & " " & dr("appname") & " " & ex.Message & "<br>"
            End Try

        Next
    End Sub
End Class
