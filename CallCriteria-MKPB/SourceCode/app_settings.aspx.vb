Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Common
Partial Class app_settings
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not User.IsInRole("Admin") Then Response.Redirect("default.aspx")


    End Sub



    Protected Sub ddlApps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApps.SelectedIndexChanged
        'dsSettings.DataBind()
        ''dsNotifications.DataBind()

        'If ddlApps.SelectedValue <> "" Then
        '    btnNewAPI.Visible = True
        'Else
        '    btnNewAPI.Visible = False
        'End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtNewApp.Text <> "" Then
            UpdateTable("insert into app_settings (appname) select '" & txtNewApp.Text & "'")
            Response.Redirect("app_settings.aspx")
        End If


    End Sub

    Private Sub dsSettings_Updated(sender As Object, e As SqlDataSourceStatusEventArgs) Handles dsSettings.Updated
        Dim fup As FileUpload = fvSettings.FindControl("fupLogo")

        If fup.FileName <> "" Then


            Try
                IO.Directory.CreateDirectory(Server.MapPath("/audio/" & ddlApps.SelectedValue & "/"))
            Catch ex As Exception

            End Try


            Dim originalBMP As New Bitmap(fup.FileContent)

            ' Calculate the new image dimensions
            Dim origWidth As Integer = originalBMP.Width
            Dim origHeight As Integer = originalBMP.Height
            Dim sngRatio As Single = origWidth / origHeight
            Dim newWidth As Integer = 250
            Dim newHeight As Integer = newWidth / sngRatio

            ' Create a new bitmap which will hold the previous resized bitmap
            Dim newBMP As New Bitmap(originalBMP, newWidth, newHeight)
            ' Create a graphic based on the new bitmap
            Dim oGraphics As Graphics = Graphics.FromImage(newBMP)

            ' Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
            ' Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight)

            ' Save the new graphic file to the server
            newBMP.Save(Server.MapPath("/audio/" & ddlApps.SelectedValue & "/" & fup.FileName))

            ' Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose()
            newBMP.Dispose()
            oGraphics.Dispose()

            UpdateTable("update app_settings set client_logo = '/audio/" & ddlApps.SelectedValue & "/" & fup.FileName & "' where appname = '" & ddlApps.SelectedValue & "'")

        End If

        Dim fupsmall As FileUpload = fvSettings.FindControl("fupLogoSmall")

        If fupsmall.FileName <> "" Then


            Try
                IO.Directory.CreateDirectory(Server.MapPath("/audio/" & ddlApps.SelectedValue & "/"))
            Catch ex As Exception

            End Try


            Dim originalBMP As New Bitmap(fupsmall.FileContent)

            ' Calculate the new image dimensions
            Dim origWidth As Integer = originalBMP.Width
            Dim origHeight As Integer = originalBMP.Height
            Dim sngRatio As Single = origWidth / origHeight
            Dim newWidth As Integer = 250
            Dim newHeight As Integer = newWidth / sngRatio

            ' Create a new bitmap which will hold the previous resized bitmap
            Dim newBMP As New Bitmap(originalBMP, newWidth, newHeight)
            ' Create a graphic based on the new bitmap
            Dim oGraphics As Graphics = Graphics.FromImage(newBMP)

            ' Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
            ' Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight)

            ' Save the new graphic file to the server
            newBMP.Save(Server.MapPath("/audio/" & ddlApps.SelectedValue & "/" & fupsmall.FileName))

            ' Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose()
            newBMP.Dispose()
            oGraphics.Dispose()

            UpdateTable("update app_settings set client_logo_small = '/audio/" & ddlApps.SelectedValue & "/" & fupsmall.FileName & "' where appname = '" & ddlApps.SelectedValue & "'")

        End If

    End Sub
    Protected Sub btnAddNote_Click(sender As Object, e As EventArgs)
        Dim txtNewNote As TextBox = fvSettings.FindControl("txtNewNote")
        UpdateTable("insert into app_notes (appname,note) select '" & ddlApps.SelectedValue & "','" & txtNewNote.Text.Replace("'", "''") & "'")

        Dim gvAppNotes As GridView = fvSettings.FindControl("gvAppNotes")
        gvAppNotes.DataBind()
    End Sub



    'Private Sub btnSaveProduction_Click(sender As Object, e As EventArgs) Handles btnSaveProduction.Click

    '    Dim WebClient_down As New System.Net.WebClient()
    '    WebClient_down.DownloadFile("https://lasoft-review-app.herokuapp.com/v" & txtVer.Text & "/bundle.js", Server.MapPath("\bundle\bundle.js"))
    '    lblUploadResult.Text = "Uploaded"
    'End Sub
    Protected Sub btnAddRole_Click(sender As Object, e As EventArgs)
        Dim ds As SqlDataSource = sender.parent.findcontrol("dsNotiRules")
        Dim gvNotiRules As GridView = sender.parent.findcontrol("gvNotiRules")
        ds.Insert()
        ds.DataBind()
        gvNotiRules.DataBind()
    End Sub

    Protected Sub btnNewBillRate_Click(sender As Object, e As EventArgs)
        Dim ds As SqlDataSource = sender.parent.findcontrol("dsBillRate")
        Dim gvBillRate As GridView = sender.parent.findcontrol("gvBillRate")
        ds.Insert()
        ds.DataBind()
        gvBillRate.DataBind()
    End Sub

    Protected Sub btnNewAPI_Click(sender As Object, e As EventArgs)

        Dim dsAPI As SqlDataSource = sender.parent.findcontrol("dsAPI")
        Dim gvAPI As GridView = sender.parent.findcontrol("gvAPI")
        dsAPI.Insert()
        gvAPI.DataBind()
    End Sub


    Protected Sub btnAddExport_Click(sender As Object, e As EventArgs)
        Dim dsAppFields As SqlDataSource = sender.parent.findcontrol("dsAppFields")
        Dim gvAppFields As GridView = sender.parent.findcontrol("gvAppFields")
        dsAppFields.Insert()
        gvAppFields.DataBind()
    End Sub

End Class
