﻿<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="agent2_Dashboard.aspx.vb" Inherits="Admin_Dashboard" %>

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

        var chart;
        var options = {
            title: 'Score Performance',
            pointSize: 2,
            //chartArea: { width: '80%', height: '80%' },
            legend: { position: 'out' },
            titlePosition: 'out',
            axisTitlesPosition: 'out',
            hAxis: {
                textPosition: 'none',
                viewWindow: { min: <%=min_line_range%>, max: <%=max_line_range%> }
            },
            animation: {
                duration: 1000,
                easing: 'linear'
            },
            backgroundColor: { fill: '#fcfcf2' },
            series: {
                0: { type: "area", targetAxisIndex: 0 },
                1: { type: "line", targetAxisIndex: 1 }
            },

            vAxes:[
               {title: 'Avg Score', textStyle:{color: 'blue'}, titleTextStyle: {color: 'blue', italic: false}}, // Nothing specified for axis 0
               {title: 'Number Reviewed', textStyle:{color: 'red'}, titleTextStyle: {color: 'red', italic: false}} // Axis 1
            ]
        };





        function drawChart() {
            data = google.visualization.arrayToDataTable([
              ['Date', 'Avg Score', 'Number Reviewed']
               <%=line_graph_data%>
            ]);

            var formatter_short = new google.visualization.DateFormat({ formatType: 'short' });
            formatter_short.format(data, 1);

            chart = new google.visualization.AreaChart(document.getElementById('line_div'));
            chart.draw(data, options);
        }



    </script>






