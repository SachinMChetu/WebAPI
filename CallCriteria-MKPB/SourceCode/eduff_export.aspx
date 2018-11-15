<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="eduff_export.aspx.vb" Inherits="eduff_export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="maiin-container dash-modules general-button">
        <h2>Edufficient Details Export</h2>

        <table>
            <tr>
                <td>Start Date</td>
                <td>
                    <asp:TextBox ID="txtStart" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>End Date</td>
                <td>
                    <asp:TextBox ID="txtEnd" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Group</td>
                <td>
                    <asp:DropDownList ID="ddlGroup" DataSourceID="dsgroups" DataTextField="agent_group" DataValueField="agent_group" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Agent</td>
                <td>
                    <asp:Listbox ID="ddlAgent" SelectionMode="Multiple"  DataSourceID="dsAgents" DataTextField="agent" DataValueField="agent" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:Listbox></td>
            </tr>
            <tr>
                <td>Campaign</td>
                <td>
                    <asp:DropDownList ID="ddlCampaign" DataSourceID="dsCampaigns" DataTextField="campaign" DataValueField="campaign" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Scorecard</td>
                <td>
                    <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecard" DataTextField="scorecard_name" DataValueField="scorecard" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <asp:Button ID="btnExport" runat="server" Text="Export" />
        <asp:SqlDataSource ID="dsgroups" SelectCommand="select distinct agent_group from vwForm join userapps on scorecard = user_scorecard where call_date > dateadd(d, -90, getdate()) and username=@username and agent_group != '' and agent_group is not null order by agent_group" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsAgents" SelectCommand="select distinct agent from vwForm join userapps on scorecard = user_scorecard where call_date > dateadd(d, -90, getdate()) and username=@username and agent != '' and agent is not null order by agent" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsCampaigns" SelectCommand="select distinct campaign from vwForm join userapps on scorecard = user_scorecard where call_date > dateadd(d, -90, getdate()) and username=@username and campaign != '' and campaign is not null order by campaign" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsScorecard" SelectCommand="select distinct scorecard, scorecard_name from vwForm join userapps on scorecard = user_scorecard where call_date > dateadd(d, -90, getdate()) and username=@username order by scorecard_name" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

         <asp:GridView ID="gvDetails" GridLines="None" AlternatingRowStyle-BackColor="#ebf2fa" RowStyle-BackColor="White" 
            HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#334a5a"  runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsDetails" runat="server"></asp:SqlDataSource>

    </section>
</asp:Content>

