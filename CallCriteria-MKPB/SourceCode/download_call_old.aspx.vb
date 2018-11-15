Imports System.IO
Imports System.Data
Imports Common

Partial Class download_call
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Public data_rate As String
    Public download_id As String
    Public play_option As String = "true"

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub





    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        download_id = Request("ID")
        If Session("appname") Is Nothing Then
            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            Session("appname") = domain(0)
        End If

        If Not IsPostBack Then


            If Request("ID") IsNot Nothing Then
                hdnThisID.Value = Request("ID")
                Dim record_dt2 As DataTable = GetTable("select * from vwForm left join form_notifications on form_notifications.form_id = vwForm.f_id where vwForm.F_id = " & Request("ID"))
                If record_dt2.Rows.Count > 0 Then
                    hdnCallLength.Value = record_dt2.Rows(0).Item("call_length").ToString
                    hdnThisAgent.Value = record_dt2.Rows(0).Item("Agent").ToString
                Else
                    Response.Redirect("default.aspx")

                End If
            End If





            Dim record_dt As DataTable = GetTable("select * from vwForm join scorecards on scorecards.id = scorecard where f_id =  " & Request("ID"))

            If record_dt.Rows.Count > 0 Then

                Dim filename As String = record_dt.Rows(0).Item("call_date").ToString.Replace("/", "_").Replace(" 12:00:00 AM", "") & "_" & record_dt.Rows(0).Item("phone").ToString & "_" & record_dt.Rows(0).Item("agent").ToString



                litPersonal.Text = litPersonal.Text & getSidebarData3("First", UpperLeft(record_dt.Rows(0).Item("First_Name").ToString()))
                litPersonal.Text = litPersonal.Text & getSidebarData3("Last", UpperLeft(record_dt.Rows(0).Item("Last_Name").ToString()))
                litPersonal.Text = litPersonal.Text & getSidebarData3("Email", record_dt.Rows(0).Item("Email").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Phone", record_dt.Rows(0).Item("phone").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Website", record_dt.Rows(0).Item("website").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Education Level", record_dt.Rows(0).Item("EducationLevel").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("High School Grad Year", record_dt.Rows(0).Item("HighSchoolGradYear").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Degree Start Timeframe", record_dt.Rows(0).Item("DegreeStartTimeframe").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Campaign", record_dt.Rows(0).Item("Campaign").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Call Type", record_dt.Rows(0).Item("call_type").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Disposition", record_dt.Rows(0).Item("Disposition").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Notes", record_dt.Rows(0).Item("disposition").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Compliance Sheet", record_dt.Rows(0).Item("compliance_sheet").ToString())


                'Contact
                litPersonal.Text = litPersonal.Text & getSidebarData3("Address", record_dt.Rows(0).Item("address").ToString())
                If record_dt.Rows(0).Item("State").ToString() <> "" Then
                    litPersonal.Text = litPersonal.Text & getSidebarData3("", record_dt.Rows(0).Item("City").ToString() & ", " & record_dt.Rows(0).Item("State").ToString() & " " & record_dt.Rows(0).Item("Zip").ToString())
                End If
                litPersonal.Text = litPersonal.Text & getSidebarData3("ANI", record_dt.Rows(0).Item("ANI").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("DNIS", record_dt.Rows(0).Item("DNIS").ToString())

                litPersonal.Text = litPersonal.Text & getSidebarData3("Call Date", CDate(record_dt.Rows(0).Item("call_date").ToString()).ToShortDateString())
                litPersonal.Text = litPersonal.Text & getSidebarData3("Scorecard", record_dt.Rows(0).Item("short_name").ToString())




                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=\" & Chr(34) & filename & Chr(34) & ".xls")
                Response.Charset = ""
                Response.ContentType = "application/vnd.ms-excel"

                Dim sw As New StringWriter()
                Dim hw As New HtmlTextWriter(sw)

                rptSections.DataBind()

                form1.RenderControl(hw)
                'style to format numbers to string
                Dim style As String = "<style>.textmode{mso-number-format:\@;.section-header {background-color: rgba(24,33,40,.14); color: #242f3a; text-shadow: 0px 1px 1px rgba(255,255,255,1);box-shadow: -1px 0px 0px rgba(255,255,255,0.5), 1px 0px 0px rgba(0,0,0,0.1);}}</style>"
                Response.Write(style)
                Response.Output.Write(sw.ToString())
                Response.Flush()
                Response.End()
            End If

        End If
    End Sub

    Protected Sub fvFORMData_DataBound(sender As Object, e As System.EventArgs) Handles fvFORMData.DataBound
        Dim lbl As Label = fvFORMData.FindControl("lblPlayer")




        Dim drv As DataRowView = fvFORMData.DataItem

        If drv Is Nothing Then
            Response.Redirect("default.aspx")
        End If




        If drv("website").ToString <> "" Then
            pnlLogo.Visible = False
            Dim pnl As Panel = fvFORMData.FindControl("pnlNonWeb")
            pnl.Visible = False
        Else
            Dim img As New Image
            img.ImageUrl = "http://app.callcriteria.com/img/CallCriteria-side.png"
            img.Height = 100
            pnlLogo.Controls.Add(img)

        End If


    End Sub

    Protected Sub gvQuestions_RowDataBound(sender As Object, e As RepeaterItemEventArgs) 'Handles gvQuestions.RowDataBound
        If hdnCallLength.Value <> "" Then
            If e.Item.ItemType = ListItemType.Item Then
                'Dim litClass As Literal = e.Item.FindControl("litClass")
                Dim tr As HtmlTableRow = e.Item.FindControl("trHeader")

                For Each td As HtmlTableCell In tr.Cells
                    If e.Item.DataItem("bad-response") = "bad-response" Then
                        'litClass.Text = "style='background-color:rgba(214,208,100,0.55)'"
                        'tr.Attributes.Add("style", "background-color:rgba(214,208,100,0.55)")
                        td.Attributes.Add("style", "background-color:#eeebc0")
                    Else
                        'litClass.Text = "style='background-color:rgba(174,174,174,0.02)'"
                        td.Attributes.Add("style", "background-color:#f1f6f8")
                        'tr.BgColor = "rgba(174,174,174,0.02)"
                    End If
                Next

            End If
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                'Dim litClass As Literal = e.Item.FindControl("litClass")
                Dim tr As HtmlTableRow = e.Item.FindControl("trHeader")

                For Each td As HtmlTableCell In tr.Cells
                    If e.Item.DataItem("bad-response") = "bad-response" Then
                        ' litClass.Text = "style='background-color:rgba(214,208,100,0.55)'"
                        'tr.BgColor = "rgba(214,208,100,0.55)"
                        'tr.Attributes.Add("style", "background-color:rgba(214,208,100,0.55)")
                        td.Attributes.Add("style", "background-color:#eeebc0")
                    Else
                        'litClass.Text = "style='background-color:rgba(97,148,174,0.08)'"
                        'tr.BgColor = "rgba(97,148,174,0.08)"
                        'tr.Attributes.Add("style", "background-color:rgba(97,148,174,0.08)")
                        td.Attributes.Add("style", "background-color:#fdfdfd")
                    End If
                Next


            End If

        End If
    End Sub


    Protected Sub rptContact_PreRender(sender As Object, e As EventArgs)
        Dim myRepeater As Repeater = DirectCast(sender, Repeater)
        Dim counter As Integer = myRepeater.Items.Count
        If counter = 0 Then
            myRepeater.Visible = False
        End If
    End Sub



    Protected Function getSidebarData3(lbl As String, value As String) As String
        If value <> "" Then
            'Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"
            Return " <tr><td><strong>" & lbl & "</strong></td><td class='info-data'>" & value & "</td></tr>"
        Else
            Return ""
        End If

    End Function

    Protected Sub rptComments_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)




    End Sub
End Class

