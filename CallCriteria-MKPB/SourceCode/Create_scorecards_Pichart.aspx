<%@ Page Title="" Language="C#" MasterPageFile="~/CC_Master.master" AutoEventWireup="true" CodeFile="Create_scorecards_Pichart.aspx.cs" Inherits="Create_scorecards_Pichart" EnableEventValidation="false" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript">      </script>
    <script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization','version':'1.1','packages':['corechart']}]}"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization','version':'1.1','packages':['imagebarchart']}]}"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.js" type="text/javascript">      </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js" type="text/javascript">      </script>
    <style type="text/css">
        .auto-style1 {
            font-size: large;
        }
        .auto-style2 {
            font-size: x-large;
        }
        #ContentPlaceHolder1_Button1:hover
        {
            background-color: #597D32;
    border: 1px solid #FFF;
    color: white;
        }
         #ContentPlaceHolder1_Button1
        {
            background-color: #64982D;
    border: 1px solid #FFF;
    color: white;
        }
        #ContentPlaceHolder1_DropDownList2
        {
    display: block;
    position: relative;        
    border-radius: 3px;
    color: #333333;
    font-family: 'Open Sans', Arial, Helvetica, sans-serif;
    font-weight: 400;
    line-height: 28px;
    cursor: pointer;
    z-index: 30;
        }
        #ContentPlaceHolder1_DropDownList3
        {
    display: block;
    position: relative;   
    border-radius: 3px;
    color: #333333;
    font-family: 'Open Sans', Arial, Helvetica, sans-serif;
    font-weight: 400;
    line-height: 28px;
    cursor: pointer;
    z-index: 30;
        }
          #ContentPlaceHolder1_scorecarddropdown
        {
    display: block;
    position: relative;   
    border-radius: 3px;
    color: #333333;
    font-family: 'Open Sans', Arial, Helvetica, sans-serif;
    font-weight: 400;
    line-height: 28px;
    cursor: pointer;
    z-index: 30;
        }
           #ContentPlaceHolder1_scrrcrddrop
        {
    display: block;
    position: relative;   
    border-radius: 3px;
    color: #333333;
    font-family: 'Open Sans', Arial, Helvetica, sans-serif;
    font-weight: 400;
    line-height: 28px;
    cursor: pointer;
    z-index: 30;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <%-- <asp:HiddenField ID="hiddendroporclick" runat="server" Value="All" />--%>
        <table>
             <tr>
                <td colspan="2" style="width: 15%;"><img src="http://partner.callcriteria.com/images/edufficient.png"/></td>
                 <td colspan="3"><p class="auto-style2" style="height: 28px;"><strong> Edufficient Group Compliance</strong></p></td>
                </tr>
        </table>

        <table style="width: 58%;" class="header-table" cellspacing="10">
           

            <tr>

                <td>
                    <asp:DropDownList ID="DropDownList2" runat="server" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AppendDataBoundItems="true" Font-Size="16px" Height="33px" Width="90px">
                        <Items>
                            <%-- <asp:ListItem Text="Month" Value="">Month</asp:ListItem>--%>
                            <asp:ListItem Text="Jan" Value="01">Jan</asp:ListItem>
                            <asp:ListItem Text="Feb" Value="02">Feb</asp:ListItem>
                            <asp:ListItem Text="Mar" Selected="True" Value="03">Mar</asp:ListItem>
                            <asp:ListItem Text="Apr" Value="04">Apr</asp:ListItem>
                            <asp:ListItem Text="May" Value="05">May</asp:ListItem>
                            <asp:ListItem Text="Jun" Value="06">Jun</asp:ListItem>
                            <asp:ListItem Text="Jul" Value="07">Jul</asp:ListItem>
                            <asp:ListItem Text="Aug" Value="08">Aug</asp:ListItem>
                            <asp:ListItem Text="Sep" Value="09">Sep</asp:ListItem>
                            <asp:ListItem Text="Oct" Value="10">Oct</asp:ListItem>
                            <asp:ListItem Text="Nov" Value="11">Nov</asp:ListItem>
                            <asp:ListItem Text="Dec" Value="12">Dec</asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                </td>
              
                <td>
                    <asp:DropDownList ID="DropDownList3" runat="server"  AppendDataBoundItems="true" Font-Size="16px" Height="33px" Width="88px">
                        <Items>
                            <%--<asp:ListItem Text="Select" Value="">Select</asp:ListItem>--%>
                            <asp:ListItem Text="2014" Value="2014">2014</asp:ListItem>
                            <asp:ListItem Text="2015" Value="2015">2015</asp:ListItem>
                            <asp:ListItem Text="2016" Selected="True" Value="2016">2016</asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                    <%-- <asp:SqlDataSource ID="SqlDataSource1" runat="server" //DataSourceID="SqlDataSource1" DataTextField="yearlist" DataValueField="yearlist"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select distinct year(date_added) as yearlist  from  vwform order by yearlist">
                    </asp:SqlDataSource>--%>
                </td>

                <td>
                    <asp:DropDownList ID="scorecarddropdown" runat="server" DataTextField="short_name" DataValueField="id" AppendDataBoundItems="true" Font-Size="16px" Height="33px" Width="258px">
                        <Items>
                            <asp:ListItem Text="All" Value="All" Selected="True">All</asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                    <%--<asp:SqlDataSource ID="scorecardsource" runat="server"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                     
                        
 SelectCommand="select distinct short_name, scorecards.ID from xcc_report_new join scorecards on scorecards.id = scorecard where datepart(month, call_date) = @month and datepart(year, call_date) = @year and scorecards.appname = 'edufficient' order by short_name">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList2" Name="month" />
                            <asp:ControlParameter ControlID="DropDownList3" Name="year" />
                          
                        </SelectParameters>
                    </asp:SqlDataSource>--%>
                </td>
                <td class="auto-style1">
                    <%-- <asp:Label ID="Label2" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">Select Scorecard</asp:Label>--%>
                    <asp:DropDownList ID="scrrcrddrop" runat="server" DataSourceID="dsQAs" DataTextField="agent" DataValueField="agent" AppendDataBoundItems="true" Font-Size="16px" Height="33px" Width="258px">
                        <Items>
                            <%-- <asp:ListItem Text="Select" Value="">Select</asp:ListItem>--%>
                            <asp:ListItem Text="All" Value="All" Selected="True">All</asp:ListItem>
                            <%-- <asp:ListItem Text="allen" Value="allen">allen</asp:ListItem>
                                    <asp:ListItem Text="cehe" Value="cehe">cehe</asp:ListItem>
                                    <asp:ListItem Text="delta" Value="delta">delta</asp:ListItem>
                                    <asp:ListItem Text="EVP" Value="EVP">EVP</asp:ListItem>
                                    <asp:ListItem Text="post" Value="post">post</asp:ListItem>
                                    <asp:ListItem Text="quad" Value="quad">quad</asp:ListItem>
                                    <asp:ListItem Text="STAR" Value="STAR">STAR</asp:ListItem>--%>
                        </Items>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsQAs" runat="server"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                      
                      SelectCommand="select distinct agent from vwform where agent is not null and agent !=''
                        and datepart(month, call_date) = @month and datepart(year, call_date) = @year 
                        and 1 = case when @scorecard = 'All' then 1 when scorecard = @scorecard then 1 else 0 end  and appname = 'edufficient' order by agent">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList2" Name="month" />
                            <asp:ControlParameter ControlID="DropDownList3" Name="year" />
                            <asp:ControlParameter ControlID="scorecarddropdown" Name="scorecard" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>

                <td>
                    <asp:Button ID="Button1" runat="server" Text="Export to Pdf"  OnClick="Button1_Click1"  />
                </td>
               
            </tr>
            <tr>
                <td></td>
                <td></td>
                 <td>
                    <div class="loading" id="loadimg" style="display: none;"> 
            <img src="/img/103.gif"/>
        </div>
                </td>
                <td></td>
                <td></td>
            </tr>



        </table>
         <asp:HiddenField ID="hdnscorecardtext" runat="server" Value="All" />
         <asp:HiddenField ID="hdnscorecardvalue" runat="server" Value="All" />
        <asp:HiddenField ID="scrrcrdid" runat="server" Value="All" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table id="tbl1" style="width:100%;">
                    <tr style="background-color: white">
                        
                        <td>
                            <div id="visualization" style="width: 700px; height: 550px; margin-top: 20px;">
                            </div>
                            <asp:HiddenField ID="Hiddengraphimage1" runat="server" Value="All" />
                            <asp:HiddenField ID="Hiddengraphimage1details" runat="server" Value="All" />
                        </td>
                        <td style="vertical-align:top">

                            <div id="visualizationfirst" style="background-color: white; font-size: 13px; font-family: Arial; text-align: left; padding-top: 77px;">
                            </div>
                        </td>
                    </tr>
                    <tr style="background-color: white">
                       
                        <td>
                            <div id="visualization1" style="width: 700px; height: 550px; margin-top: 20px;">
                            </div>
                            <asp:HiddenField ID="Hiddengraphimage2" runat="server" Value="All" />
                            <asp:HiddenField ID="Hiddengraphimage2details" runat="server" Value="All" />
                        </td>
                         <td style="vertical-align:top">
                            <div id="visualization11" style="background-color: white; font-size: 13px; font-family: Arial; text-align: left; padding-top: 75px;">
                            </div>
                        </td>
                    </tr>
                    <tr style="background-color: white" id="thirdchartid">
                      
                        <td>
                            <div id="visualization2" style="width: 800px;">
                            </div>
                            <asp:HiddenField ID="Hiddengraphimage3" runat="server" Value="All" />
                            <asp:HiddenField ID="Hiddengraphimage3details" runat="server" Value="All" />
                        </td>
                          <td>
                            <div id="visualization21" style="background-color: white; font-size: 13px; font-family: Arial; text-align: left; padding-top: 0px;">
                            </div>
                        </td>
                    </tr>
                    <%--      ////////////--%>
                    <tr style="background-color: white">
                        
                        <td>
                            <div id="visualization4" style="width: 700px; height: 550px; margin-top: 20px;">
                            </div>
                            <asp:HiddenField ID="Hiddengraphimage4" runat="server" Value="All" />
                             <asp:HiddenField ID="Hiddengraphimage4details" runat="server" Value="All" />
                        </td>
                        <td>
                            <div id="visualization41" style="background-color: white; font-size: 13px; font-family: Arial; text-align: left; padding-top: 0px;">
                            </div>
                        </td>
                    </tr>
                    <%--      ////////////--%>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                          <table id="colorkeys">
                            <tr>
                               <td style="background-color:#3399CC;">&nbsp;&nbsp;</td> <td><div>COMPLIANT PAGES</div></td>
                            </tr>
                            <tr>
                               <td style="background-color:#80C65A;">&nbsp;&nbsp;</td> <td><div>NON COMPLIANT PAGES</div></td>
                            </tr>
                        </table>
                            </td>
                    </tr>
                    <tr>

                        <td>                       
                <asp:GridView ID="GridViewcomplains" DataSourceID="dsComplains" CssClass="detailsTable" Style="font-size: xx-small;float:left;" runat="server" AutoGenerateColumns="false" DataKeyNames="agent" OnRowDataBound="GridViewcomplains_RowDataBound" Width="100%" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:BoundField DataField="agent" HeaderText="Partner" SortExpression="agent" />
                        <asp:BoundField DataField="date_last_checked" HeaderText="Date Last Checked" SortExpression="date_last_checked" />
                        <asp:BoundField DataField="pass" HeaderText="Compliant pages" SortExpression="pass" />
                        <asp:BoundField DataField="fail" HeaderText="Non Compliant pages" SortExpression="fail" />
                        <asp:TemplateField HeaderText="" SortExpression="username">
                            <HeaderStyle BackColor="#41637C" ForeColor="White" />
                            <ItemTemplate>
                                <div id="chart_div<%#(Container.DataItemIndex+1)%>" style="height: 25px; width: 200px; overflow: hidden"></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                    
                      
                         </td>
                    </tr>
                    </table>

                <asp:SqlDataSource ID="dsComplains" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand=" select agent, sum(case when pass_fail = 'Pass' then 1 else 0 end) as pass, 
                    sum(case when pass_fail = 'Fail' then 1 else 0 end) as fail, max(review_date) as date_last_checked
                     from vwForm where datepart(month, call_date) = @month and datepart(year, call_date) = @year 
                     and appname = 'edufficient' 
                     and 1 = case when @agent = 'All' then 1 when agent=@agent then 1 else 0 end 
                     and 1 = case when @scorecard = 'All' then 1 when scorecard=@scorecard then 1 else 0 end
                     group by agent">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownList2" Name="month" />
                        <asp:ControlParameter ControlID="DropDownList3" Name="year" />
                        <asp:ControlParameter ControlID="scorecarddropdown" Name="scorecard" />
                        <asp:ControlParameter ControlID="scrrcrddrop" Name="agent" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <%-- //////////////////--%>
                <asp:GridView ID="GridView2complains" CssClass="detailsTable" Style="font-size: xx-small;" runat="server" AutoGenerateColumns="false" DataKeyNames="reviewer" Width="99%" ShowHeaderWhenEmpty="True">
                    <Columns>                        
                         <asp:HyperLinkField DataNavigateUrlFields="F_id" Target="_blank" DataTextField="F_id" HeaderText="Link to Record" SortExpression="F_id"  DataNavigateUrlFormatString="review_record.aspx?ID={0}"/> 
                         <asp:BoundField DataField="agent" HeaderText="partner Name" SortExpression="agent" />
                    <%--<asp:BoundField DataField="reviewer" HeaderText="reviewer" SortExpression="reviewer"/>  --%>
                          <asp:BoundField DataField="scorecardname" HeaderText="scorecard" SortExpression="scorecardname" />
                          <asp:BoundField DataField="agent_group" HeaderText="school name" SortExpression="agent_group" />                    
                         <asp:HyperLinkField DataNavigateUrlFields="website" Target="_blank" DataTextField="website" HeaderText="link" SortExpression="website" /> 
                           <asp:BoundField DataField="pass_fail" HeaderText="pass/fail" SortExpression="pass_fail" />                                      
                         <asp:BoundField DataField="total_score" HeaderText="score" SortExpression="total_score" />
                          <asp:BoundField DataField="num_missed" HeaderText="num missed" SortExpression="num_missed" />
                          <asp:BoundField DataField="missed_list" HeaderText="Missed Item List" SortExpression="missed_list" />
                          <asp:BoundField DataField="review_date" HeaderText="Review Date" SortExpression="review_date" />                      
                     <%--<asp:HyperLinkField DataNavigateUrlFields="audio_link" Target="_blank" DataTextField="audio_link" HeaderText="audio_link" SortExpression="audio_link"/>                     --%>
                        
                      
                      
                    
                       
                        


                    </Columns>
                </asp:GridView>              
               
            </ContentTemplate>
        </asp:UpdatePanel>
         

    </section>
    <div style="margin-top: 50px;display:none;" id="canvasImg"></div>
     <asp:HiddenField ID="gridviewdetails" runat="server" Value="All" />
   <%-- //////////////////////audio player--%>
    
   <%-- /////////////////--%>


    <script type="text/javascript">
        // google.load('visualization', '1', { packages: ['corechart'] });
        //google.setOnLoadCallback(drawChart);
        $('#visualization41').hide();
        $('#visualization4').hide();

        $("#ContentPlaceHolder1_GridViewcomplains").show();
        $("#colorkeys").show();
        $("#ContentPlaceHolder1_GridView2complains").hide();
        
        $(document).ready(function () {
            $("#ContentPlaceHolder1_Button1").hide();
            $("#ContentPlaceHolder1_scrrcrddrop").val('All');
            bindscorecarddropdown();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetData',
                data: '{dropval:"All",mnth:"03",yrval:"2016",scorecarddropdown:"All"}',
                success:
                    function (response) {

                        drawVisualization(response.d);
                        // drawChart();
                    }
            });

            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetDatainfraction',
                data: '{dropval:"All",mnth:"03",yrval:"2016",scorecarddropdown:"All"}',
                success:
                    function (response) {
                        drawVisualizationinfraction(response.d);
                        // detailsdrawChart();
                    }
            });

            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetDatainfractiondetails',
                data: '{dropval:"All",mnth:"03",yrval:"2016",scorecarddropdown:"All"}',
                success:
                    function (response) {
                       
                        drawVisualizationinfractiondetails(response.d);
                    }
            });

            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetDatabardetails',
                data: '{dropval:"All",mnth:"03",yrval:"2016",scorecarddropdown:"All"}',
                success:
                    function (response) {
                     
                        drawbarChart2(response.d);
                      
                        htmlconverttoimage('ContentPlaceHolder1_GridViewcomplains');
                    }
            });           
          
          
           
        });


        //1 st pie chart
        function drawVisualization(dataValues) {
            $("#ContentPlaceHolder1_Button1").hide();
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Column Name');
            data.addColumn('number', 'Column Value');
            $("#visualizationfirst").html('');
            var html = '<table style="width:100%;text-align: left;padding: 39px;"><tbody>';
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                html +='<tr><td style="width:90%">' + [dataValues[i].ColumnName] + '</td><td style="width:30%">' + '<b>' + [dataValues[i].Value] + '</b></td></tr>';
            }
            html += '</tbody></table>';
            $("#visualizationfirst").append(html);
            //new google.visualization.PieChart(document.getElementById('visualization')).
            //    draw(data, { title: "PAGE COMPLIANCE SNAPSHOT :OVERALL", width: '700', height: '500', is3D: true });

            //var columnchart= new google.visualization.PieChart(document.getElementById('visualization'));
            //google.visualization.events.addListener(columnchart, 'ready', function () {
            //    var ExportData = columnchart.getImageURI();
            //    $('#Labelgraphimage').attr({ 'href': ExportData, 'download': 'first graph chart' }).show();
            //});
            var visualizationa = document.getElementById('visualization');
            var chart = new google.visualization.PieChart(document.getElementById('visualization'));
            //var chart2 = new google.visualization.DataTable(document.getElementById('visualizationfirst'));
            //google.visualization.events.addListener(chart2, 'ready', function () {

            //    $('#ContentPlaceHolder1_Hiddengraphimage1details').val(chart2.getImageURI());
            //});

            google.visualization.events.addListener(chart, 'ready', function () {
                //visualizationa.innerHTML = '<img id="firstchart" src="' + chart.getImageURI() + '">';
                // $('#ContentPlaceHolder1_Hiddengraphimage1').val($("#firstchart").attr('src'));
                $('#ContentPlaceHolder1_Hiddengraphimage1').val(chart.getImageURI());
            });

            chart.draw(data, { title: "PAGE COMPLIANCE SNAPSHOT: OVERALL", width: '700', height: '500', is3D: true });

            $("#ContentPlaceHolder1_Hiddengraphimage1details").val($("#visualizationfirst").html());

        }

        //2nd pie chart
        function drawVisualizationinfraction(dataValues) {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Column Name');
            data.addColumn('number', 'Column Value');
          
            $("#visualization11").html('');
            var html = '<table style="width:100%;text-align: left;padding: 39px;"><tbody>';
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                html +='<tr><td style="width:90%">' + [dataValues[i].ColumnName] + '</td><td style="width:10%">' + '<b>' + [dataValues[i].Value] + '</b></td></tr>';
            }

            html += '</tbody></table>';
            $("#visualization11").append(html);
            var options = {
                'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION CATEGORY',
                'width': 700,
                'height': 500,
                is3D: true,
                'colors': ['#dc3912', '#3366cc']
            };

            var visualizationaa = document.getElementById('visualization1');
            var chart = new google.visualization.PieChart(document.getElementById('visualization1'));
            //////////////
            //var chart2 = new google.visualization.PieChart(document.getElementById('visualization11'));
            //google.visualization.events.addListener(chart2, 'ready', function () {

            //    $('#ContentPlaceHolder1_Hiddengraphimage2details').val(chart2.getImageURI());
            //});
            google.visualization.events.addListener(chart, 'ready', function () {
                //visualization.innerHTML = '<img id="secondchart" src="' + chart.getImageURI() + '">';
                //$('#ContentPlaceHolder1_Hiddengraphimage2').val($("#secondchart").attr('src'));
                $('#ContentPlaceHolder1_Hiddengraphimage2').val(chart.getImageURI());
            });
            ////////////////
            chart.draw(data, options);
            /////////

            ///////////////////
            google.visualization.events.addListener(chart, 'select', function () {
                var selection = chart.getSelection();
                var message = '';

                for (var i = 0; i < selection.length; i++) {
                    var item = selection[i];
                    if (item.row != null && item.column != null) {
                        message += '{row:' + item.row + ',column:' + item.column + '}';
                    } else if (item.row != null) {
                        message += '{row:' + item.row + '}';
                    } else if (item.column != null) {
                        message += '{column:' + item.column + '}';
                    }
                }
                if (message == '') {
                    message = 'nothing';
                }
                var qshortname = data.getValue(chart.getSelection()[0].row, 0);
                //selectHandler(qshortname);
                // ajaxinfractiongraph(qshortname);
            });
            ////////////////// 
            if ($("#visualization11 table tr").length > 0) {
                $("#ContentPlaceHolder1_Hiddengraphimage2details").val($("#visualization11").html());
            }
            else {
                $("#ContentPlaceHolder1_Hiddengraphimage2details").val('<table style="width:100%;text-align: left;padding: 39px;"><tbody><tr><td></td><td></td></tr></tbody></table>');
            }
           
        }

        //3rd pie chart

        function drawVisualizationinfractiondetails(dataValues) {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Column Name');
            data.addColumn('number', 'Column Value');
            $("#visualization21").html('');
            var html = '<table style="width:100%;text-align: left;padding: 39px;"><tbody>';
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                html +='<tr><td style="width:90%">' + [dataValues[i].ColumnName] + '</td><td style="width:10%">' + '<b>' + [dataValues[i].Value] + '</b></td></tr>';
            }
            html += '</tbody></table>';
            $("#visualization21").append(html);
            //var heght = $("#visualization21 table").height();
            //if (heght < 650)
            //{
            //    heght=650
            //}
            //$("#visualization2").attr('height', '650');
            //    var options = {
            //        'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
            //        'width': '800',
            //        'height': heght,
            //        'fontSize': 12,
            //        is3D: true,

            //    };


           
            if (dataValues.length <= 15)
            {
                $("#visualization2").attr('height', '650');
                var options = {
                    'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
                    'width': '800',
                    'height': '600',
                    'fontSize': 12,
                    is3D: true,
                    sliceVisibilityThreshold: 0.0,

                };
              
            }
            if (dataValues.length > 15 && dataValues.length < 30) {
                $("#visualization2").attr('height', '1000');
                var options = {
                    'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
                    'width': '800',
                    'height': '900',
                    'fontSize': 12,
                    is3D: true,
                    sliceVisibilityThreshold: 0.0,

                };
               
            }
            if (dataValues.length > 30 && dataValues.length < 50) {
                $("#visualization2").attr('height', '1450');
                var options = {
                    'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
                    'width': '800',
                    'height': '1250',
                    'fontSize': 12,                   
                    is3D: true,
                    sliceVisibilityThreshold: 0.0,

                };
                
            }
            if (dataValues.length > 50 && dataValues.length < 100) {
                $("#visualization2").attr('height', '1950');
                var options = {
                    'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
                    'width': '800',
                    'height': '1780',
                    'fontSize': 12,                   
                    is3D: true,
                    sliceVisibilityThreshold: 0.0,

                };
               
            }
            if (dataValues.length > 100) {
                $("#visualization2").attr('height', '2100');
                var options = {
                    'title': 'PAGE COMPLIANCE SNAPSHOT: INFRACTION DETAIL',
                    'width': '800',
                    'height': '2050',
                    'fontSize': 12,
                    is3D: true,
                    sliceVisibilityThreshold: 0.0,

                };
                
            }
        

            var visualizationas = document.getElementById('visualization2');
            var chart = new google.visualization.PieChart(document.getElementById('visualization2'));
            //////////
            //var chart2 = new google.visualization.PieChart(document.getElementById('visualization11'));
            //google.visualization.events.addListener(chart2, 'ready', function () {

            //    $('#ContentPlaceHolder1_Hiddengraphimage2details').val(chart2.getImageURI());
            //});
            google.visualization.events.addListener(chart, 'ready', function () {
                //visualization.innerHTML = '<img id="thirdchart" src="' + chart.getImageURI() + '">';
                //$('#ContentPlaceHolder1_Hiddengraphimage3').val($("#thirdchart").attr('src'));
                $('#ContentPlaceHolder1_Hiddengraphimage3').val(chart.getImageURI());
            });
            chart.draw(data, options);
            ///////////////

            ////////////////
            google.visualization.events.addListener(chart, 'select', function () {
                var selection = chart.getSelection();
                var message = '';

                for (var i = 0; i < selection.length; i++) {
                    var item = selection[i];
                    if (item.row != null && item.column != null) {
                        message += '{row:' + item.row + ',column:' + item.column + '}';
                    } else if (item.row != null) {
                        message += '{row:' + item.row + '}';
                    } else if (item.column != null) {
                        message += '{column:' + item.column + '}';
                    }
                }
                if (message == '') {
                    message = 'nothing';
                }

                var qshortname = data.getValue(chart.getSelection()[0].row, 0);
                // selectHandler(qshortname);
                //$('#ContentPlaceHolder1_scorecarddropdown option').filter(function () { return $(this).html() == qshortname; }).attr('selected', 'selected');
                //ajaxdropbindgraph();

                //dropbindgrid();


            });
            if ($("#visualization21 table tr").length > 0)
            {
                $("#ContentPlaceHolder1_Hiddengraphimage3details").val($("#visualization21").html());
            }
            else
            {
                $("#ContentPlaceHolder1_Hiddengraphimage3details").val('<table style="width:100%;text-align: left;padding: 39px;"><tbody><tr><td></td><td></td></tr></tbody></table>');
            }
           


        }
        //select alert on 3rd pie chart
        function selectHandler(qshortname) {
            $("#ContentPlaceHolder1_GridViewcomplains").show();
            $("#colorkeys").show();
            $("#ContentPlaceHolder1_GridView2complains").hide();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/Getgridonclick',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",qshortname:"' + qshortname + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
                success:
                    function (response) {
                        //drawbarChart2(response.d)
                        //////////////

                        var xmlDoc = $.parseXML(response.d);
                        var xml = $(xmlDoc);
                        var customers = xml.find("Table");
                        $("#ContentPlaceHolder1_GridViewcomplains").append('<tr><td></td><td></td><td></td><td></td><td></td><td></td></tr>');
                        var row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
                        $("#ContentPlaceHolder1_GridViewcomplains tr").not($("#ContentPlaceHolder1_GridViewcomplains tr:first-child")).remove();
                        var numberr = 1;
                        var p = 0;
                        $.each(customers, function () {
                            var customer = $(this);
                            $("td", row).eq(0).html($(this).find("agent").text());
                            $("td", row).eq(1).html($(this).find("week_ending_date").text());
                            $("td", row).eq(2).html($(this).find("complaint").text());
                            $("td", row).eq(3).html($(this).find("noncomplaint").text());
                            $("td", row).eq(4).html('').append('<div id="chart_div' + numberr + '" style="height:25px; width:200px;overflow:hidden;"></div>');

                            $("#ContentPlaceHolder1_GridViewcomplains").append(row);
                            row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
                            numberr++;
                            //////////////
                            var dataTable = new google.visualization.DataTable();
                            dataTable.addColumn('number');
                            dataTable.addColumn('number');
                            dataTable.addRows([
                           [parseInt($(this).find("complaint").text()), parseInt($(this).find("noncomplaint").text())]

                            ]);

                            p = p + 1;
                            var options = {
                                'width': 200,
                                'height': 20,
                                'showCategoryLabels': false,
                                'showValueLabels': false,
                                legend: { position: "none" },


                            };
                            var chart = new google.visualization.ImageBarChart(document.getElementById('chart_div' + p));
                            chart.draw(dataTable, options);
                            ///////////////////
                        });

                        ////////////
                    }
            });

        }

        // make bar chart
        function drawbarChart2(dataValues) {
            var p = 0;
            //$(".detailsTable tbody tr:not(first) td:nth-child(5)").each(function () {
            $("#ContentPlaceHolder1_GridViewcomplains tbody tr:not(first) td:nth-child(5)").each(function () {
                //alert(dataValues[p].firstvalue);
                var dataTable = new google.visualization.DataTable();
                dataTable.addColumn('number');
                dataTable.addColumn('number');
                dataTable.addRows([
               [dataValues[p].firstvalue, dataValues[p].secondvalue]

                ]);

                p = p + 1;
                var options = {
                    'width': 200,
                    'height': 20,
                    'showCategoryLabels': false,
                    'showValueLabels': false,
                    legend: { position: "none" },


                };
                var chart = new google.visualization.ImageBarChart(document.getElementById('chart_div' + p));
                chart.draw(dataTable, options);

            });


        }



        function drawChart() {

            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetDatainfraction',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
                success:
                    function (response) {
                        drawVisualizationinfraction(response.d);
                        detailsdrawChart();
                    }
            });

        }


        function detailsdrawChart() {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetDatainfractiondetails',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
                success:
                    function (response) {
                        drawVisualizationinfractiondetails(response.d);
                    }
            });

        }

        /////////////////////////////

        //dropdown change function
        $("#ContentPlaceHolder1_scrrcrddrop").change(function (event) {
            $("#ContentPlaceHolder1_Button1").hide();
            setValues();
           
            ajaxdropbindgraph();

            dropbindgrid();

            if ($('#ContentPlaceHolder1_scorecarddropdown').val() == 'All' && $('#ContentPlaceHolder1_scrrcrddrop').val() != 'All') {
                graphfour();
                $('#visualization41').show();
                $('#visualization4').show();
                $("#ContentPlaceHolder1_GridViewcomplains").hide();
                $("#colorkeys").hide();
                $("#ContentPlaceHolder1_GridView2complains").show();
                //$("#ContentPlaceHolder1_gridviewdetails").val('');
                //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridView2complains").html());
                //htmlconverttoimage(ContentPlaceHolder1_GridView2complains);
            }
            else {
                $('#visualization41').hide();
                $('#visualization4').hide();
                $("#ContentPlaceHolder1_GridViewcomplains").show();
                $("#colorkeys").show();
                $("#ContentPlaceHolder1_GridView2complains").hide();
                //$("#ContentPlaceHolder1_gridviewdetails").val('');
                //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridViewcomplains").html());
               // htmlconverttoimage(ContentPlaceHolder1_GridViewcomplains);
            }
      
        });
        //hidden field value set
        function setValues() {
            $('#ContentPlaceHolder1_scrrcrdid').val($('#ContentPlaceHolder1_scrrcrddrop').val());

        }
        //hdden value for scrcardid
        //hidden field value set
        function setcorecardhddnValues() {
            
            $('#ContentPlaceHolder1_hdnscorecardtext').val($('#ContentPlaceHolder1_scorecarddropdown option:selected').text());
            $('#ContentPlaceHolder1_hdnscorecardvalue').val($('#ContentPlaceHolder1_scorecarddropdown').val());
        }
        $('#ContentPlaceHolder1_DropDownList2').change(function () {
            $("#ContentPlaceHolder1_Button1").hide();
            $('#ContentPlaceHolder1_hdnscorecardtext').val('All');
            $('#ContentPlaceHolder1_hdnscorecardvalue').val('All');
            bindscorecarddropdown();
            ajaxdropbindgraph();
         
            dropbindgrid();
          
        });
        $('#ContentPlaceHolder1_DropDownList3').change(function () {
            $("#ContentPlaceHolder1_Button1").hide();
            $('#ContentPlaceHolder1_hdnscorecardtext').val('All');
            $('#ContentPlaceHolder1_hdnscorecardvalue').val('All');
            bindscorecarddropdown();
            ajaxdropbindgraph();
          
            dropbindgrid();
         
         

        });
        $('#ContentPlaceHolder1_scorecarddropdown').change(function () {
          $("#ContentPlaceHolder1_Button1").hide();
            setcorecardhddnValues();
            bindagentdropdown();
            

            //if ($('#ContentPlaceHolder1_scorecarddropdown').val() == 'All' && $('#ContentPlaceHolder1_scrrcrddrop').val() != 'All') {
            //    graphfour();
            //    $('#visualization41').show();
            //    $('#visualization4').show();
            //    $("#ContentPlaceHolder1_GridViewcomplains").hide();
            //    $("#colorkeys").hide();
            //    $("#ContentPlaceHolder1_GridView2complains").show();
            //    //$("#ContentPlaceHolder1_gridviewdetails").val('');
            //    //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridView2complains").html());
            //   // htmlconverttoimage(ContentPlaceHolder1_GridView2complains);
            //}
            //else {
                $('#visualization41').hide();
                $('#visualization4').hide();
                $("#ContentPlaceHolder1_GridViewcomplains").show();
                $("#colorkeys").show();
                $("#ContentPlaceHolder1_GridView2complains").hide();
                //$("#ContentPlaceHolder1_gridviewdetails").val('');
                //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridViewcomplains").html());
              //  htmlconverttoimage(ContentPlaceHolder1_GridViewcomplains);
           // }
       
        });
        function ajaxdropbindgraph() {
            $("#loadimg").show();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/GetData',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',              
                success:
                    function (response) {
                        drawVisualization(response.d);
                        drawChart();                     
                        dropbindgrid();
                    }
            });
        }
        function dropbindgrid() {
            ////////////////
            var qshortname = "";
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/Getgridonclick',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",qshortname:"' + qshortname + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
                success:
                    function (response) {
                        if ($('#ContentPlaceHolder1_scorecarddropdown').val() == 'All' && $('#ContentPlaceHolder1_scrrcrddrop').val() != 'All') {

                            var xmlDoc = $.parseXML(response.d);
                            var xml = $(xmlDoc);
                            var customers = xml.find("Table");
                            $("#ContentPlaceHolder1_GridView2complains").append('<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>');
                            //$("#ContentPlaceHolder1_GridViewcomplains").append('<tr><td></td><td></td><td></td><td></td><td></td><td></td></tr>');
                            var row = $("#ContentPlaceHolder1_GridView2complains tr:last-child").clone(true);
                            $("#ContentPlaceHolder1_GridView2complains tr").not($("#ContentPlaceHolder1_GridView2complains tr:first-child")).remove();
                            // alert('ss');
                            var numberr = 1;
                            var p = 0;
                            $.each(customers, function () {
                                //,num_missed,audio_link,  F_id     
                                var customer = $(this);
                                $("td", row).eq(0).html("<a href='review_record.aspx?ID=" + $(this).find("F_id").text() + "' target='_blank'>" + $(this).find("F_id").text() + "</a>");
                                $("td", row).eq(1).html($(this).find("agent").text());
                                $("td", row).eq(2).html($(this).find("scorecardname").text());
                                $("td", row).eq(3).html($(this).find("agent_group").text());
                                $("td", row).eq(4).html("<a href='" + $(this).find("website").text() + "' target='_blank'>" + $(this).find("website").text() + "</a>");
                                $("td", row).eq(5).html($(this).find("pass_fail").text()); 
                                $("td", row).eq(6).html($(this).find("total_score").text());
                                $("td", row).eq(7).html($(this).find("num_missed").text());
                                $("td", row).eq(8).html($(this).find("missed_list").text());
                                $("td", row).eq(9).html($(this).find("review_date").text());
                                var symbol = "'";
                                //$("td", row).eq(10).html("<a href='" + $(this).find("audio_link").text() + "' target='_blank'>" + $(this).find("audio_link").text() + "</a>");
                               
                                                       

                                $("#ContentPlaceHolder1_GridView2complains").append(row);

                                row = $("#ContentPlaceHolder1_GridView2complains tr:last-child").clone(true);
                                numberr++;

                            });
                            //$("#ContentPlaceHolder1_gridviewdetails").val('');
                            //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridView2complains").html());
                            htmlconverttoimage('ContentPlaceHolder1_GridView2complains');
                            //////////////////////////////////////////////////////////
                        }
                        else {
                            var xmlDoc = $.parseXML(response.d);
                            var xml = $(xmlDoc);
                            var customers = xml.find("Table");

                            $("#ContentPlaceHolder1_GridViewcomplains").append('<tr><td></td><td></td><td></td><td></td><td></td></tr>');
                            var row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
                            $("#ContentPlaceHolder1_GridViewcomplains tr").not($("#ContentPlaceHolder1_GridViewcomplains tr:first-child")).remove();

                            var numberr = 1;
                            var p = 0;
                            $.each(customers, function () {

                                var customer = $(this);
                                $("td", row).eq(0).html($(this).find("agent").text());
                                $("td", row).eq(1).html($(this).find("date_last_checked").text());
                                $("td", row).eq(2).html($(this).find("pass").text());
                                $("td", row).eq(3).html($(this).find("fail").text());
                                $("td", row).eq(4).html('').append('<div id="chart_div' + numberr + '" style="height:25px; width:200px;overflow:hidden;"></div>');
                                $("#ContentPlaceHolder1_GridViewcomplains").append(row);
                               
                                row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
                                numberr++;
                                //////////////
                                var dataTable = new google.visualization.DataTable();
                                dataTable.addColumn('number');
                                dataTable.addColumn('number');
                                dataTable.addRows([
                               [parseInt($(this).find("pass").text()), parseInt($(this).find("fail").text())]

                                ]);

                                p = p + 1;
                                var options = {
                                    'width': 200,
                                    'height': 20,
                                    'showCategoryLabels': false,
                                    'showValueLabels': false,
                                    legend: { position: "none" },


                                };
                                var chart = new google.visualization.ImageBarChart(document.getElementById('chart_div' + p));
                                chart.draw(dataTable, options);
                                ///////////////////
                            });
                            //$("#ContentPlaceHolder1_gridviewdetails").val('');
                            //$("#ContentPlaceHolder1_gridviewdetails").val($("#ContentPlaceHolder1_GridViewcomplains").html());
                            htmlconverttoimage('ContentPlaceHolder1_GridViewcomplains');
                        }
                        $("#loadimg").hide();
                    }
            });

            ////////////////////////
        }



        //function ajaxinfractiongraph(qshortname) {

        //    $.ajax({
        //        type: 'POST',
        //        dataType: 'json',
        //        contentType: 'application/json',
        //        url: 'Create_scorecards_Pichart.aspx/Getgridoninfractgraphclick',
        //        data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",qshortname:"' + qshortname + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
        //        success:
        //            function (response) {
        //                //drawbarChart2(response.d)
        //                //////////////

        //                var xmlDoc = $.parseXML(response.d);
        //                var xml = $(xmlDoc);
        //                var customers = xml.find("Table");
        //                var row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
        //                //alert(row);
        //                $("#ContentPlaceHolder1_GridViewcomplains tr").not($("#ContentPlaceHolder1_GridViewcomplains tr:first-child")).remove();
        //                var numberr = 1;
        //                var p = 0;
        //                $.each(customers, function () {
        //                    var customer = $(this);
        //                    $("td", row).eq(0).html($(this).find("agent").text());
        //                    $("td", row).eq(1).html($(this).find("week_ending_date").text());
        //                    $("td", row).eq(2).html($(this).find("complaint").text());
        //                    $("td", row).eq(3).html($(this).find("noncomplaint").text());
        //                    $("td", row).eq(4).html('').append('<div id="chart_div' + numberr + '" style="height:25px; width:200px;overflow:hidden;"></div>');

        //                    $("#ContentPlaceHolder1_GridViewcomplains").append(row);
        //                    row = $("#ContentPlaceHolder1_GridViewcomplains tr:last-child").clone(true);
        //                    numberr++;
        //                    //////////////
        //                    var dataTable = new google.visualization.DataTable();
        //                    dataTable.addColumn('number');
        //                    dataTable.addColumn('number');
        //                    dataTable.addRows([
        //                   [parseInt($(this).find("complaint").text()), parseInt($(this).find("noncomplaint").text())]

        //                    ]);

        //                    p = p + 1;
        //                    var options = {
        //                        'width': 200,
        //                        'height': 20,
        //                        'showCategoryLabels': false,
        //                        'showValueLabels': false,
        //                        legend: { position: "none" },


        //                    };
        //                    var chart = new google.visualization.ImageBarChart(document.getElementById('chart_div' + p));
        //                    chart.draw(dataTable, options);
        //                    ///////////////////
        //                });

        //                ////////////
        //            }
        //    });

        //}


        function graphfour() {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/Getgraphfour',
                data: '{dropval:"' + $("#ContentPlaceHolder1_scrrcrdid").val() + '",mnth:"03",yrval:"2016",scorecarddropdown:"All"}',
                success:
                    function (response) {
                        drawgraphfour(response.d);
                        // detailsdrawChart();
                        //alert('graphfour success');
                    }
            });
        }
        //4 th pie chart
        function drawgraphfour(dataValues) {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Column Name');
            data.addColumn('number', 'Column Value');
            $("#visualization41").html('');
            var html = '<table style="width:100%;text-align: left;padding: 39px;"><tbody>';
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                html +='<tr><td style="width:90%">' + [dataValues[i].ColumnName] + '</td><td style="width:10%">' + '<b>' + [dataValues[i].Value] + '</b></td></tr>';
            }
            html += '</body></table>';
            $("#visualization41").append(html);
            var options = {
                'title': 'PAGE COMPLIANCE SNAPSHOT: SCORECARD DETAIL',
                'width': 700,
                'height': 500,
                is3D: true,
                sliceVisibilityThreshold: 0.0,
            };

            var visualizationas = document.getElementById('visualization4');
            var chart = new google.visualization.PieChart(document.getElementById('visualization4'));
            //////////
            google.visualization.events.addListener(chart, 'ready', function () {

                $('#ContentPlaceHolder1_Hiddengraphimage4').val(chart.getImageURI());
            });
            chart.draw(data, options);
            ///////////////
            //alert('graphfour draw success');
            ////////////////
            google.visualization.events.addListener(chart, 'select', function () {
                var selection = chart.getSelection();
                var message = '';

                for (var i = 0; i < selection.length; i++) {
                    var item = selection[i];
                    if (item.row != null && item.column != null) {
                        message += '{row:' + item.row + ',column:' + item.column + '}';
                    } else if (item.row != null) {
                        message += '{row:' + item.row + '}';
                    } else if (item.column != null) {
                        message += '{column:' + item.column + '}';
                    }
                }
                if (message == '') {
                    message = 'nothing';
                }

                var qshortname = data.getValue(chart.getSelection()[0].row, 0);
                //selectHandler(qshortname);
                //$('#ContentPlaceHolder1_scorecarddropdown option').filter(function () { return $(this).html() == qshortname; }).attr('selected', 'selected');
                //ajaxdropbindgraph();

                //dropbindgrid();


            });
            if ($("#visualization41 table tr").length > 0) {
                $("#ContentPlaceHolder1_Hiddengraphimage4details").val($("#visualization41").html());
            }
            else {
                $("#ContentPlaceHolder1_Hiddengraphimage4details").val($("#visualization41").html('<table style="width:100%;text-align: left;padding: 39px;"><tbody><tr><td style="width:90%"></td><td style="width:10%"></td></tr></tbody></table>'));
            }

           
        }

        //$('#ContentPlaceHolder1_Button1').click(function () {
        //    ///////////////
        //    html2canvas($('#ContentPlaceHolder1_GridViewcomplains').not('tbody tr td:nth-child(2)'), {
        //        proxy: "server.js",
        //        useCORS: true,
        //        onrendered: function (canvas) {
        //            var canvasImg = canvas.toDataURL("image/jpg");
        //           //var xml = (new XMLSerializer()).serializeToString(this);

        //            $('#canvasImg').html('<img src="' + canvasImg + '" alt="">');
        //        }
                
        //    });
        //    ////////////////
        //});
       
        function htmlconverttoimage(grid)
        {
           // alert('htmlconverttoimage' + grid);
            var canvasImg = "";
            if (grid == "ContentPlaceHolder1_GridViewcomplains")
            {
                html2canvas($('#ContentPlaceHolder1_GridViewcomplains'), {
                    proxy: "server.js",
                    useCORS: true,
                    onrendered: function (canvas) {
                        canvasImg = canvas.toDataURL("image/jpg");
                        //var xml = (new XMLSerializer()).serializeToString(this);
                        $('#canvasImg').html('');
                        $('#canvasImg').html('<img src="' + canvasImg + '" alt="">');
                       // alert('ContentPlaceHolder1_GridViewcomplains');
                        $("#ContentPlaceHolder1_gridviewdetails").val('');
                        $("#ContentPlaceHolder1_gridviewdetails").val(canvasImg);
                    }

                });
                $("#ContentPlaceHolder1_Button1").show();
            }
            else if (grid == "ContentPlaceHolder1_GridView2complains")
            {
                html2canvas($('#ContentPlaceHolder1_GridView2complains'), {
                    proxy: "server.js",
                    useCORS: true,
                    onrendered: function (canvas) {
                        canvasImg = canvas.toDataURL("image/jpg");
                        //var xml = (new XMLSerializer()).serializeToString(this);
                        $('#canvasImg').html('');
                        $('#canvasImg').html('<img src="' + canvasImg + '" alt="">');
                        //alert('ContentPlaceHolder1_GridView2complains');
                        $('#ContentPlaceHolder1_gridviewdetails').val('');
                        $('#ContentPlaceHolder1_gridviewdetails').val(canvasImg);
                    }

                });
                $("#ContentPlaceHolder1_Button1").show();
            }
           
           
           
            //$("#ContentPlaceHolder1_gridviewdetails").val($('#canvasImg').find('img').attr("src"));
        }


        function bindagentdropdown() {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/bindagentdropdown',
                data: '{mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '",scorecarddropdown:"' + $('#ContentPlaceHolder1_scorecarddropdown').val() + '"}',
                success:
                    function (Result) {
                        Result = Result.d;
                        $("#ContentPlaceHolder1_scrrcrddrop").html('');
                        $("#ContentPlaceHolder1_scrrcrddrop").append($("<option value='All' selected='true'>All</option>"));
                        $.each(Result, function (key, value) {                          
                            
                            $('#ContentPlaceHolder1_scrrcrddrop').append('<option value="' + value + '">' + value + '</option>');
                            $('#ContentPlaceHolder1_scrrcrdid').val('All');
                        });
                        ajaxdropbindgraph();
                        }
            });
        }


        function bindscorecarddropdown() {
            $("#loadimg").show();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Create_scorecards_Pichart.aspx/bindscorecarddropdown',
                data: '{mnth:"' + $("#ContentPlaceHolder1_DropDownList2").val() + '",yrval:"' + $("#ContentPlaceHolder1_DropDownList3").val() + '"}',
                success:
                    function (Result) {
                        Result = Result.d;
                      
                        $("#ContentPlaceHolder1_scorecarddropdown").html('');
                        $("#ContentPlaceHolder1_scorecarddropdown").append($("<option value='All' selected='true'>All</option>"));
                        $.each(Result, function (key, value) {
                            
                           
                            $('#ContentPlaceHolder1_scorecarddropdown').append('<option value="' + value.Value + '">' + value.ColumnName + '</option>');
                            //$('#ContentPlaceHolder1_scorecarddropdown').val('All');
                        });
                        $("#loadimg").hide();
                        ajaxdropbindgraph();
                    }
            });
        }


        //if (document.readyState === 'complete') {
         
        //}
        //var everythingLoaded = setInterval(function () {
        //    if (/loaded|complete/.test(document.readyState)) {
        //        clearInterval(everythingLoaded);
        //        $("#ContentPlaceHolder1_Button1").show(); //this is the function that gets called when everything is loaded
        //    }
        //}, 10);
    </script>
</asp:Content>

