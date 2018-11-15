Imports Common
Imports System.Data
Imports HttpExtensions2
Imports Amazon.S3
Imports Amazon.S3.Model

Partial Class download_audio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request("ID") IsNot Nothing Then
            Dim record_Dt As DataTable = GetTable("select isnull(dnis,isnull(ani,isnull(phone,''))) as thephone,call_date, onAWS, replace(agent,' ','_') as agent, audio_link from xcc_report_new where id =  (select review_id from form_score3 where id = " & Request("ID") & ")")
            If record_Dt.Rows.Count > 0 Then
                Dim filename As String = record_Dt.Rows(0).Item("call_date").ToString.Replace("/", "_").Replace(" 12:00:00 AM", "") & "_" & record_Dt.Rows(0).Item("thephone").ToString & "_" & record_Dt.Rows(0).Item("agent").ToString & ".mp3"
                Dim he As New HttpExtensions2


                If record_Dt.Rows(0).Item("onaws").ToString = "True" Then

                    Dim audio_link As String = record_Dt.Rows(0).Item("audio_link").ToString

                    Dim s3Client As IAmazonS3
                    s3Client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.APSoutheast1)

                    Dim request1 As GetPreSignedUrlRequest = New GetPreSignedUrlRequest()

                    Dim URL_REQ As New GetPreSignedUrlRequest
                    URL_REQ.BucketName = "callcriteriasingapore" & Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/")
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1)
                    URL_REQ.Expires = DateTime.Now.AddHours(1)
                    he.ForceStream(s3Client.GetPreSignedURL(URL_REQ), filename)
                Else
                    he.ForceStream("http://files.callcriteria.com" & record_Dt.Rows(0).Item("audio_link").ToString, filename)

                End If





            End If
        ElseIf Request("GUID") IsNot Nothing Then
            Dim record_Dt As DataTable = GetTable("select filename,s3key from review_documents where s3key=  '" & Request("GUID") & "'")

            If record_Dt.Rows.Count > 0 Then
                Dim he As New HttpExtensions2
                Dim audio_link As String = record_Dt.Rows(0).Item("s3key").ToString
                Dim filename As String = record_Dt.Rows(0).Item("filename").ToString
                Dim s3Client As IAmazonS3
                s3Client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.USWest2)

                Dim request1 As GetPreSignedUrlRequest = New GetPreSignedUrlRequest()

                Dim URL_REQ As New GetPreSignedUrlRequest
                URL_REQ.BucketName = "callcriteriaftproot/uploads/ClientFiles"
                URL_REQ.Key = audio_link
                URL_REQ.Expires = DateTime.Now.AddHours(1)
                he.ForceStream(s3Client.GetPreSignedURL(URL_REQ), filename)
            End If
        End If
    End Sub
End Class
