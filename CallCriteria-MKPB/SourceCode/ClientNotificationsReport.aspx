<%@ Page Title="Notifications Report" Language="VB" MasterPageFile="~/CC_MASter.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ClientNotificationsReport.aspx.vb" Inherits="NotificationsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <%--  <asp:UpdatePanel runat="server">
        <ContentTemplate>--%>
    <section class="main-container dash-modules general-button">
        Scorecard:
        <asp:DropDownList ID="ddlApps" DataSourceID="dsApps" DataTextField="short_name" DataValueField="id" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getScorecards" SelectCommandType="StoredProcedure" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

         App:
        <asp:DropDownList ID="ddlAppname" DataSourceID="dsAppname" DataTextField="appname" DataValueField="appname" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsAppname" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getAppnames" SelectCommandType="StoredProcedure" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

        Supervisor:
        <asp:DropDownList ID="ddlSuper" DataSourceID="dsSuper" DataTextField="user_group" DataValueField="user_group" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsSuper" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct user_group from userapps join userextrainfo on userextrainfo.username = userapps.username where user_scorecard in (select user_scorecard from userapps join scorecards on scorecards.id = user_scorecard where userapps.username = @username and active = 1) and user_group <> '' and user_group is not null order by user_group" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

        From:
                        
                <asp:TextBox ID="from" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>
        To:
                <asp:TextBox ID="to" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>

        <asp:Button ID="btnApply" runat="server" Text="Apply" />

        <br />
        <br />

        <div>

            <table>
                <tr>
                    <td valign="top">Notifications by Scorecard/Supervisor
                        <asp:GridView ID="gvSummary" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                            DataSourceID="dsSummary" CellPadding="5" CssClass="detailsTable">
                            <Columns>
                                <asp:BoundField DataField="total" SortExpression="total" HeaderText="Total" />
                                <asp:BoundField DataField="totalopen" SortExpression="totalopen" HeaderText="Open" />
                                <asp:BoundField DataField="totalclose" SortExpression="totalclose" HeaderText="Close" />
                                <asp:BoundField DataField="scorecard_name" SortExpression="scorecard_name" HeaderText="Scorecard" />
                                <asp:BoundField DataField="Supervisor" SortExpression="Supervisor" HeaderText="Supervisor" />
                                <asp:BoundField DataField="week_ending_date" SortExpression="week_ending_date" HeaderText="Week Ending" DataFormatString="{0:MM/dd/yyy}" />
                            </Columns>
                        </asp:GridView>

                        <asp:SqlDataSource ID="dsSummary" runat="server"
                            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="getNotificationSupervisors" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:Parameter Name="username" />
                                <asp:ControlParameter ControlID="from" Name="start_date" />
                                <asp:ControlParameter ControlID="to" Name="end_date" />
                                <asp:ControlParameter ControlID ="ddlApps" Name="scorecard" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID ="ddlAppname" Name="appname" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID ="ddlSuper" Name="agent_group" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <br />
                          Disputed Call Plus Edits
                        <asp:GridView ID="gvDisputes"  runat="server" DataSourceID="dsDisputes" AllowSorting="true" CssClass="detailsTable">
                            <EmptyDataTemplate>No disputes from supervisors</EmptyDataTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsDisputes" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="getNotificationDisputes" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="from" Name="start_date" />
                                <asp:ControlParameter ControlID="to" Name="end_date" />
                                <asp:ControlParameter ControlID ="ddlApps" Name="scorecard" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                    <td>&nbsp;</td>
                    <td valign="top">Notifications by User 
                        <asp:GridView ID="gvUsers" Visible="true" runat="server" DataSourceID="dsUsers" AllowSorting="true" CssClass="detailsTable"></asp:GridView>
                        <asp:SqlDataSource ID="dsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                            SelectCommand="getNotificationUsers" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="username" />
                                <asp:ControlParameter ControlID="from" Name="start_date" />
                                <asp:ControlParameter ControlID="to" Name="end_date" />
                                <asp:ControlParameter ControlID ="ddlApps" Name="scorecard" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID ="ddlAppname" Name="appname" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID ="ddlSuper" Name="agent_group" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>


        </div>
        <br />
        <br />
        <div>


            <asp:Button ID="btnExport" runat="server" Text="Export to Excel" />
            <asp:GridView ID="gvReport" runat="server"  AutoGenerateColumns="False" AllowSorting="true" 
                DataSourceID="dsReport" CellPadding="5" CssClass="detailsTable">
                <Columns>

                    <%--<asp:BoundField DataField="appname" SortExpression="appname" HeaderText="AppName" />--%>
                    <asp:BoundField DataField="scorecard_name" SortExpression="scorecard_name" HeaderText="Scorecard" />
                    <asp:BoundField DataField="f_id" SortExpression="f_id" HeaderText="Call ID" />

                    <asp:HyperLinkField DataNavigateUrlFields="F_ID" DataNavigateUrlFormatString="~/review/{0}" HeaderText="Call Link" DataTextField="F_ID" />

                    <asp:BoundField DataField="call_date" SortExpression="call_date" HeaderText="Call Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="phone" SortExpression="phone" HeaderText="Phone" />
                    <asp:BoundField DataField="AGENT" SortExpression="AGENT" HeaderText="Agent" />
                    <asp:BoundField DataField="AGENT_group" SortExpression="AGENT_group" HeaderText="Group" />

                    <asp:BoundField DataField="missed_list" SortExpression="missed_list" HeaderText="Missed Points" />

                    <asp:BoundField DataField="all_comments" HtmlEncode="false" SortExpression="all_comments" HeaderText="Comments" />
                    <asp:BoundField DataField="wasEdited" HtmlEncode="false" SortExpression="wasEdited" HeaderText="wasEdited" />
                    <asp:BoundField DataField="reviewer" SortExpression="reviewer" HeaderText="QA" />

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsReport" runat="server"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="getNotificationDetails" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                    <asp:ControlParameter ControlID="from" Name="start_date" />
                    <asp:ControlParameter ControlID="to" Name="end_date" />
                    <asp:ControlParameter ControlID ="ddlApps" Name="scorecard" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID ="ddlAppname" Name="appname" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID ="ddlSuper" Name="agent_group" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </section>

    <%-- "SELECT TOP 500 vwFN.appname, F_ID,short_name,  call_date, AGENT, AGENT_group, missed_list, comment, date_created, opened_by, date_closed, closed_by, close_reason, reviewer
                                FROM vwFN join scorecards on scorecards.id = scorecard
                                where  scorecard in (select user_scorecard from userapps where username=@username) 
                                ORDER BY date_created DESC   </ContentTemplate>
    </asp:UpdatePanel>--%>

    <script>
        $(document).ready(function () {
            <%--$('[id*=ddlWEDate]').attr('disabled', 'disabled');
            $('#range').click(function () {
                $('#ContentPlaceHolder1_from').removeAttr('disabled');
                if (('#ContentPlaceHolder1_from').val().length == 10)
                    $('#ContentPlaceHolder1_to').removeAttr('disabled');
                else
                    $('#ContentPlaceHolder1_to').attr('disabled', 'disabled');
                $('[id*=ddlWEDate]').attr('disabled', 'disabled');
            });
            $('#wedate').click(function () {
                $('#ContentPlaceHolder1_from').attr('disabled', 'disabled');
                $('#ContentPlaceHolder1_to').attr('disabled', 'disabled');
                $('[id*=ddlWEDate]').removeAttr('disabled');
            });
            $('#apply').click(function () {
                if ($('#range').is(':checked'))
                    window.location = window.location.pathname + '?from=' + $('#ContentPlaceHolder1_from').val() + '&to=' + $('#ContentPlaceHolder1_to').val();
                else
                    window.location = window.location.pathname + '?wedate=' + $('#ContentPlaceHolder1_ddlWEDate').find(":selected").text();
            });
            $('#ContentPlaceHolder1_from').val('<% Response.Write(Request.QueryString("from"))%>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            $('#ContentPlaceHolder1_to').val('<% Response.Write(Request.QueryString("to"))%>').datepicker({
                dateFormat: "mm/dd/yy"
            });
            check();
            $('#ContentPlaceHolder1_from').keyup(function () {
                check();
            }).change(function () {
                check();
            });
            function check() {
                if ($('#ContentPlaceHolder1_from').val().length == 10)
                    $('#ContentPlaceHolder1_to').removeAttr('disabled');
                else {
                    $('#ContentPlaceHolder1_to').val('');
                    $('#ContentPlaceHolder1_to').attr('disabled', 'disabled');
                }
            }--%>
        });

    </script>


    <script>
        $(document).ready(function () {


            $('#ContentPlaceHolder1_from').datepicker({
                dateFormat: "mm/dd/yy"
            });
            $('#ContentPlaceHolder1_to').datepicker({
                dateFormat: "mm/dd/yy"
            });

        });

    </script>


</asp:Content>
