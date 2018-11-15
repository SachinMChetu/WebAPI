<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="clerked_data.aspx.vb" Inherits="clerked_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Collected Data</h2>
        <asp:Button ID="btnAdd" runat="server" Text="Add Item" />
        <br />
        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsItems">
            <Columns>
                <asp:BoundField DataField="short_name" HeaderText="Scorecard" SortExpression="short_name" ReadOnly="true" />
                <asp:BoundField DataField="value" HeaderText="Value" SortExpression="value" />
                <asp:BoundField DataField="notes" HeaderText="Notes" SortExpression="notes" />
                <asp:TemplateField HeaderText="Type" SortExpression="value_type">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%#Bind("value_type") %>'>
                            <asp:ListItem Value="">(Select)</asp:ListItem>
                            <asp:ListItem>Text</asp:ListItem>
                            <asp:ListItem>Date</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("value_type") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="value_order" HeaderText="Order" SortExpression="value_order" />
                <asp:BoundField DataField="date_added" HeaderText="Added" SortExpression="date_added" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="added_by" HeaderText="Added By" SortExpression="added_by" ReadOnly="true" />
                <asp:CheckBoxField DataField="active" HeaderText="Active" SortExpression="active" />
                <asp:CheckBoxField DataField="required_value" HeaderText="Required" SortExpression="required_value" />
                <asp:CheckBoxField DataField="dash_visible" HeaderText="Dash Visible" SortExpression="dash_visible" />
                <asp:CommandField ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            DeleteCommand="DELETE FROM [sc_inputs] WHERE [id] = @id"
            InsertCommand="INSERT INTO [sc_inputs] ([scorecard], [date_added], [added_by]) VALUES (@scorecard, dbo.getMTDate(), @added_by)"
            SelectCommand="SELECT * FROM [sc_inputs] join scorecards on scorecards.id = scorecard WHERE ([scorecard] = @scorecard)"
            UpdateCommand="UPDATE [sc_inputs] SET dash_visible=@dash_visible, [value] = @value, [notes] = @notes, required_value=@required_value, [value_type] = @value_type, [value_order] = @value_order, active=@active WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:QueryStringParameter Name="scorecard" QueryStringField="ID" Type="Int32" />
                <asp:Parameter Name="added_by" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="scorecard" QueryStringField="ID" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="scorecard" Type="Int32" />
                <asp:Parameter Name="value" Type="String" />
                <asp:Parameter Name="notes" Type="String" />
                <asp:Parameter Name="value_type" Type="String" />
                <asp:Parameter Name="value_order" Type="Int32" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="required_value" Type="Boolean" />
                <asp:Parameter DbType="Date" Name="date_added" />
                <asp:Parameter Name="added_by" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>

    </section>
</asp:Content>

