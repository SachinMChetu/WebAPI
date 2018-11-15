<%@ Page Language="VB" AutoEventWireup="false" CodeFile="email_bad_calls.aspx.vb" Inherits="email_bad_calls" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="GVBadCalls" AllowSorting="true" GridLines="Both" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ID" DataSourceID="dsBadCalls">
            <Columns>

                <asp:HyperLinkField DataNavigateUrlFields="SESSION_ID" Target="_blank" DataNavigateUrlFormatString="listen_2options_force.aspx?sessionID={0}" DataTextField="SESSION_ID" HeaderText="SESSION_ID" SortExpression="SESSION_ID" />

                <asp:BoundField DataField="TIMESTAMP" HeaderText="TIMESTAMP"
                    SortExpression="TIMESTAMP" />

                <asp:BoundField DataField="Phone" HeaderText="Phone"
                    SortExpression="Phone" />

                <asp:BoundField DataField="review_started" HeaderText="Review Started"
                    SortExpression="review_started" />

                <asp:HyperLinkField DataNavigateUrlFields="audio_link" Target="_blank" DataTextField="audio_link" HeaderText="Audio Link" SortExpression="audio_link" />

                <asp:BoundField DataField="bad_call_who" HeaderText="Who"
                    SortExpression="bad_call_who" />

                <asp:BoundField DataField="bad_call_date" HeaderText="Date"
                    SortExpression="bad_call_date" />

                <asp:BoundField DataField="bad_call_reason" HeaderText="Reason"
                    SortExpression="bad_call_reason" />
                
                <asp:BoundField DataField="Notes" HeaderText="Notes"
                    SortExpression="Notes" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsBadCalls" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT * FROM xcc_report_new WHERE call_date = CONVERT(DATE, DATEADD(DAY, -1, dbo.getMTDate())) AND bad_call = 1 AND bad_call_accepted IS NULL">
        </asp:SqlDataSource>
    </form>
</body>
</html>
