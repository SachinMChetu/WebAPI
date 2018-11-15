Imports ClarityVoice

Partial Class getClarity
    Inherits System.Web.UI.Page

    Private Sub getClarity_Load(sender As Object, e As EventArgs) Handles Me.Load
        'callsourcetest Vial2291sksdk2
        Dim cv As New ClarityVoice.cpiSessionClient
        Dim cvSession As String = ""
        cv.sessionInit("callsourcetest", "Vial2291sksdk2", cvSession)


    End Sub
End Class
