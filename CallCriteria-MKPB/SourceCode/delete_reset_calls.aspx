<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="delete_reset_calls.aspx.vb" Inherits="delete_reset_calls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Mass Edit Calls</h2>

        <table>
            <tr>
                <td>Data Type:</td>
                <td>
                    <asp:DropDownList ID="ddlDataType" runat="server">
                        <asp:ListItem>Session ID</asp:ListItem>
                        <asp:ListItem Value="F_ID" Text="Form ID (review_record.aspx?ID=)"></asp:ListItem>
                        <asp:ListItem Value="ID" Text="Review ID (from XCC_REPORT_NEW)"></asp:ListItem>
                        <asp:ListItem>Phone</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Function:</td>
                <td>
                    <asp:DropDownList ID="ddlFunction" runat="server">
                        <asp:ListItem>Reset Call</asp:ListItem>
                        <asp:ListItem>Mark Call Bad</asp:ListItem>
                        <asp:ListItem>Delete Call Completely</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td valign="top">Enter Values -- one per line</td>
                <td>
                    <asp:TextBox ID="TextBox1" TextMode="MultiLine" Height="300" runat="server"></asp:TextBox></td>
            </tr>
        </table>

        <asp:Button ID="btnGo" runat="server" Text="Go" />

        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

    </section>
</asp:Content>

