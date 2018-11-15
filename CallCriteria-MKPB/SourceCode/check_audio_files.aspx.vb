Imports System.Data
Imports Common
Partial Class check_audio_files
    Inherits System.Web.UI.Page

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click



        Dim dt As DataTable = GetTable("select * from calibration_pending join vwForm on vwForm.f_id = form_id where vwForm.appname = 'XL' and call_date > '7/19/2015'")

        For Each dr As DataRow In dt.Rows
            If IO.File.Exists(Server.MapPath(dr("audio_link").ToString)) Then
                Response.Write("Found - " & dr("audio_link").ToString & "<br>")
            Else
                Response.Write("Missing - " & dr("audio_link").ToString & "<br>")
            End If
        Next

    End Sub
End Class
