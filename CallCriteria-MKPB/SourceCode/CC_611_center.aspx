<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="CC_611_center.aspx.vb" Inherits="CC_DM_center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>611 Call Center Stats</h2>
       From:
        <asp:TextBox ID="date_from" runat="server"></asp:TextBox>
        To:
        <asp:TextBox ID="date_to" runat="server"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" />
        <br />

        OR
         Week Ending:
        <asp:DropDownList ID="ddlWE" DataSourceID="dsWEDate" DataTextField="we_date" DataValueField="we_date" AutoPostBack="true"  AppendDataBoundItems="true" runat="server">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsWEDate" SelectCommand=" select distinct convert(varchar(10),week_ending_date,101) as we_date, week_ending_date from form_score3 order by week_ending_date desc"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"></asp:SqlDataSource>



        Calls Processed<br />
        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" ShowFooter="true" FooterStyle-Font-Bold="true" DataSourceID="dsSummary" Height="194px">
            <Columns>
                <asp:BoundField DataField="Number Calls" HeaderText="Number Calls" ReadOnly="True" SortExpression="Number Calls" />
                <asp:BoundField DataField="Call Time" HeaderText="Call Time" SortExpression="Call Time" ReadOnly="True" />
                <asp:BoundField DataField="Averge Speed" HeaderText="Averge Speed" ReadOnly="True" SortExpression="Averge Speed" />
                <asp:BoundField DataField="number_calib" HeaderText="Number Calibrations" ReadOnly="True" SortExpression="number_calib" />
                <asp:BoundField DataField="avg_cali" HeaderText="Averge Calibration" ReadOnly="True" SortExpression="avg_cali" />
                <asp:BoundField DataField="Review Time" HeaderText="Review Time" ReadOnly="True" SortExpression="Review Time" />
                <asp:BoundField DataField="reviewer" HeaderText="reviewer" SortExpression="reviewer" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsSummary" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select count(*) as [Number Calls], dbo.ConvertTimeToHHMMSS(sum(call_length),'s') as [Call Time], 
            convert(varchar(100),convert(decimal(10,2),sum(call_length)/sum(datediff(s, review_started, review_date)) * 100)) + '%' as [Averge Speed],
            dbo.ConvertTimeToHHMMSS(sum (datediff(s, review_started, review_date)),'s') as [Review Time], vwForm.reviewer, convert(decimal(10,2), avg(avg_cali)) as avg_cali, avg(number_calib) as number_calib
            from vwForm join userextrainfo on reviewer = username 
            left join (select avg(calibration_form.total_score) as avg_cali, count(*) as number_calib, reviewer from calibration_form join vwForm on vwForm.f_id = original_form where calibration_form.review_date between @start and @end group by reviewer) a
            on a.reviewer = vwForm.reviewer
            where review_date between @start and dateadd(d,1,@end) 
            and call_center = 1 group by vwForm.reviewer">
            <SelectParameters>
                <asp:ControlParameter ControlID="date_from" Name="start" PropertyName="Text" />
                <asp:ControlParameter ControlID="date_to" Name="end" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>

        <br />
        Training Processed<br />
        <asp:GridView ID="gvTrain" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataSourceID="dsTrain" Height="194px">
            <Columns>
                <asp:BoundField DataField="Number Calls" HeaderText="Number Calls" ReadOnly="True" SortExpression="Number Calls" />
                <asp:BoundField DataField="reviewer" HeaderText="reviewer" SortExpression="reviewer" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsTrain" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select count(*) as [Number Calls], sum(call_length) as [Call Time], reviewer from vwTrain 
            join userextrainfo on reviewer = username where review_date between @start and dateadd(d,1,@end) and call_center = 1 group by reviewer">
            <SelectParameters>
                <asp:ControlParameter ControlID="date_from" Name="start" PropertyName="Text" />
                <asp:ControlParameter ControlID="date_to" Name="end" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>



    </section>

    <script>
        $('#ContentPlaceHolder1_date_from').val('<% =date_from.Text %>').datepicker({
            dateFormat: "mm/dd/yy"
        });
        $('#ContentPlaceHolder1_date_to').val('<% =date_to.Text%>').datepicker({
            dateFormat: "mm/dd/yy"
        });
    </script>
</asp:Content>

