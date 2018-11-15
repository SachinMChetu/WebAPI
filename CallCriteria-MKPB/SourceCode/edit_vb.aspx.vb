Imports Common
Imports System.Data

Partial Class edit_vb
    Inherits System.Web.UI.Page

    Public new_filename As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=edit_vb.aspx?ID=" & Request("ID"))
        End If

        If Not IsPostBack Then
            'Load the existing stuff
            Dim exist_list As DataTable = GetTable("select * from vwForm where f_id = " & Request("ID"))
            If exist_list.Rows.Count > 0 Then
                new_filename = exist_list.Rows(0).Item("audio_link")
            End If
            'Very Negative|Somewhat Negative|Somewhat Positive|Very Positive|NA|Confusing|Frustrating|Difficult|Question|Task|Other|End
        End If



    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        For Each rpt As RepeaterItem In rptQuestions.Items

            Dim hdnQID As HiddenField = rpt.FindControl("hdnQID")
            Dim QID As String = hdnQID.Value

            Dim rpt2 As Repeater = rpt.FindControl("Repeater1")

            UpdateTable("delete from form_q_scores_options where form_id = " & Request("ID") & " and question_id = " & QID)

            For Each rpt_inner As RepeaterItem In rpt2.Items
                Dim hdnExistingID As HiddenField = rpt_inner.FindControl("hdnExistingID")
                Dim Label2 As Label = rpt_inner.FindControl("Label2")
                Dim TextBox13 As TextBox = rpt_inner.FindControl("TextBox13")

                If TextBox13.Text <> "" Then

                    Dim times() As String = TextBox13.Text.Split(":")

                    Dim secs As Integer
                    If times.Length < 3 Then
                        secs = times(0) * 60 + times(1)
                    Else
                        secs = times(1) * 60 + times(2)
                    End If

                    UpdateTable("insert into  form_q_scores_options(option_value,option_pos,question_id,form_id) select '" & Label2.Text & "'," & secs & "," & QID & "," & Request("ID"))
                End If
            Next
        Next

        'Response.End()
    End Sub
End Class
