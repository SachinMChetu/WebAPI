<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="high_priority_scorecards.aspx.vb" Inherits="high_priority_scorecards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <meta http-equiv="refresh" content="120">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button ">
        <h2>High Priority Scorecards</h2>
        <asp:GridView ID="gvHiPro" AllowSorting="true"  CssClass="detailsTable" DataSourceID="dsHiPro" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsHiPro" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
            SelectCommand="exec GetHiProCards" runat="server"></asp:SqlDataSource>
        <br /><br />
        <h4>Last Working</h4>
        <asp:GridView ID="gvLastWorking" AllowSorting="true" CssClass="detailsTable" DataSourceID="dsLastWorking"  runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsLastWorking" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
            SelectCommand="getWorkingHighPriorityQAs" runat="server"></asp:SqlDataSource>


<%--   <br /><br />
        <h4>Last Served</h4>
        <asp:GridView ID="gvLastServed" CssClass="detailsTable" DataSourceID="dsLastServed"  runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsLastServed" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
            SelectCommand="select top 100 * from next_item order by id desc" runat="server"></asp:SqlDataSource>
--%>

    </section>
</asp:Content>

