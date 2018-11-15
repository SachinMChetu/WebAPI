<%@ Page Title="Deactivated Users" Language="VB" MasterPageFile="~/CC_MASter.master" AutoEventWireup="false" CodeFile="DeactivatedUsers.aspx.vb" Inherits="DeactivatedUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        body {
            background-color: #EEE;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 16px;
        }
        input[type=text], select {
            padding: 5px;
            font-size: 16px;
            width: 120px;
        }
        .jdiv div {
            padding: 10px;
        }
        .gvReport {
            width: 100%;
        }
        .gvReport tr:nth-child(even) {
            background-color: #FFF;
        }
        .gvReport th, .gvReport td, .filter {
            padding: 5px;
        }
        input[type="submit"] {
            background-color: #FFF;
            border: 1px solid #000;
            color: #000;

            border-radius: 6px;
            padding: 5px;
            font-size: 16px;
            font-weight: 600;
            min-width: 100px;
        }
        input[type="submit"]:hover {
            background-color: #000;
            color: #FFF;
        }
    </style>

    <div class="jdiv">
        <div style="line-height:60px;">&nbsp;</div>

        <div>
            <asp:GridView ID="gvReport" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                DataSourceID="dsReport" CellPadding="5" CssClass="gvReport">
                <Columns>

                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White">
                        <ItemTemplate>
                            <asp:Button ID="btnReset" CommandName="Reset" runat="server" Text="Reset" OnClick="btn_Click" /><br />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="username" SortExpression="username" HeaderText="QA" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                    
<%--                    <asp:BoundField DataField="active" SortExpression="active" HeaderText="Active" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                    --%>
                    <asp:BoundField DataField="startdate" SortExpression="startdate" HeaderText="Start Date" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                    
                    <asp:BoundField DataField="lastActiveDate" SortExpression="lastActiveDate" HeaderText="Last Active Date" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                    
                    <asp:BoundField DataField="idle" SortExpression="idle" HeaderText="Idle" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                    
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsReport" runat="server"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT username, startdate, lastActiveDate,
                                CAST(DATEDIFF(hh, lastActiveDate, dbo.getMTDate()) / 24 AS NVARCHAR(10)) + ' days' AS idle
                                FROM UserExtraInfo
                                WHERE user_role = 'QA' AND active = 1 AND DATEDIFF(hh, lastActiveDate, dbo.getMTDate()) > 95
                                ORDER BY lastActiveDate DESC">

                                <%--SELECT e.username, CASE WHEN e.active = 1 THEN 'Yes' ELSE 'No' END AS active, e.startdate, m.LastLoginDate,
                                CAST(DATEDIFF(hh, m.LastLoginDate, dbo.getMTDate()) / 24 AS NVARCHAR(10)) + ' days' AS idle FROM UserExtraInfo e
                                JOIN aspnet_Users u ON u.username = e.username
                                JOIN aspnet_Membership m ON m.UserID = u.userID
                                WHERE user_role = 'QA' AND DATEDIFF(hh, m.LastLoginDate, dbo.getMTDate()) > 95 --AND active = 0 
                                ORDER BY m.LastLoginDate DESC--%>
            </asp:SqlDataSource>
        </div>
    </div>

    <script>

        $(document).ready(function () {

        });

    </script>

</asp:Content>