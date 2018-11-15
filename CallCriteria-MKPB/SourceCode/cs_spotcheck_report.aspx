<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="cs_spotcheck_report.aspx.vb" Inherits="cs_spotcheck_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <h2>Spot Check QA/QA Performance</h2>
                Start Date:
        <asp:TextBox ID="txtStart" CssClass="hasDatePicker" runat="server"></asp:TextBox>
                End Date:
        <asp:TextBox ID="txtEnd" CssClass="hasDatePicker" runat="server"></asp:TextBox>
                Scorecard:
        <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecard" DataTextField="short_name" DataValueField="id" runat="server"></asp:DropDownList>
                <asp:SqlDataSource ID="dsScorecard" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="select id, short_name from scorecards where high_priority = 1 order by short_name" runat="server">
                    <SelectParameters>
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Button ID="Button1" runat="server" Text="Go" />
                <br />

                <table>
                    <tr>
                        <td style="vertical-align:top">
                            <asp:GridView CssClass="detailsTable" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="true" FooterStyle-Font-Bold="true" ID="gvPerf" DataSourceID="dsPerf" runat="server">

                                <Columns>
                                    <asp:BoundField  DataField="REVIEWER" HeaderText="REVIEWER" SortExpression="REVIEWER" />
                                    <asp:BoundField  DataField="TOTAL CALLS" HeaderText="TOTAL CALLS" SortExpression="TOTAL CALLS" />
                                    <asp:BoundField  DataField="QAQA CALLS" HeaderText="QAQA CALLS" SortExpression="QAQA CALLS" />
                                    <asp:BoundField  DataField="QAQA W/MISSED ITEM" HeaderText="QAQA W/MISSED ITEM" SortExpression="QAQA W/MISSED ITEM" />
                                    <asp:BoundField  DataField="SPOTCHECK PERCENTAGE" HeaderText="SPOTCHECK PERCENTAGE" SortExpression="SPOTCHECK PERCENTAGE" />
                                    <asp:BoundField  DataField="CLEARED SPOTCHECKS" HeaderText="CLEARED SPOTCHECKS" SortExpression="CLEARED SPOTCHECKS" />
                                    <asp:BoundField  DataField="SPOTCHECKS W/DETERMINATION" HeaderText="SPOTCHECKS W/DETERMINATION" SortExpression="SPOTCHECKS W/DETERMINATION" />
                                    <asp:ButtonField CommandName="getDetails" DataTextField="QA MISTAKES"  HeaderText="QA MISTAKES" SortExpression="QA MISTAKES" />
                                    <asp:BoundField  DataField="% QA MISTAKES" HeaderText="% QA MISTAKES" SortExpression="% QA MISTAKES" />

                                    								
                                </Columns>

                            </asp:GridView>
                            <asp:SqlDataSource ID="dsPerf" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommandType="StoredProcedure"
                                SelectCommand="getQAQAPerformance2" runat="server">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtStart" Name="start" />
                                    <asp:ControlParameter ControlID="txtEnd" Name="end" />
                                    <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td style="vertical-align:top">
                            <asp:HiddenField ID="hdnSelectedQA" runat="server" />
                            <asp:GridView CssClass="detailsTable" ID="gvDetails" AutoGenerateColumns="false" DataSourceID="dsDetails" runat="server">
                                <Columns>
                                    <asp:BoundField  DataField="QA1" HeaderText="QA1" SortExpression="QA1" />
                                    <asp:HyperLinkField HeaderText="QA1 ID" DataTextField="QA1 CALL ID" DataNavigateUrlFields="QA1 CALL ID" DataNavigateUrlFormatString="/review/{0}" Target="_blank" />
                                    <asp:BoundField  DataField="QA2" HeaderText="QA2" SortExpression="QA2" />
                                    <asp:HyperLinkField HeaderText="QA2 ID" DataTextField="QA2 CALL ID" DataNavigateUrlFields="QA2 CALL ID" DataNavigateUrlFormatString="/review/{0}" Target="_blank" />
                                    <asp:BoundField  DataField="CLOSE REASON" HeaderText="CLOSE REASON" SortExpression="CLOSE REASON" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="dsDetails" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommandType="StoredProcedure"
                                SelectCommand="getQAQADetails" runat="server">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtStart" Name="start" />
                                    <asp:ControlParameter ControlID="txtEnd" Name="end" />
                                    <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                                    <asp:ControlParameter ControlID="hdnSelectedQA" Name="reviewer" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                </table>




            </ContentTemplate>
        </asp:UpdatePanel>

    </section>
</asp:Content>

