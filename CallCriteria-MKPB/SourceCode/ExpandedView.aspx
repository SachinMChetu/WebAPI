<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="ExpandedView.aspx.vb" Inherits="ExpandedView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        $(document).ready(function () {
            //setupAudioPlayer('nofile.mp3', 0.75);
            setupExpandCollapse();
            setupCalendars();
            //$('.audio-player').hide();
        });

        function jumpPos(to_time) {

            audio = $('.audio-player audio').get(0);
            audio.currentTime = to_time;
            audio.play();

            //$('#notification_player').currentTime = to_time;
            //$('#notification_player').play();
            //audio.currentTime = to_time;
            //audio.play();
        }

        //function show_audio(file_name, position, form_id) {
        //    var new_filename;
        //    $.ajax({
        //        type: 'POST',
        //        url: "convert_audio.aspx",
        //        data: { ID: form_id },
        //        success: function (result) {
        //            new_filename = result;
        //        },
        //        async: false
        //    });
        //    $('#notification_holder').show();

        //    document.getElementById('ContentPlaceHolder1_hdnOpenFormID').value = form_id;
        //    setupAudioPlayer2(new_filename, 0.75, true, position);
        //    //jumpPos(position);

        //}
    </script>

    <asp:HiddenField ID="hdnFilter" runat="server" />
    <section class="main-container dash-modules general-button ">


        <h1 class="section-title"><i class="fa fa-bar-chart-o"></i>Call History Report</h1>


        <%-- <div style="float: right;">
                <div class="report-nav"><a href="ExpandedView.aspx" class="selected-type"><i class="fa fa-table"></i><span>Call History Report</span></a></div>

                <div class="report-nav" id="QA_Weekly_div" runat="server"><a href="QA_Report2.aspx"><i class="fa fa-calendar-o"></i><span>QA Report</span></a></div>
            </div>--%>

        <div class="applied-filters-top">
            <label>
                Applied Filters:</label>
            <span><i class="fa fa-user"></i><em>Group/s: <strong>
                <asp:Literal ID="litGroupFilter" runat="server"></asp:Literal></strong></em> </span>
            <span><i class="fa fa-clock-o"></i><em>Period: <strong>
                <asp:Literal ID="litDateRange" runat="server"></asp:Literal></strong></em>
            </span>
        </div>
        <!-- close applied-filters -->




        <div class="standard-reports-container">


            <div class="my-shared-reports">

                <%--   <h2><i class="fa fa-table"></i>Expanded Report</h2>--%>



                <div >


                    <div >
                        <div class="field-holder">
                            <i class="fa fa-calendar-o"></i>
                            <asp:TextBox ID="txtGroupStartxp" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                        </div>
                        <!-- close date-holder -->


                        <div class="field-holder">
                            <i class="fa fa-calendar-o"></i>
                            <asp:TextBox ID="txtgroupEndxp" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                        </div>
                        <!-- close date-holder -->




                        <div class="field-holder">
                            <i class="fa fa-tag"></i>
                            <asp:DropDownList ID="ddlGroup" runat="server" DataTextField="AGent_group"
                                DataSourceID="dsGroupList" AutoPostBack="true" AppendDataBoundItems="true" DataValueField="AGent_group">
                                <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                                <%--<asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsGroupList" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="SELECT distinct AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and agent_group is not null and agent_group <> '' order by AGENT_GROUP">
                                <SelectParameters>
                                    <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue="" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>

                        <div class="field-holder" runat="server" id="agentHolder">
                            <i class="fa fa-user"></i>
                            <asp:DropDownList ID="ddlAgent" Visible="true" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                DataTextField="AGent" DataValueField="AGent">
                                <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                            </asp:DropDownList>

                        </div>


                        <asp:Button ID="btnAgentGroupxp" runat="server" Text="Apply Filters" />
                        <asp:Button ID="btnViewDetail"  runat="server" Text="Detail View" />


                    </div>
                    <!-- close sub-filters-content -->
                </div>
                <!-- close sub-filters -->





                <%-- <asp:UpdatePanel runat="server">
                    <ContentTemplate>--%>



                <asp:Label ID="lblRows" runat="server" Text=""></asp:Label>
                <div class="table-outline users-list">

                    <asp:HiddenField ID="hdnExtraFilters" Value="" runat="server" />

                    <asp:GridView ID="gvAgentGroupxp" DataSourceID="dsFormData" ShowHeader="true" ShowFooter="true" EnableViewState="false" AllowSorting="true"
                        runat="server" AutoGenerateColumns="true" DataKeyNames="id" CellPadding="4"
                        GridLines="None" Width="100%">
                    </asp:GridView>

                    <asp:SqlDataSource ID="dsFormData" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="GetExpandedViewMissed" SelectCommandType="StoredProcedure" runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="where_clause" DefaultValue="" />
                            <asp:SessionParameter SessionField="appname" Name="appname" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <asp:Button ID="btnSupeExportxp" runat="server" CssClass="add-category-btn main-cta" Visible="false" Text="Export to Excel" />
                <script type="text/javascript">
                    setupCalendars();
                </script>

                <asp:Repeater ID="rptExpanded" runat="server">
                    <HeaderTemplate>
                        <img src="http://app.callcriteria.com/images/cc_logo_white_bg.JPG" />
                        <br /><br /><br /> <%=Now %><br />

                       
                        <table style="border:1px solid black;">
                            <thead style="font-weight:bold;">
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Play Call</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Reviewer</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Comments</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Sup Ack</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Total Score</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Call Date</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Call Length</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Review Date</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Campaign</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Agent Group</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Agent</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Phone</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Number Missed</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Missed</td>
                                <td style="border:1px solid #DDDDDD;font-weight:bold;">Profile ID</td>
                            </thead>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="border:1px solid #DDDDDD;"><a href='http://app.callcriteria.com/review_record.aspx?ID=<%#Eval("ID")%>'>Play</a></a></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Reviewer")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Comments")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Sup Ack")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Total Score")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Call Date")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Call Length")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Review Date")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Campaign")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Agent Group")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Agent")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Phone")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Number Missed")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Missed")%></td>
                            <td style="border:1px solid #DDDDDD;"><%#Eval("Profile ID")%></td>
                        </tr>
                    </ItemTemplate>
                     <AlternatingItemTemplate>
                        <tr style="background-color:#fafbfb;">
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><a href='http://app.callcriteria.com/review_record.aspx?ID=<%#Eval("ID")%>'>Play</a></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Reviewer")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Comments")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Sup Ack")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Total Score")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Call Date")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Call Length")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Review Date")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Campaign")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Agent Group")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Agent")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Phone")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Number Missed")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Missed")%></td>
                            <td style="border:1px solid #DDDDDD;background-color:#fafbfb;"><%#Eval("Profile ID")%></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

            </div>
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
                    <button runat="server" title="Agree" onserverclick="btnAgree_Click" style="background-color: green;"><i class="fa fa-check"></i></button>
                    <button runat="server" title="Disagree" onserverclick="btnDisagree_Click" style="background-color: red;"><i class="fa fa-xing-square"></i></button>
                    <%--<button runat="server" title="Refer" onserverclick="btnRefer_Click" style="background-color: yellow;"><i class="fa fa-share"></i></button>--%>
                    <br />
                    <asp:TextBox ID="txtComments" placeholder="Commments...." TextMode="MultiLine" Columns="30" Rows="3" runat="server"></asp:TextBox>
                    <div>
                    </div>

                </div>
                <!-- close player-agent -->

            </div>
            <!-- close audio-player-inner-content -->
        </div>

        <asp:HiddenField ID="hdnFormID" runat="server" />

    </section>
</asp:Content>
