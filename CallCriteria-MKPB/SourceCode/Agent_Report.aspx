<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="Agent_Report.aspx.vb" Inherits="Agent_Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container dash-modules general-button">
        
            <h1 class="section-title"><i class="fa fa-desktop"></i>
                Agent Report
            </h1>
        
        <div class="general-filter">
            <div class="">
                <table style="float: right;">
                    <tr>
                        <td>
                            <asp:Button ID="btnAgentReport" CssClass="secondary-cta" runat="server" Text="Go" /></td>

                        <td>
                            <asp:Button ID="btnSupeExportxp" runat="server" CssClass="secondary-cta" Text="Export to Excel" /></td>
                    </tr>
                </table>

                <div class="field-holder-right">
                    <%--<i class="fa fa-calendar-o"></i>--%>
                    <asp:DropDownList ID="ddlAgentList2" runat="server" DataSourceID="dsAgentList2" DataTextField="Agent" AppendDataBoundItems="true"
                        DataValueField="Agent">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsAgentList2" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct [Agent] FROM [XCC_REPORT_NEW]   where appname=@appname  and agent_group <> '' order by [Agent] ">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="Appname" QueryStringField="appname" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>



                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>
                <!-- close date-holder -->


                <!-- close select-holder -->
                <!-- close date-holder -->

            </div>
            <!-- close yellow-container -->
        </div>
        <!-- close general-filter -->



        <div class="">


            <h2>Reviewer History</h2>
            <asp:GridView ID="gvAgentQuestions" CssClass="detailsTable " DataSourceID="dsAvgQuestions" DataKeyNames="ID" GridLines="None"
                ShowFooter="true" runat="server" AutoGenerateColumns="False" Width="100%">
                <EmptyDataTemplate>No calls</EmptyDataTemplate>
                <Columns>
                    <asp:HyperLinkField Text="Select" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="review_record.aspx?ID={0}" />
                    <%--<asp:BoundField DataField="timestamp" HeaderText="Time Stamp" SortExpression="timestamp" />--%>
                    <asp:BoundField DataField="CALL_TIME" HeaderText="Call Time" DataFormatString="{0:mm:ss}"
                        SortExpression="CALL_TIME" />
                    <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                    <asp:BoundField DataField="real_total" HeaderText="Total Score (With Fails)" SortExpression="real_total" />
                    <asp:BoundField DataField="total_result" HeaderText="Total Score" SortExpression="total_result" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle Font-Bold="True" ForeColor="White" />
                <HeaderStyle Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="dsAvgQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT form_score3.id, reviewer, form_score3.session_id, num_serious, form_score3.review_date, XCC_REPORT_NEW.CAMPAIGN, 
                    XCC_REPORT_NEW.Agent, XCC_REPORT_NEW.ANI, XCC_REPORT_NEW.DNIS,   XCC_REPORT_NEW.DISPOSITION, 
                    XCC_REPORT_NEW.CALL_TIME, xcc_REPORT_NEW.num_of_schools, form_score3.reviewer, XCC_RESULTS.total_score, total_result, 
                    case when a.num_fails = 0 then total_result else 0 end as real_total, 
                    case when call_type='Inbound' then ANI else DNIS end as Phone FROM form_score3  
                    INNER JOIN XCC_REPORT_NEW   ON form_score3.review_id = XCC_REPORT_NEW.ID 
                    INNER JOIN XCC_RESULTS   ON [XCC_REPORT_NEW].ID = XCC_RESULTS.XCC_ID join 
                    (select sum (case when ((question_result = 0) and (category=4) and (serious=0)) then 1 else 0 end) as num_fails, 
                    sum (case when question_result = -1 and serious=1 then 1 else 0 end) as num_serious, 
                    sum (case when (question_result &gt; 0) or (question_result = -1 and q_type='Yes/No') 
                    then isnull(q_percent,0) else 0 end) as total_result, form_id from form_q_scores   
                    join [Questions]   on question_id = [Questions].id join sections on sections.id = questions.section 
                    where questions.appname = @appname group by form_id) a   on a.form_id = form_score3.ID 
                    WHERE (XCC_REPORT_NEW.Agent = @AgentID) and form_score3.review_date between @start_date and @end_date + ' 11:59 pm'
                    and form_score3.appname = @appname">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtAgentStart" DefaultValue="1/1/1900" Name="start_date" />
                    <asp:ControlParameter ControlID="txtAgentEnd" DefaultValue="1/1/2014" Name="end_date" />
                    <asp:ControlParameter ControlID="ddlAgentList2" PropertyName="SelectedValue" DefaultValue="0"
                        Name="AgentID" />
                    <asp:QueryStringParameter Name="Appname" QueryStringField="appname" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div class="clear">
            </div>
        </div>
        <script type="text/javascript">
            setupCalendars();
        </script>
    </section>
</asp:Content>
