Imports System.Data
Imports Common
Partial Class assemble_audio
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub


    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        'Get list of file names
        Dim output As String = ""
        Dim out_error As String = ""


        If Not IO.Directory.Exists("d:\wwwroot\audio\" & ddlApps.SelectedValue & "\" & CDate(TextBox1.Text).ToString("MM_dd_yyyy") & "\") Then
            IO.Directory.CreateDirectory("d:\wwwroot\audio\" & ddlApps.SelectedValue & "\" & CDate(TextBox1.Text).ToString("MM_dd_yyyy") & "\")
        End If


        'get file list and match to open audio files

        Dim sql As String = "truncate table dir_list; insert into dir_list(subdir, depth, isFile) EXEC xp_dirtree '" & Server.MapPath("/audio/" & ddlApps.SelectedValue & "/to_process/") & "', 10, 1"
        UpdateTable(sql)






        Dim fileList() As String = IO.Directory.GetFiles(Server.MapPath("/audio/" & ddlApps.SelectedValue & "/to_process/"))

        Dim file_dt As DataTable = GetTable("select * from dir_list join xcc_report_new on subdir like '%' + phone + '%' or  subdir like '%' + replace(email,'.com','') + '%'  where appname = '" & ddlApps.SelectedValue & "' and max_reviews = 0 order by phone")

        Dim file_list As New DataTable

        file_list.Columns.Add("filename")
        file_list.Columns.Add("phone")
        file_list.Columns.Add("type")


        'For Each filename In fileList
        For Each fdr As DataRow In file_dt.Rows

            Dim filename As String = Server.MapPath("/audio/" & ddlApps.SelectedValue & "/to_process/") & fdr.Item("subdir")

            IO.File.Move(filename, filename.Replace(" ", "_").Replace(")", "_").Replace("(", "_"))

            filename = filename.Replace(" ", "_").Replace(")", "_").Replace("(", "_")
            Dim thisFile As String = Mid(filename, filename.LastIndexOf("\") + 2, 1000)
            Response.Write(thisFile & " " & Len(thisFile) & "<br>")
            Response.Flush()
            'If Len(thisFile) > 14 Then 'more than phone + ending

            If Right(thisFile, 4) <> ".mp3" Then
                RunFFMPEG("-i " & Chr(34) & "d:\wwwroot\audio\" & ddlApps.SelectedValue & "\to_process\" & thisFile & Chr(34) & " -b:a 13k -y " & Chr(34) & "d:\wwwroot\audio\" & ddlApps.SelectedValue & "\to_process\" & Left(thisFile, Len(thisFile) - 4) & ".mp3" & Chr(34), output, out_error)
                Response.Write("-i " & Chr(34) & thisFile & Chr(34) & " -b:a 13k -y " & Left(thisFile, Len(thisFile) - 4) & ".mp3")
            End If

            Dim dr As DataRow = file_list.NewRow
            dr("filename") = thisFile.Replace(" ", "_").Replace(")", "_").Replace("(", "_").Replace(Right(thisFile, 3), "mp3")

            'Select Case ddlApps.SelectedValue
            '    Case "edufficient"
            '        dr("phone") = Left(thisFile, 10)
            '    Case "DMS"
            '        dr("phone") = Mid(thisFile, 10, 10)
            '    Case Else


            'End Select

            dr("phone") = fdr.Item("phone")

            dr("type") = "mp3"

            file_list.Rows.Add(dr)

            UpdateTable("update xcc_report_new set audio_link = '/audio/" & ddlApps.SelectedValue & "/" & CDate(TextBox1.Text).ToString("MM_dd_yyyy") & "/" & fdr("phone").ToString & ".mp3' where id = " & fdr("ID").ToString & " and audio_link is null")

            'End If
        Next


        Dim distinctDT As DataTable = file_list.DefaultView.ToTable(True, "phone")
        For Each dist_dr As DataRow In distinctDT.Rows
            Dim result() As DataRow = file_list.Select("phone = '" & dist_dr.Item("phone") & "'")
            Dim concat_list As String = ""
            Dim concat_ending As String = ""
            For Each dr_item As DataRow In result
                concat_list &= "d:\wwwroot\audio\" & ddlApps.SelectedValue & "\to_process\" & dr_item.Item("filename") & "|"
                concat_ending = dr_item.Item("type")
            Next

            concat_list = Left(concat_list, Len(concat_list) - 1)

            RunFFMPEG("-i " & Chr(34) & "concat:" & concat_list & Chr(34) & " -c copy " & "d:\wwwroot\audio\" & ddlApps.SelectedValue & "\" & CDate(TextBox1.Text).ToString("MM_dd_yyyy") & "\" & dist_dr.Item("phone") & "." & concat_ending & " -y", output, out_error)
            Response.Write("-i " & Chr(34) & "concat:" & concat_list & Chr(34) & " -c copy " & "d:\wwwroot\audio\" & ddlApps.SelectedValue & "\" & CDate(TextBox1.Text).ToString("MM_dd_yyyy") & "\" & dist_dr.Item("phone") & "." & concat_ending & " -y" & "<br>")
            Response.Flush()

        Next



    End Sub
End Class
