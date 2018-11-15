<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Filter_editor.aspx.vb" Inherits="Filter_editor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="main-container dash-modules">
        <h2>Filter Editor</h2>

        <br />
        <asp:Button ID="btnAdd" runat="server" Text="Add" />
        <br />

        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsFilters">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="filter_key" HeaderText="filter_key" SortExpression="filter_key" />
                <asp:BoundField DataField="filter_format" HeaderText="filter_format" SortExpression="filter_format" />
                <asp:BoundField DataField="filter_data_type" HeaderText="filter_data_type" SortExpression="filter_data_type" />
            </Columns>
        </asp:GridView>

        <br />

        <asp:SqlDataSource ID="dsFilters" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="SELECT * FROM [filter_data]" runat="server" DeleteCommand="DELETE FROM [filter_data] WHERE [id] = @id"
            InsertCommand="INSERT INTO [filter_data] ([filter_key], [filter_format]) VALUES (@filter_key, @filter_format)"
            UpdateCommand="UPDATE [filter_data] SET [filter_key] = @filter_key, [filter_format] = @filter_format, filter_data_type=@filter_data_type WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="filter_key" Type="String" />
                <asp:Parameter Name="filter_format" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="filter_key" Type="String" />
                <asp:Parameter Name="filter_format" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>

    </section>

</asp:Content>

