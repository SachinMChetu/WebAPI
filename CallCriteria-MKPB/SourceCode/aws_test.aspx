<%@ Page Language="VB" AutoEventWireup="false" CodeFile="aws_test.aspx.vb" Inherits="aws_test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" Visible="false"  runat="server" Text="List Buckets" /><br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:Button ID="Button2" runat="server" Text="Get Bucket" />
    </div>
    </form>
</body>
</html>
