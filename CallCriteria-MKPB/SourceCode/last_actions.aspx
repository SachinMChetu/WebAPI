<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="last_actions.aspx.vb" Inherits="last_actions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-buttons">
        <h2>100 Last Actions</h2>

        User:
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox><asp:Button ID="btnGo" runat="server" Text="Go" />
        <br />
        <br />

        <asp:GridView ID="gvActivities" DataSourceID="dsActivities" runat="server" ></asp:GridView>
        <asp:SqlDataSource ID="dsActivities" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
            SelectCommand="select top 100 * from ">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtUser" Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

