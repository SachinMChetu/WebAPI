Imports Common
Imports System.Data
Partial Class bitwage
    Inherits System.Web.UI.Page

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click

        Dim dt As DataTable = GetTable("getPayCSV '" & lbWEDate.SelectedValue & "'")

        Dim pay_dt As New DataTable

        pay_dt.Columns.Add("QA")
        pay_dt.Columns.Add("StartDate")
        pay_dt.Columns.Add("Hourly")
        pay_dt.Columns.Add("CaliPercent")
        pay_dt.Columns.Add("Calibration")
        pay_dt.Columns.Add("# Calibrations")
        pay_dt.Columns.Add("Efficiency")
        pay_dt.Columns.Add("Disputes")
        pay_dt.Columns.Add("Call")
        pay_dt.Columns.Add("Review")
        pay_dt.Columns.Add("CallSpeed")
        pay_dt.Columns.Add("Base")

        pay_dt.Columns.Add("Paid")
        pay_dt.Columns.Add("GrossPay")
        pay_dt.Columns.Add("Deduction")
        pay_dt.Columns.Add("NetPay")
        pay_dt.Columns.Add("CPM")
        pay_dt.Columns.Add("email")

        For Each dr As DataRow In dt.Rows



            'Dim dt As DataTable = GetTable("getPay3 '" & dr("reviewer") & "','" & lbWEDate.SelectedValue & "','" & domain_selected & "'")
            Dim new_dr As DataRow = pay_dt.NewRow
            new_dr("QA") = dr("reviewer")

            'If dt.Rows.Count > 0 Then
            'new_dr("StartDate") = Left(dr.Item("startdate"), dr.Item("startdate").ToString.IndexOf(" "))
            If Not IsDBNull(dr.Item("base")) Then
                new_dr("Hourly") = FormatCurrency(dr.Item("base"), 2)
            End If
            new_dr("email") = dr.Item("email")
            new_dr("Review") = dr.Item("reviewtime")
            new_dr("Call") = dr.Item("calltime")
            new_dr("Efficiency") = dr.Item("eff_percent")
            new_dr("Disputes") = dr.Item("num_disputes")
            new_dr("# Calibrations") = dr.Item("num_calibrations")
            'If dr.Item("num_calibrations") > 4 Then
            new_dr("Calibration") = FormatNumber(dr.Item("calibration_score"), 2)
            new_dr("CaliPercent") = dr.Item("cal_percent")
            'Else
            '    new_dr("Calbration") = "Pending"
            '    new_dr("CaliPercent") = "0"
            'End If

            Dim reviews() As String = dr.Item("reviewtime").ToString.Split(":")
            Dim calls() As String = dr.Item("calltime").ToString.Split(":")


            Dim ct As TimeSpan = New TimeSpan(reviews(0), reviews(1), reviews(2))

            Dim rt2 As TimeSpan = New TimeSpan(calls(0), calls(1), calls(2))
            Dim rt As TimeSpan = New TimeSpan(calls(0), calls(1), calls(2)) '+ New TimeSpan(0, ct.TotalMinutes * 0.0556, 0)

            new_dr("CallSpeed") = FormatPercent(rt2.TotalSeconds / ct.TotalSeconds, 2)



            'If DateDiff(DateInterval.Day, CDate("8/1/2014"), CDate(lbWEDate.SelectedValue)) > 0 Then

            Dim new_eff As Single = ((dr.Item("efficiency") - 100) / 2 + 100)

                Dim deduct As Single
                If new_dr("Disputes").ToString = "" Then
                    deduct = 0
                Else
                    deduct = new_dr("Disputes") * 0.4
                End If


                Dim new_cal As Single = 0
                If dr.Item("cal_percent") <> "0" Then
                    new_cal = dr.Item("cal_percent") / 100
                End If

                new_dr("Base") = FormatCurrency(dr.Item("base") * new_eff / 100 * (1 + new_cal), 2)
                new_dr("Efficiency") = FormatNumber(new_eff, 2) & "%"
                new_dr("Paid") = dr.Item("reviewtime").ToString ' CInt(rt.TotalHours) & ":" & rt.Minutes.ToString("00") & ":" & rt.Seconds.ToString("00")
                new_dr("GrossPay") = FormatCurrency(new_dr("Base").Replace("$", "") * (ct.TotalSeconds / 3600), 2)

                new_dr("CPM") = FormatCurrency((new_dr("Base").Replace("$", "") * (ct.TotalSeconds / 3600) - deduct) / rt2.TotalMinutes, 3)



            'Else
            'new_dr("Base") = FormatCurrency(dr.Item("base") * (dr.Item("eff_percent") + dr.Item("cal_percent")) / 100, 2)

            'new_dr("Paid") = dr.Item("calltime").ToString ' CInt(rt.TotalHours) & ":" & rt.Minutes.ToString("00") & ":" & rt.Seconds.ToString("00")
            'new_dr("GrossPay") = FormatCurrency(new_dr("Base").Replace("$", "") * (rt.TotalSeconds / 3600), 2)

            'new_dr("CPM") = FormatCurrency(new_dr("Base").Replace("$", "") * (rt.TotalSeconds / 3600) / rt2.TotalMinutes, 3)

            'End If
            If Not IsDBNull(new_dr("Disputes")) Then
                new_dr("Deduction") = FormatCurrency(new_dr("Disputes") * 0.4, 2)
            Else
                new_dr("Deduction") = 0
            End If
            new_dr("NetPay") = ""


            Response.Write(new_dr("QA") & "," & new_dr("email") & "," & FormatCurrency(new_dr("Base").Replace("$", "") * (ct.TotalSeconds / 3600) - new_dr("Deduction"), 2) & "<br>")



            'End If
            pay_dt.Rows.Add(new_dr)

        Next

    End Sub
End Class
