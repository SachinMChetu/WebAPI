<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Labor_load.aspx.vb" Inherits="Labor_load" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container">

        
            <h1 class="section-title"><i class="fa fa-bar-chart-o"></i>QA Report</h1>
            <div style="float: right;">
                <div class="report-nav"><a href="ExpandedView.aspx"><i class="fa fa-table"></i><span>Expanded Report</span></a></div>

                <div class="report-nav"><a href="QA_Report2.aspx" class="selected-type"><i class="fa fa-calendar-o"></i><span>QA Report Weekly</span></a></div>
            </div>
        


        <div class="standard-reports-container">

            <!-- close report-types -->

            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                <ProgressTemplate>
                    <img width="25" src="images/WaitCursor.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>


                    <asp:SqlDataSource ID="dsQAReportWeek" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select worked_hours,review_hours, convert(varchar(10), z.week_ending_date,101) as week_ending_date, total_QAs, [dbo].[ConvertTimeToHHMMSS](total_secs/total_QAs,'s') as avg_per_QA, 
                            [dbo].[ConvertTimeToHHMMSS](total_review_secs/total_QAs,'s') as avg_rvw_per_QA  from 
                            (select [dbo].[ConvertTimeToHHMMSS](sum(call_length),'s') as worked_hours, [dbo].[ConvertTimeToHHMMSS](sum(datediff(s,review_started, review_date)),'s') as review_hours, 
                            convert(float,sum(call_length)) as total_secs, convert(float,sum(datediff(s,review_started, review_date))) as total_review_secs, week_ending_date from form_score3 
                            join xcc_report_new on xcc_report_new.id = form_score3.review_id
                            where week_ending_date &gt; dateadd(mm,-2,dbo.getMTDate())
                            group by week_ending_date) z
                            join
                            (select count(*) as total_QAs, week_ending_date from (
                            select count(*) as total1, reviewer, week_ending_date from form_score3 
                            where week_ending_date &gt; dateadd(mm,-2,dbo.getMTDate())
                            group by week_ending_date, reviewer) a 
                            group by week_ending_date) y
                            on y.week_ending_date = z.week_ending_date "
                        runat="server"></asp:SqlDataSource>


                    <div class="table-outline">

                        <asp:GridView ID="gvQAReport" DataSourceID="dsQAReportWeek" AllowSorting="true" ShowFooter="true" AutoGenerateColumns="false" GridLines="None"
                            runat="server">
                            <FooterStyle Font-Bold="true" />
                            <EmptyDataTemplate>No data found.</EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField DataField="worked_hours" HeaderText="Worked Hours" SortExpression="worked_hours" />
                                <asp:BoundField DataField="review_hours" HeaderText="Review Hours" SortExpression="review_hours" />
                                <asp:BoundField DataField="week_ending_date" HeaderText="Week Ending Date" SortExpression="week_ending_date" DataFormatString="{0:mm_dd_yyyy}"  />
                                <asp:BoundField DataField="total_QAs" HeaderText="Total Working QAs" SortExpression="total_QAs" />
                                <asp:BoundField DataField="avg_per_QA" HeaderText="Averge QA Call Hours/Week" SortExpression="avg_per_QA" />
                                <asp:BoundField DataField="avg_rvw_per_QA" HeaderText="Averge QA Review Hours/Week" SortExpression="avg_rvw_per_QA" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        </div>
    </section>
    <script type="text/javascript">
        setupCalendars();
    </script>
</asp:Content>

