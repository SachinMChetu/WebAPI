Imports Common
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Partial Class Export_Details
    Inherits System.Web.UI.Page

    Dim comment_header As Integer = 0
    Dim missed_list_header As Integer = 0
    Dim call_id_header As Integer = 0
    Dim column_count As Integer = 0
    Dim blank_columns() As Integer

    Protected Sub Page_CommitTransaction(sender As Object, e As EventArgs) Handles Me.CommitTransaction

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        dsDetails.SelectParameters("username").DefaultValue = User.Identity.Name



    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'base.VerifyRenderingInServerForm(control);
    End Sub


    Protected Sub gvDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs) 'Handles gvDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim cell_count As Integer = 0
            For x = 0 To e.Row.Cells.Count - 1
                If e.Row.Cells(x).Text.ToUpper = "COMMENTS" Then
                    comment_header = x
                End If
                If e.Row.Cells(x).Text.ToUpper = "MISSED ITEMS" Then
                    missed_list_header = x
                    e.Row.Cells(x).Visible = False
                End If

                If e.Row.Cells(x).Text.ToUpper = "CALL ID" Then
                    call_id_header = x
                End If



                If x > 12 Then
                    e.Row.Cells(x).Visible = False
                End If
            Next


            column_count = e.Row.Cells.Count - 1

            blank_columns = New Integer(column_count) {}

            For x As Integer = 0 To column_count
                blank_columns(x) = 0
            Next

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'If e.Row.DataItem("pass_fail") = "success" Then
            '    e.Row.Cells(0).Text = "<span class='final-result'>PASS <i class='fa fa-check'></i></span>"
            'Else
            '    e.Row.Cells(0).Text = "<span class='final-result'>FAIL <i class='fa fa-times'></i></span>"
            '    e.Row.Attributes.Add("class", "fail-row")
            'End If


            For x As Integer = 0 To column_count
                If e.Row.Cells(x).Text <> "&nbsp;" Then
                    blank_columns(x) = 1
                End If
            Next


            'If e.Row.Cells(comment_header).Text <> "&nbsp;" Then


            e.Row.Cells(comment_header).Text = WebUtility.HtmlDecode(Regex.Replace(e.Row.Cells(comment_header).Text, "<[^>]*(>|$)", String.Empty)).Replace("|", "").Replace("â€™", "'")
            e.Row.Cells(missed_list_header).Text = WebUtility.HtmlDecode(Regex.Replace(e.Row.Cells(missed_list_header).Text, "<[^>]*(>|$)", String.Empty)).Replace("</a", "")
            e.Row.Cells(missed_list_header).Visible = False

            For x As Integer = 13 To 17
                e.Row.Cells(x).Visible = False
            Next


            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;br&gt;", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;b&gt;", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("&lt;/b&gt;", ""))

            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("<br>", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("<b>", ""))
            'e.Row.Cells(comment_header).Text = Trim(e.Row.Cells(comment_header).Text.Replace("</b>", ""))

            'End If


            Dim drv As DataRowView = e.Row.DataItem

            If call_id_header <> 0 Then
                e.Row.Cells(call_id_header).Text = "<a href='http://" & Request.ServerVariables("SERVER_NAME") & "/review_record.aspx?ID=" & drv.Item("call id").ToString & "'>View Call</a>"

            End If


        End If

    End Sub
End Class
