<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Export_CQueue.aspx.vb" Inherits="Export_Details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

      <script type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=0,top=0,width=600,height=400,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();

        }

        function cancelBubble(e) {
            var evt = e ? e : window.event;
            if (evt.stopPropagation) evt.stopPropagation();
            if (evt.cancelBubble != null) evt.cancelBubble = true;
        }

    </script>

</head>
<body id="body_all">
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvDetails" DataSourceID="dsDetails" GridLines="None" Font-Size="Small" AlternatingRowStyle-BackColor="#ebf2fa" RowStyle-BackColor="White" 
            HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#334a5a"  runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsDetails" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="getCoachingQueue" 
            SelectCommandType="StoredProcedure" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
        <script>
            //CallPrint('body_all');
            //self.close();
        </script>
    </form>
</body>
</html>
