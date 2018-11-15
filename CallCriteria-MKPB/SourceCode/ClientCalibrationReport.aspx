<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="ClientCalibrationReport.aspx.vb" Inherits="ClientCalibrationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Client Calibration Report</h2>

        <asp:GridView ID="gvCalibrations" CssClass="detailsTable" DataSourceID="dsCalibrations" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="Call ID" DataNavigateUrlFormatString="/review/{0}" DataTextField="Call ID" />
                <asp:BoundField DataField="Assigned To" HeaderText="Assigned To" SortExpression="Assigned To" />
                <asp:BoundField DataField="Reviewer" HeaderText="Reviewer" SortExpression="Reviewer" />
                <asp:BoundField DataField="Review Date" HeaderText="Review Date" SortExpression="Review Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="Score" HeaderText="Score" SortExpression="Score" />

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsCalibrations" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getWeeklyClientDetails" SelectCommandType="StoredProcedure" runat="server">

            <SelectParameters>
                <asp:Parameter Name="username" />
                <asp:QueryStringParameter Name="week_ending" QueryStringField="week_ending" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

