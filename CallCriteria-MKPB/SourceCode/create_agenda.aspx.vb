Imports System.Data
Imports System.Data.SqlClient
Imports Common
Partial Class create_agenda
    Inherits System.Web.UI.Page

    Private Sub create_agenda_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            dsApps.SelectParameters("username").DefaultValue = User.Identity.Name


            If Request("ID") <> "" Then

                Dim agenda_dt As DataTable = GetTable("select * from agenda_item where id = " & Request("ID"))
                If agenda_dt.Rows.Count > 0 Then
                    hdnAgendaID.Value = Request("ID")
                    '  txtLink.Text = agenda_dt.Rows(0).Item("page_link").ToString
                    ddlApp.DataBind()
                    Try
                        ddlApp.SelectedValue = agenda_dt.Rows(0).Item("sc_id").ToString

                        ddlSections_SelectedIndexChanged(sender, e)
                        ddlApp.Enabled = False

                        ddlSections.Enabled = True

                    Catch ex As Exception

                    End Try

                End If
            End If
        End If


    End Sub

    Private Sub ddlApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApp.SelectedIndexChanged

        If ddlApp.SelectedValue <> "" Then
            ddlSections.Enabled = True

            clearDDL(ddlSections)
            ddlSections.Items.Add(New ListItem("Scorecard general", "0"))
            dsSections.DataBind()
        Else
            clearDDL(ddlSections)
            ddlSections.Items.Add(New ListItem("Scorecard general", "0"))
            ddlSections.Enabled = False
        End If


    End Sub

    Private Sub ddlSections_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSections.SelectedIndexChanged
        clearDDL(ddlF)
        clearDDL(ddlQ)
        clearDDL(ddlI)

        dsF.DataBind()
        dsQ.DataBind()
        dsI.DataBind()
    End Sub

    Private Sub clearDDL(ddl As DropDownList)
        ddl.Items.Clear()
        ddl.Items.Add(New ListItem("(Select)", ""))
    End Sub

    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim cn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("estomes2ConnectionString").ConnectionString)
        cn.Open()


        Dim sql As String = ""
        Dim reply As New SqlCommand(sql, cn)

        If hdnAgendaID.Value = "" Then

            sql = "declare @new_id int;Insert into agenda_item(who_created, date_created,  sc_id) select @who_created, dbo.getMTDate(),  @sc_id; select @new_id = @@Identity; select @new_id;"

            reply = New SqlCommand(sql, cn)
            reply.CommandTimeout = 60

            reply.Parameters.AddWithValue("who_created", User.Identity.Name)
            'reply.Parameters.AddWithValue("page_link", txtLink.Text)
            reply.Parameters.AddWithValue("sc_id", ddlApp.SelectedValue)


            Dim new_id As Integer = reply.ExecuteScalar()
            hdnAgendaID.Value = new_id

        End If


        ddlApp.Enabled = False


        sql = "insert into agenda_topics (who_created, date_created, agenda_id, section , header,  question, page_link ) select  @who_created, dbo.getMTDate(), @agenda_id, @section , @header,   @question, @page_link; update agenda_topics set page_link = replace(page_link,'work.pointqa.com','app.callcriteria.com'); "
        reply = New SqlCommand(sql, cn)
        reply.CommandTimeout = 60

        reply.Parameters.AddWithValue("who_created", User.Identity.Name)
        reply.Parameters.AddWithValue("agenda_id", hdnAgendaID.Value)
        reply.Parameters.AddWithValue("section", ddlSections.SelectedValue)
        reply.Parameters.AddWithValue("header", Trim(ddlF.SelectedValue & " " & ddlQ.SelectedValue & " " & ddlI.SelectedValue & " " & txtOther.Text))
        reply.Parameters.AddWithValue("question", txtDiscuss.Text)
        reply.Parameters.AddWithValue("page_link", txtLink.Text)

        reply.ExecuteNonQuery()

        dsAgendaItems.DataBind()
        gvAgendaItems.DataBind()

        cn.Close()
        cn.Dispose()
    End Sub
End Class
