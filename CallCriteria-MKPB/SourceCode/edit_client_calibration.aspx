<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="edit_client_calibration.aspx.vb" Inherits="ER2" %>

<%@ Register Src="~/controls/Notifications.ascx" TagPrefix="UC1" TagName="Notifications" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/controls/five9Sesssion.ascx" TagPrefix="UC1" TagName="five9Sesssion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="review/review_record.css" type="text/css" />
    <script type="text/javascript" src="review/review_record.js"></script>

    <script type="text/javascript">
        function process(ddl, qid, name, formid) {
            var id = formid
            var answer = $(ddl).val();

            $.ajax({
                type: "POST",
                url: "edit_client_calibration.aspx/UpdateQAAnswers",
                data: '{id: "' + id + '",qid: "' + qid + '",name: "' + name + '",answer: "' + answer + '" }',
                //data: { id: id},
                //data: '{id: "' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $('.data2').html(msg.d.score);


                    $(ddl).closest('.data-row').removeClass("bad-response");
                    if (!msg.d.right_answer)
                        $(ddl).closest('.data-row').addClass("bad-response");

                    $('#ER-table tbody').append(msg.d.added_note);

                    $(".data2").animate({ fontSize: '30px' }, 500);
                    $(".data2").animate({ fontSize: '20px' }, 1000);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container dash-modules general-button">
        <script type="text/javascript">
            function getTimeStamp(control_name) {
                var ctl = document.getElementById(control_name);
                var player = document.getElementById("mediaplayer");
                var vlc = document.getElementById("vlc");
                var HTML5Audio = document.getElementById("HTML5Audio");
                pos = HTML5Audio.currentTime;

                if (pos > 3) pos -= 3;
                ctl.value = pos;
                document.getElementById("ContentPlaceHolder1_hdnTotalCallLength").value = HTML5Audio.duration;
            }

            function jumpPos(to_time) {
                audio = $('.audio-player audio').get(0);
                if (!audio) {
                    alert('reload audio');
                    setupAudioPlayer('<%=audio_file %>', 0.75);
                    audio = $('.audio-player audio').get(0);
                    alert('audio reloaded');
                }
                audio.currentTime = to_time;
            }

        </script>


        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnFormID" runat="server" />
        <asp:HiddenField ID="hdnThisAgent" runat="server" />
        <asp:HiddenField ID="hdnCallLength" runat="server" />
        <asp:HiddenField ID="hdnCallLength2" runat="server" />
        <asp:HiddenField ID="hdnSpeedLimit" Value="2.2" runat="server" />
        <asp:HiddenField ID="hdnFilter" runat="server" />
        <asp:HiddenField ID="hdnThisApp" runat="server" />

        <!-- Start page content -->
        <div id="call-info-float">

            <div class="card-wrappers">
                <!-- Added with Javascript.  Imports cards from the first Call Info section. -->
            </div>
        </div>
        <div id="review-record">
            <!-- <h1>Review Record</h1> -->

            <table class="page-header">
                <tr>
                    <td class="pageName" valign="top">
                        <asp:Literal ID="lblAppname" runat="server" Text=""></asp:Literal>


                    </td>
                    <td class="pageInfo">
                        <table>
                            <tr>
                                <td class="agentName"><span class="label">Agent:</span> <span class="data">
                                    <asp:Literal ID="litUserName" runat="server"></asp:Literal></span></td>
                            </tr>
                            <tr>
                                <td class="score"><span class="label">Score:</span> <span class="data"><span class="data2">
                                    <asp:Literal ID="litScore" runat="server"></asp:Literal></span>%</span></td>
                            </tr>
                            <tr>
                                <td class="missedQs">
                                    <span class="label" runat="server" id="spanMissed">Missed:</span>
                                    <asp:Literal ID="litJumpQ" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <div class="call-info call-info1">
                <table class="call-info-header">
                    <tr>
                        <td class="open-arrow"></td>
                        <td class="call-info-title">Call Info</td>
                        <td class="line">
                            <div></div>
                        </td>
                    </tr>
                </table>
                <table class="call-info-table">
                    <tr>
                        <td class="rec-info-col rec-info-col1">
                            <div class="info-card">
                                <span class="card-name">Contact Info</span>
                                <table>

                                    <asp:Literal ID="litPersonal" runat="server"></asp:Literal>


                                </table>
                            </div>
                            <div class="info-card">
                                <span class="card-name">Agent Info</span>
                                <table class="agent-info">
                                    <tr>
                                        <td class="info-label">Agent</td>
                                        <td class="info-data">
                                            <asp:Literal ID="litAgentName" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td class="info-label">Reviewer</td>
                                        <td class="info-data">
                                            <asp:Literal ID="litQAName" runat="server"></asp:Literal></td>
                                    </tr>
                                </table>
                            </div>

                            <div class="info-card">
                                <span class="card-name">Lead Preferences</span>
                                <table>

                                    <asp:Literal ID="litPrefs" runat="server"></asp:Literal>


                                </table>
                            </div>

                        </td>
                        <td class="rec-info-col rec-info-col2">
                            <div class="info-card" id="liSchoolItem" runat="server">
                                <span class="card-name">Schools</span>
                                <table>
                                    <asp:Literal ID="litSchool" runat="server"></asp:Literal>
                                </table>
                            </div>
                        </td>
                        <td class="rec-info-col rec-info-extra">&nbsp;</td>
                    </tr>
                </table>
            </div>

            <div class="call-table">

                <asp:Repeater ID="rptSections" DataSourceID="dsSections" runat="server">
                    <HeaderTemplate>

                        <table id="ER-table" cellpadding="0" cellspacing="0" class="detailsTable">
                            <tr class="table-header">
                                <th class="header-open-arrow"></th>
                                <th>Question</th>
                                <th class="th-response">Response</th>
                                <th class="th-weight">Weight</th>
                                <th class="th-play"></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <tr class="section-header" data-section="<%#Eval("Row")%>" unselectable="on" data-spaceheight="220" style="-webkit-user-select: none;">
                            <td class="open-arrow"></td>
                            <td colspan="3" class="section-name"><%#Eval("section")%>
                                <asp:HiddenField ID="hdnDataSection" runat="server" Value='<%#Eval("Row")%>' />
                                <div style="float: right; font-weight: bold;"><%#Eval("full_section_score")%></div>
                            </td>
                            <td class="error-indicator">
                                <div></div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="hdnSectionID" Value='<%#Eval("ID")%>' runat="server" />
                        <asp:Repeater ID="Repeater2" runat="server" DataSourceID="dsQuestions" OnItemDataBound="gvQuestions_RowDataBound">
                            <ItemTemplate>
                                <tr class="data-row <%#Eval("bad-response")%>" data-section="<%#Eval("data_section")%>">
                                    <td class="emptyBox">
                                        <div class="rowShadow rowShadow1"></div>
                                        <div class="rowShadow rowShadow2"></div>
                                    </td>
                                    <td class="question"><strong><%#Eval("q_short_name")%>: </strong><%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                                        <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id")%>' />
                                        <asp:Repeater ID="rptComment" runat="server" DataSourceID="dsComments">
                                            <ItemTemplate>
                                                <%#Replace(Eval("comment").ToString, Chr(13), "<br>")%>&nbsp;
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select distinct isnull(comment, other_answer_text) as comment from form_q_responses left join answer_comments on answer_comments.id = form_q_responses.answer_id where form_id = @form_id and form_q_responses.question_id = @QID">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <%#Eval("template_text")%>
                                    </td>
                                    <td class="response">
                                        <%--<%#Eval("answer_text")%>--%>
                                        <asp:DropDownList ID="ddlAnswers"
                                            DataSourceID="dsQAnswers" DataTextField="answer_text" DataValueField="answer_text" SelectedValue='<%#Eval("answer_text")%>' runat="server">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsQAnswers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select id, answer_text from question_answers where question_id = @QID order by right_answer desc"
                                            runat="server">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td><%#Eval("answer_points")%></td>
                                    <td class="td-play">
                                        <button onclick="jumpPos(<%#Eval("q_pos")%>);return false;">
                                            <div></div>
                                        </button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="                                 
                            select fs.form_id, isnull(comment,form_c_responses.other_answer_text) as comment, [dbo].getTemplateText(@form_id , q.ID, 1) as template_text,
                            *, case when right_answer = 1  then 'Right' else 'Wrong' end as real_result, 
                            case when right_answer = 0 then 'bad-response' else '' end as [bad-response], data_section=@data_section
                            from dbo.calibration_scores_client fs
                            join [Questions] q on fs.question_ID = q.ID
                            join question_answers qa on qa.ID = fs.question_result
                            join sections s on s.id = q.section 
                            left join form_c_responses on fs.form_id = form_c_responses.form_id and fs.question_id = form_c_responses.question_id
                            left join answer_comments on answer_comments.id = form_c_responses.answer_id

                            where fs.form_ID = @form_id   and s.id = @section  and template not in ('Schools','Preferences')
                            order by section_order,  q_order"
                            runat="server">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnSectionID" Name="Section" />
                                <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                <asp:ControlParameter ControlID="hdnDataSection" Name="data_section" />
                                <%--<asp:ControlParameter ControlID="hdnThisApp" Name="appname" />--%>
                                <asp:SessionParameter SessionField="appname" Name="appname" />
                                <asp:ControlParameter ControlID="hdnThisAgent" Name="agent" DefaultValue="Rosvia" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <tr class="section-space" data-section="<%#Eval("Row")%>">
                            <td></td>
                        </tr>

                    </ItemTemplate>
                    <FooterTemplate>

                        <asp:Repeater ID="rptComments" OnItemDataBound="rptComments_ItemDataBound" DataSourceID="dsComments" runat="server">
                            <HeaderTemplate>
                                <tr class="section-header" data-section="99" unselectable="on" data-spaceheight="220" style="-webkit-user-select: none;">
                                    <td class="open-arrow"></td>
                                    <td colspan="3" class="section-name">Comments
                                    </td>
                                    <td class="error-indicator">
                                        <div></div>
                                    </td>
                                </tr>

                            </HeaderTemplate>
                            <ItemTemplate>

                                <tr class="data-row" data-section="<%#Eval("data_section")%>">
                                    <td class="emptyBox">
                                        <div class="rowShadow rowShadow1"></div>
                                        <div class="rowShadow rowShadow2"></div>
                                    </td>
                                    <td class="question"><strong><%#Eval("comment_header")%></strong> <%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                                    </td>
                                    <td class="response"><%#Eval("comment_who")%></td>
                                    <td><%#Eval("comment_date")%></td>
                                    <td class="td-play">
                                        <button onclick="jumpPos(<%#Eval("click_pos")%>);return false;">
                                            <div></div>
                                        </button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="select ROW_NUMBER() OVER(ORDER BY comment_date desc)  + 100 AS data_section,* from (
	                        select comment_header, comment, comment_who, comment_date, 
                            substring(comment_pos,1,CHARINDEX(':', comment_pos) - 1) * 60 + substring(comment_pos,CHARINDEX(':', comment_pos) + 1, 1000) as click_pos 
                            from system_comments where comment_id = @form_id) a
							order by comment_date desc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnFormID" Name="form_id" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        </table>
                    </FooterTemplate>
                </asp:Repeater>


                <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="getSections2" SelectCommandType="StoredProcedure" runat="server">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnFormID" Name="form_ID" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <div style="text-align: left;">
                    <asp:FormView ID="fvFORMData" DataSourceID="dsFormData" runat="server" CssClass="FormData">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnXCCID" runat="server" />
                            <asp:HiddenField ID="theXID" runat="server" Value='<%#Eval("review_id")%>' />

                            <UC1:Notifications ID="NotificationControl" runat="server" FormID='<%# Eval("F_id")%>' />
                            <UC1:five9Sesssion ID="five9update" runat="server" record_ID='<%# Eval("review_id")%>' Visible="false" audio_player="fvtopFORMData"></UC1:five9Sesssion>
                            </div>
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:SqlDataSource ID="dsFormData" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT * from  calibration_form_client join vwForm on vwForm.f_id=calibration_form_client.original_form join app_settings on app_settings.appname = vwForm.appname where calibration_form_client.id = @ID"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnThisID" Name="ID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>


            <div class="call-info call-info2 super-collapsed">
                <table class="call-info-header closed-section">
                    <tr>
                        <td class="open-arrow"></td>
                        <td class="call-info-title">Other Information</td>
                        <td class="line">
                            <div></div>
                        </td>
                    </tr>
                </table>
                <table class="call-info-table" style="display: none;">
                    <tr>
                        <td class="rec-info-col rec-info-col1">
                            <div class="info-card">
                                <span class="card-name">Other Info</span>
                                <table>
                                    <asp:Literal ID="litOther" runat="server"></asp:Literal>
                                </table>
                            </div>
                            <div class="info-card">
                                <span class="card-name">Session Viewed</span>
                                <table>
                                    <asp:Literal ID="litsession" runat="server"></asp:Literal>
                                </table>
                            </div>
                        </td>
                        <td class="rec-info-col rec-info-col2">

                            <div class="info-card">
                                <span class="card-name">Download</span>
                                <span class="text-info">

                                    <asp:Button runat="server" Width="175" Visible="true" ID="btnDldAudio" Text="Download Audio"></asp:Button>
                                    <asp:Button runat="server" Width="175" Visible="true" ID="btnDldCall" Text="Download Call"></asp:Button>
                                    <asp:Button ID="hlEdit" Width="175" runat="server" Text="Edit Call"></asp:Button>
                                    <asp:Button runat="server" Width="175" Visible="true" ID="lbAddCalibration" Text="Add Calibration"></asp:Button>
                                    <asp:Button runat="server" Width="175" Visible="false" ID="lbAddDispute" Text="Add Dispute"></asp:Button>

                                </span>
                            </div>
                        </td>
                        <td class="rec-info-col rec-info-extra">&nbsp;</td>
                    </tr>
                </table>



            </div>
        </div>
        <!-- End page content -->


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
                            <a href="#" title="-" data-rate="-<%=data_rate%>">-</a> <a href="#" title="Normal speed"
                                data-rate="0">0</a> <a href="#" title="+" data-rate="<%=data_rate%>">+</a>
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
        <!-- close audio-player -->
        <script type="text/javascript">
            $(document).ready(function () {
                setupAudioPlayer('<%=audio_file %>', 0.75, '<%=play_option%>');

                //$("tr").each(function (index) {
                //    if ($(this).hasClass('closed-section')) {
                //        closeTableSection($(this));
                //    }
                //});

                //setupReview2();

            });
        </script>



    </section>

    <script>
        $(document).ready(function () {
            $('#pnlHeader').hide();
        });
    </script>
</asp:Content>


