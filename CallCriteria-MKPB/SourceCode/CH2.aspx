<%@ Page Title="History" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="CH2.aspx.vb" Inherits="CH2" %>


<%@ Register Src="~/controls/TodayStats.ascx" TagPrefix="UC1" TagName="TodayStats" %>
<%@ Register Src="~/controls/TopMissed.ascx" TagPrefix="UC1" TagName="TopMissed" %>
<%@ Register Src="~/controls/AvgScoreModule.ascx" TagPrefix="UC1" TagName="AvgScore" %>
<%@ Register Src="~/controls/ScorePerformance.ascx" TagPrefix="UC1" TagName="ScorePerformance" %>
<%@ Register Src="~/controls/AgentRanking.ascx" TagPrefix="UC1" TagName="AgentRanking" %>
<%@ Register Src="~/controls/DashDetails.ascx" TagPrefix="UC1" TagName="DashDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!-- <script type="text/javascript" src="/scripts/jquery-migrate-1.2.1.min.js"></script> -->
    <script type="text/javascript" src="/scripts/jquery.transit.min.js"></script>
    <script type="text/javascript" src="/scripts/raphael-min.js"></script>
    <script type="text/javascript" src="/scripts/lyric/sylvester-0-1-3/sylvester.js"></script>
    <script type="text/javascript" src="/scripts/lyric/lyric.js"></script>

    <script type="text/javascript" src="/tablesorter/js/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="/tablesorter/js/jquery.tablesorter.widgets.js"></script>
    <script src="js/jquery.scrollintoview.min.js"></script>

    <script src="js/FilterManager.js"></script>

    <link href="tablesorter/css/theme.blue.css" rel="stylesheet" />

    <%--<script src="js/jquery.sortable.js"></script>--%>

    <link href="css/client_dashboard.css?1000" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hdnAppname" runat="server" />
    <asp:HiddenField ID="hdnScorecard" runat="server" />
    <asp:HiddenField ID="hdnPageRows" runat="server" />

    <asp:HiddenField ID="hdnTotalModules" runat="server" />
    <asp:HiddenField ID="hdnFailFilter" runat="server" />

    <asp:HiddenField ID="hdnMainLogo" runat="server" />
    <asp:HiddenField ID="hdnPrimayColor" runat="server" />

    <asp:HiddenField ID="hdnAgent" runat="server" />
    <asp:HiddenField ID="hdnQID" runat="server" />
    <asp:HiddenField ID="hdnOrigScorePerf" runat="server" />
    <asp:HiddenField ID="hdnHardFilters" runat="server" />


    <div id="tempHeader">
        <div></div>
    </div>


    <!-- Start page content -->
    <div id="dashboardPage">

        <table class="page-header" cellpadding="0px" cellspacing="0px">
            <tr>
                <%--<td class="pageName">
                    <img id="page_main_logo" /></td>
                <td align="center">
                    <asp:Literal ID="litClientName" Visible="false" runat="server"></asp:Literal><asp:Label ID="lblName" runat="server" Text="" Font-Size="Small"></asp:Label><br />
                    <a href="logout.aspx" style="font-size: small;">Logout</a>

                </td>--%>
                <td>&nbsp;</td>

                <td class="dateRange">
                    <asp:TextBox ID="date1" data-datetype="from" runat="server"></asp:TextBox>
                    &nbsp; &mdash; &nbsp;
                     <asp:TextBox ID="date2" data-datetype="to" runat="server"></asp:TextBox>
                </td>
                <td class="applyFiltersBtnTd">
                    <div class="filterBtn applyFiltersBtn" onclick="pageLoad(); updateFilters(); toggleOpenFilters('', 'close');">
                        <span>Apply</span>
                    </div>
                </td>
                <td class="filterBtnTd">
                    <div class="filterBtn filterBtn1" data-filternum="1"><i class="fa fa-filter"></i></div>
                </td>
                <td class="filterBtnTd <%=myRole%>">
                    <div class="filterBtn filterBtn2" data-filternum="2"><span>$</span></div>
                </td>
                <!-- just remove .hiddenFilterBtnTd and .hiddenFilterBtn to show/activate button and resize Apply button -->
                <td class="filterBtnTd">
                    <div class="filterBtn filterBtn3 addModule" data-filternum="3"><i class="fa fa-plus-circle"></i></div>
                </td>
                <%--<td id="trashModuleTd" class="filterBtnTd">
                    <div class="filterBtn filterBtn4 trashModule" data-filternum="4">
                        <img src="img/blue_trash.PNG" /></div>
                </td>--%>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="moduleTitle" style="text-align: right"><i class="fa fa-filter" id="filterIcon"></i>
                    <span id="AppliedFilters"></span></td>
                <td colspan="5"><a class="module-button resetDetails" id="globalReset" href="javascript:getDetailsData();" style="display: none;">Reset</a></td>
            </tr>
        </table>

        <asp:HiddenField ID="hdnAgentFilter" runat="server" />
        <asp:HiddenField ID="hdnFixedFilter" runat="server" Value=" and 1=1 " />

        <div class="filtersArrow1"></div>
        <div class="filtersArrow2"></div>
        <div class="filters filters1">
            <div class="filtersInner">
                <span class="filtersTitle">Filters</span>
                <div class="filtersContent">
                    <div class="filtersSet filtersSet1">
                        <span class="dropdown">
                            <select id="ddlGroup" onchange="addArrayFilter('group',  $('#ddlGroup').val(),$('#ddlGroup').val()); updateAgents(); updateCampaigns();">
                                <option value="">All Groups</option>
                            </select>
                        </span>
                        <span class="dropdown">
                            <select id="ddlAgent" onchange="addArrayFilter('agent', $('#ddlAgent').val(),$('#ddlAgent').val()); ">
                                <option value="">All Agents</option>
                            </select>
                        </span>
                        <span class="dropdown">
                            <select id="ddlCampaigns" onchange="addArrayFilter('campaign', $('#ddlCampaigns').val(),$('#ddlCampaigns').val()); ">
                                <option value="">All Campaigns</option>
                            </select>
                        </span>
                        <span class="dropdown">
                            <select id="ddlQA" onchange="addArrayFilter('reviewer', $('#ddlQA').val(),$('#ddlQA').val()); ">
                                <option value="">All QA</option>
                            </select>
                        </span>

                        <asp:LoginView runat="server">
                            <RoleGroups>
                                <asp:RoleGroup Roles="Admin,Supervisor,Client">
                                    <ContentTemplate>
                                        <span class="dropdown">
                                            <select id="ddlAppnames" onchange="addArrayFilter('scorecard', $('#ddlAppnames').val(), $('#ddlAppnames option:selected').text()); updateGroups(); updateAgents(); updateCampaigns(); $('#<%=hdnScorecard.Clientid %>').val($(this).val());">
                                            </select>
                                        </span>
                                    </ContentTemplate>

                                </asp:RoleGroup>
                            </RoleGroups>
                        </asp:LoginView>


                    </div>
                    <div class="filtersSet filtersSet2">
                        <span class="filterLabel">Comparison period:
                        </span>
                        <span class="compareDates dateRange">
                            <span id="compdate1"></span>
                            &nbsp; &mdash; &nbsp;
                            <span id="compdate2"></span>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="filters filters2">
            <div class="filtersInner">
                <span class="filtersTitle">Billing</span>
                <div class="filtersContent">
                    <div class="filtersSet filtersSet3">
                        <table style="text-align: left;">
                            <tr>
                                <td>Unbilled</td>
                                <td>
                                    <span class="billLabel">Time:</span></td>
                                <td>
                                    <span class="billData billedTime">
                                        <asp:Literal ID="litPendTime" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel">Amount:</span></td>
                                <td>
                                    <span class="billData billedAmount">
                                        <asp:Literal ID="litPendAmount" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel"># Calls:</span></td>
                                <td>
                                    <span class="billData numberCalls">
                                        <asp:Literal ID="litPendNumberCalls" runat="server"></asp:Literal></span>
                                </td>
                            </tr>

                            <asp:Repeater ID="rptBilling" DataSourceID="dsBilling" runat="server">

                                <HeaderTemplate>
                                    <tr>
                                        <td>Open Invoices:</td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style='color: <%#eval("font_color")%>'>
                                        <td>Inv #:<%#Eval("Invoice") %><div style="font-size: smaller">(<%#Eval("bill_from", "{0:M/d/yyyy}")%>- <%#Eval("bill_to", "{0:M/d/yyyy}")%>)</div>
                                        </td>
                                        <td>
                                            <span class="billLabel">Time:</span></td>
                                        <td>
                                            <span class="billData billedTime">
                                                <%#Eval("bill_seconds") %></span>
                                        </td>
                                        <td>
                                            <span class="billLabel">Amount:</span></td>
                                        <td>
                                            <span class="billData billedAmount">
                                                <%#Eval("amount", "{0:C}") %></span>
                                        </td>
                                        <td>
                                            <span class="billLabel"># Calls:</span></td>
                                        <td>
                                            <span class="billData numberCalls">
                                                <%#Eval("bill_calls") %></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>


                            </asp:Repeater>
                            <asp:SqlDataSource ID="dsBilling" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="select *, case when bill_date < DATEADD(d, -30, getdate()) then 'red' else 'black' end as font_color from billingdata join userapps on userapps.appname = billingdata.appname
                                    where userapps.username = @username and bill_paid_date is null order by bill_date">
                                <SelectParameters>
                                    <asp:Parameter Name="Username" />
                                </SelectParameters>

                            </asp:SqlDataSource>

                            <%-- <tr>
                                <td>Billed<br />
                                    <span class="compPerLabel">(thru
                                        <asp:Literal ID="litLastBilled" runat="server"></asp:Literal>)</span></td>
                                <td>
                                    <span class="billLabel">Time:</span></td>
                                <td>
                                    <span class="billData billedTime">
                                        <asp:Literal ID="litBilledTime" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel">Amount:</span></td>
                                <td>
                                    <span class="billData billedAmount">
                                        <asp:Literal ID="litBilledAmount" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel"># Calls:</span></td>
                                <td>
                                    <span class="billData numberCalls">
                                        <asp:Literal ID="litBilledCalls" runat="server"></asp:Literal></span>
                                </td>
                            </tr>
                            <tr id="overduerow" runat="server" style="color: red;">
                                <td>Overdue</td>
                                <td>
                                    <span class="billLabel">Time:</span></td>
                                <td>
                                    <span class="billData billedTime">
                                        <asp:Literal ID="litODTime" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel">Amount:</span></td>
                                <td>
                                    <span class="billData billedAmount">
                                        <asp:Literal ID="litODAmount" runat="server"></asp:Literal></span>
                                </td>
                                <td>
                                    <span class="billLabel"># Calls:</span></td>
                                <td>
                                    <span class="billData">
                                        <asp:Literal ID="litODnumber" runat="server"></asp:Literal></span>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="filters filters3">
            <div class="filtersInner">
                <span class="filtersTitle">Add/Remove Modules</span>
                <div class="filtersContent">
                    <div class="filtersSet filtersSet3">
                        <table>
                            <tr>
                                <asp:Repeater ID="dlModules" DataSourceID="dsModules" runat="server">
                                    <ItemTemplate>
                                        <td>

                                            <div class="filterBtn addModule" onclick="updateDashOrder<%#Eval("add_delete")%>('<%#Eval("moduleName")%>',0);window.location.href='cd2.aspx';" data-filternum="4">
                                                <asp:Label ID="lblModuleName" runat="server" Text='<%#Eval("moduleName")%>'></asp:Label>&nbsp;&nbsp;<i class="fa fa-<%#Eval("plus_minus")%>-circle"></i>
                                            </div>
                                        </td>
                                    </ItemTemplate>

                                </asp:Repeater>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="dsModules" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="getMyModules" SelectCommandType="StoredProcedure" runat="server">
                            <SelectParameters>
                                <asp:Parameter Name="username" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>



        <div class="dash-modules" id="bottomDashModules">

            <UC1:DashDetails ID="dd" runat="server"></UC1:DashDetails>
            <!-- End page content -->




            <script>





                function updateFilters(obj) {


                    //console.log('updateFilters');

                    var agent = $('#ddlAgent').val()
                    if (obj != undefined) {
                        agent = $(obj).val();
                    }

                    //console.log('agent = ' + agent)

                    $('#rowcount').html('-/-');

                    var filter_list = $('#ContentPlaceHolder1_hdnFixedFilter').val();
                    if ($('#ddlGroup').val() != '') {
                        filter_list += " and agent_group = '" + $('#ddlGroup').val() + "' ";

                    }
                    localStorage['group'] = $('#ddlGroup').val();

                    if ($('#ContentPlaceHolder1_hdnFailFilter').val() != '') {
                        filter_list += " and pass_fail = 'fail' ";
                        localStorage['fail'] = 'fail';
                    }
                    else
                        localStorage.removeItem('fail');

                    if (agent != '') {
                        filter_list += " and vwForm.agent = '" + agent + "' ";


                        var found_agent_el
                        //console.log($('#ddlAgent').val());
                        $('.dash-modules .agentRanking .aname a').each(function () {
                            //console.log($('#ddlAgent').val() == ($(this).text()));
                            if (agent == ($(this).text())) {
                                found_agent_el = $(this);
                            }
                        });
                        //console.log($('#ddlAgent').val() + ' after');

                        getAgentData(agent, found_agent_el);
                        found_agent_el.scrollintoview();
                    }

                    localStorage['agent'] = $('#ddlAgent').val();

                    //if (($('#ddlAppnames').val() != '') && ($('#ddlAppnames').val() != 'null') && ($('#ddlAppnames').val() != 'app'))
                    if ($('#ContentPlaceHolder1_hdnScorecard').val() != '') {
                        filter_list += " and vwForm.scorecard = '" + $('#ContentPlaceHolder1_hdnScorecard').val() + "' ";
                        localStorage['scorecard'] = $('#ContentPlaceHolder1_hdnScorecard').val();
                    }

                    localStorage['scorecard'] = $('#ContentPlaceHolder1_hdnScorecard').val();

                    if ($('#ddlCampaigns').val() != '') {
                        filter_list += " and Campaign = '" + $('#ddlCampaigns').val() + "' ";

                    }
                    localStorage['campaign'] = $('#ddlCampaigns').val();
                    /////////////////////
                    var QAgent = $('#ddlQA').val();
                    if (QAgent != '') {
                        filter_list += " and vwform.reviewer = '" + $('#ddlQA').val() + "' ";

                    }
                    localStorage['QA'] = $('#ddlQA').val();
                    /////////////////////////////

                    var qID = $('#ContentPlaceHolder1_hdnQID').val();
                    if (qID != '') {
                        filter_list += 'and f_id in (select form_id  from form_q_scores where question_id = ' + qID + ' and question_answered in (select id from question_answers where question_id = ' + qID + ' and right_answer = 0) )  ';
                        localStorage['question'] = qID;
                    }
                    localStorage['question'] = qID;

                    $('#ContentPlaceHolder1_hdnAgentFilter').val(filter_list);

                    //console.log('filter_list ' + filter_list);

                    $('#report-here tbody').empty();

                    pageIndex = 2;
                    getDetails(0);

                    //refresh_modules();
                }




                function pageLoad() {

                    //console.log('pageLoad');


                    //var ajax_active = jQuery.active;

                    //$.xhrPool.abortAll();

                    // var ajax_after = jQuery.active;


                    //alert(ajax_active + ' ' + ajax_after);

                    setupCategoryPopups();

                    $(".tablesorter-filter").val("x");
                    $(".tablesorter-filter").val("");

                    //updateApp()

                    //getWorstDev();
                    //console.log(localStorage);
                }

                function updateApp() {

                    // console.log('updateApp');



                    updateGroups();

                    updateQualityA();
                    updateAgents();
                    updateCampaigns();
                    getAppnames();


                    $(".single").sortable({
                        handle: ".moduleTitle"
                    });

                    $(".double").sortable({
                        handle: ".moduleTitle"
                    });

                    $(".triple").sortable({
                        handle: ".moduleTitle"
                    });





                }



                var pageIndex = 2;
                var pageCount;



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


                //inital page stuff
                updateGroups();
                //updateAgents();
                updateCampaigns();
                //getDetails(0);
                updateQA();

                pageLoad();

            </script>
        </div>


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





        <script>
            //$(window).load(function () {
            //    // executes when complete page is fully loaded, including all frames, objects and images
            //    console.log('Window loaded');
            //    refresh_modules();
            //});


            var have_apps = 0;
            var have_agents = 0;
            var have_groups = 0;
            var have_campaigns = 0;


            var explode = setInterval(function () {
                if (have_apps && have_agents && have_groups && have_campaigns) {
                    clearInterval(explode);
                    //console.log('all ddls loaded');
                    updateFilters();

                }

            }, 1000);

            $(document).submit(function () {
                $.xhrPool.abortAll();
            });


            updateApp();

        </script>
</asp:Content>


