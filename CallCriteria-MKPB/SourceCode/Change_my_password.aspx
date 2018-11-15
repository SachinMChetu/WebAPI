<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Change_my_password.aspx.vb" Inherits="Change_my_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
        <div class="panel-content">
            <div class="static-header">
                <h1>Change My Password
                </h1>
            </div>


            <div class="table-outline with-tabs">

                <div style="table-layout">
                    <asp:ChangePassword ID="ChangePassword1" runat="server" ></asp:ChangePassword>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

