Imports System.Data
Imports Common
Partial Class add_tt
    Inherits System.Web.UI.Page

    Private Sub add_tt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim dt As DataTable = GetTable("select * from tt_user_list")

        For Each dr As DataRow In dt.Rows
            Try
                'Dim mu As MembershipUser = Membership.CreateUser(dr("usernames"), "welcome1")
                'Roles.AddUserToRole(dr("usernames"), "Agent")
                UpdateTable("insert into userapps(username, appname) select '" & dr("usernames") & "','tedtodd'")
                UpdateTable("insert into userExtraInfo (username, user_role, startdate)  select '" & dr("usernames") & "','Agent',dbo.getMTDate()")
            Catch ex As Exception
                Response.Write(dr("usernames") & " " & ex.Message & "<br>")
            End Try


        Next
    End Sub
End Class
