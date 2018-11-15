<%@ Page Title="Guidelines" Language="VB" MasterPageFile="~/CC_Master.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="guidelines.aspx.vb" Inherits="guidelines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="gl_files/guidelines.css" rel="stylesheet" />
    <script src="gl_files/guidelines.js?1001"></script>
    <script src="gl_files/jquery.transit.min.js"></script>
    <script src="gl_files/jquery.pulse.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:Panel runat="server" ID="pnlGuide">
        <asp:HiddenField ID="hdnUsername" Value="" runat="server" />
        <asp:HiddenField ID="hdnUsertype" runat="server" />
        <section class="main-container">

            <div class="updates-btn">
                <span>
                    <i class="fa fa-bell-o"></i>
                    <i class="fa fa-angle-down"></i>
                    <i class="fa fa-check"></i>
                </span>
            </div>

            <div class="guidelinesBody">

                <span class="scorecard-select">
                    <span>
                        <asp:DropDownList ID="ddlScorecard" OnDataBound="ddlScorecard_DataBound" DataSourceID="dsSC"
                            DataTextField="scorecard_name" AppendDataBoundItems="true" Enabled="false" AutoPostBack="true"
                            DataValueField="ID" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="dsSC" SelectCommand="select scorecard_name + '|' + qu_class as scorecard_name, ID, appname 
                                from  (select scorecards.id, appname, short_name + ' (' + appname + ')' as scorecard_name, 
                                case when isnull(date_reviewed,'1/1/2010') &lt; max_date  or isnull(date_reviewed,'1/1/2010') &lt; max_date_f  then 'red' else '' end as qu_class 
                                from scorecards  left join (select sc_id, max(date_reviewed) as date_reviewed from sc_update where reviewer = @username group by sc_id) a on a.sc_id = scorecards.id 
                                left join (select max(dateadded) as max_date, scorecard_id from q_instructions join questions on questions.ID = q_instructions.question_id group by scorecard_id) b
                                on b.scorecard_id = scorecards.id
                                left join (select max(dateadded) as max_date_f, scorecard_id from q_faqs join questions on questions.ID = q_faqs.question_id group by scorecard_id) c
                                on c.scorecard_id = scorecards.id
                                where scorecards.id in (select user_scorecard from userapps where username = @username) and active =1) b  order by appname, scorecard_name"
                            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                            <SelectParameters>
                                <asp:Parameter Name="username" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </span>
                    <asp:Button ID="btnExport" runat="server" Text="Export" />
                </span>

                <span class="secHeader">Guidelines</span>
                <span style="float: right; position: relative; top: 15px; left: -22px;">
                    <img src="img/circle_plus.png" alt="Select" onclick="expand_all();"></img></span>

                <asp:Repeater ID="rptSections" DataSourceID="dsSections" runat="server">
                    <ItemTemplate>
                        <span class="recSection"><%#Eval("section") %></span>
                        <asp:HiddenField ID="hdnSectionID" Value='<%#Eval("id") %>' runat="server" />



                        <asp:Repeater ID="rptQuestions" DataSourceID="dsQs" runat="server">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnQID" Value='<%#Eval("ID") %>' runat="server" />
                                <asp:SqlDataSource ID="dsAnswers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select question_answers.ID, answer_text, case when @user_type in('QA','QA Lead','Admin','Calibrator') then qa_points else answer_points end as answer_points 
                                from question_answers join questions on questions.ID = question_answers.question_id where question_id = @QID and pre_production = 0  "
                                    runat="server">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                        <asp:ControlParameter ControlID="hdnUsertype" Name="user_type" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <div class="recordBox">

                                    <div class="recordHeader">

                                        <div class="names">
                                            <span class="name"><%#Eval("q_short_name") %> </span>
                                            -
						            <span class="definition"><%#Replace(Eval("agent_display").ToString, Chr(13), "<br>") %></span>
                                        </div>

                                        <div class="recPoints"></div>
                                        <div style="float: right; position: relative; top: -50px; width: 50px;">
                                            <i class="fa fa-<%#Eval("q_type_icon") %>" aria-hidden="true"></i>
                                            <div style="cursor: pointer; float: right; display: none;" class="client_edit">
                                                <image src="images/edit-ico.png" alt="Select" onclick="window.location.href='single_question.aspx?ID=<%#Eval("ID")%>&appname=<%#Eval("appname")%>';">
                                            </div>
                                        </div>
                                    </div>



                                    <div class="recHints">
                                        <ul>
                                            <asp:Repeater ID="rptIns" DataSourceID="dsIns" runat="server">
                                                <ItemTemplate>
                                                    <li class="<%#LCase(Eval("answer_type").ToString()) %>-point  <%#Eval("qu_class") %>"><%#Eval("question_text") %></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:SqlDataSource ID="dsIns" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                SelectCommand="select case when date_reviewed is null then 'update' when date_reviewed &lt; dateadded then 'update' else '' end as qu_class, * from q_instructions 
                                                            join questions on questions.ID = question_id
                                                            left join (select sc_id,max(date_reviewed) as date_reviewed from sc_update where reviewer = @username group by sc_id) a on a.sc_id = scorecard_id
                                                             where question_id = @QID and scorecard_id = @sc_id order by q_instructions.q_order"
                                                runat="server">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                                    <asp:ControlParameter ControlID="hdnUsername" Name="username" />
                                                    <asp:ControlParameter ControlID="ddlScorecard" Name="sc_id" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </ul>

                                    </div>
                                    <div class="expandBtn expandBtn2"></div>

                                    <div class="expandBtn"></div>
                                    <div class="recFAQ">

                                        <asp:Repeater ID="rptFAQs" DataSourceID="dsFAQs" runat="server">
                                            <ItemTemplate>
                                                <span class='question <%#Eval("qu_class") %>'><%#Eval("question_text") %></span>
                                                <span class="answer"><%#Eval("question_answer") %></span>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:SqlDataSource ID="dsFAQs" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                            SelectCommand="select case when date_reviewed is null then 'update' when date_reviewed &lt; dateadded then 'update' else '' end as qu_class, * from q_faqs join 
                                                            questions on questions.ID = question_id
                                                            left join (select sc_id,max(date_reviewed) as date_reviewed from sc_update where reviewer = @username group by sc_id) a on a.sc_id = scorecard_id
                                                             where question_id = @QID order by q_faqs.q_order
                                                            "
                                            runat="server">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                                <asp:ControlParameter ControlID="hdnUsername" Name="username" />
                                                <asp:ControlParameter ControlID="ddlScorecard" Name="sc_id" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="recCanned">
                                        <asp:Repeater ID="rptanswers" DataSourceID="dsAnswers" runat="server">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnAnswerID" runat="server" Value='<%#Eval("ID") %>' />
                                                <div class="canned-<%#LCase(Eval("answer_text").ToString()) %>">
                                                    <span class="part-header"><span><%#Eval("answer_text") %> - (<%#Eval("answer_points") %>)</span></span>
                                                    <ul>
                                                        <asp:Repeater ID="rptResponses" DataSourceID="dsAnsComments" runat="server">
                                                            <ItemTemplate>
                                                                <li><%#Eval("comment") %><%#Eval("display_comment_points") %></li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <asp:SqlDataSource ID="dsAnsComments" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                            SelectCommand="select *, case when comment_points is not null then  ' (' + convert(varchar(100), comment_points) + ')' else null end as display_comment_points from answer_comments where answer_id = @ans_id and ac_active = 1" runat="server">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hdnAnswerID" Name="ans_id" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:SqlDataSource ID="dsQs" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="select *, case when isnull(linked_question,'') <> '' then 'link' when q_type = 'Calculated' then 'calculator' when q_type='dynamic' then 'database' else '' end as q_type_icon from questions where section=@section and active = 1 
                        and 1 = case when client_guideline_visible = 0 and @user_type not in('QA','QA Lead','Admin','Calibrator') then 0 else 1 end  order by q_order"
                            runat="server">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnSectionID" Name="section" />
                                <asp:ControlParameter ControlID="hdnUserType" Name="user_type" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="dsSections" SelectCommand="select * from sections where scorecard_id = @sc_id and id in (select Section from questions where active = 1 and scorecard_id = @sc_id) order by section_order" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlScorecard" Name="sc_id" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </section>
    </asp:Panel>
</asp:Content>

