Imports Common
Partial Class keywords
    Inherits System.Web.UI.Page

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        UpdateTable("insert into utterance_flags(utterance_type, problem, utterance, scorecard, accepted_variant,date_added) select '" & ddlNewType.SelectedValue & "','" & ddlProblem.SelectedValue & "','" & txtNew.Text.Replace("'", "''") & "'," & ddlScorecard.SelectedValue & ",'" & txtAccepted.Text.Replace("'", "''") & "',dbo.getMTDate()")
        UpdateTable("INSERT INTO Keyword_Changes (Scorecard, Update_By, Update_Type, Keyword_To, Problem_To, Utterance_Type_To, Accepted_Variant_To) VALUES (" & ddlScorecard.SelectedValue & ",'" & User.Identity.Name & "','Add','" & txtNew.Text.Replace("'", "''") & "', '" & ddlProblem.SelectedValue & "', '" & ddlNewType.SelectedValue & "','" & txtAccepted.Text.Replace("'", "''") & "')")
        gvKeywords.DataBind()
    End Sub

    Private Sub ddlScorecard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlScorecard.SelectedIndexChanged
        gvKeywords.DataBind()
        If ddlScorecard.SelectedIndex > 0 Then
            hlChangelog.Visible = True
            'hlExport.Visible = True
            hlChangelog.NavigateUrl = "./keyword_updates.aspx?Scorecard=" & ddlScorecard.SelectedValue
        Else
            'hlExport.Visible = False
            hlChangelog.Visible = False
        End If
    End Sub

    Private Sub keywords_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            dsNICards.SelectParameters("username").DefaultValue = User.Identity.Name
        End If
        dsKeywords.DeleteParameters("Username").DefaultValue = User.Identity.Name
        dsKeywords.UpdateParameters("Username").DefaultValue = User.Identity.Name
    End Sub
End Class
