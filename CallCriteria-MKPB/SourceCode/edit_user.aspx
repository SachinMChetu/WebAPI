<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit_user.aspx.vb" Inherits="edit_user" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <section class="main-container dash-module general-button">
        <div class="edit-form">
            <h2>Edit User</h2>
            <%-- <button type="button" onclick="window.location.href='client_users.aspx'">Back to Users</button> --%>
            <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" DataKeyNames="ID" DataSourceID="dsMyUsers">
                <EditItemTemplate>

                    <table>
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td class="label">User:</td>
                                        <td>
                                            <asp:Label ID="usernameTextBox" runat="server" Text='<%# Bind("username") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Last Login:</td>
                                        <td>
                                            <asp:Label ID="lastLoginDateTextBox" runat="server" Text='<%# Bind("lastLoginDate") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Who Added:</td>
                                        <td>
                                            <asp:Label ID="who_addedTextBox" runat="server" Text='<%# Bind("who_added") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Date Added:</td>
                                        <td>
                                            <asp:Label ID="dateaddedTextBox" runat="server" Text='<%# Bind("dateadded") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">First Name:</td>
                                        <td>
                                            <asp:TextBox ID="first_nameTextBox" runat="server" Text='<%# Bind("first_name") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="last_nameTextBox" runat="server" Text='<%# Bind("last_name") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Email Address:</td>
                                        <td>
                                            <asp:TextBox ID="email_addressTextBox" runat="server" Text='<%# Bind("email_address") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Default Page:</td>
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("default_page") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">No Dashboard Access:</td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkNoDash" Checked='<%# Bind("no_dash") %>' />
                                    </tr>

                                </table>
                            </td>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td class="label">Password:</td>
                                        <td>
                                            <asp:TextBox ID="passwordTextBox" runat="server" Text='<%# Bind("password") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Non edit:</td>
                                        <td>
                                            <asp:CheckBox ID="non_editCheckBox" runat="server" Checked='<%# Bind("non_edit") %>' /></td>
                                    </tr>
                                     <tr>
                                        <td class="label">Non calibrating:</td>
                                        <td>
                                            <asp:CheckBox ID="non_calibCheckBox1" runat="server" Checked='<%# Bind("non_calib") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Role:</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("user_role") %>'>
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                                <asp:ListItem>Agent</asp:ListItem>
                                                <asp:ListItem>Supervisor</asp:ListItem>
                                                <asp:ListItem>Partner</asp:ListItem>
                                                <asp:ListItem>Manager</asp:ListItem>
                                                <asp:ListItem>Inactive</asp:ListItem>
                                                <asp:ListItem>Client</asp:ListItem>
                                                <asp:ListItem>Client Calibrator</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Group:</td>
                                        <td>
                                            <asp:HiddenField ID="hdnOrigGroup" runat="server" Value='<%# Bind("user_group") %>' />
                                            <asp:DropDownList ID="DropDownList2" OnDataBound="DropDownList2_DataBound" DataSourceID="dsGroup" DataTextField="agent_group" DataValueField="agent_group"
                                                AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                            </asp:DropDownList></td>

                                        <asp:GridView ID="gvUserGroups" AutoGenerateColumns="false" AutoGenerateEditButton="true" DataKeyNames="ID" DataSourceID="dsGroups" runat="server">
                                            <Columns>
                                                <asp:BoundField HeaderText="Scorecard" DataField="scorecard" />
                                                <asp:TemplateField HeaderText="Group">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("groupName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hdnscorecard" runat="server" value='<%#Eval("sc_id") %>' />
                                                         <asp:DropDownList ID="ddlGroup" DataSourceID="dsGroups" DataTextField="groupName" AppendDataBoundItems="true" DataValueField="id" selectedvalue='<%#Bind("sg_id") %>' runat="server">
                                                             <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                                         </asp:DropDownList>
                                                          <asp:SqlDataSource ID="dsGroups" SelectCommand="select groupName, id from scorecard_groups where scorecard = @scorecard order by groupName" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hdnscorecard" Name="scorecard" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField CommandName="Delete" Text="X" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsGroups" SelectCommand="getUserGroups" SelectCommandType="StoredProcedure"
                                            DeleteCommand="delete from user_groups where id = @ID"
                                            UpdateCommand ="update user_groups set userGroup = @sg_id where id = @id "
                                            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="user" Name="username" />
                                            </SelectParameters>
                                            <UpdateParameters>
                                                <%--<asp:ControlParameter ControlID="ddlGroup" Name="userGroup" />--%>
                                            </UpdateParameters>
                                        </asp:SqlDataSource>

                                        Add group for scorecard: <asp:DropDownList ID="ddlSCForGroup" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" runat="server"></asp:DropDownList>
                                        <asp:Button ID="btnSCGroupAdd" OnClick="btnSCGroupAdd_Click" runat="server" Text="Add" />

                                         <asp:SqlDataSource ID="dsScorecards" SelectCommand="select scorecards.id, short_name from userapps join scorecards on scorecards.id = user_scorecard
                                             and username = @username order by short_name" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="user" Name="username" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </tr>

                                    <tr>
                                        <td class="label">Update Older Data:</td>
                                        <td>
                                            <asp:CheckBox ID="chkUpdateHistory" runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="label">Manager:</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList3" DataSourceID="dsManager" DataTextField="username" DataValueField="username"
                                                AppendDataBoundItems="true" runat="server" SelectedValue='<%# Bind("manager") %>'>
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Force Agent Review:</td>
                                        <td>
                                            <asp:CheckBox ID="force_reviewCheckBox" runat="server" Checked='<%# Bind("force_review") %>' />
                                        </td>
                                    </tr>



                                </table>

                            </td>
                        </tr>

                    </table>

                    <table>
                        <tr>
                            <td class="label" valign="top">Scorecards:</td>
                            <td>
                                <asp:HiddenField ID="hdnThisUser" runat="server" Value='<%#Eval("username") %>'></asp:HiddenField>
                                <asp:CheckBoxList ID="cblScores" RepeatLayout="OrderedList" DataSourceID="dsScorecards" DataTextField="short_name" OnDataBound="cblScores_DataBinding"
                                    DataValueField="ID" AppendDataBoundItems="true" runat="server" CssClass="checkboxes-list">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                    </div>
					
					</div>
	
			
                    <div class="submit-buttons">
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" CssClass="button-style dark-button" />
                        <%-- <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" CssClass="button-style" /> --%>
                        <!-- <a href="client_users.aspx" class="button-style">Cancel</a> -->
                        <a href="javascript:window.parent.closeEditUserPopup();" class="button-style">Cancel</a>
                    </div>
                </EditItemTemplate>

            </asp:FormView>

            <asp:Label ID="lblUpdated" ForeColor="Red" runat="server" Text=""></asp:Label>

            <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select short_name, id from scorecards where active = 1 and id in (select user_scorecard from userapps where username = @username)  order by short_name" runat="server">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="dsGroup" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select distinct user_group collate latin1_general_cs_as as agent_group from (
                            select distinct user_group collate latin1_general_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select distinct appname from userapps where username = @username)) 
                            and user_role in ('Supervisor','Manager','Client', 'Agent') and user_group is not null
                            union all 
                            select distinct username collate latin1_general_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select distinct appname from userapps where username =@username)) 
                            and user_role in ('Supervisor','Manager','Client') 
                            union all
                            select distinct agent_group collate latin1_general_cs_as as agent_group from xcc_report_new where scorecard in (select user_scorecard from userapps where username = @username)
                            ) a
                            where user_group != ''
                            order by user_group"
                runat="server">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                    <asp:QueryStringParameter QueryStringField="user" Name="user" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="dsManager" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select distinct userextrainfo.username from userextrainfo join userapps on userapps.username = userextrainfo.username where user_scorecard in (select user_scorecard from userapps where username = @username) and user_role = 'Manager'
                    union all
                    select manager from userextrainfo where username=@user
                 order by userextrainfo.username"
                runat="server">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                    <asp:QueryStringParameter QueryStringField="user" Name="user" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="dsMyUsers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select distinct non_calib,force_review, default_page, no_dash, userextrainfo.ID, userextrainfo.manager, userextrainfo.username,first_name,last_name,email_address, '*****' as password, lastLoginDate, user_role, 
								user_group, who_added, dateadded, isnull(non_edit,0) as non_edit, dbo.getMyScorecards(userextrainfo.username) as scorecard_list
								from userextrainfo left join userapps on userapps.username = userextrainfo.username 
								where userextrainfo.username = @other_user and user_scorecard in (select user_scorecard from userapps where username = @username) 
								and user_role in ('Agent','Supervisor', 'Inactive','Manager', 'Client') order by userextrainfo.username"
                UpdateCommand="update userextrainfo set no_dash=@no_dash, default_page=@default_page, non_edit=@non_edit, force_review=@force_review,
                               first_name =@first_name, manager=@manager, last_name =@last_name, email_address=@email_address, user_role=@user_role, 
                               user_group=@user_group,non_calib=@non_calib where id=@ID"
                runat="server">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                    <asp:QueryStringParameter Name="other_user" QueryStringField="user" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

    </section>

    <link rel="stylesheet" href="edit_client_user.css" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {

            $('input[type="checkbox"]').after('<span class="checkDisplay"></span>');
            $('.checkDisplay').click(function () {
                var checkbox = $(this).prev('input[type="checkbox"]');
                if (!checkbox.attr('disabled')) {
                    checkbox.prop('checked', !checkbox.prop('checked'));
                }
            });

            $('#ContentPlaceHolder1_FormView1_DropDownList2').change(function () {
                $('#ContentPlaceHolder1_FormView1_hdnOrigGroup').val($('#ContentPlaceHolder1_FormView1_DropDownList2').val());
            })


        });
    </script>
    </form>
</body>
</html>
