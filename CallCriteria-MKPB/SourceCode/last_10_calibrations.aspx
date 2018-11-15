<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="last_10_calibrations.aspx.vb" Inherits="last_10_calibrations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Last 10 Calibrations</h2>

        <table>
            <tr>
                <td>
                    <asp:GridView CssClass="detailsTable" ID="GridView1" DataKeyNames="QA, scorecard" AllowSorting="true" DataSourceID="SqlDataSource1" runat="server">
                        <Columns>
                            <asp:CommandField SelectText="View" ShowSelectButton="true" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td valign ="top">
                    <asp:GridView CssClass="detailsTable" ID="GridView2" AllowSorting="true" DataSourceID="SqlDataSource2" runat="server"></asp:GridView>
                </td>
            </tr>
        </table>


        <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="select count(*) as [Number Calibrations last 3 weeks],  reviewer as [QA], convert(decimal(10,2),avg(total_score)) as [Avg Calibration], scorecard_name as Scorecard, min(review_date) as [Oldest Cali Date] 
            from (select row_number() over(partition by reviewer, scorecard_name order by review_date desc) as row_num, 
            reviewer, total_score, scorecard_name, review_date 
            from vwCF where review_date &gt; dateadd(d, -21, dbo.getMTDate()) and active_cali = 1) a join userextrainfo on userextrainfo.username = reviewer
            where row_num &lt; 11 and call_center is not null group by reviewer, scorecard_name
            order by scorecard_name, reviewer"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="select f_id as [Call ID],  reviewer as [QA], total_score as [Calibration Score], scorecard_name as Scorecard, review_date as [Cali Date] 
            from (select row_number() over(partition by reviewer, scorecard_name order by review_date desc) as row_num, 
            reviewer, total_score, scorecard_name, review_date, f_id
            from vwCF where review_date &gt; dateadd(d, -21, dbo.getMTDate()) and reviewer = @reviewer and active_cali = 1 and scorecard_name=@scorecard) a 
            where row_num &lt; 11 
            order by review_date desc, scorecard_name, reviewer">
            <SelectParameters>
                <asp:Parameter Name="reviewer" />
                <asp:Parameter Name="scorecard" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

