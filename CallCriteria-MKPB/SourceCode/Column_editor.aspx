<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Column_editor.aspx.vb" Inherits="Column_editor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">
        <asp:Button ID="btnAdd" runat="server" Text="New" />
        <asp:GridView ID="gvColumns" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsColumns">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="column_name" HeaderText="Dashboard Display" SortExpression="column_name" />
                <asp:BoundField DataField="column_sql" HeaderText="SQL" SortExpression="column_sql" />
                <asp:BoundField DataField="sort_key" HeaderText="Sort Key" SortExpression="sort_key" />
                <asp:CheckBoxField DataField="column_required" HeaderText="Required" SortExpression="column_required" />
                <asp:CheckBoxField DataField="column_default" HeaderText="Default" SortExpression="column_default" />
                <asp:CheckBoxField DataField="column_internal" HeaderText="Internal Only" SortExpression="column_internal" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsColumns" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>" 
            DeleteCommand="DELETE FROM [available_columns] WHERE [id] = @id" 
            InsertCommand="INSERT INTO [available_columns] ([column_name], [column_sql], [column_description], [column_required], [column_default], 
            [column_internal]) VALUES (@column_name, @column_sql, @column_description, @column_required, @column_default, @column_internal)" 
            SelectCommand="SELECT * FROM [available_columns] order by column_required desc" UpdateCommand="UPDATE [available_columns] SET sort_key=@sort_key, [column_name] = @column_name, [column_sql] = @column_sql, [column_description] = @column_description, [column_required] = @column_required, [column_default] = @column_default, [column_internal] = @column_internal WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="column_name" Type="String" />
                <asp:Parameter Name="column_sql" Type="String" />
                <asp:Parameter Name="column_description" Type="String" />
                <asp:Parameter Name="column_required" Type="Boolean" />
                <asp:Parameter Name="column_default" Type="Boolean" />
                <asp:Parameter Name="column_internal" Type="Boolean" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="column_name" Type="String" />
                <asp:Parameter Name="column_sql" Type="String" />
                <asp:Parameter Name="column_description" Type="String" />
                <asp:Parameter Name="column_required" Type="Boolean" />
                <asp:Parameter Name="column_default" Type="Boolean" />
                <asp:Parameter Name="column_internal" Type="Boolean" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

