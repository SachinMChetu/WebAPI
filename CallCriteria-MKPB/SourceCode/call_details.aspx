<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"  CodeFile="call_details.aspx.vb" Inherits="call_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        Appname:
        <asp:DropDownList ID="ddlApps" DataSourceID="dsApps" DataTextField="appname" DataValueField="appname" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select distinct appname from userapps where username = @username order by appname" runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>

        From:
                        
                <asp:TextBox ID="from" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>
        To:
                <asp:TextBox ID="to" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>

        <asp:Button ID="btnApply" runat="server" Text="Apply" />
        <asp:Button ID="btnExport" runat="server" Text="Export" />
        <br />
        <br />

        <asp:GridView ID="gvCalls" CssClass="detailsTable" DataSourceID="dsCalls" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsCalls" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="Select * from xcc_report_new left join form_score3 on xcc_report_new.id = review_id where xcc_report_new.appname = @appname and date_added between @start and @end">
            <SelectParameters>
                <asp:ControlParameter Name="appname" ControlID="ddlApps" />
                <asp:ControlParameter Name="start" ControlID="from" />
                <asp:ControlParameter Name="end" ControlID="to" />
            </SelectParameters>
        </asp:SqlDataSource>

        <script>
            $(document).ready(function () {


                $('#ContentPlaceHolder1_from').datepicker({
                    dateFormat: "mm/dd/yy"
                });
                $('#ContentPlaceHolder1_to').datepicker({
                    dateFormat: "mm/dd/yy"
                });

            });

        </script>


    </section>
</asp:Content>

