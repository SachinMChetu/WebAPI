<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="calibration_hours.aspx.vb" Inherits="calibration_hours" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h2>Calibration Hours</h2>

    <section class="main-container dash-modules general-button">

        <table>
            <tr>


                <td>Select Week Ending(s):<br />
                    <asp:DropDownList ID="lbWEDate" DataTextField="we_date" DataValueField="we_date"
                        DataSourceID="dsWEDate" runat="server">
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="dsWEDate"
                        SelectCommand=" select distinct convert(varchar(10),week_ending_date,101) as we_date, week_ending_date 
                                            from form_score3 order by week_ending_date desc"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"></asp:SqlDataSource>
                </td>

                <td>
                    <asp:Button ID="btnQAReport" runat="server" Text="Go" /></td>
                <td>
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" /></td>

                <td>
                    <asp:Button ID="btnSupeExportxp" runat="server" Text="Export to Excel" /></td>
            </tr>
            <tr>

                <td colspan="5">
                    <asp:GridView ID="gvHours" CssClass="detailsTable" DataSourceID="dsHours" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsHours" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select count(*) as Number_calibrations, calibration_form.isrecal, dbo.UpperLeft(reviewed_by) as Reviewer, 
                            dbo.ConvertTimeToHHMMSS(sum(datediff(s, calibration_form.review_started, calibration_form.review_date)),'s') as [Review Time], 
                            dbo.ConvertTimeToHHMMSS(sum(call_length),'s') as [Call Time], 
                            convert(decimal (10,2), sum(call_length)/60 * .055) as [Owed],
                            convert(decimal (10,2),sum(call_length) * 100.00/sum(datediff(s, calibration_form.review_started, calibration_form.review_date))) as Speed 
                            from calibration_form  join vwForm on original_form = f_id
                            where convert(date, dateadd(d, -datepart(weekday, vwForm.review_date) + 7, vwForm.review_date)) = @week_ending
                            group by calibration_form.isrecal,reviewed_by ">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lbWEDate" Name="week_ending" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>

    </section>
</asp:Content>
