<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="additional_pay.aspx.vb" Inherits="additional_pay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container general-button dash-modules ">
        <h2>Additional Pay</h2>
        <p>
            <asp:Button ID="btnNew" runat="server" Text="Add" />
        </p>

        <asp:SqlDataSource ID="dsWeekEnding" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select distinct week_ending_date from vwForm where review_date > dateadd(d, -14, dbo.getMTDate()) union all select convert(date, dateadd(d, - datepart(weekday, dbo.getMTDate()) + 14, dbo.getMTDate())) as week_ending_date order by week_ending_date"></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsQAList" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select distinct reviewer from vwForm where review_date > dateadd(d, -14, dbo.getMTDate()) order by reviewer"></asp:SqlDataSource>

        <asp:GridView ID="gvPay" DataSourceID="dsPay" runat="server" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:TemplateField HeaderText="week_ending" SortExpression="week_ending">
                    <EditItemTemplate>

                        <asp:DropDownList ID="ddlWeekEnding" DataSourceID="dsWeekEnding" SelectedValue='<%# Bind("week_ending") %>' DataTextField="week_ending_date" DataValueField="week_ending_date" AppendDataBoundItems="true" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList>

                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("week_ending") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="add_time" HeaderText="add_time" SortExpression="add_time" />
                <asp:BoundField DataField="add_money" HeaderText="add_money" SortExpression="add_money" />
                <asp:TemplateField HeaderText="username" SortExpression="username">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlQA" DataSourceID="dsQAList" DataTextField="reviewer" DataValueField="reviewer" SelectedValue='<%# Bind("username") %>' runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("username") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="who_added" HeaderText="who_added" ReadOnly="True" SortExpression="who_added" />
                <asp:BoundField DataField="date_added" HeaderText="date_added" SortExpression="date_added" ReadOnly="True" />
                <asp:TemplateField HeaderText="notes" SortExpression="notes">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" TextMode="MultiLine" runat="server" Text='<%# Bind("notes") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("notes") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsPay" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server"
            SelectCommand="SELECT * FROM [additional_pay]" DeleteCommand="DELETE FROM [additional_pay] WHERE [id] = @id"
            InsertCommand="INSERT INTO [additional_pay] ([week_ending], [add_time], [add_money], [username], [who_added], [date_added], [notes]) 
           VALUES (@week_ending, @add_time, @add_money, @username, @who_added, dbo.getMTDate(), @notes)"
            UpdateCommand="UPDATE [additional_pay] SET [week_ending] = @week_ending, [add_time] = @add_time, [add_money] = @add_money, [username] = @username, [notes] = @notes WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter DbType="Date" Name="week_ending" />
                <asp:Parameter Name="add_time" Type="Double" />
                <asp:Parameter Name="add_money" Type="Double" />
                <asp:Parameter Name="username" Type="String" />
                <asp:Parameter Name="who_added" Type="String" />
                <asp:Parameter Name="date_added" Type="DateTime" />
                <asp:Parameter Name="notes" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter DbType="Date" Name="week_ending" />
                <asp:Parameter Name="add_time" Type="Double" />
                <asp:Parameter Name="add_money" Type="Double" />
                <asp:Parameter Name="username" Type="String" />
                <asp:Parameter Name="who_added" Type="String" />
                <asp:Parameter Name="date_added" Type="DateTime" />
                <asp:Parameter Name="notes" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

