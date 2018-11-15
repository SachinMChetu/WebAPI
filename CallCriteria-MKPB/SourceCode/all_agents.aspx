<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="all_agents.aspx.vb" Inherits="all_agents" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container">
        
            <h1 class="section-title"><i class="fa fa-desktop"></i>
                Agent Report</h1>
        
        <div class="general-filter">
            <div class="yellow-container">
                <table style="float: right;">
                    <tr>
                        <td>
                            <asp:Button ID="btnAgentReport" CssClass="secondary-cta" runat="server" Text="Go" /></td>
                        <td>
                            <asp:Button ID="btnSupeExportxp" runat="server" CssClass="secondary-cta" Text="Export to Excel" /></td>
                    </tr>
                </table>
                <%--<asp:Button ID="btnApplyFilter" CssClass="secondary-cta" runat="server" Text="APPLY" />--%>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>
                <!-- close date-holder -->

                <div class="field-holder-right">
                    <i class="fa fa-tag"></i>

                    <asp:DropDownList ID="ddlGroup" runat="server" DataSourceID="dsAgentGrp" DataTextField="AGENT_GROUP"
                        AutoPostBack="true" AppendDataBoundItems="true"
                        DataValueField="AGENT_GROUP">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsAgentGrp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date and AGENT_GROUP is not null and AGENT_GROUP <> '' order by AGENT_GROUP ">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                            <asp:ControlParameter Name="start_date" ControlID="txtAgentStart" />
                            <asp:ControlParameter Name="end_date" ControlID="txtAgentEnd" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                <!-- close select-holder -->
                <!-- close date-holder -->

            </div>
            <!-- close yellow-container -->
        </div>
        <!-- close general-filter -->



        <div class="table-outline users-list">
            <asp:GridView ID="gvAgentQuestions" AllowSorting="true" DataSourceID="dsAvgQuestions" DataKeyNames="Agent Name" GridLines="None"
                ShowFooter="true" runat="server" AutoGenerateColumns="False" Width="100%">
                <Columns>
                    <asp:HyperLinkField Text="Select" DataNavigateUrlFields="Agent_Name,start_date,end_date"
                        DataNavigateUrlFormatString="~/ExpandedView.aspx?filter=and Agent='{0}' and call_date between '{1}' and '{2}'" />
                    <%--<asp:BoundField DataField="timestamp" HeaderText="Time Stamp" SortExpression="timestamp" />--%>
                    <asp:BoundField DataField="Agent Name" HeaderText="Agent Name" SortExpression="Agent Name" />
                    <asp:BoundField DataField="Agent Group" HeaderText="Agent Group" SortExpression="Agent Group" />
                    <asp:BoundField DataField="top_missed" HeaderText="Top 3 Missed" SortExpression="top_missed" />
                    <asp:BoundField DataField="Counts" HeaderText="Average Score" SortExpression="Counts" />
                    <asp:BoundField DataField="Total_reviews" HeaderText="Total Reviews" SortExpression="Total_reviews" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle Font-Bold="True" ForeColor="Black" />
                <HeaderStyle Font-Bold="True" ForeColor="Black" />
                <PagerStyle BackColor="#666666" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <%-- <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />--%>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="dsAvgQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT @start_date as start_date, @end_date as end_date, 
                            XCC_REPORT_NEW.AGENT as [Agent Name],XCC_REPORT_NEW.AGENT as Agent_Name,XCC_REPORT_NEW.AGENT_group as [Agent Group], 
                            convert(int,avg(form_score3.total_score)) as Counts,
                            count(*) as Total_reviews, dbo.GetTop3Missed(agent, @start_date, @end_date) as top_missed 
                            FROM form_score3 join XCC_REPORT_NEW on form_score3.review_id = xcc_report_new.id 
                            left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 
                            left join form_notifications on form_notifications.form_id = form_score3.id where date_closed is null 
                            and review_date between @start_date and @end_date group by reviewer) a  
                            on form_score3.reviewer = a.reviewer and form_score3.review_date &gt; a.min_review  where form_score3.review_id = XCC_REPORT_NEW.ID 
                            and form_score3.appname = @appname and XCC_REPORT_NEW.call_date between @start_date and @end_date
                            group By XCC_REPORT_NEW.AGENT, XCC_REPORT_NEW.AGENT_group  ORDER BY Counts DESC">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date" />
                    <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/2014" Name="end_date" />
                    <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <script type="text/javascript">
            setupCalendars();
        </script>
    </section>
</asp:Content>
