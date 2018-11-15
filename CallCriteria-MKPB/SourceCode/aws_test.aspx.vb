Imports Common
Imports Amazon
Imports Amazon.S3
Imports Amazon.S3.Model


Partial Class aws_test
    Inherits System.Web.UI.Page
    Shared bucketName As String
    Shared keyName As String
    Shared client As IAmazonS3

    Public call_count As Integer = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListingObjects()
    End Sub

    Private Sub process_files(aws_response As ListObjectsResponse, edu_bucket As String)
        For Each entry As S3Object In aws_response.S3Objects
            'RW(call_num & " key = " & entry.Key & " size = " & entry.Size & " date = " & entry.LastModified)
            'RW(entry.ETag)

            If entry.LastModified > DateAdd(DateInterval.Day, -90, Today) Then
                'If edu_bucket = "edu_clickspark" Then
                Dim URL_REQ As New GetPreSignedUrlRequest
                URL_REQ.BucketName = edu_bucket
                URL_REQ.Key = entry.Key
                URL_REQ.Expires = DateTime.Now.AddYears(1)
                RW(call_count & " " & edu_bucket & " " & client.GetPreSignedURL(URL_REQ))
                Response.Flush()

                UpdateTable("insert into edufficient_filenames(filename, bucket) select '" & client.GetPreSignedURL(URL_REQ) & "','" & edu_bucket & "'")
                call_count += 1
            End If

            'RW(client.GeneratePreSignedURL(TextBox1.Text, entry.Key, DateAdd(DateInterval.Day, 1, Now), ""))
        Next
    End Sub



    Private Sub ListingObjects()

        Dim call_num As Integer = 1


        UpdateTable("truncate table edufficient_filenames;")

        Try
            '
            Dim buckets() As String = Split("edu_zu|edu_neutroninteractive|edu_aquainteractive|edu_howlmedia|edu_integrate|edu_beckerinteractive|edu_acmedia|edu_leadprodirect|edu_academixdirect|edu_accelerex|edu_adhere|edu_backtolearn|edu_birddogmedia|edu_clickspark|edu_collegebound|edu_cysmedia|edu_danemedia|edu_dgsworld|edu_digitalmediasolutions|edu_dmipartners|edu_dmsgroup|edu_doublepositive|edu_educationdynamics|edu_edutrek|edu_higheredgrowth|edu_higherleveleducation|edu_ifficient|edu_lead5media|edu_leadhuntermedia|edu_lgtechnet|edu_mediaspike|edu_plattform|edu_pmamedia|edu_proacademix|edu_quinstreet|edu_rexdirectnet|edu_targetdirect|edu_transcendmedia|edu_ttcameridial|edu_vinylinteractive|edu_zetainteractive", "|")
            'Dim buckets() As String = Split("edu_integrate", "|")

            For Each edu_bucket In buckets
                Dim request As New ListObjectsRequest()
                request.BucketName = edu_bucket

                call_count = 0

                Dim processed As Boolean = False

                Do Until processed = True

                    Dim response As ListObjectsResponse = client.ListObjects(request)
                    process_files(response, edu_bucket)


                    If (response.IsTruncated) Then
                        request.Marker = response.NextMarker
                    Else
                        processed = True
                    End If
                Loop



                'Dim response As ListObjectsResponse = client.ListObjects(request)


                '    While response.istruncated()

                '    request.Marker = response.NextMarker
                '    response = client.listNextBatchOfObjects(response)

                '    process_files(response, edu_bucket)

                'End While


            Next

            UpdateTable("update xcc_report_new set audio_link = filename from edufficient_filenames where filename like '%' + phone + '%' and audio_link is null and appname = 'edufficient' and max_reviews = 0")


        Catch amazonS3Exception As AmazonS3Exception
            If amazonS3Exception.ErrorCode IsNot Nothing AndAlso (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") OrElse amazonS3Exception.ErrorCode.Equals("InvalidSecurity")) Then
                RW("Please check the provided AWS Credentials.")
                RW("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3")
            Else
                RW("An error occurred with the message '" & amazonS3Exception.Message & "' when listing objects")
            End If
        End Try
    End Sub




    Private Sub ListingBuckets()
        Try
            Dim lb_response As ListBucketsResponse = client.ListBuckets()
            For Each bucket As S3Bucket In lb_response.Buckets
                RW("You own Bucket with name: " & bucket.BucketName)
            Next
        Catch amazonS3Exception As AmazonS3Exception
            If amazonS3Exception.ErrorCode IsNot Nothing AndAlso (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") OrElse amazonS3Exception.ErrorCode.Equals("InvalidSecurity")) Then
                RW("Please check the provided AWS Credentials.")
                RW("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3")
            Else
                RW("An Error, number " & amazonS3Exception.ErrorCode & ", occurred when listing buckets with the message " & amazonS3Exception.Message)
            End If
        End Try
    End Sub

    Protected Sub RW(send As String)
        Response.Write(send & "<br>")
    End Sub

    Private Sub aws_test_Load(sender As Object, e As EventArgs) Handles Me.Load
        client = New AmazonS3Client()
        ListingObjects()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListingObjects()
    End Sub
End Class
