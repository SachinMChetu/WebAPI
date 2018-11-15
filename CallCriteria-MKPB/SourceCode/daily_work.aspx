<%@ Page Title="Daily Work" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="daily_work.aspx.vb" Inherits="daily_work" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div>
                    <h2>Work from <%=DateAdd(DateInterval.Day, -1, Today().AddHours(-6)).ToShortDateString%>  </h2>
                    Forecast:
                    <asp:Label ID="lblMTD" Font-Bold="true" runat="server" Text=""></asp:Label><br />
                    Auto refresh
            <input type="checkbox" checked="checked" id="page_refresh" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
            Exclude Websites
            <asp:CheckBox ID="chkNoWeb" AutoPostBack="true" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
            My Scorecards Only
            <asp:CheckBox ID="chkMyScorecards" AutoPostBack="true" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                Scorecard Manager:
                    <asp:DropDownList ID="ddlAM" runat="server" DataSourceID="dsAM" DataTextField="account_manager" DataValueField="account_manager" AppendDataBoundItems="true" AutoPostBack="true">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        <asp:ListItem Text="Unassigned" Value="0"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="dsAM" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct account_manager from scorecards where account_manager is not null and account_manager <> '' order by account_manager"></asp:SqlDataSource>


                    <br />
                    <br />

                    <asp:GridView ID="gvDaily" CssClass="detailsTable" DataSourceID="dsDaily" RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="false"
                        ShowFooter="true" AllowSorting="true" FooterStyle-Font-Bold="true" runat="server">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="sc_id" SortExpression="appname" HeaderText="Appname" DataNavigateUrlFormatString="counts.aspx?appname={0}" DataTextField="Appname" />
                            <asp:HyperLinkField DataNavigateUrlFields="sc_id,appname" SortExpression="scorecard" HeaderText="Scorecard" DataNavigateUrlFormatString="scorecard_q_manager.aspx?ID={0}&appname={1}" DataTextField="Scorecard" />
                            <asp:BoundField DataField="Team Lead" SortExpression="Team Lead" HeaderText="Team Lead" />
                            <asp:BoundField DataField="Number QA Assigned" SortExpression="Number QA Assigned" HeaderText="Number QA Assigned" />
                            <asp:BoundField DataField="MTD Calls Completed" SortExpression="MTD Calls Completed" HeaderText="MTD Calls Completed" />
                            <asp:BoundField DataField="MTD Minutes Processed" SortExpression="MTD Minutes Processed" HeaderText="MTD Minutes Processed" />
                            <asp:BoundField DataField="MTD Revenue Generated" SortExpression="MTD Revenue Generated" HeaderText="MTD Revenue Generated" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Minutes Processed" SortExpression="Minutes Processed" HeaderText="Minutes Processed" />
                            <asp:BoundField DataField="Revenue Generated" SortExpression="Revenue Generated" HeaderText="Revenue Generated" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Number Loaded" SortExpression="Number Loaded" HeaderText="Number Loaded" />
                            <asp:BoundField DataField="Number Processed" SortExpression="Number Processed" HeaderText="Number Processed" />
                            <asp:BoundField DataField="Number Pending Review" SortExpression="Number Pending Review" HeaderText="Number Pending Review" />
                            <asp:BoundField DataField="Number Missing Audio" SortExpression="Number Missing Audio" HeaderText="Number Missing Audio" />
                            <asp:BoundField DataField="Last Loaded Date" SortExpression="Last Loaded Date" HeaderText="Last Loaded Date" />
                            <asp:BoundField DataField="Last Reviewed Date" SortExpression="Last Reviewed Date" HeaderText="Last Reviewed Date" />
                            <asp:BoundField DataField="Oldest Pending Date" SortExpression="Oldest Pending Date" HeaderText="Oldest Pending Date" />
                        </Columns>

                    </asp:GridView>
                    <asp:SqlDataSource ID="dsDaily" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                        SelectCommand="getDailyWork" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="no_web" ControlID="chkNoWeb" />
                            <asp:ControlParameter Name="my_scorecard" ControlID="chkMyScorecards" />
                            <asp:Parameter Name="username" />
                            <asp:ControlParameter ControlID="ddlAM" PropertyName="SelectedValue" Name="AM" ConvertEmptyStringToNull="false" />
                        </SelectParameters>
                    </asp:SqlDataSource>


                    <script type="text/javascript">
                        setInterval(function () {
                            if ($('#page_refresh').attr("checked") == "checked")
                                window.location.reload(1);
                        }, 300000);
                    </script>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
</asp:Content>
