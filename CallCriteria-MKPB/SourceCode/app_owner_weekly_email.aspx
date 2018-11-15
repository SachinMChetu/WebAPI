<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="app_owner_weekly_email.aspx.vb" Inherits="app_owner_weekly_email" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
        }

        a {
            text-decoration: none;
        }

        #tblSummary {
            border-spacing: 0;
            border-top: 1px solid #808080;
            border-left: 1px solid #808080;
        }

        #gv tr:nth-child(odd) {
            background-color: #EEE;
        }

        #gvMissed tr:nth-child(odd) {
            background-color: #EEE;
        }

        #tblSummary td {
            padding: 5px;
            border-right: 1px solid #808080;
            border-bottom: 1px solid #808080;
        }

            #tblSummary td:first-child {
                background-color: #000;
                color: #FFF;
                border-right: 1px solid #808080;
                border-bottom: 1px solid #808080;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <img src="http://app.callcriteria.com/img/cc_words_logo.png" alt="Call Criteria" width="322" height="50" />
        <h2>Weekly Report for <%=app_name %></h2>
        <table id="tblSummary">
            <tr>
                <td>From</td>
                <td><%=Date.Now.AddDays(-6).ToString("MM/dd/yyyy")%></td>
            </tr>
            <tr>
                <td>To</td>
                <td><%=Date.Now.ToString("MM/dd/yyyy")%></td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvSummary" AllowSorting="true" GridLines="Both" runat="server" AutoGenerateColumns="False"
            DataKeyNames="" DataSourceID="dsSummary" CellPadding="5">
            <Columns>

                <asp:BoundField DataField="# of Agents" HeaderText="# of Agents" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="# of Calls" HeaderText="# of Calls" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="Total Call Length" HeaderText="Total Call Length" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="Average Score" HeaderText="Average Score" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsSummary" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT COUNT(DISTINCT agent) AS '# of Agents',
	                            COUNT(*) AS '# of Calls',
	                            dbo.ConvertTimeToHHMMSS(SUM(call_length),'s') AS 'Total Call Length',
	                            CAST(CAST(ROUND(AVG(total_score),0) AS INT) AS VARCHAR(4)) + '%' AS 'Average Score'
                            FROM VWForm 
                            WHERE appname = @appname AND review_date BETWEEN CONVERT(DATE, DATEADD(DAY, -6, dbo.getMTDate())) AND dbo.getMTDate()">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="appname" Name="appname" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <br />
        <asp:GridView ID="gvMissed" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" DataSourceID="dsMissed" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsMissed" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getWeeklyNumberMissed" SelectCommandType="StoredProcedure" runat="server">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="appname" Name="appname" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <br />
        <asp:GridView ID="gv" AllowSorting="true" GridLines="Both" runat="server" AutoGenerateColumns="False"
            DataKeyNames="" DataSourceID="ds" CellPadding="5">
            <Columns>

                <asp:BoundField DataField="Agent" HeaderText="Agent"
                    SortExpression="Agent" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                  <asp:BoundField DataField="Scorecard" HeaderText="Scorecard"
                    SortExpression="Scorecard" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="Top Missed" HeaderText="Top Missed"
                    SortExpression="Top Missed" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="# of Calls" HeaderText="# of Calls"
                    SortExpression="# of Calls" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="Total Call Length" HeaderText="Total Call Length"
                    SortExpression="Total Call Length" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

                <asp:BoundField DataField="Average Score" HeaderText="Average Score"
                    SortExpression="Average Score" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="ds" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT agent AS Agent, dbo.GetTop3Missed(agent,CONVERT(DATE, DATEADD(DAY, -6, dbo.getMTDate())), dbo.getMTDate() ) as [Top Missed], 
	                            COUNT(*) AS '# of Calls',
	                            dbo.ConvertTimeToHHMMSS(SUM(call_length),'s') AS 'Total Call Length',
	                            CAST(CAST(ROUND(AVG(total_score),0) AS INT) AS VARCHAR(4)) + '%' AS 'Average Score', scorecard_name as SCorecard
                            FROM VWForm
                            WHERE appname = @appname AND review_date BETWEEN CONVERT(DATE, DATEADD(DAY, -6, dbo.getMTDate())) AND dbo.getMTDate()
                            GROUP BY agent, scorecard_name
                            ORDER BY scorecard_name, agent">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="appname" Name="appname" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
