Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Common

Partial Class bad_call_accepted
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		If User.Identity.Name = "" Or User.Identity.Name Is Nothing Then
			Response.Redirect("login.aspx?ReturnURL=bad_call_accepted.aspx")
		End If

        dsSCORECARD.SelectParameters("username").DefaultValue = User.Identity.Name
        dsAppname.SelectParameters("username").DefaultValue = User.Identity.Name

        If Not IsDate(txtCALL_DATE_FROM.Text) Or Not IsDate(txtCALL_DATE_TO.Text) Then
			txtCALL_DATE_FROM.Text = ""
			txtCALL_DATE_TO.Text = ""
		End If

		If Not IsDate(txtBAD_CALL_ACCEPTED.Text) Then
			txtBAD_CALL_ACCEPTED.Text = ""
		End If

		If Not IsPostBack Then
			ViewState("sortColumn") = "bad_call_accepted"
			ViewState("sortOrder") = "DESC"
			Me.BindGrid("bad_call_accepted", "DESC")
		End If

	End Sub

	Private Sub BindGrid(Optional sortExpression As String = "", Optional sortDirection As String = "")

        Dim strSCORECARD As String = ddlSCORECARD.SelectedItem.Value
        Dim strAppname As String = ddlAppname.SelectedItem.Value
        Dim strCALL_DATE_FROM As String = txtCALL_DATE_FROM.Text
		Dim strCALL_DATE_TO As String = txtCALL_DATE_TO.Text
		Dim strBAD_CALL_ACCEPTED As String = txtBAD_CALL_ACCEPTED.Text

        Dim strSQL As String = "SELECT SESSION_ID, AGENT, CAMPAIGN, TIMESTAMP, AGENT_GROUP, EMAIL, CITY, STATE, ZIP, STATUS, ID, REVIEW_STARTED, APPNAME, FIRST_NAME, LAST_NAME, ADDRESS, PHONE, CALL_DATE, AUDIO_LINK, BAD_CALL, BAD_CALL_WHO, BAD_CALL_DATE, BAD_CALL_REASON, DATE_ADDED, BAD_CALL_ACCEPTED, BAD_CALL_ACCEPTED_WHO, SCORECARD FROM dbo.xcc_report_new WHERE bad_call_accepted IS NOT NULL "

        If strSCORECARD <> "" Then
            strSQL += " AND scorecard = '" + strSCORECARD + "'"
        End If

        If strAppname <> "" Then
            strSQL += " AND appname = '" + strAppname + "'"
        End If

        If strCALL_DATE_FROM <> "" And strCALL_DATE_TO <> "" Then
            strSQL += " AND CAST(call_date AS DATE) BETWEEN '" + strCALL_DATE_FROM + "' AND '" + strCALL_DATE_TO + "'"
        Else
            strSQL += "  AND bad_call_accepted > DATEADD(d, -30, getdate()) "
        End If

		If strBAD_CALL_ACCEPTED <> "" Then
			strSQL += " AND CAST(bad_call_accepted AS DATE) = '" + strBAD_CALL_ACCEPTED + "'"
		End If

        'strSQL += " ORDER BY bad_call_accepted DESC"

        strSQL += "  and scorecard in (select user_scorecard from userapps where username =  '" & User.Identity.Name & "') "

        Dim dt As DataTable
		Dim strCon As String = ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString
		Using cn As New SqlConnection(strCon)
			Using cmd As New SqlCommand(strSQL, cn)
				Using da As New SqlDataAdapter(cmd)
					dt = New DataTable
					da.Fill(dt)
				End Using
			End Using
		End Using

		'Session("dtBAD_CALL_ACCEPTED") = dt

		'Dim dt = TryCast(Session("dtBAD_CALL_ACCEPTED"), DataTable)

		If sortExpression.Length > 0 Then
			dt.DefaultView.Sort = sortExpression + " " + sortDirection
			'dt = New DataView(dt, "", sortExpression + " " + sortDirection, DataViewRowState.CurrentRows).ToTable()
			ViewState("sortColumn") = sortExpression
			ViewState("sortOrder") = sortDirection
		End If

		gvBadCallAccepted.DataSource = dt
		gvBadCallAccepted.DataBind()

		lblCOUNT.Text = "Your search returns " + dt.Rows.Count.ToString() + " record(s)."

		btnEXPORT.Visible = False
		btnSHOWALL.Visible = False

		If dt.Rows.Count > 0 Then
			btnEXPORT.Visible = True
			If dt.Rows.Count > gvBadCallAccepted.PageSize And gvBadCallAccepted.AllowPaging Then
				btnSHOWALL.Visible = True
			End If
		End If

	End Sub

	Protected Sub gvBadCallAccepted_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			e.Row.Attributes("ID") = gvBadCallAccepted.DataKeys(e.Row.RowIndex).Value
		End If
	End Sub

	Protected Sub gvBadCallAccepted_OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
		gvBadCallAccepted.PageIndex = e.NewPageIndex
		Me.BindGrid(ViewState("sortColumn").ToString(), ViewState("sortOrder").ToString())
	End Sub

	Protected Sub gvBadCallAccepted_Sorting(sender As Object, e As GridViewSortEventArgs)
		Me.BindGrid(e.SortExpression, sortOrder)
	End Sub

	Public Property sortOrder() As String
		Get
			If ViewState("sortOrder").ToString() = "ASC" Then
				ViewState("sortOrder") = "DESC"
			Else
				ViewState("sortOrder") = "ASC"
			End If
			Return ViewState("sortOrder").ToString()
		End Get
		Set(ByVal value As String)
			ViewState("sortOrder") = value
		End Set
	End Property

	Protected Sub btnSEARCH_Click(sender As Object, e As EventArgs)
		gvBadCallAccepted.AllowPaging = True
		gvBadCallAccepted.PageIndex = 0
		Me.BindGrid(ViewState("sortColumn").ToString(), ViewState("sortOrder").ToString())
	End Sub

	Protected Sub btnSHOWALL_Click(sender As Object, e As EventArgs)
		gvBadCallAccepted.AllowPaging = False
		Me.BindGrid(ViewState("sortColumn").ToString(), ViewState("sortOrder").ToString())
	End Sub

    Protected Sub btnEXPORT_Click(sender As Object, e As EventArgs) Handles btnEXPORT.Click

        If gvBadCallAccepted.AllowPaging Then
            gvBadCallAccepted.AllowPaging = False
        End If
        If gvBadCallAccepted.AllowSorting Then
            gvBadCallAccepted.AllowSorting = False
        End If
        Me.BindGrid(ViewState("sortColumn").ToString(), ViewState("sortOrder").ToString())


        GV_to_CSV(gvBadCallAccepted, "Bad_Call_Accepted_" & ddlSCORECARD.SelectedValue.ToString())

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Bad_Call_Accepted_" & ddlSCORECARD.SelectedValue.ToString() & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            gvBadCallAccepted.RenderControl(hw)

            Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.End()
        End Using
    End Sub

    Protected Sub gvBadCallAccepted_PreRender(sender As Object, e As EventArgs)
		If gvBadCallAccepted.HeaderRow IsNot Nothing Then
			gvBadCallAccepted.HeaderRow.TableSection = TableRowSection.TableHeader
		End If
	End Sub

	Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
		'Verifies that the control is rendered
	End Sub

End Class
