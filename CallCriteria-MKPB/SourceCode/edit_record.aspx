﻿<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="edit_record.aspx.vb" Inherits="ER2" %>

<%@ Register Src="~/controls/Notifications.ascx" TagPrefix="UC1" TagName="Notifications" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/controls/five9Sesssion.ascx" TagPrefix="UC1" TagName="five9Sesssion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="review/review_record.css" type="text/css" />
    <script type="text/javascript" src="review/review_record.js"></script>

    <script type="text/javascript">

        function show_options(answer_id) {
            $.ajax({
                type: "POST",
                url: "edit_record.aspx/GetAnswerOptions",
                data: '{"id": "' + answer_id + '" }',
                //data: { id: id}, 
                //data: '{id: "' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $('#expand-window h1').text("Update Comments");
                    $('#expand-window div[class=expand-popup-body]').html(msg.d);
                    $('#expand-window').width($(window).width() * .75 + 'px')
                    $('#expand-window').height($(window).height() * .75 + 'px')
                    $('div[class=expand-popup-body]').height($('#expand-window').height() * .75 - 85 + 'px')
                    $('#expand-window').height(' += 50')
                    $('div[class=expand-popup-body] table').css({ width: '100%', height: '100%' })
                    $('div[class=expand-popup-body] table td').css({ 'border-right': 'solid 1px #f3f4f4', padding: '3px' })
                    $('div[class=expand-popup-body] table tr:nth-child(even)').css('background-color', '#fafbfb')
                    $('div[class=expand-popup-body] table tr:nth-child(odd)').css('background-color', '#ffffff')
                    $('#expand-window').draggable()

                    $('#expand-window .popup-comments span').removeAttr('style')

                    $('#expand-window').show()


                    $('#update-comments').unbind("click");

                    $('#update-comments').click(function () {

                        var check_list = '';
                        var id_list = '';
                        $('.expand-popup-body input[type=checkbox]').each(function () {

                            check_list += $(this).val() + ':' + this.checked + '|';
                            id_list += $(this).data("existingid") + '|';
                        });

                        $.ajax({
                            type: "POST",
                            url: "edit_record.aspx/UpdateQAComments",
                            data: '{id: "' + answer_id + '",check_list: "' + check_list + '",id_list: "' + id_list + '", optional_text: "' + $('#other-text-comment').val() + '", website_link: "' + $('#website-link').val() + '" }',
                            //data: { id: id},
                            //data: '{id: "' + id + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                console.log(msg.d);
                                $("span[data-fsid=" + answer_id + "]").html(msg.d);
                                //$(this).parent().find("comment_holder").html(msg.d);
                                $('#expand-window').hide();
                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        });

                    });
                },
                failure: function (response) {
                    alert(response.d);
                }
            });


        }

        function process(ddl, qid, name, formid) {
            var id = formid
            var answer = $(ddl).val();

            $.ajax({
                type: "POST",
                url: "edit_record.aspx/UpdateQAAnswers",
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


    <div class="expand-container" id="expand-window" style="display: none;">
        <header class="popup-header">
            <h1></h1>

            <a id="expand-window-close" class="close-popup"><i class="fa fa-times"></i></a>
        </header>
        <!-- close popup-header -->
        <div class="expand-popup-body">
        </div>
        <!-- close popup-body -->
        <div id="expand-popup-footer" class="popup-footer">
            <div class="actions-in-right">
                <a id="update-comments" class="third-priority-buttom">Update</a>
                <a id="close-expand-window" class="close-popup third-priority-buttom">Cancel</a>
            </div>
            <!-- close actions-in-right -->
        </div>
        <!-- close popup-footer -->
    </div>




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


        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnIsWebsite" runat="server" />
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

        

                <asp:Panel ID="pnlCall" CssClass="call-info" runat="server">
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
                                        <td class="info-label">QA</td>
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
                    </asp:Panel>

                    <asp:Panel ID="pnlWeb" Visible="false" CssClass="call-info" runat="server">
                <asp:Literal ID="litWebsite" runat="server"></asp:Literal>
            </asp:Panel>



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
                                        <span id="comment_holder" data-fsid="<%#Eval("fs_id") %>" style="display: initial; font-weight: normal; margin-right: 0px;">
                                            <asp:Repeater ID="rptComment" runat="server" DataSourceID="dsComments">
                                                <ItemTemplate>
                                                    <%#Replace(Eval("comment").ToString, Chr(13), "<br>")%>&nbsp;
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <%--SelectCommand="select distinct isnull(comment, other_answer_text) as comment from form_q_responses 
                                                left join answer_comments on answer_comments.id = form_q_responses.answer_id where form_id = @form_id 
                                                and form_q_responses.question_id = @QID">--%>

                                            <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                SelectCommand="getReviewRecordComments" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                                    <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <%#Eval("template_text")%>
                                        </span>
                                        <i class="fa fa-pencil edit-record-icon" onclick="show_options('<%#Eval("fs_id") %>');"></i>
                                    </td>
                                    <td class="response">
                                        <%--<%#Eval("answer_text")%>--%>
                                        <asp:DropDownList ID="ddlAnswers" 
                                            DataSourceID="dsQAnswers" DataTextField="answer_text" DataValueField="answer_text" Enabled='<%#Eval("enabled") %>' SelectedValue='<%#Eval("answer_text")%>' runat="server">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsQAnswers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select id, answer_text from question_answers  with (nolock) where question_id = @QID order by right_answer desc"
                                            runat="server">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td><%#Eval("answer_points")%></td>
                                    <td class="td-play">
                                       <div style="position:relative;">
                                        <img src='<%#Eval("view_link")%>' class="resize" />
                                    </div>
                                     <button type="button" class="<%#Eval("btn_class")%>" onclick="jumpPos(<%#Eval("q_position")%>);return false;">
                                        <%#Eval("play_object")%>
                                    </button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>

                                <%--                 SelectCommand=" select  isnull(comment,fs.other_answer_text) as comment, fs.id as fs_id, [dbo].getTemplateText(@form_id , q.ID, 1) as template_text,
                                qa2.answer_text as qa_orig_answer,  
                                case when view_link is null and website is not null then 'hideme' else '' end as btn_class,
                                case when view_link is not null then '<span onmouseover=''show_window($(this));''><i class=''fa fa-picture-o''></i></span>' else '<div></div>' end as play_object,
                                *, case when qa.right_answer = 1  then 'Right' else 'Wrong' end as real_result, 
                                case when qa.right_answer = 0 or qa.ID != qa2.ID  then 'bad-response' else '' end as [bad-response], data_section=@data_section
                                 from dbo.form_q_scores fs with (nolock) 
                                  join vwForm with (nolock) on vwForm.f_id = fs.form_id
                                  left join answer_comments with (nolock)  on answer_comments.id = fs.answer_comment
                                  join [Questions] q with (nolock)  on fs.question_ID = q.ID
                            join sections s with (nolock)  on s.id = q.section       
                            left join question_answers qa with (nolock)  on qa.ID = question_answered 
                            left join question_answers qa2 with (nolock)  on qa2.ID = original_question_answered 
                                    where form_ID = @form_id  and s.id = @section and template not in ('Preferences')
                                    order by section_order,  q_order"--%>


                        <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            selectcommand="getReviewRecord" SelectCommandType="StoredProcedure" runat="server">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnSectionID" Name="Section" />
                                <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                <asp:ControlParameter ControlID="hdnDataSection" Name="data_section" />
                                <%--<asp:ControlParameter ControlID="hdnThisApp" Name="appname" />--%>
                              <%--  <asp:SessionParameter SessionField="appname" Name="appname" />
                                <asp:ControlParameter ControlID="hdnThisAgent" Name="agent" DefaultValue="Rosvia" />--%>
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
                                        <button type="button"  class='<%#Eval("btn_class") %>'  onclick="jumpPos(<%#Eval("click_pos")%>);return false;">
                                            <div></div>
                                        </button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="select ROW_NUMBER() OVER(ORDER BY comment_date desc)  + 100 AS data_section,* from (
	                        select comment_header, comment, comment_who, comment_date, 
                            (select top 1 case when website is not null then 'hideme' else '' end from vwForm where f_id = @form_id) as btn_class,
                            substring(comment_pos,1,CHARINDEX(':', comment_pos) - 1) * 60 + substring(comment_pos,CHARINDEX(':', comment_pos) + 1, 1000) as click_pos 
                            from system_comments with (nolock)  where comment_id = @form_id) a
							order by comment_date">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnThisID" Name="form_id" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        </table>
                    </FooterTemplate>
                </asp:Repeater>


                <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="getSections2" SelectCommandType="StoredProcedure" runat="server">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
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
                        SelectCommand="SELECT isnull(isnull(edited_score, calib_score),total_score) as real_score, *, userextrainfo.first_name as rev_first, userextrainfo.last_name as rev_last  from vwForm with (nolock)  join app_settings with (nolock)  on app_settings.appname = vwForm.appname join userextrainfo with (nolock)  on userextrainfo.username = reviewer where f_id = @ID"
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
       



    </section>

    <script>

        if ($('#ContentPlaceHolder1_hdnIsWebsite').val() == '') {
            setupAudioPlayer('<%=audio_file %>', 0.75, '<%=play_option%>');

        }
        else {
            $('.audio-player').hide();
        }




        $(document).ready(function () {
            $('#pnlHeader').hide();

        });
    </script>
</asp:Content>

