<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="all_agent_summary.aspx.vb" Inherits="agent_summary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">




    <section class="main-container">
        <h1 class="section-title"><i class="fa fa-desktop"></i>
            All Agent Score Summary</h1>
        <div class="general-filter">
            <div class="yellow-container">
                <asp:Button ID="btnExport" CssClass="secondary-cta" Style="margin-left: 10px;" runat="server" Text="Export to Excel" />
                <asp:Button ID="btnAgentReport" CssClass="secondary-cta" Style="margin-left: 10px;" runat="server" Text="Go" />

                <div class="field-holder-right" style="width: 200px;">
                    <asp:CheckBox ID="chk30Only" Text="Most Recent 30 Only" runat="server" />
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>
                <div class="field-holder-right">

                    <asp:SqlDataSource ID="dsAgentGroup" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct lower([AGENT_GROUP]) as AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and agent_group is not null and agent_group <> '' order by lower([AGENT_GROUP]) ">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="dsAgent" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand=" select distinct agent from [form_notifications] join vwForm on vwForm.f_id = form_id
                                          WHERE date_closed is null and vwForm.appname = @appname
                                          ORDER BY Agent">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="field-holder-right">

                    <asp:DropDownList ID="ddlGroup" AppendDataBoundItems="true" DataSourceID="dsAgentGroup"
                        DataTextField="Agent_group" DataValueField="Agent_group" runat="server">
                        <asp:ListItem Text="All" Value=" "></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <!-- close date-holder -->

            </div>
            <!-- close yellow-container -->
        </div>
        <!-- close general-filter -->



        <asp:Label ID="lblThroughDate" Font-Bold="true" runat="server" Text=""></asp:Label>


        <div class="table-outline">

            <asp:GridView ID="GridView1" AllowSorting="true" DataSourceID="dsSummary" AutoGenerateColumns="false" runat="server" GridLines="None">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="agent_name" HeaderText="Agent Name" DataNavigateUrlFormatString="agent_summary.aspx?agent={0}" DataTextField="Agent_name" SortExpression="Agent_name" />
                    <asp:BoundField DataField="TotalCalls" HeaderText="Total Calls" SortExpression="TotalCalls" />
                    <asp:BoundField DataField="AvgScore" HeaderText="Average Score" SortExpression="AvgScore" />
                    <asp:BoundField DataField="TotalPassed" HeaderText="Total Passed" SortExpression="TotalPassed" />
                    <asp:BoundField DataField="Oldest_Call_Date" HeaderText="Oldest Call Date" SortExpression="Oldest_Call_Date" />
                    <asp:BoundField DataField="Newest_Call_Date" HeaderText="Newest Call Date" SortExpression="Newest_Call_Date" />

                </Columns>

            </asp:GridView>

            <asp:SqlDataSource ID="dsSummary" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                runat="server" SelectCommand="GetAllAgentSummary">
                <SelectParameters>
                    <asp:Parameter Name="startdate" />
                    <asp:Parameter Name="enddate" />
                    <asp:ControlParameter Name="top30" ControlID="chk30Only" />
                    <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                    <asp:ControlParameter Name="agent_group" ControlID="ddlGroup" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </section>
    <script type="text/javascript">
        setupCalendars();
    </script>
</asp:Content>
