<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="dms_billable.aspx.vb" Inherits="dms_billable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>DMS Non-Billable</h2>

        <table style="padding: 10px; margin: 10px;">
            <tr>
                <td>Select scorecard:</td>
                <td>
                    <asp:DropDownList ID="ddlScorecard" DataSourceID="dsSC"
                        DataTextField="scorecard_name" AppendDataBoundItems="true" AutoPostBack="true"
                        DataValueField="ID" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsSC" SelectCommand="select short_name as scorecard_name, ID, appname 
                            from scorecards  where appname in (select appname from userapps where username = @username) 
                            and active =1  order by appname, short_name"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>
            </tr>

            <tr>
                <td>Start Date:</td>
                <td>
                    <asp:TextBox ID="date1" runat="server" /></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <asp:TextBox ID="date2" runat="server" /></td>
            </tr>

            <tr>

                <td colspan="2">
                    <asp:Button ID="btnGO" runat="server" Text="Go" /></td>
            </tr>

        </table>



        <table>
            <tr>
                <td rowspan="10" valign="top" style="width: 275px;">

                    <asp:HiddenField ID="hdnAgentFilter" runat="server" />
                    <asp:HiddenField ID="hdnFixedFilter" runat="server" />

                    <asp:GridView ID="gvComplaince" Caption="Non-billable Points" runat="server" AutoGenerateColumns="False" CssClass="detailsTable freezable" Width="250" DataKeyNames="id" DataSourceID="dsQs">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:BoundField DataField="q_short_name" HeaderText="Question" SortExpression="q_short_name" ReadOnly="true" />
                            <asp:BoundField DataField="q_order" HeaderText="Point #" SortExpression="q_order" ReadOnly="true" />
                            <asp:CheckBoxField DataField="compliance" HeaderText="Non-billable" SortExpression="compliance" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsQs" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                        DeleteCommand="DELETE FROM [Questions] WHERE [id] = @id"
                        InsertCommand="INSERT INTO [Questions] ([q_short_name], [compliance]) VALUES (@q_short_name, @compliance)"
                        SelectCommand="SELECT [q_short_name], [compliance], [id], q_order FROM [Questions] where scorecard_id=@scorecard and active = 1 order by q_order "
                        UpdateCommand="UPDATE [Questions] SET [compliance] = @compliance WHERE [id] = @id">
                        <DeleteParameters>
                            <asp:Parameter Name="id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="q_short_name" Type="String" />
                            <asp:Parameter Name="compliance" Type="Boolean" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="q_short_name" Type="String" />
                            <asp:Parameter Name="compliance" Type="Boolean" />
                            <asp:Parameter Name="id" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>

                <td class="caption">Compliance Report Card - Previous Month</td>
                <td class="caption">Compliance Report Card - MTD</td>
            </tr>
            <tr>

                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvPMComp" DataSourceID="dsPMComp" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsPMComp" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance @scorecard, 0, 1, @team_lead, @agent_type,'',''">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>

                    </asp:SqlDataSource>
                </td>
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvMTDComp" DataSourceID="dsMTDComp" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsMTDComp" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance @scorecard, 1, 1, @team_lead, @agent_type, @date1, @date2">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td class="caption">Non Compliance Points Report Card - Previous Month</td>
                <td class="caption">Non Compliance Points Report Card - MTD</td>
            </tr>
            <tr>
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvPMAll" DataSourceID="dsPMAll" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsPMAll" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance @scorecard, 0, 0, @team_lead, @agent_type,'',''">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvMTDAll" DataSourceID="dsMTDAll" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsMTDAll" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance @scorecard, 1, 0, @team_lead, @agent_type, @date1, @date2">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>

            <tr class="<%=myRole %>">
                <td class="caption">School - Previous Month</td>
                <td class="caption">School- MTD</td>
            </tr>
            <tr class="<%=myRole %>">
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" AllowSorting="true" ID="gvSChoolPM" DataSourceID="dsSchoolPM" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsSchoolPM" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance2 @scorecard, 0, 0, 'school', 'school','',''">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" AllowSorting="true" ID="gvSchoolMTD" DataSourceID="dsSchoolMTD" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsSchoolMTD" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance2 @scorecard, 1, 0, 'school', 'school', @date1, @date2">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>

            <tr class="<%=myRole %>">
                <td class="caption">Disposition - Previous Month</td>
                <td class="caption">Disposition - MTD</td>
            </tr>
            <tr class="<%=myRole %>">
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvFCPM" DataSourceID="dsFCPM" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsFCPM" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance2 @scorecard, 0, 0, 'disposition', 'disposition','',''">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td valign="top" style="max-width: 780px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvFCMTD" DataSourceID="dsFCMTD" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsFCMTD" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec getQSCompliance2 @scorecard, 1, 0, 'disposition', 'disposition', @date1, @date2">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent_type" DefaultValue="Name" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>

            <tr>
                <td colspan="2" class="caption">Date Details</td>
            </tr>
            <tr>

                <td valign="top" colspan="2" style="max-width: 1600px; overflow: hidden;">
                    <asp:GridView CssClass="detailsTable freezable" Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvDateRange" DataSourceID="dsDateRange" runat="server">
                        <EmptyDataTemplate>No data found</EmptyDataTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsDateRange" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server" SelectCommand="exec [getQSComplianceDate] @scorecard, 1, @team_lead, @agent,  @date1, @date2">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                            <asp:Parameter Name="team_lead" DefaultValue="All" />
                            <asp:Parameter Name="agent" DefaultValue="" />
                            <asp:ControlParameter ControlID="date1" Name="date1" />
                            <asp:ControlParameter ControlID="date2" Name="date2" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>

        </td>



            </tr>
        </table>


            <script>

                var pageIndex = 2;




                function getTopMissed(agentName) {

                    $('.topMissedPoints .loading').show();

                    // redo filters without agent

                    var filter_list = $('#ContentPlaceHolder1_hdnAgentFilter').val() + '';

                    if ($('#ContentPlaceHolder1_ddlScorecard').val() != '')
                        if ($('#ContentPlaceHolder1_ddlScorecard').val() != null) {
                            filter_list += " and vwForm.scorecard = '" + $('#ContentPlaceHolder1_ddlScorecard').val() + "' "
                        }


                    if ($('#ContentPlaceHolder1_ddlTL').val() != '' && $('#ContentPlaceHolder1_ddlTL').val() != 'ALL' && !(typeof ($('#ContentPlaceHolder1_ddlTL').val()) === 'undefined'))
                        filter_list += " and agent_group = '" + $('#ContentPlaceHolder1_ddlTL').val() + "' "
                    if ($('#ContentPlaceHolder1_ddlAgent').val() != '' && $('#ContentPlaceHolder1_ddlAgent').val() != 'ALL' && !(typeof ($('#ContentPlaceHolder1_ddlAgent').val()) === 'undefined'))
                        filter_list += " and vwForm.agent = '" + $('#ContentPlaceHolder1_ddlAgent').val() + "' "

                    if (typeof (filter_list) === 'undefined')
                        filter_list = ' and 1=1 ';


                    if (filter_list == "undefined")
                        filter_list = "";

                    $.ajax({
                        type: "POST", //GET or POST or PUT or DELETE verb
                        url: "/CDService.svc/GetTopMissed", // Location of the service
                        data: '{"start_date":"' + $('#ContentPlaceHolder1_date1').val() + '","end_date":"' + $('#ContentPlaceHolder1_date2').val() + '","hdnAgentFilter":"' + filter_list + '"}', //Data sent to server
                        contentType: "application/json; charset=utf-8", // content type sent to server
                        dataType: "json", //Expected data format from server
                        processdata: true, //True or False
                        success: function (msg) {//On Successfull service call
                            //alert(msg.d);
                            var jsonObj = eval(msg.d);
                            updateTopMissed(jsonObj);
                            $('.topMissedPoints .loading').hide();


                            $('.topMissedPoints .qscore').hover(function () {

                                $('.topMissedPoints > .datapoint span').text($(this).attr('data-callscount'));
                                $('.topMissedPoints > .datapoint').addClass('show');
                            }, function () {
                                $('.topMissedPoints > .datapoint').removeClass('show');
                            });

                            $('.topMissedPoints .qname').hover(function () {
                                var datapoint = $('.topMissedPoints .datapointbig');
                                $('.topMissedPoints .datapointbig tr').remove();
                                var datapointString = $(this).parent('tr').attr('data-datapoint');
                                var datapointString2 = datapointString.split(',');
                                $.each(datapointString2, function (index, value) {
                                    if ((value != "undefined") && (value != "")) {
                                        var datapointString3 = value.split(':');
                                        var datapointString4 = datapointString3[1].split('/');

                                        var datapointName = datapointString3[0];
                                        var datapointNum1 = Number(datapointString4[0]);
                                        var datapointNum2 = Number(datapointString4[1]);
                                        var datapointPercent = Math.floor((datapointNum1 / datapointNum2) * 100);
                                        var appendString = '<tr>';
                                        appendString += '<td class="data-name">' + datapointName + '</td>';
                                        appendString += '<td class="data-percent">' + datapointPercent + '%</td>';
                                        appendString += '<td class="data-numbers">(' + datapointNum1 + ' of ' + datapointNum2 + ' calls)</td>';
                                        appendString += '</tr>';
                                    }


                                    $('.topMissedPoints .datapointbig tbody').append(appendString);
                                    datapoint.css({ 'margin-top': (-datapoint.height() - 15) + 'px' });
                                });
                                $('.topMissedPoints > .datapointbig').addClass('show');
                            }, function () {
                                $('.topMissedPoints > .datapointbig').removeClass('show');
                            });

                            $('.expand-notif').each(function () {
                                var module_content = $(this).parent().parent().find('.moduleContent');
                                if (module_content.get(0).scrollHeight - 1 > module_content.height())
                                    $(this).show()
                                else
                                    $(this).hide()
                            });

                        },
                        error: ServiceFailed// When Service call fails
                    });
                }





                function getDetailsQS(page) {


                    var rc = $('#rowcount').html().split('/');

                    if (rc[0] == '-') rc[0] = '0';



                    $('.detailsTable .loading').show();
                    //console.log('getDetails page = ' + page);

                    $.ajax({
                        type: "POST", //GET or POST or PUT or DELETE verb
                        url: "/CDService.svc/GetDetails", // Location of the service
                        data: '{"start_date":"' + $('#ContentPlaceHolder1_date1').val() + '","end_date":"' + $('#ContentPlaceHolder1_date2').val() + '","hdnAgentFilter":"' + $('#ContentPlaceHolder1_hdnAgentFilter').val() + '","pagenum":"' + page + '","pagerows":"' + $('#hdnPageRows').val() + '","rowstart":"' + rc[0] + '","rowend":"' + (parseInt($('#hdnPageRows').val()) + parseInt(rc[0])) + '"}', //Data sent to server
                        contentType: "application/json; charset=utf-8", // content type sent to server
                        dataType: "json", //Expected data format from server
                        processdata: true, //True or False
                        //async: true,
                        success: function (msg) {//On Successfull service call
                            //alert(msg.d);
                            //var jsonObj = eval(msg.d);

                            //console.log('getDetails page = ' + page + ' msg.d =' + msg.d);

                            if (page > 0) {


                                $('#report-here tbody').append(msg.d);

                                $('.noti-click').hide();

                                setUpTableSort();

                                $('#report-here thead tr td:nth-child(1)').hide();
                                $('#report-here thead tr td:nth-child(9)').hide();
                                if ($('#ContentPlaceHolder1_hdnRole').val() == 'Supervisor') {
                                    $('#report-here thead tr td:nth-child(2)').hide();
                                    //$('#report-here thead tr td:nth-child(3)').hide();
                                }

                                $('#report-here tbody tr td:nth-child(1)').hide();
                                $('#report-here tbody tr td:nth-child(9)').hide();
                                if ($('#ContentPlaceHolder1_hdnRole').val() == 'Supervisor') {
                                    $('#report-here tbody tr td:nth-child(2)').hide();
                                    // $('#report-here tbody tr td:nth-child(3)').hide();
                                }


                            }


                            $('.detailsTable .loading').hide();

                            if (page == 0) {
                                //$('#report-here thead').empty();
                                $('#report-here tbody').empty();
                                if ($('#report-here thead').is(':empty')) {


                                    $('#report-here thead').append(msg.d);

                                    //$('#report-here thead td').each(function () {
                                    //    var regex = /(<([^>]+)>)/ig
                                    //    var result = this.innerHTML.replace(regex, "");
                                    //    $(this).html(result);
                                    //});


                                    //setUpTableSort();




                                    //setUpTableSort();
                                }


                            }




                            if (page == 0) {
                                getDetailsQS(1);
                            }
                            else {
                                $.ajax({
                                    type: "POST", //GET or POST or PUT or DELETE verb
                                    url: "/CDService.svc/GetDetailCount", // Location of the service
                                    data: '{"start_date":"' + $('#ContentPlaceHolder1_date1').val() + '","end_date":"' + $('#ContentPlaceHolder1_date2').val() + '","hdnAgentFilter":"' + $('#ContentPlaceHolder1_hdnAgentFilter').val() + '"}', //Data sent to server
                                    contentType: "application/json; charset=utf-8", // content type sent to server
                                    dataType: "json", //Expected data format from server
                                    processdata: true, //True or False
                                    success: function (msg) {//On Successfull service call
                                        var rowCount = $('#report-here tbody tr').length;
                                        $('#rowcount').html(rowCount + '/' + msg.d + ' rows');



                                    },
                                    error: ServiceFailed// When Service call fails
                                });
                            }

                            setDataTable()
                        },
                        error: ServiceFailed// When Service call fails
                    });
                }



                function jumpPos(to_time) {

                    try {
                        audio = $('.audio-player audio').get(0);
                        audio.currentTime = to_time;
                        audio.play();
                    }
                    catch (err) {

                    }


                }

                function change(sourceUrl) {
                    var audio = $("#notification_player");
                    $("#notification_player").attr("src", sourceUrl);
                    /****************/
                    audio[0].pause();
                    audio[0].load();//suspends and restores all audio element
                    audio[0].play();
                    /****************/
                }



                function stopPlayer() {
                    $('#notification_holder').hide();
                    var audio = $("#notification_player");
                    $("#notification_player").attr("src", "");
                    /****************/
                    audio[0].pause();

                }



                var player_loaded = false;


                function show_audio(file_name, position, form_id) {

                    var new_filename;
                    $.ajax({
                        type: 'POST',
                        url: "convert_audio.aspx",
                        data: { ID: form_id },
                        success: function (result) {
                            new_filename = result;

                            $('#notification_holder').show();
                            document.getElementById('ContentPlaceHolder1_hdnOpenFormID').value = form_id;


                            if (!player_loaded) {
                                setupAudioPlayer(new_filename, 0.75, true);
                                jumpPos(position);
                                player_loaded = true;
                            }
                            else {
                                if ($("#notification_player").attr("src") != new_filename) {
                                    change(new_filename);
                                }
                                jumpPos(position);
                            }

                            popIndicators();

                        },
                        cache: false,
                        timeout: 5000
                        //, async: false
                    });



                }



                $(document).ready(function () {


                    $('#ContentPlaceHolder1_date1').datepicker({
                        dateFormat: "mm/dd/yy"
                    });
                    $('#ContentPlaceHolder1_date2').datepicker({
                        dateFormat: "mm/dd/yy"
                    });

                    $('.exportReport').hide();
                    $('.printReport').hide();
                    $('.resetDetails').hide();


                    $('.freezable').each(function () {
                        $(this).tableHeadFixer({ 'left': 2 });

                    })


                    //getTopMissed();

                    //getDetailsQS(0);


                });



            </script>


    </section>
</asp:Content>

