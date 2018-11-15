<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_scorecard.aspx.vb" Inherits="edit_scorecard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button ">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <h2>Edit Scorecard</h2>
                Scorecard:
        <asp:DropDownList ID="ddlSC" DataSourceID="dsApps" DataTextField="scorecard" DataValueField="id" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
            <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
        </asp:DropDownList>
                <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="select id,appname + ' - ' + short_name as scorecard, appname  from scorecards where 
                        id in (select distinct scorecards.id from scorecards left join userapps on userapps.user_scorecard = scorecards.id 
                        where 1 = case when (select user_role from userextrainfo where username = @username) = 'Admin' then 1 
                        when (select user_role from userextrainfo where username = @username) != 'Admin' and userapps.username = @username then 1 else 0 end ) 
                        order by active desc, appname, short_name "
                    runat="server">
                    <SelectParameters>
                        <asp:Parameter Name="username" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Button ID="btnDedupe" runat="server" Text="Dedupe Scorecard" /><asp:Label ID="lblDedupe" ForeColor="Red" runat="server" Text=""></asp:Label>


                <span>
                    <a href='scorecard_q_manager.aspx?ID=<%=Request("id") %>'>Questions</a> | 
                            <a href='scorecard_section_manager.aspx?ID=<%=Request("id") %>'>Sections</a> | 
                            <a href='clerked_data.aspx?ID=<%=Request("id") %>'>Clerking</a> | 
                            <a href='counts.aspx?ID=<%=Request("id") %>'>Counts</a>
                </span>


                <asp:FormView ID="FormView1" runat="server" RenderOuterTable="false" DataKeyNames="id" DataSourceID="dsScorecard" DefaultMode="Edit">
                    <EditItemTemplate>

                        <table class="detailsTable">
                            <tr>
                                <td colspan="6"><strong>General</strong></td>
                            </tr>
                            <tr>
                                <td>Scorecard ID</td>
                                <td>
                                    <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                                </td>


                                <td>Added</td>
                                <td>
                                    <asp:Label ID="date_addedTextBox" ReadOnly="true" runat="server" Text='<%# Format(Eval("date_added"), "MM/dd/yyyy") %>' />
                                    <br />
                                    <asp:Label ID="who_addedTextBox" ReadOnly="true" runat="server" Text='<%# Eval("who_added") %>' />
                                </td>

                                <td>Active<br />
                                    On Hold</td>
                                <td>
                                    <asp:CheckBox ID="activeCheckBox" runat="server" Checked='<%# Bind("active") %>' /><br />
                                    <asp:CheckBox ID="onholdCheckbox" runat="server" Checked='<%# Bind("onhold") %>' />
                                </td>

                            </tr>

                            <tr>

                                <td>Short Name</td>
                                <td>
                                    <asp:TextBox ID="short_nameTextBox" runat="server" Width="200" Text='<%# Bind("short_name") %>' />
                                </td>

                                <td>Appname</td>
                                <td>
                                    <asp:DropDownList ID="dsMyApps" runat="server" AppendDataBoundItems="true" DataSourceID="dsApps" SelectedValue='<%# Bind("appname")%>' DataTextField="appname" DataValueField="appname">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Description</td>
                                <td>
                                    <asp:TextBox ID="descriptionTextBox" runat="server" Text='<%# Bind("description") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>Sort Order</td>
                                <td>
                                    <asp:DropDownList ID="ddlSortOrder" SelectedValue='<%# Bind("sc_sort") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>asc</asp:ListItem>
                                        <asp:ListItem>desc</asp:ListItem>
                                    </asp:DropDownList>
                                </td>



                                <td>Review Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlReviewType" SelectedValue='<%# Bind("review_type") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>audio</asp:ListItem>
                                        <asp:ListItem>website</asp:ListItem>
                                        <asp:ListItem>chat</asp:ListItem>
                                        <asp:ListItem>email</asp:ListItem>
                                        <asp:ListItem>video</asp:ListItem>
                                        <asp:ListItem>text</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>No Agent Logins</td>
                                <td>
                                    <asp:CheckBox ID="chkNoAgentLogin" runat="server" Checked='<%# Bind("no_agent_login") %>' />
                                </td>


                            </tr>

                            <tr>
                                <td>Website/Chat/Item Cost</td>
                                <td>
                                    <asp:TextBox ID="website_costTextBox" runat="server" Width="50" Text='<%# Bind("website_cost") %>' />
                                </td>
                                <td>Website Display</td>
                                <td>
                                    <asp:DropDownList ID="ddlwebsitedisplay" SelectedValue='<%# Bind("website_display") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>iFrame</asp:ListItem>
                                        <asp:ListItem>Pop-up</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>QA/QA Card
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkisQAQACard" runat="server" Checked='<%# Bind("isQAQACard") %>' />
                                    <asp:DropDownList ID="ddlQASC" DataSourceID="dsApps" DataTextField="scorecard" DataValueField="id" SelectedValue='<%# Bind("qa_qa_scorecard") %>' runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>

                                </td>

                            </tr>
                            <tr>
                                <td>Parent (question-less) Scorecard</td>
                                <td>
                                    <asp:DropDownList ID="ddlQLess" DataSourceID="dsApps" DataTextField="scorecard" DataValueField="id" SelectedValue='<%# Bind("qless_parent") %>' runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td>3rd Party (delete agents from scorecard)</td>
                                <td>
                                    <asp:CheckBox ID="chk3rdParty" runat="server" Checked='<%# Bind("third_party_scorecard") %>' /></td>
                                <td>Listen Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlListenType" SelectedValue='<%# Bind("listen_type") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Regular Listen" Value="regularlisten" ></asp:ListItem>
                                        <asp:ListItem Text="New Listen" Value="newListen" ></asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>

                                <td>Max Speed</td>
                                <td>
                                    <asp:TextBox ID="max_speedTextBox" runat="server" Width="50" Text='<%# Bind("max_speed") %>' />
                                </td>

                                <td>Scorecard Config Profile<br />
                                    If different than app</td>
                                <td>
                                    <asp:DropDownList ID="ddlProfile" DataSourceID="dsProfiles" DataTextField="profile_name" DataValueField="id" SelectedValue='<%#Bind("sc_profile") %>' runat="server" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsProfiles" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select id,profile_name from notification_profiles order by profile_name"
                                        runat="server"></asp:SqlDataSource>
                                </td>
                                <td>Rejection Reason Profile:<br />
                                    If different than app</td>
                                <td>
                                    <asp:DropDownList ID="ddlRejection" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsRejection"
                                        DataTextField="profile_name" DataValueField="id" SelectedValue='<%#Bind("rejection_profile") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsRejection" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select id, profile_name from rejection_profiles  order by profile_name" runat="server"></asp:SqlDataSource>
                                </td>


                            </tr>

                            <tr>
                            </tr>
                            <tr>

                                <td>Show Custom Questions</td>
                                <td>
                                    <asp:CheckBox ID="ni_show_custom_questions" runat="server" Checked='<%# Bind("show_custom_questions") %>' />
                                </td>
                                <td>Other Data sort order</td>

                                <td>
                                    <asp:DropDownList ID="ddlmeta_sort" runat="server" SelectedValue='<%# Bind("meta_sort") %>'>
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Alphabetical</asp:ListItem>
                                        <asp:ListItem>Order added</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Auto Accept Bad Calls</td>
                                <td>
                                    <asp:CheckBox ID="auto_accept_bad_call" runat="server" Checked='<%# Bind("auto_accept_bad_call") %>' />
                                </td>
                            </tr>

                            <tr>
                                <td>High Priority Scorecard</td>
                                <td>
                                    <asp:CheckBox ID="high_priority" runat="server" Checked='<%# Bind("high_priority") %>' /></td>
                                <td>Shift Start</td>
                                <td>
                                    <asp:TextBox ID="shift_startTextBox5" runat="server" Width="50" Text='<%# Bind("shift_start") %>' />
                                </td>
                                <td>Shift End 
                                </td>
                                <td>
                                    <asp:TextBox ID="shift_endTextBox6" runat="server" Width="50" Text='<%# Bind("shift_end") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>Pay Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPayType" SelectedValue='<%# Bind("pay_type") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Per Review Time</asp:ListItem>
                                        <asp:ListItem>Per Call Time</asp:ListItem>
                                        <asp:ListItem>Per Item</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Pay Amount -- 0.05 (fixed per item)  or 45 (base/items)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQA_pay" runat="server" Width="50" Text='<%# Bind("qa_pay") %>' />
                                </td>
                                <td>Dispute percent of pay
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdispute_base_percent" runat="server" Width="50" Text='<%# Bind("dispute_base_percent") %>' />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="6"><strong>Owners</strong></td>
                            </tr>

                            <tr>
                                <td>Account Manager</td>
                                <td>
                                    <asp:DropDownList ID="ddlAM" DataSourceID="dsAMS" DataTextField="Username" SelectedValue='<%#Bind("Account_manager") %>' DataValueField="Username" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsAMS" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select Username from userextrainfo where user_role in ('Admin','Account Manager') order by username "
                                        runat="server"></asp:SqlDataSource>
                                </td>
                                <td>Golden User</td>
                                <td>
                                    <asp:HiddenField ID="hdnThisApp" Value='<%# Eval("appname") %>' runat="server" />
                                    <asp:DropDownList ID="ddlGolden" DataSourceID="dsUsers" SelectedValue='<%# Bind("golden_user") %>' DataTextField="username" DataValueField="username" AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select distinct userapps.username from userapps join userextrainfo on userapps.username = userextrainfo.username where userapps.appname = @appname and user_role in ('supervisor','client','manager') order by userapps.username">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hdnThisApp" Name="appname" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>TL
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTL" DataSourceID="dsTL" SelectedValue='<%# Bind("team_lead") %>'
                                        DataTextField="username" DataValueField="username" AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsTL" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select distinct username from (select username from userextrainfo where user_role in ('QA Lead','Tango TL', 'Admin', 'Account Manager') 
                                        union all select distinct team_lead as username from scorecards) a where username != '' order by username"></asp:SqlDataSource>

                                </td>

                                <td>Tango TeamLead</td>
                                <td>
                                    <asp:DropDownList ID="ddlTangoTL" DataSourceID="dsTangoTL" SelectedValue='<%# Bind("tango_team_lead") %>'
                                        DataTextField="username" DataValueField="username" AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsTangoTL" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select distinct username from (select username from userextrainfo where user_role in ('QA Lead','Tango TL', 'Admin', 'Account Manager') 
                                        union all select distinct tango_team_lead as username from scorecards union all select distinct team_lead as username from scorecards) a where username != '' order by username"></asp:SqlDataSource>
                                </td>

                            </tr>

                            <tr>
                                <td colspan="6"><strong>Scoring</strong></td>
                            </tr>

                            <tr>
                                <td>Fail Score (fail if less than)</td>
                                <td>
                                    <asp:TextBox ID="fail_scoreTextBox" runat="server" Width="50" Text='<%# Bind("fail_score") %>' />
                                </td>
                                <td>Notification Score  (notification if less than)</td>
                                <td>
                                    <asp:TextBox ID="fail_sc_notification_score" runat="server" Width="50" Text='<%# Bind("sc_notification_score") %>' />
                                </td>
                                <td>Sectionless</td>
                                <td>
                                    <asp:CheckBox ID="sectionlessCheckBox" runat="server" Checked='<%# Bind("sectionless") %>' />
                                </td>

                            </tr>
                            <tr>

                                <td>Max Calls Per Day<br />
                                    Monthly Minute Cap</td>
                                <td>
                                    <asp:TextBox ID="txtmax_per_day" runat="server" Width="50" Text='<%# Bind("max_per_day") %>' />
                                    <br />
                                    <asp:TextBox ID="txtmonthly_minute_cap" runat="server" Width="50" Text='<%# Bind("monthly_minute_cap") %>' />
                                </td>


                                <td>Scoring Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlScoreType" SelectedValue='<%# Bind("score_type") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Question</asp:ListItem>
                                        <asp:ListItem>Section</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>NI Scorecard</td>
                                <td>
                                    <asp:CheckBox ID="ni_scorecardCheckBox" runat="server" Checked='<%# Bind("ni_scorecard") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>Hidden Data Elements (value1|values)</td>
                                <td>
                                    <asp:TextBox ID="hide_dataTextBox" runat="server" TextMode="MultiLine" Text='<%# Bind("hide_data") %>' /></td>
                                <td>Hidden School Elements (value1|values)</td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server" TextMode="MultiLine" Text='<%# Bind("hide_school_data") %>' /></td>
                            </tr>

                            <tr>
                                <td colspan="6"><strong>Import</strong></td>
                            </tr>
                            <tr>
                                <td>Import Type</td>
                                <td>

                                    <asp:DropDownList ID="DropDownList1" SelectedValue='<%# Bind("import_type") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Live</asp:ListItem>
                                        <asp:ListItem>Daily</asp:ListItem>
                                        <asp:ListItem>DailyCount</asp:ListItem>
                                        <asp:ListItem>DailyAgentCount</asp:ListItem>
                                        <asp:ListItem>DailyCampaign</asp:ListItem>
                                        <asp:ListItem>QAQA</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td>Import %/Count</td>
                                <td>
                                    <asp:TextBox ID="import_percentTextBox" Width="50" runat="server" Text='<%# Bind("import_percent") %>' />
                                </td>
                                <td>Import Agents</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="txtImportAgents" Checked='<%# Bind("import_agents") %>' /></td>


                            </tr>
                            <tr>
                                <td>Min Call Length(s)<br />
                                    Max Call Length(s)<br />
                                    -1 means ignore</td>
                                <td>
                                    <asp:TextBox ID="min_call_lengthTextBox" runat="server" Width="50" Text='<%# Bind("min_call_length") %>' /><br />
                                    <asp:TextBox ID="max_call_lengthTextBox" runat="server" Width="50" Text='<%# Bind("max_call_length") %>' />
                                </td>
                                <td>Truncate Calls(s)<br />
                                    Take first seconds:<br />
                                    Take last seconds:</td>
                                <td>
                                    <br />
                                    <asp:TextBox ID="truncate_timeTextBox" Width="50" runat="server" Text='<%# Bind("truncate_time") %>' /><br />
                                    <asp:TextBox ID="end_truncate_timeTextBox" Width="50" runat="server" Text='<%# Bind("end_truncate_time") %>' />
                                </td>

                                <td>Overwrite Agents Group<br />
                                    Dedupe After Import<br />
                                    Retain Unused Calls After Import
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chk_overwrite_group" Checked='<%# Bind("overwrite_group") %>' />
                                    <br />
                                    <asp:CheckBox ID="chkDedupe" runat="server" Checked='<%# Bind("dedupe") %>' />
                                    <br />
                                    <asp:CheckBox ID="chkretain" runat="server" Checked='<%# Bind("retain_non_used_calls") %>' />

                                </td>



                            </tr>
                            <tr>
                                <td>Delete unscored calls<br />
                                    after loaded days</td>
                                <td>
                                    <asp:TextBox ID="delete_after_daysTextBox" Width="50" runat="server" Text='<%# Bind("delete_after_days") %>' /></td>
                            </tr>

                            <tr>


                                <td>Transcripts<br />
                                    Redaction
                                    
                                </td>
                                <td>
                                    <asp:CheckBox ID="transcribeCheckBox" runat="server" Checked='<%# Bind("transcribe") %>' />Min Flags: 
                            <asp:TextBox ID="min_transcript_countTextBox" Width="50" runat="server" Text='<%# Bind("min_transcript_count") %>' /><br />
                                    <asp:CheckBox ID="chkRedact" runat="server" Checked='<%# Bind("redact") %>' />

                                </td>


                                <td>Required Dispositions</td>
                                <td>
                                    <asp:TextBox ID="required_dispositionsTextBox" runat="server" TextMode="MultiLine" Text='<%# Bind("required_dispositions") %>' /></td>

                                <td>Post Import SP</td>
                                <td>
                                    <asp:TextBox ID="post_import_spTextBox" runat="server" TextMode="MultiLine" Text='<%# Bind("post_import_sp") %>' /></td>

                            </tr>

                            <tr>
                                <td colspan="6"><strong>Calibration</strong></td>
                            </tr>


                            <tr>

                                <td>Calibrated</td>
                                <td>
                                    <asp:CheckBox ID="isCalibratedCheckBox" runat="server" Checked='<%# Bind("isCalibrated") %>' />
                                    <asp:TextBox ID="calib_percentTextBox" Width="50" runat="server" Text='<%# Bind("calib_percent") %>' />%
                            
                                </td>
                                <td>Recal Percent</td>
                                <td>
                                    <asp:TextBox ID="recal_percentTextBox" runat="server" Width="50" Text='<%# Bind("recal_percent") %>' />
                                </td>

                                <td>Calibrated Calls</td>
                                <td>
                                    <asp:DropDownList ID="ddlCalibRole" SelectedValue='<%# Bind("calib_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>

                                    </asp:DropDownList></td>
                            </tr>

                            <tr>
                                <td>QA Selected Calls</td>
                                <td>
                                    <asp:DropDownList ID="ddlqa_selected_role" SelectedValue='<%# Bind("qa_selected_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>
                                    </asp:DropDownList></td>

                                <td>Admin Selected Calls</td>
                                <td>
                                    <asp:DropDownList ID="ddladmin_selected_role" SelectedValue='<%# Bind("admin_selected_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>
                                    </asp:DropDownList></td>



                                <td>Client Selected Calls</td>
                                <td>
                                    <asp:DropDownList ID="ddlclient_selected_role" SelectedValue='<%# Bind("client_selected_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>

                            <tr>
                                <td>Recalibration Calls</td>
                                <td>
                                    <asp:DropDownList ID="ddlrecalib_role" SelectedValue='<%# Bind("recalib_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>
                                    </asp:DropDownList></td>

                                <td>Calibration Spotcheck Floor</td>
                                <td>
                                    <asp:TextBox ID="calibration_floorTextBox" runat="server" Width="50" Text='<%# Bind("calibration_floor") %>' />
                                </td>
                                <td>Cal Spotcheck Role</td>
                                <td>
                                    <asp:DropDownList ID="ddlcal_spot_user_role" SelectedValue='<%# Bind("cal_spot_user_role") %>' runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        <asp:ListItem>Calibrator</asp:ListItem>
                                        <asp:ListItem>QA Lead</asp:ListItem>
                                        <asp:ListItem>Teamlead</asp:ListItem>
                                        <asp:ListItem>Recalibrator</asp:ListItem>
                                        <asp:ListItem Text="Tango TL(Role)" Value="Tango TL"></asp:ListItem>
                                        <asp:ListItem>Tango TeamLead</asp:ListItem>
                                        <asp:ListItem>QA</asp:ListItem>
                                    </asp:DropDownList></td>

                            </tr>

                            <tr>
                                <td colspan="6"><strong>Training</strong></td>
                            </tr>

                            <tr>
                                <td>Training Count</td>
                                <td>
                                    <asp:TextBox ID="training_countTextBox" runat="server" Width="50" Text='<%# Bind("training_count") %>' />
                                </td>
                                <td>Passing Percent</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="50" Text='<%# Bind("pass_percent") %>' /></td>

                            </tr>


                            <tr>
                                <td>Check Calib count before kickoff</td>
                                <td>
                                    <asp:TextBox ID="num_cal_checkTextBox" runat="server" Text='<%# Bind("num_cal_check") %>' />
                                </td>
                                <td>Min Calib % before kickoff</td>
                                <td>
                                    <asp:TextBox ID="min_calTextBox" runat="server" Width="50" Text='<%# Bind("min_cal") %>' />
                                </td>
                            </tr>

                            <tr>

                                <td>Retrain Count</td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server" Width="50" Text='<%# Bind("cutoff_count") %>' /></td>
                                <td>Retrain Percent</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server" Width="50" Text='<%# Bind("cutoff_percent") %>' /></td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6"><strong>Email</strong></td>
                            </tr>

                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="gvEmails" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsEmails" runat="server">
                                        <Columns>
                                            <asp:CommandField ShowEditButton="true" />
                                            <asp:CheckBoxField DataField="email_notifications" Visible="false" HeaderText="Notifications" />
                                            <asp:CheckBoxField DataField="email_fails" HeaderText="Email on Fail Score" />
                                            <asp:CheckBoxField DataField="email_wrong" HeaderText="Email on Wrong Answer" />
                                            <asp:TemplateField HeaderText="Group">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DropDownList2" Enabled="false" AppendDataBoundItems="true" SelectedValue='<%#Bind("agent_group") %>' DataSourceID="dsGroups" DataTextField="agent_group" DataValueField="agent_group" runat="server">
                                                        <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DropDownList2" Enabled="true" AppendDataBoundItems="true" SelectedValue='<%#Bind("agent_group") %>' DataSourceID="dsGroups" DataTextField="agent_group" DataValueField="agent_group" runat="server">
                                                        <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Campaign">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCampaign" Enabled="false" AppendDataBoundItems="true" SelectedValue='<%#Bind("campaign") %>' DataSourceID="dsCampaigns" DataTextField="campaign" DataValueField="campaign" runat="server">
                                                        <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlCampaign" Enabled="true" AppendDataBoundItems="true" SelectedValue='<%#Bind("campaign") %>' DataSourceID="dsCampaigns" DataTextField="campaign" DataValueField="campaign" runat="server">
                                                        <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="email_list" />
                                            <asp:CommandField ShowDeleteButton="true" />

                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="btnAddEmail" OnClick="btnAddEmail_Click" runat="server" Text="Add Email" />
                                    <asp:SqlDataSource ID="dsEmails" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server"
                                        SelectCommand="select * from email_routing where sc_id=@id" InsertCommand="insert into email_routing(sc_id) select @id"
                                        UpdateCommand="update email_routing set email_wrong=@email_wrong,  email_list=@email_list,email_fails=@email_fails, agent_group=@agent_group, campaign=@campaign
                                        where id=@id"
                                        DeleteCommand="delete from  email_routing where id=@id">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlSC" Name="id" PropertyName="SelectedValue" Type="Int32" />
                                        </SelectParameters>
                                        <InsertParameters>
                                            <asp:ControlParameter ControlID="ddlSC" Name="id" PropertyName="SelectedValue" Type="Int32" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="dsGroups" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                                        SelectCommand="select distinct agent_group from xcc_report_new where scorecard = @ID and agent_group is not null and agent_group != '' and date_added > dateadd(d, -30, getdate()) union all select distinct agent_group from email_routing where sc_id=@ID order by agent_group" runat="server">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlSC" Name="id" PropertyName="SelectedValue" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="dsCampaigns" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                                        SelectCommand="select distinct campaign from xcc_report_new where scorecard = @ID and campaign is not null and campaign != '' and date_added > dateadd(d, -30, getdate())  union all select distinct campaign from email_routing where sc_id=@ID  order by campaign" runat="server">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlSC" Name="id" PropertyName="SelectedValue" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>

                        </table>

                    </EditItemTemplate>
                </asp:FormView>
                <asp:SqlDataSource ID="dsScorecard" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                    SelectCommand="SELECT * FROM [scorecards] WHERE ([id] = @id)" runat="server" DeleteCommand="DELETE FROM [scorecards] WHERE [id] = @id"
                    InsertCommand="INSERT INTO [scorecards] ([description], [short_name], [appname], [campaign], [date_added], [who_added], [active], [review_type], [golden_user], [calib_percent], [isCalibrated], [fail_score], [team_lead], [sc_sort], [training_count], [ni_scorecard], [transcribe], [score_type], [recal_percent], [sectionless], [website_cost], [website_display], [min_transcript_count], [user_cant_dispute], [max_speed], [min_cal], [num_cal_check], [import_type], [import_percent]) VALUES (@description, @short_name, @appname, @campaign, @date_added, @who_added, @active, @review_type, @golden_user, @calib_percent, @isCalibrated, @fail_score, @team_lead, @sc_sort, @training_count, @ni_scorecard, @transcribe, @score_type, @recal_percent, @sectionless, @website_cost, @website_display, @min_transcript_count, @user_cant_dispute, @max_speed, @min_cal, @num_cal_check, @import_type, @import_percent)"
                    UpdateCommand="UPDATE [scorecards] SET pass_percent=@pass_percent,cutoff_percent=@cutoff_percent,cutoff_count=@cutoff_count, post_import_sp=@post_import_sp, min_call_length=@min_call_length, 
                        required_dispositions=@required_dispositions, [description] = @description, [short_name] = @short_name, [appname] = @appname,
                        import_agents=@import_agents,  [active] = @active, [review_type] = @review_type, [golden_user] = @golden_user, [calib_percent] = @calib_percent, 
                         [isCalibrated] = @isCalibrated, [fail_score] = @fail_score, [team_lead] = @team_lead, [sc_sort] = @sc_sort, [training_count] = @training_count, 
                        hide_school_data=@hide_school_data, hide_data=@hide_data, [ni_scorecard] = @ni_scorecard, [transcribe] = @transcribe, [score_type] = @score_type, 
                        [recal_percent] = @recal_percent, [sectionless] = @sectionless, [website_cost] = @website_cost, [website_display] = @website_display, 
                        [min_transcript_count] = @min_transcript_count, [max_speed] = @max_speed, [min_cal] = @min_cal, 
                        [num_cal_check] = @num_cal_check, [import_type] = @import_type, [import_percent] = @import_percent, sc_notification_score=@sc_notification_score, 
                        sc_profile=@sc_profile, dedupe=@dedupe, max_per_day=@max_per_day, no_agent_login=@no_agent_login, redact=@redact,max_call_length=@max_call_length,
                        Account_manager=@Account_manager, show_custom_questions=@show_custom_questions, onhold=@onhold,meta_sort=@meta_sort,
                        overwrite_group=@overwrite_group,calib_role=@calib_role, qa_selected_role=@qa_selected_role,admin_selected_role=@admin_selected_role,
                        client_selected_role=@client_selected_role, recalib_role=@recalib_role,qa_qa_scorecard=@qa_qa_scorecard,shift_end= @shift_end, shift_start=@shift_start,
                        retain_non_used_calls=@retain_non_used_calls, rejection_profile=@rejection_profile, tango_team_lead=@tango_team_lead, truncate_time=@truncate_time,
                        end_truncate_time=@end_truncate_time, high_priority=@high_priority, isQAQACard=@isQAQACard, calibration_floor=@calibration_floor,
                        qa_pay=@qa_pay, pay_type=@pay_type, cal_spot_user_role=@cal_spot_user_role,dispute_base_percent=@dispute_base_percent,auto_accept_bad_call=@auto_accept_bad_call,
                        delete_after_days=@delete_after_days, monthly_minute_cap=@monthly_minute_cap, qless_parent=@qless_parent, third_party_scorecard=@third_party_scorecard,
                        listen_type=@listen_type WHERE [id] = @id; 
                        update scorecards set sc_profile = null where sc_profile= 0">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlSC" Name="id" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="description" Type="String" />
                        <asp:Parameter Name="short_name" Type="String" />
                        <asp:Parameter Name="appname" Type="String" />
                        <asp:Parameter Name="campaign" Type="String" />
                        <asp:Parameter Name="date_added" Type="DateTime" />
                        <asp:Parameter Name="who_added" Type="String" />
                        <asp:Parameter Name="active" Type="Boolean" />
                        <asp:Parameter Name="review_type" Type="String" />
                        <asp:Parameter Name="golden_user" Type="String" />
                        <asp:Parameter Name="calib_percent" Type="Int32" />
                        <asp:Parameter Name="isCalibrated" Type="Boolean" />
                        <asp:Parameter Name="redact" Type="Boolean" />
                        <asp:Parameter Name="fail_score" Type="Int32" />
                        <asp:Parameter Name="team_lead" Type="String" />
                        <asp:Parameter Name="sc_sort" Type="String" />
                        <asp:Parameter Name="training_count" Type="Int32" />
                        <asp:Parameter Name="ni_scorecard" Type="Boolean" />
                        <asp:Parameter Name="transcribe" Type="Boolean" />
                        <asp:Parameter Name="score_type" Type="String" />
                        <asp:Parameter Name="recal_percent" Type="Int32" />
                        <asp:Parameter Name="sectionless" Type="Boolean" />
                        <asp:Parameter Name="website_cost" Type="Double" />
                        <asp:Parameter Name="website_display" Type="String" />
                        <asp:Parameter Name="min_transcript_count" Type="Int32" />
                        <asp:Parameter Name="user_cant_dispute" Type="String" />
                        <asp:Parameter Name="max_speed" Type="Int32" />
                        <asp:Parameter Name="min_cal" Type="Double" />
                        <asp:Parameter Name="num_cal_check" Type="Int32" />
                        <asp:Parameter Name="import_type" Type="String" />
                        <asp:Parameter Name="required_dispositions" Type="String" />
                        <asp:Parameter Name="import_percent" Type="Int32" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>

                </asp:SqlDataSource>

                <h4>Scorecard Notes</h4>
                <asp:GridView ID="gvNotes" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsNotes">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                        <asp:BoundField DataField="note" HeaderText="note" SortExpression="note" />
                        <asp:BoundField DataField="sc_id" ReadOnly="true" HeaderText="sc_id" SortExpression="sc_id" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="dsNotes" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    DeleteCommand="DELETE FROM [sc_notes] WHERE [id] = @id"
                    InsertCommand="INSERT INTO [sc_notes] ([note], [sc_id]) VALUES (@note, @sc_id)" ProviderName="System.Data.SqlClient"
                    SelectCommand="SELECT * FROM [sc_notes] WHERE ([sc_id] = @sc_id)"
                    UpdateCommand="UPDATE [sc_notes] SET [note] = @note WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="note" Type="String" />
                        <asp:ControlParameter ControlID="ddlSC" Name="sc_id" />
                    </InsertParameters>
                    <SelectParameters>

                        <asp:ControlParameter ControlID="ddlSC" Name="sc_id" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="note" Type="String" />
                        <asp:Parameter Name="sc_id" Type="Int32" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                <asp:Button ID="btnAdd" Visible="false" runat="server" Text="Add Note" />

                <br />
                <br />
                <h4>Scorecard Updates</h4>
                <asp:GridView ID="gvChanges" CssClass="detailsTable" DataSourceID="dsChanges" runat="server"></asp:GridView>
                <asp:SqlDataSource ID="dsChanges" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                    SelectCommand="select * from scorecard_changes where scorecard = @scorecard order by changed_date desc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlSC" Name="scorecard" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="dsMyApps" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="select distinct appname from userapps join userextrainfo on userextrainfo.username = userapps.username where 1 = case when user_role  = 'Admin' then 1 when user_role != 'Admin' and userapps.username = @username then 1 else 0 end and userextrainfo.username =@username order by appname ">
                    <SelectParameters>
                        <asp:Parameter Name="username" />
                    </SelectParameters>
                </asp:SqlDataSource>



            </ContentTemplate>
        </asp:UpdatePanel>

    </section>
</asp:Content>

