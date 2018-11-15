<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="listen_training_old.aspx.vb" Inherits="listen" %>

<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="local" %>
<%@ Register Src="~/controls/five9Sesssion.ascx" TagPrefix="UC1" TagName="five9Sesssion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />

    <script type="text/javascript">
        function setDDLTimestamp(ddl) {
            audio = $('.audio-player audio').get(0);
            $(ddl).parent().find("input[name*='hdnQTimestamp2']").val(audio.currentTime);
        }

        function setCHKTimestamp(ddl) {
            //alert('Check change');
            audio = $('.audio-player audio').get(0);
            $(ddl).parent().find("input[name*='hdnQTimestamp3']").val(audio.currentTime);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"> 

    <section class="main-container listening-actions right-sidebar-showed">
        <h3>Call from
            <asp:Label runat="server" ID="lblThisApp"></asp:Label></h3>  <asp:Label ID="lblNumberScores" Visible="false" runat="server"></asp:Label> <asp:Label ID="lblAvgScore" Visible="false"  runat="server"></asp:Label>
        <asp:HiddenField ID="hdnTotalCallLength" runat="server" />
        <asp:HiddenField ID="hdnAutoNotify" runat="server" />
        <asp:HiddenField ID="hdnXCCID" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnThisApp" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnAutoSubmit" Value="0" runat="server" />
        <asp:HiddenField ID="hdnSpeedLimit" Value="2.2" runat="server" />
        <asp:HiddenField ID="hdnCalibID" runat="server" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" Visible="false" />
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
                                <asp:HiddenField ID="hdnCount" runat="server" />
                                <li>
                                    <a href="#" tabindex="1">
                                        <label><%# Eval("q_short_name")%></label>
                                        <%--<asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator1" ControlToValidate="hdnQAnswer"
                                            runat="server" Text="*" ErrorMessage="Field Required"></asp:RequiredFieldValidator>--%>
                                        <asp:Panel ID="pnlYesNo" runat="server">
                                            <div id="q_trigger" runat="server" title='<%# Eval("question")%>'>
                                                <local:HiddenField2 ID="hdnQAnswer" runat="server" />
                                                <asp:HiddenField ID="hdnQTimestamp" Value="0" runat="server" />

                                                <div class="trigger"></div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlSelect" runat="server" Visible="true">
                                            <div id="q_trigger2" runat="server" title='<%# Eval("question")%>' class="field-holder">
                                                <local:HiddenField2 ID="hdnQAnswer2" runat="server" />
                                                <asp:HiddenField ID="hdnQTimestamp2" Value="0" runat="server" />

                                                <i class="fa fa-tag"></i>
                                                <asp:DropDownList ID="ddlQList" onchange="setDDLTimestamp(this);" OnDataBound="FixList2" runat="server" AppendDataBoundItems="true" DataSourceID="dsQList" DataTextField="answer_text" DataValueField="id">
                                                    <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="dsQList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                    SelectCommand="SELECT * from question_answers where question_id = @QID"
                                                    runat="server">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlCheck" runat="server" Visible="true">
                                            <div id="q_timeonly" runat="server" title='<%# Eval("question")%>' class="field-holder">
                                                <local:HiddenField2 ID="hdnQAnswer3" runat="server" />
                                                <asp:HiddenField ID="hdnQTimestamp3" Value="0" runat="server" />
                                                <input  type="checkbox" onmousedown="setCHKTimestamp(this);this.checked=true;" value="Test" /> 
                                            </div>
                                        </asp:Panel>

                                        <!-- close switch -->
                                    </a>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT *, case when sections.id in(12,20,28,36) then 'switch answer-no' 
                                when class is null  then  'switch answer-neutral' else class end as starting_class
                                FROM [Questions] q  join sections on sections.id  = q.section 
                                left join (select 'switch answer-' + lower(answer_text) as class, question_id from question_answers 
                                where question_id in (select id from questions where appname = @appname) and autoselect = 1) b
                                on b.question_id = q.id
                                where q.section=@section and q.appname=@appname and q.active = 1
                                order by  q_order"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblID" Name="Section" />
                            <asp:ControlParameter ControlID="hdnThisApp" Name="appname" />
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
                SelectCommand="select * from sections where id in (select distinct section from questions where appname=@appname and active=1) and appname=@appname order by section_order"
                runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnThisApp" Name="appname" />
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


            <div class="actions-in-right listening-main-actions">
                <div class="checkbox-label">
                    <asp:CheckBox ID="chkStopWorking" runat="server" />
                    <label for="doneworking">I'm done Working</label>
                </div>
                <asp:Button ID="btnSaveSession" CssClass="main-cta close-popup" runat="server" Text="SAVE" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                &nbsp;<br />
            </div>
            <!-- close actions-in-right -->
        </div>
    </section>
    <!-- close main-container -->

    <div class="right-sidebar">
        <div class="right-sidebar-header" runat="server" id="rightHeader">
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


                <li class="expand-collapses collapsed" runat="server" id="liSchoolItem">
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

                    <li>

                        <i class="fa fa-clock-o"></i>
                        <UC1:five9Sesssion ID="five9update" runat="server" record_ID='<%# eval("id") %>' audio_player="fvtopFORMData"></UC1:five9Sesssion>
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
        });
    </script>

</asp:Content>

