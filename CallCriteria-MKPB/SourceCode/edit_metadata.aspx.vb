Imports System.Data
Imports Common
Imports CallCriteriaAPI
Imports CCInternalAPI
Imports Amazon.S3.Model
Imports Amazon.S3

Partial Class manual_call
    Inherits System.Web.UI.Page

    Protected Sub btnAddAudio_Click(sender As Object, e As EventArgs) ' Handles btnAddAudio.Click
        Dim audFUP As FileUpload = sender.parent.findcontrol("audFUP")
        Dim ddlAppname As Label = sender.parent.findcontrol("ddlAppname")
        Dim txtCall_Date As TextBox = sender.parent.findcontrol("txtCall_Date")
        Dim txtOrder As TextBox = sender.parent.findcontrol("txtOrder")
        Dim audfile_name As TextBox = sender.parent.findcontrol("audfile_name")
        Dim gvAudio As GridView = sender.parent.findcontrol("gvAudio")


        txtCall_Date.Text = CDate(txtCall_Date.Text).ToShortDateString

        If audFUP.FileName <> "" Then

            If Not System.IO.Directory.Exists(Server.MapPath("/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/")) Then
                System.IO.Directory.CreateDirectory(Server.MapPath("/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/"))
            End If

            audFUP.SaveAs(Server.MapPath("/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/" & audFUP.FileName))
            audfile_name.Text = "/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/" & audFUP.FileName
        End If

        'Dim ddlAddExisting As DropDownList = sender.parent.findcontrol("ddlAddExisting")
        'Dim hdnExistingFile As HiddenField = sender.parent.findcontrol("hdnExistingFile")


        'Dim output_name As String = ""

        'If ddlAddExisting.SelectedValue = "Add to Front" Then

        '    If audFUP.FileName <> "" Then

        '        output_name = ConcatMP3Files(Server.MapPath("/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/" & audFUP.FileName), Server.MapPath(hdnExistingFile.Value), Server.MapPath(hdnExistingFile.Value))
        '    Else
        '        output_name = ConcatMP3Files(audfile_name.Text, Server.MapPath(hdnExistingFile.Value), Server.MapPath(hdnExistingFile.Value))
        '    End If
        'End If

        'If ddlAddExisting.SelectedValue = "Add to Rear" Then


        '    If audFUP.FileName <> "" Then

        '        output_name = ConcatMP3Files(Server.MapPath(hdnExistingFile.Value), Server.MapPath("/audio/" & ddlAppname.Text & "/" & txtCall_Date.Text.Replace("/", "_") & "/" & audFUP.FileName), Server.MapPath(hdnExistingFile.Value))
        '    Else
        '        output_name = ConcatMP3Files(Server.MapPath(hdnExistingFile.Value), audfile_name.Text, Server.MapPath(hdnExistingFile.Value))
        '    End If
        'End If

        'If ddlAddExisting.SelectedValue = "Replace Audio" Then

        '    If Request("XID") IsNot Nothing Then
        '        UpdateTable("update xcc_report_new set audio_link = '" & audfile_name.Text & "' where id = " & Request("XID"))
        '    End If

        '    If Request("ID") IsNot Nothing Then
        '        UpdateTable("update xcc_report_new set audio_link = '" & audfile_name.Text & "' where id = (select review_id from vwForm where f_id = " & Request("ID") & ")")
        '    End If


        'End If


        'If output_name <> "" Then
        '    If Request("XID") IsNot Nothing Then
        '        UpdateTable("update xcc_report_new set audio_link = '" & output_name & "' where id = " & Request("XID"))
        '    End If

        '    If Request("ID") IsNot Nothing Then
        '        UpdateTable("update xcc_report_new set audio_link = '" & output_name & "' where id = (select review_id from vwForm where f_id = " & Request("ID") & ")")
        '    End If
        'End If

        Dim dt As DataTable = FormatDataTable("audio")
        Dim dr As DataRow

        For Each gvr As GridViewRow In gvAudio.Rows
            dr = dt.NewRow
            dr("file_name") = gvr.Cells(1).Text
            dr("file_date") = gvr.Cells(2).Text
            dr("file_order") = gvr.Cells(3).Text
            ' Add the row
            dt.Rows.Add(dr)
        Next


        If audfile_name.Text <> "" Then
            ' Populate the datatable with your data (put this in appropriate loop)
            dr = dt.NewRow
            dr("file_name") = audfile_name.Text
            dr("file_date") = txtCall_Date.Text
            dr("file_order") = txtOrder.Text
            ' Add the row
            dt.Rows.Add(dr)
        End If


        dt.AcceptChanges()

        audfile_name.Text = ""
        txtOrder.Text = ""

        Dim dt2 As DataTable = dt.Select("", "file_order").CopyToDataTable

        gvAudio.DataSource = dt2
        gvAudio.DataBind()

    End Sub

    Protected Sub btnAddOther_Click(sender As Object, e As EventArgs) 'Handles btnAddOther.Click

        Dim othdata_type As TextBox = sender.parent.findcontrol("othdata_type")
        Dim othdata_value As TextBox = sender.parent.findcontrol("othdata_value")
        Dim othdata_key As TextBox = sender.parent.findcontrol("othdata_key")
        Dim gvOther As GridView = sender.parent.findcontrol("gvOther")
        Dim gvCurrent As GridView = sender.parent.findcontrol("gvCurrent")

        Dim dt As DataTable = FormatDataTable("other")
        Dim dr As DataRow

        For Each gvr As GridViewRow In gvOther.Rows
            dr = dt.NewRow
            dr("data_key") = gvr.Cells(0).Text
            dr("data_value") = gvr.Cells(1).Text
            dr("data_type") = gvr.Cells(2).Text
            ' Add the row
            dt.Rows.Add(dr)
        Next

        ' Populate the datatable with your data (put this in appropriate loop)
        dr = dt.NewRow
        dr("data_key") = othdata_key.Text
        dr("data_value") = othdata_value.Text
        dr("data_type") = othdata_type.Text
        ' Add the row
        dt.Rows.Add(dr)

        dt.AcceptChanges()

        othdata_key.Text = ""
        othdata_value.Text = ""
        othdata_type.Text = ""

        gvOther.DataSource = dt
        gvOther.DataBind()
        gvCurrent.DataBind()

    End Sub

    Protected Sub btnAddSchool_Click(sender As Object, e As EventArgs) 'Handles btnAddSchool.Click


        Dim schModality As TextBox = sender.parent.findcontrol("schModality")
        Dim schAOI1 As TextBox = sender.parent.findcontrol("schAOI1")
        Dim schAOI2 As TextBox = sender.parent.findcontrol("schAOI2")
        Dim schL1_SubjectName As TextBox = sender.parent.findcontrol("schL1_SubjectName")
        Dim schL2_SubjectName As TextBox = sender.parent.findcontrol("schL2_SubjectName")
        Dim schOrigin As TextBox = sender.parent.findcontrol("schOrigin")


        Dim schDegreeOfInterest As TextBox = sender.parent.findcontrol("schDegreeOfInterest")
        Dim schCollege As TextBox = sender.parent.findcontrol("schCollege")
        Dim schSchool As TextBox = sender.parent.findcontrol("schSchool")
        Dim gvSchools As GridView = sender.parent.findcontrol("gvSchools")
        Dim dsSchools As SqlDataSource = sender.parent.findcontrol("dsSchools")



        dsSchools.InsertParameters("School").DefaultValue = schSchool.Text
        dsSchools.InsertParameters("AOI1").DefaultValue = schAOI1.Text
        dsSchools.InsertParameters("AOI2").DefaultValue = schAOI2.Text
        dsSchools.InsertParameters("L1_SubjectName").DefaultValue = schL1_SubjectName.Text
        dsSchools.InsertParameters("L2_SubjectName").DefaultValue = schL2_SubjectName.Text
        dsSchools.InsertParameters("Modality").DefaultValue = schModality.Text
        dsSchools.InsertParameters("xcc_id").DefaultValue = hdnXCCID.Value
        dsSchools.InsertParameters("College").DefaultValue = schCollege.Text
        dsSchools.InsertParameters("DegreeOfInterest").DefaultValue = schDegreeOfInterest.Text
        dsSchools.InsertParameters("origin").DefaultValue = schOrigin.Text
        dsSchools.Insert()


        schSchool.Text = ""
        schL1_SubjectName.Text = ""
        schL2_SubjectName.Text = ""
        schCollege.Text = ""
        schDegreeOfInterest.Text = ""
        schAOI1.Text = ""
        schAOI2.Text = ""
        schModality.Text = ""
        schOrigin.Text = ""

        gvSchools.DataBind()
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) 'Handles btnSubmit.Click

        Dim btn As Button = sender


        If btn.CommandName = "Insert" Then

            Dim ddlAppname As DropDownList = sender.parent.findcontrol("ddlAppname")
            Dim ddlScorecard As DropDownList = sender.parent.findcontrol("ddlScorecard")
            Dim schCollege As TextBox = sender.parent.findcontrol("schCollege")
            Dim schSchool As TextBox = sender.parent.findcontrol("schSchool")
            Dim gvOther As GridView = sender.parent.findcontrol("gvOther")
            Dim gvAudio As GridView = sender.parent.findcontrol("gvAudio")
            Dim gvSchools As GridView = sender.parent.findcontrol("gvSchools")


            Dim sql As String = "declare @new_id int; Insert into xcc_report_new (must_review, [date], appname, scorecard "
            Dim fields As String = "select 1, dbo.getMTDate(), '" & ddlAppname.Text & "', " & ddlScorecard.SelectedValue

            For Each ri In Request.Form.AllKeys
                ' ContentPlaceHolder1_txtSESSION_ID
                If ri.ToString.IndexOf("txt") > 0 Then
                    Dim theField As String = ri.Replace("ctl00$ContentPlaceHolder1$txt", "")
                    If Request.Form(ri) <> "" Then
                        sql &= ", " & theField & " "
                        fields &= ", '" & Request.Form(ri).Replace("'", "''") & "' "
                    End If
                End If
            Next

            sql = sql & ")" & fields & "; select @new_id = @@identity; select @new_id;"


            Response.Write(sql)
            Response.End()

            Dim new_id_dt As DataTable = Common.GetTable(sql)



            If new_id_dt.Rows.Count > 0 Then
                Dim new_id As Integer = new_id_dt.Rows(0).Item(0)

                UpdateTable("update xcc_report_new set pending_id = id where id = " & new_id)

                For Each gvr As GridViewRow In gvOther.Rows
                    sql = "insert into otherFormData(xcc_id"
                    fields = "select " & new_id

                    If gvr.Cells(0).Text <> "" Then
                        sql &= ", data_key"
                        fields &= ", '" & gvr.Cells(0).Text & "'"
                    End If
                    If gvr.Cells(1).Text <> "" Then
                        sql &= ", data_value"
                        fields &= ", '" & gvr.Cells(1).Text & "'"
                    End If
                    If gvr.Cells(2).Text <> "" Then
                        sql &= ", data_type"
                        fields &= ", '" & gvr.Cells(2).Text & "'"
                    End If

                    UpdateTable(sql & ") " & fields)
                Next



                'save school data
                For Each gvr As GridViewRow In gvAudio.Rows
                    sql = "insert into audiodata(pending_id"
                    fields = "select " & new_id

                    If gvAudio.Rows.Count = 1 Then
                        gvr.Cells(0).Text = gvr.Cells(0).Text.Replace("http://callcriteria.com", "")
                        UpdateTable("update xcc_report_new set audio_link = '" & gvr.Cells(0).Text & "' where id = " & new_id)
                    Else
                        gvr.Cells(0).Text = gvr.Cells(0).Text.Replace("http://callcriteria.com", "\\64.111.27.113\D$\wwwroot")
                        gvr.Cells(0).Text = gvr.Cells(0).Text.Replace("/", "\")
                    End If

                    If gvr.Cells(0).Text <> "" Then
                        sql &= ", file_name"
                        fields &= ", '" & gvr.Cells(0).Text & "'"
                    End If
                    If gvr.Cells(1).Text <> "" Then
                        sql &= ", file_date"
                        fields &= ", '" & gvr.Cells(1).Text & "'"
                    End If



                    UpdateTable(sql & ") " & fields)
                Next


                For Each gvr As GridViewRow In gvSchools.Rows
                    sql = "insert into school_x_data(xcc_id"
                    fields = "select " & new_id

                    If gvr.Cells(0).Text <> "" Then
                        sql &= ", SCHOOL"
                        fields &= ", '" & gvr.Cells(0).Text & "'"
                    End If
                    If gvr.Cells(1).Text <> "" Then
                        sql &= ", COLLEGE"
                        fields &= ", '" & gvr.Cells(1).Text & "'"
                    End If
                    If gvr.Cells(2).Text <> "" Then
                        sql &= ", DEGREEOFINTEREST"
                        fields &= ", '" & gvr.Cells(2).Text & "'"
                    End If

                    If gvr.Cells(3).Text <> "" Then
                        sql &= ", MODALITY"
                        fields &= ", '" & gvr.Cells(3).Text & "'"
                    End If

                    If gvr.Cells(4).Text <> "" Then
                        sql &= ", AOI1"
                        fields &= ", '" & gvr.Cells(4).Text & "'"
                    End If

                    If gvr.Cells(5).Text <> "" Then
                        sql &= ", AOI2"
                        fields &= ", '" & gvr.Cells(5).Text & "'"
                    End If

                    If gvr.Cells(6).Text <> "" Then
                        sql &= ", L1_SUBJECTNAME"
                        fields &= ", '" & gvr.Cells(6).Text & "'"
                    End If

                    If gvr.Cells(7).Text <> "" Then
                        sql &= ", L2_SUBJECTNAME"
                        fields &= ", '" & gvr.Cells(7).Text & "'"
                    End If

                    If gvr.Cells(8).Text <> "" Then
                        sql &= ", Origin"
                        fields &= ", '" & gvr.Cells(8).Text & "'"
                    End If


                    Response.Write(sql)
                    Response.End()

                    UpdateTable(sql & ") " & fields)
                Next


            End If

        End If



        If btn.CommandName = "Update" Then

            Dim ddlAppname As Label = sender.parent.findcontrol("ddlAppname")
            Dim ddlScorecard As DropDownList = sender.parent.findcontrol("ddlScorecard")
            Dim schCollege As TextBox = sender.parent.findcontrol("schCollege")
            Dim schSchool As TextBox = sender.parent.findcontrol("schSchool")
            Dim gvOther As GridView = sender.parent.findcontrol("gvOther")
            Dim gvAudio As GridView = sender.parent.findcontrol("gvAudio")
            Dim gvSchools As GridView = sender.parent.findcontrol("gvSchools")

            Dim sql As String = "", fields As String = ""

            Dim new_id As Integer = hdnXCCID.Value


            For Each gvr As GridViewRow In gvOther.Rows
                sql = "insert into otherFormData(xcc_id"
                fields = "select " & new_id

                If gvr.Cells(1).Text <> "" Then
                    sql &= ", data_key"
                    fields &= ", '" & gvr.Cells(1).Text & "'"
                End If
                If gvr.Cells(2).Text <> "" Then
                    sql &= ", data_value"
                    fields &= ", '" & gvr.Cells(2).Text & "'"
                End If
                If gvr.Cells(3).Text <> "" Then
                    sql &= ", data_type"
                    fields &= ", '" & gvr.Cells(3).Text & "'"
                End If

                UpdateTable(sql & ") " & fields)
                ' Response.Write(sql & ") " & fields)
            Next



            'save school data


            For Each gvr As GridViewRow In gvSchools.Rows
                sql = "insert into school_x_data(xcc_id"
                fields = "select " & new_id

                If gvr.Cells(0).Text <> "" Then
                    sql &= ", SCHOOL"
                    fields &= ", '" & gvr.Cells(0).Text & "'"
                End If
                If gvr.Cells(1).Text <> "" Then
                    sql &= ", COLLEGE"
                    fields &= ", '" & gvr.Cells(1).Text & "'"
                End If
                If gvr.Cells(2).Text <> "" Then
                    sql &= ", DEGREEOFINTEREST"
                    fields &= ", '" & gvr.Cells(2).Text & "'"
                End If

                If gvr.Cells(3).Text <> "" Then
                    sql &= ", MODALITY"
                    fields &= ", '" & gvr.Cells(3).Text & "'"
                End If

                If gvr.Cells(4).Text <> "" Then
                    sql &= ", AOI1"
                    fields &= ", '" & gvr.Cells(4).Text & "'"
                End If

                If gvr.Cells(5).Text <> "" Then
                    sql &= ", AOI2"
                    fields &= ", '" & gvr.Cells(5).Text & "'"
                End If

                If gvr.Cells(6).Text <> "" Then
                    sql &= ", L1_SUBJECTNAME"
                    fields &= ", '" & gvr.Cells(6).Text & "'"
                End If

                If gvr.Cells(7).Text <> "" Then
                    sql &= ", L2_SUBJECTNAME"
                    fields &= ", '" & gvr.Cells(7).Text & "'"
                End If

                If gvr.Cells(8).Text <> "" Then
                    sql &= ", Origin"
                    fields &= ", '" & gvr.Cells(8).Text & "'"
                End If



                UpdateTable(sql & ") " & fields)
                'Response.Write(sql & ") " & fields)
            Next
            ' Response.Write(sql)
            'Response.End()


        End If

        fvMeta.UpdateItem(False)

        Response.Redirect("edit_metadata.aspx?XID=" & hdnXCCID.Value)

    End Sub

    Protected Sub ddlAppname_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles ddlAppname.SelectedIndexChanged

        Dim ddlScorecard As DropDownList = sender.parent.findcontrol("ddlScorecard")
        Dim dsSC As SqlDataSource = sender.parent.findcontrol("dsSC")

        ddlScorecard.Items.Clear()
        ddlScorecard.Items.Add(New ListItem("(Select)", ""))
        dsSC.DataBind()
        ddlScorecard.Enabled = True
    End Sub




    Private Sub manual_call_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=edit_metadata.aspx?ID=" & Request("ID"))
        End If

        If Not IsPostBack Then
            hdnUserName.Value = User.Identity.Name '
            'If User.IsInRole("Admin") Then
            '    dsAppname.selectcommand = "Select appname from app_settings where "
            'End If
        End If
    End Sub


    Protected Function FormatDataTable(table_type As String) As DataTable
        Dim dt As DataTable = New DataTable()
        ' Create Columns

        Select Case table_type
            Case "audio"
                dt.Columns.Add("file_name", System.Type.GetType("System.String"))
                dt.Columns.Add("file_date", System.Type.GetType("System.String"))
                dt.Columns.Add("file_order", System.Type.GetType("System.String"))


                Return dt
            Case "other"
                dt.Columns.Add("data_key", System.Type.GetType("System.String"))
                dt.Columns.Add("data_value", System.Type.GetType("System.String"))
                dt.Columns.Add("data_type", System.Type.GetType("System.String"))


                Return dt
            Case "school"
                dt.Columns.Add("School", System.Type.GetType("System.String"))
                dt.Columns.Add("College", System.Type.GetType("System.String"))
                dt.Columns.Add("DegreeOfInterest", System.Type.GetType("System.String"))
                dt.Columns.Add("Modality", System.Type.GetType("System.String"))
                dt.Columns.Add("AOI1", System.Type.GetType("System.String"))
                dt.Columns.Add("AOI2", System.Type.GetType("System.String"))
                dt.Columns.Add("L1_SubjectName", System.Type.GetType("System.String"))
                dt.Columns.Add("L2_SubjectName", System.Type.GetType("System.String"))
                dt.Columns.Add("Origin", System.Type.GetType("System.String"))



                Return dt
        End Select

        dt.Columns.Add("id", System.Type.GetType("System.String"))
        dt.Columns.Add("text", System.Type.GetType("System.String"))
        Return dt
    End Function



    Protected Sub btnSubReset_Click(sender As Object, e As EventArgs)
        If Request("XID") <> "" Then
            UpdateTable("exec resetcall " & Request("XID"))
        End If

        If Request("ID") <> "" Then
            Dim f_dt As DataTable = Common.GetTable("select review_id from vwForm where f_id = " & Request("ID"))
            If f_dt.Rows.Count > 0 Then
                UpdateTable("exec resetcall " & f_dt.Rows(0).Item(0).ToString)
            End If

        End If


    End Sub

    Private Sub fvMeta_DataBound(sender As Object, e As EventArgs) Handles fvMeta.DataBound

    End Sub

    Private Sub fvMeta_ItemCreated(sender As Object, e As EventArgs) Handles fvMeta.ItemCreated

        If Not IsPostBack Then

            Dim gvAudio As GridView = sender.findcontrol("gvAudio")



            Dim fv As FormView = sender
            Dim drv As DataRowView = fv.DataItem


            hdnXCCID.Value = drv("ID").ToString

            Dim dt As DataTable = FormatDataTable("audio")
            Dim dr As DataRow

            'For Each gvr As GridViewRow In gvAudio.Rows
            dr = dt.NewRow

            Try
                dr("file_name") = Server.MapPath(drv.Item("audio_link").ToString).Replace("/", "\").Replace("d:\wwwroot", "\\64.111.27.113\d$\wwwroot")
            Catch ex As Exception
                dr("file_name") = drv.Item("audio_link").ToString
            End Try

            Try

                Dim audio_link As String = drv.Item("audio_link").ToString

                Dim s3Client As IAmazonS3
                s3Client = New AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings("CCAWSAccessKey"), System.Configuration.ConfigurationManager.AppSettings("CCCAWSSecretKey"), Amazon.RegionEndpoint.APSoutheast1)

                Dim request1 As GetPreSignedUrlRequest = New GetPreSignedUrlRequest()

                Dim URL_REQ As New GetPreSignedUrlRequest
                URL_REQ.BucketName = "callcriteriasingapore" & Left(audio_link, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/")
                URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1)
                URL_REQ.Expires = DateTime.Now.AddHours(10)

                Dim AWS_file As String = s3Client.GetPreSignedURL(URL_REQ)

                If AWS_file <> "" Then
                    dr("file_name") = AWS_file
                End If

            Catch ex As Exception

            End Try




            dr("file_date") = CDate(drv.Item("call_date")).ToShortDateString
            dr("file_order") = "5"
            ' Add the row
            dt.Rows.Add(dr)
            'Next

            dt.AcceptChanges()

            'audfile_name.Text = ""


            'Response.Write(dt.Rows.Count)
            'Response.End()

            Dim dt2 As DataTable = dt.Select("", "file_order").CopyToDataTable

            gvAudio.DataSource = dt2
            gvAudio.DataBind()

        End If
    End Sub
    Protected Sub gvAudio_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim gv As GridView = sender

        Dim dt As DataTable = FormatDataTable("audio")
        Dim dr As DataRow

        Dim row_count As Integer = 0
        For Each gvr As GridViewRow In gv.Rows
            If row_count <> e.RowIndex Then

                dr = dt.NewRow
                dr("file_name") = gvr.Cells(1).Text
                dr("file_date") = gvr.Cells(2).Text
                dr("file_order") = gvr.Cells(3).Text
                ' Add the row
                dt.Rows.Add(dr)
            End If
            row_count += 1
        Next


        dt.AcceptChanges()

        'audfile_name.Text = ""


        'Response.Write(dt.Rows.Count)
        'Response.End()

        Dim dt2 As DataTable = dt.Select("", "file_order").CopyToDataTable

        gv.DataSource = dt2
        gv.DataBind()


    End Sub
    Protected Sub btnMerge_Click(sender As Object, e As EventArgs)

        Dim gvAudio As GridView = sender.parent.findcontrol("gvAudio")


        Dim file_list As New List(Of String)

        For Each gvr As GridViewRow In gvAudio.Rows
            file_list.Add(gvr.Cells(1).Text)
        Next


        Dim ma As New CCInternalAPI.Maudio
        ma.audio_url = file_list
        ma.xcc_id = hdnXCCID.Value
        Dim cc_object As New CCInternalAPI
        cc_object.Manipulateaudio(ma)

        Dim dt As DataTable = FormatDataTable("audio")
        gvAudio.DataSource = dt
        gvAudio.DataBind()


    End Sub

End Class
