Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class forgot_password
    Inherits System.Web.UI.Page

    Protected Sub btnGetPassword_Click(sender As Object, e As EventArgs) Handles btnGetPassword.Click
        'Check to see if user name exists
        Dim mu As MembershipUser = Membership.GetUser(UserName.Text)
        If mu IsNot Nothing Then
            Dim new_pwd As String = mu.ResetPassword()

            Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
            cn.Open()


            Dim reply2 As New SqlCommand("EXEC send_dbmail  @profile_name='General',  @recipients='" & mu.Email & "',  @subject=@Subject_text,  @body=@Body , @body_format = 'HTML' ;", cn)
            reply2.Parameters.AddWithValue("Subject_text", "Password Reset")

            reply2.Parameters.AddWithValue("Body", "Your password has ben reset to: <b>" & new_pwd & "</b> Please login with it.")

            reply2.CommandTimeout = 60
            reply2.ExecuteNonQuery()

            cn.Close()
            cn.Dispose()

            Response.Redirect("login.aspx")

        End If
    End Sub
End Class
