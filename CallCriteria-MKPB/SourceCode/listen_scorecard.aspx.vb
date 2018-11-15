Imports System.Data.SqlClient
Imports System.Data

Imports Common
Imports System.IO
Imports System.Net

Partial Class listen3
    Inherits System.Web.UI.Page
    Dim category As String = ""
    Dim section As String = ""
    Public audio_file As String = ""
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim user_info_dt As DataTable = GetTable("select * from UserExtraInfo where username = '" & HttpContext.Current.User.Identity.Name & "'")
        If user_info_dt.Rows.Count > 0 Then
            data_rate = user_info_dt.Rows(0).Item("speed_increment") / 100
            hdnSpeedLimit.Value = user_info_dt.Rows(0).Item("speed_limit").ToString
        Else
            data_rate = 0.05
        End If


        If Not User.Identity.IsAuthenticated Then
            Response.Redirect("login.aspx?ReturnURL=listen.aspx")
        End If

        'Disable button on click, still allow server processing

        dsSC.SelectParameters("username").DefaultValue = User.Identity.Name

        Dim mu As MembershipUser = Membership.GetUser(User.Identity.Name)

        Dim dt2 As DataTable
        dt2 = GetTable("select top 1 * from dbo.XCC_REPORT_NEW b  with (nolock) where left(audio_link,6) = '/audio' and scorecard = '" & ddlScorecard.SelectedValue & "' and id < '" & hdnMaxID.Value & "'  order by b.id desc")

        If dt2.Rows.Count > 0 Then

            hdnMaxID.Value = dt2.Rows(0).Item("id").ToString

            hdnCampaign.Value = dt2.Rows(0).Item("campaign").ToString
            hdnThisScorecard.Value = dt2.Rows(0).Item("scorecard").ToString
            ' hdnAutoSubmit.Value = dt2.Rows(0).Item("auto_submit").ToString

            litPersonal.Text = ""
            litOtherData.Text = ""

            If dt2.Rows.Count > 0 Then
                'Personal
                litPersonal.Text = litPersonal.Text & getSidebarData("First Name", dt2.Rows(0).Item("First_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Last Name", dt2.Rows(0).Item("Last_Name").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Email", dt2.Rows(0).Item("Email").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Education Level", dt2.Rows(0).Item("EducationLevel").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("High School Grad Year", dt2.Rows(0).Item("HighSchoolGradYear").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Degree Start Timeframe", dt2.Rows(0).Item("DegreeStartTimeframe").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Campaign", dt2.Rows(0).Item("Campaign").ToString())

                'Contact
                litPersonal.Text = litPersonal.Text & getSidebarData("Address", dt2.Rows(0).Item("address").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("City", dt2.Rows(0).Item("City").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("State", dt2.Rows(0).Item("State").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Zip", dt2.Rows(0).Item("Zip").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Phone", dt2.Rows(0).Item("Phone").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Session", dt2.Rows(0).Item("Session_id").ToString())
                litPersonal.Text = litPersonal.Text & getSidebarData("Call Date", CDate(dt2.Rows(0).Item("call_date")).ToShortDateString)
                litPersonal.Text = litPersonal.Text & getSidebarData("Disposition", dt2.Rows(0).Item("Disposition").ToString())



                If dt2.Rows(0).Item("text_only").ToString() <> "" Then
                    litTextOnly.Text = dt2.Rows(0).Item("text_only").ToString()
                    divHiddenText.Visible = True
                End If

                litSchool.Text = ""

                Dim sch_dt As DataTable = GetTable("select distinct School,AOI1,AOI2,L1_SubjectName,L2_SubjectName,Modality,College,DegreeOfInterest,origin,tcpa from School_X_Data where xcc_id = " & dt2.Rows(0).Item("ID").ToString)
                Dim sch_x As Integer = 1
                For Each sch_dr In sch_dt.Rows
                    litSchool.Text = litSchool.Text & " <tr><td class='school-name' colspan='2'>" & Replace(sch_dr.Item("School").ToString(), "'", "''") & "</td></tr>"

                    litSchool.Text = litSchool.Text & getSidebarData("College", sch_dr.Item("College").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Degree", sch_dr.Item("DegreeOfInterest").ToString())

                    'litSchool.Text = litSchool.Text & getSidebarData("School " & sch_x, sch_dr.Item("School").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 1", sch_dr.Item("AOI1").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Area 2", sch_dr.Item("AOI2").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 1", sch_dr.Item("L1_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Subject 2", sch_dr.Item("L2_SubjectName").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Modality", sch_dr.Item("Modality").ToString())
                    litSchool.Text = litSchool.Text & getSidebarData("Portal", sch_dr.Item("origin").ToString())

                    Dim other_dt As DataTable = GetTable("select distinct data_key, data_value from (select top 100 percent data_key, data_value  from  otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString & " and school_name = '" & Replace(sch_dr.Item("School").ToString(), "'", "''") & "' and data_key not in (select val from  dbo.split((select hide_data from scorecards where id = '" & dt2.Rows(0).Item("scorecard") & "'),'|')) order by id) a")
                    For Each other_dr In other_dt.Rows
                        litSchool.Text = litSchool.Text & getSidebarData(other_dr.item("data_key").ToString(), other_dr.item("data_value").ToString() & " ")
                    Next
                    sch_x += 1
                Next


            Else
                ' liSchoolItem.Visible = False
            End If
            litOtherData.Text = ""
            'Dim od_dt As DataTable = GetTable("select * from otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString & "  and data_type <> 'School' and isnull(school_name,'') = ''  and data_key not in (select val from  dbo.split((select hide_data from scorecards where id = '" & dt2.Rows(0).Item("scorecard") & "'),'|')) order by id")
            Dim od_dt As DataTable = GetTable("select distinct data_key, data_value from (select top 100 percent data_key, data_value  from  otherFormData with  (nolock)  where xcc_id = " & dt2.Rows(0).Item("ID").ToString & "  and data_type <> 'School' and isnull(school_name,'') = '' and data_key not in (select val from  dbo.split((select hide_data from scorecards where id = '" & dt2.Rows(0).Item("scorecard") & "'),'|')) order by id) a")
            If od_dt.Rows.Count > 0 Then
                divOtherCard.Visible = True
                For Each dr As DataRow In od_dt.Rows
                    litOtherData.Text = litOtherData.Text & getSidebarData(dr("data_key").ToString(), dr("data_value").ToString() & " ")
                Next
            End If



            hdnXCCID.Value = dt2.Rows(0).Item("ID").ToString
            hdnThisApp.Value = dt2.Rows(0).Item("appname").ToString



            Dim session_id As String = dt2.Rows(0).Item("SESSION_ID").ToString
            ' lblSession.Text = session_id.ToString

            audio_file = GetAudioFileName(dt2.Rows(0))

            'Dim this_filename As String = GetAudioFileName(dt2.Rows(0))



        End If




    End Sub


    Public data_rate As String
    Public app_list As String

    Protected Sub CheckForPanel(sender As Object, e As RepeaterItemEventArgs)
        Dim rptitem As RepeaterItem = e.Item
        Dim lit As Literal = rptitem.FindControl("Literal1")
        Dim drv As DataRowView = rptitem.DataItem

    End Sub

    Private Function getValueOrNA(p1 As String) As String

        If [String].IsNullOrEmpty(p1) Then
            Return "NA"
        Else
            Return p1
        End If
    End Function


    Protected Function getSidebarData(lbl As String, value As String) As String
        If value <> "" Then
            'Return "<div><label>" & lbl & ":</label><span>" & value & "</span></div>"

            Return "<tr><td class='info-label'>" & lbl & "</td><td class='info-data'>" & value & "</td></tr>"

        Else
            Return ""
        End If

    End Function

    Protected Sub txtClickTime_DataBinding(sender As Object, e As EventArgs)
        Dim txt As TextBox = sender
        txt.Attributes.Add("readonly", "readonly")
    End Sub

    Protected Sub chkOption_DataBinding(sender As Object, e As EventArgs)
        Dim chk As CheckBox = sender
        Dim ri As DataRowView = sender.parent.dataitem
        chk.InputAttributes.Add("data-comment-name", ri("Value").ToString)
    End Sub

    Protected Sub txtCheckTime_DataBinding(sender As Object, e As EventArgs)
        Dim txt As TextBox = sender
        txt.Attributes.Add("readonly", "readonly")
    End Sub

    Protected Sub rblOptions_DataBound(sender As Object, e As EventArgs)
        Dim rbl As RadioButtonList = sender
        rbl.Items.Add(New ListItem("", "0"))
    End Sub

    Protected Sub chkOptions_DataBound(sender As Object, e As EventArgs)
        Dim chk As CheckBoxList = sender
        chk.Items.Add(New ListItem("", "0"))
    End Sub
End Class

