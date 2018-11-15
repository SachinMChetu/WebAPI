<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="currently_working.aspx.vb" Inherits="currently_working" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Currently Working - Last 60 Minutes</h2>
        <div class="table-outline">
            <asp:SqlDataSource ID="dsWorking" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select count(*) as [Number Procesed], short_name as Scorecard, reviewer as QA from vwForm join scorecards on scorecards.id = scorecard 
                where review_date > dateadd(minute,-60,dbo.getMTDate())  group by reviewer, short_name order by short_name "></asp:SqlDataSource>
            <asp:GridView ID="gvWorking" runat="server" AllowSorting="true" GridLines="None" CssClass="detailsTable" DataSourceID="dsWorking"></asp:GridView>
        </div>
    </section>
</asp:Content>

