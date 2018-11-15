Imports System.Data
Imports System.Data.SqlClient
Imports Common

Partial Class lasoft_review
    Inherits System.Web.UI.Page
    Public current_version As String
    Private Sub lasoft_review_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("/login.aspx?ReturnURL=/listen/")
        End If
        If Not IsPostBack Then

            ''Check w/ Calbration is needed for hiproorit first, redirect if needed

            'Dim sq As New SqlCommand("getNextWorkItem")
            'sq.CommandType = CommandType.StoredProcedure
            'sq.Parameters.AddWithValue("username", User.Identity.Name)

            'Dim calib_check_dt As DataTable = GetTable(sq)
            'If calib_check_dt.Rows.Count > 0 Then
            '    If calib_check_dt.Rows(0).Item("next_page").ToString = "calibrate" Then
            '        Response.Redirect("/calibrate")
            '    End If

            'End If



            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            If domain(1).ToLower = "callcriteria" Or domain(1).ToLower = "callcriteria-dev" Then
                If Me.Page.Title = "" Then
                    Me.Page.Title = "Call Criteria"
                End If

            Else
                Me.Page.Title = "QA Tool"
            End If

            Dim dt As DataTable = GetTable("select current_version, prod_version from system_settings")
            If Request.Url.AbsoluteUri.ToString.IndexOf("callcriteria-dev") > -1 Then

                current_version = dt.Rows(0).Item(0)
            Else

                current_version = dt.Rows(0).Item(1)
            End If
        End If

    End Sub
End Class
