<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bitwage.aspx.vb" Inherits="bitwage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        Select Week Ending:
      <asp:DropDownList ID="lbWEDate" DataTextField="we_date" DataValueField="we_date"
                                DataSourceID="dsWEDate" runat="server">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsWEDate" SelectCommand="select distinct convert(varchar(10),week_ending_date,101) as we_date, week_ending_date from form_score3 order by week_ending_date desc"
                                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                            </asp:SqlDataSource>

        <asp:Button ID="btnGo" runat="server" Text="Go" />

    </div>
    </form>
</body>
</html>
