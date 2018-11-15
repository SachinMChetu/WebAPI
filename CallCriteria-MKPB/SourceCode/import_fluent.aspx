<%@ Page Title="" Language="C#" MasterPageFile="~/CC_Master.master" AutoEventWireup="true" CodeFile="import_fluent.aspx.cs" Inherits="import_fluent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container dash-modules general-button">
        <h2>Fluent Load Porcess</h2>
        Select File: <asp:FileUpload ID="FileUpload1" runat="server" /><asp:Button ID="btnGo" OnClick="btnGo_Click" runat="server" Text="Go" />

        <br />
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text=""></asp:Label>

        <asp:GridView ID="gvTest" runat="server"></asp:GridView>

    </section>
</asp:Content>

