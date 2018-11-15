
Partial Class download_gvd_daily
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ''http://www.directboard2.net/gmbf9/reports/Call_Log_eStomes.csv', false, 'ryan','Ot32at515' 

        Dim WebClient_down As New System.Net.WebClient()
        'recording_user	record_password
        'estomes	7BanxdC1aLpx

        WebClient_down.Credentials = New System.Net.NetworkCredential("ryan", "Ot32at515", "")

        WebClient_down.DownloadFile("http://www.directboard2.net/gmbf9/reports/Call_Log_eStomes.csv", "D:\wwwroot\audio\gvd\gvd_call_data.csv")

    End Sub
End Class
