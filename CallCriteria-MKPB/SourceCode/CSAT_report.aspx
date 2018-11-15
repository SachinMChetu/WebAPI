<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="CSAT_report.aspx.vb" Inherits="CSAT_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>CSAT Report</h2>

        <table>
            <tr>
                <td>Start:</td>
                <td>
                    <asp:TextBox ID="txtStart" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>End:</td>
                <td>
                    <asp:TextBox ID="txtEnd" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Scorecard:</td>
                <td>
                    <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="id" runat="server"></asp:DropDownList>
                     <asp:SqlDataSource ID="dsScorecards" SelectCommand=" select short_name, scorecards.id from scorecards   join userapps   on userapps.user_scorecard = scorecards.id where username = @username and short_name <> '' order by short_name"
                                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                                <SelectParameters>
                                    <asp:Parameter Name="Username" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnGO" runat="server" Text="GO" />
        <asp:Button ID="btnExport" runat="server" Text="Export" />

        <asp:GridView ID="gvCSAT" ShowFooter="true" FooterStyle-Font-Bold="true" CssClass="detailsTable" AllowSorting="true" DataSourceID="dsCSAT" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsCSAT" SelectCommand="getCSATData" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStart" Name="start_date" />
                <asp:ControlParameter ControlID="txtEnd" Name="end_date" />
                <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

