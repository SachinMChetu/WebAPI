Imports Common
Imports System.Data

Partial Class bulk_numbers
    Inherits System.Web.UI.Page

    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click


        Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")

        Dim dt As New DataTable
        dt.Columns.Add("F_ID")
        dt.Columns.Add("session_id")
        dt.Columns.Add("AGENT")
        dt.Columns.Add("CAMPAIGN")
        dt.Columns.Add("DNIS")
        dt.Columns.Add("review_started")
        dt.Columns.Add("appname")
        dt.Columns.Add("total_score")
        dt.Columns.Add("Comments")
        'F_ID, expr1,AGENT,CAMPAIGN, DNIS,review_started, appname,total_score,Comments


        Dim numbers() As String = txtPhones.Text.Split(Chr(13))
        Dim bad_list As String = "Missing: "

        For Each number In numbers
            Dim dt2 As DataTable = GetTable("select F_ID, session_id,AGENT,CAMPAIGN, DNIS,review_started, appname,total_score,Comments from vwForm where (dnis = ('" & number.Replace(Chr(10), "") & "') or ani = ('" & number.Replace(Chr(10), "") & "') or phone = ('" & number.Replace(Chr(10), "") & "')) and appname = '" & domain(0) & "' ")
            If dt2.Rows.Count > 0 Then
                Dim dr As DataRow = dt.NewRow
                dr("F_ID") = dt2.Rows(0).Item("F_ID")
                dr("session_id") = dt2.Rows(0).Item("session_id")
                dr("AGENT") = dt2.Rows(0).Item("AGENT")
                dr("CAMPAIGN") = dt2.Rows(0).Item("CAMPAIGN")
                dr("DNIS") = dt2.Rows(0).Item("DNIS")
                dr("review_started") = dt2.Rows(0).Item("review_started")
                dr("appname") = dt2.Rows(0).Item("appname")
                dr("total_score") = dt2.Rows(0).Item("total_score")
                dr("Comments") = dt2.Rows(0).Item("Comments")
                dt.Rows.Add(dr)
            Else
                bad_list &= "<br>" & number.Replace(Chr(10), "")
            End If
        Next


        lblNotFound.Text = bad_list
        gvResults.DataSource = dt
        gvResults.DataBind()

    End Sub
End Class
