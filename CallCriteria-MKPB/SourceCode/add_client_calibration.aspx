<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="add_client_calibration.aspx.vb" Inherits="add_client_calibration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Copy Calibrations</h2>



        Copy open calibrations from: 
        <asp:DropDownList ID="ddlSource" DataSourceID="dsNICards" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="usernameplus" DataValueField="username" runat="server">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        to:
        <asp:DropDownList ID="ddlTo" DataSourceID="dsNICards" DataTextField="usernameplus" DataValueField="username" AppendDataBoundItems="true"  runat="server">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnGo" runat="server" Text="Copy" />
        <asp:Label ID="lblResults" ForeColor="Red" runat="server" Text=""></asp:Label>

        <br /><br />
        <asp:Button ID="btnDelete" runat="server" Text="Delete All Open" />
        <asp:Button ID="btnDeleteSelected" runat="server" Text="Delete Selected" />


        <asp:SqlDataSource ID="dsNICards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct top 100 percent userapps.username + ' (' + appname + ')' as usernameplus, userapps.username, userapps.appname 
            from userextrainfo join userapps on userapps.username = userextrainfo.username 
            where user_scorecard in (select user_scorecard from userapps where username = @username) and user_role in ('Supervisor','Manager','Client') order by userapps.appname, userapps.username" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" DefaultValue="stacemoss" />
            </SelectParameters>
        </asp:SqlDataSource>


        <asp:GridView ID="gvOpenCals" DataSourceID="dsOpenCals" AllowSorting="true" CssClass="detailsTable " runat="server" DataKeyNames="ID">
            <Columns>
               <asp:TemplateField HeaderText="Copy">
                   <ItemTemplate>
                       <asp:CheckBox ID="chkCopy"  Checked="true" runat="server" />
                       <asp:HiddenField ID="hdnThisID" runat="server" Value='<%#Eval("ID") %>' />
                   </ItemTemplate>
               </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsOpenCals"  ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="select short_name as Scorecard, cali_pending_client.ID, f_id as [Call ID], dateadded  from cali_pending_client join vwForm on vwForm.f_id = form_id join scorecards on scorecards.id = scorecard 
            where assigned_to = @assigned and date_completed is null">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSource" Name="assigned" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

