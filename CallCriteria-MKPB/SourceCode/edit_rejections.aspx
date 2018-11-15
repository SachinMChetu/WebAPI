<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_rejections.aspx.vb" Inherits="rejection_managerment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Rejection Reason Management</h2>

        Profile Name:
        <asp:DropDownList ID="ddlSC" DataSourceID="dsApps" DataTextField="profile_name" DataValueField="id" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select id,profile_name from rejection_profiles order by profile_name"
            runat="server"></asp:SqlDataSource>
        <br />
        New Profile:
        <asp:TextBox ID="txtNewProfile" title="Add Profile Name" runat="server"></asp:TextBox>
        from profile: 
        <asp:DropDownList ID="ddlFrom" DataSourceID="dsApps" DataTextField="profile_name" DataValueField="id" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
        </asp:DropDownList><asp:Button ID="btnNewProfile" runat="server" Text="Add Profile" />
        <br />


        <h2>Reasons</h2>
        <asp:Button ID="btnNew" runat="server" Text="Add" />
        <asp:GridView ID="gvNotifications" DataSourceID="dsNotifications" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField HeaderText="Rejection Reason" DataField="reason" SortExpression="reason" />

                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsNotifications" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            DeleteCommand="DELETE FROM [rejection_reasons] WHERE [id] = @id"
            InsertCommand="INSERT INTO [rejection_reasons] (profile_id) select @profile_id"
            SelectCommand="SELECT * FROM [rejection_reasons] where profile_id=@profile_id "
            UpdateCommand="UPDATE [rejection_reasons] SET  [reason] = @reason where [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="profile_id" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="ddlSC" Name="profile_id" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="reason" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>




    </section>


</asp:Content>

