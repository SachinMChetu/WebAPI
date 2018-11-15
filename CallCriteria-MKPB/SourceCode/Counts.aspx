<%@ Page Title="Counts" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="Counts.aspx.vb" Inherits="Counts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container dash-modules general-button">
        <h2>QA Counts Report</h2>
        <table>
            <tr>
                <td><a href="daily_work.aspx" title="Back to Daily Work"><i class="fa fa-backward" style="color: black;"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td>Select App:
                    <asp:DropDownList ID="ddlApps" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="appname" DataValueField="ID" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select scorecards.appname + ' (' + short_name + ')' as appname, scorecards.ID  from scorecards  
                        join app_settings   on app_settings.appname = scorecards.appname 
                        where app_settings.active=1 and scorecards.active = 1 order by scorecards.appname"
                        runat="server"></asp:SqlDataSource>
                </td>
                <td>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox></td>

                <td>
                    <asp:Button ID="btnAgentReport" runat="server" Text="Go" />
                </td>
                <td>
                    <asp:Button ID="btnSupeExportxp" Visible="false" runat="server" Text="Export to Excel" />
                </td>
            </tr>
        </table>

        <table>
            <tr>
                <td valign="top">
                    <h4>Call In Pending table -- load within the hour or EOD.</h4>
                    <div style="max-height: 500px; overflow: scroll;">
                        <asp:GridView ID="GridView1" DataSourceID="SqlDataSource1" CssClass="detailsTable" runat="server"></asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="getpendingscorecards" SelectCommandType="StoredProcedure" runat="server"></asp:SqlDataSource>
                    </div>
                </td>
                <td valign="top">


                    <h4>Calls posted to API</h4>
                    <div style="max-height: 500px; overflow: scroll;">
                        <asp:GridView ID="GridView2" DataSourceID="SqlDataSource2" CssClass="detailsTable" runat="server"></asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="getpostedips" SelectCommandType="StoredProcedure" runat="server">
                            <selectparameters>
                            <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date"
                                PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/1900" Name="end_date"
                                PropertyName="Text" />
                        </selectparameters>
                        </asp:SqlDataSource>
                    </div>
                </td>
            </tr>

        </table>

        <br />

        <h4>Calls Loaded to Working/Ready tables - by Call Date</h4>
        <asp:GridView ID="gvCounts" DataSourceID="dsCounts" FooterStyle-Font-Bold="true" ShowFooter="true" CssClass="detailsTable" GridLines="None" runat="server">
        </asp:GridView>
        <asp:SqlDataSource ID="dsCounts" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getCounts" SelectCommandType="StoredProcedure">
            <selectparameters>
                <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/1900" Name="end_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="ddlApps" Name="scorecard" />
            </selectparameters>
        </asp:SqlDataSource>

        <br />
        <br />
        <h4>Calls Loaded to Working/Ready tables - by Loaded Date</h4>
        <asp:GridView ID="gvCOunts2" DataSourceID="dsCOunts2" FooterStyle-Font-Bold="true" ShowFooter="true" CssClass="detailsTable" GridLines="None" runat="server">
        </asp:GridView>
        <asp:SqlDataSource ID="dsCOunts2" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getCountsLoaded"  SelectCommandType="StoredProcedure">
            <selectparameters>
                <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/1900" Name="end_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="ddlApps" Name="scorecard" />
            </selectparameters>
        </asp:SqlDataSource>


         <br />
        <br />
        <h4>Calls Loaded to Working/Ready tables - by Review Date</h4>
        <asp:GridView ID="gvReview" DataSourceID="SqlDataSource3" FooterStyle-Font-Bold="true" ShowFooter="true" CssClass="detailsTable" GridLines="None" runat="server">
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getCountsReview"  SelectCommandType="StoredProcedure">
            <selectparameters>
                <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/1900" Name="end_date"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="ddlApps" Name="scorecard" />
            </selectparameters>
        </asp:SqlDataSource>



    </section>
    <script type="text/javascript">
        setupCalendars();
    </script>
</asp:Content>
