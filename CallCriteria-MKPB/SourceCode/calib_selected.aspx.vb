
Partial Class calib_selected
    Inherits System.Web.UI.Page

    Public call_start As String
    Public call_end As String

    Private Sub calib_selected_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            txtEnd.Text = DateAdd(DateInterval.Day, -1, Today).ToShortDateString
            txtStart.Text = DateAdd(DateInterval.Day, -2, Today).ToShortDateString
        End If

        call_start = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(txtStart.Text)).ToShortDateString
        call_end = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(txtEnd.Text)).ToShortDateString

    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        'dsSelected.DataBind()
        'gvSelected.DataBind()
        dsCals.DataBind()
        dsRecal.DataBind()
        SqlDataSource1.DataBind()
        GridView1.DataBind()
        gvCals.DataBind()
        gvRecal.DataBind()
    End Sub

    Private Sub GridView1_DataBound(sender As Object, e As EventArgs) 'Handles gvNewSelected.DataBound
        'Common.Sum_Column(GridView1, 0)
        'Common.Sum_Column(GridView1, 2)
        'Common.Sum_Column(GridView1, 5)
        'Common.Sum_Column(GridView1, 6)
        'Common.Sum_Column(GridView1, 7)
        'Common.Sum_Column(GridView1, 9)
        'Common.Sum_Column(GridView1, 10)

        'Common.Avg_Column(GridView1, 3)
        'Common.Avg_Column(GridView1, 4)
        'Common.Avg_Column(GridView1, 8)
    End Sub
End Class
