Imports System.Data
Imports System.Data.SqlClient
Imports Common


Partial Class account_settings

    Inherits System.Web.UI.Page



    Private Sub account_settings_Load2(sender As Object, e As EventArgs)



        If Not IsPostBack Then


            Dim myColList As DataTable = GetTable("select count(*) from user_columns where username = '" & HttpContext.Current.User.Identity.Name & "'")

            If myColList.Rows(0).Item(0) = 0 Then
                UpdateTable("insert into user_columns (username, column_id) select '" & HttpContext.Current.User.Identity.Name & "', id from available_columns where column_default = 1")
            End If

            ' dsActivity.SelectParameters("username").DefaultValue = User.Identity.Name
        End If
    End Sub

    Private Sub account_settings_Load(sender As Object, e As EventArgs) Handles Me.Load


        If User.IsInRole("Agent") Then
            btnAddEmail.Visible = False
        End If


        hdnUsername.Value = User.Identity.Name
    End Sub

    Private Sub btnAddEmail_Click(sender As Object, e As EventArgs) Handles btnAddEmail.Click
        dsEmails.Insert()
        gvEmails.DataBind()
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function updateMyField(check_status As String, field_id As String, col_order As String) As String
        Dim resp As String = ""

        If check_status = "false" Then
            UpdateTable("delete from user_columns where Column_id = " & field_id & " and username = '" & HttpContext.Current.User.Identity.Name & "'")
        Else
            UpdateTable("insert into user_columns (column_id, username, col_order) select " & field_id & " ,'" & HttpContext.Current.User.Identity.Name & "'," & col_order)
        End If


        Return resp

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function resetCols() As String
        UpdateTable("delete from user_columns where username = '" & HttpContext.Current.User.Identity.Name & "'")

        Return "Updated"


    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function getMyFields() As String
        Dim resp As String = ""

        Dim answers_dt As DataTable = GetTable("exec getMyColumns '" & HttpContext.Current.User.Identity.Name & "'")

        For Each dr As DataRow In answers_dt.Rows
            resp &= "<tr><td><input type ='checkbox' class='myCol' onchange='updateColCheck($(this));' value='" & dr("column_id") & "' " & dr("isChecked") & " > " & dr("column_name").ToString.Replace("[", "").Replace("]", "") & "</td><td>" & dr("col_order") & "</td></tr>"
        Next


        Return resp

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateUserColumnPriority(colset As String) As String


        Dim col_list() As String = colset.Split(",")
        For Each col_item In col_list

            Dim col_pieces() As String = col_item.Split("=")

            If col_pieces.Length > 0 Then
                UpdateTable("update user_columns set col_order = " & col_pieces(1) & " where Column_id = " & col_pieces(0) & " and username = '" & HttpContext.Current.User.Identity.Name & "'")
            End If


        Next


        Return "Updated"

    End Function


End Class