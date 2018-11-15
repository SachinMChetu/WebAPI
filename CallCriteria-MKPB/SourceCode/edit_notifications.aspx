<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_notifications.aspx.vb" Inherits="edit_scorecard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button ">


        <h2>Edit Notification Flow</h2>

        <table>

            <tr>
                <td>Profile Name:</td>
                <td>
                    <asp:DropDownList ID="ddlSC" DataSourceID="dsApps" DataTextField="profile_name" DataValueField="id" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select id,profile_name from notification_profiles order by profile_name"
                        runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>Profile Description:
                </td>
                <td>
                    <asp:TextBox ID="txtProfile" TextMode="MultiLine" Rows="5" Columns="50" runat="server"></asp:TextBox><asp:Button ID="btnUpdate" runat="server" Text="Update" />
                </td>
            </tr>
        </table>



        <br />





        <br />
        <br />
        New Profile:
        <asp:TextBox ID="txtNewProfile" runat="server"></asp:TextBox>
        from profile: 
        <asp:DropDownList ID="ddlFrom" DataSourceID="dsApps" DataTextField="profile_name" DataValueField="id" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
        </asp:DropDownList><asp:Button ID="btnNewProfile" runat="server" Text="Add Profile" />

        <br />
        <h2>Notifications</h2>
        <asp:Button ID="btnNew" runat="server" Text="Add" />
        Row -- user's role  Column -- assignable to role
        <asp:GridView ID="gvNotifications" DataSourceID="dsNotifications" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:TemplateField HeaderText="role" SortExpression="role">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRole" runat="server" SelectedValue='<%# Bind("role") %>'>
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem>Admin</asp:ListItem>
                            <asp:ListItem>Account Manager</asp:ListItem>
                            <asp:ListItem>Client</asp:ListItem>
                            <asp:ListItem>Client Calibrator</asp:ListItem>
                            <asp:ListItem>Manager</asp:ListItem>
                            <asp:ListItem>Supervisor</asp:ListItem>
                            <asp:ListItem>Calibrator</asp:ListItem>
                            <asp:ListItem>Agent</asp:ListItem>
                            <asp:ListItem>QA</asp:ListItem>
                            <asp:ListItem>QA Lead</asp:ListItem>
                            <asp:ListItem>Partner</asp:ListItem>
                            <asp:ListItem>Tango TL</asp:ListItem>
                            <asp:ListItem>Center Manager</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="admin" SortExpression="admin">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" SelectedValue='<%# Bind("admin") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" SelectedValue='<%# Bind("admin") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="account manager" SortExpression="account_manager">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlAM" SelectedValue='<%# Bind("account_manager") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlAM" SelectedValue='<%# Bind("account_manager") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Client" SortExpression="client">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" SelectedValue='<%# Bind("client") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList2" SelectedValue='<%# Bind("client") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Client Calibrator" SortExpression="client_calibrator">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlclient_calibrator" SelectedValue='<%# Bind("client_calibrator") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlclient_calibrator" SelectedValue='<%# Bind("client_calibrator") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manager" SortExpression="manager">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList3" SelectedValue='<%# Bind("manager") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList3" SelectedValue='<%# Bind("manager") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Supervisor" SortExpression="supervisor">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList4" SelectedValue='<%# Bind("supervisor") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList4" SelectedValue='<%# Bind("supervisor") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Agent" SortExpression="agent">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList5" SelectedValue='<%# Bind("agent") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList5" SelectedValue='<%# Bind("agent") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QA" SortExpression="QA">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList6" SelectedValue='<%# Bind("QA") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList6" SelectedValue='<%# Bind("QA") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tango TL" SortExpression="tango_tl">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlTT" SelectedValue='<%# Bind("tango_tl") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTT" SelectedValue='<%# Bind("tango_tl") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Calibrator" SortExpression="calibrator">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList7" SelectedValue='<%# Bind("calibrator") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList7" SelectedValue='<%# Bind("calibrator") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="TL" SortExpression="TL">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList8" SelectedValue='<%# Bind("TL") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList8" SelectedValue='<%# Bind("TL") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CM" SortExpression="CM">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList9" SelectedValue='<%# Bind("CM") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList9" SelectedValue='<%# Bind("CM") %>' Enabled="false" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsNotifications" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            DeleteCommand="DELETE FROM [notification_steps] WHERE [id] = @id"
            InsertCommand="INSERT INTO [notification_steps] ([scorecard], [role],  [admin], [client], [manager], [supervisor], [agent], [QA], [calibrator], 
            [TL], [CM]) VALUES (@scorecard, @role, @admin, @client, @manager, @supervisor, @agent, @QA, @calibrator, @TL, @CM)"
            SelectCommand="SELECT * FROM [notification_steps] where profile_id=@profile_id "
            UpdateCommand="UPDATE [notification_steps] SET  [role] = @role, [admin] = @admin, [client] = @client, [manager] = @manager, 
            [supervisor] = @supervisor, [agent] = @agent, [QA] = @QA,  [calibrator] = @calibrator, [TL] = @TL, 
            [CM] = @CM, tango_tl=@tango_tl, account_manager=@account_manager WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="profile_id" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="role" Type="String" />
                <asp:Parameter Name="admin" Type="Boolean" />
                <asp:Parameter Name="client" Type="Boolean" />
                <asp:Parameter Name="manager" Type="Boolean" />
                <asp:Parameter Name="supervisor" Type="Boolean" />
                <asp:Parameter Name="agent" Type="Boolean" />
                <asp:Parameter Name="QA" Type="Boolean" />
                <asp:Parameter Name="calibrator" Type="Boolean" />
                <asp:Parameter Name="TL" Type="Boolean" />
                <asp:Parameter Name="CM" Type="Boolean" />
                <asp:ControlParameter ControlID="ddlSC" Name="scorecard" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="role" Type="String" />
                <asp:Parameter Name="admin" Type="Boolean" />
                <asp:Parameter Name="client" Type="Boolean" />
                <asp:Parameter Name="manager" Type="Boolean" />
                <asp:Parameter Name="supervisor" Type="Boolean" />
                <asp:Parameter Name="agent" Type="Boolean" />
                <asp:Parameter Name="QA" Type="Boolean" />
                <asp:Parameter Name="calibrator" Type="Boolean" />
                <asp:Parameter Name="TL" Type="Boolean" />
                <asp:Parameter Name="CM" Type="Boolean" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>





    </section>
</asp:Content>

