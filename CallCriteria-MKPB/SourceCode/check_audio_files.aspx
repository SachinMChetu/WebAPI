<%@ Page Language="VB" AutoEventWireup="false" CodeFile="check_audio_files.aspx.vb" Inherits="check_audio_files" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            App:
            <asp:DropDownList ID="dlApps" DataSourceID="dsApps" DataTextField="Appname" DataValueField="appname" runat="server"></asp:DropDownList>
            <asp:SqlDataSource ID="dsApps" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select * from app_settings where active = 1"></asp:SqlDataSource>

            Select date:
       <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>

            <asp:Button ID="btnGo" runat="server" Text="Go" />
        </div>
    </form>
</body>
</html>