</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnThisAgent" runat="server" />
    <section class="main-container">
        <h1 class="section-title"><i class="fa fa-desktop"></i>QA Dashboard
                
        </h1>
        <%-- <a href="edit_dashboard.aspx" class="third-priority-buttom"><i class="fa fa-gear"></i>Edit Dashboard</a>--%>


        <div style="float: right;">

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




            <!-- close yellow-container -->

        </div>
        <div style="padding-right: 15px; float: right;">
            <asp:Button ID="btnMyNotifications" runat="server" Text="Review My Notifications" CssClass="main-cta" />
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

            <table width="100%" style="margin: 5px;">
                <tr>
                    <td>

                        <div class="chart-results">

                            <div class="chart-container" style="width: 300px; height: 300px;">
                                <div class="chart" style="width: 300px; height: 300px;">
                                    <div id="chart-placeholder" style="width: 300px; height: 300px;">
                                    </div>
                                    <span><strong>
                                        <asp:Literal ID="Literal4" runat="server" Text="0"></asp:Literal></strong> <small>AVG SCORE</small> </span>
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


                        </div>

                    </td>
                    <td colspan="3">
                        <div class="line-graph-container">
                            <div class="line_graph">
                                <div id="line_div"></div>
                                <center><asp:TextBox ID="TextBox1" Width="300" Enabled="false" BackColor="White"  style="text-align:center;"
                                        BorderStyle="None" runat="server"></asp:TextBox></center>
                                <div runat="server" id="Div1" style="width: 90%; position: relative; left: 55px;"></div>

                                <script>
                                    $(function () {
                                        $('#ContentPlaceHolder1_slider_div').slider({
                                            range: true,
                                            min: <%=DateDiff(DateInterval.Day, Today(), min_date)%>,
                                            max: <%=DateDiff(DateInterval.Day, Today(), max_date)%>,
                                            values: [-30, <%=DateDiff(DateInterval.Day, Today(), max_date)%>],
                                            slide: function (event, ui) {
                                                var start_date = new Date();
                                                start_date.setDate(start_date.dbo.getMTDate()  + ui.values[0]);
                                                var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();

                                                var end_date = new Date();
                                                end_date.setDate(end_date.dbo.getMTDate() + ui.values[1]);
                                                var dateEnd = (end_date.getMonth() + 1) + '/' + end_date.dbo.getMTDate() + '/' + end_date.getFullYear();

                                                $('#ContentPlaceHolder1_txtDateRange').val(dateStart + ' to ' + dateEnd);

                                                //options.hAxis.viewWindow: {min: new Date(2002,0,0), max: new Date(2013,5,0)}
                                                //options.hAxis.viewWindow.min = start_date; // new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                                //options.hAxis.viewWindow.max = end_date;  //new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                                options.hAxis.viewWindow.min =  new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                                options.hAxis.viewWindow.max = new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                                chart.draw(data, options);
                                    
                                            }
                                        });

                                        var start_date = new Date();
                                        start_date.setDate(start_date.dbo.getMTDate() - 30);
                                        var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();


                                        var end_date = new Date();
                                        end_date.setDate(end_date.dbo.getMTDate() + <%=DateDiff(DateInterval.Day,Today(), max_date)%>);
                                        var dateEnd = (end_date.getMonth() + 1) + '/' + end_date.dbo.getMTDate() + '/' + end_date.getFullYear();

                                        $('#ContentPlaceHolder1_txtDateRange').val(dateStart + ' to ' + dateEnd);

                                        //$('#ContentPlaceHolder1_txtDateRange').val($('#ContentPlaceHolder1_slider_div').slider('values', 0) +
                                        //' to ' + $('#ContentPlaceHolder1_slider_div').slider('values', 1));

                                        options.hAxis.viewWindow.min =  new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                        options.hAxis.viewWindow.max = new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                        chart.draw(data, options);
                                    });
                                </script>
                            </div>

                        </div>
                    </td>

                </tr>
            </table>
        </div>


        <div class="panel" style="display: none">

            <div class="line-graph-container">
                <div class="line_graph">
                    <div id="line_div"></div>
                    <center><asp:TextBox ID="txtDateRange" Width="300" Enabled="false" BackColor="White"  style="text-align:center;"
                        BorderStyle="None" runat="server"></asp:TextBox></center>
                    <div runat="server" id="slider_div" style="width: 90%; position: relative; left: 55px;"></div>

                    <script>
                        $(function () {
                            $('#ContentPlaceHolder1_slider_div').slider({
                                range: true,
                                min: <%=DateDiff(DateInterval.Day, Today(), min_date)%>,
                                max: <%=DateDiff(DateInterval.Day, Today(), max_date)%>,
                                values: [<%=DateDiff(DateInterval.Day, Today(), min_date)%>, <%=DateDiff(DateInterval.Day, Today(), max_date)%>],
                                slide: function (event, ui) {
                                    var start_date = new Date();
                                    start_date.setDate(start_date.dbo.getMTDate()  + ui.values[0]);
                                    var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();

                                    var end_date = new Date();
                                    end_date.setDate(end_date.dbo.getMTDate() + ui.values[1]);
                                    var dateEnd = (end_date.getMonth() + 1) + '/' + end_date.dbo.getMTDate() + '/' + end_date.getFullYear();

                                    $('#ContentPlaceHolder1_txtDateRange').val(dateStart + ' to ' + dateEnd);

                                    //options.hAxis.viewWindow: {min: new Date(2002,0,0), max: new Date(2013,5,0)}
                                    //options.hAxis.viewWindow.min = start_date; // new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                    //options.hAxis.viewWindow.max = end_date;  //new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                    options.hAxis.viewWindow.min =  new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                    options.hAxis.viewWindow.max = new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                    chart.draw(data, options);
                                    
                                }
                            });

                            var start_date = new Date();
                            start_date.setDate(start_date.dbo.getMTDate() + <%=DateDiff(DateInterval.Day, Today(), min_date)%>);
                            var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();


                            var end_date = new Date();
                            end_date.setDate(end_date.dbo.getMTDate() + <%=DateDiff(DateInterval.Day,Today(), max_date)%>);
                            var dateEnd = (end_date.getMonth() + 1) + '/' + end_date.dbo.getMTDate() + '/' + end_date.getFullYear();

                            $('#ContentPlaceHolder1_txtDateRange').val(dateStart + ' to ' + dateEnd);

                            //$('#ContentPlaceHolder1_txtDateRange').val($('#ContentPlaceHolder1_slider_div').slider('values', 0) +
                            //' to ' + $('#ContentPlaceHolder1_slider_div').slider('values', 1));

                            options.hAxis.viewWindow.min =  new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                            options.hAxis.viewWindow.max = new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                            chart.draw(data, options);
                        });
                    </script>
                </div>

            </div>



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


                    <!-- close results -->
                </div>
                <!-- close chart-results -->
            </div>
            <!-- close panel-content -->
        </div>
        <!-- close panel -->
        <!-- close panel-title -->
        <div class="panel-content">
            <div class="calls-additional-details">
                <div class="secondary-panel">
                    <div class="panel-title">
                        <h1>Top Missed Points</h1>
                        <%--<a href="All_missed.aspx?start_date=<%=start_date%>&end_date=<%=end_date%>">VIEW ALL</a>--%>
                    </div>
                    <!-- close panel-title -->
                    <div class="panel-contant">
                        <table class="invisible-frame">
                            <tbody>

                                <asp:Repeater ID="rptMissed" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div class="color red-color">
                                                </div>
                                            </td>
                                            <td>
                                                <%--<a href="missed_details.aspx?QID=<%# Eval("ID") %>&start_date=<%# eval("start_date")%>&end_date=<%# eval("end_date")%>&short_name=<%# eval("q_short_name") %>">
                                                    </a>--%>
                                                <%#Eval("q_short_name") %>
                                            </td>
                                            <td class="text-align-right">
                                                <strong>
                                                    <asp:Literal ID="Label3" runat="server" Text='<%#Eval("Percent_Qs") %>'></asp:Literal>%</strong>
                                            </td>
                                            <td class="text-align-right">
                                                <small>(<asp:Literal ID="Literal1" runat="server" Text='<%#Eval("total_wrong")%>'></asp:Literal>)</small>
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


            </div>
            <!-- close calls-additional-details -->
            <div class="calls-list">
                <div class="sub-title">
                    <h1>Recent Call Details</h1>
                    <div class="sub-title-actions">
                        <button runat="server" id="Button1" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-print"></i>PRINT REPORT
                        </button>
                        <button runat="server" id="btnRun" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-download"></i>EXPORT REPORT
                        </button>
                    </div>
                    <!-- close sub-title-actions -->
                </div>
                <!-- close sub-title -->

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <div class="table-outline">
                            <asp:GridView ID="gvQADetails" GridLines="None" CssClass="invisible-frame" runat="server" DataSourceID="dsQADetails" AllowPaging="true" AllowSorting="true"
                                PageSize="8" AutoGenerateColumns="false">
                                <Columns>
                                    <%--<asp:BoundField DataField="reviewer" SortExpression="reviewer" HeaderText="QA Agent" />--%>
                                    <asp:TemplateField ItemStyle-CssClass="first-cell text-align-left" HeaderText="Agent" Visible="false" SortExpression="agent">
                                        <ItemTemplate>
                                            <div class="first-cell-container">
                                                <span class="result-indicator final-<%#Eval("pass_fail")%>"></span>
                                                <%-- <asp:Image ID="imgAvatar" runat="server" />--%>
                                                <span><strong>
                                                    <asp:Literal ID="Literal2" Text='<%#Eval("agent")%>' runat="server"></asp:Literal></strong></span>

                                            </div>
                                            <!-- close first-cell-container -->
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField DataField="dnis" SortExpression="dnis" HeaderText="Phone" />--%>
                                    <asp:BoundField DataField="Call_length" SortExpression="Call_length" HeaderText="Call Time" />
                                    <asp:BoundField ItemStyle-Font-Bold="true" DataField="total_score" SortExpression="total_score" HeaderText="Score" />
                                    <asp:BoundField ItemStyle-Font-Bold="true" DataField="num_missed" SortExpression="num_missed" HeaderText="# Missed" />
                                    <asp:BoundField DataField="missed_list" SortExpression="missed_list" HeaderText="Missed Items" />
                                    <asp:BoundField DataField="pass_fail" SortExpression="pass_fail" HeaderText="Results" />
                                </Columns>
                                <PagerStyle CssClass="admin-pager" />
                                <%-- <PagerTemplate>
                           
                        </PagerTemplate>--%>
                            </asp:GridView>
                            <asp:SqlDataSource ID="dsQADetails" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="select *, missed_list, form_score3.id as form_id, case when total_score between 80 and 200 
                        then 'success' else 'fail' end as pass_fail, replace(replace(comments,char(13),''''), char(10),'''')  + dbo.getCannedComments(form_score3.id) as all_comments 
                        from form_score3 with (nolock) join xcc_report_new with (nolock) on xcc_report_new.id = form_score3.review_id
                        left join (select isnull(min(form_notifications.date_created),dbo.getMTDate()) as min_review, reviewer from form_score3 
                        left join form_notifications on form_notifications.form_id = form_score3.id where date_closed is null and appname = @appname 
                        group by reviewer) a on form_score3.reviewer = a.reviewer and form_score3.review_date &lt; a.min_review   
                        where review_date between @start_date and @end_date and form_score3.appname=@appname
                        and and form_score3.reviewer=@reviewer order by review_date desc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtStartDate" Name="start_date" />
                                    <asp:ControlParameter ControlID="txtEndDate" Name="end_date" />
                                    <asp:ControlParameter ControlID="hdnThisAgent" Name="reviewer" />
                                    <asp:SessionParameter Name="appname" SessionField="appname" />

                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- close table-outline -->
            </div>

            <!-- close calls-list -->
        </div>
        <!-- close panel-content -->
    </section>

    <asp:HiddenField ID="hdnAgentFilter" runat="server" />

</asp:Content>