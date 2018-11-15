Imports System.Data
Imports Common

Partial Class lasoft_review
    Inherits System.Web.UI.Page
    Public current_version As String
    Private Sub lasoft_review_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("/login.aspx?ReturnURL=/training/")
        End If
        If Not IsPostBack Then
            Dim dt As DataTable = GetTable("select current_version, prod_version from system_settings")
            If Request.Url.AbsoluteUri.ToString.IndexOf("callcriteria-dev") > -1 Then
                current_version = dt.Rows(0).Item(0)
            Else
                current_version = dt.Rows(0).Item(1)
            End If
        End If

    End Sub
End Class
