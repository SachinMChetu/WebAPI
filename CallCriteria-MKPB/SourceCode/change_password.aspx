<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="change_password.aspx.vb" Inherits="change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">

        <h1 class="section-title"></h1>


        <table>
            <tr>
                <td>
                    <h2>Reset user's password:</h2>
                </td>
            </tr>
            <tr>
                <td>Username:</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <td>New Password:</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>

                <td>
                    <asp:Button ID="Button1" runat="server" Text="Reset" /></td>
                <td>
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text=""></asp:Label></td>
            </tr>

            <tr>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td>
                    <h2>Change User Name</h2>
                </td>
            </tr>
            <tr>
                <td>Old Username:</td>
                <td>
                    <asp:TextBox ID="txtOldUser" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>New Username:</td>
                <td>
                    <asp:TextBox ID="txtNewUser" runat="server"></asp:TextBox></td>
            </tr>
            <tr>

                <td>
                    <asp:Button ID="Button2" runat="server" Text="Update Username" /></td>
                <td>
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text=""></asp:Label></td>
            </tr>

        </table>


        <hr />
        Change all
        <asp:DropDownList ID="ddlRole" runat="server">
            <asp:ListItem>Agent</asp:ListItem>
            <asp:ListItem>Supervisor</asp:ListItem>
            <asp:ListItem>Manager</asp:ListItem>
            <asp:ListItem>Partner</asp:ListItem>
        </asp:DropDownList>
        password on scorecard 
            <asp:DropDownList ID="ddlApps" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="short_name" DataValueField="id" runat="server">
                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select id, appname + ' - ' + short_name as short_name from scorecards where active = 1 order by appname, short_name" runat="server"></asp:SqlDataSource>
        to <asp:TextBox ID="txtNewPassword" runat="server"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" /><asp:Label ID="lblReset" ForeColor="Red" runat="server" Text=""></asp:Label>

        <br /><br />


          Change all external users on 
            <asp:DropDownList ID="ddlAppnames" AppendDataBoundItems="true" DataSourceID="dsAppnames" DataTextField="appname" DataValueField="appname" runat="server">
                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            </asp:DropDownList>
        <asp:SqlDataSource ID="dsAppnames" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select appname from app_settings where active = 1 order by appname" runat="server"></asp:SqlDataSource>
        to <asp:TextBox ID="txtAppPass" runat="server"></asp:TextBox>
        <asp:Button ID="btnChangeApp" runat="server" Text="Go" /><asp:Label ID="lblAppResult" ForeColor="Red" runat="server" Text=""></asp:Label>

    </section>
</asp:Content>

