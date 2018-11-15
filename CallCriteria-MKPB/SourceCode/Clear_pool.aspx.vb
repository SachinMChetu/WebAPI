
Imports System.Data.SqlClient

Partial Class Clear_pool
    Inherits System.Web.UI.Page

    Private Sub Clear_pool_Load(sender As Object, e As EventArgs) Handles Me.Load
        'SqlConnection.ClearAllPools()

        Response.Write("Cleared")
        Response.End()
    End Sub
End Class
