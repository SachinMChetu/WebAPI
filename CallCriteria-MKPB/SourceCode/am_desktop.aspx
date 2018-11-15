<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="am_desktop.aspx.vb" Inherits="am_desktop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Account Manager MTD Status</h2>
        <asp:DropDownList AutoPostBack="true" ID="DropDownList1" runat="server">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            <asp:ListItem>AndrewLoewith</asp:ListItem>
            <asp:ListItem>BrianMercer</asp:ListItem>
            <asp:ListItem>JacquieRamirez</asp:ListItem>
            <asp:ListItem>Neil Shore</asp:ListItem>

            <asp:ListItem></asp:ListItem>
        </asp:DropDownList>
        <asp:GridView ID="GridView1" CssClass="detailsTable" AllowSorting="true" DataSourceID="SqlDataSource1"  runat="server"></asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"  SelectCommandType="StoredProcedure" 
            SelectCommand="AccountManagerStatus" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="DropDownList1" Name="accountManager" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

