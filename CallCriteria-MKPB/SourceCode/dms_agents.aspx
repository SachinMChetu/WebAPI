<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="dms_agents.aspx.vb" Inherits="Q3M_team" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
        <h2>DMS Agent Editor</h2>
        <div class="table-outline">

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvAgents" DataSourceID="dsAgents" DataKeyNames="id" GridLines="None"
                        runat="server" AutoGenerateColumns="False" AllowSorting="True">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:BoundField DataField="AGENT" ReadOnly="true" HeaderText="AGENT" SortExpression="AGENT" />
                            <asp:TemplateField HeaderText="AGENT GROUP"
                                SortExpression="AGENT_GROUP">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%#Bind("agent_group") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGroup" DataSourceID="dsGroups" DataTextField="agent_group" DataValueField="agent_group" SelectedValue='<%#Bind("agent_group") %>' AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="dsGroups" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select distinct user_group COLLATE sql_latin1_general_cp1_cs_as as agent_group from (
                            select distinct user_group COLLATE sql_latin1_general_cp1_cs_as as user_group from userextrainfo where username in (select username from userapps where appname in (select appname from userapps where username = @username)) 
                            and user_role in ('Supervisor','Manager','Client') and user_group is not null
                            union all 
                            select distinct username COLLATE sql_latin1_general_cp1_cs_as as user_group   from userextrainfo where username in (select username from userapps where appname in (select appname from userapps where username =@username)) 
                            and user_role in ('Supervisor','Manager','Client') and user_group is not null) a
                            where user_group != ''
                            order by user_group COLLATE sql_latin1_general_cp1_cs_as"
                                        runat="server"></asp:SqlDataSource>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                ReadOnly="True" SortExpression="id" />

                            <asp:TemplateField SortExpression="special" HeaderText="Non-EDMC">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" AutoPostBack="true" Checked='<%#Eval("special") %>' OnCheckedChanged="CheckBox1_CheckedChanged" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsAgents" runat="server"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        DeleteCommand="DELETE FROM [dms_agents] WHERE [id] = @id"
                        InsertCommand="INSERT INTO [dms_agents] ([AGENT], [AGENT_GROUP]) VALUES (@AGENT, @AGENT_GROUP)"
                        SelectCommand="SELECT * FROM [dms_agents] order by agent"
                        UpdateCommand="update xcc_report_new set agent_group=@agent_group where agent=@agent;
                                        update userextrainfo set user_group=@agent_group where username=@agent;
                                        UPDATE [dms_agents] SET [AGENT_GROUP] = @AGENT_GROUP WHERE [id] = @id">
                        <DeleteParameters>
                            <asp:Parameter Name="id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="AGENT" Type="String" />
                            <asp:Parameter Name="AGENT_GROUP" Type="String" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="AGENT" Type="String" />
                            <asp:Parameter Name="AGENT_GROUP" Type="String" />
                            <asp:Parameter Name="id" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
