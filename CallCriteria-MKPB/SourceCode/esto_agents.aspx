<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="esto_agents.aspx.vb" Inherits="gvd_agents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container">
        <asp:UpdatePanel runat="server" ID="up1">
            <ContentTemplate>
                <div class="table-outline">
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" s
                        DataKeyNames="id" DataSourceID="dsGVDAgents" GridLines="None">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" />
                            <asp:BoundField DataField="Agent" ReadOnly="true" HeaderText="Agent" SortExpression="Agent" />
                            <asp:TemplateField HeaderText="Group" SortExpression="[agent_group]">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("agent_group")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGroupList" DataSourceID="dsGroupList" AppendDataBoundItems="true"
                                        DataTextField="agent_group" DataValueField="agent_group" runat="server" SelectedValue='<%#Bind("agent_group")%>'>
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsGroupList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select distinct [agent_group] from esto_agents order by [agent_group]"
                                        runat="server"></asp:SqlDataSource>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="dsGVDAgents" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    DeleteCommand="DELETE FROM [esto_agents] WHERE [id] = @id" InsertCommand="INSERT INTO [esto_agents] ([Agent], [Group]) VALUES (@Agent, @Group)"
                    SelectCommand="SELECT * FROM [esto_agents]" UpdateCommand="UPDATE [esto_agents] SET [agent_group] = @agent_group WHERE [id] = @id; update xcc_report_new set agent_group = @agent_group from esto_agents where xcc_report_new.agent = @agent and appname in ('esto','estobk')">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Agent" Type="String" />
                        <asp:Parameter Name="Group" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Agent" Type="String" />
                        <asp:Parameter Name="agent_group" Type="String" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
</asp:Content>
