<%@ WebHandler Language="VB" Class="AWSStreamer" %>

Imports System
Imports System.Web
Imports System.IO
Imports Amazon.S3
Imports Amazon.S3.Model



Public Class AWSStreamer : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest


        Dim client As IAmazonS3
        client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.APSoutheast1)

        Dim getObjRespone As GetObjectResponse = client.GetObject(context.Request("bucket"), context.Request("objectName"))
        Dim stream As New MemoryStream()
        getObjRespone.ResponseStream.CopyTo(stream)

        Dim bytesInStream As Byte() = stream.ToArray()
        stream.Close()

        'Dim ret_file As New FileStreamResult(stream, "audio/mpeg")

        context.Response.Clear()
        context.Response.ContentType = "audio/mpeg"
        'Response.AddHeader("content-disposition", "attachment;    filename=name_you_file.xls")
        context.Response.BinaryWrite(bytesInStream)
        context.Response.[End]()

        'getAudio(context.Request("bucket"), context.Request("objectName"))

        'Dim client As IAmazonS3
        'client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.APSoutheast1)

        'Dim getObjRespone As GetObjectResponse = client.GetObject(context.Request("bucket"), context.Request("objectName"))
        'Dim stream As New MemoryStream()
        'getObjRespone.ResponseStream.CopyTo(stream)



        'Return File(stream, "audio/mpeg")
        ''context.Response.Write("stream")
        ''context.Response.Write(stream.Length)
        ''context.Response.End()

        ''Dim fs As New FileStream(file, FileMode.Open, FileAccess.Read)
        ''context.Response.Buffer = True
        ''context.Response.Clear()
        ''context.Response.ClearContent()
        ''context.Response.ClearHeaders()
        ''context.Response.AddHeader("content-disposition", Convert.ToString("attachement; filename=") & "audio.mp3")
        ''context.Response.AddHeader("content-length", stream.Length.ToString())
        'context.Response.ContentType = "audio/mpeg"
        'Dim bytes As Byte() = New Byte(stream.Length - 1) {}
        'Dim bytesToRead As Integer = CInt(stream.Length)
        'Dim bytesRead As Integer = 0
        'While bytesToRead > 0
        '    Dim n As Integer = stream.Read(bytes, bytesRead, bytesToRead)
        '    If n = 0 Then
        '        Exit While
        '    End If
        '    bytesRead += n
        '    bytesToRead -= n
        'End While
        'bytesToRead = bytes.Length
        'context.Response.OutputStream.Write(bytes, 0, bytes.Length)

        context.Response.End()


    End Sub


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


    Private Function GetAWSStream(Bucket As String, objectName As String) As System.IO.MemoryStream
        Dim client As IAmazonS3
        client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.APSoutheast1)

        Dim getObjRespone As GetObjectResponse = client.GetObject(Bucket, objectName)
        Dim stream As New MemoryStream()
        getObjRespone.ResponseStream.CopyTo(stream)
        Return stream
    End Function

End Class