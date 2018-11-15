Imports Common
Partial Class direct_post
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
        If Request.QueryString.AllKeys.Count > 0 Then

            Dim sql_insert As String = "insert into xcc_report_new ("
            Dim sql_values As String = ") select "
            Dim qs_string() As String = Request.QueryString.AllKeys
            For Each qs_str As String In qs_string

                sql_insert &= "[" & qs_str & "],"
                sql_values &= "'" & Request.QueryString(qs_str).ToString.Replace("'", "''") & "',"

            Next

            UpdateTable(sql_insert & " date_added, appname" & sql_values & " dbo.getMTDate(),'" & domain(0) & "'")
            Response.Write("added")
        End If
    End Sub
End Class
