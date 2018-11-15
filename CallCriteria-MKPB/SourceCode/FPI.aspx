<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="FPI.aspx.vb" Inherits="FPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>
        FPI Maintenance
    </h2>


    <asp:GridView ID="gvWebsites" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsWebsites">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:TemplateField HeaderText="Partner" SortExpression="Partner">
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlPartner" runat="server" SelectedValue='<%# Bind("Partner") %>'>
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Partner") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="school" HeaderText="school" SortExpression="school" />
            <asp:BoundField DataField="URL" HeaderText="URL" SortExpression="URL" />
            <asp:CheckBoxField DataField="active" HeaderText="active" SortExpression="active" />
            <asp:BoundField DataField="last_updated" HeaderText="last_updated" SortExpression="last_updated" />
            <asp:TemplateField HeaderText="Notes"></asp:TemplateField>
        </Columns>
    </asp:GridView>



    <asp:SqlDataSource ID="dsWebsites" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>" DeleteCommand="DELETE FROM [FPI_sites] WHERE [id] = @id" InsertCommand="INSERT INTO [FPI_sites] ([URL], [Partner], [school], [active], [last_updated]) VALUES (@URL, @Partner, @school, @active, @last_updated)" SelectCommand="SELECT * FROM [FPI_sites]" UpdateCommand="UPDATE [FPI_sites] SET [URL] = @URL, [Partner] = @Partner, [school] = @school, [active] = @active, [last_updated] = @last_updated WHERE [id] = @id"  >
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="URL" Type="String" />
            <asp:Parameter Name="Partner" Type="String" />
            <asp:Parameter Name="school" Type="String" />
            <asp:Parameter Name="active" Type="Boolean" />
            <asp:Parameter DbType="Date" Name="last_updated" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="URL" Type="String" />
            <asp:Parameter Name="Partner" Type="String" />
            <asp:Parameter Name="school" Type="String" />
            <asp:Parameter Name="active" Type="Boolean" />
            <asp:Parameter DbType="Date" Name="last_updated" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

</asp:Content>

