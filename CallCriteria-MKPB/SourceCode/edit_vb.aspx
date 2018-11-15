<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_vb.aspx.vb" Inherits="edit_vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container">




        <script>
            function getTime(control_name) {
                $(control_name).val($('.player-timeline .section-label .audio-current-time').first().text());
            }

        </script>




        <table>
            <asp:Repeater ID="rptQuestions" DataSourceID="dsQuestions" runat="server">
                <ItemTemplate>
                    <tr>
                        <td colspan="10">
                            <br />&nbsp;
                            <br />&nbsp;
                            <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id")%>' />
                            <asp:Label ID="Label1" runat="server" Text='<%#eval("q_short_name") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <asp:Repeater ID="Repeater1" DataSourceID="dsAnswers" runat="server">
                            <ItemTemplate>

                                <td><asp:Label ID="Label2" runat="server" Text='<%#eval("val") %>'></asp:Label>
                                    <br />
                                    <asp:HiddenField ID="hdnExistingID" Value='<%#Eval("FQS_ID")%>' runat="server" />
                                    <asp:TextBox ID="TextBox13" onclick="getTime($(this));" Width="50" runat="server" Text='<%#eval("ts") %>'></asp:TextBox>
                                </td>


                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <asp:SqlDataSource ID="dsAnswers" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="select * from 
(select * from dbo.split('Very Negative|Somewhat Negative|Somewhat Positive|Very Positive|NA|Confusing|Frustrating|Difficult|Question|Task|Other|End','|')) a 
left join (select *, [dbo].[ConvertTimeToHHMMSS](option_pos,'s') as ts, form_q_scores_options.ID as FQS_ID  from form_q_scores_options where form_id = @form_id and question_id = @QID) b 
on a.val = b.option_value">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="ID" Name="form_id" />
                            <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                        </SelectParameters>

                    </asp:SqlDataSource>
                </ItemTemplate>

            </asp:Repeater>
            <asp:SqlDataSource ID="dsQuestions" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select distinct question_ID, q_short_name from form_q_scores_options join questions on questions.ID = form_q_scores_options.question_id where form_id = @form_id">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ID" Name="form_id" />
                </SelectParameters>

            </asp:SqlDataSource>

        </table>
        <asp:Button ID="btnUpdate" runat="server" Text="Update/Save" />
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>

        <div class="audio-player" id="notification_holder" style="display: block;">
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
                            <a href="#" title="-" data-rate="-2">-</a>
                            <a href="#" title="Normal speed" data-rate="0">0</a>
                            <a href="#" title="+" data-rate="2">+</a>
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
            setupAudioPlayer('<%=new_filename%>', 0.75, 'True');

        </script>

    </section>
</asp:Content>

