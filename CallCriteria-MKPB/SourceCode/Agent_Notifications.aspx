<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="Agent_Notifications.aspx.vb" Inherits="My_Notifications" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Src="~/controls/DetailPlayer.ascx" TagPrefix="UC1" TagName="DetailPlayer" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        function show_audio(file_name, position, form_id) {
            var new_filename;
            $.ajax({
                type: 'POST',
                url: "convert_audio.aspx",
                data: { ID: form_id },
                success: function (result) {
                    new_filename = result;
                },
                async: false
            });
            $('#notification_holder').show();

            document.getElementById('ContentPlaceHolder1_hdnOpenFormID').value = form_id;
            setupAudioPlayer2(new_filename, 0.75, true, position);
            //jumpPos(position);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
        <div class="panel-content">
            <div class="static-header">
                <h1>Notifications
                </h1>
            </div>
           
            <asp:HiddenField ID="hdnAgent" runat="server" />
                    <asp:Button ID="lbPending" CssClass="main-cta" Visible="false" runat="server" Text="Pending"></asp:Button>
                <%--                        <li>
                    <asp:LinkButton ID="lbZero" runat="server">Zero Score</asp:LinkButton>
                </li>--%>
                    <asp:Button ID="lbCompleted" Visible="false" runat="server" Text="Completed"></asp:Button>
                    <asp:Button ID="lbMessages" CssClass="main-cta" Visible="false" runat="server" Text="Messages"></asp:Button>
            <div class="table-outline with-tabs">
               
                <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="SELECT  comments,audio_link,opened_by,AGENT,Agent_Group,role as assigned_to,total_score,comment,date_created as dateadded,form_notifications.id as id,
                                    vwForm.f_id as form_id,
                                    missed_list as MissedQuestions, convert(varchar(10),call_date,101) as call_date FROM [form_notifications] 
                                    join vwForm on form_notifications.form_id = vwForm.f_id 
                                    WHERE date_closed is null and agent = @agent and sup_override is null
                                    ORDER BY  [date_created], Agent"
                    UpdateCommand="UPDATE [notifications] SET [session_id] = @session_id, [assigned_to] = @assigned_to, [comment] = @comment, 
                                [dateadded] = @dateadded, [acknowledged] = @acknowledged, [form_id] = @form_id, [ack_date] = @ack_date WHERE [id] = @id">
                    <SelectParameters>
                        <%--<asp:QueryStringParameter Name="agent" QueryStringField="agent" />--%>
                        <asp:ControlParameter Name="agent" ControlID="hdnAgent" DefaultValue="changen" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="session_id" Type="String" />
                        <asp:Parameter Name="assigned_to" Type="String" />
                        <asp:Parameter Name="comment" Type="String" />
                        <asp:Parameter Name="dateadded" Type="DateTime" />
                        <asp:Parameter Name="acknowledged" Type="Boolean" />
                        <asp:Parameter Name="form_id" Type="Int32" />
                        <asp:Parameter Name="ack_date" Type="DateTime" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
     

                <asp:GridView ID="gvComments" runat="server" DataSourceID="dsComments" GridLines="None" DataKeyNames="form_id" Visible="true"
                    AllowPaging="true" AllowSorting="true" PageSize="50" AutoGenerateColumns="False">
                    <EmptyDataTemplate>
                        No notifications
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Play">
                            <ItemTemplate>
                                <a class="play-option main-cta" href="#" onclick="javascript:window.open('review_record.aspx?ID=<%#Eval("form_id")%>&agent=<%=Request("agent")%>', '');"">
                                    <i class="fa fa-play"></i>
                                </a>
                                <asp:HiddenField ID="hdnFormID" runat="server" Value='<%#Eval("form_id")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:BoundField DataField="AGENT" HeaderText="Agent" SortExpression="AGENT" />
                        <asp:BoundField DataField="dateadded" HeaderText=" Added" DataFormatString="{0:MM/dd/yyyy}"
                            SortExpression="dateadded" />
                        <asp:BoundField DataField="total_score" HeaderText="Total Score" SortExpression="total_score" />
                        <%--<asp:BoundField DataField="comments" HeaderText="Comment" SortExpression="comments" />--%>

                        <asp:TemplateField HeaderText="Comment" SortExpression="comments">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Height="57px" Text='<%# Bind("comments") %>'
                                    TextMode="MultiLine" Width="285px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:literal ID="Label1"  runat="server" Text='<%# Bind("comments") %>'></asp:literal>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HtmlEncode="false" DataField="MissedQuestions" HeaderText="MissedQuestions"
                            SortExpression="MissedQuestions" />
                        <asp:BoundField DataField="Agent_Group" HeaderText="Agent Group" SortExpression="Agent_Group" />
                        <asp:BoundField DataField="assigned_to" HeaderText="Assigned To" SortExpression="assigned_to" />
                        <%--<asp:BoundField DataField="opened_by" HeaderText="Opened By" SortExpression="opened_by" />--%>
                        <asp:BoundField DataField="dateadded" HeaderText="Added" SortExpression="dateadded" />
                        <asp:BoundField DataField="call_date" HeaderText="Call Date" SortExpression="call_date" />
                        <asp:TemplateField HeaderText="Actions" Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" Text="Acknowledge" Font-Size="x-small" OnClick="Ack_Item"
                                    CommandArgument='<%# Eval("id") %>' />
                                <asp:Button ID="Button2" runat="server" Text="I'm Sorry" Visible="false" Font-Size="x-small"
                                    OnClick="Sorry_Item" CommandArgument='<%# Eval("id") %>' />
                                <asp:Button ID="Button3" runat="server" Text="Agree" Visible="false" Font-Size="x-small"
                                    OnClick="Agree_Item" CommandArgument='<%# Eval("id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataNavigateUrlFields="form_id" ControlStyle-Font-Bold="true" SortExpression="form_id"
                            DataNavigateUrlFormatString="review_record.aspx?ID={0}" HeaderText="View Record" Visible="false"
                            DataTextField="form_id" />
                    </Columns>
                    <PagerStyle CssClass="admin-pager" />
                </asp:GridView>
            
            </div>
        </div>
        <p>
            <br />
            <asp:HiddenField ID="hdnFormID" runat="server" />
        </p>
    

        <asp:Button ID="btnSupeExportxp" Visible="true" CssClass="add-category-btn main-cta" runat="server" Text="Export to Excel" />
      


        <asp:Label ID="lblRows" Visible="false" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblProcessed" runat="server" Visible="false" Text=""></asp:Label>
        <%--<UC1:DetailPlayer runat="server" ID="DetailPlayer" />--%>
           <asp:UpdatePanel runat="server" ID="upUpdate">
                    <ContentTemplate>
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtComments" ForeColor="Red"  runat="server" ErrorMessage="Comment Required" Text="Comment Required"> </asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="txtComments" placeholder="Commments...." TextMode="MultiLine" Columns="30" Rows="3" runat="server"></asp:TextBox>
                   
                    <div>
                    </div>

                </div>
                     
                <!-- close player-agent -->

            </div>
            <!-- close audio-player-inner-content -->
        </div>
           </ContentTemplate>
                    </asp:UpdatePanel>
    </section>


</asp:Content>
