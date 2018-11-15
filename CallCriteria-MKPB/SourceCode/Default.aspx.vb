Imports AjaxControlToolkit
Imports System.Data.SqlClient
Imports System.Data
Imports Common
Imports System.IO
Imports System.Net

Partial Class _Default
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
            Response.Redirect("login.aspx?ReturnURL=default.aspx")
        End If

        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")



        Dim user_dt As DataTable = GetTable("select * From userextrainfo left join userapps on userapps.username = UserExtraInfo.username left join app_settings on app_settings.appname = UserApps.appname where userextrainfo.username ='" & User.Identity.Name.Replace("'", "''") & "'")


        'If domain(1).ToLower = "callcriteria" Then 'if it's our app, send them to app.callcriteria.com otherwise just to CD2 and leave the domain alone
        '    Response.Redirect("http://app.callcriteria.com/cd2.aspx")
        'Else

        If user_dt.Rows.Count > 0 Then
            If user_dt.Rows(0).Item("default_page").ToString <> "" Then
                Response.Redirect(user_dt.Rows(0).Item("default_page").ToString)
            Else

                If user_dt.Rows(0).Item("dashboard").ToString = "new" And (User.IsInRole("Client") Or User.IsInRole("Supervisor") Or User.IsInRole("Manager") Or User.IsInRole("Agent")) Then
                    Response.Redirect("/home")
                Else

                    Response.Redirect("cd2.aspx")
                End If

            End If
        End If

        Response.Redirect("cd2.aspx")
        'End If
        'End If

        If Roles.IsUserInRole("Admin") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then

            Response.Redirect("Client_dashboard.aspx")
        End If



        If Roles.IsUserInRole("Agent") Then
            If Session("thisAgent") IsNot Nothing Then
                Response.Redirect("Agent2_Dashboard.aspx?agent=" & Session("thisAgent"))
            Else
                Response.Redirect("Agent2_Dashboard.aspx")
            End If

        End If

        If Roles.IsUserInRole("Client") Or Roles.IsUserInRole("Supervisor") Then
            Response.Redirect("Client_Dashboard.aspx")
        End If



        If Not Roles.IsUserInRole("Admin") Then
            pnlAdmin.Visible = False
            hlAgentReport.Visible = False
            'Response.Redirect("work_record.aspx")
        End If




        If Not IsPostBack Then
            If Roles.IsUserInRole("Supervisor") Or Roles.IsUserInRole("Manager") Or Roles.IsUserInRole("QA Lead") Then
                'Try
                'Dim pfl As ProfileCommon = Profile.GetProfile(User.Identity.Name)
                'ddlAgentGroup.SelectedValue = pfl.Group
                'ddlAgentGroup.Enabled = False

                'ddlAgentByGroup.DataSource = GetTable("SELECT distinct AGent FROM [XCC_REPORT_NEW] where agent_group = '" & pfl.Group & "' and appname = '" & Session("appname") & "' order by AGent")
                'ddlAgentByGroup.DataBind()
                'Catch ex As Exception

                'End Try

            End If


            dsMySessions.SelectParameters("selectedDate").DefaultValue = DateAdd(DateInterval.Day, -1, Today).ToString("MM/dd/yyyy")
            gvUserSessions.DataBind()

        End If



    End Sub





    Protected Sub btnChangeDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeDate.Click
        dsMySessions.SelectParameters("selectedDate").DefaultValue = txt_date.Text
        gvUserSessions.DataBind()
    End Sub


    Protected Sub ddlSupeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridViewRow = sender.parent.parent
        Dim userProfile As ProfileCommon = ProfileCommon.Create(row.Cells(3).Text)
        'Try
        userProfile.Supervisor = sender.SelectedValue
        userProfile.Save()

    End Sub

    Protected Sub ddlGroupChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridViewRow = sender.parent.parent
        Dim userProfile As ProfileCommon = ProfileCommon.Create(row.Cells(3).Text)
        'Try
        userProfile.Group = sender.SelectedValue
        userProfile.Save()

    End Sub







    Dim category As String = ""
    Dim section As String = ""






    'Protected Sub gvSummary_DataBound(sender As Object, e As System.EventArgs) Handles gvSummary.DataBound
    '    lblSuperAverge.Text = ""
    '    lblSuperAvergeWO.Text = ""
    '    Dim total As Integer = 0
    '    Dim total2 As Integer = 0
    '    Dim num_rows As Integer = 0
    '    For Each gvr As GridViewRow In gvSummary.Rows
    '        If gvr.RowType = DataControlRowType.DataRow Then
    '            total += gvr.Cells(5).Text
    '            total2 += gvr.Cells(6).Text
    '            num_rows += 1
    '        End If

    '    Next
    '    If num_rows > 0 Then
    '        lblSuperAverge.Text = CSng(CInt(total / num_rows * 100)) / 100
    '        lblSuperAvergeWO.Text = CSng(CInt(total2 / num_rows * 100)) / 100
    '    End If



    'End Sub






    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    dsQuestions.Insert()
    '    gvQuestions.DataBind()
    'End Sub



    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub









End Class
