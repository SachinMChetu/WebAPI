<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="Agent_Dashboard.aspx.vb" Inherits="Agent_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnAgent" runat="server" />
    <div id="dashboard-info-panel">
        <div class="infopanel-container">
            <div class="left">
                <div class="block">
                    <label>
                        Records worked Today :</label>
                    <span>
                        <asp:Label ID="lblWorkedRecords" runat="server" Text="0"></asp:Label></span>
                </div>
                <div class="block">
                    <label>
                        Number left to review :</label>
                    <span>
                        <asp:Label ID="lblLeftToReview" runat="server" Text="0"></asp:Label></span>
                </div>
            </div>
            <div class="right">
                <div class="right-block">
                    <div class="block">
                        <label>
                            Agent Avg:</label>
                        <asp:Label ID="lblAvgScore" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="block">
                        <label>
                            Number Reviews:</label>
                        <asp:Label ID="lblNoOfreview" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="block">
                        <label>
                            Total Fails:</label>
                        <asp:Label ID="lblTotalFailes" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dashboard-main-area">
        <div class="main-container">
            <div class="call-reviwed-block">
                <div class="title">
                    <span class="ico"></span>
                    <h2>
                        Calls to be reviewed</h2>
                </div>
                <div class="msg-box">
                    <div class="msg">
                        <span class="ico"><a href="javascript:void(0);"></a></span>
                        <div class="msg-tooltip">
                            <span class="arrow"></span><span class="hint">message now</span>
                        </div>
                    </div>
                    <div class="block">
                        <label>
                            Your Supervisor :
                        </label>
                        <span>
                            <asp:Label ID="lblSupervisor" runat="server" Text=""></asp:Label></span>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div class="call-review-data">
                    <asp:GridView ID="gvAvailableCalls" AutoGenerateColumns="false" DataSourceID="dsAvailableCalls" 
                        runat="server" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Session_ID" HeaderText="Session ID" SortExpression="session_id" />
                            <%--<asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" />--%>
                            <asp:BoundField DataField="Talk_time" HeaderText="Talk Time" SortExpression="Talk_time" />
                            <asp:BoundField DataField="call_time" HeaderText="Call Time" SortExpression="call_time" />
                            <asp:BoundField DataField="Agent_group" HeaderText="Agent Group" SortExpression="Agent_group" />
                            <%--<asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" />--%>
                            <%-- <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="work_record5.aspx?ID={0}" HeaderText="Work Record"
                                Text="Work Record" />--%>
                        </Columns>
                        <EmptyDataTemplate>
                            No available calls
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsAvailableCalls" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select top 10 * from dbo.XCC_REPORT_NEW b where ((MAX_REVIEWS < 1 or MAX_REVIEWS is null)) and agent=@agent  and appname =  @appname"
                        runat="server">
                        <SelectParameters>
                            <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue=""/>
                            <asp:ControlParameter ControlID="hdnAgent" Name="Agent" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="last-reviwed-calls-block">
                <div class="title">
                    <span class="ico"></span>
                    <h2>
                        Last reviewed calls</h2>
                </div>
                <div class="last-call-review-data">
                    <asp:GridView ID="gvLast" AutoGenerateColumns="false" DataSourceID="dsLast" runat="server" Font-Size="XX-Small"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Session_ID" HeaderText="Session ID" SortExpression="session_id" />
                            <%--<asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" />--%>
                            <%--<asp:BoundField DataField="Talk_time" HeaderText="Talk Time" SortExpression="Talk_time" />
                            <asp:BoundField DataField="call_time" HeaderText="Call Time" SortExpression="call_time" />--%>
                            <asp:BoundField DataField="Agent_group" HeaderText="Agent Group" SortExpression="Agent_group" />
                            <%--<asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" />--%>
                            <asp:HyperLinkField DataNavigateUrlFields="id,Agent" DataNavigateUrlFormatString="review_record.aspx?ID={0}&Agent={1}"
                                HeaderText="View Record" Text="View Record" />
                        </Columns>
                        <EmptyDataTemplate>
                            No reviewed calls
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsLast" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select top 5 * from dbo.XCC_RESULTS join XCC_REPORT_NEW on XCC_ID = XCC_REPORT_NEW.ID where agent = @agent and review_date between convert(varchar(10), dateadd(d,-30,dbo.getMTDate()),101) and dbo.getMTDate() and xcc_report_new.appname =  @appname"
                        runat="server">
                        <SelectParameters>
                            <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue=""/>
                            <asp:ControlParameter ControlID="hdnAgent" Name="Agent" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <div class="notification-block">
                <div class="title">
                    <span class="ico"></span>
                    <h2>
                        Notifications</h2>
                </div>
                <div class="notification-data">
                    <asp:GridView ID="gvNotifications" AutoGenerateColumns="false" Width="100%" DataSourceID="dsNotifications"
                        runat="server">
                        <Columns>
                            <asp:BoundField DataField="opened_by" HeaderText="Opened by" />
                            <asp:BoundField DataField="Comment" HeaderText="Comment" />
                            <asp:BoundField DataField="dateadded" HeaderText="Date" />
                            <%--<asp:TemplateField  HeaderText="Actions">
                            <ItemTemplate>
                                 <asp:ImageButton ID="ImageButton1" ImageUrl="images/check-btn2.png" runat="server" OnClick="ApproveNotification"
                                        CommandArgument='<%# eval("note_id")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            <asp:HyperLinkField DataNavigateUrlFields="form_id,Agent" DataNavigateUrlFormatString="review_record.aspx?ID={0}&Agent={1}"
                                HeaderText="View Record" Text="View" />
                        </Columns>
                        <EmptyDataTemplate>
                            No pending notifications
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsNotifications" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select agent, notifications.id as note_id,form_score3.id as form_id,opened_by, dateadded, comment, Agent from notifications 
                              join form_score3 on notifications.form_id = form_score3.id
                              join XCC_REPORT_NEW on XCC_REPORT_NEW.ID =  form_score3.review_ID
                              where assigned_to = 'REP' and acknowledged is null
                              and AGENT = @AGENT">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnAgent" Name="Agent" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
