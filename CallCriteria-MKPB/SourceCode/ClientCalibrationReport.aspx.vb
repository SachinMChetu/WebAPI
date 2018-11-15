
Partial Class ClientCalibrationReport
    Inherits System.Web.UI.Page

    Private Sub gvCalibrations_Load(sender As Object, e As EventArgs) Handles gvCalibrations.Load
        dsCalibrations.SelectParameters("username").defaultvalue = User.Identity.Name
    End Sub
End Class
