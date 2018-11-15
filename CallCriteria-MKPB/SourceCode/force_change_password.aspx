<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="force_change_password.aspx.vb" Inherits="force_change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container">
        <h2>Change Your Password</h2>

        <br /><br />
        Your password is still set to the default.  Please change it below:
        <br />
        <br />
        <asp:ChangePassword ID="ChangePassword1" ChangePasswordButtonStyle-CssClass="main-cta" CancelButtonType="Link" CancelButtonText="" runat="server"></asp:ChangePassword>
    </section>
</asp:Content>

