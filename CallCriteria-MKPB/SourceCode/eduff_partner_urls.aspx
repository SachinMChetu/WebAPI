<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="eduff_partner_urls.aspx.vb" Inherits="qs_partner_urls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Partner URLs</h2>
        <asp:Button ID="btnExport" runat="server" Text="Export" />
        <asp:GridView ID="gvURLS" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataSourceID="dsURLs">
            <Columns>
                <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" />
                <asp:BoundField DataField="Partner" HeaderText="Partner" SortExpression="Partner" />
                 <asp:BoundField DataField="Campaign" HeaderText="School" SortExpression="Campaign" />
                <asp:BoundField DataField="Website" HeaderText="Website" SortExpression="Website" />
                <asp:BoundField DataField="Last Date Reviewed" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Last Date Reviewed" ReadOnly="True" SortExpression="Last Date Reviewed" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsURLs" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>" 
            SelectCommand="select agent_group as [Group],agent as Partner, Campaign, Website, max(review_date) as [Last Date Reviewed] from vwForm where appname = 'edufficient' and website is not null group by Agent_group,Campaign, agent, website order by Agent_group,agent, Campaign, website "></asp:SqlDataSource>
    </section>
</asp:Content>

