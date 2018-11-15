<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="keyword_report.aspx.vb" Inherits="keyword_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Keyword Report</h2>
        <asp:HiddenField ID="hdnAgent" runat="server" />
        <asp:HiddenField ID="hdnItem" runat="server" />

        <asp:Label ID="lblScorecard" runat="server" Text="Scorecard:"></asp:Label>
        <asp:DropDownList ID="ddlScorecard" DataSourceID="dsNICards" DataTextField="short_name" DataValueField="ID" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="dsNICards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="Select scorecards.id, short_name from userapps join scorecards on user_scorecard = scorecards.id where username = @username and transcribe = 1 and active = 1 order by short_name"
            runat="server">
            <SelectParameters>
                <asp:Parameter Name="username" DefaultValue="stacemoss" />
            </SelectParameters>
        </asp:SqlDataSource>

     
        Start Date:
        <asp:TextBox ID="txtStart" placeholder="MM/DD/YYYY" runat="server"></asp:TextBox>
        End Date:
        <asp:TextBox ID="txtEnd" placeholder="MM/DD/YYYY" runat="server"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" />
        <br />
        <br />


        <asp:GridView ID="gvKeywords" CssClass="detailsTable" AllowSorting="true" DataSourceID="dsKeywords" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsKeywords" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
            SelectCommand="getKeywords" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter Name="scorecard" ControlID="ddlScorecard" />
                <asp:ControlParameter Name="start_date" ControlID="txtStart" />
                <asp:ControlParameter Name="end_date" ControlID="txtEnd" />
            </SelectParameters>
        </asp:SqlDataSource>

        <script>
            $(document).ready(function () {


                $('#ContentPlaceHolder1_txtStart').datepicker({
                    dateFormat: "mm/dd/yy"
                });
                $('#ContentPlaceHolder1_txtEnd').datepicker({
                    dateFormat: "mm/dd/yy"
                });

            });


        </script>

    </section>
</asp:Content>

