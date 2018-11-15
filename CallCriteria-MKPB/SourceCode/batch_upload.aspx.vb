Option Compare Text
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient

Imports NPOI.HSSF.UserModel
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.IO

Partial Class batch_upload
    Inherits System.Web.UI.Page
    Dim dt As DataTable


    Protected Sub Wizard1_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        If Wizard1.ActiveStepIndex = 1 Then

            'Try
            If IO.File.Exists(Server.MapPath("docs/" & fupExcelFile.FileName)) Then
                IO.File.Delete(Server.MapPath("docs/" & fupExcelFile.FileName))
            End If
            fupExcelFile.SaveAs(Server.MapPath("docs/" & fupExcelFile.FileName))

            'Catch ex As Exception
            'Response.Write(ex.Message)
            'Response.End()
            'End Try
            hdnUploadedFile.Value = fupExcelFile.FileName
            If Right(hdnUploadedFile.Value, 4) = ".xls" Or Right(hdnUploadedFile.Value, 4) = "xlsx" Then
                Dim mySheets() As String = GetExcelSheetNames(Server.MapPath("docs/" & fupExcelFile.FileName))
                ddlExcelSheets.Items.Clear()
                If mySheets.Length > 0 Then
                    For Each sheet_name As String In mySheets
                        If sheet_name <> "" Then
                            ddlExcelSheets.Items.Add(sheet_name)
                        End If
                    Next
                Else
                    ddlExcelSheets.Items.Add(mySheets(0))
                End If
            Else
                VerifyQuery()
                Wizard1.ActiveStepIndex = 3
            End If

        End If
        If Wizard1.ActiveStepIndex = 2 Then

            GetExcelSheetColums(hdnUploadedFile.Value, ddlExcelSheets.SelectedValue)
        End If


        If Wizard1.ActiveStepIndex = 2 Then
            VerifyQuery()
        End If

    End Sub





    Protected Function GetExcelSheetNames(ByVal excelFile As String) As String()

        'Dim objConn As OleDbConnection
        'Dim dt As System.Data.DataTable


        Dim hssfwb As XSSFWorkbook
        Using file As FileStream = New FileStream(excelFile, FileMode.Open, FileAccess.Read)
            hssfwb = New XSSFWorkbook(file)
        End Using

        Dim i As Integer = 0

        Dim excelSheets(hssfwb.NumberOfSheets) As String
        For x As Integer = 0 To hssfwb.NumberOfSheets - 1

            excelSheets(x) = hssfwb.GetSheetName(x)

        Next

        Return excelSheets




        'Dim MyConnection As New System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & DataFile.Directory.FullName & "';Extended Properties='text;HDR=NO';Jet OLEDB:Engine Type=96;")
        'Dim oledataAdapter As OleDbDataAdapter
        'oledataAdapter = New OleDbDataAdapter("SELECT * FROM [" & DataFile.Name & "]", MyConnection)


        'Try
        ' Connection String. Change the excel file to the file you
        ' will search.
        'Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelFile & ";Extended Properties=Excel 8.0;"
        'connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & excelFile & "';Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';Jet OLEDB:Engine Type=96;"

        'Response.Write(connString)
        'Response.End()

        '' Create connection object by using the preceding connection string.
        'objConn = New OleDbConnection(connString)
        '' Open connection with the database.
        'objConn.Open()
        '' Get the data table containg the schema guid.
        'dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)

        'If dt Is Nothing Then
        '    Return Nothing
        'End If

        'Dim excelSheets(dt.Rows.Count) As String '= New String(dt.Rows.Count)
        'Dim i As Integer = 0

        '' Add the sheet name to the string array.
        'For Each row As DataRow In dt.Rows
        '    excelSheets(i) = row("TABLE_NAME").ToString()
        '    i += 1
        'Next

        ''Loop through all of the sheets if you want too...
        'For j As Integer = 0 To excelSheets.Length
        '    ' Query each excel sheet.
        'Next
        'Return excelSheets
        'Catch ex As Exception
        '    Response.Write(ex.Message)
        '    Response.End()
        '    Return Nothing

        'Finally

        '    ' Clean up.

        'End Try
        'If objConn IsNot Nothing Then

        '    objConn.Close()
        '    objConn.Dispose()
        'End If
        'If dt IsNot Nothing Then
        '    dt.Dispose()
        'End If
    End Function


    Public Sub GetExcelSheetColums(ByVal filename As String, ByVal sheet As String)


        'Dim myConn As New OleDbConnection
        'Dim XlsConn As String =
        '    "Provider=Microsoft.ACE.OLEDB.12.0;Data " &
        '    "Source=" & Server.MapPath("docs/" & filename) & ";" &
        '    "Extended Properties=Excel 8.0"
        ''
        '' Open an ADO connection to the Excel file.
        ''






        'myConn.ConnectionString = XlsConn
        'myConn.Open()



        'Dim commandText As String = "Select * From [" + sheet + "]"
        'Dim selecter As OleDbCommand = New OleDbCommand(commandText, myConn)
        'Dim reader As OleDbDataReader = selecter.ExecuteReader()

        ''Get the name and type of the first column
        'Dim sheetSchema As DataTable = reader.GetSchemaTable()
        ''Dim firstColumnName As String = sheetSchema.Rows(1).Item("ColumnName")


    End Sub

    Protected Sub gvColumns_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvColumns.DataBinding
        Dim myConn As New SqlClient.SqlConnection
        Dim XlsConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString
        '"Provider=sqloledb;Data Source=vsql032,2433;Initial Catalog=BMCS_OneTool_DEV;User ID=BMCS_OneTool_WebD;Password=0n3T00l"

        ' Open an ADO connection to the Excel file.
        '
        myConn.ConnectionString = XlsConn
        myConn.Open()


        Dim dt2 As New DataTable
        Dim cmd As String = "select top 1 * from dbo.xcc_report_new"
        Dim cmd_reply As New SqlDataAdapter(cmd, myConn)
        cmd_reply.Fill(dt2)
        'myConn.Close()

        'myConn.Open()


        'Dim map_dt As New DataTable
        'Dim cmd2 As String = "select * from dbo.APP_ONETOOL_UPLOAD_MAPPING where map_cust_name= '" & ddlAppname.SelectedValue & "'"
        'Dim cmd_reply2 As New SqlDataAdapter(cmd2, myConn)
        'cmd_reply.Fill(map_dt)
        'Session("map_dt") = map_dt
        'myConn.Close()



        'myConn.Open()
        Dim commandText As String = "Select top 1 * From xcc_report_new"
        Dim selecter As SqlCommand = New SqlCommand(commandText, myConn)
        Dim reader As SqlDataReader = selecter.ExecuteReader(CommandBehavior.KeyInfo)

        'Get the name and type of the first column
        Dim sheetSchema As DataTable = reader.GetSchemaTable()
        Dim dt As New DataTable
        dt.Columns.Add("ColumnName")
        dt.Columns.Add("ColumnDesc")
        Dim dr3 As DataRow = dt.Rows.Add()
        dr3("ColumnName") = "*****(ignore)*****"
        dr3("ColumnDesc") = "*****(ignore)*****"
        Dim firstColumnName As String = sheetSchema.Rows(1).Item("ColumnName")

        For Each myField As DataRow In sheetSchema.Rows
            'For each property of the field...

            Dim col_name As String
            Dim col_size As String
            Dim col_type As String

            For Each myProperty As DataColumn In sheetSchema.Columns
                'Display the field name and value.

                If myProperty.ColumnName = "ColumnName" Then
                    col_name = myField(myProperty).ToString()
                End If

                If myProperty.ColumnName = "ColumnSize" Then
                    col_size = myField(myProperty).ToString()
                End If

                If myProperty.ColumnName = "DataType" Then
                    col_type = myField(myProperty).ToString()
                End If


                'Console.WriteLine(myProperty.ColumnName + " = " + myField(myProperty).ToString())
            Next
            'Console.WriteLine()
            Dim dc As DataRow
            dc = dt.Rows.Add
            dc("ColumnName") = col_name
            'dr2("ColumnDesc") = dr("ColumnName").ToString & "(" & dr("DataType").ToString & "-" & dr("ColumnSize").ToString & ")"
            dc("ColumnDesc") = col_name & " (" & col_type & "-" & col_size & ")"
            'Pause.
            'Console.ReadLine()
        Next




        'For Each dr As DataRow In sheetSchema.Rows


        '    For Each dr2 As DataColumn In sheetSchema.Columns
        '        Dim dc As DataRow
        '        dc = dt.Rows.Add
        '        dc("ColumnName") = dr2.ColumnName
        '        'dr2("ColumnDesc") = dr("ColumnName").ToString & "(" & dr("DataType").ToString & "-" & dr("ColumnSize").ToString & ")"
        '        dc("ColumnDesc") = dr2.ColumnName.ToString & " (" & dr2.DataType.ToString & ")"
        '    Next



        '    'If dt2.Columns.Contains(dr("ColumnName").ToString) Then
        '    '        Dim dr2 As DataRow
        '    '        dr2 = dt.Rows.Add
        '    '        dr2("ColumnName") = dr("ColumnName")
        '    '        'dr2("ColumnDesc") = dr("ColumnName").ToString & "(" & dr("DataType").ToString & "-" & dr("ColumnSize").ToString & ")"
        '    '        dr2("ColumnDesc") = dt2.Rows(0).Item(dr("ColumnName").ToString) & "(" & dr("DataType").ToString & "-" & dr("ColumnSize").ToString & ")"
        '    '    End If
        'Next
        dt.DefaultView.Sort = "ColumnName"
        Session("dt") = dt
    End Sub


    Protected Sub gvColumns_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvColumns.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddl As DropDownList = e.Row.Cells(3).FindControl("ddlDestinationColumn")

            ddl.DataTextField = "ColumnDesc"
            ddl.DataValueField = "ColumnName"
            ddl.DataSource = Session("dt")
            ddl.DataBind()
            If ddl.Items.Contains(New ListItem(e.Row.Cells(0).Text)) Or ddl.Items.Contains(New ListItem(e.Row.Cells(0).Text.Replace(" ", "_"))) Then
                ddl.SelectedValue = e.Row.Cells(0).Text
            End If

            'Dim map_dt As DataTable = Session("map_dt")
            'For Each dr As DataRow In map_dt.Rows
            '    If e.Row.Cells(0).Text = dr("map_source_field") Then
            '        If ddl.Items.Contains(New ListItem(dr("map_destination_field"))) Then
            '            ddl.SelectedValue = dr("map_destination_field")
            '        End If
            '    End If

            'Next


        End If

    End Sub


    Protected Sub GetExcelSheetNames2()


        Dim excelfile As String = hdnUploadedFile.Value
        Dim dt As New DataTable

        Dim filetype As String = excelfile.Substring(excelfile.LastIndexOf(".") + 1).ToLower

        'Response.Write(Server.MapPath("docs") & " - select * from [" & ddlExcelSheets.SelectedValue.Replace("'", "") & "]")
        'Response.End()


        Select Case filetype
            Case "xls"
                Dim cn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/" & excelfile) & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"";")
                cn.Open()
                Dim reply As New OleDb.OleDbDataAdapter("select * from [" & ddlExcelSheets.SelectedValue & "]", cn)
                reply.Fill(dt)
                cn.Close()
                cn.Dispose()
            Case "xslx"
                'Dim cn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/" & excelfile) & ";Extended Properties=""Excel 12.0 Xml;HDR=YES"";")
                Dim cn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/" & excelfile) & ";Extended Properties=""Excel 8.0;HDR=YES"";")
                cn.Open()
                Dim reply As New OleDb.OleDbDataAdapter("select * from [" & ddlExcelSheets.SelectedValue & "]", cn)
                reply.Fill(dt)
                cn.Close()
                cn.Dispose()
            Case "csv", "txt"

                'Response.Write("select * from [" & excelFile & "]")
                'Response.End()

                Dim cn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/") & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";")
                cn.Open()
                Dim reply As New OleDb.OleDbDataAdapter("select * from [" & excelfile & "]", cn)
                reply.Fill(dt)
                cn.Close()
                cn.Dispose()

        End Select


        Dim dt_col As New DataTable
        dt_col.Columns.Add("ColumnName")
        dt_col.Columns.Add("DataType")



        For Each dc As DataColumn In dt.Columns
            Dim dr As DataRow = dt_col.NewRow
            dr.Item("ColumnName") = dc.ColumnName
            dr.Item("DataType") = dc.DataType
            dt_col.Rows.Add(dr)
        Next

        gvColumns.DataSource = dt_col 'dt_speeds
        gvColumns.DataBind()


    End Sub


    Protected Sub GetExcelSheetNames3()



        Dim excelfile As String = hdnUploadedFile.Value

        Dim filetype As String = excelfile.Substring(excelfile.LastIndexOf(".") + 1).ToLower


        'Response.Write(Server.MapPath("docs") & " - select * from [" & ddlExcelSheets.SelectedValue.Replace("'", "") & "]")
        'Response.End()
        Dim cn As OleDb.OleDbConnection
        Dim reply As New OleDb.OleDbCommand

        Dim myConn As New OleDbConnection

        Dim XlsConn As String = ""
        Dim commandText As String = ""

        Select Case filetype
            Case "xls"
                XlsConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Chr(34) & Server.MapPath("docs/" & hdnUploadedFile.Value) & Chr(34) & ";Extended Properties=Excel 8.0"
                commandText = "select * from [" & ddlExcelSheets.SelectedValue & "]"

            Case "xlsx"
                XlsConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Chr(34) & Server.MapPath("docs/" & hdnUploadedFile.Value) & Chr(34) & ";Extended Properties=Excel 8.0"
                'cn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/" & excelfile) & ";Extended Properties=""Excel 12.0;HDR=YES"";")
                commandText = "select * from [" & ddlExcelSheets.SelectedValue & "]"

            Case "csv", "txt"

                'Response.Write("select * from [" & excelFile & "]")
                'Response.End()

                XlsConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath("docs/") & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
                commandText = "select * from [" & excelfile & "]"

        End Select




        myConn.ConnectionString = XlsConn
        myConn.Open()


        Dim selecter As OleDbCommand = New OleDbCommand(commandText, myConn)
        Dim reader As OleDbDataReader = selecter.ExecuteReader()


        Dim records As OleDbCommand = New OleDbCommand(commandText.Replace("*", "count(*)"), myConn)

        lblNumRows.Text = records.ExecuteScalar()

        'Get the name and type of the first column
        Dim sheetSchema As DataTable = reader.GetSchemaTable()

        gvColumns.DataSource = sheetSchema 'dt_speeds
        gvColumns.DataBind()
        myConn.Close()

    End Sub





    Protected Sub VerifyQuery()

        GetExcelSheetNames3()

        Exit Sub

        Dim final_map As String = "<br>"

        Dim i As Integer
        Dim myConn As New OleDbConnection
        Dim XlsConn As String =
            "Provider=Microsoft.ACE.OLEDB.12.0;Data " &
            "Source=" & Server.MapPath("docs/" & hdnUploadedFile.Value) & ";" &
            "Extended Properties=Excel 8.0"
        '
        ' Open an ADO connection to the Excel file.
        '
        myConn.ConnectionString = XlsConn
        myConn.Open()



        Dim commandText As String = "Select * From [" + ddlExcelSheets.SelectedValue + "]"
        Dim selecter As OleDbCommand = New OleDbCommand(commandText, myConn)
        Dim reader As OleDbDataReader = selecter.ExecuteReader()

        'Get the name and type of the first column
        Dim sheetSchema As DataTable = reader.GetSchemaTable()

        Dim records As OleDbCommand = New OleDbCommand("Select count(*) From [" + ddlExcelSheets.SelectedValue + "]", myConn)

        lblNumRows.Text = records.ExecuteScalar()

        'Dim dt_speeds As New DataTable
        'dt_speeds.Columns.Add("Column Name")

        'For Each dr As DataRow In sheetSchema.Rows
        '    Dim dr_speeds As DataRow = dt_speeds.Rows.Add
        '    dr_speeds("Column Name") = dr("ColumnName")
        'Next

        gvColumns.DataSource = sheetSchema 'dt_speeds
        gvColumns.DataBind()


    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        lblFinalQuery.Text = ""

        Dim myConn As New OleDbConnection
        Dim XlsConn As String =
            "Provider=Microsoft.ACE.OLEDB.12.0;Data " &
            "Source=" & Server.MapPath("docs/" & hdnUploadedFile.Value) & ";" &
            "Extended Properties=Excel 8.0"
        '
        ' Open an ADO connection to the Excel file.
        '
        myConn.ConnectionString = XlsConn
        myConn.Open()

        Dim myConn2 As New SqlConnection
        Dim XlsConn2 As String = System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString  ' "Provider=sqloledb;Data Source=mmeashcssdb06;Initial Catalog=CSS_BMAT;User ID=svc-web-bmat;Password=Bm@tW3b"

        ' Open an ADO connection to the Excel file.
        '
        myConn2.ConnectionString = XlsConn2
        myConn2.Open()


        Dim agent_id As String = ""

        Dim found_session As Boolean = False

        Dim sql As String = "select "
        For Each dr As GridViewRow In gvColumns.Rows
            Dim ddl As DropDownList = dr.Cells(3).Controls(1)
            If ddl.SelectedValue <> "*****(ignore)*****" Then
                sql &= "[" & dr.Cells(0).Text & "], "

                If ddl.SelectedValue = "SESSION_ID" Then
                    found_session = True
                End If

            End If
            If ddl.SelectedValue = "" Then
                agent_id = dr.Cells(0).Text
            End If

        Next
        'response.write(agent_id)
        'response.end()

        sql = Left(sql, Len(sql) - 2) & " from [" & ddlExcelSheets.SelectedValue & "]"




        Dim da As OleDbDataAdapter = New OleDbDataAdapter(sql, myConn)
        Dim excel_dt As New DataTable
        da.Fill(excel_dt)

        'Response.Write(sql)
        'Response.End()

        sql = "insert into xcc_report_new ("
        For Each dr As GridViewRow In gvColumns.Rows
            Dim ddl As DropDownList = dr.Cells(3).Controls(1)
            If ddl.SelectedValue <> "*****(ignore)*****" Then
                sql &= "[" & ddl.SelectedValue & "], "
            End If
        Next

        sql &= "[appname],[scorecard], [call_date] ) values ("



        If Not found_session Then
            lblFinalQuery.Text = "No session defined -- SESSION_ID is required to be mapped and unique.<br>"
            Exit Sub
        End If


        Dim loaded As Integer = 0
        Dim errored As Integer = 0

        Dim already_added As String = ""
        For Each dr As DataRow In excel_dt.Rows
            Dim insertitems As String = ""
            For x As Integer = 0 To excel_dt.Columns.Count - 1
                insertitems &= "'" & dr.Item(x).ToString.Replace("'", "`") & "',"
            Next


            insertitems &= "'" & ddlAppname.SelectedValue & "',"
            insertitems &= "'" & ddlScorecard.SelectedValue & "',"
            insertitems &= "'" & calCallDate.SelectedDate & "')"

            'insertitems = Left(insertitems, Len(insertitems) - 1) & ")"

            'Dim ok_to_add As Boolean = True
            'If ddlChangeType.SelectedValue = "Add" Then
            '    'Check to see if the Agent Identifier already exists
            '    If AgentExists(dr("Agent Identifier")) Then
            '        ok_to_add = False
            '        already_added &= dr("Agent Identifier") & "<br>"
            '    End If
            'End If

            'If ok_to_add Then
            Dim reply2 As New SqlCommand '("update ALLSTATE_AgentMgr set agent_changed = 0 where [Agent_Identifier] = '" & dr("Agent Identifier") & "'", myConn2)
            'response.write(reply2.Commandtext & "<br>")

            'reply2.ExecuteNonQuery()
            reply2 = New SqlCommand(sql & insertitems, myConn2)
            'Response.Write(reply2.CommandText & "<br>")
            Try
                reply2.ExecuteNonQuery()
                loaded = loaded + 1

            Catch ex As Exception
                errored = errored + 1
                lblAlreadyAdded.Text = lblAlreadyAdded.Text & ex.Message & "<br>"

            End Try

            'End If
        Next
        lblAlreadyAdded.Text = lblAlreadyAdded.Text & loaded & " loaded/ " & errored & " errored."
        myConn.Close()
        myConn2.Close()

    End Sub



    Protected Sub btnSaveMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveMap.Click
        Dim myConn2 As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        myConn2.Open()

        'Step 1 delete all existing mapped data
        Dim reply2 As New SqlCommand("delete from APP_ONETOOL_UPLOAD_MAPPING where map_cust_name = '" & ddlAppname.SelectedValue & "'", myConn2)
        reply2.ExecuteNonQuery()

        ' Write current mapped data to DB
        For Each dr As GridViewRow In gvColumns.Rows
            Dim ddl As DropDownList = dr.Cells(3).Controls(1)
            If ddl.SelectedValue <> "*****(ignore)*****" Then
                reply2 = New SqlCommand("insert into APP_ONETOOL_UPLOAD_MAPPING (map_cust_name, map_source_field, map_destination_field) values('" & ddlAppname.SelectedValue & "','" & dr.Cells(0).Text & "','" & ddl.SelectedValue & "')", myConn2)
                reply2.ExecuteNonQuery()
            End If
        Next
        Page.ClientScript.RegisterClientScriptBlock(GetType(Page), "onload", "Alert('Updated.')")
    End Sub

    Private Sub ddlAppname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppname.SelectedIndexChanged
        dsScorecards.DataBind()
    End Sub
End Class

