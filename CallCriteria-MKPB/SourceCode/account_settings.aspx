<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="account_settings.aspx.vb" Inherits="account_settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="ac_files/account_settings.css" rel="stylesheet" />
    <script src="ac_files/account_settings.js"></script>
    <link href="ac_files/style.css" rel="stylesheet" />
    <script src="ac_files/dragdealer2.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <div class="settingsBody">
            <span class="secHeader">Account Settings</span>

            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Name</td>
                        <td class="settingVal" id="fullname"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">
                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Name</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputLabel">First Name</td>
                                            <td class="inputBox">
                                                <input type="text" id="first_name" value="" /></td>
                                        </tr>
                                        <tr>
                                            <td class="inputLabel">Last Name</td>
                                            <td class="inputBox">
                                                <input type="text" id="last_name" value="" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    This is your first and last name. 
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#first_name').val(), 'first_name');updateData($('#last_name').val(), 'last_name');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Username</td>
                        <td class="settingVal" id="td_username"></td>
                        <td class="editButton"><a><i class="fa fa-eye"></i>View</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Username</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputLabel">Username</td>
                                            <td class="inputBox">
                                                <input type="text" id="username" value="democlient" disabled="disabled" autocomplete="off"></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    This is your ID on this website.  You'll use this to log in.
                                </div>
                                <div class="settingButtons">
                                    <button type="button" class="cancelBtn">Hide</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>

            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Email</td>
                        <td class="settingVal" id="td_email"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Email</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputLabel">Email Address</td>
                                            <td class="inputBox">
                                                <input type="text" id="email" value="" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    This is the email address through which we'll contact you.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#email').val(), 'email_address');">>Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>

            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Password</td>
                        <td class="settingVal">********</td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Password</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputLabel">Current</td>
                                            <td class="inputBox">
                                                <input type="password" id="current_password" value="" autocomplete="off" /></td>
                                        </tr>
                                        <tr>
                                            <td class="inputLabel">New</td>
                                            <td class="inputBox">
                                                <input type="password" id="new_password" value="" /></td>
                                        </tr>
                                        <tr>
                                            <td class="inputLabel">Re-type New</td>
                                            <td class="inputBox">
                                                <input type="password" id="retype_password" value="" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    Login using this password.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#current_password').val() + '|' + $('#new_password').val() + '|' + $('#retype_password').val() , 'password');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>

            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Phone Number</td>
                        <td class="settingVal" id="td_phone"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Phone Number</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputLabel">Phone Number</td>
                                            <td class="inputBox">
                                                <input type="text" id="phone" value="" autocomplete="off"></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    Best phone number for you to be contacted at.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#phone').val(), 'phone_number');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>

            <%-- <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Coaching Queue</td>
                        <td class="settingVal" id="td_coaching_q">78%</td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Coaching Queue</td>
                            <td class="settingContent">

                                <div class="settings-slider" id="cq-slider-wrapper">
                                    <div class="slider-part">
                                        <span class="slider-label">Coaching Queue</span>
                                        <div class="slider-bar" id="cq-slider">
                                            <div class="handle slider-handle" style="perspective: 1000px; backface-visibility: hidden; transform: translateX(0px);"></div>
                                        </div>
                                    </div>
                                    <div class="number-part">
                                        <span class="num" id="coaching_q">78</span>%
                                    </div>
                                </div>

                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    The dashboard's Coaching Queue module will show
						records with values above this percentage.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button"  onclick="updateData($('#speed_increment').val(), 'speed_increment');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>--%>

          <%--  <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Speed Increment</td>
                        <td class="settingVal" id="td_speed_increment">5</td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Speed Increment</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox smallInputBox">
                                                <input type="text" id="speed_increment" value="5" class="num-only" autocomplete="off" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    This is some kind of value that sets the speed of something.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#speed_increment').val(), 'speed_increment');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>



            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Guideline Display Time</td>
                        <td class="settingVal" id="td_guideline_display">5</td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Guideline Display Time</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox smallInputBox">
                                                <input type="text" id="guideline_display" value="5" class="num-only" autocomplete="off" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    This sets the speed the guideline tips appear in Listen.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#guideline_display').val(), 'guideline_display');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>--%>



            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Calls Start Immediately</td>
                        <td class="settingVal" id="td_calls_start_immediately"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Calls Start Immediately</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox">
                                                <input type="checkbox" id="calls_start_immediately"></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                     Toggles whether call audio will play automatically upon opening a record.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData(document.getElementById('calls_start_immediately').checked, 'calls_start_immediately');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>



            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Open New Listen on Submit</td>
                        <td class="settingVal" id="td_presubmit"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Open New Listen on Submit</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox">
                                                <input type="checkbox" id="presubmit" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                     Toggles whether or not a new call will open immediately after scoring and saving a call
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData(document.getElementById('presubmit').checked, 'presubmit');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>



            <%-- <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Bypass AWS</td>
                        <td class="settingVal" id="td_bypass"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Bypass AWS</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox">
                                                <input type="checkbox" id="bypass" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    If calls are not playing or pausing, try switching this to ON.  Remember to switch back to OFF later.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData(document.getElementById('bypass').checked, 'bypass');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>--%>






            <table class="settingLine">
                <tbody>
                    <tr>
                        <td class="settingName">Export Format</td>
                        <td class="settingVal" id="td_export"></td>
                        <td class="editButton"><a><i class="fa fa-pencil"></i>Edit</a></td>
                    </tr>
                </tbody>
            </table>
            <div class="settingBox hidden">

                <table>
                    <tbody>
                        <tr>
                            <td class="settingName">Export Format</td>
                            <td class="settingContent">
                                <table class="settingInputs">
                                    <tbody>
                                        <tr>
                                            <td class="inputBox">
                                                <select id="export_type">
                                                    <option value="CSV">CSV</option>
                                                    <option value="Excel">Excel</option>
                                                </select>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="settingNote">
                                    <span class="noteLabel">Note:</span>
                                    - Toggles whether the export will be in csv or excel file format.
                                </div>
                                <div class="settingButtons">
                                    <button class="saveBtn" type="button" onclick="updateData($('#export_type').val(), 'export_type');">Save Changes</button>
                                    <button type="button" class="cancelBtn">Cancel</button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>



        </div>

        <asp:HiddenField ID="hdnUsername" runat="server" />

        <div class="loginHistory">
            <span class="secHeader">My Custom Email/Notifications
                <asp:Button ID="btnAddEmail" runat="server" Text="Add Email" /></span>

            <asp:GridView ID="gvEmails" CssClass="detailsTable" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsEmails" runat="server">
                <Columns>
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CheckBoxField DataField="email_notifications" HeaderText="Notifications" Visible="false" />
                    <asp:CheckBoxField DataField="email_fails" HeaderText="Email on Fail Score" />
                    <asp:CheckBoxField DataField="email_wrong" HeaderText="Email on Wrong Answer" />
                    <asp:TemplateField HeaderText="Scorecard">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlScorecard" Width="200" Enabled="false" AppendDataBoundItems="true" SelectedValue='<%#Bind("sc_id") %>' DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="id" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlScorecard" Width="200" Enabled="true" AppendDataBoundItems="true" SelectedValue='<%#Bind("sc_id") %>' DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="id" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList2" Width="200" Enabled="false" AppendDataBoundItems="true" SelectedValue='<%#Bind("agent_group") %>' DataSourceID="dsGroups" DataTextField="agent_group" DataValueField="agent_group" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="DropDownList2" Width="200" Enabled="true" AppendDataBoundItems="true" SelectedValue='<%#Bind("agent_group") %>' DataSourceID="dsGroups" DataTextField="agent_group" DataValueField="agent_group" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Campaign">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlCampaign" Width="200" Enabled="false" AppendDataBoundItems="true" SelectedValue='<%#Bind("campaign") %>' DataSourceID="dsCampaigns" DataTextField="campaign" DataValueField="campaign" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlCampaign" Width="200" Enabled="true" AppendDataBoundItems="true" SelectedValue='<%#Bind("campaign") %>' DataSourceID="dsCampaigns" DataTextField="campaign" DataValueField="campaign" runat="server">
                                <asp:ListItem Text="N/A" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Email Address" DataField="email_list" />
                    <asp:CommandField ShowDeleteButton="true" />

                </Columns>
            </asp:GridView>
        </div>

        <asp:SqlDataSource ID="dsEmails" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server"
            SelectCommand="select * from email_routing  left join scorecards on scorecards.id = sc_id where username = @username and isnull(active,1) = 1" InsertCommand="insert into email_routing(username, email_fails) select @username, 1"
            DeleteCommand="delete from email_routing where id=@id"
            UpdateCommand="update email_routing set email_wrong=@email_wrong, email_list=@email_list, agent_group=@agent_group, campaign=@campaign, 
                                        email_fails=@email_fails, sc_id=@sc_id, username=@username where id=@id">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsGroups" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select distinct agent_group from xcc_report_new where scorecard in (select user_scorecard from userapps where username = @username) and agent_group is not null and agent_group != '' and date_added > dateadd(d, -30, getdate()) order by agent_group" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsCampaigns" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select distinct campaign from xcc_report_new where scorecard in (select user_scorecard from userapps where username = @username)  and campaign is not null and campaign != '' and date_added > dateadd(d, -30, getdate()) order by campaign" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select short_name, id from scorecards where id in (select user_scorecard from userapps where username = @username) and active = 1  order by short_name" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>


     <%--   <div class="loginHistory">
            <span class="secHeader">My Custom Columns
                <button type="button" class="general-button" onclick="resetCols();">Reset Columns</button></span>


            <table id="tblColumns">
                <tbody class="ui-sortable">
                </tbody>
            </table>
            <div id="divFields"></div>


        </div>--%>



       <%-- <div class="loginHistory">
            <span class="secHeader">Login History</span>



            <table cellspacing="0px" cellpadding="0px">
                <tbody>
                    <tr>
                        <th>Date</th>
                        <th>Calls<br>
                            Viewed</th>
                        <th>Notifications<br>
                            Reviewed</th>
                    </tr>


                    <asp:Repeater ID="rptActivity" DataSourceID="dsActivity" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("call_date", "{0:MM/dd/yyyy}") %></td>
                                <td><%#Eval("calls_reviewed") %></td>
                                <td>23</td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:SqlDataSource ID="dsActivity" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select count(*) as calls_reviewed, convert(date, review_date) as call_date from vwForm where reviewer = @username and review_date > DATEADD(d, -30, getdate()) group by convert(date, review_date) " runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>


                </tbody>
            </table>
        </div>--%>
    </section>
    <script type="text/javascript">
        $(window).on('load', function () {
            loadUser();
            //loadColumns();
        });


        updateIndex = function (e, ui) {
            var colset = '';
            $('#tblColumns tbody td:nth-child(2)').each(function (i) {
                colset += $(this).parent().find('td:nth-child(1)').find('input:checkbox').val() + '=' + (i + 1) + ',';
            });
            colset = colset.substring(0, colset.length - 1);

            $.ajax({
                type: "POST",
                url: "account_settings.aspx/UpdateUserColumnPriority",
                data: '{"colset" : "' + colset + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    //updateReport();
                    loadColumns();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        };

        function resetCols() {
            $.ajax({
                type: "POST",
                url: "account_settings.aspx/resetCols",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    //updateReport();
                    loadColumns();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function updateColCheck(item) {
            var col_order = item.parent().parent().find('td:nth-child(2)').html();

            $.ajax({
                type: "POST",
                url: "account_settings.aspx/updateMyField",
                data: '{"check_status" : "' + item.is(':checked') + '", "field_id" : "' + item.val() + '", "col_order" : "0"}', //Data sent to server
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    //updateReport();
                    loadColumns();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }


        function loadColumns() {

            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "account_settings.aspx/getMyFields", // Location of the service
                //data: '{"start_date":"' + $('#ContentPlaceHolder1_date1').val() + '","end_date":"' + $('#ContentPlaceHolder1_date2').val() + '","hdnAgentFilter":"' + filter_list + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call
                    $('#tblColumns tbody').empty();
                    $('#tblColumns tbody').append(msg.d);


                    $("#tblColumns tbody").sortable({
                        stop: updateIndex
                    });


                },
                error: ServiceFailed// When Service call fails
            });

        }

        function loadUser() {

            $('.settingLine').removeClass('hidden');
            $('.settingBox').addClass('hidden');

            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "/CDService.svc/getUserInfo", // Location of the service
                //data: '{"start_date":"' + $('#ContentPlaceHolder1_date1').val() + '","end_date":"' + $('#ContentPlaceHolder1_date2').val() + '","hdnAgentFilter":"' + filter_list + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call

                    var jsonObj = eval(msg.d);

                    $('#first_name').val(jsonObj.first_name);
                    $('#last_name').val(jsonObj.last_name);
                    $('#fullname').html(jsonObj.first_name + ' ' + jsonObj.last_name)

                    $('#email').val(jsonObj.email);
                    $('#td_email').html(jsonObj.email);

                    $('#username').val(jsonObj.username);
                    $('#td_username').html(jsonObj.username);

                    $('#phone').val(jsonObj.phone);
                    $('#td_phone').html(jsonObj.phone);

                    $('#speed_increment').val(jsonObj.SpeedInc);
                    $('#td_speed_increment').html(jsonObj.SpeedInc);

                    $('#guideline_display').val(jsonObj.guideline_display);
                    $('#td_guideline_display').html(jsonObj.guideline_display);

                    $('#calls_start_immediately').prop("checked", jsonObj.ImmediatePlay);
                    $('#td_calls_start_immediately').html(jsonObj.ImmediatePlay.toString());

                    $('#presubmit').prop("checked", jsonObj.presubmit);
                    $('#td_presubmit').html(jsonObj.presubmit.toString());

                    $('#bypass').prop("checked", jsonObj.bypass);
                    $('#td_bypass').html(jsonObj.bypass.toString());

                    $('#export_type').val(jsonObj.export_type);
                    $('#td_export').html(jsonObj.export_type.toString());

                },
                error: ServiceFailed// When Service call fails
            });
        }

        function updateData(value, field) {
            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "/CDService.svc/updateUserInfo", // Location of the service
                data: '{"value":"' + value + '","field":"' + field + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call
                    if (msg.d) {
                        loadUser();
                    }
                    else {
                        alert("Update failed, pleae try again.")
                        loadUser();
                    }
                },
                error: ServiceFailed// When Service call fails
            });
        }

    </script>

</asp:Content>

