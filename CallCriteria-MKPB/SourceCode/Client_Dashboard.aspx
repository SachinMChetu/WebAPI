<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="Client_Dashboard.aspx.vb" Inherits="Admin_Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp_ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/controls/AgentFeedList.ascx" TagPrefix="UC1" TagName="AgentFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            setupExpandCollapse();
            setupCalendars();
        });

        function jumpPos(to_time) {

            audio = $('.audio-player audio').get(0);
            audio.currentTime = to_time;
            audio.play();

        }

        function show_audio(file_name, position, form_id) {
            var new_filename;
            $.ajax({
                type: 'POST',
                url: "convert_audio.aspx",
                data: { ID: form_id },
                success: function (result) {
                    new_filename = result;
                },
                async: false
            });
            $('#notification_holder').show();

            document.getElementById('ContentPlaceHolder1_hdnOpenFormID').value = form_id;
            setupAudioPlayer2(new_filename, 0.75, true, position);

        }
    </script>
    <script language='javascript'> 
        <!--
    function SetFocus(element) {
        window.scrollTo(0, document.body.scrollHeight);
        document.getElementById(element).focus();
        return true;
    } 
    //--> 
    </script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {

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
                    {
                        document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '< 80';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','');
                        //self.location = 'ExpandedView.aspx?filter=and total_score < 80' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    }
                    if (topping == '70 to 80')
                    {
                        document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '80-90';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','');
                        //self.location = 'ExpandedView.aspx?filter=and total_score >= 80 and total_score < 90' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    }
                    if (topping == '90 to 100')
                    {
                        document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '90-100';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','');
                        //self.location = 'ExpandedView.aspx?filter=and total_score >= 90 and total_score < 100' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    }
                    if (topping == '100')
                    {
                        document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '100';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','');
                        //self.location = 'ExpandedView.aspx?filter=and total_score >= 99 ' + document.getElementById('ContentPlaceHolder1_hdnAgentFilter').value + ' ' + startdate + enddate;
                    }
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
            legend: { position: 'none' },
            titlePosition: 'out',
            //axisTitlesPosition: 'out',
            hAxis: {
                textPosition: 'none',
                viewWindow: { min: <%=min_line_range%>, max: <%=max_line_range%> }
            },
            animation: {
                duration: 1000,
                easing: 'linear'
            },
            //backgroundColor: { fill: '#fcfcf2' },
            series: {
                0: { type: "area", targetAxisIndex: 0 },
                1: { type: "line", targetAxisIndex: 1}
            },

            vAxes:[
                {title: 'Avg Score %', textStyle:{color: 'blue'}, titleTextStyle: {color: 'blue', italic: false}}, // Nothing specified for axis 0
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

        <table width="100%">
            <tr>
                <td>
                    <img src="/images/photo.JPG" height="0" /></td>
                <td style="position: relative; top: -10px; text-align: center;" align="right">
                    <h1 class="section-title">
                        <asp:Literal ID="litClientName" runat="server" Text="ACME"></asp:Literal>
                        Dashboard</h1>
                </td>
                <td align="right">


                    <div style="float: right;">


                        <div class="field-holder">
                            <i class="fa fa-tag"></i>

                            <asp:DropDownList ID="ddlGroup" runat="server" DataSourceID="dsAgentGrp" DataTextField="AGENT_GROUP"
                                AutoPostBack="true" OnSelectedIndexChanged="Recalc_Elements" AppendDataBoundItems="true"
                                DataValueField="AGENT_GROUP">
                                <asp:ListItem Text="All Groups" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsAgentGrp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="SELECT distinct AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date and AGENT_GROUP is not null and AGENT_GROUP <> '' order by AGENT_GROUP ">
                                <SelectParameters>
                                    <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue="" />
                                    <asp:ControlParameter Name="start_date" ControlID="txtStartDate" />
                                    <asp:ControlParameter Name="end_date" ControlID="txtEndDate" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                        </div>




                        <div class="field-holder">
                            <i class="fa fa-user"></i>
                            <asp:DropDownList ID="ddlAgent" Visible="true" runat="server" AppendDataBoundItems="true"
                                DataTextField="AGent" DataValueField="AGent">
                                <asp:ListItem Text="All Agents" Value=""></asp:ListItem>
                            </asp:DropDownList>

                        </div>


                        <div class="field-holder">
                            <i class="fa fa-cogs"></i>
                            <asp:DropDownList ID="ddlCampaign" Visible="true" runat="server" AppendDataBoundItems="true"
                                DataTextField="campaign" DataValueField="campaign" DataSourceID="dsCampaign">
                                <%-- DataSourceID="dsAgents"--%>
                                <asp:ListItem Text="All Campaigns" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsCampaign" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="SELECT distinct campaign FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date order by campaign ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtStartDate" Name="start_date" />
                                    <asp:ControlParameter ControlID="txtEndDate" Name="end_date" />
                                    <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue="" />
                                </SelectParameters>
                            </asp:SqlDataSource>
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

                </td>
                <td>
                    <%--<a href="view_scorecard.aspx" class="third-priority-buttom"><i class="fa fa-gear"></i>View Scorecard</a>--%>

                </td>
            </tr>
        </table>



        <div class="applied-filters">
            <label>
                Applied Filters:</label>

            <span><i class="fa fa-flash"></i><em>Latest Date Loaded: <strong>
                <asp:Literal ID="litLatest" runat="server"></asp:Literal></strong></em> </span>

            <span><i class="fa fa-user"></i><em>Group/s: <strong>
                <asp:Literal ID="litGroupFilter" runat="server"></asp:Literal></strong></em> </span>
            <span><i class="fa fa-clock-o"></i><em>Period: <strong>
                <asp:Literal ID="litStart2" runat="server"></asp:Literal>
                -
                    <asp:Literal ID="litEnd2" runat="server"></asp:Literal></strong></em>
            </span>

            <span><i class="fa fa-clock-o"></i><em>Billed Time: <strong>
                <asp:Literal ID="litBilledTime" runat="server"></asp:Literal></strong></em> </span>
            <span><i class="fa fa-dollar"></i><em>Billed Amount: <strong>
                <asp:Literal ID="litBilledAmount" runat="server"></asp:Literal></strong></em> </span>
            <span><i class="fa fa-headphones"></i><em>Number Calls: <strong>
                <asp:Literal ID="litNumberCalls" runat="server"></asp:Literal></strong></em> </span>

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

                        </div>

                    </td>
                    <td colspan="3">
                        <div class="line-graph-container">
                            <div class="line_graph">
                                <div id="line_div"></div>
                                <center><asp:TextBox ID="txtDateRange" Width="300" Enabled="false" BackColor="White"  style="text-align:center;"
                                        BorderStyle="None" runat="server"></asp:TextBox></center>
                                <div runat="server" id="slider_div" style="width: 90%; position: relative; left: 55px; top: -40px"></div>

                                <script>
                                    $(function () {
                                        $('#ContentPlaceHolder1_slider_div').slider({
                                            range: true,
                                            min: <%=DateDiff(DateInterval.Day, Today(), min_date)%>,
                                            max: <%=DateDiff(DateInterval.Day, Today(), max_date)%>,
                                            values: [<%=DateDiff(DateInterval.Day, Today(), CDate(txtStartDate.Text))%>, <%=DateDiff(DateInterval.Day, Today(), CDate(txtEndDate.Text))%>],
                                            slide: function (event, ui) {
                                                var start_date = new Date();
                                                start_date.setDate(start_date.dbo.getMTDate()  + ui.values[0]);
                                                var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();

                                                var end_date = new Date();
                                                end_date.setDate(end_date.dbo.getMTDate() + ui.values[1]);
                                                var dateEnd = (end_date.getMonth() + 1) + '/' + end_date.dbo.getMTDate() + '/' + end_date.getFullYear();

                                                $('#ContentPlaceHolder1_txtDateRange').val(dateStart + ' to ' + dateEnd);
                                                options.hAxis.viewWindow.min =  new Date(start_date.getFullYear(), start_date.getMonth(), start_date.dbo.getMTDate());
                                                options.hAxis.viewWindow.max = new Date(end_date.getFullYear(), end_date.getMonth(), end_date.dbo.getMTDate());
                                                chart.draw(data, options);
                                    
                                            }
                                        });

                                        var start_date = new Date();
                                        start_date.setDate(start_date.dbo.getMTDate() + (<%=DateDiff(DateInterval.Day, Today(), CDate(txtStartDate.Text))%>));
                                        var dateStart = (start_date.getMonth() + 1) + '/' + start_date.dbo.getMTDate() + '/' + start_date.getFullYear();



                                        var end_date = new Date();
                                        end_date.setDate(end_date.dbo.getMTDate() + (<%=DateDiff(DateInterval.Day, Today(), max_date)%>));
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
                <tr>

                    <td valign="top">
                        <asp:LinkButton ID="lbSubmitPie" runat="server"></asp:LinkButton>
                        <div class="results">
                            <h1>Results</h1>

                            <table style="width: 100%; font-size: 13px; color: #999;" class="table-results">
                                <tr style="cursor: pointer;" onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '< 80';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','')">
                                    <td>
                                        <div class="color red-color">
                                        </div>

                                    </td>
                                    <td><strong>< 80 %</strong></td>
                                    <td>
                                        <asp:Label ID="hdn50to70" runat="server"></asp:Label></td>
                                </tr>



                                <tr style="cursor: pointer;" onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '80-90';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','')">
                                    <td>
                                        <div class="color yellow2-color">
                                        </div>
                                    </td>
                                    <td><strong>80 ~ 90 %</strong></td>
                                    <td>
                                        <asp:Label ID="hdn70to80" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="cursor: pointer;" onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '90-100';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','')">
                                    <td>
                                        <div class="color green-color">
                                        </div>
                                    </td>
                                    <td><strong>90 ~ 100 %</strong></td>
                                    <td>
                                        <asp:Label ID="hdn80to90" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="cursor: pointer;" onclick="document.getElementById('ContentPlaceHolder1_hdnSelectedRange').value= '100';__doPostBack('ctl00$ContentPlaceHolder1$lbSubmitPie','')">
                                    <td>
                                        <div class="color green2-color"></div>
                                    </td>
                                    <td><strong>100 %</strong></td>
                                    <td>
                                        <span>
                                            <asp:Label ID="hdn90plus" runat="server"></asp:Label>
                                        </span></td>
                                </tr>

                            </table>

                            <!-- close total-count -->
                        </div>

                        <!-- close results -->

                    </td>

                    <td valign="top" width="50%">
                        <div class="secondary-panel">
                            <div class="panel-title">
                                <h1>Top Missed Points</h1>
                                <a href="All_missed.aspx?start_date=<%=start_date%>&end_date=<%=end_date%>">VIEW ALL</a>
                            </div>
                            <!-- close panel-title -->

                            <div class="panel-contant scrolling-list" style="height: 400px; overflow: hidden;">
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
                                                        <%--<asp:LinkButton runat="server" ID="lbQuestionFilter" CommandName="Question" CommandArgument='<%#Eval("q_short_name") %>' Text='<%#Eval("q_short_name") %>'></asp:LinkButton>--%>
                                                        <a href="expandedview.aspx?ShortName=<%#Eval("q_short_name") %>"><%#Eval("q_short_name") %></a>
                                                    </td>
                                                    <td class="text-align-right">
                                                        <strong>
                                                            <asp:Literal ID="Label3" runat="server" Text='<%#Eval("Percent_Qs") %>'></asp:Literal>%</strong>
                                                    </td>
                                                    <td class="text-align-right">
                                                        <small>(<asp:Literal ID="Literal1" runat="server" Text='<%#Eval("total_wrong")%>'></asp:Literal>
                                                            of 
                                                            <asp:Literal ID="Literal3" runat="server" Text='<%#Eval("num_calls")%>'></asp:Literal>
                                                            Calls)</small>
                                                    </td>
                                                    <td>&nbsp;</td>
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
                    </td>
                    <td>&nbsp;</td>
                    <td valign="top" width="50%">
                        <div class="secondary-panel">
                            <div class="panel-title">
                                <h1>Agent Ranking</h1>
                                <a href="all_agents.aspx">VIEW ALL</a>
                            </div>
                            <!-- close panel-title -->

                            <div class="panel-contant scrolling-list" style="height: 400px; overflow: hidden;">

                                <asp:GridView ID="gvTBAgents" GridLines="None" AutoGenerateColumns="false" ShowHeader="false"
                                    CssClass="invisible-frame" runat="server">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="color <%#eval("div_color") %>-color"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:HyperLinkField DataNavigateUrlFields="start_date, end_date,agentname" DataNavigateUrlFormatString="ExpandedView.aspx?filter= and Agent='{2}'" DataTextField="agentname" HeaderText="Agent Name" />
                                        <asp:BoundField DataField="avg_score" ItemStyle-Font-Bold="true" ControlStyle-Font-Bold="true" HeaderText="Averge Score" DataFormatString="{0:n2}"  SortExpression="Averge Score" />
                                    </Columns>

                                </asp:GridView>

                                <!-- close invisible-frame -->
                            </div>
                            <!-- close panel-content -->
                        </div>
                        <!-- close secondary-panel -->
                    </td>
                    <td width="50px">&nbsp;</td>
                </tr>
            </table>



        </div>



        <!-- close panel -->
        <!-- close panel-title -->
        <div class="panel-content">
            <div class="calls-additional-details">
            </div>
            <!-- close calls-additional-details -->
            <div class="calls-list">
                <div class="sub-title">
                    <table>
                        <tr>
                            <td>
                                <h1><a name="details">Details</a></h1>
                            </td>
                            <td>
                                <asp:Panel ID="pnlFilters" runat="server"></asp:Panel>
                            </td>
                        </tr>
                    </table>

                    <div class="sub-title-actions">

                        <button runat="server" id="Button1" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-print"></i>PRINT REPORT
                        </button>
                        <button runat="server" id="btnRun" onserverclick="btnExportDetails" class="third-priority-buttom" title="Search">
                            <i class="fa fa-download"></i>EXPORT REPORT
                        </button>
                        <asp:Button ID="btnPrint" Visible="false" runat="server" Text="Print test" />
                    </div>
                    <!-- close sub-title-actions -->
                </div>
                <!-- close sub-title -->

                <asp:UpdatePanel runat="server" ID="upQADetails">
                    <ContentTemplate>
                        <div class="table-outline">
                            <asp:GridView ID="gvQADetails" GridLines="None" CssClass="invisible-frame" runat="server" DataSourceID="dsQADetails" AllowPaging="true" AllowSorting="true"
                                PageSize="10" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="pass_fail" SortExpression="pass_fail" HeaderText="Results" />
                                    <asp:TemplateField ItemStyle-CssClass="first-cell text-align-left" HeaderText="Call ID" SortExpression="reviewer">
                                        <ItemTemplate>
                                            <div class="first-cell-container">
                                                <span class="result-indicator final-<%#Eval("pass_fail")%>"></span>
                                                <asp:Image ID="imgAvatar" CssClass="my-avatar" Visible="false" runat="server" />
                                                <span><small>ID: <a href='review_record.aspx?ID=<%#Eval("form_id")%>'><%#Eval("form_id")%></a></small> </span>
                                                <asp:HiddenField ID="hdnFormIDRow" runat="server" Value='<%#Eval("form_id")%>' />
                                            </div>
                                            <!-- close first-cell-container -->
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField DataField="agent" SortExpression="agent" HeaderText="Agent" />
                                    <asp:BoundField DataField="dnis" SortExpression="dnis" HeaderText="Phone" />
                                    <asp:BoundField DataField="Call_date" SortExpression="Call_date" HeaderText="Call Date" />
                                    <asp:BoundField DataField="Call_length" SortExpression="Call_length" HeaderText="Call Length" />
                                    <asp:BoundField ItemStyle-Font-Bold="true" DataField="total_score" SortExpression="total_score" HeaderText="Score" />
                                    <asp:BoundField ItemStyle-Font-Bold="true" DataField="num_missed" SortExpression="num_missed" HeaderText="# Missed" />
                                    <asp:BoundField DataField="missed_list" SortExpression="missed_list" HeaderText="Missed Items" />

                                    <asp:BoundField DataField="Comments" SortExpression="Comments" HtmlEncode="false" HeaderText="Comments" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- close table-outline -->
            </div>
            <!-- close calls-list -->
        </div>
        <!-- close panel-content -->

        <div class="audio-player" id="notification_holder" style="display: none;">
            <audio id="notification_player"></audio>
            <div class="audio-player-inner-content">

                <div class="show-hide-player">
                    <a href="#" class="hide-player"><i class="fa fa-caret-down"></i>HIDE</a>
                    <a href="#" class="show-player"><i class="fa fa-caret-up"></i>&nbsp; SHOW</a>
                </div>

                <div class="player-left-part">
                    <i class="fa icon-mute fa-volume-up"></i>
                    <div class="volumne-options">
                        <span class="section-label">Volume</span>
                        <div id="volume-slider" class="slider volume-slider dragdealer">
                            <div class="slider-trigger handle"></div>
                            <div class="slider-fill"></div>
                        </div>
                        <!-- close slider -->
                    </div>
                    <!-- close volume-options -->

                    <div class="player-controls">
                        <a href="#" class="play-button"><i class="fa icon-play-pause"></i></a>
                        <div class="audio-rate">
                            <a href="#" title="-" data-rate="-<%=data_rate%>">-</a>
                            <a href="#" title="Normal speed" data-rate="0">0</a>
                            <a href="#" title="+" data-rate="<%=data_rate%>">+</a>
                        </div>
                    </div>
                    <!-- close player-controls -->
                </div>
                <!-- close player-left-part -->

                <div class="player-timeline">
                    <span class="section-label">
                        <span class="audio-current-time">0:00</span> / <span class="audio-duration">0:00</span>
                    </span>

                    <div id="timeline-slider" class="slider timeline-slider dragdealer">
                        <div class="slider-trigger handle">
                            <div>
                                <span class="audio-current-time">0:00</span>
                            </div>
                        </div>
                        <!-- close slider-trigger -->
                        <div id="warning_indicators">
                            <!-- close warning-part -->
                        </div>
                        <div class="slider-fill"></div>
                    </div>
                    <!-- close slider -->
                </div>
                <!-- close player-timeline -->


                <div class="player-agent">
                    <asp:HiddenField ID="hdnOpenFormID" runat="server" />

                    <div>
                    </div>

                </div>
                <!-- close player-agent -->

            </div>
            <!-- close audio-player-inner-content -->
        </div>


    </section>
    <script type="text/javascript">
        setupCustomScrollbars();
    </script>
    <asp:HiddenField ID="hdnAgentFilter" runat="server" />
</asp:Content>
