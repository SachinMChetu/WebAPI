<%@ Page Title="Completed Calibration Report" Language="VB" MasterPageFile="~/CC_MASter.master"
    AutoEventWireup="false" CodeFile="closed_client_calibrations.aspx.vb" Inherits="ClosedClientCalibrations" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        App:
            <asp:DropDownList ID="ddlAppList" DataTextField="AppName" DataValueField="AppName" AppendDataBoundItems="true" DataSourceID="dsAppList" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddlAppList_SelectedIndexChanged">
                <asp:ListItem Text="--Select App--" Value=""></asp:ListItem>
            </asp:DropDownList>
        <asp:SqlDataSource ID="dsAppList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT DISTINCT AppName FROM UserApps WHERE UserName = @UserName" runat="server">
            <SelectParameters>
                <asp:Parameter Name="UserName" Type="String" DefaultValue="Anonymous" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <br />
        Scorecard:
            <asp:DropDownList ID="ddlScorecard" DataTextField="Short_Name" DataValueField="ID" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                OnSelectedIndexChanged="ddlScorecard_SelectedIndexChanged" Enabled="false">
            </asp:DropDownList>
        <br />
        <br />
        <asp:GridView ID="gvCCC" AllowSorting="true" runat="server" CellPadding="5" CssClass="detailsTable" AutoGenerateColumns="false" DataSourceID="dsCCC">
            <Columns>
                <asp:HyperLinkField HeaderText="Form ID" DataNavigateUrlFields="Form_ID" DataTextField="Form_ID" DataNavigateUrlFormatString="~/review_calib_record_client.aspx?ID={0}" />
                <asp:BoundField DataField="Short_Name" SortExpression="Short_Name" HeaderText="Scorecard" />
                <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="Reviewer" SortExpression="Reviewer" HeaderText="Reviewer" />
                <asp:BoundField DataField="Client_Who_Closed" SortExpression="Client_Who_Closed" HeaderText="Client who closed" />
                <asp:BoundField DataField="DateAdded" SortExpression="DateAdded" HeaderText="Date Added" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="Client_Review_Completed" SortExpression="Client_Review_Completed" HeaderText="Client review completed" DataFormatString="{0:MM/dd/yyyy}" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsCCC" SelectCommand="select distinct DateAdded, calibration_pending.Form_ID, Short_Name, Phone, calibration_pending.Reviewer, Client_Who_Closed, Client_Review_Completed
                 from cali_pending_client With (nolock) join calibration_pending With (nolock)  On calibration_pending.form_id = cali_pending_client.form_id
                 Join (Select distinct calibration_pending.form_id, calibration_pending.date_added, Convert(Date, calibration_form.review_date) As cc_review_date,
                 client_completed + Case When date_completed Is Not null Then 1 Else 0 End + 1 As real_num_completed, client_reviews + 2 As real_num_reviews, short_name, phone
                 from calibration_pending  With (nolock) Join vwForm With (nolock)  On vwForm.f_id = calibration_pending.form_id
                 join userapps With (nolock)  On userapps.user_scorecard = vwForm.scorecard and userapps.username = @UserName
                 join scorecards On scorecards.id = @ScoreCard
                 Join(Select count(*) As client_reviews, form_id, count(date_completed) As client_completed from cali_pending_client With (nolock)  group by form_id) a On a.form_id = calibration_pending.form_id
                 left join calibration_form With (nolock)  On calibration_form.original_form = calibration_pending.form_id And calibration_pending.isRecal = 0
                 where calibration_pending.appname = @AppName) z on z.form_id = calibration_pending.form_id
                 where calibration_pending.appname = @AppName and  client_review_completed is not null  and calibration_pending.isRecal = 0"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlScorecard" Name="ScoreCard" />
                <asp:ControlParameter ControlID="ddlAppList" Name="AppName" />
                <asp:Parameter Name="UserName" Type="String" DefaultValue="Anonymous" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>
