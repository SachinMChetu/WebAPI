<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="dms_nb_custom.aspx.vb" Inherits="dms_nb_custom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="main-container dash-modules general-button">

        <h2>DMS Non-billable Fix</h2>

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
                                and id in (select distinct scorecard_id from answer_comments join questions on questions.ID = question_id where answer_comments.non_billable = 1 or special2 = 1)
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

            Rows:
            <asp:Label ID="lblRows" runat="server" Text=""></asp:Label>

            <asp:Button ID="btnExport" runat="server" Text="Export" />
            <asp:GridView ID="gvNB" DataSourceID="dsNB" CssClass="detailsTable" EnableViewState="false" runat="server" AutoGenerateColumns="false">
                <Columns>

                    <asp:HyperLinkField DataTextField="f_id" DataNavigateUrlFields="f_id" DataNavigateUrlFormatString="http://qc.thedmsgrp.com/review_record.aspx?ID={0}" />
                    <%--<asp:BoundField DataField="audio_link,f_id" HeaderText="Audio" SortExpression="audio_link"  DataFormatString=""  />--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <span style="cursor:pointer;" onclick="javascript:show_audio('<%#Eval("audio_link") %>',0,<%#Eval("f_id") %>)">Listen</span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="call_date" HeaderText="Call Date" SortExpression="call_date" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                    <asp:BoundField DataField="missed_list" HeaderText="Missed List" SortExpression="missed_list" />
                    <%--<asp:BoundField DataField="missed_reason" HeaderText="NB Reason" SortExpression="missed_reason" />--%>
                    <asp:BoundField DataField="question_id" HeaderText="Question" SortExpression="question_id" />
                    <asp:BoundField DataField="formatted_comments_clean" HeaderText="Comments" SortExpression="formatted_comments_clean" HtmlEncode="false" />
                    <asp:TemplateField >
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id") %>' />
                            <asp:HiddenField ID="hdnFID" runat="server" Value='<%#Eval("f_id") %>' />
                            <asp:DropDownList ID="ddlComments" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlComments_SelectedIndexChanged" AutoPostBack="true" DataSourceID="dsComments" DataTextField="comment" DataValueField="ID" runat="server">
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                <asp:ListItem Text="Not Non-billable" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsComments"  ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                                SelectCommand="select * from answer_comments where question_id = @QID and non_billable = 1">
                                <SelectParameters>
                                   <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="review_date" HeaderText="Review Date" SortExpression="review_date" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField DataField="real_score" HeaderText="Score" SortExpression="real_score" />
                    <asp:BoundField DataField="agent_group" HeaderText="Group" SortExpression="agent_group" />

                </Columns>
            </asp:GridView>
        <asp:SqlDataSource ID="dsNB" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select f_id, agent,call_date, phone, audio_link, missed_list, review_date, agent_group, question_id, replace(replace(formatted_comments,'|',''),'<br>', '') as formatted_comments_clean, isnull(edited_score,  total_score) as real_score 
                from vwForm with (nolock) join
				(select form_id, question_id from form_q_responses where form_id in (select f_id from vwForm with (nolock) where call_date between @date1 and @date2 and scorecard = @scorecard and calib_score is null)
                and question_id in (select question_id from answer_comments where non_billable = 1) and other_answer_text is not null) a on a.form_id = f_id 
                where f_id not in (select distinct form_id from form_q_responses with (nolock)  join answer_comments with (nolock)  on answer_comments.id = form_q_responses.answer_id where isnull(non_billable,0) = 1 )
                and (sort_order is null or sort_order >= 0)" >
               <SelectParameters>
                    <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                    <asp:ControlParameter ControlID="date1" Name="date1" />
                    <asp:ControlParameter ControlID="date2" Name="date2" />
                </SelectParameters>
        </asp:SqlDataSource>


        

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
                            <a href="#" title="-" data-rate="-10">-</a>
                            <a href="#" title="Normal speed" data-rate="0">0</a>
                            <a href="#" title="+" data-rate="10">+</a>
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

                });


                //getTopMissed();

                //getDetailsQS(0);


            });

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


        </script>

</section>
</asp:Content>

