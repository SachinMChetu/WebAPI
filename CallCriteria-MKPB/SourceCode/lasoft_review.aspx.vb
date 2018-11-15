Imports Common
Imports System.Text.RegularExpressions
Imports System.Web.Hosting
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics


Partial Class lasoft_review
    Inherits System.Web.UI.Page
    
    Public current_version As String
    Public uSettings as String

   
    Private Sub lasoft_review_Load(sender As Object, e As EventArgs) Handles Me.Load
         current_version = ""
        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("/login.aspx?ReturnURL=/home/" & Request("v"))
        End If
        If Not IsPostBack Then

            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            If domain(1).ToLower = "callcriteria" Or domain(1).ToLower = "callcriteria-dev" Then
                If Me.Page.Title = "" Then
                    Me.Page.Title = "Call Criteria"
                End If

            Else
                Me.Page.Title = "QA Tool"
            End If



           
            Dim Uname As String
            Dim PUname As String
            Uname = ""
            PUname = ""
            Try
                PUname = UserImpersonation.PrevUserName
            Catch ex As Exception

            End Try


            If Uname.Length = 0 Then
                Uname = User.Identity.Name.ToString
            End If

            Dim command As New SqlCommand
            command.CommandText = "GetPageInitialData"
            command.CommandType =CommandType.StoredProcedure
            command.Parameters.AddWithValue("@UserName",Uname)
                
          
            Dim ds As DataSet = GetTables(command)
            Dim dt1 As DataTable   =  ds.Tables(0)
            Dim dtsettings As DataTable   =  ds.Tables(1)
              
              
            Dim pattern As String = """PreviousUser"":""(^\s*$|)"""
            Dim rgx As New Regex(pattern)

            If Request.Url.AbsoluteUri.ToString.IndexOf("callcriteria-dev") > -1 Then
                current_version = dt1.Rows(0).Item(0)
            Else
                current_version = dt1.Rows(0).Item(1)
            End If


            For i As Integer = 0 To dtsettings.Rows.Count - 1                  
                if dtsettings.Rows(i).Item(0) = "allUserSettings" Then             
                    Try
                    if PUname = ""
                        uSettings = dtsettings.Rows(i).Item(1).ToString
                    Else
                        uSettings = rgx.Replace(dtsettings.Rows(i).Item(1).ToString,"""PreviousUser"":"""+PUname+"""")
                    End If
                    Catch ex As Exception 
                        'uSettings= ex.Message
                    End  Try
                End If   

                if dtsettings.Rows(i).Item(0)="bundleversion" Then             
                    Try
                        current_version = dtsettings.Rows(i).Item(1).ToString
                    Catch ex As Exception 
                       ' uSettings= ex.Message
                    End  Try
                End If   
            
            Next
          
                
            
            End If
    End Sub
End Class
