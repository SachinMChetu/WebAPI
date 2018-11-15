<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="Agent_Details.aspx.vb" Inherits="Agent_Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/controls/Notifications.ascx" TagPrefix="UC1" TagName="Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container">
        
            <h1 class="section-title"><i class="fa fa-desktop"></i>
                Agent Detail -
        <asp:Label ID="lblAgentID" runat="server" Text=""></asp:Label></h1>
        
        <div class="general-filter">
            <div class="yellow-container">
                <table style="float: right;">
                    <tr>
                        <td>
                            <asp:Button ID="btnAgentReport" CssClass="secondary-cta" runat="server" Text="Go" /></td>
                    </tr>
                </table>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>
                <!-- close date-holder -->

            </div>
            <!-- close yellow-container -->
        </div>
        <!-- close general-filter -->

        <asp:HiddenField ID="hdnAgentID" Value="GMBAgentA" runat="server" />










        Total Records:
    <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
        <div class="table-outline users-list">
            <asp:GridView ID="gvAgentQuestions" DataKeyNames="ID" DataSourceID="dsAvgQuestions" ShowHeader="true" ShowFooter="true"
                runat="server" AutoGenerateColumns="False">
                <EmptyDataTemplate>No data</EmptyDataTemplate>
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="timestamp" HeaderText="Time Stamp" SortExpression="timestamp" />
                    <asp:BoundField DataField="CALL_TIME" HeaderText="Call Time" DataFormatString="{0:mm:ss}"
                        SortExpression="CALL_TIME" />
                    <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                    <asp:BoundField DataField="real_total" HeaderText="Total Score (With Fails)" SortExpression="real_total" />
                    <asp:BoundField DataField="total_result" HeaderText="Total Score" SortExpression="total_result" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <asp:SqlDataSource ID="dsAvgQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT form_score3.id, reviewer, form_score3.session_id, num_serious, form_score3.review_date, XCC_REPORT_NEW.CAMPAIGN, XCC_REPORT_NEW.Agent, 
                        XCC_REPORT_NEW.ANI, XCC_REPORT_NEW.DNIS,  XCC_REPORT_NEW.timestamp, XCC_REPORT_NEW.DISPOSITION, XCC_REPORT_NEW.CALL_TIME, xcc_REPORT_NEW.num_of_schools, 
                        form_score3.reviewer, XCC_RESULTS.total_score, total_result, case when a.num_fails = 0 then total_result else 0 end as real_total, 
                        case when call_type='Inbound' then ANI else DNIS end as Phone FROM form_score3 INNER JOIN XCC_REPORT_NEW ON form_score3.review_id = XCC_REPORT_NEW.ID 
                        INNER JOIN XCC_RESULTS ON [XCC_REPORT_NEW].ID = XCC_RESULTS.XCC_ID join (select sum (case when ((question_result = 0) 
                        and (category=4) and serious = 0) then 1 else 0 end) as num_fails, 
                        sum (case when question_result = -1 and serious = 1 then 1 else 0 end) as num_serious, 
                        sum (case when (question_result &gt; 0) or (question_result = -1 and q_type='Yes/No') 
                        then isnull(q_percent,0) else 0 end) as total_result, form_id from form_q_scores 
                        join [Questions] on question_id = [Questions].id join sections on sections.id = questions.section 
                        group by form_id) a on a.form_id = form_score3.ID WHERE (call_date  BETWEEN @start_date AND @end_date) AND (XCC_REPORT_NEW.Agent = @AgentID) "
                runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="3/1/2012" Name="start_date" />
                    <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="12/31/2012" Name="end_date" />
                    <asp:ControlParameter ControlID="lblAgentID" DefaultValue="GMBAgentA76" Name="AgentID" />
                </SelectParameters>
            </asp:SqlDataSource>


            <hr />

            <asp:GridView ID="gvComments" runat="server" DataSourceID="dsComments" AutoGenerateColumns="False">
                <EmptyDataTemplate>
                    No notifications
                </EmptyDataTemplate>
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="form_id" DataNavigateUrlFormatString="review_record.aspx?ID={0}" Text="View Record" />
                    <asp:BoundField DataField="opened_by" HeaderText="Opened By" SortExpression="opened_by" />
                    <asp:TemplateField HeaderText="Assigned To" SortExpression="assigned_to">
                        <EditItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("assigned_to") %>'>
                                <asp:ListItem>(Select)</asp:ListItem>
                                <asp:ListItem>REP</asp:ListItem>
                                <asp:ListItem>QA</asp:ListItem>
                                <asp:ListItem>Supervisor</asp:ListItem>
                                <asp:ListItem>Admin</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" Enabled="false" SelectedValue='<%# Bind("assigned_to") %>'>
                                <asp:ListItem>(Select)</asp:ListItem>
                                <asp:ListItem>REP</asp:ListItem>
                                <asp:ListItem>QA</asp:ListItem>
                                <asp:ListItem>Supervisor</asp:ListItem>
                                <asp:ListItem>Admin</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="comment">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Height="57px" Text='<%# Bind("comment") %>'
                                TextMode="MultiLine" Width="285px"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("comment") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dateadded" HeaderText="Date Added" SortExpression="dateadded" />
                    <asp:BoundField DataField="ack_date" HeaderText="Acknowledged Date" SortExpression="ack_date" />
                    <asp:BoundField DataField="ack_by" HeaderText="Acknowledged By" SortExpression="ack_by" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Acknowledge" OnClick="Ack_Item"
                                CommandArgument='<%# Eval("id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <p>
                <br />
                <asp:HiddenField ID="hdnFormID" runat="server" />
            </p>
            <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT *,form_score3.id as form_id  FROM [form_notifications] join form_score3 on form_notifications.form_id = form_score3.id 
  join xcc_report_new on form_score3.review_id = xcc_report_new.id 
  WHERE  (date_closed is null) and Agent=@agent  ORDER BY [dateadded] DESC"
                DeleteCommand="DELETE FROM [notifications] WHERE [id] = @id"
                InsertCommand="INSERT INTO [notifications] ([session_id], [assigned_to], [comment], 
    [dateadded], [acknowledged], [form_id], [ack_date]) VALUES (@session_id, @assigned_to, @comment, @dateadded, @acknowledged, @form_id, @ack_date)"
                UpdateCommand="UPDATE [notifications] SET [session_id] = @session_id, [assigned_to] = @assigned_to, [comment] = @comment, 
    [dateadded] = @dateadded, [acknowledged] = @acknowledged, [form_id] = @form_id, [ack_date] = @ack_date WHERE [id] = @id">
                <DeleteParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="session_id" Type="String" />
                    <asp:Parameter Name="assigned_to" Type="String" />
                    <asp:Parameter Name="comment" Type="String" />
                    <asp:Parameter Name="dateadded" Type="DateTime" />
                    <asp:Parameter Name="acknowledged" Type="Boolean" />
                    <asp:Parameter Name="form_id" Type="Int32" />
                    <asp:Parameter Name="ack_date" Type="DateTime" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnFormID" DefaultValue="0" Name="form_id" PropertyName="Value"
                        Type="Int32" />
                    <asp:QueryStringParameter QueryStringField="Agent" Name="Agent" DefaultValue="" />
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
        </div>
        <script type="text/javascript">
            setupCalendars();
        </script>

    </section>
</asp:Content>
