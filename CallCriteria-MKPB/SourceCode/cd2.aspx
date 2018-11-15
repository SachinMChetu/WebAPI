<%@ Page Title="Dashboard" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="CD2.aspx.vb" Inherits="CD2" %>

<%@ Register Src="~/controls/CoachingLog.ascx" TagPrefix="UC1" TagName="CoachingLog" %>

<%@ Register Src="~/controls/DashDetails.ascx" TagPrefix="UC1" TagName="DashDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="/scripts/jquery.transit.min.js"></script>
    <script type="text/javascript" src="/scripts/raphael-min.js"></script>
    <script type="text/javascript" src="/scripts/lyric/sylvester-0-1-3/sylvester.js"></script>
    <script type="text/javascript" src="/scripts/lyric/lyric.js"></script>
    <script src="js/jquery.scrollintoview.min.js"></script>

    <script src="js/FilterManager.js?1003"></script>

    <link href="gl_files/guidelines.css" rel="stylesheet" />
    <script src="gl_files/jquery.pulse.min.js"></script>
    <script src="gl_files/jquery.transit.min.js"></script>


    <script type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'left=0,top=0,width=600,height=400,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();

        }

        function CallExport(strid) {
            var prtContent = document.getElementById(strid);
            var export_table = $(prtContent).find('table');
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent(document.getElementById(export_table[0].id).outerHTML));
            //window.open('data:application/vnd.ms-excel,<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml"><head></head><body><table>' + $(prtContent).find('table').html() + "</table></body></html>");

        }



        function cancelBubble(e) {
            var evt = e ? e : window.event;
            if (evt.stopPropagation) evt.stopPropagation();
            if (evt.cancelBubble != null) evt.cancelBubble = true;
        }


        function show_guideline_popup() {
            var dialog = $('<b>New Guidelines are available</b>').dialog({
                buttons: {
                    "OK": function () { window.location.href = 'guidelines.aspx'; },
                    "Mark as Read": function () {

                        $.ajax({
                            type: 'POST',
                            url: "cd2.aspx/ClearGuidelines",
                            data: "{}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                $('#guideline_count').html('');
                            }

                            //, async: false
                        });

                        dialog.dialog('close');

                    },
                    "Cancel": function () {
                        dialog.dialog('close');
                    }
                },
                modal: true,
                width: 'auto'
            });

            dialog.position('center');
        }

    </script>


    <style type="text/css">
        .connected, .sortable, .exclude, .handles {
            margin: auto;
            padding: 0;
            /*width: 555px;*/
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            .sortable.grid {
                overflow: hidden;
            }

            /*.connected li, .sortable li, .exclude li, .handles li {
                list-style: none;
                border: 1px solid #CCC;
                background: #F6F6F6;
                font-family: "Tahoma";
                color: #1C94C4;
                margin: 5px;
                padding: 5px;
                height: 22px;
            }*/

            .handles span {
                cursor: move;
            }

        li.disabled {
            opacity: 0.5;
        }

        /*.sortable.grid li {
            line-height: 80px;
            float: left;
            width: 254px;
            height: 80px;
            text-align: center;
        }*/

        li.highlight {
            background: #FEE25F;
        }

        /*#connected {
            width: 440px;
            overflow: hidden;
            margin: auto;
        }*/

        /*.connected {
            float: left;
            width: 200px;
        }*/

        .connected.no2 {
            float: right;
        }

        li.sortable-placeholder {
            border: 1px dashed #CCC;
            background: none;
        }


        .firstCol {
            clear: left;
        }

        .left {
            float: left;
            width: 170px;
            text-align: left;
            cursor: pointer;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hdnAppname" runat="server" />
    <asp:HiddenField ID="hdnScorecard" runat="server" />
    <asp:HiddenField ID="hdnTotalModules" runat="server" />
    <asp:HiddenField ID="hdnFailFilter" runat="server" />

    <asp:HiddenField ID="hdnMainLogo" runat="server" />
    <asp:HiddenField ID="hdnPrimayColor" runat="server" />

    <asp:HiddenField ID="hdnAgent" runat="server" />
    <asp:HiddenField ID="hdnQA" runat="server" />
    <asp:HiddenField ID="hdnNB" runat="server" />
    <asp:HiddenField ID="hdnQID" runat="server" />
    <asp:HiddenField ID="hdnCalibOnly" runat="server" />
    <asp:HiddenField ID="hdnOrigScorePerf" runat="server" />

    <asp:HiddenField ID="hdnFilterArray" runat="server" />

    <asp:HiddenField ID="hdnHardFilters" runat="server" />

    <div class="popup-container" id="general-popup" style="display: none;">

        <header class="popup-header">
            <h1 id="general-popup-main-title">Notifications</h1>

            <a class="close-popup"><i class="fa fa-times"></i></a>
        </header>
        <!-- close popup-header -->
        <div class="popup-body">

            <div class="comments">
                <span class="comments-title" id="general-popup-title"></span>
            </div>
            <hr style="border-top: dotted 1px;" />
            <div class="comments" style="overflow: auto; max-height: 200px;">
                <span class="comments-title" id="general-popup-notification-comments"></span>
            </div>


        </div>
        <!-- close popup-body -->
        <div id="general-popup-footer" class="popup-footer">
            <div class="actions-in-right">
                <span class="playBtn"></span>

                <a class="close-popup third-priority-buttom">Cancel</a>
            </div>
            <!-- close actions-in-right -->
        </div>
        <!-- close popup-footer -->
    </div>



    <div class="popup-container" id="add-report-popup" style="display: none;">

        <header class="popup-header">
            <h1>Notifications</h1>

            <a class="close-popup"><i class="fa fa-times"></i></a>
        </header>
        <!-- close popup-header -->
        <div class="popup-body">

            <div class="comments">
                <span class="comments-title" id="popup-title"></span>
            </div>
            <hr style="border-top: dotted 1px;" />
            <div class="comments" style="overflow: auto; max-height: 200px;">
                <span class="comments-title" id="popup-notification-comments"></span>
            </div>
            <textarea id="txtAddlComments" rows="5" style="width: 563px;"></textarea>
            <center>Related to question: <select id="ddlQuestion"></select></center>
            <br />
            <div id="divAgree" style="text-align: center; width: 100%;">


                <center>Action: <select id="ddlAction"></select></center>


                <%--                <input type="radio" id="noti-agree-radio" name="noti_response" disabled="disabled" value="Agree" />
                <label id="noti-agree-radio-label" for="noti-agree-radio">Agree</label>
                &nbsp;&nbsp;
            <input type="radio" id="noti-disagree-radio" name="noti_response" disabled="disabled" value="Disagree" onchange="updateAssigned" />
                <label id="noti-disagree-radio-label" for="noti-disagree-radio">Disagree</label>
                &nbsp; &nbsp; 
                 <input type="radio" id="noti-acknowledge-radio" name="noti_response" disabled="disabled" value="Acknowledge" onchange="updateAssigned" />
                <label id="noti-acknowledge-radio-label" for="noti-acknowledge-radio">Acknowledge</label>
                &nbsp; &nbsp; 
            <input type="radio" name="noti_response" checked="checked" value="Notes Only" />
                <label for="noti-disagree-radio">Notes Only</label>
                &nbsp; &nbsp; --%>



                <%-- <select name="assigned_to" id="assigned_to">
                </select>--%>
            </div>

        </div>
        <!-- close popup-body -->
        <div id="popup-footer" class="popup-footer">
            <div class="actions-in-right">
                <input type="hidden" id="noti_id" />
                <input type="hidden" id="noti_step" />
                <input type="hidden" id="noti_form_id" />
                <input type="hidden" id="noti_so" />
                <span class="playBtn">
                    <a id="playBtnNotification" href="review_record.aspx?ID=711842" target="_blank">
                        <button type="button">
                            <div></div>
                        </button>
                    </a>
                </span>

                <a class="close-popup third-priority-buttom" title="Create New Dispute" id="btnCreateUpdate" href="javascript:noti_update('New');">Create Dispute</a>
                <a class="close-popup third-priority-buttom" title="Add Notes Only" id="btnUpdate" href="javascript:noti_update('Update');">Update</a>
                <input type="checkbox" id="chkSuper" hidden="hidden" value="Supervisor Override" />
                <% If Roles.IsUserInRole("QA Lead") Or Roles.IsUserInRole("Calibrator") Then %>
                <a class="third-priority-buttom" title="Escalate" href="javascript:Escalate();">Escalate</a>
                <% End If%>
                <a class="close-popup third-priority-buttom">Cancel</a>
                <span id="sql-text"></span>
            </div>
            <!-- close actions-in-right -->
        </div>
        <!-- close popup-footer -->
    </div>



    <style>
        #expand-window {
            position: fixed;
            top: 12.5%;
            left: 12.5%;
            background: #f2f2f2;
            border-radius: 3px;
            box-shadow: 0px 10px 80px rgba(0,0,0,0.6);
            z-index: 4000;
        }

        .expand-popup-body {
            padding: 25px;
            overflow: auto;
        }
    </style>
    <div class="expand-container" id="expand-window" style="display: none;">
        <header class="popup-header">
            <h1></h1>

            <a id="expand-window-close" class="close-popup"><i class="fa fa-times"></i></a>
            <a id="print-view"><i class="fa fa-print" onclick="CallPrint('expand-window');"></i></a>
            <a id="export-view"><i class="fa fa-save" onclick="CallExport('expand-window');"></i></a>
        </header>
        <!-- close popup-header -->
        <div class="expand-popup-body">
        </div>
        <!-- close popup-body -->
        <div id="expand-popup-footer" class="popup-footer">
            <div class="actions-in-right">
                <a id="close-expand-window" class="close-popup third-priority-buttom">Cancel</a>
            </div>
            <!-- close actions-in-right -->
        </div>
        <!-- close popup-footer -->
    </div>

    <!-- Start page content -->
    <div id="dashboardPage">

        <table class="page-header" cellpadding="0px" cellspacing="0px">
            <tr>
                <td class="pageName">
                    <img id="page_main_logo" style="display:none;" />




                </td>
                <%--<td align="center">
                    <asp:Literal ID="litClientName" Visible="false" runat="server"></asp:Literal><asp:Label ID="lblName" runat="server" Text="" Font-Size="Small"></asp:Label><br />
                    <a href="logout.aspx" style="font-size: small;">Logout</a>

                </td>--%>
                <%--  <td>&nbsp;</td>--%>

                <td class="dateRange">
                    <asp:TextBox ID="date1" title="Starting Call Date" data-datetype="from" runat="server"></asp:TextBox>
                    &nbsp; &mdash; &nbsp;
                     <asp:TextBox ID="date2" title="Ending Call Date" data-datetype="to" runat="server"></asp:TextBox>
                </td>
                <td class="applyFiltersBtnTd">
                    <div class="filterBtn applyFiltersBtn" onclick="pageLoad(); updateFilters(); toggleOpenFilters('', 'close');">
                        <span>Apply</span>
                    </div>
                </td>
                <td class="filterBtnTd">
                    <div class="filterBtn filterBtn1" title="Filters" data-filternum="1"><i class="fa fa-filter"></i></div>
                </td>
                <td class="filterBtnTd <%=Replace(myRole, " ", "_")%>">
                    <div class="filterBtn filterBtn2" title="Billing" data-filternum="2"><span>$</span></div>
                </td>
                <!-- just remove .hiddenFilterBtnTd and .hiddenFilterBtn to show/activate button and resize Apply button --> 
                <td class="filterBtnTd">
                    <div class="filterBtn filterBtn3 addModule" title="Add Modules" data-filternum="3"><i class="fa fa-plus-circle"></i></div>
                </td>
                <%-- <td class="filterBtnTd">
                    <div class="filterBtn filterBtn4" title="Export Dashboard" data-filternum="4"><i class="fa fa-external-link"></i></div>
                </td>--%>
                <td class="filterBtnTd">
                    <asp:Literal ID="litUpdateGuidlines" runat="server"></asp:Literal>
                    <div class="updates-btn" id="showGuidlines" runat="server" title="Updated Guidlines">
                        <span onclick="window.location.href='guidelines.aspx';" style="text-align: left;">
                            <i class="fa fa-bell-o"></i>
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
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
                        <span class="dropdown   <%=Replace(myRole, " ", "_")%>">
                            <select id="ddlGroup" onchange="addArrayFilter('group',  $('#ddlGroup').val(),$('#ddlGroup').val()); updateAgents(); updateCampaigns();">
                                <option value="">All Groups</option>
                            </select>
                        </span>
                        <span class="dropdown  <%=Replace(myRole, " ", "_")%>">
                            <select id="ddlAgent" onchange="addArrayFilter('agent', $('#ddlAgent').val(),$('#ddlAgent').val()); ">
                                <option value="">All Agents</option>
                            </select>
                        </span>
                        <span class="dropdown  <%=Replace(myRole, " ", "_")%>">
                            <select id="ddlCampaigns" onchange="addArrayFilter('campaign', $('#ddlCampaigns').val(),$('#ddlCampaigns').val()); ">
                                <option value="">All Campaigns</option>
                            </select>
                        </span>

                        <span class="dropdown">
                            <select id="ddlAppnames" onchange="addArrayFilter('scorecard', $('#ddlAppnames').val(), $('#ddlAppnames option:selected').text()); updateGroups(); updateAgents(); updateCampaigns(); $('#<%=hdnScorecard.Clientid %>').val($(this).val());">
                            </select>
                        </span>


                    </div>

                    <div class="filtersSet filtersSet2">
                        Use CTRL to select more than one of each filter. Hold down CTRL before opening the dropdown.
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

                            <asp:Repeater ID="rptOpen" DataSourceID="dsOpenBilling" runat="server">
                                <ItemTemplate>

                                    <tr>
                                        <td>Unbilled for <%#New DateTime(2010, Eval("review_month"), 1).ToString("MMM") %> - <%#Eval("appname") %> </td>
                                        <td>
                                            <span class="billLabel">Time:</span></td>
                                        <td>
                                            <span class="billData billedTime">
                                                <asp:Literal ID="litPendTime" Text='<%# Eval("unpaidTime") %>' runat="server"></asp:Literal></span>
                                        </td>
                                        <td>
                                            <span class="billLabel">Amount:</span></td>
                                        <td>
                                            <span class="billData billedAmount">
                                                <asp:Literal ID="litPendAmount" Text='<%# FormatCurrency(Eval("unpaid_amount"), 2) %>' runat="server"></asp:Literal></span>
                                        </td>
                                        <td>
                                            <span class="billLabel"># Calls:</span></td>
                                        <td>
                                            <span class="billData numberCalls">
                                                <asp:Literal ID="litPendNumberCalls" Text='<%# Eval("num_calls") %>' runat="server"></asp:Literal></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>


                            <asp:SqlDataSource ID="dsOpenBilling" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="getBillingMonth" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter Name="Username" />
                                </SelectParameters>

                            </asp:SqlDataSource>

                            <asp:Repeater ID="rptBilling" DataSourceID="dsBilling" runat="server">

                                <HeaderTemplate>
                                    <tr>
                                        <td>Open Invoices:</td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style='color: <%#eval("font_color")%>'>
                                        <td>Inv #:<%#Eval("Invoice") %>
                                            <div style="font-size: smaller">(<%#Eval("bill_from", "{0:M/d/yyyy}")%> - <%#Eval("bill_to", "{0:M/d/yyyy}")%>)</div>
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
                                SelectCommand="select distinct * from (select Invoice,bill_seconds,amount,bill_calls,bill_from,bill_to, case when bill_date < DATEADD(d, -30, getdate()) then 'red' else 'black' end as font_color from billingdata join userapps on userapps.appname = billingdata.appname where userapps.username = @username and bill_paid_date is null ) a">
                                <SelectParameters>
                                    <asp:Parameter Name="Username" />
                                </SelectParameters>

                            </asp:SqlDataSource>

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
                                <td>
                                    <asp:Repeater ID="dlModules" DataSourceID="dsModules" runat="server">
                                        <ItemTemplate>

                                            <div class="filterBtn addModule left" onclick="updateDashOrder<%#Eval("add_delete")%>('<%#Eval("moduleName")%>',0);window.location.href='cd2.aspx';" data-filternum="4">
                                                <i class="fa fa-<%#Eval("plus_minus")%>-circle"></i>
                                                <asp:Label ID="lblModuleName" runat="server" Text='<%#Eval("moduleName")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>

                                    </asp:Repeater>
                                </td>
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



        <div class="dash-modules" id="topDashModules">
            <ul id="dashModules" class="sortable grid" runat="server"> 
            </ul>
        </div>
        <div class="dash-modules" id="bottomDashModules">
            <UC1:DashDetails runat="server" ID="dd" />
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




        <!-- End page content -->


        <script>
            $('#dashModules').sortable({
                opacity: 0.6,
                tolerance: 'touch',
                cursor: 'move',
                update: function () {
                    //console.log('sorted');
                    //$('.filterBtn3').css('backgroundcolor', '#41637c');
                    //$('.filterBtn3').html('<i class="fa fa-plus-circle"></i>');
                },
                sort: function () {
                    //console.log('sorting');
                    //$('.filterBtn3').css('backgroundcolor', '#bb1132');
                    //$('.filterBtn3').html('<i class="fa fa-trash"></i>');
                }
            });


            $('#dashModules').sortableOptions = {

                'ui-floating': true

            };


            //$('.dash-module').draggable();


            $('.sortable').bind('sortupdate', function () {
                clearDash()
                $('.dash-module').each(function (index, item) {
                    updateDashOrder($(item).data('uiname'), index);
                });
                deDupeDash()

            });



            $('#trashModuleTd').droppable({
                over: function (event, ui) {
                    ui.draggable.remove();
                }
            });

            $('#add-report-popup').draggable();



        </script>


        <script type="text/javascript">




            function refresh_modules() {

                // $.xhrPool.abortAll();


                updateApp();

                <%=registered_callbacks%>


                var filter_list = ''

                if ($('#ddlGroup').val() != '')
                    filter_list += "Group: " + $('#ddlGroup').val() + " "
                if ($('#ddlAgent').val() != '')
                    filter_list += "Agent: " + $('#ddlAgent').val() + " "
                if ($('#ContentPlaceHolder1_hdnQA').val() != '')
                    filter_list += "QA: " + $('#ContentPlaceHolder1_hdnQA').val() + " "
                if ($('#ddlCampaigns').val() != '')
                    filter_list += "Campaign: " + $('#ddlCampaigns').val() + " "
                if ($('#ddlAppnames').val() != '')
                    filter_list += "Scorecard: " + $('#ddlAppnames option:selected').text() + " "


                if (filter_list != '') {
                    //$('#AppliedFilters').text(filter_list);
                    $('#filterIcon').show();
                    $('#globalReset').show();
                }
                else {
                    $('#filterIcon').hide();
                    $('#globalReset').hide();
                }

                PopFilterList();
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



            function show_bad_audio(file_name) {
                $('#notification_holder').show();


                if (!player_loaded) {
                    setupAudioPlayer(file_name, 0.75, true);
                    player_loaded = true;
                }
                else {
                    if ($("#notification_player").attr("src") != file_name) {
                        change(file_name);
                    }
                }

            }


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
            //updateGroups();
            //updateAgents();
            //updateCampaigns();
            //getDetails(0);


            /*
            * HTML5 Sortable jQuery Plugin
            * http://farhadi.ir/projects/html5sortable
            * 
            * Copyright 2012, Ali Farhadi
            * Released under the MIT license.
            */ (function ($) {
                var dragging, placeholders = $();
                $.fn.sortable = function (options) {
                    var method = String(options);
                    options = $.extend({
                        connectWith: false,
                        onStartDrag: function () { },
                        onEndDrag: function () { },
                        onChangeOrder: function () { }
                    }, options);
                    return this.each(function () {
                        if (/^enable|disable|destroy$/.test(method)) {
                            var items = $(this)
                                .children($(this)
                                    .data('items'))
                                .attr('draggable', method == 'enable');
                            if (method == 'destroy') {
                                items.add(this)
                                    .removeData('connectWith items')
                                    .off('dragstart.h5s dragend.h5s selectstart.h5s dragover.h5s dragenter.h5s drop.h5s');
                            }
                            return;
                        }
                        var isHandle, index, items = $(this)
                            .children(options.items);
                        var placeholder = $('<' + (/^ul|ol$/i.test(this.tagName) ? 'li' : 'div') + ' class="sortable-placeholder">');
                        items.find(options.handle)
                            .mousedown(function () {
                                isHandle = true;
                            })
                            .mouseup(function () {
                                isHandle = false;
                            });
                        $(this)
                            .data('items', options.items)
                        placeholders = placeholders.add(placeholder);
                        if (options.connectWith) {
                            $(options.connectWith)
                                .add(this)
                                .data('connectWith', options.connectWith);
                        }
                        items.attr('draggable', 'true')
                            .on('dragstart.h5s', function (e) {
                                if (options.handle && !isHandle) {
                                    return false;
                                }
                                options.onStartDrag();
                                placeholder.css("height", $(this).height() + "px");
                                isHandle = false;
                                var dt = e.originalEvent.dataTransfer;
                                dt.effectAllowed = 'move';
                                dt.setData('Text', 'dummy');
                                index = (dragging = $(this))
                                    .addClass('sortable-dragging')
                                    .index();
                            })
                            .on('dragend.h5s', function () {
                                if (!dragging) {
                                    return;
                                }
                                options.onEndDrag();
                                dragging.removeClass('sortable-dragging')
                                    .show();
                                placeholders.detach();
                                if (index != dragging.index()) {
                                    dragging.parent()
                                        .trigger('sortupdate', {
                                            item: dragging
                                        });
                                }
                                dragging = null;
                            })
                            .not('a[href], img')
                            .on('selectstart.h5s', function () {
                                this.dragDrop && this.dragDrop();
                                return false;
                            })
                            .end()
                            .add([this, placeholder])
                            .on('dragover.h5s dragenter.h5s drop.h5s', function (e) {
                                if (!items.is(dragging) && options.connectWith !== $(dragging)
                                    .parent()
                                    .data('connectWith')) {
                                    return true;
                                }
                                if (e.type == 'drop') {
                                    e.stopPropagation();
                                    placeholders.filter(':visible')
                                        .after(dragging);
                                    dragging.trigger('dragend.h5s');
                                    return false;
                                }
                                e.preventDefault();
                                e.originalEvent.dataTransfer.dropEffect = 'move';
                                if (items.is(this)) {
                                    if (options.forcePlaceholderSize) {
                                        placeholder.height(dragging.outerHeight());
                                    }
                                    dragging.hide();
                                    $(this)[placeholder.index() < $(this)
                                        .index() ? 'after' : 'before'](placeholder);
                                    placeholders.not(placeholder)
                                        .detach();
                                } else if (!placeholders.is(this) && !$(this)
                                    .children(options.items)
                                    .length) {
                                    placeholders.detach();
                                    $(this)
                                        .append(placeholder);
                                }
                                options.onChangeOrder();
                                return false;
                            });
                    });
                };
            })(jQuery);




            function addFloats_orig(container) {

                var float_count = parseInt($(window).width() / 320);


                var module_rows = $('#ContentPlaceHolder1_hdnTotalModules').val() / parseInt(parseInt($(window).width() / 320));

                //if ((module_rows > 1) && (module_rows <= 2))
                //    float_count = parseInt((parseInt($('#ContentPlaceHolder1_hdnTotalModules').val()) + 1) / 2);


                //if (parseInt($('#ContentPlaceHolder1_hdnTotalModules').val()) < 7)
                //    float_count = parseInt((parseInt($('#ContentPlaceHolder1_hdnTotalModules').val()) + 1) / 2);

                //var float_count = parseInt((parseInt($('#ContentPlaceHolder1_hdnTotalModules').val()) + 1) / 2);
                //console.log('float_count =' + float_count);
                var total_width = 0;
                $(container).find("li:not(.sortable-dragging)").removeClass("firstCol").each(function (index, element) {

                    if (total_width % float_count == 0) { $(this).addClass("firstCol"); }
                    if ($(this).hasClass("single")) total_width += 1;
                    if ($(this).hasClass("double")) total_width += 2;
                    if ($(this).hasClass("triple")) total_width += 3;
                    //console.log('total_width =' + total_width);

                });

            }

            function addFloats(container) {
                var modules_per_row = parseInt($('#ContentPlaceHolder1_dashModules').width() / 320);
                //console.log('modules_per_row = ' + modules_per_row + ' because ' + $('#ContentPlaceHolder1_dashModules').width() + '/320');
                var total_modules = $('#ContentPlaceHolder1_hdnTotalModules').val();
                var i = 1; x = 0;
                while (i < modules_per_row) {
                    x = total_modules / i;
                    if (x <= modules_per_row) {
                        modules_per_row = Math.ceil(x);
                        //console.log('new modules_per_row = ' + modules_per_row);
                        break;
                    }
                    i++;
                }
                var module_count = 0; counter = modules_per_row; module_in_last_row = 0;
                $(container).find("li:not(.sortable-dragging)").removeClass("firstCol").each(function (index, element) {
                    if (module_count % modules_per_row == 0) {
                        $(this).addClass("firstCol"); counter = 0;
                        //console.log('firstCol via module count = ' + module_count); 
                    }
                    module_in_last_row = module_count;
                    if ($(this).hasClass("single")) { module_count += 1; counter += 1; }
                    if ($(this).hasClass("double")) { module_count += 2; counter += 2; }
                    if ($(this).hasClass("triple")) { module_count += 3; counter += 3; }
                    //console.log('module_in_last_row = ' + module_in_last_row + '    module_count = ' + module_count + '    counter = ' + counter);
                    if (counter > modules_per_row) {
                        $(this).addClass("firstCol");
                        counter = module_count - module_in_last_row;
                        //console.log('firstCol via counter = ' + module_count);
                    }

                });

            }

            $('.sortable').sortable({
                //onStartDrag: function () { $('.filterBtn3').css('background-color', '#bb1132'); $('.filterBtn3').html('<i class="fa fa-trash"></i>'); addFloats($(".sortable")); },
                //onEndDrag: function () { $('.filterBtn3').css('background-color', '#41637c'); $('.filterBtn3').html('<i class="fa fa-plus-circle"></i>'); addFloats($(".sortable")); },
                onEndDrag: function () { addFloats($(".sortable")); },
                //onChangeOrder: function () { addFloats($(".sortable")); }
                //handle: '.handle'

            }).bind('sortupdate', function () {

                addFloats($(".sortable"));

            });

            addFloats($(".sortable"));

            $(window).resize(function () {
                addFloats($(".sortable"));
            })


            var updatesCount = $('.update').length;
            document.updatesCount = updatesCount;
            if (updatesCount > 0) {
                setTimeout(function () {
                    $('.updates-btn').addClass('show');
                    setTimeout(function () {
                        ringUpdatesBell();
                    }, 1000);
                }, 1000);

                $('.updates-btn').click(function () {
                    $(this).addClass('arrow');
                    //nextUpdate();
                });
                document.updateNum = 0;
            }



            function ringUpdatesBell() {
                var bell = $('.updates-btn i').first();
                var deg = '10deg';
                var rotateTime = 60;
                bell.transition({
                    rotate: deg, duration: rotateTime, complete: function () {
                        bell.transition({
                            rotate: '-' + deg, duration: rotateTime, complete: function () {
                                bell.transition({
                                    rotate: deg, duration: rotateTime, complete: function () {
                                        bell.transition({
                                            rotate: '-' + deg, duration: rotateTime, complete: function () {
                                                bell.transition({
                                                    rotate: deg, duration: rotateTime, complete: function () {
                                                        bell.transition({
                                                            rotate: '-' + deg, duration: rotateTime, complete: function () {
                                                                bell.transition({
                                                                    rotate: deg, duration: rotateTime, complete: function () {
                                                                        bell.transition({
                                                                            rotate: '-' + deg, duration: rotateTime, complete: function () {
                                                                                bell.transition({
                                                                                    rotate: deg, duration: rotateTime, complete: function () {
                                                                                        bell.transition({
                                                                                            rotate: '-' + deg, duration: rotateTime, complete: function () {
                                                                                                bell.transition({ rotate: '0deg', duration: rotateTime });
                                                                                                if (!$('.updates-btn').hasClass('arrow')) {
                                                                                                    setTimeout(function () {
                                                                                                        ringUpdatesBell();
                                                                                                    }, 5000);
                                                                                                }
                                                                                            }
                                                                                        });
                                                                                    }
                                                                                });
                                                                            }
                                                                        });
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        });
                    }
                });
            }






            //pageLoad();

        </script>
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

            }, 100);

            $(document).submit(function () {
                $.xhrPool.abortAll();
            });


            $(".filterBtn4").click(function (e) {
                window.open('data:application/vnd.ms-excel,' + $('#dashboardPage').html());
                e.preventDefault();
            });


            updateApp();

            $(document).ready(function () {
                PopFilterList();
            });

    </script>

<%--    <div class="jdiv">
        <UC1:CoachingLog runat="server" />
    </div>
--%>


</asp:Content>

