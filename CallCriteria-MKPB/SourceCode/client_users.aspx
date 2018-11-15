<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="client_users.aspx.vb" Inherits="client_users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FormsAuthenticationUserImpersonation"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="popup2-background" id="add-user-popup-bg"></div>
    <div class="popup2" id="add-user-popup">
        <h2>Add User</h2>
        <!-- <span class="close-popup2"><i class="fa fa-times"></i></span> -->

        <table>
            <tr class="formline">
                <td class="label">Username</td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" autocomplete="new-password" CssClass="required-field"></asp:TextBox></td>
            </tr>
            <tr class="formline">
                <td class="label">Password</td>
                <td>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" autocomplete="new-password"></asp:TextBox></td>
            </tr>
            <tr class="formline">
                <td class="label">Email Address</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr class="formline">
                <td class="label">Role</td>
                <td>
                    <asp:DropDownList ID="ddlShareLevel" runat="server">
                        <asp:ListItem>Agent</asp:ListItem>
                        <asp:ListItem>Supervisor</asp:ListItem>
                        <asp:ListItem>Partner</asp:ListItem>
                        <asp:ListItem>Manager</asp:ListItem>
                        <asp:ListItem>Client Calibrator</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr class="formline">
                <td class="label">Initial App</td>
                <td>
                    <asp:DropDownList ID="ddlInitialApp" DataSourceID="dsApps" DataTextField="appname" DataValueField="appname" CssClass="checkbox-label" runat="server">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select appname from app_settings where active = 1 and appname in (select appname from userapps where username = @username)  order by appname" runat="server">
                        <selectparameters>
                            <asp:Parameter Name="username" />
                        </selectparameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td valign="top" class="label">Initial Scorecard(s)</td>
                <td>
                    <asp:CheckBoxList ID="cblScorecards" CellPadding="10" CssClass="scorecard-checkboxes required-checks" DataSourceID="dsScorecards" RepeatColumns="3" DataTextField="short_name" DataValueField="ID" runat="server">
                    </asp:CheckBoxList></td>
            </tr>
        </table>
        <div class="popup2-footer">
            <asp:Button ID="btnAddUser" CssClass="close-popup2 dark-button add-user-submit" disabled="disabled" runat="server" Text="Add User" />
            <button type="button" class="close-popup2">Cancel</button>
        </div>
    </div>

    <div class="popup2-background" id="edit-user-popup-bg"></div>
    <div class="popup2" id="edit-user-popup">
        <iframe></iframe>
        <div class="loading-sign">Loading...</div>
    </div>


    <section class="main-container dash-modules" style="text-align: center;">



        <table class="page-title-wrap">
            <tr>


                <td><span class="page-title">Client Users</span></td>

                <td>

                    <asp:LoginUserImpersonation ID="LoginUserImpersonation1" runat="server" />
                </td>

                <td><span class="filter-box-label">Filter:</span>
                    <input type="text" name="filter-textbox" id="filter-textbox" />
                    <input type="checkbox" name="filter-inactive" />
                    Hide Inactive </td>
                <td>
                    <asp:Button ID="btnExport" Visible="false" runat="server" Text="Export" />
                </td>
                <td class="button-wrap">
                    <button type='button' class='add-user-btn dark-button'>Add User</button></td>
            </tr>
        </table>

        Create group
        <asp:TextBox ID="txtNewGroup" runat="server"></asp:TextBox>
        for specific user
        <asp:TextBox ID="txtUserGroup" runat="server"></asp:TextBox>
        <asp:Button ID="btnUpdateGroup" runat="server" Text="Update User" />
        <br />
        <br />
        <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select short_name, id from scorecards where active = 1 and appname in (select appname from userapps where username = @username)  order by short_name" runat="server">
            <selectparameters>
                <asp:Parameter Name="username" />
            </selectparameters>
        </asp:SqlDataSource>


        <div style="float: left;">
            <asp:Label ID="lblNote" runat="server" />
        </div>

        <div style="float: right;">
        </div>

        <center>
                <asp:GridView ID="gvMyUsers" RowStyle-Height="40" allowsorting="true" CssClass="detailsTable" datakeynames="ID" AutoGenerateColumns="False" DataSourceID="dsMyUsers" runat="server">
                    <Columns>
                        <%--<asp:CommandField ShowEditButton="true" HeaderText="<button type='button' class='add-report-btn'>Add User</button>" />--%>
                       <%-- <asp:HyperLinkField HeaderText="" DataNavigateUrlFields="username" DataNavigateUrlFormatString="edit_client_user.aspx?user={0}" text="Edit" /> --%>
						<asp:TemplateField HeaderText="">
							<ItemTemplate>
								<a href="javascript:openEditUserPopup('<%#Replace(Eval("username"), "'", "\'")%>');">Edit</a>
							</ItemTemplate>
						</asp:TemplateField>
                        <%--<asp:BoundField DataField="username" HtmlEncode="false" HeaderText="Username" ReadOnly="true" sortexpression="username" />--%>
                       <%-- <asp:ButtonField ButtonType="Link" CommandName="switchUser" DataTextField="username" HeaderText="Username"  sortexpression="username" />--%>

                        <asp:TemplateField HeaderText="Username"  sortexpression="username">
							<ItemTemplate>
								<asp:Linkbutton runat="server" ID="lbUser" CommandName="switchUser" CommandArgument='<%#Eval("username") %>'><%#Eval("username") %></asp:Linkbutton>
 							</ItemTemplate>
                            <EditItemTemplate>
                                <asp:Linkbutton runat="server" ID="lbUser" CommandName="switchUser" CommandArgument='<%#Eval("username") %>'><%#Eval("username") %></asp:Linkbutton>
                            </EditItemTemplate>
						</asp:TemplateField>
                        <asp:BoundField DataField="first_name" HeaderText="First Name" sortexpression="first_name" />
                        <asp:BoundField DataField="last_name" HeaderText="Last Name" sortexpression="last_name" />
                         <asp:BoundField DataField="email_address" HeaderText="Email" sortexpression="email_address" />
                        <asp:BoundField DataField="lastLoginDate" HeaderText="Last Login" ReadOnly="true" sortexpression="lastLoginDate" />
                        <asp:TemplateField HeaderText="User Type" sortexpression="user_role">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("user_role") %>'>
                                    <asp:ListItem>Agent</asp:ListItem>
                                    <asp:ListItem>Supervisor</asp:ListItem>
                                    <asp:ListItem>Client Calibrator</asp:ListItem>
                                    <asp:ListItem>Partner</asp:ListItem>
                                    <asp:ListItem>Manager</asp:ListItem>
                                    <asp:ListItem>Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("user_role") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supervisor" sortexpression="user_group">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList2" DataSourceID="dsGroup" DataTextField="agent_group" DataValueField="agent_group" AppendDataBoundItems="true" runat="server" SelectedValue='<%# Bind("user_group") %>'>
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>

                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("user_group") %>'></asp:Label>
                               <%-- <asp:DropDownList ID="DropDownList2" OnDataBound="DropDownList2_DataBinding" Enabled="false" DataSourceID="dsGroup" DataTextField="agent_group" DataValueField="agent_group" AppendDataBoundItems="true" runat="server" >
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Scorecards">
                            <EditItemTemplate>
                               <%-- <asp:HiddenField ID="hdnThisUser" runat="server" Value='<%#Eval("username") %>'></asp:HiddenField>
                                <asp:checkboxlist ID="cblScores" DataSourceID="dsScorecards" DataTextField="short_name"  OnDataBound="cblScores_DataBinding"
                                   RepeatColumns="3"  DataValueField="ID" AppendDataBoundItems="true" runat="server">
                                </asp:checkboxlist>--%>
                                <%#Eval("scorecard_list")%>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnThisUser" runat="server" Value='<%#Eval("username") %>'></asp:HiddenField>
                              <%--  <asp:Repeater datasourceID="dsUserScorecards" runat="server">
                                    <itemtemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("short_name") %>'></asp:Label><br />
                                    </itemtemplate>
                                
                                </asp:Repeater>
                                <asp:SqlDataSource ID="dsUserScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select short_name from userapps join scorecards on user_scorecard =scorecards.id where username=@username" runat="server">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="username" ControlID="hdnThisUser" />
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>
                                 <%#Eval("scorecard_list")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:CheckBoxField DataField="non_edit" HeaderText ="Non-Edit<br>Supervisor <i class='fa fa-info-circle info-dot' style='cursor: pointer;' title='Supervisor sees all agents and queues, can not edit calls.'></i>" /> -->
                       <%-- <asp:BoundField DataField="who_added" HeaderText="Added By" ReadOnly="True" />
                        <asp:BoundField DataField="dateadded" HeaderText="Date Added" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" />--%>

                    </Columns>
                    <RowStyle Height="40px" />
                   <EmptyDataRowStyle CssClass="EmptyData" />
                    <EmptyDataTemplate>
                        <button type='button' class='add-report-btn'>Add User</button>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="dsMyUsers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="exec getClientUsers @username"  
                    UpdateCommand="update userextrainfo set non_edit=@non_edit, first_name =@first_name, last_name =@last_name, email_address=@email_address, user_role=@user_role, 
                    user_group=@user_group where id=@ID"
                    runat="server">
                    <SelectParameters>
                        <asp:Parameter Name="username" />
                    </SelectParameters>
                </asp:SqlDataSource>
              <%--  <asp:SqlDataSource ID="dsMyUsers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="select distinct userextrainfo.ID, userextrainfo.username,first_name,last_name,email_address,  lastLoginDate, user_role, 
                                    z.agent_group as user_group,  non_edit, dbo.getMyScorecards2(userextrainfo.username, @username) as scorecard_list
                                    from userextrainfo join userapps on userapps.username = userextrainfo.username 
                                    left join (select distinct user_group collate latin1_general_cs_as as agent_group from (
                                            select distinct user_group collate latin1_general_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select distinct appname from userapps where username = @username)) 
                                            and user_role in ('Supervisor','Manager','Client', 'Agent') and user_group is not null
                                            union all 
                                            select distinct username collate latin1_general_cs_as as user_group from userextrainfo 
                                                where username in (select username from userapps where appname in (select distinct appname from userapps where username =@username)) 
                                            and user_role in ('Supervisor','Manager','Client') 
                                            union all
                                            select distinct agent_group collate latin1_general_cs_as as agent_group from xcc_report_new where scorecard in (select user_scorecard from userapps where username = @username)
                                            ) a
                                            where user_group != '') z on z.agent_group = userextrainfo.user_group

                                    where  user_scorecard in (select user_scorecard from userapps where username =  @username) 
                                    and user_scorecard in (select id from scorecards where active = 1)
                                    and user_role in ('Agent','Supervisor', 'Inactive','Manager', 'Client') order by userextrainfo.username"
                    UpdateCommand="update userextrainfo set non_edit=@non_edit, first_name =@first_name, last_name =@last_name, email_address=@email_address, user_role=@user_role, user_group=@user_group where id=@ID"
                    runat="server">
                    <SelectParameters>
                        <asp:Parameter Name="username" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                </center>


        <%--       <asp:SqlDataSource ID="dsGroup" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct user_group as agent_group from (
                            select distinct user_group from userextrainfo where username in (select username from userapps where appname in (select appname from userapps where username = 'winnie')) 
                            and user_role in ('Supervisor','Manager','Client') and user_group is not null
                            union all 
                            select distinct username from userextrainfo where username in (select username from userapps where appname in (select appname from userapps where username = 'winnie')) 
                            and user_role in ('Supervisor','Manager','Client') and user_group is not null) a
                            where user_group != ''
                            order by user_group"
            runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>--%>


        <asp:SqlDataSource ID="dsGroup" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct user_group collate latin1_general_cs_as as agent_group from (
                            select distinct user_group collate latin1_general_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select distinct appname from userapps where username = @username)) 
                            and user_role in ('Supervisor','Manager','Client') and user_group is not null
                            union all 
                            select distinct username collate latin1_general_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select distinct appname from userapps where username =@username)) 
                            and user_role in ('Supervisor','Manager','Client') 
                            union all
                            select distinct agent_group collate latin1_general_cs_as as agent_group from xcc_report_new where scorecard in (select user_scorecard from userapps where username = @username)
                            ) a
                            where user_group != ''
                            order by user_group"
            runat="server">
            <selectparameters>
                <asp:Parameter Name="username" />
            </selectparameters>
        </asp:SqlDataSource>




        <div id="edit-popup" class="edit-box">
            <div id="edit-popup-container" class="edit-container">
                <div id="edit-popup-content" class="edit-content">
                    <input type="text" id="emailadd" class="edit-input" value="" /><br />
                    <a id="edit-popup-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-popup-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>


        <div id="edit-password" class="edit-box show-btns">
            <div id="edit-password-container" class="edit-container">
                <div id="edit-password-content" class="edit-content">
                    <input type="password" id="new_password" class="edit-input" value="" /><br />
                    <a id="edit-password-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-password-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>


        <div id="edit-user-group" class="edit-box">
            <div id="edit-user-group-container" class="edit-container">
                <div id="edit-user-group-content" class="edit-content">
                    <asp:DropDownList ID="ddlUserGroup" DataTextField="agent_group" class="edit-input" DataValueField="agent_group" DataSourceID="dsGroup" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>

                    <a id="edit-user-group-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-user-group-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>

        <div id="edit-user-type" class="edit-box">
            <div id="edit-user-type-container" class="edit-container">
                <div id="edit-user-type-content" class="edit-content">
                    <select id="user_type" class="edit-input">
                        <option value="">(Select)</option>
                        <option value="Agent">Agent</option>
                        <option value="Supervisor">Supervisor</option>
                    </select>
                    <a id="edit-user-type-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-user-type-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>


        <div id="edit-first" class="edit-box">
            <div id="edit-first-container" class="edit-container">
                <div id="edit-first-content" class="edit-content">
                    <input type="text" id="first_name" class="edit-input" value="" />
                    <a id="edit-first-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-first-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>



        <div id="edit-last" class="edit-box">
            <div id="edit-last-container" class="edit-container">
                <div id="edit-last-content" class="edit-content">
                    <input type="text" id="last_name" class="edit-input" value="" />
                    <a id="edit-last-done" class="done-btn"><i class="fa fa-check"></i></a>
                    <a id="edit-last-close" class="close-btn"><i class="fa fa-times"></i></a>
                </div>
            </div>
        </div>


    </section>


    <link rel="stylesheet" href="client_users.css?1000" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            setupCategoryPopups();
        });

    </script>

    <script type="text/javascript" src="client_users.js?1000"></script>



</asp:Content>

