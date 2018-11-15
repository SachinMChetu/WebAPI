Imports Common
Imports System.IO
Imports System.Data

Partial Class ApplicationForm
    Inherits System.Web.UI.Page

    Public Class Applicant
        Public id As String
        Public firstname As String
        Public lastname As String
        Public username As String
        Public password As String
        Public birthday As Date
        Public gender As String
        Public email As String
        Public cv As String
    End Class

    Public Class Question
        Public id As String
        Public question As String
        Public active As String
    End Class

    Public Class ApplicantsQnA
        Public id As String
        Public aid As String
        Public qid As String
        Public answer As String
    End Class

    Protected Sub submit_Click(sender As Object, e As EventArgs) Handles submit.Click

        Dim username, gender As String
        username = Trim(firstname.Value.Replace(" ", "")) & Trim(lastname.Value.Replace(" ", ""))

        'Decrypt password
        Dim hashpassword As String = password.Value

        If male.Checked Then gender = "Male" Else gender = "Female"
        'Response.Write(firstname.Value & " " & lastname.Value & " " & username & " " & password.Value & " " & bday.Value & " " & gender & " " & emailadd.Value & " " & file.FileName)

        'Save Applicant
        UpdateTable("INSERT INTO Applicants SELECT '" & firstname.Value & "', '" & lastname.Value & "', '" & username & "', '" & hashpassword & "', '" & bday.Value & "', '" & gender & "', '" & emailadd.Value & "', '" & File.FileName & "', dbo.getMTDate(), '1'")

        'Save CV
        Dim dt As DataTable = GetTable("SELECT IDENT_CURRENT('Applicants')")
        Dim exists As Boolean = System.IO.Directory.Exists(Server.MapPath("~/docs/" & dt.Rows(0).Item(0)))
        If Not exists Then
            System.IO.Directory.CreateDirectory(Server.MapPath("~/docs/" & dt.Rows(0).Item(0)))
        End If

        Dim filename As String = Path.GetFileName(file.FileName)
        file.SaveAs(Server.MapPath("~/docs/" & dt.Rows(0).Item(0)) & "/" & filename)

        For Each item As RepeaterItem In questions.Items
            Dim answer As TextBox = DirectCast(item.FindControl("Answer"), TextBox)
            Dim qid As String = answer.Attributes("qid")
            'Response.Write(qid & " " & answer.Text & ",")

            'Save Answers
            UpdateTable("INSERT INTO ApplicantsQnA SELECT '" & dt.Rows(0).Item(0) & "', '" & qid & "', '" & answer.Text & "'")
        Next

        Response.Redirect("ApplicationForm.aspx")

    End Sub

End Class
