<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="call_center_management.aspx.vb" Inherits="call_center_management" %>

<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Call Centers</h2>
        <p>
            <asp:Button ID="btnNewCenter" runat="server" Text="New Center" />
        </p>

        <asp:GridView ID="gvCC" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsCC">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="center_name" HeaderText="Name" SortExpression="center_name" />
                <asp:BoundField DataField="contact" HeaderText="Contact" SortExpression="contact" />
                <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                <asp:BoundField DataField="bill_rate" HeaderText="Rate" SortExpression="bill_rate" />
                <asp:BoundField DataField="startdate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Started" SortExpression="startdate" />
                <asp:TemplateField HeaderText="Manager" SortExpression="center_manager">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCMS" runat="server" selectedvalue='<%# Bind("center_manager") %>' datasourceID="dsCMS" appenddatabounditems="true" datavaluefield="username" datatextfield="username" >
                            <asp:listitem value="" text="(Select)"></asp:listitem>

                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("center_manager") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource runat="server" ID="dsCMs" ConnectionString="<%$ ConnectionStrings:estomesManual %>" selectcommand ="select username from userextrainfo where user_role = 'center manager' order by username"></asp:SqlDataSource>
        <br />
        <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>" 
            DeleteCommand="DELETE FROM [call_centers] WHERE [id] = @id" 
            InsertCommand="INSERT INTO [call_centers] ([center_name], [contact], [phone], [bill_rate], [startdate]) VALUES (@center_name, @contact, @phone, @bill_rate, @startdate)" 
            SelectCommand="SELECT * FROM [call_centers]" 
            UpdateCommand="UPDATE [call_centers] SET [center_name] = @center_name, [contact] = @contact, center_manager=@center_manager, [phone] = @phone, [bill_rate] = @bill_rate, 
            [startdate] = @startdate WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="center_name" Type="String" />
                <asp:Parameter Name="contact" Type="String" />
                <asp:Parameter Name="phone" Type="String" />

                <asp:Parameter Name="bill_rate" Type="Double" />
                <asp:Parameter DbType="Date" Name="startdate" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="center_name" Type="String" />
                <asp:Parameter Name="contact" Type="String" />
                <asp:Parameter Name="phone" Type="String" />
<asp:Parameter Name="center_manager" Type="String" />
                <asp:Parameter Name="bill_rate" Type="Double" />
                <asp:Parameter DbType="Date" Name="startdate" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </section>
</asp:content>

