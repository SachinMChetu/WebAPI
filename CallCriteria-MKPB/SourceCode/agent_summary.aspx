<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="agent_summary.aspx.vb" Inherits="agent_summary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container dash-modules general-button ">
        <h1 class="section-title">Agent Score Summary
        </h1>
        <div class="general-filter">
            <div class="">
                <table style="float: right;">
                    <tr>
                        <td>
                            <asp:CheckBox Visible="false" ID="chk30Only" Text="Most Recent 30 Only" runat="server" />
                            <asp:Button ID="btnSupeExportxp" runat="server" Text="Export to Excel" />
                            <asp:Button ID="btnAgentReport" CssClass="secondary-cta" runat="server" Text="Go" />
                            <button type="button" onclick="javascript:window.print()">Print</button>
                        </td>
                    </tr>
                </table>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <asp:DropDownList ID="ddlAgents" DataSourceID="dsAgents" DataTextField="agent" AutoPostBack="true"  AppendDataBoundItems="true"  DataValueField="agent" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="dsAgents" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select distinct agent from vwForm where call_date between @start and @end 
                                          and scorecard =@scorecard and agent is not null
                                          order by agent"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter Name="scorecard" ControlID="ddlScorecard" />
                            <asp:ControlParameter Name="start" ControlID="txtAgentStart" />
                            <asp:ControlParameter Name="end" ControlID="txtAgentEnd" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                  <div class="field-holder-right">
                    <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecard" AutoPostBack="true" DataTextField="short_name" AppendDataBoundItems="true" DataValueField="id" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="dsScorecard" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select scorecards.id, short_name from scorecards join userapps on userapps.user_scorecard = scorecards.id where username = @username order by short_name"
                        runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" DefaultValue="graspy" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>


            </div>
            <!-- close yellow-container -->
        </div>
        <!-- close general-filter -->
        <asp:Label ID="lblThroughDate" Font-Bold="true" runat="server" Text=""></asp:Label>


        <asp:Panel runat="server" ID="pnlAS">

            <div class="">
                <asp:FormView ID="fvSummary" DataSourceID="dsSummary" runat="server">
                    <EmptyDataTemplate>No data found</EmptyDataTemplate>
                    <ItemTemplate>
                        <table class="detailsTable">
                            <thead>
                                <tr>
                                    <td>Calls Scored
                                    </td>
                                     <td>Calls Failed
                                    </td>
                                    <td>Average Score
                                    </td>
                                    <td>Oldest Call Date
                                    </td>
                                    <td>Newest Call Date
                                    </td>
                                </tr>
                            </thead>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%#eval("TotalCalls") %>'></asp:Label>
                                </td>
                                 <td>
                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("TotalFailed") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text='<%# formatnumber(eval("AvgScore"),2) %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text='<%#format(eval("MinCallDate"),"MM/dd/yyyy") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text='<%#Format(Eval("MaxCallDate"), "MM/dd/yyyy")%>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
                <asp:SqlDataSource ID="dsSummary" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    runat="server" SelectCommand="getAgentSummary">
                    <SelectParameters>


                        <asp:ControlParameter Name="startdate" ControlID="txtAgentStart" />
                        <asp:ControlParameter Name="enddate" ControlID="txtAgentEnd" />
                        <asp:ControlParameter Name="scorecard" ControlID="ddlScorecard" />
                        <asp:ControlParameter Name="agent" ControlID="ddlAgents" />
                        <asp:ControlParameter Name="top30" ControlID="chk30Only" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <br />

                <asp:Button ID="btnAll" runat="server" Visible="false" Text="All Missed Last 30" CssClass="main-cta" />
                <table class="detailsTable">
                    <thead>
                        <tr>
                            <td>Question</td>
                            <td>Number Missed</td>
                            <td>Percent Missed</td>
                            <td>Explanation</td>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptDetails" DataSourceID="dsQDetails" runat="server">

                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblshort" runat="server" Text='<%#Eval("q_short_name") %>'></asp:Label></td>
                                <td>

                                    <%#Eval("total_missed") %>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("answer_range")%>'></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("agent_display") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>

                <asp:SqlDataSource ID="dsQDetails" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    runat="server" SelectCommand="getAgentQDetails">
                    <SelectParameters>
                        <asp:ControlParameter Name="startdate" ControlID="txtAgentStart" />
                        <asp:ControlParameter Name="enddate" ControlID="txtAgentEnd" />
                        <asp:ControlParameter Name="scorecard" ControlID="ddlScorecard" />
                        <asp:ControlParameter Name="agent" ControlID="ddlAgents" />
                        <asp:ControlParameter Name="top30" ControlID="chk30Only" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </div>

        </asp:Panel>
        <script type="text/javascript">
            setupCalendars();
        </script>
    </section>






</asp:Content>
