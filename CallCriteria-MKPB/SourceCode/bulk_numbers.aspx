<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="bulk_numbers.aspx.vb" Inherits="bulk_numbers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container">
        <h1 class="section-title"><i class="fa fa-desktop"></i>
            Bulk Download</h1>
        <br /><br />
        Note: These are processed calls.  If the call is pending it won't show up here yet.
        <br /><br />
        <table>
            <tr>
                <td valign="top">Enter phone #s (one per line):<br />
                    <asp:TextBox ID="txtPhones" TextMode="MultiLine" Rows="30" Columns="30" runat="server"></asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" />
                    <br />
                    <asp:Label ID="lblNotFound" runat="server" Text="" ForeColor="Red" ></asp:Label>

                </td>
                <td valign="top">
                    <div class="table-outline users-list">
                        <asp:GridView ID="gvResults"  AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:HyperLinkField DataNavigateUrlFields="F_ID" DataNavigateUrlFormatString="http://dl.callcriteria.com/download_call.aspx?ID={0}" DataTextField="F_ID" HeaderText="Download Full Call" />
                                <asp:HyperLinkField DataNavigateUrlFields="F_ID" DataNavigateUrlFormatString="http://dl.callcriteria.com/download_audio.aspx?ID={0}" DataTextField="F_ID" HeaderText="Download Audio Only" />
                                <asp:BoundField DataField="session_id" HeaderText="SESSION ID" SortExpression="session_id" />
                                <asp:BoundField DataField="AGENT" HeaderText="AGENT" SortExpression="AGENT" />
                                <asp:BoundField DataField="CAMPAIGN" HeaderText="CAMPAIGN" SortExpression="CAMPAIGN" />
                                <asp:BoundField DataField="DNIS" HeaderText="DNIS" SortExpression="DNIS" />
                                <asp:BoundField DataField="review_started" HeaderText="Review Date" SortExpression="review_started" />
                                <asp:BoundField DataField="appname" HeaderText="Appname" SortExpression="appname" />
                                <asp:BoundField DataField="total_score" HeaderText="Total Score" SortExpression="total_score" />
                                <asp:BoundField DataField="Comments" HeaderText="Comments" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>

        </table>
    </section>
</asp:Content>

