<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Dispute_details.aspx.vb" Inherits="Dispute_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Dispute Details</h2>
        <asp:GridView ID="gvdisputes" AutoGenerateColumns="false" CssClass="detailsTable" runat="server" DataSourceID="dsDisputes">
            <Columns>
                <asp:HyperLinkField  DataNavigateUrlFormatString="review_record.aspx?ID={0}" DataNavigateUrlFields="F_ID" Text="View" />
                <asp:BoundField DataField="COMMENT" HeaderText="COMMENT" SortExpression="COMMENT" HtmlEncode="false" />
                <asp:BoundField DataField="DATE_CLOSED" HeaderText="Date Closed" SortExpression="DATE_CLOSED" />
                <asp:BoundField DataField="WEEK_ENDING_DATE" HeaderText="Week Ending" SortExpression="WEEK_ENDING_DATE" DataFormatString="{0:MM/dd/yyy}"/>
                <asp:TemplateField HeaderText="Comments">
                    <ItemTemplate>
                        <%#Eval("formatted_comments").ToString.Replace("|", "") %>
                    </ItemTemplate>
                </asp:TemplateField>
           
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsDisputes" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select * from vwFN where reviewer=@reviewer and role='QA' and  ((close_reason = 'Agree')) and review_date between @start and @end">
            <SelectParameters>
                <asp:QueryStringParameter Name="reviewer" QueryStringField="reviewer" />
                <asp:QueryStringParameter Name="end" QueryStringField="end" />
                <asp:QueryStringParameter Name="start" QueryStringField="start" />
            </SelectParameters>
        </asp:SqlDataSource>

    </section>
</asp:Content>

