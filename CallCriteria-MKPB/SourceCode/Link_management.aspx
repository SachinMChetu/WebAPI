<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Link_management.aspx.vb" Inherits="Link_management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <h2>Link Management</h2>
                <p>&nbsp;</p>
                Filter Exceptions:
        <asp:DropDownList ID="ddlScorecard" DataSourceID="dsSC"
            DataTextField="appname" AppendDataBoundItems="true" AutoPostBack="true"
            DataValueField="appname" runat="server">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
                <asp:SqlDataSource ID="dsSC" SelectCommand="select appname 
                            from app_settings  where active =1  order by appname"
                    ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"></asp:SqlDataSource>

                <div style="float: right">
                    <asp:Button ID="Button2" runat="server" Text="Add" />
                </div>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="SqlDataSource2">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                        <asp:TemplateField HeaderText="appname" SortExpression="appname">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlScorecard" DataSourceID="dsSC" SelectedValue='<%# Bind("appname") %>'
                                    DataTextField="appname" AppendDataBoundItems="true" AutoPostBack="true"
                                    DataValueField="appname" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("appname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="add_remove" SortExpression="add_remove">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("add_remove") %>'>
                                    <asp:ListItem Value="">(Select)</asp:ListItem>
                                    <asp:ListItem Value="1">Add</asp:ListItem>
                                    <asp:ListItem Value="0">Remove</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList2" runat="server" Enabled="false" SelectedValue='<%# Bind("add_remove") %>'>
                                    <asp:ListItem Value="">(Select)</asp:ListItem>
                                    <asp:ListItem Value="1">Add</asp:ListItem>
                                    <asp:ListItem Value="0">Remove</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="link" HeaderText="link" SortExpression="link" />


                        <asp:BoundField DataField="url" HeaderText="url" SortExpression="url" />
                        <asp:BoundField DataField="link_order" HeaderText="link_order" SortExpression="link_order" />
                        <asp:CheckBoxField DataField="admin" HeaderText="admin" SortExpression="admin" />
                        <asp:CheckBoxField DataField="qa" HeaderText="qa" SortExpression="qa" />
                        <asp:CheckBoxField DataField="agent" HeaderText="agent" SortExpression="agent" />
                        <asp:CheckBoxField DataField="supervisor" HeaderText="supervisor" SortExpression="supervisor" />
                        <asp:CheckBoxField DataField="partner" HeaderText="partner" SortExpression="partner" />
                        <asp:CheckBoxField DataField="client" HeaderText="client" SortExpression="client" />
                        <asp:CheckBoxField DataField="client_calibrator" HeaderText="client_calibrator" SortExpression="client_calibrator" />
                        <asp:CheckBoxField DataField="qa_lead" HeaderText="QA Lead" SortExpression="qa_lead" />
                        <asp:CheckBoxField DataField="tango_tl" HeaderText="Tangl TL" SortExpression="tango_tl" />
                        <asp:CheckBoxField DataField="Trainee" HeaderText="Trainee" SortExpression="Trainee" />
                        <asp:CheckBoxField DataField="Manager" HeaderText="Manager" SortExpression="Manager" />
                        <asp:CheckBoxField DataField="Center_manager" HeaderText="Center_manager" SortExpression="Center_manager" />
                        <asp:CheckBoxField DataField="calibrator" HeaderText="calibrator" SortExpression="calibrator" />
                        <asp:CheckBoxField DataField="call_center" HeaderText="call_center" SortExpression="call_center" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                    DeleteCommand="DELETE FROM [link_exceptions] WHERE [id] = @id"
                    InsertCommand="INSERT INTO [link_exceptions] ([link], [url], [admin], [qa], [agent], [supervisor], [client], [qa_lead], [Trainee], [Manager], [Center_manager], [calibrator], [call_center], [appname], [add_remove]) VALUES (@link, @url, @admin, @qa, @agent, @supervisor, @client, @qa_lead, @Trainee, @Manager, @Center_manager, @calibrator, @call_center, @appname, @add_remove)"
                    SelectCommand="SELECT * FROM [link_exceptions] where 1  = case when @appname <> '' and appname <> @appname then 0 else 1 end order by appname, add_remove, link_order"
                    UpdateCommand="UPDATE [link_exceptions] SET [link] = @link, [url] = @url, [admin] = @admin, [qa] = @qa, [agent] = @agent, [supervisor] = @supervisor, 
            [client] = @client, [qa_lead] = @qa_lead, [Trainee] = @Trainee, [Manager] = @Manager, [partner] = @partner,[Center_manager] = @Center_manager, [calibrator] = @calibrator, 
            [call_center] = @call_center, client_calibrator=@client_calibrator,tango_tl=@tango_tl, [appname] = @appname, [add_remove] = @add_remove, link_order=@link_order WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlScorecard" Name="appname" ConvertEmptyStringToNull="false" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="link" Type="String" />
                        <asp:Parameter Name="url" Type="String" />
                        <asp:Parameter Name="admin" Type="Boolean" />
                        <asp:Parameter Name="qa" Type="Boolean" />
                        <asp:Parameter Name="agent" Type="Boolean" />
                        <asp:Parameter Name="supervisor" Type="Boolean" />
                        <asp:Parameter Name="client" Type="Boolean" />
                        <asp:Parameter Name="qa_lead" Type="Boolean" />
                        <asp:Parameter Name="Trainee" Type="Boolean" />
                        <asp:Parameter Name="Manager" Type="Boolean" />
                        <asp:Parameter Name="Center_manager" Type="Boolean" />
                        <asp:Parameter Name="calibrator" Type="Boolean" />
                        <asp:Parameter Name="call_center" Type="Boolean" />
                        <asp:Parameter Name="appname" Type="String" />
                        <asp:Parameter Name="add_remove" Type="Int32" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="link" Type="String" />
                        <asp:Parameter Name="url" Type="String" />
                        <asp:Parameter Name="admin" Type="Boolean" />
                        <asp:Parameter Name="qa" Type="Boolean" />
                        <asp:Parameter Name="agent" Type="Boolean" />
                        <asp:Parameter Name="supervisor" Type="Boolean" />
                        <asp:Parameter Name="client" Type="Boolean" />
                        <asp:Parameter Name="qa_lead" Type="Boolean" />
                        <asp:Parameter Name="Trainee" Type="Boolean" />
                        <asp:Parameter Name="Manager" Type="Boolean" />
                        <asp:Parameter Name="Center_manager" Type="Boolean" />
                        <asp:Parameter Name="calibrator" Type="Boolean" />
                        <asp:Parameter Name="call_center" Type="Boolean" />
                        <asp:Parameter Name="appname" Type="String" />
                        <asp:Parameter Name="add_remove" Type="Int32" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>


                <br />
                <br />



                <asp:Button ID="Button1" runat="server" Text="Add Link" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="SqlDataSource1">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                        <asp:BoundField DataField="link" HeaderText="link" SortExpression="link" />
                        <asp:BoundField DataField="url" HeaderText="url" SortExpression="url" />
                        <asp:BoundField DataField="link_order" HeaderText="link_order" SortExpression="link_order" />
                        <asp:CheckBoxField DataField="admin" HeaderText="admin" SortExpression="admin" />
                        <asp:CheckBoxField DataField="qa" HeaderText="qa" SortExpression="qa" />
                        <asp:CheckBoxField DataField="agent" HeaderText="agent" SortExpression="agent" />
                        <asp:CheckBoxField DataField="supervisor" HeaderText="supervisor" SortExpression="supervisor" />
                        <asp:CheckBoxField DataField="partner" HeaderText="partner" SortExpression="partner" />
                        <asp:CheckBoxField DataField="client" HeaderText="client" SortExpression="client" />
                        <asp:CheckBoxField DataField="client_calibrator" HeaderText="client_calibrator" SortExpression="client_calibrator" />
                        <asp:CheckBoxField DataField="qa_lead" HeaderText="qa_lead" SortExpression="qa_lead" />
                        <asp:CheckBoxField DataField="tango_tl" HeaderText="Tangl TL" SortExpression="tango_tl" />
                        <asp:CheckBoxField DataField="Trainee" HeaderText="Trainee" SortExpression="Trainee" />
                        <asp:CheckBoxField DataField="Manager" HeaderText="Manager" SortExpression="Manager" />
                        <asp:CheckBoxField DataField="Center_manager" HeaderText="Center_manager" SortExpression="Center_manager" />
                        <asp:CheckBoxField DataField="calibrator" HeaderText="calibrator" SortExpression="calibrator" />
                        <asp:CheckBoxField DataField="call_center" HeaderText="call_center" SortExpression="call_center" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                    DeleteCommand="DELETE FROM [link_list] WHERE [id] = @id"
                    InsertCommand="INSERT INTO [link_list] ([link], [admin], [qa], [agent], [supervisor], [client], [qa_lead], [Trainee], [Manager], [Center_manager], [calibrator], [call_center]) VALUES (@link, @admin, @qa, @agent, @supervisor, @client, @qa_lead, @Trainee, @Manager, @Center_manager, @calibrator, @call_center)"
                    SelectCommand="SELECT * FROM [link_list] order by link_order"
                    UpdateCommand="UPDATE [link_list] SET [link] = @link, url=@url, client_calibrator=@client_calibrator,tango_tl=@tango_tl, link_order=@link_order, [admin] = @admin, [qa] = @qa, [partner] = @partner,[agent] = @agent, [supervisor] = @supervisor, [client] = @client, [qa_lead] = @qa_lead, [Trainee] = @Trainee, [Manager] = @Manager, [Center_manager] = @Center_manager, [calibrator] = @calibrator, [call_center] = @call_center WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="link" Type="String" />
                        <asp:Parameter Name="admin" Type="Int32" />
                        <asp:Parameter Name="qa" Type="Int32" />
                        <asp:Parameter Name="agent" Type="Int32" />
                        <asp:Parameter Name="supervisor" Type="Int32" />
                        <asp:Parameter Name="client" Type="Int32" />
                        <asp:Parameter Name="qa_lead" Type="Int32" />
                        <asp:Parameter Name="Trainee" Type="Int32" />
                        <asp:Parameter Name="Manager" Type="Int32" />
                        <asp:Parameter Name="default_order" Type="Int32" />
                        <asp:Parameter Name="Inactive" Type="Int32" />
                        <asp:Parameter Name="Center_manager" Type="Int32" />
                        <asp:Parameter Name="calibrator" Type="Int32" />
                        <asp:Parameter Name="call_center" Type="Int32" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="link" Type="String" />
                        <asp:Parameter Name="admin" Type="Int32" />
                        <asp:Parameter Name="qa" Type="Int32" />
                        <asp:Parameter Name="agent" Type="Int32" />
                        <asp:Parameter Name="supervisor" Type="Int32" />
                        <asp:Parameter Name="client" Type="Int32" />
                        <asp:Parameter Name="qa_lead" Type="Int32" />
                        <asp:Parameter Name="Trainee" Type="Int32" />
                        <asp:Parameter Name="Manager" Type="Int32" />
                        <asp:Parameter Name="default_order" Type="Int32" />
                        <asp:Parameter Name="Inactive" Type="Int32" />
                        <asp:Parameter Name="Center_manager" Type="Int32" />
                        <asp:Parameter Name="calibrator" Type="Int32" />
                        <asp:Parameter Name="call_center" Type="Int32" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>

            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
</asp:Content>

