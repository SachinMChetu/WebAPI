<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="ExpandedView_Detail.aspx.vb" Inherits="ExpandedView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container general-button dash-modules">


        <h2>Call History Report -
            <asp:Label ID="lblRows" runat="server" Text=""></asp:Label></h2>


        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlApps" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="short_name" DataValueField="id" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select  short_name, scorecards.id from scorecards join userapps on userapps.user_scorecard = scorecards.id 
                                where active = 1 and username = @username order by short_name"
                        runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" DefaultValue="<%=User.identity.name %>" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td>
                    <asp:TextBox ID="txtGroupStartxp" CssClass="hasDatePicker" placeholder="Start..." runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtgroupEndxp" CssClass="hasDatePicker" placeholder="End..." runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAgentGroupxp" runat="server" DataTextField="AGent_group"
                        DataSourceID="dsGroupList" AutoPostBack="true" DataValueField="AGent_group">
                        <asp:ListItem Text="Select Group/ALL" Value=""></asp:ListItem>
                        <%--<asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct lower([AGENT_GROUP]) as AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and agent_group is not null and agent_group <> '' order by lower([AGENT_GROUP])">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlApps" Name="appname" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAgentByGroupxp" runat="server" AutoPostBack="true"
                        DataTextField="AGent" DataValueField="AGent">
                        <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsAgentByGroupxp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct lower([AGENT_GROUP]) as AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname order by lower([AGENT_GROUP])">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlAgentGroupxp" Name="AGent_group" DefaultValue="" />
                            <asp:ControlParameter ControlID="ddlApps" Name="appname" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td>
                    <asp:CheckBox ID="addTimes" runat="server" Checked="false" /> Add Time Position values 
                </td>
                <td>
                    <asp:Button ID="btnAgentGroupxp" runat="server" Text="Apply Filters" />
                </td>
                <td>
                    <asp:Button ID="btnViewSummary" runat="server" Text="Summary View" />
                </td>


            </tr>
        </table>










        <asp:HiddenField ID="hdnExtraFilters" Value="" runat="server" />
        <asp:HiddenField ID="HiddenField1" Value="" runat="server" />

        <asp:Button ID="btnSupeExportxp" runat="server" Visible="false" Text="Export to Excel" />
        <asp:GridView ID="gvAgentGroupxp" CssClass="detailsTable" ShowHeader="true" ShowFooter="true" EnableViewState="false" runat="server" AutoGenerateColumns="true"
            DataKeyNames="id" GridLines="None">
        </asp:GridView>


        <asp:SqlDataSource ID="dsGroupList" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT distinct AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date and AGENT_GROUP is not null and AGENT_GROUP <> '' order by AGENT_GROUP ">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlApps" Name="appname" />
                <asp:ControlParameter Name="start_date" ControlID="txtGroupStartxp" />
                <asp:ControlParameter Name="end_date" ControlID="txtGroupEndxp" />
            </SelectParameters>
        </asp:SqlDataSource>



        <script type="text/javascript">
            setupCalendars();
        </script>
    </section>
</asp:Content>
