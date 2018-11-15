Option Compare Text
Imports System.IO

Partial Class guidelines
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=guidelines.aspx")
        End If


        'If User.IsInRole("Client") Or User.IsInRole("Admin") Then
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "no_work", "$('.client_edit').show();", True)
        'End If

        dsSC.SelectParameters("username").DefaultValue = User.Identity.Name

        hdnUsername.Value = User.Identity.Name
        hdnUsertype.Value = Roles.GetRolesForUser(User.Identity.Name).Single

    End Sub


    Protected Sub ddlScorecard_DataBound(sender As Object, e As EventArgs) Handles ddlScorecard.DataBound
        If ddlScorecard.Items.Count > 1 Then
            ddlScorecard.Enabled = True
        Else
            ddlScorecard.Enabled = False
        End If

        'For Each li As ListItem In ddlScorecard.Items
        '    If li.Text.IndexOf("|red") > -1 Then
        '        li.Text = Replace(li.Text, "|red", "")
        '        li.Attributes.Add("style", "color:red")
        '    Else
        '        li.Text = Replace(li.Text, "|", "")
        '    End If
        'Next
    End Sub

    Protected Sub ddlScorecard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlScorecard.SelectedIndexChanged
        dsSections.DataBind()

        If ddlScorecard.Items.Count > 1 Then
            ddlScorecard.Enabled = True
        Else
            ddlScorecard.Enabled = False
        End If

        'For Each li As ListItem In ddlScorecard.Items
        '    If li.Text.IndexOf("|red") > -1 Then
        '        li.Text = Replace(li.Text, "|red", "")
        '        li.Attributes.Add("style", "color:red")
        '    Else
        '        li.Text = Replace(li.Text, "|", "")
        '    End If
        'Next
    End Sub


    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click


        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.word"
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.AddHeader("Content-Disposition", "attachment;filename=" & ddlScorecard.SelectedItem.Text & "_guidelines.doc")
        Response.Charset = ""
        EnableViewState = False
        Dim table As New Table()
        Dim row As New TableRow()
        row.Cells.Add(New TableCell())
        row.Cells(0).Controls.Add(pnlGuide)
        table.Rows.Add(row)
        Dim oStringWriter As New System.IO.StringWriter()
        Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
        table.RenderControl(oHtmlTextWriter)
        Response.Write(oStringWriter.ToString())
        Response.[End]()


        'HttpContext.Current.Response.Clear()
        'HttpContext.Current.Response.Buffer = True
        'HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & ddlScorecard.SelectedItem.Text & "_guidelines.doc")
        'HttpContext.Current.Response.Charset = ""
        'HttpContext.Current.Response.ContentType = "application/vnd.ms-word"

        'Dim sw As New StringWriter()
        'Dim hw As New HtmlTextWriter(sw)

        'pnlGuide.RenderControl(hw)
        ''style to format numbers to string
        'Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        'HttpContext.Current.Response.Write(style)
        'HttpContext.Current.Response.Output.Write(sw.ToString().Replace("Â", "").Replace("&nbsp;", ""))
        'Response.End()

    End Sub
End Class
