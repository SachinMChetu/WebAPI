<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="calib_selected.aspx.vb" Inherits="calib_selected" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Calibration Calls Selected</h2>

        Reviewed Calls  - 
        Start Date:
        <asp:TextBox ID="txtStart" CssClass="hasDatePicker" placeholder="MM/DD/YYYY" runat="server"></asp:TextBox>
        End Date:
        <asp:TextBox ID="txtEnd" CssClass="hasDatePicker" placeholder="MM/DD/YYYY" runat="server"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" />
        <br />
        <br />

        Calibrations completed -- of the original selected, how many were completed
        <br />
        Recal Calibration Count -- the # of calibrations completed where the recalibrations were selected from.

        <%--Searches calls completed between <%=call_start %> @ 2pm and <%=call_end %> @ 2PM--%>


         <asp:GridView ID="GridView1" ShowFooter="true" AllowSorting="true" FooterStyle-Font-Bold="true" CssClass="detailsTable" DataSourceID="SqlDataSource1" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:estomesManual %>" 
            SelectCommand="update calibration_pending set sc_id = scorecard 
            from vwForm where sc_id is null and form_id = f_id; 

            select count(*) as [Total Calls], vwForm.scorecard_name as Scorecard, 
            sum(case when isrecal = 0 then 1 else 0 end) as [Total Calibrations],
            
            calib_percent as [Target Calib %],
            convert(decimal(10,2), 100.0 * sum(case when isrecal = 0 then 1 else 0 end) /count(*)) as [% Selected],
            sum(case when isrecal = 0 and date_completed is not null then 1 else 0 end) as [Calib Completed], 
            sum(case when isrecal = 0 and date_completed is null then 1 else 0 end ) as [Calib Pending], 
            sum(case when isrecal = 1 then 1 else 0 end) as [Total Recalibrations],
            case when sum(case when isrecal = 0 then 1 else 0 end) > 0 then 
            convert(decimal(10,2), 100.0 * sum(case when isrecal = 1 then 1 else 0 end) / sum(case when isrecal = 0 then 1 else 0 end))
            else
            null end as [% Recal Selected],
            sum(case when isrecal = 1 and date_completed is not null then 1 else 0 end) as [ReCal Completed], 
            sum(case when isrecal = 1 and date_completed is null then 1 else 0 end) as [ReCal Pending]
            
            from vwForm left join calibration_pending on vwForm.f_id = form_id
            join scorecards on scorecards.id = scorecard
            where vwForm.review_date between dateadd(hh, 14, @start) and dateadd(hh, 14, @end) and website is null and isnull(bad_value, 'Daily') = 'Daily'  and bad_call is null
            group by vwForm.scorecard_name, calib_percent  order by vwForm.scorecard_name"
            runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStart" Name="start" />
                <asp:ControlParameter ControlID="txtEnd" Name="end" />
            </SelectParameters>
        </asp:SqlDataSource>
       <%-- <asp:GridView ID="gvNewSelected" CssClass="detailsTable" DataSourceID="dsNewSelected" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsNewSelected" ConnectionString="<%$ ConnectionStrings:estomesManual %>" SelectCommand="getCaliLoaded" SelectCommandType="StoredProcedure"
            runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStart" Name="startdate" />
                <asp:ControlParameter ControlID="txtEnd" Name="enddate" />
            </SelectParameters>
        </asp:SqlDataSource>


        <asp:GridView ID="gvSelected" CssClass="detailsTable" Visible="false" DataSourceID="dsSelected" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsSelected" ConnectionString="<%$ ConnectionStrings:estomesManual %>" SelectCommand="update calibration_pending set sc_id = scorecard 
            from vwForm where sc_id is null and form_id = f_id; select scorecards.id,  short_name, sum(total_calls) as [Total Calls], 
            sum(num_calls) as [Target Selected], convert(decimal(10,2), 100.0*sum(num_calls)/sum(total_calls)) as [Percent Selected],   
            sum(num_selected) as [Actual Selected], convert(decimal(10,2), 100.0*sum(num_selected)/sum(total_calls)) as [Actual % Selected],
            sum ([Calib Pending]) as [Calib Pending],
            sum ([Calib Completed]) as [Calib Completed],
            sum([ReCal Pending]) as [ReCal Pending], 
            sum([ReCal Completed]) as [ReCal Completed] from calib_inserted 
            join scorecards on scorecard = scorecards.id 
            join (
            select sum(case when isrecal = 0 and date_completed is not null then 1 else 0 end) as [Calib Completed], 
            sum(case when isrecal = 0 and date_completed is null then 1 else 0 end ) as [Calib Pending], 
            sum(case when isrecal = 1 and date_completed is not null then 1 else 0 end) as [ReCal Completed], 
            sum(case when isrecal = 1 and date_completed is null then 1 else 0 end) as [ReCal Pending], 
            short_name as sc_name from calibration_pending
            join scorecards on sc_id = scorecards.id 
            where form_id in (select f_id from vwForm where review_date between dateadd(hh, 13, @start) and dateadd(hh, 14, @end))
            and bad_value = 'Daily'
            group by short_name
            ) a on sc_name = short_name
            
            where convert(date, date_inserted) between dateadd(d, 1, @start) and  dateadd(d, 1, @end)  and total_calls > 0
            group by scorecards.id,  short_name order by short_name"
            runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStart" Name="start" />
                <asp:ControlParameter ControlID="txtEnd" Name="end" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        


        <table>
            <tr>
                <td valign="top">
                    <h2>Calibrations</h2>
                    <asp:GridView ID="gvCals" CssClass="detailsTable" DataSourceID="dsCals" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsCals" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                        SelectCommand="update calibration_pending set sc_id = scorecard 
                         from vwForm where sc_id is null and form_id = f_id; 
                         select z.scorecard, z.scorecard_name as [Scorecard Name], real_total_calls as [Total Calls], cali_calls as [Selected Calls], 
                        convert(decimal(10,2), 100.0 * cali_calls/real_total_calls) as [% Cali Selected], calib_percent as [Target %] from 
                        (select count(*) as real_total_calls, vwForm.scorecard, vwForm.scorecard_name  from vwForm 
                        where vwForm.review_date between dateadd(hh, 14, @start) and dateadd(hh, 14, @end)   group by vwForm.scorecard, vwForm.scorecard_name) z
                        join scorecards on scorecards.id = z.scorecard
                        join 
                        (select count(calibration_pending.id) as cali_calls, vwForm.scorecard, vwForm.scorecard_name from vwForm 
                        left join calibration_pending on form_id = vwForm.f_id
                        where vwForm.review_date between  dateadd(hh, 14, @start) and dateadd(hh, 14, @end)   and isnull(isrecal,0) = 0 and bad_value = 'Daily' and website is null group by vwForm.scorecard, vwForm.scorecard_name) a
                        on z.scorecard = a.scorecard 
                        order by a.Scorecard_name"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtStart" Name="start" />
                            <asp:ControlParameter ControlID="txtEnd" Name="end" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>
                <td valign="top">
                    <h2>Recalibrations</h2>
                    <asp:GridView ID="gvRecal" CssClass="detailsTable" DataSourceID="dsRecal" runat="server"></asp:GridView>
                    <asp:SqlDataSource ID="dsRecal" ConnectionString="<%$ ConnectionStrings:estomesManual %>" SelectCommand="  
                         select y.*, b.recal_calls, convert(decimal(10,2), 100.0 * recal_calls/PENDING_COMPLETED) as [% Recal Selected]  from 
                       
                        (select scorecard, scorecard_name , count(*) as pending_completed from calibration_pending join vwForm on vwForm.f_id = form_id
                        where convert(date, calibration_pending.date_completed) between dateadd(hh, 14, @start) and dateadd(hh, 14, @end)    and bad_value = 'Daily'  group by scorecard, scorecard_name) y
                        join 
                        (select  count(calibration_pending.id) as recal_calls, sc_id  from calibration_pending 
                        where convert(date, date_added) between  dateadd(hh, 14, @start) and dateadd(hh, 14, @end)   and isnull(isrecal,1) = 1 and bad_value = 'Daily' 
                         group by sc_id) b
                        on y.scorecard = b.sc_id
                        order by y.Scorecard_name"
                        runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtStart" Name="start" />
                            <asp:ControlParameter ControlID="txtEnd" Name="end" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>
            </tr>
        </table>

    </section>
</asp:Content>

