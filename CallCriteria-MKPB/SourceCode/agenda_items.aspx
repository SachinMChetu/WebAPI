<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="agenda_items.aspx.vb" Inherits="agenda_items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Open Agenda Items</h2>
        <asp:Button Text="Add Agenda Item" runat="server" ID="btnAddItem" />

        <asp:GridView ID="gvAgenda" DataSourceID="dsAgenda" CssClass="detailsTable " AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="create_agenda.aspx?ID={0}" Text="View/Edit" />
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="work_agenda.aspx?ID={0}" Text="Work Item" />

                <asp:BoundField DataField="open_items" SortExpression="open_items" HeaderText="Open" />
                <asp:BoundField DataField="Closed" SortExpression="Closed" HeaderText="Closed" />

                <asp:BoundField DataField="DATE_CREATED" SortExpression="DATE_CREATED" HeaderText="Created" />
                <asp:BoundField DataField="short_name" SortExpression="short_name" HeaderText="Scorecard" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsAgenda" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            DeleteCommand="delete from agenda_topics where id=@ID "
            SelectCommand="select * from agenda_item join scorecards on scorecards.ID = sc_id join userapps on userapps.user_scorecard = scorecards.id
            left join (select agenda_id, count(date_completed) as closed, count(*) - count(date_completed) as [open_items] from agenda_topics group by agenda_id) a on a.agenda_id = agenda_item.id
             where date_completed is null and username = @username and scorecards.active = 1 order by open_items desc" runat="server">
            <SelectParameters>
                <asp:Parameter Name="Username" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
</asp:Content>

