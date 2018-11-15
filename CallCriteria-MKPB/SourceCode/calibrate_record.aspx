<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="calibrate_record.aspx.vb" Inherits="ER2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="review/review_record.css" type="text/css" />
    <script type="text/javascript" src="review/review_record.js"></script>

     <script>
            function updateTimeStamp (ddl){
                $(ddl).parent().find("input[name*='hdnQAnswer']").val($(ddl).val());
                
                switch ($(ddl).find(':selected').text()) {
                    case 'Yes':
                        $(ddl).css('background-color', 'green');
                        break;
                    case 'No':
                        $(ddl).css('background-color', 'red');
                        break;
                    case 'NA':
                        $(ddl).css('background-color', 'yellow');
                        break;
                    default:
                        $(ddl).css('background-color', 'white');
                }
                $(ddl).find('option').css('background-color', 'white');

                audio = $('.audio-player audio').get(0);
                $(ddl).parent().find("input[name*='hdnQTimestamp']").val(audio.currentTime);
            }
           
        </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
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


            function updateTime(ddl) {
                //var time_id = $(ddl).attr('id').replace('ddlAnswers', 'hdnUpdateTime');
                ////console.log(time_id);
                //audio = $('.audio-player audio').get(0);
                //$('#' + time_id).val(audio.currentTime);
                //console.log(audio.currentTime);
                //console.log($('#' + time_id).val());

                $(ddl).parent().find("input[name*='hdnQAnswer']").val($(ddl).val());

                switch ($(ddl).find(':selected').text()) {
                    case 'Yes':
                        $(ddl).css('background-color', 'green');
                        break;
                    case 'No':
                        $(ddl).css('background-color', 'red');
                        break;
                    case 'NA':
                        $(ddl).css('background-color', 'yellow');
                        break;
                    default:
                        $(ddl).css('background-color', 'white');
                }
                $(ddl).find('option').css('background-color', 'white');

                audio = $('.audio-player audio').get(0);
                $(ddl).parent().find("input[name*='hdnQTimestamp']").val(audio.currentTime);

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

        <asp:HiddenField ID="hdnThisFormID" runat="server" />
        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnThisAgent" runat="server" />
        <asp:HiddenField ID="hdnCallLength" runat="server" />
        <asp:HiddenField ID="hdnCallLength2" runat="server" />
        <asp:HiddenField ID="hdnSpeedLimit" Value="2.2" runat="server" />
        <asp:HiddenField ID="hdnFilter" runat="server" />
        <asp:HiddenField ID="hdnThisApp" runat="server" />
        <asp:HiddenField ID="hdnStartTime" runat="server" />
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

            <style>
                .score {
                    display: none;
                }
            </style>

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
            </div>

            <div class="call-table">

                <asp:Repeater ID="rptSections" DataSourceID="dsSections" runat="server">
                    <HeaderTemplate>

                        <table id="ER-table" cellpadding="0" cellspacing="0">
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
                                <tr class="data-row" data-section="<%#Eval("data_section")%>">
                                    <td class="emptyBox">
                                        <div class="rowShadow rowShadow1"></div>
                                        <div class="rowShadow rowShadow2"></div>
                                    </td>
                                    <td class="question"><strong><%#Eval("q_short_name")%>: </strong><%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                                        <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id")%>' />
                                        <%#Eval("template_text")%>
                                    </td>
                                    <td class="response">
                                        <asp:DropDownList ID="ddlAnswers" ondatabound="ddlAnswer_DataBinding" 
                                            DataSourceID="dsQAnswers" DataTextField="answer_text" AppendDataBoundItems="true" DataValueField="id" runat="server">
                                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnUpdateTime" runat="server" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlAnswers" runat="server" Text="*" ForeColor="red" ErrorMessage=""></asp:RequiredFieldValidator>
                                        <asp:SqlDataSource ID="dsQAnswers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select id, answer_text from question_answers where question_id = @QID order by right_answer desc"
                                            runat="server">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td></td>
                                    <td class="td-play"></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand=" select form_id, isnull(comment,fs.other_answer_text) as comment, [dbo].getTemplateText(@form_id , q.ID, 1) as template_text,
                                *, data_section=@data_section
                                 from dbo.form_q_scores fs
                                  left join answer_comments on answer_comments.id = fs.answer_comment
                                  join [Questions] q on fs.question_ID = q.ID
                                  join sections s on s.id = q.section
                                    where form_ID = @form_id  and s.id = @section and template not in ('Schools','Preferences')
                                    order by section_order,  q_order"
                            runat="server">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnSectionID" Name="Section" />
                                <asp:ControlParameter ControlID="hdnThisFormID" Name="form_ID" />
                                <asp:ControlParameter ControlID="hdnDataSection" Name="data_section" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <tr class="section-space" data-section="<%#Eval("Row")%>">
                            <td></td>
                        </tr>

                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
                <br />
                <h2>Comments</h2>
                <asp:TextBox placeholder="Enter your comments here..." Rows="5" Columns="100" ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                <br />

                <asp:CheckBox ID="chkStopWorking" runat="server" />
                <label for="doneworking">I'm done Working</label>
                <asp:Button ID="btnSaveSession" CssClass="main-cta close-popup" runat="server" Text="SAVE" />
                <asp:Button ID="btnSkip" CausesValidation="false" CssClass="main-cta close-popup" runat="server" Text="SKIP" />
                <asp:DropDownList ID="ddlSkip" runat="server">
                    <asp:ListItem>Merge Issue</asp:ListItem>
                    <asp:ListItem>Duplicate</asp:ListItem>
                    <asp:ListItem>Audio Not Playing</asp:ListItem>
                </asp:DropDownList>


                <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="getSections2" SelectCommandType="StoredProcedure" runat="server">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnThisFormID" Name="form_ID" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <div style="text-align: left;">
                    <asp:FormView ID="fvFORMData" DataSourceID="dsFormData" runat="server" CssClass="FormData">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnXCCID" runat="server" />
                            <asp:HiddenField ID="theXID" runat="server" Value='<%#Eval("review_id")%>' />

                            </div>
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:SqlDataSource ID="dsFormData" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT * from vwForm join app_settings on app_settings.appname = vwForm.appname where f_id =  @ID"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnThisFormID" Name="ID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
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


