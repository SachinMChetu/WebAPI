<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 75px;">
        &nbsp;</div>
    <asp:Panel ID="Panel1" runat="server" Visible="false" GroupingText="My Sessions">
        Select date to review:
        <asp:TextBox ID="txt_date" runat="server"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server">
        </asp:CalendarExtender>
        <asp:Button ID="btnChangeDate" runat="server" Text="Go" />
        <div style="overflow: auto;">
            <asp:GridView runat="server" AllowPaging="True" AllowSorting="True" CellPadding="4"
                PageSize="50" DataSourceID="dsMySessions" ForeColor="#333333" GridLines="None"
                ID="gvUserSessions" AutoGenerateColumns="False" DataKeyNames="ID">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Session_ID" HeaderText="Session ID" SortExpression="SESSION_ID" />
                    <asp:BoundField DataField="DISPOSITION" HeaderText="DISPOSITION" SortExpression="DISPOSITION" />
                    <asp:BoundField DataField="CAMPAIGN" HeaderText="CAMPAIGN" SortExpression="CAMPAIGN" />
                    <asp:BoundField DataField="ANI" HeaderText="ANI" SortExpression="ANI" />
                    <asp:BoundField DataField="DNIS" HeaderText="DNIS" SortExpression="DNIS" />
                    <asp:BoundField DataField="DATE" HeaderText="DATE" SortExpression="DATE" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </div>
        <asp:SqlDataSource runat="server" ID="dsMySessions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT replace(convert(varchar(10), [Date],101),'/','_') as theDate,
                                  replace(convert(varchar(10), [Date],108),':','_') + ' ' + 
                                  right(convert(varchar(100), [Date],109),2) as theTime,
                                  * FROM [XCC_REPORT_NEW] with (nolock) where [Date] between @selectedDate and dateadd(dd,1,@selectedDate)">
            <SelectParameters>
                <asp:Parameter Name="selectedDate" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlAdmin" GroupingText="Admin" runat="server">
        <table>
            <tr>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink7" NavigateUrl="~/Users.aspx" runat="server"><img height="100" src="images/icons/Users.png" alt="Users" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="hlUploads" NavigateUrl="~/Uploads.aspx" runat="server"><img  height="100" src="images/icons/uploads.png" alt="Uploads" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/question_management.aspx" runat="server"><img  height="100" src="images/icons/Question_management.png" alt="Question Management" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/Category_Management.aspx" runat="server"><img  height="100" src="images/icons/Categories_management.png" alt="Category Management" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink3" NavigateUrl="~/Section_Management.aspx" runat="server"><img  height="100" src="images/icons/sections.png" alt="Section Management" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink4" NavigateUrl="~/app_settings.aspx" runat="server"><img  height="100" src="images/icons/App_settings.png"  alt="App Settings Management"/></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HyperLink ID="HyperLink8" NavigateUrl="~/Users.aspx" runat="server">Users</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink9" NavigateUrl="~/Uploads.aspx" runat="server">Uploads</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink10" NavigateUrl="~/question_management.aspx" runat="server">Question Management</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink11" NavigateUrl="~/Category_Management.aspx" runat="server">Category Management</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink12" NavigateUrl="~/Section_Management.aspx" runat="server">Section Management</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink13" NavigateUrl="~/app_settings.aspx" runat="server">App Settings Management</asp:HyperLink>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlReports" GroupingText="Reports" runat="server">
        <table>
            <tr>
                <td width="150px" align="center">
                    <asp:HyperLink ID="hlSuperReport" NavigateUrl="~/Supervisor_report.aspx" runat="server"><img  height="100" src="images/icons/supervisor_report.png" alt="Supervisor Report" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink5" NavigateUrl="~/Search.aspx" runat="server"><img  height="100" src="images/icons/search.png" alt="Search" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="hlAgentReport" NavigateUrl="~/Agent_report.aspx" runat="server"><img  height="100" src="images/icons/agent_Report.png" alt="Agent Report" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="hlExpanded" NavigateUrl="~/ExpandedView.aspx" runat="server"><img  height="100" src="images/icons/expanded_view.png" alt="Expanded View" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="HyperLink6" NavigateUrl="~/QA_Report.aspx" runat="server"><img  height="100" src="images/icons/QA_report.png" alt="QA Report" /></asp:HyperLink>
                </td>
                <td width="150px" align="center">
                    <asp:HyperLink ID="hlAvgReport" Visible="false" NavigateUrl="~/Average_Report.aspx"
                        runat="server"><img  height="100" src="images/icons/Average_report.png" alt="Average Report" /></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HyperLink ID="HyperLink14" NavigateUrl="~/Supervisor_report.aspx" runat="server">Supervisor Report</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink15" NavigateUrl="~/Search.aspx" runat="server">Search</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink16" NavigateUrl="~/Agent_report.aspx" runat="server">Agent Report</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink17" NavigateUrl="~/ExpandedView.aspx" runat="server">Expanded View</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink19" NavigateUrl="~/QA_Report.aspx" runat="server">QA Report</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink18" Visible="false" NavigateUrl="~/Average_Report.aspx"
                        runat="server">Average Report</asp:HyperLink>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:SqlDataSource ID="dsGroupList" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
        SelectCommand="SELECT distinct AGent_group FROM [XCC_REPORT_NEW]  with (nolock)  where appname = @appname order by AGent_group ">
        <SelectParameters>
            <asp:SessionParameter Name="appname" SessionField="appname" DefaultValue=""/>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
