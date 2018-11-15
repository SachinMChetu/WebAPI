Imports Common
Imports System.Data


Partial Class CC_Master
    Inherits System.Web.UI.MasterPage

    Public agenda_items As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Response.AppendHeader("Access-Control-Allow-Origin", "*")
        '                        </td> <td class="headerLink"><a href="">Billing</a></td>
        If Not IsPostBack Then




            ' Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, "OnSubmitScript", "return handleSubmit()")

            Dim domain() As String = Request.ServerVariables("SERVER_NAME").Split(".")
            If domain(1).ToLower = "callcriteria" Or domain(1).ToLower = "callcriteria-dev" Then
                If Me.Page.Title = "" Then
                    Me.Page.Title = "Call Criteria"
                End If
                favIcon.Visible = True
                favIconX.Visible = True

            Else
                Me.Page.Title = "QA Tool"
                favIcon.Visible = False
                favIconX.Visible = False
            End If

            'Populate user data

            Dim isQS As Boolean = False

            Dim qs_user As DataTable = GetTable("select * From userextrainfo with (nolock)   join userapps with (nolock)   on userapps.username = userextrainfo.username where appname = 'qs' and userapps.username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' and user_role in ('Supervisor', 'Client','Manager','Agent')")

            If qs_user.Rows.Count > 0 Then
                isQS = True
            End If


            Dim isGraspy As Boolean = False
            Dim graspy_user As DataTable = GetTable("select * From userextrainfo with (nolock)   join userapps with (nolock)   on userapps.username = userextrainfo.username where appname = 'graspy' and userapps.username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' and user_role in ('Supervisor', 'Client','Manager')")

            If graspy_user.Rows.Count > 0 Then
                isGraspy = True
            End If



            Dim user_dt As DataTable = GetTable("select *, (select count(*) from userapps with (nolock)   join scorecards with (nolock)   on scorecards.ID = userapps.user_scorecard  where username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' and ni_scorecard = 1) as ni_cards  from userextrainfo with (nolock)   where username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "'")
            If user_dt.Rows.Count > 0 Then






                litUser.Text = user_dt.Rows(0).Item("username").ToString
                litRole.Text = user_dt.Rows(0).Item("user_role").ToString

                Dim links_dt As DataTable = GetTable("exec getMyMenu '" & HttpContext.Current.User.Identity.Name & "'")


                For Each dr In links_dt.Rows
                    litLinks.Text &= "<td class='headerLink'><a href='" & dr("url").ToString & "'>" & dr("link").ToString & "</a>"
                    If dr("span_data").ToString <> "" Then
                        litLinks.Text &= "<span id ='" & dr("link").ToString.ToLower & "_count'></span>"
                    End If
                    litLinks.Text &= "</td>"
                Next

                Try

                    If user_dt.Rows(0).Item("ni_cards") > 0 And Not isQS And {"Admin", "Client", "Supervisor", "Manager"}.Contains(Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Single) Then
                        litLinks.Text &= "<td class='headerLink'><a href='ni_data.aspx'>NI Data</a></td>"
                    End If
                Catch ex As Exception

                End Try




                If Roles.IsUserInRole("QA") Then
                    Dim myPay As DataTable = GetTable("exec getPay2SC '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "','" & DateAdd(DateInterval.Day, -Today.DayOfWeek + 6, Today).ToShortDateString & "'")

                    Dim total_pay As Single = 0
                    Dim base_pay As String = "0"
                    Dim start_date As String = ""
                    Dim num_by_qal As Integer = 0


                    For Each dr In myPay.Rows
                        base_pay = dr("base").ToString
                        start_date = dr("startdate").ToString


                        Dim new_base As Single = 0

                        If IsNumeric(dr("efficiency").ToString) Then
                            new_base = dr("base") * (100 + dr("cal_percent").ToString) * FormatNumber(((dr("efficiency") - 100) / 2 + 100), 2) / 100 / 100
                        End If

                        Dim rev_time() As String = dr("reviewtime").ToString.Split(":")
                        If rev_time.Length = 3 Then
                            total_pay += new_base * (rev_time(0) + rev_time(1) / 60 + rev_time(2) / 3600) - dr("base") * 0.2 * dr("num_disputes") + dr("websites") * 0.2
                        End If
                    Next
                    litLinks.Text &= "<td class='headerLink'><a name='mypay'>" & FormatCurrency(total_pay) & "</a></td>"



                    Dim strike_count_dt As DataTable = GetTable("select count(*) from vwForm with (nolock) where f_id in (select top 15 f_id from vwForm  with (nolock) where reviewer = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' order by f_id desc) and whisperID <> QAWhisper and QAWhisper <> 0 and call_length > 60 and isnull(call_length_truncated,0) = 0 and  isnull(bad_call,0) = 0 ")




                    litLinks.Text &= "</td>"
                    If strike_count_dt.Rows(0).Item(0) > 0 Then
                        litLinks.Text &= "<td class='headerLink'>"

                        For x = 1 To strike_count_dt.Rows(0).Item(0)
                            litLinks.Text &= "<img title='Whisper Strikes' src='/img/red_strike.png' />"
                        Next
                    End If

                    If strike_count_dt.Rows(0).Item(0) > 2 Then

                        UpdateTable("exec email3strikes '" & HttpContext.Current.User.Identity.Name & "'")

                        'Try
                        '    Email_Error(HttpContext.Current.User.Identity.Name & " hit 3 strikes on Whisper within 15 calls", "chad@callcriteria.com," & strike_count_dt.Rows(0).Item(1).ToString)
                        'Catch ex As Exception
                        '    Email_Error(HttpContext.Current.User.Identity.Name & " hit 3 strikes on Whisper within 15 calls", "chad@callcriteria.com")
                        'End Try

                    End If

                End If


                Dim badge_dt As DataTable = GetTable("exec getBadges '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "'")

                If badge_dt.Rows(0).Item("agenda").ToString <> "0" Then
                    agenda_items &= "show_agenda(" & badge_dt.Rows(0).Item("agenda") & ");"
                End If

                If badge_dt.Rows(0).Item("guidelines").ToString <> "0" Then
                    agenda_items &= "show_guidelines(" & badge_dt.Rows(0).Item("guidelines") & ");"
                End If

                '' Populate Agenda count
                'Dim agenda_dt As DataTable = GetTable("Select count(*) from agenda_item With (nolock)   " &
                '        "Join agenda_topics With (nolock)   On agenda_item.id = agenda_topics.agenda_id " &
                '        "join scorecards With (nolock)   On scorecards.ID = sc_id " &
                '        "join userapps With (nolock)   On userapps.user_scorecard = scorecards.ID " &
                '        "left join agenda_topic_views With (nolock)   On agenda_topics.id = agenda_topic_views.topic_id And userapps.username = who " &
                '        "where scorecards.active = 1 And userapps.username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' and agenda_topics.date_completed is null")
                'If agenda_dt.Rows.Count > 0 Then
                '    If agenda_dt.Rows(0).Item(0) > 0 Then
                '        'some JS to update agenda_count object
                '        agenda_items &= "show_agenda(" & agenda_dt.Rows(0).Item(0) & ");"
                '    End If
                'End If


                'Dim guide_dt As DataTable = GetTable("select isnull(sum(case when isnull(date_reviewed,'1/1/2010') < max_date  or isnull(date_reviewed,'1/1/2010') < max_date_f  then 1 else '' end),0) as qu_class  from scorecards with (nolock)    left join (select sc_id, max(date_reviewed) as date_reviewed from sc_update with (nolock)   where reviewer = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "' group by sc_id ) a on a.sc_id = scorecards.id  left join (select max(dateadded) as max_date, scorecard_id from q_instructions with (nolock)   join questions with (nolock)   on questions.ID = q_instructions.question_id group by scorecard_id) b on b.scorecard_id = scorecards.id     left join (select max(dateadded) as max_date_f, scorecard_id from q_faqs with (nolock)   join questions with (nolock)   on questions.ID = q_faqs.question_id group by scorecard_id) c on c.scorecard_id = scorecards.id where scorecards.id in (select user_scorecard from userapps with (nolock)   where username = '" & HttpContext.Current.User.Identity.Name.Replace("'", "''") & "') and active =1 ")

                'If guide_dt.Rows.Count > 0 Then

                '    If guide_dt.Rows(0).Item(0) > 0 Then
                '        agenda_items &= "show_guidelines(" & guide_dt.Rows(0).Item(0) & ");"
                '    End If

                'End If

            End If
        End If

    End Sub
    Protected Sub btnGo_Click(sender As Object, e As EventArgs)
        Dim alt_user As String = CType(lvAltUser.FindControl("new_user"), TextBox).Text

        Dim user_dt As DataTable = GetTable("select * from userextrainfo with (nolock)   where username = '" & alt_user.Replace("'", "''") & "'")

        If user_dt.Rows.Count > 0 Then
            UserImpersonation.ImpersonateUser(alt_user, Page.Request.FilePath)
            Dim cookie As HttpCookie = Request.Cookies("filter")
            If cookie IsNot Nothing Then
                cookie.Expires = DateTime.Now.AddYears(-1)
                HttpContext.Current.Response.Cookies.Add(cookie)
            End If

            Response.Redirect(Page.Request.FilePath)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "nouser", "alert('User does not exist');", True)
        End If
    End Sub


End Class

