<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="dms_ni_data.aspx.vb" Inherits="graspy_ni_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="/barchart/src/js/horizBarChart.js"></script>


    <style type="text/css">
        .chart-horiz .chart {
            width: 90%;
        }



            .chart-horiz .chart li {
                display: block;
                height: 23px;
                margin-top: 3px;
                position: relative;
                float: none;
            }

                .chart-horiz .chart li .bar:nth-child(odd) {
                    background: #3d90c8;
                }

                .chart-horiz .chart li .bar:nth-child(even) {
                    background: #41637c;
                }

                .chart-horiz .chart li:before {
                    color: #fff;
                    content: attr(title);
                    left: 5px;
                    position: absolute;
                }

                .chart-horiz .chart li.title:before {
                    color: black;
                    /*font-weight: bold;*/
                    font-size: smaller;
                    left: 0;
                }

                .chart-horiz .chart li:first-child {
                    margin-top: 0;
                }

                .chart-horiz .chart li .bar {
                    /*background: #27ae60;*/
                    height: 100%;
                    /*min-width: 164px;*/
                }

                .chart-horiz .chart li .number {
                    /*color: #27ae60;*/
                    font-size: 18px;
                    font-weight: bold;
                    padding-left: 5px;
                    position: absolute;
                    top: -3px;
                }

        /*.chart-horiz .chart li.past .bar {
                background: #2ecc71;
            }

            .chart-horiz .chart li.past .number {
                color: #2ecc71;
            }*/
    </style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        

        From: 
        <asp:TextBox ID="txtFrom" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>
        To:
        <asp:TextBox ID="txtTo" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" />
        <br />
        <br />

        <table>
            <tr>
                <td style="width: 20%; vertical-align: top">
                    <h2>DMS Agent Performance</h2>
                    <asp:GridView CssClass="detailsTable" ID="gvAgents" AllowSorting="true" DataSourceID="dsAgents" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="agent, date_start, date_end" DataNavigateUrlFormatString="ch2.aspx?agent={0}&scorecard=213&start={1}&end={2}" Target="_blank" DataTextField="agent" SortExpression="Agent" HeaderText="Agent" />
                            <asp:BoundField DataField="CSA1 Counts" HeaderText="CSA1 Counts" SortExpression="CSA1 Counts" />
                            <asp:BoundField DataField="Percent Obj1 Overcome" HeaderText="Obj1 % Att Overcome" SortExpression="Percent Obj1 Overcome" DataFormatString="{0:P}" />
                            <asp:BoundField DataField="CSA2 Counts" HeaderText="CSA2 Counts" SortExpression="CSA2 Counts" />
                            <asp:BoundField DataField="Percent Obj2 Overcome" HeaderText="Obj2 % Att Overcome" SortExpression="Percent Obj2 Overcome" DataFormatString="{0:P}" />
                            <asp:BoundField DataField="Avg Overcome" HeaderText="Avg Overcome" SortExpression="Avg Overcome" DataFormatString="{0:P}" />
                        </Columns>
                    </asp:GridView>

                    <asp:SqlDataSource ID="dsAgents" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select @start as date_start, @end as date_end, a.Agent, convert(varchar(100), csa1_yes) + '/' +   convert(varchar(100), csa1_total) as [CSA1 Counts], 
            convert(varchar(100), csa2_yes) + '/' +   convert(varchar(100), csa2_total) as [CSA2 Counts],
            1.0 * csa1_yes/csa1_total as [Percent Obj1 Overcome], 1.0 * csa2_yes/csa2_total as [Percent Obj2 Overcome],  1.0 * (csa2_yes + csa1_yes)/(csa1_total + csa2_total)  as [Avg Overcome] from (
            select count(*) as csa1_total,agent, sum(case when question_answered = 10908 then 1 else 0 end) as csa1_yes from vwForm join form_q_scores on form_id = f_id where question_id in (4475)
            and f_id in (select form_id from form_q_scores where question_answered in (10906)) and call_date between @start and @end
                group by agent) a
                join 
                (select count(*) as csa2_total,agent, sum(case when question_answered = 10915 then 1 else 0 end) as csa2_yes from vwForm join form_q_scores on form_id = f_id where question_id in (4478)
            and f_id in (select form_id from form_q_scores where question_answered in (10913)) and call_date between @start and @end
                group by agent) b
                on a.agent = b.agent"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtFrom" Name="start" />
                            <asp:ControlParameter ControlID="txtTo" Name="end" />
                        </SelectParameters>

                    </asp:SqlDataSource>
                </td>
                <td>&nbsp;</td>
                <td style="width: 80%; vertical-align: top">
                    <h2>Object Issue</h2>
                    <div class="moduleContent">
                        <div class="chart-horiz">
                            <ul class="chart">
                            </ul>
                        </div>
                    </div>
                </td>
            </tr>
        </table>



        <br />

    </section>



    <script>
        $(document).ready(function () {


            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "/CDService.svc/GetSCStats", // Location of the service
                data: '{"start_date":"' + $('#ContentPlaceHolder1_txtFrom').val() + '","end_date":"' + $('#ContentPlaceHolder1_txtTo').val() + '","scorecard":"213"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call
                    var jsonObj = eval(msg.d);
                    $('.chart li').remove();
                    $(jsonObj).each(function (index, element) {
                        $('.chart').append('<li title="' + element.text + '" class="title"></li>');
                        $('.chart').append('<li title=""><span class="bar"  data-number="' + element.value + '"></span><span class="number">' + element.value + '</span></li>');
                    });

                    $('.chart').horizBarChart({
                        selector: '.bar',
                        speed: 3000
                    });


                },
                error: ServiceFailed// When Service call fails
            });


            function range() {
                $('#ContentPlaceHolder1_txtFrom').prop('disabled', false);
                //$('[id*=ddlWEDate]').prop('disabled', true);
                if ($('#ContentPlaceHolder1_txtFrom').val().length == 10)
                    $('#ContentPlaceHolder1_txtTo').prop('disabled', false);
                else
                    $('#ContentPlaceHolder1_txtTo').prop('disabled', true);
            }

            function wedate() {
                $('#ContentPlaceHolder1_txtFrom').prop('disabled', true);
                $('#ContentPlaceHolder1_txtTo').prop('disabled', true);
                // $('[id*=ddlWEDate]').prop('disabled', false);
            }

            if ($('[id*=filter]').val() == 'wedate') {
                $('ContentPlaceHolder1_rbWE').prop('checked', true);
                wedate();
            }
            else {
                $('ContentPlaceHolder1_rbRange').prop('checked', true);
                range();
            }

            $('ContentPlaceHolder1_rbRange').click(function () {
                range();
            });
            $('ContentPlaceHolder1_rbWE').click(function () {
                wedate();
            });
            $('#apply').click(function () {
                if ($('ContentPlaceHolder1_rbRange').is(':checked'))
                    window.location = window.location.pathname + '?from=' + $('#ContentPlaceHolder1_txtFrom').val() + '&to=' + $('#ContentPlaceHolder1_txtTo').val();
                else
                    window.location = window.location.pathname + '?wedate=' + $('#ContentPlaceHolder1_ddlWEDate').find(":selected").text();
            });
            $('#ContentPlaceHolder1_txtFrom').val('<% =txtFrom.Text %>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            $('#ContentPlaceHolder1_txtTo').val('<% =txtTo.Text%>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            check();
            $('#ContentPlaceHolder1_txtFrom').keyup(function () {
                check();
            }).change(function () {
                check();
            });
            function check() {
                if ($('#ContentPlaceHolder1_txtFrom').val().length == 10)
                    $('#ContentPlaceHolder1_txtTo').removeAttr('disabled');
                else {
                    $('#ContentPlaceHolder1_txtTo').val('');
                    $('#ContentPlaceHolder1_txtTo').attr('disabled', 'disabled');
                }
            }
        });

    </script>

</asp:Content>

