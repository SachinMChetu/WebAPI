<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edufficient_agents.aspx.vb" Inherits="edufficient_agents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Update Agents/Partners</h2>
        <br />
        <asp:TextBox ID="txtNewPartner" runat="server"></asp:TextBox><asp:Button ID="btnAddPartner" runat="server" Text="Add Partner" /> *Official Partner Name -- double check spelling
        <asp:GridView ID="gvAgent" CssClass="detailsTable" DataSourceID="dsAgent" AutoGenerateColumns="false" runat="server" DataKeyNames="ID">
            <Columns>
                <asp:BoundField DataField="source_name" HeaderText="Source" SortExpression="source_name" />
                <asp:TemplateField HeaderText="Destination">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlDest" DataSourceID="dsSources" DataTextField="Sources Company Name" AppendDataBoundItems="true" DataValueField="Sources Company Name"
                            SelectedValue='<%#Eval("destination_name") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsAgent" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select id, source_name, destination_name from eduff_agents union all 
        select distinct 0, agent, '' from xcc_report_new where agent not in (select [sources company name] from eduff_cehe_names) and appname = 'edufficient' and agent is not null and agent <> '' order by id"></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsSources" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct [Sources Company Name] from eduff_cehe_names order by [Sources Company Name]"></asp:SqlDataSource>


        <asp:Button ID="btnCreateAgents" runat="server" Text="Create Agents" />
    </section>
</asp:Content>

