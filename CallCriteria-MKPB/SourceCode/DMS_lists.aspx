<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="DMS_lists.aspx.vb" Inherits="DMS_lists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>List ID to Scorecard</h2>
        <asp:Button ID="btnADd" runat="server" Text="Add" />
        <asp:GridView ID="GridView1" CssClass="detailsTable" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="listID" HeaderText="listID" SortExpression="listID" />
                <asp:TemplateField HeaderText="scorecard" SortExpression="scorecard">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecard" AppendDataBoundItems="true" DataTextField="short_name" DataValueField="ID" SelectedValue='<%# Bind("scorecard") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlScorecard" Enabled="false" AppendDataBoundItems="true" DataSourceID="dsScorecard" DataTextField="short_name" DataValueField="ID" SelectedValue='<%# Bind("scorecard") %>' runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="in_out" HeaderText="in_out" SortExpression="in_out" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsScorecard" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select id, short_name from scorecards where appname = 'DMS' and active = 1"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            DeleteCommand="DELETE FROM [DMS_listID] WHERE [id] = @id"
            InsertCommand="INSERT INTO [DMS_listID] ([listID], [scorecard], [in_out]) VALUES (@listID, @scorecard, @in_out)"
            SelectCommand="SELECT * FROM [DMS_listID]" UpdateCommand="UPDATE [DMS_listID] SET [listID] = @listID, [scorecard] = @scorecard, [in_out] = @in_out WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="listID" Type="String" />
                <asp:Parameter Name="scorecard" Type="Int32" />
                <asp:Parameter Name="in_out" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="listID" Type="String" />
                <asp:Parameter Name="scorecard" Type="Int32" />
                <asp:Parameter Name="in_out" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

