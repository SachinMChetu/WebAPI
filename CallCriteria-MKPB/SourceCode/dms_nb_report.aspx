<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="dms_nb_report.aspx.vb" Inherits="dms_nb_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>DMS Non-billable Report</h2>

        <table style="padding: 10px; margin: 10px;">
            <tr>
                <td>Select scorecard:</td>
                <td>
                    <asp:DropDownList ID="ddlScorecard" DataSourceID="dsSC"
                        DataTextField="scorecard_name" AppendDataBoundItems="true" AutoPostBack="true"
                        DataValueField="ID" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsSC" SelectCommand="select short_name as scorecard_name, ID, appname 
                            from scorecards  where id in (select user_scorecard from userapps where username = @username) 
                            and id in (select distinct scorecard_id from answer_comments join questions on questions.ID = question_id where answer_comments.non_billable = 1 or special2 = 1)
                            and active =1  order by appname, short_name"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>
            </tr>

            <tr>
                <td>Start Date:</td>
                <td>
                    <asp:TextBox ID="date1" runat="server" /></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <asp:TextBox ID="date2" runat="server" /></td>
            </tr>

            <tr>
                <td>Filter: </td>
                <td>
                    <asp:CheckBox ID="chkNB" runat="server" Text="NB" Checked="true" />
                    <asp:CheckBox ID="chkEDMC" runat="server" Text="EDMC" />
                </td>
            </tr>

            <tr>

                <td colspan="2">
                    <asp:Button ID="btnGO" runat="server" Text="Go" /></td>
            </tr>

        </table>

        Rows:
        <asp:Label ID="lblRows" runat="server" Text=""></asp:Label>

        <asp:Button ID="btnExport" runat="server" Text="Export" />
        <asp:GridView ID="gvNB" DataSourceID="dsNB" CssClass="detailsTable" EnableViewState="false" runat="server" AutoGenerateColumns="false">
            <Columns>

                <asp:HyperLinkField DataTextField="f_id" DataNavigateUrlFields="f_id" Target="_blank" DataNavigateUrlFormatString="http://qc.thedmsgrp.com/review_record.aspx?ID={0}" HeaderText="Call ID" />
                <asp:BoundField DataField="agent" HeaderText="Agent" SortExpression="Agent" />
                <asp:BoundField DataField="call_date" HeaderText="Call Date" SortExpression="call_date" DataFormatString="{0:M/d/yyyy}" />
                <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                <asp:BoundField DataField="missed_list" HeaderText="Missed List" SortExpression="missed_list" />
                <%--<asp:BoundField DataField="missed_reason" HeaderText="NB Reason" SortExpression="missed_reason" />--%>
                <asp:BoundField DataField="formatted_comments_clean" HeaderText="Comments" SortExpression="formatted_comments_clean" HtmlEncode="false" />
                <asp:BoundField DataField="review_date" HeaderText="Review Date" SortExpression="review_date" DataFormatString="{0:M/d/yyyy}" />
                <asp:BoundField DataField="real_score" HeaderText="Score" SortExpression="real_score" />
                <asp:BoundField DataField="agent_group" HeaderText="Group" SortExpression="agent_group" />



            </Columns>
        </asp:GridView>
        <%--select *, replace(formatted_comments,'|','') as formatted_comments_clean, isnull(isnull(edited_score, calib_score), total_score) as real_score 
            from vwForm with (nolock) left join calibration_form on f_id = original_form and active_cali = 1
            where f_id in (select distinct form_id from form_q_responses join answer_comments on answer_comments.id = form_q_responses.answer_id where non_billable = 1) 
                        and call_date between @date1 and @date2 and scorecard = @scorecard--%>

        <%%>
        <asp:SqlDataSource ID="dsNB" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="select f_id, agent,call_date, phone, missed_list, review_date, agent_group, replace(replace(formatted_comments,'|',''),'<br>', '') as formatted_comments_clean, 
            isnull(edited_score,  total_score) as real_score 
            from vwForm with (nolock) 
            where display_score != 100 and  f_id in (select distinct form_id from form_q_responses with (nolock)  join answer_comments with (nolock)  on answer_comments.id = form_q_responses.answer_id where isnull(non_billable,0) = 1 ) 
                        and call_date between @date1 and @date2 and scorecard = @scorecard and ((calib_score is null) or (wasedited = 1))
            union all
            Select f_id, agent,call_date, phone, formatted_missed_c As missed_list, review_date, agent_group, replace(replace(formatted_comments_c,'|',''),'<br>', '') as formatted_comments_clean, total_score as real_score 
            From vwCF with (nolock) 
            Where display_score != 100 and  f_id In (Select distinct form_id from form_c_responses With (nolock)  join answer_comments With (nolock)  On answer_comments.id = form_c_responses.answer_id where isnull(non_billable,0) = 1 ) 
                        And call_date between @date1 And @date2 And scorecard = @scorecard And active_cali = 1 and wasedited is null  ">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                <asp:ControlParameter ControlID="date1" Name="date1" />
                <asp:ControlParameter ControlID="date2" Name="date2" />
                <asp:ControlParameter ControlID="chkNB" Name="NB" />
                <asp:ControlParameter ControlID="chkEDMC" Name="EDMC" />
            </SelectParameters>
        </asp:SqlDataSource>

        <script>
            $(document).ready(function () {


                $('#ContentPlaceHolder1_date1').datepicker({
                    dateFormat: "mm/dd/yy"
                });
                $('#ContentPlaceHolder1_date2').datepicker({
                    dateFormat: "mm/dd/yy"
                });

                $('.exportReport').hide();
                $('.printReport').hide();
                $('.resetDetails').hide();


                $('.freezable').each(function () {
                    $(this).tableHeadFixer({ 'left': 2 });

                });


                //getTopMissed();

                //getDetailsQS(0);


            });

        </script>
    </section>
</asp:Content>

