<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="cali_listen.aspx.vb" ValidateRequest="false" Inherits="listen" %>

<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="local" %>
<%@ Register Src="~/controls/five9Sesssion.ascx" TagPrefix="UC1" TagName="five9Sesssion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/js/countdown.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"> 

    <section class="main-container listening-actions right-sidebar-showed">
        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnTotalCallLength" runat="server" />
        <asp:HiddenField ID="hdnAutoNotify" runat="server" />
        <asp:HiddenField ID="hdnXCCID" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnStartTime" runat="server" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" Visible="false" />

        <h2><i class="fa fa-compass"></i>Calibration Record -
                <asp:Literal ID="litLeft" runat="server"></asp:Literal>

        </h2>


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


        <div style="height: 90px;">&nbsp;</div>
        <script>
            var myCountdown1 = new Countdown({ time: <%=time_left%>, rangeHi  : "day",   // The highest unit of time to display
                rangeLo  : "second" });
        </script>
        <div class="listenint-section">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <asp:Repeater ID="rptSections" OnItemDataBound="CheckForPanel" DataSourceID="dsSections"
                runat="server">
                <ItemTemplate>
                    <h1><i class="fa fa-volume-up"></i>
                        <asp:Literal ID="Label1" runat="server" Text='<%#Eval("Section") %>'></asp:Literal></h1>
                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                    <div class="switch-actions">
                        <asp:Repeater OnItemDataBound="FixList" ID="DataList1" DataSourceID="dsQuestions"
                            runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <FooterTemplate></ul></FooterTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnQID" Value='<%#eval("ID") %>' runat="server" />
                                <asp:HiddenField ID="hdnSerious" Value='<%#eval("serious") %>' runat="server" />
                                <asp:HiddenField ID="hdnInverted" Value='<%#eval("inverted") %>' runat="server" />
                                <li>
                                        <label><%# Eval("q_short_name")%></label>
                                        <br />
                                        <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator1" ControlToValidate="ddlAnswer"
                                            runat="server" Text="*" ErrorMessage="Field Required"></asp:RequiredFieldValidator>

                                        <asp:DropDownList AppendDataBoundItems="true" onchange="updateTimeStamp(this);" ToolTip='<%# Eval("question")%>' ID="ddlAnswer" 
                                            DataSourceID="dsQuestionAnswers" OnDataBound="ddlAnswer_DataBinding"
                                            DataTextField="answer_text"  DataValueField="ID" runat="server">
                                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsQuestionAnswers" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select id, answer_text from question_answers where question_id = @QID">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <local:HiddenField2 ID="hdnQAnswer" runat="server" />
                                        <asp:HiddenField ID="hdnQTimestamp" Value="0" runat="server" />
                                       
                                </li>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT * FROM [Questions] q  join sections on sections.id  = q.section 
                                where q.section=@section and q.id in (select distinct question_id 
                                from form_q_scores where form_id = (select form_id from calibration_pending  where id = @form_id))
                                order by q_order"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblID" Name="Section" />
                            <asp:ControlParameter Name="form_id" ControlID="hdnThisID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <%-- <div>

                        <div>
                        </div>
                        <div class="yesNo">
                            <div class="rowElem">
                                <asp:RadioButtonList ID="RadioButtonList1" RepeatColumns="2" DataSourceID="dsAnswerList"
                                    DataTextField="answer_text" DataValueField="id" runat="server">
                                </asp:RadioButtonList>
                                <asp:SqlDataSource ID="dsAnswerList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    runat="server" SelectCommand="select answer_text,id,autoselect from question_answers where question_id = @question_id">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnQID" Name="question_id" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RadioButtonList1" 
                                Text="*" ErrorMessage="Make a selection"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <br />
                    </div>--%>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select * from sections where id in (select section from questions where id in (select distinct question_id 
                from form_q_scores where form_id = (select form_id from calibration_pending  where id = @form_id)))
                order by section_order"
                runat="server">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                    <asp:ControlParameter Name="form_id" ControlID="hdnThisID" />
                </SelectParameters>
            </asp:SqlDataSource>




            <div class="listenint-section">
                <h1><i class="fa fa-comment"></i>Comments</h1>

                <div class="listening-comments">
                    <asp:TextBox placeholder="Enter your comments here..." ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                <!-- close listening-comments -->

            </div>
            <!-- close listening-section -->


            <div class="actions-in-right listening-main-actions" style="margin-bottom:120px;" >
                <div class="checkbox-label">
                    <asp:CheckBox ID="chkStopWorking" runat="server" />
                    <label for="doneworking">I'm done Working</label>
                </div>
                <asp:Button ID="btnSaveSession" CssClass="main-cta close-popup" runat="server" Text="SAVE" />
                <asp:Button ID="btnSkip" CausesValidation="false" CssClass="main-cta close-popup" runat="server" Text="SKIP" />
                <asp:DropDownList ID="ddlSkip" runat="server">
                    <asp:ListItem>Merge Issue</asp:ListItem>
                    <asp:ListItem>Duplicate</asp:ListItem>
                    <asp:ListItem>Audio Not Playing</asp:ListItem>
                </asp:DropDownList>

            </div>
            <!-- close actions-in-right -->
        </div>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
    </section>
    <!-- close main-container -->

    <div class="right-sidebar">
        <div class="right-sidebar-header">
            <div class="show-hide-sidebar">
                <a href="#" class="hide-sidebar" title="Hide sidebar"><i class="fa fa-chevron-right"></i></a>
                <a href="#" class="show-sidebar" title="Show sidebar"><i class="fa fa-chevron-left"></i></a>
            </div>

            <img src="images/male-avatar.png" />
            <div class="agent-name-mail">
                <strong>
                    <asp:Literal ID="litUserName" runat="server"></asp:Literal></strong>
                <span>
                    <asp:Literal ID="litUserEmail" runat="server"></asp:Literal></span>
            </div>

        </div>
        <!-- close right-sidebar-header -->

        <div class="right-sidebar-content">
            <ul>
                <li class="expand-collapses collapsed">
                    <span class="sidebar-category-title">
                        <i class="fa fa-user"></i>
                        <strong>Personal Information</strong>
                        <a href="#"><i class="fa icon-expand-collapse"></i></a>
                    </span>
                    <!-- close sidebar-category-title -->
                    <ul class="expand-collapse-content">
                        <asp:Literal ID="litPersonal" runat="server"></asp:Literal>
                    </ul>
                </li>
                <!-- close expand-collapse -->

                <li class="expand-collapses">
                    <span class="sidebar-category-title">
                        <i class="fa fa-phone"></i>
                        <strong>Contact Information</strong>
                        <a href="#"><i class="fa icon-expand-collapse"></i></a>
                    </span>
                    <!-- close sidebar-category-title -->
                    <ul class="expand-collapse-content">
                        <asp:Literal ID="litContact" runat="server"></asp:Literal>
                    </ul>
                </li>
                <!-- close expand-collapse -->


                <li class="expand-collapses collapsed">
                    <span class="sidebar-category-title">
                        <i class="fa fa-bell"></i>
                        <strong>Schools</strong>
                        <a href="#"><i class="fa icon-expand-collapse"></i></a>
                    </span>
                    <!-- close sidebar-category-title -->
                    <ul class="expand-collapse-content">
                        <asp:Literal ID="litSchool" runat="server"></asp:Literal>
                    </ul>
                </li>
                <!-- close expand-collapse -->

                <li class="expand-collapses collapsed">
                    <span class="sidebar-category-title">
                        <i class="fa fa-align-justify"></i>
                        <strong>Other Information</strong>
                        <a href="#"><i class="fa icon-expand-collapse"></i></a>
                    </span>
                    <!-- close sidebar-category-title -->
                    <ul class="expand-collapse-content">
                        <asp:Literal ID="litOther" runat="server"></asp:Literal>
                    </ul>
                </li>
                <!-- close expand-collapse -->
            </ul>

            <div class="call-contact-information">
                <ul>
                    <li>
                        <i class="fa fa-tag"></i>
                        <div>
                            <span>Session ID</span>
                            <strong>
                                <asp:Literal ID="lblSession" runat="server"></asp:Literal></strong>
                        </div>
                    </li>


                    <%--<li>
                        <i class="fa fa-clock-o"></i>
                        <div>
                            <span>Reviewed On</span>
                            <strong>12/12/2013 @ 13:22:01</strong>
                        </div>
                    </li>

                    <li>
                        <i class="fa fa-headphones"></i>
                        <div>
                            <span>Reviewd by</span>
                            <strong></strong>
                        </div>
                    </li>--%>
                </ul>
            </div>
            <!-- close call-contact-information -->
        </div>
        <!-- close right-sidebar-content -->

    </div>
    <!-- close right-sidebar -->




    <div class="audio-player">
        <audio></audio>
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
                        <%--<div class="warning-part" style="left: 12%;" title="Listen from here">
                            <span></span>
                            <a href="#" class="listen-from-here">!</a>
                        </div>
                        <!-- close warning-part -->

                        <div class="warning-part" style="left: 22%;" title="Listen from here">
                            <span></span>
                            <a href="#" class="listen-from-here">!</a>
                        </div>
                        <!-- close warning-part -->

                        <div class="warning-part" style="left: 8%;" title="Listen from here">
                            <span></span>
                            <a href="#" class="listen-from-here">!</a>
                        </div>--%>
                        <!-- close warning-part -->
                    </div>
                    <div class="slider-fill"></div>
                </div>
                <!-- close slider -->
            </div>
            <!-- close player-timeline -->


            <div class="player-agent">
                <img src="images/grey-avatar.png" />
                <div>
                    <a href="#" onclick="reload_audio();"><i class="fa fa-refresh" style="color: white;"></i></a>
                    <%-- <strong>John Smith</strong>
                    <span>ID: 2312312312</span>--%>
                </div>

                <!-- <a href="#" class="player-configuration"><i class="fa fa-cog"></i></a> -->
            </div>
            <!-- close player-agent -->

        </div>
        <!-- close audio-player-inner-content -->
    </div>
    <!-- close audio-player -->

    <script type="text/javascript">
        $(document).ready(function () {
            setupAudioPlayer('<%=audio_file %>', 0.75);
            setupExpandCollapse();
        });
    </script>

</asp:Content>

