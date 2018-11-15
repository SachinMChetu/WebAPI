<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="assemble_audio.aspx.vb" Inherits="assemble_audio" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="main-container">
         Select App:
            <asp:DropDownList ID="ddlApps" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="appname" DataValueField="appname" runat="server">
                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select appname from app_settings  order by appname" runat="server"></asp:SqlDataSource>
            <br />

        Select Date:
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
          <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="TextBox1" runat="server">
                            </asp:CalendarExtender>
    </section>
    <asp:Button ID="btnGo" runat="server" Text="Go" />
</asp:Content>

