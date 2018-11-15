<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="cs_tracking.aspx.vb" Inherits="cs_tracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Call Source Performance Tracking</h2>

        Filters:
        <table>
            <tr>
                <td>Call Date</td>
                <td>
                    <asp:TextBox ID="txtDate" CssClass="hasDatePicker" AutoPostBack="true" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>QA</td>
                <td>
                    <asp:DropDownList ID="ddlQA" DataSourceID="dsQA" AppendDataBoundItems="true" AutoPostBack="true"
                        DataTextField="reviewer" DataValueField="reviewer" runat="server">
                        <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Campaign/Client</td>
                <td>
                    <asp:DropDownList ID="ddlClient" DataSourceID="dsCampaign" AppendDataBoundItems="true" AutoPostBack="true"
                        DataTextField="campaign" DataValueField="campaign" runat="server">
                        <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>

            <tr>
                <td>Scorecard</td>
                <td>
                    <asp:DropDownList ID="ddlSC" DataSourceID="dsScorecard" AutoPostBack="true"
                        DataTextField="short_name" DataValueField="id" runat="server">
                    </asp:DropDownList></td>
            </tr>

        </table>

        <asp:FormView ID="FormView1" DataSourceID="dsPending" runat="server">
            <ItemTemplate>
                Pending:
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text='<%#Eval("num_pending") %>'></asp:Label>
            </ItemTemplate>
        </asp:FormView>

        <asp:SqlDataSource ID="dsPending" runat="server" SelectCommand="select count(*) as num_pending from xcc_report_new where scorecard = @scorecard and max_reviews = 0"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="scorecard" DefaultValue="527" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="dsQA" runat="server" SelectCommand="select distinct reviewer from vwForm where scorecard = @scorecard and call_date > dateadd(d, -15, getdate()) order by reviewer" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="scorecard" DefaultValue="527" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="dsScorecard" runat="server" SelectCommand="select id, short_name from scorecards where high_priority = 1 order by short_name" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"></asp:SqlDataSource>



        <asp:SqlDataSource ID="dsCampaign" runat="server" SelectCommand="select distinct campaign from xcc_report_new where scorecard = @scorecard  and call_date > dateadd(d, -15, getdate()) order by campaign" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="scorecard" DefaultValue="527" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="gvPerf" AutoGenerateColumns="false" CssClass="detailsTable" DataSourceID="dsPerf" runat="server">
            <Columns>
                <asp:BoundField DataField="hour_added" HeaderText="Hour" />
                <asp:BoundField DataField="number_received" HeaderText="Received" />
                <asp:BoundField DataField="number_completed" HeaderText="Completed" />
                <asp:BoundField DataField="avg_review" HeaderText="Avg Review Time" />
                <asp:BoundField DataField="avg_call_speed" HeaderText="Avg Call Speed" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="avg_call_time" HeaderText="Avg Call Time" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="within_hour" HeaderText="0-60 Minutes" />
                <asp:BoundField DataField="within_2nd_hour" HeaderText="60-120 Minutes" />
                <asp:BoundField DataField="within_3rd_hour" HeaderText="120-180 Minutes" />
                <asp:BoundField DataField="within_4th_hour" HeaderText="180-240 Minutes" />
                <asp:BoundField DataField="within_5th_hour" HeaderText="240-300 Minutes" />
                <asp:BoundField DataField="more_than_6_hours" HeaderText="> 300 Minutes" />



            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsPerf" runat="server" SelectCommand="getCSPerf" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDate" Name="call_date" />
                <asp:ControlParameter ControlID="ddlQA" Name="QA" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="ddlClient" Name="client" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="ddlSC" Name="scorecard" DefaultValue="527" />
            </SelectParameters>
        </asp:SqlDataSource>


        <h4>Last Working</h4>
        <asp:GridView ID="gvLastWorking" CssClass="detailsTable" DataSourceID="dsLastWorking" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsLastWorking" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getWorkingHighPriorityQAs" runat="server"></asp:SqlDataSource>

    </section>
</asp:Content>

