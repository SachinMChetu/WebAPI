Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Common

Partial Class My_Notifications
    Inherits System.Web.UI.Page
    Dim header_cell As Integer = -1


    Protected Sub btnAgree_Click(sender As Object, e As System.EventArgs)
        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Dim sql As String
        If txtComments.Text <> "" Then
            sql = "Update form_Notifications set date_closed = dbo.getMTDate(), comment = isnull(comment,'') + @new_comments, closed_by = '" & ack_by & "' where date_closed is null and form_id = " & hdnOpenFormID.Value
        Else
            sql = "Update form_Notifications set date_closed = dbo.getMTDate(), closed_by = '" & ack_by & "' where date_closed is null and form_id = " & hdnOpenFormID.Value
        End If

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim reply As New SqlCommand(sql, cn)
        reply.CommandTimeout = 60
        If txtComments.Text <> "" Then
            If txtComments.Text = "" Then
                reply.Parameters.AddWithValue("new_comments", "Agent Agreed")
            Else
                reply.Parameters.AddWithValue("new_comments", txtComments.Text)
            End If
        End If
        reply.ExecuteNonQuery()
        cn.Close()
        cn.Dispose()


        'UpdateTable("insert into [form_notifications](session_id,assigned_to,comment,dateadded,form_id,opened_by) SELECT session_id,assigned_to,comment,dbo.getMTDate(),form_id,'" & Request("agent") & "' FROM agent_notifications where form_id = " & hdnOpenFormID.Value)


        Response.Redirect("agent_Notifications.aspx?agent=" & Request("agent"))

    End Sub

    Protected Sub btnDisagree_Click(sender As Object, e As System.EventArgs)

        If txtComments.Text <> "" And txtComments.Text <> "Comment Required!!!!" Then
            txtComments.Text = "Comment Required!!!!"
            Exit Sub
        End If
        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Dim sql As String
        If txtComments.Text <> "" Then
            sql = "Update form_Notifications set date_closed = dbo.getMTDate(), comment = isnull(comment,'') + @new_comments, closed_by = '" & ack_by & "' where date_closed is null and form_id = " & hdnOpenFormID.Value
        Else
            sql = "Update form_Notifications set date_closed = dbo.getMTDate(), , closed_by = '" & ack_by & "' where date_closed is null and  form_id = " & hdnOpenFormID.Value
        End If

        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()

        Dim reply As New SqlCommand(sql, cn)
        reply.CommandTimeout = 60
        If txtComments.Text <> "" Then
            If txtComments.Text = "" Then
                reply.Parameters.AddWithValue("new_comments", "<br>" & ack_by & " (" & Now.ToString & ") - Agent disagreed")
            Else
                reply.Parameters.AddWithValue("new_comments", "<br>" & ack_by & " (" & Now.ToString & ") - " & txtComments.Text)
            End If
        End If


        reply.ExecuteNonQuery()
        cn.Close()
        cn.Dispose()


        UpdateTable("insert into [form_notifications](role,date_created,form_id,opened_by) SELECT 'Supervisor',dbo.getMTDate(),form_id,'" & Request("agent") & "'")


        Response.Redirect("agent_Notifications.aspx?agent=" & Request("agent"))
    End Sub


    Protected Sub Ack_Item(sender As Object, e As System.EventArgs)
        Dim btn As Button = sender

        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Common.UpdateTable("Update form_notifications set date_closed = dbo.getMTDate(), closed_by = '" & ack_by & "' where id = " & btn.CommandArgument)
        gvComments.DataBind()
    End Sub

    Protected Sub Sorry_Item(sender As Object, e As System.EventArgs)
        Dim btn As Button = sender

        Dim ack_by As String = ""
        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Common.UpdateTable("Update form_notifications set  date_closed = dbo.getMTDate(), closed_by = '" & ack_by & "' where id = " & btn.CommandArgument)
        gvComments.DataBind()
    End Sub

    Protected Sub Agree_Item(sender As Object, e As System.EventArgs)
        Dim btn As Button = sender

        Dim ack_by As String = ""

        If Request("agent") IsNot Nothing Then
            ack_by = Request("Agent")
        Else
            ack_by = HttpContext.Current.User.Identity.Name
        End If

        Common.UpdateTable("Update form_notifications set date_closed = dbo.getMTDate(), closed_by = '" & ack_by & "' where id = " & btn.CommandArgument)

        gvComments.DataBind()

    End Sub

    Public data_rate As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
        Else
            data_rate = 0.05
        End If

        If Request("agent") Is Nothing Then
            Response.Redirect("agent2_dashboard.aspx")
        End If

        If Request("agent") IsNot Nothing And User.Identity.Name = "" Then
            Session("thisAgent") = Request("agent")
            'Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, "agent", DateTime.Now, DateTime.Now.AddMinutes(30), False, String.Empty, FormsAuthentication.FormsCookiePath)
            FormsAuthentication.SetAuthCookie("agent", True)

        End If

        'If txtAgentStart.Text <> "" Then
        '    Session("StartDate") = txtAgentStart.Text
        'End If

        'If Session("StartDate") <> "" Then
        '    txtAgentStart.Text = Session("StartDate")
        'End If

        'If txtAgentEnd.Text <> "" Then
        '    Session("EndDate") = txtAgentEnd.Text
        'End If

        'If Session("EndDate") <> "" Then
        '    txtAgentEnd.Text = Session("EndDate")
        'End If




        If Not IsPostBack Then

            'Dim body As HtmlGenericControl = Master.FindControl("bodyNode")
            'body.Attributes.Add("class", "notifications-body collapsed-menu")

            hdnAgent.Value = Request("agent")

            dsComments.DataBind()
            'If txtAgentStart.Text = "" Then
            '    txtAgentStart.Text = Now.ToShortDateString
            'End If
            'If txtAgentEnd.Text = "" Then
            '    txtAgentEnd.Text = DateAdd(DateInterval.Day, 1, Now).ToShortDateString
            'End If
            'dsComments.SelectParameters("start_date").DefaultValue = Now.ToShortDateString
            'dsComments.SelectParameters("end_date").DefaultValue = DateAdd(DateInterval.Day, 1, Now).ToShortDateString

            'dsZeroComments.SelectParameters("start_date").DefaultValue = Now.ToShortDateString
            'dsZeroComments.SelectParameters("end_date").DefaultValue = DateAdd(DateInterval.Day, 1, Now).ToShortDateString

            'Dim pfl As ProfileCommon = Profile.GetProfile(User.Identity.Name)
            'If pfl.Group <> "" Then

            '    If ddlGroup.Items.Contains(New ListItem(pfl.Group)) Then
            '    Else
            '        ddlGroup.Items.Add(New ListItem(pfl.Group))
            '    End If

            '    ddlGroup.SelectedValue = pfl.Group
            '    ddlGroup.Enabled = False
            'End If


            'dsMessages.SelectParameters("to_login").DefaultValue = HttpContext.Current.User.Identity.Name
            'dsMessages.DataBind()

            'BuildFilter()
        End If


    End Sub




    Protected Sub BuildFilter()



        'dsComments.FilterExpression = "1=1"

        'dsComments.DataBind()

        'gvComments.DataBind()

    End Sub

    Protected Sub btnSupeExportxp_Click(sender As Object, e As System.EventArgs) Handles btnSupeExportxp.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Notifications.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvComments.Columns(11).Visible = True

        gvComments.AllowPaging = False
        gvComments.DataBind()


        For Each gvr As GridViewRow In gvComments.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                gvr.Cells(0).Text = "<a href='http://" & Request.ServerVariables("SERVER_NAME") & "/review_record.aspx?ID=" & CType(gvr.FindControl("hdnFormID"), HiddenField).Value & "'>View</a>"
                'hl.NavigateUrl = "http://" & Request.ServerVariables("SERVER_NAME") & "/" & hl.NavigateUrl
            End If
        Next

        gvComments.RenderControl(hw)
        'style to format numbers to string
        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub




    Protected Sub gvComments_PreRender(sender As Object, e As EventArgs) Handles gvComments.PreRender
        Dim gv As GridView = sender
        If gv.HeaderRow IsNot Nothing Then
            gv.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub gvComments_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvComments.RowDataBound


        'If e.Row.RowType = DataControlRowType.Header Then
        '    Dim cell_count As Integer = 0
        '    For Each tc As TableCell In e.Row.Cells
        '        If tc.Text.IndexOf("Comment") > -1 Then
        '            header_cell = cell_count
        '        End If
        '        cell_count += 1
        '    Next
        'End If


        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim drv As DataRowView = e.Row.DataItem


            'If Len(e.Row.Cells(4).Text) > 1 Then
            Dim lbl1 As Literal = e.Row.FindControl("Label1")
            e.Row.Cells(4).Text = "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(lbl1.Text) & "</div></i></a>"
            'End If


            If Not User.IsInRole("QA") Then
                If Len(e.Row.Cells(5).Text) > 1 Then
                    Dim replacement_text As String = ""
                    Dim mq_list() As String = e.Row.Cells(5).Text.Split(",")
                    For Each mq As String In mq_list

                        Dim q_pos As DataTable = GetTable("select case when q_position > call_length then call_length - 100 else q_position end as q_position, audio_link, form_score3.id as form_id from form_score3 join form_q_scores on form_score3.id= form_q_scores.form_id " & _
                            "join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID join Questions on questions.id = form_q_scores.question_id  " & _
                            "where form_score3.id = " & drv("form_id").ToString & " and q_short_name='" & mq & "'")

                        If q_pos.Rows.Count > 0 Then
                            'Response.Write("select q_position, audio_link from form_score3 join form_q_scores on form_score3.id= form_q_scores.form_id " & _
                            '    "join XCC_REPORT_NEW on XCC_REPORT_NEW.ID = form_score3.review_ID join Questions on questions.id = form_q_scores.question_id  " & _
                            '    "where form_score3.id = " & drv("form_id").ToString & " and q_short_name='" & mq & "'")
                            'Response.End()

                            If replacement_text <> "" Then
                                replacement_text &= ",<a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");event.stopPropagation();'>" & mq & "</a>"
                            Else
                                replacement_text &= "<a href='javascript:show_audio(" & Chr(34) & q_pos.Rows(0).Item("audio_link").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("q_position").ToString & Chr(34) & "," & Chr(34) & q_pos.Rows(0).Item("form_id").ToString & Chr(34) & ");event.stopPropagation();'>" & mq & "</a>"
                            End If
                        End If
                    Next
                    e.Row.Cells(5).Text = replacement_text
                    'e.Row.Cells(5).Attributes.Add("onclick", "event.stopPropagation();")
                End If
            End If

            'e.Row.Attributes.Add("onclick", "window.open('review_record.aspx?ID=" & drv("form_id") & "', '_blank');")

        End If


    End Sub

    Protected Sub gvComments_DataBound(sender As Object, e As System.EventArgs) Handles gvComments.DataBound
        'lblRows.Text = gvComments.Rows.Count

    End Sub

    Protected Sub dsComments_Selected(sender As Object, e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles dsComments.Selected
        lblRows.Text = e.AffectedRows

    End Sub


    Protected Sub gvCompleted_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvComments.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Len(e.Row.Cells(7).Text) > 1 Then
                e.Row.Cells(7).Text = "<a class='comments-trigger' href='#'><i class='fa fa-file'><div class='full-question-tooltip'>" & Trim(e.Row.Cells(7).Text) & "</div></i></a>"
            End If

            Dim drv As DataRowView = e.Row.DataItem

            'e.Row.Attributes.Add("onclick", "location.href='review_record.aspx?ID=" & drv("form_id") & "'")
        End If



    End Sub


End Class
