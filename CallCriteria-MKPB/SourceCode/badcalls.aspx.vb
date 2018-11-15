Imports Common
Imports System.Data

Partial Class badcalls
    Inherits System.Web.UI.Page
    Public current_version As String
    Private Sub lasoft_review_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("/login.aspx?ReturnURL=badcalls.aspx")
        End If
        current_version = ""
        If Not IsPostBack Then


            Dim dt As DataTable = GetTable("select current_version from system_settings")

            Dim Uname As String
            Uname = 0
            Try
                Uname = UserImpersonation.PrevUserName
            Catch ex As Exception

            End Try

            If Uname.Length = 0 Then
                Uname = User.Identity.Name.ToString
            End If
            Dim dt1 As DataTable = GetTable("select name, Value from userSettings join userextrainfo on userSettings.userId=userextrainfo.id where userName='" & Uname _
                                           & "' and userSettings.Name='bundleversion'")
            Try
                current_version = dt1.Rows(0).Item(1)
            Catch ex As Exception

            End Try


            If current_version.Length = 0 Then
                Dim dt2 As DataTable = GetTable("select current_version, prod_version from system_settings")
                If Request.Url.AbsoluteUri.ToString.IndexOf("callcriteria-dev") > -1 Then
                    current_version = dt2.Rows(0).Item(0)
                Else
                    current_version = dt2.Rows(0).Item(1)
                End If
            End If
        End If
    End Sub
End Class
