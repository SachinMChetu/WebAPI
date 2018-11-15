﻿<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="listen_training.aspx.vb" Inherits="listen3" %>

<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="local" %>
<%@ Register Src="~/controls/five9Sesssion.ascx" TagPrefix="UC1" TagName="five9Sesssion" %>
<%@ Register Src="~/Controls/TopDataBlock.ascx" TagPrefix="UC1" TagName="TopDataBlock" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<link rel="stylesheet" href="/listen/css/style.css" type="text/css" />--%>
    <link rel="stylesheet" href="/listen/css/listen2.css?1007" type="text/css" />
    <script type="text/javascript" src="/listen/scripts/listen2.js?1011"></script>


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
                setupAudioPlayer('<%=audio_file%>', 0.75);
                audio = $('.audio-player audio').get(0);
                alert('audio reloaded');
            }
            audio.currentTime = to_time;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">


         <div id="show-faqs"></div>

        <asp:Label ID="lblNumberScores" Visible="false" runat="server"></asp:Label>
        <asp:Label ID="lblAvgScore" Visible="false" runat="server"></asp:Label>


        <asp:HiddenField ID="hdnSpeedLimit" runat="server" />
        <asp:HiddenField ID="hdnAutoSubmit" runat="server" />
        <asp:HiddenField ID="hdnXCCID" runat="server" />
        <asp:HiddenField ID="hdnScorecard" runat="server" />
        <asp:HiddenField ID="hdnThisApp" runat="server" />
        <asp:HiddenField ID="lblSession" runat="server" />
        <%--<asp:HiddenField ID="hdnCampaign" runat="server" />--%>

        <asp:HiddenField ID="hdnCalibID" runat="server" />

        <div id="call-info-float">
            <div class="card-wrappers">
                <!-- Added with Javascript.  Imports cards from the first Call Info section. -->
            </div>
        </div>
        <div id="listenPage">

            <table class="page-header">
                <tr>
                    <td class="pageName">
                        <asp:Literal ID="lblThisApp"  enableviewstate="false"  runat="server"></asp:Literal></td>
                    <td class="pin-search"><i class="fa fa-search"></i>
                        <input type="text" value="" /></td>
                </tr>
            </table>


            <UC1:TopDataBlock runat="server" id="TopDataBlock" />


            <%--<div class="call-info call-info1">
                <table class="call-info-header alt-gray">
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
                                <table class="alt-gray">
                                    <asp:Literal ID="litPersonal" runat="server"></asp:Literal>
                                </table>
                            </div>
                        </td>
                        <td class="rec-info-col rec-info-col2">
                            <div class="info-card">
                                <span class="card-name">Schools</span>
                                <table class="schools alt-gray">
                                    <asp:Literal ID="litSchool" runat="server"></asp:Literal> 
                                </table>
                            </div>

                             <div id="divOtherCard" visible="false" runat="server" class="info-card">
                                <span class="card-name">Other Data</span>
                                <table class="alt-gray">
                                    <asp:Literal ID="litOtherData" runat="server"></asp:Literal>
                                </table>
                            </div>


                        </td>
                        <td class="rec-info-col rec-info-extra">&nbsp;</td>
                    </tr>
                </table>
            </div>--%>

            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" />--%>

            <asp:Repeater ID="rptSections" OnItemDataBound="CheckForPanel" DataSourceID="dsSections"
                runat="server">
                <HeaderTemplate>
                    <div class="pin-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="pin-options-overlay"></div>
                    <div class="category-title">
                        <asp:Literal ID="Label1"  enableviewstate="false"  runat="server" Text='<%#Eval("Section") %>'></asp:Literal>
                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                    </div>
                    <div class="category-policy">
                        <asp:Literal ID="Literal1" enableviewstate="false" runat="server" Text='<%#Eval("descrip")%>'></asp:Literal>
                    </div>
                    <asp:Repeater ID="DataList1" DataSourceID="dsQuestions"
                        runat="server">
                        <HeaderTemplate>
                            <div class="category-pins">
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:HiddenField ID="hdnQID" Value='<%#Eval("ID") %>' runat="server" />
                            <asp:HiddenField ID="hdnCount" runat="server" />
                            <asp:HiddenField ID="hdnHasOptions" runat="server" Value='<%#Eval("has_options")%>' />

                            <div class="pin-box  <%#Eval("has_options")%>  " data-pb-qid='<%# Eval("ID")%>'  data-pb-lq='<%# Eval("linked_question")%>' data-pb-la='<%# Eval("linked_answer")%>' 
                                data-pb-lc='<%# Eval("linked_comment")%>'>

                                <div class="qa-float">
                                    <asp:Repeater ID="rptFAQs" runat="server" DataSourceID="dsFAQs">
                                        <ItemTemplate>
                                            <div class="faq_q"><%#Eval("question_text") %></div> 
                                            <div class="faq_a">
                                                <%#Eval("question_answer")%><br />
                                                <br />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:SqlDataSource ID="dsFAQS" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                        SelectCommand="select * from q_faqs where question_id = @QID order by q_order" runat="server">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="notes-tab">
                                    <div></div>
                                </div>
                                <div class="circle circle1 <%#Eval("class")%>">
                                    <div class="circleCheck">&#x2714</div>
                                    <div class="circleX">&times;</div>
                                    <div class="circleNA"><i class="fa fa-minus-circle"></i></div>
                                    <!-- Can fill in name with pin name or whatever you need to -->
                                    <local:HiddenField2 ID="hdnWasChosen" runat="server" Value='<%#Eval("default_value")%>' />
                                </div>
                                <span class="pin-name"><%# Eval("q_short")%><asp:RequiredFieldValidator ID="RequiredFieldValidator1" Foylor="Red" ControlToValidate="hdnWasChosen" runat="server" Text="*">
                                </asp:RequiredFieldValidator></span>

                            </div>

                            <div class="pin-options <%#Eval("has_options")%>">
                                <!-- non template answers -->
                                <asp:Repeater ID="rptAnswers" OnItemDataBound="rptAnswers_ItemDataBound" runat="server" DataSourceID="dsAnswers">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnAnswerID" runat="server" Value='<%#Eval("ID")%>' />
                                        <div class="<%#Eval("show_pin") %>" data-qid="<%#Eval("question_id")%>" data-ansorder="<%#Eval("ans_order")%>" data-anstype="<%#Eval("div_class")%>" data-pinans="<%#Eval("ID")%>">
                                            <div class="pin-time">
                                                <span style="float: left; font-weight: bold;"><%#Eval("answer_text")%></span> Pin placed @
                                            <asp:TextBox ID="txtClickTime" runat="server" OnDataBinding="txtClickTime_DataBinding" Text='<%#Eval("default_value")%>'></asp:TextBox>
                                            </div>
                                            <div class="pin-options-select">
                                                <%--<div class="pin-option">--%>
                                                <%--<asp:RadioButtonList ID="rblOptions" OnDataBound="rblOptions_DataBound" RepeatLayout="Flow" DataSourceID="dsResponseOptions" DataValueField="ID" DataTextField="Comment" runat="server"></asp:RadioButtonList>--%>
                                                <asp:CheckBoxList runat="server" ID="chkOptions" OnDataBound="chkOptions_DataBound" RepeatLayout="Flow" DataSourceID="dsResponseOptions" DataValueField="ID" DataTextField="Comment"></asp:CheckBoxList>
                                                <%--</div>--%>
                                                <asp:SqlDataSource ID="dsResponseOptions" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                    SelectCommand="SELECT * FROM [answer_comments] where answer_id = @ANS_ID  order by isnull(ac_order,99)">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hdnAnswerID" Name="ANS_ID" DefaultValue="1" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>  
                                                <br />
                                                <asp:CheckBox ID="chkComment" runat="server" />
                                                <asp:TextBox ID="txtOtherComment" CssClass="offset-listen-textbox" TextMode="MultiLine" runat="server"></asp:TextBox><br />
                                                <asp:CheckBox ID="chkWebsite" runat="server" />
                                                <asp:TextBox ID="txtWebsiteLink" CssClass="offset-listen-textbox" placeholder="Website...." TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:SqlDataSource ID="dsAnswers" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select *, case  when right_answer = 1 and answer_text <> 'NA'  then 'Yes' when right_answer=1 and answer_text = 'NA' 
                                    then 'NA' else 'No' end as div_class,
                                    row_number() over (order by right_answer desc) as ans_order, 
                                    
                                    case when autoselect = 1 then 'showpin' else 'hidepin' end as show_pin,
                                    case when autoselect = 1 then '0:00' else '' end as default_value from question_answers where question_id = @qid order by right_answer desc">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" DefaultValue="1" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <!-- contact template answers -->
                                <asp:Repeater ID="rptContactList" runat="server" DataSourceID="dsContactList">
                                    <HeaderTemplate>
                                        <div class="pin-options-sub hidepin">
                                            <div class="pin-sub-items">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <span>
                                                <asp:CheckBox ID="chkOption" Text='<%#Eval("Value").ToString%>' OnDataBinding="chkOption_DataBinding" runat="server" />
                                                <asp:HiddenField ID="hdnOrigId" Value='<%#Eval("orig_id")%>' runat="server" />
                                                <span class="pin-time">@
                                                 <asp:TextBox ID="txtCheckTime" runat="server" OnDataBinding="txtCheckTime_DataBinding"></asp:TextBox>
                                                </span>
                                            </span>
                                        </span>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <span>
                                            <span>
                                                <asp:CheckBox ID="chkOtherList" Text='' runat="server" />
                                                <asp:TextBox ID="txtOtherList" TextMode="MultiLine" runat="server" Style="position: relative; top: 10px;"></asp:TextBox>
                                            </span></span>
                                        <div class="pin-options-select">
                                            <div class="pin-option">
                                                <input type="radio" name="ContactInfo" value="" runat="server" id="ContactInfo" />

                                                <asp:Label ID="CIInfo" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        </div>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:SqlDataSource ID="dsContactList" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select 0 as orig_id,* from [dbo].[String.Split]((select top 1 template_items from question_answers 
                                    join questions on questions.id = question_id where question_id = @qid),'|') join questions on questions.id = @qid
                                    where value != ''
                                    union all
                                    select * from dbo.[GetSchoolData](@xcc_id) join questions on questions.id = @qid and template = 'Schools' "> 
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" DefaultValue="1" />
                                        <asp:ControlParameter ControlID="hdnXCCID" Name="xcc_id" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                              


                            </div>
                        </ItemTemplate>

                        <FooterTemplate></div></FooterTemplate>
                    </asp:Repeater>
                    <asp:SqlDataSource ID="dsQuestions_old" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" CancelSelectOnNullParameter="false" 
                        SelectCommand="SELECT left(q_short_name, 30) as q_short,*, case when template != '' then 'has-sub-items' else '' end as has_options,
                                case when class is not null then 'circleYes' else '' end as circleYes, class,
                                case when class is not null then '0:00' else '' end as default_value, default_class
                                FROM [Questions] q  with (nolock) join sections on sections.id  = q.section 
                                left join (select 'circle' + case when right_answer = 1 and answer_text in ('Yes','No')  then 'Yes' 
                                when answer_text not in ('Yes','No') then 'NA' else 'No' end as class, question_id, 
                                case when autoselect = 1 then 'show-options-' + lower(answer_text) else '' end as default_class from question_answers 
                                where question_id in (select id from questions with (nolock)  where scorecard_id = @scorecard_id) and autoselect = 1) b
                                on b.question_id = q.id
                                where 1 = case when @section != 0 and q.section != @section then 0 else 1 end
                                 and q.scorecard_id = @scorecard_id and q.active = 1
                                 and isnull(q_type,'') <> 'Calculated'
                                order by q_order"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblID" Name="Section" DefaultValue="1" />
                            <asp:ControlParameter ControlID="hdnScorecard" Name="scorecard_id" />
                            <asp:ControlParameter ControlID="hdnCalibID" Name="CalibID" />
                            <asp:ControlParameter ControlID="hdnCampaign" Name="campaign" DefaultValue="" />
                        </SelectParameters>
                    </asp:SqlDataSource>




                      <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" CancelSelectOnNullParameter="false"
                        SelectCommand="getListenQuestions" SelectCommandType="StoredProcedure"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblID" Name="Section" DefaultValue="1" />
                            <%--<asp:ControlParameter ControlID="hdnThisApp" Name="appname" DefaultValue="edsoup" />--%>
                            <asp:ControlParameter ControlID="hdnScorecard" Name="scorecard_id" DefaultValue="1" />
                            <%--<asp:ControlParameter ControlID="hdnCampaign" Name="campaign" DefaultValue="" />--%>
                            <asp:ControlParameter ControlID="hdnXCCID" Name="xcc_id" />
                        </SelectParameters>
                    </asp:SqlDataSource>


                </ItemTemplate>
            </asp:Repeater>

            <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select distinct case when sectionless = 1 then 0 else sections.ID end as ID, sections.descrip,  case when sectionless = 1 then 'Questions' else sections.section end as section, 
				 case when sectionless = 1 then 1 else section_order end as section_order
                from sections with (nolock)  join questions with (nolock)  on questions.section = sections.id and questions.scorecard_id = sections.scorecard_id  
                join scorecards on scorecards.id = sections.scorecard_id
                where sections.scorecard_id = @scorecard_id and questions.active = 1 order by section_order"
                runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnScorecard" Name="scorecard_id" />
                </SelectParameters>
            </asp:SqlDataSource>

          

        <div class="listen-end-buttons">
            <asp:TextBox ID="txtComments" TextMode="MultiLine" runat="server" CssClass="comments-field"></asp:TextBox>
            <div class="save-buttons">
                <span class="check-submit">

                    <asp:Button ID="btnSaveSession" runat="server" Text="Save" CssClass="gradient" />
                </span>
            </div>

        </div>

        <!-- End page content -->



        <div class="audio-player" style="z-index: 10;">
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
                        <div class="slider-pins">
                        </div>
                        <div class="template-pins">
                            <div class='warning-part yellow-pin' style='left: 0%;' title=''><span></span><a href='#' class='listen-from-here' style=''>!</a></div>
                            <div class='warning-part green-pin' style='left: 0%;' title=''><span></span><a href='#' class='listen-from-here'>&#x2714</a></div>
                            <div class='warning-part red-pin' style='left: 0%;' title=''><span></span><a href='#' class='listen-from-here'>&times;</a></div>
                        </div>


                        <div class="slider-fill">
                        </div>
                    </div>
                    <!-- close slider -->
                </div>
                <!-- close player-timeline -->
                <div class="player-agent">
                    <a href="#" onclick="reload_audio();"><i class="fa fa-refresh" style="color: white;"></i></a>
                </div>
                <!-- close player-agent -->
            </div>
            <!-- close audio-player-inner-content -->
        </div>

       

         <script type="text/javascript">
            $(document).ready(function () {
                setupAudioPlayer('<%=audio_file %>', 0.75, 'True');
                $('[id*=txtOtherComment').css({ 'position': 'relative', 'top': '10px', 'left': '-3px' })
                //setupReview2();


                var timer;
                var delay = <%=myDelay * 1000%>;

                $('.pin-box').hover(function () {
                    // on mouse in, start a timeout

                    timer = setTimeout(function () {
                        $('#show-faqs').css({ display: 'block' });
                    }, delay);
                }, function () {
                    // on mouse out, cancel the timer
                    clearTimeout(timer);
                });



            });
        </script>





        <%-- <!-- close audio-player -->
        <script type="text/javascript">
            $(document).ready(function () {
                setupAudioPlayer('<%=audio_file%>', 0.75, 'True');
            });
        </script>--%>
    </section>
</asp:Content>

