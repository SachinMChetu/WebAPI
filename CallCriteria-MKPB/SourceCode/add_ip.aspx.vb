Imports System.Data
Imports System.Data.SqlClient

Partial Class add_ip
    Inherits System.Web.UI.Page

    Private Sub add_ip_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim cn As New SqlConnection("Server=tcp:callcriteriadb.database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=cc_admin;Password=SJQc9M89em5Gmf55;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;")
        cn.Open()

        Dim my_ip As String = Request.UserHostAddress

        Dim sql As String = "EXECUTE sp_set_firewall_rule @name = N'add_" & my_ip.Replace(".", "_") & "',  @start_ip_address = '" & my_ip & "', @end_ip_address = '" & my_ip & "'"


        Try
            Dim reply As New SqlCommand(Sql, cn)
            reply.CommandTimeout = 60
            reply.ExecuteNonQuery()
        Catch ex As Exception

        End Try


        'Dim reply2 As New SqlCommand("insert into sql_sent (sql, start_time, end_time) select @sql, @start, @end", cn)
        'reply2.Parameters.AddWithValue("sql", sql)
        'reply2.Parameters.AddWithValue("start", sql_start)
        'reply2.Parameters.AddWithValue("end", Now.ToString)
        'reply2.CommandTimeout = 60
        'reply2.ExecuteNonQuery()
        'If debug Then

        'End If

        cn.Close()
        cn.Dispose()
    End Sub
End Class
