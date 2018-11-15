Imports Common
Imports System.Data
Partial Class create_esto_agents
    Inherits System.Web.UI.Page

    Private Sub create_esto_agents_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ulist_dt As DataTable = GetTable("select username ,password  from school.school.dbo.users with (nolock) join (select distinct agent from xcc_report_new with (nolock) where agent not in (select username from userextrainfo) and appname in ('estobk','esto') and call_date > DATEADD(d, -15, getdate())) a on a.agent = username")

        For Each dr As DataRow In ulist_dt.Rows
            Try
                Dim mu As MembershipUser = Membership.CreateUser(dr("username"), dr("password"))
                Roles.AddUserToRole(dr("username"), "Agent")
                UpdateTable("insert into userapps(username, appname) select '" & dr("username") & "','estobk'")
                UpdateTable("insert into userapps(username, appname) select '" & dr("username") & "','esto'")
                UpdateTable("insert into userExtraInfo (username, user_role, startdate)  select '" & dr("username") & "','Agent',dbo.getMTDate()")
            Catch ex As Exception
                Response.Write(dr("username") & " " & ex.Message & "<br>")
            End Try


        Next

    End Sub
End Class
