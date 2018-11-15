<%@ Page Title="Calibration Report" Language="VB" MasterPageFile="~/CC_MASter.master"
    AutoEventWireup="false" CodeFile="CalibrationReport.aspx.vb" Inherits="CalibrationReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container dash-modules general-button">
        <div>
            <table class="filter">
                <tr>
                    <td>App:</td>
                    <td>
                        <asp:ListBox ID="lbApps" DataSourceID="dsApps" DataTextField="short_name" SelectionMode="Multiple" DataValueField="id" runat="server"></asp:ListBox>
                        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="select id, appname + ' - ' + short_name as short_name from scorecards where active = 1 order by appname, short_name"  runat="server">
                            <SelectParameters>
                                <asp:Parameter Name="username" />
                            </SelectParameters>
                           
                        </asp:SqlDataSource>
                    </td>
                    <td>

                        <asp:RadioButton runat="server" ID="rbRange" GroupName="date_type" Checked="true" />
                        From:
                        <input type="text" id="date_from" value="" runat="server" placeholder="MM/DD/YYYY" />
                        To:
                        <input type="text" id="date_to" value="" runat="server" placeholder="MM/DD/YYYY" disabled="disabled" />
                        <br />
                        <asp:RadioButton runat="server" ID="rbWE" GroupName="date_type" Text="Week Ending" Checked="true" />
                        <asp:DropDownList ID="ddlWEDate" DataTextField="we_date" DataValueField="we_date" AutoPostBack="true"
                            DataSourceID="dsWEDate" runat="server">
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="dsWEDate" SelectCommand=" select distinct convert(varchar(10),week_ending_date,101) as we_date, week_ending_date from form_score3 with (nolock) order by week_ending_date desc"
                            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td>QA:</td>
                    <td>
                        <asp:DropDownList ID="ddlQAs" DataTextField="reviewer" DataValueField="reviewer" AppendDataBoundItems="true"
                            DataSourceID="dsQAs" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>

                        <asp:SqlDataSource ID="dsQAs" SelectCommand="declare @myCenter int = (select call_center from userextrainfo where username = @username);
                            select distinct reviewer from vwForm with (nolock) join userextrainfo with (nolock) on userextrainfo.username = reviewer where week_ending_date = @we_date
                            and 1 = case when @myCenter is not null then case when userextrainfo.call_center = @myCenter then 1 else 0 end else 1 end
                             order by reviewer"
                            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlWEDate" Name="we_date" />
                                <asp:Parameter Name="Username" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkRecalOnly" runat="server" Text="Recalibration Only" />

                        <asp:CheckBox ID="chkSummary" runat="server" Text="Calibrator Summary Data" />
                    </td>

                    <td style="padding-left: 20px;">

                        <asp:Button ID="btnApply" runat="server" Text="Apply" />

                    </td>
                </tr>
            </table>
        </div>
        <br />
        <br />

        <div>
            <h2>Summary By Calibrator
                <asp:Button ID="btnExport1" CommandArgument="gvSummary" runat="server" Text="Export" /></h2>

            <asp:GridView ID="gvSummary" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                DataSourceID="dsSummary" CellPadding="5" CssClass="detailsTable">
                <Columns>

                    <%--<asp:BoundField DataField="week_ending_date" SortExpression="week_ending_date" HeaderText="Week Ending" DataFormatString="{0:MM/dd/yyyy}" />--%>
                    <asp:BoundField DataField="scorecard" SortExpression="scorecard" HeaderText="Scorecard" />
                    <asp:BoundField DataField="reviewed_by" SortExpression="reviewed_by" HeaderText="Calibrator" />
                    <asp:BoundField DataField="cal_done" SortExpression="cal_done" HeaderText="# Processed" />
                    <asp:BoundField DataField="avgscore" SortExpression="avgscore" HeaderText="Average Score" />
                    <%--<asp:BoundField DataField="pending" SortExpression="pending" HeaderText="# Pending" />--%>
                    <asp:BoundField DataField="reviewtime" SortExpression="reviewtime" HeaderText="Review Time" />
                    <asp:BoundField DataField="calltime" SortExpression="calltime" HeaderText="Call Time" />
                    <asp:BoundField DataField="avg_speed" SortExpression="avg_speed" HeaderText="Average Speed" DataFormatString="{0:P}" />
                    <asp:BoundField DataField="recal_done" SortExpression="recal_done" HeaderText="Recal Done" />
                    <asp:BoundField DataField="avgrecalscore" SortExpression="avgrecalscore" HeaderText="Average Recal Score" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsSummary" runat="server" SelectCommand="getCaliSummary" SelectCommandType="StoredProcedure"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
                <SelectParameters>
                    <asp:ControlParameter ControlID="date_from" PropertyName="value" Name="start_date" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="date_to" Name="end_date" PropertyName="value" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="ddlWEDate" Name="week_ending" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="chkSummary" Name="summary" />
                    <asp:Parameter Name="username" DefaultValue="" />
                    <asp:Parameter Name="scorecard_list" DefaultValue="ALL" />

                </SelectParameters>
            </asp:SqlDataSource>

            <h2>Scorecard Summary
                <asp:Button ID="btnExport2" CommandArgument="gvScorecards" runat="server" Text="Export" /></h2>

            <asp:GridView ID="gvScorecards" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                DataSourceID="dsSCSummary" CellPadding="5" CssClass="detailsTable">
                <Columns>

                    <%--<asp:BoundField DataField="week_ending_date" SortExpression="week_ending_date" HeaderText="Week Ending" DataFormatString="{0:MM/dd/yyyy}" />--%>
                    <asp:BoundField DataField="scorecard" SortExpression="scorecard" HeaderText="Scorecard" />
                    <asp:BoundField DataField="cal_done" SortExpression="cal_done" HeaderText="# Processed" />
                    <asp:BoundField DataField="avgscore" SortExpression="avgscore" HeaderText="Average Score" />
                    <%--<asp:BoundField DataField="pending" SortExpression="pending" HeaderText="# Pending" /> --%>
                    <asp:BoundField DataField="reviewtime" SortExpression="reviewtime" HeaderText="Review Time" />
                    <asp:BoundField DataField="calltime" SortExpression="calltime" HeaderText="Call Time" />
                    <asp:BoundField DataField="avg_speed" SortExpression="avg_speed" HeaderText="Average Speed" DataFormatString="{0:P}" />
                    <asp:BoundField DataField="recal_done" SortExpression="recal_done" HeaderText="Recal Done" />
                    <asp:BoundField DataField="avgrecalscore" SortExpression="avgrecalscore" HeaderText="Average Recal Score" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsSCSummary" runat="server" SelectCommand="getCaliSCSummary" SelectCommandType="StoredProcedure"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
                <SelectParameters>
                    <asp:ControlParameter ControlID="date_from" PropertyName="value" Name="start_date" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="date_to" Name="end_date" PropertyName="value" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="ddlWEDate" Name="week_ending" DefaultValue="1/1/1900" />
                    <asp:Parameter Name="username" DefaultValue="" />
                </SelectParameters>
            </asp:SqlDataSource>


            <h2>Calibrator Summary
                <asp:Button ID="btnExport3" CommandArgument="gvCalibrators" runat="server" Text="Export" /></h2>

            <asp:GridView ID="gvCalibrators" AllowSorting="true" runat="server"
                DataSourceID="dsCalibrators" CellPadding="5" CssClass="detailsTable">
            </asp:GridView>
            <asp:SqlDataSource ID="dsCalibrators" runat="server" SelectCommand="getCalibratorSummary" SelectCommandType="StoredProcedure"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
                <SelectParameters>
                    <asp:ControlParameter ControlID="date_from" PropertyName="value" Name="start_date" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="date_to" Name="end_date" PropertyName="value" DefaultValue="1/1/1900" />
                    <asp:ControlParameter ControlID="ddlWEDate" Name="week_ending" DefaultValue="1/1/1900" />
                    <asp:Parameter Name="username" DefaultValue="" />
                </SelectParameters>
            </asp:SqlDataSource>



        </div>
        <br />
        <br />
        <asp:Button ID="btnExport" runat="server" Text="Export Excel" />
        <div>
            <asp:GridView ID="gvReport" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                DataSourceID="dsReport" CellPadding="5" CssClass="detailsTable">
                <Columns>

                    <asp:HyperLinkField DataNavigateUrlFields="f_id" Text="View" DataNavigateUrlFormatString="~/review_calib_record.aspx?ID={0}" />
                    <asp:BoundField DataField="f_id" SortExpression="f_id" HeaderText="Call ID" />
                    <asp:BoundField DataField="call_date" SortExpression="call_date" HeaderText="Call Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="reviewed_by" SortExpression="reviewed_by" HeaderText="Calibrator" />
                    <asp:BoundField DataField="total_score" SortExpression="total_score" HeaderText="Calibration Score" />
                    <asp:BoundField DataField="agent_group" SortExpression="agent_group" HeaderText="Group" />
                    <asp:BoundField DataField="calibration_comment" SortExpression="calibration_comment" HeaderText="Calibration Comment" HtmlEncode="false" />
                    <asp:BoundField DataField="form_score" SortExpression="form_score" HeaderText="Record Score" />
                    <asp:BoundField DataField="formatted_comments" SortExpression="formatted_comments" HeaderText="Record Comment" HtmlEncode="false" />
                    <asp:BoundField DataField="reviewer" SortExpression="reviewer" HeaderText="QA" />
                    <asp:BoundField DataField="short_name" SortExpression="short_name" HeaderText="Scorecard" />
                    <asp:BoundField DataField="reviewed_date" SortExpression="reviewed_date" HeaderText="Date Reviewed" />
                    <asp:BoundField DataField="calib_date" SortExpression="calib_date" HeaderText="Date Calibrated" />
                    <asp:BoundField DataField="isrecal" SortExpression="isrecal" HeaderText="Recalibration" />
                    <asp:BoundField DataField="recal_score" SortExpression="recal_score" HeaderText="Recal Score" />

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsReport" runat="server"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"></asp:SqlDataSource>
        </div>
    </section>

    <input type="hidden" id="filter" value="range" runat="server" />

    <script>
        $(document).ready(function () {

            function range() {
                $('#ContentPlaceHolder1_date_from').prop('disabled', false);
                //$('[id*=ddlWEDate]').prop('disabled', true);
                if ($('#ContentPlaceHolder1_date_from').val().length == 10)
                    $('#ContentPlaceHolder1_date_to').prop('disabled', false);
                else
                    $('#ContentPlaceHolder1_date_to').prop('disabled', true);
            }

            function wedate() {
                $('#ContentPlaceHolder1_date_from').prop('disabled', true);
                $('#ContentPlaceHolder1_date_to').prop('disabled', true);
                // $('[id*=ddlWEDate]').prop('disabled', false);
            }

            if ($('[id*=filter]').val() == 'wedate') {
                $('ContentPlaceHolder1_rbWE').prop('checked', true);
                wedate();
            }
            else {
                $('ContentPlaceHolder1_rbRange').prop('checked', true);
                range();
            }

            $('ContentPlaceHolder1_rbRange').click(function () {
                range();
            });
            $('ContentPlaceHolder1_rbWE').click(function () {
                wedate();
            });
            $('#apply').click(function () {
                if ($('ContentPlaceHolder1_rbRange').is(':checked'))
                    window.location = window.location.pathname + '?from=' + $('#ContentPlaceHolder1_date_from').val() + '&to=' + $('#ContentPlaceHolder1_date_to').val();
                else
                    window.location = window.location.pathname + '?wedate=' + $('#ContentPlaceHolder1_ddlWEDate').find(":selected").text();
            });
            $('#ContentPlaceHolder1_date_from').val('<% =date_from.Value %>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            $('#ContentPlaceHolder1_date_to').val('<% =date_to.Value%>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            check();
            $('#ContentPlaceHolder1_date_from').keyup(function () {
                check();
            }).change(function () {
                check();
            });
            function check() {
                if ($('#ContentPlaceHolder1_date_from').val().length == 10)
                    $('#ContentPlaceHolder1_date_to').removeAttr('disabled');
                else {
                    $('#ContentPlaceHolder1_date_to').val('');
                    $('#ContentPlaceHolder1_date_to').attr('disabled', 'disabled');
                }
            }
        });

    </script>

</asp:Content>
