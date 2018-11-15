<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="create_secondary_calls.aspx.vb" Inherits="create_secondary_calls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Create 2nd Review Calls</h2>
        <br />
        <table>
            <tr>
                <td>Scorecard: </td>
                <td>
                    <asp:DropDownList ID="ddlSC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select short_name, id from scorecards where active = 1  order by short_name" runat="server"></asp:SqlDataSource>
                </td>
            </tr>

            <tr>
                <td>Quantity: </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <asp:Button ID="btnCreate" runat="server" Text="Create Calls" />
        <asp:Label ID="lblResults" ForeColor="Red" runat="server" Text=""></asp:Label>

        <br /><br />
        <hr />
        <h2>Open Secondary Calls</h2>
        <asp:GridView ID="GridView1" CssClass="detailsTable" DataSourceID="dsPending" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsPending"  ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="select count(*) as [Pending Calls], short_name as [Scorecard] from xcc_report_new join scorecards on scorecard = scorecards.id where max_reviews = 500 group by short_name"></asp:SqlDataSource>


    </section>
</asp:Content>

