<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="bad_call_report.aspx.vb" Inherits="bad_call_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Bad Call List</h2>




        <%--    <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>--%>
                Appname:
                <asp:DropDownList ID="ddlApps" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="appname" DataValueField="appname" runat="server">
                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select appname from app_settings where active = 1 and appname <> '' order by appname" runat="server"></asp:SqlDataSource>

        Scorecard:
                <asp:DropDownList ID="ddlScorecards" AppendDataBoundItems="true" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="id" runat="server">
                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                </asp:DropDownList>
        <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select scorecards.id, short_name from app_settings join scorecards on scorecards.appname = app_settings.appname where scorecards.active = 1 and app_settings.active = 1 order by short_name" runat="server"></asp:SqlDataSource>


        TL:
                <asp:DropDownList ID="ddlTL" AppendDataBoundItems="true" DataSourceID="dsTLs" DataTextField="team_lead" DataValueField="team_lead" runat="server">
                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                </asp:DropDownList>
        <asp:SqlDataSource ID="dsTLs" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct team_lead from scorecards where team_lead is not null order by team_lead" runat="server"></asp:SqlDataSource>

        <i class="fa fa-calendar-o"></i>
        <asp:TextBox ID="txtStartDate" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
        <i class="fa fa-calendar-o"></i>
        <asp:TextBox ID="txtEndDate" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
        <asp:Button ID="btnApplyFilter" runat="server" Text="APPLY" />
        <asp:Button ID="btnExport" runat="server" Text="Export Excel" />
        <%--   ////////////////--%>
        <br />
        <asp:Button ID="btnSelectall" runat="server" Text="ACCEPT AS BAD" />
        <%--   ////////////////--%>
        <br />
        <br />
        <asp:HiddenField ID="hdnIdusrname" runat="server" />
        Rows:
        <asp:Label ID="lblRows" runat="server" Text=""></asp:Label>
        <asp:GridView ID="GVBadCalls" AllowSorting="true" GridLines="None" runat="server" AutoGenerateColumns="False" CssClass="detailsTable"
            DataKeyNames="ID" DataSourceID="dsBadCalls">
            <Columns>
                <%--   ////////////////--%>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input type="checkbox" name="select-all" id="selectall" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkBad" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate></HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button CommandName="Accept as Bad" Text="Accept as Bad" runat="server" CommandArgument="<%# Container.DataItemIndex %>" />

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate></HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button CommandName="Reset" Text="Reset" runat="server" CommandArgument="<%# Container.DataItemIndex %>" />
                        <asp:Button CommandName="Recreate" Text="Recreate Call" runat="server" CommandArgument="<%# Container.DataItemIndex %>" />

                    </ItemTemplate>
                </asp:TemplateField>
                <%--  //////////////////--%>
                <asp:HyperLinkField DataNavigateUrlFields="SESSION_ID" Target="_blank" DataNavigateUrlFormatString="/listen?session_ID={0}" DataTextField="SESSION_ID" HeaderText="SESSION_ID" SortExpression="SESSION_ID" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="#" onclick="$('.audio-player').show();setupAudioPlayer('<%#Eval("theLink") %>', 0.75, true);reload_audio();">Play</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="appname" HeaderText="AppName" SortExpression="appname" />
                <asp:BoundField DataField="short_name" HeaderText="Scorecard" SortExpression="short_name" />
                <%-- <asp:BoundField DataField="TIMESTAMP" HeaderText="TIMESTAMP" SortExpression="TIMESTAMP" />--%>
                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                <asp:BoundField DataField="review_started" HeaderText="Review Started" SortExpression="review_started" />
                <%--  <asp:BoundField DataField="call_length" HeaderText="call_length"
                            SortExpression="call_length" />--%>
                <asp:HyperLinkField DataNavigateUrlFields="theLink" Target="_blank" DataTextField="theLink" HeaderText="Link" SortExpression="theLink" />
                <asp:BoundField DataField="bad_call_who" HeaderText="Who" SortExpression="bad_call_who" />
                <asp:BoundField DataField="bad_call_date" HeaderText="Date" SortExpression="bad_call_date" />
                <asp:BoundField DataField="bad_call_reason" HeaderText="Reason" SortExpression="bad_call_reason" />
                <asp:BoundField DataField="agent" HeaderText="Agent/Partner" SortExpression="agent" />
                <%-- <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />--%>
                <%-- <asp:ButtonField ButtonType="Button" CommandName="Reset" Text="Reset" HeaderText=" " />
                <asp:ButtonField ButtonType="Button" CommandName="Accept as Bad" Text="Accept as Bad" HeaderText=" " />--%>

                <%-- <asp:BoundField DataField="bad_call_accepted" />--%>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsBadCalls" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select *, isnull(website, 'http://files.callcriteria.com' + audio_link) as theLink from xcc_report_new 
                        join scorecards on scorecards.id = xcc_report_new.scorecard where bad_call_date between @start and @end and bad_call = 1 
                        and bad_call_accepted is null 
                        and 1 = case when @appname != '' and xcc_report_new.appname != @appname then 0 else 1 end 
                        and 1 = case when @scorecard != '' and xcc_report_new.scorecard != @scorecard then 0 else 1 end 
                        and 1 = case when @team_lead != '' and team_lead != @team_lead then 0 else 1 end 
                        order by xcc_report_new.appname">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStartDate" Name="start"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="txtEndDate" Name="end" PropertyName="Text" />
                <asp:ControlParameter ControlID="ddlApps" Name="appname" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="ddlScorecards" Name="scorecard" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="ddlTL" Name="team_lead" ConvertEmptyStringToNull="false" />
            </SelectParameters>
        </asp:SqlDataSource>

        <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
    </section>

    <div class="audio-player">
        <audio></audio>
        <div class="audio-player-inner-content">
            <div class="show-hide-player">
                <a href="#" class="hide-player"><i class="fa fa-caret-down"></i>HIDE</a> <a href="#"
                    class="show-player"><i class="fa fa-caret-up"></i>&nbsp; SHOW</a>
            </div>
            <div class="player-left-part">
                <i class="fa icon-mute fa-volume-up"></i>
                <div class="volumne-options">
                    <span class="section-label">Volume</span>
                    <div id="volume-slider" class="slider volume-slider dragdealer">
                        <div class="slider-trigger handle">
                        </div>
                        <div class="slider-fill">
                        </div>
                    </div>
                    <!-- close slider -->
                </div>
                <!-- close volume-options -->
                <div class="player-controls">
                    <a href="#" class="play-button"><i class="fa icon-play-pause"></i></a>
                    <div class="audio-rate">
                        <a href="#" title="-" data-rate="-20">-</a> <a href="#" title="Normal speed"
                            data-rate="0">0</a> <a href="#" title="+" data-rate="20>">+</a>
                    </div>
                </div>
                <!-- close player-controls -->
            </div>
            <!-- close player-left-part -->
            <div class="player-timeline">
                <span class="section-label"><span class="audio-current-time">0:00</span> / <span
                    class="audio-duration">0:00</span> </span>
                <div id="timeline-slider" class="slider timeline-slider dragdealer">
                    <div class="slider-trigger handle">
                        <div>
                            <span class="audio-current-time">0:00</span>
                        </div>
                    </div>
                    <!-- close slider-trigger -->
                    <asp:Literal ID="litSliderPoints" runat="server"></asp:Literal>
                    <div class="slider-fill">
                    </div>
                </div>
                <!-- close slider -->
            </div>
            <!-- close player-timeline -->
            <div class="player-agent">
                <%-- <img src="images/grey-avatar.png" />--%>
                <a href="#" onclick="reload_audio();"><i class="fa fa-refresh" style="color: white;"></i></a>
                <%-- <div>
                    <strong>John Smith</strong>
                    <span>ID: 2312312312</span>
                </div>--%>
                <!-- <a href="#" class="player-configuration"><i class="fa fa-cog"></i></a> -->
            </div>
            <!-- close player-agent -->
        </div>
        <!-- close audio-player-inner-content -->
    </div>

    <script type="text/javascript">
        setupCalendars();

    </script>


    <style>
        #edit-popup {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 16px;
            background-color: #CCC;
            display: none;
            position: absolute;
            z-index: 4000;
        }

        #edit-popup-container {
            display: table-cell;
            text-align: center;
            vertical-align: middle;
        }

        #edit-popup-content {
            display: inline-block;
            text-align: left;
        }

        #edit-popup-done {
            position: absolute;
            left: -20px;
            top: 0px;
        }

        #edit-popup-close {
            position: absolute;
            left: -20px;
            top: 20px;
        }
    </style>
    <div id="edit-popup">
        <div id="edit-popup-container">
            <div id="edit-popup-content">
                <textarea id="notes"></textarea>
                <a id="edit-popup-done"><i class="fa fa-check"></i></a>
                <a id="edit-popup-close"><i class="fa fa-times"></i></a>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $('#add-report-popup').hide();
        });

        $(document).ready(function () {

            var obj;

            //$('#ContentPlaceHolder1_GVBadCalls td:nth-child(10)').css({ cursor: 'pointer' });
            //$('#edit-popup-done').css({ cursor: 'pointer' });
            //$('#edit-popup-close').css({ cursor: 'pointer' });

            //$('#ContentPlaceHolder1_GVBadCalls td:nth-child(13)').each(function () {
            //    if ($(this).text().length > 6) {
            //        console.log($(this).text())
            //        $(this).parent().find('td:nth-child(12) > input').css('background-color', '#666').prop('disabled', true);
            //    }
            //    $(this).remove();
            //});

            //$('#notes').keydown(function (e) {
            //    if (e.keyCode == 13) {
            //        $('#edit-popup-done').click();
            //        return false;
            //    }
            //});

            //$('#ContentPlaceHolder1_GVBadCalls td:nth-child(10)').click(function () {
            //    obj = $(this).parent();
            //    $('#notes').val($(this).text());
            //    $('#notes').select();
            //    $('#edit-popup, #notes').css({ width: $(this).width() + 10, height: $(this).height() + 10 })
            //    $('#edit-popup').css({ left: $(this).position().left, top: $(this).position().top })
            //    $('#edit-popup-container').css({ width: $(this).width() + 10, height: $(this).height() + 10 })
            //    $('#edit-popup').show();
            //    $('#notes').focus();
            //});

            $('#edit-popup-done').click(function () {
                if (obj != null) {
                    $.ajax({
                        type: "POST",
                        url: "/CDService.svc/UpdateBadCallNotes",
                        data: '{"session_id" : "' + obj.find('td:nth-child(1)').text() + '","notes":"' + $('#notes').val() + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {
                            obj.find('td:nth-child(10)').text($('#notes').val());
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    $('#edit-popup').hide();
                }
            });

            $('#edit-popup-close').click(function () {
                $('#edit-popup').hide();
            });

        });


        ////////////////////////////////
        $('#selectall').click(function (event) {
            if (this.checked) {
                // Iterate each checkbox
                $(':checkbox').each(function () {
                    this.checked = true;
                });
            }
            else {
                $(':checkbox').each(function () {
                    this.checked = false;
                });
            }
        });




        $('#ContentPlaceHolder1_btnSelectall').click(function () {
            //alert("tt");
            var createrr = $("#ContentPlaceHolder1_hdnIdusrname").val();
            var istingg = "";
            //var commenttext = $('#ContentPlaceHolder1_txtcommentsspotcheck').val();
            $('input[type="checkbox"]').each(function () {
                if ($(this).prop("checked") == true) {
                    var s = $(this).parentsUntil("tr").parent().attr('id');

                    if (s != undefined) {
                        istingg = istingg + s + ",";
                        //$(this).parentsUntil("tr").parent().hide();
                    }

                }
            });
            $.ajax({
                type: "POST",
                url: "bad_call_report.aspx/multipleasbad",
                data: '{isting: "' + istingg + '",creater:"' + createrr + '" }',
                contentType: "application/json; charset=utf-8",

                success: function (responsee) {
                    //$('input[type="checkbox"]').each(function () {
                    //});
                    if (istingg != "") {
                        alert('successfully added multiple as bad');
                        //$('#ContentPlaceHolder1_txtcommentsspotcheck').val("");
                        $('input[type="checkbox"]').each(function () {
                            if ($(this).prop("checked") == true) {
                                $(this).removeAttr('checked');
                            }
                        });

                    } location.reload();

                },
                failure: function (response) {

                }
            });
            return false;
        });

        ////////////////////////////////


    </script>
</asp:Content>
