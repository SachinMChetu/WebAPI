<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="cloak.aspx.vb" Inherits="cloak" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">

        <h2>Rewritten URLs</h2>

        For Our URL, just use the ending -- http://app.callcriteria.com/c/1  use the 1 as our URL<br />


        <asp:GridView ID="gvRewrite" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsRewrite">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="ourURL" HeaderText="ourURL" SortExpression="ourURL" />
                <asp:BoundField DataField="cloakedURL" HeaderText="cloakedURL" SortExpression="cloakedURL" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsRewrite" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>" DeleteCommand="DELETE FROM [url_cloak] WHERE [id] = @id" InsertCommand="INSERT INTO [url_cloak] ([ourURL], [cloakedURL]) VALUES (@ourURL, @cloakedURL)" SelectCommand="SELECT * FROM [url_cloak]" UpdateCommand="UPDATE [url_cloak] SET [ourURL] = @ourURL, [cloakedURL] = @cloakedURL WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ourURL" Type="String" />
                <asp:Parameter Name="cloakedURL" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ourURL" Type="String" />
                <asp:Parameter Name="cloakedURL" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

