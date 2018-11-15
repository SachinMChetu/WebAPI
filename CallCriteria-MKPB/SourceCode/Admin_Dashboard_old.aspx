<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="Admin_Dashboard_old.aspx.vb" Inherits="Admin_Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp_ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/controls/AgentFeedList.ascx" TagPrefix="UC1" TagName="AgentFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {

            <%--  var zeroData = parseInt(document.getElementById("<%= hdnzeroData.ClientID %>").value);
            var lessfifty = parseInt(document.getElementById("<%= hdnless50.ClientID %>").value);--%>
           <%-- var fiftytoseventyData = parseInt(document.getElementById("<%= hdn50to70.ClientID %>").value);
            var seventytoeighty = parseInt(document.getElementById("<%= hdn70to80.ClientID %>").value);
            var eightytoninty = parseInt(document.getElementById("<%= hdn80to90.ClientID %>").value);
            var nintyplus = parseInt(document.getElementById("<%= hdn90plus.ClientID %>").value);--%>

            var data = google.visualization.arrayToDataTable([
              ['Range', 'Call Review'],
              ['< 80', <%= hdn50to70.Text%>],
              ['80 to 90', <%= hdn70to80.Text%>],
              ['90 to 100', <%= hdn80to90.Text%>],
              ['100', <%= hdn90plus.Text%>]
            ]);

            var options = {
                backgroundColor: '#F4FFED',
                chartArea: { left: 25, top: 25, width: "375px", height: "375px" },
                legend: { position: 'right', textStyle: { color: 'blue', fontSize: 16 } },
                is3D: false
            };

            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));

            function selectHandler() {
                var selectedItem = chart.getSelection()[0];
                if (selectedItem) {
                    var topping = data.getValue(selectedItem.row, 0);
                    var startdate = '';
                    var enddate = '';

                    if (document.getElementById('ContentPlaceHolder1_txtStartDate').value != '')
                        startdate = '&StartDate=' + document.getElementById('ContentPlaceHolder1_txtStartDate').value;
                    if (document.getElementById('ContentPlaceHolder1_txtEndDate').value != '')
                        enddate = '&EndDate=' + document.getElementById('ContentPlaceHolder1_txtEndDate').value;

                    if (topping == '0s')
                        self.location = 'ExpandedView.aspx?filter=and (total_score <= 0 or total_score is null) ' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    if (topping == '< 50')
                        self.location = 'ExpandedView.aspx?filter=and total_score > 0 and total_score < 50' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    if (topping == '< 80')
                        self.location = 'ExpandedView.aspx?filter=and total_score < 80' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    if (topping == '70 to 80')
                        self.location = 'ExpandedView.aspx?filter=and total_score >= 80 and total_score < 90' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    if (topping == '90 to 100')
                        self.location = 'ExpandedView.aspx?filter=and total_score >= 90 and total_score < 100' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    if (topping == '100')
                        self.location = 'ExpandedView.aspx?filter=and total_score >= 99 ' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    //alert('The user selected ' + topping);

                }
            }

            chart.draw(data, options);

            google.visualization.events.addListener(chart, 'select', selectHandler);
        }


    </script>




    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = google.visualization.arrayToDataTable([
              ['Date', 'Avg Score', 'Number Reviewed']
               <%=line_graph_data%>
            ]);

            var options = {
                title: 'Score Performance',
                legend: { position: 'bottom' },
                curveType: 'function',
                hAxis: { maxValue: 100 },
                series: {
                    0: { type: "line", targetAxisIndex: 0 },
                    1: { type: "line", targetAxisIndex: 1 }
                }
            };

            var chart = new google.visualization.LineChart(document.getElementById('line_div'));
            chart.draw(data, options);

            drawChart30();
            drawChart60();

        }

        function drawChart30() {
            var data = google.visualization.arrayToDataTable([
              ['Date', 'Avg Score']
               <%=line_graph_data_30%>
            ]);

            var options = {
                title: '30 Day Score Performance',
                curveType: 'function',
                hAxis: { maxValue: 100 },
                legend: { position: 'none' }

            };

            var chart = new google.visualization.LineChart(document.getElementById('line_div_30'));
            chart.draw(data, options);
        }

        function drawChart60() {
            var data = google.visualization.arrayToDataTable([
              ['Date', 'Avg Score']
               <%=line_graph_data_60%>
            ]);

            var options = {
                title: '60 Day Score Performance',
                curveType: 'function',
                hAxis: { maxValue: 100 },
                legend: { position: 'none' }
            };

            var chart = new google.visualization.LineChart(document.getElementById('line_div_60'));
            chart.draw(data, options);
        }





    </script>






