Imports Common
Imports System.Data


Partial Class CC_Master
    Inherits System.Web.UI.MasterPage

    Public agenda_items As String

    Public current_version As String


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim dt As DataTable = GetTable("select current_version, prod_version from system_settings")
        If Request.Url.AbsoluteUri.ToString.IndexOf("callcriteria-dev") > -1 Then
            current_version = dt.Rows(0).Item(0)
        Else
            current_version = dt.Rows(0).Item(1)
        End If

        Response.AppendHeader("Access-Control-Allow-Origin", "*")
        '                        </td> <td class="headerLink"><a href="">Billing</a></td>
        If Not IsPostBack Then





            ' Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, "OnSubmitScript", "return handleSubmit()")

            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            If domain(1).ToLower = "callcriteria" Or domain(1).ToLower = "callcriteria-dev" Then
                If Me.Page.Title = "" Then
                    Me.Page.Title = "Call Criteria"
                End If
                favIcon.Visible = True
                favIconX.Visible = True

            Else
                Me.Page.Title = "QA Tool"
                favIcon.Visible = False
                favIconX.Visible = False
            End If

            'Populate user data


        End If

    End Sub
    Protected Sub btnGo_Click(sender As Object, e As EventArgs)
        'Dim alt_user As String = CType(lvAltUser.FindControl("new_user"), TextBox).Text

        'Dim user_dt As DataTable = GetTable("select * from userextrainfo with (nolock)   where username = '" & alt_user.Replace("'", "''") & "'")

        'If user_dt.Rows.Count > 0 Then
        '    UserImpersonation.ImpersonateUser(alt_user, Page.Request.FilePath)
        '    Dim cookie As HttpCookie = Request.Cookies("filter")
        '    If cookie IsNot Nothing Then
        '        cookie.Expires = DateTime.Now.AddYears(-1)
        '        HttpContext.Current.Response.Cookies.Add(cookie)
        '    End If

        '    Response.Redirect(Page.Request.FilePath)
        'Else
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "nouser", "alert('User does not exist');", True)
        'End If
    End Sub


End Class

