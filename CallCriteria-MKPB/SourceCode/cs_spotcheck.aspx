<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cs_spotcheck.aspx.vb" Inherits="cs_spotcheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvdsgetCSSpotData" DataSourceID="dsgetCSSpotData" runat="server"></asp:GridView>
            <asp:SqlDataSource ID="dsgetCSSpotData" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
              SelectCommand="getCSSpotData"   runat="server"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
