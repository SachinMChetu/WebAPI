Imports Common
Imports System.Data
Partial Class import_dw_calls
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim dw_dt As DataTable = GetTable("select * from dw_calls")

        'Rename .aspx files
        For Each dr As DataRow In dw_dt.Rows
            'Rename .aspx files
            Try
                IO.File.Move("C:\Users\Administrator\Dropbox\Calls\" & dr("agent") & "\" & dr("filename"), "D:\wwwroot\Audio\DWasales\9_4_2014\" & dr("filename").ToString.Replace("aspx", "mp3").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

            'UpdateTable("insert into xcc_report_new (agent, audio_link, session_id, call_date, timestamp, appname) select '" & dr("agent") & "','/audio/dwasales/9_4_2014/" & dr("filename").ToString.Replace("aspx", "mp3").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & "','" & dr("agent").ToString.Replace(" ", "_") & dr("filename").ToString.Replace("aspx", "mp3").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & ",'9/4/2014','9/4/2014','dwasales'")
            Response.Write("insert into xcc_report_new (agent, audio_link, session_id, call_date, timestamp, appname) select '" & dr("agent") & "','/audio/dwasales/9_4_2014/" & dr("filename").ToString.Replace("aspx", "mp3").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & "','" & dr("agent").ToString.Replace(" ", "_") & dr("filename").ToString.Replace("aspx", "mp3").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & ",'9/4/2014','9/4/2014','dwasales'<br>")
        Next

    End Sub
End Class