</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnThisAgent" runat="server" />
    <section class="main-container">
        
            <h1 class="section-title"><i class="fa fa-desktop"></i>Dashboard
                
            </h1>
            <%--<a href="edit_dashboard.aspx" class="third-priority-buttom"><i class="fa fa-gear"></i>Edit Dashboard</a>--%>

            <div style="float: right;">
               


                <div class="field-holder">
                    <i class="fa fa-tag"></i>

                    <asp:DropDownList ID="ddlGroup" runat="server" DataSourceID="dsAgentGrp" DataTextField="AGENT_GROUP"
                        AutoPostBack="true" OnSelectedIndexChanged="Recalc_Elements" AppendDataBoundItems="true"
                        DataValueField="AGENT_GROUP">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsAgentGrp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct lower([AGENT_GROUP]) as AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and AGENT_GROUP is not null and AGENT_GROUP <> '' order by lower([AGENT_GROUP]) ">
                        <SelectParameters>
                            <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue="" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
                <!-- close select-holder -->

                 <div class="field-holder">
                    <i class="fa fa-user"></i>
                    <asp:DropDownList ID="ddlAgent" Visible="false"  runat="server" AppendDataBoundItems="true"
                        DataTextField="AGent" DataValueField="AGent">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="field-holder">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtStartDate" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>
                <!-- close date-holder -->


                <div class="field-holder">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtEndDate" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>
                <asp:Button ID="btnApplyFilter" CssClass="secondary-cta" runat="server" Text="APPLY" />
                <div class="field-holder">
                </div>
                <!-- close date-holder -->


                <!-- close yellow-container -->

            </div>
        
        <div class="applied-filters">
            <label>
                Applied Filters:</label>
            <%--<span><i class="fa fa-mobile-phone"></i><em>Phone: <strong>N/A</strong></em> </span>--%>
            <span><i class="fa fa-user"></i><em>Group/s: <strong>
                <asp:Literal ID="litGroupFilter" runat="server"></asp:Literal></strong></em> </span>
            <span><i class="fa fa-clock-o"></i><em>Period: <strong>
                <asp:Literal ID="litStart2" runat="server"></asp:Literal>
                -
                    <asp:Literal ID="litEnd2" runat="server"></asp:Literal></strong></em>
            </span>
        </div>
        <!-- close applied-filters -->
        <!-- close general-filter -->


        <div class="panel">
            <div class="panel-title">
                <i class="fa fa-clock-o section-icon"></i>
                <hgroup>
                    <h1>Call Reviewed History (Call Date)</h1>
                    <h2>Score chart for <strong>
                        <asp:Literal ID="litStart" runat="server"></asp:Literal>
                        -
                        <asp:Literal ID="litEnd" runat="server"></asp:Literal></strong></h2>


                </hgroup>

                <div class="panel-actions">
                    <button runat="server" id="btnViewTop" onserverclick="btnTopAgents" class="third-priority-buttom" title="Search">
                        <i class="fa fa-user"></i>VIEW TOP AGENTS
                    </button>
                    <button runat="server" id="btnExportAgents" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                        <i class="fa fa-download"></i>EXPORT RESULTS
                    </button>
                </div>
                <!-- close panel-actions -->
            </div>
            <!-- close panel-title -->
            <div class="panel-content">
                <div class="feed-agents-list">
                    <UC1:AgentFeed ID="AFL" runat="server" />
                </div>
                <!-- close feed-agents-list -->
                <div class="chart-results">

                    <div class="line-graph-container">
                        <div class="line_graph">
                            <div id="line_div"></div>
                        </div>
                    </div>

                    <div class="chart-container">
                        <div class="chart">
                            <div id="chart-placeholder">
                            </div>
                            <span><strong>
                                <asp:Literal ID="lblAvgScore" runat="server" Text="0"></asp:Literal></strong> <small>AVG SCORE</small> </span>
                        </div>
                        <!-- close chart -->
                        <div class="change-chart">
                            <a href="#" class="selected-chart"><i class="fa fa-bar-chart-o"></i></a><a href="#">
                                <i class="fa fa-bar-chart-o"></i></a><a href="#"><i class="fa fa-bar-chart-o"></i>
                                </a>
                        </div>
                        <!-- clos change-chart -->
                    </div>
                    <!-- close chart-container -->
                    <asp:HiddenField ID="hdnSelectedRange" runat="server" Value="0" />
                    <div class="results">
                        <h1>Results</h1>

                        <ul>
                            <%--<li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '0';">
                                <div class="color red-color">
                                </div>
                                <strong>0 points</strong><asp:Label ID="hdnzeroData" runat="server"></asp:Label>
                            </li>
                            <li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '0-50';">
                                <div class="color red2-color">
                                </div>
                                <strong>< 50 points</strong>
                                <asp:Label ID="hdnless50" runat="server"></asp:Label>
                            </li>--%>
                            <li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '< 80';">
                                <div class="color red-color">
                                </div>
                                <strong>< 80 points</strong>
                                <asp:Label ID="hdn50to70" runat="server"></asp:Label>
                            </li>
                            <li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '80-90';">
                                <div class="color yellow2-color">
                                </div>
                                <strong>80 ~ 90 points</strong><asp:Label ID="hdn70to80" runat="server"></asp:Label>
                            </li>
                            <li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '90-100';">
                                <div class="color green-color">
                                </div>
                                <strong>90 ~ 100 points</strong>
                                <asp:Label ID="hdn80to90" runat="server"></asp:Label>
                            </li>
                            <li onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '100';">
                                <div class="color green2-color">
                                </div>
                                <strong>100 points</strong> <span>
                                    <asp:Label ID="hdn90plus" runat="server"></asp:Label>
                                </span></li>
                            <%--  <li>
                            <div class="color dark-orange-color">
                            </div>
                            <strong>Other</strong> <span>(148 Agents)</span> </li>--%>
                        </ul>
                        <div id="line_div_30"></div>
                        <div id="line_div_60"></div>

                        <%-- <div class="total-count">
                        <span>AVG SCORE</span> <strong>71.22</strong> <a href="#" class="third-priority-buttom right-oriented-icon">COMPARE <i class="fa fa-chevron-right"></i></a>
                    </div>--%>
                        <!-- close total-count -->
                    </div>

                    <!-- close results -->
                </div>
                <!-- close chart-results -->
            </div>
            <!-- close panel-content -->
        </div>
        <!-- close panel -->
        <%--        <div class="panel">
            <div class="panel-title">
                <i class="fa fa-mobile-phone section-icon"></i>
                <hgroup>
                    <h1>Calls Details</h1>
                    <h2>Calls Details for <strong>
                        <asp:Literal ID="litStart3" runat="server"></asp:Literal>
                        -
                        <asp:Literal ID="litEnd3" runat="server"></asp:Literal></strong></h2>
                </hgroup>
            </div>--%>
        <!-- close panel-title -->
        <div class="panel-content">
            <div class="calls-additional-details">
                <div class="secondary-panel">
                    <div class="panel-title">
                        <h1>Top Missed Points</h1>
                        <a href="All_missed.aspx?start_date=<%=start_date%>&end_date=<%=end_date%>">VIEW ALL</a>
                    </div>
                    <!-- close panel-title -->
                    <div class="panel-contant">
                        <table class="invisible-frame">
                            <tbody>

                                <asp:Repeater ID="rptMissed" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div class="color <%#Eval("div_color")%>-color">
                                                </div>
                                            </td>
                                            <td>
                                                <a href="missed_details.aspx?QID=<%# Eval("ID") %>&start_date=<%# eval("start_date")%>&end_date=<%# eval("end_date")%>&short_name=<%# eval("q_short_name") %>">
                                                    <%#Eval("q_short_name") %></a>

                                            </td>
                                            <td class="text-align-right">
                                                <strong>
                                                    <asp:Literal ID="Label3" runat="server" Text='<%#Eval("Percent_Qs") %>'></asp:Literal>%</strong>
                                            </td>
                                            <td class="text-align-right">
                                                <small>(<asp:Literal ID="Literal1" runat="server" Text='<%#Eval("total_wrong")%>'></asp:Literal>
                                                    Calls)</small>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                        <!-- close invisible-frame -->
                    </div>
                    <!-- close panel-content -->
                </div>
                <!-- close secondary-panel -->
                <div class="secondary-panel">
                    <div class="panel-title">
                        <h1>Top/Bottom Agents</h1>
                        <a href="all_agents.aspx">VIEW ALL</a>
                    </div>
                    <!-- close panel-title -->
                    <div class="panel-contant">

                        <asp:GridView ID="gvTBAgents" GridLines="None" AutoGenerateColumns="false" ShowHeader="false"
                            CssClass="invisible-frame" runat="server">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="color <%#eval("div_color") %>-color"></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField DataNavigateUrlFields="start_date, end_date,agentname" DataNavigateUrlFormatString="ExpandedView.aspx?filter= and Agent='{2}' and call_date between '{0}' and '{1}' " DataTextField="agentname" HeaderText="Agent Name" />
                                <asp:BoundField DataField="Average Score" />
                            </Columns>

                        </asp:GridView>
                        <%--<asp:SqlDataSource ID="dsTBAgents" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="select case when total_score between -200 and 80 then 'red' else when total_score between 80 and 80 then 'yellow2' 
                            else when total_score between 90 and 100 then 'green' else when total_score = 100 then 'green2' end as div_color  , @start_date as start_date, @end_date as end_date, * from 
                            (SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName, convert(int,avg(form_score3.total_score)) as [Average Score]
                            FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID 
                            join notifications with (nolock)  on notifications.form_id = form_score3.id where 
                            form_score3.appname = @appname and acknowledged = 1  
                            and XCC_REPORT_NEW.call_date between @start_date and @end_date
                            group By XCC_REPORT_NEW.AGENT 
                            ORDER BY [Average Score] DESC)  a
                            union all
                            select case when total_score between -200 and 80 then 'red' else when total_score between 80 and 80 then 'yellow2' 
                            else when total_score between 90 and 100 then 'green' else when total_score = 100 then 'green2' end as div_color, @start_date as start_date, @end_date as end_date, * from 
                            (SELECT  distinct top 5 XCC_REPORT_NEW.AGENT as AgentName, convert(int,avg(form_score3.total_score)) as [Average Score]
                            FROM  form_score3  with (nolock) join XCC_REPORT_NEW  with (nolock) on form_score3.review_id = XCC_REPORT_NEW.ID 
                            join notifications with (nolock)  on notifications.form_id = form_score3.id where 
                            form_score3.appname = @appname and acknowledged = 1  
                            and XCC_REPORT_NEW.call_date between @start_date and @end_date
                            group By XCC_REPORT_NEW.AGENT 
                            ORDER BY [Average Score] asc) b
                            ORDER BY [Average Score] desc">
                            <SelectParameters>
                                <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue="" />
                                <asp:ControlParameter ControlID="txtStartDate" Name="start_date" />
                                <asp:ControlParameter ControlID="txtEndDate" Name="end_date" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>

                        <!-- close invisible-frame -->
                    </div>
                    <!-- close panel-content -->
                </div>
                <!-- close secondary-panel -->
            </div>
            <!-- close calls-additional-details -->
            <div class="calls-list">
                <div class="sub-title">
                    <h1>Recent Call Details</h1>
                    <div class="sub-title-actions">
                        <button runat="server" id="Button2" onserverclick="btnViewAllCalls" class="third-priority-buttom" title="Search">
                            <i class="fa fa-bars"></i>View All
                        </button>
                        <button runat="server" id="Button1" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-print"></i>PRINT REPORT
                        </button>
                        <button runat="server" id="btnRun" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-download"></i>EXPORT REPORT
                        </button>
                        <asp:Button ID="btnPrint" Visible="false"  runat="server" Text="Print test" />
                    </div>
                    <!-- close sub-title-actions -->
                </div>
                <!-- close sub-title -->
                <div class="table-outline">
                    <asp:GridView ID="gvQADetails" GridLines="None" CssClass="invisible-frame" runat="server" DataSourceID="dsQADetails" AllowPaging="true" AllowSorting="true"
                        PageSize="10" AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <%--<asp:BoundField DataField="reviewer" SortExpression="reviewer" HeaderText="QA Agent" />--%>
                            <asp:TemplateField ItemStyle-CssClass="first-cell text-align-left" HeaderText="QA Agent" SortExpression="reviewer">
                                <ItemTemplate>
                                    <div class="first-cell-container">
                                        <span class="result-indicator final-<%#Eval("pass_fail")%>"></span>
                                        <asp:Image ID="imgAvatar" CssClass="my-avatar" runat="server" />
                                        <span><strong>
                                            <asp:Literal ID="Literal2" Text='<%#Eval("reviewer")%>' runat="server"></asp:Literal></strong>
                                            <small>ID: <a href='review_record.aspx?ID=<%#Eval("form_id")%>'><%#Eval("form_id")%></a></small> </span>
                                    </div>
                                    <!-- close first-cell-container -->
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dnis" SortExpression="dnis" HeaderText="Phone" />
                            <asp:BoundField DataField="Call_length" SortExpression="Call_length" HeaderText="Call Time" />
                            <asp:BoundField ItemStyle-Font-Bold="true" DataField="total_score" SortExpression="total_score" HeaderText="Score" />
                            <asp:BoundField ItemStyle-Font-Bold="true" DataField="num_missed" SortExpression="num_missed" HeaderText="# Missed" />
                            <asp:BoundField DataField="missed_list" SortExpression="missed_list" HeaderText="Missed Items" />
                            <asp:BoundField DataField="pass_fail" SortExpression="pass_fail" HeaderText="Results" />
                        </Columns>
                        <PagerStyle CssClass="admin-pager" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsQADetails" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtStartDate" Name="start_date" />
                            <asp:ControlParameter ControlID="txtEndDate" Name="end_date" />
                            <asp:SessionParameter Name="appname" SessionField="appname" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!-- close table-outline -->
            </div>
            <!-- close calls-list -->
        </div>
        <!-- close panel-content -->
    </section>

    <asp:HiddenField ID="hdnAgentFilter" runat="server" />

</asp:Content>
