<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="last_edits.aspx.vb" Inherits="last_edits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container">
        <asp:GridView ID="gvLastEdit" RowStyle-VerticalAlign="Top" AutoGenerateColumns="false" CssClass="table-outline" DataSourceID="dsLastEdit" runat="server">
            <Columns>
                <asp:HyperLinkField HeaderText="Call" DataNavigateUrlFields="form_id" DataNavigateUrlFormatString="review_record.aspx?ID={0}" Text="View" />
                <asp:BoundField HeaderText="All Comments" DataField="all_Comments" HtmlEncode="false" />
                <asp:BoundField HeaderText="Edited By" DataField="changed_by" SortExpression="changed_by" />
                <asp:BoundField HeaderText="Edit Date" DataField="changed_date" SortExpression="changed_date" />
                <asp:BoundField HeaderText="Score" DataField="total_score" SortExpression="total_score" />
                <asp:BoundField HeaderText="QA" DataField="reviewer" SortExpression="reviewer" />
                <asp:BoundField HeaderText="Agent" DataField="agent" SortExpression="agent" />
                <asp:BoundField HeaderText="Notification Step" DataField="role" SortExpression="role" />
                
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsLastEdit" runat="server"  ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select top 50 dbo.getAllCannedComments(f_id) as all_comments,* from form_q_score_changes join vwForm on vwForm.F_ID = form_q_score_changes.form_id 
                            join userextrainfo on 
                            userextrainfo.username = form_q_score_changes.changed_by
                            left join form_notifications on form_notifications.form_id = vwForm.f_id
                            where user_role in ('Client','Supervisor') and date_closed is null
                            order by changed_date desc">
        </asp:SqlDataSource>
    </section>

</asp:Content>

